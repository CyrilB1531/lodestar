using Xunit;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Embeddings.Tests;

/// <summary>
/// The double-array trie the unigram and WordPiece tokenizers walk in place of probing a
/// span-keyed table once per candidate substring.
/// </summary>
public class CharTrieTests
{
    [Fact]
    public void A_present_key_is_found_with_its_index()
    {
        var trie = new CharTrie(["ab", "abc", "b"]);
        Assert.Equal(1, trie.Find("abc".AsSpan()));
        Assert.Equal(0, trie.Find("ab".AsSpan()));
        Assert.Equal(2, trie.Find("b".AsSpan()));
    }

    [Fact]
    public void An_absent_key_and_a_bare_prefix_are_not_found()
    {
        var trie = new CharTrie(["abc"]);
        Assert.Equal(-1, trie.Find("abd".AsSpan()));
        Assert.Equal(-1, trie.Find("ab".AsSpan()));
        Assert.Equal(-1, trie.Find("abcd".AsSpan()));
        Assert.Equal(-1, trie.Find("z".AsSpan()));
    }

    [Fact]
    public void A_slice_of_a_longer_string_is_found_without_copying_it()
    {
        var trie = new CharTrie(["cat", "dog"]);
        Assert.Equal(0, trie.Find("thecatsat".AsSpan(3, 3)));
    }

    [Fact]
    public void A_walk_from_a_prefix_node_finds_the_continuation()
    {
        var trie = new CharTrie(["##ing", "ing"]);
        int prefix = trie.Walk(CharTrie.Root, "##".AsSpan());
        Assert.True(prefix >= 0);
        Assert.Equal(-1, trie.ValueAt(prefix));
        Assert.Equal(0, trie.ValueAt(trie.Walk(prefix, "ing".AsSpan())));
    }

    [Fact]
    public void Stepping_enumerates_every_key_along_the_text_shortest_first()
    {
        var trie = new CharTrie(["a", "abc", "ab", "b"]);
        var found = new List<int>();
        int node = CharTrie.Root;
        foreach (char c in "abcd")
        {
            node = trie.Step(node, c);
            if (node < 0)
            {
                break;
            }
            if (trie.ValueAt(node) >= 0)
            {
                found.Add(trie.ValueAt(node));
            }
        }
        Assert.Equal([0, 2, 1], found);
    }

    [Fact]
    public void The_empty_key_is_a_key_like_any_other()
    {
        var trie = new CharTrie([string.Empty, "a"]);
        Assert.Equal(0, trie.Find([]));
        Assert.Equal(1, trie.Find("a".AsSpan()));
    }

    [Fact]
    public void An_empty_trie_finds_nothing()
    {
        var trie = new CharTrie([]);
        Assert.Equal(0, trie.Count);
        Assert.Equal(-1, trie.Find("a".AsSpan()));
        Assert.Equal(-1, trie.Find([]));
        Assert.Equal(-1, trie.Step(CharTrie.Root, char.MaxValue));
    }

    [Fact]
    public void A_repeated_key_keeps_the_last_index_as_an_indexer_assignment_would()
    {
        var trie = new CharTrie(["a", "b", "a"]);
        Assert.Equal(2, trie.Count);
        Assert.Equal(2, trie.Find("a".AsSpan()));
    }

    [Fact]
    public void A_character_no_key_holds_never_steps_even_from_a_leaf()
    {
        // A leaf's base is zero, so a step on a character no key holds lands on the root's
        // own slot: its check must refuse it, or the walk would go on from a childless node.
        string lowest = char.MinValue.ToString();
        string highest = char.MaxValue.ToString();
        var trie = new CharTrie(["a", lowest, highest]);
        int leaf = trie.Walk(CharTrie.Root, "a".AsSpan());
        Assert.Equal(-1, trie.Step(leaf, 'q'));
        Assert.Equal(-1, trie.Step(leaf, 'a'));
        Assert.Equal(1, trie.Find(lowest.AsSpan()));
        Assert.Equal(2, trie.Find(highest.AsSpan()));
    }

    [Fact]
    public void Every_key_of_a_large_trie_is_found_and_no_absent_one_is()
    {
        // Wide alphabets and shared prefixes, past the point where a first-fit placement
        // that overlapped two nodes' children would still answer correctly.
        const int Count = 5000;
        var keys = new string[Count];
        for (int i = 0; i < Count; i++)
        {
            keys[i] = $"piece{i}" + (char)(0x4E00 + (i % 700));
        }
        var trie = new CharTrie(keys);

        Assert.Equal(Count, trie.Count);
        for (int i = 0; i < Count; i++)
        {
            Assert.Equal(i, trie.Find(keys[i].AsSpan()));
            Assert.Equal(-1, trie.Find($"absent{i}".AsSpan()));
            Assert.Equal(-1, trie.Find(keys[i].AsSpan(0, keys[i].Length - 1)));
        }
    }

    [Fact]
    public void A_null_key_list_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => new CharTrie(null!));
    }

    [Fact]
    public void A_null_key_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => new CharTrie([null!]));
    }
}
