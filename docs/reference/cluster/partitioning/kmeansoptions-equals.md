# KMeansOptions.Equals

Compares every option, the centres element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(KMeansOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every option matches and both centre blocks hold the same values.

**Example** — two option sets built from separate arrays.

```csharp
using Lodestar.Cluster;

KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares `InitialCentres` by reference, so the two
above would be unequal without this, in the one place a caller has reason to compare: asserting
that a configuration built twice is the same configuration. `Tolerance` compares by bits, which
makes `NaN` equal `NaN` and keeps equality reflexive. [Decision
0113](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0113-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeansOptions.GetHashCode`](kmeansoptions-gethashcode.md),
[`KMeansOptions`](kmeansoptions.md).
