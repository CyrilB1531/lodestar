using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>A Weibull curve through Freireich's treatment arm, censored three ways, and the arms compared at one week.</summary>
internal static class ParametricSurvivalSample
{
    public static void Run()
    {
        Console.WriteLine("Parametric survival (Lodestar.Survival)");

        ParametricFit right = ParametricSurvival.Fit(ParametricModel.Weibull, Trial.TreatmentDurations, Trial.TreatmentObserved);
        Console.WriteLine($"  right-censored   : lambda {Inv.F4(right.Parameters[0])}, rho {Inv.F4(right.Parameters[1])}");

        // Read the censored subjects as having had the event by their time instead, then as an interval of a week.
        ParametricFit left = ParametricSurvival.FitLeftCensored(ParametricModel.Weibull, Trial.TreatmentDurations, Trial.TreatmentObserved);
        double[] lower = [.. Trial.TreatmentDurations.Select((d, i) => Trial.TreatmentObserved[i] ? d : d - 1.0)];
        double[] upper = [.. Trial.TreatmentDurations.Select((d, i) => Trial.TreatmentObserved[i] ? d : d + 1.0)];
        ParametricFit interval = ParametricSurvival.FitIntervalCensored(ParametricModel.Weibull, lower, upper);
        Console.WriteLine($"  left / interval  : lambda {Inv.F4(left.Parameters[0])} / {Inv.F4(interval.Parameters[0])}");

        // Half the weight on the first nine subjects, and the last three entering the study at week five.
        double[] weights = [.. Trial.TreatmentDurations.Select((_, i) => i < 9 ? 0.5 : 1.0)];
        double[] entries = [.. Trial.TreatmentDurations.Select((_, i) => i >= 18 ? 5.0 : 0.0)];
        ParametricFit weighted = ParametricSurvival.Fit(
            ParametricModel.Weibull, Trial.TreatmentDurations, Trial.TreatmentObserved, weights, entries);
        ParametricFit leftWeighted = ParametricSurvival.FitLeftCensored(
            ParametricModel.Weibull, Trial.TreatmentDurations, Trial.TreatmentObserved, weights, entries);
        ParametricFit intervalWeighted = ParametricSurvival.FitIntervalCensored(ParametricModel.Weibull, lower, upper, weights, entries);
        Console.WriteLine(
            $"  weighted, entry  : lambda {Inv.F4(weighted.Parameters[0])}, {Inv.F4(leftWeighted.Parameters[0])}, "
            + $"{Inv.F4(intervalWeighted.Parameters[0])}");

        ParametricFit control = ParametricSurvival.Fit(ParametricModel.Weibull, Trial.ControlDurations, Trial.ControlObserved);
        Console.WriteLine($"  differ at 10     : p {Inv.F4(ParametricSurvival.CompareAt(10.0, right, control).PValue)}");
        Console.WriteLine();
    }
}
