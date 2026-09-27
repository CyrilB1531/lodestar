using Lodestar.Abstractions;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

// S2245 / CA5394: every draw here is seeded test data, so a failing sequence replays; nothing is security-sensitive.
#pragma warning disable S2245, CA5394

/// <summary>
/// Random writes and searches replayed against the collection and against a from-scratch rebuild
/// over a dictionary of the same records: an <see cref="EmbeddingIndex"/>, a
/// <see cref="CountVectorizer"/> fit, a <see cref="Bm25Index"/> and <see cref="RankFusion.Rrf"/>,
/// which is what every search did before writes became incremental (#1214).
/// </summary>
/// <remarks>
/// Compared exactly: same records, same order, same score bits. Small alphabets make ties, zero
/// vectors, termless texts and near-universal terms common; the dictionary's order breaks ties.
/// </remarks>
public sealed class CollectionDifferentialTests
{
    private const int Sequences = 300;
    private const int OperationsPerSequence = 40;

    private static readonly string[] Words = ["cat", "dog", "bird", "fish", "the", "a", "x", "sun", "moon"];

    private static readonly LodestarVectorStoreOptions[] Configurations =
    [
        new(),
        new() { Vectorizer = new CountVectorizerOptions { Binary = true }, RankFusionK = 1 },
        new() { Vectorizer = new CountVectorizerOptions { MinDf = 2 } },
        new() { Vectorizer = new CountVectorizerOptions { MaxDf = 0.5 }, Bm25 = new Bm25Options(Idf: Bm25Idf.Lucene) },
        new() { Vectorizer = new CountVectorizerOptions { MinDf = 0.3, MaxDf = 3 }, RankFusionK = 5 },
        new() { Vectorizer = new CountVectorizerOptions { MinDf = 2, MaxDf = 0.5 } },
        new() { Bm25 = new Bm25Options(K1: 0.5, B: 0.3, Epsilon: 0.5) },
        new() { Vectorizer = new CountVectorizerOptions { NgramRange = (1, 2) }, Bm25 = new Bm25Options(B: 1.0) },
        new() { RankFusionK = 2 },
    ];

    [Fact]
    public async Task Incremental_writes_answer_every_search_as_a_rebuild_would()
    {
        int searches = 0;
        for (int sequence = 0; sequence < Sequences; sequence++)
        {
            searches += await Replay(sequence);
        }

        // A replay that compared nothing would pass too; this many comparisons is the evidence it did not.
        Assert.True(searches > Sequences * OperationsPerSequence / 4, $"only {searches} searches were compared");
    }

    private static async Task<int> Replay(int sequence)
    {
        // A third of the sequences hold up to 48 records, deep enough that fusion reads past its first depth.
        var random = new Draws(1214 + sequence, sequence % 3 == 0 ? 48 : 12);
        LodestarVectorStoreOptions options = Configurations[sequence % Configurations.Length];
        using var collection = new LodestarVectorStoreCollection<string, Document>("differential", options);
        var reference = new Dictionary<string, Document>();
        int searches = 0;

        for (int step = 0; step < OperationsPerSequence; step++)
        {
            string context = $"sequence {sequence}, step {step}";
            int roll = random.Next(100);
            if (roll < 65)
            {
                await Write(collection, reference, random, roll);
            }
            else if (roll < 80)
            {
                await CompareVectorSearch(collection, reference, random, context);
                searches++;
            }
            else if (roll < 96)
            {
                await CompareHybridSearch(collection, reference, options, random, context);
                searches++;
            }
            else
            {
                HashSet<string> admitted = RandomSubset(random);
                int skip = random.Next(3);
                List<string> expected = [.. reference.Values.Where(d => admitted.Contains(d.Id)).Skip(skip).Take(4).Select(d => d.Id)];
                List<Document> actual = await collection
                    .GetAsync(d => admitted.Contains(d.Id), 4, new FilteredRecordRetrievalOptions<Document> { Skip = skip })
                    .ToListAsync();
                Assert.True(expected.SequenceEqual(actual.Select(d => d.Id)), $"{context}: GetAsync order");
            }
        }

        return searches;
    }

    /// <summary>One random write, applied to the collection and to the dictionary alike.</summary>
    private static async Task Write(
        LodestarVectorStoreCollection<string, Document> collection,
        Dictionary<string, Document> reference,
        Draws random,
        int roll)
    {
        if (roll < 35)
        {
            Document record = RandomRecord(random);
            await collection.UpsertAsync(record);
            reference[record.Id] = record;
        }
        else if (roll < 45)
        {
            Document[] batch = [.. Enumerable.Range(0, random.Next(1, 5)).Select(_ => RandomRecord(random))];
            await collection.UpsertAsync(batch);
            foreach (Document record in batch)
            {
                reference[record.Id] = record;
            }
        }
        else if (roll < 58)
        {
            string key = RandomKey(random);
            await collection.DeleteAsync(key);
            reference.Remove(key);
        }
        else if (roll < 63)
        {
            string[] keys = [.. Enumerable.Range(0, random.Next(1, 4)).Select(_ => RandomKey(random))];
            await collection.DeleteAsync(keys);
            foreach (string key in keys)
            {
                reference.Remove(key);
            }
        }
        else
        {
            await collection.EnsureCollectionDeletedAsync();
            reference.Clear();
        }
    }

    private static async Task CompareVectorSearch(
        LodestarVectorStoreCollection<string, Document> collection,
        Dictionary<string, Document> reference,
        Draws random,
        string context)
    {
        float[] query = RandomQuery(random);
        int top = random.Next(1, 6);
        var settings = new VectorSearchOptions<Document> { Skip = random.Next(3) };
        HashSet<string>? admitted = null;
        if (random.Next(3) == 0)
        {
            HashSet<string> subset = RandomSubset(random);
            admitted = subset;
            settings.Filter = d => subset.Contains(d.Id);
        }

        if (random.Next(4) == 0)
        {
            settings.ScoreThreshold = (random.NextDouble() * 2) - 1;
        }

        List<(string, double?)> expected = [.. ReferenceVector(reference, query)
            .Where(hit => admitted is null || admitted.Contains(hit.Record.Id))
            .Where(hit => settings.ScoreThreshold is not { } threshold || hit.Score >= threshold)
            .Skip(settings.Skip)
            .Take(top)
            .Select(hit => (hit.Record.Id, (double?)hit.Score))];

        List<VectorSearchResult<Document>> actual = await collection.SearchAsync(query, top, settings).ToListAsync();
        Assert.True(
            expected.SequenceEqual(actual.Select(hit => (hit.Record.Id, hit.Score))),
            $"{context}: vector search\n expected {Show(expected)}\n actual   {Show(actual)}");
    }

    private static async Task CompareHybridSearch(
        LodestarVectorStoreCollection<string, Document> collection,
        Dictionary<string, Document> reference,
        LodestarVectorStoreOptions options,
        Draws random,
        string context)
    {
        float[] query = RandomQuery(random);
        string[] keywords = [.. Enumerable.Range(0, random.Next(4)).Select(_ => random.Next(8) == 0 ? "zebra" : Words[random.Next(Words.Length)])];
        int top = random.Next(1, 6);
        var settings = new HybridSearchOptions<Document> { Skip = random.Next(3) };
        HashSet<string>? admitted = null;
        if (random.Next(3) == 0)
        {
            HashSet<string> subset = RandomSubset(random);
            admitted = subset;
            settings.Filter = d => subset.Contains(d.Id);
        }

        List<(string, double?)> expected;
        try
        {
            expected = ReferenceHybrid(reference, options, query, keywords, admitted, settings.Skip, top);
        }
        catch (InvalidOperationException)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await collection.HybridSearchAsync(query, keywords, top, settings).ToListAsync());
            return;
        }

        List<VectorSearchResult<Document>> actual = await collection.HybridSearchAsync(query, keywords, top, settings).ToListAsync();
        Assert.True(
            expected.SequenceEqual(actual.Select(hit => (hit.Record.Id, hit.Score))),
            $"{context}: hybrid search [{string.Join(" ", keywords)}]\n expected {Show(expected)}\n actual   {Show(actual)}");
    }

    /// <summary>Every record by the whole vector ranking, as the rebuilt <see cref="EmbeddingIndex"/> ranked it.</summary>
    private static List<(Document Record, double Score)> ReferenceVector(Dictionary<string, Document> reference, float[] query)
    {
        List<Document> held = [.. reference.Values];
        if (held.Count == 0)
        {
            return [];
        }

        EmbeddingIndex index = Index(held);
        return [.. index.Search(query, held.Count).Select(hit => (held[hit.Index], (double)hit.Score))];
    }

    private static List<(string, double?)> ReferenceHybrid(
        Dictionary<string, Document> reference,
        LodestarVectorStoreOptions options,
        float[] query,
        string[] keywords,
        HashSet<string>? admitted,
        int skip,
        int top)
    {
        List<Document> held = [.. reference.Values];
        if (held.Count == 0)
        {
            return [];
        }

        int[] byVector = [.. Index(held).Search(query, held.Count).Select(hit => hit.Index)];
        int[] byKeyword = ReferenceKeywordRanking(held, options, keywords);
        return [.. RankFusion.Rrf([byVector, byKeyword], options.RankFusionK)
            .Select(hit => (held[hit.Document], hit.Score))
            .Where(hit => admitted is null || admitted.Contains(hit.Item1.Id))
            .Skip(skip)
            .Take(top)
            .Select(hit => (hit.Item1.Id, (double?)hit.Score))];
    }

    /// <summary>The records the keywords match, by <see cref="Bm25Index.Score"/> descending then index: the rebuilt ranking.</summary>
    private static int[] ReferenceKeywordRanking(List<Document> held, LodestarVectorStoreOptions options, string[] keywords)
    {
        CountVectorizerOptions vectorizerOptions = options.Vectorizer ?? new CountVectorizerOptions();
        var vectorizer = new CountVectorizer(vectorizerOptions);
        CsrMatrix counts;
        try
        {
            counts = vectorizer.FitTransform([.. held.Select(d => d.Text)]);
        }
        catch (InvalidOperationException) when (!BoundsCross(vectorizerOptions, held.Count))
        {
            return [];
        }

        var scorer = new Bm25Index(counts, options.Bm25);
        CsrMatrix row = vectorizer.Transform([string.Join(" ", keywords)]);
        HashSet<int> terms = [.. row.ColumnIndices];
        List<int> matched = [.. Enumerable.Range(0, counts.RowCount).Where(document =>
            Enumerable.Range(counts.RowPointers[document], counts.RowPointers[document + 1] - counts.RowPointers[document])
                .Any(k => terms.Contains(counts.ColumnIndices[k])))];
        double[] scores = scorer.Score(terms.OrderBy(term => term));
        return [.. matched.OrderByDescending(document => scores[document]).ThenBy(document => document)];
    }

    private static bool BoundsCross(CountVectorizerOptions vectorizer, int documents)
    {
        double low = vectorizer.MinDf is > 0 and < 1 ? vectorizer.MinDf * documents : vectorizer.MinDf;
        double high = vectorizer.MaxDf <= 1.0 ? vectorizer.MaxDf * documents : vectorizer.MaxDf;
        return high < low;
    }

    private static EmbeddingIndex Index(List<Document> held)
    {
        var block = new float[held.Count * 3];
        for (int i = 0; i < held.Count; i++)
        {
            held[i].Embedding.Span.CopyTo(block.AsSpan(i * 3, 3));
        }

        return EmbeddingIndex.FromOwnedBlock(block, 3, BlockNormalization.Normalize);
    }

    private static Document RandomRecord(Draws random) => new()
    {
        Id = RandomKey(random),
        Text = string.Join(" ", Enumerable.Range(0, random.Next(6)).Select(_ => Words[random.Next(Words.Length)])),
        Embedding = RandomVector(random),
    };

    private static string RandomKey(Draws random) => $"k{random.Next(random.Keys)}";

    private static float[] RandomVector(Draws random) =>
        [.. Enumerable.Range(0, 3).Select(_ => (float)(random.Next(5) - 2) * (random.Next(4) == 0 ? 0.5f : 1f))];

    // Norms 0, 1, 3, 5, 7 and 9: a whole norm divides alike in float and in double, so the scores do not
    // hang on whether the reference EmbeddingIndex normalizes a query in float, as 0.8.0 does, or in double.
    private static readonly float[][] Queries =
        [[0, 0, 0], [1, 0, 0], [0, -1, 0], [1, 2, 2], [-2, 1, 2], [2, -2, -1], [0, 3, 4], [4, 0, -3], [2, 3, 6], [-6, 2, 3], [4, 4, 7], [1, -4, 8]];

    private static float[] RandomQuery(Draws random) => Queries[random.Next(Queries.Length)];

    private static HashSet<string> RandomSubset(Draws random) =>
        [.. Enumerable.Range(0, random.Keys).Where(_ => random.Next(2) == 0).Select(i => $"k{i}")];

    private static string Show(IEnumerable<(string, double?)> hits) => string.Join(", ", hits.Select(h => $"{h.Item1}={h.Item2:R}"));

    private static string Show(IEnumerable<VectorSearchResult<Document>> hits) =>
        Show(hits.Select(hit => (hit.Record.Id, hit.Score)));

    /// <summary>A seeded generator that also carries how many distinct keys its sequence draws from.</summary>
    private sealed class Draws(int seed, int keys) : Random(seed)
    {
        public int Keys { get; } = keys;
    }
}
#pragma warning restore S2245, CA5394
