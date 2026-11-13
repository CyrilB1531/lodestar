#if NET
using System.Numerics;
using System.Runtime.InteropServices;
#endif

namespace Lodestar.Internal;

/// <summary>The largest <c>|value|</c> of a finite block, a block of lanes at a time on net10.</summary>
/// <remarks>
/// Shared source, as <see cref="RankBracket"/> is: the time series' unit scale and the regressions' exact-fit test both
/// read it (#1779). A comparison, not <c>Math.Max</c>, whose NaN and signed-zero rules cost the scan more than its reads;
/// a NaN may drop or keep its lane's other values on net10, as the hardware's maximum has it, so a caller refuses
/// non-finite values first or tests them afterwards, as ExactFit.Holds does.
/// </remarks>
internal static class LargestMagnitude
{
    /// <summary>The largest <c>|value|</c>, zero for an empty block.</summary>
    internal static double Of(ReadOnlySpan<double> values)
    {
        double largest = 0.0;
        int start = 0;
#if NET
        if (Vector.IsHardwareAccelerated)
        {
            // Four accumulators and MaxNative, whose NaN and signed-zero rules are the hardware's: a third of one
            // accumulator's Vector.Max, 3,380 ns over 4,000 values (Review A pass 15).
            ReadOnlySpan<Vector<double>> blocks = MemoryMarshal.Cast<double, Vector<double>>(values);
            Vector<double> a = Vector<double>.Zero, b = a, c = a, d = a;
            int block = 0;
            for (; block + 3 < blocks.Length; block += 4)
            {
                a = Vector.MaxNative(a, Vector.Abs(blocks[block]));
                b = Vector.MaxNative(b, Vector.Abs(blocks[block + 1]));
                c = Vector.MaxNative(c, Vector.Abs(blocks[block + 2]));
                d = Vector.MaxNative(d, Vector.Abs(blocks[block + 3]));
            }

            for (; block < blocks.Length; block++)
            {
                a = Vector.MaxNative(a, Vector.Abs(blocks[block]));
            }

            Vector<double> lanes = Vector.MaxNative(Vector.MaxNative(a, b), Vector.MaxNative(c, d));
            for (int lane = 0; lane < Vector<double>.Count; lane++)
            {
                largest = lanes[lane] > largest ? lanes[lane] : largest;
            }

            start = blocks.Length * Vector<double>.Count;
        }
#endif
        for (int row = start; row < values.Length; row++)
        {
            double size = Math.Abs(values[row]);
            largest = size > largest ? size : largest;
        }

        return largest;
    }
}
