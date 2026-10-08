# CountVectorizerOptions

Everything that decides what counts as a term, before anything is counted.

<!-- docs-declaration -->

```csharp
public sealed record CountVectorizerOptions
```

**Properties** — `Lowercase` (default `true`) folds case before tokenizing, so `Apple` and `apple`
are one term. `TokenPattern` (default `\b\w\w+\b`) is the regular expression a token must match —
note the two `\w`, which is why **single-letter words are dropped**. It is read as Python's `re`
reads it, not as .NET does: `\w` is a letter, a number of any kind or `_` and never a combining
mark, `\b`, `\d` and `\s` follow, all over code points, and scikit-learn's own spelling
`(?u)\b\w\w+\b` is accepted; so are Python's repeats, groups, flags and comments, read as Python reads them,
while a construct Python refuses — `\p{L}`, `(?<name>…)` — is read as .NET reads it, as 0.7.0 did
([#1650](https://github.com/CyrilB1531/lodestar/issues/1650)). A token is what `re.findall` returns: the whole match, or the text of
the pattern's one capturing group when it has one — `(\w+)ing\b` makes `running` the token `runn`,
and a group that took no part in a match gives the empty token. A pattern with two groups or more
gives the whole match, as 0.7.0 read it, where scikit-learn refuses it
([#1657](https://github.com/CyrilB1531/lodestar/issues/1657)); every analyzer refuses a null pattern, or one .NET refuses as written, as 0.7.0 did.
`Analyzer` (default
[`AnalyzerKind.Word`](analyzerkind.md)) chooses words or character n-grams. `NgramRange` (default
`(1, 1)`) is the inclusive range of n-gram lengths. `StopWords` (default none) is a set removed
after tokenizing. `StripAccents` (default `false`) decomposes the text (NFKD) and drops every character of a
non-zero combining class, as scikit-learn's `strip_accents="unicode"` does — an acute goes, a
Devanagari vowel sign, of class 0, stays.
`MinDf` and `MaxDf` (defaults `1` and `1.0`) drop terms appearing in too few or too many
documents. `Binary` (default `false`) records presence as `1` rather than the count; `HashingVectorizer`
reads it too, and writes `1` in every bucket a term reached. `Analyzer` must be one of the three
[`AnalyzerKind`](analyzerkind.md) members: a vectorizer refuses any other value when it is built.

**Example** — the two defaults that surprise people, made visible.

```csharp
using Lodestar.Text.Vectorization;

// "a" is a single letter, so the default token pattern never sees it as a term.
var cv = new CountVectorizer();
int features = cv.FitTransform(["a cat eats"]).ColumnCount;  // => 2
```

**Remarks** — every default here is scikit-learn's, and the properties answer to `lowercase`,
`token_pattern`, `analyzer`, `ngram_range`, `stop_words`, `strip_accents`, `min_df`, `max_df` and
`binary`. Reproducing `\b\w\w+\b` rather than choosing something more obvious is the single
decision that keeps a ported pipeline giving the same columns, and it is also the one that makes
`"I"` and `"a"` vanish from a corpus without saying so.

`MinDf` is read as a count when it is a whole number of at least `1` and as a proportion below `1` —
`MinDf = 2` means two documents, `MinDf = 0.5` means half of them. A fraction above `1` such as
`1.5`, a negative value or `NaN` is neither, and the vectorizer's constructor refuses it with
`ArgumentOutOfRangeException`, as scikit-learn does. `MaxDf` does **not** follow that rule at its
default: `MaxDf = 1.0` is a proportion meaning "in up to all of them", which is why the default
drops nothing. Measured, over two documents sharing `the`, `MaxDf = 1.0` keeps all three terms.
Both properties are `double`, so writing `1` rather than `1.0` changes nothing.

`NgramRange` must ascend, and that is the only thing asked of it: `(2, 1)` is refused by the
vectorizer's constructor with `ArgumentException`, where scikit-learn raises `ValueError`, and
every ascending range is analysed, including one whose first length is below `1`. A `Min` of `0`
or less is what scikit-learn calls a Python slice with, so it adds an empty-string term — the
zero-length slice, taken at every position and so counted once per unit plus one — and a negative
`Min` counts back from the end, `(-1, 1)` asking a character analyzer for the document but its
last character. `AnalyzerKind.Word` at `Max = 1` is the one shortcut that skips the slicing
entirely, so `(0, 1)` is `(1, 1)` there and adds nothing. Measured against scikit-learn 1.9.1,
term for term, and frozen in the oracle corpus.

This is a `record`, so two options objects with the same settings are equal. `StopWords` is
compared **as a set** rather than as a sequence, which is why
[`Equals`](countvectorizeroptions-equals.md) and
[`GetHashCode`](countvectorizeroptions-gethashcode.md) are written by hand rather than
synthesised. `StopWords` is taken and exposed as given, not copied: writing to the caller's collection changes
what these options equal, and every holder of it, a `with` copy included
([#1305](https://github.com/CyrilB1531/lodestar/issues/1305)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CountVectorizer`](countvectorizer.md), [`AnalyzerKind`](analyzerkind.md),
[`StopWords`](stopwords.md), the [Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`CountVectorizerOptions.Equals`](countvectorizeroptions-equals.md) | Value equality, comparing stop words as a set. |
| [`CountVectorizerOptions.GetHashCode`](countvectorizeroptions-gethashcode.md) | A hash consistent with that equality, in O(1). |
