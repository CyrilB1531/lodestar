using Lodestar.Internal;
using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The power-iteration condition estimate, held to the spectrum it stands in for (#985).</summary>
public sealed class ConditionEstimateTests
{
    // SonarLint S2245, CA5394: a seeded Random builds reproducible factors; no security use.
#pragma warning disable S2245, CA5394
    [Theory]
    [InlineData(8)]
    [InlineData(40)]
    [InlineData(120)]
    [InlineData(251)]
    public void Of_StaysWithinAFactorOfTwoOfTheSpectrum(int order)
    {
        var random = new Random(985 + order);
        double worst = 0.0;
        for (int trial = 0; trial < 20; trial++)
        {
            (double[] upper, double[] inverse) = Triangle(random, order);
            double estimate = ConditionEstimate.Of(upper, inverse, order);
            double[] singular = JacobiSpectrum.SingularValues(ColumnMajor(upper, order), order, order);
            double exact = singular[0] / singular[order - 1];

            Assert.True(estimate <= exact * 1.000_001, $"estimate {estimate} exceeded the exact {exact}");
            worst = Math.Max(worst, exact / estimate);
        }

        Assert.True(worst <= 1.01, $"the estimate was {worst:F6} times under the spectrum at order {order}");
    }

    /// <summary>A unit-column upper triangle and its inverse, the shape the gate measures.</summary>
    private static (double[] Upper, double[] Inverse) Triangle(Random random, int order)
    {
        var upper = new double[order * order];
        for (int row = 0; row < order; row++)
        {
            for (int column = row; column < order; column++)
            {
                upper[(row * order) + column] = random.NextDouble() - 0.5;
            }

            upper[(row * order) + row] += 1.0;
        }

        // Unit columns, which is what D⁻¹ leaves behind.
        for (int column = 0; column < order; column++)
        {
            double squared = 0.0;
            for (int row = 0; row <= column; row++)
            {
                squared += upper[(row * order) + column] * upper[(row * order) + column];
            }

            double norm = Math.Sqrt(squared);
            for (int row = 0; row <= column; row++)
            {
                upper[(row * order) + column] /= norm;
            }
        }

        return (upper, LeastSquares.InvertUpper(upper, order));
    }

    private static double[] ColumnMajor(double[] upper, int order)
    {
        var block = new double[order * order];
        for (int row = 0; row < order; row++)
        {
            for (int column = row; column < order; column++)
            {
                block[(column * order) + row] = upper[(row * order) + column];
            }
        }

        return block;
    }
#pragma warning restore S2245, CA5394
}
