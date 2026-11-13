# Stats.Regression review findings: non-finite input, singular Wald blocks and the automatic bandwidth

**Issues:** [#1225](https://github.com/CyrilB1531/lodestar/issues/1225),
[#1268](https://github.com/CyrilB1531/lodestar/issues/1268),
[#1223](https://github.com/CyrilB1531/lodestar/issues/1223) and
[#1269](https://github.com/CyrilB1531/lodestar/issues/1269).
**Status:** written with the work, 2026-11-08.
**Date:** 2026-11-08.

## The rule

Measured on statsmodels 0.15.0, linearmodels 7.0 and numpy 2.5.3 before any code; each clause below is what the
code then does.

1. **A value that is not finite in a floating-point block is refused**, by every public fit of the package, with
   `ArgumentOutOfRangeException` carrying the public parameter and naming the block and cell. The check runs once the
   lengths are judged and before anything reads the values, so it precedes every refusal that depends on the values
   (degrees of freedom, cluster labels, options, the GLM family) and no later check can misdiagnose the NaN.
   `weights`, `covariance`, `offset`, `exposure` and the GLM response already refused and keep their own refusals.
2. **A Wald test on a singular covariance block is defined by its rank, or undefined, never noise.** The rank is
   `numpy.linalg.matrix_rank`'s test on the block scaled to a unit diagonal; a cluster covariance's rank is further
   capped at `G − 1` wherever the estimator makes the cluster scores sum to zero (`X'e = 0`, 2SLS, GMM), and at `G`
   under an overidentified LIML, whose `κ ≠ 1` leaves them `(κ − 1)X'M_Z e`. `G` counts only the clusters whose
   summed scores are not zero, as a robust covariance's `n` counts the rows, since any other adds nothing to the meat.
   - OLS, WLS, GLS: statsmodels' `f_test` — the pseudo-inverse's quadratic form over the rank, the F on that rank,
     `NaN` at rank zero (statsmodels' `0/0`). The form is the one the covariance has in exact arithmetic, read from
     the score rows whose cross product is the filling and never from the formed block; it is `NaN` where the
     residuals' rounding can move it by `1e-6` of itself, since no digit of it is then known.
   - IV model and first-stage tests, panel's robust test: `NaN` statistic and p-value, degrees of freedom unchanged,
     wherever the rank, capped as above, falls short of the block's order; the panel caps two-way clustering at the
     cells' `G − 1`, and at the other clustering's `G − 1` plus the cells of every cluster holding several when grouped
     by one clustering's clusters, so one nested in the other spans the coarser's; Driscoll-Kraay at the periods less
     one ([decision 0010](../../decisions/0010-each-algorithm-follows-its-defining-document.md): linearmodels `inv()`s a
     block that either raises `LinAlgError` or returns noise such as `-6.1e17`).
   - A GMM weight singular in the second step is refused, naming the cause; the estimator itself is undefined there.
3. **An automatic kernel bandwidth that is 0/0 is refused** before the cast, as `linearmodels` raises `ValueError`
   (`int(ceil(nan))`). A given bandwidth still answers an exact fit.

## Decisions

- **NaN and infinity in a response are refused, where statsmodels answers a table of NaN or infinities.** Decision
  0010 would answer NaN where the document is silent; the issue's own fix direction, the package's existing refusal of
  every other non-finite input (weights, covariance, offset), and the impact the issue names (silent NaN tables) settle
  it the other way. The maintainer approved this (Cyril, 2026-11-10), over the alternative of returning the NaN table as
  the reference does.
- **`InstrumentalVariables` and `PanelRegression` refuse where linearmodels drops the row with a warning.** The
  issue's fix direction, and no warning channel here. A caller drops rows before the call. The maintainer
  approved this (Cyril, 2026-11-10); dropping rows would need an API that reports which rows.
- **A cluster F whose block is short of full rank follows statsmodels**, the issue's first option; for IV and panel,
  whose reference has no defined answer (noise or `LinAlgError`), decision 0010's NaN applies. The maintainer approved
  this (Cyril, 2026-11-10), though OLS and IV read the same defect differently because their references differ.
- **The rank is taken on the unit-diagonal block, not on the raw block `f_test` reads.** A badly scaled but full-rank
  block keeps the inverse main already took: on one measured design `f_test` reads `F = 10.9` where the exact inverse
  of the same block reads `9.65e8`, one real direction cut by scale. This is the one place the OLS answer departs from
  statsmodels on purpose; the equivalence row records it. `OlsSummary.FNumeratorDegreesOfFreedom` exposes that rank,
  statsmodels' `df_num`. A block of full rank is inverted from the rank test's own reflections on the unit-diagonal
  block, so it is factored once; so is the GMM weight, `C⁻¹ = S·R⁻¹Qᵀ·S`.
- **Short of full rank, the form is the exact covariance's, read from its factors.** Any choice of directions on the
  formed block fails one way or the other: forming `c·B·M·B` with a column at `1e5` leaves rounding of `1e-6` where a
  real direction at `1e-10` lives (Review A pass 3, `F = 2.5e14` against the exact 18.45), and directions chosen on the
  unit-diagonal block are another generalized inverse (`F = 241` where the exact form reads 41.139, pass 2), and so is a
  meat factored on its unit diagonal, which lifts a column that is only rounding, a cluster dummy's, to the others'
  scale (`F = 11.84` where the exact form reads 9.7177, pass 9). Every filling is `FᵀF` for score rows `F`: the cluster
  sums, the rows `√ωᵢ·xᵢ` of HC0 to HC3, and for Newey–West each window of `L + 1` scores summed over `√(L + 1)`, since
  scores `l` apart share `L + 1 − l` windows, each summed afresh. So `V = AAᵀ` with `A = √c·B·Fᵀ`, cut to its rank by
  reflections that pivot on each restriction's share of itself left, so the cut is the same whatever a coefficient's
  scale, and `bᵀV⁺b = ‖A⁺b‖²`, a least-squares solution by reflections with Powell and Reid's pivoting over rows sorted
  by size. On 120 random cluster fits with columns scaled across `1e±5`, 53 with a cluster dummy, it is within `4.2e-13`
  of the rational answer on the 71 short of full rank and `6.3e-13` on the 49 of full rank; an HC0 fit with a dummy and
  a column at `1e12` beside one at 1 is within `1e-15` (Review A pass 12, where the bound's product of norms had read
  `NaN` and a zero pivot had cost four digits); statsmodels' `pinv` cut a real direction on all six fits checked against
  it (on the pass-2 design `F = 0.00588` on one degree of freedom against the exact 41.139 on two).
- **An undetermined form is `NaN`.** Each residual's rounding scales with `|eᵢ| + Σ|xᵢⱼbⱼ|`, so each score entry's
  with `|F|`, the same rows built from `|xᵢ|` and those sizes; with the rounding of the product `A = L·Fᵀ` itself, the
  form's first-order change is bounded by `2kε Σᵣ(|Fᵣ·Lᵀz|·|Fᵣ|·|L|ᵀ|z| + |Fᵣ·LᵀV⁺z|·|Fᵣ|·|L|ᵀ|b⊥|)`,
  `L = √c·B`, `z = V⁺b`, `b⊥` the part of `b` outside the range, summed row by row. Past `1e-6` of the form the F is
  `NaN`: on the pass-3 design the bound is 100 times the form and the exact 18.45, statsmodels 31.66 and the formed
  block 2.5e14 all differ; none of the 120 random fits reaches it. The product counts because two rows of `A` parallel
  in exact arithmetic are parallel in doubles only to `ε` of themselves, an error that grows with the square of the
  ratio of two columns' scales wherever a score row holds both. On a Newey–West fit with a dummy and a column nonzero
  on half the rows, a window across the halves holds both: the fitted F is `9e-11` and `6e-9` off at a ratio of `1e4`
  and `1e5` with 200 rows, the form read from exact parts `6e-7` off at `1e6`, and the F is `NaN` from `1e6` on, from
  `1e5` with 60 rows and three lags, where a lighter bound had answered `4.6e-8` off; HC0 on the same design, whose
  rows hold one of the two each, stays exact (passes 12 to 14).
- **The cap at `G − 1` holds even where `matrix_rank` reads more.** Rounding noise can lift the numerical rank past
  what the estimator allows; statsmodels then reads a negative F, or a quadratic form over all the constraints when
  `matrix_rank` and `pinv` disagree (F 13511.6 here against 6755.8 on the two-cluster test fixture), and this keeps
  the F of the true rank.

## Settled

- The finiteness check sits with the lengths in `LeastSquares.FiniteRows`, `InstrumentalVariables.Build`,
  `PanelRegression.Fit` (between `CheckShape` and `CheckOptions`) and `MultinomialLogit.Fit`, as the issues direct. It
  precedes options and degrees of freedom on purpose: a NaN is a fact about the data, refused before anything is
  computed from it, and the reference's data handling runs first too. Its refusal order is otherwise main's; shape
  refusals still come first, IV's under-identification among them, and every other block's length with them — cluster
  labels, IV's included (pass 8), weights, the GLS covariance, the GLM's offset and exposure — so a short block is named
  before a NaN in the design (Review A pass 7); the panel and the GLM's offset overload refuse it before their options.
- A GLM response keeps its family refusal; the design has its own finite check before it, in each public overload.
- `Stationarity.AugmentedDickeyFuller` reads the series times the power of two that brings it to unit scale: the
  statistic is invariant to scale, a power of two scales exactly, and so neither a level column far from the
  intercept's nor a difference that overflows is refused; its information criterion is read on the caller's scale.
- A clustered GMM weight is singular with fewer clusters than instruments, or as many in a just-identified model,
  whose first-step scores sum to zero; it is refused naming `clusters`, else `design`. A weight that is not finite,
  its scores having overflowed, is refused naming `design` (Review A pass 6). The weight's rank is capped as the
  covariances' are, at its non-zero score groups less one where 2SLS zeroes their total, just identified (pass 7).
- The panel's largest rank is counted from the cluster and period labels the meat already builds.
- `Lodestar.Stats.Regression` no longer depends on `Lodestar.Decomposition`, whose QR only the removed robust inverse
  called; its tests keep the reference QR they compare leverages against.
- A just-identified LIML reads `κ = 1` exactly, as its reference page states, after `LimlKappa` has run its refusals.
- A block holding a non-finite entry (an HC2/HC3 infinity from a leverage of one) takes main's path, unranked.
- A robust fit whose rows carrying a regressor are no more than its parameters interpolates them, so its covariance is
  zero in exact arithmetic and its F `NaN`, where rounding read 5.1e29 and statsmodels reads 1.7e28 (pass 8).
- `Lodestar.Stats`'s tails, the model tests' degrees of freedom and the panel's exact-fit `+∞` are unchanged.
- The bandwidth refusal is raised inside the covariance, after the option checks.

## Measured

- **Random fits**: 4,000 statsmodels fits (few clusters, HC0–HC3, HAC, weights, ill-conditioned and badly scaled
  designs) replayed against main and this branch. On the 608 fits statsmodels' `matrix_rank` calls rank-deficient
  under a cluster covariance, main disagrees with statsmodels on the F or its tail at `1e-7` on all 608; this branch on
  118: 99 are blocks statsmodels cut for scale (the decision above), 19 are blocks past a scaled condition number of
  `1e13`, where the reference's own answer is noise. Review A pass 3 moved a block of full rank to the inverse of
  its unit-diagonal factorisation; the frozen corpora hold at their tolerances.
- **Grid**: 6,949 lines over every public member, net10.0 and netstandard2.0 byte-identical.
- **Cost**, against origin/main in a plain warmed loop (best of four rounds), and the reference on the same data: the
  finiteness scan is half a nanosecond a cell, so `OrdinaryLeastSquares.Estimate` goes 780 → 931 ns at 300 cells and
  85.4 → 94.4 µs at 16,000; with the Wald block factored once, Hc1 at 2,000 rows and 8 regressors goes 282 → 289 µs and
  a 40-cluster fit 220 → 226 µs, the finiteness scan's share; the fits that do no Wald test stay within 3.2 %. Every
  member remains faster than its reference: `OLS.Fit` Hc1 2.5×, `IV.GMM` 1.4×, the panel estimators 17× to 330×;
  statsmodels has no `Estimate`.
