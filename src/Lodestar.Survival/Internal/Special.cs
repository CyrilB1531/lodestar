using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>The scalar functions the parametric likelihoods are written in, accurate where a naive form would round away.</summary>
internal static class Special
{
    /// <summary>autograd_gamma clips the regularized incomplete gamma to <c>[1e-35, 1 − 1e-35]</c> before its logarithm.</summary>
    private const double ClipLow = 1e-35;

    public static double LogClipLow { get; } = Math.Log(ClipLow);

    public static double LogClipHigh { get; } = NearZero.Log1P(-ClipLow);

    /// <summary>Where Mills' ratio's asymptotic series takes over, and its terms past the first: 25!!/30^26, the first left
    /// out, is 3e-26; the generalized gamma's deep tail reads the same.</summary>
    internal const double MillsFrom = 30.0;

    /// <summary>The series' terms past its first, from <see cref="MillsFrom"/>.</summary>
    internal const int MillsTerms = 12;

    private static readonly double HalfLogTwoPi = 0.5 * Math.Log(2.0 * Math.PI);

    // Lanczos' coefficients for g = 7, n = 9: about fifteen significant digits over the positive reals.
    private static readonly double[] Lanczos =
    [
        0.99999999999980993, 676.5203681218851, -1259.1392167224028, 771.32342877765313, -176.61502916214059,
        12.507343278686905, -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7,
    ];

    /// <summary><c>1 / (1 + e⁻ˣ)</c> without overflow.</summary>
    public static double Logistic(double x) => x >= 0.0 ? 1.0 / (1.0 + Math.Exp(-x)) : Math.Exp(x) / (1.0 + Math.Exp(x));

    /// <summary><c>log(1 + eˣ)</c>, numpy's <c>logaddexp(x, 0)</c>.</summary>
    public static double LogAddExp0(double x) => Math.Max(x, 0.0) + NearZero.Log1P(Math.Exp(-Math.Abs(x)));

    /// <summary><c>log(1 − e⁻ˣ)</c> for <c>x &gt; 0</c>, Mächler's two branches.</summary>
    public static double Log1mExp(double x) => x <= Math.Log(2.0) ? Math.Log(-NearZero.Expm1(-x)) : NearZero.Log1P(-Math.Exp(-x));

    /// <summary><c>log φ(z)</c>.</summary>
    public static double NormalLogPdf(double z) => (-0.5 * z * z) - HalfLogTwoPi;

    /// <summary><c>log Φ̄(z)</c>: the tail by its asymptotic series where the survival function would underflow.</summary>
    public static double NormalLogSf(double z)
    {
        if (z < 0.0)
        {
            return NearZero.Log1P(-Distributions.NormalSf(-z));
        }

        if (z < MillsFrom)
        {
            return Math.Log(Distributions.NormalSf(z));
        }

        // Φ̄(z) = φ(z)/z · (1 − 1/z² + 3/z⁴ − 15/z⁶ + …); at z ≥ 30 twelve terms leave less than 1e-17.
        return NormalLogPdf(z) - Math.Log(z) + Math.Log(MillsSeries.Sum(1.0 / (z * z), MillsTerms));
    }

    /// <summary><c>log Γ(x)</c> for <c>x &gt; 0</c>, by Lanczos' approximation.</summary>
    public static double LogGamma(double x)
    {
        if (x < 0.5)
        {
            // The reflection formula, for the region the series converges slowly in.
            return Math.Log(Math.PI / Math.Abs(Math.Sin(Math.PI * x))) - LogGamma(1.0 - x);
        }

        double z = x - 1.0;
        double sum = Lanczos[0];
        for (int k = 1; k < Lanczos.Length; k++)
        {
            sum += Lanczos[k] / (z + k);
        }

        double t = z + 7.5;
        return HalfLogTwoPi + ((z + 0.5) * Math.Log(t)) - t + Math.Log(sum);
    }

    /// <summary><c>ψ(x)</c>, the digamma function, by recurrence up to six and its asymptotic series.</summary>
    public static double Digamma(double x)
    {
        double result = 0.0;
        while (x < 6.0)
        {
            result -= 1.0 / x;
            x += 1.0;
        }

        double inverse = 1.0 / (x * x);
        double series = inverse * ((1.0 / 12.0) - (inverse * ((1.0 / 120.0) - (inverse * ((1.0 / 252.0)
            - (inverse * ((1.0 / 240.0) - (inverse * (1.0 / 132.0)))))))));
        return result + Math.Log(x) - (0.5 / x) - series;
    }

    /// <summary><c>log Q(a, x)</c> or <c>log P(a, x)</c> through the published chi-squared tails, clipped as lifelines clips them.</summary>
    public static double LogIncompleteGamma(double a, double x, bool upper)
    {
        double value = upper ? Distributions.ChiSquaredSf(2.0 * x, 2.0 * a) : Distributions.ChiSquaredCdf(2.0 * x, 2.0 * a);
        return Math.Log(Math.Min(Math.Max(value, ClipLow), 1.0 - ClipLow));
    }

    /// <summary>The derivative of <see cref="LogIncompleteGamma"/> in <c>x</c>, given its value: the gamma density over the tail.</summary>
    public static double LogIncompleteGammaDx(double a, double x, double logValue, bool upper)
    {
        double density = Math.Exp(((a - 1.0) * Math.Log(x)) - x - LogGamma(a) - logValue);
        return upper ? -density : density;
    }
}
