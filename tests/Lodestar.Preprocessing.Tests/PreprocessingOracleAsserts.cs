using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Reads a corpus case's numbers, and compares a row of them at a stated tolerance.</summary>
/// <remarks>
/// A tolerance parameter rather than a constant, because one family here does not meet the
/// repository's <c>1e-9</c> and says so: the power transform's exponent is pinned by its own
/// log-likelihood only to about <c>6e-7</c>. Every other family passes <c>1e-9</c>.
/// </remarks>
internal static class PreprocessingOracleAsserts
{
    internal const double Tolerance = 1e-9;

    /// <summary>The tolerance the power transform is held to, and why, in one place.</summary>
    internal const double PowerTolerance = 1e-5;

    internal static double[] Doubles(JsonElement element) =>
        [.. element.EnumerateArray().Select(Number)];

    internal static double Number(JsonElement element) =>
        element.ValueKind == JsonValueKind.String
            ? element.GetString() switch
            {
                "NaN" => double.NaN,
                "Infinity" => double.PositiveInfinity,
                "-Infinity" => double.NegativeInfinity,
                var other => throw new InvalidDataException($"Unknown corpus number '{other}'."),
            }
            : element.GetDouble();

    internal static void Row(double[] expected, double[] actual, string caseName, double tolerance = Tolerance)
    {
        Assert.True(
            expected.Length == actual.Length,
            $"{caseName}: expected {expected.Length} values, got {actual.Length}.");

        for (int i = 0; i < expected.Length; i++)
        {
            if (double.IsNaN(expected[i]))
            {
                Assert.True(double.IsNaN(actual[i]), $"{caseName}[{i}]: expected NaN, got {actual[i]}.");
                continue;
            }

            double gap = Math.Abs(expected[i] - actual[i]);
            double allowed = tolerance * Math.Max(1.0, Math.Abs(expected[i]));
            Assert.True(
                gap <= allowed,
                $"{caseName}[{i}]: {actual[i]} differs from {expected[i]} by {gap}, past {allowed}.");
        }
    }
}
