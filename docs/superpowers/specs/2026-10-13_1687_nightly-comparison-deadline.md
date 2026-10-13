# The nightly's comparisons stop on the job's clock and carry what they miss

**Issues:** [#1687](https://github.com/CyrilB1531/lodestar/issues/1687).
**Status:** written with the work, 2026-10-13.
**Date:** 2026-10-13.

## The problem

Every scheduled run of `bench-nightly.yml` from 2026-09-26 to 2026-10-12 ended cancelled at the
job's `timeout-minutes: 240`; the last success was 2026-09-25. On 2026-10-12 "Run them" stopped
itself at its `BUDGET_MINUTES: 150` as designed (#650), carrying 49 classes, and "Compare against
Python" then ran 21 comparisons with no deadline of its own, 2 to 17 minutes each and about 110 in
all, against the 58 its comment budgeted for seven, until the runner killed the job in
`survival-family`. A killed job writes no page, so the baseline marker stayed at 2026-09-25 and
every night re-selected every comparison: the spiral #650 closed for classes, reopened by the
comparisons.

## Decisions

- **One clock, the job's, and each phase cut before the next one's last start.** The first step
  notes the time. Classes start until `BUDGET_MINUTES: 150` and one still running at
  `CLASS_STOP_BY_MINUTES: 165` is cut; comparisons start until `COMPARISON_START_BY_MINUTES: 210`
  and one still running at `STOP_BY_MINUTES: 228` is cut. Each cut kills the whole process group
  through `tools/run_bounded.py` or `run_comparisons.py`, since uutils' `timeout` kills the direct
  child alone, and owes what it cut. The class cut is the comparisons' guaranteed share — on
  2026-10-12 the classes spent their whole budget — and the last leaves the steps after it twelve
  minutes, where 2026-09-25's run spent about one on all of them (the pull request is the separate
  `publish` job). Five comparisons had never finished in a logged run, so only a cut bounds them.
- **The class and no other.** A class runs under `--filter "*.Name.*"`: `"*Name*"` also ran every
  class whose name contains it, `MetricsBenchmarks` three more on its budget.
- **A failure is reported and the run goes on.** A class or comparison that exits with an error, a
  class the map names and no project declares, and one cut on two nights running are listed under
  "Failed tonight" and annotated; none is owed, which would put it first every night ahead of what
  works, and none stops the step, which would cost the page as a timeout did. A shared Python side
  that fails fails its sharers without running again. The measure job's last step, after the page
  and its hand-off, turns the run red when anything failed, and the pull request opens anyway: a
  green run would hide a failure once the next page dropped it.
- **A cut is not a skip.** What a night cut while running goes in the page's `nightly-cut` marker as
  well as the owed one and under its own heading, and the next night selects it ahead of the rest of
  the owed; only a second
  cut fails it. One owed because the deadline passed before it started never had the window, so a
  first cut owes it again. The class loop and the comparisons settle each name through one rule,
  `run_comparisons.Outcomes.settle`, and tell a cut by the clock rather than by an exit code a
  command may return of its own. Every selection path, `--all` included, puts the owed first.
- **The page's lists reach the scripts through the environment.** The owed and cut lists are read
  from a page file, and a quote in one interpolated into a `run:` script would be shell code.
- **Shared work once a night.** `ols`, `glm`, `var` and `stats` run one Python script that writes
  all four results; it ran four times, about sixteen minutes, and now runs when first needed, as each
  corpus generator does.
- **What is owed goes first.** A comparison or class not reached is appended to `owed.txt`, which
  the page records in its `nightly-owed` marker; both selections are handed that list, and
  `select_benchmarks.with_owed` now returns the owed names ahead of the rest. The class loop walks
  that order, finding each class's project, where it walked project by project and kept the other
  projects' owed classes behind the first one's every night. In the map's fixed order the same 14
  of 21 comparisons would have missed the deadline every busy night.
- **Corpora on demand.** `tools/run_comparisons.py`, which replaces the shell loop, generates a
  comparison's corpus when it is about to run and once when two share it, not up front for ones the
  deadline will skip; its clock and its runner are injected, and the tests run it.
- **The page lists what ran**: the classes and comparisons that finished, and under two headings
  the ones not reached; a night that measured nothing but owes something still writes it, since
  the marker only travels through the page. The publish job's pull request names both.
- **The log reads in order.** The script's prints are flushed (`PYTHONUNBUFFERED`) so each group
  wraps its commands' output; a failing `compare.py` still shows its output; the gfm table goes to
  the page alone; and the vocabularies the workflow already generated are not generated again.
- **Owed is carried on `main` only.** A dispatch on a branch selects from its merge-base each time,
  as before (#379); the README row says so.
