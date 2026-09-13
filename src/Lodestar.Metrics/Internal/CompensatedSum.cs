#if NET5_0_OR_GREATER
using System.Numerics;
#endif

namespace Lodestar.Metrics.Internal;

/// <summary>
/// A running sum that keeps the low-order bits a sequential <c>+=</c> discards —
/// Neumaier's variant of compensated summation.
/// </summary>
/// <remarks>
/// numpy sums pairwise; a sequential loop can drift past the oracle's 1e-9
/// tolerance, and Neumaier's branch — not Kahan's — removes that failure mode
/// unconditionally. Measured, with the Kahan comparison, in
/// <c>docs/decisions/0033-compensated-sum-is-neumaiers-variant.md</c> (issue #127).
/// </remarks>
internal struct CompensatedSum
{
    private double _sum;
    private double _compensation;

    /// <summary>Adds one term, keeping what the addition rounded off.</summary>
    /// <param name="value">The term to add.</param>
    public void Add(double value)
    {
        double total = _sum + value;
        _compensation += Math.Abs(_sum) >= Math.Abs(value)
            ? (_sum - total) + value
            : (value - total) + _sum;
        _sum = total;
    }

    /// <summary>The sum, with the accumulated rounding folded back in.</summary>
    public readonly double Value => _sum + _compensation;
}

/// <summary>
/// Four <see cref="CompensatedSum"/>s taking consecutive terms in turn — the scalar
/// counterpart of a four-lane <c>VectorCompensatedSum</c>.
/// </summary>
/// <remarks>
/// One running sum makes every addition wait for the one before it; four let the
/// processor carry four chains at once. The stripes take the terms a four-lane
/// <c>VectorCompensatedSum</c> would, and <see cref="Reduce"/> folds them in its lane order;
/// no test asserts the two targets agree to the bit, only both to the oracles at 1e-9.
/// </remarks>
internal struct StripedCompensatedSum
{
    // S3459: each stripe is a mutable struct written through its own Add, which the rule misses.
#pragma warning disable S3459
    private CompensatedSum _stripe0;
    private CompensatedSum _stripe1;
    private CompensatedSum _stripe2;
    private CompensatedSum _stripe3;
#pragma warning restore S3459

    /// <summary>Adds four consecutive terms, one to each stripe.</summary>
    /// <param name="first">The term for stripe 0.</param>
    /// <param name="second">The term for stripe 1.</param>
    /// <param name="third">The term for stripe 2.</param>
    /// <param name="fourth">The term for stripe 3.</param>
    public void Add(double first, double second, double third, double fourth)
    {
        _stripe0.Add(first);
        _stripe1.Add(second);
        _stripe2.Add(third);
        _stripe3.Add(fourth);
    }

    /// <summary>Rounds each stripe to one double, then compensated-adds the four.</summary>
    public readonly CompensatedSum Reduce()
    {
        CompensatedSum result = default;
        result.Add(_stripe0.Value);
        result.Add(_stripe1.Value);
        result.Add(_stripe2.Value);
        result.Add(_stripe3.Value);
        return result;
    }
}

#if NET5_0_OR_GREATER
/// <summary>
/// <see cref="CompensatedSum"/> per SIMD lane — <see cref="Vector{T}"/> on
/// <c>net10.0</c> only; see <c>docs/decisions/0001-target-framework.md</c>.
/// </summary>
/// <remarks>
/// Each lane is Neumaier-exact on its own terms; <see cref="Reduce"/> combines
/// lanes in a different order than a scalar loop, so the two are not
/// guaranteed bit-identical — both pass the oracle's 1e-9 comparison. See
/// <c>docs/decisions/0033-compensated-sum-is-neumaiers-variant.md</c>.
/// </remarks>
internal struct VectorCompensatedSum
{
    private Vector<double> _sum;
    private Vector<double> _compensation;

    /// <summary>Adds one term per lane, keeping what each lane's addition rounded off.</summary>
    /// <param name="value">One term per lane.</param>
    public void Add(Vector<double> value)
    {
        Vector<double> total = _sum + value;
        Vector<double> useSum = Vector.GreaterThanOrEqual<double>(Vector.Abs(_sum), Vector.Abs(value));
        Vector<double> viaSum = (_sum - total) + value;
        Vector<double> viaValue = (value - total) + _sum;
        _compensation += Vector.ConditionalSelect(useSum, viaSum, viaValue);
        _sum = total;
    }

    /// <summary>
    /// Rounds each lane's own <c>_sum + _compensation</c> to one <see cref="double"/>
    /// first, then Neumaier-adds those <see cref="Vector{T}.Count"/> doubles into
    /// one <see cref="CompensatedSum"/> — a second, separate compensated
    /// combination on top of what each lane already did for itself, not a
    /// continuation of it.
    /// </summary>
    public readonly CompensatedSum Reduce()
    {
        CompensatedSum result = default;
        for (int lane = 0; lane < Vector<double>.Count; lane++)
        {
            result.Add(_sum[lane] + _compensation[lane]);
        }
        return result;
    }
}
#endif
