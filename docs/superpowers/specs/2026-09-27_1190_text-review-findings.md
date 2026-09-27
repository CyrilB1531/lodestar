# The Text package's review findings before 1.0

**Issue:** [#1190](https://github.com/CyrilB1531/lodestar/issues/1190), with
[#1191](https://github.com/CyrilB1531/lodestar/issues/1191),
[#1192](https://github.com/CyrilB1531/lodestar/issues/1192),
[#1193](https://github.com/CyrilB1531/lodestar/issues/1193),
[#1194](https://github.com/CyrilB1531/lodestar/issues/1194),
[#1195](https://github.com/CyrilB1531/lodestar/issues/1195),
[#1196](https://github.com/CyrilB1531/lodestar/issues/1196),
[#1197](https://github.com/CyrilB1531/lodestar/issues/1197),
[#1198](https://github.com/CyrilB1531/lodestar/issues/1198) and
[#1199](https://github.com/CyrilB1531/lodestar/issues/1199), in one pull request at Cyril's request.
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The pre-1.0 review of `main` at `d228321f` left ten findings on `Lodestar.Text`: four stemmers
missing a rule or searching outside their region, a Porter stemmer applying Martin's extensions
while claiming the 1980 paper, two phonetic encoders off jellyfish on real surnames, a vectorizer
option read by nobody, a saved model that would not load, a list of narrow divergences, and three
algorithms quadratic in memory or time.

## Decisions

1. **Both Porter forms ship** (Cyril, 2026-09-27: "tu prends les 2 versions"). `PorterStemmer.Stem`
   gains a `PorterStemmerMode` overload; `OriginalAlgorithm`, the default, applies the paper's
   rules — `abli`→`able`, no `logi`, short words stemmed — and `MartinExtensions` the three
   departures nltk's `MARTIN_EXTENSIONS` makes. nltk's own default, `NLTK_EXTENSIONS`, is not
   offered: `EnglishSnowballStemmer` is the answer when no index fixes the choice. The enum is data,
   so it lives in `Lodestar.Abstractions` and `Lodestar.Text` reaches that project until the next
   publication, as `Lodestar.Decomposition` does since #1232.
2. **Each stemmer follows the reference decision 0006 names, which is nltk for all six touched.**
   German gains `ung` and the `ig` rule, Portuguese `em`, `ávamos` and nltk's trema folding after
   `g`/`q`, Spanish its RV-bounded search, nltk's step 2b list and nltk's accent folding of the whole word
   after step 0 — `éamos` where Snowball has
   `íamos`, which only a non-word ending in `-éamos` can tell apart — and Italian loses a bare `er`.
   English moves R2 as nltk does, a string emptied by a rewrite longer than it, and Danish undoubles
   any consonant pair over the whole word. Both used to follow Snowball there, against their rows.
3. **The phonetic encoders follow jellyfish 1.2.1 on every input**, recorded as
   [decision 0009](../../decisions/0009-the-phonetic-encoders-follow-jellyfish-whole.md), which
   amends 0005's real-word boundary and withdraws 0007's byte-length divergence. The issues asked
   for `TCH`, `GH` and one NYSIIS rule; the differential test showed the apostrophe, the space and
   the full uppercase mapping moved thousands of dictionary words, so the whole input contract was
   aligned rather than the three rules alone.
4. **The vectorizers keep scikit-learn's stored zeros.** A hashed bucket that cancels stays stored,
   `Binary` sets every stored value to 1, `StripAccents` drops non-zero combining classes (a table,
   .NET exposing none), the loader accepts what the constructors accept, and an undefined
   `AnalyzerKind` is refused at construction.
5. **The narrow divergences align, one by one**: `Tversky` answers textdistance's quick answers,
   `JaroWinkler` takes rapidfuzz's range and cap for the weight jellyfish fixes, `LshIndex.Query`
   honours its documented order, and BM25's floor averages over the terms some document holds.
   `SearchHit` stays a record class: `Lodestar.Extensions.VectorData` 0.x binds `RankFusion.Rrf`'s
   return type as a list of a class, and a struct is a `MissingMethodException` there until that
   package is rebuilt, which the netstandard mirror proved. `MaxDf = 1` and the RAKE default stop
   words stay and their rows say what is true: a `double` cannot carry scikit-learn's `int`/`float`
   distinction, and nltk's stop-word corpus is refused by decision 0002.
6. **The three quadratic paths go linear in memory.** `WordGraph` holds neighbour lists instead of
   two dense `n × n` matrices; `DamerauLevenshtein` strips the common affixes, as rapidfuzz does,
   and runs Zhao and Sahni's three-row recurrence; `BkTree.Nearest` keeps a bounded max-heap.

## Rejected

- **Re-oracling Porter against `MARTIN_EXTENSIONS` alone**, the issue's second option: the class
  and its row promised the paper, and an index built by the paper had no way to be matched.
- **Moving English, Danish and Spanish to `snowballstemmer`**, which the code already half-followed:
  decision 0006 moves a language only on a record per language, and none exists for these three.
- **Fixing only `TCH`, `GH` and NYSIIS's comparison**: it would have left `Keats's`, `O'Brien` and
  every possessive in the dictionary answering differently from jellyfish, under a row claiming
  parity.

## Measured

- Differential tests against the Python references before the push: Porter in both modes over
  73,456 dictionary words, 0 mismatches; the six Snowball stemmers over 8,000 generated words each
  and English over 73,445, 0 against nltk; Metaphone, NYSIIS, Soundex and the match rating codex over
  134,334 dictionary and random Unicode strings, and 33,003 comparison pairs, 0; Damerau-Levenshtein
  over 20,000 random pairs, Tversky over 20,000 weighted pairs, BM25 over 500 matrices with unused
  columns, HashingVectorizer over 400 corpora, 0.
- Two differences the differential tests found beyond the ten findings, taken into this pull
  request at Cyril's request: the token patterns' `\w` matched marks and not astral letters in
  .NET, the reverse of Python's — the patterns are now translated, `\w` being exactly `L*`, `N*`
  and `_` over every scalar — and `CountVectorizer` fitted an empty vocabulary where scikit-learn
  raises, which it now refuses too. `Lodestar.Extensions.VectorData` keeps degrading to the vector
  ranking on texts with no term. Tokenization parity: 6,000 random documents, nine patterns, 0.
- The benchmarks for #1199 are in the pull request and in `bench/README.md`'s section 71.
