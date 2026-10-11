# Preprocessing replays numpy's grid, scipy's Box-Cox search and numpy's category order

**Issues:** [#1226](https://github.com/CyrilB1531/lodestar/issues/1226),
[#1227](https://github.com/CyrilB1531/lodestar/issues/1227),
[#1229](https://github.com/CyrilB1531/lodestar/issues/1229),
[#1230](https://github.com/CyrilB1531/lodestar/issues/1230),
[#1237](https://github.com/CyrilB1531/lodestar/issues/1237),
[#1253](https://github.com/CyrilB1531/lodestar/issues/1253),
[#1254](https://github.com/CyrilB1531/lodestar/issues/1254).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The pre-1.0 reviews of `main` found `Lodestar.Preprocessing` diverging from scikit-learn 1.9.1,
scipy 1.18.1 and numpy 2.5.3 in seven places. `KBinsDiscretizer` formed its edges as
`low + (high - low) · i / n` and its percentile levels as `100 · i / n`, an ulp off `np.linspace`,
so a value on an edge changed bin and an edge the reference collapses survived (#1226, #1237).
`PowerTransformer` clamped the Box-Cox exponent to `[-2, 2]`, where scipy's `brent` leaves the
bracket (#1227), and had no constant-feature rule (#1253). A floating `NaN` category sorted first
and was coded as an index (#1229); strings sorted by UTF-16 unit (#1254). #1230 gathered the rest:
the dense `Normalizer` did not floor a norm below `10 · eps`, and the sparse one took a non-finite
value; the median interpolated, reading `[-1e308, 1e308]` as infinity; a constant KBins feature
had edges `[c, c]`, and its inverse cast an unvalidated code; `PolynomialFeatures` listed every
term before checking the count; `PowerTransformer` standardised with the scalers' floor and named
its column; `KnnImputer` resorted every donor per cell and broke ties by an unstable sort; and
three `docs/equivalence.md` rows were stale — the scaler row saying `StandardScaler` propagates a
`NaN` it refuses, and the two NMF rows decision 0008 overtook.

## Decisions

- **numpy's grid, shared.** `NumpyGrid` (`src/Shared`, already `Lodestar.Metrics`') gains
  `averaged_inverted_cdf`, `median` and `np.ma.median`, each by numpy's own route: the virtual
  index, the clip and `_lerp` at `0.5` or `1` for the first; for the last, the middle value added to
  itself and halved where the count is odd, which `np.nanmedian` takes below 600 rows and
  `SimpleImputer` always. `Lodestar.Preprocessing` takes it for every percentile, median, KBins edge
  and `QuantileTransformer` reference, where its own textbook percentile went.
- **scipy's Box-Cox, ported.** `bracket` from `(-2, 2)`, then `Brent.optimize`, constants included,
  on `boxcox_llf` computed in logarithms as scipy's `_log_var` does; then `boxcox_normmax`'s
  overflow guard, which pulls the exponent back through Lambert's `W₋₁` where the transform would
  pass `double.MaxValue / 10000`. A bracket scipy cannot find is refused in its first sentence, the advice on initial points it adds replaced by the start the Box-Cox search takes, which no option here changes.
- **A constant feature, as the reference treats it.** Yeo-Johnson keeps exponent 1 where
  `_is_constant_feature` holds on `np.mean` and `np.var`; Box-Cox refuses an exactly constant
  column, "Data must not be constant.". The standardisation is a `StandardScaler`, fitted on a
  constant Yeo-Johnson feature as it was, not as `λ = 1` maps it — the reference skips transforming
  it.
- **Yeo-Johnson's likelihood, scipy's too.** Review A found a column near `1e10` fitted at the
  search's bound, `-12.9`, where scikit-learn fits `6.93`: `yeojohnson_llf` takes a one-signed
  column's variance in logarithms and a mixed one's directly, `-∞` below the smallest normal, and
  `_neg_llf` rejects an infinite likelihood; all three are ported.
- **scipy's arithmetic, branch for branch.** `boxcox_llf`'s variance goes through a port of
  `scipy.special.logsumexp` — the maximum terms set apart and counted, the rest through `log1p` —
  and `special.boxcox` and `inv_boxcox` keep their overflow branches past `λ · ln x = 709.78` and
  `λ · y = 1.79e308`, where `expm1` or the product would overflow and scipy's stay finite. The one
  place it cannot follow to the bit is the C library itself: `log1p` and `expm1`, which .NET lacks,
  are written once, in `src/Shared/NearZero.cs`, and land an ulp from glibc's on a third to a half of inputs, and numpy's vectorised
  `exp` parts from .NET's on some processors; on a likelihood flat to rounding — a column of large
  values with a small spread, or values near `1e-300` — that ulp moves where the search stops, 66 of
  100 such Yeo-Johnson columns past `1e-5` relative, which no port of the search can close. The
  Yeo-Johnson search is `fminbound`'s own loop, ported statement for statement after a hand-written
  Brent stopped elsewhere on flat likelihoods.
- **scipy's transforms, not the textbook's.** Review A found `λ = 1` mapping `1e10` to
  `9999999999.999987` in scipy, `expm1(λ · log1p(x)) / λ`, where `((x + 1)^λ − 1) / λ` gives `1e10`;
  both transforms take scipy's `expm1` form, the Box-Cox inverse `inv_boxcox`'s `log1p` form, and
  every degenerate exponent is tested within machine epsilon where the reference tests it so. The
  transform costs 1.7 times main's `Math.Pow` for it, kept on the maintainer's decision of 2026-10-11.
- **numpy's category order.** Strings compare by code point (`CodePointOrder`, shared with
  `Lodestar.Text`), and `double`, `float` and `Half` with `NaN` last, their `IsNaN` found by
  reflection so a netstandard2.0 build run on .NET 5 or later meets `Half` too; `OrdinalEncoder`
  codes a fitted `NaN` category `NaN`.
- **KBins' inverse, as numpy indexes.** A code is truncated as `astype(np.int64)` truncates, a
  negative one counts from the end, one past either end is refused; a one-hot block is read by its
  `argmax` and refused when all zeros. A constant feature is one bin over `[-inf, inf]`.
- **StandardScaler reads an overflowed variance as scikit-learn does.** Review A found a column
  holding `1e200` standardised to NaN, infinity minus infinity in the compensated sums, where
  scikit-learn reads an infinite variance and scales by 1. `Fit`, `PartialFit` and the mean-only
  fit now take `_incremental_mean_and_var`'s sums in numpy's order throughout, so an overflowed sum
  stays infinite and the correction term decides between ∞ and NaN exactly where the reference's
  does: alternating `±1e308` overflows numpy's eight accumulators to NaN, as it does there.
- **A `NaN` stays refused, both ways.** The reference's scalers skip a `NaN` where they fit and
  carry it where they transform; these refuse one, as they did, in the previous release's words.
  `PowerTransformer.InverseTransform` refuses one too, so the `NaN` its `Transform` returns for a
  constant column past the float range does not invert, as it did not before.
- **Encoders refuse what the reference's `allow-nan` check refuses, where they completed.**
  `OneHotEncoder` and `OrdinalEncoder` refuse an infinite floating category in fit, naming its
  dtype, as scikit-learn does, and `OneHotEncoder` one it would pass over as unknown; refusing
  unknowns, they meet it as the unseen category they refused it as. `LabelEncoder`, which checks
  nothing there, keeps one. A `Half` category is
  named as numpy writes a `float16`, positional only from `1e-4` to `1e3`.
- **PolynomialFeatures, built as the reference builds it.** The count is closed-form and checked
  before anything is built; `Transform` then writes each term of degree `d` as a term of degree
  `d − 1` times one feature, scikit-learn's own loop, so no term is listed first and the products
  round as the reference's do; `FeatureNames` names each term as it reaches it.
- **KnnImputer's ties go to the lower row.** The reference breaks them by `numpy.argpartition`,
  whose pick varies with the CPU tier, so there is no single order to match; the nearest are kept in
  a bounded heap.

- **A refusal the previous release made keeps its words, order and parameter name**, on the
  maintainer's decision of 2026-10-10: the first value that is not finite, as `RequireFinite` words
  it; Box-Cox's "needs strictly positive values" naming the feature, checked feature by feature
  before any exponent; `PolynomialFeatures`' options before its samples, a negative degree named
  `settings.Degree`; an encoder refusing unknowns meets an infinity as the unseen category it was.
  Only the refusals new where that release completed wrongly take the reference's words: a sparse
  `NaN` in `Normalizer`, a power that overflows, an infinite category in `Fit` or passed over as
  unknown, and `KBinsDiscretizer`'s invalid inverse codes.
- **`StandardScaler` takes `_incremental_mean_and_var`'s sums in numpy's order**, where Neumaier's
  compensated sums made a nearly constant column standardise 8% away from scikit-learn's: 300 such
  columns now match it to the last bit, and the `PowerTransformer` standardising through it with them.
- **The likelihood's variance is taken directly where it holds.** scipy's logarithms cost three
  exponentials and a logarithm a value per step and made the fit 1.45 to 1.9 times main's; the
  power's variance is read directly when it is finite and above 1e-8 of its mean square, in
  logarithms otherwise. On 550 fits against scikit-learn 1.9.1, every transform matches at `1e-5`, and
  the 36 exponents off by more on flat likelihoods are the same 36 the logarithms alone gave. The fit
  is now 6.3 to 8.7 times scikit-learn's at 10,000 and 100,000 rows.
- **An overflowed mean is an empty feature**, on the maintainer's decision of 2026-10-11: `np.ma.mean` masks a
  mean past the float range and `SimpleImputer` then drops the feature, or fills it with zero under
  `keep_empty_features`; this refuses it, as it refuses one with no value, in a sentence of its own, where the
  previous release filled with infinity.
- **A percentile's zero sign is a limit**, on the maintainer's decision of 2026-10-11: numpy reads the column
  after `np.partition`'s introselect, this after .NET's sort, and from five values on the two can order `+0.0`
  and `-0.0` differently, so a percentile between two zeros can differ in sign alone; main sorted alike, and porting
  introselect for that was judged out of proportion. `docs/equivalence.md` carries it.
- **One `log1p` and `expm1`** in `src/Shared/NearZero.cs`, for this package, `Lodestar.Survival`,
  `Lodestar.Stats` and `Lodestar.Metrics`, where five copies each sat a few ulps off glibc in its own
  way.
- **The encoders search as main did.** Without a fitted `NaN` category, the default order is numpy's,
  so the default comparer and the runtime's search serve; the NaN-last order and a typed search only
  where a `NaN` was fitted, where a comparer delegate per probe cost a transform up to half again.

## Verified

Random differentials against scikit-learn 1.9.1, run from `/var/tmp`: 1,500 `KBinsDiscretizer`
fits — two to thirty bins, the uniform and quantile strategies, both quantile methods, values rounded to zero, one or
two decimals, a tenth constant — agree on every edge bit for bit and every code; 300 Box-Cox fits
agree on the exponent and the transform at `1e-5`, `11.129` on the #1227 column included, and 400
fits of both families, constant ones among them, on the exponent, the transform and the inverse;
300 `PolynomialFeatures` expansions agree bit for bit, their names and counts too. The KBins
corpus gains the five shapes the issues name; `PreprocessingReviewFindingsTests` pins the rest,
the overflow guard at a lowered ceiling against `boxcox_normmax(x, method="mle", ymax=…)`, which no
data of a sane size reaches at the default one.
