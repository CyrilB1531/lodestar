using System.Reflection;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>The Review B findings of <c>Lodestar.Abstractions</c> after #1401, one fact each.</summary>
public sealed class ReviewBAfter1401Tests
{
    [Theory]
    [InlineData(50_000, 50_000, true)]
    [InlineData(65_536, 32_768, true)]
    [InlineData(70_000, 70_000, false)]
    [InlineData(1, int.MaxValue, false)]
    public void ToDense_bounds_a_matrix_by_what_a_two_dimensional_array_holds(int rows, int columns, bool fits)
    {
        // 50,000 square is 2.5e9 cells: past the one-dimensional limit it was held to, within uint.MaxValue (#1412).
        MethodInfo bound = typeof(CsrMatrix).GetMethod("FitsDense", BindingFlags.Static | BindingFlags.NonPublic)!;

        Assert.Equal(fits, (bool)bound.Invoke(null, [rows, columns])!);
    }

    [Fact]
    public void ToDense_refuses_a_matrix_past_the_two_dimensional_total_before_allocating() =>
        Assert.Throws<InvalidOperationException>(() => new CsrMatrix(70_000, 70_000, [], [], new int[70_001]).ToDense());

    [Fact]
    public void Only_the_update_the_products_call_is_compiled_into_the_package()
    {
        Type elementWise = typeof(CsrMatrix).Assembly.GetType("Lodestar.Internal.ElementWise", throwOnError: true)!;
        string[] methods = [.. elementWise
            .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)];

        // Rotate and SubtractScaled rode along uncalled (#1417).
        Assert.Equal(["AddScaled"], methods);
    }
}
