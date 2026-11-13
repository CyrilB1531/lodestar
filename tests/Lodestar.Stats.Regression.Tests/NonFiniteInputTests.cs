using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>A NaN or infinity is refused with the cell that holds it, by every fit in the package (#1225, #1268).</summary>
public sealed class NonFiniteInputTests
{
    private const string Design = "design";
    private const string Response = "response";

    private static readonly double[] Xs =
        [1, 4, 2, 1, 3, 7, 4, 2, 5, 9, 6, 3, 7, 8, 8, 6, 9, 5, 10, 10, 11, 12, 12, 1];
    private static readonly double[] Ys =
        [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1, 17.5, 20.3, 22.4, 23.9];
    private static readonly int[] Groups = [0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3];
    private static readonly int[] Labels = [0, 0, 1, 2, 0, 1, 2, 0, 1, 2, 2, 1];
    private static readonly double[] Weights = [1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2];

    private static readonly double[] IvY = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
    private static readonly double[] IvExogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
    private static readonly double[] IvEndogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
    private static readonly double[] IvInstruments =
        [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6, 2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];

    private static readonly int[] Entities = [0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3];
    private static readonly int[] Periods = [0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2];

    public static TheoryData<double> Bad => new() { double.NaN, double.PositiveInfinity, double.NegativeInfinity };

    private static double[] Corrupted(double[] values, int index, double bad)
    {
        var copy = (double[])values.Clone();
        copy[index] = bad;
        return copy;
    }

    private static void AssertRefused(Action call, string parameter, string contains)
    {
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(call);
        Assert.Equal(parameter, error.ParamName);
        Assert.Contains(contains, error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(Bad))]
    public void Least_squares_refuse_a_bad_regressor_naming_the_cell(double bad)
    {
        double[] design = Corrupted(Xs, 7, bad);
        var cluster = new OlsOptions { CovarianceType = CovarianceType.Cluster };
        double[] covariance = new double[12 * 12];
        for (int i = 0; i < 12; i++)
        {
            covariance[(i * 12) + i] = 1.0;
        }

        const string Cell = "design[7], row 3 column 1,";
        AssertRefused(() => OrdinaryLeastSquares.Fit(design, Ys, 2), Design, Cell);
        AssertRefused(() => OrdinaryLeastSquares.Fit(design, Ys, Groups, 2, cluster), Design, Cell);
        AssertRefused(() => OrdinaryLeastSquares.Estimate(design, Ys, 2), Design, Cell);
        AssertRefused(() => WeightedLeastSquares.Fit(design, Ys, Weights, 2), Design, Cell);
        AssertRefused(() => WeightedLeastSquares.Fit(design, Ys, Weights, Groups, 2, cluster), Design, Cell);
        AssertRefused(() => GeneralizedLeastSquares.Fit(design, Ys, covariance, 2), Design, Cell);
        AssertRefused(() => GeneralizedLinearModel.Fit(design, Ys.Select(v => Math.Round(v)).ToArray(), 2, GlmFamily.Poisson), Design, Cell);
        AssertRefused(() => GeneralizedLinearModel.Fit(design, Ys.Select(v => Math.Round(v)).ToArray(), default, default, 2, GlmFamily.Poisson), Design, Cell);
        AssertRefused(() => MultinomialLogit.Fit(design, Labels, 2), Design, Cell);
    }

    [Theory]
    [MemberData(nameof(Bad))]
    public void Least_squares_refuse_a_bad_response_naming_the_cell(double bad)
    {
        double[] y = Corrupted(Ys, 5, bad);
        var cluster = new OlsOptions { CovarianceType = CovarianceType.Cluster };
        double[] covariance = new double[12 * 12];
        for (int i = 0; i < 12; i++)
        {
            covariance[(i * 12) + i] = 1.0;
        }

        const string Cell = "response[5]";
        AssertRefused(() => OrdinaryLeastSquares.Fit(Xs, y, 2), Response, Cell);
        AssertRefused(() => OrdinaryLeastSquares.Fit(Xs, y, Groups, 2, cluster), Response, Cell);
        AssertRefused(() => OrdinaryLeastSquares.Estimate(Xs, y, 2), Response, Cell);
        AssertRefused(() => WeightedLeastSquares.Fit(Xs, y, Weights, 2), Response, Cell);
        AssertRefused(() => WeightedLeastSquares.Fit(Xs, y, Weights, Groups, 2, cluster), Response, Cell);
        AssertRefused(() => GeneralizedLeastSquares.Fit(Xs, y, covariance, 2), Response, Cell);
    }

    [Fact]
    public void A_glm_response_keeps_its_family_refusal()
    {
        double[] y = Corrupted(Ys.Select(v => Math.Round(v)).ToArray(), 2, double.NaN);

        ArgumentException error = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(Xs, y, 2, GlmFamily.Poisson));
        Assert.Equal(Response, error.ParamName);
        Assert.Contains("Poisson", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_shape_that_is_wrong_is_refused_before_a_bad_value_is_read()
    {
        Assert.Throws<ArgumentException>(() => OrdinaryLeastSquares.Fit([1.0, double.NaN, 2.0], [1.0], 2));
    }

    /// <summary>Every block's length is judged before any value is read, the labels, weights, covariance and offset too (pass 7).</summary>
    [Fact]
    public void Every_length_is_refused_before_a_bad_value_is_read()
    {
        double[] design = Corrupted(Xs, 4, double.NaN);
        var cluster = new OlsOptions { CovarianceType = CovarianceType.Cluster };
        double[] counts = [.. Ys.Select(v => Math.Round(v))];

        Assert.Equal("clusters", Assert.Throws<ArgumentException>(() => OrdinaryLeastSquares.Fit(design, Ys, [0, 1], 2, cluster)).ParamName);
        Assert.Equal("weights", Assert.Throws<ArgumentException>(() => WeightedLeastSquares.Fit(design, Ys, [1.0, 2.0], 2)).ParamName);
        Assert.Equal("clusters", Assert.Throws<ArgumentException>(() => WeightedLeastSquares.Fit(design, Ys, Weights, [0, 1], 2, cluster)).ParamName);
        Assert.Equal("covariance", Assert.Throws<ArgumentException>(() => GeneralizedLeastSquares.Fit(design, Ys, [1.0, 0.0, 0.0, 1.0], 2)).ParamName);
        Assert.Equal("offset", Assert.Throws<ArgumentException>(
            () => GeneralizedLinearModel.Fit(design, counts, [1.0], default, 2, GlmFamily.Poisson)).ParamName);
        double[] ivY = Corrupted(IvY, 3, double.NaN);
        int[] short11 = [.. Groups.Take(11)];
        Assert.Equal("clusters", Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(ivY, IvExogenous, 1, IvEndogenous, 1, IvInstruments, 2),
            short11,
            new IvOptions { CovarianceType = IvCovarianceType.Clustered })).ParamName);
    }

    [Theory]
    [MemberData(nameof(Bad))]
    public void Instrumental_variables_name_the_block_that_holds_a_bad_value(double bad)
    {
        var clustered = new IvOptions { CovarianceType = IvCovarianceType.Clustered };
        var cases = new (string Cell, double[] Y, double[] Exogenous, double[] Endogenous, double[] Instruments)[]
        {
            ("design.Response[4]", Corrupted(IvY, 4, bad), IvExogenous, IvEndogenous, IvInstruments),
            ("design.Exogenous[1]", IvY, Corrupted(IvExogenous, 1, bad), IvEndogenous, IvInstruments),
            ("design.Endogenous[9]", IvY, IvExogenous, Corrupted(IvEndogenous, 9, bad), IvInstruments),
            ("design.Instruments[3], row 1 column 1,", IvY, IvExogenous, IvEndogenous, Corrupted(IvInstruments, 3, bad)),
        };

        foreach ((string cell, double[] y, double[] exogenous, double[] endogenous, double[] instruments) in cases)
        {
            AssertRefused(() => InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2)), Design, cell);
            AssertRefused(() => InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2), Groups, clustered), Design, cell);
            AssertRefused(() => InstrumentalVariables.Liml(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2)), Design, cell);
            AssertRefused(() => InstrumentalVariables.Liml(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2), Groups, clustered), Design, cell);
            AssertRefused(() => InstrumentalVariables.Gmm(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2)), Design, cell);
            AssertRefused(() => InstrumentalVariables.Gmm(new IvDesign(y, exogenous, 1, endogenous, 1, instruments, 2), Groups, clustered), Design, cell);
        }
    }

    [Theory]
    [MemberData(nameof(Bad))]
    public void Panel_regression_names_the_block_that_holds_a_bad_value(double bad)
    {
        var clustered = new PanelOptions { CovarianceType = PanelCovarianceType.Clustered };
        var first = new PanelOptions { WithIntercept = false };
        var firstClustered = new PanelOptions { WithIntercept = false, CovarianceType = PanelCovarianceType.Clustered };
        var cases = new (string Cell, double[] Y, double[] X)[]
        {
            ("design.Response[8]", Corrupted(Ys, 8, bad), Xs),
            ("design.Exogenous[5], row 2 column 1,", Ys, Corrupted(Xs, 5, bad)),
        };

        foreach ((string cell, double[] y, double[] x) in cases)
        {
            AssertRefused(() => PanelRegression.FixedEffects(new PanelDesign(y, x, 2, Entities, Periods)), Design, cell);
            AssertRefused(() => PanelRegression.FixedEffects(new PanelDesign(y, x, 2, Entities, Periods), Groups, clustered), Design, cell);
            AssertRefused(() => PanelRegression.Between(new PanelDesign(y, x, 2, Entities, Periods)), Design, cell);
            AssertRefused(() => PanelRegression.Between(new PanelDesign(y, x, 2, Entities, Periods), Groups, clustered), Design, cell);
            AssertRefused(() => PanelRegression.FirstDifference(new PanelDesign(y, x, 2, Entities, Periods), first), Design, cell);
            AssertRefused(() => PanelRegression.FirstDifference(new PanelDesign(y, x, 2, Entities, Periods), Groups, firstClustered), Design, cell);
            AssertRefused(() => PanelRegression.RandomEffects(new PanelDesign(y, x, 2, Entities, Periods)), Design, cell);
            AssertRefused(() => PanelRegression.RandomEffects(new PanelDesign(y, x, 2, Entities, Periods), Groups, clustered), Design, cell);
        }
    }

    /// <summary>A NaN is refused before an option no value could have made valid, through every overload (#1225).</summary>
    [Fact]
    public void A_bad_value_is_refused_before_an_option_the_fit_does_not_read()
    {
        double[] y = Corrupted(Ys, 2, double.NaN);
        var parzen = new PanelOptions { Kernel = KernelType.Parzen };
        var intercept = new PanelOptions { WithIntercept = true };
        double[] counts = [.. Ys.Select(v => Math.Round(v))];
        var inverse = new GlmOptions { Link = GlmLink.Inverse };

        AssertRefused(() => PanelRegression.FixedEffects(new PanelDesign(y, Xs, 2, Entities, Periods), parzen), Design, "design.Response[2]");
        AssertRefused(() => PanelRegression.FirstDifference(new PanelDesign(y, Xs, 2, Entities, Periods), intercept), Design, "design.Response[2]");
        AssertRefused(() => GeneralizedLinearModel.Fit(Corrupted(Xs, 1, double.NaN), counts, 2, GlmFamily.Poisson, inverse), Design, "design[1]");
        AssertRefused(() => GeneralizedLinearModel.Fit(Corrupted(Xs, 1, double.NaN), counts, default, default, 2, GlmFamily.Poisson, inverse), Design, "design[1]");
    }

    /// <summary>Under-identification is a shape, judged from the counts, and refused before any value is read (#1225).</summary>
    [Fact]
    public void An_under_identified_model_is_refused_before_a_bad_value_is_read()
    {
        double[] endogenous = [.. IvEndogenous.SelectMany(v => new[] { v, -v })];
        double[] y = Corrupted(IvY, 3, double.NaN);
        double[] instruments = [.. IvInstruments.Where((_, i) => i % 2 == 0)];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => InstrumentalVariables.TwoStageLeastSquares(new IvDesign(y, IvExogenous, 1, endogenous, 2, instruments, 1)));
        Assert.Contains("under-identifies", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_cell_is_named_whichever_row_it_sits_in()
    {
        AssertRefused(() => OrdinaryLeastSquares.Fit(Corrupted(Xs, 0, double.NaN), Ys, 2), Design, "design[0], row 0 column 0,");
        AssertRefused(() => OrdinaryLeastSquares.Fit(Corrupted(Xs, 23, double.NaN), Ys, 2), Design, "design[23], row 11 column 1,");
    }
}
