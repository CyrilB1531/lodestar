using System.Runtime.CompilerServices;

// Data types declared here until 0.6.x live in Lodestar.Abstractions (decision 0003, #1142), and
// PorterStemmerMode was born there (#1193); these forwarders keep both bound to this package.

[assembly: TypeForwardedTo(typeof(Lodestar.Text.TextElement))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Indexing.BkTreeMatch))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Keywords.KeywordMatch))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Keywords.RakeMetric))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Keywords.RakeOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Keywords.TextRankOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Phonetics.DoubleMetaphoneCode))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Search.Bm25Idf))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Search.Bm25Options))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Search.SearchHit))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Stemming.PorterStemmerMode))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Similarity.MinHashScheme))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Vectorization.AnalyzerKind))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Vectorization.CountVectorizerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Vectorization.HashingVectorizerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Vectorization.TfidfOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Text.Vectorization.TfidfVectorizerOptions))]
