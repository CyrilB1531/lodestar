# MatchRatingApproach.Compare

Whether two names' codices rate as a match.

<!-- docs-declaration -->

```csharp
public static bool? Compare(string a, string b)
public static bool? Compare(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
```

**Parameters** — `a` and `b` are the two names to compare, under the same rules
[`Codex`](matchratingapproach-codex.md) applies to a single one: any alphabetic character plus a
space is accepted. The `string` overload forwards to the span one.

**Returns** — `bool?`. `true` when the two codices rate as a match, `false` when they do not, and
`null` — not `false` — when either name holds a character `Codex` refuses, or when the codices'
lengths differ by 3 or more UTF-8 bytes, which the algorithm declares too far apart to rate at all.

**Exceptions** — `ArgumentNullException` when `a` or `b` is `null` (the `string` overload only).
A name `Codex` would refuse is answered with `null` here, as `jellyfish` answers `None`:
`Compare("O'Brien", "Obrien")` is `null`.

**Example** — the 1977 description's own pair, and the length gap that makes a rating impossible.

```csharp
using Lodestar.Text.Phonetics;

bool? byrneBoern = MatchRatingApproach.Compare("Byrne", "Boern");   // => True
bool? timTimothy = MatchRatingApproach.Compare("Tim", "Timothy");   // => null
```

**Remarks** — `Compare` recomputes both codices itself; a caller already holding two from
[`Codex`](matchratingapproach-codex.md) still calls `Compare`; there is no way to reach the same
answer from the codices alone, because the minimum rating a comparison must clear is read from a
table keyed by their **combined** length:

| combined codex length | minimum rating (out of 6) |
| --- | --- |
| 4 or fewer | 5 |
| 5 to 7 | 4 |
| 8 to 11 | 3 |
| 12 or more | 2 |

`Byrne`/`Boern` codes to `BYRN`/`BRN`, a combined length of 7 and a minimum rating of 4; cancelling
shared characters from the start and then the end of what is left rates the pair at 5, which
clears it. `Tim`/`Timothy` codes to `TM`/`TMTHY`, 2 and 5 characters apart — a gap of 3, so the
comparison returns `null` before the table is even consulted.

That table, and the length it is keyed by, were measured directly against `jellyfish` 1.2.1 by
bisection rather than assumed from a textbook. The length is counted in **UTF-8 bytes**, as
jellyfish counts it: identical to the character count for ASCII names, and larger for anything
else, so `Compare("日本", "AB")` is `null` — six bytes against two —
[decision 0009](../../../decisions/0009-the-phonetic-encoders-follow-jellyfish-whole.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MatchRatingApproach`](matchratingapproach.md),
[`MatchRatingApproach.Codex`](matchratingapproach-codex.md), [the phonetics index](../phonetics.md).
