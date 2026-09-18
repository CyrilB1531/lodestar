using Lodestar.Internal;
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

    /// <summary>
    /// A cubic on [4, 5]: <c>U</c>'s diagonal stays within 200, which the old guard read as conditioned, while the
    /// column-scaled design's <c>κ</c> is 3.5e4. Frozen from <c>sm.OLS(y, sm.add_constant(X)).fit()</c>, statsmodels 0.15.0,
    /// on the rows built below; the fraction-exact solution is 4e-14 from these (#870).
    /// </summary>
    [Fact]
    public void An_ill_conditioned_polynomial_matches_statsmodels()
    {
        double[] design = new double[36];
        for (int row = 0; row < 12; row++)
        {
            double x = 4.0 + (row / 11.0);
            design[row * 3] = x;
            design[(row * 3) + 1] = x * x;
            design[(row * 3) + 2] = x * x * x;
        }

        double[] response = [4.78, 4.67, 4.59, 4.62, 4.76, 4.99, 5.25, 5.47, 5.58, 5.58, 5.49, 5.37];
        double[] coefficients = [667.5005128205491, -447.1756512006914, 100.08428571429556, -7.427193732194485];
        double[] errors = [51.77374082737747, 34.68255565443952, 7.724981579902265, 0.5721082934224907];

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 3);

        for (int i = 0; i < 4; i++)
        {
            Assert.True(Math.Abs(summary.Coefficients[i] - coefficients[i]) <= 1e-9 * Math.Abs(coefficients[i]), $"coefficient {i}: {summary.Coefficients[i]:R}");
            Assert.True(Math.Abs(summary.StandardErrors[i] - errors[i]) <= 1e-9 * errors[i], $"standard error {i}: {summary.StandardErrors[i]:R}");
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
    [Fact]
    public void The_in_place_solve_gives_the_bits_of_the_copying_one()
    {
        // IRLS hands its weighted design over column-major and lets the solve overwrite it (#782).
        (double[] expected, double[] expectedInverse) = LeastSquares.SolveByReflections(Design, 8, 2, false, Response);

        double[] columnMajor = LeastSquares.ColumnMajor([.. Design], 8, 2);
        (double[] actual, double[] actualInverse) =
            LeastSquares.SolveByReflectionsInPlace(columnMajor, 8, 2, [.. Response]);

        Assert.Equal(Bits(expected), Bits(actual));
        Assert.Equal(Bits(expectedInverse), Bits(actualInverse));
    }

    [Fact]
    public void The_inner_product_sums_four_running_terms_at_every_length()
    {
        for (int length = 0; length < 23; length++)
        {
            double[] left = [.. Enumerable.Range(0, length).Select(i => Math.Sin(i + 0.5) * 1e3)];
            double[] right = [.. Enumerable.Range(0, length).Select(i => Math.Cos((i * 1.7) + 0.1) / 7.0)];

            double s0 = 0.0, s1 = 0.0, s2 = 0.0, s3 = 0.0;
            int i = 0;
            for (; i + 4 <= length; i += 4)
            {
                s0 += left[i] * right[i];
                s1 += left[i + 1] * right[i + 1];
                s2 += left[i + 2] * right[i + 2];
                s3 += left[i + 3] * right[i + 3];
            }

            double expected = s0 + s1 + s2 + s3;
            for (; i < length; i++)
            {
                expected += left[i] * right[i];
            }

            Assert.Equal(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(Reflections.Dot(left, right)));
        }
    }

    private static long[] Bits(double[] values) => [.. values.Select(BitConverter.DoubleToInt64Bits)];

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible design; no security use.
#pragma warning disable S2245, CA5394
    [Fact]
    public void Solve_OnAWideWellConditionedDesign_AgreesWithTheReflections()
    {
        // p = 250 scores 258 on the Frobenius bound though its true kappa is 1.65, so a design
        // this wide took the reflections before #985. The two paths must still agree (#985).
        const int Rows = 1200;
        const int Features = 250;
        var seeded = new Random(985);
        var design = new double[Rows * Features];
        var response = new double[Rows];
        for (int i = 0; i < design.Length; i++)
        {
            design[i] = (seeded.NextDouble() * 2.0) - 1.0;
        }

        for (int row = 0; row < Rows; row++)
        {
            response[row] = (seeded.NextDouble() * 2.0) - 1.0;
        }

        (double[] fast, _) = LeastSquares.Solve(design, Rows, Features, true, response);
        (double[] reflected, _) = LeastSquares.SolveByReflections(design, Rows, Features, true, response);

        int differing = 0;
        for (int k = 0; k < fast.Length; k++)
        {
            if (BitConverter.DoubleToInt64Bits(fast[k]) != BitConverter.DoubleToInt64Bits(reflected[k]))
            {
                differing++;
            }

            Assert.Equal(reflected[k], fast[k], (Math.Abs(reflected[k]) * 1e-9) + 1e-11);
        }

        // Solve falls back to this very call, so bit-identical would mean it took the reflections
        // after all. 247 of the 251 differ; the intercept is one of the four that agree.
        Assert.True(differing > 200, $"only {differing} of {fast.Length} coefficients differ");
    }
#pragma warning restore S2245, CA5394
}
