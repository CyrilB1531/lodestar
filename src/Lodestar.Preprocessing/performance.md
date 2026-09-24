# Performance — Lodestar.Preprocessing

What `Lodestar.Preprocessing` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## The splitters against ML.NET and scikit-learn (issue #762)

Full method and what cannot be made to agree:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#44-the-splitters-against-mlnet-and-scikit-learn-issue-762).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, default job, one run. Five folds,
three classes at 60 / 30 / 10.

**ML.NET's split is lazy**, so it appears twice: the call, and the call followed by reading which
rows each fold holds. Only the second is a split a caller can fit on, and it is the one that scales
with the data — the construction is flat in the row count because it builds ten wrappers and stops.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 10,000 | five folds, constructed | **52.52 μs** | 79.19 μs | 1.51 |
| 10,000 | five folds, rows read | **52.52 μs** | 3,330.43 μs | **63.44** |
| 10,000 | train/test, constructed | **3.11 μs** | 15.70 μs | 5.05 |
| 10,000 | train/test, rows read | **3.11 μs** | 592.81 μs | **190.55** |
| 100,000 | five folds, constructed | **695.48 μs** | 78.35 μs | 0.11 |
| 100,000 | five folds, rows read | **695.48 μs** | 25,527.38 μs | **36.71** |
| 100,000 | train/test, constructed | **56.81 μs** | 16.30 μs | 0.29 |
| 100,000 | train/test, rows read | **56.81 μs** | 4,454.57 μs | **78.42** |

Stratifying costs 136.77 μs at 10,000 rows and 1,489.37 μs at 100,000 — 2.6× and 2.1× the plain fold
cut. **ML.NET has no row to compare it against**: `CrossValidationSplit` and `TrainTestSplit` never
stratify ([dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396), open
since 2019).

Allocation, per operation: 273.94 KB against ML.NET's 149.07 KB at 10,000 rows for the fold cut —
**this returns every index of every fold, where ML.NET returns views that allocate again on each
read**, so the two columns count different things and the larger one is the one holding the answer.

Against `scikit-learn` 1.9.0 through `compare-splitters`, one run of each side, milliseconds per
split, best of five:

| n | operation | Lodestar | `scikit-learn`, wall / cpu | ratio, wall |
| ---: | --- | ---: | ---: | ---: |
| 10,000 | `KFold` | **0.050 ms** | 0.057 / 0.057 ms | **1.14** |
| 10,000 | `StratifiedKFold` | **0.135 ms** | 0.445 / 0.445 ms | **3.30** |
| 10,000 | train/test | **0.003 ms** | 0.073 / 0.073 ms | **25.70** |
| 100,000 | `KFold` | **0.798 ms** | 2.145 / 2.145 ms | **2.69** |
| 100,000 | `StratifiedKFold` | **1.657 ms** | 5.718 / 5.717 ms | **3.45** |
| 100,000 | train/test | **0.149 ms** | 0.194 / 0.194 ms | **1.30** |
| 1,000,000 | `KFold` | **12.273 ms** | 14.919 / 14.917 ms | **1.22** |
| 1,000,000 | `StratifiedKFold` | **20.507 ms** | 48.187 / 48.178 ms | **2.35** |
| 1,000,000 | train/test | **0.517 ms** | 1.323 / 1.323 ms | **2.56** |

The million-row rows move about ±20% run to run on this machine — `KFold` there read 10.0 ms once
and 12.0 to 12.4 ms in the four runs after, on unchanged code. The ratios below 100,000 rows are
stable to the third decimal across every run.

`TrainTest` writes the two index halves straight out: an identity read comes out ascending already,
so neither half is sorted and no order array is filled.

## The scalers against ML.NET's normalizers (issue #763)

Full method, and why `fixZero: false` is passed:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#45-the-scalers-against-mlnets-normalizers-issue-763).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, one invocation per iteration,
five warmups and twenty iterations. Ten features per row; ML.NET's estimator is lazy, so it appears both
as a fit and as a fit whose values are read back.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | min-max, fit + transform | **68.91 μs** | 225.28 μs (fit only) | **3.27** |
| 1,000 | min-max, values read | **68.91 μs** | 1,208.41 μs | **17.54** |
| 1,000 | robust, values read | **1,189.85 μs** | 3,521.85 μs | **2.96** |
| 20,000 | min-max, fit + transform | **1,106.62 μs** | 3,614.47 μs (fit only) | **3.27** |
| 20,000 | min-max, values read | **1,106.62 μs** | 9,674.44 μs | **8.74** |
| 20,000 | robust, values read | **10,430.85 μs** | 15,245.13 μs | **1.46** |

`MaxAbsScaler` costs 65.21 μs and 1,116.54 μs at the two sizes and **has no ML.NET row**: its
normalizers offer mean-variance, min-max, log-mean-variance, robust scaling, binning and L_p norm,
and none of them is "divide by the largest absolute value" — `NormalizeMinMax(fixZero: true)` comes
closest and is a different transform.

Both sides scale the same column to the same values, checked row by row to `1e-6` before either was
timed — single precision, which is what ML.NET's pipeline carries.

**Where the cost is.** `RobustScaler` is about nine times `MinMaxScaler` here and allocates twice as
much: a percentile has to order its column, so it sorts each feature once where the other two take a
single pass. `numpy.percentile` partitions rather than sorting, which is the same asymptotic work
with smaller constants and is where to look if that row ever needs to be cheaper. Allocation, at
20,000 rows: 1,563.96 KB for min-max against ML.NET's 1,133.80 KB, and 3,126.26 KB for robust
against 4,821.33 KB.

## The encoders and the imputer against ML.NET (issue #764)

Full method, and the shape that had to be corrected first:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#46-the-encoders-and-the-imputer-against-mlnet-issue-764).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-16. `BenchmarkDotNet` 0.14.0, one invocation per iteration,
five warmups and twenty iterations. Twenty categories; ML.NET's estimator is lazy, so it appears as a fit
and as a fit whose values are read back.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | one-hot, fit + transform | **136.37 μs** | 195.66 μs (fit only) | **1.43** |
| 1,000 | one-hot, **values read** | **136.37 μs** | 728.92 μs | **5.35** |
| 1,000 | impute, **values read** | **84.09 μs** | 650.84 μs | **7.74** |
| 20,000 | one-hot, fit + transform | **3,672.62 μs** | 2,083.19 μs (fit only) | 0.57 |
| 20,000 | one-hot, **values read** | **3,672.62 μs** | 5,384.35 μs | **1.47** |
| 20,000 | impute, **values read** | **925.39 μs** | 4,670.33 μs | **5.05** |

[`Encoders.Ordinal`](../../docs/reference/preprocessing/encoding/encoders-ordinal.md) costs 133.73 μs and 2,875.13 μs at the two sizes — about three quarters of the
one-hot encoding, at a **tenth** of the allocation (16.70 KB against 165.24 KB at 1,000 rows), since
it produces one column rather than one per category. ML.NET's `MapValueToKey` is its counterpart and
is not measured here: it maps to a key type inside the pipeline rather than to a number a caller
holds.

**The correction, stated because the first table was wrong.** `OneHotEncoding` takes **one named
column** where this package's encoders take a matrix of however many features, so the first run
encoded four features here against one there — four times the work for the same row, reported as
**2.8× slower** at 20,000. One column on both sides is the comparison above. `ReplaceMissingValues`
does take a vector column, so the imputer rows compare four features against four.

**Allocation** is where the two differ most on the read: 165.24 KB against 413.76 KB at 1,000 rows,
and 3,282.43 KB against 1,212.64 KB at 20,000 — this package materialises every encoded column as a
`double`, where ML.NET's cursor yields rows one at a time and never holds the matrix.

## The feature transformers against ML.NET and scikit-learn (issue #1122)

Full method and what each row does and does not compare:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#55-the-feature-transformers-against-mlnet-and-scikit-learn-issue-1122).
Machine: the AMD Ryzen 7 8700G named above, on 2026-09-23. `BenchmarkDotNet` 0.14.0, one invocation
per iteration, five warmups and twenty iterations. Four features; ML.NET's estimators are lazy, so
each appears as a fit whose rows are read.

**Only two of the seven have an incumbent in .NET at all.** `NormalizeLpNorm` scales a row to unit
norm as [`Normalizer`](../../docs/reference/preprocessing/transforming/normalizer.md) does; `NormalizeBinning`
cuts a feature into bins but emits a position in `[0, 1]` rather than the bin, so the two price the
same traversal and not the same answer.

| rows | operation | Lodestar | ML.NET 5.0.0 | ML.NET / Lodestar |
| ---: | --- | ---: | ---: | ---: |
| 1,000 | unit-norm rows, **values read** | **28.01 μs** | 717.09 μs | **25.60** |
| 1,000 | five bins, fit + transform, **values read** | **566.79 μs** | 1,525.96 μs | **2.69** |
| 20,000 | unit-norm rows, **values read** | **313.21 μs** | 4,816.59 μs | **15.38** |
| 20,000 | five bins, fit + transform, **values read** | **8,657.46 μs** | 18,112.77 μs | **2.09** |

The other five have nothing to race. Their own numbers at 20,000 rows and four features, so the
shape of the cost is on the record:
[`PolynomialFeatures.Transform`](../../docs/reference/preprocessing/transforming/polynomialfeatures-transform.md) 950.72 μs,
[`QuantileTransformer.Fit`](../../docs/reference/preprocessing/transforming/quantiletransformer-fit.md) 7,173.56 μs and its
transform 10,806.04 μs,
[`PowerTransformer.Fit`](../../docs/reference/preprocessing/transforming/powertransformer-fit.md) 32,350.17 μs against a
transform of 1,085.94 μs, and
[`KnnImputer.Transform`](../../docs/reference/preprocessing/encoding/knnimputer-transform.md) 38,939.98 μs over its own fixed
2,000 rows. **[`PowerTransformer`](../../docs/reference/preprocessing/transforming/powertransformer.md)'s fit is thirty times its transform** — it maximises a
log-likelihood by Brent's method per feature — and [`KnnImputer`](../../docs/reference/preprocessing/encoding/knnimputer.md) is the one member here that is
quadratic in the rows, which is why it is pinned and why the type itself refuses past 100 million
distance terms.

Against `scikit-learn` 1.9.1 and `numpy` 2.5.3 through `compare-transformers`, one run of each
side, milliseconds per operation, best of five. **Ratios above 1 mean Lodestar is faster; several
are below it, and that is the finding.**

| n | operation | Lodestar | `scikit-learn`, wall / cpu | ratio, wall | ratio, cpu |
| ---: | --- | ---: | ---: | ---: | ---: |
| 10,000 | `Normalizer` | **0.188 ms** | 0.196 / 0.196 ms | **1.04** | **1.03** |
| 10,000 | `PolynomialFeatures` | 0.588 ms | **0.492** / 0.492 ms | 0.84 | 0.83 |
| 10,000 | `KBinsDiscretizer`, fit | **0.964 ms** | 1.147 / 1.147 ms | **1.19** | **1.19** |
| 10,000 | `KBinsDiscretizer`, transform | **0.261 ms** | 2.661 / 2.660 ms | **10.19** | **8.12** |
| 10,000 | `QuantileTransformer`, fit | **1.034 ms** | 2.312 / 2.311 ms | **2.24** | **2.24** |
| 10,000 | `QuantileTransformer`, transform | 1.106 ms | **0.982** / 0.982 ms | 0.89 | 0.89 |
| 10,000 | `PowerTransformer`, fit | **14.035 ms** | 38.040 / 38.033 ms | **2.71** | **2.71** |
| 10,000 | `PowerTransformer`, transform | 0.569 ms | **0.453** / 0.452 ms | 0.80 | 0.79 |
| 10,000 | `KnnImputer`, transform | 28.056 ms | **16.730** / 200.865 ms | 0.60 | **7.13** |
| 10,000 | `LabelEncoder` | 0.683 ms | **0.445** / 0.445 ms | 0.65 | 0.65 |
| 100,000 | `Normalizer` | 1.211 ms | **1.067** / 1.067 ms | 0.88 | 0.69 |
| 100,000 | `PolynomialFeatures` | **4.289 ms** | 4.539 / 4.533 ms | **1.06** | 0.87 |
| 100,000 | `KBinsDiscretizer`, fit | 9.622 ms | **4.619** / 4.619 ms | 0.48 | 0.45 |
| 100,000 | `KBinsDiscretizer`, transform | **3.252 ms** | 10.622 / 10.620 ms | **3.27** | **2.61** |
| 100,000 | `QuantileTransformer`, fit | **9.903 ms** | 11.505 / 11.503 ms | **1.16** | **1.08** |
| 100,000 | `QuantileTransformer`, transform | 10.161 ms | **9.145** / 9.143 ms | 0.90 | 0.86 |
| 100,000 | `PowerTransformer`, fit | **147.715 ms** | 272.278 / 272.239 ms | **1.84** | **1.69** |
| 100,000 | `PowerTransformer`, transform | 4.838 ms | **3.062** / 3.061 ms | 0.63 | 0.58 |
| 100,000 | `KnnImputer`, transform | 29.016 ms | **13.597** / 163.092 ms | 0.47 | **5.63** |
| 100,000 | `LabelEncoder` | 8.846 ms | **4.674** / 4.674 ms | 0.53 | 0.51 |

**Where this package wins, it wins on the work rather than on the loop.** The discretizer's
transform is a binary search per value against a `numpy.digitize` that builds an index array; the
power fit is Brent's method against `scipy.optimize.brent` driven from Python, which is why the fit
is ahead and the transform — one `Math.Pow` per value against a vectorised `numpy.power` — is
behind.

**Where it loses, it loses to vectorised C, and the losses are where a single array operation does
the whole job**: [`PolynomialFeatures`](../../docs/reference/preprocessing/transforming/polynomialfeatures.md) multiplies column by column
there, [`LabelEncoder`](../../docs/reference/preprocessing/encoding/labelencoder.md) is one `numpy.unique`, and the quantile map is one `numpy.interp`. Roughly a factor of two at 100,000
rows, which is what a scalar managed loop costs against SIMD C over a contiguous array.

**[`KnnImputer`](../../docs/reference/preprocessing/encoding/knnimputer.md) is the row to read twice.** It is behind on elapsed time and **5.6× ahead on
processor time**, because `scikit-learn`'s pairwise distances run on every core through joblib and
this runs on one: the reference spends 163 ms of CPU to finish in 13.6 ms. This one is a
parallelism gap, not an arithmetic one, and it is the honest candidate for its own `perf/` issue.

[`KBinsDiscretizer`](../../docs/reference/preprocessing/transforming/kbinsdiscretizer.md)'s fit at 100,000 rows is the other
one: it sorts each column with
`Array.Sort` against numpy's introsort over a contiguous buffer, and 0.48× is about the constant
factor that costs.
