# Changelog — Lodestar.Extensions.VectorData

What changed in `Lodestar.Extensions.VectorData`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- A record type with two vector properties is refused at construction with `NotSupportedException`, where the first reflection listed was searched. ([#1212](https://github.com/CyrilB1531/lodestar/issues/1212))
- A record type with two keys, a property marked for two roles, a key, vector or sole full-text property without a getter, or a definition listing a property twice or naming one the type lacks is refused at construction with `ArgumentException`. ([#1212](https://github.com/CyrilB1531/lodestar/issues/1212))
- Two full-text indexed properties fail `HybridSearchAsync` with `NotSupportedException`, where the first was indexed. ([#1212](https://github.com/CyrilB1531/lodestar/issues/1212))
- A `VectorProperty` or `AdditionalProperty` selecting any property but the one searched is refused with `ArgumentException`, where it was ignored. ([#1212](https://github.com/CyrilB1531/lodestar/issues/1212))
- Of a base property and the `new` member hiding it, the schema reads the member, and an attribute-read schema's refusals name no parameter. ([#1212](https://github.com/CyrilB1531/lodestar/issues/1212))
- A disposed `LodestarVectorStore` refuses its name lookups with `ObjectDisposedException`, and `ListCollectionNamesAsync` refuses a token already cancelled before it reads any name, where an empty store answered nothing. ([#1266](https://github.com/CyrilB1531/lodestar/issues/1266))
- A collection whose texts yield no term still degrades to the vector ranking once `Lodestar.Text` refuses such a corpus, as scikit-learn does; crossed `MinDf`/`MaxDf` bounds still throw. ([#1239](https://github.com/CyrilB1531/lodestar/pull/1239))
- The `Lodestar.Embeddings` and `Lodestar.Text` dependency floors rise from 0.6.0 to 0.8.0 and 0.7.0, the releases that forward their data types to `Lodestar.Abstractions`. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- A write costs its own record, its vector normalized into its slot and its text tokenized once by the next hybrid search, where every write made the next search rebuild both indexes over every record: replacing one record then searching 100,000 takes 4.1 ms where it took 148 ms. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- `HybridSearchAsync` reads both rankings only as deep as the fused top needs, where it sorted the whole vector ranking and fused every record, 3.2× to 4.5× faster at 100,000 records. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- A vector search no longer reads the keyword options, so options the records refuse fail `HybridSearchAsync` alone, and a record changed in place after its upsert is searched as it was written rather than as the next rebuild found it. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214), [#1211](https://github.com/CyrilB1531/lodestar/issues/1211))
- `LodestarVectorStore.GetCollection` disposes the deleted collection a name held when another key or record type gets a new collection under it, so a handle kept to the old one is to be dropped. ([#1821](https://github.com/CyrilB1531/lodestar/issues/1821))
- `SearchAsync` and `HybridSearchAsync` refuse a null search value with an `ArgumentNullException` naming `searchValue`, where it was refused as not a vector, even one declared a `float[]`. ([#1804](https://github.com/CyrilB1531/lodestar/issues/1804))

### Fixed

- A refusal shows a collection name, key text, distance function, definition property name or a name in a list of several bare only when it is one word of letters but modifier ones, decimal digits, non-enclosing marks, `_` and `-` holding a letter or digit, and quoted otherwise, escaping quotes and backslashes and writing what prints nothing or as blank but the space, and a combining mark opening the text or following a space or an escape, as `\u` escapes, where only an empty or blank collection name, key text or distance function was quoted and the rest printed raw. ([#1820](https://github.com/CyrilB1531/lodestar/issues/1820))
- A record type whose marked key or vector a `new` member hides is refused naming the hidden property, as a hidden full-text property already was, and of several hidden properties in any of the three roles the refusal names the ordinal first, whatever order reflection lists them in. ([#1799](https://github.com/CyrilB1531/lodestar/issues/1799), [#1805](https://github.com/CyrilB1531/lodestar/issues/1805))
- A refusal names each type it prints, a refused selector apart, which prints as its expression, by its CLR name with its own type arguments in place of the arity suffix, and tells apart any two of them that would read alike; a held collection is named by its key and record types, and a search value declared `object` by its runtime type. ([#1799](https://github.com/CyrilB1531/lodestar/issues/1799), [#1804](https://github.com/CyrilB1531/lodestar/issues/1804), [#1807](https://github.com/CyrilB1531/lodestar/issues/1807))
- A write reads a record's key, vector and full-text property once, before any record is written, so the vector stored is the one whose width was checked and a getter that throws fails a batch that wrote nothing, with what it threw rather than a `TargetInvocationException`. ([#1801](https://github.com/CyrilB1531/lodestar/issues/1801))
- A collection's exists and deleted flags are volatile, so each read the store makes of one is a fresh acquire load rather than a cached value; the two are written and read one at a time, not as a pair. ([#1802](https://github.com/CyrilB1531/lodestar/issues/1802), [#1806](https://github.com/CyrilB1531/lodestar/issues/1806))
- `LodestarVectorStore` reads and writes the names it holds under a lock, so concurrent `GetCollection` calls for new names get one collection per name where they could corrupt the dictionary. ([#1266](https://github.com/CyrilB1531/lodestar/issues/1266))
- Before .NET 6 — on .NET Framework, .NET Core and .NET 5, and on legacy Mono, held there unmeasured — the refusals of an array past the largest one read those runtimes' bound for elements wider than a byte, `0x7FEFFFFF`, where a count between it and `Array.MaxLength` passed them and failed to allocate. ([#1614](https://github.com/CyrilB1531/lodestar/issues/1614))
- A query is normalized in double, as the stored vectors are, so one whose components pass about 1e19 ranks by direction where its float norm overflowed and every score came back zero. ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214))
- Collections refuse a record past the largest array before resizing, name a null collection name or key under the caller's parameter, and return a cancelled task, writing nothing, for a token already cancelled. ([#1338](https://github.com/CyrilB1531/lodestar/issues/1338), [#1267](https://github.com/CyrilB1531/lodestar/issues/1267), [#1353](https://github.com/CyrilB1531/lodestar/issues/1353), [#1354](https://github.com/CyrilB1531/lodestar/issues/1354))

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
