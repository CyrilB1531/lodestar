using Lodestar.Internal;
using Xunit;

/// <summary>The shared one-sided Jacobi sweep the rank refusal and the conditioning gate read (#978).</summary>
namespace Lodestar.Stats.Regression.Tests;

public sealed class JacobiSpectrumTests
{
    [Fact]
    public void SingularValues_OfADiagonalBlock_AreItsDiagonalSortedDescending()
    {
        // Column-major 3 x 3 with 2, 5, 1 on the diagonal.
        double[] block = [2, 0, 0, 0, 5, 0, 0, 0, 1];

        double[] values = JacobiSpectrum.SingularValues(block, 3, 3);

        Assert.Equal(5.0, values[0], 12);
        Assert.Equal(2.0, values[1], 12);
        Assert.Equal(1.0, values[2], 12);
    }

    [Fact]
    public void SingularValues_OfASingularTriangle_EndAtZero()
    {
        // Column-major upper triangular [[1, 2], [0, 0]]: rank 1.
        double[] block = [1, 0, 2, 0];

        double[] values = JacobiSpectrum.SingularValues(block, 2, 2);

        Assert.Equal(Math.Sqrt(5.0), values[0], 12);
        Assert.Equal(0.0, values[1], 12);
    }

    [Fact]
    public void SingularValues_MatchNumpy_OnTheIssue978Design()
    {
        // numpy.linalg.svd on [1, x1, x2, x1 - x2] with delta 1e-2, compute_uv=False.
        double[] expected = [2.0361e+01, 1.2732e+00, 4.4662e-02];
        double[] r = [0.3, -1.1, 0.7, 2.2, -0.4, 1.5, -2.0, 0.9];
        var block = new double[8 * 4];
        for (int i = 0; i < 8; i++)
        {
            double x1 = i + 1;
            double x2 = x1 + (1e-2 * r[i]);
            block[i] = 1.0;
            block[8 + i] = x1;
            block[16 + i] = x2;
            block[24 + i] = x1 - x2;
        }

        double[] values = JacobiSpectrum.SingularValues(block, 8, 4);

        for (int k = 0; k < expected.Length; k++)
        {
            Assert.Equal(expected[k], values[k], expected[k] * 1e-4);
        }

        // The fourth is rounding on both sides; only its order of magnitude is meaningful.
        Assert.True(values[3] < 1e-14, $"sigma_min was {values[3]}");
    }
}
