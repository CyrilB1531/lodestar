#!/usr/bin/env python3
"""Refuse an ADR whose frontmatter is missing, unreadable or points nowhere (#630).

`docs/decisions/index.yaml` is generated from what the records declare, so a record
that declares nothing is invisible to it -- and invisible in the one direction that
matters, because an immutable ADR cannot be edited to name the decision that later
amended it. A missing frontmatter block is not a gap in a file, it is a reader sent
to a decision without being told it moved.

What every record carries, and what this holds it to:

  status       the word its `**Status:**` line opens with
  supersedes   the decisions it replaces, in whole or in the part it names
  amends       the decisions it changes and leaves standing
  applies      the decisions it uses again, unchanged, on a rule they already
               stated -- `0097` of `0095`: "This record does not amend 0095 so
               much as apply it"

All four are required, empty lists included. An absent key and an empty list read
the same to a generator and not at all the same to a reader: one is a record that
declared nothing, the other a record that declared no relation, and only the second
is a statement. Requiring all four is what makes `[]` mean something.

A relation naming a number no record carries is a finding here rather than a
silently dropped edge in the index, and so is a record naming itself.

Standard library only: `.githooks/pre-commit` runs this through whichever of
`python3` or `python` a contributor's machine resolves, so an import outside the
standard library would turn a missing package into a failed commit.

Usage:  python tools/check_adr_frontmatter.py
        python tools/check_adr_frontmatter.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import sys

# PYTHONSAFEPATH=1 keeps this script's own directory off sys.path, the way
# generate_oracles.py documents -- appended, so nothing here shadows a package.
sys.path.append(str(pathlib.Path(__file__).resolve().parent))

import adr_index  # noqa: E402

REQUIRED = ("status", *adr_index.FORWARD)


def label(path: pathlib.Path) -> str:
    """A repository-relative path, so a finding reads the same on every machine."""
    try:
        return path.relative_to(adr_index.ROOT).as_posix()
    except ValueError:
        return path.as_posix()


def findings_for(path: pathlib.Path, known: set[str]) -> list[str]:
    """One record, against the shape every record carries."""
    where = label(path)
    try:
        record = adr_index.read_record(path)
    except adr_index.AdrError as error:
        return [f"{where}: {error}"]

    frontmatter = record["frontmatter"]
    if frontmatter is None:
        return [
            f"{where}: no YAML frontmatter. Open the file with a `---` block declaring "
            f"{', '.join(REQUIRED)} -- empty lists where there is no relation, which is "
            "a statement rather than a gap."
        ]

    found = []
    for key in REQUIRED:
        if key not in frontmatter:
            found.append(f"{where}: frontmatter declares no `{key}`.")
    for key in frontmatter:
        if key not in REQUIRED:
            found.append(
                f"{where}: frontmatter declares `{key}`, which is not one of "
                f"{', '.join(REQUIRED)}. The index reads four keys and nothing else.")

    found += status_findings(where, frontmatter, record["status_line"])
    found += relation_findings(where, str(record["number"]), frontmatter, known)
    return found


def status_findings(where: str, frontmatter: dict, status_line: str | None) -> list[str]:
    """The frontmatter's `status` against the `**Status:**` line below it.

    Two spellings of one fact in one file is how a fact drifts, so the rule is
    that the line opens with the word: `0013`'s line reads "accepted, superseded
    in part by `0014`", whose first word is what the frontmatter carries.
    """
    status = str(frontmatter.get("status", "")).strip()
    if not status:
        return [f"{where}: frontmatter `status` is empty."]
    if status_line is None:
        return [
            f"{where}: no `**Status:**` line, so the frontmatter's `status: {status}` "
            "answers to nothing."
        ]
    if not status_line.startswith(status):
        return [
            f"{where}: frontmatter says `status: {status}` and the `**Status:**` line "
            f"reads {status_line!r}. The line is the prose and the frontmatter is its "
            "first word; make them agree."
        ]
    return []


def relation_findings(
        where: str, number: str, frontmatter: dict, known: set[str]) -> list[str]:
    """Every declared edge, against the records that exist."""
    found = []
    for relation in adr_index.FORWARD:
        targets = frontmatter.get(relation)
        if not isinstance(targets, list):
            continue
        for target in targets:
            if target == number:
                found.append(f"{where}: `{relation}` names {target}, which is itself.")
            elif target not in known:
                found.append(
                    f"{where}: `{relation}` names {target}, and there is no "
                    f"docs/decisions/{target}-*.md. A relation onto nothing is an edge "
                    "the index cannot reverse.")
        if len(set(targets)) != len(targets):
            found.append(f"{where}: `{relation}` names the same decision twice.")
    return found


def findings() -> list[str]:
    paths = adr_index.adr_paths()
    if not paths:
        return ["docs/decisions/: no ADR files at all, which cannot be right."]
    known = {adr_index.number_of(path) for path in paths}
    return [finding for path in paths for finding in findings_for(path, known)]


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
        print(f"ok  {len(adr_index.adr_paths())} ADRs declare a readable frontmatter, "
              "and every relation names a record that exists")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
