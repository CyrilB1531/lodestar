using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Internal;
using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// A fit whose every residual is exactly zero has a zero robust covariance, whose Wald tests are NaN and whose GMM weight
/// is refused; one exact but for its rounding reads the ordinary formulas (#1779, #1784); a response with nothing to
/// explain leaves every test divided by its total NaN; and HC2 and HC3 weigh a leverage of one as statsmodels does.
/// </summary>
public sealed class ExactFitTests
{
    private const int Rows = 40;

    // Integers and quarters, so y = 3 + 2·x₁ − x₂ is exact in doubles and only the solve's rounding is left.
    private static readonly double[] Design = [.. Enumerable.Range(0, Rows).SelectMany(i => new[] { (i * 7 % 23) - 11.0, ((i * 5 % 17) - 8) * 0.25 })];
    private static readonly double[] Exact = [.. Enumerable.Range(0, Rows).Select(i => 3.0 + (2.0 * Design[2 * i]) - Design[(2 * i) + 1])];
    private static readonly double[] Weights = [.. Enumerable.Range(0, Rows).Select(i => 1.0 + (i % 3))];
    private static readonly int[] Clusters = [.. Enumerable.Range(0, Rows).Select(i => i % 6)];
    private static readonly int[] Entities = [.. Enumerable.Range(0, Rows).Select(i => i / 5)];
    private static readonly int[] Periods = [.. Enumerable.Range(0, Rows).Select(i => i % 5)];

    // One instrument pair; the endogenous column is not a function of them, so only the second stage is exact.
    private static readonly double[] Instruments = [.. Enumerable.Range(0, Rows).SelectMany(i => new[] { (i * 3 % 13) - 6.0, (i * 11 % 19) - 9.0 })];
    private static readonly double[] Endogenous = [.. Enumerable.Range(0, Rows).Select(i => Instruments[2 * i] + (2.0 * Instruments[(2 * i) + 1]) + (i * 13 % 7) - 3.0)];
    private static readonly double[] Exogenous = [.. Enumerable.Range(0, Rows).Select(i => Design[2 * i])];
    private static readonly double[] IvExact = [.. Enumerable.Range(0, Rows).Select(i => 1.0 + Exogenous[i] - (2.0 * Endogenous[i]))];

    // The same responses tens of ulps off on four rows in five: those residuals cannot round to zero, whatever order
    // a solve sums in, so the fit is not exact, and no tolerance this side of #1784 may call it so.
    private static readonly double[] Near = [.. Exact.Select((value, i) => value + (1e-13 * ((i % 5) - 2)))];
    private static readonly double[] IvNear = [.. IvExact.Select((value, i) => value + (1e-12 * ((i % 5) - 2)))];

    public static TheoryData<CovarianceType> Robust => new()
    {
        CovarianceType.Hc0, CovarianceType.Hc1, CovarianceType.Hc2, CovarianceType.Hc3, CovarianceType.Hac, CovarianceType.Cluster,
    };

    private static OlsOptions Options(CovarianceType type) =>
        new() { CovarianceType = type, HacLags = type == CovarianceType.Hac ? 3 : null };

    // Two indicators of four rows each and four rows zero in both: every solve is exact in doubles, y = 3·d₁ − 2·d₂ leaves
    // every residual exactly zero, and the zero rows are the ones a zero weight or a missing category leaves (#1784).
    private static readonly double[] Dummies =
        [.. Enumerable.Range(0, 12).SelectMany(i => new[] { i is >= 1 and <= 4 ? 1.0 : 0.0, i is >= 6 and <= 9 ? 1.0 : 0.0 })];
    private static readonly double[] DummyExact = [.. Enumerable.Range(0, 12).Select(i => (3.0 * Dummies[2 * i]) - (2.0 * Dummies[(2 * i) + 1]))];
    private static readonly int[] DummyClusters = [.. Enumerable.Range(0, 12).Select(i => i % 4)];

    private static OlsOptions NoIntercept(CovarianceType type) => Options(type) with { WithIntercept = false };

    /// <summary>Residuals exactly zero beside rows zero in every column: the covariance is zero, its tests undefined.</summary>
    [Theory]
    [MemberData(nameof(Robust))]
    public void An_exact_ordinary_fit_has_no_robust_f_test(CovarianceType type)
    {
        OlsSummary fit = type == CovarianceType.Cluster
            ? OrdinaryLeastSquares.Fit(Dummies, DummyExact, DummyClusters, 2, NoIntercept(type))
            : OrdinaryLeastSquares.Fit(Dummies, DummyExact, 2, NoIntercept(type));

        Assert.Equal(0.0, fit.ResidualStandardError);
        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        Assert.True(double.IsNaN(fit.FPValue));
        Assert.Equal(0, fit.FNumeratorDegreesOfFreedom);
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>A zero weight zeroes its row; the others, weighted by squares, keep every residual exactly zero.</summary>
    [Theory]
    [MemberData(nameof(Robust))]
    public void An_exact_weighted_fit_has_no_robust_f_test(CovarianceType type)
    {
        double[] weights = [.. Enumerable.Range(0, 12).Select(i => i == 2 ? 0.0 : 1.0 + (3.0 * (i % 2)))];

        OlsSummary fit = type == CovarianceType.Cluster
            ? WeightedLeastSquares.Fit(Dummies, DummyExact, weights, DummyClusters, 2, NoIntercept(type))
            : WeightedLeastSquares.Fit(Dummies, DummyExact, weights, 2, NoIntercept(type));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    [Theory]
    [InlineData(CovarianceType.Hc0)]
    [InlineData(CovarianceType.Hc3)]
    public void An_exact_generalized_fit_has_no_robust_f_test(CovarianceType type)
    {
        double[] sigma = new double[12 * 12];
        for (int i = 0; i < 12; i++)
        {
            sigma[(i * 12) + i] = i % 2 == 0 ? 1.0 : 4.0;
        }

        OlsSummary fit = GeneralizedLeastSquares.Fit(Dummies, DummyExact, sigma, 2, NoIntercept(type));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>
    /// An AR(1) covariance mixes the rows its whitening reads, and <c>y = 0</c> leaves the whitened residuals exactly
    /// zero: the robust covariance is zero and its F NaN, the homoskedastic F <c>+∞</c>.
    /// </summary>
    [Fact]
    public void An_exact_generalized_fit_with_correlated_errors_has_no_robust_f_test()
    {
        double[] correlated = new double[Rows * Rows];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                correlated[(i * Rows) + j] = Math.Pow(0.9, Math.Abs(i - j));
            }
        }

        double[] zero = new double[Rows];
        OlsSummary robust = GeneralizedLeastSquares.Fit(Design, zero, correlated, 2, Options(CovarianceType.Hc1));
        OlsSummary classical = GeneralizedLeastSquares.Fit(Design, zero, correlated, 2);

        Assert.True(double.IsNaN(robust.FStatistic), $"{robust.FStatistic}");
        AssertZeroCovariance(robust.StandardErrors, robust.TStatistics, robust.PValues);
        Assert.True(double.IsPositiveInfinity(classical.FStatistic), $"{classical.FStatistic}");
    }

    /// <summary>
    /// <c>y = 3 + 2·x₁ − x₂</c> tens of ulps off: no tolerance calls that exact, so the robust tests read the ordinary
    /// formulas, as statsmodels and linearmodels do (#1784, #1785).
    /// </summary>
    [Fact]
    public void A_fit_tens_of_ulps_from_exact_reads_the_ordinary_formulas()
    {
        double[] weights = [.. Weights.Select((w, i) => i == 7 ? 0.0 : w)];
        OlsSummary ols = OrdinaryLeastSquares.Fit(Design, Near, 2, Options(CovarianceType.Hc0));
        OlsSummary weighted = WeightedLeastSquares.Fit(Design, Near, weights, 2, Options(CovarianceType.Hc1));
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvNear, Exogenous, 1, Endogenous, 1, Instruments, 2), new IvOptions { CovarianceType = IvCovarianceType.Robust });
        double[] correlated = new double[Rows * Rows];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                correlated[(i * Rows) + j] = Math.Pow(0.9, Math.Abs(i - j));
            }
        }

        OlsSummary generalized = GeneralizedLeastSquares.Fit(Design, Near, correlated, 2, Options(CovarianceType.Hc0));
        PanelSummary panel = PanelRegression.FixedEffects(
            new PanelDesign(Near, Design, 2, Entities, Periods),
            new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Robust });

        Assert.True(ols.ResidualStandardError > 0.0);
        double[] statistics =
            [ols.FStatistic, weighted.FStatistic, generalized.FStatistic, iv.ModelTest!.Statistic, panel.RobustModelTest!.Statistic];
        foreach (double statistic in statistics)
        {
            Assert.True(double.IsFinite(statistic) && statistic > 1e20, $"{statistic}");
        }

        Assert.All(ols.StandardErrors.Concat(iv.StandardErrors).Concat(panel.StandardErrors), error => Assert.True(error > 0.0, $"{error}"));
    }

    [Fact]
    public void A_fit_one_part_in_a_billion_off_exact_keeps_its_test()
    {
        double[] y = [.. Exact.Select((value, i) => value + (1e-9 * ((i % 5) - 2)))];

        OlsSummary fit = OrdinaryLeastSquares.Fit(Design, y, 2, Options(CovarianceType.Hc1));

        Assert.True(fit.FStatistic > 1e15 && double.IsFinite(fit.FStatistic), $"{fit.FStatistic}");
        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
    }

    [Theory]
    [InlineData(IvCovarianceType.Robust)]
    [InlineData(IvCovarianceType.Kernel)]
    [InlineData(IvCovarianceType.Clustered)]
    public void An_exact_instrumental_fit_has_no_model_test(IvCovarianceType type)
    {
        var design = new IvDesign(HadamardResponse, HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2);
        var options = new IvOptions { CovarianceType = type, Bandwidth = type == IvCovarianceType.Kernel ? 2 : null };

        IvSummary fit = type == IvCovarianceType.Clustered
            ? InstrumentalVariables.TwoStageLeastSquares(design, [0, 1, 2, 3, 0, 1, 2, 3], options)
            : InstrumentalVariables.TwoStageLeastSquares(design, options);

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
        Assert.True(double.IsNaN(fit.ModelTest.PValue));
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>
    /// A first stage is its own fit: an endogenous column one Hadamard column off the instruments' span is not exact, so
    /// its instrument test keeps its ratio beside a second stage whose every residual is exactly zero.
    /// </summary>
    [Fact]
    public void A_first_stage_that_is_not_exact_keeps_its_test_beside_an_exact_second_stage()
    {
        double[] offSpan = [1, 1, 1, 1, -1, -1, -1, -1];
        double[] endogenous = [.. HadamardEndogenous.Select((value, i) => value + offSpan[i])];
        double[] y = [.. Enumerable.Range(0, 8).Select(i => 1.0 + HadamardExogenous[i] - (2.0 * endogenous[i]))];

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(y, HadamardExogenous, 1, endogenous, 1, HadamardInstruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Robust });

        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
        Assert.True(double.IsFinite(fit.FirstStage[0].InstrumentTest.Statistic), $"{fit.FirstStage[0].InstrumentTest.Statistic}");
    }

    /// <summary>
    /// The unadjusted covariance is <c>s²</c> times a fixed matrix, the homoskedastic one: a fit tens of ulps from exact
    /// reads its residuals, as OLS's non-robust and the panel's unadjusted covariances do, so its test stays a ratio.
    /// </summary>
    [Fact]
    public void A_near_exact_instrumental_fit_keeps_the_unadjusted_covariance()
    {
        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvNear, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted });

        Assert.True(fit.ModelTest!.Statistic > 1e20, $"{fit.ModelTest.Statistic}");
        Assert.All(fit.StandardErrors, error => Assert.True(error > 0.0, $"{error}"));
    }

    /// <summary>Every autocovariance of scores exactly zero is zero, so the automatic bandwidth is 0/0.</summary>
    [Fact]
    public void An_exact_fit_has_no_automatic_kernel_bandwidth()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(HadamardResponse, HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Kernel }));

        Assert.Equal("options", refusal.ParamName);
    }

    /// <summary>An exact fit still refuses a kernel bandwidth past the sample's lags, through <c>IvScores.RequireBandwidth</c>.</summary>
    [Fact]
    public void An_exact_fit_refuses_a_kernel_bandwidth_past_its_sample()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(HadamardResponse, HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Kernel, Bandwidth = 100 }));

        Assert.Contains("weights 101 lags of a sample of 8", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Gmm_refuses_an_exact_first_step_naming_the_design()
    {
        double[] zero = new double[Rows];
        ArgumentException plain = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(zero, Exogenous, 1, Endogenous, 1, Instruments, 2)));
        ArgumentException clustered = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(zero, Exogenous, 1, Endogenous, 1, Instruments, 2),
            Clusters,
            new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered }));

        Assert.All(new[] { plain, clustered }, refusal =>
        {
            Assert.Equal("design", refusal.ParamName);
            Assert.Contains("first-step fit is exact", refusal.Message, StringComparison.Ordinal);
        });
    }

    /// <summary>
    /// An unadjusted weight, <c>s²</c> times <c>ZᵀZ/n</c>, only rescales 2SLS's: a first step tens of ulps from exact
    /// leaves it invertible, so GMM accepts it and reads those residuals, whose J no reference fixes.
    /// </summary>
    [Fact]
    public void Gmm_accepts_an_unadjusted_weight_on_a_near_exact_first_step()
    {
        IvSummary fit = InstrumentalVariables.Gmm(
            new IvDesign(IvNear, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted, GmmWeightType = IvCovarianceType.Unadjusted });

        Assert.True(fit.ModelTest!.Statistic > 1e20, $"{fit.ModelTest.Statistic}");
        Assert.True(double.IsFinite(fit.Overidentification!.Statistic) && fit.Overidentification.Statistic >= 0.0, $"{fit.Overidentification.Statistic}");
    }

    /// <summary>Residuals exactly zero leave even the unadjusted weight zero, refused as such rather than as too few clusters.</summary>
    [Theory]
    [InlineData(IvCovarianceType.Unadjusted)]
    [InlineData(IvCovarianceType.Robust)]
    [InlineData(IvCovarianceType.Kernel)]
    public void Gmm_refuses_an_unadjusted_weight_on_residuals_exactly_zero(IvCovarianceType reported)
    {
        double[] zero = new double[Rows];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(zero, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = reported, GmmWeightType = IvCovarianceType.Unadjusted }));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("is zero", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Gmm_refuses_an_unadjusted_weight_on_residuals_exactly_zero_under_a_clustered_covariance()
    {
        double[] zero = new double[Rows];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(zero, Exogenous, 1, Endogenous, 1, Instruments, 2),
            Clusters,
            new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Unadjusted }));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("is zero", refusal.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Small integers whose first GMM step keeps a residual of one rounding and whose second, under the robust weight,
    /// leaves every residual exactly zero, found by search: the reported covariance is zero, a kernel's bandwidth GMM's
    /// <c>n − 2</c>, and the J statistic still reads the weight.
    /// </summary>
    [Theory]
    [InlineData(IvCovarianceType.Robust)]
    [InlineData(IvCovarianceType.Kernel)]
    public void Gmm_reports_a_zero_covariance_on_an_exact_second_step(IvCovarianceType reported)
    {
        double[] exogenous = [2, -2, 3, 2, -2, -3, -3, 2, 2, 0, 2, -2, 3, -1, -1, 2, 3, -3, -1, 3];
        double[] first = [-2, 3, 0, 1, -2, -3, 1, -1, 1, -2, 2, 3, -2, 1, -3, -1, 1, 3, -3, -1];
        double[] second = [1, -2, 0, 0, -1, -2, 1, 2, 3, -3, -2, 0, -2, 0, 0, 1, -1, 2, 2, -2];
        double[] endogenous = [-3, 7, 0, 1, -4, -7, 2, -5, -5, 4, 4, 4, 0, 3, -3, -3, -1, 3, -5, 3];
        double[] instruments = [.. Enumerable.Range(0, 20).SelectMany(i => new[] { first[i], second[i] })];
        double[] y = [.. Enumerable.Range(0, 20).Select(i => -1.0 - (3.0 * exogenous[i]) - endogenous[i])];

        IvSummary fit = InstrumentalVariables.Gmm(
            new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2),
            new IvOptions { CovarianceType = reported, GmmWeightType = IvCovarianceType.Robust });

        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
        Assert.True(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
        Assert.Equal(reported == IvCovarianceType.Kernel ? 18 : null, fit.Bandwidth);
        Assert.NotNull(fit.Overidentification);
    }

    /// <summary>
    /// Without a constant, columns of ±1 summing to zero leave <c>Zᵀy</c> exactly zero on a constant response, every
    /// partial sum a multiple of it: every residual is the response, and a dyadic one's mean rounds back to it, so s² is
    /// exactly zero, refused as that. Beside a constant, at 0.1 and 7.7 too, the residuals are one rounding apiece and
    /// s² zero or not as it falls: accepted or refused the same way, and neither blames clusters (#1786).
    /// </summary>
    [Theory]
    [InlineData(5.0, 0.1)]
    [InlineData(-2.5, 7.7)]
    [InlineData(0.375, 1e-3)]
    [InlineData(1e10, 123.456)]
    public void Gmm_refuses_an_unadjusted_weight_on_residuals_that_do_not_vary(double level, double other)
    {
        var options = new IvOptions { CovarianceType = IvCovarianceType.Unadjusted, GmmWeightType = IvCovarianceType.Unadjusted };

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign([.. Enumerable.Repeat(level, 8)], HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2),
            options with { WithIntercept = false }));
        AssertZeroSquares(refusal);

        foreach (double value in new[] { level, other })
        {
            try
            {
                IvSummary fit = InstrumentalVariables.Gmm(
                    new IvDesign([.. Enumerable.Repeat(value, Rows)], Exogenous, 1, Endogenous, 1, Instruments, 2), options);
                Assert.NotNull(fit.Overidentification);
            }
            catch (ArgumentException zero)
            {
                AssertZeroSquares(zero);
            }
        }
    }

    /// <summary>
    /// Residuals near 1e-170 leave the weight's squared scores underflowing to a singular block: refused naming the
    /// design, not blamed on clusters, which only a clustered weight has, and here six of them for four instruments (#1786).
    /// </summary>
    [Theory]
    [InlineData(IvCovarianceType.Robust)]
    [InlineData(IvCovarianceType.Kernel)]
    [InlineData(IvCovarianceType.Clustered)]
    public void Gmm_names_clusters_only_where_they_fall_short(IvCovarianceType weight)
    {
        double[] y = [.. Enumerable.Range(0, Rows).Select(i => 1e-170 * ((((i * 53) % 97) / 97.0) - 0.5))];
        var options = new IvOptions { CovarianceType = IvCovarianceType.Unadjusted, GmmWeightType = weight };

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => weight == IvCovarianceType.Clustered
            ? InstrumentalVariables.Gmm(new IvDesign(y, Exogenous, 1, Endogenous, 1, Instruments, 2), Clusters, options)
            : InstrumentalVariables.Gmm(new IvDesign(y, Exogenous, 1, Endogenous, 1, Instruments, 2), options));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("is singular", refusal.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("clusters", refusal.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Residuals near 1e-161 leave the unadjusted weight's s² a subnormal whose product with ZᵀZ/n underflows to a
    /// singular block: refused as that, an unadjusted weight being built from no scores (#1786).
    /// </summary>
    [Fact]
    public void Gmm_refuses_an_unadjusted_weight_that_underflows_as_that()
    {
        double[] y = [.. Enumerable.Range(0, Rows).Select(i => 1e-161 * ((((i * 53) % 97) / 97.0) - 0.5))];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(y, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted, GmmWeightType = IvCovarianceType.Unadjusted }));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("s² times ZᵀZ/n falls short of full rank", refusal.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Four clusters for three instruments, two of them on rows the instruments leave zero: their score sums vanish, so
    /// the weight spans two directions, refused naming the design, since the clusters are not too few (#1786).
    /// </summary>
    [Fact]
    public void Gmm_does_not_blame_clusters_enough_for_scores_that_span_too_few_directions()
    {
        double[] exogenous = [.. HadamardExogenous, 0, 0, 0, 0];
        double[] endogenous = [.. HadamardEndogenous.Select((value, i) => value + (i % 3)), 0, 0, 0, 0];
        double[] instruments = [.. HadamardInstruments, 0, 0, 0, 0, 0, 0, 0, 0];
        double[] y = [.. Enumerable.Range(0, 12).Select(i => i < 8 ? 1.0 + exogenous[i] - endogenous[i] + (0.25 * (i % 3)) : 0.5 * (i - 7))];
        int[] clusters = [0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 3, 3];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2),
            clusters,
            new IvOptions { WithIntercept = false, CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered }));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("span fewer directions", refusal.Message, StringComparison.Ordinal);
    }

    private static void AssertZeroSquares(ArgumentException refusal)
    {
        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("squares about their mean sum to zero", refusal.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("clusters", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(PanelCovarianceType.Robust)]
    [InlineData(PanelCovarianceType.Clustered)]
    [InlineData(PanelCovarianceType.Kernel)]
    public void An_exact_panel_fit_has_no_robust_model_test(PanelCovarianceType type)
    {
        var options = new PanelOptions { EntityEffects = true, CovarianceType = type, ClusterEntity = type == PanelCovarianceType.Clustered };

        PanelSummary fit = PanelRegression.FixedEffects(new PanelDesign(new double[Rows], Design, 2, Entities, Periods), options);

        Assert.True(double.IsNaN(fit.RobustModelTest!.Statistic), $"{fit.RobustModelTest.Statistic}");
        Assert.True(double.IsNaN(fit.RobustModelTest.PValue));
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>
    /// The unadjusted covariance is the homoskedastic one, so its Wald test stays the classical F on a fit tens of ulps
    /// from exact, a finite ratio its rounding reads, where residuals exactly zero read <c>+∞</c> in both.
    /// </summary>
    [Fact]
    public void A_near_exact_panel_fit_keeps_the_unadjusted_test_the_classical_f()
    {
        PanelSummary fit = PanelRegression.FixedEffects(
            new PanelDesign(Near, Design, 2, Entities, Periods), new PanelOptions { EntityEffects = true });

        Assert.True(double.IsFinite(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
        Assert.Equal(fit.ModelTest.Statistic, fit.RobustModelTest!.Statistic, Math.Abs(fit.ModelTest.Statistic) * 1e-12);
    }

    // Columns of an 8-point Hadamard matrix: ZᵀZ = 8·I, so each solve is exact and every residual exactly zero.
    private static readonly double[] HadamardExogenous = [1, -1, 1, -1, 1, -1, 1, -1];
    private static readonly double[] HadamardInstruments = [1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, 1];
    private static readonly double[] HadamardEndogenous =
        [.. Enumerable.Range(0, 8).Select(i => HadamardInstruments[2 * i] + (2.0 * HadamardInstruments[(2 * i) + 1]))];
    private static readonly double[] HadamardResponse =
        [.. Enumerable.Range(0, 8).Select(i => 1.0 + HadamardExogenous[i] - (2.0 * HadamardEndogenous[i]))];

    /// <summary>
    /// A constant response has no variance to explain: <c>(TSS − RSS)/RSS</c> read <c>−df_resid</c> off the residuals'
    /// rounding, −98 at 5, as statsmodels does; and its rounded total is rarely zero, so 0.1, 3.7 and 12.34 read main's
    /// −84.65, −843.4 and −86.58. Read on the response itself, each is undefined, as each R² is (Review A passes 23, 25, 26),
    /// and so is each unadjusted Wald test, the same F in another form, which read 2.887 and 3.258 (#1790).
    /// </summary>
    [Theory]
    [InlineData(5.0, 100)]
    [InlineData(0.1, 100)]
    [InlineData(3.7, 1000)]
    [InlineData(12.34, 100)]
    public void A_constant_response_has_no_classical_f_test(double level, int n)
    {
        (double[] x, double[] weights, int[] entities, int[] periods) = Degenerate(n);
        double[] y = [.. Enumerable.Repeat(level, n)];
        var unadjusted = new PanelOptions { CovarianceType = PanelCovarianceType.Unadjusted };
        PanelSummary within = PanelRegression.FixedEffects(
            new PanelDesign(y, x, 1, entities, periods), unadjusted with { EntityEffects = true });

        PanelSummary pooled = PanelRegression.FixedEffects(new PanelDesign(y, x, 1, entities, periods), unadjusted);
        PanelSummary byPeriod = PanelRegression.FixedEffects(
            new PanelDesign(y, x, 1, entities, periods), unadjusted with { TimeEffects = true });
        PanelSummary random = PanelRegression.RandomEffects(new PanelDesign(y, x, 1, entities, periods), unadjusted);

        AssertUndefined(OrdinaryLeastSquares.Fit(x, y, 1));
        AssertUndefined(WeightedLeastSquares.Fit(x, y, weights, 1));
        AssertUndefined(pooled.ModelTest!);
        AssertUndefined(within.ModelTest!);
        AssertUndefined(within.PoolabilityTest!);
        AssertUndefined(random.ModelTest!);
        Assert.All(new[] { pooled, within, byPeriod, random }, AssertUndefined);
        Assert.All(new[] { pooled, within, byPeriod, random }, fit => AssertUndefined(fit.ModelTest!));
        Assert.All(new[] { pooled, within, byPeriod, random }, fit => AssertUndefined(fit.RobustModelTest!));

        double[] instrument = [.. x.Select(v => v * v)];
        double[] endogenous = [.. x.Select((v, i) => v + (0.5 * instrument[i]) + (i % 7))];
        var ivDesign = new IvDesign(y, x, 1, endogenous, 1, instrument, 1);
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(ivDesign);
        Assert.True(double.IsNaN(iv.RSquared) && double.IsNaN(iv.AdjustedRSquared), $"{iv.RSquared} {iv.AdjustedRSquared}");
        AssertUndefined(InstrumentalVariables.TwoStageLeastSquares(
            ivDesign, new IvOptions { CovarianceType = IvCovarianceType.Unadjusted }).ModelTest!);
    }

    /// <summary>
    /// Two group indicators span the constant without holding it, so the IV model test leaves no column out and tests
    /// coefficients near 5: not 0/0, so not the NaN of a response with nothing to explain, though the R² is. The panel's
    /// Wald form at 0.7 also tests an indicator near 0.7 over errors of rounding size, 8.4e31 before #1848, and now reads
    /// NaN where its classical F does, by that issue's decision rather than a 0/0.
    /// </summary>
    [Fact]
    public void An_implied_constant_keeps_the_iv_model_test_and_not_the_panel_wald_form_on_a_constant_response()
    {
        double[] groups = [.. Enumerable.Range(0, Rows).SelectMany(i => new[] { i < Rows / 2 ? 1.0 : 0.0, i < Rows / 2 ? 0.0 : 1.0 })];
        double[] y = [.. Enumerable.Repeat(5.0, Rows)];

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(y, groups, 2, Endogenous, 1, Instruments, 2),
            new IvOptions { WithIntercept = false, CovarianceType = IvCovarianceType.Unadjusted });

        Assert.True(double.IsNaN(fit.RSquared), $"{fit.RSquared}");
        Assert.False(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");

        (_, _, int[] entities, int[] periods) = Degenerate(Rows);
        // At 0.7 the residuals are one rounding apiece, so the classical F is the NaN of nothing to explain, not +∞.
        PanelSummary panel = PanelRegression.FixedEffects(
            new PanelDesign([.. Enumerable.Repeat(0.7, Rows)], groups, 2, entities, periods), new PanelOptions { WithIntercept = false });
        Assert.True(double.IsNaN(panel.RSquared) && double.IsNaN(panel.ModelTest!.Statistic), $"{panel.RSquared} {panel.ModelTest.Statistic}");
        AssertUndefined(panel.RobustModelTest!);
    }

    /// <summary>
    /// A constant alone explains nothing, so its within and overall R² read 0; on entity means bitwise equal the between
    /// total is zero, and its between R², 0 before #1847, reads NaN. <see cref="PanelEdgeGridTests"/> holds the rest.
    /// </summary>
    [Fact]
    public void A_constant_alone_reads_no_between_r_squared_on_equal_entity_means()
    {
        (_, _, int[] entities, int[] periods) = Degenerate(Rows);
        double[] y = EqualMeans(entities, periods);
        Assert.Single(y.Select((v, i) => (v, e: entities[i])).GroupBy(row => row.e).Select(rows => rows.Average(row => row.v)).Distinct());
        PanelSummary alone = PanelRegression.FixedEffects(
            new PanelDesign(y, [.. Enumerable.Repeat(1.0, Rows)], 1, entities, periods),
            new PanelOptions { WithIntercept = false });

        Assert.True(double.IsNaN(alone.RSquaredBetween), $"{alone.RSquaredBetween}");
        Assert.Equal(0.0, alone.RSquaredWithin);
        Assert.Equal(0.0, alone.RSquaredOverall);
        Assert.True(alone.ModelTest is null && alone.RobustModelTest is null && double.IsFinite(alone.RSquared), $"{alone.RSquared}");
    }

    /// <summary>
    /// An entity of three rows leaves the means of <c>y = 0.1</c> an ulp apart, 0.1 and 0.10000000000000002: the response
    /// is constant, so the between fit reads NaN whatever its means' rounding, as at 604602e5, where it read the response
    /// as given: its reading of the means (#1847) must read a constant response first.
    /// </summary>
    [Fact]
    public void A_constant_response_reads_no_between_statistic_whatever_its_means_round_to()
    {
        const int n = 23;
        int[] entities = [.. Enumerable.Range(0, n).Select(i => i < 3 ? 0 : 1 + ((i - 3) / 5))];
        int[] periods = [.. Enumerable.Range(0, n).Select(i => i < 3 ? i : (i - 3) % 5)];
        (double[] x, _, _, _) = Degenerate(n);
        double[] y = [.. Enumerable.Repeat(0.1, n)];
        double[] means = [.. y.Select((v, i) => (v, e: entities[i])).GroupBy(row => row.e).Select(rows => rows.Average(row => row.v))];
        Assert.NotEqual(means[0], means[1]);

        // Beside a prepended constant column, and beside two indicators that span one.
        double[] spanned = [.. Enumerable.Range(0, n).SelectMany(i => new[] { i % 3 == 0 ? 1.0 : 0.0, i % 3 == 0 ? 0.0 : 1.0, x[i] })];
        var unadjusted = new PanelOptions { CovarianceType = PanelCovarianceType.Unadjusted };
        PanelSummary[] fits =
        [
            PanelRegression.Between(new PanelDesign(y, x, 1, entities, periods), unadjusted),
            PanelRegression.Between(new PanelDesign(y, spanned, 3, entities, periods), unadjusted with { WithIntercept = false }),
        ];

        foreach (PanelSummary between in fits)
        {
            AssertUndefined(between);
            AssertUndefined(between.ModelTest!);
            AssertUndefined(between.RobustModelTest!);
        }
    }

    // Integers in each entity, rotated by the entity so they vary within each period: each entity sums 16 exactly,
    // so their means are bitwise equal, 3.2, though the response varies within every entity and every period.
    private static double[] EqualMeans(int[] entities, int[] periods) =>
        [.. periods.Select((t, i) => EqualMeanValues[(t + entities[i]) % 5])];

    private static readonly double[] EqualMeanValues = [1.0, 2.0, 3.0, 4.0, 6.0];

    /// <summary>
    /// The IV unadjusted model test reads NaN exactly where its R² does and a constant column is left out of it: beside
    /// a constant the columns only span it tests coefficients the response loads on, and without one it is uncentred (#1790).
    /// </summary>
    [Fact]
    public void The_unadjusted_iv_model_test_reads_nan_only_beside_a_constant_column()
    {
        double[] groups = [.. Enumerable.Range(0, Rows).SelectMany(i => new[] { i < Rows / 2 ? 1.0 : 0.0, i < Rows / 2 ? 0.0 : 1.0 })];
        var designs = new (double[] Exogenous, int Columns, bool WithIntercept, bool Literal, bool Spanned)[]
        {
            (Exogenous, 1, true, true, false),
            (groups, 2, false, false, true),
            (Exogenous, 1, false, false, false),
        };
        double[][] responses =
        [
            [.. Enumerable.Repeat(5.0, Rows)],
            [.. Enumerable.Repeat(0.1, Rows)],
            [.. Enumerable.Repeat(0.7, Rows)],
            [.. Enumerable.Range(0, Rows).Select(i => 1.0 + Exogenous[i] + (0.1 * (i % 3)))],
        ];

        foreach ((double[] exogenous, int columns, bool withIntercept, bool literal, bool spanned) in designs)
        {
            foreach (double[] y in responses)
            {
                IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
                    new IvDesign(y, exogenous, columns, Endogenous, 1, Instruments, 2),
                    new IvOptions { WithIntercept = withIntercept, CovarianceType = IvCovarianceType.Unadjusted });
                bool flat = y.Distinct().Count() == 1;
                double statistic = fit.ModelTest!.Statistic;
                string where = $"literal {literal}, spanned {spanned}, y[0] {y[0]}: R² {fit.RSquared}, test {statistic}";

                Assert.True(double.IsNaN(fit.RSquared) == ((literal || spanned) && flat), where);
                // +∞ only on residuals exactly zero, whose unadjusted standard errors are zero.
                bool vanished = double.IsPositiveInfinity(statistic) && fit.StandardErrors.All(error => error is 0.0);
                Assert.True(vanished || double.IsNaN(statistic) == (literal && flat), where);
            }
        }
    }

    /// <summary>
    /// One row an entity leaves no within rows, whose total is zero: the within R² is NaN, on one period as on rows
    /// spread over many, where one period read 0 and the other NaN.
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void A_within_r_squared_with_no_within_rows_is_undefined(bool onePeriod)
    {
        const int n = 12;
        double[] x = [.. Enumerable.Range(0, n).Select(i => (double)((i * 7) % 11))];
        double[] y = [.. Enumerable.Range(0, n).Select(i => (2.0 * x[i]) + ((5 * i) % 3))];
        int[] entities = [.. Enumerable.Range(0, n)];
        int[] periods = [.. Enumerable.Range(0, n).Select(i => onePeriod ? 0 : i)];

        PanelSummary fit = PanelRegression.FixedEffects(new PanelDesign(y, x, 1, entities, periods));

        Assert.True(double.IsNaN(fit.RSquaredWithin), $"{fit.RSquaredWithin}");
        Assert.True(double.IsFinite(fit.RSquaredOverall), $"{fit.RSquaredOverall}");
    }

    /// <summary>A constant alone read each panel R² 0 on <c>y = 5</c>, where one regressor more read NaN (#1789).</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void A_constant_alone_reads_no_r_squared_on_a_constant_response(bool timeEffects)
    {
        (_, _, int[] entities, int[] periods) = Degenerate(40);
        double[] ones = [.. Enumerable.Repeat(1.0, 40)];
        double[] y = [.. Enumerable.Repeat(5.0, 40)];

        PanelSummary fit = PanelRegression.FixedEffects(
            new PanelDesign(y, ones, 1, entities, periods), new PanelOptions { WithIntercept = false, TimeEffects = timeEffects });

        AssertUndefined(fit);
    }

    /// <summary>
    /// A zero weight removes its row: 0.1 on the weighted rows and 7 on the others is a constant to the fit, which read F
    /// −95.27 and R² −34.94 on the rows as given (Review A pass 26).
    /// </summary>
    [Fact]
    public void A_response_constant_on_its_weighted_rows_has_no_classical_f_test()
    {
        const int n = 100;
        (double[] x, _, _, _) = Degenerate(n);
        double[] weights = [.. Enumerable.Range(0, n).Select(i => i % 10 == 0 ? 0.0 : 1.0 + (i % 3))];
        double[] y = [.. Enumerable.Range(0, n).Select(i => i % 10 == 0 ? 7.0 : 0.1)];

        AssertUndefined(WeightedLeastSquares.Fit(x, y, weights, 1));
    }

    /// <summary>
    /// A response constant within each entity, <c>1 + e mod 3</c>, leaves entity effects nothing to explain: main read
    /// −36.5 and linearmodels 475.4 off the rounding; at <c>1e10</c> random effects read 16.28 (Review A passes 24, 25).
    /// </summary>
    [Fact]
    public void A_response_its_effects_absorb_has_no_classical_f_test()
    {
        const int n = 100;
        (double[] x, _, int[] entities, int[] periods) = Degenerate(n);
        double[] perEntity = [.. entities.Select(e => 1.0 + (e % 3))];
        var unadjusted = new PanelOptions { CovarianceType = PanelCovarianceType.Unadjusted };

        PanelSummary within = PanelRegression.FixedEffects(
            new PanelDesign(perEntity, x, 1, entities, periods), unadjusted with { EntityEffects = true });
        AssertUndefined(within.ModelTest!);
        Assert.True(double.IsNaN(within.RSquared) && double.IsNaN(within.RSquaredWithin), $"{within.RSquared} {within.RSquaredWithin}");
        Assert.True(double.IsFinite(within.RSquaredOverall), $"{within.RSquaredOverall}");
        AssertUndefined(PanelRegression.FixedEffects(
            new PanelDesign(perEntity, x, 1, entities, periods), unadjusted with { EntityEffects = true, TimeEffects = true }).ModelTest!);
        AssertUndefined(PanelRegression.RandomEffects(
            new PanelDesign([.. Enumerable.Repeat(1e10, n)], x, 1, entities, periods), unadjusted).ModelTest!);
    }

    /// <summary>
    /// Random effects removes a response constant within each entity whole, θ one: on <c>0.1 + 0.3·(e mod 3)</c> over
    /// 8 × 5 it read F −3.9e-15 and R² −2.2e-16 off a θ of <c>1 − 3.6e-15</c>, linearmodels F −12.98 (#1787).
    /// </summary>
    [Fact]
    public void Random_effects_on_a_response_constant_within_each_entity_has_no_classical_f_test()
    {
        (double[] x, _, int[] entities, int[] periods) = Degenerate(40);
        double[] y = [.. entities.Select(e => 0.1 + (0.3 * (e % 3)))];

        PanelSummary fit = PanelRegression.RandomEffects(new PanelDesign(y, x, 1, entities, periods), new PanelOptions());

        AssertUndefined(fit.ModelTest!);
        AssertUndefined(fit.RobustModelTest!);
        Assert.True(double.IsNaN(fit.RSquared) && double.IsNaN(fit.RSquaredWithin), $"{fit.RSquared} {fit.RSquaredWithin}");
    }

    /// <summary>A response one ulp from constant varies: it keeps the ratio its rounding reads, −96.95 here, as main.</summary>
    [Fact]
    public void A_response_that_barely_varies_keeps_its_classical_f_test()
    {
        const int n = 100;
        (double[] x, _, int[] entities, int[] periods) = Degenerate(n);
        double[] y = [.. Enumerable.Range(0, n).Select(i => 5.0 + (1e-15 * ((((i * 53) % 97) / 97.0) - 0.5)))];

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, 1);
        Assert.True(double.IsFinite(fit.FStatistic) && double.IsFinite(fit.RSquared), $"{fit.FStatistic} {fit.RSquared}");
        Assert.True(double.IsFinite(PanelRegression.FixedEffects(
            new PanelDesign(y, x, 1, entities, periods), new PanelOptions { CovarianceType = PanelCovarianceType.Unadjusted }).ModelTest!.Statistic));
    }

    private static (double[] X, double[] Weights, int[] Entities, int[] Periods) Degenerate(int n) => (
        [.. Enumerable.Range(0, n).Select(i => ((i * 37 % 101) / 10.0) + (Math.Sqrt(i + 2) / 7.0))],
        [.. Enumerable.Range(0, n).Select(i => 1.0 + (i % 3))],
        [.. Enumerable.Range(0, n).Select(i => i / 5)],
        [.. Enumerable.Range(0, n).Select(i => i % 5)]);

    private static void AssertUndefined(OlsSummary fit)
    {
        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        Assert.True(double.IsNaN(fit.FPValue), $"{fit.FPValue}");
        Assert.True(double.IsNaN(fit.RSquared) && double.IsNaN(fit.AdjustedRSquared), $"{fit.RSquared} {fit.AdjustedRSquared}");
    }

    private static void AssertUndefined(PanelSummary fit)
    {
        double[] all = [fit.RSquared, fit.RSquaredOverall, fit.RSquaredWithin, fit.RSquaredBetween];
        Assert.All(all, value => Assert.True(double.IsNaN(value), $"{value}"));
    }

    private static void AssertUndefined(WaldTest test)
    {
        Assert.True(double.IsNaN(test.Statistic), $"{test.Statistic}");
        Assert.True(double.IsNaN(test.PValue), $"{test.PValue}");
    }

    /// <summary>
    /// Residuals exactly zero leave every homoskedastic covariance zero, and its tests infinite, as the classical F and
    /// the panel's unadjusted test always were; each t is the coefficient over a zero error (#1779).
    /// </summary>
    [Fact]
    public void Every_homoskedastic_test_is_infinite_on_residuals_exactly_zero()
    {
        OlsSummary ols = OrdinaryLeastSquares.Fit(Design, new double[Rows], 2);
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(HadamardResponse, HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted });
        PanelSummary panel = PanelRegression.FixedEffects(
            new PanelDesign(new double[Rows], Design, 2, Entities, Periods), new PanelOptions { EntityEffects = true });

        Assert.True(double.IsPositiveInfinity(ols.FStatistic), $"{ols.FStatistic}");
        Assert.Equal(0.0, ols.FPValue);
        foreach (WaldTest test in new[] { iv.ModelTest!, iv.FirstStage[0].InstrumentTest, panel.ModelTest!, panel.RobustModelTest! })
        {
            Assert.True(double.IsPositiveInfinity(test.Statistic), $"{test.Statistic}");
            Assert.Equal(0.0, test.PValue);
        }

        Assert.All(iv.StandardErrors, error => Assert.Equal(0.0, error));
        Assert.All(iv.TStatistics, t => Assert.True(double.IsInfinity(t), $"{t}"));
        Assert.All(iv.PValues, p => Assert.Equal(0.0, p));
    }

    /// <summary>
    /// A noisy fit scaled to 1e-170 has residuals and a response whose squares underflow to zero, though they are not zero:
    /// its tests read their arithmetic, NaN, not the <c>+∞</c> of residuals exactly zero a sum-of-squares test gave it
    /// (Review A pass 7).
    /// </summary>
    [Fact]
    public void Residuals_whose_squares_underflow_are_not_exactly_zero()
    {
        double[] y = [.. Enumerable.Range(0, Rows).Select(i => ((2.0 * Design[2 * i]) + (0.1 * ((i % 5) - 2))) * 1e-170)];

        OlsSummary ols = OrdinaryLeastSquares.Fit(Design, y, 2);
        PanelSummary panel = PanelRegression.FixedEffects(
            new PanelDesign(y, Design, 2, Entities, Periods), new PanelOptions { EntityEffects = true });

        Assert.True(double.IsNaN(ols.FStatistic), $"{ols.FStatistic}");
        foreach (WaldTest test in new[] { panel.ModelTest!, panel.RobustModelTest!, panel.PoolabilityTest! })
        {
            Assert.True(double.IsNaN(test.Statistic), $"{test.Statistic}");
        }
    }

    [Fact]
    public void A_robust_covariance_on_residuals_exactly_zero_is_zero_and_its_tests_undefined()
    {
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(HadamardResponse, HadamardExogenous, 1, HadamardEndogenous, 1, HadamardInstruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Robust });

        Assert.True(double.IsNaN(iv.ModelTest!.Statistic), $"{iv.ModelTest.Statistic}");
        Assert.True(double.IsNaN(iv.FirstStage[0].InstrumentTest.Statistic));
        AssertZeroCovariance(iv.StandardErrors, iv.TStatistics, iv.PValues);
    }

    /// <summary>
    /// A row whose leverage is exactly one, its residual exactly zero, weighs 0/0 under HC2 and HC3, as statsmodels'
    /// does: its standard errors are NaN there too (Review A pass 10, Cyril: as Python does).
    /// </summary>
    [Theory]
    [InlineData(CovarianceType.Hc2)]
    [InlineData(CovarianceType.Hc3)]
    public void A_leverage_of_one_weighs_as_statsmodels_weighs_it(CovarianceType type)
    {
        double[] x = [1, 0, 0, 1, 0, 2, 0, 3, 0, 4, 0, 5, 0, 6, 0, 7];
        double[] y = [5, 1.1, 2.3, 2.9, 4.2, 4.8, 6.1, 7.2];

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, 2, new OlsOptions { CovarianceType = type, WithIntercept = false });

        Assert.All(fit.StandardErrors, error => Assert.True(double.IsNaN(error), $"{error}"));

        // Every residual exactly zero too: the 0/0 still reads NaN, as statsmodels', not the exact fit's zero.
        OlsSummary exact = OrdinaryLeastSquares.Fit(
            [1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1], [5, 0, 2, 2, 2, 2], 2, new OlsOptions { CovarianceType = type, WithIntercept = false });

        Assert.Equal(0.0, exact.ResidualStandardError);
        Assert.All(exact.StandardErrors, error => Assert.True(double.IsNaN(error), $"{error}"));
        Assert.True(double.IsNaN(exact.FStatistic), $"{exact.FStatistic}");
    }

    /// <summary>
    /// Forty rows on <c>[0, 1]</c> and one at <c>1e4</c>, <c>R²</c> within 1e-13 of one: the F is statsmodels'
    /// <c>mse_model / mse_resid</c>, where <c>1 − R²</c> read <c>+∞</c> at noise 1e-6 and 4.1e-5 off at 1e-2. Each side's
    /// RSS rounds apart, so the bound is that rounding's, not 8 ulps (Review A pass 12).
    /// </summary>
    [Theory]
    [InlineData(1e-2, 37130860022262.97, 1e-13)]
    [InlineData(1e-6, 3.713085953043895e21, 1e-10)]
    public void The_f_beside_a_far_row_is_statsmodels(double noise, double expected, double relative)
    {
        const int n = 41;
        // CA5394: a seeded Random draws the review's reproducible data; no security use.
#pragma warning disable CA5394
        var random = new Random(7);
        var x = new double[n];
        var y = new double[n];
        for (int i = 0; i < n - 1; i++)
        {
            x[i] = i / 40.0;
            y[i] = 1.0 + (2.0 * x[i]) + (noise * (random.NextDouble() - 0.5));
        }
#pragma warning restore CA5394

        x[n - 1] = 1e4;
        y[n - 1] = 1.0 + 2e4;

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, 1);

        Assert.Equal(expected, fit.FStatistic, expected * relative);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(13)]
    [InlineData(33)]
    public void The_first_value_that_is_not_finite_is_found_wherever_it_lies(int length)
    {
        for (int at = 0; at < length; at++)
        {
            foreach (double bad in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
            {
                double[] values = [.. Enumerable.Range(0, length).Select(i => (i - 3) * 1e300)];
                values[at] = bad;
                if (at + 2 < length)
                {
                    values[at + 2] = double.NaN;
                }

                Assert.Equal(at, FiniteValues.FirstNonFinite(values));
            }
        }

        Assert.Equal(-1, FiniteValues.FirstNonFinite([.. Enumerable.Range(0, length).Select(i => i * (double.MaxValue / 40))]));
    }

    /// <summary>A zero covariance: every standard error zero, and each coefficient's test, a Wald test on it, undefined.</summary>
    private static void AssertZeroCovariance(IReadOnlyList<double> errors, IReadOnlyList<double> t, IReadOnlyList<double> p)
    {
        Assert.All(errors, error => Assert.Equal(0.0, error));
        Assert.All(t, value => Assert.True(double.IsNaN(value), $"{value}"));
        Assert.All(p, value => Assert.True(double.IsNaN(value), $"{value}"));
    }
}
