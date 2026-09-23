# Bartlett

Bartlett's test: do several groups share one variance, assuming each is normal?

The same question [`Levene`](levene.md) asks, under the stronger assumption — which buys power
when it holds and costs correctness when it does not. Bartlett's statistic is sensitive to
departures from normality in a way that looks like a difference in variance, so on data whose
shape is unknown [`Levene`](levene.md) is the safer reading, and that is why both ship.

## Members

| Member | What it does |
| --- | --- |
| [`Bartlett.Test`](bartlett-test.md) | Compares the variances of two or more groups. |
