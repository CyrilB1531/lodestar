namespace Lodestar.Preprocessing.Internal;

/// <summary>The part of <c>numpy.random.RandomState</c> scikit-learn's splitters draw from, replayed call for call.</summary>
/// <remarks>
/// <c>check_random_state(seed)</c> builds a legacy <c>RandomState</c>: MT19937 seeded by <c>init_genrand</c>, bounded
/// integers by masked rejection, <c>shuffle</c> by Fisher-Yates from the top, and <c>choice(replace=False)</c> as
/// <c>permutation(n)[:size]</c>. Decision 0008 says why it is written here; the
/// <c>numpy_random_state.json</c> corpus holds it to numpy's raw outputs before any splitter uses it.
/// </remarks>
internal sealed class NumpyRandomState
{
    private const int StateLength = 624;
    private const int Shift = 397;
    private const uint MatrixA = 0x9908b0dfU;
    private const uint UpperMask = 0x80000000U;
    private const uint LowerMask = 0x7fffffffU;

    /// <summary>The largest seed numpy's legacy seeding accepts, <c>2³² − 1</c>.</summary>
    public const long MaxSeed = uint.MaxValue;

    private readonly uint[] _state = new uint[StateLength];
    private int _position;

    /// <summary>Seeds the generator as <c>RandomState(seed)</c> does.</summary>
    /// <param name="seed">The seed, in <c>[0, 2³² − 1]</c>; the caller checks the range.</param>
    public NumpyRandomState(long seed)
    {
        _state[0] = (uint)seed;
        for (int i = 1; i < StateLength; i++)
        {
            uint previous = _state[i - 1];
            _state[i] = unchecked((1812433253U * (previous ^ (previous >> 30))) + (uint)i);
        }

        _position = StateLength;
    }

    /// <summary>Refuses a seed numpy's legacy seeding refuses.</summary>
    /// <param name="seed">The seed a caller passed.</param>
    /// <param name="parameterName">The parameter to name in the exception.</param>
    public static void RequireSeed(long seed, string parameterName)
    {
        if (seed < 0 || seed > MaxSeed)
        {
            throw new ArgumentOutOfRangeException(
                parameterName, seed, "A random state lies in [0, 2^32 - 1], the range numpy's RandomState accepts.");
        }
    }

    /// <summary>The next 32-bit output, numpy's <c>random_raw</c>.</summary>
    public uint NextUInt32()
    {
        if (_position >= StateLength)
        {
            Twist();
        }

        uint y = _state[_position++];
        y ^= y >> 11;
        y ^= (y << 7) & 0x9d2c5680U;
        y ^= (y << 15) & 0xefc60000U;
        y ^= y >> 18;
        return y;
    }

    /// <summary>An integer in <c>[0, max]</c>, numpy's legacy <c>random_interval</c>.</summary>
    /// <param name="max">The largest value to return, below <c>2³¹</c> since every caller here bounds an index.</param>
    public int Interval(int max)
    {
        if (max == 0)
        {
            return 0;
        }

        uint mask = SmearedMask((uint)max);
        uint value;
        do
        {
            value = NextUInt32() & mask;
        }
        while (value > (uint)max);

        return (int)value;
    }

    /// <summary>Shuffles in place, as <c>RandomState.shuffle</c> does a one-dimensional array.</summary>
    /// <param name="values">What to shuffle.</param>
    /// <remarks>
    /// <see cref="Interval"/> inlined, its mask narrowed only when <c>i</c> drops below a power of two: the draw per
    /// element is what <c>train_test_split</c> costs, and the reference's is C.
    /// </remarks>
    public void Shuffle(Span<int> values)
    {
        if (values.Length < 2)
        {
            return;
        }

        uint mask = SmearedMask((uint)(values.Length - 1));
        for (int i = values.Length - 1; i > 0; i--)
        {
            while ((uint)i <= (mask >> 1))
            {
                mask >>= 1;
            }

            uint j;
            do
            {
                j = NextUInt32() & mask;
            }
            while (j > (uint)i);

            (values[i], values[(int)j]) = (values[(int)j], values[i]);
        }
    }

    private static uint SmearedMask(uint max)
    {
        uint mask = max;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;
        return mask;
    }

    /// <summary><c>RandomState.permutation(count)</c>: <c>0..count-1</c>, shuffled.</summary>
    /// <param name="count">How many indices.</param>
    public int[] Permutation(int count)
    {
        var permutation = new int[count];
        for (int i = 0; i < count; i++)
        {
            permutation[i] = i;
        }

        Shuffle(permutation);
        return permutation;
    }

    /// <summary><c>RandomState.choice(pool, size, replace=False)</c>: the pool read through <c>permutation(pool.Length)[:size]</c>.</summary>
    /// <param name="pool">What to choose from.</param>
    /// <param name="size">How many to choose, at most the pool's length.</param>
    public int[] Choice(ReadOnlySpan<int> pool, int size)
    {
        int[] permutation = Permutation(pool.Length);
        var chosen = new int[size];
        for (int i = 0; i < size; i++)
        {
            chosen[i] = pool[permutation[i]];
        }

        return chosen;
    }

    /// <summary>Regenerates the state: the reference loop, split where its indices wrap so no step takes a modulo.</summary>
    private void Twist()
    {
        uint[] state = _state;
        int k = 0;
        for (; k < StateLength - Shift; k++)
        {
            state[k] = state[k + Shift] ^ Mix(state[k], state[k + 1]);
        }

        for (; k < StateLength - 1; k++)
        {
            state[k] = state[k + Shift - StateLength] ^ Mix(state[k], state[k + 1]);
        }

        state[StateLength - 1] = state[Shift - 1] ^ Mix(state[StateLength - 1], state[0]);
        _position = 0;
    }

    private static uint Mix(uint upper, uint lower)
    {
        uint y = (upper & UpperMask) | (lower & LowerMask);
        return (y >> 1) ^ ((y & 1U) == 0 ? 0U : MatrixA);
    }
}
