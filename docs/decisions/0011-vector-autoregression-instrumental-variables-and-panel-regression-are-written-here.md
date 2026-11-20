---
status: accepted
supersedes: []
amends: ["0004"]
applies: []
---
# 0011 — Vector autoregression, instrumental variables and panel regression are written here

**Status:** accepted · **Date:** 2026-11-19

## Context

[`0004`](0004-what-is-written-here-and-what-is-delegated.md) records, among its **not written**
rows, three capabilities:

- the vector autoregression, *the one of the four that could be*;
- instrumental variables: 2SLS, LIML and two-step GMM;
- the panel estimators: `PanelOLS`, `BetweenOLS`, `FirstDifferenceOLS` and `RandomEffects`.

Its consequences name these three, among others, as rows a caller reopens.

All three ship, which here means merged to `main`, released or not:

- `VectorAutoregression.Fit` in `Lodestar.Stats.TimeSeries` landed on 2026-09-16
  ([#786](https://github.com/CyrilB1531/lodestar/issues/786)). Four days later `0004` carried over
  the first numbering's `0134`, whose decision 3 says of the VAR "It is not written by this record",
  as a *not written* row, dropping the "read here" that limited its absence claim to the packages
  read.
- `InstrumentalVariables` ([#1155](https://github.com/CyrilB1531/lodestar/issues/1155)) and
  `PanelRegression` ([#1156](https://github.com/CyrilB1531/lodestar/issues/1156)) in
  `Lodestar.Stats.Regression` landed on 2026-09-25, five days after `0004`.

`VectorAutoregression`'s remark said "decision 0004 could write this model", and
`InstrumentalVariables`' cited `0004` for why it is "written here rather than delegated", while the
rows they cite say neither was written. On
2026-09-26 the review of `main` before 1.0.0 found the record contradicting the code that cites it
([#1219](https://github.com/CyrilB1531/lodestar/issues/1219)).

## Options

- **A new record amending `0004`'s rows.** Chosen: `0004` keeps the verdicts it carried from
  `0134`'s and `0135`'s readings, and this record says what changed and when.
- **Rewriting `0004` in place under a raised `.numbering-epoch`.** Epoch 3 did that for `0003`'s
  rule. Loses here: each *not written* row was true of the day it was read, `0134`'s and `0135`'s
  2026-09-15, before any of the three merged, and rewriting would erase those readings where an
  amendment keeps both. `0004`'s restatement of the VAR row on 2026-09-20 was already stale, which
  this record says rather than rewrites.
- **Withdrawing the three capabilities.** Loses: the vector autoregression is released, in
  `Lodestar.Stats.TimeSeries` 0.1.0, and taking a release back is what `0004`'s caller question
  warns against: "Publishing later is always available; unpublishing is not." Instrumental variables and the panel
  estimators were not yet released on 2026-11-19, so `0004`'s caller rule points toward withdrawing
  them. They are kept because #1155 and #1156 were opened to close their *no counterpart yet* rows of
  `docs/equivalence.md` under #689's v1 scope, and merged; this record takes that scope as their
  claim on the void, and no further.

## Decision

**The three rows read *written here*, each on the measurement `0004` gave it.** These three verdicts
are this record's, taken on 2026-11-19. Each *why* is quoted from `0004`'s row, and rests on the
reading the first numbering's `0134` and `0135` took on 2026-09-15 (they read at
`git show 53af23c2:docs/decisions/`). The absence of a .NET incumbent is not re-read here.

| capability | verdict | why | why from |
| --- | --- | --- | --- |
| vector autoregression | written here, in `Lodestar.Stats.TimeSeries` | `VAR(...).fit(p)` is equation-by-equation least squares on stacked lags and matches `numpy.linalg.lstsq` at `0.0`; no .NET package estimates one | `0134` |
| instrumental variables — 2SLS, LIML, two-step GMM | written here, in `Lodestar.Stats.Regression` | seven .NET libraries, three commercial, export nothing that fits one; `linearmodels` 7.0 reproduces each closed form at `1e-15`. Iterated GMM is out: it moves with its tolerance at `6.6e-6` | `0135` |
| panel estimators — `PanelOLS`, `BetweenOLS`, `FirstDifferenceOLS`, `RandomEffects` | written here, in `Lodestar.Stats.Regression` | the same reading; the small-sample factors are `linearmodels`' own and a corpus has to pin them rather than assume `statsmodels`' | `0135` |

This amends, in `0004`, for these three rows only:

- the rows themselves;
- its method's "Every verdict below was taken between 2026-08-30 and 2026-09-16": these three are
  this record's, taken on 2026-11-19;
- the consequence that a caller reopens them, which no longer applies, since they are not *not
  written*;
- `0004`'s third question, *is there a caller?*, under which "a void that is real, provable and
  unclaimed is recorded and left". None names a caller; each was opened instead as an issue of
  Cyril's, as the next paragraph records, and they are written.

What `0004` says of any other row, and whether a lot like these may claim another void, is
untouched. How the three were opened: in the first numbering, `0134` left the vector autoregression
to "a new issue" "if Cyril wants one", and `0135` left IV and panel to a lot "if Cyril wants
either", "proposed rather than opened" under the first numbering's `0095` rule. Cyril opened
each: #786, which
rests on `0134`'s finding that the model could be written, and, ten days after `0135`, the
issues #1155 and #1156, which cite `0004` and the *no counterpart yet* rows of `docs/equivalence.md`
and rest on [#689](https://github.com/CyrilB1531/lodestar/issues/689)'s v1 scope, stated in their bodies as "a
package reaches 1.0.0 when `docs/equivalence.md` carries no *not written* row it means to close".

## Consequences

- **A citation of `0004` for one of these three verdicts reads this record too.** The vector
  autoregression's, the instrumental variables' and the panel regression's API remarks, reference
  pages and migration rows cite both, and `index.yaml` carries the edge back from `0004`. A citation of `0004` for a measurement
  or for an absence it records stays a citation of `0004` alone.
