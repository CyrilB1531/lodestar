# Changelog — Lodestar.Abstractions

What changed in `Lodestar.Abstractions`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Added

- `IvDesign`, `IvOptions`, `IvCovarianceType`, `IvSummary` and `IvFirstStage` in `Lodestar.Stats.Regression.Instrumental`, the data `InstrumentalVariables` takes and returns, with `WaldTest` and `KernelType` in `Lodestar.Stats.Regression`, shared with the panel estimators. ([#1155](https://github.com/CyrilB1531/lodestar/issues/1155))
- `PanelDesign`, `PanelOptions`, `PanelCovarianceType` and `PanelSummary` in `Lodestar.Stats.Regression.Panel`, the data `PanelRegression` takes and returns. ([#1156](https://github.com/CyrilB1531/lodestar/issues/1156))
- `CrossConformalMethod` and `CrossConformalAggregation` in `Lodestar.Conformal`, which `CrossConformal` takes. ([#1159](https://github.com/CyrilB1531/lodestar/issues/1159))
- `OneHotEncoderOptions.MinFrequency`, `MinFrequencyShare` and `MaxCategories`, and `UnknownCategory.Infrequent`. ([#1161](https://github.com/CyrilB1531/lodestar/issues/1161))
- `AndersonKSampleVariant` and `CorrelationMatrix` in `Lodestar.Stats`, which the k-sample Anderson-Darling test and Spearman's correlation matrix take and return. ([#1162](https://github.com/CyrilB1531/lodestar/issues/1162))
- `KMeansOptions.InitialCentreSets` and `Restarts`, the starts `KMeans` runs from. ([#1163](https://github.com/CyrilB1531/lodestar/issues/1163))
- `LogRankOptions`, `LogRankWeighting`, `PairwiseLogRankResult` and `RestrictedMeanResult` in `Lodestar.Survival`, which the log-rank family and the restricted mean take and return. ([#1170](https://github.com/CyrilB1531/lodestar/issues/1170))
- `CoxBaseline` and `CoxTimeTransform` in `Lodestar.Survival`, the Cox baselines and the proportional hazards test's time scale. ([#1171](https://github.com/CyrilB1531/lodestar/issues/1171))
- `ParametricModel`, `AftModel` and `SurvivalCurve` in `Lodestar.Survival`, the parametric models and the Breslow-Fleming-Harrington curve. ([#1172](https://github.com/CyrilB1531/lodestar/issues/1172))
- `PorterStemmerMode` in `Lodestar.Text.Stemming`, which `PorterStemmer.Stem` takes. ([#1193](https://github.com/CyrilB1531/lodestar/issues/1193))

### Changed

- `Chi2ContingencyResult` is `ChiSquaredContingencyResult` and its `Dof` is `DegreesOfFreedom`, the spellings `Lodestar.Stats` settles on before 1.0 — a breaking rename, since a type forward cannot carry the old name ([#1298](https://github.com/CyrilB1531/lodestar/issues/1298)); take this release with the next `Lodestar.Stats`. ([#1217](https://github.com/CyrilB1531/lodestar/issues/1217))
- `TruncatedSvdOptions` and `NmfOptions` are records comparing Ω by value like every other options type, and `NmfOptions.Initialization` defaults to `NndSvda`, which is scikit-learn's `init=None` at every rank a fit accepts — a default `Nmf.Fit` reads from this package, so taking this release changes what `Lodestar.Decomposition` 0.3.0 computes too. ([#1232](https://github.com/CyrilB1531/lodestar/issues/1232))
- `CsrMatrix` refuses a null array, a negative dimension or a dense operand without a column through the shared `Guard` rather than its own copy — on net10.0 the negative-dimension message is now `ThrowIfLessThan`'s — and its remarks no longer call instances immutable while the arrays they hold are shared. ([#1282](https://github.com/CyrilB1531/lodestar/issues/1282))
- `CoxBaseline` says its arrays are the record's own and that the summary it came from predicts from a copy. ([#1304](https://github.com/CyrilB1531/lodestar/issues/1304))
- `TruncatedSvdOptions.RandomMatrix` and `NmfOptions.RandomMatrix` are `min(rows, features)` rows tall, since a matrix with fewer rows than features is factored as its transpose. ([#1256](https://github.com/CyrilB1531/lodestar/issues/1256))

### Removed

- `GpuSearchResult`, which duplicated `SearchResult`, removed with no forward, so code built against it fails to load ([#1299](https://github.com/CyrilB1531/lodestar/issues/1299)); take this release with the next `Lodestar.Gpu`, which returns `SearchResult`. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))

### Fixed

- The records that write their own equality hash an absent member and every `NaN` as they compare them, `CsrMatrix.NormalizeRows` refuses an undefined `SparseNorm`, and `CsrMatrix.ToDense` and the block products refuse a result past the largest array rather than failing to allocate it. ([#1284](https://github.com/CyrilB1531/lodestar/issues/1284), [#1285](https://github.com/CyrilB1531/lodestar/issues/1285), [#1286](https://github.com/CyrilB1531/lodestar/issues/1286), [#1287](https://github.com/CyrilB1531/lodestar/issues/1287))
- `SearchResult` hashes its `float` score, and the Rake, TextRank and count-vectorizer options their token pattern, as they compare them, and `CsrMatrix.NormalizeRows` refuses a `NaN` or an infinity as scikit-learn's `normalize` does. ([#1294](https://github.com/CyrilB1531/lodestar/issues/1294), [#1295](https://github.com/CyrilB1531/lodestar/issues/1295), [#1296](https://github.com/CyrilB1531/lodestar/issues/1296))
- Records hash NaN doubles consistently across all NaN bit-patterns, the twenty that keep their generated equality included. ([#1285](https://github.com/CyrilB1531/lodestar/issues/1285))
- The array members of `TruncatedSvdOptions`, `NmfOptions`, `KMeansOptions`, `AndersonResult`, `ChiSquaredContingencyResult`, `CorrelationMatrix`, `KaplanMeierCurve`, `NelsonAalenCurve` and `SurvivalCurve` say they are not copied, so a write reaches the record, what it equals and every holder of the array. ([#1305](https://github.com/CyrilB1531/lodestar/issues/1305))
- `CsrMatrix`'s length refusal names `columnIndices` and its constructor documents every refusal on a page of its own, `KMeansOptions` compares its starting blocks through `ValueEquality` alone, and the README, package description and sparse page say which data types the package holds. ([#1326](https://github.com/CyrilB1531/lodestar/issues/1326), [#1327](https://github.com/CyrilB1531/lodestar/issues/1327), [#1328](https://github.com/CyrilB1531/lodestar/issues/1328), [#1329](https://github.com/CyrilB1531/lodestar/issues/1329), [#1330](https://github.com/CyrilB1531/lodestar/issues/1330))
- A test reads every data type's IL against decision 0003(e)'s no-code rule, the three #1364 decides excepted, the reference gate knows a constructor entry, and the pages say which data types moved and document the `CsrMatrix`, `IvDesign`, `PanelDesign` and `UndefinedMetricException` constructors. ([#1363](https://github.com/CyrilB1531/lodestar/issues/1363), [#1365](https://github.com/CyrilB1531/lodestar/issues/1365), [#1366](https://github.com/CyrilB1531/lodestar/issues/1366), [#1367](https://github.com/CyrilB1531/lodestar/issues/1367), [#1368](https://github.com/CyrilB1531/lodestar/issues/1368), [#1369](https://github.com/CyrilB1531/lodestar/issues/1369))

## [0.2.0] — 2026-09-24

### Added

- The public data types of `Lodestar.Stats`, `Lodestar.Cluster`, `Lodestar.Conformal`, `Lodestar.Decomposition`, `Lodestar.Embeddings`, `Lodestar.Fuzzy`, `Lodestar.Gpu`, `Lodestar.Metrics`, `Lodestar.Preprocessing`, `Lodestar.Stats.Regression`, `Lodestar.Stats.TimeSeries` and `Lodestar.Survival`, each under its own namespace. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- The sixteen public data types of `Lodestar.Text`, under their `Lodestar.Text.*` namespaces. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `CsrMatrix.CreateUnchecked` is public, for a producer whose arrays are valid by construction, so `Lodestar.Text` 0.6.0 keeps running once the package grants no `InternalsVisibleTo`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

### Changed

- The package grants no `InternalsVisibleTo`, to `Lodestar.Text` or to its own tests, and compiles the shared `ValueEquality` its moved data types will call. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `CsrMatrix.Multiply` and `CsrMatrix.TransposeMultiply` add each non-zero's scaled row over spans with vector lanes, up to 2.5 times faster. ([#845](https://github.com/CyrilB1531/lodestar/issues/845))
- `CsrMatrix.ToDense` adds a column stored twice in one row instead of keeping its last entry, as `Multiply` and scipy do. ([#878](https://github.com/CyrilB1531/lodestar/issues/878))
- The `CsrMatrix` page no longer says a column stored twice sums everywhere: the row norms and `NormalizeRows` treat each entry on its own, as scikit-learn does. ([#981](https://github.com/CyrilB1531/lodestar/issues/981))

## [0.1.1] — 2026-09-01

### Fixed

- **The shared internal helpers are no longer compiled into this package.** `src/Shared/Guard.cs` and its siblings are compiled into every library, and this one grants `InternalsVisibleTo` to `Lodestar.Text`, which compiles them too — one internal type in both assemblies is CS0436 at every call site on the consuming side, 96 of them across the two target frameworks. `CsrMatrix` carries the two argument guards it needs instead; behaviour and exception types are unchanged. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))

## [0.1.0] — 2026-09-01

### Added

- **The sparse primitive the packages share.** `CsrMatrix` and `SparseNorm` ship in a package of their own, with two new products — `Multiply(block, columnCount)` and `TransposeMultiply(block, columnCount)` — that read the matrix once per non-zero rather than once per block column. `Lodestar.Text` still declares its own copy until its next release; [decision 0071](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0071-csrmatrix-moves-to-an-abstractions-package.md) amends [0069](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0069-the-package-layout-as-built-and-what-enforces-it.md) and records the sequence. ([#440](https://github.com/CyrilB1531/lodestar/issues/440))
