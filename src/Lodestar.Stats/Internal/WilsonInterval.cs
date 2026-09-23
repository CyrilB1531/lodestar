namespace Lodestar.Stats.Internal;

/// <summary>Wilson's score interval for a proportion, plain and continuity-corrected.</summary>
/// <remarks>
/// Newcombe (1998), which is the form scipy implements. Closed form, unlike the Clopper-Pearson
/// bounds beside it: Wilson inverts the normal approximation to the binomial rather than the
/// binomial itself, which is what makes it narrower and what makes it occasionally cover less
/// than the level it was asked for.
/// </remarks>
internal static class WilsonInterval
{
    internal static (double Low, double High) Bounds(
        BinomialResult result, double level, bool corrected)
    {
        int k = result.Successes;
        double n = result.Trials;
        double p = k / n;
        double q = 1.0 - p;

        double z = Distributions.NormalQuantile(
            result.Alternative == Alternative.TwoSided ? 0.5 + (0.5 * level) : level);
        double denominator = 2.0 * (n + (z * z));
        double centre = ((2.0 * n * p) + (z * z)) / denominator;

        double lowHalf;
        double highHalf;
        if (corrected)
        {
            lowHalf = (1.0 + (z * Math.Sqrt((z * z) - 2.0 - (1.0 / n) + (4.0 * p * ((n * q) + 1.0)))))
                / denominator;
            highHalf = (1.0 + (z * Math.Sqrt((z * z) + 2.0 - (1.0 / n) + (4.0 * p * ((n * q) - 1.0)))))
                / denominator;
        }
        else
        {
            lowHalf = z / denominator * Math.Sqrt((4.0 * n * p * q) + (z * z));
            highHalf = lowHalf;
        }

        double low = k == 0 || result.Alternative == Alternative.Less ? 0.0 : centre - lowHalf;
        double high = k == result.Trials || result.Alternative == Alternative.Greater
            ? 1.0
            : centre + highHalf;

        return (low, high);
    }
}
