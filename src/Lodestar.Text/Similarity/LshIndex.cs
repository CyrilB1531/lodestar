namespace Lodestar.Text.Similarity;

/// <summary>A banded index over MinHash signatures: candidates without comparing every pair.</summary>
/// <remarks>
/// Reference behavior: <c>datasketch</c> 1.6.5's <c>MinHashLSH</c>. Two signatures are
/// candidates when they agree on **every slot of at least one band**, which is what turns a
/// quadratic scan into a lookup. <see cref="Query"/> returns candidates, not matches —
/// scoring them with <see cref="MinHash.Jaccard"/> is the caller's step, and the banding
/// decides how often that step is wasted.
/// <para>
/// Adding is not thread-safe; concurrent <see cref="Query"/> calls are.
/// </para>
/// </remarks>
public sealed class LshIndex
{
    private readonly Dictionary<BandKey, List<string>> _buckets = [];
    private readonly HashSet<string> _keys = new(StringComparer.Ordinal);

    /// <summary>How the signatures are cut.</summary>
    public LshBanding Banding { get; }

    /// <summary>How many signatures have been added.</summary>
    public int Count => _keys.Count;

    /// <summary>Builds an empty index over one banding.</summary>
    /// <param name="banding">The banding every signature is cut by.</param>
    /// <exception cref="ArgumentOutOfRangeException">The banding has a non-positive dimension.</exception>
    public LshIndex(LshBanding banding)
    {
        Guard.NotLessThan(banding.Bands, 1);
        Guard.NotLessThan(banding.RowsPerBand, 1);
        Banding = banding;
    }

    /// <summary>Adds one signature under a key of the caller's choosing.</summary>
    /// <param name="key">What <see cref="Query"/> returns when this signature is a candidate.</param>
    /// <param name="signature">A signature at least <c>Banding.Permutations</c> long.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
    /// <exception cref="ArgumentException">The signature is too short, or the key is already held.</exception>
    public void Add(string key, ReadOnlySpan<uint> signature)
    {
        Guard.NotNull(key);
        RequireLength(signature);
        if (!_keys.Add(key))
        {
            throw new ArgumentException($"'{key}' is already in the index.", nameof(key));
        }

        for (int band = 0; band < Banding.Bands; band++)
        {
            BandKey bucket = KeyOf(band, signature);
            if (!_buckets.TryGetValue(bucket, out List<string>? members))
            {
                members = [];
                _buckets[bucket] = members;
            }

            members.Add(key);
        }
    }

    /// <summary>The keys sharing at least one band with a signature.</summary>
    /// <param name="signature">A signature at least <c>Banding.Permutations</c> long.</param>
    /// <returns>
    /// Each candidate once, in the order it was added — a key colliding on several bands is
    /// not returned several times, and the order does not encode how many bands agreed.
    /// </returns>
    /// <exception cref="ArgumentException">The signature is too short.</exception>
    public IReadOnlyList<string> Query(ReadOnlySpan<uint> signature)
    {
        RequireLength(signature);

        var seen = new HashSet<string>(StringComparer.Ordinal);
        var candidates = new List<string>();
        for (int band = 0; band < Banding.Bands; band++)
        {
            if (!_buckets.TryGetValue(KeyOf(band, signature), out List<string>? members))
            {
                continue;
            }

            foreach (string member in members)
            {
                bool firstSighting = seen.Add(member);
                if (firstSighting)
                {
                    candidates.Add(member);
                }
            }
        }

        return candidates;
    }

    private void RequireLength(ReadOnlySpan<uint> signature)
    {
        if (signature.Length < Banding.Permutations)
        {
            throw new ArgumentException(
                $"a signature of {signature.Length} cannot fill {Banding.Bands} bands of "
                + $"{Banding.RowsPerBand}.", nameof(signature));
        }
    }

    /// <summary>The bucket one band of a signature falls in.</summary>
    /// <remarks>
    /// The band's index is part of the key, so two different bands holding the same slot
    /// values do not collide — which they otherwise would, and silently.
    /// </remarks>
    private BandKey KeyOf(int band, ReadOnlySpan<uint> signature)
    {
        int start = band * Banding.RowsPerBand;
        uint[] slots = new uint[Banding.RowsPerBand];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = signature[start + i];
        }

        return new BandKey(band, slots);
    }

    /// <summary>A band index and the slot values in it, compared by value.</summary>
    private sealed class BandKey : IEquatable<BandKey>
    {
        private readonly int _band;
        private readonly uint[] _slots;
        private readonly int _hash;

        public BandKey(int band, uint[] slots)
        {
            _band = band;
            _slots = slots;

            // FNV-1a rather than HashCode, which netstandard2.0 does not carry: the values
            // being combined are already hashes, so the mixing only has to spread them.
            unchecked
            {
                uint hash = 2166136261u;
                hash = (hash ^ (uint)band) * 16777619u;
                foreach (uint slot in slots)
                {
                    hash = (hash ^ slot) * 16777619u;
                }

                _hash = (int)hash;
            }
        }

        public bool Equals(BandKey? other)
        {
            if (other is null || other._band != _band || other._slots.Length != _slots.Length)
            {
                return false;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != other._slots[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj) => Equals(obj as BandKey);

        public override int GetHashCode() => _hash;
    }
}
