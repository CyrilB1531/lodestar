# ParametricOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the confidence level, the iteration budget and the number of breakpoints.

**Example** — equal options hash alike.

```csharp
using Lodestar.Survival;

ParametricOptions left = new() { Breakpoints = [2.0, 5.0] };
ParametricOptions right = new() { Breakpoints = [2.0, 5.0] };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the breakpoints contribute their count, never their values: equal options agree on the
count, and unequal ones are allowed to collide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricOptions.Equals`](parametricoptions-equals.md),
[`ParametricOptions`](parametricoptions.md).
