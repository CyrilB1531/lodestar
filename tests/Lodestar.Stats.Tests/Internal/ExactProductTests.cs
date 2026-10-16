using Lodestar.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// The residual of a product, against exact rationals: a fused multiply-add on net10.0, and on the
/// netstandard2.0 mirror Dekker's split, its operands traded or scaled where a split or the product overflows, and up near
/// the subnormals.
/// </summary>
public sealed class ExactProductTests
{
    [Theory]
    [InlineData(1.0, 3.0, 1.0 / 3.0, 5.551115123125783e-17)]
    [InlineData(7.0, 10.0, 0.7, 4.440892098500626e-16)]
    [InlineData(1e300, 3e300, 1.0 / 3.0, 5.551115123125783e+283)]
    [InlineData(5e299, 1e300, 0.5000000000000001, -1.1102230246251566e+284)]
    [InlineData(1.0, 1e-300, 1.5e300, -0.5000000000000001)]
    [InlineData(2e300, 1.5, 1.4e300, -1e299)]
    [InlineData(2e-290, 1e-145, 2.0000000000000004e-145, -2.3266887309075252e-306)]
    [InlineData(1.7e308, 1.34e154, 1.34e154, -9.560000000000009e+306)]
    [InlineData(1.7e308, 1.35e154, 1.35e154, -1.2250000000000014e+307)]
    [InlineData(1.0, 1e200, 1e200, double.NegativeInfinity)]
    [InlineData(3e-10, 1.5e300, 2e-310, 8.987737089600162e-25)]
    public void The_residual_is_exact(double minuend, double left, double right, double residual)
    {
        // Python's fractions.Fraction(minuend) - Fraction(left) * Fraction(right), rounded once to a double.
        Assert.Equal(residual, ExactProduct.Residual(minuend, left, right));
    }

    [Fact]
    public void An_infinite_operand_answers_as_the_plain_difference()
    {
        // Scaling an infinity on netstandard2.0 recursed until the stack overflowed (#1690's review).
        Assert.Equal(double.NegativeInfinity, ExactProduct.Residual(1.0, double.PositiveInfinity, 1.0));
    }
}
