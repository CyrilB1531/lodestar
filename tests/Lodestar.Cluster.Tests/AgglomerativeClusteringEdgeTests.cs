using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>The refusals the corpus cannot hold, and the claims the two factories make about each other.</summary>
/// <remarks>
/// Every refusal here is one the reference makes too, probed rather than assumed: one sample, a
/// threshold outside <c>[0, inf)</c>, more clusters than samples.
/// </remarks>
public sealed class AgglomerativeClusteringEdgeTests
{
    private static readonly double[] Line = [0.0, 1.0, 5.0, 6.0, 20.0];

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void A_cluster_count_outside_one_to_the_sample_count_is_refused(int clusterCount)
    {
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => AgglomerativeClustering.Fit(Line, 1, clusterCount));

        Assert.Equal("clusterCount", error.ParamName);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_threshold_outside_zero_to_infinity_is_refused(double threshold)
    {
        // Infinity is refused because the reference refuses it: its range is open at the top.
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => AgglomerativeClustering.FitToThreshold(Line, 1, threshold));

        Assert.Equal("distanceThreshold", error.ParamName);
    }

    [Fact]
    public void A_threshold_of_zero_leaves_every_sample_its_own_cluster()
    {
        AgglomerativeClustering fitted = AgglomerativeClustering.FitToThreshold(Line, 1, 0.0);

        Assert.Equal(5, fitted.ClusterCount);
    }

    [Fact]
    public void One_sample_is_refused_as_the_reference_refuses_it()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => AgglomerativeClustering.Fit([4.0], 1, 1));

        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void A_partial_row_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => AgglomerativeClustering.Fit([0.0, 1.0, 2.0], 2, 1));

        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void An_undefined_linkage_is_refused()
    {
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => AgglomerativeClustering.Fit(Line, 1, 2, (Linkage)99));

        Assert.Equal("linkage", error.ParamName);
    }

    [Theory]
    [InlineData(Linkage.Ward)]
    [InlineData(Linkage.Complete)]
    [InlineData(Linkage.Average)]
    [InlineData(Linkage.Single)]
    public void A_threshold_just_above_a_cut_gives_the_same_labels_as_that_cut(Linkage linkage)
    {
        // The two factories describe one tree: cutting at a count and cutting just above the
        // height that count leaves must agree, or one of them is reading the tree wrongly.
        AgglomerativeClustering byCount = AgglomerativeClustering.Fit(Line, 1, 2, linkage);
        double height = byCount.Distances[byCount.Distances.Count - 2];

        AgglomerativeClustering byHeight =
            AgglomerativeClustering.FitToThreshold(Line, 1, Math.BitIncrement(height), linkage);

        Assert.Equal(byCount.Labels, byHeight.Labels);
    }

    [Fact]
    public void The_whole_tree_is_built_whatever_the_cut()
    {
        AgglomerativeClustering two = AgglomerativeClustering.Fit(Line, 1, 2);
        AgglomerativeClustering four = AgglomerativeClustering.Fit(Line, 1, 4);

        Assert.Equal(8, two.Children.Count);
        Assert.Equal(two.Children, four.Children);
        Assert.Equal(two.Distances, four.Distances);
    }
}
