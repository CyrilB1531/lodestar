# Changelog — Lodestar.Stats

What changed in `Lodestar.Stats`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `Distributions` publishes the density, the lower tail and the inverse upper tail of the standard normal, Student's t, F and chi-squared laws, and the F and chi-squared quantiles: fifteen members, twenty with the five already published, at `scipy.stats` parity. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))
- `Fligner.Test`, `AndersonDarling.KSample`, `Spearman.Matrix` and `PointBiserial.Test` add the Fligner-Killeen test, the k-sample Anderson-Darling test, Spearman's correlation matrix and the point-biserial correlation at `scipy.stats` parity. ([#1162](https://github.com/CyrilB1531/lodestar/issues/1162))

### Changed

- `OneWayAnova.Test` answers `NaN` for groups of one value each and for a null or empty group, where it threw. ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `KruskalWallis.Test`, `Levene.Test` and `Bartlett.Test` answer `NaN` for a null or empty group, given or left by omission, where they threw. ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `KruskalWallis.Test` answers `NaN` for a fully tied pooled sample, and `TTest`, `MannWhitney.Test` and `KolmogorovSmirnov.TwoSample` for an empty sample, where they threw (decision 0010). ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `Bartlett.Test` answers `NaN` for a group of one value, `Friedman.Test` for no block and `ChiSquared.GoodnessOfFit` a `NaN` p-value for no category, given or left by omission, where they threw (decision 0010). ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- Every test refuses an option outside its enumeration — a `NanPolicy`, `Alternative`, `Variance`, `Continuity`, `ExactMethod`, `ZeroMethod`, `KendallVariant`, `Center` or `ProportionInterval` — on every input, once the input has met its own refusals, where some computed with it as another value and others let an empty or `NaN` sample answer for it. ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `Pearson.Test` answers `NaN` below two pairs, `AndersonDarling.Test` for an empty sample, `AndersonDarling.KSample` for an empty sample or a pool of one distinct value, `ChiSquared.Contingency` for a zero marginal, where they threw, and `ChiSquared.GoodnessOfFit` reads a zero expectation as scipy does, `+∞` where a count meets it and `NaN` at `0/0`, where it threw (decision 0010). ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `Wilcoxon.Paired` and `Wilcoxon.OneSample` answer `NaN` for empty samples, given or left by omission, where they threw. ([#1712](https://github.com/CyrilB1531/lodestar/issues/1712))
- `MannWhitney.Test` and `Wilcoxon` answer a `NaN` p-value where their normal approximation has no variance — a pool tied throughout, two-sided with the continuity correction, for Mann-Whitney; every difference zero under Wilcox's or Pratt's zero method, on any side, for Wilcoxon — as scipy's do, where with the continuity correction, or every Wilcox difference zero, they answered 1. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `Levene.Test` answers `NaN` for a propagated `NaN` whatever the trim's size, where an out-of-range trim was refused, and `Fligner.Test` for deviations that all tie, where it answered a rounding error. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `MultipleComparisons` adjusts an empty family to an empty one, where it threw. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `KolmogorovSmirnov.TwoSample` answers a `NaN` p-value for one value against one, two-sided and asymptotic, as scipy's `kstwo` does, where it answered `0` or `1`. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `ChiSquared.Contingency` answers a table of one row or one column with no zero total with a statistic of `0` and a p-value of `1` on no degree of freedom, where it threw. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `Binomial.Test` answers a two-sided p-value of `0` for a count impossible at `int.MaxValue` trials, where an overflow read 1, and its interval at zero trials keeps the limit a one-sided test fixes. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `Spearman.Matrix` under `NanPolicy.Omit` re-ranks only the pairs holding a `NaN`. ([#1717](https://github.com/CyrilB1531/lodestar/issues/1717))
- `Friedman.Test` ranks every block in one set of scratch arrays. ([#1718](https://github.com/CyrilB1531/lodestar/issues/1718))
- `Distributions.StudentQuantile` and `StudentIsf` take `log B(df/2, ½)` once per quantile rather than once per Newton step. ([#1686](https://github.com/CyrilB1531/lodestar/issues/1686))
- `Distributions.ChiSquaredQuantile`, `ChiSquaredIsf`, `FisherSnedecorQuantile` and `FisherSnedecorIsf` settle on a converged root instead of restarting their bracket's search. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.StudentQuantile` and `StudentIsf` take the density's constant from the cached `log B`, which can move a quantile by a few ulps. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.FisherSnedecorQuantile`, `FisherSnedecorIsf` and `BinomialResult.ProportionConfidenceInterval` take `log(1 − x)` from one helper, which can move a value by up to about 60 ulps. ([#1725](https://github.com/CyrilB1531/lodestar/issues/1725))
- `log1p` comes from the one implementation `Lodestar.Preprocessing`, `Lodestar.Stats`, `Lodestar.Survival` and `Lodestar.Metrics` share, which can move a value by an ulp. ([#1230](https://github.com/CyrilB1531/lodestar/issues/1230))
- `Distributions.NormalQuantile` and `NormalIsf` are scipy's `ndtri`, Cephes' rational approximations ported statement for statement, so they match `norm.ppf` and `norm.isf` to the last bit on Linux, and to `1e-14` relative elsewhere, where inverting the tail by Newton strayed up to 5.5e-13 near one half, and `ShapiroWilk.Test`'s Blom scores and the chi-squared quantiles' starting point take it too, which can move their last bits. ([#1684](https://github.com/CyrilB1531/lodestar/issues/1684))
- `Distributions.StudentQuantile` and `NormalQuantile` answer `0` and `1` with the support's ends, as scipy's `ppf` does, where they refused them. ([#1158](https://github.com/CyrilB1531/lodestar/issues/1158))
- `TTestResult.Df` and `Chi2ContingencyResult.Dof` are `DegreesOfFreedom`, `ChiSquare` and `Chi2ContingencyResult` are `ChiSquared` and `ChiSquaredContingencyResult`, and the F law's `Distributions.Fisher*` members are `FisherSnedecor*`, one spelling each before 1.0. ([#1217](https://github.com/CyrilB1531/lodestar/issues/1217))
- The chi-squared and F members answer at a df of `5e-324`, whose half rounds to zero, where they threw from a private parameter. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690), [#1729](https://github.com/CyrilB1531/lodestar/issues/1729))

### Fixed

- `Distributions.FisherSnedecorSf(1e-318, 20, 1e-320)` reads one, where it read `0.99994`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorPdf`, `FisherSnedecorCdf`, `FisherSnedecorSf`, `FisherSnedecorQuantile`, `FisherSnedecorIsf`, `ChiSquaredPdf`, `ChiSquaredCdf`, `ChiSquaredSf`, `ChiSquaredQuantile` and `ChiSquaredIsf` take a df at its exact half where that half is not a double, so `Distributions.FisherSnedecorCdf(1e-5, 1e-310, 1.5e-323)` reads `1.48e-13`, where it read a third more. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.ChiSquaredCdf` and `ChiSquaredSf` no longer halve a statistic below `2⁻¹⁰²¹` before taking its tails, nor `ChiSquaredQuantile` and `ChiSquaredIsf` a root there, so `ChiSquaredSf(5e-324, 2e-300)` reads `7.4e-298`, where it read one. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The chi-squared members and `Binomial.Test` keep their digits at a small statistic or probability: `Distributions.ChiSquaredCdf(1e-300, 2)` reads `5e-301`, and `Binomial.Test(1, 1000, 1e-300)` a p-value of `1e-297`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The chi-squared density and lower tail at an integer df below 20, and the members reaching the same gamma function, take 1/Γ of a half-integer correctly rounded. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorSf` and `FisherSnedecorCdf` keep a small df's digits in every term of the large-shape expansion, so `FisherSnedecorSf(2e20, 1e-20, 2000)` is `1.0987596822392122e-21`, where it read `1.0987598357523485e-21`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorSf` and `FisherSnedecorCdf` at two dfs below `0.2`, the second far the smaller, keep their digits. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorSf`, `FisherSnedecorCdf` and the Student tails read one less the other side's series below a shape of `1e-8` at most half the other, so `FisherSnedecorSf(1e-300, 3.7, 1e-300)` reads one and `StudentSf(0.004855995613680737, 7.543351270323165e-12)` `0.49999999996918243`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorPdf` and the F tails take `y` from the odds `d₁f/d₂` where `d₁f` underflows to zero, so `FisherSnedecorPdf(8.4e-323, 0.003562409783441407, 1e-300)` is `5.383368601372557e21`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorPdf` at a shape below `1e-3` or far in its tails, and the F tails at one shape below `0.1`, keep their digits, so `FisherSnedecorPdf(0.5, 1e-25, 1e-47)` is `1e-47` and `FisherSnedecorPdf(1e10, 0.002, 30)` `4.393098157787252e-101`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorPdf`, `FisherSnedecorCdf` and `FisherSnedecorSf` keep their digits where `d₁f/d₂` or its inverse is below the doubles, so `FisherSnedecorCdf(1e-310, 1, 3)` is `7.351051938957217e-156`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorPdf` at a df just above `2⁻¹⁰²¹` keeps its digits, so `FisherSnedecorPdf(1e-300, 4.46e-308, 3)` is `2.23e-8`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorQuantile` and `FisherSnedecorIsf` where one half of the split is below the doubles answer a root whose tail is the target, as `FisherSnedecorQuantile(3.5e-318, 1, 1e-320)` does. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorQuantile` and `FisherSnedecorIsf` at two tiny dfs judge the support's ends by the side of the target the tail is on, so a target the flat tail meets at the smallest double has its root at 0, where it answered `+∞`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.ChiSquaredSf(0.034145041613094756, 19)` and `ChiSquaredSf(0.11059961137646561, 19)` read one, where they read `1.0000000000000007` and `0.9999999999999986`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.ChiSquaredQuantile` and `ChiSquaredIsf` at a subnormal probability keep their digits, so `ChiSquaredIsf(5e-324, 1)` is `1481.1266547553564`, where it read `1481.4857237250408`. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The correlation tests', the t-tests' and the binomial test's confidence intervals keep their digits at a level near one, which moves an ordinary level's by a few ulps toward the exact quantile, the t-tests' and the exact binomial's at a one-sided level near zero too, where the first threw and the second read (0, 0), and `Distributions.ChiSquaredQuantile` and `ChiSquaredIsf` near one at a subnormal root are the other side's at one less it. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- `Distributions.FisherSnedecorCdf`, `FisherSnedecorSf` and `FisherSnedecorPdf` keep their digits at an f far below one at shapes under 10, and `ChiSquaredPdf` and `ChiSquaredCdf` at a shape under one half and a tiny statistic. ([#1729](https://github.com/CyrilB1531/lodestar/issues/1729))
- The F law takes y from `numeratorDf · f / denominatorDf` where `numeratorDf · f` is subnormal, so `Distributions.FisherSnedecorPdf`, `FisherSnedecorCdf` and `FisherSnedecorSf` keep their digits there, where they lost them. ([#1725](https://github.com/CyrilB1531/lodestar/issues/1725))
- `Distributions.FisherSnedecorIsf` answers `+∞` at a subnormal numerator df where the root lies past the largest double, where it threw. ([#1704](https://github.com/CyrilB1531/lodestar/issues/1704))
- `Distributions.FisherSnedecorSf` and `FisherSnedecorCdf` answer `0` and `1` at an infinite `f` where `denominatorDf / numeratorDf` overflows, where they threw. ([#1705](https://github.com/CyrilB1531/lodestar/issues/1705))
- `ShapiroWilk.Test` shifts the sample by the value at its middle position as scipy's `shapiro` does, so a near-constant sample keeps its digits and a constant one whose mean rounded is refused, where it answered `W = 0`. ([#1706](https://github.com/CyrilB1531/lodestar/issues/1706))
- `ShapiroWilk.Test` follows Royston's AS R94: it refuses a range below `1e-19`, divides by an extreme range so no square overflows, computes past 5000 values, where it refused, and answers `NaN` for a NaN or infinite range. ([#1706](https://github.com/CyrilB1531/lodestar/issues/1706))
- `ShapiroWilk.Test` at three values clips W to its bound of `3/4`, as scipy does, and takes its p-value as scipy writes it, `1 − 6/π acos(√W)`, where 15-digit constants read `[0, 1e-8, 1]` `2.8e-7` off and `[0, 0, 1]` as `0`. ([#1706](https://github.com/CyrilB1531/lodestar/issues/1706))
- `Fligner.Test` answers `NaN` where an infinite value meets an infinite centre, as scipy's does. ([#1707](https://github.com/CyrilB1531/lodestar/issues/1707))
- `Distributions.ChiSquaredPdf` and `FisherSnedecorPdf` keep their digits at a tiny argument, where they read `0`. ([#1708](https://github.com/CyrilB1531/lodestar/issues/1708))
- `Distributions.ChiSquaredPdf` and `FisherSnedecorPdf` answer zero at `+∞` at every df, the density's limit, where they answered `NaN` past two (decision 0010). ([#1708](https://github.com/CyrilB1531/lodestar/issues/1708))
- `ChiSquared.GoodnessOfFit` answers a `NaN` p-value for a negative statistic, which negative counts can give, where it threw. ([#1709](https://github.com/CyrilB1531/lodestar/issues/1709))
- `ChiSquared.Contingency` answers `NaN` where finite cells of two rows and two columns or more overflow the marginals, where it threw. ([#1710](https://github.com/CyrilB1531/lodestar/issues/1710))
- The Pearson confidence interval holds its digits on netstandard2.0 near `r = 0`, where its Fisher transform was `8e-8` off. ([#1711](https://github.com/CyrilB1531/lodestar/issues/1711))
- `TTest.Independent`, its confidence interval and every internal Student tail read an infinite df as the normal law, where they threw. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The Student members take the normal law from df `1e20` and Student's first order in df to df `1e-12`, and answer `±∞` where a root lies past the largest double. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.StudentQuantile` and `StudentIsf` hold `scipy.stats.t` from df `1e16` up. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.StudentSf` and `StudentCdf` never read a tail above one half at a positive `t`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.StudentQuantile(0.5, df)` answers `+0`, as `scipy.stats.t.ppf` does. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.StudentPdf` answers at a subnormal df and past an overflowing `t²/df`, and is the normal density from df `1e20`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F tails and every incomplete beta behind them stay inside `[0, 1]` and keep their digits below a shape of `0.1`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The chi-squared tails and density keep their digits below a shape of `0.1`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F members take the tail, density and root from a half's limit where that half of `d₁f / (d₁f + d₂)` is below the smallest normal double. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.FisherSnedecorQuantile` and `FisherSnedecorIsf` answer `0` or `+∞` where the root lies outside the doubles. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.FisherSnedecorQuantile` and `FisherSnedecorIsf` answer a finite root at a denominator df from `1e200`, where they answered `+∞`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F members hold their symmetry and density at subnormal shapes. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.ChiSquaredIsf` and `ChiSquaredQuantile` answer `0` where the root lies below the smallest double. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `TTestResult.ConfidenceInterval` answers the estimate on both bounds for a zero standard error at a df whose quantile is infinite. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F tails take Temme's uniform expansion once both shapes reach `1e5`, where the continued fraction ran out of terms. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The incomplete beta past shapes of 10 takes its prefactor against the exact mean `a/(a+b)`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F and chi-squared quantiles hold their root at a huge df. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.ChiSquaredIsf` and `ChiSquaredQuantile` answer the root at a tiny df and a far tail, where they answered a non-root. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F tail and quantile with a tiny df beside a huge one answer the tail and the root, where they answered a wrong tail and `0`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.FisherSnedecorSf(1, df, df)` is exactly one half at a huge df. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- The F tails with one df past `1e154` answer their tail, where they answered `NaN`. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.NormalSf` and Student's tail from df `1e20` stay at or below one half at a tiny positive argument. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Distributions.NormalSf`, `NormalCdf` and Student's tail from df `1e20` keep the exponent of the normal far tail exact. ([#1690](https://github.com/CyrilB1531/lodestar/issues/1690))
- `Spearman.Matrix` refuses a variable count whose square is more cells than one array holds, before splitting out the columns and after a raised NaN, where the product wrapped or failed to allocate. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- `FisherExact.Test` counts a table as extreme within scipy's `1 + 1e-14` margin rather than `binomtest`'s `1 + 1e-7`, which gave `[[20, 38], [40, 106]]` a p-value of 0.394 where scipy has 0.314. ([#1243](https://github.com/CyrilB1531/lodestar/issues/1243))
- `OneWayAnova.Test` recognises constant groups by their values, as scipy does, so three copies of `0.1` against themselves answer `(NaN, NaN)` rather than `F = 16`, and groups each constant answer `(+∞, 0)`. ([#1244](https://github.com/CyrilB1531/lodestar/issues/1244))
- `KruskalWallis.Test` answers a NaN p-value when H rounds below zero, as scipy does, where it threw `ArgumentOutOfRangeException`. ([#1245](https://github.com/CyrilB1531/lodestar/issues/1245))
- `Pearson.Test` and `PointBiserial.Test` recognise a constant sample by its values, as scipy does, so six copies of `0.1` answer `(NaN, NaN)` rather than `r = 0`. ([#1246](https://github.com/CyrilB1531/lodestar/issues/1246))
- `Binomial.Test` computes each outcome's probability in Loader's saddle-point form, whose rounding past 1e8 trials stays under the two-sided margin where three log-gammas' did not, so `Test(49_999_999, 100_000_000)` answers 1 where it answered 0.99992. ([#1247](https://github.com/CyrilB1531/lodestar/issues/1247))
- `ChiSquared.GoodnessOfFit` accepts expectations summing to the observed total within scipy's `sqrt(eps)` of the smaller total, where it refused past `1e-8` of the observed one. ([#1248](https://github.com/CyrilB1531/lodestar/issues/1248))
- The t-tests answer a one-value sample, `ChiSquared.GoodnessOfFit` one category, `Binomial.Test` zero trials, `AndersonDarling.Test` one value or a constant sample, and `ChiSquaredPdf`/`FisherSnedecorPdf` at `+∞` past two degrees of freedom as scipy does, where they threw or answered zero; Levene's and Fligner's tests refuse a NaN `proportionToCut` or a cut below zero, and answer NaN for one that trims a group to nothing. ([#1217](https://github.com/CyrilB1531/lodestar/issues/1217))
- Welch's `TTest.Independent` on two constant samples takes one degree of freedom, as scipy does, where the 0/0 threw from the log-gamma; a propagated NaN makes the degrees of freedom and the interval NaN, and a one-sided interval's open bound is NaN where the standard error is zero, as scipy's are. ([#1215](https://github.com/CyrilB1531/lodestar/issues/1215))
- `AndersonDarling.Test` answers a NaN p-value for a sample holding a NaN or an infinity, as scipy does, where the table's interpolation fell through to 0.01. ([#1216](https://github.com/CyrilB1531/lodestar/issues/1216))
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
