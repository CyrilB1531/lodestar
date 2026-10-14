# What the nightly cut goes last, and its gates see the commands again

**Issues:** [#1694](https://github.com/CyrilB1531/lodestar/issues/1694),
[#1695](https://github.com/CyrilB1531/lodestar/issues/1695).
**Status:** written with the work, 2026-10-13; the cut rule simplified on 2026-10-14.
**Date:** 2026-10-13.

## The problem

Both were found by the Review B after #1689, which put the nightly on the job's clock.

- **#1694.** #1687 failed a class or comparison cut on two consecutive nights, and put a cut name
  first the next night. It read the previous cut from `docs/guides/nightly_run.md` in the checkout.
  That page only advances when the nightly's pull request merges, and #1110 merged the morning after
  it opened, so a night could read a page two nights old. A class cut, then completed the next
  night, was then failed on its next cut.
- **#1695.** #1689 moved the nightly's `dotnet run` commands out of the workflow into
  `tools/run_bounded.py` and `tools/run_comparisons.py`, and three guards were lost with them:
  - `tools/check_bench_map.py`'s #478 gate scans the workflows for them, and no longer found any;
  - nothing asserted that a benchmark class can be matched by `--filter "*.Name.*"`;
  - nothing required a harness to carry the keys `run_comparisons` reads.

  A comparison step that raised also cost the page, because the steps after it did not run.

## Decisions

- **A cut name goes last, and nothing fails for being cut.** The cut-again rule only existed to
  stop a name that never fits the window from being cut first every night ahead of the rest.
  Ordering does the same with no state to keep fresh. `select_benchmarks.with_owed` asks for the
  owed first, then the newly selected, then what was cut, so a name that does not fit takes only
  what the rest leave, and the page lists it under "Asked for last" every night it is. A name cut
  before and still owed, because the deadline came before its turn, stays in the cut list until it
  runs; otherwise it would go first every other night.
  - A stale cut marker now only reorders a night, and a stale owed list or baseline only costs
    minutes, as before #1689. Reading the previous night's page from its run's artifact, tried first
    and reviewed eleven times, kept finding a case where staleness still produced a wrong failure.
    Cyril chose to remove the rule over patching it (2026-10-14).
  - All three markers go through one parser, `render_nightly.py --markers`, and reach the scripts
    as `--name=value`, so a value starting with `-` is not read as an option.
- **The gates judge the commands where they now live, by conventions the code already follows.**
  - `nightly_command_findings` builds each command through `run_bounded.command_for` and
    `run_comparisons.plan`, and judges its first argument as the workflow scan judged the
    workflow's. `Lodestar.Text.Benchmarks` must forward `--filter`, and must dispatch each harness's
    subcommand as a comparison (`case "compare…":`, an arm starting its line, so one in a comment
    does not count). An option there would reach BenchmarkDotNet, which lists or runs and exits 0.
  - Every other project's `Program.cs` is held to the one statement all three are,
    `BenchmarkSwitcher.FromAssembly(...).Run(args);`, using directives and whole-line comments
    aside. Any other shape is a finding, so widening the rule is reviewed along with the program
    that needs it. Whole-line comments are dropped from `Lodestar.Text.Benchmarks`' too, so neither
    a `case` arm nor its option check is found in one.
  - A file carrying a `[Benchmark]` holds one benchmark class: the one declared last before its
    first `[Benchmark]` attribute starting a line (not `[BenchmarkCategory]`, nor a mention in a
    comment), which is all 114 the map names. `run_bounded.declarations` is that one
    enumeration, read by the runner to find a class's project and by the gate's coverage and shape
    rules, so all three count the same classes; a helper declared beside one is not one, and mapping
    it is a finding.
  - A mapped class must be declared a plain `public class`, neither sealed, abstract, static nor
    generic, since BenchmarkDotNet runs no other, in a file with a file-scoped namespace, a
    byte-order mark aside. Every benchmark file has one; proving that a block namespace encloses a
    class would need a C# parser. A declaration is one rule, `run_bounded.DECLARATION`, shared by
    the gate and the runner: it starts its line, so a mention in a comment is not one. A partial
    lexer of C# comments and literals, tried first, misread a new literal form on every review.
  - A harness must be an object. Its `python`, `subcommand` and `compare` must be strings. Its
    `sources`, which the selection reads on every harness, and its `generate`, if present, must be
    lists of non-empty relative paths. A `python` or `generate` script must be a file. A malformed
    harness, or a diagnostic without a `subcommand` string or with a `python` that is not a relative
    path to a file, is a finding rather than a traceback. Every other rule judges the well-formed
    ones alone, while a malformed harness still counts as naming its scripts.
  - `run_bounded.PROJECTS` is the one list of projects a class may live in. The gate scans it, and
    `run_bounded` finds a class's project there, by a declaration starting its line, so "Run them"
    no longer keeps its own list and `grep`, nor the gate a rule holding the two together.
- **A measuring step that raises costs neither the page nor the comparisons.**
  - "Run them" and "Compare against Python" are `continue-on-error`. Every later step keeps the
    ordinary rule, so a failed render, wiki clone or series comparison stops what depends on it from
    publishing. The last step turns the run red when either measuring step failed or anything
    failed, and names every failure either way.
  - What raises fails that name alone. The class loop fails a class on `run_bounded.py`'s nonzero
    exit, unless it had already settled the class. `run_comparisons` fails a comparison on any
    exception, a shared script that raises is marked broken, and each comparison is recorded as it
    settles.
  - Only a step dying, for example the runner killing it, leaves a name unsettled. That name is
    owed, since the baseline advances past it (#650), and asked for last, so one that kills its step
    every night cannot starve the rest. `nightly_outcomes.carry` holds this and the cut rule's
    continuity, and is unit-tested; the page step only calls it.
  - The pull request's "Classes re-run" and "Comparisons run" come from the page step, not from the
    last line of a measuring step that a raise skips.
- **Carried from the Review B after #1689:**
  - the #1687 spec named `run_comparisons.Outcomes.settle` for `nightly_outcomes`;
  - `tools/README.md` did not describe `select_benchmarks`' `--owed` and `--cut`, nor
    `render_nightly`'s new flags;
  - the page's "Classes re-run" note did not mention owed classes;
  - the cut test's `pgrep "sleep 30"` could match another process's sleep.
