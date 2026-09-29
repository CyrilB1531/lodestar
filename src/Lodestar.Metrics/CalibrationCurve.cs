using System.Buffers;
using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// The reliability curve as plot data — the equivalent of
/// <c>sklearn.calibration.calibration_curve</c>.
/// </summary>
/// <remarks>
/// <c>sklearn.calibration</c>, not <c>sklearn.metrics</c>: the one member of the family
/// that lives in the other module. Both arrays share a length, and it is <b>not</b>
/// <c>nBins</c> — an empty bin is dropped, so it follows the data (#286).
/// </remarks>
public sealed class CalibrationCurve
{
    private CalibrationCurve(double[] probTrue, double[] probPred)
    {
        // Read-only views: cast back to double[], the arrays let a caller rewrite the curve (#1473).
        ProbTrue = Array.AsReadOnly(probTrue);
        ProbPred = Array.AsReadOnly(probPred);
    }

    /// <summary>The share of positives in each non-empty bin.</summary>
    public IReadOnlyList<double> ProbTrue { get; }

    /// <summary>The mean predicted probability in each non-empty bin, same length as <see cref="ProbTrue"/>.</summary>
    public IReadOnlyList<double> ProbPred { get; }

    /// <summary>Draws the curve — <c>calibration_curve(y_true, y_prob, pos_label=…, n_bins=…, strategy=…)</c>.</summary>
    /// <param name="yTrue">The true labels, one per sample; at most two distinct values.</param>
    /// <param name="yProb">A probability per sample, each within <c>[0, 1]</c>.</param>
    /// <param name="posLabel">The label counted as positive. Explicit here where the reference infers it.</param>
    /// <param name="nBins">How many bins to cut <c>[0, 1]</c> into. The result may be shorter.</param>
    /// <param name="strategy">Where the bin edges come from.</param>
    /// <exception cref="ArgumentException">The inputs disagree in length, are empty, carry a probability outside <c>[0, 1]</c> while none is <c>NaN</c> (numpy's NaN extremes pass the reference's range test, #1464), or name more than two classes.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="nBins"/> is below 1, or leaves no room for its edges in one array (#1538).</exception>
    public static CalibrationCurve Compute(
        ReadOnlySpan<int> yTrue,
        ReadOnlySpan<double> yProb,
        int posLabel = 1,
        int nBins = 5,
        BinStrategy strategy = BinStrategy.Uniform)
    {
        Validate(yTrue, yProb, nBins);

        double[] edges = Edges(yProb, nBins, strategy);
        double[] sums = new double[nBins];
        double[] positives = new double[nBins];
        int[] totals = new int[nBins];

        for (int i = 0; i < yProb.Length; i++)
        {
            // searchsorted over the interior edges, which puts an exact edge in the
            // lower bin and 1.0 in the last.
            int bin = LeftmostAtLeast(edges, yProb[i], nBins);
            sums[bin] += yProb[i];
            positives[bin] += yTrue[i] == posLabel ? 1.0 : 0.0;
            totals[bin]++;
        }

        int kept = totals.Count(total => total != 0);
        var probTrue = new double[kept];
        var probPred = new double[kept];
        int at = 0;
        for (int bin = 0; bin < nBins; bin++)
        {
            if (totals[bin] == 0)
            {
                continue;
            }
            probTrue[at] = positives[bin] / totals[bin];
            probPred[at] = sums[bin] / totals[bin];
            at++;
        }

        return new CalibrationCurve(probTrue, probPred);
    }

    private static void Validate(ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yProb, int nBins)
    {
        // nBins + 1 edges must fit one array; at int.MaxValue the count wrapped inside linspace (#1538).
        if (nBins < 1 || nBins >= TableLength.MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(nBins), nBins, "nBins must be >= 1 and leave room for its edges in one array.");
        }
        if (yTrue.Length != yProb.Length || yTrue.Length == 0)
        {
            throw new ArgumentException(
                $"y_true and y_prob must be the same non-empty length; got {yTrue.Length} and {yProb.Length}.");
        }

        RequireInUnitInterval(yProb);

        int first = yTrue[0];
        int second = first;
        foreach (int label in yTrue)
        {
            if (label != first && label != second)
            {
                if (second != first)
                {
                    throw new ArgumentException(
                        "Only binary classification is supported: y_true names more than two labels.");
                }
                second = label;
            }
        }
    }

    /// <summary>The <c>nBins + 1</c> edges, from the interval or from the data.</summary>
    /// <remarks>
    /// numpy's own grid and percentile, bit for bit: <c>linspace(0, 1, n + 1)</c> for the uniform
    /// edges, and <c>percentile(y_prob, linspace(0, 1, n + 1) * 100)</c> for the quantile ones, so a
    /// probability on an edge falls in the bin scikit-learn puts it in (#1204).
    /// </remarks>
    private static double[] Edges(ReadOnlySpan<double> yProb, int nBins, BinStrategy strategy)
    {
        double[] grid = NumpyGrid.Linspace(0.0, 1.0, nBins + 1);
        if (strategy == BinStrategy.Uniform)
        {
            return grid;
        }

        if (HasNaN(yProb))
        {
            // numpy.percentile propagates a NaN to every edge, so every probability lands in one bin.
            double[] undefined = new double[nBins + 1];
            undefined.AsSpan().Fill(double.NaN);
            return undefined;
        }

        double[] rented = ArrayPool<double>.Shared.Rent(yProb.Length);
        try
        {
            yProb.CopyTo(rented);

            // Array.Sort over the rented range rather than Span<T>.Sort, which
            // netstandard2.0 does not carry.
            Array.Sort(rented, 0, yProb.Length);
            var edges = new double[nBins + 1];
            for (int i = 0; i <= nBins; i++)
            {
                edges[i] = NumpyGrid.Percentile(rented.AsSpan(0, yProb.Length), grid[i] * 100.0);
            }

            return edges;
        }
        finally
        {
            ArrayPool<double>.Shared.Return(rented);
        }
    }

    /// <summary>The reference's <c>y_prob.min() &lt; 0 or y_prob.max() &gt; 1</c>, numpy's NaN-propagating extremes included.</summary>
    /// <remarks>
    /// A NaN turns numpy's minimum and maximum into NaN, so the reference's test passes; the NaN is binned last and
    /// averaged into a NaN mean rather than refused (#1464).
    /// </remarks>
    private static void RequireInUnitInterval(ReadOnlySpan<double> yProb)
    {
        if (HasNaN(yProb))
        {
            return;
        }

        foreach (double probability in yProb)
        {
            if (probability < 0.0 || probability > 1.0)
            {
                throw new ArgumentException("y_prob has values outside [0, 1].");
            }
        }
    }

    private static bool HasNaN(ReadOnlySpan<double> values)
    {
        foreach (double value in values)
        {
            if (double.IsNaN(value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>The bin an interior-edge <c>searchsorted</c> puts a probability in.</summary>
    private static int LeftmostAtLeast(double[] edges, double probability, int nBins)
    {
        // The reference searches edges[1..^1], so the answer is bounded by nBins - 1 and
        // 1.0 lands in the last bin rather than off the end.
        for (int bin = 1; bin < nBins; bin++)
        {
            if (edges[bin] >= probability)
            {
                return bin - 1;
            }
        }

        return nBins - 1;
    }
}
