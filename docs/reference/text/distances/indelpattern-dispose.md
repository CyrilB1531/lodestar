# IndelPattern.Dispose

Returns the equality table to the pool. Scanning afterwards throws.

<!-- docs-declaration -->

```csharp
public void Dispose()
```

**Returns** — nothing.

**Example** — a `using` statement is the intended shape; this is what it calls.

```csharp
using Lodestar.Text.Distances;

IndelPattern query = IndelPattern.For("kitten");
int distance = query.Distance("sitting".AsSpan());   // => 5
query.Dispose();
```

**Remarks** — **idempotent, which is why [`IndelPattern`](indelpattern.md) is a class.** A struct
holding a rented array is trivially copyable, and two copies disposed would return one array
twice; the pool would then hand it to two callers at once, which is silent corruption rather than
an exception. The one object header this costs is paid once per pattern and amortised over every
text the handle scans.

A handle that is never disposed leaks nothing — the array is ordinary managed memory — but the
pool has to allocate a replacement for it, which is the cost a `using` avoids.

Disposal is not thread-safe. Scanning is: the table is only read.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`IndelPattern`](indelpattern.md), [`IndelPattern.For`](indelpattern-for.md).
