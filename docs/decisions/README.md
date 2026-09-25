# Architecture decision records — index

Eight records. Seven state one axis each of the project's trajectory: what it targets, what it may
read, how it is laid out, what it writes rather than delegates, what proves it, which reference
each stemmer follows, and where it knowingly differs from that reference. The eighth amends the
fourth: numpy's legacy generator is written here. One row per file. The status and date
columns are copied from each file's own `**Status:** … · **Date:** …` line — correct the record to
correct this table, not the other way around.

A record states an axis. A tool, a numerical mechanism or a shape the code already shows is not an
axis, and does not earn a record: it belongs in the tool's own documentation, in the XML comment on
the member, in [`../guides/performance.md`](../guides/performance.md) when it is a measurement, or
in [`../equivalence.md`](../equivalence.md) when it is a divergence. That rule is what
[#1103](https://github.com/CyrilB1531/lodestar/issues/1103) applied.

`index.yaml` is the mechanical layer beneath the table. Each record declares `supersedes`, `amends`
and `applies` in its own frontmatter; `tools/regen_adr_index.py` reverses those edges, because an
immutable record cannot name the decision that later amended it, and `tools/check_adr_index_sync.py`
refuses any drift. One edge exists today, `0008` amending `0004`: an amendment to one of these
records is a new record, which is what immutability means here — **a record is never edited, and may only be
deleted**, as `tools/check_adr_immutable.py` enforces. The one route past that is a new numbering
epoch, below: a diff that raises `.numbering-epoch` may rewrite a record in place, and the raised
line is what tells a reader the number now holds a different text.

| # | Title | Status | Date | Relationships |
| --- | --- | --- | --- | --- |
| [`0001`](0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md) | The foundations: target frameworks, comparison unit, persistence and versioning | accepted | 2026-09-20 | — |
| [`0002`](0002-provenance-and-the-allowed-references.md) | Provenance and the allowed references | accepted | 2026-09-20 | — |
| [`0003`](0003-the-package-layout-tiers-boundaries-and-edges.md) | The package layout: tiers, boundaries and edges | accepted | 2026-09-24 | — |
| [`0004`](0004-what-is-written-here-and-what-is-delegated.md) | What is written here and what is delegated | accepted | 2026-09-20 | — |
| [`0005`](0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md) | The proof standard and the oracle each family is frozen from | accepted | 2026-09-20 | — |
| [`0006`](0006-the-stemmers-references.md) | The stemmers' references | accepted | 2026-09-20 | — |
| [`0007`](0007-the-deliberate-divergences.md) | The deliberate divergences | accepted | 2026-09-20 | — |
| [`0008`](0008-numpy-s-legacy-generator-is-replayed.md) | numpy's legacy generator is replayed | accepted | 2026-09-25 | amends `0004` |

## What `accepted` means here

All eight carry `accepted`. None has been rejected or withdrawn — a status this table would
otherwise need a second word for.

## The numbering restarted at 0001

These seven replace the 147 records of the first numbering, written between 2026-08-01 and
2026-09-18 and deleted on 2026-09-20. A number below therefore names a different record from the
one it named before that date: `0003` was *Code provenance and license* in the first numbering and
is *The package layout* in this one. Read an old citation — in `CHANGELOG.md`, in
`docs/superpowers/`, or in a commit message — against the tree it was written in:

```bash
git show 53af23c2:docs/decisions/0072-omega-is-an-input-not-a-seed.md
```

`docs/decisions/.numbering-epoch` carries the epoch this directory numbers in, and `.next-adr`
skips any checkout or ref declaring another one, so a branch opened before the restart cannot push
the next number back into the 0148 range.

## Epoch 3: 0003 rewritten in place

On 2026-09-24 [#1103](https://github.com/CyrilB1531/lodestar/issues/1103) rewrote `0003` under its
own number and file name, and raised the epoch to 3. Its rule for `Lodestar.Abstractions` changed
from *the types packages exchange* to *the public data types the packages declare, and no code*, and
it stopped listing the edges `tools/check_nuspec_dependencies.py`'s `EXPECTED` already holds. The
six other records kept their bytes. A citation of `0003` made before that date reads against the
epoch-2 text:

```bash
git show 9f9406c5:docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md
```

## What each record absorbed

The first numbering's 75 axis records were merged into these seven; its other 72 stated a mechanism
and were deleted, their content moved to the tool, the guide or the equivalence row that carries it.
Each record's own tables name the first-numbering record every case came from.

| # | merges |
| --- | --- |
| `0001` | 0001, 0002, 0011, 0012, 0055 |
| `0002` | 0003, 0010, 0082, 0084, 0099 |
| `0003` | 0016, 0071, 0076, 0081, 0089, 0095, 0096, 0097, 0098, 0100, 0101, 0103, 0111, 0119, 0124, 0138, 0139 |
| `0004` | 0059, 0060, 0068, 0072, 0074, 0104, 0105, 0115, 0116, 0129, 0130, 0131, 0132, 0133, 0134, 0135, 0136, 0137, 0140 |
| `0005` | 0007, 0013, 0014, 0017, 0034, 0036, 0075, 0077, 0127, 0143, 0144, 0146 |
| `0006` | 0008, 0086, 0087, 0090, 0091, 0092, 0094, 0145 |
| `0007` | 0005, 0006, 0023, 0039, 0063, 0070, 0080, 0093, 0117 |
