# The seven `sklearn.preprocessing` members that were never decided

**Issue:** [#1122](https://github.com/CyrilB1531/lodestar/issues/1122).
**Status:** written before the work, 2026-09-23.
**Date:** 2026-09-23.

## The problem

`Lodestar.Preprocessing` ships four scalers, two encoders, one imputer and the splitters.
`Normalizer`, `KBinsDiscretizer`, `PolynomialFeatures`, `QuantileTransformer`,
`PowerTransformer`, `LabelEncoder` and `KNNImputer` appear nowhere: not as a written type, not as
a delegation in [decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md),
and not as a row in `docs/equivalence.md`. Measured against its reference this is the thinnest
package in the repository, and [#680](https://github.com/CyrilB1531/lodestar/issues/680) closed the
splitter half of that scope while leaving this half unstated.

The issue asks for the reading decision 0004 applies to each of the seven — *is there a maintained
.NET answer at the reference's parity?* — and for a member decided against to be recorded as a
void with its reopening condition rather than left silent.

## What was measured

### What .NET answers, member by member

Read by reflection out of the restored assemblies, and searched on NuGet with the search API
checked against known packages first, so an empty result means empty rather than broken.

| member | .NET answer | shape |
| --- | --- | --- |
| `Normalizer` | `Microsoft.ML` `NormalizeLpNorm` — `L1`, `L2`, `Infinity`, `StandardDeviation` | `IDataView` pipeline, named columns |
| `LabelEncoder` | `Microsoft.ML` `MapValueToKey` | the same |
| `KBinsDiscretizer` | `Microsoft.ML` `NormalizeBinning` — **but it normalises into `[0, 1]`**, where the reference emits a bin index or a one-hot row | the same, and a different answer |
| `PolynomialFeatures` | **none** | — |
| `QuantileTransformer` | **none** | — |
| `PowerTransformer` | **none** | — |
| `KNNImputer` | **none** — `ReplaceMissingValues` is mean, mode or a default | — |

`SharpLearning.FeatureTransformations` (121,489 downloads) exports ten types — `LinearNormalizer`,
`MinMaxTransformer`, `MeanZeroFeatureTransformer`, `OneHotTransformer`,
`MapCategoricalFeaturesTransformer`, `ReplaceMissingValuesTransformer` and three more. They are
**the members this package has already written**, and none of the seven. `NumFlat`'s
`PolynomialKernel` is a kernel function, not a feature expansion.

NuGet returns nothing at all for `yeo-johnson`, `quantile transformer`, `knn imputer`,
`imputation` or `discretizer`; `polynomial features` returns one Avalonia charting control. The
search itself works — `Accord.Statistics`, `NumFlat` and `SharpLearning` all come back — so those
zeroes are measurements rather than a broken query.

**The precedent is already settled here, which is what decides six of the seven.** ML.NET *and*
SharpLearning both carry `MinMaxScaler`, `RobustScaler`, `OneHotEncoder` and `SimpleImputer`
equivalents, and this package wrote all four anyway; `docs/migration/sklearn.md` records both
routes in the same row — *"**native** … Inside a pipeline, **ML.NET** …"*. A pipeline over an
`IDataView` has never counted as parity for a package whose thesis is spans in, arrays out.

### `QuantileTransformer` is unreproducible at its own default

`subsample=10000` by default. At 20,000 rows, two seeds produce quantiles differing by **0.19** —
a fifth of a standard deviation, not a rounding difference. That is the trap decision 0004 already
recorded for `nndsvdar`: *"it draws from numpy's Gaussian stream, so it could not be checked"*.
With `subsample=None`, or below the threshold, it is deterministic.

So the member is written **without a `subsample` parameter at all**: always the whole sample, which
is the reference's `subsample=None`. A narrower API that is provable, rather than a wider one that
is not, and the equivalence row says which call it corresponds to.

### `PowerTransformer` leaves the `1e-9` standard, and the reason is the objective's own resolution

The issue expects this member to "earn the most argument", and it does — but not for the reason it
suggests. Measured: `PowerTransformer`'s lambda agrees with `scipy.stats.yeojohnson_normmax` and
`boxcox_normmax` **to the last bit**, both methods. There is no disagreement between two
optimisers to reproduce or to refuse.

What there is instead is a flat objective. At the optimum the negative log-likelihood has curvature
about `176`, so `f(λ) - f(λ*) ≈ 88·(λ - λ*)²`. The objective itself is around `443`, which a double
carries to roughly `3e-11` absolute. Solving `88·δ² = 3e-11`:

> **the data determines λ only to about ±6×10⁻⁷**, and moving λ by that much moves the transformed
> values by **9.1×10⁻⁷ relative**.

Both numbers are reproduced by evaluating `scipy.stats.yeojohnson_llf` around the optimum, so a
reader can check the claim without trusting it. No implementation can do better from this
objective, and no corpus can honestly claim `1e-9` on λ or on the values it produces.

**This family therefore ships with a stated tolerance of `1e-5`**, which is two orders of magnitude
inside what the measurement allows and two outside
[decision 0005](../../decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)'s
standard. `docs/equivalence.md` and the reference page carry the measurement and the arithmetic,
not the conclusion alone. It is not a void: the reference agrees with itself, a caller is served,
and the limit is a property of the problem that the documentation can state exactly.

### The rest are deterministic and exactly checkable

`LabelEncoder`'s class order is `OrdinalEncoder`'s — code points, `B` before `a` before `b` before
`é` — so `Internal.CategoryLookup` already holds the machinery. `PolynomialFeatures`' contract is
its column order, `['1', 'x0', 'x1', 'x0^2', 'x0 x1', 'x1^2']` at degree two, and
`['x0', 'x1', 'x2', 'x0 x1', 'x0 x2', 'x1 x2', 'x0 x1 x2']` under `interaction_only`.
`KNNImputer` is `nan_euclidean` distances and a weighted mean of the neighbours that have the
feature. `KBinsDiscretizer`'s `quantile` strategy reads `numpy.percentile`'s linear interpolation,
which `RobustScaler` already implements here.

### Two edges, one taken and one refused

**`KBinsDiscretizer`'s `kmeans` strategy takes an edge to `Lodestar.Cluster`.** The reference runs
Lloyd's algorithm on the one-dimensional column from centres seeded at the uniform strategy's bin
midpoints, and `KMeansOptions.InitialCentres` is **already published**, so the edge needs no new
API from that package and no release order. Preferring an edge to re-writing a published member is
the rule [#763](https://github.com/CyrilB1531/lodestar/issues/763) settled.

**`Normalizer` does not take one to `Lodestar.Abstractions`.** The obvious route —
`SparseNorm.Max` beside `L1` and `L2`, then `CsrMatrix.NormalizeRows` — cannot merge:
`CONTRIBUTING.md`'s *Working across two packages* states that a branch whose downstream package
needs new upstream API **cannot go green**, because CI asserts the `PackageReference` path against
the published floor. It would take a release of `Lodestar.Abstractions` first.

It is also the wrong shape. `NormalizeRows` mutates in place, where every member of this package
promises never to write to its input. So `Normalizer` declares its own `RowNorm` and computes the
row norms from `Values`, `ColumnIndices` and `RowPointers` — all public on the published
`CsrMatrix` — returning a **new** matrix. `SparseNorm.Max` remains a reasonable addition to
`Lodestar.Abstractions` on its own terms, for a caller who wants `NormalizeRows` to do it; it is
left for its own issue rather than made a precondition of this one.

## What ships

```csharp
namespace Lodestar.Preprocessing;

public enum RowNorm { L1, L2, Max }
public enum BinStrategy { Uniform, Quantile, KMeans }
public enum BinEncoding { Ordinal, OneHot }
public enum PowerMethod { YeoJohnson, BoxCox }
public enum QuantileOutput { Uniform, Normal }
public enum NeighbourWeights { Uniform, Distance }

public static class Normalizer
{
    public static double[] Transform(ReadOnlySpan<double> samples, int featureCount, RowNorm norm = RowNorm.L2);
    public static CsrMatrix Transform(CsrMatrix matrix, RowNorm norm = RowNorm.L2);
}

public sealed class LabelEncoder<T>
    where T : IComparable<T>, IEquatable<T>
{
    public IReadOnlyList<T> Classes { get; }
    public int[] Transform(ReadOnlySpan<T> labels);
    public T[] InverseTransform(ReadOnlySpan<int> codes);
}

public static class Encoders
{
    public static LabelEncoder<T> Label<T>(ReadOnlySpan<T> labels)
        where T : IComparable<T>, IEquatable<T>;
}

public static class PolynomialFeatures
{
    public static double[] Transform(ReadOnlySpan<double> samples, int featureCount, PolynomialFeaturesOptions? options = null);
    public static string[] FeatureNames(int featureCount, PolynomialFeaturesOptions? options = null);
    public static int OutputFeatureCount(int featureCount, PolynomialFeaturesOptions? options = null);
}

public sealed class KBinsDiscretizer
{
    public static KBinsDiscretizer Fit(ReadOnlySpan<double> samples, int featureCount, KBinsDiscretizerOptions? options = null);
    public IReadOnlyList<IReadOnlyList<double>> BinEdges { get; }
    public IReadOnlyList<int> BinCounts { get; }
    public int OutputFeatureCount { get; }
    public double[] Transform(ReadOnlySpan<double> samples);
    public double[] InverseTransform(ReadOnlySpan<double> encoded);
}

public sealed class QuantileTransformer
{
    public static QuantileTransformer Fit(ReadOnlySpan<double> samples, int featureCount, QuantileTransformerOptions? options = null);
    public IReadOnlyList<double> References { get; }
    public IReadOnlyList<IReadOnlyList<double>> Quantiles { get; }
    public double[] Transform(ReadOnlySpan<double> samples);
    public double[] InverseTransform(ReadOnlySpan<double> samples);
}

public sealed class PowerTransformer
{
    public static PowerTransformer Fit(ReadOnlySpan<double> samples, int featureCount, PowerTransformerOptions? options = null);
    public IReadOnlyList<double> Lambdas { get; }
    public double[] Transform(ReadOnlySpan<double> samples);
    public double[] InverseTransform(ReadOnlySpan<double> samples);
}

public sealed class KnnImputer
{
    public static KnnImputer Fit(ReadOnlySpan<double> samples, int featureCount, KnnImputerOptions? options = null);
    public int OutputFeatureCount { get; }
    public IReadOnlyList<int> KeptFeatures { get; }
    public double[] Transform(ReadOnlySpan<double> samples);
}
```

`Normalizer` and `PolynomialFeatures` are static: nothing is fitted, so there is no state for an
instance to carry and no `Fit` that would only return `this` in disguise. The other five are
fitted, and follow `StandardScaler`'s shape exactly — a private constructor, a static `Fit`, the
fitted statistics readable, and `Transform` never writing to its input.

`KnnImputer` rather than `KNNImputer`: the repository's own naming (`BkTree`, `Bm25Index`,
`OnnxTextEmbedder`) capitalises an initialism as a word.

Every fitted type also carries `FeatureCount` and `SampleCount`, the shape it was fitted on, as
the scalers already do.

### Three things this section got wrong before the work

- **`LabelEncoder` is generic**, `LabelEncoder<T>` fitted through `Encoders.Label<T>`, not a
  string-only type with its own `Fit`. The reference takes any dtype, the two encoders already
  here are generic, and CA1000 refuses a public static on a generic type — which is why
  `Encoders` is the factory for all three.
- **`KBinsDiscretizer`'s quantile strategy does not read `numpy.percentile`'s linear
  interpolation.** scikit-learn 1.9 deprecated leaving `quantile_method` unstated and defaults it
  to `averaged_inverted_cdf`, which gives different edges — `2.5` and `4.5` over `1..6` cut in
  three, where linear gives `2.667` and `4.333`. Both are offered, with the reference's default.
- **`KnnImputer` drops a feature missing from every fitted row**, so its output can be narrower
  than its input; that is `keep_empty_features=False`, the reference's own default. It was listed
  under *What is not written* as pipeline plumbing, which was a misreading: the flag is absent,
  but the behaviour it turns off is what ships. `OutputFeatureCount` and `KeptFeatures` say which
  features a transformed row carries.

## What is not written

- **`Normalizer(norm='max')` over a `CsrMatrix` through `SparseNorm`.** The enum belongs to
  `Lodestar.Abstractions` and adding a member there is a release ahead of this branch; the
  reasoning is above, and the addition is left for its own issue.
- **`QuantileTransformer`'s `subsample`.** Measured unreproducible; the member covers
  `subsample=None` and says so.
- **`KBinsDiscretizer`'s `subsample`** and `random_state`, for the same reason and with the same
  scope: the default is 200,000 rows, so the reference does not subsample below that either.
- **`PolynomialFeatures(order='F')`**, which is numpy's memory layout rather than a different
  answer.
- **`KNNImputer`'s `add_indicator` and `keep_empty_features`**, which are pipeline plumbing rather
  than imputation, and `metric` — `nan_euclidean` is the only one the reference implements.

## Proof

Seven corpora, frozen from scikit-learn 1.9.1 per decision 0005.

| corpus | tolerance | what it holds |
| --- | --- | --- |
| `preprocessing_normalizer.json` | `1e-9` | the three norms, dense and sparse, a zero row left alone |
| `preprocessing_label_encoder.json` | exact strings, exact codes | code-point order, an unseen label refused, round trip |
| `preprocessing_polynomial.json` | `1e-9`, names exact | degrees 1 to 4, `interaction_only`, `include_bias`, one feature and five |
| `preprocessing_kbins.json` | `1e-9` | the three strategies by the two encodings, bin edges included, a constant feature |
| `preprocessing_quantile.json` | `1e-9` | both outputs, `subsample=None` throughout, values below and above the fitted range |
| `preprocessing_power.json` | **`1e-5`, stated** | both methods, standardised and not, negative and zero values under Yeo-Johnson |
| `preprocessing_knn_imputer.json` | `1e-9` | both weightings, a row missing every feature, a column missing everywhere |

The `PowerTransformer` corpus carries the curvature measurement in its generator docstring, so the
tolerance is derivable from the corpus rather than asserted beside it.

## Implementation order

Written here rather than in a tracked plan file: `docs/superpowers/plans/` stays empty
([#1104](https://github.com/CyrilB1531/lodestar/issues/1104)).

1. **The three deterministic members** — `Normalizer`, `LabelEncoder`, `PolynomialFeatures`. No new
   machinery; `LabelEncoder` reuses `Internal.CategoryLookup`.
2. **`Internal/Percentiles.cs`** — `numpy.percentile`'s linear interpolation, lifted from where
   `RobustScaler` computes it so `KBinsDiscretizer` and `QuantileTransformer` read one
   implementation rather than three.
3. **`KBinsDiscretizer`**, including the `Lodestar.Cluster` edge for the `kmeans` strategy, and the
   three assertion files that edge touches: `tools/check_nuspec_dependencies.py`'s `EXPECTED`,
   `tools/check_claude_md_packages.py`'s map, and `CLAUDE.md`'s own edge count.
4. **`QuantileTransformer`**, on the percentiles above.
5. **`PowerTransformer`** — Brent's minimiser over the log-likelihood, and the `1e-5` tolerance
   documented everywhere it is claimed.
6. **`KnnImputer`** — `nan_euclidean`, with a measured size ceiling past which it refuses rather
   than running an O(n²) scan for however long that takes, the `MannWhitney` precedent.
7. **The corpora**, seven generators, regenerated from `/var/tmp` with the exit code read directly.
8. **The suites**, one replay per member plus an edge suite for what no corpus case can hold.
9. **The four gates** — `docs/equivalence.md` rows, reference pages under
   `docs/reference/preprocessing/` with their index, samples, and benchmarks.
   **`PackagingGate.Excluded` takes every new result record's constructor** before the first run,
   not after ([#1125](https://github.com/CyrilB1531/lodestar/pull/1125)).
10. **The local gates**, each read by its exit code rather than by its output.

## Risks

- **Brent's minimiser reaching scipy's λ.** The objective is flat, so two correct minimisers land
  up to `6e-7` apart; `1e-5` is the stated tolerance and the corpus is built to it. If the C#
  minimiser lands further out than that, the finding is the minimiser's and the fix is in it.
- **The `Lodestar.Cluster` edge.** It needs no new API from that package, but it does move the
  edge count three assertion files check. Those are updated in the same commit or the build fails,
  which is the point of them.
- **Seven members in one pull request.** Larger than #1121's five. The order above front-loads the
  three that need no new machinery, so a decision to split later costs the last four and not the
  first three.
