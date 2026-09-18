using Lodestar.Abstractions;
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

    /// <summary>
    /// The dense fit was the one entry point of the four scalers that answered a statistic for a
    /// value no statistic can answer for: it returned a <c>NaN</c> scale and mean and poisoned
    /// every later transform of that feature, where scikit-learn nan-skips (#1042).
    /// </summary>
    [Fact]
    public void The_dense_fit_refuses_a_non_finite_value()
    {
        double[] withNan = [-1.2216917e9, 1.0, 2.0, 2.0, 3.0, double.NaN, 4.0, 4.0];
        double[] withInfinity = [-1.2216917e9, 1.0, 2.0, 2.0, 3.0, double.PositiveInfinity, 4.0, 4.0];

        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => StandardScaler.Fit(withNan, 2)).ParamName);
        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => StandardScaler.Fit(withInfinity, 2)).ParamName);
    }

    /// <summary>
    /// <c>partial_fit</c> folds a batch into the same statistics, so it refuses the same batch: the
    /// page already promised that these scalers refuse a non-finite value (#1042).
    /// </summary>
    [Fact]
    public void Partial_fit_refuses_a_non_finite_batch()
    {
        StandardScaler fitted = StandardScaler.Fit(TwoByTwo, 2);

        Assert.Throws<ArgumentException>(() => fitted.PartialFit([3.0, double.NaN]));
        Assert.Throws<ArgumentException>(() => fitted.PartialFit([3.0, double.NegativeInfinity]));
    }

    /// <summary>
    /// Refusing on the way in and not on the way through is the same contradiction one step later:
    /// the dense transform answered a <c>NaN</c> where its three neighbours refuse the input.
    /// </summary>
    [Fact]
    public void The_dense_transform_and_its_inverse_refuse_a_non_finite_value()
    {
        StandardScaler fitted = StandardScaler.Fit(TwoByTwo, 2);

        Assert.Throws<ArgumentException>(() => fitted.Transform([1.0, double.NaN]));
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform([1.0, double.PositiveInfinity]));
    }

    /// <summary>
    /// The sparse transform passed both through where <c>MaxAbsScaler</c> and <c>RobustScaler</c>
    /// refuse both, which is the contradiction the equivalence row stated as a fact (#1041).
    /// </summary>
    [Fact]
    public void The_sparse_transform_and_its_inverse_refuse_a_stored_non_finite_value()
    {
        StandardScaler fitted = StandardScaler.Fit(
            new CsrMatrix(2, 2, [1.0, 2.0, 3.0, 4.0], [0, 1, 0, 1], [0, 2, 4]));
        var withNan = new CsrMatrix(2, 2, [1.0, double.NaN], [0, 1], [0, 1, 2]);
        var withInfinity = new CsrMatrix(2, 2, [1.0, double.PositiveInfinity], [0, 1], [0, 1, 2]);

        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => fitted.Transform(withNan)).ParamName);
        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => fitted.Transform(withInfinity)).ParamName);
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform(withNan));
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform(withInfinity));
    }
}
