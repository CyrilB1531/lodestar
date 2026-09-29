# Changelog — Lodestar.Metrics

What changed in `Lodestar.Metrics`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- Precision, recall, the F-scores, Jaccard and `ClassificationReport` count per label in `O(n + k)` from the samples, where they built a `k × k` matrix that overflowed past 46,341 labels. ([#1200](https://github.com/CyrilB1531/lodestar/issues/1200))

### Fixed

- `MedianAbsoluteError` refuses a non-finite output weight again, binary `RocAuc` scores one class NaN whatever the weights, and one-vs-one weighted ROC AUC over one present class is refused as scikit-learn refuses it, where the parallel path failed on a degree of zero. ([#1564](https://github.com/CyrilB1531/lodestar/issues/1564), [#1565](https://github.com/CyrilB1531/lodestar/issues/1565), [#1566](https://github.com/CyrilB1531/lodestar/issues/1566), [#1567](https://github.com/CyrilB1531/lodestar/issues/1567), [#1568](https://github.com/CyrilB1531/lodestar/issues/1568))
- `Averaging.Binary` scores a batch without the positive class through `zeroDivision` and refuses only a target with more than two classes, or two without `posLabel`. ([#1201](https://github.com/CyrilB1531/lodestar/issues/1201))
- `Averaging.Binary` ignores `labels` on a binary target, as scikit-learn replaces it with `[pos_label]`. ([#1249](https://github.com/CyrilB1531/lodestar/issues/1249))
- The precision family and `ClassificationReport` score requested labels absent from `yTrue`, which only `ConfusionMatrix` and `CohenKappa` refuse, as `confusion_matrix` and `cohen_kappa_score` do. ([#1202](https://github.com/CyrilB1531/lodestar/issues/1202))
- `RocCurve`, `PrecisionRecallCurve` and `DetCurve` drop zero-weight samples before forming thresholds, and `RocCurve` answers a `NaN` axis, the origin included, for a class with no weight. ([#1203](https://github.com/CyrilB1531/lodestar/issues/1203))
- `CalibrationCurve` places its edges with numpy's `linspace` and `percentile`, bit for bit, so a probability on an edge falls in scikit-learn's bin. ([#1204](https://github.com/CyrilB1531/lodestar/issues/1204))
- `DaviesBouldin.Score` answers 0 when every spread or centroid distance is within `np.allclose` of zero, as `davies_bouldin_score` does. ([#1205](https://github.com/CyrilB1531/lodestar/issues/1205))
- The ranking, label-ranking, clustering-validity and curve scores refuse non-finite input, the curves and the area refuse weights that are zero throughout, `Silhouette.ScoreFromDistances` refuses a non-zero diagonal, `RocAuc.MultiClass` refuses a `yTrue` label outside `Labels`, and `MeanSquaredLogError` no longer overflows near `1e306`. ([#1206](https://github.com/CyrilB1531/lodestar/issues/1206))
- `LikelihoodRatios.Compute` replaces `LR-` when there is no true negative even without a positive sample, and refuses a target with one class or two without `posLabel`. ([#1250](https://github.com/CyrilB1531/lodestar/issues/1250))
- `LikelihoodRatios.Compute` computes both ratios from the counts, as scikit-learn does, rather than through `1 − specificity`. ([#1252](https://github.com/CyrilB1531/lodestar/issues/1252))
- `DetCurve.Compute` refuses a `yTrue` holding other than two classes, as `det_curve` does. ([#1251](https://github.com/CyrilB1531/lodestar/issues/1251))
- The comments, reference pages and `docs/equivalence.md` rows cite no decision record #1103 deleted and name no rule whose record is gone, stating each reason instead. ([#1396](https://github.com/CyrilB1531/lodestar/issues/1396), [#1474](https://github.com/CyrilB1531/lodestar/issues/1474), [#1475](https://github.com/CyrilB1531/lodestar/issues/1475), [#1476](https://github.com/CyrilB1531/lodestar/issues/1476))
- The regression scores refuse a NaN or infinite `outputWeights`, and `TweedieDeviance` and `D2Tweedie` a NaN or infinite `power`, as scikit-learn does. ([#1461](https://github.com/CyrilB1531/lodestar/issues/1461), [#1462](https://github.com/CyrilB1531/lodestar/issues/1462))
- `TopKAccuracy` refuses a non-finite score, and `CalibrationCurve` bins a NaN probability as `calibration_curve` does rather than refusing it. ([#1463](https://github.com/CyrilB1531/lodestar/issues/1463), [#1464](https://github.com/CyrilB1531/lodestar/issues/1464))
- `LogLoss.Score` refuses a `yTrue` holding one label, as `log_loss` does without `labels`, and `FBeta` scores `beta = inf` as the recall. ([#1465](https://github.com/CyrilB1531/lodestar/issues/1465), [#1466](https://github.com/CyrilB1531/lodestar/issues/1466))
- `ConfusionMatrix`, `DaviesBouldin`, one-vs-one `RocAuc`, `Silhouette` and the label union refuse a table past the largest array, and four shape checks multiply in `long`, where they wrapped. ([#1467](https://github.com/CyrilB1531/lodestar/issues/1467), [#1468](https://github.com/CyrilB1531/lodestar/issues/1468), [#1469](https://github.com/CyrilB1531/lodestar/issues/1469), [#1470](https://github.com/CyrilB1531/lodestar/issues/1470), [#1471](https://github.com/CyrilB1531/lodestar/issues/1471), [#1472](https://github.com/CyrilB1531/lodestar/issues/1472))
- The curves' lists are read-only views no cast writes through. ([#1473](https://github.com/CyrilB1531/lodestar/issues/1473))
- The average-precision, label-ranking and performance pages state the zero-weight results and the benchmark rows as they are. ([#1477](https://github.com/CyrilB1531/lodestar/issues/1477), [#1478](https://github.com/CyrilB1531/lodestar/issues/1478), [#1479](https://github.com/CyrilB1531/lodestar/issues/1479))
- The regression scores refuse output weights on a single output and sample weights that sum to zero, as scikit-learn does, `RootMeanSquaredError` and `MedianAbsoluteError` keeping the one it accepts. ([#1533](https://github.com/CyrilB1531/lodestar/issues/1533), [#1273](https://github.com/CyrilB1531/lodestar/issues/1273))
- Weighted average precision and one-vs-rest ROC AUC answer 0 when the weighted positives are within numpy's `isclose` of zero, and binary ROC AUC answers `NaN` for one class where it threw. ([#1534](https://github.com/CyrilB1531/lodestar/issues/1534), [#1277](https://github.com/CyrilB1531/lodestar/issues/1277))
- `RocAuc.Score`, `AveragePrecision.Score` and `LogLoss.Score` refuse a target of two labels without `posLabel`, and the first two one of three labels; `FBeta` refuses a finite beta whose square overflows; `LogLoss` and `BrierScore` refuse a non-finite probability with scikit-learn's sentence; `CalibrationCurve` refuses an `nBins` past one array. ([#1277](https://github.com/CyrilB1531/lodestar/issues/1277), [#1542](https://github.com/CyrilB1531/lodestar/issues/1542), [#1540](https://github.com/CyrilB1531/lodestar/issues/1540), [#1541](https://github.com/CyrilB1531/lodestar/issues/1541), [#1538](https://github.com/CyrilB1531/lodestar/issues/1538))
- `Silhouette.ScoreFromDistances` refuses a negative distance, `LabelIndex` takes its direct table only when it is dense, its union is bounded, and the expected mutual information's log-factorials are a compensated sum. ([#1275](https://github.com/CyrilB1531/lodestar/issues/1275), [#1280](https://github.com/CyrilB1531/lodestar/issues/1280), [#1539](https://github.com/CyrilB1531/lodestar/issues/1539), [#1281](https://github.com/CyrilB1531/lodestar/issues/1281))
- `CoverageError` loses its unreachable NaN path, and the pages and XML state the refusals #1480 added, the calibration's conditional range test and the performance ratios as they are, without pointing at a deleted rule; four tests pin the refusals they are named for. ([#1278](https://github.com/CyrilB1531/lodestar/issues/1278), [#1535](https://github.com/CyrilB1531/lodestar/issues/1535), [#1536](https://github.com/CyrilB1531/lodestar/issues/1536), [#1537](https://github.com/CyrilB1531/lodestar/issues/1537), [#1543](https://github.com/CyrilB1531/lodestar/issues/1543), [#1544](https://github.com/CyrilB1531/lodestar/issues/1544))

## [0.4.0] — 2026-09-24

### Changed

- `Averaging`, `BinStrategy`, `KappaWeighting`, `MultiClassStrategy`, `Normalization`, `ZeroDivision`, `AverageRow`, `ClassRow` and `UndefinedMetricException` are compiled into `Lodestar.Abstractions` under the same names and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- The confusion matrices, classifier curves, ranked-row scores, cluster validity scores and `ClassificationReport.Compute` make fewer passes and allocations over their input, up to 24× faster for unweighted `Accuracy.Score`. ([#850](https://github.com/CyrilB1531/lodestar/issues/850))
- `Silhouette.PerSample` sums each pair's distance per cluster instead of holding the n × n distance matrix. ([#815](https://github.com/CyrilB1531/lodestar/issues/815))
- The clustering agreement scores spread their contingency cells across the hash table, where a 100 × 100 table's cells shared 128 hash values. ([#812](https://github.com/CyrilB1531/lodestar/issues/812))
- `TopKAccuracy.Score` counts the true class's rank in each row instead of sorting the row, and allocates nothing per row. ([#827](https://github.com/CyrilB1531/lodestar/issues/827))
- `MeanSquaredError`, `MeanAbsoluteError` and `R2` read their input once to validate and score it. ([#715](https://github.com/CyrilB1531/lodestar/issues/715), [`a954161e`](https://github.com/CyrilB1531/lodestar/commit/a954161e))

### Fixed

- Macro and weighted precision, recall, F-scores, Jaccard and the report's average rows skip `NaN` classes and fall back to the unweighted mean on zero total support, as scikit-learn's `_nanaverage` does. ([#861](https://github.com/CyrilB1531/lodestar/issues/861))
- The classification metrics refuse a non-finite or all-zero `sampleWeight` and, where scikit-learn divides by it, a zero-sum one, `HingeLoss` refuses a non-finite decision, and the binary `LogLoss`, `BrierScore` and `HingeLoss` refuse a third label instead of counting it negative. ([#890](https://github.com/CyrilB1531/lodestar/issues/890))
- Weighted `JaccardScore.Score` refuses class supports that sum to zero without all being zero, as `jaccard_score`'s `numpy.average` does, where [#861](https://github.com/CyrilB1531/lodestar/issues/861) had it answer the unweighted mean its three sibling metrics reach through `_nanaverage`. ([#988](https://github.com/CyrilB1531/lodestar/issues/988))

## [0.3.0] — 2026-08-21

### Added

- `docs/guides/metrics.md` answers which metric to reach for, which the per-member reference pages deliberately cannot: a router across the four families, and the four things true of all of them — row-major input with a count, `sampleWeight` as a weighted mean, `ZeroDivision` as an argument rather than a warning, and the answers that look like bugs and are scikit-learn's. ([#203](https://github.com/CyrilB1531/lodestar/issues/203), [`8aaa19a`](https://github.com/CyrilB1531/lodestar/commit/8aaa19a))

### Added — ranking

- `Dcg`, `Ndcg` and `TopKAccuracy` score an ordered list of documents at scikit-learn parity, tie handling included: equal scores have their discounted gain averaged over the permutations of the tie by default, which on a row whose four scores are equal is `0.8069…` against `0.6138…` for `ignoreTies: true`. ([#173](https://github.com/CyrilB1531/lodestar/issues/173), [`8f3fda1`](https://github.com/CyrilB1531/lodestar/commit/8f3fda1))
- `ReciprocalRank` scores rankings by the position of their first relevant document — the one member of this package **not verified against a reference**, because `sklearn.metrics` has no counterpart to freeze; its definition is pinned by tests under [`docs/decisions/0036`](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0036-a-member-may-ship-without-an-oracle-if-it-says-so.md), which also says what would retire the exception. ([#173](https://github.com/CyrilB1531/lodestar/issues/173), [`8f3fda1`](https://github.com/CyrilB1531/lodestar/commit/8f3fda1))
- `CoverageError`, `LabelRankingLoss` and `LabelRankingAveragePrecision` score a boolean label matrix at scikit-learn parity, the two places the reference disagrees with itself included: a single label column is accepted by the average precision and refused by the other two, and a weight vector summing to zero gives `NaN` there where the other two raise. ([#201](https://github.com/CyrilB1531/lodestar/issues/201), [`eec79dd`](https://github.com/CyrilB1531/lodestar/commit/eec79dd))
- A sample with no relevant label contributes `0` to `CoverageError` rather than the label count, so its mean can sit below `1` — measured, `0.5` on two samples one of which is empty; a tie between a relevant and an irrelevant label counts as an error in `LabelRankingLoss`, so a sample whose scores are all equal scores `1`. ([#201](https://github.com/CyrilB1531/lodestar/issues/201), [`eec79dd`](https://github.com/CyrilB1531/lodestar/commit/eec79dd))
- `Dcg.Score`, `Ndcg.Score` and `TopKAccuracy.Score` take a `sampleWeight`, which the reference has always had and these three did not — three rows of `docs/equivalence.md` called them identical anyway. With weights `TopKAccuracy`'s `normalize: false` returns the **sum of the weights** of the hits rather than how many there are, measured `7.0` against the unweighted `3.0`, and because that path never divides it does not refuse a zero-sum vector at all, where the fraction does — what it returns there is the weighted sum of the hits, `3.0` on weights `[1, 1, 1, -3]` whose total is zero. ([#216](https://github.com/CyrilB1531/lodestar/issues/216), [`e2b62e3`](https://github.com/CyrilB1531/lodestar/commit/e2b62e3))

### Changed

- **Faster, same answers.** `MeanSquaredError`, `MeanAbsoluteError` and `RootMeanSquaredError` accumulate through `Vector<double>` on `net10.0` when there is a single output, which is the rule [decision 0027](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0027-r2-and-explainedvariance-vectorize-only-a-single-output.md) already set for `R2` and `ExplainedVariance`: **1.65×** on `mse` and **1.60×** on `mae` at a million rows, with `r2` re-run as an untouched control. The lanes reduce in a different order from a scalar loop, so the values can differ in their last bits; the frozen scikit-learn corpora pass unchanged at their `1e-9` comparison. ([#321](https://github.com/CyrilB1531/lodestar/issues/321), [`36ec36e`](https://github.com/CyrilB1531/lodestar/commit/36ec36e))
- **Numerical change, under `1e-14`.** `NormalizedMutualInformation`, `Homogeneity`, `Completeness` and `VMeasure` return slightly different values on inputs where one labelling is a single cluster: the shared mutual-information term now zeroes each contribution below the machine epsilon before summing and returns `0.0` outright when either side has one label, both of which the reference does. The old values were up to `5.13e-15` from scikit-learn's and the new ones are exact, so anything comparing at the corpus tolerance of `1e-9` is unaffected — this is recorded because the values moved, not because a caller should have to react. ([#191](https://github.com/CyrilB1531/lodestar/issues/191), [`43b4368`](https://github.com/CyrilB1531/lodestar/commit/43b4368))

### Fixed — ranking

- `Dcg.Score` refuses a `logBase` outside `(0, ∞)` instead of returning a silent `NaN`: zero, a negative, `NaN` and infinity now raise `ArgumentOutOfRangeException`, which is where `dcg_score` raises too. A base below `1` is still accepted, and still takes the score negative. ([#215](https://github.com/CyrilB1531/lodestar/issues/215), [`1eff5e5`](https://github.com/CyrilB1531/lodestar/commit/1eff5e5))

## [0.2.0] — 2026-08-16

### Added

- `docs/reference/metrics/classification.md` and `docs/reference/metrics/regression.md` document every type of `Lodestar.Metrics` in the layout of the .NET API reference, and the same test checks each declaration, parameter list and `Applies to` against the assembly. ([#181](https://github.com/CyrilB1531/data.net/issues/181), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))

### Added — clustering

- `AdjustedRand`, `NormalizedMutualInformation`, `Homogeneity`, `Completeness` and `VMeasure` score a clustering against a reference partition at scikit-learn parity, degenerate cases included: an empty input and a single sample both score `1`, and two independent partitions score `-0.5` on adjusted Rand. ([#172](https://github.com/CyrilB1531/data.net/issues/172), [`3d10674`](https://github.com/CyrilB1531/lodestar/commit/3d10674))
- `Silhouette` scores a clustering with no reference partition, from the samples with the euclidean distance or from a distance matrix already computed, per sample or as their mean. ([#172](https://github.com/CyrilB1531/data.net/issues/172), [`714dd80`](https://github.com/CyrilB1531/lodestar/commit/714dd80))

### Changed

- The reference is one page per member, with a type page and a namespace index above it: the two documents above become 31 type pages and 42 member pages, and the index a reader lands on is 102 lines rather than 1646. ([#189](https://github.com/CyrilB1531/data.net/issues/189), [`754a61d`](https://github.com/CyrilB1531/lodestar/commit/754a61d))
- The package is `Lodestar.Metrics`, and its namespaces are `Lodestar.Metrics.*`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

## [0.1.0] — 2026-08-14 (published as DataNet.Metrics)

First release of a fourth package.

### Added

- Classification metrics at scikit-learn parity: `ConfusionMatrix`, `Accuracy`, `Precision`, `Recall`, `F1`, `FBeta`, `ClassificationReport` and `RocAuc`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- All four averaging modes — `Averaging.Binary`, `Micro`, `Macro` and `Weighted` — are an enum instead of a string, with `average=None` becoming a separate `PerClass` method. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `ClassificationReport` comes in both shapes: structured rows a program can read, and `ToText(digits)` reproducing `classification_report`'s printed output character for character. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `RocAuc.Score` mirrors `_binary_clf_curve`'s sort-and-accumulate, and `RocAuc.MultiClass` covers both `ovr` and Hand & Till's `ovo`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `ZeroDivision.Zero`, `One`, `NaN` or `Throw` give an explicit, caller-chosen answer for the 0/0 case scikit-learn silently defaults and warns on. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- `sampleWeight` is threaded throughout, which is why matrix cells and support figures are `double` rather than `int`. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- All 29 operations are measured at or above 1× scikit-learn's processor time rather than merely asserted, narrowest margin 2.74×. ([`3355f94`](https://github.com/CyrilB1531/data.net/commit/3355f94))
- Opt-in parallelism for multiclass ROC-AUC: `RocAuc.MultiClass(…, new MultiClassRocOptions { MaxDegreeOfParallelism = … })`, sequential by default and bit-identical either way. ([#86](https://github.com/CyrilB1531/data.net/issues/86), [`a2cae2b`](https://github.com/CyrilB1531/data.net/commit/a2cae2b))
- At n=100 000, k=10, on four physical cores, one-vs-rest drops from 76 ms sequential to 27 ms at eight workers, and one-vs-one from 127 ms to 37 ms at four. ([#86](https://github.com/CyrilB1531/data.net/issues/86), [`a2cae2b`](https://github.com/CyrilB1531/data.net/commit/a2cae2b))
- Balanced accuracy, Matthews correlation and Cohen's kappa — `BalancedAccuracy.Score`, `MatthewsCorrelation.Score` and `CohenKappa.Score` — each from labels or from an already-built `ConfusionMatrix`. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- `confusion_matrix(…, normalize=…)` is a projection: `ConfusionMatrix.ToArray(Normalization.None/True/Pred/All)` returns scaled cells without the matrix itself remembering it was normalized. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- `ZeroDivision` keeps a faithful default per metric rather than one across the package — `Zero` for precision, recall, F1, F-beta, the report and Matthews correlation; `NaN` for Cohen's kappa. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- 18 new cross-language rows — three operations over six shapes — are at or above 1× scikit-learn's processor time, narrowest margin 16.59× on `balanced_accuracy` at n=1 000 000. ([`d00294a`](https://github.com/CyrilB1531/data.net/commit/d00294a))
- Regression metrics at scikit-learn parity: `MeanSquaredError`, `RootMeanSquaredError`, `MeanAbsoluteError`, `MedianAbsoluteError`, `MeanAbsolutePercentageError`, `MeanSquaredLogError`, `RootMeanSquaredLogError`, `MaxError`, `R2`, `ExplainedVariance` and `PinballLoss`. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- `multioutput=` is spelled by choosing a method: `Score(…)` is `uniform_average`, `PerOutput(…)` is `raw_values`, and `VarianceWeighted(…)` is `variance_weighted` on `R2` and `ExplainedVariance`. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- The undefined cases are two knobs, not one: `forceFinite` answers zero variance over two or more samples, and `R2`'s `ZeroDivision` separately answers fewer than two samples. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))
- The weighted median averages within one machine epsilon rather than exactly, matching scikit-learn's own overshoot test against `np.finfo(float64).eps`. ([`859da5c`](https://github.com/CyrilB1531/data.net/commit/859da5c))
- Two refusals taken from `check_array` and from `numpy.average`: a `sampleWeight` that is zero throughout, and `outputWeights` that sum to zero. ([`2216d5b`](https://github.com/CyrilB1531/data.net/commit/2216d5b))
- `log(1 + x)` is computed as `log1p`, using Kahan's identity, in `MeanSquaredLogError` and `RootMeanSquaredLogError`. ([`2216d5b`](https://github.com/CyrilB1531/data.net/commit/2216d5b))
- `R2`'s two passes, `ExplainedVariance`'s five accumulations, and `Outputs.WeightedMean` now sum with Neumaier compensation rather than a running total. ([#127](https://github.com/CyrilB1531/data.net/issues/127), [`fcb705b`](https://github.com/CyrilB1531/data.net/commit/fcb705b))
- `mse`, `mae`, `median_ae` and `r2` were benchmarked against scikit-learn over six shapes; `median_ae` is the one operation below the 1× processor-time gate, at 0.80–0.90×. ([#92](https://github.com/CyrilB1531/data.net/issues/92), [`641f098`](https://github.com/CyrilB1531/data.net/commit/641f098))

### Changed

- `DataNet.Metrics`'s long comment blocks became ten decision records, so the reasoning lives where it can be cited instead of duplicated at each call site. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`d4d9326`](https://github.com/CyrilB1531/data.net/commit/d4d9326))
- The Neumaier-versus-Kahan argument for `CompensatedSum` moved into a record of its own instead of living only as comments in the source. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))
- `MultiClassRocOptions`'s doc comments no longer restate `docs/decisions/0018`, and `Normalization`'s comment points at `0020` instead of repeating it. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))
- The rest of the package's remaining long comments were trimmed to their reason, with no behaviour changed. ([#151](https://github.com/CyrilB1531/data.net/issues/151), [`4abb609`](https://github.com/CyrilB1531/data.net/commit/4abb609))
