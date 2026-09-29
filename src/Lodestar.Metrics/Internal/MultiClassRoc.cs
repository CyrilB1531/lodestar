using System.Buffers;

namespace Lodestar.Metrics.Internal;

/// <summary>
/// Multiclass ROC-AUC by reduction to binary problems — scikit-learn's
/// <c>multi_class="ovr"</c> and <c>multi_class="ovo"</c>.
/// </summary>
internal static class MultiClassRoc
{
    // NumPy's allclose defaults, which is the comparison sklearn makes.
    private const double RelativeTolerance = 1e-5;
    private const double AbsoluteTolerance = 1e-8;

    public static double Score(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<double> yScore,
        int classCount,
        MultiClassRocOptions options)
    {
        Averaging average = options.Average ?? Averaging.Macro;
        int n = Validate(yTrue, yScore, classCount, options, average);
        int[] classes = ResolveLabels(yTrue, options.Labels, classCount);
        ValidateRowSums(yScore, n, classCount);

        // 0 and 1 both mean sequential.
        int workers = Math.Max(1, options.MaxDegreeOfParallelism);

        if (options.Strategy == MultiClassStrategy.OneVsRest)
        {
            // One total per class, the weights NumpyAverage.Weighted averages by and the shortcut below reads; macro reads neither (#1586).
            double[] classTotals = average == Averaging.Weighted ? ClassTotals(yTrue, classes, options.SampleWeight) : [];

            // _average_binary_score returns 0 when the class totals sum close to zero, before any class curve reads
            // a weight (#1534, #1586); Validate has already refused a non-finite score (#1569).
            if (average == Averaging.Weighted && !options.SampleWeight.IsEmpty
                && Math.Abs(NumpyAverage.Sum(classTotals)) <= ZeroTotalTolerance)
            {
                return 0.0;
            }

            // Where roc_curve would refuse a weight in every class alike, refused once, before any worker starts.
            Inputs.ValidateSampleWeight(options.SampleWeight);

            return workers == 1
                ? OneVsRest(yTrue, yScore, classes, average, options.SampleWeight, classTotals)
                : OneVsRestParallel(yTrue, yScore, classes, average, options.SampleWeight, classTotals, workers);
        }

        return workers == 1
            ? OneVsOne(yTrue, yScore, classes, average)
            : OneVsOneParallel(yTrue, yScore, classes, average, workers);
    }

    private static int Validate(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int classCount,
        MultiClassRocOptions options, Averaging average)
    {
        int n = yTrue.Length;
        if (options.MaxDegreeOfParallelism < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.MaxDegreeOfParallelism,
                "MaxDegreeOfParallelism cannot be negative. 0 and 1 are both sequential.");
        }
        if (classCount < 2)
        {
            throw new ArgumentOutOfRangeException(
                nameof(classCount), classCount, "Multiclass ROC AUC needs at least two classes.");
        }

        // check_array's sentences, y_true's before y_score's, ahead of any length comparison (#1585).
        if (n == 0)
        {
            throw new ArgumentException(
                "Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required.", nameof(yTrue));
        }
        if (yScore.IsEmpty)
        {
            throw new ArgumentException(
                $"Found array with 0 sample(s) (shape=(0, {classCount})) while a minimum of 1 is required.", nameof(yScore));
        }

        // check_array refuses a non-finite score in its own words before any row is summed; the row-sum test lets a
        // NaN through, and a class curve named it by an index into a compacted column (#1569).
        Inputs.RequireFinite(yScore, nameof(yScore));
        if (yScore.Length != (long)n * classCount)
        {
            throw new ArgumentException(
                $"yScore has {yScore.Length} entries; {n} samples over {classCount} classes needs {(long)n * classCount}.",
                nameof(yScore));
        }
        if (average is not (Averaging.Macro or Averaging.Weighted))
        {
            throw new ArgumentException(
                "Multiclass ROC AUC accepts only Averaging.Macro and Averaging.Weighted, as scikit-learn does.",
                nameof(options));
        }
        if (!options.SampleWeight.IsEmpty)
        {
            if (options.SampleWeight.Length != n)
            {
                throw new ArgumentException(
                    $"sampleWeight has {options.SampleWeight.Length} entries but there are {n} samples.",
                    nameof(options));
            }
            if (options.Strategy == MultiClassStrategy.OneVsOne)
            {
                throw new ArgumentException(
                    "scikit-learn does not support sampleWeight for one-vs-one ROC AUC, and neither does this.",
                    nameof(options));
            }
        }

        return n;
    }

    private static int[] ResolveLabels(ReadOnlySpan<int> yTrue, ReadOnlySpan<int> labels, int classCount)
    {
        if (labels.IsEmpty)
        {
            int[] resolved = LabelIndex.SortedUnion(yTrue, default);
            if (resolved.Length != classCount)
            {
                throw new ArgumentException(
                    $"yTrue holds {resolved.Length} distinct labels but classCount is {classCount}. "
                    + "Pass labels when a class is absent from yTrue.",
                    nameof(classCount));
            }
            return resolved;
        }

        if (labels.Length != classCount)
        {
            throw new ArgumentException(
                $"labels has {labels.Length} entries but classCount is {classCount}.", nameof(labels));
        }
        for (int i = 1; i < labels.Length; i++)
        {
            if (labels[i] <= labels[i - 1])
            {
                throw new ArgumentException(
                    "labels must be sorted ascending and unique for multiclass ROC AUC, as scikit-learn requires.",
                    nameof(labels));
            }
        }

        // _multiclass_roc_auc_score's last check on labels, np.setdiff1d(y_true, classes) (#1206).
        foreach (int label in yTrue)
        {
            if (labels.BinarySearch(label) < 0)
            {
                throw new ArgumentException("'y_true' contains labels not in parameter 'labels'", nameof(labels));
            }
        }
        return labels.ToArray();
    }

    private static void ValidateRowSums(ReadOnlySpan<double> yScore, int n, int classCount)
    {
        for (int i = 0; i < n; i++)
        {
            double sum = 0.0;
            int offset = i * classCount;
            for (int c = 0; c < classCount; c++)
            {
                sum += yScore[offset + c];
            }

            if (Math.Abs(sum - 1.0) > AbsoluteTolerance + (RelativeTolerance * Math.Abs(sum)))
            {
                throw new ArgumentException(
                    $"yScore row {i} sums to {sum}; multiclass ROC AUC needs probabilities that sum to 1.",
                    nameof(yScore));
            }
        }
    }

    /// <summary>
    /// A score matrix and the layout it is stored in, so a column can be read
    /// without the caller and the callee having to agree on two loose integers.
    /// The sequential path builds a row-major source over the caller's own span;
    /// the parallel path builds a column-major one over the transposed copy from
    /// <see cref="CopyForWorkers"/> — one <see langword="bool"/> picks the layout,
    /// rather than a pair of integers that could disagree with each other.
    /// </summary>
    private readonly ref struct ScoreSource
    {
        private readonly int _classCount;
        private readonly bool _columnMajor;

        public ScoreSource(
            ReadOnlySpan<int> yTrue, ReadOnlySpan<double> scores, int sampleCount, int classCount, bool columnMajor,
            ClassMembers? members = null)
        {
            // sampleCount is explicit, not derived from yTrue.Length: two spans
            // sliced to a rented array's length can silently agree.
            if (yTrue.Length != sampleCount)
            {
                throw new ArgumentException(
                    $"yTrue holds {yTrue.Length} entries but sampleCount is {sampleCount}. A span sliced to a rented "
                    + "array's length rather than the sample count lands here, and would otherwise shift every "
                    + "column at no visible cost.",
                    nameof(yTrue));
            }
            if (scores.Length != sampleCount * classCount)
            {
                throw new ArgumentException(
                    $"scores holds {scores.Length} entries; {sampleCount} samples over {classCount} classes needs "
                    + $"{sampleCount * classCount}. A span sliced to a rented array's length rather than the sample "
                    + "count lands here, and would otherwise read the wrong column at no visible cost.",
                    nameof(scores));
            }

            YTrue = yTrue;
            Scores = scores;
            _classCount = classCount;
            _columnMajor = columnMajor;
            Members = members;
        }

        /// <summary>The samples of each class, which only the one-vs-one drivers build.</summary>
        public ClassMembers? Members { get; }

        public ReadOnlySpan<int> YTrue { get; }

        public ReadOnlySpan<double> Scores { get; }

        /// <summary>Where class <paramref name="column"/>'s scores begin.</summary>
        public int Offset(int column) => _columnMajor ? column * YTrue.Length : column;

        /// <summary>How far apart consecutive samples of one column are.</summary>
        public int Step => _columnMajor ? 1 : _classCount;
    }

    /// <summary>
    /// One binary ROC-AUC over column <paramref name="column"/> of <paramref name="source"/>,
    /// where samples equal to <paramref name="positiveLabel"/> are the positive class.
    /// </summary>
    /// <remarks>
    /// <paramref name="column"/> is the class's position in the score matrix and
    /// <paramref name="positiveLabel"/> is the label value compared against
    /// <c>yTrue</c>; one-vs-rest needs both because they are not always the same
    /// number once <see cref="MultiClassRocOptions.Labels"/> is used.
    /// </remarks>
    private static double ClassScore(
        ScoreSource source, int column, int positiveLabel, ReadOnlySpan<double> sampleWeight,
        BinaryRoc.Scratch scratch)
    {
        ReadOnlySpan<int> yTrue = source.YTrue;
        int offset = source.Offset(column);
        int step = source.Step;
        int n = yTrue.Length;
        int[] binary = scratch.Binary;
        double[] scoreColumn = scratch.Column;
        for (int i = 0; i < n; i++)
        {
            binary[i] = yTrue[i] == positiveLabel ? 1 : 0;
            scoreColumn[i] = source.Scores[offset + (i * step)];
        }

        return BinaryRoc.Score(
            binary.AsSpan(0, n), scoreColumn.AsSpan(0, n), 1, sampleWeight, scratch);
    }

    /// <summary>
    /// One ordering of one Hand &amp; Till pair: the samples of two classes only,
    /// scored with column <paramref name="column"/> — <paramref name="positiveLabel"/>'s
    /// column, which is one of the two classes <paramref name="pair"/> names by position.
    /// </summary>
    private static double PairScore(
        ScoreSource source, int column, (int A, int B) pair, int positiveLabel, BinaryRoc.Scratch scratch)
    {
        ReadOnlySpan<int> yTrue = source.YTrue;
        int offset = source.Offset(column);
        int step = source.Step;
        int[] binary = scratch.Binary;
        double[] scoreColumn = scratch.Column;
        ClassMembers members = source.Members!;
        int[] indices = members.Indices;
        int nextA = members.Starts[pair.A];
        int endA = members.Starts[pair.A + 1];
        int nextB = members.Starts[pair.B];
        int endB = members.Starts[pair.B + 1];
        int next = 0;

        // Merged by sample index, so the two classes' samples arrive in the order a scan of
        // every sample would have kept them, and the binary problem is the same one.
        while (nextA < endA || nextB < endB)
        {
            int i = nextB >= endB || (nextA < endA && indices[nextA] < indices[nextB])
                ? indices[nextA++]
                : indices[nextB++];

            binary[next] = yTrue[i] == positiveLabel ? 1 : 0;
            scoreColumn[next] = source.Scores[offset + (i * step)];
            next++;
        }

        return BinaryRoc.Score(
            binary.AsSpan(0, next), scoreColumn.AsSpan(0, next), 1, default, scratch);
    }

    /// <summary>The indices of each class's samples, ascending, grouped by the class's position.</summary>
    /// <remarks>
    /// Built once per one-vs-one call, so a pair reads only its two classes' samples instead of
    /// scanning every sample three times. A label outside the classes belongs to no group.
    /// </remarks>
    private sealed class ClassMembers
    {
        public ClassMembers(ReadOnlySpan<int> yTrue, int[] classes)
        {
            int k = classes.Length;
            int[] ordinals = new int[yTrue.Length];
            Starts = new int[k + 1];
            for (int i = 0; i < yTrue.Length; i++)
            {
                int ordinal = Array.BinarySearch(classes, yTrue[i]);
                ordinals[i] = ordinal;
                if (ordinal >= 0)
                {
                    Starts[ordinal + 1]++;
                }
            }

            for (int c = 0; c < k; c++)
            {
                Starts[c + 1] += Starts[c];
            }

            Indices = new int[Starts[k]];
            int[] fill = new int[k];
            Array.Copy(Starts, fill, k);
            for (int i = 0; i < ordinals.Length; i++)
            {
                if (ordinals[i] >= 0)
                {
                    Indices[fill[ordinals[i]]++] = i;
                }
            }
        }

        /// <summary>Sample indices, class by class.</summary>
        public int[] Indices { get; }

        /// <summary>Where each class's run begins in <see cref="Indices"/>, with one entry past the last.</summary>
        public int[] Starts { get; }

        public int Count(int ordinal) => Starts[ordinal + 1] - Starts[ordinal];
    }

    private static double OneVsRest(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int[] classes,
        Averaging average, ReadOnlySpan<double> sampleWeight, double[] classTotals)
    {
        int k = classes.Length;
        double[] scores = new double[k];
        BinaryRoc.Scratch scratch = BinaryRoc.Scratch.Rent(yTrue.Length);

        try
        {
            ScoreSource source = new(yTrue, yScore, yTrue.Length, k, columnMajor: false);
            for (int c = 0; c < k; c++)
            {
                scores[c] = ClassScore(source, c, classes[c], sampleWeight, scratch);
            }
        }
        finally
        {
            scratch.Return();
        }

        return average == Averaging.Macro ? NumpyAverage.Mean(scores) : NumpyAverage.Weighted(scores, classTotals, nameof(yTrue));
    }

    /// <summary>
    /// The inputs, in a shape a worker thread can be handed: <c>yTrue</c>, the
    /// weights if any, and the score matrix transposed so each class's column is
    /// contiguous for the worker that reads it.
    /// </summary>
    /// <remarks>
    /// A copy is the only legal option, and every span sliced from the result
    /// must use the sample count, never the rented length.
    /// </remarks>
    private static (int[] YTrue, double[] ColumnMajor, double[] Weights) CopyForWorkers(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int classCount, ReadOnlySpan<double> sampleWeight)
    {
        int n = yTrue.Length;
        // Named yTrueCopy, not "labels": MultiClassRocOptions.Labels is the
        // k-entry class vocabulary, a same-length array only by coincidence.
        int[] yTrueCopy = ArrayPool<int>.Shared.Rent(n);
        double[] columnMajor = ArrayPool<double>.Shared.Rent(n * classCount);
        double[] weights = sampleWeight.IsEmpty
            ? []
            : ArrayPool<double>.Shared.Rent(n);

        yTrue.CopyTo(yTrueCopy.AsSpan(0, n));
        if (!sampleWeight.IsEmpty)
        {
            sampleWeight.CopyTo(weights.AsSpan(0, n));
        }

        for (int i = 0; i < n; i++)
        {
            int row = i * classCount;
            for (int c = 0; c < classCount; c++)
            {
                columnMajor[(c * n) + i] = yScore[row + c];
            }
        }

        return (yTrueCopy, columnMajor, weights);
    }

    private static void ReturnToPool((int[] YTrue, double[] ColumnMajor, double[] Weights) copy)
    {
        ArrayPool<int>.Shared.Return(copy.YTrue);
        ArrayPool<double>.Shared.Return(copy.ColumnMajor);
        if (copy.Weights.Length > 0)
        {
            // Length 0 is the no-weights case: [] was never rented, so handing it
            // back would give the pool an array it does not own.
            ArrayPool<double>.Shared.Return(copy.Weights);
        }
    }

    /// <summary>
    /// Runs indices <c>0 .. count - 1</c> over at most <paramref name="workers"/> threads, one
    /// <see cref="BinaryRoc.Scratch"/> per worker; each index writes only its own slot.
    /// </summary>
    /// <remarks>
    /// The determinism lives here, not in each driver, so a second parallel driver — the
    /// one-vs-one pair loop — cannot re-derive it differently. Every refusal happens before a
    /// worker starts, so a body that throws has met a defect and escapes as one.
    /// </remarks>
    private static void RunPerIndex(
        int count, int workers, int scratchLength, Action<int, BinaryRoc.Scratch> body)
    {
        // One class alone leaves no pair, and ParallelOptions refuses a degree of zero (#1566).
        if (count == 0)
        {
            return;
        }

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Math.Min(workers, count) };

        // Nothing a worker scores can be refused: Score has refused every input first (#1569), so a worker that throws
        // has met a defect, which Parallel.For surfaces as it is.
        Parallel.For(
            0,
            count,
            parallelOptions,
            () => BinaryRoc.Scratch.Rent(scratchLength),
            (index, _, scratch) =>
            {
                body(index, scratch);
                return scratch;
            },
            scratch => scratch.Return());
    }

    /// <summary>
    /// One-vs-rest with the per-class loop spread over workers. Bit-identical to
    /// <see cref="OneVsRest"/>: class <c>c</c> writes <c>scores[c]</c> and nothing
    /// else, and the averaging below runs on this thread over the caller's class
    /// totals, so no thread's timing can reach a sum.
    /// </summary>
    private static double OneVsRestParallel(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int[] classes,
        Averaging average, ReadOnlySpan<double> sampleWeight, double[] classTotals, int workers)
    {
        int n = yTrue.Length;
        int k = classes.Length;
        bool weighted = !sampleWeight.IsEmpty;
        double[] scores = new double[k];
        var copy = CopyForWorkers(yTrue, yScore, k, sampleWeight);

        try
        {
            RunPerIndex(k, workers, n, (c, scratch) =>
            {
                // Per worker: a span cannot cross into a lambda.
                ScoreSource source = new(
                    copy.YTrue.AsSpan(0, n), copy.ColumnMajor.AsSpan(0, n * k), n, k, columnMajor: true);

                // default, not a zero-length slice: ClassScore reads IsEmpty to
                // decide whether weighting applies at all.
                ReadOnlySpan<double> classWeight = weighted ? copy.Weights.AsSpan(0, n) : default;

                // classes[c], not c: the column and the positive label are
                // the same number only when the labels happen to be 0..k-1.
                scores[c] = ClassScore(source, c, classes[c], classWeight, scratch);
            });
        }
        finally
        {
            ReturnToPool(copy);
        }

        return average == Averaging.Macro ? NumpyAverage.Mean(scores) : NumpyAverage.Weighted(scores, classTotals, nameof(yTrue));
    }

    /// <summary>
    /// One-vs-one with the per-pair loop spread over workers, bit-identical to
    /// <see cref="OneVsOne"/> — each pair writes only its own two slots.
    /// </summary>
    /// <remarks>
    /// Reads pairs from the same <see cref="Pairs"/> table <see cref="OneVsOne"/>
    /// walks, rather than decoding a triangular index, so the two orders cannot
    /// disagree. No weights: <see cref="Validate"/> refuses them here, as
    /// scikit-learn does.
    /// </remarks>
    private static double OneVsOneParallel(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int[] classes, Averaging average, int workers)
    {
        int n = yTrue.Length;
        int k = classes.Length;
        var members = new ClassMembers(yTrue, classes);
        (int A, int B)[] pairs = PresentPairs(Pairs(k, "classCount"), members);
        double[] pairScores = new double[pairs.Length];
        double[] prevalence = new double[pairs.Length];
        var copy = CopyForWorkers(yTrue, yScore, k, default);

        try
        {
            RunPerIndex(pairs.Length, workers, n, (pair, scratch) =>
            {
                ScoreSource source = new(
                    copy.YTrue.AsSpan(0, n), copy.ColumnMajor.AsSpan(0, n * k), n, k, columnMajor: true, members);

                // The tuple carries both columns, so the worker asks for nothing new; ScorePair is at S107's limit.
                ScorePair(source, classes, pairs[pair], pair, pairScores, prevalence, scratch);
            });
        }
        finally
        {
            ReturnToPool(copy);
        }

        return average == Averaging.Macro ? NumpyAverage.Mean(pairScores) : NumpyAverage.Weighted(pairScores, prevalence, nameof(yTrue));
    }

    private static double OneVsOne(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int[] classes, Averaging average)
    {
        int k = classes.Length;
        var members = new ClassMembers(yTrue, classes);
        (int A, int B)[] pairs = PresentPairs(Pairs(k, "classCount"), members);
        double[] pairScores = new double[pairs.Length];
        double[] prevalence = new double[pairs.Length];
        BinaryRoc.Scratch scratch = BinaryRoc.Scratch.Rent(yTrue.Length);

        try
        {
            ScoreSource source = new(
                yTrue, yScore, yTrue.Length, k, columnMajor: false, members);
            for (int pair = 0; pair < pairs.Length; pair++)
            {
                ScorePair(source, classes, pairs[pair], pair, pairScores, prevalence, scratch);
            }
        }
        finally
        {
            scratch.Return();
        }

        return average == Averaging.Macro ? NumpyAverage.Mean(pairScores) : NumpyAverage.Weighted(pairScores, prevalence, nameof(yTrue));
    }

    /// <summary>
    /// The body of one pair, kept separate so the arithmetic exists in one
    /// place regardless of what iterates the pairs. Writes only its own two
    /// slots.
    /// </summary>
    private static void ScorePair(
        ScoreSource source, int[] classes, (int A, int B) pair, int index,
        double[] pairScores, double[] prevalence, BinaryRoc.Scratch scratch)
    {
        int n = source.YTrue.Length;
        int size = source.Members!.Count(pair.A) + source.Members.Count(pair.B);

        // Hand & Till: each ordering of the pair is scored with its own column,
        // and the two are averaged.
        double aScore = PairScore(source, pair.A, pair, classes[pair.A], scratch);
        double bScore = PairScore(source, pair.B, pair, classes[pair.B], scratch);

        pairScores[index] = (aScore + bScore) * 0.5;
        prevalence[index] = (double)size / n;
    }

    /// <summary>Every unordered class pair, in the order this method's nested loops produce them.</summary>
    private static (int A, int B)[] Pairs(int k, string paramName)
    {
        // In long: k * (k - 1) wraps in int past 46,341 classes and threw OverflowException (#1469).
        long count = (long)k * (k - 1) / 2;
        if (count > TableLength.MaxLength)
        {
            throw new ArgumentException($"{k} classes make {count} pairs, more than one array holds.", paramName);
        }

        (int A, int B)[] pairs = new (int, int)[count];
        int next = 0;
        for (int a = 0; a < k; a++)
        {
            for (int b = a + 1; b < k; b++)
            {
                pairs[next++] = (a, b);
            }
        }
        return pairs;
    }

    /// <summary><c>numpy.isclose(total, 0)</c>'s absolute tolerance, which is all it applies against zero.</summary>
    internal const double ZeroTotalTolerance = 1e-8;

    /// <summary>The pairs whose two classes both occur in the target, the only ones scikit-learn scores.</summary>
    /// <remarks>
    /// <c>_average_multiclass_ovo_score</c> pairs the classes <c>np.unique(y_true)</c> holds, so a class <c>labels</c>
    /// names and no sample carries is in no pair: 0.8888888888888888 where this answered NaN (#1277).
    /// </remarks>
    private static (int A, int B)[] PresentPairs((int A, int B)[] pairs, ClassMembers members) =>
        Array.FindAll(pairs, pair => members.Count(pair.A) > 0 && members.Count(pair.B) > 0);

    /// <summary>
    /// The weight each class carries, summed in sample order — a count when the samples are unweighted:
    /// <c>average_weight</c> in <c>_average_binary_score</c>, which the weighted mean averages by (#1586).
    /// </summary>
    private static double[] ClassTotals(ReadOnlySpan<int> yTrue, int[] classes, ReadOnlySpan<double> sampleWeight)
    {
        double[] totals = new double[classes.Length];
        bool weighted = !sampleWeight.IsEmpty;
        for (int i = 0; i < yTrue.Length; i++)
        {
            int c = Array.BinarySearch(classes, yTrue[i]);
            if (c >= 0)
            {
                totals[c] += weighted ? sampleWeight[i] : 1.0;
            }
        }

        return totals;
    }
}
