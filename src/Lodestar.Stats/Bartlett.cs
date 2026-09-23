using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>Bartlett's test: do several groups share one variance, assuming each is normal?</summary>
/// <remarks>
/// The same question <see cref="Levene"/> asks, under the stronger assumption — which buys power
/// when it holds and costs correctness when it does not. Bartlett's statistic is sensitive to
/// departures from normality in a way that looks like a difference in variance, so on data whose
/// shape is unknown <see cref="Levene"/> is the safer reading and the reason both ship.
/// </remarks>
public static class Bartlett
{
    /// <summary>Compares the variances of two or more groups.</summary>
    /// <param name="groups">The groups; at least two, each holding at least two values.</param>
    /// <returns>The T statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">Fewer than two groups, or a group holding one value or none.</exception>
    // S2368: groups mirrors scipy.stats.bartlett's own one-array-per-group shape.
#pragma warning disable S2368
    public static TestResult Test(params double[][] groups)
#pragma warning restore S2368
        => Test(NanPolicy.Propagate, groups);

    /// <summary>The same test, with a policy for the <c>NaN</c> values in the groups.</summary>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="groups">Two or more groups of observations.</param>
    /// <returns>The T statistic and its p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than two groups, a group holding fewer than two values, or a <c>NaN</c> under
    /// <see cref="NanPolicy.Raise"/>.
    /// </exception>
#pragma warning disable S2368
    public static TestResult Test(NanPolicy nanPolicy, params double[][] groups)
#pragma warning restore S2368
    {
        Guard.NotNull(groups);
        double[][] samples = nanPolicy == NanPolicy.Propagate
            ? groups
            : NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups));

        if (samples.Length < 2)
        {
            throw new ArgumentException(
                $"Bartlett's test needs at least two groups; got {samples.Length}.", nameof(groups));
        }

        int k = samples.Length;
        int total = 0;
        double[] variances = new double[k];
        for (int g = 0; g < k; g++)
        {
            // A group of one has no variance at ddof = 1, and the logarithm below would read it.
            if (samples[g] is not { Length: > 1 })
            {
                throw new ArgumentException(
                    $"Group {g} holds fewer than two values, so it has no variance.", nameof(groups));
            }

            variances[g] = Variance(samples[g]);
            total += samples[g].Length;
        }

        return Statistic(samples, variances, total);
    }

    private static TestResult Statistic(double[][] samples, double[] variances, int total)
    {
        int k = samples.Length;
        double pooled = 0.0;
        double logSum = 0.0;
        double reciprocals = 0.0;
        for (int g = 0; g < k; g++)
        {
            double freedom = samples[g].Length - 1.0;
            pooled += freedom * variances[g];
            logSum += freedom * Math.Log(variances[g]);
            reciprocals += 1.0 / freedom;
        }

        double residualDf = total - k;
        pooled /= residualDf;

        double numerator = (residualDf * Math.Log(pooled)) - logSum;
        double denominator = 1.0 + ((reciprocals - (1.0 / residualDf)) / (3.0 * (k - 1.0)));
        double t = numerator / denominator;
        if (double.IsNaN(t))
        {
            // A NaN carries through the variance and the logarithm; the chi-squared tail
            // refuses one rather than passing it through, so it is not asked.
            return new TestResult(double.NaN, double.NaN);
        }

        double pValue = Distributions.ChiSquaredSf(t, k - 1.0);

        // The tail is read off the statistic as computed, and only then is it floored: scipy
        // reports the floor with the p-value its unfloored value produced.
        return new TestResult(Math.Max(0.0, t), pValue);
    }

    private static double Variance(double[] values)
    {
        double sum = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        double mean = sum / values.Length;
        double squares = 0.0;
        for (int i = 0; i < values.Length; i++)
        {
            double deviation = values[i] - mean;
            squares += deviation * deviation;
        }

        return squares / (values.Length - 1.0);
    }
}
