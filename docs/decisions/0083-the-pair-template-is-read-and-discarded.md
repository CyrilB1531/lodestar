# 0083 — A TemplateProcessing's `pair` template is read and discarded

**Status:** accepted · **Date:** 2026-09-08

## Context

[#548](https://github.com/CyrilB1531/lodestar/issues/548): [`LoadBpe`](../reference/embeddings/persistence/tokenizerjsonloader-loadbpe.md) refused every
`tokenizer.json` carrying a `post_processor`, and that refusal — not `Metaspace`, not
`byte_fallback`, both of which shipped in [#316](https://github.com/CyrilB1531/lodestar/issues/316)
and [#317](https://github.com/CyrilB1531/lodestar/issues/317) — is what kept Llama-2 and
Mistral v0.1 from loading. Measured on `net10.0` before any code was written:

```text
Llama-2 (daryl149/llama-2-7b-chat-hf) -> REFUSED  InvalidDataException
Llama-2 (TheBloke/Llama-2-7B-fp16)    -> REFUSED  InvalidDataException
Mistral (mistralai/Mistral-7B-v0.1)   -> REFUSED  InvalidDataException

  "…declares a 'post_processor' section: Lodestar tokenizers do not insert special
   tokens such as [CLS] and [SEP]."
```

Reading the section means deciding what to do with both templates it carries. `single` is
uncontroversial — all three files write the same thing:

```json
"single": [ {"SpecialToken": {"id": "<s>", "type_id": 0}},
            {"Sequence":     {"id": "A",   "type_id": 0}} ]
```

One prefix token, no suffix, which is exactly what [`SpecialTokenTemplate`](../reference/embeddings/tokenization/specialtokentemplate.md) already models.

`pair` is not uncontroversial. **Two mirrors of the same model disagree on it:**

```text
daryl149/llama-2-7b-chat-hf : [<s>, A, B]
TheBloke/Llama-2-7B-fp16    : [<s>, A, <s>(type_id 1), B(type_id 1)]
mistralai/Mistral-7B-v0.1   : same as TheBloke
```

Those two Llama-2 files agree byte for byte on `model.vocab` (32 000 entries), `model.merges`
(61 249), `normalizer`, `pre_tokenizer`, `decoder`, the model flags and `single`. They part on
`pair` alone. [Decision 0017](0017-bpe-parity-scope.md) §5's two-mirror method exists to catch
exactly this, and it caught it: `pair` is a property of whichever export tool wrote the file, not
of Llama-2.

## Decision

**`single` is read into [`BpeVocabulary`](../reference/embeddings/tokenization/bpevocabulary.md)'s `PrefixTokens` and `SuffixTokens`.
`pair` is parsed for its shape and then discarded.** `type_id` is discarded with it.

Three things follow, and each is a refusal by name rather than a silent acceptance:

1. A post-processor whose `type` is not `TemplateProcessing` — `ByteLevel`,
   `RobertaProcessing`, a `Sequence` of them — is refused naming what it found.
2. A `single` template that names no sequence, names more than one, names `B` rather than
   `A`, or holds a step that is neither a `SpecialToken` nor a `Sequence`, is refused naming
   which of those it is.
3. [`LoadWordPiece`](../reference/embeddings/persistence/tokenizerjsonloader-loadwordpiece.md) and [`LoadUnigram`](../reference/embeddings/persistence/tokenizerjsonloader-loadunigram.md) keep refusing any post-processor at all. This lot exists
   for the SentencePiece-BPE lineage; widening the other two paths would accept files nothing
   has measured.

**The pad token is not read.** `SpecialTokenTemplate` needs one and a `post_processor` does not
carry one — that lives in `enable_padding`, a section that stays refused. So the file's
declaration reaches a caller as
`new SpecialTokenTemplate(vocabulary.PrefixTokens, vocabulary.SuffixTokens, padToken)`, with the
one value the file does not state supplied by whoever knows it. Inventing a pad token here would
be the guess this repository's provenance rule ([0003](0003-provenance-and-licensing.md)) exists
to prevent, and the candidates differ per model — Llama-2 is served `<unk>` by some wrappers and
`</s>` by others.

## Options considered

**Refuse a `pair` that is not `single` with `B` appended.** More consistent with this loader's
habits, which refuse every unreproduced section by name. Rejected on a narrow, decisive fact:
under that rule **all three target files are refused** — `[<s>, A, B]` and `[<s>, A, <s>, B]`
both differ from `single + B`. A rule that rejects the files the work exists to load is the
wrong rule.

**Reproduce `pair`.** Requires choosing between two mirrors that disagree, which would make an
arbitrary choice look measured. It also has no caller: [`BpeTokenizer.Encode`](../reference/embeddings/tokenization/bpetokenizer-encode.md) takes one string,
and nothing in the public surface encodes a pair.

**Accept the section and ignore it entirely, behind an `ArtifactLoadOptions` opt-in.** Rejected
outright. Accepting a section and not reproducing it is the silent divergence this loader refuses
everywhere else, and the refusal message names the consequence — embeddings that do not match the
model. Reading `single` is what makes this decision *not* that.

## Consequences

- Llama-2 and Mistral v0.1 load. Measured after the change, on the same three files:

  ```text
  PrefixTokens=[<s>]  SuffixTokens=[]   ByteFallback=True  FuseUnk=True  NoPreTokenizer=True
  "the cat" -> [▁the ▁cat]                          the Metaspace escape ran
  "🦙"      -> [▁ <0xF0> <0x9F> <0xA6> <0x99>]      byte_fallback ran, and Decode round-trips
  ```

- Discarding `pair` is a stated non-reproduction of a section describing an operation the public
  surface does not offer — which is a different thing from ignoring a section that changes an
  operation it does offer. **If a pair-encoding API is ever added, this decision is the one to
  revisit**, and `pair` becomes live scope at that moment.

- `SpecialTokenTemplate.None`'s remark no longer describes the BPE loader; it is corrected in the
  same commit. [`EncodingOptions`](../reference/embeddings/tokenization/encodingoptions.md)'s `Template` still defaults to `SpecialTokenTemplate.Bert`, which is
  wrong for this lineage — a Llama-2 vocabulary has no `[CLS]`, `[SEP]` or `[PAD]`, so a caller
  who batches one without setting `Template` fails loudly at encode time. Changing that default
  is a behaviour change for existing callers and is left to its own lot.

- `docs/equivalence.md`'s `LoadBpe` row moves `post_processor` out of its **Refuses** list and
  states what is read, what is discarded and why.
