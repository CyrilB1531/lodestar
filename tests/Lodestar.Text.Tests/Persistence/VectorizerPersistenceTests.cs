using Lodestar.Abstractions;
using System.Text;
using Lodestar.Text.Persistence;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests.Persistence;

/// <summary>
/// Proves the round trip a persisted model exists for: fit here, score there.
/// </summary>
/// <remarks>
/// The comparisons are bit-exact, not tolerant. A tolerance would hide exactly
/// the failure that matters — a idf weight that lost its last mantissa bits on
/// the way through the file, producing scores that are almost right forever.
/// </remarks>
public sealed class VectorizerPersistenceTests
{
    private static readonly string[] TrainingCorpus =
    [
        "the quick brown fox jumps over the lazy dog",
        "the lazy dog sleeps all day long",
        "quick thinking beats slow planning every time",
        "a fox and a dog walk into a field",
        "planning the day beats improvising the day",
    ];

    private static readonly string[] HoldoutCorpus =
    [
        "the quick fox and the lazy dog",
        "slow planning beats no planning",
        "nothing here appears in the training corpus",
    ];

    /// <summary>Every option that changes behaviour, set away from its default.</summary>
    private static CountVectorizerOptions NonDefaultCountOptions => new()
    {
        Lowercase = false,
        StripAccents = true,
        Analyzer = AnalyzerKind.Word,
        NgramRange = (1, 2),
        MinDf = 0.2,
        MaxDf = 0.9,
        Binary = true,
        StopWords = ["the", "a", "over"],
        TokenPattern = @"\b\w\w+\b",
    };

    [Fact]
    public void Tfidf_save_load_transform_is_bit_exact()
    {
        var options = new TfidfVectorizerOptions
        {
            Count = NonDefaultCountOptions with { Binary = false },
            Tfidf = new TfidfOptions { UseIdf = true, SmoothIdf = false, SublinearTf = true, Norm = SparseNorm.L1 },
        };
        var original = new TfidfVectorizer(options).Fit(TrainingCorpus);
        CsrMatrix expected = original.Transform(HoldoutCorpus);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        TfidfVectorizer reloaded = TfidfVectorizer.Load(stream);

        AssertIdentical(expected, reloaded.Transform(HoldoutCorpus));
        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
        AssertIdenticalDoubles(original.Idf, reloaded.Idf);
    }

    [Fact]
    public void Tfidf_round_trip_preserves_every_option()
    {
        var options = new TfidfVectorizerOptions
        {
            Count = NonDefaultCountOptions,
            Tfidf = new TfidfOptions { UseIdf = false, SmoothIdf = false, SublinearTf = true, Norm = null },
        };
        var original = new TfidfVectorizer(options).Fit(TrainingCorpus);

        TfidfVectorizer reloaded = RoundTrip(original);

        // Behaviour is the observable proof: a vectorizer that lost Binary, the
        // n-gram range or the stop words would matrix the same documents differently.
        AssertIdentical(original.Transform(HoldoutCorpus), reloaded.Transform(HoldoutCorpus));
    }

    [Fact]
    public void Tfidf_round_trip_preserves_the_document_frequency_bounds()
    {
        // MinDf and MaxDf prune during Fit only, so a Load that dropped both leaves
        // every other test green. Re-fitting on a corpus they prune is what shows it.
        string[] corpus = ["alpha beta", "alpha gamma", "alpha delta", "beta epsilon"];
        var options = new TfidfVectorizerOptions { Count = new CountVectorizerOptions { MinDf = 0.5 } };
        TfidfVectorizer reloaded = RoundTrip(new TfidfVectorizer(options).Fit(TrainingCorpus));

        IReadOnlyList<string> refitted = reloaded.Fit(corpus).GetFeatureNames();

        Assert.Equal(new TfidfVectorizer(options).Fit(corpus).GetFeatureNames(), refitted);
        // A vectorizer that lost MinDf keeps the terms appearing in a single document.
        Assert.NotEqual(
            new TfidfVectorizer(new TfidfVectorizerOptions()).Fit(corpus).GetFeatureNames(),
            refitted);
    }

    [Fact]
    public void Tfidf_round_trip_survives_a_non_ascii_vocabulary()
    {
        // The relaxed JSON encoder emits non-ASCII as UTF-8, and every other corpus
        // here is a-z: this is the only test that would notice if it were wrong.
        string[] corpus =
        [
            "ação français über señor niño",
            "coração élève größe corazón città",
            "irmã déjà être où però ähnlich",
        ];
        var original = new TfidfVectorizer().Fit(corpus);

        TfidfVectorizer reloaded = RoundTrip(original);

        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
        Assert.Contains("ação", reloaded.GetFeatureNames());
        Assert.Contains("größe", reloaded.GetFeatureNames());
        AssertIdenticalDoubles(original.Idf, reloaded.Idf);
        AssertIdentical(original.Transform(corpus), reloaded.Transform(corpus));
    }

    [Fact]
    public void Tfidf_round_trip_survives_a_char_wb_analyzer()
    {
        var options = new TfidfVectorizerOptions
        {
            Count = new CountVectorizerOptions { Analyzer = AnalyzerKind.CharWordBoundary, NgramRange = (2, 3) },
        };
        var original = new TfidfVectorizer(options).Fit(TrainingCorpus);

        AssertIdentical(original.Transform(HoldoutCorpus), RoundTrip(original).Transform(HoldoutCorpus));
    }

    [Fact]
    public void Count_save_load_transform_is_bit_exact()
    {
        var original = new CountVectorizer(NonDefaultCountOptions).Fit(TrainingCorpus);
        CsrMatrix expected = original.Transform(HoldoutCorpus);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        CountVectorizer reloaded = CountVectorizer.Load(stream);

        AssertIdentical(expected, reloaded.Transform(HoldoutCorpus));
        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
    }

    [Theory]
    [InlineData(AnalyzerKind.Char, 1, 1, "\U0001F600")]
    [InlineData(AnalyzerKind.Char, 2, 2, "\U0001F600x")]
    [InlineData(AnalyzerKind.CharWordBoundary, 1, 3, "a\U0001F600 \U00020B9F\uFF76")]
    public void Count_round_trip_survives_an_astral_character_gram(AnalyzerKind analyzer, int min, int max, string document)
    {
        // A gram used to hold half a surrogate pair, which the writer turned into U+FFFD: two such
        // keys failed Load as duplicates, one lost its count (#1263).
        var options = new CountVectorizerOptions { Analyzer = analyzer, NgramRange = (min, max) };
        var original = new CountVectorizer(options).Fit([document]);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        CountVectorizer reloaded = CountVectorizer.Load(stream);

        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
        AssertIdentical(original.Transform([document]), reloaded.Transform([document]));
    }

    [Theory]
    [InlineData(AnalyzerKind.Word, 1, 2)]
    [InlineData(AnalyzerKind.Char, 1, 1)]
    [InlineData(AnalyzerKind.Char, 2, 3)]
    [InlineData(AnalyzerKind.CharWordBoundary, 1, 3)]
    public void Count_round_trip_survives_a_lone_surrogate_in_the_input(AnalyzerKind analyzer, int min, int max)
    {
        // Built in code: an attribute stores its strings as UTF-8, where a lone surrogate becomes U+FFFD.
        // The writer used to write it as U+FFFD too, so two terms saved as one key and Load refused them.
        string high = ((char)0xD800).ToString();
        string low = ((char)0xDC00).ToString();
        string[] corpus = ["ab" + high + "cd " + low + "x", low + high + "yz", "ab" + low + "cd \"q\"\\" + high];
        var options = new CountVectorizerOptions { Analyzer = analyzer, NgramRange = (min, max) };
        var original = new CountVectorizer(options).Fit(corpus);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        CountVectorizer reloaded = CountVectorizer.Load(stream);

        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
        AssertIdentical(original.Transform(corpus), reloaded.Transform(corpus));
    }

    [Fact]
    public void Count_round_trip_keeps_a_lone_surrogate_in_the_stop_words_and_the_pattern()
    {
        string high = ((char)0xDBFF).ToString();
        var options = new CountVectorizerOptions { StopWords = ["the", "x" + high], TokenPattern = "[^ ]+" + high + "?" };
        var original = new CountVectorizer(options).Fit(["the x" + high + " fox", "a fox x" + high]);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        CountVectorizer reloaded = CountVectorizer.Load(stream);

        Assert.Equal(original.Options, reloaded.Options);
        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
    }

    [Fact]
    public void A_vocabulary_python_escaped_loads_its_lone_surrogates()
    {
        // One term holds a lone surrogate beside an astral character, so its token carries a lone
        // escape and a pair escape together; json.dumps writes both in lowercase.
        string high = ((char)0xD800).ToString();
        var options = new CountVectorizerOptions { TokenPattern = "[^ ]+" };
        var original = new CountVectorizer(options).Fit(["a" + high + "\U0001F600 b"]);
        using var stream = new MemoryStream();
        original.Save(stream);
        string json = Encoding.UTF8.GetString(stream.ToArray());
        string bs = "\\";
        string written = bs + "uD800" + bs + "uD83D" + bs + "uDE00";
        string pythonShaped = json.Replace(written, bs + "ud800" + bs + "ud83d" + bs + "ude00", StringComparison.Ordinal);
        Assert.NotEqual(json, pythonShaped);

        CountVectorizer reloaded = CountVectorizer.Load(new MemoryStream(Encoding.UTF8.GetBytes(pythonShaped)));

        Assert.Equal(original.GetFeatureNames(), reloaded.GetFeatureNames());
    }

    [Fact]
    public void Hashing_round_trip_preserves_the_configuration()
    {
        var options = new HashingVectorizerOptions
        {
            Count = NonDefaultCountOptions,
            NumFeatures = 4096,
            AlternateSign = false,
            Norm = SparseNorm.L1,
        };
        var original = new HashingVectorizer(options);

        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        HashingVectorizer reloaded = HashingVectorizer.Load(stream);

        Assert.Equal(original.NumFeatures, reloaded.NumFeatures);
        AssertIdentical(original.Transform(HoldoutCorpus), reloaded.Transform(HoldoutCorpus));
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 1)]
    public void A_first_ngram_length_below_one_loads_back(int min, int max)
    {
        // The constructors accept it since #1065; the loader refused it (#1197).
        var count = new CountVectorizerOptions { NgramRange = (min, max) };
        var tfidf = new TfidfVectorizer(new TfidfVectorizerOptions { Count = count }).Fit(TrainingCorpus);
        var hashing = new HashingVectorizer(new HashingVectorizerOptions { Count = count });

        using var tfidfStream = new MemoryStream();
        tfidf.Save(tfidfStream);
        tfidfStream.Position = 0;
        using var hashingStream = new MemoryStream();
        hashing.Save(hashingStream);
        hashingStream.Position = 0;

        AssertIdentical(tfidf.Transform(HoldoutCorpus), TfidfVectorizer.Load(tfidfStream).Transform(HoldoutCorpus));
        AssertIdentical(hashing.Transform(HoldoutCorpus), HashingVectorizer.Load(hashingStream).Transform(HoldoutCorpus));
    }

    [Fact]
    public void Save_and_load_round_trip_through_a_file()
    {
        var original = new TfidfVectorizer(new TfidfVectorizerOptions { Count = NonDefaultCountOptions }).Fit(TrainingCorpus);
        string path = Path.Combine(Path.GetTempPath(), $"datanet-tfidf-{Guid.NewGuid():N}.json");
        try
        {
            original.Save(path);
            TfidfVectorizer reloaded = TfidfVectorizer.Load(path);
            AssertIdentical(original.Transform(HoldoutCorpus), reloaded.Transform(HoldoutCorpus));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task SaveAsync_and_LoadAsync_round_trip()
    {
        var original = new TfidfVectorizer(new TfidfVectorizerOptions { Count = NonDefaultCountOptions }).Fit(TrainingCorpus);

        using var stream = new MemoryStream();
        await original.SaveAsync(stream, cancellationToken: TestContext.Current.CancellationToken);
        stream.Position = 0;
        TfidfVectorizer reloaded = await TfidfVectorizer.LoadAsync(stream, cancellationToken: TestContext.Current.CancellationToken);

        AssertIdentical(original.Transform(HoldoutCorpus), reloaded.Transform(HoldoutCorpus));
    }

    [Fact]
    public async Task Async_and_sync_writers_produce_the_same_bytes()
    {
        var original = new TfidfVectorizer(new TfidfVectorizerOptions { Count = NonDefaultCountOptions }).Fit(TrainingCorpus);

        using var syncBytes = new MemoryStream();

        // SonarLint S6966: the synchronous Save is half of what this test compares.
        // Awaiting SaveAsync in its place would leave the sync writer unexercised
        // and assert that SaveAsync agrees with itself.
#pragma warning disable S6966
        original.Save(syncBytes);
#pragma warning restore S6966

        using var asyncBytes = new MemoryStream();
        await original.SaveAsync(asyncBytes, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(syncBytes.ToArray(), asyncBytes.ToArray());
    }

    [Fact]
    public void Save_leaves_the_caller_s_stream_open()
    {
        var original = new CountVectorizer().Fit(TrainingCorpus);

        using var stream = new MemoryStream();
        original.Save(stream);

        Assert.True(stream.CanWrite);
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public void Load_leaves_the_caller_s_stream_open()
    {
        var original = new CountVectorizer().Fit(TrainingCorpus);
        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;

        _ = CountVectorizer.Load(stream);

        Assert.True(stream.CanRead);
    }

    [Fact]
    public void Save_writes_utf8_without_a_byte_order_mark()
    {
        var original = new CountVectorizer().Fit(["café naïve"]);

        using var stream = new MemoryStream();
        original.Save(stream);
        byte[] bytes = stream.ToArray();

        Assert.False(bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF);
        Assert.StartsWith("{\"$schema\":\"datanet/count-vectorizer\",\"version\":1", Encoding.UTF8.GetString(bytes), StringComparison.Ordinal);
    }

    [Fact]
    public void Save_writes_featureCount_before_the_vocabulary()
    {
        var original = new CountVectorizer().Fit(TrainingCorpus);

        using var stream = new MemoryStream();
        original.Save(stream);
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // A reader must be able to size its buffers from a bounded count before it
        // meets the array that count describes.
        Assert.True(json.IndexOf("\"featureCount\"", StringComparison.Ordinal) < json.IndexOf("\"vocabulary\"", StringComparison.Ordinal));
    }

    [Fact]
    public void Save_is_byte_reproducible_across_calls()
    {
        var original = new CountVectorizer(NonDefaultCountOptions).Fit(TrainingCorpus);

        using var first = new MemoryStream();
        original.Save(first);
        using var second = new MemoryStream();
        original.Save(second);

        Assert.Equal(first.ToArray(), second.ToArray());
    }

    [Fact]
    public void Saving_an_unfitted_vectorizer_is_rejected()
    {
        using var stream = new MemoryStream();

        Assert.Throws<InvalidOperationException>(() => new CountVectorizer().Save(stream));
        Assert.Throws<InvalidOperationException>(() => new TfidfVectorizer().Save(stream));
    }

    [Fact]
    public void Loading_an_artifact_of_the_wrong_kind_is_rejected()
    {
        var counts = new CountVectorizer().Fit(TrainingCorpus);
        using var stream = new MemoryStream();
        counts.Save(stream);
        stream.Position = 0;

        var error = Assert.Throws<InvalidDataException>(() => TfidfVectorizer.Load(stream));
        Assert.Contains("datanet/tfidf-vectorizer", error.Message, StringComparison.Ordinal);
        Assert.Contains("datanet/count-vectorizer", error.Message, StringComparison.Ordinal);
    }

    private static TfidfVectorizer RoundTrip(TfidfVectorizer original)
    {
        using var stream = new MemoryStream();
        original.Save(stream);
        stream.Position = 0;
        return TfidfVectorizer.Load(stream);
    }

    private static void AssertIdentical(CsrMatrix expected, CsrMatrix actual)
    {
        Assert.Equal(expected.RowCount, actual.RowCount);
        Assert.Equal(expected.ColumnCount, actual.ColumnCount);
        Assert.Equal(expected.RowPointers, actual.RowPointers);
        Assert.Equal(expected.ColumnIndices, actual.ColumnIndices);
        AssertIdenticalDoubles(expected.Values, actual.Values);
    }

    private static void AssertIdenticalDoubles(IReadOnlyList<double> expected, IReadOnlyList<double> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            // Compare the bit patterns: "equal to within a tolerance" is exactly
            // the claim this test exists to refuse.
            Assert.Equal(BitConverter.DoubleToInt64Bits(expected[i]), BitConverter.DoubleToInt64Bits(actual[i]));
        }
    }
}
