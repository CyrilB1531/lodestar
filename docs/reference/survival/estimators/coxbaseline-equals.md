# CoxBaseline.Equals

Compares the label and the four arrays, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(CoxBaseline other)
```

**Parameters** — `other` is the baseline to compare against, or `null`.

**Returns** — `true` when the stratum labels match and the times, hazards, cumulative hazards and
survivals hold the same values.

**Example** — the same baseline, built twice.

```csharp
using Lodestar.Survival;

CoxBaseline left = new(0, [1.0, 2.0], [0.1, 0.2], [0.1, 0.3], [0.9, 0.74]);
CoxBaseline right = new(0, [1.0, 2.0], [0.1, 0.2], [0.1, 0.3], [0.9, 0.74]);

bool same = left == right;  // => True
```

**Remarks** — the generated record equality would compare the arrays by reference, so two baselines
of the same fit rebuilt would be unequal; this compares their values.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxBaseline.GetHashCode`](coxbaseline-gethashcode.md), [`CoxBaseline`](coxbaseline.md).
