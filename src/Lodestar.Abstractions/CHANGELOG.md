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
