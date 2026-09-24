# Changelog — Lodestar.Preprocessing

What changed in `Lodestar.Preprocessing`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.2.0] — 2026-09-24

### Added

- `Normalizer`, `PolynomialFeatures`, `KBinsDiscretizer`, `QuantileTransformer`, `PowerTransformer`, `LabelEncoder` and `KnnImputer` reshape a feature rather than rescale it, at scikit-learn parity, with an edge on `Lodestar.Cluster` for the k-means bin strategy. ([#1122](https://github.com/CyrilB1531/lodestar/issues/1122))
- `Splitters` cuts cross-validation folds and a train/test split over row indices, at scikit-learn parity. ([#762](https://github.com/CyrilB1531/lodestar/issues/762), [`1cdec9f1`](https://github.com/CyrilB1531/lodestar/commit/1cdec9f1))
- `MinMaxScaler`, `MaxAbsScaler` and `RobustScaler` join `StandardScaler`, with an edge on `Lodestar.Stats` for `unit_variance`. ([#763](https://github.com/CyrilB1531/lodestar/issues/763), [`3b3c4164`](https://github.com/CyrilB1531/lodestar/commit/3b3c4164))
- `Encoders.OneHot`, `Encoders.Ordinal` and `SimpleImputer` encode categories and fill missing values, at scikit-learn parity. ([#764](https://github.com/CyrilB1531/lodestar/issues/764), [`89923b23`](https://github.com/CyrilB1531/lodestar/commit/89923b23))
- The scalers fit over batches with `PartialFit` and over a `CsrMatrix`, with an edge on `Lodestar.Abstractions`. ([#765](https://github.com/CyrilB1531/lodestar/issues/765), [`e3a38ca9`](https://github.com/CyrilB1531/lodestar/commit/e3a38ca9))
- `MaxAbsScaler`, `StandardScaler` and `RobustScaler` transform and inverse-transform a `CsrMatrix`, keeping its stored positions, so a sparse fit no longer has to be applied to dense data. ([#895](https://github.com/CyrilB1531/lodestar/issues/895))

### Changed

- `BinEncoding`, `BinStrategy`, `CategoryDrop`, `ImputationStrategy`, `NeighbourWeights`, `PowerMethod`, `QuantileMethod`, `QuantileOutput`, `RowNorm`, `UnknownCategory`, `KBinsDiscretizerOptions`, `KnnImputerOptions`, `MaxAbsScalerOptions`, `MinMaxScalerOptions`, `OneHotEncoderOptions`, `PolynomialFeaturesOptions`, `PowerTransformerOptions`, `QuantileTransformerOptions`, `RobustScalerOptions`, `SimpleImputerOptions` and `StandardScalerOptions` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `StandardScaler`, `MinMaxScaler` and `MaxAbsScaler` walk rows and features without a modulo per element, the encoders look categories up by hash where equality allows it, and `Splitters.StratifiedKFold` keeps a fold cursor per class. ([#853](https://github.com/CyrilB1531/lodestar/issues/853))
- `RobustScaler.Fit(CsrMatrix)` groups the stored values by column in one pass, where it scanned every stored value for each column. ([#817](https://github.com/CyrilB1531/lodestar/issues/817))

### Fixed

- `KBinsDiscretizer` removes an edge on the successive gaps of the fitted edges, not on the distance to the last edge kept, so a run of gaps within `1e-8` collapses the way the reference collapses it. ([#1128](https://github.com/CyrilB1531/lodestar/issues/1128))
- `Splitters.StratifiedKFold` numbers classes over the order given and `Splitters.TrainTest` holds out that order's head, so scikit-learn's permutation reproduces `ShuffleSplit`. ([#893](https://github.com/CyrilB1531/lodestar/issues/893))
- `SimpleImputer.Fit` fills a kept empty feature with `FillValue` under `ImputationStrategy.Constant`, where it filled it with zero. ([#894](https://github.com/CyrilB1531/lodestar/issues/894))
- `SimpleImputer.Fit` and `Encoders.OneHot` refuse an undefined `ImputationStrategy`, `CategoryDrop` or `UnknownCategory`, where they read it as the mean, no drop or ignoring. ([#912](https://github.com/CyrilB1531/lodestar/issues/912))
- The sparse `Transform` and `InverseTransform` overloads refuse a matrix with no row, as the dense ones do, and their non-finite refusal no longer cites a percentile the transform never takes. ([#989](https://github.com/CyrilB1531/lodestar/issues/989))
- `StandardScaler` refuses a non-finite value on its dense `Fit`, `PartialFit`, `Transform` and `InverseTransform` and on both `CsrMatrix` overloads, where it answered a `NaN` mean and scale, and the six sparse `<exception>` tags now carry the refusals the reference pages already state. ([#1041](https://github.com/CyrilB1531/lodestar/issues/1041), [#1042](https://github.com/CyrilB1531/lodestar/issues/1042), [#1046](https://github.com/CyrilB1531/lodestar/issues/1046))
- The three sparse fits sum a column stored twice in one row before reading its statistics, as `scipy.sparse`'s reductions do, so `MaxAbsScaler.Fit` answers `8` rather than `5` and `RobustScaler.Fit` no longer leaks an `Array.Copy` failure. ([#1044](https://github.com/CyrilB1531/lodestar/issues/1044), [#1045](https://github.com/CyrilB1531/lodestar/issues/1045))
- The sparse overloads refuse a cell whose duplicate entries sum to an infinity, and a clipping `MaxAbsScaler.Transform(CsrMatrix)` clamps such a cell's sum rather than each entry, so both read what the dense overload reads. ([#1101](https://github.com/CyrilB1531/lodestar/issues/1101))

## [0.1.0] — 2026-09-10

### Added

- **`Lodestar.Preprocessing` 0.1.0 — `StandardScaler`, at scikit-learn parity, with spans instead of an `IDataView`.** `Fit` takes a row-major span and a feature count — the shape `Lodestar.Metrics` already uses — and returns a scaler whose `Mean`, `Variance` and `Scale` are readable and **nullable exactly where the reference reports `None`**: `with_mean=False` still fits a mean, and only turning both steps off drops it. `Transform` and `InverseTransform` return new arrays and never write to the input. **A near-constant feature scales by 1, and the test is not `variance == 0`**: scikit-learn compares the variance against the two-pass error bound of Chan, Golub and LeVeque, `var <= n·eps·var + (n·mean·eps)²`, so a feature with a large mean and a tiny variance is constant to within what the computation could resolve. `tests/oracles/preprocessing_standard_scaler.json` freezes eight cases against scikit-learn 1.9.0, including the pair that separates the two readings — `1e8 ± 1e-8`, whose variance is `1.48e-16` and whose scale is 1, against `1e8 ± 1e-7`, scaled by `8.5e-08`. An implementation testing `variance == 0` passes every other case and fails those two by eight orders of magnitude. The threshold is read from `sklearn.preprocessing._data._is_constant_feature` (BSD-3, allowed by [decision 0003](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0003-provenance-and-licensing.md)) and attributed in the source: the papers give the error analysis, not the number. Core tier — no external dependency, no inter-package edge. ([#568](https://github.com/CyrilB1531/lodestar/issues/568))
