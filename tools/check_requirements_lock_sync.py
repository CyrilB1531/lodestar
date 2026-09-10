#!/usr/bin/env python3
"""Refuse a requirements.txt pin that requirements.lock.txt has never heard of (#605).

CI installs the oracle dependencies from tools/requirements.lock.txt, not from
tools/requirements.txt. The first is pip-compile output and the second is written by
hand, so they drift in exactly one direction: a pin added to requirements.txt and
never compiled into the lock.

Measured on 2026-09-10: #604 added datasketch and simhash to requirements.txt and not
to the lock. Every local check passed -- the developer venv already had both installed
by hand -- and the `Oracles are reproducible` job stopped on
`ModuleNotFoundError: No module named 'datasketch'`. One push, one red job, one
recompile.

#38 introduced the hashed lock and that was right. What it did not add is anything
reading both files, which is the shape #597 already fixed twice for README.md's pack
loop and CLAUDE.md's package table. tools/check_bench_map.py is the model: read each
side from the file that owns it and compare, so a divergence fails on the commit that
introduces it rather than going quiet until someone follows the instructions.

Three things this deliberately does NOT check:

  transitive pins   The lock holds far more than requirements.txt names, and that is
                    the whole point of compiling it. Only the direct pins are read.
  hashes            `pip install --require-hashes` verifies those and already runs.
  freshness         Asserting the lock is a *current* compile would mean resolving the
                    index, which an offline guard cannot do -- and the install step
                    catches a stale resolution anyway.

Usage:  python tools/check_requirements_lock_sync.py
        python tools/check_requirements_lock_sync.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
REQUIREMENTS = ROOT / "tools" / "requirements.txt"
LOCK = ROOT / "tools" / "requirements.lock.txt"

# A direct pin. Anything else -- a bare name, a range, an include -- cannot be
# compared against a lock, and is reported rather than skipped.
PIN = re.compile(r"^(?P<name>[A-Za-z0-9][A-Za-z0-9._-]*)==(?P<version>[^\s;#]+)")

# The lock's own pins open a line and carry a trailing backslash for the hashes that
# follow. Continuation lines are indented, so an anchored match cannot confuse them.
LOCK_PIN = re.compile(r"^(?P<name>[A-Za-z0-9][A-Za-z0-9._-]*)==(?P<version>[^\s\;]+)")


def normalize(name: str) -> str:
    """PEP 503's rule: a distribution name compares case-insensitively, runs folded.

    pip-compile writes `rake_nltk` where requirements.txt says `rake-nltk`, so a
    comparison on the raw strings reports a divergence that does not exist.
    """
    return re.sub(r"[-_.]+", "-", name).lower()


def read_pins(path: pathlib.Path, pattern: re.Pattern[str]) -> dict[str, str]:
    """The direct pins a file declares, keyed by normalized name."""
    pins: dict[str, str] = {}
    for line in path.read_text(encoding="utf-8").splitlines():
        if not line or line[0].isspace() or line.lstrip().startswith("#"):
            continue
        found = pattern.match(line.strip())
        if found is not None:
            pins[normalize(found.group("name"))] = found.group("version")

    return pins


def unpinned(path: pathlib.Path) -> list[str]:
    """Lines that declare a dependency without pinning it to one version."""
    loose: list[str] = []
    for line in path.read_text(encoding="utf-8").splitlines():
        stripped = line.strip()
        if not stripped or stripped.startswith(("#", "-")):
            continue
        if PIN.match(stripped) is None:
            loose.append(stripped)

    return loose


def findings() -> list[str]:
    """Everything wrong, so one fix does not hide the next."""
    found: list[str] = []
    for path in (REQUIREMENTS, LOCK):
        if not path.exists():
            found.append(f"{path.relative_to(ROOT)}: missing.")
    if found:
        return found

    declared = read_pins(REQUIREMENTS, PIN)
    locked = read_pins(LOCK, LOCK_PIN)

    for loose in unpinned(REQUIREMENTS):
        found.append(
            f"tools/requirements.txt: '{loose}' is not pinned to one version, so the "
            f"lock cannot be compared against it.")

    for name, version in sorted(declared.items()):
        if name not in locked:
            found.append(
                f"tools/requirements.lock.txt: no pin for '{name}', which "
                f"tools/requirements.txt asks for at {version}. Recompile with the "
                f"command in the lock file's own header.")
        elif locked[name] != version:
            found.append(
                f"tools/requirements.lock.txt: pins '{name}' at {locked[name]} where "
                f"tools/requirements.txt asks for {version}.")

    return found


def main() -> int:
    if len(sys.argv) > 1:
        if sys.argv[1] in ("--help", "-h"):
            print(__doc__)
            return 0

        print(f"unexpected argument: {sys.argv[1]}", file=sys.stderr)
        return 2

    found = findings()
    for finding in found:
        print(f"::error::{finding}")
    if found:
        return 1

    declared = read_pins(REQUIREMENTS, PIN)
    print(f"ok  the lock carries all {len(declared)} pins tools/requirements.txt declares, "
          f"at the versions it declares them")
    return 0


if __name__ == "__main__":
    sys.exit(main())
