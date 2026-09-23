# Process.Cdist

Scores every query against every choice, at `rapidfuzz.process.cdist` parity.

<!-- docs-declaration -->

```csharp
public static ScoreMatrix Cdist(IReadOnlyList<string> queries, IReadOnlyList<string> choices, Func<string, string, double> scorer = null, double scoreCutoff = 0)
```

**Parameters** — `queries` are the rows and `choices` the columns. `scorer` returns a similarity
in `[0, 100]`; `null` takes [`Fuzz.Ratio`](fuzz-ratio.md). `scoreCutoff` is the minimum score
reported — a cell below it reads `0` rather than being dropped, and on the default scorer it is
also what lets a pair be rejected on its lengths alone.

**Returns** — a [`ScoreMatrix`](scorematrix.md) of `queries.Count` rows by `choices.Count` columns.

**Exceptions** — `ArgumentNullException` when either list is null. `ArgumentOutOfRangeException`
when the matrix would hold more than `int.MaxValue` scores.

**Example** — three queries against four choices, read by the pair that matters.

```csharp
using Lodestar.Fuzzy;

string[] queries = ["new york", "boston", "atlanta falcons"];
string[] choices = ["new york mets", "boston red sox", "atlanta braves", "brooklyn nets"];

ScoreMatrix scores = Process.Cdist(queries, choices);

int rows = scores.Rows;                              // => 3
int columns = scores.Columns;                        // => 4
double bostonAgainstItsOwn = Math.Round(scores[1, 1], 4);   // => 60

// A cutoff zeroes the cell, and on the default scorer it also skips the scan: "boston" cannot
// reach 90 against a choice more than twice its length, whatever the letters are.
ScoreMatrix strict = Process.Cdist(queries, choices, scoreCutoff: 90);
double bostonUnderCutoff = strict[1, 1];             // => 0
```

**Remarks — the default scorer is [`Fuzz.Ratio`](fuzz-ratio.md), and
[`Process.Extract`](process-extract.md)'s is [`Fuzz.WRatio`](fuzz-wratio.md).** That
inconsistency is deliberate: `rapidfuzz.process.cdist` defaults to `fuzz.ratio` where
`rapidfuzz.process.extract` defaults to `WRatio`, and a caller porting a `cdist` call is the
caller this member exists for. **Pass the scorer explicitly** if you want the two members of this
package to agree, and read the
[Python equivalence table](../../../equivalence.md) for the other two spellings that differ.

**The cutoff zeroes rather than filters**, which is again the reference's meaning and the opposite
of [`Extract`](process-extract.md)'s. A matrix has a cell for every pair whatever it scores, so
there is nothing to drop; what a cutoff can do is refuse to report a score, and `0` is how both
sides say that.

**A cutoff buys time on the default scorer.** An Indel edit moves one character, so the distance
between two strings is at least the difference in their lengths, and a pair's ratio cannot exceed
`100 × (1 − |la − lb| / (la + lb))`. Where that ceiling misses the cutoff the cell reads `0`
without being scanned — and the ceiling is exact rather than merely an upper bound, so a pair
scoring the cutoff to the last bit survives it. `"cat"` against `"the cat sat on the mat"` has a
ceiling of 24 and a ratio of 24.

**Pass a scorer of your own and every pair is scored**, because that bound holds for no other
member of [`Fuzz`](fuzz.md): the same pair is 100 under [`PartialRatio`](fuzz-partialratio.md)
and under [`TokenSetRatio`](fuzz-tokensetratio.md), and 90 under [`WRatio`](fuzz-wratio.md), all
of which ignore length by construction. Only the delegate this member defaults to is recognised,
and a lambda that merely calls [`Ratio`](fuzz-ratio.md) is not it — the gate errs towards scoring
the pair, since a missed rejection is slower and a wrong one is wrong.

Measured on a 500 × 500 matrix of phrases one to six words long, Ryzen 7 8700G: a cutoff of 90
takes this call from 8.335 ms to **3.416 ms**, where `rapidfuzz` reads 1.096 ms and 1.098 ms —
the same number with the cutoff and without. The deficit below halves, from 0.13× to 0.32×.

**It is not the reference's speed on the cheap scorers, and the shape of the gap is worth
knowing.** `cdist` builds each query's bit-parallel equality table once and scans every choice
against it; [`Fuzz.Ratio`](fuzz-ratio.md) takes two strings and rebuilds that table per pair.
Measured over a 200 × 200 matrix on a Ryzen 7 8700G with no cutoff: with `Ratio` this is
**0.15×** the reference (1.575 ms against 0.229), and with [`WRatio`](fuzz-wratio.md) it is
**1.02×** — level. The deficit
is the per-pair fixed cost, which an expensive scorer amortises away and a cheap one does not.
[Issue #1130](https://github.com/CyrilB1531/lodestar/issues/1130) carries the reusable pattern
that would close it, and the release order it needs.

**Against .NET, there is nothing to lose to**: the maintained incumbent publishes no matrix call,
and calling its `ExtractAll` once per query is 2.3× slower than this at 200 × 200, allocating 15×
as much.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ScoreMatrix`](scorematrix.md), [`Process.Extract`](process-extract.md),
[`Fuzz`](fuzz.md), the [matching index](../matching.md), the
[Python equivalence table](../../../equivalence.md).
