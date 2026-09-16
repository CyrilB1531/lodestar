using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>The refusals and the boundaries the frozen corpus cannot reach.</summary>
/// <remarks>
/// A corpus holds what scikit-learn answered; it cannot hold what it refused, because a
/// refusal has no numbers to freeze. These are the other half — and the last two are the
/// claims the two entry points make about each other.
/// </remarks>
public sealed class DbscanEdgeTests
{
    private static readonly double[] TwoBlobs =
        [0.0, 0.0, 0.0, 0.3, 0.3, 0.0, 5.0, 5.0, 5.0, 5.2, 9.0, 9.0];

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_radius_that_is_not_positive_and_finite_is_refused(double epsilon)
    {
        // NaN is the one worth naming: it fails every comparison, so a `<= 0` guard lets it
        // through and every neighbourhood then comes back empty, which reads as sparse data.
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => Dbscan.Fit([0.0, 1.0], 1, epsilon, 2));

        Assert.Equal("epsilon", error.ParamName);
    }

    [Fact]
    public void A_minimum_below_one_is_refused()
    {
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => Dbscan.Fit([0.0, 1.0], 1, 1.0, 0));

        Assert.Equal("minimumSamples", error.ParamName);
    }

    [Fact]
    public void A_partial_row_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Dbscan.Fit([0.0, 1.0, 2.0], 2, 1.0, 2));

        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void An_empty_matrix_is_refused()
    {
        Assert.Throws<ArgumentException>(() => Dbscan.Fit([], 2, 1.0, 2));
    }

    [Fact]
    public void A_distance_matrix_that_is_not_square_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => Dbscan.FitPrecomputed([0.0, 1.0, 1.0, 0.0, 2.0, 2.0], 3, 1.0, 2));

        Assert.Equal("distances", error.ParamName);
    }

    [Fact]
    public void One_sample_is_core_at_one_and_noise_at_two()
    {
        // The neighbourhood of a lone sample holds exactly itself, so this pair is the
        // smallest statement of "min_samples counts the point itself".
        Assert.Equal([0], Dbscan.Fit([4.0], 1, 1.0, 1).Labels);
        Assert.Equal([Dbscan.Noise], Dbscan.Fit([4.0], 1, 1.0, 2).Labels);
    }

    [Fact]
    public void Noise_is_minus_one_rather_than_a_cluster()
    {
        Dbscan fitted = Dbscan.Fit(TwoBlobs, 2, 0.5, 2);

        Assert.Equal(-1, Dbscan.Noise);
        Assert.Equal(2, fitted.ClusterCount);
        Assert.DoesNotContain(Dbscan.Noise, fitted.CoreSampleIndices);
    }

    [Fact]
    public void The_precomputed_path_agrees_with_the_euclidean_one()
    {
        Dbscan dense = Dbscan.Fit(TwoBlobs, 2, 0.5, 2);
        double[] distances = Pairwise(TwoBlobs, 2, 6);

        Dbscan precomputed = Dbscan.FitPrecomputed(distances, 6, 0.5, 2);

        Assert.Equal(dense.Labels, precomputed.Labels);
        Assert.Equal(dense.CoreSampleIndices, precomputed.CoreSampleIndices);
    }

    [Fact]
    public void A_border_sample_joins_whichever_cluster_is_grown_first()
    {
        // The same nine points in two orders: the border at 1.3 takes cluster 0 either way,
        // and cluster 0 is a different group in the two calls. Sorting a matrix changes this.
        double[] leftFirst = [0.0, 0.1, 0.2, 0.3, 1.3, 2.3, 2.4, 2.5, 2.6];
        double[] rightFirst = [2.3, 2.4, 2.5, 2.6, 1.3, 0.0, 0.1, 0.2, 0.3];

        Assert.Equal([0, 0, 0, 0, 0, 1, 1, 1, 1], Dbscan.Fit(leftFirst, 1, 1.0, 4).Labels);
        Assert.Equal([0, 0, 0, 0, 0, 1, 1, 1, 1], Dbscan.Fit(rightFirst, 1, 1.0, 4).Labels);
    }

    private static double[] Pairwise(double[] samples, int featureCount, int sampleCount)
    {
        var distances = new double[sampleCount * sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            for (int other = 0; other < sampleCount; other++)
            {
                double total = 0.0;
                for (int feature = 0; feature < featureCount; feature++)
                {
                    double gap = samples[(row * featureCount) + feature]
                        - samples[(other * featureCount) + feature];
                    total += gap * gap;
                }

                distances[(row * sampleCount) + other] = Math.Sqrt(total);
            }
        }

        return distances;
    }
}
