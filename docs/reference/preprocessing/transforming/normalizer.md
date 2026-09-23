# Normalizer

Scales each row to unit norm, at `sklearn.preprocessing.Normalizer` parity.

The one member of this package that fits nothing: every row is scaled by its own norm, so there
is no statistic to learn from a training set and no state for an instance to carry. The reference
is a transformer all the same, with a `fit` that only validates — so this is static.

**Rows, not features.** Every other member here scales a column; this one scales a row, which is
what a distance or a dot product between two rows reads afterwards.

## Members

| Member | What it does |
| --- | --- |
| [`Normalizer.Transform`](normalizer-transform.md) | Scales each row of a matrix to unit norm. |
