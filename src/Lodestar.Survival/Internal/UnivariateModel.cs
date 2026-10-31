using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>One of lifelines' parametric univariate models: its parameters, its hazards and where lifelines starts it.</summary>
/// <remarks>
/// Each model is written as lifelines writes it — the same parameters, the same clips on the time, the same
/// <c>log(1 − S)</c> where lifelines overrides it — so the likelihood is lifelines' to the rounding.
/// </remarks>
internal abstract class UnivariateModel
{
    /// <summary>lifelines' <c>_fitted_parameter_names</c>.</summary>
    public abstract string[] Names { get; }

    /// <summary>lifelines' <c>_compare_to_values</c>: what each z statistic is measured from.</summary>
    public abstract double[] CompareTo { get; }

    /// <summary>The lower bound of each parameter, <see cref="double.NegativeInfinity"/> where it has none; lifelines' <c>_bounds</c>.</summary>
    public abstract double[] LowerBounds { get; }

    public static UnivariateModel For(ParametricModel model, double[] breakpoints) => model switch
    {
        ParametricModel.Exponential => new Exponential(),
        ParametricModel.Weibull => new Weibull(),
        ParametricModel.LogNormal => new LogNormal(),
        ParametricModel.LogLogistic => new LogLogistic(),
        ParametricModel.PiecewiseExponential => new Piecewise(breakpoints),
        _ => new GeneralizedGamma(),
    };

    /// <summary>A Newton step's largest move, relative to each parameter's size or one; unbounded but where a model sets it.</summary>
    public virtual double MaximumReach => double.PositiveInfinity;

    /// <summary>A start inside the bounds; lifelines' <c>_create_initial_point</c> where it has one.</summary>
    public abstract double[] Start(double[] times);

    public abstract Jet CumulativeHazard(Jet[] p, double t);

    public abstract Jet LogHazard(Jet[] p, double t);

    /// <summary><c>log h(t) − H(t)</c>, the log density; a model whose two share a tail overrides it, so the tail is not
    /// computed only to cancel.</summary>
    public virtual Jet LogDensity(Jet[] p, double t) => LogHazard(p, t) - CumulativeHazard(p, t);

    /// <summary><c>log(1 − S(t))</c>; lifelines' default is <c>log1p(−S)</c>, the same value as this.</summary>
    public virtual Jet LogOneMinusSurvival(Jet[] p, double t) => Jet.Log1mExp(CumulativeHazard(p, t));

    /// <summary>The time by which the survival falls to <paramref name="probability"/>.</summary>
    public abstract double Percentile(double[] p, double probability);

    /// <summary>The cumulative hazard at the fitted parameters.</summary>
    public double CumulativeHazard(double[] p, double t) => CumulativeHazard(Constants(p), t).Value;

    /// <summary>The hazard at the fitted parameters, <c>exp(log h)</c>.</summary>
    public double Hazard(double[] p, double t) => Math.Exp(LogHazard(Constants(p), t).Value);

    /// <summary>The gradient of the cumulative hazard in the parameters, for the delta-method bounds.</summary>
    public double[] CumulativeHazardGradient(double[] p, double t)
    {
        Jet[] variables = Variables(p);
        return CumulativeHazard(variables, t).Gradient;
    }

    public static Jet[] Variables(double[] p)
    {
        var jets = new Jet[p.Length];
        for (int i = 0; i < p.Length; i++)
        {
            jets[i] = Jet.Variable(p[i], i, p.Length);
        }

        return jets;
    }

    private static Jet[] Constants(double[] p)
    {
        var jets = new Jet[p.Length];
        for (int i = 0; i < p.Length; i++)
        {
            jets[i] = Jet.Constant(p[i], p.Length);
        }

        return jets;
    }

    /// <summary><c>log(max(t, 1e-25))</c>, lifelines' clip before a logarithm of time.</summary>
    protected static double ClippedLog(double t) => Math.Log(Math.Max(t, 1e-25));

    private sealed class Exponential : UnivariateModel
    {
        public override string[] Names => ["lambda_"];

        public override double[] CompareTo => [0.0];

        public override double[] LowerBounds => [0.0];

        public override double[] Start(double[] times) => [1.0];

        public override Jet CumulativeHazard(Jet[] p, double t) => t / p[0];

        public override Jet LogHazard(Jet[] p, double t) => -Jet.Log(p[0]);

        public override double Percentile(double[] p, double probability) => -p[0] * Math.Log(probability);
    }

    private sealed class Weibull : UnivariateModel
    {
        public override string[] Names => ["lambda_", "rho_"];

        public override double[] CompareTo => [1.0, 1.0];

        public override double[] LowerBounds => [0.0, 0.0];

        public override double[] Start(double[] times) => [times.Average(), 1.0];

        public override Jet CumulativeHazard(Jet[] p, double t) => Jet.Exp(p[1] * (ClippedLog(t) - Jet.Log(p[0])));

        public override Jet LogHazard(Jet[] p, double t) =>
            Jet.Log(p[1]) - Jet.Log(p[0]) + ((p[1] - 1.0) * (Math.Log(t) - Jet.Log(p[0])));

        public override double Percentile(double[] p, double probability) => p[0] * Math.Pow(-Math.Log(probability), 1.0 / p[1]);
    }

    private sealed class LogNormal : UnivariateModel
    {
        public override string[] Names => ["mu_", "sigma_"];

        public override double[] CompareTo => [0.0, 1.0];

        public override double[] LowerBounds => [double.NegativeInfinity, 0.0];

        public override double[] Start(double[] times)
        {
            double[] sorted = [.. times.Select(t => Math.Log(t)).OrderBy(v => v)];
            int n = sorted.Length;
            return [n % 2 == 1 ? sorted[n / 2] : (sorted[(n / 2) - 1] + sorted[n / 2]) / 2.0, 1.0];
        }

        public override Jet CumulativeHazard(Jet[] p, double t) => -Jet.NormalLogSf((Math.Log(t) - p[0]) / p[1]);

        public override Jet LogHazard(Jet[] p, double t)
        {
            Jet z = (Math.Log(t) - p[0]) / p[1];
            return Jet.NormalLogPdf(z) - Jet.Log(p[1]) - Math.Log(t) - Jet.NormalLogSf(z);
        }

        public override Jet LogDensity(Jet[] p, double t) =>
            Jet.NormalLogPdf((Math.Log(t) - p[0]) / p[1]) - Jet.Log(p[1]) - Math.Log(t);

        public override Jet LogOneMinusSurvival(Jet[] p, double t) => Jet.NormalLogCdf((Math.Log(t) - p[0]) / p[1]);

        // The upper normal quantile of p itself: 1 − p kept only log10(1/p) digits, and read +∞ at p = 1e-20.
        public override double Percentile(double[] p, double probability) =>
            Math.Exp(p[0] + (p[1] * Distributions.NormalIsf(probability)));
    }

    private sealed class LogLogistic : UnivariateModel
    {
        public override string[] Names => ["alpha_", "beta_"];

        public override double[] CompareTo => [1.0, 1.0];

        public override double[] LowerBounds => [0.0, 0.0];

        public override double[] Start(double[] times) => [times.Average(), 1.0];

        public override Jet CumulativeHazard(Jet[] p, double t) => Jet.LogAddExp0(p[1] * (ClippedLog(t) - Jet.Log(p[0])));

        public override Jet LogHazard(Jet[] p, double t)
        {
            Jet scaled = Math.Log(t) - Jet.Log(p[0]);
            return Jet.Log(p[1]) - Jet.Log(p[0]) + ((p[1] - 1.0) * scaled) - Jet.LogAddExp0(p[1] * scaled);
        }

        public override Jet LogOneMinusSurvival(Jet[] p, double t) => -Jet.LogAddExp0(-p[1] * (Math.Log(t) - Jet.Log(p[0])));

        public override double Percentile(double[] p, double probability) => p[0] * Math.Pow(probability / (1.0 - probability), -1.0 / p[1]);
    }

    /// <summary>A constant hazard between consecutive breakpoints, one rate per piece.</summary>
    private sealed class Piecewise(double[] breakpoints) : UnivariateModel
    {
        public override string[] Names => [.. Enumerable.Range(0, breakpoints.Length + 1).Select(i => $"lambda_{i}_")];

        // lifelines leaves _compare_to_values unset here, so it is the initial point, the bound plus one.
        public override double[] CompareTo => Start([]);

        public override double[] LowerBounds => [.. Enumerable.Repeat(1e-9, breakpoints.Length + 1)];

        public override double[] Start(double[] times) => [.. Enumerable.Repeat(1e-9 + 1.0, breakpoints.Length + 1)];

        public override Jet CumulativeHazard(Jet[] p, double t)
        {
            Jet total = Jet.Constant(0.0, p.Length);
            double previous = 0.0;
            for (int j = 0; j <= breakpoints.Length; j++)
            {
                double edge = j < breakpoints.Length ? Math.Min(breakpoints[j], t) : t;
                total += (edge - previous) / p[j];
                previous = edge;
            }

            return total;
        }

        public override Jet LogHazard(Jet[] p, double t)
        {
            int piece = Piece(t);

            // lifelines differentiates min(breakpoint, t), and autograd splits that tie in half (#1309).
            // S1244: a time on a breakpoint is the tie itself, not a neighbourhood of it.
#pragma warning disable S1244
            return piece < breakpoints.Length && breakpoints[piece] == t
#pragma warning restore S1244
                ? Jet.Log((0.5 / p[piece]) + (0.5 / p[piece + 1]))
                : -Jet.Log(p[piece]);
        }

        public override double Percentile(double[] p, double probability)
        {
            double target = -Math.Log(probability);
            double accumulated = 0.0;
            double previous = 0.0;
            for (int j = 0; j <= breakpoints.Length; j++)
            {
                double edge = j < breakpoints.Length ? breakpoints[j] : double.PositiveInfinity;
                double piece = (edge - previous) / p[j];
                if (accumulated + piece >= target)
                {
                    return previous + ((target - accumulated) * p[j]);
                }

                accumulated += piece;
                previous = edge;
            }

            return double.PositiveInfinity;
        }

        /// <summary>The piece a time falls in: the number of breakpoints strictly below it.</summary>
        private int Piece(double t) => breakpoints.Count(b => b < t);
    }

    /// <summary>The generalized gamma, in lifelines' <c>(μ, log σ, λ)</c>, with its three branches in the sign of <c>λ</c>.</summary>
    private sealed class GeneralizedGamma : UnivariateModel
    {
        public override string[] Names => ["mu_", "ln_sigma_", "lambda_"];

        // Unbounded, the first step went from μ = 4.7 to −232, where the clipped variate lets the likelihood reach 1e61, and
        // a fit with one maximum threw; three times reached it, and 4 of #1222's 400 samples with it.
        public override double MaximumReach => 3.0;

        public override double[] CompareTo => [0.0, 0.0, 1.0];

        public override double[] LowerBounds => [double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity];

        public override double[] Start(double[] times)
        {
            double[] logs = [.. times.Select(t => Math.Log(t))];
            double mean = logs.Average();
            double deviation = Math.Sqrt(logs.Sum(v => (v - mean) * (v - mean)) / logs.Length);
            return [mean * 1.5, Math.Log(deviation + 0.1), 1.0];
        }

        public override Jet CumulativeHazard(Jet[] p, double t)
        {
            Jet z = (Math.Log(t) - p[0]) / Jet.Exp(p[1]);
            if (GeneralizedGammaUniform.Standardize(z, p[2]) is { } zeta)
            {
                return -GeneralizedGammaUniform.LogSurvival(zeta, p[2]);
            }

            (Jet inverseSquare, _, Jet logX) = GammaVariate(p, z);
            return -Jet.LogIncompleteGamma(inverseSquare, logX, p[2].Value > 0.0);
        }

        public override Jet LogHazard(Jet[] p, double t)
        {
            Jet z = (Math.Log(t) - p[0]) / Jet.Exp(p[1]);
            if (GeneralizedGammaUniform.Standardize(z, p[2]) is { } zeta)
            {
                return GeneralizedGammaUniform.LogDensity(zeta, p[2]) - p[1] - Math.Log(t)
                    - GeneralizedGammaUniform.LogSurvival(zeta, p[2]);
            }

            (Jet density, Jet inverseSquare, Jet logX) = GammaLogDensity(p, z, t);
            return density - Jet.LogIncompleteGamma(inverseSquare, logX, p[2].Value > 0.0);
        }

        public override Jet LogDensity(Jet[] p, double t)
        {
            Jet z = (Math.Log(t) - p[0]) / Jet.Exp(p[1]);
            // The survival in log h and in H cancels, so neither branch computes it; the closed form holds at every z.
            return GeneralizedGammaUniform.Zeta(z, p[2]) is { } zeta
                ? GeneralizedGammaUniform.LogDensity(zeta, p[2]) - p[1] - Math.Log(t)
                : GammaLogDensity(p, z, t).Density;
        }

        /// <summary>The gamma's shape 1/λ², log |λ|, and the clipped log of its variate <c>e^(λz)/λ²</c>.</summary>
        private static (Jet InverseSquare, Jet LogLambda, Jet LogX) GammaVariate(Jet[] p, Jet z)
        {
            Jet inverseSquare = 1.0 / (p[2] * p[2]);
            Jet logLambda = Jet.Log(p[2].Value > 0.0 ? p[2] : -p[2]);
            return (inverseSquare, logLambda, ClippedLogVariate((p[2] * z) - (2.0 * logLambda)));
        }

        /// <summary>The log density through the gamma at shape 1/λ², with that shape and the clipped log of its variate.</summary>
        private static (Jet Density, Jet InverseSquare, Jet LogX) GammaLogDensity(Jet[] p, Jet z, double t)
        {
            (Jet inverseSquare, Jet logLambda, Jet logX) = GammaVariate(p, z);
            Jet density = logLambda - Math.Log(t) - p[1] - Jet.LogGamma(inverseSquare) - (2.0 * logLambda * inverseSquare)
                - Jet.Exp(logX) + (z / p[2]);
            return (density, inverseSquare, logX);
        }

        public override double Percentile(double[] p, double probability) =>
            Math.Exp(p[0] + (Math.Exp(p[1]) * GeneralizedGammaQuantile.Standardized(p[2], probability)));

        /// <summary>The log of lifelines' <c>clip(x, 1e-300, 1e20)</c>, which stops the derivative outside it as numpy's does.</summary>
        private static Jet ClippedLogVariate(Jet logX)
        {
            if (logX.Value < GeneralizedGammaUniform.LowestVariate)
            {
                return Jet.Constant(GeneralizedGammaUniform.LowestVariate, logX.Size);
            }

            return logX.Value > GeneralizedGammaUniform.HighestVariate ? Jet.Constant(GeneralizedGammaUniform.HighestVariate, logX.Size) : logX;
        }
    }
}
