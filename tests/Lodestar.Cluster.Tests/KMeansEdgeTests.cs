using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>What the corpus does not reach: the shapes refused, and the drawn-start path.</summary>
public sealed class KMeansEdgeTests
{
    private static readonly double[] Line = [0.0, 1.0, 2.0, 10.0, 11.0, 12.0];

    [Fact]
    public void A_feature_or_cluster_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => KMeans.Fit(Line, 0, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => KMeans.Fit(Line, 1, 0));
    }

    [Fact]
    public void Fewer_iterations_than_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => KMeans.Fit(Line, 1, 2, new KMeansOptions { MaxIterations = 0 }));
    }

    [Fact]
    public void A_span_that_is_not_a_whole_number_of_rows_is_refused()
    {
        Assert.Throws<ArgumentException>(() => KMeans.Fit([1.0, 2.0, 3.0], 2, 1));
        Assert.Throws<ArgumentException>(() => KMeans.Fit([], 2, 1));
    }

    /// <summary>More clusters than samples has no answer, so it is refused rather than padded.</summary>
    [Fact]
    public void More_clusters_than_samples_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => KMeans.Fit(Line, 1, 7));

        Assert.Equal("clusterCount", error.ParamName);
    }

    /// <summary>A starting block of the wrong shape is refused before anything is fitted.</summary>
    [Fact]
    public void Initial_centres_of_the_wrong_shape_are_refused()
    {
        Assert.Throws<ArgumentException>(() => KMeans.Fit(
            Line, 1, 2, new KMeansOptions { InitialCentres = [0.0, 1.0, 2.0] }));
    }

    /// <summary>
    /// With no centres given, k-means++ draws them from this package's own generator. The
    /// seed reproduces a run of Lodestar and nothing else, which is the whole reason the
    /// corpus passes centres instead of seeding — so what is asserted here is repeatability,
    /// never a scikit-learn value.
    /// </summary>
    [Fact]
    public void The_drawn_start_is_repeatable_within_this_library()
    {
        var options = new KMeansOptions { Seed = 7 };

        KMeans first = KMeans.Fit(Line, 1, 2, options);
        KMeans second = KMeans.Fit(Line, 1, 2, options);

        Assert.Equal(first.Labels, second.Labels);
        Assert.Equal(first.Centres, second.Centres);
        Assert.Equal(first.Inertia, second.Inertia);
    }

    /// <summary>Two obvious groups are found whatever the draw, which is what k-means is for.</summary>
    [Fact]
    public void Two_separated_groups_are_found_from_a_drawn_start()
    {
        KMeans model = KMeans.Fit(Line, 1, 2, new KMeansOptions { Seed = 3 });

        // The three low values share a label and the three high ones share the other.
        Assert.Equal(model.Labels[0], model.Labels[1]);
        Assert.Equal(model.Labels[0], model.Labels[2]);
        Assert.Equal(model.Labels[3], model.Labels[4]);
        Assert.NotEqual(model.Labels[0], model.Labels[3]);
        // Each group of three contributes 1 + 0 + 1 around its own mean.
        Assert.Equal(4.0, model.Inertia, 1e-9);
    }

    /// <summary>Unseen rows are assigned without refitting, and the shape rule still applies.</summary>
    [Fact]
    public void Predict_assigns_unseen_rows_and_refuses_a_partial_one()
    {
        KMeans model = KMeans.Fit(Line, 1, 2, new KMeansOptions { InitialCentres = [1.0, 11.0] });

        Assert.Equal([0, 1], model.Predict([0.5, 11.5]));
        Assert.Throws<ArgumentException>(() => model.Predict([]));
    }
}
