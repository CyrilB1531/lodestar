#if NET9_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace Lodestar.Text.Vectorization;

/// <summary>
/// The ordinal string set behind the shipped stop-word lists and the analyzer's
/// filter: built once, then read once per token.
/// </summary>
/// <remarks>
/// Built-once-read-often is why this type exists: on net10 a <c>FrozenSet&lt;string&gt;</c> answers a stop
/// word from a span, never materialising it as a string. netstandard2.0 keeps a plain
/// <see cref="HashSet{T}"/>, handed out read-only — the single <c>#if</c> below is the only place either build is
/// named. Matching is always ordinal, against the analyzer's already-lowercased output.
/// </remarks>
internal sealed class StopWordSet
{
    private static int _frozen;

    /// <summary>How many of the shipped lists have been built since the type loaded.</summary>
    /// <remarks>
    /// The lists initialise lazily, one nested holder type each. This counter is
    /// the only way a test can watch a holder <em>not</em> initialise: any
    /// reflection that could read a holder's field would run its initialiser
    /// first. See <c>StopWordsLazinessTests</c>, which loads the assembly into a
    /// fresh <c>AssemblyLoadContext</c> — fresh statics — and reads this.
    /// </remarks>
    internal static int FrozenLists => Volatile.Read(ref _frozen);

#if NET9_0_OR_GREATER

    private readonly FrozenSet<string> _words;
    private readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> _lookup;

    private StopWordSet(FrozenSet<string> words)
    {
        _words = words;
        _lookup = words.GetAlternateLookup<ReadOnlySpan<char>>();
    }

    /// <summary>The set itself, for callers that only need to enumerate or count it.</summary>
    public IReadOnlyCollection<string> Words => _words;

    /// <summary>Builds one of the shipped lists, once, for a <see cref="StopWords"/> property.</summary>
    public static IReadOnlyCollection<string> Freeze(string[] words)
    {
        Interlocked.Increment(ref _frozen);
        return words.ToFrozenSet(StringComparer.Ordinal);
    }

    /// <summary>Wraps a caller's collection for use as a filter.</summary>
    /// <remarks>
    /// A shipped list arrives already frozen with <see cref="StringComparer.Ordinal"/>
    /// and is reused as-is — <c>ToFrozenSet</c> returns its argument in that case,
    /// so nothing is re-hashed per vectorizer. Anything else is copied, which is
    /// not merely defensive: a caller's <see cref="HashSet{T}"/> is theirs to
    /// mutate, and aliasing it would let a later <c>Add</c> change what a fitted
    /// vectorizer removes.
    /// </remarks>
    public static StopWordSet Adopt(IReadOnlyCollection<string> words) =>
        new(words.ToFrozenSet(StringComparer.Ordinal));

    /// <summary>Whether the token spanning <paramref name="token"/> is a stop word.</summary>
    public bool Contains(ReadOnlySpan<char> token) => _lookup.Contains(token);

#else

    private readonly HashSet<string> _words;

    private StopWordSet(HashSet<string> words) => _words = words;

    /// <summary>The set itself, for callers that only need to enumerate or count it.</summary>
    public IReadOnlyCollection<string> Words => _words;

    /// <summary>Builds one of the shipped lists, once, for a <see cref="StopWords"/> property.</summary>
    /// <remarks>
    /// Handed out read-only, as net10's frozen set is: a cast to <see cref="ICollection{T}"/> no longer edits the list
    /// for the whole process (#1674).
    /// </remarks>
    public static IReadOnlyCollection<string> Freeze(string[] words)
    {
        Interlocked.Increment(ref _frozen);
        return new ShippedList(new HashSet<string>(words, StringComparer.Ordinal));
    }

    /// <summary>Wraps a caller's collection for use as a filter, always by copy.</summary>
    /// <remarks>
    /// A shipped list is copied too, so <see cref="Words"/> never hands out the set behind it: that is the netstandard2.0
    /// cost, and the behaviour this build has always had.
    /// </remarks>
    public static StopWordSet Adopt(IReadOnlyCollection<string> words) =>
        new(new HashSet<string>(words is ShippedList shipped ? shipped.Set : words, StringComparer.Ordinal));

    /// <summary>Whether <paramref name="token"/> is a stop word.</summary>
    public bool Contains(string token) => _words.Contains(token);

    /// <summary>A shipped list: its set, which only reads go through, an <see cref="ISet{T}"/> as 0.7.0's was.</summary>
    private sealed class ShippedList : ISet<string>, IReadOnlyCollection<string>
    {
        public ShippedList(HashSet<string> set) => Set = set;

        public HashSet<string> Set { get; }

        public int Count => Set.Count;

        public bool IsReadOnly => true;

        public bool Contains(string item) => Set.Contains(item);

        public void CopyTo(string[] array, int arrayIndex) => Set.CopyTo(array, arrayIndex);

        public bool IsProperSubsetOf(IEnumerable<string> other) => Set.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<string> other) => Set.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<string> other) => Set.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<string> other) => Set.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<string> other) => Set.Overlaps(other);

        public bool SetEquals(IEnumerable<string> other) => Set.SetEquals(other);

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => Set.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Set.GetEnumerator();

        bool ISet<string>.Add(string item) => throw ReadOnly();

        void ICollection<string>.Add(string item) => throw ReadOnly();

        void ICollection<string>.Clear() => throw ReadOnly();

        bool ICollection<string>.Remove(string item) => throw ReadOnly();

        void ISet<string>.ExceptWith(IEnumerable<string> other) => throw ReadOnly();

        void ISet<string>.IntersectWith(IEnumerable<string> other) => throw ReadOnly();

        void ISet<string>.SymmetricExceptWith(IEnumerable<string> other) => throw ReadOnly();

        void ISet<string>.UnionWith(IEnumerable<string> other) => throw ReadOnly();

        private static NotSupportedException ReadOnly() => new("A shipped stop-word list is read-only.");
    }

#endif
}
