using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The two routes <c>LeastSquares.Solve</c> chooses between, held to each other (#782).</summary>
public sealed class LeastSquaresTests
{
    private static readonly double[] Design = [0.5, 2.0, 1.5, 1.0, 2.5, 3.5, 3.0, 2.0, 4.5, 5.0, 5.0, 4.0, 6.5, 6.5, 7.0, 5.5];
    private static readonly double[] Response = [3.1, 4.9, 8.2, 8.8, 12.7, 13.1, 16.9, 17.2];
    private static readonly double[] Weights = [1.0, 2.0, 0.5, 1.5, 1.0, 0.25, 2.0, 1.0];

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void The_normal_equations_agree_with_the_reflections_on_a_well_conditioned_design(bool weighted)
    {
        double[] weights = weighted ? Weights : [];

        (double[] chosen, double[] chosenInverse) = LeastSquares.Solve(Design, 8, 2, true, Response, weights);
        (double[] reflected, double[] reflectedInverse) = LeastSquares.SolveByReflections(Design, 8, 2, true, Response, weights);

        for (int i = 0; i < 3; i++)
        {
            Assert.Equal(reflected[i], chosen[i], 1e-12);

            // R is unique only up to the sign of each row, so the rows' squared norms are what must agree.
            double chosenNorm = 0.0;
            double reflectedNorm = 0.0;
            for (int k = i; k < 3; k++)
            {
                chosenNorm += chosenInverse[(i * 3) + k] * chosenInverse[(i * 3) + k];
                reflectedNorm += reflectedInverse[(i * 3) + k] * reflectedInverse[(i * 3) + k];
            }

            Assert.Equal(reflectedNorm, chosenNorm, 1e-12);
        }
    }

    [Fact]
    public void A_near_collinear_design_is_solved_by_the_reflections()
    {
        // The second column is the first plus a thousandth, which spreads the Gram factor's diagonal past 200.
        double[] design = [1.0, 1.001, 2.0, 2.002, 3.0, 2.999, 4.0, 4.001, 5.0, 5.002, 6.0, 5.999];
        double[] response = [2.1, 4.0, 6.2, 7.9, 10.1, 12.0];

        (double[] chosen, _) = LeastSquares.Solve(design, 6, 2, true, response);
        (double[] reflected, _) = LeastSquares.SolveByReflections(design, 6, 2, true, response);

        Assert.Equal(reflected, chosen);
    }
}
