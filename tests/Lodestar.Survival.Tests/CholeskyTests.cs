using Lodestar.Survival.Internal;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>
/// The p×p factorization the Cox fit solves its Newton step and inverts its information with.
/// </summary>
/// <remarks>
/// No corpus: the expectations are exact rational inverses a reader can check by hand, and the
/// refusals are the two singular cases the Cox fit turns into its separation and collinearity
/// diagnostic.
/// </remarks>
public sealed class CholeskyTests
{
    private const double Tolerance = 1e-12;

    /// <summary><c>[[4, 2], [2, 3]]</c>, whose inverse is <c>[[3, -2], [-2, 4]] / 8</c>.</summary>
    private static readonly double[] TwoByTwo = [4.0, 2.0, 2.0, 3.0];

    /// <summary><c>[[4, 2, 0], [2, 5, 2], [0, 2, 5]]</c>, whose inverse has dyadic entries, exact in binary.</summary>
    private static readonly double[] ThreeByThree = [4.0, 2.0, 0.0, 2.0, 5.0, 2.0, 0.0, 2.0, 5.0];

    [Fact]
    public void The_factor_multiplies_back_to_the_matrix()
    {
        Assert.True(Cholesky.TryFactor(ThreeByThree, 3, out double[] lower));

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                double product = 0.0;
                for (int k = 0; k < 3; k++)
                {
                    product += lower[(row * 3) + k] * lower[(column * 3) + k];
                }

                Assert.Equal(ThreeByThree[(row * 3) + column], product, Tolerance);
            }
        }
    }

    [Fact]
    public void The_factor_is_lower_triangular()
    {
        Assert.True(Cholesky.TryFactor(ThreeByThree, 3, out double[] lower));

        Assert.Equal(0.0, lower[1]);
        Assert.Equal(0.0, lower[2]);
        Assert.Equal(0.0, lower[5]);
    }

    [Fact]
    public void Solve_returns_the_vector_the_matrix_maps_to_the_right_hand_side()
    {
        Assert.True(Cholesky.TryFactor(TwoByTwo, 2, out double[] lower));

        double[] solution = Cholesky.Solve(lower, 2, [8.0, 8.0]);

        // [[3, -2], [-2, 4]] / 8 · [8, 8] = [1, 2].
        Assert.Equal(1.0, solution[0], Tolerance);
        Assert.Equal(2.0, solution[1], Tolerance);
    }

    [Theory]
    [MemberData(nameof(Inverses))]
    public void Inverse_matches_the_exact_rational_inverse(double[] matrix, int order, double[] expected)
    {
        Assert.True(Cholesky.TryFactor(matrix, order, out double[] lower));

        double[] inverse = Cholesky.Inverse(lower, order);

        Assert.Equal(expected.Length, inverse.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], inverse[i], Tolerance);
        }
    }

    public static TheoryData<double[], int, double[]> Inverses() => new()
    {
        { [5.0], 1, [0.2] },
        { TwoByTwo, 2, [0.375, -0.25, -0.25, 0.5] },
        { ThreeByThree, 3, [0.328125, -0.15625, 0.0625, -0.15625, 0.3125, -0.125, 0.0625, -0.125, 0.25] },
    };

    [Fact]
    public void An_indefinite_matrix_is_refused()
    {
        // Eigenvalues 3 and -1: the second pivot is negative.
        Assert.False(Cholesky.TryFactor([1.0, 2.0, 2.0, 1.0], 2, out _));
    }

    [Fact]
    public void A_singular_matrix_is_refused_rather_than_factored_through_rounding()
    {
        // The information of a duplicated covariate: rank one, so the second pivot is zero
        // up to rounding, and a factor built on that residue would invert to nonsense.
        double third = 1.0 / 3.0;
        Assert.False(Cholesky.TryFactor([third, third, third, third], 2, out _));
    }

    [Fact]
    public void A_non_finite_entry_is_refused()
    {
        Assert.False(Cholesky.TryFactor([double.NaN, 0.0, 0.0, 1.0], 2, out _));
    }
}
