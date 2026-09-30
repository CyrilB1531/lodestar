# The MathNet findings of the Review B after #1562

**Issues:** [#1570](https://github.com/CyrilB1531/lodestar/issues/1570) to
[#1574](https://github.com/CyrilB1531/lodestar/issues/1574).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Extensions.MathNet` after #1562, on `main` at `30029d08`, found the
unreleased changelog still saying `ToSparseMatrix` leaves sorting to Math.NET's compressed-row
factory, which #1548 stopped using; `CheckStructure` testing the first and last row pointer together
before the decrease loop, so pointers `[0, 3, 0]` on one stored value reported the ends where the
`CsrMatrix` constructor reports the decrease; the decreasing-pointers test pinning only the
exception type; and two branches no test reached — the rows of a tall diagonal matrix past its
diagonal, and a caller's storage reporting more values than one array holds.

## Decisions

- **The #1402 citations for the copy go.** What #1402 described no longer ships in 0.2.0: its
  changelog entry is removed, and `docs/equivalence.md` credits the as-is copy of ordered rows to
  #1548 alone, whose entry under Fixed says what ships.
- **`CheckStructure` takes the constructor's order and its three checks**: the first pointer, then
  each decrease, then the last pointer, each with its own sentence under `matrix`, a decrease
  naming the pointer as the constructor does.
- **The decreasing-pointers test names `matrix`**, as the other #1549 tests do.
- **A caller's storage may report `Array.MaxLength` values.** Review A found the count refused at
  `0x7FFFFFC7`, which one array holds; it now accepts that many and refuses from `0x7FFFFFC8`, as
  `TableLength.Of` does.
- **The untested branches get tests.** A 5 × 3 diagonal matrix holding `NaN` and `-0.0` converts as
  its dense copy does, `NaN` stored and `-0.0` dropped; a storage yielding `0x7FFFFFC8` copies of
  one entry lazily is refused without allocating, in about eight seconds per target framework.
