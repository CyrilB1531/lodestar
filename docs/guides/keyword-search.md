# Keyword search

BM25 over the matrix you already have, and how to fuse its ranking with a vector one.

## What you need first

A document-term matrix of **raw counts**. That is what
[`CountVectorizer`](../reference/text/vectorizers/countvectorizer.md) returns, and everything that
shapes it — the analyzer, the n-gram range, the stop words, `MinDf` — is decided there rather than
here.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);

var index = new Bm25Index(counts);
int documents = index.DocumentCount;  // => 3
```

**Do not hand it a `TfidfVectorizer` matrix.** BM25's saturation and length normalization replace
TF-IDF's; scoring an already-weighted matrix applies two weightings and the numbers stop meaning
anything. This is the one mistake worth naming up front.

## Scoring a query

Queries are **column indices**, because the vocabulary is the vectorizer's and this type does not
own it. `GetFeatureNames()` is the map.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);
IReadOnlyList<string> vocabulary = vectorizer.GetFeatureNames();
int cat = vocabulary.ToList().IndexOf("cat");

IReadOnlyList<SearchHit> hits = new Bm25Index(counts).Top([cat], 2);

int best = hits[0].Document;  // => 0
```

One entry per query **occurrence**: a term given twice counts twice, which is how the reference
counts it. A term the corpus never saw is ignored rather than refused.

## Reading the numbers

Two properties surprise people, and both are the weighting working as published rather than a bug.

**A common term is worth almost nothing.** `cat` is in one document of three and scores `0.5755…`;
`sat` is in two of three and scores `0.1013…` on the document that holds it *twice*. That is
inverse document frequency doing its job — nearly six times between them for the same kind of
match.

**A score can be negative.** Robertson's IDF goes below zero once a term is in more than half the
corpus. The default variant floors those negatives at a fraction of the mean IDF — and when that
mean is itself negative, the floor is too, so scores go negative and a *longer* document scores
higher. If that is awkward downstream, [`Bm25Idf.Lucene`](../reference/text/search/bm25idf.md) is
non-negative by construction.

The default `K1` is **1.5**, `rank_bm25`'s, not the 1.2 Lucene uses. Numbers ported from a Lucene
index will not line up unless you say so.

## Hybrid search: fusing two rankings

The reason [`RankFusion.Rrf`](../reference/text/search/rankfusion-rrf.md) exists is that a BM25
score and a cosine similarity are **not comparable**. Normalising two score distributions and adding
them is a guess. Reciprocal rank fusion reads only positions, so neither scale has to be reconciled.

```csharp
using Lodestar.Text.Search;

// One ranking from BM25, one from a vector search over the same documents.
int[] keyword = [2, 0, 1];
int[] vector = [0, 1, 2];

IReadOnlyList<SearchHit> fused = RankFusion.Rrf([keyword, vector]);

int best = fused[0].Document;  // => 0
int worst = fused[2].Document;  // => 1
```

Document 2 is ranked first by the keyword side and last by the vector one; document 0 is ranked
second and then first, and wins. At `k = 60` consecutive ranks are close together, so consistency
across rankings beats a single strong opinion — which is what `k` is for.

Rankings of different lengths need no padding — a BM25 top-10 and a vector top-100 fuse directly,
because a document absent from a ranking simply contributes nothing from it.

`k` defaults to 60 and flattens the advantage of a top position as it grows: at `k = 1` first place
is worth `1/2` against second's `1/3`; at `k = 1000` the two are within a thousandth.

## What this is not

No index on disk, no `Directory`, no `IndexWriter`, no codec, no postings format, and no Block-Max
WAND top-k. Scoring walks every document for every query term, which is what makes it simple and
what makes it linear in the corpus. If the corpus does not fit in memory, or queries are hot enough
that the walk shows, a real search engine is the answer and this is not trying to be one.

## See also

- [Keyword search reference](../reference/text/search.md) — the types.
- [Python → C# equivalence](../equivalence.md) — the `rank_bm25` call this replaces.
- [From string to vector](vectorization.md) — where the matrix comes from.
