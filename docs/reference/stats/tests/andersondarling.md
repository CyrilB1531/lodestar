# AndersonDarling

The Anderson-Darling test: could this sample be normal?

The other normality test beside [`ShapiroWilk`](shapirowilk.md), and the one that keeps working
where Shapiro-Wilk's own reference stops: Royston's approximation is fitted over `3 ≤ n ≤ 5000`,
where this statistic has no upper bound at all. It weighs the tails more heavily than the centre,
which is usually where a departure from normality matters, and is the reason the two disagree on
the same sample often enough to be worth running both.

## Members

| Member | What it does |
| --- | --- |
| [`AndersonDarling.Test`](andersondarling-test.md) | Tests a sample against the normal distribution, fitting its mean and spread. |
