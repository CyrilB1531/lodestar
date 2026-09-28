# pr-closes.yml leaves no cancelled run of its required check

**Issues:** [#1317](https://github.com/CyrilB1531/lodestar/issues/1317).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

Pull request #1316 showed fourteen green checks and a disabled merge button. `gh pr create` with labels, a
milestone and an assignee fired several `pull_request` events within a second; `pr-closes.yml`'s
concurrency group, `cancel-in-progress: true`, cancelled all runs but the last. "Pull request closes
only open issues" is a required check of the `main` ruleset, and each workflow run is a check suite
of its own, and the newest runs had been cancelled — two before their job began, so no check
listing showed them, while the UI showed the required check pending and the merge button stayed
disabled. Re-running each cancelled run alone cleared it.

## Decisions

- **No concurrency group.** `cancel-in-progress: false` is not enough: a group holds one pending
  run and cancels it when the next is queued, whatever that flag says. The check reads and takes
  seconds, so every run goes to completion.
- **No event is skipped.** A job skipped by its condition posts its check as a success; since the
  newest run is what the ruleset reads, an unrelated label added by hand would have cleared a
  failure. A first version skipped every label but `no-issue`, and Review A caught exactly that.
- **The body and labels are read as they stand**, through `gh pr view`, rather than from the event,
  so the newest run judges the newest pull request. Without a group, runs finish in any order; a run
  that read the body before an edit and finished after the edit's run can still post a stale
  verdict, a window reading at run time narrows to seconds, and editing again or re-running clears.
  The body and labels come from one `gh pr view`, under `set -euo pipefail`: a failed read fails the
  check rather than handing the script an empty body, which `no-issue` would let through unchecked.

## Rejected

- **`cancel-in-progress: false` alone**, #1317's option (a): it still cancels the pending run.
- **Dropping `labeled` from the triggers, or skipping unrelated labels**: `no-issue` changes the
  verdict, and a skipped run posts a success that would clear a failure.

## Verification

- `tools/tests/test_pr_closes_workflow.py` pins these changes and the required check's name;
  three of its six tests fail against the previous workflow. The pull request that carries this
  change is itself opened with its labels added one by one, as #1317's workaround asks, since the
  workflow that runs on it is the one being replaced.
