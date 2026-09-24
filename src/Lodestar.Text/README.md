# Lodestar.Text

String distances, phonetic codes, set similarity, stemmers, tokenizers and sparse text
vectorizers, native and allocation-lean. Levenshtein with a bit-parallel path, Jaro-Winkler,
Damerau-Levenshtein and the rest of the edit distances; Soundex, Metaphone, Double Metaphone and
NYSIIS; Snowball stemmers for fifteen languages; `CountVectorizer`, `TfidfVectorizer` and
`HashingVectorizer` producing a sparse matrix; BM25 and keyword search; RAKE and TextRank
keyword extraction; a BK-tree for dictionary lookup; MinHash, SimHash and LSH sketches.

## Install

```bash
dotnet add package Lodestar.Text
```

## Example

```csharp
using Lodestar.Text.Distances;

int edits = Levenshtein.Distance("kitten", "sitting");                   // 3
double similarity = Levenshtein.NormalizedSimilarity("kitten", "sitting"); // 0.5714…
```

## Parity

Each function is replayed against the Python library it mirrors: rapidfuzz, jellyfish,
textdistance, difflib, scikit-learn's text feature extraction and nltk's Snowball stemmers.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [quickstart](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/quickstart.md)
- Guide: [vectorization](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/vectorization.md)
- Guide: [dictionary lookup](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/dictionary-lookup.md)
- Guide: [keyword extraction](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/keyword-extraction.md)
- Guide: [keyword search](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/keyword-search.md)
- Reference: [text/core](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/core.md)
- Reference: [text/distances](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/distances.md)
- Reference: [text/indexing](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/indexing.md)
- Reference: [text/keywords](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/keywords.md)
- Reference: [text/persistence](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/persistence.md)
- Reference: [text/phonetics](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/phonetics.md)
- Reference: [text/search](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/search.md)
- Reference: [text/similarity](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/similarity.md)
- Reference: [text/stemming](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/stemming.md)
- Reference: [text/vectorizers](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/text/vectorizers.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Text/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Text/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
