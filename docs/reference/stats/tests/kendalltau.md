# KendallTau

Kendall's tau: do two rankings agree, pair by pair?

Where [`Spearman`](spearman.md) correlates the ranks themselves, this counts the pairs the two
samples order the same way against the pairs they order oppositely, so it reads as a probability
rather than as a correlation and is the steadier of the two on a short sample. Both
normalisations scipy publishes are offered — see [`KendallVariant`](kendallvariant.md) — and they
share a p-value, because the count they normalise is the same count.

## Members

| Member | What it does |
| --- | --- |
| [`KendallTau.Test`](kendalltau-test.md) | Correlates two paired samples by counting concordant and discordant pairs. |
