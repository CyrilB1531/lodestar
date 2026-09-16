# Encoding and imputation — `Lodestar.Preprocessing`

Two encoders and an imputer, at `sklearn.preprocessing` and `sklearn.impute` parity, over row-major
spans rather than an `IDataView`: [`Encoders.OneHot`](encoding/encoders-onehot.md) gives each
category a column, [`Encoders.Ordinal`](encoding/encoders-ordinal.md) gives it a code, and
[`SimpleImputer`](encoding/simpleimputer.md) fills what is missing.

**One element type per call.** A 2-D array carries one dtype in the reference too, so the encoders
are generic over the category type and a caller with a string column and an integer column makes two
calls. The type decides the order — **strings sort by code point** as numpy's do, integers as
numbers — and that order decides the columns.

## Why this exists when ML.NET has all three

ML.NET has `OneHotEncoding`, `MapValueToKey` and `ReplaceMissingValues`; SharpLearning has
`OneHotTransformer` and `ReplaceMissingValuesTransformer`. **Nothing here is absent from .NET.** Each
one of them is reached through an `IDataView` or through a catalog naming columns, or works on that
library's own matrix type — and what is absent is a call that takes an array and returns one. That
is the whole argument, and [decision 0132](../../decisions/0132-preprocessing-writes-splitters-scalers-and-encoders-and-not-smote.md)
says so rather than claiming a capability gap.

## Types

| Type | What it is |
| --- | --- |
| [`Encoders`](encoding/encoders.md) | Fits the two encoders. |
| [`OneHotEncoder`](encoding/onehotencoder.md) | One column per category. |
| [`OneHotEncoderOptions`](encoding/onehotencoderoptions.md) | Which category to drop, and what an unseen value becomes. |
| [`CategoryDrop`](encoding/categorydrop.md) | None, the first, or the first of a binary feature. |
| [`UnknownCategory`](encoding/unknowncategory.md) | Refuse an unseen value, or encode it as zeros. |
| [`OrdinalEncoder`](encoding/ordinalencoder.md) | One code per category. |
| [`SimpleImputer`](encoding/simpleimputer.md) | Fills missing values with a per-feature statistic. |
| [`SimpleImputerOptions`](encoding/simpleimputeroptions.md) | Which statistic, and what to do with an empty feature. |
| [`ImputationStrategy`](encoding/imputationstrategy.md) | Mean, median, most frequent, or a constant. |

## See also

- [Feature scaling](scaling.md) and [splitting](splitting.md) — the rest of this package.
- [scikit-learn → .NET](../../migration/sklearn.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
