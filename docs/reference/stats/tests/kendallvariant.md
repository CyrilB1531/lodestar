# KendallVariant

Which normalisation Kendall's tau divides the concordance excess by.

<!-- docs-declaration -->

```csharp
public enum KendallVariant { TauB, TauC }
```

**Members** — `TauB` divides by the tied-pair counts of each sample separately; scipy's `'b'`,
and the default there too. `TauC` is Stuart's tau-c, scaled by the smaller number of distinct
values, scipy's `'c'`.

**Example** — two judges ranking eight entries, with ties on both cards: the same count,
normalised twice.

```csharp
using Lodestar.Stats;

double[] judgeA = [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0];
double[] judgeB = [1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0, 5.0];

TestResult b = KendallTau.Test(judgeA, judgeB);
TestResult c = KendallTau.Test(judgeA, judgeB, variant: KendallVariant.TauC);

double tauB = Math.Round(b.Statistic, 4);   // => 0.8573
double tauC = Math.Round(c.Statistic, 4);   // => 0.875
```

**Remarks — the p-value is the same either way.** Both variants normalise the same quantity, the
excess of concordant pairs over discordant ones, and that excess is what the null distribution is
read against; only the divisor differs. The two statistics above sit either side of each other,
and both come with `0.006153`.

```csharp
using Lodestar.Stats;

double[] judgeA = [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0];
double[] judgeB = [1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0, 5.0];

TestResult b = KendallTau.Test(judgeA, judgeB);
TestResult c = KendallTau.Test(judgeA, judgeB, variant: KendallVariant.TauC);

double shared = Math.Round(c.PValue - b.PValue, 12);   // => 0
```

Which to report is a question about the table, not about the data. Tau-b cannot reach `1` when
the two samples hold different numbers of distinct values, because its divisor counts the ties it
could not have ordered; tau-c rescales by the smaller of those counts so that a table with
unequal margins can still reach the ends of the range. On an untied sample the two are equal, and
both equal Kendall's original tau-a — which is why no third member exists here, and none does in
scipy either.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KendallTau.Test`](kendalltau-test.md), [`ExactMethod`](exactmethod.md), the
[Python equivalence table](../../../equivalence.md).
