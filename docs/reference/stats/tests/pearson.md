# Pearson

The Pearson correlation: is there a linear relationship between two variables?

The parametric member of the three correlation tests, and the only one that measures a *linear*
relationship rather than a monotone one — [`Spearman`](spearman.md) and
[`KendallTau`](kendalltau.md) rank first and so answer a weaker question about stronger data.
The null it tests is that the two samples are uncorrelated and normally distributed, which is
where the normality assumption enters; on a heavy-tailed sample the rank tests are the safer
reading.

## Members

| Member | What it does |
| --- | --- |
| [`Pearson.Test`](pearson-test.md) | Correlates two paired samples. |
