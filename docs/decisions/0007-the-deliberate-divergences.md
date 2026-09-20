---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0007 — The deliberate divergences

**Status:** accepted · **Date:** 2026-09-20

## Context

Every algorithm here is measured against a Python reference and frozen into
`tests/oracles/`, so parity is the default and a difference is an event. Nine records asked
the same question one member at a time: *this member does not answer what the reference
answers — is that allowed?* They answered it nine times, and a reader who wanted the rule
read nine records and assembled it.

The nine cases fall into three shapes, and the shape is what decides them.

**The reference is wrong in a way it does not state.** `jellyfish` 1.2.1 diverges from the
published Hamming and Jaro definitions on 62 of 1241 corpus pairs, always on combining
marks, emoji or mixed scripts, and the cause is inside a Rust core nobody here can read
(`0005`). The same library measures a Match Rating codex by its UTF-8 **byte** length in
three places — the six-character truncation, the length-gap check and the minimum-rating
lookup — so `match_rating_codex("並丝七世")` returns a six-character codex from a
four-character word, longer than the word it truncated (`0080`). `scikit-learn` 1.9.0 raises
`ValueError: math domain error` on `mutual_info_score([], [])`, from an unguarded `log(0)`
eight frames down, with no parameter guard and no docstring note (`0039`).

**The reference's behaviour is unseeded, or it degrades in silence.** `sklearn.cluster.KMeans`
was observed breaking an exact distance tie toward the second centre in one configuration and
the first in another, inside compiled Cython over a BLAS product (`0093`). `tokenizers` 0.23.1
accepts a vocabulary that declares `byte_fallback` without all 256 `<0xXX>` pieces and
degrades per symbol to the unknown token — or, with no `unk_token`, drops the symbol and lets
its neighbours merge across the hole (`0063`). `difflib.SequenceMatcher` applies an
`autojunk` heuristic past 200 elements that is an optimisation of `difflib`, not a property
of the Ratcliff-Obershelp metric (`0006`).

**The reference refuses, or answers, where the answer is the caller's guarantee.** MAPIE 1.5.0
raises when `alpha < 1/(n + 1)`, and under `allow_infinite_bounds=True` returns a *finite*
interval whose half-width is the largest calibration score — narrower than the level asked
for, in exactly the regime where nobody would notice (`0070`). scipy answers `(nan, nan)` with
a warning on a sample its own guard finds degenerate, which under `nan_policy='omit'` would
make one input raise or not according to how it arrived (`0117`).

`0023` is in this family for the opposite reason, and is kept in the table below because of
it: [`BpeTokenizer.Decode`](../reference/embeddings/tokenization/bpetokenizer-decode.md) used to throw on a byte sequence that is not well-formed UTF-8,
where `tokenizers` substitutes U+FFFD. Nothing defended the strict decode — it made one-id-at-a-time
decoding unusable for any text outside Latin-1 — so the code was aligned with the reference
and the divergence was withdrawn. That is the axis running the other way.

## Decision

**A divergence from the reference is accepted only when reproducing it would mean reproducing
a defect, an unseeded behaviour or a refusal the reference itself does not defend — otherwise
the reference wins and the code is aligned with it.**

An accidental byte/character mixup, a tie broken inside a compiled kernel, a silent
degradation into a wrong token stream and a logarithm of zero are all of that kind. A rule
table, a default, a parameter's presence, an exception type and a lossy decode are not: they
are what a caller migrating a script depends on, and they are reproduced even where they look
wrong read on their own.

**Every accepted divergence carries a row in `docs/equivalence.md`, written in the same commit
as the member.** The row states what the reference does and what this does, so a reader meets
the divergence in the mapping table rather than in a stack trace. Where the corpus cannot hold
the case — because the reference produces no value for it — a named test pins it instead, and
that test names this record.

| member | what the reference does | what this does | why the divergence is accepted | record |
| --- | --- | --- | --- | --- |
| [`Hamming.Distance`](../reference/text/distances/hamming-distance.md), [`Jaro.Similarity`](../reference/text/distances/jaro-similarity.md), [`JaroWinkler.Similarity`](../reference/text/distances/jarowinkler-similarity.md) | `jellyfish` 1.2.1 departs from the published definition on 62 of 1241 pairs, all carrying combining marks, emoji or mixed scripts; it is neither a byte comparison nor an NFC effect | the published definition, over `TextElement.Utf16Unit` or `TextElement.CodePoint`; the oracle is generated from an explicit reference of that definition and records the jellyfish divergence count | an unspecified quirk of a Rust core, reproducible only by copying it; parity holds on every input where jellyfish computes a standard result, pinned by the classic name pairs | `0005` |
| [`RatcliffObershelp.Similarity`](../reference/text/distances/ratcliffobershelp-similarity.md) | `difflib.SequenceMatcher` treats an element occupying more than 1% of positions as junk past 200 elements, changing `ratio()` | `2·M/T` over the recursive pairing, discarding nothing; the oracle is frozen with `autojunk=False` | `autojunk` is an optimisation of `difflib`, not a property of the metric; identical to the default below 200 elements and to `autojunk=False` at any length | `0006` |
| [`MutualInformation.Score`](../reference/metrics/clustering/mutualinformation-score.md) | `mutual_info_score([], [])` raises `ValueError: math domain error` from `log(pi.sum())`, with no guard and no note | returns `0.0`, as every sibling agreement metric answers on an empty input | an unguarded logarithm surfacing, not a designed refusal — it was found by a generator crashing; `mutual_info_score` is `0.0` on every one-class input, and empty is that shape's boundary | `0039` |
| [`TokenizerJsonLoader.LoadBpe`](../reference/embeddings/persistence/tokenizerjsonloader-loadbpe.md), `BpeVocabulary` | `tokenizers` 0.23.1 accepts `byte_fallback: true` with a partial `<0xXX>` alphabet and degrades per symbol to `<unk>`, or drops the symbol outright when no `unk_token` is declared; it accepts any `decoder` shape | refuses the load, naming the first missing piece, and reads the `decoder` of such a file strictly — a bare `ByteFallback`, or exactly `Sequence[Replace, ByteFallback, Fuse, Strip]` | the degradation is silent and its output is a wrong token stream; refusing beats embeddings that are quietly wrong, and a complete alphabet also makes the reference's own unknown-token ordering bug unreachable rather than something to reproduce | `0063` |
| [`SplitConformal.Quantile`](../reference/conformal/prediction/splitconformal-quantile.md) at `alpha < 1/(n + 1)` | MAPIE 1.5.0 raises `ValueError`; under `allow_infinite_bounds=True` the regressor returns a finite interval whose half-width is the largest calibration score, and the classifier has no such flag | returns `double.PositiveInfinity` under either [`ConformalQuantileRule`](../reference/conformal/prediction/conformalquantilerule.md), carried to `(-inf, +inf)` by `Interval` and to the full label set by `PredictionSet` | MAPIE's finite answer under-covers silently, in the one regime where the calibration set is already too small for anyone to notice; a package promising finite-sample coverage cannot ship that as its edge case | `0070` |
| [`MatchRatingApproach.Codex`](../reference/text/phonetics/matchratingapproach-codex.md), [`MatchRatingApproach.Compare`](../reference/text/phonetics/matchratingapproach-compare.md) | `jellyfish` 1.2.1 measures the codex in UTF-8 bytes for the six-character truncation, the length-gap check and the minimum-rating lookup, so a codex can grow under truncation and two 2-character codices can be refused a rating | measures `string.Length` throughout — equal to the character count for every codex either side produces | a byte/character mixup nothing in the 1977 description states; deriving it would mean reading it out of the reference implementation, which [`0149`](0002-provenance-and-the-allowed-references.md) forbids | `0080` |
| [`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) on an exact tie | `sklearn.cluster.KMeans(algorithm="lloyd")` assigned the second centre in one measured configuration and the first in another; neither `<` nor `<=` reproduces both | the lowest-indexed centre, `numpy.argmin`'s rule, implemented as a strict `<` over the squared distance | the choice lives in compiled Cython driven by BLAS and cannot be read, only guessed; and the tie is arbitrary in Lloyd's algorithm — a sustained one differs by a relabelling every metric here is invariant to | `0093` |
| [`ShapiroWilk.Test`](../reference/stats/tests/shapirowilk-test.md), [`KruskalWallis.Test`](../reference/stats/tests/kruskalwallis-test.md) under [`NanPolicy.Omit`](../reference/stats/nanpolicy.md); [`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) on a `NaN` cell | scipy answers `(nan, nan)` with a warning on a sample omission left degenerate, and `chi2_contingency` carries a `NaN` cell through | raises `ArgumentException`: omission is a filter and the family's own guards run afterwards, unchanged; a contingency cell is a count and its marginals are divided by | answering `(nan, nan)` only on the path reached through omission would make one degenerate input raise or not according to how it arrived, which is a worse contract than either answer | `0117` |
| [`BpeTokenizer.Decode`](../reference/embeddings/tokenization/bpetokenizer-decode.md) | `tokenizers` decodes lossily — U+FFFD for a byte sequence that is not well-formed — and encodes strictly | the same: U+FFFD on the way out, a lone surrogate still refused on the way in | **not a divergence** — the strict decode was one, and it was withdrawn. It threw on 6 of 6 ids for `東京 👋` decoded one at a time, and nothing defended that. The asymmetry is the reference's own, not a policy invented here | `0023` |

## Consequences

- Nine rows in `docs/equivalence.md` carry these cases. Two are incomplete and are corrected
  in this lot: `jaro_winkler_similarity`, which states the Winkler parameters and not the
  combining-mark divergence its sibling rows state, and `match_rating_codex`, which claims
  exact parity over 420 words without naming the truncation half of `0080` — the comparison
  row names it, the codex row does not.
- Four cases cannot be frozen, because the reference produces no value to freeze: MAPIE
  raises where `Quantile` answers an infinity, `jellyfish` has nothing correct past the
  byte-safe codex range, `tokenizers` accepts every file this loader refuses, and
  `scikit-learn` raises on the empty labelling. They are pinned by `SplitConformalEdgeTests`,
  by `[InlineData]` facts in `MatchRatingApproachOracleTests`, by loader tests, and by a
  `mutual_information: null` fixture in `clustering_agreement.json`.
- No frozen case turns on the k-means tie: the two fixtures that hinged on one were replaced
  with centres the arithmetic decides between.
- The oracle corpora are generated from an explicit reference of the published definition
  wherever the library is the thing diverging — `_hamming_reference`, `_jaro_reference` and
  `_jaro_winkler_reference` in `tools/generate_oracles.py`, and `difflib` called with
  `autojunk=False`.
- A change in a reference that closes one of these gaps does not reopen the decision by
  itself. The divergence was never a bet on the reference staying wrong; it was a refusal to
  freeze something unreadable or indefensible. What surfaces such a change is the
  `Oracles are reproducible` job, and what answers it is a new record.
