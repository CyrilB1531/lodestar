# 0003 rewritten in place, under a third numbering epoch

**Issue:** [#1103](https://github.com/CyrilB1531/lodestar/issues/1103), reopened 2026-09-24.
**Status:** written with the work, 2026-09-24.
**Date:** 2026-09-24.

## The problem

`0003` drew `Lodestar.Abstractions`' line by *exchange*: the package held "the types packages
exchange", so a public enum or options record no second package happened to name stayed where it
was declared. The maintainer's rule is different — the package holds the public data types and no
code — and an audit on 2026-09-24 over the net10.0 assemblies of `9f9406c5` found 94 types it
covers, across thirteen packages ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142)).

The record also counted itself among an edge's five declarations and carried a table of sixteen
edges while `EXPECTED` held seventeen. An immutable record cannot take the ten edges #1142 adds.

## What was measured

Every placement of the corrected text, run against `tools/check_adr_immutable.py` as it stood:

| placement | what the guard sees | verdict |
| --- | --- | --- |
| new body at 0003, same file | an accepted record edited in place | refused |
| new body at 0003, new file name | a delete and an add more than 50% alike: a rename | refused |
| 0003 deleted, 0008 added | the same rename | refused |
| 0003 kept, 0008 amending it | an untouched record and a new one | passes |
| new epoch, 0001–0007 renumbered | six unchanged files, and 0003 as in the first row | refused |

Only the amending record passes, and the maintainer ruled it out: the record is the axis, and an
axis stated across two records is what #1103 merged away.

Citations of `0003`, by `git grep` over the citation shapes on `9f9406c5`: 150 lines in 83 files
outside `CHANGELOG.md` (13 lines), the specs (16 files) and the oracle corpora. Twelve sit in
records 0002 and 0006 and name epoch 1's `0003`, *Code provenance and license*. Of the 138 naming
today's record, five name `Lodestar.Abstractions` on the same line, and each was read: three cite
`0071`'s move of `CsrMatrix`, which the new text keeps; one — in
`docs/reference/abstractions/sparse/csrmatrix.md` — credited `0003` with an invariant no record, old
or new, states, and that attribution is removed; and `CLAUDE.md`'s package row, which restated the
old rule, now states the new one beside a paragraph on the epoch. The other 133 cite rules the
rewrite keeps word for word — tiers, audience, publishing, edges — and are left as they are.

## The decision

- **0003 keeps its number and its file name**, so every link to it stays valid; its text is
  rewritten with the rule in a new §(e), (d) declaring an edge in four places, and no edge table.
- **The numbering enters epoch 3.** `.numbering-epoch` exists to say a number now names a different
  record; this is the second time that is true, for one record.
- **`tools/check_adr_immutable.py` admits a rewritten or renamed record only in a diff that raises
  the epoch.** Every other diff is judged exactly as before.

## Options rejected

- **Raising nothing and editing the guard to allow "deleted and recreated under one number".** The
  guard would need a signal to tell that from any other rewrite; the epoch line is that signal, and
  it already existed.
- **A full renumbering, as epoch 2 did.** Six records would move for no change in their text, and
  their citations — all still true — with them.
- **Deleting 0003 for a new 0008.** The guard refuses the pair as a rename, and 138 true citations
  would have been rewritten to say the same thing under another number.

## Consequences

- Reading a citation of `0003` made before 2026-09-24 means reading `9f9406c5`'s text;
  `docs/decisions/README.md` and `CLAUDE.md` say so.
- The guard's tests pin the route: a rewrite under an unchanged epoch is refused, a rewrite with a
  raise passes, and a raise alone passes.
