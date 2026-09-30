using System.Buffers;

namespace Lodestar.Metrics.Internal;

/// <summary>
/// The binary ROC curve and the area under it — the mechanics of
/// scikit-learn's <c>_binary_clf_curve</c> followed by <c>auc</c>.
/// </summary>
/// <remarks>
/// Samples are sorted by descending score and equal scores are consumed as one
/// group, which is what makes ties come out the same as scikit-learn's. The
/// trapezoid is accumulated on unnormalised counts and divided once at the end:
/// fewer roundings, and the same number.
/// </remarks>
internal static class BinaryRoc
{
    // The radix loses below ~8 000 and wins above (1.21x at 10 000, 1.32x at a
    // million, Intel i7-4770S; #206).
    private const int RadixThreshold = 8_192;

    // 16-bit digits: four passes and a 64 K histogram beat eight passes and a
    // 256-entry one from 16 000 samples up, by 1.17x at a million.
    private const int RadixBits = 16;
    private const int RadixBuckets = 1 << RadixBits;
    private const ulong RadixMask = RadixBuckets - 1;
    private const ulong SignBit = 0x8000000000000000UL;

    private struct Point
    {
        public double Weight;
        public double PositiveWeight;
    }

    /// <summary>
    /// The four buffers one ROC curve needs, rented once and reused across
    /// curves. Going parallel means one of these per worker — never one per
    /// class, which is what the sequential loop's shared buffers already avoid,
    /// and never one per call, which is what <c>keys</c> and <c>points</c> used
    /// to be. At n=100 000 those two are 800 KB and 1.6 MB: large-object heap,
    /// whose allocation takes a lock that eight workers would queue on.
    /// </summary>
    internal sealed class Scratch
    {
        private readonly double[] _keys;
        private readonly Point[] _points;

        // Null below RadixThreshold, where Array.Sort is the faster call and these
        // would be 48 bytes per sample rented for nothing.
        private readonly ulong[]? _codes;
        private readonly ulong[]? _codesAlt;
        private readonly int[]? _order;
        private readonly int[]? _orderAlt;
        private readonly double[]? _sortedKeys;
        private readonly Point[]? _sortedPoints;

        private Scratch(int[] binary, double[] column, double[] keys, Point[] points, int radixLength)
        {
            Binary = binary;
            Column = column;
            _keys = keys;
            _points = points;

            if (radixLength == 0)
            {
                return;
            }

            _codes = ArrayPool<ulong>.Shared.Rent(radixLength);
            _codesAlt = ArrayPool<ulong>.Shared.Rent(radixLength);
            _order = ArrayPool<int>.Shared.Rent(radixLength);
            _orderAlt = ArrayPool<int>.Shared.Rent(radixLength);
            _sortedKeys = ArrayPool<double>.Shared.Rent(radixLength);
            _sortedPoints = ArrayPool<Point>.Shared.Rent(radixLength);
        }

        internal int[] Binary { get; }

        internal double[] Column { get; }

        internal static Scratch Rent(int minimumLength)
        {
            int length = Math.Max(1, minimumLength);
            return new Scratch(
                ArrayPool<int>.Shared.Rent(length),
                ArrayPool<double>.Shared.Rent(length),
                ArrayPool<double>.Shared.Rent(length),
                ArrayPool<Point>.Shared.Rent(length),
                length >= RadixThreshold ? length : 0);
        }

        internal void Return()
        {
            ArrayPool<int>.Shared.Return(Binary);
            ArrayPool<double>.Shared.Return(Column);
            ArrayPool<double>.Shared.Return(_keys);
            ArrayPool<Point>.Shared.Return(_points);

            if (_codes is null)
            {
                return;
            }

            ArrayPool<ulong>.Shared.Return(_codes);
            ArrayPool<ulong>.Shared.Return(_codesAlt!);
            ArrayPool<int>.Shared.Return(_order!);
            ArrayPool<int>.Shared.Return(_orderAlt!);
            ArrayPool<double>.Shared.Return(_sortedKeys!);
            ArrayPool<Point>.Shared.Return(_sortedPoints!);
        }

        // _keys/_points stay private (Point is private to BinaryRoc). Named
        // Compute, not Score, so it doesn't shadow BinaryRoc.Score (S3218).
        internal double Compute(
            ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight,
            bool weightsChecked, string weightParam)
        {
            // Every caller has refused a non-finite score already (#1585, #1569); some have checked the weights too.
            int n = Validate(yTrue, yScore, sampleWeight, weightsChecked);
            bool hasNegative = BuildPoints(yTrue, yScore, posLabel, sampleWeight, _keys, _points, scoresFinite: true);

            double area;
            bool monotone;
            if (hasNegative)
            {
                // Only a negative weight can turn the rates back, and then the order ties are summed in decides it.
                area = AccumulateAsRocCurve(_keys, _points, n, out monotone);
            }
            else if (_codes is null || n < RadixThreshold)
            {
                Array.Sort(_keys, _points, 0, n);
                area = Accumulate(_keys, _points, n, checkMonotone: false, out monotone);
            }
            else
            {
                RadixSort(n);
                area = Accumulate(_sortedKeys!, _sortedPoints!, n, checkMonotone: false, out monotone);
            }

            // auc refuses a false-positive rate that turns back, which only a negative weight can make (#1601).
            return monotone
                ? area
                : throw new ArgumentException(
                    "x is neither increasing nor decreasing: a negative sample weight turns the false-positive rate back.",
                    weightParam);
        }

        /// <summary>
        /// The same sorted walk, summed as scikit-learn's <c>average_precision_score</c>
        /// rather than integrated as <c>auc</c>.
        /// </summary>
        internal double ComputeAveragePrecision(
            ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight)
        {
            int n = Validate(yTrue, yScore, sampleWeight);
            BuildPoints(yTrue, yScore, posLabel, sampleWeight, _keys, _points, scoresFinite: false);

            if (_codes is null || n < RadixThreshold)
            {
                Array.Sort(_keys, _points, 0, n);
                return AccumulateAveragePrecision(_keys, _points, n);
            }

            RadixSort(n);
            return AccumulateAveragePrecision(_sortedKeys!, _sortedPoints!, n);
        }

        /// <summary>
        /// Orders the first <paramref name="n"/> points by ascending key into
        /// <c>_sortedKeys</c>/<c>_sortedPoints</c>, by radix rather than by comparison.
        /// </summary>
        /// <remarks>
        /// Four LSD passes over 16-bit digits of the order-preserving encoding, carrying
        /// a position rather than the 16-byte point: the pairs moved are 12 bytes, and
        /// the points are gathered once at the end instead of on every pass.
        /// </remarks>
        private void RadixSort(int n)
        {
            ulong[] codes = _codes!;
            ulong[] codesAlt = _codesAlt!;
            int[] order = _order!;
            int[] orderAlt = _orderAlt!;

            for (int i = 0; i < n; i++)
            {
                codes[i] = Encode(_keys[i]);
                order[i] = i;
            }

            int[] histogram = ArrayPool<int>.Shared.Rent(RadixBuckets);
            try
            {
                for (int shift = 0; shift < 64; shift += RadixBits)
                {
                    if (!Pass(codes, order, codesAlt, orderAlt, n, shift, histogram))
                    {
                        continue;
                    }

                    (codes, codesAlt) = (codesAlt, codes);
                    (order, orderAlt) = (orderAlt, order);
                }
            }
            finally
            {
                ArrayPool<int>.Shared.Return(histogram);
            }

            double[] sortedKeys = _sortedKeys!;
            Point[] sortedPoints = _sortedPoints!;
            for (int i = 0; i < n; i++)
            {
                int source = order[i];
                sortedKeys[i] = _keys[source];
                sortedPoints[i] = _points[source];
            }
        }

        /// <summary>One counting pass; false when the digit is constant and the pass would only copy.</summary>
        private static bool Pass(
            ulong[] codes, int[] order, ulong[] codesOut, int[] orderOut, int n, int shift, int[] histogram)
        {
            Array.Clear(histogram, 0, RadixBuckets);
            for (int i = 0; i < n; i++)
            {
                histogram[(int)((codes[i] >> shift) & RadixMask)]++;
            }

            // Scores routinely share an exponent, which leaves whole digits constant.
            // Skipping those passes is most of what makes four passes cheap.
            if (histogram[(int)((codes[0] >> shift) & RadixMask)] == n)
            {
                return false;
            }

            int total = 0;
            for (int bucket = 0; bucket < RadixBuckets; bucket++)
            {
                int count = histogram[bucket];
                histogram[bucket] = total;
                total += count;
            }

            for (int i = 0; i < n; i++)
            {
                int destination = histogram[(int)((codes[i] >> shift) & RadixMask)]++;
                codesOut[destination] = codes[i];
                orderOut[destination] = order[i];
            }

            return true;
        }

        /// <summary>
        /// Maps a non-NaN <see cref="double"/> onto a <see cref="ulong"/> whose unsigned
        /// order is the double's own order.
        /// </summary>
        /// <remarks>
        /// Negatives invert entirely, positives flip only the sign bit — the standard
        /// transform. NaN is refused by <c>BuildPoints</c> before it can reach here, which
        /// is what makes a total order available at all.
        /// </remarks>
        private static ulong Encode(double value)
        {
            ulong bits = (ulong)BitConverter.DoubleToInt64Bits(value);
            return (bits & SignBit) != 0 ? ~bits : bits | SignBit;
        }

        // These — Validate, BuildPoints, AccumulateAsRocCurve, Accumulate, IsLastOfGroup, and Encode, Pass and
        // AccumulateAveragePrecision — are reachable only from Scratch (S3398).
        private static int Validate(
            ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, ReadOnlySpan<double> sampleWeight, bool weightsChecked = false)
        {
            int n = yTrue.Length;
            if (yScore.Length != n)
            {
                throw new ArgumentException(
                    $"yTrue has {n} entries and yScore has {yScore.Length}; they must agree.", nameof(yScore));
            }
            if (n == 0)
            {
                throw new ArgumentException("yTrue and yScore are empty; there is nothing to score.", nameof(yTrue));
            }
            if (!sampleWeight.IsEmpty && sampleWeight.Length != n)
            {
                throw new ArgumentException(
                    $"sampleWeight has {sampleWeight.Length} entries but there are {n} samples.",
                    nameof(sampleWeight));
            }
            if (!weightsChecked)
            {
                Inputs.ValidateSampleWeight(sampleWeight);
            }

            return n;
        }

        /// <summary>Fills the keys and points, and says whether a weight is negative, the one case a curve can turn back.</summary>
        private static bool BuildPoints(
            ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight,
            double[] keys, Point[] points, bool scoresFinite)
        {
            bool weighted = !sampleWeight.IsEmpty;
            bool hasNegative = false;

            for (int i = 0; i < yTrue.Length; i++)
            {
                double score = yScore[i];

                // RocAuc.Score has already refused a non-finite score in check_array's words (#1585).
                if (!scoresFinite)
                {
                    ClassifierCurve.RequireFiniteScore(yScore, i);
                }

                double weight = weighted ? sampleWeight[i] : 1.0;
                hasNegative |= weight < 0.0;
                keys[i] = -score;
                points[i].Weight = weight;
                points[i].PositiveWeight = yTrue[i] == posLabel ? weight : 0.0;
            }

            return hasNegative;
        }

        /// <summary>
        /// The area with the samples in <c>roc_curve</c>'s own order, <c>argsort(y_score, stable=True,
        /// descending=True)</c>: score descending, a tie's earlier sample first, so its cumulative weights round as
        /// scikit-learn's do. Pooled buffers, and a (key, index) pair sorted by value, so no comparer is allocated.
        /// </summary>
        private static double AccumulateAsRocCurve(double[] keys, Point[] points, int n, out bool monotone)
        {
            (double Key, int Index)[] order = ArrayPool<(double, int)>.Shared.Rent(n);
            double[] sortedKeys = ArrayPool<double>.Shared.Rent(n);
            Point[] sortedPoints = ArrayPool<Point>.Shared.Rent(n);
            try
            {
                for (int i = 0; i < n; i++)
                {
                    order[i] = (keys[i], i);
                }

                Array.Sort(order, 0, n);
                for (int i = 0; i < n; i++)
                {
                    sortedKeys[i] = order[i].Key;
                    sortedPoints[i] = points[order[i].Index];
                }

                return Accumulate(sortedKeys, sortedPoints, n, checkMonotone: true, out monotone);
            }
            finally
            {
                ArrayPool<(double, int)>.Shared.Return(order);
                ArrayPool<double>.Shared.Return(sortedKeys);
                ArrayPool<Point>.Shared.Return(sortedPoints);
            }
        }

        /// <summary>The trapezoid area under the curve, and whether its false-positive rates move one way only.</summary>
        /// <remarks>
        /// <c>roc_curve</c> prepends a 0 and turns a total at or below zero into NaN rates, so such a curve is NaN and
        /// never refused; <c>auc</c> then refuses rates that rise and fall. Only the false-positive side is checked,
        /// as <c>auc</c> checks only <c>x</c>.
        /// </remarks>
        private static double Accumulate(double[] keys, Point[] points, int n, bool checkMonotone, out bool monotone)
        {
            // auc reads fps / fps[-1], so the turn is tested on that quotient, whose rounding can flatten a step.
            double falseTotal = 0.0;
            if (checkMonotone)
            {
                for (int i = 0; i < n; i++)
                {
                    falseTotal += points[i].Weight - points[i].PositiveWeight;
                }
            }

            double truePositives = 0.0;
            double falsePositives = 0.0;
            double previousTrue = 0.0;
            double previousFalse = 0.0;
            double previousRate = 0.0;
            double area = 0.0;
            bool rose = false;
            bool fell = false;

            for (int i = 0; i < n; i++)
            {
                truePositives += points[i].PositiveWeight;
                falsePositives += points[i].Weight - points[i].PositiveWeight;

                if (!IsLastOfGroup(keys, i, n))
                {
                    continue;
                }

                if (checkMonotone)
                {
                    double rate = falsePositives / falseTotal;
                    rose |= rate > previousRate;
                    fell |= rate < previousRate;
                    previousRate = rate;
                }

                area += (falsePositives - previousFalse) * (truePositives + previousTrue) * 0.5;
                previousTrue = truePositives;
                previousFalse = falsePositives;
            }

            // A total at or below zero makes roc_curve's rates NaN, one class included: 1.9.1 warns and answers NaN (#1277).
            monotone = falsePositives <= 0.0 || !(rose && fell);
            return truePositives <= 0.0 || falsePositives <= 0.0 ? double.NaN : area / (truePositives * falsePositives);
        }

        /// <summary>The step sum over the precision-recall curve, not the area under it.</summary>
        /// <remarks>
        /// <c>Sum (R_n - R_(n-1)) * P_n</c>, deliberately not the trapezoid, which reads
        /// two thresholds apart as if the curve were linear between them: measured on
        /// scikit-learn 1.9.1, y_true = [0, 0, 1, 1] against y_score = [0.1, 0.4, 0.35,
        /// 0.8] sums to 0.8333333333333333 where the trapezoid gives 0.7916666666666666.
        /// Dividing by the positive weight waits until the end -- every recall step
        /// shares that denominator, so once is the reference's own <c>tps / tps[-1]</c>.
        /// </remarks>
        private static double AccumulateAveragePrecision(double[] keys, Point[] points, int n)
        {
            double truePositives = 0.0;
            double falsePositives = 0.0;
            double previousTrue = 0.0;
            double sum = 0.0;

            for (int i = 0; i < n; i++)
            {
                truePositives += points[i].PositiveWeight;
                falsePositives += points[i].Weight - points[i].PositiveWeight;

                if (!IsLastOfGroup(keys, i, n))
                {
                    continue;
                }

                double positives = truePositives + falsePositives;
                double precision = positives > 0.0 ? truePositives / positives : 0.0;
                sum += (truePositives - previousTrue) * precision;
                previousTrue = truePositives;
            }

            // No positive sample: scikit-learn warns that recall is taken as one for
            // all thresholds and returns 0.0, measured on 1.9.1. Dividing would be 0/0.
#pragma warning disable S1244
            return truePositives == 0.0 ? 0.0 : sum / truePositives;
#pragma warning restore S1244
        }

        private static bool IsLastOfGroup(double[] keys, int i, int n)
        {
            // SonarLint S1244 warns against comparing floating point for exact
            // equality, which is right for arithmetic and wrong here: ties in a
            // score column are bit-identical doubles, and grouping them is the
            // whole point. scikit-learn's _binary_clf_curve locates its own
            // thresholds the same way — nonzero(diff(y_score)) at
            // sklearn/metrics/_ranking.py:917, scikit-learn 1.9.1. A tolerance
            // would merge scores that are genuinely distinct and change the
            // curve — the approximate version is the wrong answer here, not a
            // safer one.
#pragma warning disable S1244
            return i == n - 1 || keys[i] != keys[i + 1];
#pragma warning restore S1244
        }
    }

    /// <summary>
    /// The binary score for a caller that has refused a non-finite score already — <c>RocAuc.Score</c>, and
    /// <c>RocAuc.MultiClass</c>'s micro average, which has checked its weights too.
    /// </summary>
    public static double Score(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight,
        bool weightsChecked, string weightParam)
    {
        Scratch scratch = Scratch.Rent(yTrue.Length);
        try
        {
            return scratch.Compute(yTrue, yScore, posLabel, sampleWeight, weightsChecked, weightParam);
        }
        finally
        {
            scratch.Return();
        }
    }

    /// <summary>A multiclass class or pair curve: <c>RocAuc.MultiClass</c> has checked the scores and the weights it carries in <c>options</c>.</summary>
    public static double Score(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight,
        Scratch scratch) =>
        scratch.Compute(yTrue, yScore, posLabel, sampleWeight, weightsChecked: true, MultiClassWeightParam);

    /// <summary>The parameter <c>RocAuc.MultiClass</c> carries its sample weights in.</summary>
    private const string MultiClassWeightParam = "options";

    public static double AveragePrecision(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight)
    {
        Scratch scratch = Scratch.Rent(yTrue.Length);
        try
        {
            return scratch.ComputeAveragePrecision(yTrue, yScore, posLabel, sampleWeight);
        }
        finally
        {
            scratch.Return();
        }
    }

    public static double AveragePrecision(
        ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int posLabel, ReadOnlySpan<double> sampleWeight,
        Scratch scratch) =>
        scratch.ComputeAveragePrecision(yTrue, yScore, posLabel, sampleWeight);
}
