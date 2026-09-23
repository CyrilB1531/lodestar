namespace Lodestar.Stats.Internal;

/// <summary>The entry every correlation test shares: apply the policy, then refuse a mismatch.</summary>
/// <remarks>
/// <see cref="NanFilter.ApplyAligned"/> does the filtering; this is the dozen lines around it —
/// the two spans, the length guard and its message — which all three tests would otherwise
/// spell identically. Written once so they cannot come to disagree about what an invalid pair
/// is, and so the same sentence reaches a caller whichever test they reached for.
/// </remarks>
internal static class PairedSamples
{
    /// <summary>Filters both samples under <paramref name="policy"/> and checks they still align.</summary>
    /// <exception cref="ArgumentException">
    /// The samples differ in length, or <paramref name="policy"/> is <see cref="NanPolicy.Raise"/>
    /// and either holds a <c>NaN</c>.
    /// </exception>
    internal static void Align(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        NanPolicy policy,
        string xName,
        string yName,
        out ReadOnlySpan<double> left,
        out ReadOnlySpan<double> right)
    {
        left = x;
        right = y;
        if (policy != NanPolicy.Propagate)
        {
            (double[] filteredLeft, double[] filteredRight) =
                NanFilter.ApplyAligned(x, y, policy, xName, yName);
            left = filteredLeft;
            right = filteredRight;
        }

        if (left.Length != right.Length)
        {
            throw new ArgumentException(
                $"A correlation needs the same number of values in both samples; got {left.Length} and {right.Length}.",
                yName);
        }
    }
}
