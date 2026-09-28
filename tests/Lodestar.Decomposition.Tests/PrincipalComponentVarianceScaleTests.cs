using Xunit;

namespace Lodestar.Decomposition.Tests;

/// <summary>The spectrum at scales whose squares overflow a double, which scikit-learn's PCA reads unchanged (#1255).</summary>
public sealed class PrincipalComponentVarianceScaleTests
{
    // Fifty rows of four columns with distinct spreads, so the four ratios differ.
    private static double[] Sample(double scale)
    {
        var rows = new double[50 * 4];
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                rows[(i * 4) + j] = ((((i * 37) + (j * 11)) % 23) - 11.0) * (4 - j) * scale;
            }
        }

        return rows;
    }

    [Theory]
    [InlineData(1e40)]
    [InlineData(1e150)]
    [InlineData(1e-150)]
    [InlineData(1e100)]
    [InlineData(1e-100)]
    public void The_ratios_do_not_move_with_the_scale(double scale)
    {
        // The stop test used to compare against √(α·β), infinite past 1.8e308, and every pair then
        // passed for orthogonal: [0.577, 0.393, 0.022, 0.008] became [0.317, 0.280, 0.205, 0.199].
        PrincipalComponentVariance reference = PrincipalComponentVariance.Compute(Sample(1.0), 50, 4);
        PrincipalComponentVariance scaled = PrincipalComponentVariance.Compute(Sample(scale), 50, 4);

        for (int k = 0; k < 4; k++)
        {
            Assert.Equal(reference.ExplainedVarianceRatio[k], scaled.ExplainedVarianceRatio[k], 12);
            Assert.Equal(1.0, scaled.ExplainedVariance[k] / (reference.ExplainedVariance[k] * scale * scale), 12);
        }
    }

    [Fact]
    public void A_value_that_is_not_finite_is_refused_as_check_array_refuses_it()
    {
        double[] rows = Sample(1.0);
        rows[5] = double.NaN;

        var error = Assert.Throws<ArgumentException>(() => PrincipalComponentVariance.Compute(rows, 50, 4));
        Assert.StartsWith("Input X contains NaN.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Values_whose_column_means_overflow_are_refused_rather_than_thrown_through()
    {
        // Past 1.8e308 a column sum is infinite; this used to surface as ArithmeticException from Math.Sign.
        double[] rows = Sample(1.0);
        for (int i = 0; i < 50; i++)
        {
            rows[i * 4] = 1.5e308;
        }

        var error = Assert.Throws<ArgumentException>(() => PrincipalComponentVariance.Compute(rows, 50, 4));
        Assert.Contains("too large to centre", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_variance_past_the_largest_double_is_refused_rather_than_thrown_from_the_sweep()
    {
        // Centred values near 1e306 once gave a NaN Gram entry and ArithmeticException from Math.Sign (#1255).
        double[] matrix = [1e306, 1e306, -1e306, 1e306, 0.0, -2e306];

        ArgumentException refused = Assert.Throws<ArgumentException>(() => PrincipalComponentVariance.Compute(matrix, 3, 2));
        Assert.Equal("matrix", refused.ParamName);
    }
}
