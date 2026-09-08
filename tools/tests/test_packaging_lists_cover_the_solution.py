"""Every `src/` project in the solution appears in every packaging list in CI.

Issue #545 turned four jobs red for one omission. `Lodestar.Stats` was in
`Lodestar.slnx`, declared its `PackageId` and its own `Version.props`, and had its
entry in `check_nuspec_dependencies.py`'s EXPECTED table -- but it was missing from
the `for proj in ...` lists that drive every `dotnet pack` in CI. No `.nupkg` reached
`./artifacts`, `--require-all` reported it correctly, and the three jobs that consume
that directory failed behind it for want of the artefact.

Nothing derives those lists from the solution, and nothing warned. There are five of
them -- four in `ci.yml` (the pack step of build-test-pack, the `src/ ships
PackageReference only` assertion, and the packs the sample and docs-snippets jobs each
do for themselves) and one in `sonarcloud.yml` -- so adding a package means editing
five places in the same commit, which is exactly the kind of coupling a reader cannot
see and a test can.

A project deliberately left out of packaging belongs in NOT_PACKAGED below, named and
with its reason, rather than silently absent from a list.
"""

from __future__ import annotations

import pathlib
import re

import pytest
import yaml

ROOT = pathlib.Path(__file__).resolve().parents[2]
SOLUTION = ROOT / "Lodestar.slnx"
WORKFLOWS = ROOT / ".github" / "workflows"

# Projects under src/ that are deliberately not packed. Empty today: every library
# in the solution ships. An entry here needs its reason beside it.
NOT_PACKAGED: frozenset[str] = frozenset()

SOLUTION_PROJECT = re.compile(r'Path="(src/[^"]+\.csproj)"')

# `for proj in src/A src/B ...; do` -- the shape all five lists share.
PACK_LOOP = re.compile(r"for\s+proj\s+in\s+((?:src/[\w.]+\s+)*src/[\w.]+)\s*;\s*do")

# What #545 cost, and so what this test is worth: one omission, four red jobs.
EXPECTED_LIST_COUNT = 5


def solution_projects() -> set[str]:
    """The src/ project directories the solution holds, as the lists spell them."""
    found = SOLUTION_PROJECT.findall(SOLUTION.read_text(encoding="utf-8"))
    return {path.rsplit("/", 1)[0] for path in found} - NOT_PACKAGED


def packaging_lists() -> list[tuple[str, str, set[str]]]:
    """Every `for proj in ...` list in the workflows, with where it was found."""
    lists = []
    for path in sorted(WORKFLOWS.glob("*.yml")):
        workflow = yaml.safe_load(path.read_text(encoding="utf-8"))
        for job_name, job in (workflow.get("jobs") or {}).items():
            for step in job.get("steps") or []:
                script = step.get("run")
                if not isinstance(script, str):
                    continue
                for match in PACK_LOOP.finditer(script):
                    where = f"{path.name}:{job_name}:{step.get('name', '<unnamed>')}"
                    lists.append((path.name, where, set(match.group(1).split())))
    return lists


def test_the_solution_holds_at_least_one_packable_project():
    assert solution_projects(), "no src/ project found: the solution parse is wrong"


def test_every_packaging_list_was_found():
    found = packaging_lists()
    assert len(found) == EXPECTED_LIST_COUNT, (
        f"expected {EXPECTED_LIST_COUNT} `for proj in ...` lists, found {len(found)}: "
        f"{[where for _, where, _ in found]}. A list that moved or was added changes "
        "what this test guards, so update the count deliberately.")


@pytest.mark.parametrize(
    ("where", "listed"),
    [(where, listed) for _, where, listed in packaging_lists()],
    ids=[where for _, where, _ in packaging_lists()],
)
def test_each_packaging_list_matches_the_solution(where, listed):
    expected = solution_projects()
    missing = sorted(expected - listed)
    extra = sorted(listed - expected)
    assert not missing, f"{where} does not pack {missing}, which the solution builds"
    assert not extra, f"{where} packs {extra}, which the solution does not hold"
