# OneHotEncoder: sparse output, infrequent categories and feature names, at scikit-learn parity

**Issue:** [#1161](https://github.com/CyrilB1531/lodestar/issues/1161).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`docs/equivalence.md` carried `OneHotEncoder(sparse_output=True)`, `min_frequency` and
`max_categories` as *not written*. scikit-learn's default output is sparse, so a high-cardinality
column one-hot encoded here came back as a dense matrix of mostly zeros, while every sparse consumer
in these packages takes a `CsrMatrix`.

## What scikit-learn 1.9.1 does

Read from `sklearn/preprocessing/_encoders.py`:

- `_identify_infrequent`: a category is infrequent when its count is below `min_frequency`, an
  integer of at least 1, or below `n_samples × min_frequency` for a float strictly inside (0, 1).
  When the frequent ones plus one infrequent column exceed `max_categories`, all but the
  `max_categories − 1` most frequent are infrequent, ranked by a stable sort of the counts, so ties
  keep the sorted category order; `max_categories = 1` groups them all.
- The infrequent categories share one column, the last of their feature's block, named
  `infrequent_sklearn` in `get_feature_names_out`.
- `drop="first"` drops the first column after grouping, and `"if_binary"` counts grouped columns.
- `handle_unknown="infrequent_if_exist"` maps an unknown value to the infrequent column where the
  feature has one and to all zeros otherwise; `"warn"` does the same and warns.
- `infrequent_categories_` raises `AttributeError` when neither setting is given.

## Decisions

As approved on the issue:

1. `OneHotEncoderOptions.MinFrequency` (`int?`) and `MinFrequencyShare` (`double?`, in (0, 1)),
   refused together, and `MaxCategories` (`int?`, at least 1): scikit-learn tells the integer from
   the float by its Python type, which two properties make explicit.
2. `UnknownCategory.Infrequent` for `"infrequent_if_exist"`; `"warn"` has no member.
3. `OneHotEncoder<T>.TransformSparse` returns a `CsrMatrix` with exactly the dense output's ones;
   the dense `Transform` stays.
4. `OneHotEncoder<T>.FeatureNames(inputFeatures)` spells `get_feature_names_out`'s names, a
   floating category as Python's `str` writes it (`1.0`, `1e-05`), checked against `repr` on 20,017
   doubles, and a `float` as numpy's `float32` does, positional only from `1e-4` to `1e6` by value
   (`1.234567e+06`), checked on 20,012 singles. The shortest digits come from the smallest `"G"`
   precision that round-trips, since `"R"` is shortest only from .NET Core 3.0.
5. `OneHotEncoder<T>.InfrequentCategories` is `infrequent_categories_`, `null` for a feature with
   none and for every feature when no setting is given, where the reference raises.
6. The grouping, the column order and the drop interplay are replayed as above.
7. The options and the enum member are in `Lodestar.Abstractions`, reached by project until its next
   publication, as #1155 and #1159 did.

## Proof

- `tests/oracles/preprocessing_onehot_infrequent.json`, 684 cases: a string feature, two string
  features, an integer feature and a floating one, each under `min_frequency` as a count (2, 3) and a share (0.1,
  0.25), `max_categories` (1, 2, 3), and their combinations, crossed with `drop` and
  `handle_unknown`; the dense encoding, the sparse `data`, `indices` and `indptr`, the feature
  names, `infrequent_categories_` and rows the fit never saw, compared exactly.
- A random differential against scikit-learn: 800 random encoders of one to three string or
  integer features with skewed counts and random settings, every value and name identical.
