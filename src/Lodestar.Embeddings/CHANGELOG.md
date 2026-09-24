# Changelog — Lodestar.Embeddings

What changed in `Lodestar.Embeddings`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.8.0] — 2026-09-24

### Changed

- `BlockNormalization`, `SentencePieceType`, `SplitBehavior`, `TruncationStrategy`, `BpeSplitStep`, `ISubwordTokenizer`, `MergePair`, `NpyBlock`, `SearchResult`, `SentencePiece` and `TokenizationResult` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- Added tokens are found in one pass over the text, 1.6× faster over a Llama-3-shaped table, and the SentencePiece-lineage `BpeTokenizer`, `PrecompiledNormalizer` and `EmbeddingIndex`'s stored-row normalization allocate or divide less. ([#849](https://github.com/CyrilB1531/lodestar/issues/849))
- `EmbeddingIndex.Search` keeps the best k in a bounded heap instead of sorting every score, allocating k results rather than the whole index. ([#813](https://github.com/CyrilB1531/lodestar/issues/813))
- `SentencePieceTokenizer` and `WordPieceTokenizer` find their pieces by walking a trie. ([#713](https://github.com/CyrilB1531/lodestar/issues/713), [`285a8ced`](https://github.com/CyrilB1531/lodestar/commit/285a8ced))
- `BpeTokenizer` encodes byte-level text 5.4× faster. ([#673](https://github.com/CyrilB1531/lodestar/issues/673), [`c2a848df`](https://github.com/CyrilB1531/lodestar/commit/c2a848df))
- `BpeTokenizer` caches each piece's merged ids. ([#743](https://github.com/CyrilB1531/lodestar/issues/743), [`6ab11a2c`](https://github.com/CyrilB1531/lodestar/commit/6ab11a2c))
- `EmbeddingIndex.Load` reads a stream of undeclared length into pooled segments. ([#716](https://github.com/CyrilB1531/lodestar/issues/716), [`b7eb6e48`](https://github.com/CyrilB1531/lodestar/commit/b7eb6e48))
- `System.Text.Json` moves from 10.0.10 to 10.0.12 on `netstandard2.0`. ([#622](https://github.com/CyrilB1531/lodestar/issues/622), [`8603bb01`](https://github.com/CyrilB1531/lodestar/commit/8603bb01))
- **Breaking, in ids.** `VocabTxtLoader` returns a vocabulary with the new `WordPieceVocabulary.BasicTokenization` set, so a `vocab.txt` tokenizes through BERT's BasicTokenizer as `BertTokenizer` does, where accented words of an uncased model became `[UNK]` and punctuation runs stayed whole. ([#883](https://github.com/CyrilB1531/lodestar/issues/883))
- BERT's basic tokenization tells from its own loop whether it changed the text and whether it held an unassigned code point, instead of comparing the result with the input and scanning it a second time, which takes accented text under an uncased model back below its pre-#992 time at #992's allocations; a word's code points are counted only once its UTF-16 length is over the cap. ([#1048](https://github.com/CyrilB1531/lodestar/issues/1048), [#1050](https://github.com/CyrilB1531/lodestar/issues/1050), [#1052](https://github.com/CyrilB1531/lodestar/issues/1052))

### Fixed

- `Mmr.Select` refuses a `NaN` lambda with `ArgumentOutOfRangeException` instead of throwing `IndexOutOfRangeException`. ([#886](https://github.com/CyrilB1531/lodestar/issues/886))
- `BpeVocabulary` equality compares `PrefixTokens` and `SuffixTokens`, so a vocabulary with the Llama-2 template no longer equals one without it. ([#885](https://github.com/CyrilB1531/lodestar/issues/885))
- `NpyBlock` compares its elements and shape by value, and `Pooler`'s methods refuse a negative size with `ArgumentOutOfRangeException` instead of overflowing. ([#902](https://github.com/CyrilB1531/lodestar/issues/902))
- `WordPieceTokenizer` and the `BpePatterns.Whitespace` split keep spacing and enclosing marks, letter numbers, ZWJ/ZWNJ, circled letters and astral letters inside a word, as `pre_tokenizers.Whitespace()` does. ([#887](https://github.com/CyrilB1531/lodestar/issues/887))
- The `vocab.txt` route keeps unassigned code points as `tokenizers` does, where it dropped them as control characters and a recent emoji vanished instead of becoming `[UNK]`. ([#983](https://github.com/CyrilB1531/lodestar/issues/983))
- BERT's normalizer keeps two adjacent code points .NET calls unassigned in the order `tokenizers` keeps them, where normalizing the whole string let ICU's newer tables reorder them past an accent strip that does not remove them — and the sweep that reported that change as output-preserving never put two of them side by side. ([#1087](https://github.com/CyrilB1531/lodestar/issues/1087), [#1088](https://github.com/CyrilB1531/lodestar/issues/1088), [#1089](https://github.com/CyrilB1531/lodestar/issues/1089), [#1090](https://github.com/CyrilB1531/lodestar/issues/1090), [#1091](https://github.com/CyrilB1531/lodestar/issues/1091))
- BERT's normalizer passes a code point the runtime refuses to normalize through as it passes an unassigned one, where NLS on the `netstandard2.0` asset could throw `ArgumentException` out of `Encode` for one emoji newer than the operating system. ([#1094](https://github.com/CyrilB1531/lodestar/issues/1094))
- `WordPieceTokenizer` caps a word at `maxCharsPerWord` code points rather than UTF-16 units, where a 51-character word outside the BMP became `[UNK]` under the default 100. ([#992](https://github.com/CyrilB1531/lodestar/issues/992))
- The `vocab.txt` route's record says what the route does: [decision 0146](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0146-berts-normalizer-keeps-the-unassigned-code-points.md) amends 0144's dropped-Cn clause, `docs/equivalence.md` credits the ~600-code-point residue to the sweep that measured it and lists the four code points diverging the other way, and the `CA1308` suppression left behind on `Normalize` is gone. ([#1049](https://github.com/CyrilB1531/lodestar/issues/1049), [#1051](https://github.com/CyrilB1531/lodestar/issues/1051), [#1053](https://github.com/CyrilB1531/lodestar/issues/1053), [#1064](https://github.com/CyrilB1531/lodestar/issues/1064))

## [0.7.0] — 2026-09-10

`Lodestar.Embeddings` 0.7.0 is a **minor** bump and not a patch, because it carries a data
break: a `normalized: true` added token now matches on the pattern its normalizer makes, so
**Llama-2 ids change** — an embedding produced through that lineage by 0.6.0 carries an
extra `▁` and has to be regenerated. The version number is where a caller who reads nothing
else will see that, which is why the fix did not go out as 0.6.1.

### Fixed

- **A special token written as ordinary text no longer encodes two ways on Llama-2, and no longer becomes a control token in the middle of a sentence.** `tokenizers` normalizes a `normalized: true` added token's **content** with the file's declared normalizer, so Llama-2 — whose whitespace escape is a `Prepend`+`Replace` normalizer — matches on `▁<s>`, not `<s>`. `BpeTokenizer` built that pattern from the Unicode forms alone while escaping the text it searched, which parted two ways: `"<s>"` encoded to `['▁', '<s>']` where the reference answers the single id `1`, and `"the cat<s>"` matched the added token where the reference does **not** — so a caller's own `<s>` silently became the BOS id. The escape now joins the pattern only where the file spelled it as a normalizer; Mistral v0.1, which declares its entries raw under a `Metaspace` pre-tokenizer, is unchanged and was already exact. A `Metaspace` pre-tokenizer carrying a `normalized: true` entry is refused at construction rather than approximated — the escape would depend on the piece's position, which a fixed pattern cannot carry. [Decision 0085](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0085-a-normalized-added-tokens-pattern-carries-the-normalizers-escape.md) amends [0062](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0062-the-two-metaspace-spellings-part-on-the-prepend-twice.md) with the third place the two spellings part, and `sentencepiece_bpe_lineage.json` grows from 16 rows to 26, freezing the five texts [#318](https://github.com/CyrilB1531/lodestar/issues/318) had taken out rather than settle. **Llama-2 ids change**: an embedding produced through this lineage by 0.6.0 carries the extra `▁` and must be regenerated. ([#551](https://github.com/CyrilB1531/lodestar/issues/551))

## [0.6.0] — 2026-09-08

### Added

- **`Mmr.Select` (`Lodestar.Embeddings.Search`) picks a diverse, relevance-weighted subset of candidate vectors — Maximal Marginal Relevance**, knowing nothing about text: the candidates are vectors and the result is their indices, in selection order. It replays `keybert` 0.9.0's own selection step, `keybert._mmr.mmr`, compared as a set rather than a sequence (`tests/oracles/mmr.json`) — [decision 0077](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0077-the-keyword-extractors-take-their-oracles-lists-and-not-their-own.md) has the three divergences, and [decision 0078](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0078-keybert-is-declared-nodeps-not-compiled-into-the-lock.md) why `keybert` itself stays out of the oracle lock file. Composes with `Rake` and `OnnxTextEmbedder` into a KeyBERT-style pipeline, walked through in [`docs/guides/keyword-extraction.md`](../../docs/guides/keyword-extraction.md). ([#525](https://github.com/CyrilB1531/lodestar/issues/525))

- **`LoadBpe` reads a `TemplateProcessing` post-processor instead of refusing the file, which is what let Llama-2 and Mistral v0.1 load at last.** Neither `Metaspace` nor `byte_fallback` was the obstacle — both shipped earlier — but a `post_processor` section was refused outright, and all three reference files carry one. `BpeVocabulary.PrefixTokens` and `BpeVocabulary.SuffixTokens` now carry what the `single` template puts around the text, `["<s>"]` and nothing for both models; they are public because the caller composes the `SpecialTokenTemplate` itself, that type needing a pad token the file does not declare. The `pair` template is read and discarded — [decision 0083](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0083-the-pair-template-is-read-and-discarded.md) has the three files measured before any code was written, and why reproducing a two-sequence encoding would mean inventing a type to express it. ([#548](https://github.com/CyrilB1531/lodestar/issues/548))

### Removed

- **The `SentencePieceTokenizer(IReadOnlyList<SentencePiece>, int)` constructor is gone.** It guessed which pieces were controls from their ids being 0, 1 or 2, which is wrong for any model laying them out differently; `SentencePieceTokenizer(SentencePieceVocabulary)` is told instead, by the file. It carried an `[Obsolete]` since 0.4.0 — **whose message named v2.0.0, and this removal lands in 0.6.0 instead**: v2.0.0 is not a release this project has planned, and waiting for one would mean shipping a constructor that is wrong by construction indefinitely. A caller still on it passes `SentencePieceModelLoader.Load` or `TokenizerJsonLoader.LoadUnigram` output to the remaining constructor, or builds a `SentencePieceVocabulary` with the piece types it knows. ([#539](https://github.com/CyrilB1531/lodestar/issues/539))

- **`OnnxTextEmbedder` moved to the new `Lodestar.Onnx` package, and this one now carries no external dependency at all.** `Microsoft.ML.OnnxRuntime` 1.28.0 was the repository's only external dependency and was reached by one file of 407 lines, while the four sub-word tokenizers, the batch encoder, the pooling, the `.npy` reader and the SIMD kNN index could not be had without it — `dotnet add package Lodestar.Embeddings` restored a native runtime for a caller who only tokenizes. Migration is one `using`: the type is `Lodestar.Onnx.OnnxTextEmbedder`, with the same members and the same behaviour, in a package that depends on this one. [Decision 0076](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0076-a-core-package-carries-no-external-dependency.md) states the rule it settles — a core package carries no external dependency, an external dependency earns its own satellite package — supersedes [0069](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md), and records what was refused. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))

## [0.5.0] — 2026-09-03

One package, published to nuget.org and never tagged, so it had no section here — the
same gap the 2026-09-01 wave was reconstructed for, and filled the same way: each entry
is filed under the release its own commit is an ancestor of. `Lodestar.Embeddings` 0.5.0
is what `Lodestar.Onnx` 0.1.0 depends on, and `src/Directory.Packages.props` pins.

### Added

- **`BatchEncoder.EncodeAll` and `BatchEncoder.Pad` are public**, so a caller that groups rows itself no longer needs a second copy of the padding. `EncodeAll` returns one unpadded row per text, template applied and truncation done; `Pad` lays a **window** of those rows out as one rectangle, widened to the longest row in that window rather than in the corpus — which is what makes grouping by length worth anything. `EncodeBatch` is unchanged, and is still the two of them over the whole corpus at once. ([#533](https://github.com/CyrilB1531/lodestar/issues/533))

- `EmbeddingIndex.FromBlock` and `EmbeddingIndex.FromOwnedBlock` build an index from a contiguous block of vectors in one copy or none, where replaying the block through `Add` cost three times the read that produced it — the adopting factory keeps the caller's array for the life of the index, an invariant the caller keeps and [decision 0056](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0056-a-block-may-be-adopted-and-the-invariant-is-the-callers-to-keep.md) argues for. ([#474](https://github.com/CyrilB1531/lodestar/issues/474), [`13bdacc`](https://github.com/CyrilB1531/lodestar/commit/13bdacc))

- `bench/Lodestar.Text.Benchmarks -- sidecar` prices a binary sidecar against the JSON artifact in bytes and in time, and [decision 0055](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0055-the-artifact-gets-a-binary-sidecar-once-a-block-can-be-ingested-whole.md) takes one — conditional on a bulk ingest into `EmbeddingIndex`, without which the sidecar route is slower than what it replaces. No shipped behaviour changes yet. ([#436](https://github.com/CyrilB1531/lodestar/issues/436), [`7ab80d1`](https://github.com/CyrilB1531/lodestar/commit/7ab80d1))

- **numpy's `.npy` reads and writes, for the vector block only.** `NpyFile.Read` and `NpyFile.Write` carry a contiguous `float32` block in numpy's own format, returning an `NpyBlock` of the values and the shape; the header is parsed against a fixed grammar and never evaluated, so `descr: '|O'` — numpy's pickle-backed dtype — is refused by name before the payload is touched. It is interop and not a second artifact format: a `.npy` carries no ids, no normalize flag and no schema, so `EmbeddingIndex.Save` is untouched and [decision 0011](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0011-persistence-format.md) is not reopened. ([#450](https://github.com/CyrilB1531/lodestar/issues/450), [`0f05972`](https://github.com/CyrilB1531/lodestar/commit/0f05972))

- **`byte_fallback` resolves an uncovered symbol into `<0xXX>` byte pieces instead of the unknown token, so Llama-2 and Mistral v0.1 both load.** `BpeVocabulary.ByteFallback` and `TokenizerJsonLoader.LoadBpe` require the vocabulary to carry all 256 pieces, refusing by name a file that does not rather than reproduce the silent degradation — or, with no unknown token declared, the dropped symbol — `tokenizers` 0.23.1 falls back to; the expansion runs before the merges, on the decorated symbol, so a `continuing_subword_prefix` or `end_of_word_suffix` on it is itself encoded as bytes. `BpeTokenizer.Decode` now reproduces such a file's `decoder` block too, a bare `ByteFallback` or Llama-2's own `Sequence[Replace, ByteFallback, Fuse, Strip]`, round-tripping the byte pieces and the whitespace escape together — [decision 0063](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0063-byte-fallback-requires-the-whole-alphabet-and-its-decoder-is-read-strictly-too.md) has the measurements against the reference, including an upstream ordering bug found and not reproduced. ([#317](https://github.com/CyrilB1531/lodestar/issues/317), [`6b4f2b6`](https://github.com/CyrilB1531/lodestar/commit/6b4f2b6))

### Changed

- **The `.npy` read copies the block once, and a second entry point copies it none.** `NpyFile.Read(Stream)` reads the payload straight into the `float[]` the returned block keeps, where it used to stage the same bytes through two buffers first, and names that array as `NpyBlock.OwnedArray` so `EmbeddingIndex.FromOwnedBlock` can adopt it rather than copy the block once more; `NpyFile.Read(ReadOnlyMemory<byte>)` serves a caller already holding the file by **aliasing** those bytes, which must not change while the block lives, and leaves `OwnedArray` null because a borrowed block has no array to hand on. Reading the same 15 360 128 bytes against `np.load` measured 0.21–0.23× of numpy's wall time with three copies between the stream and the block, and a fourth into the index that held it; on the adopting route it now measures **1.00–1.13× cpu and 1.21–1.25× wall** — parity in the first round and slightly ahead in the other two on cpu, the column this project trusts, where it was four to five times behind. The stream read is one copy on `net10.0` and two on `netstandard2.0`, which has no `Stream.Read(Span<byte>)` to read into a caller's array — one API and one behaviour at two speeds, as [decision 0057](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0057-the-npy-read-serves-a-stream-and-a-buffer-differently.md) records with the view on every path it refused. ([#466](https://github.com/CyrilB1531/lodestar/issues/466), [`a3d3145`](https://github.com/CyrilB1531/lodestar/commit/a3d3145))
- **The payload buffer is rented, not allocated.** `EmbeddingIndex.Load(Stream)` takes its artifact buffer from `ArrayPool<byte>.Shared` and returns it once parsing is done, which removes 20.5 MB of allocation and three of the four collections a load provoked: renting is **42× the allocation and 1.74 ms a load**, about a tenth of one, because what cost was never the allocation but the large-object collection it triggered. The pool holds 33.5 MB for the life of the process in exchange — see [decision 0054](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0054-the-payload-buffer-is-pooled-after-all-because-the-collection-is-the-cost.md), which amends [0053](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0053-the-payload-buffer-is-not-pooled-because-residency-outlives-the-load.md) for refusing that trade without ever timing it. ([#470](https://github.com/CyrilB1531/lodestar/issues/470), [`f8de2ba`](https://github.com/CyrilB1531/lodestar/commit/f8de2ba))
- **Half the allocation, same bytes on disk.** `EmbeddingIndex.Save` and `SaveAsync` write the vector block a slice at a time instead of handing `Utf8JsonWriter.WriteBase64String` the whole thing, so the writer's buffer no longer doubles its way up to the 20.48 MB the encoding occupies: `EmbeddingIndexSave` allocates **19.87 MB against 39.64**, with a third fewer collections in every generation, and the row against `numpy.save` moves **0.29× to 0.39×**. Slices are 245 760 bytes — a multiple of 12, so a whole number of base64 groups and of floats — which is what makes the artifact byte-for-byte what it was; `SaveAsync` loses its intermediate `MemoryStream` with it. The load pays part of it back, having been subsidised by the buffer the save used to leave behind — see [decision 0051](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0051-the-save-paths-cost-is-the-buffer-not-the-encoding.md), which also records why parallelising the base64 was refused: it runs at `memcpy` speed already. ([#430](https://github.com/CyrilB1531/lodestar/issues/430), [`2a50cc1`](https://github.com/CyrilB1531/lodestar/commit/2a50cc1))

### Fixed

- `NpyFile.Read` bounds a block by `ArtifactLoadOptions.MaxTotalBytes` rather than by `MaxArrayLength`, which that option documents as not applying to a vector block: a 2 605 × 384 block — small for embeddings — was refused at the default options while the same vectors loaded from an index artifact. ([#468](https://github.com/CyrilB1531/lodestar/issues/468), [`c480c1f`](https://github.com/CyrilB1531/lodestar/commit/c480c1f))

## [0.4.0] — 2026-08-21

### Added

- The embeddings guide documents how to compress an artifact, and what it costs: the caller wraps the stream on both sides, which works today and needed no library change. Deflate takes back the format's 1.33× base64 expansion almost exactly, at **26.67× the save and 7.19× the load**, and 76.8× and 14.8× on the benchmark corpus's larger index — so the library declines to do it by default, and [decision 0044](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0044-compression-belongs-to-the-caller.md) records why. `compare-persistence` now reports the bytes each row wrote or read, next to its time. ([#378](https://github.com/CyrilB1531/lodestar/issues/378), [`01642c9`](https://github.com/CyrilB1531/lodestar/commit/01642c9))
- `EmbeddingIndex.Load(ReadOnlyMemory<byte>)` reads an index from bytes the caller already holds — a blob, a cache entry, an embedded resource — where handing them to the `Stream` overload made the loader copy them back out first. Measured **1.40×** on processor time against that overload, both rows in the same run, which is the read phase [#324](https://github.com/CyrilB1531/lodestar/issues/324) profiled at about a third of the load. It checks `MaxTotalBytes` before parsing rather than while reading, the length being known up front, and has no `Async` counterpart because nothing is waited on. It is the only loader to gain one: the saving scales with the artifact and no other is large enough. ([#336](https://github.com/CyrilB1531/lodestar/issues/336), [`27fa908`](https://github.com/CyrilB1531/lodestar/commit/27fa908))

### Changed

- **`EmbeddingIndex.LoadAsync` no longer refuses an index that `Load` accepts.** The segmented read #377 gave the synchronous path stopped there, so the same artifact past the CLR's array ceiling loaded one way and threw the other — a disagreement between two overloads of the same method rather than a missing feature. Both now take the same decision on the same threshold, and the chain they build is one implementation so they cannot drift apart again; a cancelled read throws instead of parsing a partial chain. ([#396](https://github.com/CyrilB1531/lodestar/issues/396), [`3a89dde`](https://github.com/CyrilB1531/lodestar/commit/3a89dde))
- **An index is no longer capped by the text encoding of its vectors.** An artifact past the CLR's array ceiling was read into one `byte[]` and could not be, so the format's 1.34× expansion came straight off the largest index that could exist — about 1.04 million vectors at 384 dimensions where the raw block allowed 1.40 million. `EmbeddingIndex.Load` now reads such an artifact in segments and hands the parser a `ReadOnlySequence<byte>`, which `Utf8JsonReader` reads natively. **The bytes on disk do not change**, so an artifact written by any earlier version still loads. ([#377](https://github.com/CyrilB1531/lodestar/issues/377), [`cfc1945`](https://github.com/CyrilB1531/lodestar/commit/cfc1945))
- **Faster, same answers.** Loading an artifact no longer has the runtime zero the two large buffers it overwrites in full — the payload the stream fills and the vector block the base64 decoder fills. Measured **1.18×** on `embedding_index_load`, with a write-only operation re-run as an untouched control; a small artifact such as a fitted vectorizer sees nothing, its buffers never reaching the large-object heap. ([#324](https://github.com/CyrilB1531/lodestar/issues/324), [`359d889`](https://github.com/CyrilB1531/lodestar/commit/359d889))
- **Faster, same bytes.** `EmbeddingIndex.Save` no longer allocates and copies the whole vector block before encoding it: on a little-endian machine the bytes to base64 are the ones already in the span, and the copy existed only to carry an endianness swap that is a no-op there. Measured **1.46×** on processor time at the benchmark's size, with the load direction re-run as an untouched control, and the encoding pinned byte for byte by a new test. ([#323](https://github.com/CyrilB1531/lodestar/issues/323), [`4359d32`](https://github.com/CyrilB1531/lodestar/commit/4359d32))

## [0.3.1] — 2026-08-16

### Changed

- The package is `Lodestar.Embeddings`, and its namespaces are `Lodestar.Embeddings.*`. `Lodestar.Embeddings 0.3.1` holds the same code as `DataNet.Embeddings 0.3.0`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

## [0.3.0] — 2026-08-14 (published as DataNet.Embeddings)

### Added

- Vocabulary loaders cover the three formats a pretrained tokenizer ships in: `vocab.txt`, `tokenizer.json` and `spiece.model`. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `WordPieceVocabulary` and `SentencePieceVocabulary` carry the settings that change tokenization: the unknown token, the continuation prefix, lowercasing, and piece type. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `SentencePieceTokenizer(SentencePieceVocabulary)` decides what may match text from each piece's declared type. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- The loaders refuse a file whose pipeline they do not reproduce — an `NFKC` or precompiled normalizer, a `BertPreTokenizer`, a `post_processor` inserting `[CLS]`/`[SEP]` — naming what they found. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- `added_tokens` are read rather than dropped, reaching both tokenizers instead of tokenizing to the unknown token. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))
- The four `added_tokens` matching flags that decide where an entry matches now apply on both tokenizers. ([#104](https://github.com/CyrilB1531/data.net/issues/104), [`21f808b`](https://github.com/CyrilB1531/data.net/commit/21f808b))
- WordPiece added tokens are matched as text, not folded into the vocabulary, changing tokenization for any `tokenizer.json` carrying a non-empty `added_tokens` table. ([#104](https://github.com/CyrilB1531/data.net/issues/104), [`96b1b6b`](https://github.com/CyrilB1531/data.net/commit/96b1b6b))
- `BpeTokenizer`, `BpeVocabulary`, `BpeFilesLoader` and `TokenizerJsonLoader.LoadBpe` add a third sub-word tokenizer, matching `tokenizers.models.BPE` in both its classic and byte-level lineages, with byte-level `Encode`/`Decode` round-tripping any well-formed string exactly. ([`b46c474`](https://github.com/CyrilB1531/data.net/commit/b46c474))
- `continuing_subword_prefix` loads instead of being refused, applied to every symbol after the first of each pre-tokenized piece on the classic, non-byte-level lineage. ([#120](https://github.com/CyrilB1531/data.net/issues/120), [`dfa7639`](https://github.com/CyrilB1531/data.net/commit/dfa7639))
- `fuse_unk` loads instead of being refused: a run of consecutive uncovered characters becomes one unknown token rather than one each. ([#119](https://github.com/CyrilB1531/data.net/issues/119), [`c91f3ef`](https://github.com/CyrilB1531/data.net/commit/c91f3ef))
- The merge loop threads symbols on a doubly-linked list and a hand-rolled priority queue, replacing a rescan-and-shift loop that was quadratic on a token with no split point. ([`b46c474`](https://github.com/CyrilB1531/data.net/commit/b46c474))
- A batch encoding pipeline — `BatchEncoder`, `EncodingOptions`, `SpecialTokenTemplate`, `EncodedBatch`, `ISubwordTokenizer` — now owns matching a model's special-token wrapping instead of leaving it to the caller. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `OnnxTextEmbedder.EmbedBatch` takes text in and returns one normalized vector per text out, in input order, mirroring `SentenceTransformer.encode`. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `CancellationToken` is now accepted on every batch entry point. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `Pooler.MeanPoolBatch` and `MeanPoolAndNormalizeBatch` pool a `[batch, seq, dim]` tensor with each row against its own mask slice. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- `EmbeddingIndex.Save`/`Load`, with `SaveAsync`/`LoadAsync` counterparts, round-trip a built index so embedding a corpus is not lost with the process. ([#62](https://github.com/CyrilB1531/data.net/issues/62), [`7e093c9`](https://github.com/CyrilB1531/data.net/commit/7e093c9))
- `EmbeddingIndex.Add(vector, id)`, `GetId` and `HasIds` attach an opaque id to each vector, kept off `SearchResult`. ([#62](https://github.com/CyrilB1531/data.net/issues/62), [`c06b472`](https://github.com/CyrilB1531/data.net/commit/c06b472))

### Changed

- A `Sequence`'s `Split` step whose `pattern` declares both `Regex` and `String` is now refused, where it loaded by silently reading the first. ([#167](https://github.com/CyrilB1531/data.net/issues/167), [`01c0de1`](https://github.com/CyrilB1531/data.net/commit/01c0de1))
- `EmbeddingIndex.Load` now moves a vector block in three passes instead of five. ([#100](https://github.com/CyrilB1531/data.net/issues/100), [`114245f`](https://github.com/CyrilB1531/data.net/commit/114245f))
- `OnnxTextEmbedder.Embed` takes `ReadOnlySpan<long>` where it took `IReadOnlyList<long>`, a source break that removes two defensive copies per call. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- The default output is chosen deterministically instead of by dictionary key order. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- An output of unexpected rank now throws instead of producing an out-of-range access or a silently wrong result. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- The zero `token_type_ids` buffer is thread-static and never written to, instead of being allocated per call. ([#60](https://github.com/CyrilB1531/data.net/issues/60), [`c67b6c5`](https://github.com/CyrilB1531/data.net/commit/c67b6c5))
- An added token is a token, not a vocabulary entry: a single-character added token `model.vocab` does not declare no longer makes that character look covered. ([#130](https://github.com/CyrilB1531/data.net/issues/130), [`d785b86`](https://github.com/CyrilB1531/data.net/commit/d785b86))
- `BpeVocabulary.PreSplitPattern` becomes `PreSplit`, a `BpeSplitStep` carrying the pattern, the `behavior` and the `invert` flag together. ([#145](https://github.com/CyrilB1531/data.net/issues/145), [`9546b1c`](https://github.com/CyrilB1531/data.net/commit/9546b1c))
- A `BpeVocabulary` has to say how its text is split, and is refused when it declares none of `PreSplit`, `PreTokenizerPattern` or `NoPreTokenizer`. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`545c51e`](https://github.com/CyrilB1531/data.net/commit/545c51e))

### Deprecated

- `SentencePieceTokenizer(IReadOnlyList<SentencePiece>, int)`, the id-based constructor, is deprecated in favor of building a `SentencePieceVocabulary` with a loader. ([#58](https://github.com/CyrilB1531/data.net/issues/58), [`d147abd`](https://github.com/CyrilB1531/data.net/commit/d147abd))

### Fixed

- A merge pair listed twice in `model.merges` now keeps its last occurrence instead of its first, changing the tokens produced for a file that repeats one. ([#160](https://github.com/CyrilB1531/data.net/issues/160), [`708982f`](https://github.com/CyrilB1531/data.net/commit/708982f))
- A `Sequence` of `Split` then `ByteLevel` now applies both patterns instead of only the `Split` step's, changing the tokens produced for Llama-3 and Qwen2 on ordinary text. ([#143](https://github.com/CyrilB1531/data.net/issues/143), [`9a8d15c`](https://github.com/CyrilB1531/data.net/commit/9a8d15c))
- A `Sequence`'s `Split` step now honours its `behavior` and `invert` fields instead of always acting as `Removed` with `invert: true`. ([#145](https://github.com/CyrilB1531/data.net/issues/145), [`9546b1c`](https://github.com/CyrilB1531/data.net/commit/9546b1c))
- A `tokenizer.json` declaring no `pre_tokenizer`, or a bare `ByteLevel` step with `use_regex` off, now loads as `BpeVocabulary.NoPreTokenizer` instead of the `Whitespace` split. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`545c51e`](https://github.com/CyrilB1531/data.net/commit/545c51e))
- With a `Sequence` pre-tokenizer and `add_prefix_space` on, the space now goes on every piece the `Split` step produces instead of once per added-token segment, so `"a|b|c|d"` decodes to `" a | b | c | d"` where it decoded to `" a|b|c|d"`. ([#122](https://github.com/CyrilB1531/data.net/issues/122), [`26481a9`](https://github.com/CyrilB1531/data.net/commit/26481a9))
- A `Sequence`'s `Split` step whose pattern is spelled `{"String": …}` now loads, the literal escaped into the regex matching exactly it, instead of being refused for declaring no `pattern.Regex`. ([#167](https://github.com/CyrilB1531/data.net/issues/167), [`01c0de1`](https://github.com/CyrilB1531/data.net/commit/01c0de1))

## [0.2.0] — 2026-08-05

Reach, correctness and honesty about performance. Nothing in the public API was
removed or renamed, so upgrading from `0.1.0` is a version bump.

### Added

- `netstandard2.0` becomes a second target framework, reaching .NET Framework 4.6.1+, Mono, Xamarin and Unity through conditional compilation rather than a reduced API. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Four Snowball stemmers join English and French: `SpanishSnowballStemmer`, `PortugueseSnowballStemmer`, `ItalianSnowballStemmer` and `GermanSnowballStemmer`. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Blocked (multi-word) Myers removes the 64-character cap on `Levenshtein.Distance`'s bit-parallel path. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A benchmark suite compares the `net10.0` and `netstandard2.0` builds of the same library. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Mirror test projects replay the entire suite against the `netstandard2.0` assemblies, 339 tests across both builds. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))
- A sample under `samples/DataNet.Sample` consumes the packages by `PackageReference` from a locally packed feed, and runs in CI. ([#50](https://github.com/CyrilB1531/data.net/issues/50), [`391a71c`](https://github.com/CyrilB1531/data.net/commit/391a71c))
- `CONTRIBUTING.md` and this changelog are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- SonarQube Cloud analysis, a `lint` CI job (markdownlint and `dotnet format`), and Dependabot for GitHub Actions are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Changed

- Long-string `Levenshtein.Distance` is 20–33× faster: 684 µs to 21 µs at 512 characters. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Regular expressions are bounded by a match timeout: a pathological pattern now raises `RegexMatchTimeoutException` instead of hanging the calling thread. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Warnings are errors across the whole repository, covering `src`, `tests` and `bench` rather than the libraries alone. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Fixed

- Static-analysis defects fixed and verified against the oracle corpora: an `int` division widened to `double` in `Jaro`, nested classes shadowing their outer type in the Snowball stemmers, unread step-method return values, and nested ternaries in three files. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Code coverage was never collected: CI referenced `coverlet.collector` without depending on it, so the collection step silently did nothing. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Security

- A `workflow_dispatch` input was interpolated directly into a shell command in a job holding `id-token: write`, letting it mint a nuget.org publishing key; values now reach the shell through the environment. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- GitHub Actions are pinned to full commit SHAs, so a moved tag cannot change what runs in CI. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- CI dependency installation is hardened: markdownlint pinned with lifecycle scripts disabled, and `pip install --require-hashes` against a generated lock file pinning all 29 packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Documentation

- Package metadata now attributes the project to Cyril BRUNET (`Authors`, `Company`, `Copyright`). ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))
- `THIRD-PARTY-NOTICES.md` now records the shipped dependencies instead of saying "None yet". ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))

### Notes

- Deliberate analyzer suppressions live in the source as `#pragma warning disable` with their justification, since SonarLint reads neither `.editorconfig` nor workspace settings. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- The `netstandard2.0` build is behavior-verified: the whole suite is replayed against those assemblies, not only compiled. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))

## [0.1.0] — 2026-08-01

First release. All four lots of the project brief are delivered, and every
building block is validated by replaying frozen reference outputs captured from
the canonical Python libraries — see [`docs/equivalence.md`](../../docs/equivalence.md).

### Added

- Lot 3 — embeddings and semantic search (`DataNet.Embeddings`): WordPiece and SentencePiece (unigram Viterbi) tokenizers, pooling, SIMD kNN, ONNX inference, with ONNX Runtime isolated to this package. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Migration guides for NumPy, pandas, scikit-learn, statsmodels, PyTorch, matplotlib and seaborn, plus a three-column inventory mapping each need to use / build / decide. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A decision log records the deliberate divergences from the Python references. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Publishing to nuget.org via Trusted Publishing (keyless, OIDC) and to GitHub Packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
