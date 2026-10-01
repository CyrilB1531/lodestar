# Changelog — Lodestar.Extensions.VectorData

What changed in `Lodestar.Extensions.VectorData`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- A collection whose texts yield no term still degrades to the vector ranking once `Lodestar.Text` refuses such a corpus, as scikit-learn does; crossed `MinDf`/`MaxDf` bounds still throw. ([#1239](https://github.com/CyrilB1531/lodestar/pull/1239))
- The `Lodestar.Embeddings` and `Lodestar.Text` dependency floors rise from 0.6.0 to 0.8.0 and 0.7.0, the releases that forward their data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- A write costs its own record, its vector normalized into its slot and its text tokenized once by the next hybrid search, where every write made the next search rebuild both indexes over every record: replacing one record then searching 100,000 takes 4.1 ms where it took 148 ms. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- `HybridSearchAsync` reads both rankings only as deep as the fused top needs, where it sorted the whole vector ranking and fused every record, 3.2× to 4.5× faster at 100,000 records. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- A vector search no longer reads the keyword options, so options the records refuse fail `HybridSearchAsync` alone, and a record changed in place after its upsert is searched as it was written rather than as the next rebuild found it. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))

### Fixed

- Before .NET 6 — on .NET Framework, .NET Core and .NET 5, and on legacy Mono, held there unmeasured — the refusals of an array past the largest one read those runtimes' bound for elements wider than a byte, `0x7FEFFFFF`, where a count between it and `Array.MaxLength` passed them and failed to allocate. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- A query is normalized in double, as the stored vectors are, so one whose components pass about 1e19 ranks by direction where its float norm overflowed and every score came back zero. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- Collections refuse a record past the largest array before resizing, name a null collection name or key under the caller's parameter, and return a cancelled task, writing nothing, for a token already cancelled. ([#1338](https://github.com/CyrilB1531/lodestar/issues/1338), [#1353](https://github.com/CyrilB1531/lodestar/issues/1353), [#1354](https://github.com/CyrilB1531/lodestar/issues/1354))

## [0.1.0] — 2026-09-24

### Added

- The package: an in-process `Microsoft.Extensions.VectorData` store with hybrid keyword and vector search. ([#682](https://github.com/CyrilB1531/lodestar/issues/682), [`afc1909d`](https://github.com/CyrilB1531/lodestar/commit/afc1909d))

### Changed

- A filtered `SearchAsync` runs the filter once on every record in the order held and scores only the admitted ones, 1.9× faster at 10,000 records, where it used to rank the whole collection and stop filtering once enough records passed. ([#849](https://github.com/CyrilB1531/lodestar/issues/849))

### Fixed

- `HybridSearchAsync` keeps a record the keywords matched in the keyword ranking when its BM25 score is zero or negative, where it used to drop it as unmatched. ([#884](https://github.com/CyrilB1531/lodestar/issues/884))
- A key property of another type than the collection's key or a non-`string` full-text property is refused at construction, a deleted name can be taken by another record type, and an empty collection refuses a query of the wrong width. ([#904](https://github.com/CyrilB1531/lodestar/issues/904))
- `HybridSearchAsync` ranks only the records a keyword matched, read from the term postings, where it scored and sorted every record of the collection on every query. ([#993](https://github.com/CyrilB1531/lodestar/issues/993))
- `HybridSearchAsync` sorts a keyword's matched records in one array by their own scores, where #993 gathered them in a sorted set and sorted through the whole score array, which made a keyword most records hold slower than before #993. ([#1036](https://github.com/CyrilB1531/lodestar/issues/1036))
