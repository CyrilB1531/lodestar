# Process skips a null choice as rapidfuzz skips None

**Issues:** [#1233](https://github.com/CyrilB1531/lodestar/issues/1233).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The systematic review of `main` at `d228321f` before 1.0.0 found `Process.Extract`,
`Process.ExtractOne` and `Process.Cdist` handing a null choice to the scorer, which threw
`ArgumentNullException` naming its own parameter `b`. rapidfuzz 3.14.6, run from `/var/tmp`:
`process.extract("apple", ["apple", None, "apples", ""], limit=None)` answers
`[("apple", 100, 0), ("apples", 90.90…, 2), ("", 0, 3)]`, `process.extractOne("apple", [None])`
answers `None`, and `process.cdist(["apple"], ["apple", None, "apples", ""])` answers
`[[100, 0, 90.90…, 0]]`.

## Decisions

- **Aligned with rapidfuzz, not refused.** `Extract` and `ExtractOne` skip a null choice and count
  its index, so the indices of the others are the ones rapidfuzz reports; `Cdist` leaves its cell at
  `0` under any cutoff and any scorer. The scorer, a caller's included, never sees a null: rapidfuzz
  hands `None` to a Python scorer, but a `Func<string, string, double>` has no `None` to be handed,
  and rapidfuzz's own scorers answer `0` for it.
- **The choices, the query and the queries are annotated `string?`.** The declarations the
  reference pages carry are nullability-oblivious, so the pages keep them.
- **A null query is aligned too.** `process.extract(None, …)` answers `[]` before it reads the limit
  or even the choices, and `process.extractOne(None, …)` answers `None`, so `Extract` and
  `ExtractOne` return an empty list and `null` where they refused, before any other check. In
  `Cdist` a null query scores `0` in every column, as `process.cdist([None], …, scorer=fuzz.WRatio)`
  does; the default `ratio` answers uninitialised memory there (`1.19e+21` on this machine), which
  is no reference to follow.
- **The frozen corpora carry the cases.** `process.json` gains three cases holding `None` among
  their own choices and one with a `None` query, `process_cdist.json` three over `None` choices and
  one over `None` queries, so a rapidfuzz release changing its treatment of `None` fails the oracle
  job.

## Verified

600 random cases — up to eight choices, a third of them null, every limit in `None, 0, 1, 2, 5` and
cutoff in `0, 50, 80, 95` — agree with rapidfuzz on `extract` under `WRatio`, `extractOne` and
`cdist` under `ratio`, indices exactly and scores at `1e-9`. Their queries are never null: a null
query is pinned by the corpora and the unit tests, under `WRatio` for `cdist`.
