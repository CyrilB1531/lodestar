# TfidfVectorizer.FitTransform

Learn the vocabulary and frequencies, and weight the same corpus.

<!-- docs-declaration -->

```csharp

public CsrMatrix FitTransform(IEnumerable<string> documents)
```

**Parameters** — `documents` is the corpus, both learned from and weighted.

**Returns** — [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md), one row per document, weighted and normalized by
[`TfidfOptions.Norm`](tfidfoptions.md).

**Exceptions** — `ArgumentNullException` when `documents` is null. `ArgumentException` when `documents` holds a null document. `InvalidOperationException` when the corpus yields no term at all — every document empty or
made only of stop words — when [`MinDf`](countvectorizeroptions.md) and
[`MaxDf`](countvectorizeroptions.md) leave no term, or when `MaxDf` corresponds to fewer
documents than `MinDf`: scikit-learn raises `ValueError` for each, and so there is no model of
zero columns.

**Example** — the whole corpus in one call, which is the usual way in.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Vectorization;

string[] docs = ["the cat eats", "the dog eats", "the cat and the dog"];

CsrMatrix weighted = new TfidfVectorizer().FitTransform(docs);

int rows = weighted.RowCount;         // => 3
int columns = weighted.ColumnCount;   // => 5
```

**Remarks** — equivalent to [`Fit`](tfidfvectorizer-fit.md) then
[`Transform`](tfidfvectorizer-transform.md) on the same corpus, in one enumeration rather than
two. It is not equivalent to fitting one corpus and transforming another, and the difference is
not cosmetic here: the document frequencies would come from the wrong corpus.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TfidfVectorizer.Fit`](tfidfvectorizer-fit.md),
[`TfidfTransformer.FitTransform`](tfidftransformer-fittransform.md), [`CsrMatrix`](../../abstractions/sparse/csrmatrix.md).
