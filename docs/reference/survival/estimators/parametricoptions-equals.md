# ParametricOptions.Equals

Compares every option, the breakpoints element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(ParametricOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when the confidence level, the iteration budget and every breakpoint match.

**Example** — two option sets built from separate arrays.

```csharp
using Lodestar.Survival;

ParametricOptions left = new() { Breakpoints = [2.0, 5.0] };
ParametricOptions right = new() { Breakpoints = [2.0, 5.0] };

bool same = left == right;  // => True
```

**Remarks** — the `init` copies the breakpoints, so a record's generated equality, which compares
them by reference, would call these two unequal
([#1307](https://github.com/CyrilB1531/lodestar/issues/1307)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricOptions.GetHashCode`](parametricoptions-gethashcode.md),
[`ParametricOptions`](parametricoptions.md).
