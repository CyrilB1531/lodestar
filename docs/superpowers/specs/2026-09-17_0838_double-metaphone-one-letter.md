# 0838 — Double Metaphone on the one-letter word "W"

**Status:** accepted, 2026-09-17. Written after the fix it records.

Issue: [#838](https://github.com/CyrilB1531/lodestar/issues/838), found by a performance review of `main`.

## Change

`EncodeW`'s "Arnow"/"Arnoff" rule reads the letter before a final W. On the one-letter word
"W" there is none, and the read was `w[-1]`. The rule now requires `current > 0`, so "W" falls
through to no code and returns `("", "")`, as `doublemetaphone` 1.2 does.

## Measured

Every uppercase word of one, two and three letters (18,278) was encoded by both
`doublemetaphone` 1.2 and `DoubleMetaphone.Encode`. Before the fix "W" threw and every other
word agreed; after it, all 18,278 agree.

The corpus gains the one-letter words B to Z and the two-letter W words `AW`, `OW`, `WA`, `WH`,
`WR` and `SW`, appended after the existing cases so their ids do not move.

## Rejected

- **Padding the working string on the left as well.** It would make every backward read safe,
  but shifts every index the rules compute, and over every word up to three letters this read
  was the only one to reach before the word.
- **All 18,278 words in the corpus.** The comparison is a one-off check; the corpus keeps the
  cases that pin a rule.
