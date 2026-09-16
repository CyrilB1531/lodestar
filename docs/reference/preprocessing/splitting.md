# Splitting — `Lodestar.Preprocessing`

One entry point, [`Splitters`](splitting/splitters.md): it cuts the rows into cross-validation folds
or into a single train and test split, at `sklearn.model_selection` parity wherever the reference is
deterministic.

**Indices in, indices out.** A splitter here never sees the data. It is told how many rows there are
— or, to stratify, what class each row belongs to — and it hands back the row numbers that train and
the row numbers that are held out. Nothing is copied, and nothing decides the caller's layout.

## Why this exists when ML.NET splits data

ML.NET has `TrainTestSplit(IDataView, double, …)` and `CrossValidationSplit(IDataView, int, …)`, and
both return data views. They also **never stratify**:
[dotnet/machinelearning#4396](https://github.com/dotnet/machinelearning/issues/4396) has asked for it
since 2019 and is open. `samplingKeyColumnName` keeps rows that share a key *together*, which is the
opposite operation — it prevents a group from straddling the split, where stratifying spreads a class
across every fold.

SharpLearning.CrossValidation does stratify, and its `StratifiedIndexSampler<T>` always shuffles from
a seed, so it cannot reproduce a scikit-learn fold. What is missing in .NET is a splitter that is
framework-free **and** reproducible, which is what
[decision 0132](../../decisions/0132-preprocessing-writes-splitters-scalers-and-encoders-and-not-smote.md)
wrote this for.

## The permutation is an argument, not a seed

Each splitter has a second overload taking `order`, a permutation of `0..n−1` that it reads the rows
in. Passing scikit-learn's own permutation reproduces scikit-learn's shuffled folds; passing your own
gives a split this package can describe exactly, without claiming a generator no reference shares —
the same choice `KMeansOptions.InitialCentres` makes by taking the centres rather than a seed.

## Types

| Type | What it is |
| --- | --- |
| [`Splitters`](splitting/splitters.md) | The three splitters. |
| [`FoldSplit`](splitting/foldsplit.md) | One fold: which rows train, and which are held out. |
| [`TrainTestSplit`](splitting/traintestsplit.md) | A single train and test split of the rows. |

## See also

- [Feature scaling](scaling.md) — the other half of this package.
- [scikit-learn → .NET](../../migration/sklearn.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
