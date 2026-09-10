# LshIndex.Query

The keys sharing at least one band with a signature.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<string> Query(ReadOnlySpan<uint> signature)
```

**Parameters** — `signature` is at least `Banding.Permutations` long.

**Returns** — `IReadOnlyList<string>`, each candidate once, in the order it was added.

**Exceptions** — `ArgumentException` when the signature is too short.

**Example** — a band differing in one slot does not collide.

```csharp
using Lodestar.Text.Similarity;

var index = new LshIndex(new LshBanding(2, 2));
index.Add("stored", [1u, 2u, 3u, 4u]);

IReadOnlyList<string> missed = index.Query([1u, 99u, 3u, 99u]);

int howMany = missed.Count;  // => 0
```

**Remarks** — a key colliding on several bands is returned **once**, and the order does not
encode how many bands agreed. That is deliberate: a band count is not a similarity, and returning
candidates ranked by it would invite treating it as one. Score them with
[`MinHash.Jaccard`](minhash-jaccard.md) instead.

The band's index is part of its bucket key, so two different bands holding the same slot values do
not collide with each other — which they otherwise would, and silently.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshIndex.Add`](lshindex-add.md), [`MinHash.Jaccard`](minhash-jaccard.md).
