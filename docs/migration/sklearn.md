# scikit-learn → .NET

**Verdict: use** ML.NET (or SharpLearning for a sklearn-like API), **except text
vectorization**, which is the gap filled natively by `Lodestar.Text` (exact
`CountVectorizer`/`TfidfVectorizer` semantics), and the two `sklearn.decomposition`
estimators that work on a sparse matrix.

| sklearn need | Recommended .NET |
| --- | --- |
| Pipelines, training, deployment | **ML.NET** (`Microsoft.ML`) |
| sklearn-like API (trees, ensembles) | **SharpLearning** |
| `CountVectorizer` / `TfidfVectorizer` **to the character** | **`Lodestar.Text`** |
| `classification_report`, `roc_auc_score`, the averaging modes | **`Lodestar.Metrics`** |
| `TruncatedSVD`, `NMF(solver="mu")` on a sparse matrix | **`Lodestar.Decomposition`** |
| `PCA` on a dense matrix | **ML.NET** `ProjectToPrincipalComponents` on any target, or [NumFlat](https://www.nuget.org/packages/NumFlat) `PrincipalComponentAnalysis` on `net8.0`+, or [Meta.Numerics](https://www.nuget.org/packages/Meta.Numerics) `PrincipalComponentAnalysis` (MS-PL, `netstandard2.0`). Not `Lodestar.Decomposition`: centring densifies a `CsrMatrix`, so PCA is refused for sparse input by name ([decision 0004](../decisions/0004-what-is-written-here-and-what-is-delegated.md)) |
| `PCA().explained_variance_ratio_` | `Lodestar.Decomposition` [`PrincipalComponentVariance.Compute`](../reference/decomposition/factorization/principalcomponentvariance-compute.md) on any target, at scikit-learn parity — ML.NET's fourteen public PCA members carry no eigenvalue, and NumFlat's `EigenValues` ships `net8.0` only ([decision 0003](../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)). Meta.Numerics' `PrincipalComponentAnalysis` reports it on `netstandard2.0` too, 37× to 809× slower and refusing a matrix wider than tall ([performance](../guides/performance.md#metanumerics-against-lodestarstats-and-principalcomponentvariance-issue-756)) |
| `StandardScaler` on arrays rather than on an `IDataView` | **`Lodestar.Preprocessing`**, with the whole scaler surface: [`StandardScaler`](../reference/preprocessing/scaling/standardscaler.md), `MinMaxScaler`, `MaxAbsScaler` and `RobustScaler`, fitted whole, [over batches](../reference/preprocessing/scaling/standardscaler-partialfit.md) or over a `CsrMatrix` where the reference accepts one |
| `KFold`, `StratifiedKFold`, `train_test_split` | **native** — [`Splitters`](../reference/preprocessing/splitting/splitters.md) in `Lodestar.Preprocessing`, at scikit-learn's unshuffled folds exactly, with the permutation as an argument rather than a seed. ML.NET `TrainTestSplit`/`CrossValidationSplit` group by key and never stratify ([dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396), open since 2019); [SharpLearning.CrossValidation](https://www.nuget.org/packages/SharpLearning.CrossValidation) `StratifiedIndexSampler` stratifies over arrays but always shuffles from a seed; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `MinMaxScaler`, `RobustScaler`, `MaxAbsScaler` on arrays | **native** — [`MinMaxScaler`](../reference/preprocessing/scaling/minmaxscaler.md), [`MaxAbsScaler`](../reference/preprocessing/scaling/maxabsscaler.md) and [`RobustScaler`](../reference/preprocessing/scaling/robustscaler.md) in `Lodestar.Preprocessing`, spans in and arrays out, at scikit-learn parity including the `range < 10·eps` floor and `numpy.percentile`'s linear interpolation. Inside a pipeline, **ML.NET** `NormalizeMinMax`, `NormalizeRobustScaling`; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `OneHotEncoder`, `OrdinalEncoder`, `SimpleImputer` on arrays | **native** — [`Encoders`](../reference/preprocessing/encoding/encoders.md) and [`SimpleImputer`](../reference/preprocessing/encoding/simpleimputer.md) in `Lodestar.Preprocessing`, spans in and arrays out, at scikit-learn parity including the code-point category order and the `most_frequent` tie rule. Inside a pipeline, **ML.NET** `OneHotEncoding`, `MapValueToKey`, `ReplaceMissingValues`; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `imblearn` SMOTE and resampling | ⚠️ **gap, not scheduled** — nothing maintained in .NET, and no reproducible reference to hold one to; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `KMeans(algorithm="lloyd")` on arrays | **`Lodestar.Cluster`** [`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) on any target — ahead of NumFlat and Meta.Numerics on the same data ([performance](../guides/performance.md#k-means-against-numflat-and-metanumerics-issue-681)) |
| `DBSCAN` | **`Lodestar.Cluster`** [`Dbscan.Fit`](../reference/cluster/partitioning/dbscan-fit.md) on any target — n-dimensional where [Dbscan](https://www.nuget.org/packages/Dbscan) is planar, and ahead of [NumFlat](https://www.nuget.org/packages/NumFlat) on the same data ([performance](../guides/performance.md#dbscan-against-numflat-and-dbscan-issue-759)); [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `AgglomerativeClustering` | **`Lodestar.Cluster`** [`AgglomerativeClustering.Fit`](../reference/cluster/partitioning/agglomerativeclustering-fit.md) on any target, ties included. [Aglomera](https://www.nuget.org/packages/Aglomera) (MIT, `netstandard1.3`, last released 2020) agrees only where no two merge heights tie, and reports Ward on another scale ([performance](../guides/performance.md#agglomerative-clustering-against-aglomera-issue-760)); [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `HDBSCAN` | [HdbscanSharp](https://www.nuget.org/packages/HdbscanSharp) (MIT, `netstandard2.0`) — not compared with scikit-learn here; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `GaussianMixture`, k-medoids | [NumFlat](https://www.nuget.org/packages/NumFlat) `GaussianMixtureModel`, `KMedoids<T>` on `net8.0`+. Below it, ⚠️ **gap** for the mixture; k-medoids is `scikit-learn-extra`, not scikit-learn; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |
| `MiniBatchKMeans`, `SpectralClustering` | ⚠️ **gap, not scheduled** — mini-batch draws its batches inside the fit, so no run is reproducible from .NET; spectral needs an eigensolver this repository does not have; [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md) |

```bash
dotnet add package Microsoft.ML
```

```csharp
using Microsoft.ML;

var ml = new MLContext(seed: 0);
IDataView data = ml.Data.LoadFromTextFile<Row>("data.csv", hasHeader: true, separatorChar: ',');
var pipeline = ml.Transforms.Concatenate("Features", "f1", "f2")
    .Append(ml.Regression.Trainers.Sdca(labelColumnName: "Label"));
var model = pipeline.Fit(data);
```

## Pitfalls

- **`TfidfVectorizer` is non-standard.** The sklearn formula (`smooth_idf`,
  per-row L2 normalization) must be reproduced to the character — ML.NET's
  `FeaturizeText` does not reproduce it. That is exactly the reason for
  `Lodestar.Text`. See [`../equivalence.md`](../equivalence.md).
- **`min_df` / `max_df`, n-gram bounds**: on the Lodestar side, not ML.NET.
- **Preprocessing is mostly a coupling gap, not an absence.** ML.NET has
  `NormalizeMeanVariance`, `NormalizeMinMax`, `OneHotEncoding`, `ReplaceMissingValues`,
  `TrainTestSplit` and `CrossValidationSplit` — read on `Microsoft.ML` 5.0.0's exported
  surface, every one of them is reached through an `IDataView` or through a catalog naming
  columns. `NormalizeMeanVariance(TransformsCatalog, string inputColumn, string
  outputColumn, …)` never sees a value. `Lodestar.Preprocessing` answers the entry point,
  not the capability: a caller holding a `double[]` gets a `double[]` back. Two exceptions
  are real: ML.NET cannot **stratify** a split — `samplingKeyColumnName` groups — and nothing
  in .NET does **SMOTE**. [`decisions/0004`](../decisions/0004-what-is-written-here-and-what-is-delegated.md)
  has the reading and the order the rest is written in; until then, ML.NET remains the answer
  if you are already inside a pipeline.

## Metrics: the averaging mode is not a formatting choice

This is the pitfall that used to read "check the definitions before comparing to
sklearn", which names the trap without getting anyone out of it.

`precision_score(y_true, y_pred, average=…)` returns a different **number**, not
a different presentation, for each mode. On an imbalanced problem the modes do
not disagree slightly — they disagree by a factor of two, and every one of them
is arithmetically correct.

A worked example, taken from this repository's own oracle corpus
(`binary_imbalanced`: 190 samples of class 0, 10 of class 1, a classifier with
30 % label noise). Its confusion matrix is `[[133, 57], [4, 6]]`, so the model
finds 6 of the 10 positives and calls 57 negatives positive:

| Class | Precision | Recall | F1 | Support |
| --- | ---: | ---: | ---: | ---: |
| 0 | 0.971 | 0.700 | 0.813 | 190 |
| 1 | 0.095 | 0.600 | 0.164 | 10 |

| `average=` | Precision | Recall | F1 | What it means |
| --- | ---: | ---: | ---: | --- |
| `"micro"` | 0.695 | 0.695 | 0.695 | Pool every sample, then score once. On a full label set this **is** accuracy. |
| `"macro"` | 0.533 | 0.650 | **0.489** | Mean of the per-class scores. The 10-sample class weighs exactly as much as the 190-sample one. |
| `"weighted"` | 0.927 | 0.695 | **0.781** | Mean of the per-class scores weighted by support. The majority class dominates. |
| `"binary"` | 0.095 | 0.600 | 0.164 | Not an average: class `posLabel` alone, ignoring the other. sklearn's default. |

Macro F1 says 0.489, weighted F1 says 0.781, for one model on one dataset. Report
either without naming the mode and the reader learns nothing. The two are
answering different questions: macro asks how the model does on a class picked at
random, weighted asks how it does on a *sample* picked at random.

In C#, the mode is an enum rather than a string, so a typo is a compile error
instead of a `ValueError` at the end of a run. One
[`ConfusionMatrix.Compute`](../reference/metrics/classification/confusionmatrix-compute.md)
pass feeds both
[`F1.Score`](../reference/metrics/classification/f1-score.md) and
[`ClassificationReport.Compute`](../reference/metrics/classification/classificationreport-compute.md):

```csharp
using Lodestar.Metrics;

ConfusionMatrix cm = ConfusionMatrix.Compute(yTrue, yPred);   // one O(samples) pass
double macro    = F1.Score(cm, Averaging.Macro);              // 0.489
double weighted = F1.Score(cm, Averaging.Weighted);           // 0.781
double[] perClass = F1.PerClass(cm);                          // [0.813, 0.164]

Console.WriteLine(ClassificationReport.Compute(cm).ToText()); // what sklearn prints
```

Two differences from the Python spelling are deliberate. `average=None` becomes
[`F1.PerClass`](../reference/metrics/classification/f1-perclass.md), a method,
because it returns one value per class rather than a scalar — an enum member
cannot change its method's return type. And
`Averaging.Binary` throws on a target with more than two classes instead of
guessing which class was meant. Both are recorded in
[`../decisions/0003`](../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md).

**Absent classes.** A class with no predictions gives 0/0. sklearn returns 0 and
emits an `UndefinedMetricWarning`; a warning is easy to miss in a log and has no
natural .NET equivalent. `Lodestar.Metrics` makes the choice explicit —
`ZeroDivision.Zero` (sklearn's value), `One`, `NaN`, or `Throw`, which raises
`UndefinedMetricException` rather than letting a silent 0 flow into a report.

Every function, with its sklearn call and its deliberate divergences, is in
[`../equivalence.md`](../equivalence.md).

```bash
dotnet add package Lodestar.Metrics
```

*Guide to be expanded as real needs arise.*
