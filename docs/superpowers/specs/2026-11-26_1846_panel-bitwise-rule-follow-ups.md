# Panel follow-ups to the bitwise rule: the between total and the spanned constant

**Issues:** [#1846](https://github.com/CyrilB1531/lodestar/issues/1846),
[#1847](https://github.com/CyrilB1531/lodestar/issues/1847),
[#1848](https://github.com/CyrilB1531/lodestar/issues/1848).
**Status:** written with the work, 2026-11-26.
**Date:** 2026-11-26.

## The problem

Three findings of the post-merge review of [#1843](https://github.com/CyrilB1531/lodestar/pull/1843), which closed
issues #1784 to #1790 under [its spec](2026-11-19_1784_regression-review-findings.md). The two defects were reproduced
on `main` at 604602e5 by tests of `ExactFitTests` before the source changed:

- `ExactFit`'s summary said `IsZero` serves "a panel total", where its panel caller reads one response value, which
  stands for the uncentred total only on a flat response (#1846).
- The between R² divides by the entity means' total. On a response that varies within each entity but whose entity
  means are bitwise equal, `1, 2, 3, 4, 6` rotated by entity over 8 × 5, a constant alone read a between R² of 0,
  pooled least squares beside an intercept −5.7e27, and the between fit an R² of −116, a model F of −5.95 and an
  unadjusted Wald form of 3.38 (#1847).
- Beside a constant the columns span without holding it, two indicators and a regressor at `y = 5`, the unadjusted
  `RobustModelTest` read 3.84e30 where `ModelTest` read `NaN` (#1848).

## The rule

Cyril's, from the #1784 spec, unchanged: a fit is exact only when every residual is bitwise `0.0`, which gives `+∞`; a
response is constant only when its values are bitwise equal, over the whole sample or within each entity, which gives
`NaN` for every statistic divided by that total ([decision 0010](../../decisions/0010-each-algorithm-follows-its-defining-document.md));
everything else reads the ordinary formulas, with no tolerance. #1847 and #1848 reopen two points that spec had
settled, at Cyril's request.

## Decisions

- **The between R² reads its total's zero on the entity means as computed** (#1847): `NaN` where they are bitwise
  equal beside a constant, or all exactly zero without one, the uncentred total's zero; a constant alone reads it too.
  The flat-response test stays first, so a constant response, whose means an entity size can leave an ulp apart, keeps
  its `NaN`.
- **The between fit reads its response's constancy on the entity means it regresses** (#1847): a new
  `ResponseGroups.EntityMeans` for `Between` with a constant, constant where the response is or where the means are
  bitwise equal, which its R², model F and unadjusted Wald form read. The summary computes the means once and both
  readings take that one flag, so the two read the means' constancy alike (Review A pass 1); a centred total that
  underflows on means apart is `Explained`'s guard, settled below.
- **The unadjusted Wald form reads `NaN` wherever the response has nothing to explain** (#1848), beside a constant the
  columns only span as beside a constant column or none, as `ModelTest` does; the `IvScores.IsFlat` scan that split
  the two goes. The IV model test is not touched: no issue names it.
- **`ExactFit`'s summaries name the values tested** (#1846): a response value and an entity mean, not a total.
- **One grid pins both invariants**: `PanelEdgeGridTests` crosses seven estimators (pooled, entity, period and
  two-way effects, between, random effects, first differences) with five shapes (balanced, one entity, one period,
  unbalanced, singleton entities), four constants (a column beside a regressor, two indicators that span one, a column
  alone, none) and ten responses (varying, varying in a pattern period effects absorb, constant at 0.1 and at 5,
  constant within each entity, constant within each period, entity means equal while it varies, equal in exact
  arithmetic but an ulp apart, zero while it varies, zero), 1400 cells, debiased or not, under the unadjusted, robust
  and entity-clustered covariances, 862 of them fitted, 812 clustered. Each refuses with the message its estimator and
  shape predict, or reads `+∞` with p-value 0, `NaN` with `NaN` or a finite value as the rule predicts for its F and
  its unadjusted Wald form, `NaN` on its zero block or a finite value for its robust or clustered one, and `NaN` or a
  finite value for its four R², the three panel R² 0 exactly for a constant alone, which has no test, wherever their
  total is not zero; the means are read by LINQ rather than the library's code (Review A passes 9 to 23). It replaces
  `main`'s `Every_unadjusted_panel_wald_form_reads_as_its_model_f`; a constant alone keeps a fact of its own in `ExactFitTests`.
  An unbalanced panel at `y = 0.1`, whose means an entity of three rows leaves an ulp apart, pins the flat response
  read first (Review A pass 1).

## Settled

- **The #1784 spec's Settled section holds**, but for what these issues reopen or retire: its Settled entry *Between
  reads its response's constancy … on the rows as given, not on the entity means* (#1847); its *Decisions* clause that
  a constant the columns only span reads its ratio (#1848); and its Settled entry *`IvScores.IsFlat` reads the one
  column `FindConstant` chose*, whose panel call goes with that clause, `IsFlat` now private to `FirstFlatColumn`.
  Its entries that the Wald-form grid pins random effects without a constant, and that `ExactFitTests` checks each
  degenerate Wald form against its F over a grid, now point at `PanelEdgeGridTests` for the panel.
- **The IV unadjusted model test beside a spanned constant keeps its ratio** (Review A pass 3): #1848 names the panel's
  two tests disagreeing on one fit, the IV fit has one model test, and the #1784 spec settled it (Review A passes 1,
  7, 8, 13); this branch's scope is the three issues.
- **Random effects beside a spanned constant reads its Wald form `NaN` where it fits** (Review A passes 4, 8): on a
  response varying across entities but constant within each, θ rounds to one on these shapes and the fit is refused
  as collinear, as a constant column is where θ rounds to one, which these shapes do not reach; on a response constant
  overall, θ stays below one (0.87 at `y = 5`, 0.96 at `y = 0.1` on 8 × 5) and the fit reads `NaN`, which its row
  states.
- **A panel of one entity reads its between R² `NaN` beside a constant** (Review A pass 3): its one mean is bitwise
  equal to itself, so the centred total is exactly zero, the rule's case, where linearmodels' guard reads 0.
- **The between fit's constant is the one found on the rows as given** (Review A pass 1): linearmodels' `has_constant`
  reads the exogenous rows, and a constant the means span but the rows do not leaves the fit uncentred, whose total,
  the means' squares, is not zero on equal nonzero means; no statistic divides by zero there.
- **The Wald form's `NaN` follows `hasConstant` as `ModelTest`'s does** (Review A pass 2): `FindConstant` is
  linearmodels' `has_constant`, which decides the model, its rank fallback included, not whether the response varies;
  #1848 asks that the two tests agree, and the response's constancy stays bitwise.
- **`Explained` keeps its `0` on a total that is not positive** (Review A pass 2): it is linearmodels' own
  `1 - residual_ss / total_ss if total_ss > 0.0 else 0.0`, which only a total that underflows, or one an overflowed
  sum leaves `NaN`, reaches once the bitwise zero totals read `NaN` first; it predates this branch.
- **The summary computes the entity means itself, once and always, not from the between fit's rows** (Review A passes
  2, 4): one pass over the rows into an array of one value an entity, which keeps the reading independent of how a fit
  transforms its rows; skipping it on a constant response cost a nullable field for a degenerate input.
- **The #1784 spec is not edited** (Review A pass 4): a spec records the work of its date, as that spec says of
  #1779's; the entries it settled that these issues reopen or retire are named in the first entry here, and both
  Settled sections travel to every review together.
- **`Family`'s guard states the overall total's zero and `BetweenTotalVanishes` the between total's** (Review A
  passes 5, 6): on a flat response the two coincide, by the rule, and each reads the total it names.
- **Each expected refusal is matched by words of its message** (Review A passes 6, 10): a rewording fails the grid
  loudly at that cell, which is the point, and any other refusal fails it too.
- **The edge grid's predicates restate the rule, not the code** (Review A pass 11): per estimator, from this spec
  and the #1784 one, with the means by LINQ; a rule wrong in both places is the spec's to change, and the linearmodels
  replay below is the independent reading. LINQ's average sums in row order and divides as `GroupMeans` does, on
  purpose (Review A passes 9, 20, 21): the rule reads the bits the fit computes, so the test computes the same bits by
  code of its own; the facts beside it read their preconditions either way.
- **`Constancy` returns the entity means beside the flags, not in their struct** (Review A passes 5, 10): a record
  struct holding the array would compare it by reference in the equality it generates.
- **A between fit's own R² and its between R² may part on a total that underflows** (Review A passes 15, 32): means
  apart by an ulp near 1e-170 beside a constant, or means near 1e-170 but not zero without one, leave `Explained` its
  linearmodels guard, 0, and the fit's R² and F `0/0`; each is its reference's formula, read on values not zero, and no
  total of ordinary magnitude reaches it.
- **The edge grid runs the unadjusted, robust and entity-clustered covariances** (Review A pass 16): they reach both
  of the summary's degenerate branches, the zero block and the classical F; the kernel and period or two-way
  clusters clear the same block through the same `exact` test, which this branch does not touch.
- **The edge grid bounds neither a finite F nor an R²** (Review A passes 19, 23): entity means an ulp apart read the
  between F −5.99 off the rounding, a constant alone its R² −2.2e-16, and random effects, centred on the untransformed
  mean, −0.023 on the singleton shape, each the ordinary formula the rule's third clause keeps.
- **"Bitwise equal" is equality as doubles compare** (Review A pass 25): `IsConstant` and `ConstantWithin` test `!=`,
  so −0.0 and +0.0 are one value, whose centred total is exactly zero all the same, as #1784's rule reads them.
- **`ZeroTotals` is a record struct** (Review A pass 27): named fields across `Constancy` and `Family`, whose generated
  members are the compiler's, as the package's other record structs' are.
- **A transform that rounds a varying response to one value reads its ratio** (Review A pass 30): entity effects
  over an entity near 1e-9 beside entities near 1e10 leave the transformed response bitwise constant once the grand
  mean returns, F −df beside a finite Wald form; the rule reads the rows as given within each entity, which vary, and
  #1847 reopens the between fit alone, so the ordinary formulas stand there. Reading the fit's own transformed response
  is a wider rule than these issues ask, Cyril's to take up.
- **Entity sums that overflow read as `main` read them** (Review A passes 33, 39): sums that overflow to one infinity
  are equal means, the rule's `NaN` where the arithmetic gave it before; sums that overflow apart leave the between
  total `NaN`, which `Explained`'s guard reads 0 as on `main`, beside `NaN` elsewhere. No guard is added for values past
  the double range.
- **The within R² demeans the response through `Demean`, which recomputes its entity means** (Review A pass 42): as
  on `main`; sharing the summary's means would move a line no issue here names.
- **The grid's `NoVariance` spells the between arm's beside-a-constant half** (Review A passes 24, 25, 41): the between
  fit's flag exists only beside a constant, so `BetweenTotalZero`'s no-constant half cannot apply there, and calling it
  behind `hasConstant &&` reads as if it could.
- **The edge grid pins its fitted-cell count exactly** (Review A passes 24, 37): the per-cell failures are reported
  beside it, so a count that moves with them names its cells; one that moves alone means a refusal prediction drifted.
- **`BetweenTotalVanishes` keeps its name** (Review A pass 31): its summary says it is the rule's reading of the total,
  the constant response included, not the arithmetic total.
- **`IvScores.IsFlat` stays a private helper of `FirstFlatColumn`** (Review A pass 29): its empty-column guard is the
  #1784 spec's settled one, and inlining it moves code no issue here names.
- **A fact restates its precondition's means inline** (Review A passes 20, 23): one LINQ line, so the fact reads alone.
- **`GroupMeans` keeps its sum over the count** (Review A pass 20): it is the mean every panel oracle pins at `1e-9`,
  linearmodels' own, and a constant group's mean an ulp off is why a constant response is read on itself first;
  changing it would move every between and within estimate, beyond these issues.
- **`Family`'s early return reads a flat response's zero on its first value** (Review A passes 14, 15, 22): the
  values are bitwise equal there, so `IsZero(y[0])` is `AllZero(y)` without its scan of `n` equal values.
- **The implied-constant panel case keeps its `NaN` at `y = 0.7` without branching on vanishing residuals** (Review A
  pass 22): the #1784 spec settles that it fails loudly if they ever vanish.
- **A between summary recomputes the means of its regressors for the between R²** (Review A pass 19): as on `main`,
  one pass the fit's rows already hold; this branch adds no pass over the regressors.
- **The grid builds each `PanelDesign` per fit** (Review A pass 17): it is a ref struct, which cannot live across the
  iterator's `yield`.
- **Each suite keeps its own regressor** (Review A passes 14, 17): `ExactFitTests`' searched designs rest on its
  column, so a change made for the edge grid must not move them.
- **`ZeroTotals` carries the fit's `NoVariance` beside the three readings** (Review A passes 10, 17): one value built
  once in `Constancy`, `BetweenZero` itself for a between fit, which `Summarise` reads and `Family` does not.
- **The random-effects line drops #1787's "beside a constant column or none"** (Cyril, after the clean pass): #1848 reads
  the unadjusted robust test `NaN` wherever `ModelTest` does, so the line names #1848 beside #1787.
- **`ExactFit.Vanishes` keeps its name beside `AllZero`** (Review A pass 12): it names the residuals' test its remarks
  explain, which seven call sites read; `AllZero` is its body, the one the between total's zero shares.
- **The edge grid infers `+∞` from zero standard errors** (Review A passes 5, 43): the summary exposes no residuals,
  and `s²` underflows to zero only on residuals below about 1e-162, which no response of order one reaches; under the
  robust and clustered covariances the zeros are the summary's own clearing, which the unadjusted theories check
  against `s²`.
- **The between fit without a constant reads its R² NaN on entity means all zero by its arithmetic** (Review A pass 4):
  its least squares on a zero response gives coefficients and residuals exactly zero, so `1 − 0/0`, deterministically;
  the residuals vanish, so its tests are `+∞` first, the rule's first clause.
- **Random effects beside a spanned constant on a response varying across entities but constant within each is
  refused**, as settled for a constant column: θ rounds to one and zeroes the spanned constant; the edge grid expects
  that refusal.

- **The edge grid departs from linearmodels 7.0 in these classes only**, replayed cell by cell on the same data, every
  other finite value within `1e-9` (pre-review gate, 2026-11-26):
  - *The rule's `NaN` or `+∞` where linearmodels reads the rounding*: model F, Wald form and R² on a total the rule
    reads zero (−20.5 within on `y = 0.1` unbalanced, 3.1e32 beside a spanned constant), the between R² on equal entity
    means, on one entity and on zero means without a constant, 0 there, the within R² over one period or on one
    constant in each entity, 0 or rounding there, and `+∞` on residuals exactly zero, F 0 there; settled by the #1784
    spec and #1847.
  - *The rule's answer where linearmodels raises*: `ZeroDivisionError` from `fit()` on zero totals (between, first
    differences, within, random effects), `LinAlgError` from the Wald form on a zero response; the equivalence rows'
    "the reference raises".
  - *Refusals where linearmodels fits*: random effects over one entity, its variance components refused as on `main`;
    random effects beside a constant alone at `y = 5`, fitted exactly, both variance components zero and θ 0/0, refused
    as collinear as on `main`;
    random effects beside a spanned constant on a response varying across entities but constant within each, θ
    rounding to one, settled above.
  - *No model test where linearmodels reports an invalid one*: a constant alone reads `ModelTest` and
    `RobustModelTest` `null`, as on `main`, where linearmodels returns a test of `NaN`.
  - *Both reading rounding, apart*: a fit its regressor and effects make exact but for rounding, period and two-way
    effects on the response a period pattern adds, the between and one-period fits on it, and two-way effects on the
    singleton shape's varying response (F 6.5e29 against 1.8e31), entity means equal in exact arithmetic that each
    side's summation leaves an ulp apart differently (between R² −491 against −0.8, numpy summing pairwise), random
    effects on a response constant within each entity beside a constant column, whose coefficients θ's rounding sets
    (between R² −0.0033 against −2.40), and pooled least squares over one entity beside two indicators that tie in
    normed variance, so rounding picks the column the Wald form leaves out (1462 against 6731 on the varying response);
    `FindConstant` is not touched here.

## Evidence

- `PanelEdgeGridTests` fails on `main`'s `PanelDiagnostics` and `PanelCore`, with `IvScores` restored only because
  `main`'s summary calls `IsFlat`: #1847 in the between R² on equal or zero means and the between fit's F and R², #1848
  in the unadjusted Wald form beside a spanned constant, finite where the model F is `NaN`; it passes here.
- `A_constant_alone_reads_no_between_r_squared_on_equal_entity_means` (#1847) and
  `An_implied_constant_keeps_the_iv_model_test_and_not_the_panel_wald_form_on_a_constant_response` (#1848) fail on
  `main`'s source and pass here; `A_constant_response_reads_no_between_statistic_whatever_its_means_round_to` passes on
  both, pinning that the means' reading keeps `main`'s answer on a constant response. The values above were read on
  `main`'s source.
