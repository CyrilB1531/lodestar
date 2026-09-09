# Embedding generation — `Lodestar.Extensions.AI`

One type, [`OnnxEmbeddingGenerator`](generation/onnxembeddinggenerator.md): it presents the ONNX
embedding path as `Microsoft.Extensions.AI`'s `IEmbeddingGenerator<string, Embedding<float>>`, so a
consumer written against that abstraction — a Semantic Kernel pipeline, a `Microsoft.Extensions.AI`
chain, anything that takes the interface — can hold Lodestar embeddings without knowing Lodestar.

This package adds **no arithmetic**. Every vector it returns comes from
`OnnxTextEmbedder.EmbedBatch`, unchanged, so what it owes is the interface contract and not a
numerical one. That is why it carries no oracle corpus of its own: its suite asserts that its
output is *identical* to the embedder's, and the embedder's own frozen corpora say what those
numbers are.

## Why every example here is unexecuted

Weights are never committed to this repository. A running example would need a model of tens of
megabytes, and [`decisions/0003`](../../decisions/0003-provenance-and-licensing.md) rules that
out; `tools/fetch_*.py` pulls vocabularies against a pinned SHA-256 when they are needed, and
weights are not among them.

So the fences on these pages **compile** against the packed package and are marked
`docs-run: skip`, which is what that marker is for — the same arrangement
[`Lodestar.Onnx`](../onnx/inference.md) makes, and for the same reason.

## Where this sits

`Lodestar.Extensions.AI` is the second member of the satellite tier, after `Lodestar.Onnx`. It
exists because a core package carries no external dependency and an external dependency earns its
own package, named for it — [`decisions/0076`](../../decisions/0076-a-core-package-carries-no-external-dependency.md).
A caller who wants embeddings and not the `Microsoft.Extensions.AI` abstractions takes
`Lodestar.Onnx` alone and restores nothing extra.

## Types

| Type | What it is |
| --- | --- |
| [`OnnxEmbeddingGenerator`](generation/onnxembeddinggenerator.md) | Presents an `OnnxTextEmbedder` as an `IEmbeddingGenerator<string, Embedding<float>>`. |

## See also

- [ONNX inference](../onnx/inference.md) — the package this one adapts.
- [Semantic search with embeddings](../../guides/embeddings.md) — the chain it sits in.
- [Python → C# equivalence](../../equivalence.md).
