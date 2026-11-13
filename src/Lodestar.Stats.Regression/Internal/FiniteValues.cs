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
    public static int FirstNonFinite(ReadOnlySpan<double> values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            // One comparison per cell, NaN included: this runs on every fit, whose cheapest path is a few hundred nanoseconds.
            if (!(Math.Abs(values[i]) <= double.MaxValue))
            {
                return i;
            }
        }

        return -1;
    }
}
