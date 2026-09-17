#if NET5_0_OR_GREATER
using System.Numerics;
using System.Runtime.InteropServices;
#endif

namespace Lodestar.Metrics.Internal;

/// <summary>
/// Validates the <c>(yTrue, yPred, sampleWeight)</c> triple every metric in this
/// package accepts, in one place — so the metric types landing in later tasks
/// do not each restate the same three checks.
/// </summary>
internal static class Inputs
{
    /// <summary>
    /// Checks that <paramref name="yTrue"/> and <paramref name="yPred"/> agree in
    /// length and are not empty, and that <paramref name="sampleWeight"/>, when
    /// supplied, agrees in length with them and passes <see cref="ValidateSampleWeight"/>.
    /// </summary>
    /// <param name="yTrue">The true labels.</param>
    /// <param name="yPred">The predicted labels, expected to be the same length as <paramref name="yTrue"/>.</param>
    /// <param name="sampleWeight">A weight per sample, or empty when every sample is weighted 1.</param>
    /// <exception cref="ArgumentException">The inputs disagree in length or are empty, or the sample weight holds a non-finite value or is zero throughout.</exception>
    public static void Validate(ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, ReadOnlySpan<double> sampleWeight)
    {
        if (yTrue.Length != yPred.Length)
        {
            throw new ArgumentException(
                $"yTrue has {yTrue.Length} entries and yPred has {yPred.Length}; they must agree.",
                nameof(yPred));
        }
        if (yTrue.Length == 0)
        {
            throw new ArgumentException("yTrue and yPred are empty; there is nothing to score.", nameof(yTrue));
        }
        if (!sampleWeight.IsEmpty && sampleWeight.Length != yTrue.Length)
        {
            throw new ArgumentException(
                $"sampleWeight has {sampleWeight.Length} entries but there are {yTrue.Length} samples.",
                nameof(sampleWeight));
        }

        ValidateSampleWeight(sampleWeight);
    }

    /// <summary>
    /// The regression counterpart: two of the three checks above — the length
    /// agreement and the emptiness — plus a non-finite target, then the same
    /// weight checks.
    /// </summary>
    /// <param name="yTrue">The true values.</param>
    /// <param name="yPred">The predicted values, expected to be the same length as <paramref name="yTrue"/>.</param>
    /// <param name="sampleWeight">A weight per sample, or empty when every sample is weighted 1.</param>
    /// <remarks>
    /// The length check on <paramref name="sampleWeight"/> lives in
    /// <see cref="Outputs.Validate"/> instead, once <c>outputCount</c> gives the
    /// sample count. The finiteness scan matches scikit-learn's <c>check_array</c>.
    /// </remarks>
    /// <exception cref="ArgumentException">The inputs disagree in length, are empty, hold a non-finite value, or the sample weight is zero throughout.</exception>
    public static void Validate(
        ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPred, ReadOnlySpan<double> sampleWeight)
    {
        if (yTrue.Length != yPred.Length)
        {
            throw new ArgumentException(
                $"yTrue has {yTrue.Length} entries and yPred has {yPred.Length}; they must agree.",
                nameof(yPred));
        }
        if (yTrue.Length == 0)
        {
            throw new ArgumentException("yTrue and yPred are empty; there is nothing to score.", nameof(yTrue));
        }

        RequireFinite(yTrue, nameof(yTrue));
        RequireFinite(yPred, nameof(yPred));
        ValidateSampleWeight(sampleWeight);
    }

    /// <summary>
    /// What <c>_check_sample_weight</c> refuses in a weight vector already of the right
    /// length: a non-finite value, then a vector that is zero throughout.
    /// </summary>
    /// <param name="sampleWeight">A weight per sample, or empty when every sample is weighted 1.</param>
    /// <remarks>
    /// Every classification metric reaching <c>_check_targets</c> calls it, as do
    /// <c>log_loss</c> and <c>brier_score_loss</c>; <c>hinge_loss</c> does not.
    /// </remarks>
    /// <exception cref="ArgumentException">A weight is not finite, or every weight is zero.</exception>
    public static void ValidateSampleWeight(ReadOnlySpan<double> sampleWeight)
    {
        if (!sampleWeight.IsEmpty)
        {
            RequireFinite(sampleWeight, nameof(sampleWeight), "sample_weight");
            RequireAnyNonZero(sampleWeight);
        }
    }

    /// <summary>
    /// Reproduces <c>_check_sample_weight</c>'s refusal of a weight that is zero
    /// throughout, with its message.
    /// </summary>
    /// <remarks>
    /// The test is <c>_check_sample_weight</c>'s own — <c>all(sample_weight == 0)</c>
    /// at <c>sklearn/utils/validation.py:2198</c> — not "the weights sum to zero":
    /// it accepts <c>[1, -1, 0]</c>, which numpy refuses one layer later instead.
    /// <see cref="Outputs.Validate"/>'s <c>RequireNormalizable</c> is that other rule.
    /// </remarks>
    private static void RequireAnyNonZero(ReadOnlySpan<double> sampleWeight)
    {
        foreach (double weight in sampleWeight)
        {
            // S1244: this is a comparison against the exact zero a caller wrote,
            // not against a computed quantity, and scikit-learn's own test is
            // the same exact `sample_weight == 0`. A tolerance would refuse a
            // legitimately tiny weight — 1e-320 is one scikit-learn accepts.
#pragma warning disable S1244
            if (weight != 0.0)
#pragma warning restore S1244
            {
                return;
            }
        }

        throw new ArgumentException(
            "Sample weights must contain at least one non-zero number.", nameof(sampleWeight));
    }

    /// <summary>Reproduces scikit-learn's two <c>check_array</c> messages, which differ.</summary>
    /// <param name="values">The values to check.</param>
    /// <param name="paramName">The argument the values came from.</param>
    /// <param name="inputName">The <c>input_name</c> the reference's message carries, or <see langword="null"/> where it carries none.</param>
    /// <exception cref="ArgumentException">A value is not finite.</exception>
    public static void RequireFinite(ReadOnlySpan<double> values, string paramName, string? inputName = null)
    {
        if (!AllFinite(values))
        {
            throw NonFinite(values, paramName, inputName);
        }
    }

    // S1764: in both overloads x - x is the test itself, zero only for a finite x; .NET does not
    // reassociate or fold floating-point arithmetic (docs/decisions/0033), so it survives the JIT.
#pragma warning disable S1764
    /// <summary>Zero exactly when <paramref name="value"/> is finite, and nonzero otherwise.</summary>
    /// <param name="value">The value to test.</param>
    /// <remarks>
    /// A finite <c>x</c> gives <c>x - x</c> = +0.0, whose bits are all zero, and an infinity or a
    /// NaN gives NaN, whose bits are not. OR-ing these over a span and testing once replaces two
    /// branches per value; only an input that fails pays a second, scalar, pass for its message.
    /// </remarks>
    public static long NonFiniteBits(double value) => BitConverter.DoubleToInt64Bits(value - value);

#if NET5_0_OR_GREATER
    /// <summary><see cref="NonFiniteBits(double)"/> per lane.</summary>
    /// <param name="values">The values to test, one per lane.</param>
    public static Vector<long> NonFiniteBits(Vector<double> values) => Vector.AsVectorInt64(values - values);
#endif
#pragma warning restore S1764

    /// <summary>Whether every value is finite, tested without a branch per value.</summary>
    /// <param name="values">The values to test.</param>
    private static bool AllFinite(ReadOnlySpan<double> values)
    {
#if NET5_0_OR_GREATER
        long bits = Vector.IsHardwareAccelerated
            ? LaneNonFiniteBits(values, out int i)
            : GroupNonFiniteBits(values, out i);
#else
        long bits = GroupNonFiniteBits(values, out int i);
#endif
        for (; i < values.Length; i++)
        {
            bits |= NonFiniteBits(values[i]);
        }
        return bits == 0;
    }

#if NET5_0_OR_GREATER
    /// <summary>Tests the whole <see cref="Vector{T}"/> blocks of <paramref name="values"/>.</summary>
    /// <param name="values">The values to test.</param>
    /// <param name="consumed">How many leading values the blocks covered.</param>
    /// <returns>Zero when every value the blocks covered is finite, and nonzero otherwise.</returns>
    private static long LaneNonFiniteBits(ReadOnlySpan<double> values, out int consumed)
    {
        ReadOnlySpan<Vector<double>> blocks = MemoryMarshal.Cast<double, Vector<double>>(values);
        Vector<long> laneBits = Vector<long>.Zero;
        foreach (Vector<double> block in blocks)
        {
            laneBits |= NonFiniteBits(block);
        }

        consumed = blocks.Length * Vector<double>.Count;
        return laneBits == Vector<long>.Zero ? 0 : 1;
    }
#endif

    /// <summary>Tests the whole groups of four of <paramref name="values"/>, four values per step.</summary>
    /// <param name="values">The values to test.</param>
    /// <param name="consumed">How many leading values the groups covered.</param>
    /// <returns>Zero when every value the groups covered is finite, and nonzero otherwise.</returns>
    /// <remarks>
    /// The walk <c>netstandard2.0</c> takes. Internal rather than private so the <c>net10.0</c> tests,
    /// where the lanes route around it, can reach it directly.
    /// </remarks>
    internal static long GroupNonFiniteBits(ReadOnlySpan<double> values, out int consumed)
    {
        long bits = 0;
        int i = 0;
        for (; i <= values.Length - 4; i += 4)
        {
            bits |= NonFiniteBits(values[i]) | NonFiniteBits(values[i + 1])
                | NonFiniteBits(values[i + 2]) | NonFiniteBits(values[i + 3]);
        }

        consumed = i;
        return bits;
    }

    /// <summary>scikit-learn's message for the first non-finite value, which decides between its two.</summary>
    /// <param name="values">Values holding at least one that is not finite.</param>
    /// <param name="paramName">The argument the values came from.</param>
    /// <param name="inputName">The <c>input_name</c> the message carries, or <see langword="null"/>.</param>
    private static ArgumentException NonFinite(ReadOnlySpan<double> values, string paramName, string? inputName)
    {
        int index = 0;
        while (NonFiniteBits(values[index]) == 0)
        {
            index++;
        }

        // check_array names the input only when its caller passed input_name, which
        // _check_sample_weight does and the targets' checks do not.
        string input = inputName is null ? "Input" : "Input " + inputName;
        return double.IsNaN(values[index])
            ? new ArgumentException($"{input} contains NaN.", paramName)
            : new ArgumentException(
                $"{input} contains infinity or a value too large for dtype('float64').", paramName);
    }
}
