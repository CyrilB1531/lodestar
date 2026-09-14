using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Search;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus for this
// test; the sequence is fixed by the seed and nothing here is a security decision.
#pragma warning disable S2245, CA5394

/// <summary>
/// <see cref="Bm25Index.Top"/>'s bounded selection against the full sort it replaced (#751): every
/// document ordered by score descending and index ascending, then the first <c>count</c> kept.
/// </summary>
/// <remarks>
/// A small vocabulary makes ties common, which is where a heap keeping the wrong one of two equal
/// scores would show; counts of zero, one, the whole corpus and past it bound the selection.
/// </remarks>
public sealed class Bm25TopSelectionTests
{
    private static List<SearchHit> ByFullSort(Bm25Index index, int[] query, int count)
    {
        double[] scores = index.Score(query);
        int[] order = [.. Enumerable.Range(0, scores.Length)];
        Array.Sort(order, (left, right) =>
        {
            int byScore = scores[right].CompareTo(scores[left]);
            return byScore != 0 ? byScore : left.CompareTo(right);
        });

        return [.. order.Take(count).Select(document => new SearchHit(document, scores[document]))];
    }

    private static Bm25Index Corpus(Random random, int documents, int vocabulary, out int terms)
    {
        string[] words = [.. Enumerable.Range(0, vocabulary).Select(i => $"w{i:D2}")];
        string[] texts = [.. Enumerable.Range(0, documents).Select(_ =>
            string.Join(' ', Enumerable.Range(0, random.Next(1, 8)).Select(__ => words[random.Next(words.Length)])))];
        var vectorizer = new CountVectorizer();
        var index = new Bm25Index(vectorizer.FitTransform(texts));
        terms = vectorizer.GetFeatureNames().Count;
        return index;
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 10)]
    [InlineData(3, 257)]
    public void Random_queries_keep_what_the_full_sort_kept(int seed, int documents)
    {
        var random = new Random(seed);
        Bm25Index index = Corpus(random, documents, vocabulary: 6, out int terms);
        for (int trial = 0; trial < 200; trial++)
        {
            int[] query = [.. Enumerable.Range(0, random.Next(0, 4)).Select(_ => random.Next(-1, terms + 1))];
            int count = random.Next(4) switch
            {
                0 => 0,
                1 => 1,
                2 => documents + random.Next(0, 3),
                _ => random.Next(0, documents + 1),
            };

            Assert.Equal(ByFullSort(index, query, count), index.Top(query, count));
        }
    }

    [Fact]
    public void A_query_matching_nothing_returns_the_corpus_in_its_own_order()
    {
        Bm25Index index = Corpus(new Random(4), documents: 50, vocabulary: 6, out _);

        IReadOnlyList<SearchHit> hits = index.Top([-1], 5);

        Assert.Equal([0, 1, 2, 3, 4], hits.Select(hit => hit.Document));
        Assert.All(hits, hit => Assert.Equal(0.0, hit.Score));
    }

    [Fact]
    public void A_null_query_is_still_refused_when_nothing_is_asked_for() =>
        Assert.Throws<ArgumentNullException>(
            () => Corpus(new Random(5), documents: 3, vocabulary: 3, out _).Top(null!, 0));
}
