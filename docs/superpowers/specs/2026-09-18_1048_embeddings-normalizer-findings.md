# Seven BERT-path findings in Lodestar.Embeddings — design

**Status:** accepted, written before the work.
**Issues:** [#1048](https://github.com/CyrilB1531/lodestar/issues/1048),
[#1049](https://github.com/CyrilB1531/lodestar/issues/1049),
[#1050](https://github.com/CyrilB1531/lodestar/issues/1050),
[#1051](https://github.com/CyrilB1531/lodestar/issues/1051),
[#1052](https://github.com/CyrilB1531/lodestar/issues/1052),
[#1053](https://github.com/CyrilB1531/lodestar/issues/1053),
[#1064](https://github.com/CyrilB1531/lodestar/issues/1064).
**Date:** 2026-09-18.

## Why the seven travel together

All seven are delta review #2's reading of the same two commits —
[#983](https://github.com/CyrilB1531/lodestar/issues/983) "keep unassigned code points" and
[#992](https://github.com/CyrilB1531/lodestar/issues/992) "cap a word by code points, and copy
the BERT text less". Four are the code those commits left: #1048 and #1050 are the two halves of
`BertBasicTokenization.Normalize`'s cost, #1051 the suppression #992's move left behind, #1052 the
scan #992 put in front of every word. Three are the sentences those commits wrote about
themselves: #1049 the ADR whose Decision #983 reversed, #1064 and #1053 the equivalence row #992
added, wrong in its attribution and short by four code points.

`BertBasicTokenization.cs` is 219 lines and #1048, #1050 and #1051 touch three of its methods, two
of them the same one. #1052 is the other half of #992 in `WordPieceTokenizer`. #1053 and #1064 are
the same table row. Splitting them means editing one file four times, one row twice, and measuring
the same normalizer twice.

## What is measured

On `main` at `00774eb1`, AMD Ryzen 7 8700G, Ubuntu 26.04.1, .NET SDK 10.0.401, `tokenizers` 0.23.2.
The two #1048 rows and the #1052 row are the review's own harness, quoted from the issues; every
other row was re-measured here before any of it was believed.

| Finding | Input | This, today | Should be |
| --- | --- | --- | --- |
| #1048 | 1,231 characters, accented, uncased, 3,000 calls | 18.3 µs a call, against 10.5 µs before #992 | no slower than before #992 |
| #1048 | the same, ASCII, uncased | 10.3 µs a call, against 8.7 µs before #992 | as above |
| #1050 | the 819,533 code points .NET 10 calls `OtherNotAssigned`, one at a time through `string.Normalize(FormD)` | the summary says it refuses them; it refused **1**, `U+FFFE`, and accepted 819,532 | the segmented walk is the exception path, not the default |
| #1050 | `U+FFFE` through `BertNormalizer`, `tokenizers` | kept | kept — the walk must stay reachable |
| #1051 | `#pragma warning disable CA1308` around `Normalize` | suppresses a rule `Normalize` cannot trip since #992 moved the `ToLowerInvariant` out | gone |
| #1052 | 50,400,000 words, `CodePointLength` unconditional | 330 ms; `word.Length > cap` first, 22 ms | the UTF-16 length answers first |
| #1053 | `U+1734`, `U+1171E` between two `a`s, uncased | kept here (.NET 10: `SpacingCombiningMark`), stripped by `tokenizers` | the row says so |
| #1053 | `U+166D`, `U+111C9` between two `a`s | one word here (.NET 10: `OtherSymbol`, `NonSpacingMark`), three pre-tokens in `tokenizers` | the row says so |
| #1064 | `docs/equivalence.md:129` | attributes ~600 residual code points to #887 | #887's own sweep measured **0** differences; the residue is #992/#983's |
| #1049 | `docs/decisions/0144`, Decision | "the Cc, Cf, Cs, Co and **Cn** characters … dropped" | #983 keeps Cn; the record must say so |

The `U+FFFE` row is why #1050 is a documentation-and-order fix rather than a deletion: on .NET 10
`string.Normalize` refuses exactly that one code point, `tokenizers` keeps it, and the `netstandard2.0`
assembly on .NET Framework reaches NLS, which refuses every unassigned code point. The walk is
still needed; it is just not needed first.

## Decisions

### 1. Both halves of #1048, so there is no trade to accept

Issue #1048 leaves open "whether −2.5 KB for +29 % time was the intended trade". Neither half has to be
given up. `Same` exists because `Normalize` did not know whether it had changed anything; the loop
that appends does know, so a `bool` replaces the second pass and the allocation #992 saved stays
saved. `Decompose`'s mandatory category scan (#983) goes away with #1050's reordering. The target
is under #992's own before-numbers on every case, with #992's allocations.

### 2. `Decompose`'s segmented walk becomes the `catch` path

Measured above: the premise "`Normalize` refuses unassigned code points" is NLS behaviour, true for
the `netstandard2.0` assembly on .NET Framework and false on .NET 10 except for `U+FFFE`. `try` the
whole-string `Normalize` and walk only when it throws, so both runtimes produce today's output and
only the runtime that actually refuses pays for it. The summary names the runtime instead of
claiming a blanket refusal.

### 3. #1049 earns an amending ADR

The maintainer's rule is that an ADR states the project's trajectory, never documents code. 0144's
Decision is a trajectory statement — *this is the pipeline a `vocab.txt` runs* — and one clause of
it is now false, which is exactly the 0101 → 0103 shape CLAUDE.md warns about. So **ADR 0146 amends
0144**: same decision, one clause corrected, with why parity chose the other answer. The CHANGELOG
entry #983 already has is not a substitute, because nothing in the CHANGELOG is reachable from
0144.

### 4. One pull request, seven `Closes`

Seven issues, one file's worth of code and one table row. The maintainer asked for a single pull
request.

## Design

### `Normalize`'s `changed` flag (#1048)

The loop already distinguishes its three non-identity outcomes: a dropped code point, a whitespace
character that is not already a space, and a padded CJK ideograph. Setting a local `bool` at those
three points answers what `Same` re-derived by walking the builder, and it answers it for the case
`Same` could not short-circuit either — text the normalizer did not change, where `Same` compared
every character before returning `true`. `builder.Length == text.Length` goes with it: the flag is
exact, so the length check adds nothing.

### `Decompose`'s `try` (#1048, #1050)

```csharp
try
{
    return text.Normalize(NormalizationForm.FormD);
}
catch (ArgumentException)
{
    return Segmented(text);
}
```

`Segmented` is today's loop, unchanged, moved into its own method so the `try` block holds one call.
Nothing else in `Normalize` can throw `ArgumentException`, and the input reaching `Decompose` has
already lost its lone surrogates to `IsControl`, so the only two things that reach the `catch` are
`U+FFFE` on .NET 10 and any unassigned code point under NLS.

### The suppression (#1051)

`#pragma warning disable CA1308` and its `restore` around `Normalize` are deleted. `Lowered`'s own
justification stops pointing at `Normalize` and states the reason itself.

### The cap (#1052)

`if (word.Length > _maxCharsPerWord && CodePointLength(word) > _maxCharsPerWord)`. A code point is
one or two UTF-16 units, so `CodePointLength(w) <= w.Length`, and the scan cannot change the answer
when the units already fit. The remark says which of the two the short-circuit protects.

### The row (#1053, #1064)

`docs/equivalence.md:129` loses "the same residue #887 records for the `Whitespace` pre-tokenizer"
— #887's sweep measured zero differences, and rows 105 and 136 still say so — and cites the BERT
path's own sweep instead. It gains the four code points that go the other way: `U+1734` and
`U+1171E` stripped by `tokenizers` and kept here, `U+166D` and `U+111C9` splitting a word there and
not here.

### ADR 0146

`amends: ["0144"]`, `docs/decisions/index.yaml` regenerated by `tools/regen_adr_index.py`, and the
prose table in `docs/decisions/README.md` with it.

## Testing

- **The oracle is the regression guard.** `tests/oracles/vocab_txt.json` already replays tabs,
  ideographic spaces, dropped controls, CJK padding, unassigned code points and the 51-code-point
  astral word, cased and uncased — every branch the `changed` flag and the cap short-circuit
  decide. It gains one case, `a￾a`, which is the only input on this runtime that reaches
  `Decompose`'s `catch`; `tools/generate_oracles.py` grows the text and the corpus is regenerated.
- A unit test asserts `U+FFFE` survives the normalizer with its neighbours' accents stripped, so
  the `catch` path is covered by name and not only by corpus id.
- `bench/Lodestar.Text.Benchmarks/BertNormalizerBenchmarks.cs` measures the `vocab.txt` route over
  ASCII, accented and CJK documents, cased and uncased — the five cases #1048's table has — and is
  run on `main` and on the branch, interleaved, on a named machine.

## Out of scope

- `HasMark` and `StripAccents`' own two passes over decomposed text. They are #992's design, they
  cost only text that holds a mark, and #1048 does not name them.
- Teaching `IsPunctuation` or the accent strip the four code points of #1053. The divergence is the
  Unicode tables' own skew — .NET 10's against `tokenizers`' vendored ones — and hard-coding either
  side's answer would break on the next table either one ships. `docs/equivalence.md` is where it
  is recorded, as it already records the ~600 going the other way.
- `TokenizerJsonLoader` accepting a `BertNormalizer` pipeline, which 0144 already parked.
