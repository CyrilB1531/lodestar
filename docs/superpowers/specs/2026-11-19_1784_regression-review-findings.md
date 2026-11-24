# Stats.Regression review findings: an exact fit is one whose residuals are exactly zero

**Issues:** [#1784](https://github.com/CyrilB1531/lodestar/issues/1784),
[#1785](https://github.com/CyrilB1531/lodestar/issues/1785),
[#1786](https://github.com/CyrilB1531/lodestar/issues/1786),
[#1787](https://github.com/CyrilB1531/lodestar/issues/1787),
[#1788](https://github.com/CyrilB1531/lodestar/issues/1788),
[#1789](https://github.com/CyrilB1531/lodestar/issues/1789),
[#1790](https://github.com/CyrilB1531/lodestar/issues/1790).
**Status:** written with the work, 2026-11-24; the rework of a branch begun 2026-11-19, whose rounding bounds 56
review passes never closed.
**Date:** 2026-11-19.

## The problem

Seven findings of the Review B after [#1783](https://github.com/CyrilB1531/lodestar/pull/1783), on the exact-fit test
and the degenerate-response rule [#1779](2026-11-13_1779_review-b-after-1774.md) introduced. Each was reproduced on
`main` at 64dcf7b3 by a test of `ExactFitTests` before it was changed:

- `ExactFit.Holds` charged every row the projection's shared rounding, so a row zero in every column could never pass:
  an exact fit beside a zero weight, a zero row or a missing category was not exact, whose residuals all were exactly
  zero on the indicator design the tests use (#1784).
- Relative noise of 1e-14 to 1e-11 passed the raw reject and paid the reflections before failing: 10.4 ms and
  11.8 MB at 40,000 × 8 under HC1, where the fit without the test takes 4.75 ms and 3.1 MB (#1785, *Evidence*).
- GMM's unadjusted weight on a constant response, whose first-step residuals all equal one rounding, fell to the
  singular-weight message that blames clusters the fit does not have (#1786).
- Random effects on `0.1 + 0.3·(e mod 3)` read `F = −3.9e-15` and `R² = −2.2e-16`, θ being `1 − 3.6e-15` (#1787).
- `PanelSummary`'s page said the poolability F is `NaN` on a constant response; it is `+∞` first where the residuals
  are exactly zero (#1788).
- A constant alone read each panel R² 0 on `y = 5`, where one regressor more read `NaN` (#1789).
- Under the unadjusted covariance the panel's `RobustModelTest` read 2.887 at `y = 0.1` beside a `NaN` `ModelTest`,
  and the IV unadjusted model test 3.258 at `y = 5` beside a `NaN` R² (#1790).

## The rule

Cyril's, after 56 passes of a rounding bound that each found a new edge case:

- **A fit is exact only when every residual is bitwise `0.0`.** Its homoskedastic tests are then `+∞` with p-value 0,
  #1779's rule, and its robust, kernel and clustered covariances zero, their tests `NaN`.
- **A response is constant only when its values are bitwise equal**, over the whole sample or within each entity or
  period a transform removes. Every statistic divided by its total sum of squares is then `NaN`, as
  [decision 0010](../../decisions/0010-each-algorithm-follows-its-defining-document.md) reads an undefined statistic.
- **Everything else reads the ordinary formulas, with no tolerance.** A fit exact but for one rounding per residual
  reads the robust F its rounding gives, 5.7e35 here under HC0 on `y = 3 + 2x₁ − x₂`, as statsmodels and linearmodels
  read theirs, each its own rounding's number.

## Decisions

- **`ExactFit` keeps `Vanishes` alone.** `Holds`, `Share`, `Magnitude`, the span's reflections and the transforms'
  floors go, with the conditioning numbers they read (`IvCore.Conditioning`, `PanelFit.Conditioning`), the rows as
  given a GLS fit kept for them (`SolvedFit.GivenRows`, and the design's copy), and `Lodestar.Stats.Regression`'s use of
  `ExactProduct` and `LargestMagnitude`; `LargestMagnitude` moves into `Lodestar.Stats.TimeSeries`, its one reader
  left, where #1779 had shared it. The residual sizes the robust score rows read stay, inlined into
  `RobustCovariance.ResidualMagnitudes`, their one reader (#1784, #1785).
- **A leverage of exactly one keeps HC2's and HC3's 0/0 on an exact fit** (Review A passes 9, 10): statsmodels
  reads `NaN` standard errors there, and #1779 pass 10 settled reading as it does, so the zero covariance waits on no
  divisor being zero; such a fit pays its leverages, as every HC2 or HC3 fit does. Rows interpolated keep their zero
  covariance first, as on `main`, under every type.
- **An exact panel fit's robust covariance is cleared, not read** (Review A passes 9, 10, 22): zero whatever the bread
  and scaling multiplied its zero meat by, as the IV fits return a fresh zero block; the fit estimates it before the
  summary reads the type's exactness, so one exactness test, the summary's, stays.
- **The leverage of one is read as computed, bitwise, as the residuals are** (Review A pass 22): a leverage that rounds
  to one less 1.1e-16 divides by that, as statsmodels' does, so two solves of one exact fit can read 0 and `NaN`.
- **The zero divisor is tested on its own, not by building every exact fit's meat** (Review A pass 22): that would cost
  each exact HC0, HAC and clustered fit a filling it never reads to find a 0/0 only HC2 and HC3 can hold.
- **The residuals tested are the ones the covariance reads**: the whitened residuals of a weighted or generalized fit,
  the transformed ones of a panel fit, the second stage's of an IV fit. A zero weight zeroes its whitened row, so its
  residual is exactly zero, and a row zero in every column scores nothing (#1784).
- **GMM refuses every weight on first-step residuals exactly zero**, naming `design`, and an unadjusted weight whose
  `s²` about the residuals' mean is exactly zero, naming `design` and that cause (#1786). Residuals all one value whose
  mean rounds off it leave `s²` one rounding, which the weight inverts as linearmodels' does: the rule's constancy is
  the response's, not the residuals' (Review A passes 6, 18). A singular weight names clusters only under a clustered weight whose clusters fall short (Review A
  passes 4, 12).
- **Random effects groups its response by entity** (#1787): a response constant within each entity leaves `σₑ²` zero
  in exact arithmetic, so θ one, which removes it whole; where `σᵤ²` is computed zero too θ is 0/0, refused as on
  `main` (Review A pass 51). Its model F, its unadjusted Wald form and its R² read `NaN` there whatever θ the
  rounding reads.
- **The panel's R² family reads the response before the constant's zeros** (#1789): a constant alone on a constant
  response reads `NaN` for the three, and the within R² `NaN` on one constant in each entity, which a panel of one
  period, or of one row an entity, always is: its within total is zero, where one period read 0 (Review A pass 12).
- **The unadjusted Wald forms read as the classical F does** (#1790): the panel's `RobustModelTest` under the unadjusted
  covariance, and the IV model test under it, are `+∞` on residuals exactly zero, then `NaN` where they are the
  classical F, a response with nothing to explain beside a constant column the test leaves out, or, in the panel,
  none: their restricted fit is then the constant or nothing, whose residuals are the total. Beside a constant the
  columns only span the restricted fit is one of those columns, no total divides the test, and it reads its ratio by
  the rule's third clause, whatever exact arithmetic gives it, a 0/0 under random effects or a zero grand mean included
  (Review A passes 1, 7, 8, 13). `ExactFitTests` checks the panel's invariant over every estimator, intercept and
  degenerate response it fits.
- **The poolability F's page states its `+∞` first** (#1788), as the model F's sentence does.

## Settled

- **The rule above is Cyril's and settles every exactness question this branch raised.** A finding that a fit exact
  in exact arithmetic reads its rounding, a huge robust F, a robust test of rounding on a constant response, or a
  GMM J of 21052.6 at `y = 7.7`, describes the rule working, not a defect; so does a homoskedastic test on residuals
  of one rounding.
- **Residuals exactly zero read `+∞` first, a constant response's included** (Review A pass 15): the rule's first
  clause comes before its second, as #1779 settled, so `y = 0` under an intercept, or any response whose residuals
  vanish bitwise, reads a homoskedastic test of `+∞` beside a `NaN` R², where a response one rounding off reads `NaN`.
- **Between reads its response's constancy, and its constant column's, on the rows as given, not on the entity
  means** (Review A passes 17, 25): the rule
  reads a constant response on the values the caller gave, whole or within each entity; equal entity means of a
  response that varies are a computed sum's rounding, and read the ordinary formulas, the constant-alone between R² of
  0 among them, as the `PanelOLS` row already says of a response the entity means absorb.
- **The IV model test's flat column may be all zero only on a design refused first** (Review A pass 17): a zero column
  leaves `XᵀX` singular, refused as collinear before any test is read.
- **`Interpolates` stays** (Review A passes 1, 6): at most as many rows carrying a regressor as parameters fit those
  rows exactly whatever the rounding leaves, a count rather than a tolerance, and it predates #1779, #1223 having read
  that robust block's rank as zero; the IV and panel fits count their score rows through their own largest rank.
- **Robust Wald tests on a constant response keep their ordinary formula**, OLS's robust F included, which `Holds`
  made `NaN` on `main`: they divide by no total sum of squares. Sargan's test and Hansen's J keep their ratio for the
  same reason, and so do the per-coefficient t statistics.
- **Random effects beside a constant on a response constant within each entity is refused as collinear where θ
  rounds to one**, as on `main`: θ one zeroes the constant's column. Where θ rounds below one, the table reads the
  rounding, its model F and R² `NaN` by the rule.
- **The second GMM step's exact test stays, reached by a searched design** (Review A passes 6, 18, 31): twenty rows of
  small integers whose first step keeps a residual of one rounding and whose second, under a robust weight, leaves
  every residual exactly zero, on both targets; without the branch such a fit would read t statistics of ±∞ beside
  its zero covariance, where the k-class fits read `NaN`.
- **The weight's zero refusal travels as one optional name into the score covariance** (Review A pass 31): the
  reported covariance passes none, and the `s²` it would test is read once.
- **Random effects reads its response's constancy on the rows as given, not off the θ it computed** (Review A pass 1):
  a response constant within each entity leaves θ one in exact arithmetic, undefined for a statistic divided by the
  transformed total, so a θ the rounding lands at 0 or 0.5 changes nothing the rule reads.
- **The first stage reads each response's constancy, centred or not** (Review A passes 3, 4, 7): `IsConstant` stops at
  the first value that differs, so the scan costs a varying response two reads.
- **The GMM weight's `s²` keeps its own loop beside the R²'s total** (Review A passes 6, 8): sharing it coupled the
  weight, which oracles pin at 1e-9, to a helper changed for R²'s sake.
- **The implied-constant panel case reads `y = 0.7`** (Review A pass 7): its residuals are one rounding apiece, which
  the classical F's `NaN` beside it asserts, so the test fails loudly rather than pass on `+∞` if they ever vanish.
- **`IvScores.IsFlat` reads the one column `FindConstant` chose** (Review A pass 9): only on a response with nothing to
  explain, so the rescan costs a degenerate fit one column, where widening `FindConstant`'s tuple moved every caller.
- **The degenerate Wald forms keep their predicates beside each model F** (Review A pass 9): the panel's and the IV's
  differ by what their transform removes, and `ExactFitTests` checks each against its own F or R² over a grid.
- **The equivalence rows keep "the reference raises" on exact fits** (Review A pass 13): linearmodels' `fit()` itself
  raises, `LinAlgError` for 2SLS on the Hadamard design and `ZeroDivisionError` for entity effects on `y = 0`, before
  any standard error can be read.
- **A GMM weight whose block is finite but whose inverse overflows reads as on `main`** (Review A passes 22, 23):
  `CovarianceRank.Inverse` returns that inverse, and residuals near 1e-158 then reach the collinear refusal of the
  second step; no issue of this branch names it, and the overflow refusal keeps its words.
- **GMM's unadjusted reported covariance on second-step residuals all one value reads its formulas** (Review A pass
  28): its centred `s²` may round to zero, leaving zero errors and infinite t statistics, the rule's third clause, as
  no exact fit; no input was found to reach it.
- **The IV estimators' repeated products stay as on `main`** (Review A pass 29): GMM's three `XᵀZ` passes and its
  second inversion of the normal matrix, and the k-class inverse discarded before the bread's, predate this branch.
- **`FindConstant`'s rank fallback keeps choosing the column the panel's Wald form leaves out** (Review A pass 29): a
  design rank-deficient by numpy's tolerance yet invertible is `main`'s case, and its column reads as a spanned constant.
- **`IvFit` carries one flag, `Vanishes`, which each reader splits by its covariance type** (Review A passes 21, 29).
- **An exact kernel fit's bandwidth past the sample is refused with the message and parameter the meat's path gives**
  (Review A pass 32): `RequireBandwidth` is the refusal `KernelWeights` made, unnamed as there.
- **`IvCore.Gmm` keeps taking its two parameter names from the public overload** (Review A pass 32): `nameof` at the
  caller, as `main`'s `singularName` was; clusters too few by label count, not by spanning sums, are what it blames,
  as `Gmm_does_not_blame_clusters_enough_for_scores_that_span_too_few_directions` pins.
- **The panel's Wald form reads its covariance type beside `vanishes`, not the summary's `exact`** (Review A passes
  6, 33): the type decides both of its degenerate answers, `+∞` and `NaN`, where `exact` holds only one.
- **`Shape` keeps its one pass over three flags beside `IsFlat`'s early exit** (Review A passes 29, 33): `FindConstant`
  reads all three, and both compare through `Same`.
- **An exact HC2 or HC3 fit at a leverage of one builds its NaN filling** (Review A pass 34): the formulas give the
  `NaN` statsmodels reads, and a hand-built `NaN` block would be a second source of it.
- **A GMM weight bandwidth past the sample is refused where the data reach it, as on `main`** (Review A pass 34): an
  exact first step is refused first, its order predating this branch.
- **An exact clustered OLS fit returns its zero block before reading its labels** (Review A pass 36): `Interpolates`
  did on `main`, and every clustered fit carries its labels from the public overload.
- **No test drives an exact fit through a κ other than one** (Review A pass 39): an exact fit leaves the response
  purged of the instruments a multiple of the endogenous columns purged of them, so LIML, and Fuller's κ from it,
  refuses its κ as undefined before any covariance; 2SLS, κ one, is what reaches the zero block.
- **`ExactFit.IsZero` is the one exact-zero test, residuals' and the others'** (Review A pass 42): a divisor, an `s²`
  and an uncentred total read zero by the same bitwise rule, so a rule changed would rightly change them together.
- **A NaN `s²` reaches GMM's not-finite refusal** (Review A pass 44): `CovarianceRank.Inverse` returns full rank and no
  inverse on a block holding a NaN, which `Gmm` tests before the rank.
- **The robust F's score-row rank test keeps its rounding bound** (Review A pass 44): it judges the form's rank, not
  exactness, and predates #1779 (#1223).
- **Too few clusters for a clustered GMM weight are named after the design's collinearity and before the
  residuals' refusals** (Review A passes 45, 47): the
  shortfall is the model's, singular whatever the response, so an exact first step or overflowing scores no longer
  hide it behind `design`.
- **`RequireBandwidth` tests the kernel itself** (Review A pass 45): the exact path calls it with any kernel, so its
  quadratic-spectral exemption is the helper's own, dead only inside `KernelWeights`.
- **The `+∞` tests read `Vanishes` alone, with no sum-of-squares filter before it** (Review A passes 42, 48): it stops
  at the first residual that is not zero, so the filter saved a scan only past leading rows exactly zero.
- **GLS keeps its own whitened-residual loop** (Review A pass 48): it predates this branch, beside the panel's twin.
- **Random effects without a constant is pinned by the Wald-form grid** (Review A pass 49): its intercept-free rows on
  a response constant within each entity are among the sixty fits it reads, model F and Wald form agreeing.
- **`ExactFit.Magnitude` is gone, its loop inlined into its one reader** (Review A pass 49): Cyril's rework named it
  among the members to remove, and the rank test's row sizes are the only use left.
- **GMM's score covariance computes its debiasing before the clustered branch, which scales by its own factor**
  (Review A pass 50): the order predates this branch.
- **`GmmScoreCovariance` names its clustered arm and throws on any other** (Review A passes 21, 52): a default that
  read clusters for any unlisted type is what pass 21 replaced; the throw guards a type added later.
- **A θ of 0/0 keeps `main`'s collinear refusal and its words** (Review A passes 51, 52): it predates this branch, and
  no issue here asks to reword it.
- **The first-stage R² keeps its three-argument overload** (Review A passes 47, 51, 52): one definition of the
  predicate, `NothingToExplain`, read by both overloads.
- **`IsFlat` calls an empty column flat** (Review A passes 50, 54): no caller reaches it, the row counts refused first,
  and the guard keeps the helper from reading past an empty block.
- **The exact second GMM step's test rests on a searched design** (Review A pass 57): a change of summation order
  that left one residual of rounding fails it loudly, and the search in the rework's scratch finds another.
- **The poolability F keeps its ratio on a response constant within each entity** (Review A pass 58): it compares the
  pooled fit's sum of squares with the effects' one, which removes that response whole, so its ratio tends to the
  `+∞` exact arithmetic gives, no total of the response dividing it; only a response constant overall leaves both
  sums rounding, `NaN`.
- **`Gmm_refuses_an_unadjusted_weight_on_residuals_that_do_not_vary` keeps its name** (Review A passes 4, 61): its
  dyadic half pins the refusal, its beside-a-constant half the message only, as settled above.
- **#1785 has no changelog entry** (Review A pass 1): the cost it removes is #1779's, unreleased, so against 0.2.0 a
  robust fit costs what it did; the changelog line #1779 wrote is rewritten to the rule instead.
- **The unadjusted first-stage instrument test keeps its ratio on a constant endogenous column** (Review A passes 4, 6): the
  rule's `NaN` needs the constant in the span of the columns the test leaves out, here the exogenous ones, and a
  constant endogenous column there is collinear with them and refused; beside a constant among the instruments the
  tested coefficients are nonzero in exact arithmetic, so the test is `+∞` there and reads its rounding, beside an R²
  that is `NaN` because its total is zero, not because the test is 0/0.
- **An exact fit returns its zero block before the bread, the weight's products and the meat** (Review A passes 2, 6,
  20, 24, 26, 27): a kernel one still refuses what its meat would, a bandwidth past the sample's lags through the
  kernel weights alone and an automatic k-class bandwidth as 0/0, without the kernel sum its zero scores would discard.
- **The response's constancy within each entity is read for every panel fit** (Review A passes 3, 4): one `O(n)` scan,
  which the within R² of every fit with more than one period reads.
- **The beside-a-constant GMM loop pins the message, not the decision** (Review A pass 4): whether those residuals
  fall equal is the rounding's, the rule's to leave alone; the dyadic design pins the refusal itself.
- **The poolability F keeps its own scans of the residuals and the response** (Review A pass 4): it is read by the fit,
  before the summary's flags exist.
- **The tests of a fit near exact perturb it by tens of ulps** (Review A pass 5): a fit exact in exact arithmetic
  leaves its residuals to the solve's rounding, which a change of summation order could round to zero, so they read a
  response no solve can fit exactly.
- **#1779's spec is not edited**: its exact-fit decisions are superseded here, and a spec records the work of its date.

## Evidence

- The tests reproducing the seven findings fail on `main`'s source (24 of them) and pass here.
- HC1 on `y = 1e6 + Σ(j + 1)xⱼ + σ·N(0, 1)`, one fit timed fifteen times after three warm-ups, tiered compilation off,
  AMD Ryzen 7 8700G, 2026-11-24, median: 40,000 × 8 at σ 1e-7, 10.4 ms and 11.8 MB on `main`, 4.75 ms and 3.1 MB here;
  10,000 × 2 at σ 1e-8, 0.82 ms and 1.5 MB, 0.35 ms and 315 KB; σ 1, 4.6 and 5.3 ms, 3.1 MB on both.
