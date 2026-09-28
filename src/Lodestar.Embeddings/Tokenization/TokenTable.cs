namespace Lodestar.Embeddings.Tokenization;

/// <summary>Id to token: an array when the ids are dense, a dictionary when a file spreads them far apart.</summary>
/// <remarks>
/// tokenizers keeps its inverse vocabulary in a hash map, so a file whose one id is 2,000,000,000 loads there;
/// an array sized by the largest id would ask for 16 GB here, or overflow at <c>int.MaxValue</c> (#1333).
/// </remarks>
internal sealed class TokenTable
{
    /// <summary>Slots an array may spend per entry, past a fixed allowance, before a dictionary is cheaper.</summary>
    private const int DenseSlotsPerEntry = 2;

    private const int DenseAllowance = 1024;

    private readonly string?[]? _dense;
    private readonly Dictionary<int, string>? _sparse;

    private TokenTable(string?[]? dense, Dictionary<int, string>? sparse, long bound)
    {
        _dense = dense;
        _sparse = sparse;
        Bound = bound;
    }

    /// <summary>One past the largest id: the range a decoded id is checked against.</summary>
    internal long Bound { get; }

    /// <summary>Builds the table over non-negative ids; a later entry for an id replaces an earlier one.</summary>
    internal static TokenTable Build(IReadOnlyList<KeyValuePair<int, string>> entries)
    {
        long bound = 0;
        foreach (KeyValuePair<int, string> entry in entries)
        {
            bound = Math.Max(bound, (long)entry.Key + 1);
        }

        if (bound <= ((long)DenseSlotsPerEntry * entries.Count) + DenseAllowance)
        {
            var dense = new string?[bound];
            foreach (KeyValuePair<int, string> entry in entries)
            {
                dense[entry.Key] = entry.Value;
            }

            return new TokenTable(dense, null, bound);
        }

        var sparse = new Dictionary<int, string>(entries.Count);
        foreach (KeyValuePair<int, string> entry in entries)
        {
            sparse[entry.Key] = entry.Value;
        }

        return new TokenTable(null, sparse, bound);
    }

    /// <summary>The token of <paramref name="id"/>, or <see langword="null"/> when none has it.</summary>
    internal string? this[int id]
    {
        get
        {
            if (_dense is { } dense)
            {
                return (uint)id < (uint)dense.Length ? dense[id] : null;
            }

            return _sparse!.TryGetValue(id, out string? token) ? token : null;
        }
    }
}
