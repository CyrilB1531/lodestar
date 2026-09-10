#!/usr/bin/env python3
"""Refuse a README pack loop that cannot restore the sample it is followed by (#597).

README.md introduces a `for p in src/Lodestar...` loop as "A runnable version of the
above, consuming the packages exactly as you would", and follows it with a
`dotnet run --project samples/Lodestar.Sample`. samples/NuGet.config maps Lodestar.* to
the local ./artifacts feed, so a package the loop does not pack is a package the sample
cannot restore -- the command fails as written, and nothing noticed.

Measured on 2026-09-10: the loop packed nine of the fifteen the sample references, and
five of the six missing had been missing for whole lots. Seven hard-coded pack lists exist
in this repository; the six that something reads stayed current, and this one did not.

That is the same shape #586 fixed for benchmarks, and tools/check_bench_map.py is the
model: read both sides from their own file and compare, so the divergence fails on the
commit that introduces it rather than going quiet until someone follows the instructions.

The sample's own <PackageReference> list is the source of truth, not src/: a package that
exists and that the sample does not consume has no business in a loop whose stated purpose
is to make the sample runnable. Order is not compared -- the loop reads dependency-first
and that is a matter of presentation.

Usage:  python tools/check_readme_pack_loop.py
        python tools/check_readme_pack_loop.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
README = ROOT / "README.md"
SAMPLE = ROOT / "samples" / "Lodestar.Sample" / "Lodestar.Sample.csproj"

# One flat run, not a repeated group: a nested quantifier over optional whitespace splits
# the same run many ways, which is exponential backtracking (python:S5852, measured at 10s).
LOOP = re.compile(r"for p in([^;]*);\s*do")
PACKAGE_REFERENCE = re.compile(r'<PackageReference\s+Include="(Lodestar\.[A-Za-z.]+)"')


def label(path: pathlib.Path) -> str:
    """A repository-relative path, so a finding is the same on every machine."""
    try:
        return path.relative_to(ROOT).as_posix()
    except ValueError:
        # A path outside the tree, which only the unit tests hand this. Reporting it
        # whole beats raising out of a function whose job is to format a message.
        return path.as_posix()


def sample_packages() -> set[str]:
    """Every Lodestar package the packaging sample declares a reference to."""
    return set(PACKAGE_REFERENCE.findall(SAMPLE.read_text(encoding="utf-8")))


def loop_packages(text: str) -> set[str] | None:
    """The packages the README's loop packs, or None when there is no loop to read."""
    match = LOOP.search(text)
    if match is None:
        return None

    # Whitespace and the backslash continuations fall out of split(); anything that is
    # not a src/Lodestar.* path is not a package this loop packs.
    return {
        token.removeprefix("src/")
        for token in match.group(1).split()
        if token.startswith("src/Lodestar.")
    }


def findings() -> list[str]:
    for path in (README, SAMPLE):
        if not path.exists():
            return [f"{label(path)}: missing"]

    packed = loop_packages(README.read_text(encoding="utf-8"))
    if packed is None:
        return [
            f"{label(README)}: no `for p in src/Lodestar...; do` loop to check. It is what "
            "makes the sample runnable, and a README that lost it cannot be vouched for."
        ]

    needed = sample_packages()
    missing = sorted(needed - packed)
    extra = sorted(packed - needed)

    result = []
    if missing:
        result.append(
            f"{label(README)}: the pack loop does not pack {', '.join(missing)}, which "
            f"{label(SAMPLE)} references. samples/NuGet.config maps Lodestar.* to "
            "./artifacts, so the sample cannot restore and the documented command fails."
        )
    if extra:
        result.append(
            f"{label(README)}: the pack loop packs {', '.join(extra)}, which "
            f"{label(SAMPLE)} does not reference. The loop exists to make that sample "
            "runnable, so a package it never consumes only slows the instructions down."
        )

    return result


def main() -> int:
    if len(sys.argv) > 1:
        if sys.argv[1] in ("--help", "-h"):
            print(__doc__)
            return 0
        print(__doc__)
        return 2

    found = findings()
    for finding in found:
        print(finding)
    if not found:
        print(f"ok  the README pack loop packs the {len(sample_packages())} packages "
              "the sample references")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
