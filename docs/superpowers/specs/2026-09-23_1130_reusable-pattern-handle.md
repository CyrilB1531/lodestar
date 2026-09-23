# The table built once, for a caller with one pattern and many texts

**Issue:** [#1130](https://github.com/CyrilB1531/lodestar/issues/1130).
**Status:** written before the work, 2026-09-23.
**Date:** 2026-09-23.

## The problem

`BitParallelLcs` already separates the two halves of a bit-parallel LCS: `TryFill` builds the
pattern's equality table, and the scan walks the text against it. Every public entry point
rebuilds that table per call, because every public entry point takes two strings. That is the
right shape for a pairwise call and the wrong one for a caller holding one pattern and a list.

## What was measured

One pattern against 64 texts, in-process BenchmarkDotNet 0.14.0 on .NET 10.0.12, AMD Ryzen 7
8700G, Ubuntu 26.04.1, 2026-09-23. Every row is checked to compute the same answers as the
shipped path before it is timed — a row that computes something else is not a comparison, and the
check caught one that did.

| corpus | m | today | handle | ratio |
| --- | ---: | ---: | ---: | ---: |
| unrelated | 16 | 1,922 ns | **697 ns** | **0.36** |
| unrelated | 32 | 3,589 ns | **1,288 ns** | **0.36** |
| 24 shared characters | 16 | 2,599 ns | **1,578 ns** | **0.61** |
| 24 shared characters | 32 | 4,212 ns | **2,148 ns** | **0.51** |
| pattern three times the text | 16 | 3,233 ns | **706 ns** | **0.22** |
| pattern three times the text | 32 | 6,225 ns | **2,513 ns** | **0.40** |
| **120 shared characters** | 16 | **4,110 ns** | **8,921 ns** | **2.17** |
| **120 shared characters** | 32 | **4,102 ns** | **8,971 ns** | **2.19** |

### The lifetime, which the issue left open

| held as | unrelated, m=16 | ratio |
| --- | ---: | ---: |
| a `ref struct` over the caller's `stackalloc` | 697 ns | 0.36 |
| **a rented array, disposed** | **693 ns** | **0.36** |
| a batch call, no handle at all | 748 ns | 0.39 |
| its own array | 798 ns | 0.42 |

**A rented array costs nothing against a `stackalloc` and can outlive the frame**, so the handle
does not have to be a `ref struct` to be free. Owning an array costs 10%, which is the 2 KB the
allocation and its eventual collection are worth over 64 scans.

**A batch call is 5% slower and strictly less useful.** It cannot serve a caller that discovers
its texts as it walks — a BK-tree search, a deduplication pass — so the handle dominates it on
both axes and the batch shape is not written.

### The regression, and the guard that removes it

The handle's table spans the whole pattern, so it cannot take the affix trim: trimming changes
the pattern per pair. In the probe, 120 shared units around a four-unit difference read **2.2×**;
rebuilding the table when the trim bites was worse still, 1.78× to 2.88×. Sixteen units compared
as one vector send such pairs to the pairwise call instead, which trims.

**The probe's figures did not survive the shipped benchmark, and the review said why.** Its first
long-affix corpus built a 136- and 152-unit query, past the table, so the guard never ran on it
([#1137](https://github.com/CyrilB1531/lodestar/issues/1137)); and the guard read the prefix only,
where `Affixes.Trim` strips both ends ([#1138](https://github.com/CyrilB1531/lodestar/issues/1138)).
Three guards were then measured on corpora kept inside the table — 64 texts, length of the middle
4 / 16 / 32, 2026-09-24:

| corpus | no guard | both ends, always | **both ends, two words only** |
| --- | --- | --- | --- |
| unrelated | 0.45 / 0.45 / 0.44 | 0.45 / 0.49 / 0.45 | **0.46 / 0.46 / 0.43** |
| 12 + 12 shared | 0.85 / 0.70 / 0.59 | 0.91 / 0.72 / 0.61 | **0.86 / 0.71 / 0.59** |
| 45 suffix | 0.92 / 0.78 / 1.20 | 1.09 / 1.02 / 0.96 | **0.92 / 0.78 / 0.96** |
| 45 + 45 shared | 2.14 / 1.90 / 1.56 | 1.12 / 1.07 / 1.08 | **1.18 / 1.08 / 1.04** |
| 28 + 28 shared | not run | not run | **1.08 / 1.13 / 1.03** |

**The guard pays only where the pattern spans two words.** There, scanning a long affix costs
twice per unit what one word does, and the pairwise rebuild is cheaper: 1.56–2.14 unguarded, 1.04–
1.18 guarded. Over one word the handle wins even on a shared suffix — 0.78 at 61 units — because
rebuilding 61 units of table per text costs more than scanning the 45 the trim would drop, and a
probe that fires there turns that 0.78 into 1.02. The 28 + 28 corpus is the one that could have
refuted leaving one word unguarded, and at 60 units with a four-unit middle it reads 1.08.

A prefix-only probe was measured on an earlier corpus set and loses the 45-suffix row at 32 units
(1.17); it is not the one shipped.

**The worst case is 1.18×, not parity.** The guarded path pays the probe and a frame to reach a
call that does almost nothing, and on those rows that is most of the time.

## What ships

```csharp
namespace Lodestar.Text.Distances;

public sealed class IndelPattern : IDisposable
{
    public static IndelPattern For(string pattern);
    public static IndelPattern For(ReadOnlySpan<char> pattern);
    public int Distance(ReadOnlySpan<char> text);
    public double NormalizedDistance(ReadOnlySpan<char> text);
    public double NormalizedSimilarity(ReadOnlySpan<char> text);
    public void Dispose();
}
```

The table is rented from `ArrayPool<ulong>` and returned on `Dispose`, and a handle used after
disposal throws rather than reading a table another caller now holds.

**A class, against the issue's sketch of a `readonly struct`.** A struct holding a rented array is
trivially copyable, and two copies disposed is a double `Return` — the pool then hands one array
to two callers, which is silent corruption rather than an exception. The one object header that
buys the idempotent `Dispose` is amortised over every text the handle scans, and the measurement
that ranked the four lifetimes was of the rented table, which a class holds as readily.

**Every pattern is accepted.** The table covers one and two machine words over Latin-1, which is
a pattern of up to 128 units; a longer one, or one that leaves Latin-1, falls through to the
pairwise path per call, so the handle is never wrong and never slower than not having used it.
The band is on the page rather than in the signature.

## What is not written

- **The `Affixes.Trim` char specialisation.** Measured on the same machine, per pair:

  | variant | 24 shared characters | unrelated, 53 | phrases |
  | --- | ---: | ---: | ---: |
  | generic, as it ships | 20.72 ns | **2.41 ns** | **2.25 ns** |
  | `char`-specialised throughout | **10.56 ns** | 5.54 ns | 5.33 ns |
  | scalar prologue, then vectors | 12.98 ns | 4.18 ns | 4.15 ns |
  | vector prefix, scalar suffix | 16.83 ns | 3.09 ns | 3.06 ns |

  Every variant trades a win where the affixes are long for a loss where they are short, and the
  minimal one still costs 28% to 36% on the short inputs that dominate. The generic loop is
  already about five cycles; a vector path has a fixed setup it cannot repay with nothing to trim.
  Worth revisiting **inside the guarded path alone**, where a long shared prefix is established
  before the trim runs — but that is a different change and needs its own measurement.

- **The `Lodestar.Fuzzy` side.** `Process.Cdist` is the caller this exists for, and it cannot
  take it here: `Lodestar.Fuzzy` reaches `Lodestar.Text` through a `PackageReference` on a
  published floor, so a branch whose downstream package needs new upstream API cannot go green
  (`CONTRIBUTING.md`, *Working across two packages*). `Lodestar.Text` releases first.

- **A code-point handle.** Every `Fuzz` scorer and every distance takes a `TextElement`, and this
  takes none: it compares UTF-16 units, the package's default. The code-point path establishes its
  alphabet from the *pair*, so a pattern-anchored table is a different construction and a
  different measurement — and none of the eight rows above is of it. A parameter that silently
  bought nothing would be worse than its absence.

- **A handle over the other metrics.** `Myers` holds Levenshtein's table and measured the
  opposite way on the same sweep that set `BitParallelLcs`'s — its recurrence is a dozen
  operations per text character against this one's four, so the fixed cost is a far smaller share
  of a call. A `LevenshteinPattern` needs its own measurement before it needs an API.

## What the benchmark found that no test could

`Indel.Distance(a, b)` over two `ReadOnlySpan<char>` does **not** bind to the character overload.
C# prefers a candidate whose parameters all have arguments, so the generic `Distance<char>` wins
over `Distance(ReadOnlySpan<char>, ReadOnlySpan<char>, TextElement = Utf16Unit)` and takes the
dynamic program. The handle's fallback was written that way and read **3.19× and 9.33×** on the
long-affix rows; every answer was correct, so the 1,522-pair replay stayed green. The comparison
unit is named explicitly now, with the reason beside it, and `src/` carries no other site where
the same two spans are passed without one.

That is the argument for the agreement check in the benchmark's `GlobalSetup`: a row that computes
the right answer by the wrong route is invisible to a test suite and visible here.

## What shipped, measured

`IndelPatternBenchmarks`, one query against 64 texts, BenchmarkDotNet 0.14.0 on .NET 10.0.12,
AMD Ryzen 7 8700G, Ubuntu 26.04.1, 2026-09-24. Held against pairwise, middle 4 / 16 / 32:

| the texts | held ÷ pairwise | FuzzySharp 6.0.0 ÷ held |
| --- | --- | --- |
| unrelated | 0.46 / 0.46 / 0.43 | 8.93 / 5.87 / 5.44 |
| 12 + 12 shared | 0.86 / 0.71 / 0.59 | 3.42 / 3.22 / 3.56 |
| 45 suffix | 0.92 / 0.78 / 0.96 | 2.23 / 2.44 / 1.95 |
| 28 + 28 shared | 1.08 / 1.13 / 1.03 | 1.82 / 1.78 / 1.89 |
| 45 + 45 shared | 1.18 / 1.08 / 1.04 | 1.41 / 1.63 / 1.66 |
| past the table | 1.00 / 0.96 / 0.95 | 1.53 / 1.63 / 1.74 |

Higher than the probe's 0.36 on unrelated texts because the shipped handle carries the guard, the
interleaved table that serves both widths at a stride of two, and the normalization the probe
inlined. A win where the texts share little, at worst 1.18× where they share a long run at both
ends, and 1.41× to 8.93× against the .NET incumbent over every row.

## Proof

The existing corpora replayed through the handle: `Indel.NormalizedSimilarity(a, b)` and
`IndelPattern.For(a).NormalizedSimilarity(b)` must agree exactly, over every oracle pair the
package already freezes, and over a random corpus that crosses each width band and the guard in
both directions.
