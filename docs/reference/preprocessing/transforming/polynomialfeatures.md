# PolynomialFeatures

Expands each row into its polynomial terms, at `sklearn.preprocessing.PolynomialFeatures` parity.

Fits nothing — the terms are decided by the feature count and the degree, not by the data — so
this is static, as [`Normalizer`](normalizer.md) is.

**The column order is the contract.** A caller that fits a model on these columns and reads its
coefficients back needs to know which coefficient belongs to which term, and the only useful
answer is the reference's own order: the bias, then every term of degree one in feature order,
then every term of degree two as `x0²`, `x0·x1`, `x1²`, and so on.
[`FeatureNames`](polynomialfeatures-featurenames.md) returns exactly that order, in the
reference's own spelling.

## Members

| Member | What it does |
| --- | --- |
| [`PolynomialFeatures.Transform`](polynomialfeatures-transform.md) | Expands a matrix into its polynomial terms. |
| [`PolynomialFeatures.FeatureNames`](polynomialfeatures-featurenames.md) | Each term's name, in the order the columns come out. |
| [`PolynomialFeatures.OutputFeatureCount`](polynomialfeatures-outputfeaturecount.md) | How many terms an expansion produces, without producing one. |
