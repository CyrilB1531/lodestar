# The Fuzz scorers score a null string 0, and a null blocking key is refused by name

**Issues:** [#1611](https://github.com/CyrilB1531/lodestar/issues/1611),
[#1612](https://github.com/CyrilB1531/lodestar/issues/1612).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Fuzzy` after #1610, on `main` at `414582b2`, found every `Fuzz` scorer
refusing a null string with `ArgumentNullException`, where rapidfuzz 3.14.6 answers
`fuzz.ratio(None, "a") == 0` — and 0 for the six other scorers, with `None` on either side or
both. #1610 had made `Process` answer `None` as rapidfuzz does; the scorers it wraps still
diverged, undocumented. It also found `Deduplicator.FindClusters` failing on a null blocking key
from `Dictionary.TryGetValue`, naming its parameter `key`, with no Exceptions on the page and no
test of the three argument guards.

## Decisions

- **A null string scores 0 in every scorer**, on both overloads. #891 had added the guards to
  replace a `NullReferenceException`, not to diverge from the reference, so the guards give way to
  rapidfuzz's answer. The parameters are annotated `string?`.
- **The unit is checked first.** An undeclared `TextElement` is refused before a null is scored:
  `Fuzz.Ratio(null, "a", (TextElement)2)` threw `ArgumentNullException` and now throws
  `ArgumentOutOfRangeException`, since a null no longer has a refusal of its own to come first.
- **A null blocking key is refused with `ArgumentException`, naming `blockingKey`**, where
  `ArgumentNullException` came from the dictionary: the argument itself is not null, what it
  returned is. Blocking has no rapidfuzz counterpart;
  treating null as one more block would silently compare records whose key could not be computed.

## Verified

rapidfuzz 3.14.6, run from `/var/tmp`: `ratio`, `partial_ratio`, `token_sort_ratio`,
`token_set_ratio`, `partial_token_sort_ratio`, `partial_token_set_ratio` and `WRatio` all answer
`0` for `(None, "a")`, `("a", None)`, `(None, None)`, `(None, "")` and `("", None)`;
`fuzz.json` freezes the five pairs, replayed on both units, and `FuzzNullScoreTests` pins them on both
overloads.

`DeduplicatorTests` pins the null-key refusal — its parameter, `blockingKey`, and the record it
names — and the three argument guards, which nothing reached before. `blockingKey` is typed
`Func<T, string?>`, so a key read from a nullable property compiles to that refusal.
