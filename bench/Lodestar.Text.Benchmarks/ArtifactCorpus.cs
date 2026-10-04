using System.Globalization;
using System.Text;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Text.Benchmarks;

/// <summary>The generated vectorizers and indexes the artifact benchmarks save and load.</summary>
internal static class ArtifactCorpus
{
    /// <summary><paramref name="count"/> terms of nine characters, in ordinal order, one document.</summary>
    public static string Terms(int count) =>
        string.Join(" ", Enumerable.Range(0, count).Select(i => "t" + i.ToString("D8", CultureInfo.InvariantCulture)));

    /// <summary>Document <paramref name="index"/> of a corpus of 20,000-token documents over 5,000 words.</summary>
    public static string LongDocument(int index) =>
        string.Join(" ", Enumerable.Range(0, 20_000).Select(w => "w" + ((index * 31 + w * 7) % 5_000).ToString(CultureInfo.InvariantCulture)));

    public static CountVectorizer Count(int terms) => new CountVectorizer().Fit([Terms(terms)]);

    /// <summary>An index of 16 dimensions with one id an item.</summary>
    public static EmbeddingIndex Index(int items)
    {
        var index = new EmbeddingIndex(16, normalize: false);
        var vector = new float[16];
        for (int i = 0; i < items; i++)
        {
            vector[i % 16] = i;
            index.Add(vector, "id" + i.ToString(CultureInfo.InvariantCulture));
        }
        return index;
    }

    /// <summary>Loads <paramref name="artifact"/> asynchronously from a stream of undeclared length.</summary>
    public static async Task<CountVectorizer> LoadAsync(byte[] artifact)
    {
        using var pipe = new UndeclaredStream(artifact);
        return await CountVectorizer.LoadAsync(pipe).ConfigureAwait(false);
    }

    /// <summary>Loads <paramref name="artifact"/> from a stream of undeclared length.</summary>
    public static CountVectorizer Load(byte[] artifact)
    {
        using var pipe = new UndeclaredStream(artifact);
        return CountVectorizer.Load(pipe);
    }

    public static byte[] Bytes(CountVectorizer vectorizer)
    {
        using var stream = new MemoryStream();
        vectorizer.Save(stream);
        return stream.ToArray();
    }

    /// <summary>The artifact <see cref="Count"/> would save, its vocabulary written into a two-term one's.</summary>
    public static byte[] SplicedCount(int terms)
    {
        const string TwoTerms = "\"featureCount\":2,\"vocabulary\":[\"t00000000\",\"t00000001\"]";
        string template = Encoding.UTF8.GetString(Bytes(Count(2)));
        if (!template.Contains(TwoTerms, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The two-term artifact no longer writes its count and vocabulary as this splices them.");
        }

        string vocabulary = "\"featureCount\":" + terms.ToString(CultureInfo.InvariantCulture) + ",\"vocabulary\":[\""
            + Terms(terms).Replace(" ", "\",\"", StringComparison.Ordinal) + "\"]";
        byte[] spliced = Encoding.UTF8.GetBytes(template.Replace(TwoTerms, vocabulary, StringComparison.Ordinal));
        return spliced;
    }
}
