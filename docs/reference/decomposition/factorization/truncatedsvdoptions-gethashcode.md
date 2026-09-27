# TruncatedSvdOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the scalars and the length of Ω.

**Example** — equal options hash alike.

```csharp
using Lodestar.Decomposition;

TruncatedSvdOptions left = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };
TruncatedSvdOptions right = new() { RandomMatrix = [1.0, 2.0], Seed = 3 };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — Ω contributes its length, never its values: equal options necessarily agree on the
length, and hashing a `features × (components + oversampling)` block would make the cheap
operation the expensive one. Unequal options are allowed to collide. An absent Ω contributes `-1`
rather than `0`, so it does not collide with an empty one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TruncatedSvdOptions.Equals`](truncatedsvdoptions-equals.md),
[`TruncatedSvdOptions`](truncatedsvdoptions.md).
