# Friedman

The Friedman test: do several treatments differ, measured on the same subjects?

What [`Wilcoxon`](wilcoxon.md) is to [`MannWhitney`](mannwhitney.md), this is to
[`KruskalWallis`](kruskalwallis.md): the same rank-based comparison of several groups, but with
the groups measured on one set of subjects rather than on independent ones. Each subject is ranked
against itself, which removes whatever made that subject high or low to begin with — the reason it
sees differences a between-subjects test would lose in the noise.

## Members

| Member | What it does |
| --- | --- |
| [`Friedman.Test`](friedman-test.md) | Compares three or more treatments measured on the same blocks. |
