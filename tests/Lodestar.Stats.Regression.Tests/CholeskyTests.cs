using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The factor and the whitening <see cref="GeneralizedLeastSquares"/> reads, on matrices small enough to check by hand.</summary>
public sealed class CholeskyTests
{
    private static readonly double[] Matrix = [4.0, 2.0, 0.4, 2.0, 5.0, 1.0, 0.4, 1.0, 3.0];

    [Fact]
    public void The_factor_multiplies_back_to_the_matrix()
    {
        Assert.True(Cholesky.TryFactor(Matrix, 3, out double[] lower));

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                double product = 0.0;
                for (int k = 0; k < 3; k++)
                {
                    product += lower[(i * 3) + k] * lower[(j * 3) + k];
                }

                Assert.Equal(Matrix[(i * 3) + j], product, 1e-14);
            }
        }
    }

    [Fact]
    public void Forward_substitution_inverts_the_factor()
    {
        Assert.True(Cholesky.TryFactor(Matrix, 3, out double[] lower));
        double[] b = [1.0, -2.0, 0.5];
        double[] whitened = [.. b];

        Cholesky.ForwardSubstitute(lower, 3, whitened);

        for (int i = 0; i < 3; i++)
        {
            double back = 0.0;
            for (int k = 0; k <= i; k++)
            {
                back += lower[(i * 3) + k] * whitened[k];
            }

            Assert.Equal(b[i], back, 1e-14);
        }
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(1e-13)]
    public void A_pivot_that_is_not_safely_positive_is_refused(double excess)
    {
        // [[1, 1], [1, 1 + excess]] leaves a last pivot of excess: negative below zero, and at 1e-13 a
        // pivot under the 1e-12 of its diagonal the floor asks for — two rows that are nearly one.
        double[] matrix = [1.0, 1.0, 1.0, excess > 0.1 ? 1.0 - excess : 1.0 + excess];

        Assert.False(Cholesky.TryFactor(matrix, 2, out _));
    }
}
