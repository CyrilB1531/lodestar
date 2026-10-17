using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Shapiro-Wilk test for normality, by Royston's AS R94.</summary>
/// <remarks>
/// Written from Royston's 1995 published description and its polynomial constants (Applied
/// Statistics 44:547-551), not from any implementation of it (ADR 0002). It follows the algorithm's
/// faults: fewer than three values and a range below 1e-19 are refused, and past 5000 values, where
/// the fitted transform is extrapolated, it computes, as AS R94 does with a warning (decision 0010).
/// </remarks>
public static class ShapiroWilk
{
    private const int MinimumSample = 3;

    // AS R94's SMALL: a range below it is the algorithm's sixth fault.
    private const double SmallRange = 1e-19;

    // Above it a square of the values could overflow, so the values are divided by the range.
    private const double ScaleAbove = 1e100;

    // Royston's polynomial coefficients, ascending. The first two correct the
    // last two Blom weights; the rest carry the normalising transform.
    private static readonly double[] WeightCorrectionLast =
        [0.0, 0.221157, -0.147981, -2.071190, 4.434685, -2.706056];

    private static readonly double[] WeightCorrectionSecondLast =
        [0.0, 0.042981, -0.293762, -1.752461, 5.682633, -3.582633];

    private static readonly double[] SmallMu = [0.5440, -0.39978, 0.025054, -6.714e-4];
    private static readonly double[] SmallSigma = [1.3822, -0.77857, 0.062767, -0.0020322];
    private static readonly double[] LargeMu = [-1.5861, -0.31082, -0.083751, 0.0038915];
    private static readonly double[] LargeSigma = [-0.4803, -0.082676, 0.0030302];

    /// <summary>Tests whether a sample could have come from a normal distribution.</summary>
    /// <param name="sample">The sample; at least 3 values, as Royston's AS R94 requires, and past 5000 its fitted transform extrapolated.</param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <returns>Royston's W statistic and its p-value; <c>NaN</c> for both where a NaN or infinite range, an overflowing one included, makes them undefined.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than 3 values, or a range below 1e-19, the two cases AS R94 refuses (ifault 1 and 6) -- checked after
    /// <paramref name="nanPolicy"/> has run, so omission that leaves either still raises (decisions 0007 and 0010). When
    /// <paramref name="nanPolicy"/> is <see cref="NanPolicy.Raise"/> and the sample holds a <c>NaN</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside its enumeration.</exception>
    public static TestResult Test(
        ReadOnlySpan<double> sample,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        ReadOnlySpan<double> values = NanFilter.Apply(sample, nanPolicy, nameof(sample));

        // Royston's AS R94 refuses fewer than three values (ifault 1), and past 5000 only warns (ifault 2) that its fitted
        // transform is extrapolated: it computes, and so does this, where it refused (decision 0010).
        int n = values.Length;
        if (n < MinimumSample)
        {
            throw new ArgumentException(
                $"Royston's algorithm needs at least {MinimumSample} values; got {n}.",
                nameof(sample));
        }

        // Shifted by the value at the middle position, as scipy's shapiro is (gh-15777): a constant sample then has a range of
        // exactly zero, and near-constant ones keep their digits, where [0.1, 0.1, 0.1 + 1.4e-17] read W = 0 (#1706).
        double shift = values[n / 2];
        double[] sorted = new double[n];
        for (int i = 0; i < n; i++)
        {
            sorted[i] = values[i] - shift;
        }
        Array.Sort(sorted);

        // AS R94 refuses a range below 1e-19, its sixth fault, and divides by the range, so no square overflows at 1e200
        // (#1706's Review A); a NaN or infinite range, which it does not cover, answers NaN.
        double range = sorted[n - 1] - sorted[0];
        if (range < SmallRange)
        {
            throw new ArgumentException(
                "The sample's range is below 1e-19, which Royston's algorithm refuses, so there is no shape to test.",
                nameof(sample));
        }
        Options.Require(nanPolicy);
        if (!(range < double.PositiveInfinity))
        {
            return new TestResult(double.NaN, double.NaN);
        }
        // W is scale-free, so the division matters only where a square would overflow; always taken it cost 8% at 500. A range
        // small enough for squares to fade is already refused above.
        if (range > ScaleAbove)
        {
            for (int i = 0; i < n; i++)
            {
                sorted[i] /= range;
            }
        }

        double[] weights = Weights(n);

        double mean = 0.0;
        for (int i = 0; i < n; i++)
        {
            mean += sorted[i];
        }
        mean /= n;

        double numerator = 0.0;
        double denominator = 0.0;
        for (int i = 0; i < n; i++)
        {
            numerator += weights[i] * sorted[i];
            double deviation = sorted[i] - mean;
            denominator += deviation * deviation;
        }

        // Rounding can lift a perfectly linear sample past 1 ([1, 2, 3] gives 1 + 2^-52), where
        // asin and log(1 - w) are undefined; scipy reports W = 1 and p = 1 there (#863).
        double w = Math.Min(1.0, numerator * numerator / denominator);
        // Three values bound W below by 3/4 (Shapiro and Wilk's Corollary 4), which scipy clips to and rounding fell under.
        if (n == 3)
        {
            w = Math.Max(0.75, w);
        }

        return new TestResult(w, PValue(w, n));
    }

    // Royston's weights: Blom scores rescaled to unit length, the largest one (or two,
    // above n = 5) replaced by his polynomial corrections, the rest rescaled to match.
    private static double[] Weights(int n)
    {
        double[] blom = new double[n];
        double sumSquares = 0.0;
        for (int i = 0; i < n; i++)
        {
            // Blom's plotting position through scipy's ndtri, the smallest score first; swilk takes the lower
            // half and mirrors it, so an upper score can part from scipy's in its last bit, within the 1e-9.
            double p = ((i + 1) - 0.375) / (n + 0.25);
            blom[i] = Ndtri.Of(p);
            sumSquares += blom[i] * blom[i];
        }

        double norm = Math.Sqrt(sumSquares);
        double u = 1.0 / Math.Sqrt(n);

        double[] weights = new double[n];
        if (n == 3)
        {
            // AS R94 fixes n = 3 at sqrt(1/2) and a zero middle weight: rescaling the
            // middle Blom score, exactly zero, divided by a zero scale and gave NaN (#863).
            weights[0] = -Math.Sqrt(0.5);
            weights[2] = Math.Sqrt(0.5);
            return weights;
        }

        int corrected = n > 5 ? 2 : 1;

        double top = (blom[n - 1] / norm) + Polynomial(WeightCorrectionLast, u);
        weights[n - 1] = top;
        weights[0] = -top;

        double replacedRaw = blom[n - 1] * blom[n - 1];
        double replacedCorrected = top * top;

        if (corrected == 2)
        {
            double second = (blom[n - 2] / norm) + Polynomial(WeightCorrectionSecondLast, u);
            weights[n - 2] = second;
            weights[1] = -second;

            replacedRaw += blom[n - 2] * blom[n - 2];
            replacedCorrected += second * second;
        }

        // What the corrected weights left for the rest to share -- dividing by its square
        // root keeps the whole vector unit length after two entries were replaced.
        double remaining =
            (sumSquares - (2.0 * replacedRaw)) / (1.0 - (2.0 * replacedCorrected));
        double scale = Math.Sqrt(remaining);

        for (int i = corrected; i < n - corrected; i++)
        {
            weights[i] = blom[i] / scale;
        }

        return weights;
    }

    private static double PValue(double w, int n)
    {
        if (n == 3)
        {
            // Royston's closed form as scipy writes it, 1 - 6/pi acos(sqrt W): 15-digit constants for 6/pi and pi/3, pi/3 being
            // 2.4e-15 high, read [0, 1e-8, 1] 2.8e-7 off and a W of 3/4 as 0 for 3.3e-16 (#1706's Review A).
            double p = 1.0 - (6.0 / Math.PI * Math.Acos(Math.Sqrt(w)));
            return Math.Min(1.0, Math.Max(0.0, p));
        }

        double logN = Math.Log(n);
        double y = Math.Log(1.0 - w);

        double mu;
        double sigma;
        if (n <= 11)
        {
            double gamma = -2.273 + (0.459 * n);
            mu = Polynomial(SmallMu, n);
            sigma = Math.Exp(Polynomial(SmallSigma, n));

            // Below twelve, W is transformed through gamma first; above it, the
            // transform is in log n instead.
            y = -Math.Log(gamma - y);
        }
        else
        {
            mu = Polynomial(LargeMu, logN);
            sigma = Math.Exp(Polynomial(LargeSigma, logN));
        }

        return Normal.Sf((y - mu) / sigma);
    }

    private static double Polynomial(double[] coefficients, double x)
    {
        double result = 0.0;
        for (int i = coefficients.Length - 1; i >= 0; i--)
        {
            result = (result * x) + coefficients[i];
        }

        return result;
    }
}
