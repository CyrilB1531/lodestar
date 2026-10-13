"""Run the nightly's comparisons against Python inside the job's time, carrying what does not fit (#1687).

Each comparison is three commands -- the Python side, the C# side, and `bench/compare.py` --
over a corpus some of them generate first. The nightly ran them in a shell loop with no
deadline: 21 comparisons took about 110 minutes after the classes' 150, every night from
2026-09-26 met the job's `timeout-minutes: 240`, and a killed job writes no page, so the next
night selected them all again.

So, against a clock counted from the job's start:

  * no comparison starts past ``--start-by`` minutes; it is owed, and the next night asks for it
    first (``select_benchmarks.with_owed``);
  * one already running is cut at ``--stop-by`` minutes and owed too, so the page, wiki and pull
    request still get their minutes whatever a comparison's length turns out to be;
  * a comparison's corpus is generated when it is about to run, not up front for ones the
    deadline will skip, and once when two share it.

It reads the selection from ``harnesses.txt`` and writes what ran to ``harnesses-run.txt``, one
name a line, for the page; what did not is appended to ``owed.txt``, which already holds the
classes the run loop did not reach, and what was cut to ``cut.txt`` as well, which the page carries
so the next night can tell a second cut from a first. Every path is fixed here, not passed in.
"""

from __future__ import annotations

import argparse
import json
import pathlib
import subprocess
import sys
import time
from collections.abc import Callable, Sequence

import select_benchmarks
from nightly_outcomes import ENCODING, Outcomes, annotate, run_command

ROOT = pathlib.Path(__file__).resolve().parents[1]
RESULTS = ROOT / "bench" / "results"
SELECTED = pathlib.Path("harnesses.txt")
RAN = pathlib.Path("harnesses-run.txt")
PYTHON = "python"

# (command, seconds it may take, file its standard output goes to or None) -> completed in time
Runner = Callable[[Sequence[str], float, "pathlib.Path | None"], bool]


def outputs(name: str) -> tuple[str, str]:
    """The plain table a human reads in the log, and the gfm copy the page includes."""
    return f"compare-{name}.txt", f"compare-{name}.md"


def plan(entry: dict, name: str, results: pathlib.Path) -> list[tuple[list[str], pathlib.Path | None]]:
    """The three commands one comparison runs, as the workflow's loop ran them."""
    mode = [entry["compare"]] if entry.get("compare") else []
    table, gfm = outputs(name)
    return [
        ([PYTHON, entry["python"]], None),
        (["dotnet", "run", "-c", "Release", "--project", "bench/Lodestar.Text.Benchmarks", "--",
          entry["subcommand"]], None),
        ([PYTHON, "bench/compare.py", *mode], results / table),
        ([PYTHON, "bench/compare.py", *mode, "--format=gfm"], results / gfm),
    ]


def run_one(name: str, command: list[str], output: pathlib.Path | None, limit: float,
            runner: Runner) -> str:
    """One command: ``ran``, ``cut`` or ``failed``, the failure annotated."""
    try:
        return "ran" if runner(command, limit, output) else "cut"
    except subprocess.CalledProcessError as error:
        annotate(f"{name}: {' '.join(command)} exited {error.returncode}")
        return "failed"


def shared_script(command: list[str], output: pathlib.Path | None) -> str | None:
    """The script a bare ``python script`` step runs, which several comparisons may share."""
    return command[1] if output is None and command[0] == PYTHON and len(command) == 2 else None


def run_steps(name: str, steps: list[tuple[list[str], pathlib.Path | None]], left: Callable[[], float],
              runner: Runner, done: set[str], broken: set[str]) -> str:
    """One comparison's commands in order: ``ran``, ``cut`` or ``failed``. A shared Python step runs
    once a night, and one that failed fails its later sharers without running again."""
    for command, output in steps:
        shared = shared_script(command, output)
        if shared in done:
            continue
        if shared in broken:
            annotate(f"{name}: {shared} already failed tonight")
            return "failed"
        outcome = run_one(name, command, output, left(), runner)
        if shared is not None and outcome != "cut":
            (done if outcome == "ran" else broken).add(shared)
        if outcome != "ran":
            return outcome
    return "ran"


def run(names: Sequence[str], harnesses: dict, started: float, start_by: float, stop_by: float,
        results: pathlib.Path, clock: Callable[[], float] = time.time,
        runner: Runner = run_command, generated: Sequence[str] = (),
        cut_before: Sequence[str] = ()) -> Outcomes:
    """Run what fits, and settle each name in the order given.

    ``generated`` names corpora already made. A shared step that fails is not run again for the
    next comparison sharing it: ``bench_stats.py`` feeds four, about four minutes a run.
    """
    outcomes = Outcomes()
    done: set[str] = set(generated)
    broken: set[str] = set()
    for name in names:
        entry = harnesses[name]
        # Past the stop as well, whatever the two are set to: a comparison started there would be
        # "cut" before it ran, and a second cut fails it.
        if clock() >= started + min(start_by, stop_by):
            outcomes.settle(name, "late")
            continue

        # Corpora and the Python side once a night: ols, glm, var and stats share one script, which
        # writes all four results, and ran it four times for about sixteen minutes.
        steps = [([PYTHON, script], None) for script in entry.get("generate", [])]
        steps += plan(entry, name, results)
        print(f"::group::{name}", flush=True)
        outcome = run_steps(name, steps, lambda: started + stop_by - clock(), runner, done, broken)
        print("::endgroup::", flush=True)

        if outcome != "ran":
            # A half-made comparison publishes nothing.
            for leftover in outputs(name):
                (results / leftover).unlink(missing_ok=True)
        outcomes.settle(name, outcome, cut_before)
    return outcomes


def main(argv: Sequence[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--started", type=float, required=True, help="the job's start, epoch seconds")
    parser.add_argument("--start-by", type=float, required=True,
                        help="minutes after the start past which no comparison starts")
    parser.add_argument("--stop-by", type=float, required=True,
                        help="minutes after the start at which a running comparison is cut")
    parser.add_argument("--cut-before", default="",
                        help="space-separated names the previous night cut, so one cut again is failed")
    parser.add_argument("--generated", nargs="*", default=[],
                        help="corpus generators an earlier step already ran, not to be run again")
    args = parser.parse_args(argv)

    harnesses = json.loads(select_benchmarks.MAP.read_text(encoding=ENCODING))["harnesses"]
    names = [line.strip() for line in SELECTED.read_text(encoding=ENCODING).splitlines()
             if line.strip()]
    RESULTS.mkdir(parents=True, exist_ok=True)
    RAN.write_text("", encoding=ENCODING)
    outcomes = run(names, harnesses, args.started, args.start_by * 60, args.stop_by * 60, RESULTS,
                   generated=args.generated, cut_before=args.cut_before.split())
    outcomes.append_to(RAN)
    if outcomes.owed:
        print(f"::warning::out of time with {len(outcomes.owed)} comparison(s) not run; "
              f"they are carried to the next run: {' '.join(outcomes.owed)}", flush=True)
    return 0


if __name__ == "__main__":
    sys.exit(main())
