"""Run one nightly class for at most a number of seconds, killing its whole process group when cut (#1687).

The nightly's class loop cuts a BenchmarkDotNet run at the class stop. `timeout` did it until the
runner's coreutils changed: GNU's sends its kill to the group, uutils' to the direct child alone, so
the benchmark `dotnet run` starts as a child of its own outlived the cut. This runs the class in a
session of its own and kills the group, as `nightly_outcomes.py` does, and settles it through the
same rule the comparisons use, appending its name to `classes-run.txt` or the shared outcome files:

    python tools/run_bounded.py --name NAME --seconds SECONDS

The command is built here, from a class `bench/bench-map.json` maps and the project in `PROJECTS`
that declares it, so nothing from the command line is run or written to. The outcome is told by
whether the run was cut, never by its exit code. Exits 0 once the name is settled, a class no
project declares being failed, and 2 for a name the map does not know.
"""

from __future__ import annotations

import argparse
import json
import pathlib
import re
import subprocess
import sys
from collections.abc import Callable, Sequence

import nightly_outcomes

ROOT = pathlib.Path(__file__).resolve().parents[1]
MAP = ROOT / "bench" / "bench-map.json"
RAN = pathlib.Path("classes-run.txt")

# A class declaration, as both this and check_bench_map read one: it starts its line, so a comment
# mentioning the class is not one. Groups: its modifiers, its name, and `<` when it is generic.
DECLARATION = re.compile(r"^[ \t]*((?:(?:public|internal|sealed|abstract|partial|static)\s+)*)class\s+(\w+)(\s*<)?",
                         re.MULTILINE)

# The [Benchmark] attribute itself, bare, with arguments, beside others or qualified, starting its line:
# not [BenchmarkCategory], and not a mention in a comment.
BENCHMARK = re.compile(r"^[ \t]*\[(?:BenchmarkDotNet\.Attributes\.)?Benchmark(?:Attribute)?\s*[\](,]",
                       re.MULTILINE)

# C# sources as an editor may save them: a byte-order mark would otherwise stand before the first token.
SOURCE = "utf-8-sig"

# The projects a [Benchmark] class may live in, which check_bench_map.py scans. Lodestar.NetStandard
# .Benchmarks is absent: it links Lodestar.Text.Benchmarks' .cs files rather than declaring its own.
PROJECTS = ("bench/Lodestar.Text.Benchmarks", "bench/Lodestar.Stats.Benchmarks",
            "bench/Lodestar.Survival.Benchmarks", "bench/Lodestar.Gpu.Benchmarks")


def declarations():
    """Each benchmark class, as (match, path, text): in a file of PROJECTS carrying a [Benchmark], the
    class declared last before the first one. Helpers declared beside it are not benchmark classes,
    and this is the one enumeration the runner and check_bench_map both read."""
    for project in PROJECTS:
        for path in sorted((ROOT / project).glob("*.cs")):
            text = path.read_text(encoding=SOURCE)
            attribute = BENCHMARK.search(text)
            before = list(DECLARATION.finditer(text, 0, attribute.start())) if attribute else []
            if before:
                yield before[-1], path, text


def project_of(name: str) -> str | None:
    """The project whose [Benchmark] sources declare ``name``."""
    return next((path.parent.relative_to(ROOT).as_posix()
                 for match, path, _ in declarations() if match.group(2) == name), None)


def command_for(name: str, project: str) -> list[str]:
    """The class and no other: "*Name*" also matched every class whose name contains it. --job short:
    a hosted runner cannot hold the default job's iterations inside a nightly window."""
    return ["dotnet", "run", "-c", "Release", "--project", project, "--",
            "--filter", f"*.{name}.*", "--job", "short", "--exporters", "github"]


def settle_class(name: str, seconds: float, command: Sequence[str],
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
    outcomes.settle(name, outcome)
    outcomes.append_to(RAN, directory)


def main(argv: list[str]) -> int:
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--name", required=True)
    parser.add_argument("--seconds", type=float, required=True)
    args = parser.parse_args(argv)

    # Taken from the map rather than from the argument, which only selects it.
    name = next((known for known in json.loads(MAP.read_text(encoding=nightly_outcomes.ENCODING))
                 .get("benchmarks", {}) if known == args.name), None)
    if name is None:
        print(f"unknown class {args.name!r}", file=sys.stderr)
        return 2
    project = project_of(name)
    if project is None:
        nightly_outcomes.annotate(f"{name} is in bench/bench-map.json and in no benchmark project")
        outcomes = nightly_outcomes.Outcomes()
        outcomes.settle(name, "failed")
        outcomes.append_to(RAN)
        return 0
    print(f"::group::{project} -- *.{name}.*", flush=True)
    try:
        settle_class(name, args.seconds, command_for(name, project))
    finally:
        print("::endgroup::", flush=True)
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
