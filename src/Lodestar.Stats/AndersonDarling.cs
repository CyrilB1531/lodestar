using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Anderson-Darling test: could this sample be normal?</summary>
/// <remarks>
/// The other normality test beside <see cref="ShapiroWilk"/>, and the one that keeps working
/// where Shapiro-Wilk's own reference stops: Royston's approximation is fitted over
/// <c>3 ≤ n ≤ 5000</c>, where this statistic has no upper bound at all. It weighs the tails more
/// heavily than the centre, which is usually where a departure from normality matters and is the
/// reason the two disagree on the same sample often enough to be worth running both.
/// </remarks>
public static class AndersonDarling
{
    /// <summary>Stephens' critical values for the normal case, at 15%, 10%, 5%, 2.5% and 1%.</summary>
    private static readonly double[] StephensNormal = [0.561, 0.631, 0.752, 0.873, 1.035];

    /// <summary>The significance levels those critical values belong to, in percent, as scipy reports them.</summary>
    private static readonly double[] Levels = [15.0, 10.0, 5.0, 2.5, 1.0];

    /// <summary>Tests a sample against the normal distribution, fitting its mean and spread.</summary>
    /// <param name="x">The sample; at least two values, since the spread is estimated from it.</param>
    /// <returns>
    /// The A² statistic, the p-value interpolated from the table, and the table itself — the
    /// critical values for this sample size, and the significance levels they belong to.
    /// </returns>
    /// <exception cref="ArgumentException">Fewer than two values, or a constant sample.</exception>
    /// <remarks>
    /// The distribution is fitted rather than given, which is what makes the critical values
    /// depend on the sample size: they are Stephens' constants divided by
    /// <c>1 + 0.75/n + 2.25/n²</c> and rounded to three decimals, exactly as scipy scales them.
    /// </remarks>
    public static AndersonResult Test(ReadOnlySpan<double> x)
    {
        if (x.Length < 2)
        {
            throw new ArgumentException(
                $"Anderson-Darling needs at least two values; got {x.Length}.", nameof(x));
        }

        int n = x.Length;
        double[] sorted = x.ToArray();
        Array.Sort(sorted);

        double mean = Mean(sorted);
        double deviation = StandardDeviation(sorted, mean);

        // S1244: a constant sample has an exactly zero spread, and standardising it would divide
        // by that zero rather than by something merely small.
#pragma warning disable S1244
        if (deviation == 0.0)
#pragma warning restore S1244
        {
            throw new ArgumentException(
                "Every value in the sample is the same, so it has no spread to standardise by.",
                nameof(x));
        }

        double squared = Squared(sorted, mean, deviation, n);
        double[] critical = Critical(n);

        return new AndersonResult(squared, Interpolated(squared, critical), critical, (double[])Levels.Clone());
    }

    /// <summary>A², the weighted distance between the sample's own steps and the fitted normal.</summary>
    private static double Squared(double[] sorted, double mean, double deviation, int n)
    {
        double sum = 0.0;
        for (int i = 0; i < n; i++)
        {
            double low = (sorted[i] - mean) / deviation;
            double high = (sorted[n - 1 - i] - mean) / deviation;

            // log Φ(low) + log(1 - Φ(high)), both through the tail so neither underflows: an
            // outlier ten deviations out would otherwise take the whole statistic to infinity.
            sum += ((2.0 * (i + 1)) - 1.0) / n * (Normal.LogSf(-low) + Normal.LogSf(high));
        }

        return -n - sum;
    }

    /// <summary>Stephens' constants, scaled by the sample size and rounded as scipy rounds them.</summary>
    private static double[] Critical(int n)
    {
        double scale = 1.0 + (0.75 / n) + (2.25 / ((double)n * n));
        double[] critical = new double[StephensNormal.Length];
        for (int i = 0; i < critical.Length; i++)
        {
            critical[i] = Math.Round(StephensNormal[i] / scale, 3, MidpointRounding.ToEven);
        }

        return critical;
    }

    /// <summary>The p-value read off the table, clamped at both ends as <c>numpy.interp</c> clamps it.</summary>
    /// <remarks>
    /// The table runs from 15% down to 1%, so this cannot report a p-value outside
    /// <c>[0.01, 0.15]</c> whatever the statistic. That is the table's resolution, not a bug:
    /// <see cref="ShapiroWilk.Test"/> is the member to reach for when the number matters beyond
    /// "reject or not", and <see cref="AndersonResult.CriticalValues"/> is what this family
    /// actually offers.
    /// </remarks>
    private static double Interpolated(double squared, double[] critical)
    {
        int last = critical.Length - 1;
        if (squared <= critical[0])
        {
            return Levels[0] / 100.0;
        }
        if (squared >= critical[last])
        {
            return Levels[last] / 100.0;
        }

        for (int i = 1; i <= last; i++)
        {
            if (squared <= critical[i])
            {
                double span = critical[i] - critical[i - 1];
                double weight = (squared - critical[i - 1]) / span;
                return (Levels[i - 1] + (weight * (Levels[i] - Levels[i - 1]))) / 100.0;
            }
        }

        return Levels[last] / 100.0;
    }

    private static double Mean(double[] values)
    {
        double sum = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum / values.Length;
    }

    private static double StandardDeviation(double[] values, double mean)
    {
        double squares = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            double gap = values[i] - mean;
            squares += gap * gap;
        }

        return Math.Sqrt(squares / (values.Length - 1.0));
    }
}
