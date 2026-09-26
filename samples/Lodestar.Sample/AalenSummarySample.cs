using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>Every table an Aalen fit reports, and what it predicts for each arm.</summary>
internal static class AalenSummarySample
{
    public static void Run()
    {
        Console.WriteLine("AalenSummary (Lodestar.Survival)");

        (double[] design, double[] weeks, bool[] observed) = AcceleratedFailureTimeSample.Arms();
        AalenSummary s = AalenAdditive.Fit(design, weeks, observed, featureCount: 1);
        int k = s.CovariateIndices.Count;
        for (int j = 0; j < k; j++)
        {
            string name = s.CovariateIndices[j] < 0 ? "Intercept" : $"x{s.CovariateIndices[j]}";
            Console.WriteLine(
                $"  {name,-16} : first step {Inv.F4(s.Hazards[j])}, at the end {Inv.F4(s.CumulativeHazards[((s.EventTimes.Count - 1) * k) + j])} "
                + $"(variance {Inv.F4(s.CumulativeVariance[((s.EventTimes.Count - 1) * k) + j])}, "
                + $"[{Inv.F3(s.ConfidenceLower[((s.EventTimes.Count - 1) * k) + j])}, {Inv.F3(s.ConfidenceUpper[((s.EventTimes.Count - 1) * k) + j])}]), "
                + $"slope {Inv.F4(s.Slopes[j])} se {Inv.F4(s.SlopeStandardErrors[j])}");
        }

        Console.WriteLine($"  concordance      : {Inv.F3(s.ConcordanceIndex)} ({s.FeatureCount} covariate, intercept {s.FitIntercept})");

        // One row per arm: control, then treatment.
        double[] arms = [0.0, 1.0];
        double[] cumulative = s.PredictCumulativeHazard(arms);
        double[] survival = s.PredictSurvivalFunction(arms);
        double[] medians = s.PredictMedian(arms);
        double[] quartiles = s.PredictPercentile(arms, 0.75);
        double[] means = s.PredictExpectation(arms);
        double[] smoothed = s.SmoothedHazards(2.0);
        Console.WriteLine(
            $"  control          : H {Inv.F4(cumulative[0])}, S {Inv.F4(survival[0])}, median {Inv.F1(medians[0])}, "
            + $"p75 {Inv.F1(quartiles[0])}, mean {Inv.F3(means[0])}; treated median {Inv.F1(medians[1])}");
        Console.WriteLine($"  smoothed         : {Inv.F4(smoothed[0])} at the first event time");
        Console.WriteLine();
    }
}
