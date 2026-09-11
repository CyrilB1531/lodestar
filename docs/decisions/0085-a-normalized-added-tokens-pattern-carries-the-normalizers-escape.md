---
status: accepted
supersedes: []
amends: ["0062"]
applies: []
---
# 0085 — A normalized added token's pattern carries the normalizer's escape, not the pre-tokenizer's

**Status:** accepted · **Date:** 2026-09-08 · **Amends:** [`0062`](0062-the-two-metaspace-spellings-part-on-the-prepend-twice.md)

## Context

[0062](0062-the-two-metaspace-spellings-part-on-the-prepend-twice.md) bounded
[0050 §2](0050-the-sentencepiece-bpe-lineage-stays-a-bpe-model.md)'s "two writings of one value"
with two fields, after measuring two places where the `Metaspace` block and the
`Prepend` + `Replace` normalizer sequence part: the prepend's guard, and whether the scheme is
counted per piece. Its title says *twice*, and both of those are about the **text**.

A third place was never measured: the **pattern** an added token is matched by.
[0022 §3](0022-added-token-matching-flags.md) says which text an entry is looked for in —
`normalized: false` runs over the raw input, `normalized: true` has its own content normalized and
runs over the normalized text — and `BpeTokenizer` keeps the two scanners that requires. What it
does not settle is what "normalized" means for the content when the file's whitespace escape is
one of two spellings, because 0050 §2 had made them one transform.

[#551](https://github.com/CyrilB1531/lodestar/issues/551) is where that shows. It was filed on one
symptom — a special token as the whole text encoding three ways — and the measurement below found
a second, worse one that the issue does not name.

## What was measured

`tokenizers` 0.23.1, `add_special_tokens=False`, against the two vendored fixtures
`tests/oracles/llama2_tokenizer.json` and `tests/oracles/mistral_v01_tokenizer.json`, compared to
`BpeTokenizer` on the same files. **Mistral v0.1 agrees on all eight texts. Llama-2 diverges on
all eight.**

| text | reference (Llama-2) | Lodestar (Llama-2) |
| --- | --- | --- |
| `<s>` | `['▁<s>']` — `[1]` | `['▁', '<s>']` — `[29871, 1]` |
| `</s>` | `['▁</s>']` — `[2]` | `['▁', '</s>']` — `[29871, 2]` |
| `<unk>` | `['▁<unk>']` — `[0]` | `['▁', '<unk>']` — `[29871, 0]` |
| `<s>the cat` | `[1, 1552, 6635]` | `[29871, 1, 1552, 6635]` |
| `<s> the cat` | `[1, 278, 6635]` | `[29871, 1, 278, 6635]` |
| `" <s>"` | `['▁', '▁<s>']` — `[29871, 1]` | `['▁▁', '<s>']` — `[259, 1]` |
| `the cat<s>` | `['<', 's', '>']` — `[…, 29966, 29879, 29958]` | `['<s>']` — `[…, 1]` |
| `the cat<s>the cat` | `<s>` **not matched** | `<s>` matched, id `1` |

The two files declare the same three special tokens and differ in one field each:

```text
llama2       normalizer    = Sequence[Prepend "▁", Replace " " -> "▁"]   added_tokens: normalized: true
mistral_v01  pre_tokenizer = Metaspace{prepend_scheme: "first", split: false}   added_tokens: normalized: false
```

`tokenizers` normalizes a `normalized: true` entry's **content** with the declared normalizer
before matching it. Llama-2's effective pattern is therefore `▁<s>`, not `<s>`, and every row
above follows from that one fact: it matches where a `▁` precedes — at the start of a text, or
after the escape of a space — and **does not match after a letter**, where the BPE model then
spells `<`, `s`, `>` from the model vocabulary.

`BpeTokenizer` builds that scanner's patterns with `Normalize` — the declared Unicode forms —
while the text it searches goes through `Preprocess`, which is `Normalize` **plus** the metaspace
escape. The escape reaches the haystack and not the needle. The remark on `Preprocess` states the
reasoning as deliberate:

> Added-token content goes through `Normalize` alone — escaping it would spell the entry with a
> symbol the file did not put there.

That is **right for Mistral and wrong for Llama-2**, and the difference is which section of the
file declared the escape. A `Metaspace` block is a pre-tokenizer, and `tokenizers` does not run a
pre-tokenizer over an added token's content; a `Prepend` + `Replace` sequence is a normalizer, and
it does. 0050 §2 folded both into one `MetaspaceEscape` and lost the distinction that decides it.

**Two failure modes, and the second is not in the issue.** The extra `▁` token is the one #551
reports: one token too many at the head of every prompt on this lineage. The other is
`the cat<s>` — Lodestar answers the BOS id where the reference answers three ordinary characters,
so **user text containing `<s>` silently becomes a control token**. That is the class
[0017 §3](0017-bpe-parity-scope.md) and [0050 §4](0050-the-sentencepiece-bpe-lineage-stays-a-bpe-model.md)
refuse by name — *refusing beats producing embeddings that are quietly wrong* — reached here
without a refusal anywhere.

## Decision

**1. The escape follows the pattern when the file declared it as a normalizer, and not when it
declared it as a pre-tokenizer.** `BpeVocabulary` carries that provenance, and `BpeTokenizer`
builds the normalized scanner's patterns with `Preprocess` in the first case and `Normalize` in
the second. Mistral is untouched: its entries are `normalized: false` and never reach that
scanner. Llama-2's pattern becomes `▁<s>`, which is what the reference matches on.

The alternative — escaping every normalized entry's content — is the cheaper diff and is wrong on
the Mistral side, where it would spell an entry with a symbol the pre-tokenizer never applies to
it. Provenance is what the reference itself keys on.

**2. The `Preprocess` remark is corrected rather than deleted.** It describes the pre-tokenizer
spelling accurately, and that spelling is still served by `Normalize` alone. What it lacked was
the other half.

**3. A `Metaspace` pre-tokenizer with a `normalized: true` added token is refused at load, by
name.** Under that combination the pattern would depend on its own position — `prepend_scheme:
first` prepends to the opening piece, and the guard reads what precedes — which a static scanner
pattern cannot express. Neither reference file carries the shape, and 0050 §4 prefers a refusal to
a token stream that differs. Approximating it with one of the two readings would be the quietly
wrong answer this ADR exists to remove.

## What was rejected

**Recording the divergence in `docs/equivalence.md` and changing nothing.** #551 is explicit that
neither answer had been argued, and that is true of the extra `▁`. It is not true of `the cat<s>`:
a caller's text turning into a control token is a defect, not a divergence, and documenting it
would spend [0036](0036-a-member-may-ship-without-an-oracle-if-it-says-so.md)'s exception — for
behaviour no reference covers — on behaviour a reference covers exactly.

**Refusing both files at load until the pattern is right.** [#318](https://github.com/CyrilB1531/lodestar/issues/318)
shipped "Llama-2 and Mistral v0.1 both load" in `Lodestar.Embeddings` 0.6.0 five days before this
was written. Revoking a published capability for a defect that is reparable in the loader is not
what 0050 §4 asks for; it asks for a refusal where the answer would otherwise be wrong, and here
the answer can be made right.

## Consequences

The `"<s>"` row that [#318](https://github.com/CyrilB1531/lodestar/issues/318) took out of
`generate_llama2_mistral` rather than freeze — *"filed on its own rather than frozen here as
though it were settled"* — goes back in, on both models, together with the mid-text rows that
carry the second failure mode. The generator's docstring loses the paragraph that points here.

`docs/equivalence.md` gains the rule on its `add_tokens` row: which text an entry is matched
against comes from `normalized` (0022 §3), and what its content is normalized *with* comes from
whether the file spelled the escape as a normalizer or as a pre-tokenizer.

**Llama-2 ids change.** Any embedding produced through this lineage by `Lodestar.Embeddings` 0.6.0
carries the extra `▁` token, and a caller who stored them must regenerate. The entry goes under
`#### Fixed`, and the change ships in 0.7.0 rather than as a patch, because the vectors move.

0062's title stays as it is. It counted the two partings it measured, and this is a third one it
did not reach; the body records the relationship, which is the convention
[0022 §10](0022-added-token-matching-flags.md) sets for a later decision rather than rewriting an
earlier one.
