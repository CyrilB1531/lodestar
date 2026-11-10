using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Internal;
using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>A covariance of too few clusters is singular: its Wald test is defined by rank, or undefined, never noise (#1223, #1269).</summary>
public sealed class SingularWaldTests
{
    // Twelve rows of three regressors, the OLS corpus' own; six clusters of two.
    private static readonly double[] ThreeColumns =
    [
        1.0, 4.0, 0.5, 2.0, 3.0, 1.5, 3.0, 5.0, 2.5, 4.0, 2.0, 0.5,
        5.0, 6.0, 3.5, 6.0, 1.0, 1.0, 7.0, 7.0, 4.5, 8.0, 3.0, 2.0,
        9.0, 8.0, 5.5, 10.0, 4.0, 2.5, 11.0, 9.0, 6.5, 12.0, 5.0, 3.0,
    ];

    private static readonly double[] Ys = [7.2, 9.1, 12.4, 10.8, 16.3, 14.1, 20.7, 18.2, 24.9, 22.4, 29.1, 26.8];
    private static readonly int[] TwoGroups = [0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1];
    private static readonly int[] ThreeGroups = [0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2];

    private static readonly double[] IvY = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
    private static readonly double[] IvExogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
    private static readonly double[] IvEndogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
    private static readonly double[] IvInstruments =
        [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6, 2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];

    private static readonly double[] PanelX =
        [1, 4, 2, 1, 3, 7, 4, 2, 5, 9, 6, 3, 7, 8, 8, 6, 9, 5, 10, 10, 11, 12, 12, 1];

    private static readonly double[] TwoColumns = PanelX;

    private static readonly double[] PanelY =
        [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1, 17.5, 20.3, 22.4, 23.9];
    private static readonly int[] Entities = [0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3];
    private static readonly int[] Periods = [0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2];

    private static readonly double[] ScaledColumns =
    [
        -1.0644279652484553e-05, 0.9880355912095965, -21649.63937675718,
        -2.336012858265085e-06, 0.6918230398972127, -9745.689118322241,
        9.080582112913376e-06, 0.1440110758011814, 133.79453391329906,
        -2.310455197543147e-05, 1.5021237791940034, 4464.264022045747,
        7.85409814017268e-06, 1.078866276716847, 3284.0601350856687,
        8.25984093827465e-06, -0.32425288621618453, 4483.460153383779,
        -9.350709287321009e-06, -1.193481968289436, -13995.946433427242,
        -1.4695789465193707e-05, 0.1832879218351938, -8091.803330370929,
        -6.0311366805394785e-06, -1.0907255817540968, -8175.4682799134835,
        1.6335337956496528e-07, -1.2702564444180398, 21256.15140029774,
        7.893149236809282e-06, 0.957459714054314, -17401.32515240944,
        -2.741301322674649e-06, -1.1621150623980752, 18438.839603435717,
        -7.2933883758220465e-06, -0.5069934332438134, 5845.558140691423,
        1.026125537899345e-05, -0.6921442961542581, -7551.817104588068,
        -8.841439918353255e-06, 0.5629951828746645, 1782.3515248979834,
        -1.4326852069155746e-05, 0.9489929582231348, 12227.101206825595,
        1.1729568837616436e-05, 0.7625587599211745, -5106.833098256287,
        4.238287953445394e-06, -1.7759527584443258, 10467.645263440338,
        -9.806035847923175e-06, -0.1015349700229588, 9391.268603251629,
        1.6338021549367998e-05, -0.46093982861969035, -19789.652935907365,
        1.0935704511937677e-05, 0.0850195223310396, -1608.8073040736315,
        -1.6596846988162615e-05, -2.1290087715153945, -5751.572139278537,
        3.0510610514560347e-06, -1.1249332693248109, -32317.640641722708,
        -3.2029488657875395e-06, -1.2119941813655277, 9483.098137441193,
        8.335229896531512e-06, 0.026384904429302365, -5355.916047016963,
        1.737507713858538e-06, 0.40379042321936165, 26.2353538417719,
        2.79117316964149e-06, -0.12961377535240715, 2545.570954047398,
        2.7132511650790317e-06, -0.12649999388163216, 6559.397158121173,
        -2.9636063164111565e-06, 0.9409461686219084, -2890.599028349924,
        -5.0511304248611115e-06, -0.7182443180234099, -12758.779923134409,
    ];

    private static readonly double[] ScaledY =
    [
        -0.6037746894556996, 0.12660171610511517, 0.12169221076955994, 0.3660450254146868, 1.5113095058759467, 0.5811834350820223,
        0.37109122799551875, -1.1870660446293497, -1.0708234752139085, -0.448030387882177, 0.9813809776812636, -1.4175009253734796,
        0.771598071456196, 0.3178347175784716, -0.834302943393169, -0.4115074245675075, -0.7365537127262187, -0.05065201370908956,
        -0.7027736059747453, -0.43566606238608274, -2.6009332380310775, 0.49723802213969837, 1.1207433968305733, -1.7896891394255192,
        0.01067669584350972, -0.3243948744530942, -1.0015710755577352, -0.40292863749919955, -0.7475598975790667, 1.4454558165517324,
    ];

    private static readonly int[] ScaledGroups = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];

    private static readonly double[] CancellingColumns =
    [
        7.62939453125e-06, 2.288818359375e-05, -393216.0,
        1.52587890625e-05, 7.62939453125e-06, -393216.0,
        2.288818359375e-05, 1.52587890625e-05, -393216.0,
        3.0517578125e-05, 3.814697265625e-05, -262144.0,
        1.52587890625e-05, 7.62939453125e-06, 131072.0,
        7.62939453125e-06, 3.0517578125e-05, 393216.0,
        3.0517578125e-05, 1.52587890625e-05, 393216.0,
        2.288818359375e-05, 2.288818359375e-05, 393216.0,
        0.0, 0.0, 262144.0,
        0.0, 0.0, -131072.0,
        0.0, 0.0, 393216.0,
        0.0, 0.0, 131072.0,
    ];

    private static readonly double[] CancellingY = [7.0, 7.5, 13.9, 10.4, 17.5, 13.4, 22.2, 16.3, 25.9, 22.7, 27.8, 26.7];

    private static readonly double[] FullRankColumns =
    [
        -16428279.849286791, -5.146144938160207e-05, -3.7644358264528645e-07,
        -7609815.358455866, 5.5034411142606426e-05, -4.021627642238352e-06,
        3270732.8162848055, -8.947747489904859e-06, 5.498720059337423e-06,
        -10638449.535401369, 6.93840078805733e-06, 3.501409333268145e-06,
        5848140.59423098, 9.871194935237935e-06, 1.4435226174670133e-07,
        10467557.675074773, 1.1884381530677931e-05, -3.97491493667904e-06,
        -6410758.551318371, 2.45179859989165e-05, 2.513239017678946e-07,
        -6657785.577895957, -4.361728094590078e-05, 3.179139643195101e-07,
        2839168.3874902967, 3.2702811198649106e-05, -3.7197643782044887e-06,
        -1540788.278810197, 2.3954655014946536e-05, 8.660082370397752e-07,
        7791082.077988368, -1.3730010014814147e-05, -6.021592541837571e-06,
        -1149492.0637045773, -4.018238791696381e-05, -3.633270092049563e-07,
        -2439083.824785416, -1.7597334221186508e-05, 1.0077918888388516e-06,
        1224907.9703806918, -7.404721246425896e-06, -6.323989935604193e-06,
    ];

    private static readonly double[] FullRankY = [1.5, 1.7, 0.8, -1.3, 5.7, -0.1, -5.6, 1.8, -1.7, 1.6, 3.2, -3.1, -1.3, 3.9];
    private static readonly int[] FullRankGroups = [0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3];

    private static IvDesign Iv() =>
        new(IvY, IvExogenous, 1, IvEndogenous, 1, IvInstruments, 2);

    private static int Rank(double[] block, int order) => CovarianceRank.Inverse(block, order).Rank;

    // On main this read an inverse of noise, -1.3734698622845465E+20 with a p-value of 1; the rank is one, so the F has one numerator degree of freedom.
    [Fact]
    public void A_cluster_block_short_of_full_rank_reads_positive_and_on_its_rank()
    {
        OlsSummary fit = OrdinaryLeastSquares.Fit(TwoColumns, PanelY, TwoGroups, 2, new OlsOptions { CovarianceType = CovarianceType.Cluster });

        Assert.True(fit.FStatistic > 0.0 && !double.IsInfinity(fit.FStatistic), $"F = {fit.FStatistic}");
#pragma warning disable CS0618 // FisherSf is the name both the published Stats and the working tree export.
        Assert.Equal(Distributions.FisherSf(fit.FStatistic, 1, 1), fit.FPValue, 1e-12);
#pragma warning restore CS0618
    }

    [Fact]
    public void A_weighted_cluster_block_short_of_full_rank_matches_its_ordinary_fit_at_unit_weights()
    {
        var options = new OlsOptions { CovarianceType = CovarianceType.Cluster };
        OlsSummary ordinary = OrdinaryLeastSquares.Fit(ThreeColumns, Ys, ThreeGroups, 3, options);
        OlsSummary weighted = WeightedLeastSquares.Fit(ThreeColumns, Ys, Enumerable.Repeat(1.0, 12).ToArray(), ThreeGroups, 3, options);

        Assert.Equal(ordinary.FStatistic, weighted.FStatistic, 8);
        Assert.Equal(ordinary.FPValue, weighted.FPValue, 8);
        Assert.False(double.IsNaN(ordinary.FStatistic));
    }

    [Fact]
    public void A_block_of_full_rank_keeps_the_inverse_it_always_took()
    {
        double[] block = [4.0, 1.0, 1.0, 3.0];

        Assert.Equal(2, Rank(block, 2));
        Assert.Equal(3, Rank([2, 0, 0, 0, 3, 0, 0, 0, 4], 3));
    }

    [Fact]
    public void The_rank_ignores_the_scale_of_a_column()
    {
        // A variance of 1e-20 beside one of 1e+6: 26 orders apart, and still two independent directions.
        double[] block = [1e-20, 1e-8, 1e-8, 1e+6];

        Assert.Equal(2, Rank(block, 2));
    }

    [Fact]
    public void A_dependent_block_counts_its_independent_directions()
    {
        double[] one = [1.0, 2.0, 2.0, 4.0];
        double[] zero = new double[4];
        double[] twoOfThree = [1.0, 0.0, 1.0, 0.0, 1.0, 1.0, 1.0, 1.0, 2.0];

        Assert.Equal(1, Rank(one, 2));
        Assert.Equal(0, Rank(zero, 2));
        Assert.Equal(2, Rank(twoOfThree, 3));
    }

    [Fact]
    public void A_block_holding_a_value_that_is_not_finite_is_not_given_a_rank()
    {
        Assert.Equal(2, Rank([double.PositiveInfinity, 0.0, 0.0, 1.0], 2));
    }

    [Fact]
    public void A_block_of_full_rank_is_inverted_off_its_rank_test()
    {
        (int rank, double[]? inverse) = CovarianceRank.Inverse([2.0, 0.5, 0.5, 1.0], 2);

        Assert.Equal(2, rank);
        Assert.Equal([1.0 / 1.75, -0.5 / 1.75, -0.5 / 1.75, 2.0 / 1.75], inverse!, (left, right) => Math.Abs(left - right) <= 1e-15);
        Assert.Null(CovarianceRank.Inverse([1.0, 1.0, 1.0, 1.0], 2).Inverse);
    }

    /// <summary>Two clusters of 1e100-sized regressors overflow the meat: no rank describes it, so both slopes stay restrictions.</summary>
    [Fact]
    public void A_cluster_block_that_overflows_keeps_every_restriction()
    {
        double[] x = [.. Enumerable.Range(0, 24).Select(i => 1e100 * (1 + (i * 7 % 5)))];
        double[] y = [.. Enumerable.Range(0, 12).Select(i => 1e110 * (i % 2 == 0 ? 1.0 : -0.5))];
        var options = new OlsOptions { CovarianceType = CovarianceType.Cluster, WithIntercept = false };

        OlsSummary fit = OrdinaryLeastSquares.Fit(x, y, TwoGroups, 2, options);

        Assert.True(double.IsNaN(fit.FStatistic));
        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
    }

    /// <summary>
    /// A cluster whose rows score zero, a regressor row of zeros or a weight of zero, adds nothing to the filling: two of
    /// three clusters span one direction, and counting the third let rounding read two, an F of 2.8e15 (Review A pass 5
    /// on #1223).
    /// </summary>
    [Fact]
    public void A_cluster_scoring_zero_does_not_raise_the_cap()
    {
        (double[] x, double[] y, int[] groups) = ThirdClusterIdle(zeroRows: true);
        (double[] full, _, _) = ThirdClusterIdle(zeroRows: false);
        double[] weights = [.. groups.Select(g => g == 2 ? 0.0 : 1.0)];
        var options = new OlsOptions { CovarianceType = CovarianceType.Cluster, WithIntercept = false };

        OlsSummary zeroRows = OrdinaryLeastSquares.Fit(x, y, groups, 2, options);
        OlsSummary zeroWeights = WeightedLeastSquares.Fit(full, y, weights, groups, 2, options);

        Assert.Equal(1, zeroRows.FNumeratorDegreesOfFreedom);
        Assert.InRange(zeroRows.FStatistic, 0.0, 1e6);
        Assert.Equal(1, zeroWeights.FNumeratorDegreesOfFreedom);
        Assert.InRange(zeroWeights.FStatistic, 0.0, 1e6);
    }

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible design; no security use.
#pragma warning disable S2245, CA5394
    [Fact]
    public void An_instrumental_cluster_scoring_zero_does_not_raise_the_cap()
    {
        var r = new Random(0);
        var y = new double[18];
        var exogenous = new double[18];
        var endogenous = new double[18];
        var instruments = new double[36];
        for (int i = 0; i < 18; i++)
        {
            double live = i / 6 == 2 ? 0.0 : 1.0;
            exogenous[i] = live * r.NextDouble();
            instruments[2 * i] = live * r.NextDouble();
            instruments[(2 * i) + 1] = live * r.NextDouble();
            endogenous[i] = r.NextDouble();
            y[i] = r.NextDouble();
        }

        int[] groups = [.. Enumerable.Range(0, 18).Select(i => i / 6)];
        var options = new IvOptions { CovarianceType = IvCovarianceType.Clustered, WithIntercept = false };

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2), groups, options);

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic));
    }

    /// <summary>
    /// A just-identified clustered weight on as many clusters as instruments spans one direction fewer, its 2SLS scores
    /// summing to zero; rounding lifted the rank of one of 2000 such weights and the fit inverted noise (pass 7 on #1223).
    /// </summary>
    [Fact]
    public void A_just_identified_clustered_weight_is_capped_at_its_clusters_less_one()
    {
        var rng = new Random(7);
        var options = new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered };
        int accepted = 0;
        for (int trial = 0; trial < 2000; trial++)
        {
            int n = 6 + rng.Next(30);
            int exogenous = rng.Next(3);
            int width = 2 + exogenous;
            var y = new double[n];
            var x = new double[n * exogenous];
            var endogenous = new double[n];
            var z = new double[n];
            double scale = Math.Pow(10, rng.Next(-6, 7));
            for (int i = 0; i < n; i++)
            {
                z[i] = rng.NextDouble() * scale;
                for (int j = 0; j < exogenous; j++)
                {
                    x[(i * exogenous) + j] = rng.NextDouble() * Math.Pow(10, rng.Next(-3, 4));
                }

                endogenous[i] = z[i] + (rng.NextDouble() * scale);
                y[i] = (2 * endogenous[i]) + rng.NextDouble();
            }

            int[] groups = [.. Enumerable.Range(0, n).Select(i => i < width ? i : rng.Next(width))];
            try
            {
                InstrumentalVariables.Gmm(new IvDesign(y, x, exogenous, endogenous, 1, z, 1), groups, options);
                accepted++;
            }
            catch (ArgumentException)
            {
                // Refused, as every such weight must be.
            }
        }

        Assert.Equal(0, accepted);
    }

    /// <summary>A second-step weight whose scores overflow is refused for that, naming the design, not as collinear (pass 6).</summary>
    [Fact]
    public void A_gmm_weight_that_overflows_is_refused_as_not_finite()
    {
        var r = new Random(1);
        var y = new double[60];
        var endogenous = new double[60];
        var instruments = new double[120];
        for (int i = 0; i < 60; i++)
        {
            instruments[2 * i] = r.NextDouble();
            instruments[(2 * i) + 1] = r.NextDouble();
            endogenous[i] = instruments[2 * i] + instruments[(2 * i) + 1] + r.NextDouble();
            y[i] = 1e160 * (1.0 + (2.0 * endogenous[i]) + r.NextDouble());
        }

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => InstrumentalVariables.Gmm(new IvDesign(y, [], 0, endogenous, 1, instruments, 2)));

        Assert.Equal("design", error.ParamName);
        Assert.Contains("not finite", error.Message, StringComparison.Ordinal);
    }

    /// <summary>Eighteen rows of two regressors in three clusters of six, the third's regressors zero when asked.</summary>
    private static (double[] X, double[] Y, int[] Groups) ThirdClusterIdle(bool zeroRows)
    {
        var r = new Random(0);
        var x = new double[36];
        var y = new double[18];
        for (int i = 0; i < 18; i++)
        {
            double live = zeroRows && i / 6 == 2 ? 0.0 : 1.0;
            x[2 * i] = live * r.NextDouble();
            x[(2 * i) + 1] = live * r.NextDouble();
            y[i] = r.NextDouble();
        }

        return (x, y, [.. Enumerable.Range(0, 18).Select(i => i / 6)]);
    }
#pragma warning restore S2245, CA5394

    /// <summary>
    /// Three rows carrying weight for three parameters are fitted exactly, so the covariance is zero and the F undefined;
    /// rounding left an F of 5.1e29 (Review A pass 8 on #1223), and statsmodels reads 1.7e28.
    /// </summary>
    [Fact]
    public void A_weighted_fit_that_interpolates_its_rows_has_no_f_test()
    {
        double[] x = [1.0, 4.0, 2.0, 1.0, 3.0, 7.0, 4.0, 2.0, 5.0, 9.0, 6.0, 3.0, 7.0, 8.0, 8.0, 6.0, 9.0, 5.0, 10.0, 10.0];
        double[] y = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1, 17.5, 20.3];
        double[] weights = [1.0, 2.0, 3.0, 0, 0, 0, 0, 0, 0, 0];

        OlsSummary hc0 = WeightedLeastSquares.Fit(x, y, weights, 2, new OlsOptions { CovarianceType = CovarianceType.Hc0 });
        OlsSummary own = WeightedLeastSquares.Fit(
            x, y, weights, [.. Enumerable.Range(0, 10)], 2, new OlsOptions { CovarianceType = CovarianceType.Cluster });

        Assert.True(double.IsNaN(hc0.FStatistic), $"{hc0.FStatistic}");
        Assert.True(double.IsNaN(own.FStatistic), $"{own.FStatistic}");
    }

    [Fact]
    public void An_exact_fit_has_no_f_test_to_read()
    {
        // Four rows of each indicator: XᵀX is 4·I, its factor exactly 2·I, every residual exactly zero; statsmodels answers nan.
        double[] design = [1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1];
        double[] y = [2, 3, 2, 3, 2, 3, 2, 3];
        OlsSummary fit = OrdinaryLeastSquares.Fit(design, y, 2, new OlsOptions { CovarianceType = CovarianceType.Hc0, WithIntercept = false });

        Assert.True(double.IsNaN(fit.FStatistic));
        Assert.True(double.IsNaN(fit.FPValue));
    }

    [Fact]
    public void Instrumental_variables_report_no_model_test_on_a_singular_cluster_block()
    {
        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(Iv(), TwoGroups, new IvOptions { CovarianceType = IvCovarianceType.Clustered });

        Assert.True(double.IsNaN(fit.ModelTest!.Statistic));
        Assert.True(double.IsNaN(fit.ModelTest.PValue));
        Assert.Equal(2, fit.ModelTest.DegreesOfFreedom);
        Assert.All(fit.StandardErrors, e => Assert.True(e > 0.0, $"{e}"));
    }

    /// <summary>
    /// LIML's <c>κ ≠ 1</c> leaves its scores a nonzero total, so two clusters span both tested directions where 2SLS's span
    /// one; linearmodels 7.0's <c>IVLIML</c> reads 607644.2830749521 at <c>κ = 2.436539255675779</c> (#1223).
    /// </summary>
    [Fact]
    public void A_liml_model_test_on_as_many_clusters_as_coefficients_is_defined()
    {
        IvSummary fit = InstrumentalVariables.Liml(Iv(), TwoGroups, new IvOptions { CovarianceType = IvCovarianceType.Clustered });

        Assert.Equal(2.436539255675779, fit.Kappa!.Value, 1e-9);
        Assert.Equal(607644.2830749521, fit.ModelTest!.Statistic, 607644.2830749521 * 1e-9);
        Assert.Equal(2, fit.ModelTest.DegreesOfFreedom);
    }

    /// <summary>
    /// Rounding can lift a block's numerical rank past what its construction allows; the cap keeps the test undefined
    /// rather than an inverse of noise, as a cluster covariance of two clusters on two coefficients is (#1223).
    /// </summary>
    [Fact]
    public void A_rank_past_the_construction_is_capped()
    {
        double[] block = [2.0, 0.5, 0.5, 1.0];

        Assert.True(double.IsNaN(CovarianceRank.QuadraticForm([1.0, 1.0], (double[])block.Clone(), 2, largestRank: 1)));
        Assert.Equal(2.0 / 1.75, CovarianceRank.QuadraticForm([1.0, 1.0], (double[])block.Clone(), 2, largestRank: 2), 12);
    }

    /// <summary>
    /// Slopes at 1e-5, 1 and 1e5 with three clusters: the exact rational covariance has rank two, and its pseudo-inverse's
    /// form over the slopes is 82.27803015180792, an F of 41.13901507590396; statsmodels' pinv of the formed block reads
    /// 0.00588 on one degree of freedom (Review A passes 2 and 3, #1223).
    /// </summary>
    [Fact]
    public void A_reduced_rank_form_is_the_exact_covariances()
    {
        OlsSummary fit = OrdinaryLeastSquares.Fit(ScaledColumns, ScaledY, ScaledGroups, 3, new OlsOptions { CovarianceType = CovarianceType.Cluster });

        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(41.13901507590396, fit.FStatistic, 41.13901507590396 * 1e-10);
    }

    /// <summary>
    /// A slope and six cluster dummies on six clusters of five rows: each dummy's summed score is rounding alone. The exact
    /// covariance has rank one and the F 9.717746159268424, which statsmodels' f_test reads too; a meat factored on its unit
    /// diagonal lifted the dummies' rounding to the slope's scale and read 11.8365 (Review A pass 9, #1223).
    /// </summary>
    [Fact]
    public void Cluster_dummies_leave_the_slopes_form_exact()
    {
        double[] slope =
        [
            0.345584192064786, 0.8216181435011584, 0.33043707618338714, -1.303157231604361, 0.9053558666731177,
            0.4463745723640113, -0.5369532353602852, 0.5811181041963531, 0.36457239618607573, 0.294132496655526,
            0.02842224131579679, 0.5467129866124469, -0.7364540870016669, -0.16290994799305278, -0.48211931267997826,
            0.5988462126346276, 0.03972210748165899, -0.2924567509650886, -0.7819084623568421, -0.2571922406188707,
            0.008142180518343508, -0.2756029052993704, 1.2940638143982073, 1.0067243153057943, -2.7111624789659685,
            -1.8890132459676727, -0.17477209205516195, -0.42219041157635356, 0.2136429974986111, 0.21732193102256359,
        ];
        double[] y =
        [
            2.290630851083441, -0.7012116909417021, -0.2123864690353045, 1.3911929916901498, 1.0993809295384058,
            1.1862506585582673, -0.48248298936760553, -1.0575161187574762, 0.649750942315779, 0.5560803361493105,
            -0.6131409335866759, 0.1901298315256612, 0.1597292767719391, -0.4262065970573038, 0.26067037580779356,
            1.294906133786768, 0.9554472907963151, 0.24747996620314083, 1.1027938406074016, 1.662570833972893,
            1.5249193948257354, 0.24396831996000767, 2.5786841909845446, 1.2019221391858448, 0.723579378805001,
            -0.5162940398612808, 2.3270811571012002, 1.2688413395963427, 0.35807260841488997, 1.294761493544434,
        ];
        var design = new double[30 * 7];
        var groups = new int[30];
        for (int row = 0; row < 30; row++)
        {
            groups[row] = row / 5;
            design[row * 7] = slope[row];
            design[(row * 7) + 1 + (row / 5)] = 1.0;
        }

        OlsSummary fit = OrdinaryLeastSquares.Fit(
            design, y, groups, 7, new OlsOptions { CovarianceType = CovarianceType.Cluster, WithIntercept = false });

        Assert.Equal(1, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(9.717746159268424, fit.FStatistic, 9.717746159268424 * 1e-10);
    }

    /// <summary>
    /// Newey–West on two slopes, one at 2e5, and a dummy the fit passes through, so its scores are rounding: the exact
    /// covariance over the three has rank two and the F 4.8440010951725325, read from the windows of L + 1 scores whose
    /// cross product is the filling; statsmodels' pinv of the formed block reads 4.844001101.
    /// </summary>
    [Fact]
    public void A_hac_form_short_of_full_rank_is_the_exact_covariances()
    {
        double[] design =
        [
        0.1257302210933933, -232503.07746388344, 0.0,
        -0.1321048632913019, -21879.166393254574, 0.0,
        0.6404226504432821, -124591.09472530652, 1.0,
        0.10490011715303971, -73226.73547034516, 0.0,
        -0.535669373161111, -54425.89828573099, 0.0,
        0.36159505490948474, -31630.015636915454, 0.0,
        1.3040000451301372, 41163.053637413286, 0.0,
        0.9470809631292422, 104251.33694426776, 0.0,
        -0.7037352358069926, -12853.466294403426, 0.0,
        -1.2654214710460525, 136646.3470549686, 0.0,
        -0.6232744625373522, -66519.46734866136, 0.0,
        0.0413259793472436, 35151.00700930197, 0.0,
        ];
        double[] y =
        [
            0.9663352921985052, 0.027959866115223622, -0.42328792413216737, -0.8692753176818996, -0.7255605122478946,
            0.40099265092479175, -0.3576181609736673, 0.264364906692908, -0.511092627817974, -0.09186515083721858,
            -0.0969781087623352, 0.3760356987135432,
        ];

        OlsSummary fit = OrdinaryLeastSquares.Fit(
            design, y, 3, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 2, SmallSampleCorrection = false, WithIntercept = false });

        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(4.8440010951725325, fit.FStatistic, 4.8440010951725325 * 1e-10);
    }

    /// <summary>
    /// Newey–West with more lags than rows, 50 on 8: the windows ending between the last row and the last lag each
    /// hold every score, so they merge into one row of that multiplicity; the exact F, on rank two, is
    /// 857119.9914882764.
    /// </summary>
    [Fact]
    public void A_hac_form_with_more_lags_than_rows_merges_the_full_windows()
    {
        double[] design =
        [
            -0.006826779865523179, -10775.250794802987, 0.0,
            1.0461432923049026, 99876.36553170226, 0.0,
            0.7415884212884828, -2194.788627038025, 1.0,
            0.7239565416499906, 49588.006646422175, 0.0,
            1.6187762233340763, -191076.8664176647, 0.0,
            -1.2055581426463289, 14706.416587832766, 0.0,
            -0.6269554710763733, -90694.32512592964, 0.0,
            -1.3206632116051251, 177538.93872461407, 0.0,
        ];
        double[] y =
        [
            0.8834356865260308, 1.472421129410485, 0.3129392481432719, 0.9748405451050888, 1.4672782737215386,
            -0.9471817377437276, -0.8108497710340421, -0.7751043892094325,
        ];

        OlsSummary fit = OrdinaryLeastSquares.Fit(
            design, y, 3, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 50, SmallSampleCorrection = false, WithIntercept = false });

        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(857119.9914882764, fit.FStatistic, 857119.9914882764 * 1e-10);
    }

    /// <summary>
    /// A slope at 1e8 on the first half of the rows, one at 1 on the second and a dummy the fit passes through: the
    /// exact HC0 F, on rank two, is 1.8118215296379343 at any scale of the first, which the rounding bound once read as
    /// undetermined past 5e4 (Review A pass 12).
    /// </summary>
    [Fact]
    public void A_reduced_form_is_the_same_whatever_a_slope_s_scale()
    {
        double[] design =
        [
            -122407267.5713965, 0.0, 0.0,
            99499962.76709397, 0.0, 0.0,
            -132897920.0686309, 0.0, 0.0,
            47801210.2249154, 0.0, 0.0,
            -121587459.95120141, 0.0, 0.0,
            70592790.25963144, 0.0, 0.0,
            153378572.8681451, 0.0, 0.0,
            -10178149.71074989, 0.0, 0.0,
            0.0, 2.5705280509823467, 0.0,
            0.0, -0.7849353784015642, 0.0,
            0.0, 0.09546082324696264, 0.0,
            0.0, -0.3448655969718953, 0.0,
            0.0, 0.2730920054356136, 0.0,
            0.0, 1.0968021236733474, 0.0,
            0.0, -1.789781270473164, 0.0,
            0.0, -1.419697124601525, 1.0,
        ];
        double[] y =
        [
            0.3775881983015979, -0.5132099485894354, -0.06552940501343624, 1.0981847664441085, -1.2928644508468463,
            0.4740857132903602, 0.9824589072499044, -0.2742366218853893, -0.2888019556653029, -1.1383433665031757,
            0.1453134652743637, -0.06215834662536746, 0.6923974214105837, 0.20981612093370408, 0.7017246662002706, -0.17141711102961038,
        ];

        OlsSummary fit = OrdinaryLeastSquares.Fit(
            design, y, 3, new OlsOptions { CovarianceType = CovarianceType.Hc0, WithIntercept = false });

        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(1.8118215296379343, fit.FStatistic, 1.8118215296379343 * 1e-12);
    }

    /// <summary>
    /// Slopes at 2^-17 and 2^17: the exact F is 18.45, but rounding the residuals at one part in 2^52 moves the form by
    /// more than itself, so no digit of it is known in doubles; statsmodels reads 31.66 (Review A pass 3, #1223).
    /// </summary>
    [Fact]
    public void A_form_rounding_leaves_undetermined_is_not_a_number()
    {
        OlsSummary fit = OrdinaryLeastSquares.Fit(
            CancellingColumns, CancellingY, ThreeGroups, 3, new OlsOptions { CovarianceType = CovarianceType.Cluster, WithIntercept = false });

        Assert.Equal(2, fit.FNumeratorDegreesOfFreedom);
        Assert.True(double.IsNaN(fit.FStatistic));
        Assert.True(double.IsNaN(fit.FPValue));
    }

    /// <summary>
    /// A full-rank block of slopes near 1e7, 1e-5 and 1e-6: inverted on its unit diagonal, from the rank test's own
    /// factorisation, it reads the exact F 96.03988603038107, where main's inverse of the raw block was 3e-4 off.
    /// </summary>
    [Fact]
    public void A_badly_scaled_block_of_full_rank_is_inverted_on_its_unit_diagonal()
    {
        OlsSummary fit = OrdinaryLeastSquares.Fit(FullRankColumns, FullRankY, FullRankGroups, 3, new OlsOptions { CovarianceType = CovarianceType.Cluster });

        Assert.Equal(3, fit.FNumeratorDegreesOfFreedom);
        Assert.Equal(96.03988603038107, fit.FStatistic, 96.03988603038107 * 1e-10);
    }

    [Fact]
    public void The_f_tests_numerator_degrees_of_freedom_are_the_slopes_or_the_blocks_rank()
    {
        OlsSummary plain = OrdinaryLeastSquares.Fit(ThreeColumns, Ys, 3);
        OlsSummary clustered = OrdinaryLeastSquares.Fit(TwoColumns, PanelY, TwoGroups, 2, new OlsOptions { CovarianceType = CovarianceType.Cluster });

        Assert.Equal(3, plain.FNumeratorDegreesOfFreedom);
        Assert.Equal(1, clustered.FNumeratorDegreesOfFreedom);
    }

    /// <summary>A just-identified model's first-step scores sum to zero, so as many clusters as instruments leave G − 1 directions.</summary>
    [Fact]
    public void A_just_identified_gmm_weight_on_as_many_clusters_as_instruments_is_refused_naming_clusters()
    {
        double[] instrument = [.. IvInstruments.Where((_, i) => i % 2 == 0)];
        var options = new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered };

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => InstrumentalVariables.Gmm(new IvDesign(IvY, [], 0, IvEndogenous, 1, instrument, 1), TwoGroups, options));
        Assert.Equal("clusters", error.ParamName);
        Assert.Contains("just-identified", error.Message, StringComparison.Ordinal);
        _ = InstrumentalVariables.Gmm(new IvDesign(IvY, [], 0, IvEndogenous, 1, instrument, 1), ThreeGroups, options);
    }

    [Fact]
    public void An_undersized_gmm_weight_is_refused_for_what_it_is()
    {
        var options = new IvOptions { CovarianceType = IvCovarianceType.Clustered, GmmWeightType = IvCovarianceType.Clustered };

        ArgumentException error = Assert.Throws<ArgumentException>(() => InstrumentalVariables.Gmm(Iv(), TwoGroups, options));
        Assert.Contains("fewer clusters than instruments", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_panel_model_test_is_undefined_on_a_singular_cluster_block()
    {
        var design = new PanelDesign(PanelY, PanelX, 2, Entities, Periods);
        var options = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered };

        PanelSummary fit = PanelRegression.FixedEffects(design, TwoGroups, options);

        Assert.True(double.IsNaN(fit.RobustModelTest!.Statistic));
        Assert.True(double.IsNaN(fit.RobustModelTest.PValue));
    }

    /// <summary>
    /// Sub-clusters nested in two entities: the two-way meat is the entities' own, rank one, and the cancelled terms'
    /// rounding must not lift it to the two slopes tested (Review A pass 4 on #1223).
    /// </summary>
    [Fact]
    public void A_panel_model_test_is_undefined_on_two_clusterings_one_nested_in_the_other()
    {
        int[] entities = [0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1];
        int[] periods = [0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5];
        int[] nested = [.. Enumerable.Range(0, 12).Select(i => ((2 - (i % 3)) * 10) + (1 - entities[i]))];
        var options = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered, ClusterEntity = true };

        PanelSummary fit = PanelRegression.FixedEffects(new PanelDesign(PanelY, PanelX, 2, entities, periods), nested, options);

        Assert.True(double.IsNaN(fit.RobustModelTest!.Statistic));
        Assert.True(double.IsNaN(fit.RobustModelTest.PValue));
    }

    [Theory]
    [InlineData(KernelType.Bartlett)]
    [InlineData(KernelType.Parzen)]
    [InlineData(KernelType.QuadraticSpectral)]
    public void An_exact_fit_has_no_automatic_bandwidth(KernelType kernel)
    {
        // y = 2x on an alternating sign: 1/12·12 is one in binary, so every score is exactly zero and γ is 0/0 (#1269).
        double[] x = [.. Enumerable.Range(0, 12).Select(i => i % 2 == 0 ? 1.0 : -1.0)];
        double[] y = [.. x.Select(v => 2.0 * v)];
        var options = new IvOptions { CovarianceType = IvCovarianceType.Kernel, Kernel = kernel, WithIntercept = false };

        ArgumentException error = Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, [], 0, x, 1, x, 1), options));
        Assert.Contains("bandwidth", error.Message, StringComparison.Ordinal);
        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void A_given_bandwidth_answers_the_exact_fit()
    {
        double[] x = [.. Enumerable.Range(0, 12).Select(i => i % 2 == 0 ? 1.0 : -1.0)];
        double[] y = [.. x.Select(v => 2.0 * v)];
        var options = new IvOptions { CovarianceType = IvCovarianceType.Kernel, Bandwidth = 2, WithIntercept = false };

        IvSummary fit = InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, [], 0, x, 1, x, 1), options);

        Assert.Equal(0.0, fit.StandardErrors[0]);
        Assert.Equal(2, fit.Bandwidth);
        Assert.True(double.IsNaN(fit.ModelTest!.Statistic));
    }
}
