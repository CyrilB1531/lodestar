# TextRankOptions.Equals

Compares every option, treating the stop words as a set.

<!-- docs-declaration -->

```csharp
public bool Equals(TextRankOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every scalar matches and both stop-word collections hold the same
words.

**Example** — the same configuration, written two ways.

```csharp
using Lodestar.Text.Keywords;

TextRankOptions left = new() { StopWords = ["the", "the"] };
TextRankOptions right = new() { StopWords = ["the"] };

bool same = left == right;  // => True
```

**Remarks** — `Damping`, `Tolerance` and `Ratio` compare by bits, which makes `NaN` equal `NaN`
and keeps equality reflexive. the equality rule
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TextRankOptions.GetHashCode`](textrankoptions-gethashcode.md),
[`TextRankOptions`](textrankoptions.md).
