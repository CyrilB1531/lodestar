# What a segment the runtime refuses to normalize becomes — design

**Issue:** [#1094](https://github.com/CyrilB1531/lodestar/issues/1094) (minor), found by the code
review of [#1092](https://github.com/CyrilB1531/lodestar/pull/1092), which closed #1087.
**Status:** written alongside the work, 2026-09-18.
**Package:** `Lodestar.Embeddings`.

## The hole, and a second one beside it

`BertBasicTokenization.Segmented` normalizes each stretch between the code points `CharUnicodeInfo`
calls unassigned, and none of those calls is guarded. Since #1087 it is reached from the
`unassigned` flag, outside `Decompose`'s `try`, as well as from its `catch`.

On .NET 10 neither route can throw: ICU refuses exactly one code point, `U+FFFE`, and
`CharUnicodeInfo` calls it `OtherNotAssigned`, so `Segmented` always cuts it out first. Under
**NLS**, which the `netstandard2.0` asset meets on .NET Framework, Mono and Unity, the refusal set
comes from the OS's tables and is larger. A refused code point `CharUnicodeInfo` calls assigned is
not cut out, its stretch reaches `Normalize`, and the `ArgumentException` leaves `Encode`. The
issue's example, on an NLS older than the astral emoji: `Encode("\U0001F600a\u0378")`.

**The `catch` route had the same hole in a worse form.** A text with no unassigned code point that
NLS refuses goes to `Segmented`, which finds nothing to cut at and normalizes the whole string
again — the same call, the same refusal, now outside the `try`. That is the case the `catch`'s own
comment names (#1050, #1090), so the guard it describes has never held.

## The decision: a refused code point is treated as an unassigned one

A code point the runtime refuses on its own is cut out and passes through unnormalized, exactly as
an unassigned code point does, and the stretches around it are decomposed. Three reasons:

- It is **the reference's answer** for every code point its tables have no mapping for: combining
  class 0 and no decomposition. A code point NLS refuses because the OS's tables are older than
  .NET's is one the reference, whose tables are newer still, usually knows as an ordinary
  character without a decomposition — an emoji, a letter of a recent script.
- It is **the rule already in force** for the unassigned code points, so one text is not split by
  two different rules depending on which table made the call.
- It is **bounded**: only the refused code point is left as it is, not the stretch around it.

A stretch still refused once those are cut — a refusal no single code point explains, which no
runtime is known to produce — passes through whole rather than throw. That is a silent answer, and
it is written down here and in the remarks because nothing else can make it visible.

### Rejected options

- **Let the `ArgumentException` out of `Encode`.** It is a refusal rather than a silent answer, but
  it refuses a whole text for one emoji newer than the operating system, on the frameworks the
  `netstandard2.0` asset exists for, where the same text encodes on .NET 10. A tokenizer that
  throws on valid Unicode is not a usable tokenizer.
- **Pass the whole refused stretch through.** Simpler — one `try` per stretch — but an accented
  word beside the refused code point keeps its accent, and under an uncased model it becomes
  `[UNK]` where the reference strips it.
- **Normalize code point by code point.** Never refuses more than one code point, but loses the
  canonical reordering of a mark sequence, which is what NFD is for.

## How it is proven

No runtime here meets NLS (#1089): the mirror runs the `netstandard2.0` assembly on the .NET 10
runtime. So `Decompose` takes the normalization as a `Func<string, string>` parameter behind a
private overload that passes the runtime's own, and three tests hand it one that refuses what an
NLS older than the astral emoji would:

- a refused code point beside an unassigned one, reached from the flag — the issue's route;
- a refused code point in a text with no unassigned code point, reached from the `catch` — the
  route that re-threw;
- a refusal no single code point explains, which leaves its stretch as it is.

The .NET 10 output does not move, and a differential run says so: 50,000 random texts drawn from
ASCII, accented Latin, the combining blocks, `U+1AB0..U+1AFF`, emoji, CJK, Hangul, Greek, the
controls and any code point at all, through `BertNormalizer(lowercase=True)` of `tokenizers` 0.23.2
and through `Normalize` on both `main` and the branch. The branch and `main` agree on all 50,000;
both differ from the reference on 5,501, every one of which holds a code point that differs from
the reference on its own between two letters — 28 such code points, all within the table skew
`docs/equivalence.md` already records for this route.
