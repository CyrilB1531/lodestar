namespace Lodestar.Preprocessing.Internal;

/// <summary>What every scaler here checks about a row-major matrix before reading it.</summary>
/// <remarks>
/// Shared rather than repeated per scaler: four copies of the same two sentences would be four
/// places for the message to drift, and S1192 would report the literal anyway.
/// </remarks>
internal static class SampleMatrix
{
    /// <summary>The row count, with the two shapes that are not a matrix refused.</summary>
    /// <exception cref="ArgumentException">The span is empty, or is not a whole number of rows.</exception>
    public static int Rows(ReadOnlySpan<double> samples, int featureCount)
    {
        if (samples.Length == 0 || samples.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"samples holds {samples.Length} values, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(samples));
        }

        return samples.Length / featureCount;
    }

    /// <summary>Refuses a non-finite value, which the scalers reading a sorted column cannot answer for.</summary>
    /// <exception cref="ArgumentException">A value is <c>NaN</c> or infinite.</exception>
    public static void RequireFinite(ReadOnlySpan<double> samples, string parameterName)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            if (double.IsNaN(samples[i]) || double.IsInfinity(samples[i]))
            {
                throw new ArgumentException(
                    $"{parameterName}[{i}] is {samples[i]}. The reference skips a NaN; a percentile over a "
                    + "sorted column cannot, so it is refused rather than answered wrongly.",
                    parameterName);
            }
        }
    }
}
