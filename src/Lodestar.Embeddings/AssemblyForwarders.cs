using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Embeddings still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Persistence.NpyBlock))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Search.BlockNormalization))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Search.SearchResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.BpeSplitStep))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.ISubwordTokenizer))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.MergePair))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.SentencePiece))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.SentencePieceType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.SplitBehavior))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.TokenizationResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Embeddings.Tokenization.TruncationStrategy))]
