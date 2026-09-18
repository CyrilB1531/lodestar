# The normalizer's whole-string decomposition, and what #1085 claimed for it — design

**Issues:** [#1087](https://github.com/CyrilB1531/lodestar/issues/1087) (major),
[#1088](https://github.com/CyrilB1531/lodestar/issues/1088) (major),
[#1089](https://github.com/CyrilB1531/lodestar/issues/1089),
[#1090](https://github.com/CyrilB1531/lodestar/issues/1090),
[#1091](https://github.com/CyrilB1531/lodestar/issues/1091) — every finding of the code review of
[#1085](https://github.com/CyrilB1531/lodestar/pull/1085), which closed #1048.
**Status:** written before the work, 2026-09-18.
**Package:** `Lodestar.Embeddings`. `docs/guides/performance.md` is edited because it is where the
evidence #1088 disputes lives.

## What #1085 did, and the one input it did not survive

`#992` had `StripAccents` scan every code point for `OtherNotAssigned` before decomposing, so the
text could be cut around the ones `string.Normalize` refuses. `#1085` removed that scan: it
normalizes the whole string and reaches the segmented walk only by catching the refusal.

The two are not equivalent. `Segmented` cuts at what **`CharUnicodeInfo`** calls `OtherNotAssigned`;
the whole-string form is **ICU**'s, whose tables are newer. A code point .NET calls `Cn` and ICU
knows as a combining mark with non-zero canonical combining class is reordered by the one and left
alone by the other, and the accent strip that follows does not remove it, because it is not a
`NonSpacingMark` to `CharUnicodeInfo` either. So it reaches the tokens, moved.

Measured on this .NET 10 runtime — `U+1ACF` and `U+1ADD` are both `OtherNotAssigned` to
`CharUnicodeInfo`:

| | `a᫏᫝b` |
| --- | --- |
| whole-string `FormD` | `0061 1ADD 1ACF 0062` |
| `Segmented` | `0061 1ACF 1ADD 0062` |
| `tokenizers` 0.23.2 `BertNormalizer(lowercase=True)` | `0061 1ACF 1ADD 0062` |

**The reference keeps the order**, and Python's `unicodedata` at UCD 15.0.0 agrees with it: both
code points are `Cn` with combining class 0 there. So this is not a judgement call between two
defensible readings — the post-#1085 output diverges from the reference, on an input the corpus
does not hold. In the `U+1AB0..U+1AFF` block alone, 74 of its 2,401 `Cn` pairs move under a
whole-string `FormD`; the review's sweep over all 1,114,112 code points found 34 such code points
and 149 diverging ordered pairs in the final output.

[ADR 0146](../../decisions/0146-berts-normalizer-keeps-the-unassigned-code-points.md) — *BERT's
normalizer keeps the unassigned code points* — is what the fix restores, not what it changes. It is
not amended.

## The fix: the scan comes back, where it is already paid for

`Normalize`'s own loop already asks `CharUnicodeInfo` for the category of nearly every code point,
inside `IsControl`. Hoisting that one lookup out of `IsControl` lets the loop answer a second
question for free — *did this text hold an unassigned code point* — and the answer travels to
`Decompose`, which takes `Segmented` when it is true and the whole-string form when it is not.

The flag is computed on the input and used on the cleaned text, which is sound: the loop drops
only `Cc`, `Cf`, `Cs`, `Co`, NUL and `U+FFFD`, pads with spaces and maps whitespace to a space.
None of those adds or removes a `Cn` code point.

This is not #992's scan returning: that one was a second full pass, over the cleaned text, paying
for itself on every call including the ASCII ones. This is a `bool` set inside a walk that was
already happening, and only `\t`, `\n` and `\r` newly pay a category lookup they short-circuited
past. The six benchmark rows say what that costs.

The `try`/`catch` stays. Which code points a runtime refuses is the runtime's own answer, and
under NLS it is the OS's tables rather than .NET's — a code point Windows refuses and
`CharUnicodeInfo` calls assigned would otherwise throw out of `Encode`. What changes is that it is
no longer the *mechanism*: on the netstandard2.0 asset, where NLS refuses every unassigned code
point, such a text now takes `Segmented` directly instead of paying a failed whole-string
normalize and a throw first (#1090).

## The claims that were carried too far

- **`docs/guides/performance.md:4966`** says the sweep covered "each pair of unassigned code points
  side by side". `7,375,797 = 819,533 × 9`: it is every unassigned code point between nine *fixed*
  neighbour pairs, and no pair of unassigned ones. That pair is the diverging case, so the
  paragraph's conclusion — output unchanged — was not established by the run it cites. The same
  sentence reached the `CHANGELOG` entry. Both are rewritten around a sweep that does include the
  case, run on the fix (#1088).
- **`BertNormalizerTests:11`** says "Both mirrors run this, which is what covers the two paths".
  The mirror project targets `net10.0` and pins only its *project reference* to `netstandard2.0`,
  so both runs meet ICU; the NLS path is never executed by CI. After the fix the segmented walk is
  no longer NLS-only — any text with an unassigned code point takes it on .NET 10 — so the multi-cut
  loop becomes reachable from an ordinary test, and the comment is rewritten to say what the mirror
  actually proves (#1089).
- **`BertBasicTokenization.cs:48`** says the input is handed back unchanged for text that is never
  dropped, padded or mapped, "which ASCII text never is". Tab, newline and return are ASCII and are
  mapped; the other C0 controls are ASCII and are dropped. `"tab\there"` is in the corpus and takes
  the mapped path. The claim is narrowed to letters, digits and punctuation (#1091).

## How it is proven

- **Two oracle cases**, `a᫏᫝b` and `Á᫏᫝á`, with the vocabulary pieces
  `a᫏᫝b` and `a᫏᫝a` appended so the corpus can tell the two orderings apart —
  without a matching piece both orderings are `[UNK]` and the case would prove nothing. The second
  carries the accent strip and the unassigned pair together. Cased and uncased, from
  `tokenizers` 0.23.2, as every other `vocab_txt.json` case.
- **A sweep that includes the case the old one skipped**, run on the fix and published in
  `docs/guides/performance.md` with its real count: every unassigned code point between the nine
  fixed neighbour pairs, plus every ordered pair drawn from the set of code points .NET calls `Cn`
  and ICU gives a non-zero combining class, plus every unassigned code point against each member of
  that set in both orders.
- **The six benchmark rows re-measured** A/B against `main` on the named machine, holding the
  repository's lock, because the fix puts a lookup back into the hot loop (#1090 is a performance
  finding and this is the number that answers it).

## The rejected option

**Revert to #992's unconditional scan.** It restores parity as surely, and it gives back the 1.09×
to 1.17× #1085 measured — a second full pass over the cleaned text on every call, ASCII included,
to answer a question the first pass already had the data for. Rejected: the defect is that the
scan was removed rather than moved.
