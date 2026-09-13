using System.Runtime.CompilerServices;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>A read-only double-array trie over UTF-16 code units, walked one character at a time.</summary>
/// <remarks>
/// Aoe's double array (IEEE TSE 15(9), 1989), which sentencepiece reaches through darts-clone
/// for the same job. A step is <c>t = base[node] + code(c)</c>, valid when <c>check[t] == node</c>,
/// so every key starting at a position is found in one walk, where a hash probe per candidate
/// substring rehashed each of its characters. Codes go by descending frequency, which keeps
/// the arrays dense. Built once; thread-safe to read.
/// </remarks>
internal sealed class CharTrie
{
    /// <summary>The node every walk starts from.</summary>
    public const int Root = 0;

    private const int Free = -1;

    // Slot 0 is the root; its check holds a value no node id takes, so a code-0 step
    // (a character no key contains) landing on it is refused like any other mismatch.
    private const int RootCheck = -2;

    private readonly int[] _codes;
    private readonly int[] _base;
    private readonly int[] _check;
    private readonly int[] _values;

    /// <summary>Builds the trie; the value of a key is its index in <paramref name="keys"/>, the last index winning a repeated key.</summary>
    /// <param name="keys">The keys. A null key is refused.</param>
    /// <exception cref="ArgumentNullException"><paramref name="keys"/>, or one of its keys, is null.</exception>
    public CharTrie(IReadOnlyList<string> keys)
    {
        Guard.NotNull(keys);
        _codes = AssignCodes(keys, out int alphabet);

        var builder = new Builder(keys, _codes, alphabet);
        builder.Build();
        _base = builder.Base;
        _check = builder.Check;
        _values = builder.Values;
        Count = builder.DistinctKeys;
    }

    /// <summary>How many distinct keys the trie holds.</summary>
    public int Count { get; }

    /// <summary>The child of <paramref name="node"/> along <paramref name="c"/>, or <c>-1</c>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Step(int node, char c)
    {
        int code = c < _codes.Length ? _codes[c] : 0;
        int next = _base[node] + code;
        return (uint)next < (uint)_check.Length && _check[next] == node ? next : -1;
    }

    /// <summary>The value of the key ending at <paramref name="node"/>, or <c>-1</c> when no key ends there.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ValueAt(int node) => _values[node];

    /// <summary>Walks <paramref name="key"/> from <paramref name="node"/>, returning the node reached or <c>-1</c>.</summary>
    public int Walk(int node, ReadOnlySpan<char> key)
    {
        for (int i = 0; i < key.Length && node >= 0; i++)
        {
            node = Step(node, key[i]);
        }
        return node;
    }

    /// <summary>The value of exactly <paramref name="key"/>, or <c>-1</c> when it is absent.</summary>
    public int Find(ReadOnlySpan<char> key)
    {
        int node = Walk(Root, key);
        return node < 0 ? -1 : _values[node];
    }

    /// <summary>Codes 1..alphabet by descending frequency, ties by character; 0 marks a character no key holds.</summary>
    private static int[] AssignCodes(IReadOnlyList<string> keys, out int alphabet)
    {
        int maxChar = -1;
        for (int k = 0; k < keys.Count; k++)
        {
            string key = keys[k];
            Guard.NotNull(key);
            foreach (char c in key)
            {
                maxChar = Math.Max(maxChar, c);
            }
        }

        long[] frequency = new long[maxChar + 1];
        for (int k = 0; k < keys.Count; k++)
        {
            foreach (char c in keys[k])
            {
                frequency[c]++;
            }
        }

        var present = new List<int>();
        for (int c = 0; c <= maxChar; c++)
        {
            if (frequency[c] > 0)
            {
                present.Add(c);
            }
        }
        present.Sort((a, b) => frequency[a] != frequency[b] ? frequency[b].CompareTo(frequency[a]) : a.CompareTo(b));

        int[] codes = new int[maxChar + 1];
        for (int i = 0; i < present.Count; i++)
        {
            codes[present[i]] = i + 1;
        }
        alphabet = present.Count;
        return codes;
    }

    /// <summary>Places the keys breadth first, one node's children at a time.</summary>
    /// <remarks>
    /// Free slots are kept on a linked list, so a placement visits only slots it could use,
    /// and a slot that has refused <see cref="FailLimit"/> placements leaves the list for good:
    /// darts-clone abandons old blocks for the same reason, trading a few holes for a build
    /// that does not rescan a nearly full prefix for every node.
    /// </remarks>
    private sealed class Builder
    {
        private const int FailLimit = 16;
        private const int Unlinked = -2;

        private readonly string[] _keys;
        private readonly int[] _codes;
        private readonly int _alphabet;
        private int[] _next;
        private int[] _prev;
        private int[] _fails;
        private int _head = -1;
        private int _tail = -1;
        private int _used = 1;

        public Builder(IReadOnlyList<string> keys, int[] codes, int alphabet)
        {
            // Copied once: every node reads keys by index, and through the interface each read was a dispatch.
            _keys = [.. keys];
            _codes = codes;
            _alphabet = alphabet;
            Base = [];
            Check = [];
            Values = [];
            _next = [];
            _prev = [];
            _fails = [];
            // No trie has more nodes than its keys have characters, so this rarely grows again.
            long characters = 0;
            foreach (string key in _keys)
            {
                characters += key.Length;
            }
            Grow((int)Math.Min(int.MaxValue / 2, Math.Max(16, characters + alphabet + 1)));
            Unlink(Root);
            Check[Root] = RootCheck;
        }

        public int[] Base { get; private set; }

        public int[] Check { get; private set; }

        public int[] Values { get; private set; }

        public int DistinctKeys { get; private set; }

        public void Build()
        {
            int[] order = new int[_keys.Length];
            for (int i = 0; i < order.Length; i++)
            {
                order[i] = i;
            }
            long[] sortKeys = new long[order.Length];

            // (node, lo, hi, depth): order[lo..hi) share their first depth characters.
            var pending = new Queue<(int Node, int Lo, int Hi, int Depth)>();
            pending.Enqueue((Root, 0, order.Length, 0));
            var children = new List<(int Code, int Lo, int Hi)>();
            while (pending.Count > 0)
            {
                (int node, int lo, int hi, int depth) = pending.Dequeue();
                if (hi - lo == 1)
                {
                    PlaceTail(node, order[lo], depth);
                    continue;
                }
                SortByCharAt(order, sortKeys, lo, hi, depth);
                int at = TakeKeysEndingAt(node, order, lo, hi, depth);
                GroupChildren(children, order, at, hi, depth);
                if (children.Count == 0)
                {
                    continue;
                }

                int nodeBase = Place(children);
                Base[node] = nodeBase;
                foreach ((int code, int childLo, int childHi) in children)
                {
                    int child = nodeBase + code;
                    Occupy(child, node);
                    pending.Enqueue((child, childLo, childHi, depth + 1));
                }
            }

            Trim();
        }

        /// <summary>Gives <paramref name="node"/> the value of the keys that end at it, returning where the longer keys start.</summary>
        /// <remarks>Keys that end here sort first, in index order, so the last of them is the one that wins.</remarks>
        private int TakeKeysEndingAt(int node, int[] order, int lo, int hi, int depth)
        {
            int at = lo;
            while (at < hi && _keys[order[at]].Length == depth)
            {
                at++;
            }
            if (at > lo)
            {
                Values[node] = order[at - 1];
                DistinctKeys++;
            }
            return at;
        }

        /// <summary>Splits the sorted <c>order[at..hi)</c> into one run per character at <paramref name="depth"/>, each run a child.</summary>
        private void GroupChildren(List<(int Code, int Lo, int Hi)> children, int[] order, int at, int hi, int depth)
        {
            children.Clear();
            while (at < hi)
            {
                char c = _keys[order[at]][depth];
                int end = at + 1;
                while (end < hi && _keys[order[end]][depth] == c)
                {
                    end++;
                }
                children.Add((_codes[c], at, end));
                at = end;
            }
        }

        /// <summary>Places the rest of a key no other key shares a prefix with, one single-child node per character.</summary>
        /// <remarks>
        /// A lone child fits at the first free slot at or past its code, so a tail needs no
        /// sort, no child list and no queue entry per character.
        /// </remarks>
        private void PlaceTail(int node, int key, int depth)
        {
            string text = _keys[key];
            for (int d = depth; d < text.Length; d++)
            {
                int code = _codes[text[d]];
                int slot = _head;
                while (true)
                {
                    if (slot < 0)
                    {
                        slot = Grow(Math.Max(Check.Length * 2, code + 1));
                    }
                    if (slot >= code)
                    {
                        break;
                    }
                    int following = _next[slot];
                    if (++_fails[slot] >= FailLimit)
                    {
                        Unlink(slot);
                    }
                    slot = following;
                }
                Base[node] = slot - code;
                Occupy(slot, node);
                node = slot;
            }
            Values[node] = key;
            DistinctKeys++;
        }

        private void Occupy(int slot, int parent)
        {
            Check[slot] = parent;
            Unlink(slot);
            _used = Math.Max(_used, slot + 1);
        }

        /// <summary>Orders <c>order[lo..hi)</c> by the character at <paramref name="depth"/>, keys ending before it first, ties by index.</summary>
        /// <remarks>
        /// One primitive sort per node over a packed key, rather than one ordinal string sort
        /// up front whose every comparison re-reads the prefix the keys share.
        /// </remarks>
        private void SortByCharAt(int[] order, long[] sortKeys, int lo, int hi, int depth)
        {
            if (hi - lo < 2)
            {
                return;
            }
            for (int k = lo; k < hi; k++)
            {
                string key = _keys[order[k]];
                long rank = key.Length > depth ? key[depth] + 1L : 0L;
                sortKeys[k] = (rank << 32) | (uint)order[k];
            }
            Array.Sort(sortKeys, lo, hi - lo);
            for (int k = lo; k < hi; k++)
            {
                order[k] = (int)sortKeys[k];
            }
        }

        /// <summary>A base that fits every child code, found by walking the free list.</summary>
        private int Place(List<(int Code, int Lo, int Hi)> children)
        {
            int minCode = int.MaxValue;
            int maxCode = 0;
            foreach ((int code, _, _) in children)
            {
                minCode = Math.Min(minCode, code);
                maxCode = Math.Max(maxCode, code);
            }

            // slot is where the lowest child would land. Running off the list grows the
            // arrays, whose new slots are all free, so a base is always found.
            int slot = _head;
            while (true)
            {
                if (slot < 0)
                {
                    slot = Grow(Check.Length * 2);
                }
                int candidate = slot - minCode;
                if (candidate >= 0)
                {
                    if (candidate + maxCode >= Check.Length)
                    {
                        Grow(Math.Max(Check.Length * 2, candidate + maxCode + 1));
                    }
                    if (Fits(candidate, children))
                    {
                        return candidate;
                    }
                }
                // A slot below the lowest code counts as a refusal too, or a wide alphabet's
                // rare characters would walk every low hole on every placement.
                int following = _next[slot];
                if (++_fails[slot] >= FailLimit)
                {
                    Unlink(slot);
                }
                slot = following;
            }
        }

        private bool Fits(int candidate, List<(int Code, int Lo, int Hi)> children)
        {
            foreach ((int code, _, _) in children)
            {
                if (Check[candidate + code] != Free)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Grows every array to <paramref name="capacity"/> and appends the new slots to the free list, returning the first of them.</summary>
        private int Grow(int capacity)
        {
            int old = Check.Length;
            Base = Resized(Base, capacity, 0);
            Check = Resized(Check, capacity, Free);
            Values = Resized(Values, capacity, -1);
            _next = Resized(_next, capacity, -1);
            _prev = Resized(_prev, capacity, -1);
            _fails = Resized(_fails, capacity, 0);
            for (int slot = old; slot < capacity; slot++)
            {
                _prev[slot] = _tail;
                if (_tail >= 0)
                {
                    _next[_tail] = slot;
                }
                else
                {
                    _head = slot;
                }
                _tail = slot;
            }
            return old;
        }

        private void Unlink(int slot)
        {
            int before = _prev[slot];
            if (before == Unlinked)
            {
                return;
            }
            int after = _next[slot];
            if (before >= 0)
            {
                _next[before] = after;
            }
            else
            {
                _head = after;
            }
            if (after >= 0)
            {
                _prev[after] = before;
            }
            else
            {
                _tail = before;
            }
            _prev[slot] = Unlinked;
        }

        private static int[] Resized(int[] array, int capacity, int fill)
        {
            int[] grown = new int[capacity];
            Array.Copy(array, grown, array.Length);
            grown.AsSpan(array.Length).Fill(fill);
            return grown;
        }

        /// <summary>Drops the unused tail, keeping room for every code past the highest base so a step never reads outside the arrays.</summary>
        private void Trim()
        {
            int maxBase = 0;
            for (int i = 0; i < _used; i++)
            {
                maxBase = Math.Max(maxBase, Base[i]);
            }
            int size = Math.Max(_used, maxBase + _alphabet + 1);
            if (size > Check.Length)
            {
                Grow(size);
            }
            else if (size < Check.Length)
            {
                Base = Base.AsSpan(0, size).ToArray();
                Check = Check.AsSpan(0, size).ToArray();
                Values = Values.AsSpan(0, size).ToArray();
            }
        }
    }
}
