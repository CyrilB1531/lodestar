# RakeOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the scalars and whether stop words are present.

**Example** — options equal as sets hash alike.

```csharp
using Lodestar.Text.Keywords;

RakeOptions left = new() { StopWords = ["the", "the", "a"] };
RakeOptions right = new() { StopWords = ["a", "the"] };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the stop words contribute only their presence, never their count. Set equality
makes `["the", "the"]` equal `["the"]` while the counts differ, and equal objects must hash
alike; hashing the words themselves would be the walk this avoids.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RakeOptions.Equals`](rakeoptions-equals.md), [`RakeOptions`](rakeoptions.md).
