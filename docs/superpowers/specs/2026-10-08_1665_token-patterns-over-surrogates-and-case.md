# Token patterns over text holding a surrogate, under `(?i)`, and a read-only vocabulary

**Issues:** [#1665](https://github.com/CyrilB1531/lodestar/issues/1665),
[#1666](https://github.com/CyrilB1531/lodestar/issues/1666),
[#1667](https://github.com/CyrilB1531/lodestar/issues/1667),
[#1668](https://github.com/CyrilB1531/lodestar/issues/1668),
[#1669](https://github.com/CyrilB1531/lodestar/issues/1669),
[#1670](https://github.com/CyrilB1531/lodestar/issues/1670).
**Status:** written with the work, 2026-10-08.
**Date:** 2026-10-08.

## The problem

The Review B after #1664, on `main` at `41da8898`, measured the pair-aware spelling, the one a text
holding a surrogate takes, which no in-repo benchmark covers, and fuzzed the grammar against Python
3.12's `re`:

- a fit over such text ran up to ten times slower than Lodestar.Text 0.7.0 on a short document with
  `\w`, which compiled a 5,000-character supplementary set, and up to three times slower on a large
  one, the per-position guard against starting inside a pair defeating .NET's searches (#1665);
- a greedy loop that fails at the end of a long text, `.+x` or `\S+\d`, ran past the 1 s timeout,
  where 0.7.0 and Python complete (#1666);
- the pair-aware spelling, translated at the first text holding a surrogate, fell back for good to
  the plain one on a thread short of stack (#1667);
- `(?i)` was handed to .NET, which folds `ſ`, `İ` and supplementary letters otherwise
  than Python (#1668);
- `\B` matched the empty document (#1669);
- `GetFeatureNames` handed out the vocabulary array, which a cast edits under a later `Save` (#1670).

## Decisions

- **Each item takes whole code points, in branches that take disjoint units.** `.`, `\w`, `\S`,
  `\W`, `\D` and the classes are alternations of a BMP unit, a pair and a lone surrogate, each branch
  but the pair's checking it does not start one, so backtracking never gives a pair up halfway. The
  branch that could take a pair's low half checks it does not start there: without it, a search
  starting on a low half ran a loop to the end of the text before the scan dropped the match,
  `\W\w*` over 20,000 CJK letters 6.9 s against 70 ms. A back-reference checks where it ends.
- **Branches of one literal, class or category each are one class**, as Python's parser makes them a
  charset: as alternatives, `(?:\w|\S)+!` over 30 letters ran past the timeout. Each branch is read on
  its own, so a `-` or an escape ending one never runs into the next.
- **The check that no match starts inside a pair is made outside the pattern.** In front of the
  pattern it cost .NET its search for what follows a loop, 4.3 s against 1 ms; the scan drops a match
  starting on a low half and searches again from the next unit, as Python moves past a position its
  `str` does not have.
- **The pair-aware spelling is interpreted for a text of 256 units or fewer until 65,536 units have
  been scanned, compiled otherwise.** Compiling the spelling of `\w` took 10 ms, where 0.7.0's whole
  first fit took 1; interpreted, a short document takes 50 µs, and a large corpus pays the
  compilation once. Past 256 units the interpreter backtracks too slowly: `\W+\d` over 2,000 units of
  emoji ran past a second, where compiled it takes 150 ms. The interpreted one keeps 0.7.0's second,
  and a text that reaches it is read again compiled; a pattern repeating an item that can match empty,
  `(?:a|)+?`, is never interpreted, .NET's two engines disagreeing on it — `(?:a|)+?.+` over `𐐀𐐀😀`
  at 3 interpreted, at 0 compiled and in Python — so it reads alike whatever the text's length. Where dynamic code
  is not supported — NativeAOT, iOS, WebAssembly — .NET interprets every pattern, so such a pattern can
  match there as its interpreter does, or throw its `OverflowException`; each search starts past the
  last kept, so a scan ends wherever an engine hands back a match before the search start.
- **Over a text holding a surrogate, a token pattern has no match timeout, as Python's has none.** A
  loop that fails at the end of a run of supplementary letters walks back by code point, as in Python:
  `\w+\d` over 10,000 of them takes Python 0.6 s, `\W+\W+\d` over 4,096 units 18 s, cubic with two
  loops; 0.7.0 finished both in milliseconds by reading no supplementary letter as one, and its 1 s
  would turn them into timeouts. A timeout growing with the square of the length, chosen first on
  2026-10-08, held one loop and not two, and a faster spelling of `\W` — a positive set of pairs, or
  the second loop made atomic — took 1.28 and 0.55 s against 1.22 at 1,024 units, a constant and not a
  degree; the maintainer then chose no timeout on 2026-10-09. A pattern read on Python's grammar that
  never ends hangs over such a text as it does in Python, where 0.7.0 gave up at a second; one read as
  .NET writes it, which Python refuses, and any text without a surrogate keep 0.7.0's second. A translation that fails, its thread unable to
  start, reads that text with the plain spelling and is tried again at the next.
- **A translation short of stack starts again on a thread of 16 MB**, so the result does not depend
  on the thread that asks.
- **`(?i)` is folded here.** A cased literal becomes the class of the code points `_sre` matches it
  with — those whose simple lowercase is its own, or one CPython's `_casefix` table pairs with it —
  and a class holds what each code point it names folds with, as `_sre`'s charset does. The table is
  built from the runtime's simple lowercase at the first `(?i)` pattern, over the BMP and plane 1, the
  one supplementary plane with cased letters, with `U+0130` lowered to `i` as Unicode and Python do,
  where the invariant culture leaves it, and without the cases Unicode added after 15.0 — Garay, Beria
  Erfe, Latin and Cyrillic letters — which Python 3.12's tables lack: over every code point it then
  equals `_sre.unicode_tolower` with `_casefix`, where .NET 10's tables folded 110 code points more. A back-reference keeps .NET's own folding, which leaves a
  supplementary letter's cases apart, and `İ` apart from `i`: the one gap, three cases in 6,600.
- **`\B` refuses the empty string**, as `_sre` does.
- **After an empty match, a non-empty one at the same place is tried first**, as Python 3.7 does, by
  the pattern anchored there and forbidden to end there; .NET moved on a unit, so `|a` over `a` gave
  `''` alone where scikit-learn gives `''` and `a`. This predates the branch, found by its review, and
  only a pattern that can match the empty string, its least width 0, leaves 0.7.0's scan.
- **`GetFeatureNames` hands out a read-only view**, as `Idf` does since #1627.

## Measured against the release

BenchmarkDotNet 0.14, .NET 10.0.401, AMD Ryzen 7 8700G, Linux x64; a scratch benchmark fitting one
document of words with an emoji every eleventh, through a new vectorizer each time.

| pattern | size | 0.7.0 | main before (41da8898) | this branch |
| --- | --- | --- | --- | --- |
| `[a-z]+` | 200 | 0.96 ms, 12.6 KB | 1.19 ms, 20.1 KB | 0.005 ms, 11.2 KB |
| `[a-z]+` | 2 MB | 14.5 ms | 14.9 ms | 14.6 ms |
| `[A-Za-z]\w+` | 200 | 0.95 ms, 20.5 KB | 9.7 ms, 222.5 KB | 0.043 ms, 100.1 KB |
| `[A-Za-z]\w+` | 40 KB | 2.1 ms | 11.2 ms | 11.6 ms |
| `[A-Za-z]\w+` | 2 MB | 18.1 ms | 31.8 ms | 29.6 ms |
| `\S+` | 40 KB | 2.0 ms | 3.5 ms | 2.2 ms |
| `\S+` | 2 MB | 18.6 ms | 55.7 ms | 18.3 ms |
| `\w+\|.` | 200 | 0.79 ms, 21.7 KB | 10.1 ms, 227 KB | 0.057 ms, 104.1 KB |
| `\w+\|.` | 2 MB | 27.5 ms | 47.4 ms | 38.4 ms |

What stays above the release is `\w` past 256 units, which compiles its 5,000-character spelling once
— 10 ms of a 40 KB document's 11.6 — and over a large text, the compilation `\S+` pays the same
way, 2.2 ms against 2.0 over 40 KB, and the bytes a short text allocates: 0.7.0's
`\w` took no supplementary letter, where Python's takes a 4,900-character set of them. The maintainer
accepted both on 2026-10-08, preferring time to allocation. A construction under `(?i)` takes 9.6 µs and 8.2 KB for `(?i)[a-z]+`,
against 0.7.0's 14.1 µs and 11.1 KB; the first `(?i)` pattern builds the case table once, under
30 ms with its compilation by the JIT, and keeps 77 KB.

## Parity with Python

The differential of #1664, now 110 patterns over 60 documents each, `(?i)` ones and cased letters
added — 6,600 cases: every mismatch is one of the four patterns .NET refuses as written, and three
cases of a back-reference under `(?i)` between a supplementary letter's cases.

## Rejected

- **Atomic groups for each item.** A failing `\w+!` over 20,000 supplementary letters ran in 5.7 s
  rather than 10.8, but .NET's compiled engine mis-read a lazy bounded repeat of an atomic group:
  `\w(?:\S{1,3}?x)?` over a 420-unit text gave `ab` and `lk` where Python gives single letters.

- **Keeping the guard in the pattern and making every item atomic.** On large text it ran at the
  release's time, but a loop that fails at the end of a long text still took 559 ms of a 1 s timeout
  at 20,000 units, and more past them.
- **Compiling the pair-aware spelling at once, or interpreting it always.** The first cost 10 ms on
  every short document with `\w`; the second ran a 2 MB text two and a half times slower.
- **Spelling `\w`'s supplementary set by its complement.** It is 4,600 characters against 4,900, and
  allocates the same 46 KB to parse.
