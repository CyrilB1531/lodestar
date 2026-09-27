# NmfOptions.Equals

Compares every option, Ω element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(NmfOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every option matches and both Ω blocks hold the same values.

**Example** — two option sets built from separate arrays.

```csharp
using Lodestar.Decomposition;

NmfOptions left = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };
NmfOptions right = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares `RandomMatrix` by reference, so the two
above would be unequal without this. `BetaLoss`, `Initialization`, `MaxIterations` and `Seed` compare as values, and `Tolerance` through `double.Equals`, which makes `NaN` equal `NaN` and keeps equality reflexive. An absent Ω equals only an absent Ω, never an empty
one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NmfOptions.GetHashCode`](nmfoptions-gethashcode.md),
[`NmfOptions`](nmfoptions.md).
