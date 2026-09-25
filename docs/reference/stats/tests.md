# Hypothesis tests — `Lodestar.Stats`

Eighteen families of classical hypothesis test, at `scipy.stats` 1.18.1 parity.
Arrays in, a statistic and a p-value out; nothing is fitted, so every entry
point is static.

| test | what it asks | entry point |
| --- | --- | --- |
| Student / Welch *t* | do two samples have the same mean? | [`TTest`](tests/ttest.md) |
| Mann-Whitney *U* | the same question, assuming no shape | [`MannWhitney`](tests/mannwhitney.md) |
| Wilcoxon signed-rank | the same, on paired measurements | [`Wilcoxon`](tests/wilcoxon.md) |
| χ² | do counts match an expected distribution, or are two factors independent? | [`ChiSquare`](tests/chisquare.md) |
| Fisher exact | the same for a 2×2 table, at any sample size | [`FisherExact`](tests/fisherexact.md) |
| Kolmogorov-Smirnov | do two samples share a distribution? | [`KolmogorovSmirnov`](tests/kolmogorovsmirnov.md) |
| one-way ANOVA | do several groups share one mean? | [`OneWayAnova`](tests/onewayanova.md) |
| Kruskal-Wallis | the same, assuming no shape | [`KruskalWallis`](tests/kruskalwallis.md) |
| Shapiro-Wilk | could this sample be normal? | [`ShapiroWilk`](tests/shapirowilk.md) |
| Bonferroni / BH / BY | how many of these results are chance? | [`MultipleComparisons`](tests/multiplecomparisons.md) |
| Pearson *r* | are these two variables related, along a line? | [`Pearson`](tests/pearson.md) |
| Spearman rho | the same, along any rising or falling curve | [`Spearman`](tests/spearman.md) |
| point-biserial | how a variable differs between the two sides of a binary one | [`PointBiserial`](tests/pointbiserial.md) |
| Kendall tau | the same, counted pair by pair | [`KendallTau`](tests/kendalltau.md) |
| Levene | do several groups share one variance? | [`Levene`](tests/levene.md) |
| Bartlett | the same, assuming each group is normal | [`Bartlett`](tests/bartlett.md) |
| Fligner-Killeen | the same, by ranks | [`Fligner`](tests/fligner.md) |
| Friedman | do several treatments differ on the same subjects? | [`Friedman`](tests/friedman.md) |
| binomial | is this proportion what it was said to be? | [`Binomial`](tests/binomial.md) |
| Anderson-Darling | could this sample be normal, or do several share a distribution? | [`AndersonDarling`](tests/andersondarling.md) |

Every family but six returns the same two numbers, [`TestResult`](tests/testresult.md); a
t-test also carries its degrees of freedom ([`TTestResult`](tests/ttestresult.md)), a
contingency table also carries the table independence would have produced
([`Chi2ContingencyResult`](tests/chi2contingencyresult.md)), Kolmogorov-Smirnov also carries
where and in which direction the two samples parted furthest ([`KsResult`](tests/ksresult.md)),
a Pearson correlation and a binomial test each carry what their intervals need
([`PearsonResult`](tests/pearsonresult.md), [`BinomialResult`](tests/binomialresult.md)), and
Anderson-Darling carries the table its statistic is read against
([`AndersonResult`](tests/andersonresult.md)). Spearman's rho over every pair of variables returns
both matrices ([`CorrelationMatrix`](tests/correlationmatrix.md)).

| result | carries | returned by |
| --- | --- | --- |
| [`TestResult`](tests/testresult.md) | a statistic, a p-value | fourteen of the twenty families |
| [`TTestResult`](tests/ttestresult.md) | + degrees of freedom, a confidence interval | [`TTest`](tests/ttest.md) |
| [`Chi2ContingencyResult`](tests/chi2contingencyresult.md) | + degrees of freedom, the expected table | [`ChiSquare.Contingency`](tests/chisquare-contingency.md) |
| [`KsResult`](tests/ksresult.md) | + where the gap was reached, and its sign | [`KolmogorovSmirnov`](tests/kolmogorovsmirnov.md) |
| [`PearsonResult`](tests/pearsonresult.md) | + a confidence interval for the correlation | [`Pearson`](tests/pearson.md) |
| [`BinomialResult`](tests/binomialresult.md) | + a confidence interval for the proportion | [`Binomial`](tests/binomial.md) |
| [`AndersonResult`](tests/andersonresult.md) | + the critical values and their significance levels | [`AndersonDarling`](tests/andersondarling.md) |
| [`CorrelationMatrix`](tests/correlationmatrix.md) | a statistic and a p-value for every pair of variables | [`Spearman.Matrix`](tests/spearman-matrix.md) |

Nine small enums choose what a test asks, and how it answers when the exact and the approximate
route disagree.

| option | chooses |
| --- | --- |
| [`Alternative`](tests/alternative.md) | which tail the p-value covers |
| [`Variance`](tests/variance.md) | whether an independent-samples *t*-test pools the two variances |
| [`Continuity`](tests/continuity.md) | whether a discrete statistic's normal approximation gets the half-unit correction |
| [`ExactMethod`](tests/exactmethod.md) | the exact null distribution, its normal approximation, or a choice between them |
| [`ZeroMethod`](tests/zeromethod.md) | what Wilcoxon does with a pair whose difference is exactly zero |
| [`KendallVariant`](tests/kendallvariant.md) | which normalisation Kendall's tau carries; the p-value is the same either way |
| [`Center`](tests/center.md) | which centre Levene's and Fligner-Killeen's tests measure each group's spread around |
| [`AndersonKSampleVariant`](tests/andersonksamplevariant.md) | which form of the k-sample Anderson-Darling statistic is computed |
| [`ProportionInterval`](tests/proportioninterval.md) | which interval a binomial proportion is reported with |

The [hypothesis-testing guide](../../guides/hypothesis-testing.md) says which
test answers which question, and the
[Python equivalence table](../../equivalence.md) maps each `scipy` call to its
counterpart here.
