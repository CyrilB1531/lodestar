using Lodestar.Stats.TimeSeries.Internal;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>The two-parameter fit KPSS and the trend extrapolation share.</summary>
public sealed class LineFitTests
{
    [Fact]
    public void An_exact_line_comes_back_exactly()
    {
        (double slope, double intercept) = LineFit.Through([1.0, 2.0, 3.0, 4.0], [5.0, 7.0, 9.0, 11.0]);

        Assert.Equal(2.0, slope, 12);
        Assert.Equal(3.0, intercept, 12);
    }

    [Fact]
    public void One_point_takes_the_minimum_norm_solution_lstsq_returns()
    {
        // np.linalg.lstsq([[3, 1]], [5]) is [1.5, 0.5]: the solution of least norm among the line's many.
        (double slope, double intercept) = LineFit.Through([3.0], [5.0]);

        Assert.Equal(1.5, slope, 12);
        Assert.Equal(0.5, intercept, 12);
    }
}
