#if NET
using System.Numerics;
using System.Runtime.InteropServices;
#endif

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The refusal of a value no fit can read, naming the cell that holds it.</summary>
/// <remarks>
/// One NaN turned the least-squares kernels' whole table into NaN and sent the instrumental-variables, panel and
/// multinomial kernels to a pivot test that blamed collinearity or separation (#1225, #1268). The check runs once the
/// lengths are known and before anything reads the values, so no later refusal can misname this one.
/// </remarks>
internal static class FiniteValues
{
    /// <summary>Refuses a NaN or an infinity in a row-major block.</summary>
    /// <param name="values">The block.</param>
    /// <param name="parameterName">The public parameter the block arrived in.</param>
    /// <param name="block">What the message calls the block.</param>
    /// <param name="columns">Values per row, one for a block with a single value per row.</param>
    /// <exception cref="ArgumentOutOfRangeException">A value in <paramref name="values"/> is NaN or infinite.</exception>
    public static void Require(ReadOnlySpan<double> values, string parameterName, string block, int columns)
    {
        int i = FirstNonFinite(values);
        if (i >= 0)
        {
            string cell = columns == 1 ? $"{block}[{i}]" : $"{block}[{i}], row {i / columns} column {i % columns},";
            throw new ArgumentOutOfRangeException(parameterName, values[i], $"{cell} is not a finite number.");
        }
    }

    /// <summary>The index of the first NaN or infinity in <paramref name="values"/>, or −1 when every value is finite.</summary>
    /// <remarks>
    /// This runs on every fit, whose cheapest path is a few hundred nanoseconds: one comparison a cell took 20 to 29% of
    /// <c>OLS.Estimate</c> at 100 × 3 (#1779). On net10 a block of lanes is tested at once, and the scalar loop, the
    /// whole scan on netstandard2.0, starts at the first block holding a value that fails, so the index is the same.
    /// </remarks>
    public static int FirstNonFinite(ReadOnlySpan<double> values)
    {
#if NET
        int start = FirstFailingBlock(values);
#else
        const int start = 0;
#endif
        for (int i = start; i < values.Length; i++)
        {
            // One comparison per cell, NaN included: NaN compares false against everything.
            if (!(Math.Abs(values[i]) <= double.MaxValue))
            {
                return i;
            }
        }

        return -1;
    }

#if NET
    /// <summary>Where the first block of lanes holding a NaN or an infinity starts, or the tail past the whole blocks.</summary>
    /// <remarks>
    /// <c>0·v</c> is a zero for a finite value and NaN otherwise, so two running sums of it stay zero exactly when every
    /// block is finite: a product and an addition a block, with no branch. Only a sum that is not zero searches
    /// the blocks for the first that fails.
    /// </remarks>
    private static int FirstFailingBlock(ReadOnlySpan<double> values)
    {
        if (!Vector.IsHardwareAccelerated)
        {
            return 0;
        }

        ReadOnlySpan<Vector<double>> blocks = MemoryMarshal.Cast<double, Vector<double>>(values);
        Vector<double> even = Vector<double>.Zero;
        Vector<double> odd = Vector<double>.Zero;
        int block = 0;
        for (; block + 1 < blocks.Length; block += 2)
        {
            even += blocks[block] * Vector<double>.Zero;
            odd += blocks[block + 1] * Vector<double>.Zero;
        }

        if (block < blocks.Length)
        {
            even += blocks[block] * Vector<double>.Zero;
        }

        if (even + odd == Vector<double>.Zero)
        {
            return blocks.Length * Vector<double>.Count;
        }

        var largest = new Vector<double>(double.MaxValue);
        for (block = 0; block < blocks.Length; block++)
        {
            if (!Vector.LessThanOrEqualAll(Vector.Abs(blocks[block]), largest))
            {
                return block * Vector<double>.Count;
            }
        }

        return blocks.Length * Vector<double>.Count;
    }
#endif
}
