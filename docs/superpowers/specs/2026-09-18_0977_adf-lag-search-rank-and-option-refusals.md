# 0977 — The lag search that ranks a rank-deficient candidate, and two refusals that give the wrong reason

**Status:** accepted, 2026-09-18. Written with the fix it records.

Issues: [#977](https://github.com/CyrilB1531/lodestar/issues/977) and
[#984](https://github.com/CyrilB1531/lodestar/issues/984), both found by the delta review of `main`
(`de9f192d..f7fff453`, 2026-09-17). One pull request, because both touch `Stationarity.cs` and the same
edge-test file.

## Reproduced on `main` at `ba224f8e`

The reproductions #977 names no longer reach the regression: #1080 refuses a straight line before any fit,
and `1, …, 50` is one. The defect survives on a series that is not a line but whose **search rows** are:

| call | `main` | statsmodels 0.15.0 |
| --- | --- | --- |
| `1, …, 50` with `x[0] = −5`, default options | `Statistic = 0`, lag 1 | `1.7802`, lag 3, `SingularMatrixWarning` |
| the same, `TStatistic` | `0`, lag 1 | `0.5553`, lag 10, `SingularMatrixWarning` |
| the same, `Schwarz` | `0`, lag 1 | `1.7802`, lag 3, `SingularMatrixWarning` |
| `0.5·i²`, 50 points, default options | refused naming `series` | `3.4035`, lag 11, `SingularMatrixWarning` |
| `n = 101`, `ConstantAndTrend`, `MaxLag = 48` | "leaves … no degree of freedom; at most 47 does" | refused: `maxlag` above `nobs/2 − 1 − ntrend` |
| `n = 5`, `ConstantAndQuadraticTrend`, `MaxLag = 3` | "… at most -2 does" | refused |
| `new KpssOptions { LagRule = (KpssLagRule)42 }` | built; `Kpss` then throws naming `options` | — |

The search reads `n − maxLag − 1` rows from the end, which excludes the bent first point, so every
difference it reads is 1 and each lagged difference repeats the intercept. `DickeyFullerRegression.Candidates`
never asks whether a candidate is full rank; it ranked lag 1 on a residual sum and t statistic made of
rounding. The final fit at the chosen lag reads one more row, is full rank, and answers.

Item 3 of #984, `HobijnLag`'s summary sitting above `RefuseExactFit`'s, is gone: 0b87dee1 (#1080)
deleted `RefuseExactFit`, and `HobijnLag` carries its summary again. Nothing to do.

## Decision: refuse, as every other rank-deficient fit here does

The issue leaves refuse-or-minimum-norm to the maintainer. #979 and #1095 (ADR 0147) already settled it for
the final ADF fit, `OrdinaryLeastSquares` and the VAR: a design with no unique solution is refused. A search
that ranked the reference's minimum-norm fits while the fit it selects refuses them would hold two contracts
for one input. The loser is statsmodels' answer on these series, recorded in the `adfuller` equivalence row.

### Why one check, on the widest design

Every candidate's design is a leading block of the widest one's columns over the same rows. A leading
block's singular values interlace inside the whole matrix's — its largest is no larger, its smallest no
smaller — so its condition number is no larger, and `RankBracket`'s test, `σmin ≤ σmax·max(rows, p)·ε`,
uses `max(rows, p) = rows` for every candidate. The widest design passing therefore proves every candidate
passes, and one call to `SharedReflections.RequireFullRank` after the loop replaces one per lag.

The refusal is re-raised with the wording `DickeyFullerRegression.Fit` already uses, naming `series`:
`RequireFullRank`'s own message tells a VAR caller to drop a variable, which an ADF caller has none of. That
wording's tail, "which a series lying on a straight line does at every lag above zero", described the only
case that reached it before #1080; it now says a low-degree polynomial over the rows the regression reads.

### `MaxLag`'s two reasons

The ceiling (`given > n/2 − terms − 1`, the reference's rule) and the degree-of-freedom check
(`n − 2·given − terms − 2 < 1`) each get their own sentence. The largest lag both allow is
`min(ceiling, ⌊(n − terms − 3)/2⌋)`, floored by hand because C#'s division truncates toward zero and would
allow lag 0 at `n − terms = 2`. When it is negative the message says no lag fits instead of offering one.

### `KpssOptions.LagRule`

An `init` setter refusing undeclared values with `ParamName = "value"`, as `DickeyFullerOptions.LagSelection`
and `KpssOptions.Regression` do. The switch in `Kpss` then answers `Fixed` through its two discard arms: with
the setter guarding the enum, `KpssLagRule.Fixed` arms plus a default arm would be two names for one case.

## Verified

- A random differential test against statsmodels 0.15.0: 600 series (random walks, white noise, scaled
  MA(2) between 1e-3 and 1e3), `n` from 12 to 400, every `regression` and `autolag` and fixed lags 0–3.
  596 agree on the lag and on the statistic to 3.9e-12 relative; the other 4 are the documented refusal of a
  default search too wide for a short series under `regression="n"`. The new check refused none.
- The `Lodestar.Stats.TimeSeries` suite and its `netstandard2.0` mirror.
- `StationarityBenchmarks`, `main` / branch / `main` under `./.dotnet-guarded`, on an AMD Ryzen 7 8700G
  with a background process holding one core. `LodestarAdfAutolag` at 200 points: 17.74 / 17.37 / 16.01 µs;
  at 2,000: 693.4 / 622.9 / 628.2 µs — inside the spread the two `main` runs show between themselves.
  Allocation rises from 62.05 to 66.56 KB and from 989.03 to 1,001.27 KB: the triangle, its inverse and
  the spectrum bracket the rank check builds once per search.
