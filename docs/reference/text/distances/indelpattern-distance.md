# IndelPattern.Distance

The Indel distance between the held pattern and a text.

<!-- docs-declaration -->

```csharp
public int Distance(ReadOnlySpan<char> text)
```

**Parameters** — `text` is the string to scan against the pattern.

**Returns** — the fewest insertions and deletions that turn one into the other, the number
[`Indel.Distance`](indel-distance.md) gives for the same pair over UTF-16 units.

**Exceptions** — `ObjectDisposedException` when the handle has been disposed.

**Example** — the same answer as the pairwise call, with the table built once.

```csharp
using Lodestar.Text.Distances;

using IndelPattern query = IndelPattern.For("kitten");

int held = query.Distance("sitting".AsSpan());          // => 5
int pairwise = Indel.Distance("kitten", "sitting");     // => 5
```

**Remarks** — a substitution costs two here as it does everywhere in
[`Indel`](indel.md): this counts insertions and deletions, never replacements.

Scanning is thread-safe — the table is only read — but disposal is not, so do not dispose a handle
another thread is still scanning with.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IndelPattern`](indelpattern.md), [`Indel.Distance`](indel-distance.md),
[`IndelPattern.NormalizedSimilarity`](indelpattern-normalizedsimilarity.md).
