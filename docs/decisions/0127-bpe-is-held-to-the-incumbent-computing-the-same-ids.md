---
status: accepted
supersedes: []
amends: ["0017"]
applies: []
---
# 0127 — Byte-level BPE is held to the incumbent computing the same ids, and its unigram ratio is a record

**Status:** accepted · **Date:** 2026-09-14 · **Amends:** [`0017`](0017-bpe-parity-scope.md), its merge-loop section's acceptance bar

## Context

[0017](0017-bpe-parity-scope.md) cleared [#59](https://github.com/CyrilB1531/lodestar/issues/59)'s
bar with one reading: `BpeBenchmarks.Bpe` at **1.08×** `Unigram`, "comfortably inside the 2×
acceptance bar". Its other amendment, [0050](0050-the-sentencepiece-bpe-lineage-stays-a-bpe-model.md),
concerns a different section and leaves the bar alone.

[#673](https://github.com/CyrilB1531/lodestar/issues/673) watched the ratio leave the bar without
BPE changing:

- `8de0da96` stopped the unigram tokenizer building strings only to hash them, and the ratio
  went to 1.84.
- [#713](https://github.com/CyrilB1531/lodestar/issues/713) gave the unigram tokenizer a trie, and
  it went to **17.97**.

The issue asked to take the same fixes to BPE and re-measure, or to record here why the ratio
cannot come back and what the bar now means. Both happened.

## What the encode path got

Measured on AMD Ryzen 7 8700G, Ubuntu 26.04.1, .NET 10.0.12, BenchmarkDotNet 0.14.0 default job,
over the 5,000 documents of `bench/corpus/vocabs/documents.json`. `docs/guides/performance.md`
has the table and the cost of each step.

| | `origin/main` | this change |
| --- | ---: | ---: |
| `BpeBenchmarks.Bpe` | 315.17 ms | **58.33 ms** |
| ratio to `Unigram` | 17.97 | **3.36** |
| allocated | 112.18 MB | **28.47 MB** |

The tokens and ids for the whole corpus are byte-identical to `origin/main`'s.

## Why the unigram ratio does not come back to 2×

**The two benchmarks do not do the same work.** The corpus is random letter strings, and the
30k unigram model learned most of its 34,274 distinct words whole. It emits **200,000 tokens, one
per word**, which since #713 is one trie walk each. The byte-level BPE model emits **367,354** from
1,381,820 byte symbols, so it applies 1,014,466 merges, each followed by up to two pair lookups.

**What is left is mostly those merges.** With the merge loop switched off, the encode took 25 ms of
its 53, measured with a `Stopwatch` beside the benchmark. That is splitting, byte mapping and the
result lists alone, though it lists one id per byte rather than per token, and already 1.4× the
whole unigram encode. Reaching 2×, about 35 ms, would
leave roughly 10 ms for a million merges.

Two further attempts were measured and not kept:

- **A rescan loop for pieces of up to 16 symbols**, beside the heap, saved 11%. That is not
  worth a second merge algorithm next to the one 0017 chose on its scaling.
- **Interleaving the rank table's keys and values** saved nothing, so its lookups are not
  memory-bound.

A word cache, as `tokenizers` keeps, cannot show on this corpus: its 10,000 most frequent words
cover 47% of its pieces.

## Decision

**The bar byte-level BPE is held to is the incumbent computing the same ids.**
`TokenizerIncumbentBenchmarks` gains a `ByteLevelBpe` model. Both sides read
`tokenizer_30k_bpe.json`, and Microsoft.ML.Tokenizers 2.0.0's `CodeGenTokenizer` is its GPT-2
byte-level BPE. Over the whole corpus the two return identical ids.

| | Lodestar | Microsoft.ML.Tokenizers | ratio |
| --- | ---: | ---: | ---: |
| `origin/main` | 312.60 ms | 146.77 ms | 0.47 |
| this change | **57.95 ms** | 148.73 ms | **2.57** |

**`Lodestar.Embeddings`' BPE encode must not be slower than the incumbent's on that row.** That
is what the WordPiece and SentencePiece rows of the same class already show, and it is what a
caller choosing between the two libraries would ask. It was failing on `origin/main`, at
0.47, and nothing measured it.

**`BpeBenchmarks` keeps `Unigram` as its baseline, as a record.** Its ratio is still the one
number that shows a fix reaching one tokenizer and not the other, as `8de0da96` did. The nightly
series [0126](0126-the-nightly-reports-a-ratio-that-steps-past-its-noise-or-drifts-over-ten-days.md)
records is keyed on it, and changing the baseline would restart that series. That series will
report this change as a step, which is what it is. Its value is no longer measured against 2×.

## Options

- **Keep 2× against `Unigram`, and fail it.** Lost: the bar would stay red for a difference in
  the work, not in the implementation.
- **Raise the bar to 4×.** Lost: a number chosen to pass, which would move again the next time
  either tokenizer improves.
- **Replace `Unigram` with the incumbent inside `BpeBenchmarks`.** Lost: it restarts the nightly
  series for no gain, since the incumbent class already carries the model parameter this needs.

## Consequences

- `BpeBenchmarks`' summary no longer calls itself an acceptance bar.
- The split pattern is now compiled. Matching it over the corpus took 17 ms interpreted and 5
  compiled, of an encode that took 71 ms. The cost is about 2 ms of code generation once per
  tokenizer, at its first encode. Construction stays at about 5 to 6 ms warm. The `netstandard2.0` build compiles it too, and that
  cost was not measured on .NET Framework.
- The single-token scaling rows stay linear. Each doubling from 4,096 to 32,768 characters costs
  2.0 to 2.2×. There is one step of 3.1× between 2,048 and 4,096, and every length is faster than
  on `origin/main`.
- Natural text was not measured. A word cache would be the next thing to measure there, not here.
