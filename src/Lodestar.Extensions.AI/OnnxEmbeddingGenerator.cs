using Lodestar.Embeddings.Tokenization;
using Lodestar.Onnx;
using Microsoft.Extensions.AI;

namespace Lodestar.Extensions.AI;

/// <summary>
/// Presents an <see cref="OnnxTextEmbedder"/> as an
/// <see cref="IEmbeddingGenerator{TInput, TEmbedding}"/>, for a Microsoft.Extensions.AI caller.
/// </summary>
/// <remarks>
/// Adds no arithmetic: every vector is <see cref="OnnxTextEmbedder"/>'s, unchanged. Three
/// contracts the interface hides are stated on the members below — the task comes back
/// completed, a requested dimension is checked rather than honoured, and this generator
/// owns the embedder it was given.
/// </remarks>
public sealed class OnnxEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
{
    /// <summary>Named on the metadata so a consumer inspecting a chain can tell whose generator it holds.</summary>
    private const string Provider = "Lodestar.Onnx";

    private readonly OnnxTextEmbedder _embedder;
    private readonly BatchEncoder _encoder;
    private readonly EmbeddingGeneratorMetadata _metadata;
    private bool _disposed;

    /// <summary>Wraps an embedder and the encoder it should tokenize with.</summary>
    /// <param name="embedder">The loaded model. <strong>This generator takes ownership</strong>: <see cref="Dispose"/> disposes it.</param>
    /// <param name="encoder">The encoder that owns the tokenizer, template and truncation.</param>
    /// <param name="modelId">What to report as the model's name; <see langword="null"/> reports none. An ONNX file does not carry one, so nothing can be read off the session.</param>
    /// <exception cref="ArgumentNullException"><paramref name="embedder"/> or <paramref name="encoder"/> is null.</exception>
    /// <remarks>
    /// Ownership is taken rather than shared because <see cref="IEmbeddingGenerator"/> is
    /// <see cref="IDisposable"/> and a consumer holding one through the interface has no
    /// way to learn that disposing it would leave a native session open. A caller who
    /// wants to keep the embedder builds a second one.
    /// </remarks>
    public OnnxEmbeddingGenerator(OnnxTextEmbedder embedder, BatchEncoder encoder, string? modelId = null)
    {
        Guard.NotNull(embedder);
        Guard.NotNull(encoder);

        _embedder = embedder;
        _encoder = encoder;

        // Dimension reads minus one where the output axis is symbolic, which most exports
        // are, so the metadata reports "unknown" rather than passing that sentinel on
        int dimension = embedder.Dimension;
        _metadata = new EmbeddingGeneratorMetadata(
            Provider, providerUri: null, modelId, dimension > 0 ? dimension : null);
    }

    /// <summary>Embeds each text into one normalized vector.</summary>
    /// <param name="values">The texts to embed.</param>
    /// <param name="options">Only <see cref="EmbeddingGenerationOptions.Dimensions"/> is read; see the remarks.</param>
    /// <param name="cancellationToken">Observed while tokenizing and between sub-batches.</param>
    /// <returns>One <see cref="Embedding{T}"/> per input, in the order they were given.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="options"/> asks for a dimension the loaded model does not produce.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled, before or between sub-batches.</exception>
    /// <exception cref="ObjectDisposedException">This generator has been disposed.</exception>
    /// <remarks>
    /// The task is already completed: the model runs in this process, so the work happens
    /// on the calling thread. <see cref="EmbeddingGenerationOptions.Dimensions"/> is
    /// checked rather than honoured — an ONNX model's width is fixed at export — and only
    /// where the model declares a fixed output axis, since refusing a symbolic one would
    /// mean guessing. <see cref="EmbeddingGenerationOptions.ModelId"/> is not read: it
    /// selects among the models a service hosts, and this generator holds exactly one.
    /// </remarks>
    public Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        Guard.NotNull(values);
        RequireCompatibleDimensions(options);

        float[][] vectors = _embedder.EmbedBatch(values, _encoder, cancellationToken);

        var embeddings = new GeneratedEmbeddings<Embedding<float>>(vectors.Length);
        foreach (float[] vector in vectors)
        {
            embeddings.Add(new Embedding<float>(vector));
        }

        return Task.FromResult(embeddings);
    }

    /// <summary>Answers for the services this generator can hand out.</summary>
    /// <param name="serviceType">The service being asked for.</param>
    /// <param name="serviceKey">A key; a keyed lookup always answers <see langword="null"/> here, since nothing is registered under one.</param>
    /// <returns>The service, or <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is null.</exception>
    /// <remarks>
    /// <see cref="OnnxTextEmbedder"/> is offered so that a consumer holding this through
    /// the interface can reach the single-sequence
    /// <see cref="OnnxTextEmbedder.Embed"/>, which Microsoft.Extensions.AI has no shape for.
    /// </remarks>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        Guard.NotNull(serviceType);

        if (serviceKey is not null)
        {
            return null;
        }

        if (serviceType == typeof(EmbeddingGeneratorMetadata))
        {
            return _metadata;
        }

        if (serviceType == typeof(OnnxTextEmbedder))
        {
            return _embedder;
        }

        return serviceType.IsInstanceOfType(this) ? this : null;
    }

    /// <summary>Disposes the embedder this generator was given.</summary>
    /// <remarks>Disposing twice is safe; calling <see cref="GenerateAsync"/> afterwards is not.</remarks>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _embedder.Dispose();
    }

    /// <summary>Refuses a width the loaded model provably does not produce.</summary>
    private void RequireCompatibleDimensions(EmbeddingGenerationOptions? options)
    {
        int? requested = options?.Dimensions;
        int declared = _embedder.Dimension;

        if (requested is int wanted && declared > 0 && wanted != declared)
        {
            throw new ArgumentException(
                $"the loaded model produces {declared}-dimensional embeddings, so {wanted} cannot be "
                + "returned. An ONNX model's output width is fixed at export.",
                nameof(options));
        }
    }

    /// <summary>Refuses a call made after <see cref="Dispose"/>.</summary>
    private void ThrowIfDisposed()
    {
#if NET
        ObjectDisposedException.ThrowIf(_disposed, this);
#else
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(OnnxEmbeddingGenerator));
        }
#endif
    }
}
