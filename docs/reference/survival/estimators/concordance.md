# Concordance

Harrell's concordance index on any predicted scores: lifelines' `utils.concordance_index`.

<!-- docs-declaration -->

```csharp
public static class Concordance
```

**Example** — scores that order four of five pairs correctly.

```csharp
using Lodestar.Survival;

double c = Concordance.Index([1, 2, 3, 4, 5], [2, 1, 3, 5, 4]);   // => 0.8
```

**Remarks** — the index [`CoxSummary`](coxsummary.md) reports for its own fit, opened to any score: a
survival model's prediction, a risk score negated, a biomarker. Reference behaviour is
`lifelines.utils.concordance_index` 0.30.3.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`CoxSummary`](coxsummary.md).

## Members

| Member | What it does |
| --- | --- |
| [`Concordance.Index`](concordance-index.md) | The share of comparable pairs the scores order correctly. |
