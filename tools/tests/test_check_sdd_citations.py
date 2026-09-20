"""check_sdd_citations.py's guard on a citation of a file that was never tracked (#730).

Seventeen comments and ADR 0002 cited `task-N-report.md` from a plan's workspace,
which is ignored by git and deleted when the plan finishes. The strings below are
the shapes that reached the repository, taken from the lines #730 rewrote, so the
guard is shown catching what happened rather than what was imagined.

The first test is the one that matters on every commit: the tree as it stands
carries no such citation outside the exemptions.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_sdd_citations as guard  # noqa: E402

ROOT = Path(__file__).resolve().parents[2]

# argv[0], unused by main(); a constant keeps python:S1192 quiet.
PROG = "check_sdd_citations.py"
SOURCE = "src/Lodestar.Stats/Example.cs"

# As they appeared in src/Lodestar.Stats/KolmogorovSmirnov.cs and MannWhitney.cs.
REPORT = "    // reported the wrong D-/location past the shorter sample's end (task-8-report.md).\n"
ROUND = "    // and no corpus case exercised that (task-6-report.md, fix round 1,\n"
BRIEF = "    // the brief said so (task-12-brief.md).\n"

# As it appeared in ADR 0002, and the same path as Windows writes it.
WORKSPACE = "Task 8 (`.superpowers/sdd/2026-09-05_0442_lodestar-stats/`, finding 4)\n"
WORKSPACE_WINDOWS = "see .superpowers\\sdd\\plan\\progress.md\n"


def _tree(tmp_path, files):
    for path, text in files.items():
        target = tmp_path / path
        target.parent.mkdir(parents=True, exist_ok=True)
        target.write_text(text, encoding="utf-8")
    return guard.findings(tmp_path, list(files))


def test_the_tree_as_it_stands_cites_no_workspace_file():
    assert guard.findings(ROOT, guard.tracked_files()) == []


def test_a_report_citation_in_a_comment_is_a_finding_on_its_line(tmp_path):
    found = _tree(tmp_path, {SOURCE: "namespace X;\n\n" + REPORT})
    assert found == [f"{SOURCE}:3: task-8-report.md"]


def test_every_shape_that_reached_the_repository_is_a_finding(tmp_path):
    found = _tree(tmp_path, {SOURCE: ROUND + BRIEF + WORKSPACE + WORKSPACE_WINDOWS})
    assert len(found) == 4


def test_a_placeholder_with_no_task_number_is_not_a_citation(tmp_path):
    # Prose describing the workspace's layout names no file anyone could have opened.
    assert _tree(tmp_path, {"README.md": "each task writes task-N-report.md\n"}) == []


def test_plans_and_the_vendored_skills_describe_the_workspace_and_are_exempt(tmp_path):
    files = {
        "docs/superpowers/plans/2026-09-05_0442_lodestar-stats.md": WORKSPACE,
        ".claude/skills/subagent-driven-development/scripts/task-brief": BRIEF,
    }
    assert _tree(tmp_path, files) == []


def test_adr_0082_is_exempt_and_a_new_record_is_not(tmp_path):
    files = {
        "docs/decisions/0002-provenance-and-the-allowed-references.md": WORKSPACE,
        "docs/decisions/0200-a-later-decision.md": WORKSPACE,
    }
    assert _tree(tmp_path, files) == ["docs/decisions/0200-a-later-decision.md:1: .superpowers/sdd"]


def test_every_exempt_file_still_exists():
    # An exemption outliving its file is how a list like this rots.
    for path in guard.EXEMPT_FILES:
        assert (ROOT / path).is_file(), f"{path} is gone; drop its exemption"


def test_help_prints_to_stdout_and_exits_zero(capsys):
    assert guard.main([PROG, "--help"]) == 0
    captured = capsys.readouterr()
    assert captured.out
    assert not captured.err


def test_an_unrecognised_argument_is_bad_usage_on_stderr(capsys):
    assert guard.main([PROG, "--nonsense"]) == 2
    captured = capsys.readouterr()
    assert not captured.out
    assert captured.err
