using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>Every figure an AFT fit reports, and what it predicts for each arm.</summary>
internal static class AftSummarySample
{
    public static void Run()
    {
        Console.WriteLine("AftSummary (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = AcceleratedFailureTimeSample.Arms();
        AftSummary s = AcceleratedFailureTime.Fit(AftModel.LogLogistic, design, weeks, observed, featureCount: 1);
        for (int j = 0; j < s.Coefficients.Count; j++)
        {
            string covariate = s.CovariateIndices[j] < 0 ? "Intercept" : $"x{s.CovariateIndices[j]}";
            Console.WriteLine(
                $"  {s.ParameterNames[j]}{covariate,-10} : {Inv.F4(s.Coefficients[j])} se {Inv.F4(s.StandardErrors[j])} "
                + $"z {Inv.F3(s.ZStatistics[j])} p {Inv.F4(s.PValues[j])} [{Inv.F3(s.ConfidenceLower[j])}, {Inv.F3(s.ConfidenceUpper[j])}] "
                + $"exp {Inv.F3(s.ExpCoefficients[j])} [{Inv.F3(s.ExpConfidenceLower[j])}, {Inv.F3(s.ExpConfidenceUpper[j])}]");
        }

        Console.WriteLine(
            $"  {s.Model} fit : log-likelihood {Inv.F4(s.LogLikelihood)} against {Inv.F4(s.NullLogLikelihood)}, "
            + $"LR {Inv.F3(s.LikelihoodRatioStatistic)} on {s.LikelihoodRatioDegreesOfFreedom} df p {Inv.F4(s.LikelihoodRatioPValue)}");
        Console.WriteLine(
            $"  AIC, concordance : {Inv.F3(s.Aic)}, {Inv.F3(s.ConcordanceIndex)} at {Inv.F3(s.ConfidenceLevel)} "
            + $"({s.FeatureCount} covariate, robust {s.Robust})");

        // One row per arm: control, then treatment.
        double[] arms = [0.0, 1.0];
        double[] medians = s.PredictMedian(arms);
        double[] quartiles = s.PredictPercentile(arms, 0.25);
        double[] means = s.PredictExpectation(arms);
        Console.WriteLine($"  median, p25, mean: control {Inv.F3(medians[0])}, {Inv.F3(quartiles[0])}, {Inv.F3(means[0])}; treated {Inv.F3(medians[1])}");
        double[] survival = s.PredictSurvivalFunction(arms, [10.0]);
        double[] cumulative = s.PredictCumulativeHazard(arms, [10.0]);
        Console.WriteLine($"  S(10), H(10)     : {Inv.F4(survival[0])} / {Inv.F4(survival[1])}, {Inv.F4(cumulative[0])} / {Inv.F4(cumulative[1])}");
        Console.WriteLine();
    }
}
