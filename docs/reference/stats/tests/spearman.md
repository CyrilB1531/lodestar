# Spearman

Spearman's rho: is there a monotone relationship between two variables?

[`Pearson`](pearson.md) on the mid-ranks rather than on the values, which is the whole
definition: it answers whether the two rise and fall together, not whether they do so along a
line, and a single outlier moves a rank by one place instead of moving a mean. The p-value is the
Student approximation on `n - 2` degrees of freedom at every sample size — scipy publishes no
exact branch for `spearmanr`, so neither is offered here.

## Members

| Member | What it does |
| --- | --- |
| [`Spearman.Test`](spearman-test.md) | Correlates the ranks of two paired samples. |
| [`Spearman.Matrix`](spearman-matrix.md) | Correlates every pair of variables at once. |
