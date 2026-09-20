# 1080 — The straight line one test refuses, the other answers, and the refusal that translates the wrong error

**Status:** written with the work, 2026-09-18, with the fix it records.

Issue: [#1080](https://github.com/CyrilB1531/lodestar/issues/1080), found by the `code-review` run on
[#1034](https://github.com/CyrilB1531/lodestar/pull/1034) and [#1035](https://github.com/CyrilB1531/lodestar/pull/1035)
after they merged.

## Reproduced

Six calls on `main` at `04882224`, every one of them measured:

| call | `main` | wanted |
| --- | --- | --- |
| `DickeyFullerRegression.Candidates(walk, None, maxLag: 5, rows: 6, withTStatistics: true)` | `ArgumentException("the lagged design this series builds has no unique least-squares solution … which a series lying on a straight line does", "series")` on a **full-rank** random walk | the estimate's own *no residual degree of freedom* refusal |
| `AugmentedDickeyFuller`'s `<exception>` XML | no mention of the straight line | the refusal the reference page and `Estimate` already carry |
| `AugmentedDickeyFuller(0.3 + 0.1·i, 40 points, Constant, Fixed, MaxLag = 0)` | `Statistic = 0.1474698045709202`, `PValue = 0.9691565700329396` | refused |
| `Kpss(1.0 ± 1e-10 white noise, n = 1 000 000, ConstantAndTrend)` | refused as "one straight line" | answered |
| `Kpss(1000 + 0.5·i ± 1e-7, n = 50 000, ConstantAndTrend)` | refused as "one straight line" | answered |
| `Kpss(30 values alternating 1e307 and 2e307, ConstantAndTrend)` | refused as "one straight line" | answered, as before #1035 |
| `Kpss`'s `<exception>` XML | "lies exactly on a line" | the wording the reference page and the equivalence row now carry |

### Why the translation catches too much

`HouseholderEstimate.Fit` raises `ArgumentException` naming `design` twice — once for a rank-deficient
design and once for a fit with no residual degree of freedom — and `DickeyFullerRegression.Fit` filtered
on the parameter name alone. `Candidates` raises the second one *on purpose*, through `Fit`, at the lag
the search first reaches it, so a search that runs out of rows is reported as a collinear straight line.

### Why the KPSS tolerance grows with the series

`RefuseExactFit` compared `√(Σr²/n)` — a root-mean-square, which does not grow with `n` — against
`n·ε·max|x|`. The bar on the residual *norm* therefore rose as `n^1.5`, and swallowed two series whose
residuals are thousands to hundreds of thousands of ulps. The predicate was also written as an early
`return` on `>`, so a `NaN` residual — which 30 values alternating `1e307` and `2e307` produce, the line
fit's mean overflowing — fell through to the refusal and was reported as a straight line.

## The measurement the new bar rests on

A perfect line's residue is not a fixed number of ulps: the line fit sums naively, so its rounding grows
with `n`. Measured over random `a + b·i`, worst of 200 draws below 10 000 points and 20 above, as
`√(Σr²/n) / (ε·max|x|)`:

| n | 30 | 1 000 | 10 000 | 100 000 | 1 000 000 |
| --- | ---: | ---: | ---: | ---: | ---: |
| residual RMS, in ulps | 2.7 | 9.8 | 78 | 36 | 1.05e4 |
| **max second difference, in ulps** | **1.5** | **2.2** | **1.5** | **1.5** | **1.4** |

The second difference `x[i+2] − 2·x[i+1] + x[i]` reads three neighbours and sums nothing, so its rounding
floor is a handful of ulps at every length — and the two series #1080 wants answered sit far above it:
`6.8e4` ulps for `1000 + 0.5·i ± 1e-7`, `1.8e6` for `1.0 ± 1e-10`, `4.5e15` for the overflowing pair.

Neither test alone is enough. The second difference is local, and a series whose second differences are
`δ` still departs from its endpoint line by as much as `δ·n²/8`, so a slow bend at a large `n` would be
refused by it. The residual RMS is global but cannot be tightened, because the fit's own rounding is what
it measures. **A series is refused when both say line**: the residuals are inside the fit's rounding, and
the data is a line to the last bits of its own storage.

## Change

`SeriesChecks.RefuseStraightLine(series, lineResiduals, consequence)` holds both criteria and both callers' message. It takes the residuals rather than fitting the line again, because `Kpss` has already computed exactly those:

* `√(Σr²/n) ≤ n·ε·max|x|` over the residuals of `LineFit.Through(1…n, series)` — the bar #1035
  introduced, kept, and now only half the test;
* `max |x[i+2] − 2·x[i+1] + x[i]| ≤ 8·ε·max|x|` — four times the worst perfect line measured above,
  and four orders of magnitude below the nearest series the issue wants answered.

Both are written as `≤`, so a `NaN` on either side answers rather than refuses.

`Kpss` calls it under `ConstantAndTrend`, where `RefuseExactFit` stood. `AugmentedDickeyFuller` calls it
too, for every trend specification and every lag, which is finding 3: a line's differences are one
constant, so under `Constant` at lag 0 the design is full rank and the fit exact, and the statistic is
the quotient of two rounding errors. Under `None` at lag 0 the design is full rank and the fit is *not*
exact, and the same line was answered `12.0499`, which statsmodels answers too; it is refused here as
well, because a line is one input and a reader holds one contract for it rather than seven refusals and a
number. The window `HobijnLag` computes is now fixed at zero when its arithmetic is not finite, rather
than cast from a NaN: the cast is 0 on .NET 10 and `int.MinValue` on .NET Framework, which is the
runtime-dependent answer #874 closed, and answering an overflowing fit reopens the path to it.

`DickeyFullerRegression.Fit` translates the estimate's refusal only when the design it just built has a
residual degree of freedom left — `rows − (terms + 1 + lag) ≥ 1`, the same arithmetic `Candidates` uses
to decide when to raise it.

## Rejected

* **Comparing the residual norm `‖r‖₂` with `n·ε·max|x|`** — the dimensionally consistent reading of
  finding 4. It answers both of the issue's series, but it also answers a perfect line at `n = 1 000 000`,
  whose norm is `1.05e7·ε·max|x|` against a bar of `1e6`: the regression #874 and #976 closed.
* **Compensating the summation in `LineFit.Through`**, which would flatten the residual floor to a few
  ulps at every `n` and let one tight bar do the whole job. It moves every `ct` KPSS statistic in its last
  bits, so it is an oracle change and a separate concern from six review findings.
* **A distinguishable exception type for the rank refusal in `Lodestar.Stats.Regression`.** It is the
  direct reading of finding 1, but the two packages meet through a `PackageReference`, so the only things
  that travel are the type, the parameter name and the message: a new public exception type, on a
  published package, to let one caller tell two refusals apart that it can already tell apart by counting
  its own rows.
