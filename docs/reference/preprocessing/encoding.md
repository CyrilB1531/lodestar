# Encoding and imputation — `Lodestar.Preprocessing`

Three encoders and two imputers, at `sklearn.preprocessing` and `sklearn.impute` parity, over
row-major spans rather than an `IDataView`: [`Encoders.OneHot`](encoding/encoders-onehot.md) gives
each category a column, [`Encoders.Ordinal`](encoding/encoders-ordinal.md) gives it a code,
[`Encoders.Label`](encoding/encoders-label.md) does the same for a target column,
[`SimpleImputer`](encoding/simpleimputer.md) fills what is missing from the column's own statistic,
and [`KnnImputer`](encoding/knnimputer.md) fills it from the rows that resemble the one with the
gap.

**One element type per call.** A 2-D array carries one dtype in the reference too, so the encoders
are generic over the category type and a caller with a string column and an integer column makes two
calls. The type decides the order — **strings sort by code point** as numpy's do, integers as
numbers — and that order decides the columns.

## Why this exists when ML.NET has most of it

ML.NET has `OneHotEncoding`, `MapValueToKey` and `ReplaceMissingValues`; SharpLearning has
`OneHotTransformer` and `ReplaceMissingValuesTransformer`. **Almost nothing here is absent from
.NET.** Each one of them is reached through an `IDataView` or through a catalog naming columns, or
works on that library's own matrix type — and what is absent is a call that takes an array and
returns one. That is the whole argument, and
[decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md) says so rather
than claiming a capability gap.

The one member with no counterpart at all is [`KnnImputer`](encoding/knnimputer.md): ML.NET's
`ReplaceMissingValues` fills from a column statistic — its mean, minimum, maximum or the type's
default — and offers nothing that reads the row being filled.

## Types

| Type | What it is |
| --- | --- |
| [`Encoders`](encoding/encoders.md) | Fits the three encoders. |
| [`OneHotEncoder`](encoding/onehotencoder.md) | One column per category. |
| [`OneHotEncoderOptions`](encoding/onehotencoderoptions.md) | Which category to drop, and what an unseen value becomes. |
| [`CategoryDrop`](encoding/categorydrop.md) | None, the first, or the first of a binary feature. |
| [`UnknownCategory`](encoding/unknowncategory.md) | Refuse an unseen value, or encode it as zeros. |
| [`OrdinalEncoder`](encoding/ordinalencoder.md) | One code per category. |
| [`LabelEncoder`](encoding/labelencoder.md) | One code per label, for a target column. |
| [`SimpleImputer`](encoding/simpleimputer.md) | Fills missing values with a per-feature statistic. |
| [`SimpleImputerOptions`](encoding/simpleimputeroptions.md) | Which statistic, and what to do with an empty feature. |
| [`ImputationStrategy`](encoding/imputationstrategy.md) | Mean, median, most frequent, or a constant. |
| [`KnnImputer`](encoding/knnimputer.md) | Fills missing values from the nearest rows. |
| [`KnnImputerOptions`](encoding/knnimputeroptions.md) | How many donors, and how they are weighted. |
| [`NeighbourWeights`](encoding/neighbourweights.md) | Equal shares, or the reciprocal of the distance. |

## See also

- [Feature scaling](scaling.md), [transforming](transforming.md) and [splitting](splitting.md) —
  the rest of this package.
- [scikit-learn → .NET](../../migration/sklearn.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
