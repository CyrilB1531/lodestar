# CountVectorizer.GetFeatureNames

The term each column stands for, in column order.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<string> GetFeatureNames()
```

**Returns** — `IReadOnlyList<string>` of length `ColumnCount`, sorted by code point, where index `i` is the term
counted by column `i`.

**Exceptions** — `InvalidOperationException` when nothing has been fitted yet.

**Example** — the vocabulary is sorted, so the order depends on the terms and not the documents.

```csharp
using Lodestar.Text.Vectorization;

var cv = new CountVectorizer();
cv.Fit(["the cat eats", "the dog eats", "the cat and the dog"]);

IReadOnlyList<string> names = cv.GetFeatureNames();

string first = names[0];   // => and
string last = names[4];    // => the
```

**Remarks** — this is what makes a count matrix readable, and it is the thing
[`HashingVectorizer`](hashingvectorizer.md) cannot offer: hashing throws the vocabulary away, so
there is no name to return and no method to return it. Choosing that vectorizer is choosing to
give this up.

The list is a read-only view of the vectorizer's own vocabulary, made at the first call and not
copied, so reading it allocates nothing after that; a cast back to an array fails rather than
editing the fitted model under a later `Save`
([#1670](https://github.com/CyrilB1531/lodestar/issues/1670)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CountVectorizer`](countvectorizer.md), [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md).
