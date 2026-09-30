using System.Buffers;
using System.Runtime.ExceptionServices;

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

        RefuseBinaryAveraging(average, nameof(options));

        // roc_auc_score's order: the arrays, the binary path of one or two columns (#1605), then _multiclass_roc_auc_score's —
        // the row sums, the averaging, the labels, a one-vs-one weight, the row and weight counts, the weights (#1601, #1604).
        int n = Validate(yTrue, yScore, classCount, options);
        int rows = yScore.Length / classCount;

        // One or two columns over a y_true of two labels or one is roc_auc_score's binary path, not a multiclass one (#1605).
        int distinct = classCount <= 2 ? DistinctLabelsUpToThree(yTrue) : 3;
        if (distinct <= 2)
        {
            return BinaryPath(yTrue, yScore, classCount, distinct, options, rows, nameof(yScore));
        }

        ValidateRowSums(yScore, rows, classCount);
        RefuseMicroOneVsOne(average, options.Strategy, nameof(options));

        int[] classes = ResolveLabels(yTrue, options.Labels, classCount);
        ValidateWeightShape(options, n, rows, nameof(yScore));

        // 0 and 1 both mean sequential.
        int workers = Math.Max(1, options.MaxDegreeOfParallelism);

        if (options.Strategy == MultiClassStrategy.OneVsRest && average == Averaging.Micro)
        {
            // Micro is the one binary score over the class matrix raveled, each weight repeated per class (#1601).
            Inputs.ValidateSampleWeight(options.SampleWeight, nameof(options));
            return Micro(yTrue, yScore, classes, options.SampleWeight, nameof(options));
        }

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

            // roc_curve checks the weights only for a class with both labels; one-label classes answer nan first.
            if (AnyClassHasBothLabels(yTrue, classes))
            {
                Inputs.ValidateSampleWeight(options.SampleWeight, nameof(options));
            }

            return workers == 1
                ? OneVsRest(yTrue, yScore, classes, average, options.SampleWeight, classTotals)
                : OneVsRestParallel(yTrue, yScore, classes, average, options.SampleWeight, classTotals, workers);
        }

        return workers == 1
            ? OneVsOne(yTrue, yScore, classes, average)
            : OneVsOneParallel(yTrue, yScore, classes, average, workers);
    }

    /// <summary><c>roc_auc_score</c>'s <c>validate_params</c>, which refuses <c>'binary'</c> before any array is read.</summary>
    private static void RefuseBinaryAveraging(Averaging average, string paramName)
    {
        if (average is not (Averaging.Micro or Averaging.Macro or Averaging.Weighted))
        {
            string name = average == Averaging.Binary ? "binary" : average.ToString();
            throw new ArgumentException(
                $"The 'average' parameter of roc_auc_score must be a str among {{'macro', 'micro', 'samples', 'weighted'}} or None. Got '{name}' instead.",
                paramName);
        }
    }

    /// <summary><c>_multiclass_roc_auc_score</c>'s averaging step: micro has no one-vs-one meaning.</summary>
    private static void RefuseMicroOneVsOne(Averaging average, MultiClassStrategy strategy, string paramName)
    {
        if (average == Averaging.Micro && strategy == MultiClassStrategy.OneVsOne)
        {
            throw new ArgumentException(
                "average must be one of ('macro', 'weighted', None) for multiclass problems", paramName);
        }
    }

    /// <summary>
    /// <c>_average_binary_score(…, average='micro')</c>: <c>label_binarize</c>'s matrix and the scores raveled row by
    /// row, a sample's weight repeated across its classes, scored as one binary problem.
    /// </summary>
    private static double Micro(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int[] classes, ReadOnlySpan<double> sampleWeight, string weightParam)
    {
        int n = yTrue.Length;
        int k = classes.Length;
        int[] flat = new int[yScore.Length];
        double[] repeated = sampleWeight.IsEmpty ? [] : new double[yScore.Length];
        for (int i = 0; i < n; i++)
        {
            for (int c = 0; c < k; c++)
            {
                int at = (i * k) + c;
                flat[at] = yTrue[i] == classes[c] ? 1 : 0;
                if (repeated.Length != 0)
                {
                    repeated[at] = sampleWeight[i];
                }
            }
        }

        return BinaryRoc.Score(flat, yScore, 1, repeated, weightsChecked: true, weightParam);
    }

    private static int Validate(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int classCount, MultiClassRocOptions options)
    {
        int n = yTrue.Length;
        if (options.MaxDegreeOfParallelism < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.MaxDegreeOfParallelism,
                "MaxDegreeOfParallelism cannot be negative. 0 and 1 are both sequential.");
        }
        if (classCount < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(classCount), classCount, "yScore needs at least one column of scores.");
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

        // Only a whole number of rows makes a flat span a matrix; how many rows is checked where scikit-learn does (#1604).
        if (yScore.Length % classCount != 0)
        {
            throw new ArgumentException(
                $"yScore has {yScore.Length} entries, not a whole number of rows of {classCount} classes.", nameof(yScore));
        }

        return n;
    }

    /// <summary>
    /// What scikit-learn refuses after the labels: a weight under one-vs-one, then a row or weight count other than
    /// one per sample, under <c>yScore</c> for the rows and <c>options</c> for the weights.
    /// </summary>
    private static void ValidateWeightShape(MultiClassRocOptions options, int n, int rows, string scoreParam)
    {
        if (!options.SampleWeight.IsEmpty && options.Strategy == MultiClassStrategy.OneVsOne)
        {
            throw new ArgumentException(
                "sample_weight is not supported for multiclass one-vs-one ROC AUC, 'sample_weight' must be None in this case.",
                nameof(options));
        }

        // check_consistent_length, which the class curves reach after the labels and the one-vs-one weight (#1604).
        RequireConsistentLengths(options, n, rows, scoreParam);
    }

    /// <summary>
    /// <c>check_consistent_length(y_true, y_score, sample_weight)</c>, naming every length it compared, the weights'
    /// included: <c>[4, 3, 2]</c>, and <c>[4, 4, 2]</c> when only the weights disagree.
    /// </summary>
    private static void RequireConsistentLengths(MultiClassRocOptions options, int n, int rows, string scoreParam)
    {
        bool weighted = !options.SampleWeight.IsEmpty;
        if (rows != n || (weighted && options.SampleWeight.Length != n))
        {
            string lengths = weighted ? $"[{n}, {rows}, {options.SampleWeight.Length}]" : $"[{n}, {rows}]";
            throw new ArgumentException(
                $"Found input variables with inconsistent numbers of samples: {lengths}", rows != n ? scoreParam : nameof(options));
        }
    }

    /// <summary>
    /// <c>roc_auc_score</c>'s binary path, which one or two score columns take over a <c>y_true</c> of two labels or one,
    /// once the arrays have passed their own checks: one label is nan; two meet <c>check_consistent_length</c>, then
    /// <c>column_or_1d</c>, which refuses two columns and scores one against the greater label.
    /// </summary>
    private static double BinaryPath(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int classCount, int distinct,
        MultiClassRocOptions options, int rows, string scoreParam)
    {
        if (distinct == 1)
        {
            return double.NaN;
        }

        int n = yTrue.Length;
        RequireConsistentLengths(options, n, rows, scoreParam);
        if (classCount == 2)
        {
            throw new ArgumentException($"y should be a 1d array, got an array of shape ({rows}, 2) instead.", scoreParam);
        }

        // label_binarize(y_true, classes=[a, b])[:, 0] marks the greater label positive.
        int greater = int.MinValue;
        foreach (int label in yTrue)
        {
            greater = Math.Max(greater, label);
        }

        return BinaryRoc.Score(yTrue, yScore, greater, options.SampleWeight, weightsChecked: false, nameof(options));
    }

    /// <summary>How many distinct labels <paramref name="yTrue"/> holds, counted up to three: <c>type_of_target</c>'s binary is two or fewer.</summary>
    private static int DistinctLabelsUpToThree(ReadOnlySpan<int> yTrue)
    {
        int first = yTrue[0];
        int? second = null;
        foreach (int label in yTrue)
        {
            if (label == first || label == second)
            {
                continue;
            }

            if (second is not null)
            {
                return 3;
            }

            second = label;
        }

        return second is null ? 1 : 2;
    }

    private static int[] ResolveLabels(ReadOnlySpan<int> yTrue, ReadOnlySpan<int> labels, int classCount)
    {
        if (labels.IsEmpty)
        {
            int[] resolved = LabelIndex.SortedUnion(yTrue, default);
            if (resolved.Length != classCount)
            {
                throw new ArgumentException(
                    "Number of classes in y_true not equal to the number of columns in 'y_score'", nameof(classCount));
            }
            return resolved;
        }

        // _multiclass_roc_auc_score's order and sentences: unique, then ordered, then counted, then covering y_true (#1601).
        int[] sorted = labels.ToArray();
        Array.Sort(sorted);
        for (int i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                throw new ArgumentException("Parameter 'labels' must be unique", nameof(labels));
            }
        }
        if (!labels.SequenceEqual(sorted))
        {
            throw new ArgumentException("Parameter 'labels' must be ordered", nameof(labels));
        }
        if (labels.Length != classCount)
        {
            throw new ArgumentException(
                $"Number of given labels, {labels.Length}, not equal to the number of columns in 'y_score', {classCount}",
                nameof(labels));
        }

        // np.setdiff1d(y_true, classes) (#1206).
        foreach (int label in yTrue)
        {
            if (Array.BinarySearch(sorted, label) < 0)
            {
                throw new ArgumentException("'y_true' contains labels not in parameter 'labels'", nameof(labels));
            }
        }
        return sorted;
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
                    "Target scores need to be probabilities for multiclass roc_auc, i.e. they should sum up to 1.0 over classes",
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
        int positives = 0;
        for (int i = 0; i < n; i++)
        {
            binary[i] = yTrue[i] == positiveLabel ? 1 : 0;
            positives += binary[i];
            scoreColumn[i] = source.Scores[offset + (i * step)];
        }

        // _binary_roc_auc_score answers nan on a one-label column before roc_curve reads a weight (#1601).
        if (positives == 0 || positives == n)
        {
            return double.NaN;
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
    /// one-vs-one pair loop — cannot re-derive it differently. A class curve can still refuse its
    /// rates under a negative weight, so each index returns its refusal into its own slot and the
    /// lowest one is rethrown as the instance it is, whatever the workers' timing.
    /// </remarks>
    private static void RunPerIndex(
        int count, int workers, int scratchLength, bool canRefuse, Func<int, BinaryRoc.Scratch, ArgumentException?> body)
    {
        // One class alone leaves no pair, and ParallelOptions refuses a degree of zero (#1566).
        if (count == 0)
        {
            return;
        }

        // Only a negative weight lets a curve refuse; otherwise no slot is ever filled, so none is allocated.
        ArgumentException?[]? failures = canRefuse ? new ArgumentException?[count] : null;
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Math.Min(workers, count) };
        Parallel.For(
            0,
            count,
            parallelOptions,
            () => BinaryRoc.Scratch.Rent(scratchLength),
            (index, _, scratch) =>
            {
                // Its own slot, so which worker lost the race cannot decide which refusal the caller sees.
                ArgumentException? failure = body(index, scratch);
                if (failures is not null)
                {
                    failures[index] = failure;
                }
                return scratch;
            },
            scratch => scratch.Return());

        // The lowest class's, as the sequential loop meets it first; the instance itself, not an AggregateException.
        ArgumentException? first = failures is null ? null : Array.Find(failures, failure => failure is not null);
        if (first is not null)
        {
            ExceptionDispatchInfo.Capture(first).Throw();
        }
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
            bool canRefuse = HasNegative(sampleWeight);
            RunPerIndex(k, workers, n, canRefuse, (c, scratch) =>
            {
                // Per worker: a span cannot cross into a lambda.
                ScoreSource source = new(
                    copy.YTrue.AsSpan(0, n), copy.ColumnMajor.AsSpan(0, n * k), n, k, columnMajor: true);

                // default, not a zero-length slice: ClassScore reads IsEmpty to
                // decide whether weighting applies at all.
                ReadOnlySpan<double> classWeight = weighted ? copy.Weights.AsSpan(0, n) : default;

                try
                {
                    // classes[c], not c: the column and the positive label are
                    // the same number only when the labels happen to be 0..k-1.
                    scores[c] = ClassScore(source, c, classes[c], classWeight, scratch);
                    return null;
                }
                catch (ArgumentException ex) when (canRefuse)
                {
                    // A class curve can still refuse its own rates under a negative weight (#1601); anything else escapes.
                    return ex;
                }
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
    /// disagree. No weights: <see cref="ValidateWeightShape"/> refuses them here, as
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
            RunPerIndex(pairs.Length, workers, n, canRefuse: false, (pair, scratch) =>
            {
                ScoreSource source = new(
                    copy.YTrue.AsSpan(0, n), copy.ColumnMajor.AsSpan(0, n * k), n, k, columnMajor: true, members);

                // The tuple carries both columns, so the worker asks for nothing new; ScorePair is at S107's limit.
                // One-vs-one takes no weights, so no pair curve can refuse.
                ScorePair(source, classes, pairs[pair], pair, pairScores, prevalence, scratch);
                return null;
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

    /// <summary>Whether any weight is negative, the one case a class curve can refuse its rates.</summary>
    private static bool HasNegative(ReadOnlySpan<double> weights)
    {
        foreach (double weight in weights)
        {
            if (weight < 0.0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Whether some class's one-vs-rest column holds both labels, so that its curve reads the weights.</summary>
    private static bool AnyClassHasBothLabels(ReadOnlySpan<int> yTrue, int[] classes)
    {
        // A column holds both labels unless every sample is its class or none is: two present classes suffice.
        int first = Array.BinarySearch(classes, yTrue[0]);
        foreach (int value in yTrue)
        {
            if (Array.BinarySearch(classes, value) != first)
            {
                return true;
            }
        }

        return false;
    }

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
