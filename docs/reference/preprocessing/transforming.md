# Feature transforming — `Lodestar.Preprocessing`

Five transformers that change the shape of a feature rather than its scale, at
`sklearn.preprocessing` parity: [`Normalizer`](transforming/normalizer.md) scales each *row* to
unit norm, [`PolynomialFeatures`](transforming/polynomialfeatures.md) expands a row into its
products, [`KBinsDiscretizer`](transforming/kbinsdiscretizer.md) cuts a feature into bins,
[`QuantileTransformer`](transforming/quantiletransformer.md) maps a feature onto a uniform or
normal distribution by its ranks, and
[`PowerTransformer`](transforming/powertransformer.md) raises it to the power that makes it most
nearly symmetric.

**Which one, in one line each.** `Normalizer` is for rows compared by distance or dot product —
it is the only member here that works across a row rather than down a column.
`PolynomialFeatures` is for a linear model that needs to see curvature.
`KBinsDiscretizer` is for turning a measurement into a category. `QuantileTransformer` is the
blunt instrument that makes any feature uniform by throwing away everything but the order of its
values. `PowerTransformer` is the gentle one that keeps the values and looks for a single
exponent.

**Spans in, arrays out**, as everywhere in this package: a sample matrix is row-major,
`FeatureCount` values per row, and no member writes to its input.

## Two scopes worth reading before use

**[`QuantileTransformer`](transforming/quantiletransformer.md) takes no `subsample`.** The
reference's own default draws a subsample of 10,000 rows from numpy's generator, and two seeds
were measured moving a quantile by `0.19` over twenty thousand rows — so that configuration
cannot be frozen into a corpus and is not offered. This transformer always reads every row, which
is the reference's `subsample=None`.

**[`PowerTransformer`](transforming/powertransformer.md) is held to `1e-5`, not `1e-9`.** Its
exponent is fitted by maximising a log-likelihood whose curvature at the optimum is about `176`
against a value around `443`; a double carries that to roughly `3e-11`, so the data pins the
exponent only to about `6e-7`, and moving it that far moves the transformed values by `9.1e-7`
relative. The [Python equivalence table](../../equivalence.md) carries the arithmetic. Every other
member here meets `1e-9`.

## Members

| Type | What it does |
| --- | --- |
| [`Normalizer`](transforming/normalizer.md) | Scales each row to unit norm. |
| [`RowNorm`](transforming/rownorm.md) | Which norm a row is scaled by. |
| [`PolynomialFeatures`](transforming/polynomialfeatures.md) | Expands each row into its polynomial terms. |
| [`PolynomialFeaturesOptions`](transforming/polynomialfeaturesoptions.md) | What is expanded, and how far. |
| [`KBinsDiscretizer`](transforming/kbinsdiscretizer.md) | Cuts each feature into bins. |
| [`KBinsDiscretizerOptions`](transforming/kbinsdiscretizeroptions.md) | How the bins are placed, and what is emitted. |
| [`BinStrategy`](transforming/binstrategy.md) | Where the bin edges go. |
| [`BinEncoding`](transforming/binencoding.md) | What a transformed row carries. |
| [`QuantileMethod`](transforming/quantilemethod.md) | Which percentile convention a quantile fit reads. |
| [`QuantileTransformer`](transforming/quantiletransformer.md) | Maps each feature onto a uniform or normal distribution. |
| [`QuantileTransformerOptions`](transforming/quantiletransformeroptions.md) | How many quantiles, and what they map onto. |
| [`QuantileOutput`](transforming/quantileoutput.md) | Which distribution the ranks map onto. |
| [`PowerTransformer`](transforming/powertransformer.md) | Raises each feature to its most normalising power. |
| [`PowerTransformerOptions`](transforming/powertransformeroptions.md) | Which family, and whether to standardise after. |
| [`PowerMethod`](transforming/powermethod.md) | Which power family is fitted. |
