using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// <summary>How a collection builds its two derived indexes, and how it fuses them.</summary>
/// <remarks>
/// All three are the defaults of the members they configure, so a collection constructed
/// without options behaves as <c>CountVectorizer</c>, <c>Bm25Index</c> and
/// <c>RankFusion.Rrf</c> do on their own.
/// </remarks>
public sealed class LodestarVectorStoreOptions
{
    private readonly int _rankFusionK = RankFusion.DefaultK;

    /// <summary>How the full-text property is tokenized and counted; <see langword="null"/> takes the defaults.</summary>
    public CountVectorizerOptions? Vectorizer { get; init; }

    /// <summary>The BM25 saturation and length-normalisation settings; <see langword="null"/> takes the defaults.</summary>
    public Bm25Options? Bm25 { get; init; }

    /// <summary>The rank offset reciprocal-rank fusion uses; 60 by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not positive.</exception>
    public int RankFusionK
    {
        get => _rankFusionK;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value,
                    "The rank offset is positive; zero would divide by zero at rank zero.");
            }

            _rankFusionK = value;
        }
    }
}
