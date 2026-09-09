using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// What the corpus does not reach: the shapes that are refused, and the two contracts a
/// caller reads off the API rather than off a frozen number.
/// </summary>
public sealed class StandardScalerEdgeTests
{
    private static readonly double[] TwoByTwo = [1.0, 2.0, 3.0, 4.0];

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => StandardScaler.Fit(TwoByTwo, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => StandardScaler.Fit(TwoByTwo, -1));
    }

    /// <summary>A partial row is not a matrix, and neither is an empty span.</summary>
    [Fact]
    public void A_span_that_is_not_a_whole_number_of_rows_is_refused()
    {
        Assert.Throws<ArgumentException>(() => StandardScaler.Fit([1.0, 2.0, 3.0], 2));
        Assert.Throws<ArgumentException>(() => StandardScaler.Fit([], 2));
    }

    /// <summary>The same rule applies on the way out, not only on the way in.</summary>
    [Fact]
    public void Transforming_a_shape_the_scaler_was_not_fitted_for_is_refused()
    {
        StandardScaler scaler = StandardScaler.Fit(TwoByTwo, 2);

        Assert.Throws<ArgumentException>(() => scaler.Transform([1.0, 2.0, 3.0]));
        Assert.Throws<ArgumentException>(() => scaler.InverseTransform([]));
    }

    /// <summary>
    /// The asymmetry a reader would not predict, and the reason <c>Mean</c> is nullable
    /// rather than always present: <c>with_mean=False</c> still computes a mean, and only
    /// turning both steps off drops it.
    /// </summary>
    [Fact]
    public void With_mean_off_still_reports_a_mean_and_still_does_not_subtract_it()
    {
        StandardScaler scaler = StandardScaler.Fit(
            TwoByTwo, 2, new StandardScalerOptions { WithMean = false, WithStd = true });

        Assert.NotNull(scaler.Mean);
        Assert.Equal(2.0, scaler.Mean[0], 1e-12);

        // Centring is off, so the first column keeps its own sign and magnitude rather
        // than being moved to zero mean.
        double[] transformed = scaler.Transform(TwoByTwo);
        Assert.Equal(1.0 / scaler.Scale![0], transformed[0], 1e-12);
    }

    /// <summary>With both steps off, nothing is reported and nothing is done.</summary>
    [Fact]
    public void With_both_steps_off_the_transform_is_the_identity()
    {
        StandardScaler scaler = StandardScaler.Fit(
            TwoByTwo, 2, new StandardScalerOptions { WithMean = false, WithStd = false });

        Assert.Null(scaler.Mean);
        Assert.Null(scaler.Variance);
        Assert.Null(scaler.Scale);
        Assert.Equal(TwoByTwo, scaler.Transform(TwoByTwo));
    }

    /// <summary>
    /// A feature forced to scale by 1 cannot be inverted back to its spread, because the
    /// step threw that spread away rather than recording it. Stated on the page and pinned
    /// here so the claim is not only prose.
    /// </summary>
    [Fact]
    public void A_constant_feature_does_not_round_trip_to_its_own_spread()
    {
        double[] constant = [5.0, 5.0, 5.0];
        StandardScaler scaler = StandardScaler.Fit(constant, 1);

        Assert.Equal(1.0, scaler.Scale![0]);
        Assert.Equal([0.0, 0.0, 0.0], scaler.Transform(constant));
        Assert.Equal(constant, scaler.InverseTransform(scaler.Transform(constant)));
    }
}
