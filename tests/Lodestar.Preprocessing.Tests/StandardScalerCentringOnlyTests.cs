using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Centring without scaling runs its own row loop, both ways; a signed zero shows the centre is
/// still added when centring is off, as the element-wise version did.
/// </summary>
public sealed class StandardScalerCentringOnlyTests
{
    [Fact]
    public void Centring_alone_moves_each_feature_and_moves_it_back()
    {
        double[] samples = [1.0, 10.0, 3.0, 30.0];
        StandardScaler scaler = StandardScaler.Fit(samples, 2, new StandardScalerOptions { WithStd = false });

        Assert.Equal([-1.0, -10.0, 1.0, 10.0], scaler.Transform(samples));
        Assert.Equal(samples, scaler.InverseTransform([-1.0, -10.0, 1.0, 10.0]));
    }

    [Fact]
    public void With_both_steps_off_a_negative_zero_keeps_its_sign_forward_and_loses_it_back()
    {
        StandardScaler scaler = StandardScaler.Fit(
            [1.0, 2.0], 1, new StandardScalerOptions { WithMean = false, WithStd = false });

        // -0.0 - 0.0 is -0.0, and -0.0 + 0.0 is +0.0.
        Assert.Equal(BitConverter.DoubleToInt64Bits(-0.0), BitConverter.DoubleToInt64Bits(scaler.Transform([-0.0])[0]));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(scaler.InverseTransform([-0.0])[0]));
    }
}
