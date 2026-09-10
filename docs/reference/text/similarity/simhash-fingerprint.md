# SimHash.Fingerprint

The fingerprint of a bag of tokens.

<!-- docs-declaration -->

```csharp
public static ulong Fingerprint(IEnumerable<string> tokens)
public static ulong Fingerprint(IEnumerable<KeyValuePair<string, int>> weighted)
```

**Parameters** — `tokens` are weighted once each, repeats included. `weighted` carries its own
non-negative weight per token, which is the overload to use when a term frequency is already known.

**Returns** — `ulong`, a 64-bit fingerprint. An empty input gives zero.

**Exceptions** — `ArgumentNullException` when the sequence, or a token in it, is null;
`ArgumentOutOfRangeException` when a weight is negative.

**Example** — repeating a token moves the fingerprint.

```csharp
using Lodestar.Text.Similarity;

ulong once = SimHash.Fingerprint(["the", "quick", "brown", "fox"]);
ulong twice = SimHash.Fingerprint(["the", "the", "quick", "brown", "fox"]);

bool moved = once != twice;  // => True
```

**Remarks** — each token is hashed with MD5, read as a big-endian integer, and its low 64 bits
decide which columns it pushes up and which down; a column that ends positive sets its bit. MD5 is
a hash function here and never a signature, and every frozen value depends on it, so a stronger
digest would be a different algorithm rather than an improvement.

**A weight of zero is not the same as omitting the token.** It contributes nothing to any column,
which is what omitting it does too — but it still has to be hashed, so passing a long tail of
zero-weight tokens costs time and buys nothing.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimHash.HammingDistance`](simhash-hammingdistance.md).
