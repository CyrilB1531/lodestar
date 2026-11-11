namespace Lodestar.Survival.Internal;

/// <summary>A value with its gradient and Hessian over a handful of variables: second-order forward differentiation.</summary>
/// <remarks>
/// lifelines differentiates its likelihoods with autograd; these carry the same derivatives exactly, so a Newton loop
/// over them reaches the optimum autograd's gradient vanishes at, and the inverse Hessian there is the covariance.
/// Every operation applies the chain rule to both orders: <c>∇f(u) = f′ ∇u</c> and <c>∇²f(u) = f″ ∇u ∇uᵀ + f′ ∇²u</c>.
/// </remarks>
internal sealed class Jet
{
    private Jet(double value, double[] gradient, double[] hessian)
    {
        Value = value;
        Gradient = gradient;
        Hessian = hessian;
    }

    public double Value { get; }

    public double[] Gradient { get; }

    /// <summary>Row-major, symmetric.</summary>
    public double[] Hessian { get; }

    public int Size => Gradient.Length;

    /// <summary>A constant: no gradient, no curvature.</summary>
    public static Jet Constant(double value, int size) => new(value, new double[size], new double[size * size]);

    /// <summary>A linear combination of the variables: <paramref name="value"/> with <paramref name="gradient"/> and no curvature.</summary>
    public static Jet Linear(double value, double[] gradient) => new(value, gradient, new double[gradient.Length * gradient.Length]);

    /// <summary>A jet from its parts, as a caller that applied the chain rule itself assembles one.</summary>
    public static Jet FromParts(double value, double[] gradient, double[] hessian) => new(value, gradient, hessian);

    /// <summary>Variable <paramref name="index"/> of <paramref name="size"/>, at <paramref name="value"/>.</summary>
    public static Jet Variable(double value, int index, int size)
    {
        var gradient = new double[size];
        gradient[index] = 1.0;
        return new Jet(value, gradient, new double[size * size]);
    }

    public static Jet operator +(Jet u, Jet v) => Combine(u, v, 1.0, 1.0, u.Value + v.Value);

    public static Jet operator -(Jet u, Jet v) => Combine(u, v, 1.0, -1.0, u.Value - v.Value);

    public static Jet operator +(Jet u, double c) => new(u.Value + c, u.Gradient, u.Hessian);

    public static Jet operator +(double c, Jet u) => u + c;

    public static Jet operator -(Jet u, double c) => new(u.Value - c, u.Gradient, u.Hessian);

    public static Jet operator -(double c, Jet u) => (-u) + c;

    public static Jet operator -(Jet u) => Scale(u, -1.0, -u.Value);

    public static Jet operator *(Jet u, double c) => Scale(u, c, u.Value * c);

    public static Jet operator *(double c, Jet u) => u * c;

    public static Jet operator /(Jet u, double c) => Scale(u, 1.0 / c, u.Value / c);

    public static Jet operator *(Jet u, Jet v)
    {
        int n = u.Size;
        var gradient = new double[n];
        var hessian = new double[n * n];
        for (int a = 0; a < n; a++)
        {
            gradient[a] = (u.Value * v.Gradient[a]) + (v.Value * u.Gradient[a]);
            for (int b = 0; b < n; b++)
            {
                hessian[(a * n) + b] = (u.Value * v.Hessian[(a * n) + b]) + (v.Value * u.Hessian[(a * n) + b])
                    + (u.Gradient[a] * v.Gradient[b]) + (v.Gradient[a] * u.Gradient[b]);
            }
        }

        return new Jet(u.Value * v.Value, gradient, hessian);
    }

    public static Jet operator /(Jet u, Jet v) => u * Reciprocal(v);

    public static Jet operator /(double c, Jet v) => Reciprocal(v) * c;

    /// <summary><c>f(u)</c> from <c>f(u)</c>, <c>f′(u)</c> and <c>f″(u)</c>.</summary>
    public static Jet Apply(Jet u, double value, double first, double second)
    {
        int n = u.Size;
        var gradient = new double[n];
        var hessian = new double[n * n];
        for (int a = 0; a < n; a++)
        {
            gradient[a] = first * u.Gradient[a];
            for (int b = 0; b < n; b++)
            {
                hessian[(a * n) + b] = (second * u.Gradient[a] * u.Gradient[b]) + (first * u.Hessian[(a * n) + b]);
            }
        }

        return new Jet(value, gradient, hessian);
    }

    /// <summary><c>f(u, v)</c> from its value and its partial derivatives to second order.</summary>
    public static Jet Apply(Jet u, Jet v, double value, (double U, double V) first, (double UU, double UV, double VV) second)
    {
        int n = u.Size;
        var gradient = new double[n];
        var hessian = new double[n * n];
        for (int a = 0; a < n; a++)
        {
            gradient[a] = (first.U * u.Gradient[a]) + (first.V * v.Gradient[a]);
            for (int b = 0; b < n; b++)
            {
                hessian[(a * n) + b] = (second.UU * u.Gradient[a] * u.Gradient[b])
                    + (second.UV * ((u.Gradient[a] * v.Gradient[b]) + (v.Gradient[a] * u.Gradient[b])))
                    + (second.VV * v.Gradient[a] * v.Gradient[b])
                    + (first.U * u.Hessian[(a * n) + b]) + (first.V * v.Hessian[(a * n) + b]);
            }
        }

        return new Jet(value, gradient, hessian);
    }

    public static Jet Exp(Jet u)
    {
        double e = Math.Exp(u.Value);
        return Apply(u, e, e, e);
    }

    public static Jet Log(Jet u) => Apply(u, Math.Log(u.Value), 1.0 / u.Value, -1.0 / (u.Value * u.Value));

    public static Jet Reciprocal(Jet u) =>
        Apply(u, 1.0 / u.Value, -1.0 / (u.Value * u.Value), 2.0 / (u.Value * u.Value * u.Value));

    /// <summary><c>log(1 + eᵘ)</c>, numpy's <c>logaddexp(u, 0)</c>.</summary>
    public static Jet LogAddExp0(Jet u)
    {
        double sigma = Special.Logistic(u.Value);
        return Apply(u, Special.LogAddExp0(u.Value), sigma, sigma * (1.0 - sigma));
    }

    /// <summary><c>log(1 − e⁻ᵘ)</c> for <c>u &gt; 0</c>: the log of a probability from a cumulative hazard.</summary>
    public static Jet Log1mExp(Jet u)
    {
        double expm1 = NearZero.Expm1(u.Value);
        // −e^u/(e^u − 1)² as −1/((e^u − 1)(1 − e^−u)): past u = 709.78 both of the first form's terms overflowed to a NaN,
        // and one left-censored subject far out took the whole Hessian with it (#1756).
        return Apply(u, Special.Log1mExp(u.Value), 1.0 / expm1, -1.0 / (expm1 * -NearZero.Expm1(-u.Value)));
    }

    /// <summary><c>log(1 + u)</c>.</summary>
    public static Jet Log1p(Jet u) => Apply(u, NearZero.Log1P(u.Value), 1.0 / (1.0 + u.Value), -1.0 / ((1.0 + u.Value) * (1.0 + u.Value)));

    /// <summary><c>eᵘ − 1</c>.</summary>
    public static Jet Expm1(Jet u)
    {
        double e = Math.Exp(u.Value);
        return Apply(u, NearZero.Expm1(u.Value), e, e);
    }

    /// <summary><c>log Φ̄(u)</c>, scipy's <c>norm.logsf</c>.</summary>
    public static Jet NormalLogSf(Jet u)
    {
        (double logSf, double hazard) = LogSfAndHazard(u.Value);
        return LogSfJet(u, logSf, hazard);
    }

    /// <summary>log Φ̄(u) and the normal hazard φ(u)/Φ̄(u), each special function evaluated once.</summary>
    private static (double LogSf, double Hazard) LogSfAndHazard(double u)
    {
        double logSf = Special.NormalLogSf(u);
        return (logSf, Math.Exp(Special.NormalLogPdf(u) - logSf));
    }

    /// <summary>log Φ̄ as a jet: its derivative is minus the hazard h, its second −h(h − u).</summary>
    private static Jet LogSfJet(Jet u, double logSf, double hazard) => Apply(u, logSf, -hazard, -hazard * (hazard - u.Value));

    /// <summary><c>log Φ̄(u)</c> and the normal hazard <c>h = φ(u)/Φ̄(u)</c>, from one evaluation of each: h′ = h(h − u),
    /// h″ = h′(h − u) + h(h′ − 1).</summary>
    public static (Jet LogSf, Jet Hazard) NormalLogSfAndHazard(Jet u)
    {
        (double logSf, double hazard) = LogSfAndHazard(u.Value);
        double first = hazard * (hazard - u.Value);
        return (LogSfJet(u, logSf, hazard), Apply(u, hazard, first, (first * (hazard - u.Value)) + (hazard * (first - 1.0))));
    }

    /// <summary><c>log Φ(u)</c>, scipy's <c>norm.logcdf</c>.</summary>
    public static Jet NormalLogCdf(Jet u) => NormalLogSf(-u);

    /// <summary><c>log φ(u)</c>, scipy's <c>norm.logpdf</c>.</summary>
    public static Jet NormalLogPdf(Jet u) => Apply(u, Special.NormalLogPdf(u.Value), -u.Value, -1.0);

    /// <summary><c>log Γ(u)</c>.</summary>
    public static Jet LogGamma(Jet u) => Apply(u, Special.LogGamma(u.Value), Special.Digamma(u.Value), Trigamma.Of(u.Value));

    /// <summary>
    /// <c>log Q(a, x)</c> or <c>log P(a, x)</c>, the regularized incomplete gamma, in <c>a</c> and <c>u = log x</c>, clipped
    /// to <c>[1e-35, 1 − 1e-35]</c> as lifelines' <c>autograd_gamma</c> clips it.
    /// </summary>
    /// <remarks>
    /// In <c>u</c>, not <c>x</c>: the second derivative in x, of order <c>x^(a−2)</c>, overflowed at x = 1e-169 and a = 0.01
    /// before the chain rule's x² took it back, and the Hessian read NaN (#1222). Those in <c>u</c> are exact; those in
    /// <c>a</c> central differences with Richardson's extrapolation, where lifelines takes one five-point difference.
    /// </remarks>
    public static Jet LogIncompleteGamma(Jet a, Jet logX, bool upper)
    {
        double av = a.Value;
        double uv = logX.Value;
        double xv = Math.Exp(uv);
        double value = Special.LogIncompleteGamma(av, xv, upper);
        double du = Special.LogIncompleteGammaDLogX(av, uv, value, upper);
        double duu = du * (av - xv - du);
        // Relative to a: log Q goes as log a below one, so 1e-3 absolute biased d/da by 2e-4 at a = 0.007 (#1222).
        double h = 1e-3 * av;
        // At a ± h and a ± h/2, each tail evaluated once for both differences.
        double tailUp = Special.LogIncompleteGamma(av + h, xv, upper);
        double tailDown = Special.LogIncompleteGamma(av - h, xv, upper);
        double tailHalfUp = Special.LogIncompleteGamma(av + (h / 2.0), xv, upper);
        double tailHalfDown = Special.LogIncompleteGamma(av - (h / 2.0), xv, upper);
        double da = Richardson(value, (tailUp, tailDown, tailHalfUp, tailHalfDown), h, out _);
        // The second at 3h: the tails' rounding over h² held H_λλ to 9e-6 at λ = 0.25; at |λ| = 0.35, 2e-6 at 3h and
        // 7e-6 at 2h, whose stencil would share a ± h.
        double wide = 3.0 * h;
        Richardson(
            value,
            (Special.LogIncompleteGamma(av + wide, xv, upper), Special.LogIncompleteGamma(av - wide, xv, upper),
             Special.LogIncompleteGamma(av + (wide / 2.0), xv, upper), Special.LogIncompleteGamma(av - (wide / 2.0), xv, upper)),
            wide,
            out double daa);
        double dau = Richardson(
            du,
            (Special.LogIncompleteGammaDLogX(av + h, uv, tailUp, upper), Special.LogIncompleteGammaDLogX(av - h, uv, tailDown, upper),
             Special.LogIncompleteGammaDLogX(av + (h / 2.0), uv, tailHalfUp, upper),
             Special.LogIncompleteGammaDLogX(av - (h / 2.0), uv, tailHalfDown, upper)),
            h,
            out _);
        return Special.Clips(value, out _)
            ? Apply(a, logX, value, (0.0, 0.0), (0.0, 0.0, 0.0))
            : Apply(a, logX, value, (da, du), (daa, dau, duu));
    }

    /// <summary>The first and second derivative at zero by central differences at <c>h</c> and <c>h/2</c>, extrapolated,
    /// from <c>f(0)</c> and <c>f</c> at <c>h, −h, h/2, −h/2</c>.</summary>
    private static double Richardson(
        double f0, (double Up, double Down, double HalfUp, double HalfDown) at, double h, out double second)
    {
        (double fp, double fm, double fp2, double fm2) = at;
        double firstCoarse = (fp - fm) / (2.0 * h);
        double firstFine = (fp2 - fm2) / h;
        double secondCoarse = (fp - (2.0 * f0) + fm) / (h * h);
        double secondFine = (fp2 - (2.0 * f0) + fm2) / (h * h / 4.0);
        second = ((4.0 * secondFine) - secondCoarse) / 3.0;
        return ((4.0 * firstFine) - firstCoarse) / 3.0;
    }

    private static Jet Combine(Jet u, Jet v, double cu, double cv, double value)
    {
        int n = u.Size;
        var gradient = new double[n];
        var hessian = new double[n * n];
        for (int a = 0; a < n; a++)
        {
            gradient[a] = (cu * u.Gradient[a]) + (cv * v.Gradient[a]);
        }

        for (int k = 0; k < hessian.Length; k++)
        {
            hessian[k] = (cu * u.Hessian[k]) + (cv * v.Hessian[k]);
        }

        return new Jet(value, gradient, hessian);
    }

    private static Jet Scale(Jet u, double c, double value)
    {
        var gradient = new double[u.Size];
        var hessian = new double[u.Hessian.Length];
        for (int a = 0; a < gradient.Length; a++)
        {
            gradient[a] = c * u.Gradient[a];
        }

        for (int k = 0; k < hessian.Length; k++)
        {
            hessian[k] = c * u.Hessian[k];
        }

        return new Jet(value, gradient, hessian);
    }
}
