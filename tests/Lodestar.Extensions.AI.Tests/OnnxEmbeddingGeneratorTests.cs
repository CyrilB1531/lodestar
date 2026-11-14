using Lodestar.Embeddings.Tokenization;
using Lodestar.Onnx;
using Lodestar.Tests.Fixtures;
using Microsoft.Extensions.AI;
using Xunit;

namespace Lodestar.Extensions.AI.Tests;

/// <summary>
/// The adapter, judged against what it adapts: every fact drives a real ONNX session,
/// <c>tiny_embedder.onnx</c>, because a stub would prove the stub was called.
/// </summary>
/// <remarks>
/// No oracle corpus here on purpose. This package adds no arithmetic, so what it owes
/// is identity with <c>OnnxTextEmbedder.EmbedBatch</c> — asserted exactly rather than
/// within a tolerance — and the interface contract around it; the numbers are pinned
/// by that package's own corpora.
/// </remarks>
public sealed class OnnxEmbeddingGeneratorTests
{
    private const string ModelId = "tiny-embedder";

    private static readonly string EmbedderPath =
        Path.Combine(AppContext.BaseDirectory, "oracles", "tiny_embedder.onnx");

    /// <summary>Six positions: a seventh token indexes past the table (<c>tools/build_tiny_models.py</c>).</summary>
    private static readonly string PositionalPath =
        Path.Combine(AppContext.BaseDirectory, "oracles", "tiny_positional.onnx");

    /// <summary>Two texts whose token sets differ, so a vector that leaked between them would move.</summary>
    private static readonly string[] Texts = ["the cat sat", "a dog ran"];

    /// <summary>
    /// The embedder every fact runs against. Handed to a generator it is disposed twice —
    /// once by the <c>using</c> here and once by the generator that owns it — which is what
    /// the ownership contract promises is safe, and CA2000 has no way to see.
    /// </summary>
    private static OnnxTextEmbedder Embedder() => new(EmbedderPath, BatchCorpus.Tokenizer());

    private static BatchEncoder Encoder() => new(BatchCorpus.Tokenizer());

    /// <summary>
    /// The whole claim of this package: a consumer gets the vectors
    /// <see cref="OnnxTextEmbedder.EmbedBatch(IEnumerable{string}, BatchEncoder, CancellationToken)"/>
    /// returns, in the order asked for. Compared exactly — the adapter reorders nothing and
    /// re-normalizes nothing, so any difference at all is a defect and not float32 noise.
    /// </summary>
    [Fact]
    public async Task Generates_exactly_what_the_embedder_returns()
    {
        using var reference = Embedder();
        float[][] expected = reference.EmbedBatch(Texts, Encoder(), cancellationToken: TestContext.Current.CancellationToken);

        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i].Vector.ToArray());
        }
    }

    /// <summary>
    /// Exercised through the interface rather than the class, which is the only way a
    /// Microsoft.Extensions.AI consumer will ever hold it — and the shape that would break
    /// first if the generic arguments were declared wrong.
    /// </summary>
    [Fact]
    public async Task Is_usable_through_the_interface_alone()
    {
        using var embedder = Embedder();

        // CA1859 asks for the concrete type here, which would delete the fact: what is under
        // test is that the abstraction alone is enough to embed with.
#pragma warning disable CA1859
        using IEmbeddingGenerator<string, Embedding<float>> generator =
            new OnnxEmbeddingGenerator(embedder, Encoder());
#pragma warning restore CA1859

        GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(Texts.Length, embeddings.Count);
        Assert.All(embeddings, embedding => Assert.Equal(embedding.Vector.Length, embedding.Dimensions));
    }

    /// <summary>Every vector is the width the loaded model declares, which is what the metadata promises.</summary>
    [Fact]
    public async Task Every_embedding_has_the_dimension_the_metadata_reports()
    {
        using var embedder = Embedder();
        int declared = embedder.Dimension;
        Assert.True(declared > 0, "tiny_embedder.onnx declares a fixed output width; this suite rests on that");

        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        var metadata = (EmbeddingGeneratorMetadata?)generator.GetService(typeof(EmbeddingGeneratorMetadata));
        GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(metadata);
        Assert.Equal(declared, metadata.DefaultModelDimensions);
        Assert.All(embeddings, embedding => Assert.Equal(declared, embedding.Dimensions));
    }

    /// <summary>
    /// The metadata a chain inspects: who produced this generator, and which model it holds.
    /// An ONNX file carries no model name, so the identifier is the caller's and nothing is
    /// invented when none is given.
    /// </summary>
    [Fact]
    public void Metadata_names_the_provider_and_the_model_it_was_given()
    {
        using var first = Embedder();
        using var second = Embedder();
        using var named = new OnnxEmbeddingGenerator(first, Encoder(), ModelId);
        using var anonymous = new OnnxEmbeddingGenerator(second, Encoder());

        var withId = (EmbeddingGeneratorMetadata?)named.GetService(typeof(EmbeddingGeneratorMetadata));
        var withoutId = (EmbeddingGeneratorMetadata?)anonymous.GetService(typeof(EmbeddingGeneratorMetadata));

        Assert.NotNull(withId);
        Assert.NotNull(withoutId);
        Assert.Equal("Lodestar.Onnx", withId.ProviderName);
        Assert.Equal(ModelId, withId.DefaultModelId);
        Assert.Null(withoutId.DefaultModelId);
    }

    /// <summary>
    /// <c>GetService</c> answers for itself and for the embedder underneath, and for nothing
    /// else. The embedder is offered on purpose: the single-sequence
    /// <see cref="OnnxTextEmbedder.Embed"/> has no shape in this interface, so without this a
    /// consumer holding the abstraction could not reach it at all.
    /// </summary>
    [Fact]
    public void GetService_answers_for_itself_the_embedder_and_the_metadata()
    {
        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        Assert.Same(embedder, generator.GetService(typeof(OnnxTextEmbedder)));
        Assert.Same(generator, generator.GetService(typeof(IEmbeddingGenerator<string, Embedding<float>>)));
        Assert.Same(generator, generator.GetService(typeof(OnnxEmbeddingGenerator)));
        Assert.NotNull(generator.GetService(typeof(EmbeddingGeneratorMetadata)));
        Assert.Null(generator.GetService(typeof(string)));
    }

    /// <summary>Nothing is registered under a key, so a keyed lookup answers null rather than ignoring the key.</summary>
    [Fact]
    public void GetService_answers_null_for_a_keyed_lookup_and_refuses_a_null_type()
    {
        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        Assert.Null(generator.GetService(typeof(OnnxTextEmbedder), serviceKey: "any"));
        Assert.Throws<ArgumentNullException>(() => generator.GetService(null!));
    }

    /// <summary>
    /// A width the model provably does not produce is refused rather than silently ignored: an
    /// ONNX model's output axis is fixed at export, so honouring the request is impossible and
    /// dropping it would return vectors of a size the caller did not ask for.
    /// </summary>
    [Fact]
    public async Task A_dimension_the_model_cannot_produce_is_refused()
    {
        using var embedder = Embedder();
        int declared = embedder.Dimension;
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
            () => generator.GenerateAsync(Texts, new EmbeddingGenerationOptions { Dimensions = declared + 1 }, cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal("options", error.ParamName);
    }

    /// <summary>The width the model does produce is accepted, so asking for what you get is not an error.</summary>
    [Fact]
    public async Task The_dimension_the_model_does_produce_is_accepted()
    {
        using var embedder = Embedder();
        int declared = embedder.Dimension;
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(
            Texts, new EmbeddingGenerationOptions { Dimensions = declared }, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(Texts.Length, embeddings.Count);
    }

    /// <summary>Cancellation is observed by the batch path underneath, and reaches the caller.</summary>
    [Fact]
    public async Task A_cancelled_token_stops_the_generation()
    {
        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        using var source = new CancellationTokenSource();
        await source.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => generator.GenerateAsync(Texts, options: null, source.Token));
    }

    /// <summary>
    /// The generator owns the embedder, so disposing it closes the native session — and a call
    /// afterwards says so plainly instead of failing inside ONNX Runtime.
    /// </summary>
    [Fact]
    public async Task Disposing_closes_the_embedder_and_refuses_later_calls()
    {
        using var embedder = Embedder();
        var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        generator.Dispose();
        generator.Dispose();

        await Assert.ThrowsAsync<ObjectDisposedException>(() => generator.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Throws<ObjectDisposedException>(() => embedder.EmbedBatch(Texts, Encoder(), cancellationToken: TestContext.Current.CancellationToken));
    }

    /// <summary>Neither half of the pipeline may be null: both are used on every call.</summary>
    [Fact]
    public void The_constructor_refuses_a_null_embedder_or_encoder()
    {
        using var embedder = Embedder();

        Assert.Throws<ArgumentNullException>(() => new OnnxEmbeddingGenerator(null!, Encoder()));
        Assert.Throws<ArgumentNullException>(() => new OnnxEmbeddingGenerator(embedder, null!));
    }

    /// <summary>The texts are read on every call, so a null enumerable is refused before the session runs.</summary>
    [Fact]
    public async Task Generating_refuses_a_null_enumerable()
    {
        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        await Assert.ThrowsAsync<ArgumentNullException>(() => generator.GenerateAsync(null!, cancellationToken: TestContext.Current.CancellationToken));
    }

    /// <summary>
    /// Every failure comes back in the task, never from the call, as Microsoft.Extensions.AI's own
    /// generators do (#1214): a caller composing tasks never sees an exception it did not await.
    /// </summary>
    [Fact]
    public async Task Every_failure_surfaces_through_the_returned_task()
    {
        using var embedder = Embedder();
        var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        using var source = new CancellationTokenSource();
        await source.CancelAsync();

        Task<GeneratedEmbeddings<Embedding<float>>> nullValues = generator.GenerateAsync(null!, cancellationToken: TestContext.Current.CancellationToken);
        Task<GeneratedEmbeddings<Embedding<float>>> wrongWidth = generator.GenerateAsync(
            Texts, new EmbeddingGenerationOptions { Dimensions = embedder.Dimension + 1 }, TestContext.Current.CancellationToken);
        Task<GeneratedEmbeddings<Embedding<float>>> cancelled = generator.GenerateAsync(Texts, options: null, source.Token);
        generator.Dispose();
        Task<GeneratedEmbeddings<Embedding<float>>> disposed = generator.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(nullValues.IsFaulted);
        Assert.True(wrongWidth.IsFaulted);
        Assert.True(cancelled.IsCanceled);
        Assert.True(disposed.IsFaulted);
        await Assert.ThrowsAsync<ArgumentNullException>(() => nullValues);
        await Assert.ThrowsAsync<ArgumentException>(() => wrongWidth);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => cancelled);
        await Assert.ThrowsAsync<ObjectDisposedException>(() => disposed);
    }

    /// <summary>
    /// Each embedding names the model that made it and when, as the metadata and the clock say;
    /// Microsoft.Extensions.AI's OpenAI generator fills both the same way.
    /// </summary>
    [Fact]
    public async Task Each_embedding_carries_the_model_id_and_its_creation_time()
    {
        using var first = Embedder();
        using var second = Embedder();
        using var named = new OnnxEmbeddingGenerator(first, Encoder(), ModelId);
        using var anonymous = new OnnxEmbeddingGenerator(second, Encoder());

        DateTimeOffset before = DateTimeOffset.UtcNow;
        GeneratedEmbeddings<Embedding<float>> withId = await named.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);
        GeneratedEmbeddings<Embedding<float>> withoutId = await anonymous.GenerateAsync(Texts, cancellationToken: TestContext.Current.CancellationToken);
        DateTimeOffset after = DateTimeOffset.UtcNow;

        Assert.All(withId, embedding => Assert.Equal(ModelId, embedding.ModelId));
        Assert.All(withoutId, embedding => Assert.Null(embedding.ModelId));
        Assert.All(withId.Concat(withoutId), embedding =>
        {
            Assert.NotNull(embedding.CreatedAt);
            Assert.InRange(embedding.CreatedAt.Value, before, after);
        });
    }

    /// <summary>A text the encoder refuses faults the task under <c>values</c>, this method's parameter (#1345).</summary>
    [Fact]
    public async Task A_refused_text_faults_the_task_under_values()
    {
        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(
            embedder,
            new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { MaxLength = 3, Truncation = TruncationStrategy.None }));

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
            () => generator.GenerateAsync(["the cat sat on the mat"], cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal("values", error.ParamName);
    }

    /// <summary>
    /// The README's encoder sets no <c>MaxLength</c>: a text past the model's table is cut to it, as sentence-transformers
    /// cuts it, not refused (#1257). Empty, at, one past and far past the limit in one batch.
    /// </summary>
    [Fact]
    public async Task A_text_past_the_model_limit_is_truncated_not_refused()
    {
        const int Limit = 6;
        string atLimit = string.Join(" ", Enumerable.Repeat("the", Limit - 2));
        string[] texts = ["", atLimit, atLimit + " dog", string.Join(" ", Enumerable.Repeat("the", 600))];
        using var reference = new OnnxTextEmbedder(PositionalPath, BatchCorpus.Tokenizer());
        float[][] expected = reference.EmbedBatch(
            texts, new EncodingOptions { MaxLength = Limit }, TestContext.Current.CancellationToken);

        using var embedder = new OnnxTextEmbedder(PositionalPath, BatchCorpus.Tokenizer());
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(texts.Length, actual.Count);
        for (int i = 0; i < texts.Length; i++)
        {
            Assert.Equal(expected[i], actual[i].Vector.ToArray());
        }
    }

    /// <summary>An encoder that never cuts faults the task under <c>values</c>: the text is what cannot be fed (#1257).</summary>
    [Fact]
    public async Task A_text_past_the_model_limit_under_no_truncation_faults_under_values()
    {
        using var embedder = new OnnxTextEmbedder(PositionalPath, BatchCorpus.Tokenizer());
        using var generator = new OnnxEmbeddingGenerator(
            embedder,
            new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { Truncation = TruncationStrategy.None }));

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
            () => generator.GenerateAsync([string.Join(" ", Enumerable.Repeat("the", 600))], cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal("values", error.ParamName);
    }

    /// <summary>
    /// A model that states no maximum, with no sentence-transformers configuration beside it, cuts at 256, the
    /// maintainer's default (Cyril 2026-11-10); an empty batch runs nothing (#1257).
    /// </summary>
    [Fact]
    public async Task A_model_with_no_limit_cuts_at_256_and_an_empty_batch_is_empty()
    {
        string[] farPast = [string.Join(" ", Enumerable.Repeat("the dog", 300))];
        using var reference = Embedder();
        float[][] expected = reference.EmbedBatch(
            farPast, new EncodingOptions { MaxLength = 256 }, TestContext.Current.CancellationToken);
        float[][] uncut = reference.EmbedBatch(
            farPast,
            new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { MaxLength = 1000 }),
            TestContext.Current.CancellationToken);

        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

        GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(farPast, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Null(embedder.MaxSequenceLength);
        Assert.Equal(expected[0], actual[0].Vector.ToArray());
        Assert.NotEqual(uncut[0], actual[0].Vector.ToArray());
        Assert.Empty(await generator.GenerateAsync([], cancellationToken: TestContext.Current.CancellationToken));
    }

    /// <summary>The <c>max_seq_length</c> a sentence-transformers export states beside the model is the cut (#1257).</summary>
    [Fact]
    public async Task A_configured_max_seq_length_beside_the_model_is_the_cut()
    {
        string root = Path.Combine(Path.GetTempPath(), $"lodestar-1257-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(root, "onnx"));
        string modelPath = Path.Combine(root, "onnx", "model.onnx");
        File.Copy(EmbedderPath, modelPath);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(root, "modules.json"),
                """[{"path": "", "type": "sentence_transformers.models.Transformer"}]""",
                TestContext.Current.CancellationToken);
            await File.WriteAllTextAsync(
                Path.Combine(root, "sentence_bert_config.json"),
                """{"max_seq_length": 300, "do_lower_case": false}""",
                TestContext.Current.CancellationToken);
            string[] farPast = [string.Join(" ", Enumerable.Repeat("the dog", 300))];
            using var embedder = new OnnxTextEmbedder(modelPath, BatchCorpus.Tokenizer());
            float[][] expected = embedder.EmbedBatch(
                farPast, new EncodingOptions { MaxLength = 300 }, TestContext.Current.CancellationToken);
            using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

            GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(farPast, cancellationToken: TestContext.Current.CancellationToken);

            Assert.Equal(expected[0], actual[0].Vector.ToArray());
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    /// <summary>
    /// A configured length below the template cuts nothing, as sentence-transformers 6.2.0 leaves such a row, measured:
    /// a short text is embedded whole, and one past the model's table faults the task under <c>encoder</c> (#1780).
    /// </summary>
    [Fact]
    public async Task A_configured_length_below_the_template_cuts_nothing()
    {
        CancellationToken token = TestContext.Current.CancellationToken;
        string root = Path.Combine(Path.GetTempPath(), $"lodestar-1780-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        string modelPath = Path.Combine(root, "model.onnx");
        File.Copy(PositionalPath, modelPath);
        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(root, "modules.json"), """[{"path": "", "type": "sentence_transformers.models.Transformer"}]""", token);
            await File.WriteAllTextAsync(Path.Combine(root, "sentence_bert_config.json"), """{"max_seq_length": 1}""", token);
            string[] whole = ["the cat sat"];
            using var embedder = new OnnxTextEmbedder(modelPath, BatchCorpus.Tokenizer());
            float[][] expected = embedder.EmbedBatch(whole, new EncodingOptions { MaxLength = 6 }, token);
            using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);

            GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(whole, cancellationToken: token);
            ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(
                () => generator.GenerateAsync([string.Join(" ", Enumerable.Repeat("the", 600))], cancellationToken: token));

            Assert.Equal(expected[0], actual[0].Vector.ToArray());
            Assert.Equal("encoder", error.ParamName);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
