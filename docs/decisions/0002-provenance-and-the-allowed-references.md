---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0002 — Provenance and the allowed references

**Status:** accepted · **Date:** 2026-09-20

## Context

This repository is published on GitHub and on nuget.org, which is distribution under
every licence involved, and the obligations have applied since the first commit. Five
records settled the question one occasion at a time: `0003` set the project's own
licence and the allowed-source rule; `0010` chose Snowball over the `nltk` corpus for
the stop-word lists; `0082` added scipy to the named references; `0084` vendored the
Llama-2 tokenizer under a licence that is not permissive; `0099` refused
`scikit-survival` on its licence rather than on its quality. They are merged here, with
each library's licence re-read from the wheel installed against `tools/requirements.lock.txt`
rather than carried over from the record that named it.

Three things the pins say that the merged records do not, found while re-reading them:

- `tools/requirements.txt` cites **decision 0098** for the `scikit-survival` refusal and
  **decision 0090** for the `snowballstemmer` measurement. The records are **0099** and
  **0091**; 0098 is the normal quantile and 0090 is Finnish. The pins are right, the
  citations are off by one.
- `THIRD-PARTY-NOTICES.md`'s development-only table names fourteen libraries and misses
  six that are pinned and read today: **scipy, mapie, statsmodels, rake-nltk, keybert,
  summa**. scipy is the conspicuous one, since 0082 exists to admit it.
- numpy's wheel declares `BSD-3-Clause AND 0BSD AND MIT AND Zlib AND CC0-1.0`, for the
  components it vendors; the notices file records BSD-3-Clause alone. Every term in that
  expression is permissive, so the rule below is satisfied either way.

## Decision

**Lodestar is Apache-2.0, and a reference library may be read for its behaviour only —
its inputs, its outputs, and where one of its branches cedes to the next — and only when
its own licence is permissive; a copyleft reference is read for nothing, and no reference
of any licence is transcribed.**

Sources rank, in order of preference: published papers and pseudo-code, which are not
protectable; textbooks and documentation; then a permissively licensed implementation as
a behaviour reference. What crosses over is the observable behaviour and analogous
naming, never the source. Running such a library to generate test data creates no claim
over the output, and the corpora under `tests/oracles/` are that output.

### The allowed references

Read from each installed wheel's `METADATA`, not from PyPI's field — `0075` found that
field unreliable and `0099` re-found it. Versions are the pins in
`tools/requirements.txt`, `tools/requirements-nodeps.txt` and the resolved lock.

| library | pinned | licence | admitted by |
| --- | --- | --- | --- |
| rapidfuzz | 3.14.6 | MIT | `0003` |
| jellyfish | 1.2.1 | MIT | `0003` |
| textdistance | 4.6.3 | MIT | `0003` |
| scikit-learn | 1.9.0 | BSD-3-Clause | `0003` |
| scipy | 1.18.1 (lock) | BSD-3-Clause | `0082`, for the Kolmogorov dispatch table |
| doublemetaphone | 1.2 | Artistic-2.0 | `0075` |
| nltk | 3.10.3 | Apache-2.0 — **the code only**, never the corpora | `0008`; bounded by `0010` |
| snowballstemmer | 3.1.1 | BSD-3-Clause | `0091` |
| tokenizers | 0.23.2 | Apache-2.0 | `0017` |
| sentencepiece | 0.2.2 | Apache-2.0 | `0013` |
| numpy | 2.5.3 | BSD-3-Clause and four other permissive terms | `0003`'s rule |
| mapie | 1.5.0 | BSD-3-Clause | `0070`, applied again by `0143` |
| statsmodels | 0.15.0 | BSD-3-Clause | `0096` |
| lifelines | 0.30.3 | MIT | `0099` |
| rank-bm25 | 0.2.2 | Apache-2.0 | `0003`'s rule (#573) |
| datasketch | 2.0.0 | MIT | `0108` |
| simhash | 2.1.2 | MIT | `0003`'s rule (#602) |
| rake-nltk | 1.0.6 | MIT | `0077` |
| keybert | 0.9.0, `--no-deps` | MIT | `0078` |
| summa | 1.2.0, `--no-deps` | MIT | `0077` |

`difflib` is read too, and is not pinned: it is CPython's standard library, under the PSF
licence, and arrives with the interpreter. Every library above is a **development**
dependency — none is referenced by a shipped package, and none is redistributed.

### The refusals

| refused | licence | record |
| --- | --- | --- |
| `python-Levenshtein` | GPL | `0003` — not transcribed, and not even run to generate oracles, as a matter of hygiene |
| `abydos` | GPL | `0003`'s rule, applied |
| `scikit-survival` | GPL-3.0-or-later, from its PyPI `license_expression` | `0099` — the reason on file is *licence*, not unavailability |

`scikit-survival` is the nearest survival reference in any language, and NuGet had none
at all on 2026-09-09. It was still refused, and `lifelines` (MIT) took the oracle.

### The two exceptions, each limited to its artifact

**The Snowball stop-word lists (`0010`).** BSD-3-Clause, five languages — French, German,
Italian, Portuguese, Spanish — pinned by SHA-256 in `tools/fetch_stopwords.py` and
compiled into `Lodestar.Text`. They are the exception to *read but do not redistribute*:
they are the one resource the packages ship rather than merely consult, so the copyright
notice and the licence travel with them in `NOTICE` and `THIRD-PARTY-NOTICES.md`. The
exception covers those five files and nothing else. `StopWords.English` stays
scikit-learn's 318-word list, for the `stop_words="english"` parity it promises, and the
`nltk` stop-word corpus is not vendored in whole or in part — `nltk_data` classifies it
as unclarified, and the Apache-2.0 on `nltk`'s code does not reach it.

**The Llama-2 `tokenizer.json` (`0084`).** The LLAMA 2 COMMUNITY LICENSE AGREEMENT is not
an OSI licence and not permissive. The exception is **this artifact alone** and does not
relax the list above: the Agreement, its notice and its acceptable-use policy are vendored
complete under `docs/vendored/llama2/`, the file is pinned by SHA-256 and corroborated
against a second mirror by `tools/fetch_llama2_mistral_tokenizers.py`, and what is
committed is the vocabulary, the merge table and the pipeline flags. **Every future
non-permissive source needs its own record**; this one grants nothing by analogy.

### The two hard rules

- **Never transcribe GPL-licensed code.** Implement from the published algorithm
  description. Reading a reference implementation to diagnose one failing case is
  diagnosis; deriving the implementation from it is not, whatever the language. This is
  why the stemmers and the phonetic encoders are original implementations, and the oracle
  is what proves the behaviour matches without the source needing to.
- **Never commit model weights.** Test fixtures are small and synthetic; a vocabulary is
  fetched against a pinned SHA-256 by `tools/fetch_gpt2_bpe.py`,
  `tools/fetch_xlmr_vocab.py`, `tools/fetch_llama2_mistral_tokenizers.py` and
  `tools/fetch_stopwords.py`, each of which fails loudly when the upstream bytes move
  rather than producing a different library in silence.

## Consequences

- Adding a reference means reading its licence out of the wheel and its transitive
  graph the same way — `0099`'s table is the worked example, where two of `lifelines`'
  dependencies report `License: UNKNOWN` in legacy metadata and carry MIT in the wheel.
  A blank field is not a licence and is not a refusal either.
- A new reference lands in `tools/requirements.txt` (or `tools/requirements-nodeps.txt`,
  under `0078`'s closure test) and in `THIRD-PARTY-NOTICES.md` in the same commit. The
  six omissions named in the Context are a debt against that rule, not a relaxation of it.
- A source that is not permissive is refused unless a record argues it, artifact by
  artifact, and records the conditions its licence attaches and how each is met.
- A branch-selection question settled by reading a permissive reference's own dispatch —
  rather than a formula derived independently — is behaviour, and this record is what it
  cites; `Internal/Kolmogorov.cs`'s long comment remains the line-numbered account of
  what was read from scipy.
- Shipped resources are attributed where they ship; development references are
  attributed for traceability and are redistributed nowhere.
