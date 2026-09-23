# Binomial

The binomial test: is this proportion of successes consistent with a stated one?

The exact counterpart of what [`ChiSquare.GoodnessOfFit`](chisquare-goodnessoffit.md) answers
asymptotically over two categories. Exact means the p-value is a sum of binomial probabilities
rather than a chi-squared approximation of one, so it is right at any number of trials — which is
the point, because the approximation is worst exactly where a proportion is most often tested: few
trials, or a probability near zero or one.

## Members

| Member | What it does |
| --- | --- |
| [`Binomial.Test`](binomial-test.md) | Tests a count of successes against a stated probability. |
