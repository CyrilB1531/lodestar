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

    /// <summary>scikit-learn's <c>check_array</c> refusal of a non-finite value, in its words: NaN first, then infinity.</summary>
    /// <param name="values">The values to check.</param>
    /// <param name="parameterName">The parameter a refusal names.</param>
    /// <param name="inputName">The <c>input_name</c> the reference's message carries, <c>X</c> for an estimator's.</param>
    public static void RequireFiniteAsChecked(ReadOnlySpan<double> values, string parameterName, string? inputName = null)
    {
        // One pass: a NaN anywhere wins over an infinity met earlier, as the reference's two checks order them.
        bool infinite = false;
        foreach (double value in values)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException($"{Input(inputName)} contains NaN.", parameterName);
            }

            infinite |= double.IsInfinity(value);
        }

        if (infinite)
        {
            throw InfinityRefused(parameterName, inputName);
        }
    }

    /// <summary>The infinity half of <see cref="RequireFiniteAsChecked"/>, for a check that lets a NaN through.</summary>
    public static void RejectInfinityAsChecked(ReadOnlySpan<double> values, string parameterName, string? inputName = null)
    {
        foreach (double value in values)
        {
            if (double.IsInfinity(value))
            {
                throw InfinityRefused(parameterName, inputName);
            }
        }
    }

    private static ArgumentException InfinityRefused(string parameterName, string? inputName) => new(
        $"{Input(inputName)} contains infinity or a value too large for dtype('float64').", parameterName);

    private static string Input(string? inputName) => inputName is null ? "Input" : "Input " + inputName;

    /// <summary>Refuses a non-finite value, which the scalers reading a sorted column cannot answer for.</summary>
    /// <remarks>The first one met, in the previous release's words, which every refusal it made keeps.</remarks>
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
