# RowNorm

Which norm [`Normalizer`](normalizer.md) scales each row to one by.

<!-- docs-declaration -->

```csharp
public enum RowNorm { L1, L2, Max }
```

**Members** — `L1` is the sum of absolute values, scikit-learn's `'l1'`. `L2` is the Euclidean
length, scikit-learn's `'l2'`, and the default on both sides. `Max` is the largest absolute
value, scikit-learn's `'max'`.

**Example** — three documents as word counts, scaled three ways.

```csharp
using Lodestar.Preprocessing;

double[] counts = [2.0, 0.0, 1.0, 0.0, 4.0, 4.0, 1.0, 1.0, 1.0];

double[] euclidean = Normalizer.Transform(counts, 3);
double[] absolute = Normalizer.Transform(counts, 3, RowNorm.L1);

double firstUnderL2 = Math.Round(euclidean[0], 4);   // => 0.8944
double firstUnderL1 = Math.Round(absolute[0], 4);    // => 0.6667
```

**Remarks — the choice decides what "the same document twice as long" means.** Under `L2` two
rows pointing the same way become the same row, which is what a cosine similarity wants. Under
`L1` each row becomes a distribution summing to one, which is what a comparison of proportions
wants. Under `Max` the largest entry becomes one and the rest are read against it.

This is its own enum rather than `Lodestar.Abstractions`' `SparseNorm`, which carries `L1` and
`L2` and no maximum. Adding a member there would mean releasing that package before this one
([`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#working-across-two-packages)),
and `CsrMatrix.NormalizeRows` mutates its matrix in place where every member here leaves its
input alone.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Normalizer`](normalizer.md), the [Python equivalence table](../../../equivalence.md).
