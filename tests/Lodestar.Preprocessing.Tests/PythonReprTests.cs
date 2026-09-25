using Lodestar.Preprocessing.Internal;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Floating categories named as Python's <c>str</c> and numpy's <c>float32</c> name them (#1161); the full check ran
/// against <c>repr</c> on 20,017 doubles and numpy's <c>str</c> on 20,012 singles.
/// </summary>
public sealed class PythonReprTests
{
    [Theory]
    [InlineData(1.0, "1.0")]
    [InlineData(0.0001, "0.0001")]
    [InlineData(1e-05, "1e-05")]
    [InlineData(1.5e16, "1.5e+16")]
    [InlineData(1e22, "1e+22")]
    [InlineData(-0.0, "-0.0")]
    [InlineData(123.456, "123.456")]
    [InlineData(double.NaN, "nan")]
    [InlineData(double.NegativeInfinity, "-inf")]
    public void A_double_is_written_as_python_writes_it(double value, string expected) =>
        Assert.Equal(expected, PythonRepr.Double(value));

    [Theory]
    [InlineData(0.1f, "0.1")]
    [InlineData(0.0001f, "1e-04")]
    [InlineData(123456f, "123456.0")]
    [InlineData(1234567f, "1.234567e+06")]
    [InlineData(1e-5f, "1e-05")]
    public void A_single_is_written_as_numpy_writes_it(float value, string expected) =>
        Assert.Equal(expected, PythonRepr.Single(value));
}
