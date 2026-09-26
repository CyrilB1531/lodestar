# Lodestar

Lodestar brings to C#/.NET the parts of Python's data-science stack that have no maintained .NET
equivalent at the reference's parity, with no Python at runtime. Every algorithm replays values
captured from the Python library it follows, and the [equivalence table](../equivalence.md) maps
each Python call to its C# counterpart.

These pages describe `main`. Each package's released versions are frozen under their own names on
the [wiki](https://github.com/CyrilB1531/lodestar/wiki).

## What do you want to do?

| Task | Start here |
| --- | --- |
| Compare two strings: edit distance, Jaro-Winkler, longest common subsequence | [Distances](../reference/text/distances.md) |
| Compare two bags of words: Jaccard, Dice, cosine | [Set similarity](../reference/text/similarity.md) |
| Find the best match for a string among candidates, as rapidfuzz does | [Migrating from rapidfuzz](../guides/migrating-from-rapidfuzz.md) |
| Find every entry of a large dictionary within a distance of a query | [Indexing strings for fast lookup](../guides/dictionary-lookup.md) |
| Match names that sound alike | [Phonetic encoding](../reference/text/phonetics.md) |
| Reduce words to their stem | [Stemming](../reference/text/stemming.md) |
| Turn documents into count, TF-IDF or hashed vectors | [From string to vector](../guides/vectorization.md) |
| Rank documents against a query with BM25 | [Keyword search](../guides/keyword-search.md) |
| Pull the keywords out of a document | [Keyword extraction](../guides/keyword-extraction.md) |
| Tokenize for a transformer: WordPiece, SentencePiece, BPE | [Tokenization](../reference/embeddings/tokenization.md) |
| Embed sentences and search them by meaning | [Semantic search with embeddings](../guides/embeddings.md) |
| Run a sentence-transformer exported to ONNX | [ONNX](../guides/onnx.md) |
| Serve those embeddings through `Microsoft.Extensions.AI` | [Embedding generation](../reference/extensions-ai/generation.md) |
| Store vectors in process, with hybrid keyword and vector search | [Vector store](../reference/extensions-vectordata/store.md) |
| Score a classifier, a regressor, a clustering or a ranking | [Which metric?](../guides/metrics.md) |
| Scale, encode or impute features | [Feature scaling](../reference/preprocessing/scaling.md), [encoding and imputation](../reference/preprocessing/encoding.md) |
| Reshape a feature: bins, polynomials, quantiles, powers | [Feature transforming](../reference/preprocessing/transforming.md) |
| Split rows into cross-validation folds | [Splitting](../reference/preprocessing/splitting.md) |
| Group samples into clusters | [Partitioning](../reference/cluster/partitioning.md) |
| Reduce a sparse matrix: truncated SVD, NMF | [Decomposition](../guides/decomposition.md) |
| Turn a prediction into an interval with guaranteed coverage | [Conformal prediction](../guides/conformal.md) |
| Run a classical hypothesis test | [Hypothesis testing](../guides/hypothesis-testing.md) |
| Fit a regression and read its inference table | [Regression inference](../guides/regression-inference.md) |
| Diagnose a time series: autocorrelation, unit roots, seasonality | [Time-series diagnostics](../guides/time-series-diagnostics.md) |
| Estimate survival under censoring | [Survival analysis](../guides/survival-analysis.md) |
| Run the heavy kernels on a GPU | [GPU kernels](../guides/gpu-kernels.md) |
| Hand a sparse matrix to Math.NET and back | [Matrix conversion](../reference/extensions-mathnet/conversion.md) |

## Coming from Python

- [From Python to .NET](../migration/README.md) — which .NET library covers each Python package
  Lodestar does not rewrite.
- [Performance](../guides/performance.md) — each capability measured against the .NET library it
  replaces.
- [Decisions](../decisions/README.md) — why the project is shaped as it is, and where a result
  deliberately differs from the Python reference.
