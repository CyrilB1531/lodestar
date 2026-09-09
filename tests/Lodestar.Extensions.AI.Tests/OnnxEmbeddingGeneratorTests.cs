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
        float[][] expected = reference.EmbedBatch(Texts, Encoder());

        using var embedder = Embedder();
        using var generator = new OnnxEmbeddingGenerator(embedder, Encoder(), ModelId);
        GeneratedEmbeddings<Embedding<float>> actual = await generator.GenerateAsync(Texts);

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

        GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(Texts);

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
        GeneratedEmbeddings<Embedding<float>> embeddings = await generator.GenerateAsync(Texts);

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
            () => generator.GenerateAsync(Texts, new EmbeddingGenerationOptions { Dimensions = declared + 1 }));

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
            Texts, new EmbeddingGenerationOptions { Dimensions = declared });

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

        await Assert.ThrowsAsync<ObjectDisposedException>(() => generator.GenerateAsync(Texts));
        Assert.Throws<ObjectDisposedException>(() => embedder.EmbedBatch(Texts, Encoder()));
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

        await Assert.ThrowsAsync<ArgumentNullException>(() => generator.GenerateAsync(null!));
    }
}
