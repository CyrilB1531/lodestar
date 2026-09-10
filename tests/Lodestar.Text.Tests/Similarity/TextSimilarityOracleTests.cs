using System.Text.Json;
using Lodestar.Text.Similarity;
using Xunit;

namespace Lodestar.Text.Tests.Similarity;

/// <summary>MinHash, SimHash and the LSH banding against their frozen references.</summary>
/// <remarks>
/// Signatures and fingerprints are compared <strong>exactly</strong>: they are hashes, so a
/// tolerance would mean the algorithm had diverged rather than the arithmetic. The Jaccard
/// estimate is a ratio of two integers and is compared exactly for the same reason.
/// </remarks>
public sealed class TextSimilarityOracleTests
{
    private static readonly JsonDocument Corpus = LoadRaw("text_similarity.json");

    private static JsonDocument LoadRaw(string name)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", name);
        return JsonDocument.Parse(File.ReadAllText(path));
    }

    private static MinHashPermutations Permutations()
    {
        ulong[] multipliers = [.. Corpus.RootElement.GetProperty("multipliers")
            .EnumerateArray().Select(value => value.GetUInt64())];
        ulong[] addends = [.. Corpus.RootElement.GetProperty("addends")
            .EnumerateArray().Select(value => value.GetUInt64())];
        return new MinHashPermutations(multipliers, addends);
    }

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    private static string[] Tokens(JsonElement document) =>
        [.. document.GetProperty("tokens").EnumerateArray().Select(token => token.GetString()!)];

    private static JsonElement Document(string key) =>
        Cases.First(document => document.GetProperty("key").GetString() == key);

    public static TheoryData<string> Keys()
    {
        var data = new TheoryData<string>();
        foreach (JsonElement document in Cases)
        {
            data.Add(document.GetProperty("key").GetString()!);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Keys))]
    public void Every_signature_matches_datasketch(string key)
    {
        JsonElement document = Document(key);
        uint[] expected = [.. document.GetProperty("signature")
            .EnumerateArray().Select(value => value.GetUInt32())];

        uint[] actual = new MinHash(Permutations()).Signature(Tokens(document));

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(Keys))]
    public void Every_fingerprint_matches_simhash(string key)
    {
        JsonElement document = Document(key);
        ulong expected = document.GetProperty("simHash").GetUInt64();

        Assert.Equal(expected, SimHash.Fingerprint(Tokens(document)));
    }

    public static TheoryData<int> PairIndices()
    {
        var data = new TheoryData<int>();
        int count = Corpus.RootElement.GetProperty("pairs").GetArrayLength();
        for (int i = 0; i < count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(PairIndices))]
    public void Every_pair_agrees_on_jaccard_and_hamming(int index)
    {
        JsonElement pair = Corpus.RootElement.GetProperty("pairs")[index];
        JsonElement left = Document(pair.GetProperty("left").GetString()!);
        JsonElement right = Document(pair.GetProperty("right").GetString()!);
        var hasher = new MinHash(Permutations());

        uint[] leftSignature = hasher.Signature(Tokens(left));
        uint[] rightSignature = hasher.Signature(Tokens(right));

        Assert.Equal(pair.GetProperty("jaccard").GetDouble(),
            MinHash.Jaccard(leftSignature, rightSignature));
        Assert.Equal(pair.GetProperty("hamming").GetInt32(),
            SimHash.HammingDistance(
                SimHash.Fingerprint(Tokens(left)), SimHash.Fingerprint(Tokens(right))));
    }

    public static TheoryData<int> BandingIndices()
    {
        var data = new TheoryData<int>();
        int count = Corpus.RootElement.GetProperty("bandings").GetArrayLength();
        for (int i = 0; i < count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(BandingIndices))]
    public void Every_banding_solve_matches_the_reference(int index)
    {
        JsonElement frozen = Corpus.RootElement.GetProperty("bandings")[index];

        LshBanding solved = LshBanding.Solve(
            frozen.GetProperty("threshold").GetDouble(),
            frozen.GetProperty("permutations").GetInt32());

        Assert.Equal(frozen.GetProperty("bands").GetInt32(), solved.Bands);
        Assert.Equal(frozen.GetProperty("rowsPerBand").GetInt32(), solved.RowsPerBand);
    }

    [Fact]
    public void The_same_set_in_another_order_gives_the_same_signature()
    {
        // Documents a and e hold the same tokens in a different order, and the corpus
        // freezes both, so this claim is replayed rather than merely asserted.
        var hasher = new MinHash(Permutations());

        Assert.Equal(
            hasher.Signature(Tokens(Document("a"))),
            hasher.Signature(Tokens(Document("e"))));
    }

    [Fact]
    public void A_repeated_token_moves_simhash_and_not_minhash()
    {
        // Document f repeats a token that document a holds once. A minimum is idempotent so
        // MinHash cannot see it, where SimHash sums weights and does.
        var hasher = new MinHash(Permutations());

        Assert.Equal(
            hasher.Signature(Tokens(Document("a"))),
            hasher.Signature(Tokens(Document("f"))));
        Assert.NotEqual(
            SimHash.Fingerprint(Tokens(Document("a"))),
            SimHash.Fingerprint(Tokens(Document("f"))));
    }

    [Fact]
    public void A_disjoint_pair_estimates_zero()
    {
        var hasher = new MinHash(Permutations());

        Assert.Equal(0.0, MinHash.Jaccard(
            hasher.Signature(Tokens(Document("a"))),
            hasher.Signature(Tokens(Document("d")))));
    }
}
