using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The largest array the shared <c>TableLength</c> bounds every package's refusals by, per runtime (#1614): runtimes before
/// .NET 6 cap an array of elements wider than one byte at <c>0x7FEFFFFF</c>, where .NET 6 and later cap every array at
/// <c>Array.MaxLength</c>; a count between the two was refused on neither and failed to allocate there. Here because
/// every package compiles the same source and Metrics grants its suite the internals, where Abstractions grants none.
/// </summary>
public sealed class TableLengthTests
{
    [Theory]
    [InlineData(".NET Framework 4.8.9290.0", 0x7FEFFFFFL)]
    [InlineData(".NET Core 4.6.26614.01", 0x7FEFFFFFL)]
    [InlineData(".NET Core 3.1.32", 0x7FEFFFFFL)]
    [InlineData(".NET 5.0.17", 0x7FEFFFFFL)]
    [InlineData(".NET 6.0.0-preview.7.21377.19", 0x7FFFFFC7L)]
    [InlineData(".NET 6.0.36", 0x7FFFFFC7L)]
    [InlineData(".NET 10.0.0", 0x7FFFFFC7L)]
    [InlineData(".NET Native 2.2", 0x7FEFFFFFL)]
    [InlineData("Mono 6.12.0.206", 0x7FEFFFFFL)]
    [InlineData(".NET ", 0x7FEFFFFFL)]
    [InlineData("", 0x7FEFFFFFL)]
    [InlineData(null, 0x7FEFFFFFL)]
    public void The_bound_is_the_runtime_s(string? framework, long expected)
    {
        Assert.Equal(expected, Lodestar.Internal.TableLength.Bound(framework));
    }

    [Fact]
    public void This_runtime_reads_array_max_length()
    {
        // The suites run on .NET 10, both the net10.0 build and the netstandard2.0 one its mirror loads.
        Assert.Equal(Array.MaxLength, Lodestar.Internal.TableLength.MaxLength);
        Assert.Equal(0x7FFFFFC7L, Lodestar.Internal.TableLength.MaxByteLength);
    }
}
