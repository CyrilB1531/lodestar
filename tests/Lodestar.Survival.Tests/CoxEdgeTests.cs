using Lodestar.Stats;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>What <see cref="CoxProportionalHazards.Fit"/> refuses, and the settings it honours.</summary>
/// <remarks>
/// lifelines returns numbers behind a warning for a separated or collinear design; this refuses
/// both, because the information is singular there and no standard error is worth reporting.
/// </remarks>
public sealed class CoxEdgeTests
{
    /// <summary>Six subjects on one covariate, with ties and a censoring: a well-posed fit.</summary>
    private static readonly double[] Design = [0.2, 1.1, -0.4, 0.9, -1.3, 0.5];
    private static readonly double[] Durations = [5.0, 3.0, 8.0, 3.0, 9.0, 6.0];
    private static readonly bool[] Events = [true, true, false, true, true, true];

    [Fact]
    public void A_well_posed_fit_reports_one_row_per_covariate()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 1);

        double coefficient = Assert.Single(summary.Coefficients);
        Assert.Equal(0.95, summary.ConfidenceLevel);
        Assert.Equal(1, summary.LikelihoodRatioDegreesOfFreedom);
        Assert.Equal(Math.Exp(coefficient), Assert.Single(summary.HazardRatios), 1e-15);
    }

    [Fact]
    public void The_confidence_level_sets_the_interval_multiplier()
    {
        CoxSummary summary = CoxProportionalHazards.Fit(
            Design, Durations, Events, featureCount: 1, new CoxOptions { ConfidenceLevel = 0.9 });

        double multiplier = Distributions.NormalQuantile(0.95);
        Assert.Equal(0.9, summary.ConfidenceLevel);
        Assert.Equal(
            summary.Coefficients[0] - (multiplier * summary.StandardErrors[0]),
            summary.ConfidenceLower[0], 1e-12);
        Assert.Equal(
            summary.Coefficients[0] + (multiplier * summary.StandardErrors[0]),
            summary.ConfidenceUpper[0], 1e-12);
    }

    [Fact]
    public void A_covariate_that_separates_the_events_is_refused()
    {
        // Every event has a higher covariate than every subject still at risk after it, so the
        // likelihood keeps rising as the coefficient grows and has no maximum.
        double[] design = [4.0, 3.0, 2.0, 1.0, 0.0];
        double[] durations = [1.0, 2.0, 3.0, 4.0, 5.0];
        bool[] events = [true, true, true, true, true];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, durations, events, featureCount: 1));

        Assert.Contains("separat", error.Message, StringComparison.Ordinal);
        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_duplicated_covariate_is_refused_as_collinear()
    {
        double[] design = new double[Design.Length * 2];
        for (int i = 0; i < Design.Length; i++)
        {
            design[2 * i] = Design[i];
            design[(2 * i) + 1] = Design[i];
        }

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, Durations, Events, featureCount: 2));

        Assert.Contains("collinear", error.Message, StringComparison.Ordinal);
        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_sample_with_no_event_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, new bool[Design.Length], featureCount: 1));

        Assert.Equal("eventObserved", error.ParamName);
    }

    [Fact]
    public void A_design_whose_length_disagrees_with_the_sample_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 2));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_non_finite_covariate_is_refused()
    {
        double[] design = [.. Design];
        design[3] = double.NaN;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(design, Durations, Events, featureCount: 1));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void Spans_of_different_lengths_are_refused()
    {
        Assert.Throws<ArgumentException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events.AsSpan(0, 5), featureCount: 1));
    }

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CoxProportionalHazards.Fit(Design, Durations, Events, featureCount: 0));
    }

    [Fact]
    public void Running_out_of_iterations_throws_rather_than_reporting_a_table()
    {
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => CoxProportionalHazards.Fit(
                Design, Durations, Events, featureCount: 1, new CoxOptions { MaximumIterations = 1 }));

        Assert.Contains("1", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void An_iteration_budget_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoxOptions { MaximumIterations = 0 });
    }

    [Fact]
    public void A_line_search_no_halving_completes_does_not_converge()
    {
        // Undefined at every trial point, with a score of one where it starts: a stuck search, not a maximum (#1756).
        var fit = Internal.CoxNewton.Optimum(new UndefinedAwayFromZero(), 1, new Internal.ElasticNet(1.0, 0.0, 0.0), 10);

        Assert.Equal(Internal.CoxOutcome.Stalled, fit.Outcome);
    }

    [Fact]
    public void A_separation_whose_steps_halve_to_nothing_is_refused_as_separated()
    {
        // Past β = 113 the likelihood is flat to rounding and each step halved below an ulp of β: read as a search that
        // never ended, it was reported as not converging (#1756).
        double[] design = [-3, -7, -8, -5, -9, -2, -6, -1, 0, -4];
        double[] durations =
        [
            3.267531289377963, 7.712867221661316, 9.464928521525547, 4.461847229144465, 9.58739518634388,
            3.2338156473049033, 4.75147557200467, 2.9389857980138556, 0.19936126200452509, 3.746795427867582,
        ];
        bool[] events = [true, true, true, true, true, true, true, false, false, true];

        Assert.Throws<ArgumentException>(() => CoxProportionalHazards.Fit(design, durations, events, featureCount: 1));
    }

    [Fact]
    public void A_design_whose_linear_predictors_spread_past_745_reaches_its_maximum()
    {
        // Rows at ±300 left one shift a stratum to read a late risk set's sums as zero, and the design as separated. Each
        // risk set weighed from its own largest η, the proportional hazards fit agrees with the time-varying one (#1756).
        double[] design =
        [
            0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 300, -300,
        ];
        double[] durations =
        [
            1.2379408114677768, 0.02489171455561244, 0.04959140840698079, 0.05582264552393282, 1.1281877313523783,
            0.3790999966463894, 0.49704039725041826, 0.14697071492304847, 0.20451226025531533, 0.8162111396844042,
            0.10140501516874886, 0.7755750226411942, 0.9318486282039408, 0.00554944234733867, 0.17309781790931167,
            0.22327188217312052, 0.013652787957221635, 1.3931137436788028, 0.4649417481613966, 0.2642525933740092,
            0.10908036786687728, 0.15361968812112067, 0.5433791358185212, 0.13480724941155436, 0.11108347971579695,
            0.04185898198187138, 0.20685048076241427, 0.41778801669189475, 0.002774721173669335, 2.7862274873576056,
        ];
        bool[] events = [.. durations.Select((_, i) => i != 13 && i != 22)];

        CoxSummary fit = CoxProportionalHazards.Fit(design, durations, events, featureCount: 1);
        CoxSummary timeVarying = CoxTimeVarying.Fit(design, new double[durations.Length], durations, events, featureCount: 1);

        Assert.Equal(1.5094509141957, fit.Coefficients[0], 1e-9);
        Assert.Equal(timeVarying.Coefficients[0], fit.Coefficients[0], 1e-9);
        Assert.Equal(timeVarying.LogLikelihood, fit.LogLikelihood, 1e-9);
    }

    [Fact]
    public void A_row_far_below_the_others_keeps_the_robust_variance_and_the_proportional_hazards_test_finite()
    {
        // One row at η = −1274 alone at risk read its risk sum as zero: the robust variance and the test read NaN, and
        // the time-varying fit refused the design as separated (#1756).
        double[] design = [.. Enumerable.Range(0, 50).Select(i => ((i * 37 % 100) / 50.0) - 1.0)];
        double[] durations = [.. design.Select((x, i) => (Math.Exp(-x) * (1.0 + (i * 13 % 17 / 17.0))) + 0.01)];
        bool[] events = [.. design.Select((_, i) => i % 5 != 0)];
        (design[0], durations[0], events[0]) = (-1000.0, 100.0, true);

        CoxSummary fit = CoxProportionalHazards.Fit(design, durations, events, featureCount: 1);
        CoxSummary robust = CoxProportionalHazards.Fit(design, durations, events, featureCount: 1, new CoxOptions { Robust = true });
        CoxSummary timeVarying = CoxTimeVarying.Fit(design, new double[50], durations, events, featureCount: 1);

        Assert.Equal(fit.Coefficients[0], timeVarying.Coefficients[0], 1e-9);
        Assert.True(double.IsFinite(robust.StandardErrors[0]));
        Assert.True(double.IsFinite(CoxProportionalHazards.TestProportionalHazards(design, durations, events, fit)[0].PValue));
    }

    [Fact]
    public void A_maximum_far_out_converges_on_the_newton_decrement()
    {
        // The maximum sits at a standardised β of 661, where the step kept the score's rounding, 1.3e-10, past the
        // tolerance: the fit spent its budget and threw (#1756).
        double[] design =
        [
            -0.0012677115730984326, -0.0018026637973886995, -0.0030709757865924176, -0.003831515106581547,
            -0.0055788723730259195, -0.006008973548965481, -0.006747256928236899, -0.00800329361109484,
            -0.009303865138192492, -0.00997755038379574, -0.010518357261680208, -0.011714673601314847,
            -0.01263378339353966, -0.014263968664663479, -0.015004792927738883, -0.01616914212782199,
            -0.01705064829650098, -0.01828416559776469, -0.019205984092705644, -0.019603625222089267,
            -0.02136068121406766, -0.021795707006935346, -0.023046424352063423, -0.024173137508003675,
            -0.02478430265953915, -0.025945355192854517, -0.026600862333717838, -0.027379595487043322,
            -0.028798843229820997, -0.03010570563637175, -0.03117937474735492, -0.03248941552495689,
            -0.03297786284242804, -0.03418060493104655, -0.03435109839062121, -0.03616397252582886,
            -0.037188565009130974, -0.03828377398301546, -0.03903056254834782, -0.04016993400275666,
            -0.041526562642531004, -0.04166909531711537, -0.04285805633691587, -0.04350395052877507,
            -0.04479429496943828, -0.045993219348760245, -0.046712440427100294, -0.04800853883582374,
            -0.048658694631135486, -0.04999504262582052, -0.05091642132038156, -0.05253142009287792,
            -0.05311906031154281, -0.053894658667947994, -0.05495700063144101, -0.056018624923874244,
            -0.05664921665400687, -0.05764094055390819, -0.05937167261636438, -0.059974996028591264,
            -0.06085284871845741, -0.061368519194893834, -0.06290466208314685, -0.06412895181184546,
            -0.06502431826348282, -0.06563031676824059, -0.06772082042312333, -0.06770415252910957,
            -0.06864778537036256, -0.07004923281118117, -0.07065617144979545, -0.07221571520755311,
            -0.07332253727031231, -0.07447819623632478, -0.07505316369156241, -0.07587698500125674,
            -0.07695460758342522, -0.07840778168654759, -0.07898537599651731, -0.079831729524842, -0.08086853298442442,
            -0.08157130979716803, -0.083314622642339, -0.08389164354404065, -0.08504920875797398, -0.0859750817864324,
            -0.08716962790418764, -0.08798939482536743, -0.08940999674539736, -0.09018086700490956,
            -0.09071483127827623, -0.0921757258875225, -0.09309619062476941, -0.09389140619979525, -0.09484064139834038,
            -0.09563661452209425, -0.09723761912627028, -0.0980723512792275, -0.09938249066678671, -0.0998675951086909,
            -0.1009798354759904, -0.10194579091535681, -0.10332020389502236,
        ];
        double[] durations = [.. design.Select((_, i) => i + 1.0)];
        int[] censored = [10, 17, 27, 29, 30, 36, 59, 74, 78, 83, 88, 94];
        bool[] events = [.. design.Select((_, i) => !censored.Contains(i))];

        CoxSummary fit = CoxProportionalHazards.Fit(design, durations, events, featureCount: 1);

        Assert.Equal(22117.00205458567, fit.Coefficients[0], 1e-9 * 22117.0);
        Assert.Equal(-0.9593811311738136, fit.LogLikelihood, 1e-12);
    }

    private sealed class UndefinedAwayFromZero : Internal.IPartialLikelihood
    {
        public double Evaluate(ReadOnlySpan<double> coefficients, Span<double> score, Span<double> information)
        {
            score[0] = 1.0;
            information[0] = 1.0;
            return coefficients[0] == 0.0 ? -1.0 : double.NaN;
        }
    }
}
