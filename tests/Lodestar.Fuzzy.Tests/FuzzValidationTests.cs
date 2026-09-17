using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

/// <summary>
/// <see cref="Fuzz.TokenSortRatio"/> and <see cref="Fuzz.PartialTokenSortRatio"/>, then the two
/// token-set ratios (#891), used to reach <c>string.Split</c> before checking either argument, so
/// a null string surfaced as a bare <see cref="NullReferenceException"/> instead of the
/// <see cref="ArgumentNullException"/> every other public method in <see cref="Fuzz"/> already throws.
/// </summary>
public sealed class FuzzValidationTests
{
    [Fact]
    public void TokenSortRatio_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Fuzz.TokenSortRatio(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Fuzz.TokenSortRatio("x", null!));
    }

    [Fact]
    public void PartialTokenSortRatio_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Fuzz.PartialTokenSortRatio(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Fuzz.PartialTokenSortRatio("x", null!));
    }

    [Fact]
    public void TokenSetRatio_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Fuzz.TokenSetRatio(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Fuzz.TokenSetRatio("x", null!));
    }

    [Fact]
    public void PartialTokenSetRatio_NullArgument_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Fuzz.PartialTokenSetRatio(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => Fuzz.PartialTokenSetRatio("x", null!));
    }
}
