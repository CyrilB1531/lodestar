using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>Nothing a fit hands out can edit what its predictions read (#1304).</summary>
/// <remarks>
/// A list backed by the fit's own array could be cast back and written; a baseline record's arrays can
/// be written outright. Either way the next prediction changed, silently.
/// </remarks>
public sealed class FittedImmutabilityTests
{
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 1.0, 6.0, 2.5, 9.0, 4.0, 1.5, 7.0];
    private static readonly bool[] Events = [true, true, false, true, true, true, false, true, true, true];
    private static readonly double[] Design = [0.5, -1.2, 0.3, 1.8, -0.4, 0.9, -1.5, 0.2, 1.1, -0.7];

    [Fact]
    public void Writing_to_a_cox_baseline_leaves_the_predictions_as_they_were()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 1);
        double[] before = summary.PredictCumulativeHazard(Design, [], []);

        CoxBaseline baseline = summary.Baselines[0];
        Array.Fill(baseline.CumulativeHazard, 1e6);
        Array.Fill(baseline.Times, 0.0);

        Assert.Equal(before, summary.PredictCumulativeHazard(Design, [], []));
        Assert.Equal(1e6, summary.Baselines[0].CumulativeHazard[0]);
    }

    [Fact]
    public void The_strata_of_one_fit_do_not_share_their_times()
    {
        int[] strata = [0, 0, 0, 0, 0, 1, 1, 1, 1, 1];
        CoxSummary summary = CoxProportionalHazards.Fit(Design, Durations, Events, [], strata, [], featureCount: 1);

        Assert.NotSame(summary.Baselines[0].Times, summary.Baselines[1].Times);
    }

    [Fact]
    public void A_cox_summary_hands_out_no_array_it_reads_back()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 1);

        Assert.IsNotType<double[]>(summary.Coefficients);
        Assert.IsNotType<double[]>(summary.CovariateMeans);
        Assert.IsNotType<CoxBaseline[]>(summary.Baselines);
        Assert.IsNotType<double[]>(summary.StandardErrors);
    }

    [Fact]
    public void An_aft_summary_hands_out_no_array_it_reads_back()
    {
        AftSummary summary = AcceleratedFailureTime.Fit(AftModel.Weibull, Design, Durations, Events, 1);

        Assert.IsNotType<double[]>(summary.Coefficients);
        Assert.IsNotType<double[]>(summary.StandardErrors);
    }

    [Fact]
    public void A_parametric_fit_hands_out_no_array_it_reads_back()
    {
        ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, Durations, Events);

        Assert.IsNotType<double[]>(fit.Parameters);
        Assert.IsNotType<string[]>(fit.ParameterNames);
        Assert.IsNotType<double[]>(fit.StandardErrors);
    }

    [Fact]
    public void An_aalen_summary_hands_out_no_array_it_reads_back()
    {
        AalenSummary summary = AalenAdditive.Fit(Design, Durations, Events, 1);

        Assert.IsNotType<double[]>(summary.EventTimes);
        Assert.IsNotType<double[]>(summary.CumulativeHazards);
        Assert.IsNotType<double[]>(summary.Slopes);
    }
}
