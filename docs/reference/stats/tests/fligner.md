# Fligner

The Fligner-Killeen test: do several groups share one variance?

The third equality-of-variance test beside [`Levene`](levene.md) and [`Bartlett`](bartlett.md),
and the one that ranks: each group's absolute deviations from its centre are ranked together,
turned into normal scores, and compared across groups by a χ² statistic. Ranking is what makes it
hold up where the data are neither normal nor free of outliers.

## Members

| Member | What it does |
| --- | --- |
| [`Fligner.Test`](fligner-test.md) | Compares the spread of two or more groups. |
