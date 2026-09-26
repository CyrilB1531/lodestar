using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>What the weighted corpus cannot hold: the refusals, and the claims the overloads make about each other.</summary>
public sealed class WeightedClusterEdgeTests
{
    private static readonly double[] Line = [0.0, 1.0, 2.0, 10.0, 11.0, 12.0];

    [Fact]
    public void Weights_that_are_not_one_finite_value_per_row_are_refused()
    {
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => KMeans.Fit(Line, [1.0, 1.0], 1, 2)).ParamName);
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => KMeans.Fit(Line, [1.0, 1.0, double.NaN, 1.0, 1.0, 1.0], 1, 2)).ParamName);
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => KMeans.Fit(Line, [0.0, 0.0, 0.0, 0.0, 0.0, 0.0], 1, 2)).ParamName);
    }

    /// <summary>The shape is judged before the weights, so a bad shape blames the argument the unweighted fit blames.</summary>
    [Fact]
    public void A_bad_shape_is_refused_before_the_weights_are_read()
    {
        Assert.Equal("featureCount", Assert.Throws<ArgumentOutOfRangeException>(
            () => KMeans.Fit(Line, [1.0], 0, 2)).ParamName);
        Assert.Equal("samples", Assert.Throws<ArgumentException>(
            () => KMeans.Fit([1.0, 2.0, 3.0], [1.0], 2, 1)).ParamName);
    }

    /// <summary>A weight of one on every row is no weighting, so the two overloads agree to the bit.</summary>
    [Fact]
    public void Unit_weights_give_the_unweighted_fit()
    {
        var options = new KMeansOptions { InitialCentres = [0.5, 10.5] };

        KMeans plain = KMeans.Fit(Line, 1, 2, options);
        KMeans weighted = KMeans.Fit(Line, [1.0, 1.0, 1.0, 1.0, 1.0, 1.0], 1, 2, options);

        Assert.Equal(plain.Centres.ToArray(), weighted.Centres.ToArray());
        Assert.Equal(plain.Labels.ToArray(), weighted.Labels.ToArray());
        Assert.Equal(plain.Inertia, weighted.Inertia);
    }

    /// <summary>A row weighing two is the row written twice: the documented meaning of a weight.</summary>
    [Fact]
    public void A_weight_of_two_is_a_duplicated_row()
    {
        var options = new KMeansOptions { InitialCentres = [0.5, 10.5] };

        KMeans weighted = KMeans.Fit(Line, [2.0, 1.0, 1.0, 1.0, 1.0, 1.0], 1, 2, options);
        KMeans duplicated = KMeans.Fit([0.0, 0.0, 1.0, 2.0, 10.0, 11.0, 12.0], 1, 2, options);

        Assert.Equal(duplicated.Centres[0], weighted.Centres[0], 12);
        Assert.Equal(duplicated.Centres[1], weighted.Centres[1], 12);
        Assert.Equal(duplicated.Inertia, weighted.Inertia, 12);
    }

    [Fact]
    public void Starting_sets_beside_a_single_start_or_restarts_are_refused()
    {
        double[][] sets = [[0.5, 10.5]];

        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentreSets = sets, InitialCentres = [0.5, 10.5] }));
        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentreSets = sets, Restarts = 2 }));
        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentreSets = [] }));
        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentres = [0.5, 10.5], Restarts = 2 }));
        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentreSets = [[0.5, 10.5, 3.0]] }));
    }

    [Fact]
    public void Fewer_restarts_than_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => KMeans.Fit(Line, 1, 2, new KMeansOptions { Restarts = 0 }));
    }

    /// <summary>Restarts keep the lowest inertia, so they never do worse than their first draw.</summary>
    [Fact]
    public void Restarts_never_do_worse_than_their_first_draw()
    {
        double[] blobs = [0.0, 0.2, 0.4, 5.0, 5.1, 5.3, 9.0, 9.4, 9.9, 20.0];

        for (int seed = 0; seed < 20; seed++)
        {
            KMeans once = KMeans.Fit(blobs, 1, 3, new KMeansOptions { Seed = seed });
            KMeans many = KMeans.Fit(blobs, 1, 3, new KMeansOptions { Seed = seed, Restarts = 8 });

            Assert.True(many.Inertia <= once.Inertia, $"seed {seed}: {many.Inertia} above {once.Inertia}");
        }
    }

    [Fact]
    public void Dbscan_weights_that_are_not_one_finite_value_per_row_are_refused()
    {
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => Dbscan.Fit(Line, 1, 1.5, 2, [1.0])).ParamName);
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => Dbscan.Fit(Line, 1, 1.5, 2, [1.0, 1.0, 1.0, double.PositiveInfinity, 1.0, 1.0])).ParamName);
        Assert.Equal("sampleWeights", Assert.Throws<ArgumentException>(
            () => Dbscan.FitPrecomputed([0.0, 1.0, 1.0, 0.0], 2, 1.5, 2, [1.0])).ParamName);
    }

    /// <summary>Weighted by ones, both DBSCAN entry points answer as their unweighted forms do.</summary>
    [Fact]
    public void Unit_weights_give_the_unweighted_dbscan()
    {
        double[] ones = [1.0, 1.0, 1.0, 1.0, 1.0, 1.0];
        Dbscan plain = Dbscan.Fit(Line, 1, 1.5, 3);
        Dbscan weighted = Dbscan.Fit(Line, 1, 1.5, 3, ones);

        Assert.Equal(plain.Labels.ToArray(), weighted.Labels.ToArray());
        Assert.Equal(plain.CoreSampleIndices.ToArray(), weighted.CoreSampleIndices.ToArray());
    }
}
