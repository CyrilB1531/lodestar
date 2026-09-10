# LshIndex

A banded index over MinHash signatures: candidates without comparing every pair.

<!-- docs-declaration -->

```csharp
public sealed class LshIndex
```

**Example** — one band agreeing is enough to be a candidate.

```csharp
using Lodestar.Text.Similarity;

var index = new LshIndex(new LshBanding(2, 2));
index.Add("stored", [1u, 2u, 3u, 4u]);

IReadOnlyList<string> found = index.Query([1u, 2u, 99u, 99u]);

int howMany = found.Count;  // => 1
int held = index.Count;  // => 1
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`LshIndex.Add`](lshindex-add.md) | Adds one signature under a key |
| [`LshIndex.Query`](lshindex-query.md) | The keys sharing at least one band with a signature |

**Remarks** — reference behaviour is `datasketch` 1.6.5's `MinHashLSH`. Two signatures are
candidates when they agree on **every slot of at least one band**, which is what turns a quadratic
scan into a lookup.

**`Query` returns candidates, not matches.** Scoring them with
[`MinHash.Jaccard`](minhash-jaccard.md) is the caller's next step, and
[`LshBanding`](lshbanding.md) decides how often that step is wasted. An index that returned matches
would have to hold every signature and compare them, which is the cost this exists to avoid.

A signature longer than the banding needs is accepted and its tail ignored, because
[`Solve`](lshbanding-solve.md) usually returns a banding consuming fewer slots than the length it
was given.

Adding is not thread-safe; concurrent `Query` calls are.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshBanding`](lshbanding.md), [`MinHash`](minhash.md),
[the set-similarity index](../similarity.md).
