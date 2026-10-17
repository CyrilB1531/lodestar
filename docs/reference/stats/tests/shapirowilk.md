# ShapiroWilk

The Shapiro-Wilk test for normality, by Royston's AS R94.

Written from Royston's 1995 published description and its published polynomial constants
(Applied Statistics 44:547-551), not from any implementation of it.

It follows the algorithm's faults: fewer than three values and a range below `1e-19` are
refused, and past 5000 values, where the fitted transform is extrapolated, it computes, as AS R94
does with a warning ([decision 0010](../../../decisions/0010-each-algorithm-follows-its-defining-document.md)).

## Members

| Member | What it does |
| --- | --- |
| [`ShapiroWilk.Test`](shapirowilk-test.md) | Tests whether a sample could have come from a normal distribution. |
