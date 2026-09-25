using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

public static partial class AndersonDarling
{
    /// <summary>The significance levels scipy's <c>anderson_ksamp</c> tabulates, in percent, descending.</summary>
    private static readonly double[] KSampleLevels = [25.0, 10.0, 5.0, 2.5, 1.0, 0.5, 0.1];

    /// <summary>Scholz and Stephens' Table 2 coefficients: the critical value is <c>b0 + b1/√m + b2/m</c>, <c>m = k − 1</c>.</summary>
    private static readonly double[] B0 = [0.675, 1.281, 1.645, 1.96, 2.326, 2.573, 3.085];

    private static readonly double[] B1 = [-0.245, 0.25, 0.678, 1.149, 1.822, 2.364, 3.615];

    private static readonly double[] B2 = [-0.105, -0.305, -0.362, -0.391, -0.396, -0.345, -0.154];

    /// <summary>Tests whether several samples come from one distribution, by the mid-rank statistic.</summary>
    /// <param name="samples">The samples; at least two, none empty.</param>
    /// <returns>As <see cref="KSample(AndersonKSampleVariant, double[][])"/>.</returns>
    /// <exception cref="ArgumentException">As <see cref="KSample(AndersonKSampleVariant, double[][])"/>.</exception>
    // S2368: samples mirrors scipy's list of one array per sample, as Levene.Test does.
#pragma warning disable S2368
    public static AndersonResult KSample(params double[][] samples)
#pragma warning restore S2368
        => KSample(AndersonKSampleVariant.Midrank, samples);

    /// <summary>Tests whether several samples come from one distribution — scipy's <c>anderson_ksamp</c>.</summary>
    /// <param name="variant">Which form of the statistic; scipy's <c>variant</c>.</param>
    /// <param name="samples">The samples; at least two, none empty, with at least two distinct values between them.</param>
    /// <returns>
    /// The normalised statistic, the p-value interpolated from Scholz and Stephens' table and clamped to
    /// <c>[0.001, 0.25]</c> as scipy clamps it, and the critical values at 25 %, 10 %, 5 %, 2.5 %, 1 %, 0.5 % and 0.1 %.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Fewer than two samples, an empty one, a <c>NaN</c>, or fewer than two distinct values, where scipy raises.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="variant"/> is not a declared value.</exception>
    /// <remarks>
    /// scipy fits a quadratic in the critical values to the log of the levels and reads it at the statistic; outside
    /// the table it returns the table's end with a warning, which here is simply the clamped value.
    /// </remarks>
#pragma warning disable S2368
    public static AndersonResult KSample(AndersonKSampleVariant variant, params double[][] samples)
#pragma warning restore S2368
    {
        Guard.NotNull(samples);
        if (variant is < AndersonKSampleVariant.Midrank or > AndersonKSampleVariant.Continuous)
        {
            throw new ArgumentOutOfRangeException(nameof(variant), variant, "Not a declared k-sample variant.");
        }

        CheckSamples(samples);
        int k = samples.Length;
        double[] pooled = Sorted(samples.SelectMany(sample => sample).ToArray());
        (double[] distinct, int[] tied) = Runs(pooled);
        if (distinct.Length < 2)
        {
            throw new ArgumentException("The k-sample test needs more than one distinct observation.", nameof(samples));
        }

        int total = pooled.Length;
        double[][] sorted = [.. samples.Select(sample => Sorted((double[])sample.Clone()))];
        double raw = variant switch
        {
            AndersonKSampleVariant.Midrank => KSampleMidrank(sorted, total, distinct, tied),
            AndersonKSampleVariant.Right => KSampleRight(sorted, total, distinct, tied),
            _ => KSampleContinuous(sorted, pooled),
        };

        double m = k - 1.0;
        double statistic = (raw - m) / Math.Sqrt(KSampleVariance(samples, total, k));
        var critical = new double[B0.Length];
        for (int i = 0; i < critical.Length; i++)
        {
            critical[i] = B0[i] + (B1[i] / Math.Sqrt(m)) + (B2[i] / m);
        }

        return new AndersonResult(statistic, KSamplePValue(statistic, critical), critical, [.. KSampleLevels]);
    }

    private static void CheckSamples(double[][] samples)
    {
        if (samples.Length < 2)
        {
            throw new ArgumentException($"The k-sample test needs at least two samples; got {samples.Length}.", nameof(samples));
        }

        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i] is not { Length: > 0 })
            {
                throw new ArgumentException($"Sample {i} has no observations.", nameof(samples));
            }

            if (Ranks.HasNaN(samples[i]))
            {
                throw new ArgumentException($"Sample {i} holds a NaN, which has no place in the pooled order.", nameof(samples));
            }
        }
    }

    /// <summary>Scholz and Stephens' equation 7: mid-ranks, for samples with ties.</summary>
    /// <remarks>Each sample is walked once beside the pooled distinct values, where scipy binary-searches each of them.</remarks>
    private static double KSampleMidrank(double[][] sorted, int total, double[] distinct, int[] tied)
    {
        bool untied = distinct.Length == total;
        double sum = 0.0;
        foreach (double[] sample in sorted)
        {
            double inner = 0.0;
            int below = 0;
            int lower = 0;
            int upper = 0;
            for (int j = 0; j < distinct.Length; j++)
            {
                double z = distinct[j];
                lower = Below(sample, lower, z);
                upper = AtMost(sample, Math.Max(upper, lower), z);
                double count = untied ? 1.0 : tied[j];
                double midrank = below + (count / 2.0);
                double within = upper - ((upper - lower) / 2.0);
                double gap = (total * within) - (midrank * sample.Length);
                inner += count / total * gap * gap / ((midrank * (total - midrank)) - (total * count / 4.0));
                below += tied[j];
            }

            sum += inner / sample.Length;
        }

        return sum * (total - 1.0) / total;
    }

    /// <summary>Their equation 6: the right-continuous empirical distribution, for discrete samples.</summary>
    private static double KSampleRight(double[][] sorted, int total, double[] distinct, int[] tied)
    {
        double sum = 0.0;
        foreach (double[] sample in sorted)
        {
            double inner = 0.0;
            double cumulative = 0.0;
            int upper = 0;
            for (int j = 0; j < distinct.Length - 1; j++)
            {
                double z = distinct[j];
                upper = AtMost(sample, upper, z);
                cumulative += tied[j];
                double gap = (total * (double)upper) - (cumulative * sample.Length);
                inner += tied[j] / (double)total * gap * gap / (cumulative * (total - cumulative));
            }

            sum += inner / sample.Length;
        }

        return sum;
    }

    /// <summary>Their equation 3, which assumes no ties.</summary>
    private static double KSampleContinuous(double[][] sorted, double[] pooled)
    {
        int total = pooled.Length;
        double sum = 0.0;
        foreach (double[] sample in sorted)
        {
            double inner = 0.0;
            int upper = 0;
            for (int j = 1; j < total; j++)
            {
                double z = pooled[j - 1];
                upper = AtMost(sample, upper, z);
                double gap = (total * (double)upper) - ((double)j * sample.Length);
                inner += gap * gap / ((double)j * (total - j));
            }

            sum += inner / sample.Length;
        }

        return sum / total;
    }

    /// <summary>The first index at or after <paramref name="from"/> holding a value not below <paramref name="z"/>: a merge step.</summary>
    private static int Below(double[] sorted, int from, double z)
    {
        int at = from;
        while (at < sorted.Length && sorted[at] < z)
        {
            at++;
        }

        return at;
    }

    /// <summary>The first index at or after <paramref name="from"/> holding a value above <paramref name="z"/>.</summary>
    private static int AtMost(double[] sorted, int from, double z)
    {
        int at = from;
        while (at < sorted.Length && sorted[at] <= z)
        {
            at++;
        }

        return at;
    }

    /// <summary>The distinct values of a sorted array, and how many times each occurs.</summary>
    private static (double[] Distinct, int[] Counts) Runs(double[] sorted)
    {
        var values = new List<double>();
        var counts = new List<int>();
        for (int i = 0; i < sorted.Length; i++)
        {
            // S1244: a run is literally equal values, as numpy.unique finds them.
#pragma warning disable S1244
            if (i > 0 && sorted[i] == sorted[i - 1])
#pragma warning restore S1244
            {
                counts[counts.Count - 1]++;
            }
            else
            {
                values.Add(sorted[i]);
                counts.Add(1);
            }
        }

        return ([.. values], [.. counts]);
    }

    private static double[] Sorted(double[] values)
    {
        if (RadixSort.Worthwhile(values))
        {
            RadixSort.Sort(values);
        }
        else
        {
            Array.Sort(values);
        }

        return values;
    }

    /// <summary>The statistic's variance under the null, Scholz and Stephens' σ²_N.</summary>
    private static double KSampleVariance(double[][] samples, int total, int k)
    {
        double harmonic = samples.Sum(sample => 1.0 / sample.Length);
        double h = 1.0;
        double g = 0.0;
        double cumulative = 0.0;
        for (int i = total - 1; i > 1; i--)
        {
            cumulative += 1.0 / i;
            g += cumulative / (total + 1.0 - i);
        }

        h += cumulative;
        double n = total;
        double a = (((4 * g) - 6) * (k - 1)) + ((10 - (6 * g)) * harmonic);
        double b = (((2 * g) - 4) * k * k) + (8 * h * k) + ((((2 * g) - (14 * h)) - 4) * harmonic) - (8 * h) + (4 * g) - 6;
        double c = ((((6 * h) + (2 * g)) - 2) * k * k) + ((((4 * h) - (4 * g)) + 6) * k) + (((2 * h) - 6) * harmonic) + (4 * h);
        double d = (((2 * h) + 6) * k * k) - (4 * h * k);
        return ((a * n * n * n) + (b * n * n) + (c * n) + d) / ((n - 1.0) * (n - 2.0) * (n - 3.0));
    }

    /// <summary>scipy's reading of the table: a least-squares quadratic of <c>log(level)</c> in the critical values, clamped at its ends.</summary>
    private static double KSamplePValue(double statistic, double[] critical)
    {
        if (statistic < critical.Min())
        {
            return 0.25;
        }

        if (statistic > critical.Max())
        {
            return 0.001;
        }

        double[] logs = [.. KSampleLevels.Select(level => Math.Log(level / 100.0))];
        double[] coefficients = QuadraticFit(critical, logs);
        return Math.Exp((((coefficients[0] * statistic) + coefficients[1]) * statistic) + coefficients[2]);
    }

    /// <summary><c>numpy.polyfit(x, y, 2)</c>: highest power first, least squares on columns scaled to unit norm, as it scales them.</summary>
    private static double[] QuadraticFit(double[] x, double[] y)
    {
        double[][] columns = [[.. x.Select(v => v * v)], [.. x], [.. x.Select(_ => 1.0)]];
        double[] scale = [.. columns.Select(column => Math.Sqrt(column.Sum(v => v * v)))];
        for (int j = 0; j < columns.Length; j++)
        {
            columns[j] = [.. columns[j].Select(v => v / scale[j])];
        }

        // The 3 × 3 normal equations, solved by Cramer's rule.
        double[][] gram = [.. columns.Select(left => columns.Select(right => Dot(left, right)).ToArray())];
        double[] moment = [.. columns.Select(column => Dot(column, y))];
        double determinant = Determinant(gram);
        var solution = new double[columns.Length];
        for (int j = 0; j < columns.Length; j++)
        {
            double[][] replaced = [.. gram.Select((row, r) => row.Select((value, c) => c == j ? moment[r] : value).ToArray())];
            solution[j] = Determinant(replaced) / determinant / scale[j];
        }

        return solution;
    }

    private static double Dot(double[] left, double[] right) => left.Select((value, i) => value * right[i]).Sum();

    private static double Determinant(double[][] m) =>
        (m[0][0] * ((m[1][1] * m[2][2]) - (m[1][2] * m[2][1])))
        - (m[0][1] * ((m[1][0] * m[2][2]) - (m[1][2] * m[2][0])))
        + (m[0][2] * ((m[1][0] * m[2][1]) - (m[1][1] * m[2][0])));
}
