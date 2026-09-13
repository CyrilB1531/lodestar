---
status: accepted
supersedes: []
amends: ["0054"]
applies: []
---
# 0120 — A stream with no declared length is pooled too

**Status:** accepted · **Date:** 2026-09-13 · **Amends:** [`0054`](0054-the-payload-buffer-is-pooled-after-all-because-the-collection-is-the-cost.md)

## Context

[0054](0054-the-payload-buffer-is-pooled-after-all-because-the-collection-is-the-cost.md) rented
the payload buffer for a stream that declares its length, and carved out the other case in its
consequences:

> **A stream with no declared length is not pooled.** The growable path sizes itself as it reads
> and owns the buffer it grows, so `RentedPayload` carries it owning nothing and `Dispose` is a
> no-op.

That path is not rare. A `GZipStream` — the recipe
[0044](0044-compression-belongs-to-the-caller.md) hands the caller for compression — a network
stream and a pipe all take it. It accumulated into a `MemoryStream` that doubled from nothing, so a
20.6 MB artifact grew through fresh zeroed arrays up to 32 MB and copied everything read so far at
each growth.

**Nobody had measured it**: no BenchmarkDotNet row reached a stream without a length, and the
nightly's `embedding_index_load_gzip` row is dominated by the inflate, so the growth hid inside it.

## The measurement

`PersistenceBenchmarks.EmbeddingIndexLoadGzip`, added for this, AMD Ryzen 7 8700G, .NET 10.0.12,
BenchmarkDotNet `DefaultJob`, before and after interleaved twice
([#716](https://github.com/CyrilB1531/lodestar/issues/716)):

| | before | after |
| --- | ---: | ---: |
| mean | 50.678 / 50.383 ms | 43.078 / 43.048 ms |
| allocated per load | 91 562.54 KB | 16 095.15 KB |
| gen0 / gen1 / gen2 per 1 000 loads | 600 / 500 / 500 | 83 / 0 / 0 |

`EmbeddingIndexLoad`, which reads a declared-length stream and is untouched, read 4.796 / 4.872 ms
before and 4.887 / 4.747 ms after — the control crosses in both directions.

**It is 0054's mechanism again.** The time follows the collections, and the collections follow
91.6 MB of large-object allocation, none of which survives the load.

## Decision

**A stream with no declared length is read into rented 1 MiB segments, then copied once into one
rented buffer sized to the total**, and that buffer is handed to the parser as a `RentedPayload`
that owns it. A stream that ends inside its first segment is parsed from that segment, with no
second copy.

- **Segments, not rented doubling.** Doubling through the pool copies everything read so far at
  every step, and rents a 32 MB bucket on the way to the final one. Segments copy each byte once.
- **1 MiB.** Small enough that the pool's power-of-two rounding wastes nothing, large enough that a
  20 MB index is a score of reads.
- **`MaxTotalBytes` is checked as each read lands**, exactly as the growable path checked it, so a
  hostile stream is refused before it is buffered past the limit rather than after.

## Consequences

- **0054's carve-out is withdrawn**; the rest of 0054 stands. Its residency argument applies with
  one addition: the pool now also keeps about the artifact's size in 1 MiB arrays, beside the
  power-of-two bucket 0054 already accepted.
- **The same tail hazard 0054 named applies to the segments**, and is guarded the same way: only
  the byte count bounds what is parsed. `EmbeddingIndexReadPathTests` loads a short artifact
  through a non-seekable stream after a long one, and covers a read spanning several segments,
  one ending exactly on a segment boundary, and one refused mid-read by `MaxTotalBytes`.
- **Only [`EmbeddingIndex.Load`](../reference/embeddings/search/embeddingindex-load.md) takes it.** The other loaders call `ReadAllBytes`, which keeps the
  unpooled growable path because it returns memory the caller keeps.
- **The asynchronous read is unchanged.** [`LoadAsync`](../reference/embeddings/search/embeddingindex-loadasync.md) on a stream with no length still grows a
  `MemoryStream`; it is the next place to apply this, and was not measured here.
- **What would reopen this** is 0054's own condition: a caller measured on peak resident memory in
  a short-lived process rather than on load time.
