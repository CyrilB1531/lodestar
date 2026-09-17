using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>What the multinomial logit reduces to at two categories, what a label is, and what it refuses (#788).</summary>
public sealed class MultinomialLogitEdgeTests
{
    private static readonly double[] Design =
        [-1.1, -0.73, -0.78, 0.27, -0.25, 0.13, 0.84, 0.86, 0.48, -0.45, -0.75, -0.81, -0.34, -0.05, -0.97];

    private static readonly int[] Binary = [1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1];

    private static readonly int[] Three = [2, 1, 0, 0, 1, 0, 2, 2, 0, 1, 1, 2, 1, 0, 1];

    [Fact]
    public void Two_categories_are_the_binomial_logit()
    {
        MultinomialLogitSummary multinomial = MultinomialLogit.Fit(Design, Binary, 1);
        GlmSummary binomial = GeneralizedLinearModel.Fit(
            Design, [.. Binary.Select(label => (double)label)], 1, GlmFamily.Binomial);

        // The same maximum, reached two ways: IRLS stops on a deviance change of 1e-8, which leaves its coefficients
        // about 5e-8 from Newton's, so the two agree to that and no closer.
        for (int k = 0; k < 2; k++)
        {
            Assert.Equal(binomial.Coefficients[k], multinomial.Coefficients[0][k], 1e-6);
            Assert.Equal(binomial.StandardErrors[k], multinomial.StandardErrors[0][k], 1e-6);
        }

        Assert.Equal(binomial.LogLikelihood, multinomial.LogLikelihood, 1e-9);
    }

    [Fact]
    public void Relabelling_in_the_same_order_changes_nothing()
    {
        int[] renamed = [.. Three.Select(label => (label * 50) - 17)];

        MultinomialLogitSummary original = MultinomialLogit.Fit(Design, Three, 1);
        MultinomialLogitSummary relabelled = MultinomialLogit.Fit(Design, renamed, 1);

        Assert.Equal([-17, 33, 83], relabelled.Categories);
        Assert.Equal(original.Coefficients[1], relabelled.Coefficients[1]);
        Assert.Equal(original.StandardErrors[0], relabelled.StandardErrors[0]);
        Assert.Equal(original.LikelihoodRatio, relabelled.LikelihoodRatio);
    }

    [Fact]
    public void A_single_category_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => MultinomialLogit.Fit(Design, new int[15], 1));

        Assert.Equal("response", error.ParamName);
    }

    [Fact]
    public void Lengths_that_disagree_are_refused_naming_the_response()
    {
        // The design is a whole number of rows, so the labels are what disagree, as OLS and the GLM name it (#905).
        ArgumentException error = Assert.Throws<ArgumentException>(() => MultinomialLogit.Fit(Design, Binary.AsSpan(0, 10), 1));

        Assert.Equal("response", error.ParamName);
    }

    [Fact]
    public void A_design_that_is_not_a_whole_number_of_rows_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => MultinomialLogit.Fit(Design, Binary, 2));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_perfectly_separated_response_is_refused()
    {
        // The reference returns NaN coefficients here and reports converged; decision 0136.
        double[] design = [-2.0, -1.5, -1.0, -0.5, 0.5, 1.0, 1.5, 2.0];
        int[] separated = [0, 0, 0, 0, 1, 1, 1, 1];

        Exception? error = Record.Exception(() => MultinomialLogit.Fit(design, separated, 1));

        Assert.True(error is ArgumentException or InvalidOperationException, $"got {error?.GetType().Name ?? "no exception"}");
    }

    [Fact]
    public void A_design_that_leaves_no_residual_degree_of_freedom_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => MultinomialLogit.Fit([0.1, 0.4, 0.2, 0.9], [0, 1, 2, 1], 1));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_budget_spent_throws_unless_asked_not_to()
    {
        var once = new MultinomialLogitOptions { MaximumIterations = 1 };

        Assert.Throws<InvalidOperationException>(() => MultinomialLogit.Fit(Design, Three, 1, once));
        MultinomialLogitSummary inspected = MultinomialLogit.Fit(Design, Three, 1, once with { ThrowOnNonConvergence = false });
        Assert.False(inspected.Converged);
        Assert.Equal(1, inspected.Iterations);
    }
}
