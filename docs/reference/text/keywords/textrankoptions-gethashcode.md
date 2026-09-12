# TextRankOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the scalars and whether stop words are present.

**Example** — options equal as sets hash alike.

```csharp
using Lodestar.Text.Keywords;

TextRankOptions left = new() { StopWords = ["the", "the"] };
TextRankOptions right = new() { StopWords = ["the"] };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the stop words contribute only their presence, never their count, because set
equality does not preserve a count and equal objects must hash alike. An absent `Words` limit
contributes `-1`, so it does not collide with a limit of zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TextRankOptions.Equals`](textrankoptions-equals.md),
[`TextRankOptions`](textrankoptions.md).
