"""check_spec_status.py: a spec says when it was written, and a plan is not committed.

The vocabulary is closed on purpose, and the test that matters is the one for a
status the old tree was full of: "accepted, 2026-09-16. Written before the work."
carries the fact, buried behind a lifecycle word every spec would carry alike. A
guard that searched the line for "before" would pass it and leave the reader
doing the parsing, so the rule reads the opening clause and nothing else.

The other half is the plan. `docs/superpowers/plans/` held 116 files written for
work that had merged; nothing outside `docs/superpowers/` cited one. The guard
refuses their return rather than trusting the rule to be remembered.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_spec_status as guard  # noqa: E402

NAME = "2026-09-20_1104_specs-and-plans.md"


def spec_body(status: str) -> str:
    """A spec whose metadata paragraph carries `status`, in the shape the tree uses."""
    return (f"# 1104 — A spec\n\n"
            f"**Issue:** [#1104](https://example.invalid/1104) · **Status:** {status} · "
            f"**Date:** 2026-09-20\n\n## Problem\n\nSomething.\n")


def spec(tmp_path: Path, status: str, *, name: str = NAME) -> Path:
    path = tmp_path / name
    path.write_text(spec_body(status), encoding="utf-8")
    return path


def test_each_of_the_three_timings_passes(tmp_path):
    for timing in guard.TIMINGS:
        assert guard.status_findings(spec(tmp_path, timing)) == []


def test_free_text_after_the_opening_clause_is_not_read(tmp_path):
    status = "**retrospective** — written 2026-08-29 from the commits that closed it"
    assert guard.status_findings(spec(tmp_path, status)) == []
    assert guard.status_findings(
        spec(tmp_path, "written before the work, 2026-09-12; amended during it")) == []


def test_a_buried_timing_is_a_finding(tmp_path):
    # The shape 143 specs carried: the fact is there, behind a word every spec shares.
    found = guard.status_findings(spec(tmp_path, "accepted, 2026-09-16. Written before the work."))
    assert len(found) == 1
    assert "does not open with one of" in found[0]
    # The finding quotes the field alone, not the two metadata fields beside it.
    assert "**Date:**" not in found[0]


def test_a_lifecycle_word_alone_is_a_finding(tmp_path):
    assert len(guard.status_findings(spec(tmp_path, "accepted"))) == 1


def test_a_spec_with_no_status_is_a_finding(tmp_path):
    path = tmp_path / NAME
    path.write_text("# 1104 — A spec\n\n**Date:** 2026-09-20\n", encoding="utf-8")
    found = guard.status_findings(path)
    assert len(found) == 1
    assert "no `**Status:**` field" in found[0]


def test_two_status_fields_are_a_finding(tmp_path):
    path = tmp_path / NAME
    path.write_text(
        "# 1104 — A spec\n\n**Status:** written before the work\n\n"
        "## Amendment\n\n**Status:** **retrospective**\n", encoding="utf-8")
    found = guard.status_findings(path)
    assert len(found) == 1
    assert "2 `**Status:**` fields" in found[0]


def test_the_name_is_checked_too(tmp_path):
    found = guard.status_findings(
        spec(tmp_path, "written before the work", name="1104-specs-and-plans.md"))
    assert len(found) == 1
    assert "<date>_<issue padded to four>_<kebab slug>" in found[0]


def test_a_second_spec_on_one_issue_may_carry_a_letter(tmp_path):
    # 0122b and 0134a are the two in the tree.
    assert guard.status_findings(
        spec(tmp_path, "written before the work",
             name="2026-08-14_0122b_prefix-space-per-piece.md")) == []


def tree(tmp_path, monkeypatch, files: dict[str, str]):
    """A repository whose git file set is exactly `files`."""
    for name, body in files.items():
        path = tmp_path / name
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    monkeypatch.setattr(guard, "tracked_files", lambda: sorted(files))


def test_a_committed_plan_is_a_finding_and_nothing_else_is_read(tmp_path, monkeypatch):
    tree(tmp_path, monkeypatch, {
        guard.PLANS + "2026-09-05_0442_lodestar-stats.md": "- [ ] Task 1\n",
        guard.SPECS + NAME: "# 1104\n\nno status at all\n",
    })
    found = guard.findings()
    assert len(found) == 1
    assert "1 file(s)" in found[0]
    assert "2026-09-05_0442_lodestar-stats.md" in found[0]


def test_a_plan_git_does_not_see_is_not_this_guard_s_business(tmp_path, monkeypatch):
    # CONTRIBUTING.md sends a plan to `.git/info/exclude`, so an ignored file under
    # plans/ is the rule followed, not broken -- which is why git's file set is read.
    ignored = tmp_path / guard.PLANS / "2026-09-20_1104_a-live-plan.md"
    ignored.parent.mkdir(parents=True, exist_ok=True)
    ignored.write_text("- [ ] Task 1\n", encoding="utf-8")
    tree(tmp_path, monkeypatch, {guard.SPECS + NAME: spec_body("written before the work")})
    assert guard.findings() == []


def test_an_empty_specs_directory_is_a_finding(tmp_path, monkeypatch):
    tree(tmp_path, monkeypatch, {"README.md": "# nothing here\n"})
    assert len(guard.findings()) == 1


def test_the_repository_passes_its_own_guard():
    assert guard.findings() == []


def test_a_status_quoted_in_a_fenced_block_is_not_a_second_field(tmp_path, monkeypatch):
    # A spec that documents the rule shows the field; that is prose about a status,
    # not a status, and counting it would refuse the one spec explaining the vocabulary.
    path = tmp_path / NAME
    path.write_text(
        spec_body("written before the work")
        + "\n```markdown\n**Status:** accepted, 2026-09-16. Written before the work.\n```\n",
        encoding="utf-8")
    assert guard.status_findings(path) == []


def test_a_status_that_wraps_onto_the_next_line_is_read(tmp_path, monkeypatch):
    # The metadata paragraph wraps in this tree, and the wrap can fall right after
    # the field. Reporting that as "no field" sends the author to add a second one.
    path = tmp_path / NAME
    path.write_text(
        "# 1104 — A spec\n\n**Issue:** [#1104](https://example.invalid/1104) · **Status:**\n"
        "written before the work · **Date:** 2026-09-20\n", encoding="utf-8")
    assert guard.status_findings(path) == []
