# Changelog — Lodestar.Extensions.VectorData

What changed in `Lodestar.Extensions.VectorData`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- The `Lodestar.Embeddings` and `Lodestar.Text` dependency floors rise from 0.6.0 to 0.8.0 and 0.7.0, the releases that forward their data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

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
