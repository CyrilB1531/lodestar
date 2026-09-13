using Lodestar.Metrics.Internal;
using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The four-stripe walks <c>netstandard2.0</c> takes for <see cref="MeanSquaredError"/>,
/// <see cref="MeanAbsoluteError"/> and <see cref="R2"/>, called directly: on a <c>net10.0</c>
/// runner with SIMD the metrics route around them, so without these the only execution they
/// get is the mirror's.
/// </summary>
/// <remarks>
/// Every value is a multiple of 0.25 below 2^40, so each sum, square and difference here is
/// exact in a double, and the stripes must agree with a single running sum to the bit.
/// </remarks>
public sealed class StripedWalkTests
{
    public static TheoryData<int> Lengths() => [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

    public static TheoryData<int, bool> StripePositions()
    {
        var data = new TheoryData<int, bool>();
        for (int position = 0; position < 8; position++)
        {
            data.Add(position, true);
            data.Add(position, false);
        }
        return data;
    }

    private static double[] Truth(int length) => [.. Enumerable.Range(0, length).Select(i => (i * 1.25) - 3.0)];

    private static double[] Prediction(int length) => [.. Enumerable.Range(0, length).Select(i => (i * 0.5) + 0.75)];

    private static double SequentialSum(IEnumerable<double> terms)
    {
        CompensatedSum sum = default;
        foreach (double term in terms)
        {
            sum.Add(term);
        }
        return sum.Value;
    }

    [Theory]
    [MemberData(nameof(Lengths))]
    public void The_striped_kernel_walk_covers_whole_groups_and_sums_them_exactly(int length)
    {
        double[] yTrue = Truth(length);
        double[] yPred = Prediction(length);

        bool finite = Outputs.TryStripeSum(yTrue, yPred, default(SquaredDifference), out CompensatedSum sum, out int consumed);

        Assert.True(finite);
        Assert.Equal(length - (length % 4), consumed);
        double expected = SequentialSum(Enumerable.Range(0, consumed).Select(i => Square(yTrue[i] - yPred[i])));
        Assert.Equal(expected, sum.Value);
    }

    [Theory]
    [MemberData(nameof(StripePositions))]
    public void The_striped_kernel_walk_sees_a_non_finite_value_in_every_stripe(int position, bool inTruth)
    {
        double[] yTrue = Truth(9);
        double[] yPred = Prediction(9);
        (inTruth ? yTrue : yPred)[position] = position % 2 == 0 ? double.NaN : double.PositiveInfinity;

        Assert.False(Outputs.TryStripeSum(yTrue, yPred, default(SquaredDifference), out _, out _));
    }

    [Fact]
    public void The_striped_kernel_walk_leaves_the_tail_to_its_caller()
    {
        double[] yTrue = Truth(9);
        double[] yPred = Prediction(9);
        yPred[8] = double.NaN;

        Assert.True(Outputs.TryStripeSum(yTrue, yPred, default(SquaredDifference), out _, out int consumed));
        Assert.Equal(8, consumed);
    }

    [Theory]
    [MemberData(nameof(Lengths))]
    public void The_striped_total_covers_whole_groups_and_sums_them_exactly(int length)
    {
        double[] values = Truth(length);

        bool finite = R2.TryStripeTotal(values, out CompensatedSum sum, out int consumed);

        Assert.True(finite);
        Assert.Equal(length - (length % 4), consumed);
        Assert.Equal(SequentialSum(values.Take(consumed)), sum.Value);
    }

    [Theory]
    [MemberData(nameof(StripePositions))]
    public void The_striped_total_sees_a_non_finite_value_in_every_stripe(int position, bool nan)
    {
        double[] values = Truth(9);
        values[position] = nan ? double.NaN : double.NegativeInfinity;

        Assert.False(R2.TryStripeTotal(values, out _, out _));
    }

    [Theory]
    [MemberData(nameof(Lengths))]
    public void The_striped_squares_cover_whole_groups_and_sum_them_exactly(int length)
    {
        double[] yTrue = Truth(length);
        double[] yPred = Prediction(length);
        const double Mean = 1.5;

        bool finite = R2.TryStripeSquares(
            yTrue, yPred, Mean, out CompensatedSum numerator, out CompensatedSum centredSquare, out int consumed);

        Assert.True(finite);
        Assert.Equal(length - (length % 4), consumed);
        Assert.Equal(SequentialSum(Enumerable.Range(0, consumed).Select(i => Square(yTrue[i] - yPred[i]))), numerator.Value);
        Assert.Equal(SequentialSum(yTrue.Take(consumed).Select(t => Square(t - Mean))), centredSquare.Value);
    }

    [Theory]
    [MemberData(nameof(StripePositions))]
    public void The_striped_squares_test_only_the_prediction_in_every_stripe(int position, bool inTruth)
    {
        // The mean pass has already tested the truth, so a non-finite truth here is not this walk's to see.
        double[] yTrue = Truth(9);
        double[] yPred = Prediction(9);
        (inTruth ? yTrue : yPred)[position] = double.NaN;

        bool finite = R2.TryStripeSquares(yTrue, yPred, 0.0, out _, out _, out _);

        Assert.Equal(inTruth, finite);
    }

    [Theory]
    [MemberData(nameof(Lengths))]
    public void The_grouped_finiteness_test_covers_whole_groups(int length)
    {
        double[] values = Truth(length);

        Assert.Equal(0, Inputs.GroupNonFiniteBits(values, out int consumed));
        Assert.Equal(length - (length % 4), consumed);
    }

    [Theory]
    [MemberData(nameof(StripePositions))]
    public void The_grouped_finiteness_test_sees_a_non_finite_value_in_every_position(int position, bool nan)
    {
        double[] values = Truth(9);
        values[position] = nan ? double.NaN : double.PositiveInfinity;

        Assert.NotEqual(0, Inputs.GroupNonFiniteBits(values, out _));
    }

    [Fact]
    public void Four_stripes_keep_what_one_uncompensated_sum_rounds_away()
    {
        // 2^53 + 1 is not representable; each stripe's compensation carries the 1s a plain sum drops.
        StripedCompensatedSum stripes = default;
        stripes.Add(9007199254740992.0, 9007199254740992.0, 9007199254740992.0, 9007199254740992.0);
        stripes.Add(1.0, 1.0, 1.0, 1.0);
        stripes.Add(-9007199254740992.0, -9007199254740992.0, -9007199254740992.0, -9007199254740992.0);

        Assert.Equal(4.0, stripes.Reduce().Value);
    }

    private static double Square(double value) => value * value;

    private readonly struct SquaredDifference : IResidualKernel
    {
        public double Apply(double truth, double prediction) => Square(truth - prediction);
    }
}
