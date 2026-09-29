# The Embeddings findings of the Review B after #1380

**Issues:** [#1387](https://github.com/CyrilB1531/lodestar/issues/1387) to
[#1391](https://github.com/CyrilB1531/lodestar/issues/1391),
[#1393](https://github.com/CyrilB1531/lodestar/issues/1393).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The post-fix Review B of what #1380 shipped, on `main` at `bf5cf39b`, found the constructor checks
it added missing a null split pattern and an undefined normalization form; `SentencePieceTokenizer`
emitting a matchable piece's own id where it emits a non-matchable piece's position; the
vocabulary records hashing an absent list but throwing from `ToString`; `CharTrie` growing in
`int`; and `BpeTokenizer`'s documentation missing two refusals and citing records #1103 deleted.

## Decisions

- **A piece's id must be its position**: both loaders already number pieces that way, and a hand
  built vocabulary that does not is refused rather than read two ways.
- **An absent list counts as empty in the printed properties**, `Count` and `SpecialTokenCount`,
  where the hash counts it `-1`: a count is read by people, the hash only has to be consistent.
- **Citations of deleted records are removed, not repointed**: the Metaspace refusal points at
  `docs/equivalence.md`'s `tokenizer.add_tokens` row, which carries it today, and the five comments
  that cited 0023, 0034, 0035 and 0050 keep their reasoning in their own words, citing nothing — the
  maintainer's review of this pull request, a deleted record being no reference.

## Verification

- `ReviewBFindingsTests` gains three facts; `BpeMetaspaceLoaderTests` checks the new citation.
- Review A found four more stale citations in the package, and `CharTrie` reading past its array
  when a capped growth still fell short of a base's children; both fixed here. A first push
  repointed those citations at `53af23c2`; the maintainer's review removed them.
