# IndelPattern

One pattern's equality table, built once and scanned against many texts — the bulk form of
[`Indel`](indel.md), at the same answers.

[`Indel`](indel.md) takes two strings, so it rebuilds the pattern's bit-parallel table on every
call. That is the right shape for a pairwise comparison and the wrong one for a caller holding one
query and a list of candidates, which pays it once per candidate. This type pays it once.

The answers are `Indel`'s, to the last bit: the same corpus that proves the pairwise path against
rapidfuzz is replayed through this one, in both orientations. What changes is only what gets
rebuilt.

## When it is worth it

Measured over 64 texts on a Ryzen 7 8700G, against the pairwise loop it replaces, with the part
that differs 4, 16 and 32 units long:

| the texts | 4 | 16 | 32 |
| --- | ---: | ---: | ---: |
| unrelated to the pattern | **0.46** | **0.46** | **0.43** |
| sharing 12 units at each end | **0.86** | **0.71** | **0.59** |
| sharing a 45-unit suffix | **0.92** | **0.78** | **0.96** |
| sharing 28 units at each end | 1.08 | 1.13 | 1.03 |
| sharing 45 units at each end | 1.18 | 1.08 | 1.04 |
| a pattern past 128 units | 1.00 | 0.96 | 0.95 |

The rows above 1 are the ones to understand. The table spans the whole pattern, so this cannot
drop a long common prefix and suffix the way [`Indel`](indel.md) does, and on texts that share a
long run at both ends the pairwise call — which trims — has almost nothing left to scan. Where the
pattern needs two machine words, sixteen units compared at either end send those pairs back to it;
without that guard the 45-unit row reads 1.56 to 2.14. Over one word the guard is not taken,
because scanning the affix there costs about what trimming it saves.

**So this is at worst 1.18× the pairwise call, and a clear win only where the texts share little
with the pattern.** One pattern against one text is [`Indel`](indel.md)'s job, and so is a list
of near-duplicates of the pattern.

## What it does not do

It compares **UTF-16 units** and takes no `TextElement`. The code-point path establishes its
alphabet from the *pair*, so a pattern-anchored table there is a different construction that has
not been measured; a code-point workload keeps [`Indel`](indel.md).

The fast path covers a Latin-1 pattern of up to 128 units, which is one or two machine words. A
longer pattern, or one that leaves Latin-1, falls through to the pairwise call per text — correct,
and no slower than never having built the handle.

## Members

| Member | What it does |
| --- | --- |
| [`IndelPattern.For`](indelpattern-for.md) | Builds a handle over a pattern, renting its table. |
| [`IndelPattern.Distance`](indelpattern-distance.md) | The Indel distance between the pattern and a text. |
| [`IndelPattern.NormalizedDistance`](indelpattern-normalizeddistance.md) | The distance scaled into `[0, 1]`. |
| [`IndelPattern.NormalizedSimilarity`](indelpattern-normalizedsimilarity.md) | `1 - NormalizedDistance`; ×100 it is `fuzz.ratio`. |
| [`IndelPattern.Dispose`](indelpattern-dispose.md) | Returns the table to the pool. |
