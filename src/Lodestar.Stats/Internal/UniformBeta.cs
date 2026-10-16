namespace Lodestar.Stats.Internal;

/// <summary>I_x(a, b) for both shapes large, by Temme's uniform expansion in the error function.</summary>
/// <remarks>
/// With s = a + b, p = a/s, q = b/s, r = √(pq) and δ = x − p, the change of variable −η²/2 = p log(x/p) + q log(y/q),
/// η taking δ's sign, turns the integral into one of e^(−sη²/2) f(η), f = η/δ. Integrating by parts twice,
/// I_x(a, b) = ½ erfc(−η √(s/2)) − K e^(−sη²/2) (h₀/s + h₁/s²) + O(s⁻³), K = p^a q^b / B(a, b), h₀ = 1/δ − 1/(rη) and
/// h₁ = (h₀′ − h₀′(0))/η (Temme, Special Functions, 1996, 11.3.3; DLMF 8.18.ii). The fraction needs about √a terms at
/// the mean: past a shape of 1e10 its million stopped short, and F's tail at (1, 1e18, 1e18) read 0.70 for 0.5; from
/// 1e5 it drifted to 6e-11 of a 60-digit fraction, as at shapes (1e7, 1e10), where this keeps 2e-13 (#1690's review).
/// </remarks>
internal static class UniformBeta
{
    // The term dropped is of order 1/min(a, b)^2: measured against a 60-digit fraction, within 2e-13 from here, and
    // 5e-9 at shapes of 1e3, where the fraction keeps 7e-14.
    internal const double MinimumShape = 1e5;

    // Below this fraction of the smaller of p and q, h0 and h1 come from their series in delta, whose radius is that
    // smaller one: 12 terms leave 0.05^12, and the direct forms, which cancel there, lose under two digits past it.
    private const double SeriesReach = 0.05;
    private const int SeriesTerms = 12;

    // e^-750 is below the smallest double, so past it the tail is the limit itself.
    private const double FarExponent = 750.0;

    /// <summary>Whether both shapes are large enough for the expansion.</summary>
    internal static bool Holds(double a, double b) => a >= MinimumShape && b >= MinimumShape;

    /// <summary>I_x(a, b), given y = 1 − x as the caller formed it, and x's distance from the exact mean, or NaN to take it from x.</summary>
    internal static double Regularized(double a, double b, double x, double y, double deviation)
    {
        double s = a + b;
        double p = a / s;
        double q = b / s;
        double r = Math.Sqrt(p * q);

        // delta and s eta^2 / 2 from Beta.ShapeExponent, one side for both, so exchanging the shapes and points only
        // negates delta: a delta of zero beside a nonzero exponent read both tails as one at shapes (2e108, 5e106).
        double exponent = Math.Max(-Beta.ShapeExponent(a, b, x, y, deviation, out double delta), 0.0);
        // Past FarExponent both terms underflow, and 2 exponent / s overflowed past 9e307 into NaN: F's tail at
        // (7.6e-132, 1.9e306, 5e232) read NaN for 1 (#1690's review).
        if (exponent > FarExponent)
        {
            return delta < 0.0 ? 0.0 : 1.0;
        }
        // A NaN y, which RegularizedIncomplete does not validate, answers NaN as the fraction did: Math.Sign throws on it.
        if (double.IsNaN(delta) || double.IsNaN(exponent))
        {
            return double.NaN;
        }
        double eta = Math.Sign(delta) * Math.Sqrt(2.0 * exponent / s);

        (double h0, double h1) = Corrections(p, q, r, delta, eta, x, y);
        // a b / s as p q s, since a b overflows past shapes of 1e154; h0 and h1 come scaled by min(p, q) and its square.
        double k = Math.Sqrt(p * q * s / (2.0 * Math.PI))
            * Math.Exp(Gamma.LogStirlingCorrection(s) - Gamma.LogStirlingCorrection(a) - Gamma.LogStirlingCorrection(b) - exponent);
        double small = Math.Min(a, b);
        double remainder = k * ((h0 / small) + (h1 / small / small));

        // erfc(|eta| sqrt(s/2)) from its square, s eta^2 / 2, so the exponent is not rounded twice.
        double tail = 0.5 * Normal.ErfcOfSquareRoot(exponent);
        return delta < 0.0 ? tail - remainder : 1.0 - tail - remainder;
    }

    /// <summary>m h₀ and m² h₁ at η, m = min(p, q): by their series in t = δ/m near the mean, directly away from it.</summary>
    /// <remarks>
    /// h₀ grows as 1/m and h₁ as 1/m², and the series in δ carried 1/q^15: at q = 1e-24 it overflowed and the tail read
    /// NaN at shapes (4e92, 8e68) (#1690's review). In t every coefficient is of order one.
    /// </remarks>
    private static (double H0, double H1) Corrections(double p, double q, double r, double delta, double eta, double x, double y)
    {
        double m = Math.Min(p, q);
        double t = delta / m;
        bool nearMean = Math.Abs(t) <= SeriesReach;

        // Away from the mean only g_0 is read, which takes u to its second term: the rest was 200 of 225 multiply-adds.
        int terms = nearMean ? SeriesTerms : 0;
        Span<double> u = stackalloc double[terms + 3];
        Span<double> g = stackalloc double[terms + 1];
        Series(p, q, r, m, u, g);

        if (nearMean)
        {
            // m h0 = -sqrt(m) (u1 + u2 t + ...); m^2 h1 = (g1 + g2 t + ...) / S, S = eta / delta, 1/r at the mean.
            double h0 = 0.0;
            for (int k = SeriesTerms; k >= 1; k--)
            {
                h0 = (h0 * t) - u[k];
            }
            double h1Numerator = 0.0;
            for (int k = SeriesTerms; k >= 1; k--)
            {
                h1Numerator = (h1Numerator * t) + g[k];
            }
            // S = eta / delta tends to 1/r at the mean, its value too where the exponent rounds to zero beside a nonzero delta.
            double etaOverDelta = Math.Abs(eta) > 0.0 ? eta / delta : 1.0 / r;
            return (Math.Sqrt(m) * h0, h1Numerator / etaOverDelta);
        }

        // m^2 times h0's slope, -eta x y / delta^3 + 1 / (r eta^2), each factor of order one.
        double ratio = m / eta;
        double slope = (-eta * (x * y / m) / (t * t * t)) + (ratio * ratio / r);
        return ((1.0 / t) - (ratio / r), (slope - (m * g[0])) / eta);
    }

    /// <summary>The series in t = δ/m of 1/(r √m S) into <paramref name="u"/>, and of m h₀′ into <paramref name="g"/>.</summary>
    /// <remarks>
    /// η² = Σ z_k δ^k from k = 2, z_k = −2 φ_k, φ_k = ((−1)^(k+1)/p^(k−1) − 1/q^(k−1))/k, the Taylor coefficients of
    /// p log(1 + δ/p) + q log(1 − δ/q) less their linear terms; in t, m η²/δ² has coefficients z_(j+2) m^(j+1), with
    /// powers of m/p and m/q at most one. S = η/δ, U = 1/(r S) = √m u(t), so m h₀ = (m/δ)(1 − U) = −√m Σ u_(k+1) t^k;
    /// h₀′ = (dh₀/dδ)/(dη/dδ) = (1/m) Σ g_k t^k.
    /// </remarks>
    private static void Series(double p, double q, double r, double m, Span<double> u, Span<double> g)
    {
        int n = u.Length;
        Span<double> w = stackalloc double[n];
        Span<double> root = stackalloc double[n];
        double towardP = 1.0;
        double towardQ = 1.0;
        for (int j = 0; j < n; j++)
        {
            // w_j = z_(j+2) m^(j+1), from phi_(j+2) = ((-1)^(j+3)/p^(j+1) - 1/q^(j+1)) / (j + 2).
            towardP *= m / p;
            towardQ *= m / q;
            double sign = j % 2 == 0 ? -1.0 : 1.0;
            w[j] = -2.0 * ((sign * towardP) - towardQ) / (j + 2);
        }

        root[0] = Math.Sqrt(w[0]);
        for (int k = 1; k < n; k++)
        {
            double cross = 0.0;
            for (int i = 1; i < k; i++)
            {
                cross += root[i] * root[k - i];
            }
            root[k] = (w[k] - cross) / (2.0 * root[0]);
        }

        // 1 / (r root(t)), by the reciprocal's recurrence; root(t) = sqrt(m) S.
        u[0] = 1.0 / (r * root[0]);
        for (int k = 1; k < n; k++)
        {
            double sum = 0.0;
            for (int i = 1; i <= k; i++)
            {
                sum += root[i] * u[k - i];
            }
            u[k] = -sum / root[0];
        }

        // m h0' = -(sum (k + 1) u_(k+2) t^k) / (sum (k + 1) root_k t^k), divided term by term.
        int terms = g.Length;
        for (int k = 0; k < terms; k++)
        {
            double numerator = -(k + 1) * u[k + 2];
            double sum = 0.0;
            for (int i = 1; i <= k; i++)
            {
                sum += (i + 1) * root[i] * g[k - i];
            }
            g[k] = (numerator - sum) / root[0];
        }
    }
}
