namespace Lodestar.Preprocessing.Internal;

/// <summary>The two power families' transforms and inverses, scipy's, and the arithmetic their likelihoods share.</summary>
internal static class PowerLikelihood
{
    /// <summary>Yeo and Johnson's transform at one exponent, as <c>scipy.stats</c>'s <c>_yeojohnson_transform</c> computes it.</summary>
    /// <remarks>
    /// Four branches, because the family is defined piecewise about zero and degenerates at
    /// <c>lambda = 0</c> on the non-negative side and at <c>lambda = 2</c> on the other — within machine
    /// epsilon of either, as scipy tests it. The power is <c>expm1(λ · log1p(x)) / λ</c>, scipy's stable
    /// form, not <c>((x + 1)^λ − 1) / λ</c>, which at <c>λ = 1</c> maps <c>1e10</c> seven ulps from scipy's (#1227).
    /// </remarks>
    internal static double YeoJohnson(double value, double lambda)
    {
        if (value >= 0.0)
        {
            return Math.Abs(lambda) < NearZero.MachineEpsilon
                ? NearZero.Log1P(value)
                : NearZero.Expm1(lambda * NearZero.Log1P(value)) / lambda;
        }

        return Math.Abs(lambda - 2.0) > NearZero.MachineEpsilon
            ? -NearZero.Expm1((2.0 - lambda) * NearZero.Log1P(-value)) / (2.0 - lambda)
            : -NearZero.Log1P(-value);
    }

    /// <summary>Box and Cox's transform at one exponent, <c>scipy.special.boxcox</c>; the value must be positive.</summary>
    /// <remarks>Past <c>λ · ln x = 709.78</c>, where <c>expm1</c> overflows, scipy divides inside the exponential instead.</remarks>
    internal static double BoxCox(double value, double lambda)
    {
        if (Math.Abs(lambda) < 1e-19)
        {
            return Math.Log(value);
        }

        double scaled = lambda * Math.Log(value);
        return scaled < 709.78
            ? NearZero.Expm1(scaled) / lambda
            : (Math.Sign(lambda) * Math.Exp(scaled - Math.Log(Math.Abs(lambda)))) - (1.0 / lambda);
    }

    /// <summary><c>scipy.special.inv_boxcox</c>: <c>exp(log1p(λ · y) / λ)</c>, <c>exp(y)</c> at exactly zero.</summary>
    /// <remarks>Past <c>λ · y = 1.79e308</c>, where the product overflows, scipy adds the two logarithms instead.</remarks>
    internal static double InverseBoxCox(double value, double lambda)
    {
        // S1244: scipy's inv_boxcox tests the exponent against exactly zero.
#pragma warning disable S1244
        if (lambda == 0.0)
#pragma warning restore S1244
        {
            return Math.Exp(value);
        }

        return lambda * value < 1.79e308
            ? Math.Exp(NearZero.Log1P(lambda * value) / lambda)
            : Math.Exp((Math.Log(Math.Sign(lambda) * value) + Math.Log(Math.Abs(lambda))) / lambda);
    }

    /// <summary>scikit-learn's <c>_yeo_johnson_inverse_transform</c>, each degenerate exponent tested within machine epsilon.</summary>
    internal static double InverseYeoJohnson(double value, double lambda)
    {
        if (value >= 0.0)
        {
            return Math.Abs(lambda) < NearZero.MachineEpsilon
                ? Math.Exp(value) - 1.0
                : Math.Pow((value * lambda) + 1.0, 1.0 / lambda) - 1.0;
        }

        return Math.Abs(lambda - 2.0) > NearZero.MachineEpsilon
            ? 1.0 - Math.Pow((-(2.0 - lambda) * value) + 1.0, 1.0 / (2.0 - lambda))
            : 1.0 - Math.Exp(-value);
    }

    /// <summary>
    /// The log of the variance of <c>exp(λ · logs)</c>: taken directly, one exponential a value, where the powers are
    /// finite and their variance holds more than 1e-8 of their mean square, so cancellation costs it nothing scipy's
    /// tolerance sees; in logarithms otherwise, as scipy's <c>_log_var</c> takes it always, at three exponentials and a
    /// logarithm a value — 1.45 to 1.9 times main's fit, where the direct form is main's cost (#1227).
    /// </summary>
    /// <param name="logs">The logs, read only.</param>
    /// <param name="lambda">The exponent.</param>
    /// <param name="scratch">Scratch of the same length, overwritten.</param>
    /// <param name="work">Scratch of the same length, overwritten.</param>
    internal static double ScaledLogVariance(ReadOnlySpan<double> logs, double lambda, Span<double> scratch, Span<double> work)
    {
        for (int i = 0; i < logs.Length; i++)
        {
            scratch[i] = Math.Exp(lambda * logs[i]);
        }

        double mean = NumpyReduction.Sum(scratch) / logs.Length;
        for (int i = 0; i < logs.Length; i++)
        {
            double deviation = scratch[i] - mean;
            work[i] = deviation * deviation;
        }

        double variance = NumpyReduction.Sum(work) / logs.Length;
        if (variance > 1e-8 * mean * mean && !double.IsInfinity(variance) && !double.IsInfinity(mean * mean))
        {
            return Math.Log(variance);
        }

        for (int i = 0; i < logs.Length; i++)
        {
            scratch[i] = lambda * logs[i];
        }

        return LogVariance(scratch, work);
    }

    /// <summary>The log of the variance of <c>exp(values)</c>, from the logs alone: scipy's <c>_log_var</c>.</summary>
    /// <param name="values">The logs; overwritten, as the second sum's scratch.</param>
    /// <param name="work">Scratch of the same length.</param>
    internal static double LogVariance(Span<double> values, Span<double> work)
    {
        double logCount = Math.Log(values.Length);
        double logMean = LogSumExp(values, work) - logCount;
        for (int i = 0; i < values.Length; i++)
        {
            work[i] = 2.0 * LogDifference(values[i], logMean);
        }

        return LogSumExp(work, values) - logCount;
    }

    /// <summary>
    /// <c>log|eᵃ − eᵇ|</c> as scipy's signed two-term <c>logsumexp</c> reaches it: the larger term set apart, the
    /// other's <c>−e^(min − max)</c> through <c>log1p</c>, then the direct form: <c>−∞</c> for two equal terms
    /// <c>exp</c> holds, NaN past its overflow or for two infinities, as scipy's own fallback reads them.
    /// </summary>
    private static double LogDifference(double a, double b)
    {
        // Where the two differ by less than exp can resolve, log1p reads -∞; scipy then falls back to the direct form.
        double top = Math.Max(a, b);
        double result = NearZero.Log1P(-Math.Exp(Math.Min(a, b) - top)) + top;
        return double.IsInfinity(result) || double.IsNaN(result) ? Math.Log(Math.Abs(Math.Exp(a) - Math.Exp(b))) : result;
    }

    /// <summary>
    /// <c>scipy.special.logsumexp</c>: the largest terms set apart and counted, the rest shifted, summed pairwise
    /// and added through <c>log1p</c>; the direct <c>log(Σ eˣ)</c> where that is not finite.
    /// </summary>
    /// <param name="values">The terms, read only.</param>
    /// <param name="scratch">Scratch of the same length.</param>
    private static double LogSumExp(ReadOnlySpan<double> values, Span<double> scratch)
    {
        double top = double.NegativeInfinity;
        for (int i = 0; i < values.Length; i++)
        {
            top = Math.Max(top, values[i]);
        }

        int ties = 0;
        for (int i = 0; i < values.Length; i++)
        {
            // S1244: scipy sets apart the elements equal to the maximum, exactly.
#pragma warning disable S1244
            bool isTop = values[i] == top;
#pragma warning restore S1244
            ties += isTop ? 1 : 0;
            scratch[i] = isTop ? 0.0 : Math.Exp(values[i] - top);
        }

        double rest = NumpyReduction.Sum(scratch);
        // S1244: as scipy's where(s == 0, s, s / m).
#pragma warning disable S1244
        double scaled = rest == 0.0 ? rest : rest / ties;
#pragma warning restore S1244
        double result = NearZero.Log1P(scaled) + Math.Log(ties) + top;
        if (!double.IsInfinity(result) && !double.IsNaN(result))
        {
            return result;
        }

        for (int i = 0; i < values.Length; i++)
        {
            scratch[i] = Math.Exp(values[i]);
        }

        return Math.Log(NumpyReduction.Sum(scratch));
    }

    /// <summary>The bounds the reference searches Yeo-Johnson's exponent over.</summary>
    /// <remarks>
    /// Derived from the data so the transform cannot overflow inside the search: the widest
    /// value expected is twenty times the largest observed, and the exponent is bounded so that
    /// raising it stays inside half the exponent range of a double. The reference computes them
    /// the same way, and reflects them about one when every value is negative.
    /// </remarks>
    internal static (double Low, double High) YeoJohnsonBounds(ReadOnlySpan<double> column)
    {
        double largest = 0.0;
        bool anyNegative = false;
        bool allNegative = true;
        for (int i = 0; i < column.Length; i++)
        {
            largest = Math.Max(largest, Math.Abs(column[i]));
            anyNegative |= column[i] < 0.0;
            allNegative &= column[i] < 0.0;
        }

        double spread = NearZero.Log1P(20.0 * largest);
        double logEpsilon = Math.Log(NearZero.MachineEpsilon);
        double logTiny = (Math.Log(NearZero.SmallestNormal) - logEpsilon) / 2.0;
        double logLargest = (Math.Log(double.MaxValue) + logEpsilon) / 2.0;

        double low = logTiny / spread;
        double high = logLargest / spread;
        if (allNegative)
        {
            return (2.0 - high, 2.0 - low);
        }

        return anyNegative ? (Math.Max(2.0 - high, low), Math.Min(2.0 - low, high)) : (low, high);
    }
}
