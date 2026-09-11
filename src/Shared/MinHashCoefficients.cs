using System;

namespace Lodestar.Internal;

/// <summary>
/// What an affine-32 MinHash permutation requires of its coefficients, checked once
/// (compiled into each assembly that sketches).
/// </summary>
/// <remarks>
/// Shared source rather than a shared package: <c>Lodestar.Gpu</c> carries no edge to any
/// Lodestar package, so the two had the same thirty lines twice until SonarCloud counted
/// them (#645). The reference generates its own coefficients and checks neither of these;
/// here they arrive from a caller, which decision 0072 is what makes true.
/// </remarks>
internal static class MinHashCoefficients
{
    /// <summary>Refuses a pair the affine-32 family cannot read.</summary>
    /// <param name="multipliers">The <c>a</c> coefficients.</param>
    /// <param name="addends">The <c>b</c> coefficients, one per multiplier.</param>
    /// <param name="parameterName">The caller's parameter to name in the exception.</param>
    /// <exception cref="ArgumentException">
    /// A coefficient does not fit 32 bits, or a multiplier is even.
    /// </exception>
    public static void RefuseWhatAffine32CannotRead(
        ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends, string parameterName)
    {
        for (int i = 0; i < multipliers.Length; i++)
        {
            if (multipliers[i] > uint.MaxValue || addends[i] > uint.MaxValue)
            {
                // Past 32 bits is the other family's width, so this is a pair from the wrong
                // scheme rather than a value out of range.
                throw new ArgumentException(
                    $"permutation {i} carries {multipliers[i]} and {addends[i]}, and Affine32 "
                    + "reads 32-bit coefficients.", parameterName);
            }

            if ((multipliers[i] & 1UL) == 0UL)
            {
                // An even multiplier maps distinct hashes onto each other -- every one sharing
                // a low bit lands together -- so the estimate is merely wrong, never an error.
                throw new ArgumentException(
                    $"permutation {i} carries the even multiplier {multipliers[i]}, which maps "
                    + "distinct hashes onto each other instead of permuting them.", parameterName);
            }
        }
    }
}
