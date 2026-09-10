#!/usr/bin/env python3
"""Assert every netstandard2.0 mirror really mirrors, and says so.

Each ``tests/<Package>.NetStandard.Tests`` project links its sibling suite's sources
and pins the library under test to the netstandard2.0 build with
``SetTargetFramework``. That pin is the whole point of the project: without it the
assemblies shipped to .NET Framework, Mono and Unity are compile-verified but never
executed.

Two things make the pin fail silently, and this checks both.

1. **``SetTargetFramework`` does not travel across a ``PackageReference``.** ``src/``
   projects reach each other through published packages, and NuGet resolves package
   assets against the *consuming* project's framework — net10.0 for a mirror. So a
   mirror that pins only its own library still loads its dependencies' net10.0 build.
   Every ``Lodestar.*`` package a library depends on therefore needs its own pinned
   ``ProjectReference`` in the mirror. Measured on 2026-09-02: ``Lodestar.Text`` and
   ``Lodestar.Decomposition`` were running against the net10.0 ``Lodestar.Abstractions``,
   832 tests green and half of each one proving nothing (#529).

2. **Nothing asserts the pin at run time** unless the mirror carries
   ``NetStandardAssemblyGuardTests.cs``, which reads the loaded assembly's
   ``TargetFrameworkAttribute``. Three of seven mirrors had no such file, which is how
   rule 1's breakage survived.

Usage:  python tools/check_netstandard_guards.py
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent

GUARD = "NetStandardAssemblyGuardTests.cs"

# The suffix every mirror's directory carries, spelled once.
SUFFIX = ".NetStandard.Tests"

# A library's dependencies, as the src project declares them.
SRC_PACKAGE_REFERENCE = re.compile(r'<PackageReference\s+Include="(Lodestar\.[A-Za-z.]+)"')

# long-comment: why the contract is a table rather than a constant. Every package
# targets netstandard2.0 except Lodestar.Gpu, whose dependency publishes no such asset
# and does publish a 2.1 one (decisions 0101 and 0103). Read against a constant, a 2.1
# mirror passed this guard vacuously: it has no Lodestar dependency to pin, so the loop
# below had nothing to iterate and nothing to report, while the guard printed "16
# netstandard2.0 mirrors" and one of them was not.
MIRRORED = {"Lodestar.Gpu": "netstandard2.1"}
DEFAULT_CONTRACT = "netstandard2.0"

SRC_TARGETS = re.compile(r"<TargetFrameworks?>([^<]+)</TargetFrameworks?>")


def contract_of(package: str) -> str:
    """The netstandard moniker this package's mirror has to pin."""
    return MIRRORED.get(package, DEFAULT_CONTRACT)


def mirror_pins(text: str, contract: str) -> set[str]:
    """The Lodestar projects a mirror pins to <paramref name="contract"/>."""
    pattern = re.compile(
        r'<ProjectReference\s+Include="[^"]*/(Lodestar\.[A-Za-z.]+)\.csproj"'
        rf'\s+SetTargetFramework="TargetFramework={re.escape(contract)}"')
    return set(pattern.findall(text))


def mirrors() -> list[pathlib.Path]:
    return sorted((ROOT / "tests").glob(f"*{SUFFIX}"))


def failures_in(mirror: pathlib.Path) -> list[str]:
    package = mirror.name[: -len(SUFFIX)]
    found = []

    if not (mirror / GUARD).is_file():
        found.append(
            f"{mirror.relative_to(ROOT)}: no {GUARD}. Nothing asserts this suite loads the "
            f"netstandard2.0 assembly, so a reference that resolved back to net10.0 would "
            f"leave every test passing while proving nothing.")

    project = next(mirror.glob("*.csproj"), None)
    if project is None:
        found.append(f"{mirror.relative_to(ROOT)}: no .csproj.")
        return found

    source = ROOT / "src" / package / f"{package}.csproj"
    if not source.is_file():
        found.append(f"{mirror.relative_to(ROOT)}: no library at src/{package}.")
        return found

    contract = contract_of(package)
    body = project.read_text(encoding="utf-8")
    library = source.read_text(encoding="utf-8")

    # long-comment: the library has to declare the contract its mirror claims to replay,
    # or a mirror pinning a framework that does not exist reads as a pass with an
    # unexecuted assembly behind it. A csproj declaring no framework at all is left
    # alone: it does not build, so the compiler reports it long before this does, and
    # the fixtures in tools/tests deliberately do not model one.
    declared = {target.strip() for group in SRC_TARGETS.findall(library)
                for target in group.split(";")}
    if declared and contract not in declared:
        found.append(
            f"src/{package}/{package}.csproj: targets {sorted(declared)} and its mirror "
            f"replays {contract}. One of the two is wrong, and tools/"
            f"check_netstandard_guards.py's MIRRORED is where the intended one is written.")
        return found

    if declared and not mirror_pins(body, contract) and (
            package in MIRRORED or SRC_PACKAGE_REFERENCE.search(library)):
        found.append(
            f"{project.relative_to(ROOT)}: pins nothing to {contract}, so this suite replays "
            f"the net10.0 build of {package} and its guard asserts a framework the assembly "
            f"does not carry.")

    pinned = mirror_pins(body, contract)
    for dependency in sorted(set(SRC_PACKAGE_REFERENCE.findall(library))):
        if dependency not in pinned:
            found.append(
                f"{project.relative_to(ROOT)}: {package} depends on {dependency}, which is not "
                f"pinned here. SetTargetFramework does not cross a PackageReference, so this "
                f"suite loads {dependency}'s net10.0 build. Add a ProjectReference to "
                f"../../src/{dependency}/{dependency}.csproj with "
                f"SetTargetFramework=\"TargetFramework={contract}\".")
    return found


def main() -> int:
    projects = mirrors()
    if not projects:
        print("::error::no tests/*.NetStandard.Tests projects found")
        return 1

    # Every mirror is checked before anything is reported: stopping at the first
    # failure would hide a second one behind a fix for the first.
    found = [failure for mirror in projects for failure in failures_in(mirror)]

    for failure in found:
        print(f"::error::{failure}")
    if found:
        return 1

    contracts = ", ".join(
        f"{mirror.name[: -len(SUFFIX)]} on {contract_of(mirror.name[: -len(SUFFIX)])}"
        for mirror in projects if mirror.name[: -len(".NetStandard.Tests")] in MIRRORED)
    print(f"ok  {len(projects)} netstandard mirrors carry a guard and pin every Lodestar "
          f"dependency they load ({contracts or 'all on ' + DEFAULT_CONTRACT})")
    return 0


if __name__ == "__main__":
    sys.exit(main())
