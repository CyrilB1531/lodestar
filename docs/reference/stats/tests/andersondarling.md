# AndersonDarling

The Anderson-Darling test: could this sample be normal?

The other normality test beside [`ShapiroWilk`](shapirowilk.md), and the one whose reference holds
at any size: Royston's approximation is fitted over `3 ≤ n ≤ 5000` and only extrapolated past it,
where this statistic has no upper bound at all. It weighs the tails more heavily than the centre,
which is usually where a departure from normality matters, and is the reason the two disagree on
the same sample often enough to be worth running both.

## Members

| Member | What it does |
| --- | --- |
| [`AndersonDarling.Test`](andersondarling-test.md) | Tests a sample against the normal distribution, fitting its mean and spread. |
| [`AndersonDarling.KSample`](andersondarling-ksample.md) | Tests whether several samples come from one distribution. |
