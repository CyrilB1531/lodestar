# BinEncoding

What a row transformed by [`KBinsDiscretizer`](kbinsdiscretizer.md) carries.

<!-- docs-declaration -->

```csharp
public enum BinEncoding
```

**Values** — `Ordinal` gives one column per feature, holding its bin's index; scikit-learn's
`'ordinal'`. `OneHot` gives one column per bin of each feature, one of them set; `'onehot-dense'`,
and the default shape.

**Example** — the same value, both ways.

```csharp
using Lodestar.Preprocessing;

double[] ages = [19.0, 22.0, 25.0, 31.0, 38.0, 44.0, 52.0, 61.0, 67.0, 74.0];

KBinsDiscretizer hot = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions { BinCount = 4 });

KBinsDiscretizer code = KBinsDiscretizer.Fit(
    ages, 1, new KBinsDiscretizerOptions { BinCount = 4, Encoding = BinEncoding.Ordinal });

string spread = string.Join(",", hot.Transform([44.0]));    // => 0,0,1,0
string index = string.Join(",", code.Transform([44.0]));    // => 2
```

**Remarks — `Ordinal` states an order the data may not have.** A linear model reading bin 3 as
three times bin 1 is reading a claim the discretizer never made, which is why the reference
defaults to one-hot and why this does too. `Ordinal` is right for a tree, which splits on the
order and ignores the spacing, and for a column being written out for a human.

**scikit-learn's third value, `'onehot'`, is absent**: it returns a sparse matrix, and the
difference is the storage rather than the encoding. A caller who wants the sparse form builds a
`CsrMatrix` from the dense one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KBinsDiscretizer.Transform`](kbinsdiscretizer-transform.md),
[`KBinsDiscretizerOptions`](kbinsdiscretizeroptions.md),
[`OneHotEncoder`](../encoding/onehotencoder.md).
