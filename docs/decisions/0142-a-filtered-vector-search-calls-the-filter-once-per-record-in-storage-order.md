---
status: accepted
supersedes: []
amends: ["0123"]
applies: []
---
# 0142 — A filtered vector search calls the filter once per record, in storage order

**Status:** accepted · **Date:** 2026-09-17 · **Amends:** [`0123`](0123-the-vectordata-store-holds-the-records-and-derives-both-indexes.md)

## Context

[Decision 0123](0123-the-vectordata-store-holds-the-records-and-derives-both-indexes.md), decision 2,
made a filtered search exact — `top` means `top` — and described how: **every record is scored and
the filter runs before the cut**. It priced that at nothing, because
[`EmbeddingIndex.Search`](../reference/embeddings/search/embeddingindex-search.md) scored and sorted all
`n` records whatever `k` was asked for, and the filter added "one predicate call per record".

Two things have moved since. [#813](https://github.com/CyrilB1531/lodestar/issues/813) gave
[`EmbeddingIndex.Search`](../reference/embeddings/search/embeddingindex-search.md) a bounded heap, so the sort the argument leaned on is gone. And
[#849](https://github.com/CyrilB1531/lodestar/issues/849) (PR #852) changed the filtered path itself:
before it, every record was scored and sorted, and the filter then walked that ranking and stopped
once `top + Skip` records had passed — at most `n` calls, in score order. Now the filter runs first,
exactly once on every record in the order the collection holds them, only the records it admits are
scored, and a bounded heap keeps the best `top + Skip` (1.4× to 1.9× faster, measured in #849).

## Decision

The exactness decision 0123 took stands unchanged: the filter still applies before the cut, and a
caller asking for five matching records gets five whenever five match.

**The contract of the filter in
[`LodestarVectorStoreCollection.SearchAsync`](../reference/extensions-vectordata/store/lodestarvectorstorecollection-searchasync.md)
is one predicate call per record, in storage order**, whatever `top` is and never in score order.
"Every record is scored" no longer holds: a record the filter rejects costs no dot product. Two
consequences are observable, and are the contract rather than an accident of it:

- **A filter with side effects sees every record**, in storage order, where it used to see a prefix
  of the ranking.
- **A filter that throws on some record fails the search**, even when enough records ahead of it in
  the ranking would have satisfied `top`, where it used to stop before reaching that record.

An expensive filter therefore costs `n` calls even for a `top` of one. The reference page states all
three.

This record covers the vector search only. The hybrid search still filters the fused ranking in rank
order and stops after `Skip + top` admitted records, as 0123 left it.

## Options

- **Keep scoring everything and filter the ranking lazily** (the path before #849): fewer filter calls
  when the filter is permissive and `top` small, but a dot product for every rejected record and a
  sort or heap over all `n`. Lost on the measurement: a selective filter paid for scores it discarded.
- **Filter first, in storage order** (chosen): the rejected records cost one predicate call and
  nothing else, and the filter's calls no longer depend on the query, which makes a side effect or an
  exception reproducible for a given collection.
