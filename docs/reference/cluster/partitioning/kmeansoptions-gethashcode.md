# KMeansOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the scalars and the number of centres.

**Example** — equal options hash alike.

```csharp
using Lodestar.Cluster;

KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the centres contribute their count, never their values: equal options necessarily
agree on the count, and hashing the block itself would make the cheap operation the expensive one
on a large start. Unequal options are allowed to collide. An absent block contributes `-1` rather
than `0`, so it does not collide with an empty one — which it must not, since the two are
unequal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeansOptions.Equals`](kmeansoptions-equals.md),
[`KMeansOptions`](kmeansoptions.md).
