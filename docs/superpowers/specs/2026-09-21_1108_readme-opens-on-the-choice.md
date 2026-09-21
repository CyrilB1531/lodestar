# Open the README on the choice a reader came to make

**Issue:** [#1108](https://github.com/CyrilB1531/lodestar/issues/1108), section 1 of
[#1071](https://github.com/CyrilB1531/lodestar/issues/1071).
**Status:** written with the work, 2026-09-21.
**Date:** 2026-09-21.

## The problem

`README.md` is 359 lines. Someone arriving has one decision to make — this library or the one they
already had in mind — and the page answers it only at line 92, in a seventeen-row table whose cells
run to a paragraph each. What comes before it is a premise, a generic argument for C# over Python,
and a numbered list of seven gaps; what comes after it is an inventory: the five lots of the
original brief, the structure tree, a copy of `CLAUDE.md`'s *Where a fact belongs* table, and the
release procedure.

Three defects, and they are not the same defect.

**The answer is below the fold.** The table that names each incumbent is keyed by *package*, which
is the repository's axis, not the reader's. A reader arriving from FuzzySharp has to know that the
answer is under `Lodestar.Fuzzy`.

**Some of it is stale.** The table was written before
[#1106](https://github.com/CyrilB1531/lodestar/issues/1106) rebuilt the performance guide.
`Lodestar.Preprocessing` reads **Not measured** while the guide now carries three comparisons
against ML.NET for it.

**Some of it is a second copy.** The *Where a fact belongs* table is `CLAUDE.md`'s, and had already
drifted from it: its `docs/decisions/` row predates the axis-only records of
[#1103](https://github.com/CyrilB1531/lodestar/issues/1103), and it has no `.claude/skills/` row.
The *What is delivered* table describes lots the repository grew past long ago, and says so.

## What was decided

The page opens on a table **keyed by the alternative**: what the reader would reach for, what for,
the number that settles it, and the guide to go to. Ten rows, one per incumbent family, each number
read out of `docs/guides/performance.md` as it stands after #1106, so each carries its machine and
window by reference. One paragraph below it says who the library is **not** for, naming the .NET
library to use instead, and links `docs/migration/`. That is the screen.

Below it, in order: getting started, why not call Python, why the gap is where it is (the premise,
and the four gaps that need a sentence of their own), the packages the first table does not carry,
parity with the Python reference, developing, structure, publishing, license.

| what | from | to |
| --- | --- | --- |
| the premise and the seven-item gap list | the top | `Why the gap is where it is`, cut to what the first table does not already say |
| the per-package incumbent table | line 92 | `Measured against the .NET incumbents`, keeping only the rows the first table does not carry, every link checked against an anchor that exists |
| `What is delivered` | the middle | deleted; `CLAUDE.md`'s architecture table is the inventory |
| `Where a fact belongs` | the middle | deleted; `CLAUDE.md` holds the table, and `Structure` links it |
| `Oracle validation` | under `Developing` | `Parity with the Python reference`, naming the references the oracles actually freeze |

Three things stay that a first reading would have moved.

- **The structure tree and the pack loop.** `tools/check_readme_packages.py` and
  `tools/check_readme_pack_loop.py` hold both to `src/` and to the sample; they now sit below the
  reader's part of the page rather than leaving it.
- **`Publishing`.** [#1105](https://github.com/CyrilB1531/lodestar/issues/1105) moved the release
  procedure here from `CONTRIBUTING.md`, and the consumer half — adding the feed — is a reader's.
- **The heading `Measured against the .NET incumbents`**, which the #1106 spec cites.

### Options that lost

**Keep the per-package table and move it to the top.** Rejected: it is keyed by the wrong thing,
and its cells are paragraphs, so it does not fit a screen however it is placed.

**Move the inventory to `CLAUDE.md` wholesale.** Rejected for the tree: `CLAUDE.md`'s architecture
table already is the inventory, with each package's tier, and a second copy there would be the
drift this change removes from the README.

## Result

`README.md` goes from **359 lines to 275**, and the first `##` section — the alternatives table and
who the library is not for — ends at line 33. The one citation of the deleted
`README.md#where-a-fact-belongs`, in the #1105 spec, now points at `CLAUDE.md`'s table. Every
relative link and anchor in the page was checked against the headings that exist.
