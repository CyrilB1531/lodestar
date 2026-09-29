using System.Buffers;
using Lodestar.Embeddings.Pooling;
using Lodestar.Embeddings.Tokenization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Lodestar.Onnx;

/// <summary>
/// Runs a transformer encoder exported to ONNX and turns its token outputs into a
/// single sentence embedding (mean pooling + L2 normalization).
/// </summary>
/// <remarks>
/// Delegates to ONNX Runtime; weights are <em>not</em> shipped, supply a path you
/// downloaded. Only declared inputs are fed, so a model with no
/// <c>token_type_ids</c> still runs (<c>OnnxTextEmbedderTests.Embed_runs_model_and_pools</c>);
/// a float16 or bfloat16 output is widened to float. "Embed a batch" in the guide has the rest.
/// </remarks>
public sealed class OnnxTextEmbedder : IDisposable
{
    // Consulted in order when no output name is given and the model has several.
    // "Whichever key the dictionary yields first" is not a contract; these are.
    private static readonly string[] PreferredOutputNames =
        ["last_hidden_state", "token_embeddings", "sentence_embedding", "output"];

    private readonly InferenceSession _session;
    private readonly string _inputIdsName;
    private readonly string _attentionMaskName;
    private readonly string? _tokenTypeIdsName;
    private readonly string _outputName;
    private readonly string[] _outputNames;
    private readonly ISubwordTokenizer? _tokenizer;
    private readonly int? _maxSequenceLength;
    private readonly int? _fixedSequence;
    private readonly int? _fixedBatch;
    private bool _disposed;

    /// <summary>Opens an ONNX encoder model from <paramref name="modelPath"/>.</summary>
    /// <param name="modelPath">Path to the <c>.onnx</c> model file.</param>
    /// <param name="options">Optional ONNX Runtime session options.</param>
    /// <param name="inputIdsName">Name of the token-ids input (default <c>input_ids</c>).</param>
    /// <param name="attentionMaskName">Name of the attention-mask input (default <c>attention_mask</c>).</param>
    /// <param name="tokenTypeIdsName">Name of the token-type-ids input (default <c>token_type_ids</c>), used only if the model declares it.</param>
    /// <param name="outputName">Name of the token-embeddings output; see the remarks for how one is chosen when this is null.</param>
    /// <remarks>
    /// When <paramref name="outputName"/> is null the output is chosen
    /// deterministically: the model's only output if it has one, else the first of
    /// <c>last_hidden_state</c>, <c>token_embeddings</c>, <c>sentence_embedding</c>
    /// and <c>output</c> that it declares, else the ordinally first name. Dictionary
    /// key order is not part of ONNX Runtime's contract, so "the model's first
    /// output" was a coin toss on a multi-output model.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="modelPath"/>, <paramref name="inputIdsName"/>, <paramref name="attentionMaskName"/> or <paramref name="tokenTypeIdsName"/> is null.</exception>
    /// <exception cref="ArgumentException">The model declares no input under <paramref name="inputIdsName"/> or <paramref name="attentionMaskName"/>, or no output under <paramref name="outputName"/>.</exception>
    public OnnxTextEmbedder(
        string modelPath,
        SessionOptions? options = null,
        string inputIdsName = "input_ids",
        string attentionMaskName = "attention_mask",
        string tokenTypeIdsName = "token_type_ids",
        string? outputName = null)
    {
        Guard.NotNull(modelPath);

        // Before the session opens: a null name reached ONNX Runtime's dictionary as ArgumentNullException("key") (#1343).
        Guard.NotNull(inputIdsName);
        Guard.NotNull(attentionMaskName);
        Guard.NotNull(tokenTypeIdsName);
        _session = options is null ? new InferenceSession(modelPath) : new InferenceSession(modelPath, options);
        try
        {
            _inputIdsName = RequireInput(_session, inputIdsName, nameof(inputIdsName));
            _attentionMaskName = RequireInput(_session, attentionMaskName, nameof(attentionMaskName));
            _tokenTypeIdsName = _session.InputMetadata.ContainsKey(tokenTypeIdsName) ? tokenTypeIdsName : null;
            _outputName = ChooseOutput(_session, outputName, nameof(outputName));
            _outputNames = [_outputName];
            _fixedSequence = DeclaredSequenceLength(_session, _inputIdsName);
            _fixedBatch = DeclaredBatchSize(_session, _inputIdsName);
            _maxSequenceLength = _fixedSequence ?? PositionTable.UsableLength(modelPath, _inputIdsName);
        }
        catch
        {
            // A constructor that throws hands the caller nothing to dispose, so the native session is released here.
            _session.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Opens an ONNX encoder model and attaches the tokenizer the text-level
    /// overloads encode with.
    /// </summary>
    /// <remarks>
    /// The equivalent of building a <c>sentence_transformers.SentenceTransformer</c>
    /// from a model directory: the tokenizer and the graph travel together, because
    /// a model run against a tokenizer it was not trained with produces embeddings
    /// that are wrong without being invalid.
    /// </remarks>
    /// <param name="modelPath">Path to the <c>.onnx</c> model file.</param>
    /// <param name="tokenizer">The tokenizer matching the model — a <see cref="WordPieceTokenizer"/>, a <see cref="SentencePieceTokenizer"/> or a <see cref="BpeTokenizer"/>, or any other <see cref="ISubwordTokenizer"/>.</param>
    /// <param name="options">Optional ONNX Runtime session options.</param>
    /// <param name="inputIdsName">Name of the token-ids input (default <c>input_ids</c>).</param>
    /// <param name="attentionMaskName">Name of the attention-mask input (default <c>attention_mask</c>).</param>
    /// <param name="tokenTypeIdsName">Name of the token-type-ids input (default <c>token_type_ids</c>), used only if the model declares it.</param>
    /// <param name="outputName">Name of the token-embeddings output; defaults as described on the other constructor.</param>
    /// <exception cref="ArgumentNullException"><paramref name="modelPath"/>, <paramref name="tokenizer"/>, <paramref name="inputIdsName"/>, <paramref name="attentionMaskName"/> or <paramref name="tokenTypeIdsName"/> is null.</exception>
    /// <exception cref="ArgumentException">The model declares no input under <paramref name="inputIdsName"/> or <paramref name="attentionMaskName"/>, or no output under <paramref name="outputName"/>.</exception>
    public OnnxTextEmbedder(
        string modelPath,
        ISubwordTokenizer tokenizer,
        SessionOptions? options = null,
        string inputIdsName = "input_ids",
        string attentionMaskName = "attention_mask",
        string tokenTypeIdsName = "token_type_ids",
        string? outputName = null)
        : this(RequireTokenizer(tokenizer, modelPath), options, inputIdsName, attentionMaskName, tokenTypeIdsName, outputName)
    {
        _tokenizer = tokenizer;
    }

    /// <summary>The embedding dimension reported by the model output, if known (else -1).</summary>
    public int Dimension
    {
        get
        {
            int[] shape = _session.OutputMetadata[_outputName].Dimensions;
            return shape.Length > 0 ? shape[^1] : -1;
        }
    }

    /// <summary>
    /// The longest sequence the model can take: its declared sequence axis when that is fixed,
    /// else the positions its position-embedding table can index; else <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// What a null <see cref="EncodingOptions.MaxLength"/> falls back to. The table is the
    /// model's hard limit, not sentence-transformers' <c>max_seq_length</c> (256 for
    /// all-MiniLM-L6-v2, whose table holds 512), which no graph carries: see the guide's
    /// "Embed a batch". A RoBERTa-style table's padding offset is subtracted (514 reads 512).
    /// </remarks>
    public int? MaxSequenceLength => _maxSequenceLength;

    /// <summary>Embeds a single tokenized sequence into a normalized sentence vector.</summary>
    /// <remarks>
    /// Matches <c>session.run(None, {"input_ids": …, "attention_mask": …})</c>
    /// followed by the sentence-transformers mean-pool and normalize. The caller
    /// owns the special tokens and the mask here; use
    /// <see cref="EmbedBatch(IEnumerable{string}, EncodingOptions, CancellationToken)"/>
    /// to have the library own them instead.
    /// </remarks>
    /// <param name="inputIds">Token ids.</param>
    /// <param name="attentionMask">Attention mask (same length as <paramref name="inputIds"/>).</param>
    /// <exception cref="ArgumentException"><paramref name="inputIds"/> and <paramref name="attentionMask"/> differ in length, or are longer than the model's fixed sequence axis.</exception>
    /// <exception cref="InvalidOperationException">The model output is not <c>[batch, sequence, dim]</c> or <c>[batch, dim]</c> for the batch it was fed.</exception>
    /// <exception cref="NotSupportedException">The model output's elements are not float, float16 or bfloat16.</exception>
    /// <exception cref="ObjectDisposedException">The embedder has been disposed.</exception>
    public float[] Embed(ReadOnlySpan<long> inputIds, ReadOnlySpan<long> attentionMask)
    {
        ThrowIfDisposed();
        if (inputIds.Length != attentionMask.Length)
        {
            throw new ArgumentException("inputIds and attentionMask must have equal length.", nameof(attentionMask));
        }

        int seqLen = inputIds.Length;
        // ONNX Runtime wraps a Memory<T>, which a span cannot supply, so the copy
        // stays; renting it is what removes the two per-call array allocations.
        long[] ids = ArrayPool<long>.Shared.Rent(seqLen);
        long[] mask = ArrayPool<long>.Shared.Rent(seqLen);
        try
        {
            inputIds.CopyTo(ids);
            attentionMask.CopyTo(mask);
            return Run(ids, mask, batchSize: 1, seqLen, nameof(inputIds))[0];
        }
        finally
        {
            ArrayPool<long>.Shared.Return(ids);
            ArrayPool<long>.Shared.Return(mask);
        }
    }

    /// <summary>
    /// Embeds a corpus: tokenizes, inserts the model's special tokens, truncates,
    /// pads each sub-batch to its own longest sequence — or to the model's fixed axes, for a static
    /// export — and returns one normalized vector per input text, in the input order.
    /// </summary>
    /// <remarks>
    /// The equivalent of
    /// <c>SentenceTransformer.encode(texts, batch_size=…, normalize_embeddings=True)</c>.
    /// Requires the constructor overload that takes a tokenizer.
    /// </remarks>
    /// <param name="texts">The texts to embed.</param>
    /// <param name="options">Template, truncation and batching settings; <see langword="null"/> uses the defaults, with <c>MaxLength</c> taken from <see cref="MaxSequenceLength"/>.</param>
    /// <param name="cancellationToken">Observed while tokenizing and between sub-batches.</param>
    /// <exception cref="ArgumentNullException"><paramref name="texts"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="options"/> is refused by <see cref="BatchEncoder"/> or sets a <c>MaxLength</c> past the model's fixed sequence axis, or a text is refused, as one over <c>MaxLength</c> under <see cref="TruncationStrategy.None"/> is.</exception>
    /// <exception cref="InvalidOperationException">The embedder was built without a tokenizer, or the model output is not shaped for the batch it was fed.</exception>
    /// <exception cref="NotSupportedException">The model output's elements are not float, float16 or bfloat16.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled.</exception>
    /// <exception cref="ObjectDisposedException">The embedder has been disposed.</exception>
    public float[][] EmbedBatch(
        IEnumerable<string> texts,
        EncodingOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (_tokenizer is null)
        {
            throw new InvalidOperationException(
                "This embedder has no tokenizer. Use the constructor overload that takes one, " +
                "or call EmbedBatch(texts, encoder, cancellationToken) with a BatchEncoder you built.");
        }
        return EmbedBatch(texts, new BatchEncoder(_tokenizer, ResolveOptions(options)), cancellationToken);
    }

    /// <summary>Embeds a corpus using an explicit <see cref="BatchEncoder"/>.</summary>
    /// <remarks>
    /// Same contract as
    /// <see cref="EmbedBatch(IEnumerable{string}, EncodingOptions, CancellationToken)"/>,
    /// for the case where the tokenizer is not the one the embedder was built with.
    /// </remarks>
    /// <param name="texts">The texts to embed.</param>
    /// <param name="encoder">The encoder that owns the tokenizer, template and truncation.</param>
    /// <param name="cancellationToken">Observed while tokenizing and between sub-batches.</param>
    /// <exception cref="ArgumentNullException"><paramref name="texts"/> or <paramref name="encoder"/> is null.</exception>
    /// <exception cref="ArgumentException">The encoder refuses a text, as it refuses one over <c>MaxLength</c> under <see cref="TruncationStrategy.None"/>, or encodes one past the model's fixed sequence axis.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled.</exception>
    /// <exception cref="InvalidOperationException">The model output is not <c>[batch, sequence, dim]</c> or <c>[batch, dim]</c> for the batch it was fed.</exception>
    /// <exception cref="NotSupportedException">The model output's elements are not float, float16 or bfloat16.</exception>
    /// <exception cref="ObjectDisposedException">The embedder has been disposed.</exception>
    public float[][] EmbedBatch(IEnumerable<string> texts, BatchEncoder encoder, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        Guard.NotNull(encoder);
        IReadOnlyList<long[]> sequences = encoder.EncodeAll(texts, cancellationToken);

        int total = sequences.Count;
        var embeddings = new float[total][];
        if (total == 0)
        {
            return embeddings;
        }

        int batchSize = encoder.Options.BatchSize;
        int[]? order = encoder.Options.SortByLength && total > batchSize ? SortedByLength(sequences) : null;

        for (int start = 0; start < total; start += batchSize)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int count = Math.Min(batchSize, total - start);
            EncodedBatch batch = encoder.Pad(sequences, start, count, order);
            float[][] vectors = RunBatch(batch, nameof(encoder));
            for (int i = 0; i < count; i++)
            {
                embeddings[order is null ? start + i : order[start + i]] = vectors[i];
            }
        }
        return embeddings;
    }

    /// <summary>Embeds an already-encoded batch, in batch order.</summary>
    /// <remarks>
    /// The lowest-level batch entry point: one ONNX Runtime call over
    /// <see cref="EncodedBatch.InputIds"/>, pooled behind
    /// <see cref="EncodedBatch.AttentionMask"/>, and no reordering; a static export's fixed batch axis
    /// splits it into chunks of that size.
    /// </remarks>
    /// <param name="batch">A batch from <see cref="BatchEncoder.EncodeBatch"/>.</param>
    /// <param name="cancellationToken">Observed before the call is made.</param>
    /// <exception cref="ArgumentNullException"><paramref name="batch"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="batch"/> is longer than the model's fixed sequence axis.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was cancelled.</exception>
    /// <exception cref="InvalidOperationException">The model output is not <c>[batch, sequence, dim]</c> or <c>[batch, dim]</c> for the batch it was fed.</exception>
    /// <exception cref="NotSupportedException">The model output's elements are not float, float16 or bfloat16.</exception>
    /// <exception cref="ObjectDisposedException">The embedder has been disposed.</exception>
    public float[][] EmbedBatch(EncodedBatch batch, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        Guard.NotNull(batch);
        cancellationToken.ThrowIfCancellationRequested();
        return batch.Count == 0 ? [] : RunBatch(batch, nameof(batch));
    }

    /// <summary>Releases the underlying ONNX Runtime session.</summary>
    public void Dispose()
    {
        _disposed = true;
        _session.Dispose();
    }

    // Without this, a call on a disposed embedder reaches into a disposed
    // InferenceSession and surfaces as NullReferenceException (measured, #266).
    private void ThrowIfDisposed()
    {
#if NET
        ObjectDisposedException.ThrowIf(_disposed, this);
#else
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(OnnxTextEmbedder));
        }
#endif
    }

    /// <summary>Fills in the model-derived defaults the caller left open.</summary>
    private EncodingOptions ResolveOptions(EncodingOptions? options)
    {
        EncodingOptions resolved = options ?? new EncodingOptions();
        if (_fixedSequence is int axis && resolved.MaxLength > axis)
        {
            throw new ArgumentException(
                $"MaxLength {resolved.MaxLength} is past the model's fixed sequence axis of {axis}.", nameof(options));
        }

        return resolved.MaxLength is null && _maxSequenceLength is int limit
            ? resolved with { MaxLength = limit }
            : resolved;
    }

    /// <summary>The token-ids input's last axis, when fixed; a symbolic axis reads back negative.</summary>
    private static int? DeclaredSequenceLength(InferenceSession session, string inputIdsName)
    {
        int[] shape = session.InputMetadata[inputIdsName].Dimensions;
        return shape.Length >= 2 && shape[^1] > 0 ? shape[^1] : null;
    }

    /// <summary>The batch axis a static export fixes, or <see langword="null"/> when it is symbolic.</summary>
    private static int? DeclaredBatchSize(InferenceSession session, string inputIdsName)
    {
        int[] shape = session.InputMetadata[inputIdsName].Dimensions;
        return shape.Length >= 2 && shape[0] > 0 ? shape[0] : null;
    }

    /// <summary>Indices of <paramref name="sequences"/> ordered by length, ties by original position.</summary>
    private static int[] SortedByLength(IReadOnlyList<long[]> sequences)
    {
        var order = new int[sequences.Count];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }
        // The index tie-break keeps the permutation a function of the input alone,
        // so two runs over the same corpus pad identically.
        Array.Sort(order, (a, b) =>
        {
            int byLength = sequences[a].Length.CompareTo(sequences[b].Length);
            return byLength != 0 ? byLength : a.CompareTo(b);
        });
        return order;
    }

    // ONNX Runtime wraps a Memory<T>, which EncodedBatch exposes only as spans:
    // renting and copying beats a second padding that can drift from BatchEncoder's.
    private float[][] RunBatch(EncodedBatch batch, string paramName)
    {
        long[] ids = ArrayPool<long>.Shared.Rent(batch.InputIds.Length);
        long[] mask = ArrayPool<long>.Shared.Rent(batch.AttentionMask.Length);
        try
        {
            batch.InputIds.CopyTo(ids);
            batch.AttentionMask.CopyTo(mask);
            return Run(ids, mask, batch.Count, batch.SequenceLength, paramName);
        }
        finally
        {
            ArrayPool<long>.Shared.Return(ids);
            ArrayPool<long>.Shared.Return(mask);
        }
    }

    /// <summary>Runs one padded batch and pools it, in the shape a static export fixes where it fixes one.</summary>
    /// <remarks>
    /// ONNX Runtime refuses any other dimension on a fixed axis (#1258): each row is widened to the fixed sequence
    /// axis with masked padding, which the mean leaves out, and the rows are run in chunks of the fixed batch axis,
    /// the last filled with masked rows whose outputs are dropped. A padded id is never read unmasked, so its value
    /// is irrelevant; it is 0.
    /// </remarks>
    private float[][] Run(long[] ids, long[] mask, int batchSize, int seqLen, string paramName)
    {
        // A row past a fixed axis cannot be fed; refused under the caller's parameter rather than by the runtime.
        if (_fixedSequence is int axis && seqLen > axis)
        {
            throw new ArgumentException(
                $"A sequence of {seqLen} tokens is past the model's fixed sequence axis of {axis}.", paramName);
        }

        int width = _fixedSequence is int fixedWidth && fixedWidth > seqLen ? fixedWidth : seqLen;
        int chunk = _fixedBatch ?? batchSize;
        if (width == seqLen && chunk == batchSize)
        {
            return RunExact(ids, mask, batchSize, seqLen);
        }

        var pooled = new float[batchSize][];
        long[] chunkIds = new long[(long)chunk * width];
        long[] chunkMask = new long[chunkIds.Length];
        for (int first = 0; first < batchSize; first += chunk)
        {
            int rows = Math.Min(chunk, batchSize - first);
            Array.Clear(chunkIds, 0, chunkIds.Length);
            Array.Clear(chunkMask, 0, chunkMask.Length);
            for (int row = 0; row < rows; row++)
            {
                Array.Copy(ids, (first + row) * seqLen, chunkIds, row * width, seqLen);
                Array.Copy(mask, (first + row) * seqLen, chunkMask, row * width, seqLen);
            }

            float[][] vectors = RunExact(chunkIds, chunkMask, chunk, width);
            Array.Copy(vectors, 0, pooled, first, rows);
        }

        return pooled;
    }

    /// <summary>Runs one batch exactly as shaped and pools it.</summary>
    private float[][] RunExact(long[] ids, long[] mask, int batchSize, int seqLen)
    {
        int elements = batchSize * seqLen;
        var inputs = new List<NamedOnnxValue>(3)
        {
            NamedOnnxValue.CreateFromTensor(_inputIdsName, new DenseTensor<long>(ids.AsMemory(0, elements), [batchSize, seqLen])),
            NamedOnnxValue.CreateFromTensor(_attentionMaskName, new DenseTensor<long>(mask.AsMemory(0, elements), [batchSize, seqLen])),
        };
        if (_tokenTypeIdsName is not null)
        {
            inputs.Add(NamedOnnxValue.CreateFromTensor(
                _tokenTypeIdsName,
                new DenseTensor<long>(ZeroTokenTypeIds.OfLength(elements).AsMemory(0, elements), [batchSize, seqLen])));
        }

        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _session.Run(inputs, _outputNames);
        object value = results[0].Value;
        int[] shape = value switch
        {
            Tensor<float> single => single.Dimensions.ToArray(),
            Tensor<Float16> half => half.Dimensions.ToArray(),
            Tensor<BFloat16> brain => brain.Dimensions.ToArray(),
            _ => throw new NotSupportedException(
                $"The model output '{_outputName}' holds {_session.OutputMetadata[_outputName].ElementDataType} elements. " +
                "A token-embedding output must be float, float16 or bfloat16."),
        };
        RequireShape(shape, batchSize, seqLen);

        int dim = shape[^1];
        int length = batchSize * dim * (shape.Length == 3 ? seqLen : 1);
        float[]? widened = value is Tensor<float> ? null : Widen(value, length);
        try
        {
            // Reading the tensor's own buffer, rather than ToArray(), keeps the whole
            // [batch, sequence, dim] block from being copied on the way to the pooler.
            ReadOnlySpan<float> flat = value switch
            {
                DenseTensor<float> dense => dense.Buffer.Span,
                Tensor<float> single => single.ToArray(),
                _ => widened.AsSpan(0, length),
            };

            if (shape.Length == 2)
            {
                // Already pooled by the graph: normalize each row and stop.
                var pooled = new float[batchSize][];
                for (int b = 0; b < batchSize; b++)
                {
                    float[] vector = flat.Slice(b * dim, dim).ToArray();
                    Pooler.L2Normalize(vector);
                    pooled[b] = vector;
                }
                return pooled;
            }

            return Pooler.MeanPoolAndNormalizeBatch(flat, batchSize, seqLen, dim, mask.AsSpan(0, elements));
        }
        finally
        {
            if (widened is not null)
            {
                ArrayPool<float>.Shared.Return(widened);
            }
        }
    }

    /// <summary>Refuses an output whose leading axes are not the batch the model was fed.</summary>
    /// <remarks>
    /// <c>[seq, batch, dim]</c> holds as many elements as <c>[batch, seq, dim]</c>, so the pooler's
    /// own length check passed a transposed output and averaged the wrong rows (#1214).
    /// </remarks>
    private void RequireShape(int[] shape, int batchSize, int seqLen)
    {
        if (shape.Length is not (2 or 3))
        {
            throw new InvalidOperationException(
                $"The model output '{_outputName}' has rank {shape.Length}. A token-embedding output must be " +
                "[batch, sequence, dim] (rank 3), or [batch, dim] (rank 2) if the model pools internally. " +
                "Pass outputName to select a different output.");
        }
        bool matches = shape[0] == batchSize && (shape.Length == 2 || shape[1] == seqLen);
        if (!matches)
        {
            string expected = shape.Length == 3 ? $"[{batchSize}, {seqLen}, dim]" : $"[{batchSize}, dim]";
            throw new InvalidOperationException(
                $"The model output '{_outputName}' has shape [{string.Join(", ", shape)}] where {expected} was expected " +
                $"for a batch of {batchSize} sequence(s) of {seqLen} token(s). " +
                "Pass outputName to select a different output.");
        }
    }

    /// <summary>Converts a half-precision output to a rented float buffer the caller returns.</summary>
    private static float[] Widen(object value, int length)
    {
        float[] widened = ArrayPool<float>.Shared.Rent(length);
        switch (value)
        {
            case DenseTensor<Float16> half:
                ReadOnlySpan<Float16> halves = half.Buffer.Span;
                for (int i = 0; i < length; i++)
                {
                    widened[i] = (float)halves[i];
                }
                break;
            case DenseTensor<BFloat16> brain:
                ReadOnlySpan<BFloat16> brains = brain.Buffer.Span;
                for (int i = 0; i < length; i++)
                {
                    widened[i] = (float)brains[i];
                }
                break;
            default:
                ArrayPool<float>.Shared.Return(widened);
                throw new NotSupportedException($"ONNX Runtime returned a {value.GetType().Name}, which is not a dense tensor.");
        }
        return widened;
    }

    /// <summary>Refuses a null tokenizer, then hands back the model path.</summary>
    /// <remarks>
    /// Called in the chained constructor's argument list so the refusal comes before a session is
    /// opened: checked in the body, a null tokenizer threw with that session left open.
    /// </remarks>
    private static string RequireTokenizer(ISubwordTokenizer tokenizer, string modelPath)
    {
        Guard.NotNull(tokenizer);
        return modelPath;
    }

    private static string RequireInput(InferenceSession session, string name, string parameterName)
    {
        if (!session.InputMetadata.ContainsKey(name))
        {
            throw new ArgumentException(
                $"The model declares no input named '{name}'. It declares: {string.Join(", ", session.InputMetadata.Keys)}.",
                parameterName);
        }
        return name;
    }

    private static string ChooseOutput(InferenceSession session, string? requested, string parameterName)
    {
        IReadOnlyDictionary<string, NodeMetadata> outputs = session.OutputMetadata;
        if (requested is not null)
        {
            if (!outputs.ContainsKey(requested))
            {
                throw new ArgumentException(
                    $"The model declares no output named '{requested}'. It declares: {string.Join(", ", outputs.Keys)}.",
                    parameterName);
            }
            return requested;
        }
        if (outputs.Count == 1)
        {
            return outputs.Keys.First();
        }
        return PreferredOutputNames.FirstOrDefault(outputs.ContainsKey)
            ?? outputs.Keys.OrderBy(name => name, StringComparer.Ordinal).First();
    }

    /// <summary>A per-thread buffer of zeros, fed as <c>token_type_ids</c>.</summary>
    /// <remarks>
    /// Nothing ever writes to it, so it needs no clearing on reuse — which is the
    /// difference between this and renting from <see cref="ArrayPool{T}"/>. It is
    /// thread-static rather than shared because the session may be run from several
    /// threads at once, and a single-segment model has no other use for it.
    /// </remarks>
    private static class ZeroTokenTypeIds
    {
        [ThreadStatic]
        private static long[]? _buffer;

        public static long[] OfLength(int length)
        {
            long[]? buffer = _buffer;
            if (buffer is null || buffer.Length < length)
            {
                buffer = new long[Math.Max(length, 512)];
                _buffer = buffer;
            }
            return buffer;
        }
    }
}
