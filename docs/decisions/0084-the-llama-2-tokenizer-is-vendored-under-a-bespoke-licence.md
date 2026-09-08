# 0084 — The Llama-2 tokenizer is vendored under a bespoke licence, as a named exception

**Status:** accepted · **Date:** 2026-09-08

## Context

[#318](https://github.com/CyrilB1531/lodestar/issues/318) is the lot that makes
[#175](https://github.com/CyrilB1531/lodestar/issues/175)'s opening sentence false —
*"Llama-2 and Mistral v0.1 are files a user actually has, and neither tokenizer here
loads them"*. Both load since [#548](https://github.com/CyrilB1531/lodestar/issues/548),
and the corpus that proves it needs the files themselves, because a synthetic model
would not exercise a 32 000-entry vocabulary over a merge table of 61 249 ranks
(Llama-2) or 58 980 (Mistral).

Mistral v0.1 is Apache-2.0 and raises nothing. Llama-2 does:

```text
meta-llama/Llama-2-7b-hf        license:llama2        the gated original (HTTP 401)
TheBloke/Llama-2-7B-fp16        (none declared)       ungated mirror
daryl149/llama-2-7b-chat-hf     (none declared)       ungated mirror
```

[Decision 0003](0003-provenance-and-licensing.md) names its allowed sources as
permissive ones — rapidfuzz (MIT), jellyfish (MIT), textdistance (MIT), scikit-learn
(BSD-3) — and records that model **weights** are not redistributed. It does not rule on
a *vocabulary* under a bespoke licence, because the question had not come up. The two
model artifacts `THIRD-PARTY-NOTICES.md` carries today, `gpt2` and `xlm-roberta-base`,
are both MIT.

The LLAMA 2 COMMUNITY LICENSE AGREEMENT (version release date 2023-07-18) is not an OSI
licence. It grants a worldwide, non-exclusive, royalty-free right to use, reproduce,
distribute and modify the Llama Materials, subject to conditions this decision has to
meet rather than assume.

[Decision 0017](0017-bpe-parity-scope.md) §5 does not settle it either, and the gap is
worth naming: its two-mirror method answers **verification** — what does the file say —
and is silent on the **right to redistribute** it. Reading a regex off two agreeing
mirrors to justify a public constant is a different act from committing 1.8 MB of that
model's vocabulary into a public repository.

## Decision

**The Llama-2 `tokenizer.json` is vendored, as a named exception to 0003's allowed-source
list.** The exception is limited to this artifact and does not relax the list.

**Every future non-permissive source needs its own decision.** This one grants nothing by
analogy: a reader auditing a fixture should find the licence argued for that fixture, not
inferred from a precedent.

The conditions the licence attaches, and how each is met:

| condition | how it is met |
| --- | --- |
| Retain the attribution notice in redistributed Materials | `docs/vendored/llama2/NOTICE`, reproduced verbatim, and a block in `THIRD-PARTY-NOTICES.md` |
| Provide a copy of the Agreement | `docs/vendored/llama2/LICENSE`, complete and unmodified |
| Comply with the Acceptable Use Policy | `docs/vendored/llama2/USE_POLICY.txt`, vendored alongside |
| The >700 M monthly-active-user clause | Not triggered; recorded here so a future reader knows it was read |
| No use to improve any other large language model | Nothing here trains anything: the file is a tokenizer fixture replayed by tests |

**Scope is the tokenizer only.** No weights, consistent with 0003's existing rule and with
`CLAUDE.md`'s hard rule. What is vendored is the vocabulary, the merge table and the
pipeline flags — the file a caller loads, not the model it belongs to.

**The source is `TheBloke/Llama-2-7B-fp16`**, corroborated by
`daryl149/llama-2-7b-chat-hf` under 0017 §5's two-mirror method. TheBloke is the primary
because it ships `LICENSE`, `Notice` and `USE_POLICY.md` beside the artifact, so the
licence travels from the same place as the file rather than from a third party. Its
`LICENSE` is byte-identical to `NousResearch/Llama-2-7b-hf`'s — the same two-source
discipline applied to the licence text itself.

`NousResearch/Llama-2-7b-hf` is **not** a mirror for the artifact, though 0017 §5 uses it
for Llama-3 where it is correct. For Llama-2 its merge table holds the same 61 249 pairs
as a multiset but orders 119 of them differently, from index 61 129 onward and all in the
whitespace runs — and merge order is rank in BPE, so runs of spaces would segment
differently.

## Options considered

**Keep Llama-2 fetch-and-verify, vendoring nothing.** The reversible option, and the
one #318 carried in the interim: `tools/fetch_llama2_mistral_tokenizers.py` downloads
both mirrors, checks their agreement, and writes nothing. It costs the corpus its
discriminating row — U+1F600 is a whole token to Mistral and four byte pieces to
Llama-2, the pair that proves the two halves are not measuring the same thing twice —
and leaves #175's sentence half true. Rejected in favour of the exception, deliberately
and with the trade-off read.

**Vendor a reduced fixture holding only the entries the corpus touches.** Rejected: a
subset of a vocabulary is still a redistribution of it, and dropping merges changes the
ranks, so the fixture would no longer be the model it claims to be.

## Consequences

- `tests/oracles/llama2_tokenizer.json` is committed, 1.8 MB, pinned by SHA-256 in
  `tools/fetch_llama2_mistral_tokenizers.py` and corroborated against a second mirror on
  every run of it.
- `docs/vendored/llama2/` holds `LICENSE`, `NOTICE` and `USE_POLICY.txt`. The last is
  renamed from its upstream `.md` so that `markdownlint`, which globs `docs/**/*.md`,
  does not lint a third party's document to this repository's house style.
- `THIRD-PARTY-NOTICES.md` gains a Llama-2 block naming the component, the licence, the
  source and the copyright holder.
- **0003's allowed-source list is unchanged.** This decision sits beside it as an
  exception, in the shape [decision 0082](0082-scipy-joins-the-allowed-permissive-references.md)
  used to extend that list — an extension is recorded as its own decision rather than as
  an edit, per the immutability rule `tools/check_adr_immutable.py` enforces.
- #175 closes with #318: both files now load, encode and decode at parity with
  `tokenizers` 0.23.1, on the real files.
