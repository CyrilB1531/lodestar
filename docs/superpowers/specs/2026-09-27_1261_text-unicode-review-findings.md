# Lodestar.Text's Unicode review findings before 1.0

**Issues:** [#1261](https://github.com/CyrilB1531/lodestar/issues/1261),
[#1262](https://github.com/CyrilB1531/lodestar/issues/1262),
[#1263](https://github.com/CyrilB1531/lodestar/issues/1263),
[#1264](https://github.com/CyrilB1531/lodestar/issues/1264).
**Status:** written with the work, 2026-09-27.
**Date:** 2026-09-27.

## The problem

The full-repository review of `main` at `9b142ab2` found four places where `Lodestar.Text` read a
string in UTF-16 units where its Python reference reads a `str`, whose unit is the code point:

1. The q-gram measures at `TextElement.CodePoint` rebuilt each gram as a string through
   `char.ConvertFromUtf32`, which refuses a lone surrogate. `Jaccard.Similarity("a\uD800b", "ab",
   1, TextElement.CodePoint)` threw where textdistance answers `0.667`; so did `SorensenDice`,
   `Overlap`, `Cosine` and `Tversky`.
2. A `TokenPattern` with a capturing group yielded the whole match. scikit-learn's
   `build_tokenizer` returns `re.findall`, which returns the group: `(\w+)ing\b` over "running and
   jumping" is `['jump', 'runn']` there and `[jumping, running]` here. A second group, which
   scikit-learn refuses, was accepted.
3. The `char` and `char_wb` analyzers cut n-grams over UTF-16 units: `"a😀"` gave `a` and the two
   halves of the pair where scikit-learn gives `a` and `😀`. `Utf8JsonWriter` writes a lone
   surrogate as U+FFFD, so a saved model held duplicate keys and failed `Load`, or lost a count;
   `HashingVectorizer` hashed both halves into one bucket.
4. The vocabulary sorted with `StringComparer.Ordinal`. A supplementary character's leading
   surrogate (U+D800–U+DBFF) sorts below U+E000–U+FFFF, so `𠮟る` preceded `ｶﾀｶﾅ` where Python puts
   it after. `Rake`'s tie-break did the same and documented it.

## Decisions

1. **The q-grams of the code-point mode are sorted integer runs**, the machinery the UTF-16 mode
   already used, made generic over the element. A lone surrogate is an `int` like any other; the
   dictionary of rebuilt strings, and its allocation per gram, is gone.
2. **`PythonTokenPattern` returns `re.findall`'s item**: the whole match without a group, the
   group's last capture with one, and the empty string where the group took no part. More than one
   group is refused with an `ArgumentException` when the word analyzer is built, as
   `build_tokenizer` raises `ValueError`; the character analyzers neither compile nor check the
   pattern, as scikit-learn does not. `Rake` and `TextRank` judge the gap between two tokens on
   the whole match, since the characters the pattern consumed around its group are not a
   separator: rake-nltk with `(\w+)ing\b` gives `runn jump` for "running jumping", one phrase.
3. **Character n-grams index code points.** A document or padded word holding a surrogate pair
   gets a table of each code point's UTF-16 offset, and a gram is the span between two of them;
   text without a pair keeps slicing directly, so the common case pays one counting pass. The
   Python-slice path for a length below 1 counts in code points too.
4. **Strings sort by code point through one comparer, `CodePointOrder`**, shared source in
   `src/Shared` behind `LodestarIncludesCodePointOrder`, so `Lodestar.Preprocessing`'s categories
   ([#1254](https://github.com/CyrilB1531/lodestar/issues/1254)) can take the same one. The
   vocabulary, the load-time order check and `Rake`'s tie-break use it, and the documented Rake
   divergence is removed rather than kept.
5. **A lone surrogate survives `Save` and `Load`.** It still reaches a term when the input holds one,
   as in Python, and `Utf8JsonWriter` writes it as U+FFFD, so such a model held duplicate keys or a
   term its counts no longer reached. `JsonArtifact.WriteText` writes it as a `\uD800` escape and
   passes the text around it through the writer's own encoder, and `GetText` unescapes a token only
   when it holds such an escape outside a pair, since `Utf8JsonReader.GetString` refuses that one
   escape and takes every other. Terms, stop words, the token pattern and `EmbeddingIndex`'s ids go
   through them, the ids having the same fault in the same shared writer. Text without a lone
   surrogate is written byte for byte as before, so no existing artifact changes, and Python's
   `json` reads the escape in either case.

## Verification

- New oracle cases, which fail on `main`: five over an astral corpus (the word analyzer's order,
  `char` at `(1, 2)` and `(0, 2)`, `char_wb` at `(1, 3)` and `(-1, 1)`), three token patterns with
  a group, and a `HashingVectorizer` `char` case over the same corpus.
- Unit tests for what JSON cannot carry or the corpora do not cover: the lone surrogate against
  textdistance 4.6.3 at `qval` 1 and 2, the save/load round trip of astral grams, the load-time
  refusal of a UTF-16-ordered vocabulary, the two-group refusal, and `Rake`'s tie-break and gap
  rule against rake-nltk 1.0.6.
- A random differential against textdistance 4.6.3 and scikit-learn 1.9.1 over text drawn from BMP,
  halfwidth, private-use, astral and lone-surrogate characters: 3,354 set-similarity pairs across
  the five measures and `qval` 1 to 3, and 3,000 `CountVectorizer` fits across the three analyzers,
  six n-gram ranges and six token patterns, the two-group one included, each saved and reloaded.
  On `main`: 2,249 throws, 1,407 vocabulary or count mismatches, 119 refusal mismatches and 1,711
  fits that did not survive `Save` and `Load`. Here: 0.

## Rejected

- **Keying the code-point grams on strings built without `ConvertFromUtf32`**, the issue's
  direction taken literally. It fixes the throw but keeps an allocation per gram, where the
  integer runs need none.
- **Loading a vocabulary saved in the old UTF-16 order.** The files concerned hold an astral term
  beside one in U+E000–U+FFFF, the case that computed the wrong columns in the first place, and the
  project is before 1.0.

## Consequences

- A vectorizer saved before this change with a one-group `TokenPattern` still loads, but its
  vocabulary holds whole matches and the tokens are now the group's text, so `Transform` finds
  fewer terms. Refit it. No such model can have matched scikit-learn in the first place.
- A null `TokenPattern` is refused when the vectorizer is built, whatever the analyzer, since the
  character analyzers no longer compile it and `Load` requires a string where `Save` would have
  written `null`. It used to throw a `NullReferenceException` there.
