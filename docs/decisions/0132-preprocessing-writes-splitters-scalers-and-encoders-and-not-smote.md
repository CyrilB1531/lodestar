---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075"]
---
# 0132 — `Lodestar.Preprocessing` writes splitters, scalers and encoders, and not SMOTE

**Status:** accepted · **Date:** 2026-09-14 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md)

## Context

`Lodestar.Preprocessing` is `StandardScaler` and its options. `docs/equivalence.md` named the rest —
`MinMaxScaler`, `RobustScaler`, `OneHotEncoder`, `SimpleImputer`, `train_test_split`,
`StratifiedKFold`, `partial_fit`, a `CsrMatrix` overload — and pointed at
[#568](https://github.com/CyrilB1531/lodestar/issues/568), which is closed.
[#680](https://github.com/CyrilB1531/lodestar/issues/680) asked for the list to be taken in order of
how empty .NET is, and put two items first on a claim of its own: **stratified splitting and SMOTE
"have no .NET answer at all"**. [Decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)
says that claim is read before it is made.

It is half true.

## The reading

nuget.org, 2026-09-14: `stratified`, `train test split`, `kfold`, `oversampling`, `one hot encoding`,
`imputation` and `min max scaler` return nothing; `smote` returns one package, `imbalanced` one
unrelated; `cross validation` returns `SharpLearning.CrossValidation` first. GitHub, searched for
`smote language:C#`, returns four repositories. Every package was read with `tools/survey.cs`
([decision 0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)) and its
licence from the package, per [decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md).

| package | published | licence | `lib/` | surface | what it answers |
| --- | --- | --- | --- | --- | --- |
| `Microsoft.ML` 5.0.0, assembly `Microsoft.ML.Data` | 2025-11-11 | MIT | `netstandard2.0` | 167 types, 500 members | `DataOperationsCatalog.TrainTestSplit(data, testFraction, samplingKeyColumnName, seed)` and `CrossValidationSplit(data, numberOfFolds, samplingKeyColumnName, seed)`; `MapValueToKey` |
| `Microsoft.ML` 5.0.0, assembly `Microsoft.ML.Transforms` | 2025-11-11 | MIT | `netstandard2.0` | 88 types, 282 members | `NormalizeMinMax`, `NormalizeRobustScaling`, `NormalizeMeanVariance` and four more; `OneHotEncoding`, `OneHotHashEncoding`; `ReplaceMissingValues`, `IndicateMissingValues` |
| `SharpLearning.CrossValidation` 0.31.8 | 2020-07-12 | MIT, by `licenseUrl` to the repository | `net461`, `netstandard2.0` | 28 types, 70 members | `StratifiedIndexSampler<T>(seed).Sample(data, sampleSize)`, `StratifiedCrossValidation<T>(folds, seed)`, `RandomIndexSampler`, `NoShuffleIndexSampler`, `CrossValidationUtilities.GetKFoldCrossValidationIndexSets` |
| `SharpLearning.FeatureTransformations` 0.31.8 | 2020-07-12 | MIT, by `licenseUrl` | `net461`, `netstandard2.0` | 10 types, 28 members | `MinMaxTransformer` and `MeanZeroFeatureTransformer` over its `F64Matrix`; `OneHotTransformer`, `MapCategoricalFeaturesTransformer`, `ReplaceMissingValuesTransformer` over its CSV rows |
| `EasyCNTK` 0.4.2 | 2019-08-28 | MIT, by `licenseUrl` | `netstandard2.0` | — | the only NuGet hit for `smote`: a wrapper over `CNTK.GPU`, a framework Microsoft stopped developing in 2019 |

`mdabros/SharpLearning` has 411 stars and was last pushed on 2025-03-16, five years after its last
release. The GitHub search for SMOTE finds EasyCNTK's repository and three with no licence or two
stars.

**Stratified splitting has a .NET answer**: SharpLearning's sampler takes plain arrays of targets
and returns indices, on `netstandard2.0`, under MIT. What it does not do is reproduce scikit-learn —
it takes a seed and has no unshuffled mode — and it has not been released since 2020. ML.NET does
not stratify at all: `samplingKeyColumnName` keeps rows sharing a key on one side of the split, which
is grouping, and [dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396)
has asked for stratified folds since 2019 and is still open.

**SMOTE has no .NET answer.** The one package wraps a dead framework.

**Everything else exists, coupled to a framework.** ML.NET's scalers, encoders and imputer take an
`IDataView` and a column name; SharpLearning's take its own matrix and CSV row types.

## What the reference allows

A frozen corpus is how this repository proves parity, so what scikit-learn lets a caller pin down
decides what can be proven. Measured on scikit-learn 1.9.0:

- **`KFold` and `StratifiedKFold` without shuffle are deterministic.** Ten samples, three folds:
  `KFold` gives `[0,1,2,3] [4,5,6] [7,8,9]` and `StratifiedKFold` over six zeros and four ones gives
  `[0,1,6,7] [2,3,8] [4,5,9]`. Those are exact parity targets.
- **Every shuffled split draws from numpy's generator**, and `train_test_split(stratify=y,
  shuffle=False)` raises *"Stratified train/test split is not implemented for shuffle=False"*. The
  stratified allocation also breaks ties through `_approximate_mode(class_counts, n_draws, rng)`. A
  shuffled split can be compared only once the permutation is an input, as
  [`KMeansOptions.InitialCentres`](../reference/cluster/partitioning/kmeansoptions.md) made k-means
  comparable.
- **SMOTE draws its neighbours and its interpolation gaps from `random_state`**, and
  `imbalanced-learn` exposes no parameter that hands them in. No input makes a run reproducible from
  .NET — the reason [decision 0131](0131-lodestar-cluster-writes-what-netstandard2-0-lacks.md)
  gave for leaving `MiniBatchKMeans` unwritten — and `imbalanced-learn` is not in
  `tools/requirements.lock.txt`.
- **The scalers, encoders and imputer are deterministic** and are ordinary parity targets.

## Decision

| subject | verdict | lot |
| --- | --- | --- |
| `KFold`, `StratifiedKFold`, `train_test_split` | **written first** | [#762](https://github.com/CyrilB1531/lodestar/issues/762) |
| `MinMaxScaler`, `RobustScaler`, `MaxAbsScaler` | **written** | [#763](https://github.com/CyrilB1531/lodestar/issues/763) |
| `OneHotEncoder`, `OrdinalEncoder`, `SimpleImputer` | **written, on the framework-free argument alone** | [#764](https://github.com/CyrilB1531/lodestar/issues/764) |
| SMOTE and imbalanced resampling | **not written** until a caller needs it | — |
| `partial_fit`, a `CsrMatrix` overload | **kept out**, tracked | [#765](https://github.com/CyrilB1531/lodestar/issues/765) |

**The splitters come first, on the ground #680 gave and a narrower claim.** Not "no .NET answer":
no .NET splitter reproduces scikit-learn's folds, the maintained first-party one cannot stratify,
and this repository has a caller — `Lodestar.Conformal`'s guarantee assumes exchangeable calibration
and test data, which is a property of how the split was made. Unshuffled `KFold` and
`StratifiedKFold` are held to scikit-learn exactly; shuffled splits take the permutation as an input,
with a seed over this repository's generator documented as *not comparable*.

**The scalers are next because they are cheapest, not because anything is missing.** ML.NET has all
three; `Lodestar.Preprocessing` exists so that fitting a scaler does not require a pipeline, and a
package with `StandardScaler` alone makes that argument for one estimator.

**The encoders and the imputer are written, and the claim is stated at its real size.** ML.NET's
`OneHotEncoding` and `ReplaceMissingValues` work. What this package offers is the same answer on an
array, at scikit-learn's category order and unknown-value handling, and the reference page says
that is the whole of it.

**SMOTE waits.** The gap is the largest on the list and the least provable. Written without a
reproducible reference, it would be the one estimator here held to nothing but its own tests — the
reason [decision 0130](0130-mixed-models-have-no-incumbent-and-wait-for-a-caller.md) gave for mixed
models. A caller who needs it reopens it, and the lot starts from a conformance method a property
test can hold: every synthetic sample lies on a segment between a minority sample and one of its
`k` nearest minority neighbours, and the class counts come out as `sampling_strategy` says.

## Options that lost

- **Delegate stratified splitting to SharpLearning.** MIT, `netstandard2.0`, arrays in and indices
  out — closer to this package's shape than ML.NET is. It lost on reproducibility, not on quality: a
  seeded sampler with no unshuffled mode cannot give a caller scikit-learn's folds, and a caller
  porting a notebook wants those folds. The migration row names it for a caller who does not.
- **Take the list in #680's order, SMOTE second.** The order was set by how empty .NET is, and on
  that measure SMOTE is the emptiest. It lost because emptiness decides what is worth writing, not
  what can be proven, and a lot that cannot freeze a corpus does not start before one that can.
- **Write nothing more and close the package at `StandardScaler`.** Honest about ML.NET, and it
  would leave `Lodestar.Conformal` asking callers for a split no .NET library makes the way the
  reference makes it.

## Consequences

- `docs/equivalence.md`'s rows for `partial_fit`, sparse input and the remaining estimators point at
  #762 to #765, and SMOTE gains its own row with its reason.
- `docs/migration/sklearn.md` gains the splitter, scaler, encoder and resampling rows, with
  SharpLearning named for stratified splitting and ML.NET for the coupled estimators, and its
  preprocessing pitfall stops saying the gap is only a coupling.
