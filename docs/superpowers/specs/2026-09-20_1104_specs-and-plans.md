# Keep every spec with a status that says when it was written, and stop committing plans

**Issue:** [#1104](https://github.com/CyrilB1531/lodestar/issues/1104), section 7 of
[#1071](https://github.com/CyrilB1531/lodestar/issues/1071).
**Status:** written with the work, 2026-09-20.
**Date:** 2026-09-20.

## The problem

`docs/superpowers/` holds 254 specs and 118 plans. The issue asks for a decision per file — complete
it, mark it retrospective, or delete it — and for the rule that was followed to be written where the
naming rule already lives.

Two defects, and they are not the same defect.

**A plan outlives its branch.** A plan is an instrument for work that has not started: checkbox
steps, a `Branch:` line, a task order. Once the pull request merges, the branch is gone and the
steps are checkboxes nobody may tick. 55 of the 118 plans still carry unticked boxes, 1,771 of them,
all for issues that closed.

**A spec does not say whether it led the work or caught up with it.** 76 carry no `**Status:**` at
all. The 178 that do use about twenty wordings, and most of them open on a lifecycle word every spec
in the tree would carry alike — `accepted, 2026-09-16. Written before the work.` states the fact a
reader is after, behind the one word that separates nothing from anything.

A third thing was found on the way: [#1103](https://github.com/CyrilB1531/lodestar/issues/1103)
deleted 147 decision records the day before, and its own spec deferred the citations inside
`docs/superpowers/` to this issue. 173 links in 82 specs point at a file that no longer exists.

## What was measured

| quantity | value |
| --- | --- |
| specs | 254 |
| plans | 118 |
| lines under `plans/` | 88,745 |
| lines under `specs/` | 21,272 |
| citations into `specs/` from outside `docs/superpowers/` | 88 |
| citations into `plans/` from outside `docs/superpowers/` | 0 |
| plans with an unticked box | 55, holding 1,771 boxes |
| specs with no `**Status:**` | 76 |
| dangling `docs/decisions/` links, after #1103 | 173, in 82 specs |

The citation counts come from a `git grep -n` for `superpowers/specs/` and `superpowers/plans/` over
tracked files, with `docs/superpowers/` and the vendored `.claude/skills/` excluded. The three lines
that do name a plan outside it are `CLAUDE.md` naming the directory, the vendored `writing-plans`
skill, and one test using a plan path as a synthetic fixture string — none of them a reader being
sent to a plan.

## The decision

**`docs/superpowers/plans/` is deleted, and a plan is never committed again.** It is written beside
the work — the session's scratch directory, or a path `.git/info/exclude` keeps out, the way the
`.superpowers/` workspace already is — and it stays there. The spec is the tracked record.

**Every spec opens its `**Status:**` with one of three clauses, and nothing else:**

| clause | what it says |
| --- | --- |
| `written before the work` | the spec led; the work followed it |
| `written with the work` | spec and work moved together, in one commit |
| `**retrospective**` | the work merged first; the spec caught up |

Anything after the clause is free text — the writing date, an amendment, a note that the work was
never done — and nothing reads it. A retrospective spec stays dated by the work, per `CLAUDE.md`'s
*Workflow*, so the status line is the only place that says the file arrived late and carries the
writing date instead.

Where a spec already declared a timing it was kept and re-worded; the 76 that declared none take
theirs from the commit that added the file — a backfill commit means retrospective, the commit that
did the work means the spec led it. The two readings disagreed on 6 files out of 178, and in each of
those six the spec's own statement won.

**The 173 dangling citations are pointed at the commit that still holds the record**,
`53af23c2`, rather than at a new number: the numbering restarted on 2026-09-20 and `0011` now names
a different record from the one those specs cite. 13 of them named a slug no record ever had and
were matched by number instead.

**No spec is deleted, and none is completed.** Every one of the 254 states a problem and what came
of it; the five that a search for `TODO`, `TBD` or *placeholder* turned up were `0xXX`, `Sxxxx` and
the word used in prose. What was incomplete was the header, not the record.

## Options rejected

- **Keep the plans, closed out.** A status line saying "executed, merged in #N" and the boxes
  resolved. Rejected on 2026-09-20 by the maintainer: it leaves 88,745 lines in the tree that no
  file cites, and a resolved checkbox is a claim nobody re-ran.
- **Keep a plan while its issue is open, and delete it in the pull request that closes it.**
  Rejected: one commit per pull request means the plan would be added and removed in the same
  commit, so it never appears on `main` anyway — the rule would describe a file that cannot exist.
- **Leave the status wordings and only fill the 76 gaps.** Rejected: twenty wordings is not a
  vocabulary, and no guard can read one.
- **De-link the dangling citations to plain text.** Rejected in favour of the pinned commit: #1103's
  spec had already said a `docs/superpowers/` citation should point at the commit that still holds
  the record, and a URL costs the reader nothing.

## Consequences

- **`tools/check_spec_status.py`** reads the opening clause, the file name and the empty `plans/`.
  It joins the pre-commit hook, which goes from nineteen guards to twenty, and the `Lint` job.
- **`CLAUDE.md`'s *Workflow* and a new `CONTRIBUTING.md` section** carry the rule next to the naming
  rule, as the issue asks.
- **`tools/check_sdd_citations.py`'s exemption for `docs/superpowers/`** stays, and its reason moves:
  it covered plans describing the workspace they ran in, and now covers a spec that describes one.
- **A second spec on one issue keeps its letter** — `0122b` and `0134a` are the two, and the guard's
  name pattern allows it rather than renaming them.

## Definition of done

- `docs/superpowers/` holds `specs/` and nothing else.
- Every spec opens its status with one of the three clauses, and `tools/check_spec_status.py` says so.
- No link in a spec points at a file that does not exist.
- `pytest tools/tests` is green, every `tools/check_*.py` is green, the build and the suites are
  green, and `npx markdownlint-cli2` is clean.
