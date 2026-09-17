#!/usr/bin/env python3
"""Copy the Markdown and map a test project copies to its output, from the checkout, into a built output (#997).

A docs-only pull request runs the documentation tests on the net10.0 binaries main's own CI run
published for the pull request's base commit, so nothing is compiled. Those binaries were built
with main's docs; the tests read the files beside the assembly, so this puts the pull request's
own docs there, exactly where MSBuild would have: every ``<None Include=... CopyToOutputDirectory>``
item of the project whose ``Include`` reaches ``docs/``, at its ``Link``, or at ``LinkBase`` plus the
path below the glob's fixed part (with neither, at the file's own name), minus any ``Exclude``.

Reading the project rather than assuming its layout is the point: ``Lodestar.Stats.Tests`` copies
``docs/reference/stats.md`` beside the pages of its own folder, and a hard-coded pattern missed it.

Usage:  python tools/stage_doc_inputs.py tests/Lodestar.Stats.Tests/Lodestar.Stats.Tests.csproj OUTPUT_DIR
Replaces the ``docs``, ``reference`` and copied files under OUTPUT_DIR; exits 1 on a project it
cannot read.
"""

from __future__ import annotations

import fnmatch
import pathlib
import shutil
import sys
import xml.etree.ElementTree as ET


def _split(pattern: str) -> tuple[str, str]:
    """The pattern's fixed directory part and the wildcard remainder."""
    parts = pattern.split("/")
    fixed: list[str] = []
    for part in parts:
        if any(ch in part for ch in "*?["):
            break
        fixed.append(part)
    return "/".join(fixed), "/".join(parts[len(fixed):])


def _matches(relative: str, remainder: str) -> bool:
    """Whether a path below the fixed part matches a remainder such as ``**/*.md``."""
    if remainder.startswith("**/"):
        tail = remainder[3:]
        return fnmatch.fnmatch(pathlib.PurePosixPath(relative).name, tail) or fnmatch.fnmatch(relative, tail)
    return fnmatch.fnmatch(relative, remainder)


def plan(project: pathlib.Path) -> list[tuple[pathlib.Path, str]]:
    """Each (source file, destination relative to the output) the project copies from ``docs/``."""
    root = ET.parse(project).getroot()
    base = project.parent
    copies: list[tuple[pathlib.Path, str]] = []
    for item in root.iter("None"):
        include = (item.get("Include") or "").replace("\\", "/")
        if "docs/" not in include or not item.get("CopyToOutputDirectory"):
            continue
        link_base = item.get("LinkBase")
        excludes = [(base / e.replace("\\", "/")).resolve().as_posix() for e in (item.get("Exclude") or "").split(";") if e]
        fixed, remainder = _split(include)
        fixed_dir = (base / fixed).resolve()
        if not remainder:
            if fixed_dir.is_file():
                link = (item.get("Link") or "").replace("\\", "/")
                copies.append((fixed_dir, link or (f"{link_base}/{fixed_dir.name}" if link_base else fixed_dir.name)))
            continue
        for source in sorted(fixed_dir.rglob("*")):
            if not source.is_file():
                continue
            relative = source.relative_to(fixed_dir).as_posix()
            if not _matches(relative, remainder):
                continue
            absolute = source.as_posix()
            if any(fnmatch.fnmatch(absolute, e) or absolute.startswith(e.rstrip("*").rstrip("/") + "/") for e in excludes):
                continue
            copies.append((source, f"{link_base}/{relative}" if link_base else source.name))
    return copies


def stage(project: pathlib.Path, output: pathlib.Path) -> int:
    """Replace the output's copies with the checkout's, returning how many files were copied."""
    copies = plan(project)
    for top in {destination.split("/", 1)[0] for _, destination in copies if "/" in destination}:
        shutil.rmtree(output / top, ignore_errors=True)
    for source, destination in copies:
        target = output / destination
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(source, target)
    return len(copies)


def main(argv: list[str]) -> int:
    if len(argv) != 3:
        print(__doc__, file=sys.stderr)
        return 2
    try:
        count = stage(pathlib.Path(argv[1]), pathlib.Path(argv[2]))
    except (OSError, ET.ParseError) as error:
        print(f"{argv[1]}: {error}", file=sys.stderr)
        return 1
    print(f"{argv[1]}: {count} files")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
