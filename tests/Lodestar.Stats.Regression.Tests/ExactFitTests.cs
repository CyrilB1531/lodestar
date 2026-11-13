using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Internal;
using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// A fit whose residuals are all zero within their rounding has a zero robust covariance, whose Wald tests are NaN and
/// whose GMM weight is refused; and HC2 and HC3 weigh a leverage of one as statsmodels does (#1779).
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

    public static TheoryData<CovarianceType> Robust => new()
    {
        CovarianceType.Hc0, CovarianceType.Hc1, CovarianceType.Hc2, CovarianceType.Hc3, CovarianceType.Hac, CovarianceType.Cluster,
    };

    private static OlsOptions Options(CovarianceType type) =>
        new() { CovarianceType = type, HacLags = type == CovarianceType.Hac ? 3 : null };

    /// <summary>Main read F = 1.4e36 under HC0 here, p = 0, where the covariance is zero in exact arithmetic.</summary>
    [Theory]
    [MemberData(nameof(Robust))]
    public void An_exact_ordinary_fit_has_no_robust_f_test(CovarianceType type)
    {
        OlsSummary fit = type == CovarianceType.Cluster
            ? OrdinaryLeastSquares.Fit(Design, Exact, Clusters, 2, Options(type))
            : OrdinaryLeastSquares.Fit(Design, Exact, 2, Options(type));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        Assert.True(double.IsNaN(fit.FPValue));
        Assert.Equal(0, fit.FNumeratorDegreesOfFreedom);
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    [Theory]
    [MemberData(nameof(Robust))]
    public void An_exact_weighted_fit_has_no_robust_f_test(CovarianceType type)
    {
        OlsSummary fit = type == CovarianceType.Cluster
            ? WeightedLeastSquares.Fit(Design, Exact, Weights, Clusters, 2, Options(type))
            : WeightedLeastSquares.Fit(Design, Exact, Weights, 2, Options(type));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
    }

    /// <summary>
    /// AR(1) errors at ρ = 0.9 over 200 rows: whitening mixes the rows, so main read each whitened residual against its
    /// whitened row and found HC1's F at 3.5e32; the fit is read on the rows as given (Review A pass 15).
    /// </summary>
    [Theory]
    [InlineData(CovarianceType.Hc0)]
    [InlineData(CovarianceType.Hc1)]
    public void An_exact_generalized_fit_with_correlated_errors_has_no_robust_f_test(CovarianceType type)
    {
        const int n = 200;
        double[] x = [.. Enumerable.Range(0, n).Select(i => (i * 37 % 101) / 10.0)];
        double[] y = [.. x.Select(v => 2.0 + (3.0 * v))];
        double[] sigma = new double[n * n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                sigma[(i * n) + j] = Math.Pow(0.9, Math.Abs(i - j));
            }
        }

        OlsSummary fit = GeneralizedLeastSquares.Fit(x, y, sigma, 1, Options(type));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>
    /// <c>x₂ = x₁ + d·u</c>: through <c>(XᵀX)⁻¹</c>, which squares the condition number, pass 15 kept main's HC1 F of
    /// −2.5e15 at <c>d = 1e-11</c>, and its inversion threw on every generalized fit here; the rows' reflections remove
    /// the span until <c>4nkε·κ·√n</c>, roughly, nears one: 0.95 at 200 rows was still exact (Review A passes 16 to 19).
    /// </summary>
    [Theory]
    [InlineData(1e-7, 50, 0)]
    [InlineData(1e-9, 200, 1)]
    [InlineData(1e-11, 50, 2)]
    [InlineData(1e-10, 200, 4)]
    public void A_near_collinear_exact_fit_is_exact(double d, int n, int seed)
    {
        (double[] x, double[] y) = NearCollinear(d, n, seed);
        double[] identity = new double[n * n];
        double[] correlated = new double[n * n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                identity[(i * n) + j] = i == j ? 1.0 : 0.0;
                correlated[(i * n) + j] = Math.Pow(0.5, Math.Abs(i - j));
            }
        }

        foreach (OlsSummary fit in new[]
        {
            OrdinaryLeastSquares.Fit(x, y, 2, Options(CovarianceType.Hc1)),
            GeneralizedLeastSquares.Fit(x, y, identity, 2, Options(CovarianceType.Hc1)),
            GeneralizedLeastSquares.Fit(x, y, correlated, 2, Options(CovarianceType.Hc1)),
        })
        {
            Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
            AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
        }
    }

    /// <summary>
    /// The IV and panel coefficients come from the normal equations, whose rounding the raw reject took for noise: main and
    /// this work's pass 15 read 2SLS's model test at 4.8e7 and the entity-effects panel's at 5.1e14 (Review A pass 16).
    /// </summary>
    [Fact]
    public void A_near_collinear_exact_normal_equations_fit_is_exact()
    {
        const int n = 50;
        // CA5394: a seeded Random draws the review's reproducible data; no security use.
#pragma warning disable CA5394
        var random = new Random(3);
        var exogenous = new double[n * 2];
        var endogenous = new double[n];
        var instruments = new double[n * 2];
        var y = new double[n];
        for (int i = 0; i < n; i++)
        {
            exogenous[2 * i] = random.NextDouble();
            exogenous[(2 * i) + 1] = exogenous[2 * i] + (1e-4 * random.NextDouble());
            instruments[2 * i] = random.NextDouble();
            instruments[(2 * i) + 1] = random.NextDouble();
            endogenous[i] = instruments[2 * i] + instruments[(2 * i) + 1] + random.NextDouble();
            y[i] = 1.0 + exogenous[2 * i] + (2.0 * exogenous[(2 * i) + 1]) - endogenous[i];
        }

        random = new Random(3);
        const int entities = 40;
        const int periods = 5;
        var panelX = new double[entities * periods * 2];
        var panelY = new double[entities * periods];
        var entity = new int[entities * periods];
        var period = new int[entities * periods];
        for (int i = 0; i < panelY.Length; i++)
        {
            entity[i] = i / periods;
            period[i] = i % periods;
            panelX[2 * i] = random.NextDouble();
            panelX[(2 * i) + 1] = panelX[2 * i] + (1e-4 * random.NextDouble());
            panelY[i] = panelX[2 * i] + (2.0 * panelX[(2 * i) + 1]) + (entity[i] % 3);
            _ = random.NextDouble();
        }
#pragma warning restore CA5394

        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(y, exogenous, 2, endogenous, 1, instruments, 2), new IvOptions { CovarianceType = IvCovarianceType.Robust });
        PanelSummary panel = PanelRegression.FixedEffects(
            new PanelDesign(panelY, panelX, 2, entity, period),
            new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Robust });

        Assert.True(double.IsNaN(iv.ModelTest!.Statistic), $"{iv.ModelTest.Statistic}");
        Assert.True(double.IsNaN(panel.RobustModelTest!.Statistic), $"{panel.RobustModelTest.Statistic}");
    }

    /// <summary>
    /// <c>x = w + 1e-6·z</c> with noise of 1e-14, 1.4 times the rule's bound read in rationals: the span's part of the
    /// residual, <c>κ·ε</c> of the coefficients, rounded the projection by more than the noise, and pass 16 called the fit
    /// exact; the part is taken off before projecting (Review A pass 17).
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void A_near_collinear_instrumental_fit_past_its_rounding_keeps_its_covariance(int seed)
    {
        const int n = 300;
        // CA5394: a seeded Random draws the review's reproducible data; no security use.
#pragma warning disable CA5394
        var random = new Random(seed);
        var exogenous = new double[n];
        var endogenous = new double[n];
        var instrument = new double[n];
        var y = new double[n];
        for (int i = 0; i < n; i++)
        {
            exogenous[i] = random.NextDouble();
            instrument[i] = random.NextDouble();
            endogenous[i] = exogenous[i] + (1e-6 * instrument[i]);
            y[i] = 1.0 + exogenous[i] + (2.0 * endogenous[i]) + (1e-14 * (random.NextDouble() - 0.5));
        }
#pragma warning restore CA5394

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(y, exogenous, 1, endogenous, 1, instrument, 1), new IvOptions { CovarianceType = IvCovarianceType.Robust });

        Assert.All(fit.StandardErrors, error => Assert.True(error > 0.0, $"{error}"));
    }

    private static (double[] X, double[] Y) NearCollinear(double d, int n, int seed)
    {
        // CA5394: a seeded Random draws the review's reproducible data; no security use.
#pragma warning disable CA5394
        var random = new Random(seed);
        var x = new double[n * 2];
        var y = new double[n];
        for (int i = 0; i < n; i++)
        {
            x[2 * i] = random.NextDouble();
            x[(2 * i) + 1] = x[2 * i] + (d * random.NextDouble());
            y[i] = 1.0 + x[2 * i] + (2.0 * x[(2 * i) + 1]);
        }
#pragma warning restore CA5394

        return (x, y);
    }

    /// <summary>
    /// <c>y = x₁</c> leaves the intercept and <c>x₂</c>'s coefficients rounding, so a row with <c>x₁ = 0</c> has a residual
    /// as large as its own terms: bounded by its row it read HC0's F as 2.1e34 and the panel's robust test as 3.9e32, with
    /// entity effects 2.0e32. The coefficients' rounding, in the regressors' span, is removed first (Review A passes 8, 9).
    /// </summary>
    [Fact]
    public void A_fit_whose_rows_cancel_is_exact_at_the_fit_scale()
    {
        double[] y = [.. Enumerable.Range(0, Rows).Select(i => Design[2 * i])];
        double[] fourth = [.. Enumerable.Range(0, Rows).Select(i => (2.0 * Design[2 * i]) - (4.0 * Design[(2 * i) + 1]))];
        double[] instrumented = [.. Enumerable.Range(0, Rows).Select(i => Exogenous[i])];

        OlsSummary ols = OrdinaryLeastSquares.Fit(Design, y, 2, Options(CovarianceType.Hc0));
        var robust = new PanelOptions { CovarianceType = PanelCovarianceType.Robust };
        PanelSummary pooled = PanelRegression.FixedEffects(new PanelDesign(y, Design, 2, Entities, Periods), robust);
        PanelSummary within = PanelRegression.FixedEffects(
            new PanelDesign(y, Design, 2, Entities, Periods), robust with { EntityEffects = true });
        PanelSummary withinFourth = PanelRegression.FixedEffects(
            new PanelDesign(fourth, Design, 2, Entities, Periods), robust with { EntityEffects = true });
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(instrumented, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Robust });

        Assert.True(double.IsNaN(ols.FStatistic), $"{ols.FStatistic}");
        foreach (WaldTest test in new[] { pooled.RobustModelTest!, within.RobustModelTest!, withinFourth.RobustModelTest!, iv.ModelTest! })
        {
            Assert.True(double.IsNaN(test.Statistic), $"{test.Statistic}");
        }
    }

    /// <summary>
    /// Forty rows on <c>[0, 1]</c> and one far row on <c>y = 1 + 2x</c>: exact, the small rows' residuals are the far row's
    /// rounding carried through the coefficients, a few 1e-12; with noise seventy times that, or 1e-6 beside a row at 1e10,
    /// a bound at the fit's largest row, two hundred times theirs, still called them exact (Review A pass 9).
    /// </summary>
    [Theory]
    [InlineData(1e6, 0.0, true)]
    [InlineData(1e6, 1e-9, false)]
    [InlineData(1e10, 0.0, true)]
    [InlineData(1e10, 1e-6, false)]
    public void A_far_row_holds_each_residual_to_its_own_rounding(double far, double noise, bool exact)
    {
        const int n = 41;
        double[] x = [.. Enumerable.Range(0, n).Select(i => i < n - 1 ? i / 40.0 : far)];
        double[] y = [.. Enumerable.Range(0, n).Select(i => 1.0 + (2.0 * x[i]) + (i < n - 1 ? noise * ((i % 7) - 3) : 0.0))];

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, 1, Options(CovarianceType.Hc1));

        Assert.Equal(exact, double.IsNaN(fit.FStatistic));
        Assert.Equal(exact, fit.StandardErrors.All(error => error == 0.0));
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
    public void An_exact_instrumental_fit_has_no_model_test(IvCovarianceType type)
    {
        var options = new IvOptions { CovarianceType = type, Bandwidth = type == IvCovarianceType.Kernel ? 2 : null };

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2), options);

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
        Assert.True(double.IsNaN(fit.ModelTest.PValue));
        Assert.True(double.IsFinite(fit.FirstStage[0].InstrumentTest.Statistic));
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>
    /// The unadjusted covariance is <c>s²</c> times a fixed matrix, the homoskedastic one: it reads the rounding on an
    /// exact fit, as OLS's non-robust and the panel's unadjusted covariances do, so its test stays a ratio, not NaN.
    /// </summary>
    [Fact]
    public void An_exact_instrumental_fit_keeps_the_unadjusted_covariance()
    {
        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted });

        Assert.True(fit.ModelTest!.Statistic > 1e20, $"{fit.ModelTest.Statistic}");
        Assert.All(fit.StandardErrors, error => Assert.True(error > 0.0, $"{error}"));
    }

    [Fact]
    public void An_exact_clustered_instrumental_fit_has_no_model_test()
    {
        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            Clusters,
            new IvOptions { CovarianceType = IvCovarianceType.Clustered });

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
    }

    /// <summary>Main chose a bandwidth of 39 from the rounding's autocovariances.</summary>
    [Fact]
    public void An_exact_fit_has_no_automatic_kernel_bandwidth()
    {
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Kernel }));

        Assert.Equal("options", refusal.ParamName);
    }

    [Fact]
    public void Gmm_refuses_an_exact_first_step_naming_the_design()
    {
        ArgumentException plain = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2)));
        ArgumentException clustered = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            Clusters,
            new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered }));

        Assert.Equal("design", plain.ParamName);
        Assert.Equal("design", clustered.ParamName);
        Assert.Contains("first-step fit is exact", plain.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// An unadjusted weight, <c>s²</c> times <c>ZᵀZ/n</c>, only rescales 2SLS's: it is not zero on an exact first step, so
    /// GMM accepts it and reads the rounding, as main does (J = 74.490014785701 there and here, Review A pass 3).
    /// </summary>
    [Fact]
    public void Gmm_accepts_an_unadjusted_weight_on_an_exact_first_step()
    {
        IvSummary fit = InstrumentalVariables.Gmm(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Unadjusted, GmmWeightType = IvCovarianceType.Unadjusted });

        Assert.True(fit.ModelTest!.Statistic > 1e20, $"{fit.ModelTest.Statistic}");
        Assert.Equal(74.490014785701, fit.Overidentification!.Statistic, 74.490014785701 * 1e-9);
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

    /// <summary>On an exact second step a robust reported covariance is zero, as the <c>k</c>-class fits'.</summary>
    [Fact]
    public void Gmm_reports_a_zero_robust_covariance_on_an_exact_second_step()
    {
        IvSummary fit = InstrumentalVariables.Gmm(
            new IvDesign(IvExact, Exogenous, 1, Endogenous, 1, Instruments, 2),
            new IvOptions { CovarianceType = IvCovarianceType.Robust, GmmWeightType = IvCovarianceType.Unadjusted });

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic), $"{fit.ModelTest.Statistic}");
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    [Theory]
    [InlineData(PanelCovarianceType.Robust)]
    [InlineData(PanelCovarianceType.Clustered)]
    [InlineData(PanelCovarianceType.Kernel)]
    public void An_exact_panel_fit_has_no_robust_model_test(PanelCovarianceType type)
    {
        var options = new PanelOptions { EntityEffects = true, CovarianceType = type, ClusterEntity = type == PanelCovarianceType.Clustered };

        PanelSummary fit = PanelRegression.FixedEffects(new PanelDesign(Exact, Design, 2, Entities, Periods), options);

        Assert.True(double.IsNaN(fit.RobustModelTest!.Statistic), $"{fit.RobustModelTest.Statistic}");
        Assert.True(double.IsNaN(fit.RobustModelTest.PValue));
        AssertZeroCovariance(fit.StandardErrors, fit.TStatistics, fit.PValues);
    }

    /// <summary>The unadjusted covariance is the homoskedastic one, so its Wald test stays the classical F, as on main.</summary>
    [Fact]
    public void An_exact_panel_fit_keeps_the_unadjusted_test_the_classical_f()
    {
        PanelSummary fit = PanelRegression.FixedEffects(
            new PanelDesign(Exact, Design, 2, Entities, Periods), new PanelOptions { EntityEffects = true });

        Assert.Equal(fit.ModelTest!.Statistic, fit.RobustModelTest!.Statistic, Math.Abs(fit.ModelTest.Statistic) * 1e-12);
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
    /// −84.65, −843.4 and −86.58. Read on the response itself, each is undefined, as each R² is (Review A passes 23, 25, 26).
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
        PanelSummary random = PanelRegression.RandomEffects(new PanelDesign(y, x, 1, entities, periods), unadjusted);

        AssertUndefined(OrdinaryLeastSquares.Fit(x, y, 1));
        AssertUndefined(WeightedLeastSquares.Fit(x, y, weights, 1));
        AssertUndefined(pooled.ModelTest!);
        AssertUndefined(within.ModelTest!);
        AssertUndefined(within.PoolabilityTest!);
        AssertUndefined(random.ModelTest!);
        Assert.All(new[] { pooled, within, random }, AssertUndefined);

        double[] instrument = [.. x.Select(v => v * v)];
        double[] endogenous = [.. x.Select((v, i) => v + (0.5 * instrument[i]) + (i % 7))];
        IvSummary iv = InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, x, 1, endogenous, 1, instrument, 1));
        Assert.True(double.IsNaN(iv.RSquared) && double.IsNaN(iv.AdjustedRSquared), $"{iv.RSquared} {iv.AdjustedRSquared}");
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
    }

    /// <summary>
    /// <c>y = Σ(j + 1)·xⱼ</c>, eight regressors uniform on <c>[0, 1]</c>, 2,000 rows: the raw residual holds the coefficients'
    /// rounding times <c>X</c>, 1e-12 past a quick bound of 3.2e-13, and read HC1's F as 3.2e28 (Review A pass 11).
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void A_long_exact_fit_is_exact(int seed)
    {
        const int n = 2000;
        const int k = 8;
        // CA5394: a seeded Random draws the review's reproducible data; no security use.
#pragma warning disable CA5394
        var random = new Random(seed);
        var x = new double[n * k];
        var y = new double[n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < k; j++)
            {
                x[(i * k) + j] = random.NextDouble();
                y[i] += (j + 1) * x[(i * k) + j];
            }
        }
#pragma warning restore CA5394

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, k, Options(CovarianceType.Hc1));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
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

    /// <summary>
    /// <c>x₂ = c·x₁ + (i mod 3)</c>: the rows' rounding, carried through the nearly collinear span with signs that do not
    /// cancel, reached 5.35 times a bound that projected the rows' bounds with theirs, at <c>c = 30</c> (Review A pass 10).
    /// </summary>
    [Theory]
    [InlineData(30.0)]
    [InlineData(1000.0)]
    public void A_collinear_exact_fit_is_exact(double c)
    {
        double[] x = [.. Enumerable.Range(0, Rows).SelectMany(i => new[] { (double)i, (c * i) + (i % 3) })];
        double[] y = [.. Enumerable.Range(0, Rows).Select(i => (3.0 * x[2 * i]) - (2.0 * x[(2 * i) + 1]) + 5.0)];

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, 2, Options(CovarianceType.Hc0));

        Assert.True(double.IsNaN(fit.FStatistic), $"{fit.FStatistic}");
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
