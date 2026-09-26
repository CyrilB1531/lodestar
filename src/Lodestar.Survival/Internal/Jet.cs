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
        double expm1 = Special.Expm1(u.Value);
        return Apply(u, Special.Log1mExp(u.Value), 1.0 / expm1, -Math.Exp(u.Value) / (expm1 * expm1));
    }

    /// <summary><c>log(1 + u)</c>.</summary>
    public static Jet Log1p(Jet u) => Apply(u, Special.Log1p(u.Value), 1.0 / (1.0 + u.Value), -1.0 / ((1.0 + u.Value) * (1.0 + u.Value)));

    /// <summary><c>eᵘ − 1</c>.</summary>
    public static Jet Expm1(Jet u)
    {
        double e = Math.Exp(u.Value);
        return Apply(u, Special.Expm1(u.Value), e, e);
    }

    /// <summary><c>log Φ̄(u)</c>, scipy's <c>norm.logsf</c>.</summary>
    public static Jet NormalLogSf(Jet u)
    {
        double logSf = Special.NormalLogSf(u.Value);
        double ratio = Math.Exp(Special.NormalLogPdf(u.Value) - logSf);
        return Apply(u, logSf, -ratio, -ratio * (ratio - u.Value));
    }

    /// <summary><c>log Φ(u)</c>, scipy's <c>norm.logcdf</c>.</summary>
    public static Jet NormalLogCdf(Jet u) => NormalLogSf(-u);

    /// <summary><c>log φ(u)</c>, scipy's <c>norm.logpdf</c>.</summary>
    public static Jet NormalLogPdf(Jet u) => Apply(u, Special.NormalLogPdf(u.Value), -u.Value, -1.0);

    /// <summary><c>log Γ(u)</c>.</summary>
    public static Jet LogGamma(Jet u) => Apply(u, Special.LogGamma(u.Value), Special.Digamma(u.Value), Special.Trigamma(u.Value));

    /// <summary>
    /// <c>log Q(a, x)</c> or <c>log P(a, x)</c>, the regularized incomplete gamma, clipped to <c>[1e-35, 1 − 1e-35]</c>
    /// as lifelines' <c>autograd_gamma</c> clips it.
    /// </summary>
    /// <remarks>
    /// The derivatives in <c>x</c> are exact; those in <c>a</c> are central differences with Richardson's
    /// extrapolation, where lifelines takes a single five-point difference: no closed form of them is needed to
    /// reach the optimum, only one accurate enough that the gradient vanishes where the likelihood peaks.
    /// </remarks>
    public static Jet LogIncompleteGamma(Jet a, Jet x, bool upper)
    {
        double av = a.Value;
        double xv = x.Value;
        double value = Special.LogIncompleteGamma(av, xv, upper);
        double dx = Special.LogIncompleteGammaDx(av, xv, value, upper);
        double dxx = dx * (((av - 1.0) / xv) - 1.0 - dx);
        double h = 1e-3 * Math.Max(1.0, Math.Abs(av));
        double da = Richardson(s => Special.LogIncompleteGamma(av + s, xv, upper), h, out double daa);
        double dax = Richardson(
            s => Special.LogIncompleteGammaDx(av + s, xv, Special.LogIncompleteGamma(av + s, xv, upper), upper), h, out _);
        bool clipped = value <= Special.LogClipLow || value >= Special.LogClipHigh;
        return clipped
            ? Apply(a, x, value, (0.0, 0.0), (0.0, 0.0, 0.0))
            : Apply(a, x, value, (da, dx), (daa, dax, dxx));
    }

    /// <summary>The first and second derivative at zero by central differences at <c>h</c> and <c>h/2</c>, extrapolated.</summary>
    private static double Richardson(Func<double, double> f, double h, out double second)
    {
        double f0 = f(0.0);
        double fp = f(h);
        double fm = f(-h);
        double fp2 = f(h / 2.0);
        double fm2 = f(-h / 2.0);
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
