using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>One of lifelines' AFT models in its two linear predictors: the primary <c>η₁</c> and the ancillary <c>η₂</c>.</summary>
/// <remarks>Each is written as lifelines' <c>WeibullAFTFitter</c>, <c>LogNormalAFTFitter</c> and <c>LogLogisticAFTFitter</c> write it.</remarks>
internal abstract class AftShape
{
    public abstract string Primary { get; }

    public abstract string AncillaryName { get; }

    /// <summary>The univariate model lifelines starts from, and scores the null model with.</summary>
    public abstract ParametricModel Univariate { get; }

    public static AftShape For(AftModel model) => model switch
    {
        AftModel.Weibull => new Weibull(),
        AftModel.LogNormal => new LogNormal(),
        _ => new LogLogistic(),
    };

    public abstract Jet CumulativeHazard(Jet primary, Jet ancillary, double t);

    public abstract Jet LogHazard(Jet primary, Jet ancillary, double t);

    public virtual Jet LogOneMinusSurvival(Jet primary, Jet ancillary, double t) => Jet.Log1mExp(CumulativeHazard(primary, ancillary, t));

    /// <summary>The time by which a subject's survival falls to <paramref name="p"/>, lifelines' closed form.</summary>
    public abstract double Percentile(double primary, double ancillary, double p);

    /// <summary>A subject's expected survival time, lifelines' closed form.</summary>
    public abstract double Expectation(double primary, double ancillary);

    /// <summary>lifelines' start: each univariate parameter as the intercept of its block, logged where it is positive.</summary>
    public static double Transformed(double parameter) => parameter <= 0.0 ? parameter : Math.Log(parameter);

    protected static double ClippedLog(double t, double floor) => Math.Log(Math.Max(t, floor));

    private sealed class Weibull : AftShape
    {
        public override string Primary => "lambda_";

        public override string AncillaryName => "rho_";

        public override ParametricModel Univariate => ParametricModel.Weibull;

        public override Jet CumulativeHazard(Jet primary, Jet ancillary, double t) =>
            Jet.Exp(Jet.Exp(ancillary) * (ClippedLog(t, 1e-100) - primary));

        public override Jet LogHazard(Jet primary, Jet ancillary, double t) =>
            ancillary - primary + (Jet.Expm1(ancillary) * (Math.Log(t) - primary));

        public override double Percentile(double primary, double ancillary, double p) =>
            Math.Exp(primary) * Math.Pow(-Math.Log(p), 1.0 / Math.Exp(ancillary));

        public override double Expectation(double primary, double ancillary) =>
            Math.Exp(primary) * Math.Exp(Special.LogGamma(1.0 + (1.0 / Math.Exp(ancillary))));
    }

    private sealed class LogNormal : AftShape
    {
        public override string Primary => "mu_";

        public override string AncillaryName => "sigma_";

        public override ParametricModel Univariate => ParametricModel.LogNormal;

        public override Jet CumulativeHazard(Jet primary, Jet ancillary, double t) =>
            -Jet.NormalLogSf((Math.Log(t) - primary) / Jet.Exp(ancillary));

        public override Jet LogHazard(Jet primary, Jet ancillary, double t)
        {
            Jet z = (Math.Log(t) - primary) / Jet.Exp(ancillary);
            return Jet.NormalLogPdf(z) - ancillary - Math.Log(t) - Jet.NormalLogSf(z);
        }

        public override Jet LogOneMinusSurvival(Jet primary, Jet ancillary, double t) =>
            Jet.NormalLogCdf((Math.Log(t) - primary) / Jet.Exp(ancillary));

        public override double Percentile(double primary, double ancillary, double p) =>
            Math.Exp(primary) * Math.Exp(Math.Exp(ancillary) * Distributions.NormalQuantile(1.0 - p));

        public override double Expectation(double primary, double ancillary)
        {
            double sigma = Math.Exp(ancillary);
            return Math.Exp(primary) * Math.Exp(sigma * sigma / 2.0);
        }
    }

    private sealed class LogLogistic : AftShape
    {
        public override string Primary => "alpha_";

        public override string AncillaryName => "beta_";

        public override ParametricModel Univariate => ParametricModel.LogLogistic;

        public override Jet CumulativeHazard(Jet primary, Jet ancillary, double t) =>
            Jet.LogAddExp0(Jet.Exp(ancillary) * (ClippedLog(t, 1e-25) - primary));

        public override Jet LogHazard(Jet primary, Jet ancillary, double t) =>
            ancillary - primary + (Jet.Expm1(ancillary) * (Math.Log(t) - primary))
            - Jet.LogAddExp0(Jet.Exp(ancillary) * (Math.Log(t) - primary));

        public override Jet LogOneMinusSurvival(Jet primary, Jet ancillary, double t) =>
            -Jet.LogAddExp0(-Jet.Exp(ancillary) * (Math.Log(t) - primary));

        public override double Percentile(double primary, double ancillary, double p) =>
            Math.Exp(primary) * Math.Pow((1.0 / p) - 1.0, 1.0 / Math.Exp(ancillary));

        /// <summary><c>απ/β / sin(π/β)</c> where <c>β &gt; 1</c>, NaN otherwise: the mean diverges, as lifelines reports it.</summary>
        public override double Expectation(double primary, double ancillary)
        {
            double beta = Math.Exp(ancillary);
            return beta > 1.0 ? Math.Exp(primary) * Math.PI / beta / Math.Sin(Math.PI / beta) : double.NaN;
        }
    }
}
