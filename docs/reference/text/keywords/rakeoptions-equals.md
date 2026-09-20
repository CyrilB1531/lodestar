# RakeOptions.Equals

Compares every option, treating the stop words as a set.

<!-- docs-declaration -->

```csharp
public bool Equals(RakeOptions other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every scalar matches and both stop-word collections hold the same
words.

**Example** — the same configuration, written two ways.

```csharp
using Lodestar.Text.Keywords;

RakeOptions left = new() { StopWords = ["the", "the", "a"] };
RakeOptions right = new() { StopWords = ["a", "the"] };

bool same = left == right;  // => True
```

**Remarks** — the stop words are a set, so order and repetition do not count, and an absent
collection is not an empty one. the equality rule
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RakeOptions.GetHashCode`](rakeoptions-gethashcode.md),
[`RakeOptions`](rakeoptions.md).
