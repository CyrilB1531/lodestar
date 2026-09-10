# Third-party notices

This file records attributions for third-party components. Per §10.3 of the
project brief, every shipped dependency is listed here with its license.
Build-time and development-only tools — used to compile or to *generate* test
data, but never linked into the distributed libraries — are recorded separately
for traceability.

## Runtime / shipped dependencies

`Lodestar.Text`, `Lodestar.Fuzzy` and `Lodestar.Metrics` have **no** runtime
dependencies on `net10.0`, by design (§3).

| Component | License | Shipped by | Target |
| --- | --- | --- | --- |
| Microsoft.ML.OnnxRuntime | MIT | `Lodestar.Embeddings` | both |
| System.Memory | MIT | all four packages | `netstandard2.0` only |
| System.Numerics.Vectors | MIT | all four packages | `netstandard2.0` only |
| System.Text.Json | MIT | `Lodestar.Text`, `Lodestar.Embeddings` | `netstandard2.0` only |

`System.Memory` and `System.Numerics.Vectors` supply `Span`, `Memory`,
`ArrayPool` and `Vector<T>`, which are in-box on `net10.0`. They appear only in
the `netstandard2.0` dependency group of each package.

`System.Text.Json` is likewise in-box from `net8.0` onwards, and appears only in
the `netstandard2.0` group. It backs the persistence layer — saving a fitted
vectorizer, reading a `tokenizer.json` — and is the one place the "no external
dependencies" rule is knowingly bent rather than a polyfill for something the
modern framework already provides. `Lodestar.Fuzzy` ships no I/O and does not
take it. The reasoning is in
[`docs/decisions/0011-persistence-format.md`](docs/decisions/0011-persistence-format.md).

ONNX Runtime is deliberately isolated to `Lodestar.Embeddings`, so consumers of
the distance, vectorization and fuzzy-matching packages take no native
dependency.

## Redistributed resources (shipped inside the assemblies)

| Component | License | Shipped by | Source |
| --- | --- | --- | --- |
| Snowball stop-word lists (fr, de, it, pt, es) | BSD-3-Clause | `Lodestar.Text` | `https://snowballstem.org/algorithms/<language>/stop.txt` |

These are data, not code: the five lists are compiled into
`Lodestar.Text.Vectorization.StopWords` by `tools/fetch_stopwords.py`, which pins a
SHA-256 per file. Unlike the libraries below, they *are* redistributed, so the
licence travels with them. The English list is scikit-learn's (BSD-3-Clause), not
Snowball's; the nltk stop-word corpus is deliberately not used — see
[`docs/decisions/0010-stop-word-list-provenance.md`](docs/decisions/0010-stop-word-list-provenance.md).

```text
Copyright (c) 2001, Dr Martin Porter,
Copyright (c) 2002, Richard Boulton.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this
   list of conditions and the following disclaimer in the documentation and/or
   other materials provided with the distribution.
3. Neither the name of the copyright holder nor the names of its contributors may
   be used to endorse or promote products derived from this software without
   specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
```

## Redistributed test fixtures (not shipped)

| Component | License | Used by | Source |
| --- | --- | --- | --- |
| `xlm-roberta-base` SentencePiece vocabulary | MIT | `tests/oracles/xlmr_fairseq.model` | `https://huggingface.co/xlm-roberta-base/resolve/main/sentencepiece.bpe.model` |
| `gpt2` byte-level BPE vocabulary and merge table | MIT | `tests/oracles/gpt2_vocab.json`, `tests/oracles/gpt2_merges.txt` | `https://huggingface.co/openai-community/gpt2/resolve/main/vocab.json`, `.../merges.txt` |
| `Mistral-7B-v0.1` SentencePiece-BPE vocabulary and merge table | Apache-2.0 | `tests/oracles/mistral_v01_tokenizer.json` | `https://huggingface.co/mistralai/Mistral-7B-v0.1/resolve/main/tokenizer.json` |
| `Llama-2` SentencePiece-BPE vocabulary and merge table | LLAMA 2 Community License | `tests/oracles/llama2_tokenizer.json` | `https://huggingface.co/TheBloke/Llama-2-7B-fp16/resolve/main/tokenizer.json` |

The **vocabulary only** — the 250 000 pieces, their scores, their types and the
`nmt_nfkc` character map the file's `normalizer_spec` carries (a table compiled
by `sentencepiece`, Apache-2.0, from Unicode data). No model weights are
redistributed, per
[`docs/decisions/0003-provenance-and-licensing.md`](docs/decisions/0003-provenance-and-licensing.md).
The file is compiled into no package: it lives under `tests/`, is copied to the
test output, and exists so the tokenizer's parity claim is checked against a real
multilingual vocabulary in the layout HuggingFace uses. It is re-emitted from the
upstream download by `tools/fetch_xlmr_vocab.py`, which pins the upstream
SHA-256; the reasoning is in
[`docs/decisions/0013-sentencepiece-parity-scope.md`](docs/decisions/0013-sentencepiece-parity-scope.md).

`xlm-roberta-base` is published under the MIT license by its authors (Facebook AI
Research), as declared on its model card:

```text
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

The **vocabulary and merge table only** — the 50 257 token-to-id entries of
`gpt2_vocab.json` and the ranked merge pairs of `gpt2_merges.txt`, in the exact
`merges.txt` layout GPT-2 ships. No model weights are redistributed, per
[`docs/decisions/0003-provenance-and-licensing.md`](docs/decisions/0003-provenance-and-licensing.md).
The files are compiled into no package: they live under `tests/`, are copied to
the test output, and exist so `ByteLevelBpeTests`' claim of byte-exact parity
with HuggingFace `tokenizers` is checked against GPT-2's real 50 257-entry
vocabulary. A self-trained toy model could never exercise a merge table with
50 000 ranks. They are downloaded verbatim by `tools/fetch_gpt2_bpe.py`, which
pins the upstream SHA-256 of each file.

`gpt2` is published under the MIT license by its authors (OpenAI, mirrored as
`openai-community/gpt2`), as declared on its model card:

```text
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

### `Mistral-7B-v0.1` tokenizer

The **vocabulary and merge table only** — the 32 000 entries and 58 980 ranked
pairs of `tokenizer.json`, with the `Metaspace` pre-tokenizer and `byte_fallback`
flags that make it the SentencePiece-BPE lineage. No model weights are
redistributed, per
[`docs/decisions/0003-provenance-and-licensing.md`](docs/decisions/0003-provenance-and-licensing.md).

The file is compiled into no package: it lives under `tests/`, is copied to the
test output, and exists so `SentencePieceBpeLineageOracleTests` checks the
whitespace escape and the byte fallback against a file a user actually has,
rather than a synthetic model built from this project's own understanding of the
format. It is downloaded verbatim by
`tools/fetch_llama2_mistral_tokenizers.py`, which pins its upstream SHA-256 and
corroborates it against a second mirror.

### `Llama-2` tokenizer

The **vocabulary and merge table only** — the 32 000 entries and 61 249 ranked
pairs of `tokenizer.json`, with the `Prepend` plus `Replace` normalizer and
`byte_fallback` flags that make it the other spelling of the SentencePiece-BPE
lineage. **No model weights are redistributed**, per
[`docs/decisions/0003-provenance-and-licensing.md`](docs/decisions/0003-provenance-and-licensing.md)
and `CLAUDE.md`'s hard rule.

Llama 2 is licensed under the **LLAMA 2 COMMUNITY LICENSE AGREEMENT**, which is not
one of the permissive licenses decision 0003 names.
[`docs/decisions/0084-the-llama-2-tokenizer-is-vendored-under-a-bespoke-licence.md`](docs/decisions/0084-the-llama-2-tokenizer-is-vendored-under-a-bespoke-licence.md)
accepts it **for this artifact alone**; 0003's allowed-source list is unchanged, and
every future non-permissive source needs its own decision. The Agreement's conditions
are met here:

- the complete Agreement is at [`docs/vendored/llama2/LICENSE`](docs/vendored/llama2/LICENSE),
  unmodified;
- the required attribution notice is at [`docs/vendored/llama2/NOTICE`](docs/vendored/llama2/NOTICE),
  reproduced verbatim below;
- the Acceptable Use Policy is at [`docs/vendored/llama2/USE_POLICY.txt`](docs/vendored/llama2/USE_POLICY.txt),
  renamed from its upstream `.md` only so that `markdownlint` does not lint a third
  party's document to this repository's house style.

```text
Llama 2 is licensed under the LLAMA 2 Community License, Copyright © Meta Platforms, Inc. All Rights Reserved.
```

The file is compiled into no package: it lives under `tests/`, is copied to the test
output, and exists so `SentencePieceBpeLineageOracleTests` checks the normalizer
spelling of the whitespace escape against a file a user actually has. It is downloaded
verbatim by `tools/fetch_llama2_mistral_tokenizers.py`, which pins its upstream
SHA-256 and corroborates it against `daryl149/llama-2-7b-chat-hf` under
[`docs/decisions/0017-bpe-parity-scope.md`](docs/decisions/0017-bpe-parity-scope.md)
§5's two-mirror method — the original, `meta-llama/Llama-2-7b-hf`, is gated and answers
HTTP 401. `TheBloke/Llama-2-7B-fp16` is the source rather than the other mirror because
it ships the Agreement, the notice and the policy beside the artifact, so the license
travels from the same place as the file. That same tool pins the three license files
too, so an upstream change to the Agreement fails the fetch rather than drifting from
what is committed here.

`mistralai/Mistral-7B-v0.1` is published under the Apache License 2.0 by Mistral
AI, as declared on its model card. The full license text is at
<https://www.apache.org/licenses/LICENSE-2.0>; its redistribution conditions are
met here by this notice, which states the component, its license and its source.

## Build-time dependencies (not shipped)

| Component | License | Usage |
| --- | --- | --- |
| PolySharp | MIT | Compile-time polyfills for the `netstandard2.0` target. `PrivateAssets="all"`, so it emits source and adds no package dependency. |

## Test and benchmark dependencies (not shipped)

| Component | License | Usage |
| --- | --- | --- |
| xUnit | Apache-2.0 | Test framework (`tests/`) |
| coverlet.collector | MIT | Code coverage collection (`tests/`) |
| BenchmarkDotNet | MIT | Micro-benchmarks (`bench/`) |

## Development-only oracle generation (not distributed)

These libraries are executed by `tools/generate_oracles.py` to produce the
reference JSON committed under `tests/oracles/`. They are **not** dependencies of
the shipped libraries, and are **not** transcribed — only their observable
input/output behavior is reproduced (permitted per §10.2). Running a program to
generate test data creates no license claim over the output.

| Component | License | Usage |
| --- | --- | --- |
| rapidfuzz | MIT | Reference values for edit-distance / fuzzy metrics |
| jellyfish | MIT | Reference values for phonetic / Jaro-family metrics |
| doublemetaphone | Artistic-2.0 | Reference values for Double Metaphone, which `jellyfish` 1.2.1 does not export |
| textdistance | MIT | Reference values for set/token similarity metrics |
| scikit-learn | BSD-3-Clause | Reference values for the vectorizers |
| nltk | Apache-2.0 | Reference values for Porter and every Snowball stemmer but Hungarian |
| snowballstemmer | BSD-3-Clause | Reference values for the Hungarian stemmer, which `nltk` 3.10.1 stems incorrectly (decision 0091) |
| tokenizers | Apache-2.0 | Reference values for WordPiece |
| sentencepiece | Apache-2.0 | Reference values for the unigram tokenizer |
| numpy | BSD-3-Clause | Reference values for pooling and kNN |
| lifelines | MIT | Reference values for Kaplan-Meier, Nelson-Aalen and the log-rank test |
| rank_bm25 | Apache-2.0 | Reference values for BM25 Okapi, including its floor on a negative IDF |

> The Apache-2.0 above covers the `nltk` *code* we execute. It does not extend to
> the `nltk_data` corpora, which are licensed individually — the `stopwords`
> corpus among them has no stated licence, which is why the shipped lists come
> from Snowball instead. See
> [`docs/decisions/0010-stop-word-list-provenance.md`](docs/decisions/0010-stop-word-list-provenance.md).
> `python-Levenshtein` (GPL) is deliberately **not** used, as a matter of both
> transcription hygiene and generated-data hygiene. See
> [`docs/decisions/0003-provenance-and-licensing.md`](docs/decisions/0003-provenance-and-licensing.md).
