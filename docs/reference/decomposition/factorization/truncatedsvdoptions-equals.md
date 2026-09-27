# TruncatedSvdOptions.Equals

Compares every option, Ω element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(TruncatedSvdOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every option matches and both Ω blocks hold the same values.

**Example** — two option sets built from separate arrays.

```csharp
using Lodestar.Decomposition;

TruncatedSvdOptions left = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };
TruncatedSvdOptions right = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares `RandomMatrix` by reference, so the two
above would be unequal without this. `Oversampling`, `PowerIterations`, `Normalizer` and `Seed` compare as values. An absent Ω equals only an absent Ω, never an empty
one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TruncatedSvdOptions.GetHashCode`](truncatedsvdoptions-gethashcode.md),
[`TruncatedSvdOptions`](truncatedsvdoptions.md).
