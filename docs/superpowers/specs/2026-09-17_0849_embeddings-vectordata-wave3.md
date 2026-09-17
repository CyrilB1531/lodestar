# 0849 — Filtered vector search, added tokens and SentencePiece buffers

**Status:** accepted, 2026-09-17. Written after the measurement it records.

Issue: [#849](https://github.com/CyrilB1531/lodestar/issues/849), found by a performance review of `main`.

## Problem

- A filtered vector search in `Lodestar.Extensions.VectorData` scored and sorted the whole
  collection before the filter ran, whatever `top` was.
- The added-token scanner ran one scan of the text per entry, 256 for a Llama-3-shaped table.
- The SentencePiece-lineage BPE files (Llama-2, Mistral) have no pre-tokenizer, so each document
  is one uncached piece, and every character cut a string to look up.
- The precompiled normalizer allocated the UTF-8 input, a byte list and its copy per call.
- Normalizing a stored row divided one element at a time.

## Change

- The filter reads every record once, in index order; admitted records are scored against the
  index's own normalized block and the best `top + Skip` kept in a bounded heap with
  `EmbeddingIndex.Search`'s total order. **Behaviour change:** `main` ran the filter lazily in rank
  order and stopped once enough records passed. A pure filter gets the same results; a filter with
  side effects or one that throws on some records does not, and an expensive filter is called `n`
  times. The reference page states the new contract.
- The scanner searches once for any entry's first character, then tries that character's
  entries longest first: the earliest position, then the longest entry, as before.
- The decorated symbol is built in a stack buffer and looked up by span on net9+.
- The normalizer rents both byte buffers and copies replacements as spans.
- The row division widens, divides and narrows a `Vector<float>` block at a time; the sum stays
  scalar and in order.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| filtered search, half admitted, top 10, 10,000 × 384 | 867 µs, 164 KB | **462 µs, 7.2 KB** |
| the same, 100,000 × 384 | 10.2 ms, 1.53 MB | 7.36 ms, 7.2 KB |
| Llama-2 BPE, corpus documents | 91.2 ms, 67.4 MB | 88.9 ms, 37.2 MB |
| Mistral v0.1 BPE, corpus documents | 90.3 ms, 67.4 MB | 85.0 ms, 37.2 MB |
| 256 added tokens, chat-template text | 105 ms, 33.3 MB | **64.3 ms**, 35.1 MB |
| the same table, prose holding none | 81.0 ms, 28.5 MB | 52.6 ms, 30.5 MB |
| precompiled normalizer, corpus documents | 8.52 ms, 7.26 MB | 8.74 ms, **2.75 MB** |
| unigram encode over the same | 36.9 ms, 12.7 MB | 37.2 ms, 8.19 MB |
| `FromBlock` normalizing 100,000 × 384 | 82.4 ms, 146 MB | **48.2 ms**, 146 MB |

The scanner allocates 5–7% more. Not measured: text made almost entirely of `<` against a table
whose entries all open on `<|`, where every position tries the whole bucket. The normalizer is an
allocation win only.

## Rejected

- **BPE pre-tokenizer pieces as spans**, looked up in the word cache by span: `BpeBenchmarks` and
  `BpeWordCacheBenchmarks` 0.99–1.01× with 7% less allocation. Reverted.
- **MMR skipping taken candidates**: 1.01–1.05×. Reverted.
- **ONNX `Run` through `OrtValue`**: 1–3% at batch size 1 at best, and removing the second copy
  needs `EncodedBatch` to expose its arrays, a public-surface change.
- **Caching the compiled filter**: hits only when a caller reuses the expression instance, which
  callers rarely do, and `RecordFilter` documents that it does not cache so that fresh closures are
  not held alive.
