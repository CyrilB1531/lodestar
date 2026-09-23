# Levene

Levene's test: do several groups share one variance?

The assumption [`OneWayAnova`](onewayanova.md) makes, and the one
[`TTest.Independent`](ttest-independent.md) makes unless [`Variance.Welch`](variance.md) is asked
for — so this is how a caller checks the ground those two stand on. It compares each group's
absolute deviations from its own centre, which turns a question about spread into a one-way ANOVA
on those deviations. [`Bartlett`](bartlett.md) asks the same question assuming normal data and is
sharper when that holds; this one survives when it does not.

## Members

| Member | What it does |
| --- | --- |
| [`Levene.Test`](levene-test.md) | Compares the spread of two or more groups. |
