# Changelog — Lodestar.Stats

What changed in `Lodestar.Stats`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `Distributions` publishes the density, the lower tail and the inverse upper tail of the standard normal, Student's t, F and chi-squared laws, and the F and chi-squared quantiles: fifteen members, twenty with the five already published, at `scipy.stats` parity. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))

### Changed

- `Distributions.StudentQuantile` and `NormalQuantile` answer `0` and `1` with the support's ends, as scipy's `ppf` does, where they refused them. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))

### Fixed

- The regularized incomplete beta keeps the digits of `1 − x` when `x` rounds to one, and forms `log B` from Stirling's series when one shape is large and the other is not, where it lost up to `6e-7` at shapes `(5e9, 0.5)`. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))
- An infinite `df` is the standard normal law for the Student members, as in scipy, and refused for the F and chi-squared ones, where it returned wrong values or NaN; a Student quantile past the largest double is `±∞` rather than `±double.MaxValue`. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))
- Student's upper tail stays exact past `|t| = 1.3e154`, where the square overflowed and the tail read zero, so `StudentQuantile` and `StudentSf` reach the Cauchy's `1e-300` point, `3.18e299`. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))

## [0.5.0] — 2026-09-24

### Added

- `Levene.Test`, `Bartlett.Test`, `Friedman.Test`, `Binomial.Test` and `AndersonDarling.Test` answer whether the assumptions the other families make hold, at `scipy.stats` parity, with Clopper-Pearson and Wilson intervals on the binomial result. ([#1121](https://github.com/CyrilB1531/lodestar/issues/1121))
- `Pearson.Test`, `Spearman.Test` and `KendallTau.Test` answer whether two variables are related, at `scipy.stats` parity, with a Fisher-z interval on the Pearson result and both Kendall variants. ([#1120](https://github.com/CyrilB1531/lodestar/issues/1120))
- `NanPolicy` on the eleven test entry points whose scipy counterpart takes `nan_policy`. ([#687](https://github.com/CyrilB1531/lodestar/issues/687), [`aaabaf72`](https://github.com/CyrilB1531/lodestar/commit/aaabaf72))

### Changed

- `Alternative`, `Center`, `Continuity`, `ExactMethod`, `KendallVariant`, `NanPolicy`, `ProportionInterval`, `Variance`, `ZeroMethod`, `TestResult`, `KsResult`, `Chi2ContingencyResult` and `AndersonResult` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `KruskalWallis.Test` past sixteen groups and the sorting `Wilcoxon` path read their tie terms off one ranking pass, and Durbin's Kolmogorov matrix power no longer allocates per product. ([#843](https://github.com/CyrilB1531/lodestar/issues/843))
- The exact Mann-Whitney distribution sizes its table by the smaller sample and reuses two buffers, where 8 against 2,500 allocated 3.35 GB. ([#814](https://github.com/CyrilB1531/lodestar/issues/814))
- The exact Kolmogorov-Smirnov table walk swaps two rows instead of allocating one per step. ([#830](https://github.com/CyrilB1531/lodestar/issues/830))
- `FisherExact.Test` and the equal-size exact `KolmogorovSmirnov.TwoSample` return the same p-values at a fraction of the cost. ([#756](https://github.com/CyrilB1531/lodestar/issues/756), [`e6323c08`](https://github.com/CyrilB1531/lodestar/commit/e6323c08))
- `KolmogorovSmirnov.TwoSample`'s default is exact for two samples of the same size up to 10,000 values each, as scipy's is. ([#802](https://github.com/CyrilB1531/lodestar/issues/802))
- `MannWhitney.Test` merges two sorted samples instead of sorting the pooled one, and no longer allocates. ([#711](https://github.com/CyrilB1531/lodestar/issues/711), [`940d978b`](https://github.com/CyrilB1531/lodestar/commit/940d978b))
- `KruskalWallis.Test` and `Wilcoxon` merge sorted values the same way. ([#719](https://github.com/CyrilB1531/lodestar/issues/719), [`7ccec349`](https://github.com/CyrilB1531/lodestar/commit/7ccec349))
- The chi-squared and normal tails no longer iterate on every call. ([#710](https://github.com/CyrilB1531/lodestar/issues/710), [`3006be3c`](https://github.com/CyrilB1531/lodestar/commit/3006be3c))
- `Distributions.NormalQuantile` and `Distributions.StudentQuantile` invert their tail by Newton instead of by bisection. ([#709](https://github.com/CyrilB1531/lodestar/issues/709), [`adb2d62c`](https://github.com/CyrilB1531/lodestar/commit/adb2d62c))
- `Chi2ContingencyResult` compares its expected table by value. ([#668](https://github.com/CyrilB1531/lodestar/issues/668), [`a2b11493`](https://github.com/CyrilB1531/lodestar/commit/a2b11493))

### Fixed

- `Wilcoxon`'s documentation says a one-sided test returns the positive rank sum, as it and scipy do, rather than the smaller sum. ([#875](https://github.com/CyrilB1531/lodestar/issues/875))
- `ShapiroWilk.Test` at three observations uses AS R94's fixed weights and caps W at 1, where it returned NaN. ([#863](https://github.com/CyrilB1531/lodestar/issues/863))
- `Distributions.ChiSquaredSf` and `Distributions.FisherSf` are no longer wrong by up to half a unit at large degrees of freedom near the mean. ([#837](https://github.com/CyrilB1531/lodestar/issues/837))
- `Distributions.StudentSf` and `Distributions.FisherSf` are no longer off by 4e-7 relative at 2e8 degrees of freedom, and more past it, when one shape of the incomplete beta is large and the other small. ([#841](https://github.com/CyrilB1531/lodestar/issues/841))
- `MannWhitney.Test` no longer returns a wrong statistic past about 46,340 values per sample. ([#712](https://github.com/CyrilB1531/lodestar/issues/712), [`cfe9f048`](https://github.com/CyrilB1531/lodestar/commit/cfe9f048))

## [0.4.0] — 2026-09-10

`Lodestar.Stats` appears three times on one date, and that is
[decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
rule in practice rather than three changes of mind: it published four members and wrote
that publishing later is always available, so the chi-squared tail (0.3.0) and the normal
quantile (0.4.0) each went out when a caller asked for it. `src/` reaches its neighbours
through published packages, so each of those numbers is what stood between a caller and its
floor — 0.2.0 for `Lodestar.Stats.Regression`, 0.4.0 for `Lodestar.Survival`.

### Added

- **`Distributions.NormalQuantile` joins them, and `Lodestar.Stats` reaches 0.4.0.** [#569](https://github.com/CyrilB1531/lodestar/issues/569)'s Kaplan-Meier confidence bounds are built on the log-log transform of the estimate, and their multiplier is `scipy.stats.norm.ppf`. The obvious way to avoid publishing anything — a Student quantile at a very large degrees of freedom, which is already public — was tried and **measured**: its accuracy has an optimum near 1e8 degrees of freedom and a floor around **9e-9**, because below that the convergence to the normal is incomplete and above it the bisection loses more than it gains. The transform amplifies that into the seventh digit of a bound, past the `1e-9` the corpora compare at, and the frozen corpus is what caught it. This member answers to about `1e-15`. Two details carry over from publishing `StudentQuantile`: it is the **quantile, not the inverse survival function**, so the published member negates the internal helper — symmetry, not a correction — and the median returns **positive** zero rather than `-0`. A test pins the substitute's failure alongside the member's success, so the measurement cannot rot silently. Everything else in the numerical layer stays internal ([decision 0098](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)). ([#569](https://github.com/CyrilB1531/lodestar/issues/569))

## [0.3.0] — 2026-09-10

### Added

- **`Distributions.ChiSquaredSf` joins the four tails `Lodestar.Stats` already publishes.** [Decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) published four members and wrote that *publishing later is always available*; [#569](https://github.com/CyrilB1531/lodestar/issues/569)'s log-rank test is the second caller to ask, so [decision 0097](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0097-the-chi-squared-tail-joins-the-published-four.md) applies that rule rather than amending it. The member is the same tail `ChiSquare.GoodnessOfFit` and `ChiSquare.Contingency` already report, so a statistic routed either way gives the same p-value to the last bit — asserted by a test, and the reason re-deriving a tail inside the new package was refused on [decision 0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md)'s own ground. Unlike the three before it, **`x` is not validated**: the distribution has no mass below zero, so a non-positive statistic returns one instead of surfacing an internal helper's parameter name from a public method. Six cases join `tests/oracles/stats_distributions.json`, reaching `7.7e-26` and compared relatively. Everything else in the numerical layer stays internal. ([#569](https://github.com/CyrilB1531/lodestar/issues/569))

## [0.2.0] — 2026-09-10

### Added

- **`Distributions` publishes three tails, and no more.** `StudentSf`, `StudentQuantile` and `FisherSf` are what an OLS table needs — a p-value per coefficient, a multiplier per interval, and the overall *F* — and they become public so `Lodestar.Stats.Regression` can reach them rather than carry a second copy. [Decision 0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md) kept this layer internal and named the condition for changing that; [decision 0095](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) exercises it and holds the line at four members: the log-gamma, the incomplete beta and gamma, the normal tail and the Kolmogorov machinery stay internal. **Paying 0081's stated price found a defect.** It asked for "a corpus at the tolerance a general-purpose caller would need, not the one the ten tests above it happen to need" — and that corpus, reaching `3.1e-24` and compared relatively, showed that the internal `Beta.StudentQuantile` solves `P(T > x) = p` and therefore carries the **opposite sign to a quantile**. Nothing internal noticed, because its one caller wanted exactly that; published unchanged it would have handed a reader `-2.1788` where every table prints `+2.1788`, and an interval built on it would have been reflected through its own estimate. `Distributions.StudentQuantile` publishes `scipy.stats.t.ppf`'s meaning, and the negation is the distribution's symmetry rather than a correction. ([#566](https://github.com/CyrilB1531/lodestar/issues/566))

## [0.1.0] — 2026-09-08

### Added

- **`Lodestar.Stats` is a new package: ten families of classical hypothesis

  test at `scipy.stats` 1.18.0 parity.** Student and Welch *t*, Mann-Whitney
  *U*, Wilcoxon signed-rank, χ² goodness-of-fit and contingency, Fisher exact,
  two-sample Kolmogorov-Smirnov, one-way ANOVA, Kruskal-Wallis, Shapiro-Wilk,
  and the Bonferroni, Benjamini-Hochberg and Benjamini-Yekutieli corrections.
  Core tier: the tail probabilities come from the package's own internal
  log-gamma, incomplete beta and incomplete gamma rather than from a numerical
  dependency. `TTest.Independent` defaults to Welch where `scipy` defaults to
  Student, which is the one deliberate divergence and has its row in
  [`docs/equivalence.md`](../../docs/equivalence.md). Ten frozen corpora, each case
  carrying the full argument set it was generated with, and p-values compared
  at `1e-9` **relative** — ordinary cases reach `2.38e-53`, where the
  repository's absolute tolerance would accept a zero.
  ([#442](https://github.com/CyrilB1531/lodestar/issues/442), decision
  [0081](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0081-the-stats-numerical-layer-stays-internal.md))
