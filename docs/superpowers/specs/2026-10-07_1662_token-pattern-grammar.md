# Token patterns read by Python's grammar, and a read-only fitted idf

**Issues:** [#1627](https://github.com/CyrilB1531/lodestar/issues/1627),
[#1650](https://github.com/CyrilB1531/lodestar/issues/1650),
[#1662](https://github.com/CyrilB1531/lodestar/issues/1662),
[#1663](https://github.com/CyrilB1531/lodestar/issues/1663).
**Status:** written with the work, 2026-10-08.
**Date:** 2026-10-07.

## The problem

The token-pattern translation scanned a Python pattern for its classes and escapes and left the rest to
.NET, which reads a good part of Python's grammar otherwise:

- a `[` inside a `(?x)` comment or after `\c` opened a class up to a later `]`, which could swallow a
  live group: the groups were then numbered on a spelling that lacked one, and every token was empty
  (#1663);
- a `(?#...)` comment ended at a different `)` in the spelling parsed at construction than in the one
  compiled at the first emoji, which then threw (#1662);
- surrogates in a class, a range ending in a supplementary character, a quantified supplementary
  character, a match starting between the two halves of a pair, `\Z` and `{,n}` read otherwise than
  Python reads them, and constructs Python refuses, `(?<n>…)` or `\p{L}`, were translated rather than
  read as 0.7.0 read them (#1650);
- `TfidfTransformer.Idf`, and through it `TfidfVectorizer.Idf`, handed out the fitted array, which a
  cast to `double[]` edits under every later `Transform` and `Save` (#1627).

## Decisions

- **The pattern is parsed by a grammar that mirrors CPython 3.12's `re/_parser.py`**, written here from
  its documented behaviour: escapes with their exact digit counts, group references checked against
  open and lookbehind groups, repeats with Python's "nothing to repeat" and "multiple repeat", a `{`
  opening no repeat read as a literal, groups, lookarounds with a fixed-width lookbehind, numbered
  conditionals, global flags only at the start and scoped ones. Comments, `(?#...)` and verbose ones,
  are dropped, so no `[` or `)` inside one moves a class or a group, and an escaped `)` or a `\` before a
  newline ends none, as Python's tokenizer reads them. A possessive repeat after the `{,n}` .NET reads as
  text is an atomic group; a flag both set and cleared is refused; a look-behind wider than Python's
  `2**32 - 1` is refused. Nesting is counted as Python's frames, two per group and one per conditional, and
  past 960 of them — Python 3.12 runs out past 495 groups or 990 conditionals — or on a thread short of
  stack, the pattern is read as written rather than ending the process on a stack overflow.
- **The groups are numbered on the spelling compiled**, as Python numbers them: every capturing group
  stays one, in order. #1660 numbered them on the pattern as written, which a comment can make other
  than Python's — `(?#\)(a)` has no group in Python and one in .NET — and a translation dropping one
  then gave empty tokens (#1663).
- **A construct Python refuses is read as 0.7.0 read it**, by .NET as written: the translation throws,
  and the token pattern falls back to the raw reading. Where .NET refuses the pattern as written, the
  construction refuses it with .NET's exception, as 0.7.0 did.
- **A code point is one character, as in Python's `str`.** A supplementary literal is spelled as its
  pair in a group, so a quantifier binds the pair; `.` takes a pair whole; a lone surrogate matches only where the text holds
  it alone; a class splits each range into its BMP, surrogate and supplementary parts; no match starts
  between the two halves of a pair.
- **The construction parses the plain spelling, not the pair-aware one.** The pair-aware spelling
  differs from the plain one only by fragments written here, each an atom where the plain spelling has
  one and none capturing, so one parses exactly when the other does, and the groups are numbered alike;
  a theory over 50 patterns, lone surrogates, supplementary sets and refused ones included, asserts
  both. The stand-in spelling #1660 parsed instead is gone: with comments in the pattern it ended them
  elsewhere (#1662), and parsing it was 8 KB of the construction's 11.
- **The fitted idf is handed out as a read-only view**, built at the first read and reset at each fit
  or load. The tests that plant a non-finite weight reach the array through an internal seam.

## Measured against the release

BenchmarkDotNet 0.14, .NET 10.0.401, AMD Ryzen 7 8700G, Linux x64; `TokenPatternBenchmarks`, the
release built from nuget.org (Lodestar.Text 0.7.0) and this branch from the working tree.

| pattern | `Construct`, 0.7.0 | `Construct`, this branch | with a first fit, 0.7.0 | with a first fit, this branch |
| --- | --- | --- | --- | --- |
| `[^ ]+` | 3.5–3.6 µs, 8.34 KB | 1.0 µs, 4.53 KB | 12.15 KB | 16.46 KB |
| `[a-z]+` | 3.6 µs, 8.16 KB | 1.1 µs, 4.58 KB | 11.7 KB | 16.11 KB |
| `[A-Za-z]\w+` | 5.3 µs, 10.27 KB | 2.0 µs, 5.96 KB | 14.2 KB | 21.48 KB |
| `\b\w\w+\b` | 6.7 µs, 11 KB | 0.7 µs, 1.84 KB | 15 KB | 3.78 KB |
| `\S+` | 4.4 µs, 8.85 KB | 1.2 µs, 4.51 KB | 12.71 KB | 18.18 KB |

Construction alone is under the release on every pattern, and under main before this branch, which
spent 0.7–3.1 µs and 1.8–9.0 KB. A construction and a first fit allocate 4.3–7.3 KB more than the
release for the four translated patterns, within the 5–10 KB the maintainer accepted on 2026-10-07, and
1–3 KB less than main; for the default pattern, scanned by hand, 11.2 KB less. A fit of
5,000 documents runs at main's time and allocation for every pattern.

## Parity with Python

A differential run, 99 generated patterns over 60 documents each holding emoji, astral letters and lone
surrogates, compared the vocabulary `CountVectorizer` builds with scikit-learn's on Python 3.12. Every
mismatch is by design: four patterns .NET refuses as written, which the construction refuses as 0.7.0
did.

## Rejected

- **Parsing the pair-aware spelling at construction**, as main did before #1660: it is the longer
  spelling, and the plain one parses exactly when it does.
- **Keeping the scanner and teaching it comments and `\c`.** Each item of #1650 was another construct
  the scanner left to .NET; a grammar that reads the whole pattern as Python does closes the class of
  defect rather than its instances.
