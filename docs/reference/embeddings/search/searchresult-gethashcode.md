# SearchResult.GetHashCode

A hash consistent with the generated equality, with every `NaN` hashed alike.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over every member, its `float` score through one rule that sends every `NaN` to
the same value.

**Example** — a `NaN` and its negation are different bit patterns, and still equal and hash alike.

```csharp
using Lodestar.Embeddings.Search;

SearchResult left = new(2, float.NaN);
SearchResult right = new(2, -float.NaN);

bool equal = left.Equals(right);                          // => True
bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the equality stays the one the compiler generates, under which every `NaN` equals
every other. .NET Framework, Mono and Unity hash a `NaN`'s sign and payload bits, so without this
override two equal values could hash apart there and miss each other in a `Dictionary` or a
`HashSet` ([#1294](https://github.com/CyrilB1531/lodestar/issues/1294)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SearchResult`](searchresult.md).
