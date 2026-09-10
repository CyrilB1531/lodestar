# LshIndex.Add

Adds one signature under a key of the caller's choosing.

<!-- docs-declaration -->

```csharp
public void Add(string key, ReadOnlySpan<uint> signature)
```

**Parameters** — `key` is what [`Query`](lshindex-query.md) returns when this signature is a
candidate. `signature` is at least `Banding.Permutations` long.

**Exceptions** — `ArgumentNullException` when `key` is null; `ArgumentException` when the
signature is too short, or the key is already held.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Text.Similarity;

var index = new LshIndex(new LshBanding(1, 2));
index.Add("first", [5u, 5u]);
index.Add("second", [5u, 5u]);

int held = index.Count;  // => 2
```

**Remarks** — **a key may be added once.** Adding it twice throws rather than replacing, because
the signature is already scattered across the bands it landed in and a silent replace would leave
the old one findable. Removing and re-adding is not offered: an index that has to change is
cheaper to rebuild than to unpick.

`Count` tracks keys, not buckets — one signature occupies `Banding.Bands` buckets and still counts
once.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshIndex.Query`](lshindex-query.md).
