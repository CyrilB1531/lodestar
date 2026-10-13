"""Run one nightly class for at most a number of seconds, killing its whole process group when cut (#1687).

The nightly's class loop cuts a BenchmarkDotNet run at the class stop. `timeout` did it until the
runner's coreutils changed: GNU's sends its kill to the group, uutils' to the direct child alone, so
the benchmark `dotnet run` starts as a child of its own outlived the cut. This runs the class in a
session of its own and kills the group, as `nightly_outcomes.py` does, and settles it through the
same rule the comparisons use, appending its name to `classes-run.txt` or the shared outcome files:

    python tools/run_bounded.py --name NAME --project PROJECT --seconds SECONDS --cut-before "A B"

The command is built here, from a class `bench/bench-map.json` maps and a project in `PROJECTS`,
so nothing from the command line is run or written to. The outcome is told by whether the run was
cut, never by its exit code. Exits 0 once the name is settled, and 2 for an unknown name or project.
"""

from __future__ import annotations

import argparse
import json
import pathlib
import subprocess
import sys
from collections.abc import Callable, Sequence

import nightly_outcomes

ROOT = pathlib.Path(__file__).resolve().parents[1]
MAP = ROOT / "bench" / "bench-map.json"
RAN = pathlib.Path("classes-run.txt")

# The projects a [Benchmark] class may live in: check_bench_map.py's CLASS_DIRS, which a test holds
# this to.
PROJECTS = ("bench/Lodestar.Text.Benchmarks", "bench/Lodestar.Stats.Benchmarks",
            "bench/Lodestar.Survival.Benchmarks", "bench/Lodestar.Gpu.Benchmarks")


def command_for(name: str, project: str) -> list[str]:
    """The class and no other: "*Name*" also matched every class whose name contains it. --job short:
    a hosted runner cannot hold the default job's iterations inside a nightly window."""
    return ["dotnet", "run", "-c", "Release", "--project", project, "--",
            "--filter", f"*.{name}.*", "--job", "short", "--exporters", "github"]


def settle_class(name: str, seconds: float, cut_before: Sequence[str], command: Sequence[str],
                 runner: Callable[..., bool] = nightly_outcomes.run_command,
                 directory: pathlib.Path = pathlib.Path()) -> None:
    """Run ``command`` for ``name`` and append the name to the file its outcome names."""
    outcomes = nightly_outcomes.Outcomes()
    if seconds <= 0:
        outcome = "late"
    else:
        try:
            outcome = "ran" if runner(command, seconds, None) else "cut"
        except subprocess.CalledProcessError as error:
            nightly_outcomes.annotate(f"{name} failed with exit code {error.returncode}")
            outcome = "failed"
    outcomes.settle(name, outcome, cut_before)
    outcomes.append_to(RAN, directory)


def main(argv: list[str]) -> int:
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--name", required=True)
    parser.add_argument("--project", required=True)
    parser.add_argument("--seconds", type=float, required=True)
    parser.add_argument("--cut-before", default="",
                        help="space-separated names the previous night cut, so one cut again is failed")
    args = parser.parse_args(argv)

    # Each taken from its own list rather than from the argument, which only selects it.
    name = next((known for known in json.loads(MAP.read_text(encoding=nightly_outcomes.ENCODING))
                 .get("benchmarks", {}) if known == args.name), None)
    project = next((known for known in PROJECTS if known == args.project), None)
    if name is None or project is None:
        print(f"unknown class {args.name!r} or project {args.project!r}", file=sys.stderr)
        return 2
    settle_class(name, args.seconds, args.cut_before.split(), command_for(name, project))
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
