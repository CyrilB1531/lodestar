# The Embeddings package and its satellites: review findings before 1.0

**Issue:** [#1209](https://github.com/CyrilB1531/lodestar/issues/1209), with
[#1210](https://github.com/CyrilB1531/lodestar/issues/1210),
[#1213](https://github.com/CyrilB1531/lodestar/issues/1213) and
[#1214](https://github.com/CyrilB1531/lodestar/issues/1214), in one pull request at Cyril's request.
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The pre-1.0 review of `main` at `d228321f` left four findings on `Lodestar.Embeddings` and the
packages built on it: added-token matching off `tokenizers` on overlapping entries, on the word
class and on `İ`; SentencePiece segmenting onto pieces sentencepiece never uses; a loader refusing
every stock BERT `tokenizer.json` for reasons no longer true; and a list of numerical, validation
and cost gaps across Embeddings, Onnx, Extensions.AI, Extensions.VectorData and Gpu.

## Decisions

1. **Added tokens are matched as `tokenizers`' leftmost-longest automaton matches them.** The
   longest entry present wins its position before `single_word` is checked, so a rejected match
   consumes its span (#1209). The word class is `WhitespaceScanner`'s `\w` over code points, which
   is the `regex` crate's (#1213).
2. **`Lowercase` maps U+0130 to `i` + U+0307**, the one unconditional multi-character entry of
   SpecialCasing.txt. A text holding it lowercases per gap, as BERT's normalizer already did,
   since the raw pass's positions no longer hold.
3. **An `UNUSED` SentencePiece piece is not matchable**, as the reference page already said and
   sentencepiece does; its id stays reachable through `TryGetId`.
4. **`LoadWordPiece` reads a stock BERT file.** The default `BertNormalizer` followed by a
   `BertPreTokenizer` is `BasicTokenization`; any other pairing of the two is refused, since they
   are reproduced as one unit. The post-processor is read for all three loaders into
   `PrefixTokens`/`SuffixTokens` — new on `WordPieceVocabulary` and `SentencePieceVocabulary`,
   mirroring `BpeVocabulary` — with `BertProcessing` and `RobertaProcessing` read as `cls $A sep`.
   `truncation` and `padding` are accepted and not read: they are call settings, which
   `EncodingOptions` carries; reading them into the vocabulary would give the library two sources
   of truth for one length.
5. **Embeddings' numerical gaps align with the references.** `Pooler.L2Normalize` divides by
   `max(‖v‖, 1e-12)` as `F.normalize` does; `EmbeddingIndex.Search` normalizes the query with the
   stored rows' own double arithmetic; `NpyFile.Read` byte-swaps on a big-endian host, copying
   there instead of aliasing; `EmbeddingIndex.Save` and `SaveAsync` refuse a non-finite vector
   before the first byte, `SaveAsync` still through a faulted task.
6. **`OnnxTextEmbedder` checks what the model returns.** A rank-3 output must be
   `[batch, seq, dim]` — a transposed `[seq, batch, dim]` had the right element count and pooled
   the wrong rows — and fp16 and bf16 outputs are widened to float rather than failing a cast.
7. **Without `MaxLength`, truncation stops at the model's position table** (Cyril, 2026-09-27,
   over a refusal): read once from the graph, the `Gather` over a `position_embeddings` weight,
   less RoBERTa's `padding_idx + 1` offset where a `CumSum` feeds the index. No table, no
   truncation, as before. sentence-transformers truncates to `max_seq_length`, which no ONNX
   graph carries; passing `MaxLength` matches it exactly.
8. **`OnnxEmbeddingGenerator.GenerateAsync` fails through its task**, as Microsoft.Extensions.AI's
   own generators do, and each embedding carries `ModelId` and one `CreatedAt` per call.
9. **Gpu top-k keeps a heap per lane and merges the lanes**, where the argmax rescanned every row
   per hit; every kernel refuses device data from another context or a disposed one; and
   `GpuSearchResult` is removed for `SearchResult`, its twin, before 1.0.
10. **VectorData's writes are incremental.** Records live in reused slots, each vector normalized
    once at its write; BM25 keeps per-term postings and counts, and a written text is tokenized at
    the next hybrid search rather than at the write. Hybrid fusion reads both rankings only as deep
    as Fagin's threshold algorithm needs. Results equal the from-scratch
    `EmbeddingIndex` + `Bm25Index` + `RankFusion.Rrf` bit for bit, which is why BM25's arithmetic is
    restated rather than delegated.

## Rejected

- **Resuming one position after a rejected `single_word` match**, today's behaviour: it lets a
  shorter overlapping entry match where `tokenizers` never does.
- **Reading `truncation` and `padding` into the vocabulary**: `BatchEncoder` already takes both
  from `EncodingOptions`, and `WordPieceTokenizer.Encode` is the model-level call, which
  `tokenizers` answers without them when asked for the bare encoding.
- **Refusing to embed without `MaxLength` on a symbolic axis**, the first pass at #1214's Onnx
  item: Cyril preferred the limit the graph states to a call that fails.
- **A fixed default length such as 512**: wrong for all-MiniLM-L6-v2 (256) and all-mpnet-base-v2
  (384) on every longer text, and wrong the other way for a model with a longer table.
- **Refusing `BertProcessing` and `RobertaProcessing`** as the BPE path did: both wrap one
  sequence exactly as a `TemplateProcessing` does, and older BERT and RoBERTa files carry them.

## Measured

- Differential test against the Python references before the push: 6,000 random texts over 150
  random WordPiece models with random added-token tables and flags, 3,000 random texts through
  all-MiniLM-L6-v2's own `tokenizer.json` (post-processed and bare), and 2,400 texts over 60
  `tiny_sp.model` variants with random `UNUSED` pieces — 11,400 cases, 0 mismatches against
  `tokenizers` 0.23.2 and sentencepiece 0.2.2. Two added tokens normalizing to one content are
  left out of the draw: `tokenizers` picks between them by `HashMap` order, differently from one
  process to the next, so no answer is the reference's.
- Gpu top-k, A/B/A on an RTX 5070 Ti (CUDA, ILGPU 1.5.3) and a Ryzen 7 8700G, 16 queries of 384
  dimensions: 76.8 ms → 16.0 ms at 100k rows and k = 1000, 4.64 → 2.73 ms at 10k rows, unchanged at
  k = 10; 400 random cases equal to the old kernel's hits exactly, ties included.
- VectorData, A/B/A on the Ryzen 7 8700G: an upsert then a search over 100k records 147.5 ms →
  4.07 ms and 257 MB → 3.1 KB allocated; a hybrid search over 100k 22.06 → 6.97 ms. 300 random
  write-and-search sequences over 9 option sets equal a from-scratch rebuild exactly, ids and
  score bits.
