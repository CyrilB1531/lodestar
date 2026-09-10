"""check_claude_md_packages.py's guard on CLAUDE.md's architecture table.

#597: the section opened with "Eight independently versioned packages" and listed eight
while src/ held fifteen, and the sentence after it named four inter-package edges where
check_nuspec_dependencies.py's EXPECTED carried ten. CLAUDE.md is the file a new session
reads to learn what exists, so it is the worst place for a stale enumeration.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_claude_md_packages as guard  # noqa: E402

ROOT = Path(__file__).resolve().parents[2]

HEADER = "| Package | Tier | Holds |\n| --- | --- | --- |\n"


def _claude(monkeypatch, tmp_path, count_word, rows, edge_word="two"):
    body = (
        f"{count_word} independently versioned packages under `src/`.\n\n"
        + HEADER
        + "".join(f"| `{n}` | core | what it holds. |\n" for n in rows)
        + f"\nThe edges: **{edge_word}**, all asserted per target framework.\n"
    )
    path = tmp_path / "CLAUDE.md"
    path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(guard, "CLAUDE", path)
    return path


def test_the_shipped_claude_md_matches_the_tree():
    assert guard.findings() == []


def test_a_package_in_src_and_not_in_the_table_is_a_finding(monkeypatch, tmp_path):
    monkeypatch.setattr(guard, "source_packages", lambda: {"Lodestar.Text", "Lodestar.Survival"})
    monkeypatch.setattr(guard, "expected_edges", lambda: 2)
    _claude(monkeypatch, tmp_path, "Two", ["Lodestar.Text"])

    found = guard.findings()

    assert any("no row for Lodestar.Survival" in f for f in found)


def test_a_table_row_for_a_package_src_does_not_hold_is_a_finding(monkeypatch, tmp_path):
    monkeypatch.setattr(guard, "source_packages", lambda: {"Lodestar.Text"})
    monkeypatch.setattr(guard, "expected_edges", lambda: 2)
    _claude(monkeypatch, tmp_path, "One", ["Lodestar.Text", "Lodestar.Ghost"])

    found = guard.findings()

    assert any("lists Lodestar.Ghost" in f for f in found)


def test_the_prose_count_must_match_its_own_table(monkeypatch, tmp_path):
    # The two drifted apart once already, in the other direction: a table that grew
    # while the sentence above it did not.
    monkeypatch.setattr(guard, "source_packages", lambda: {"Lodestar.Text", "Lodestar.Fuzzy"})
    monkeypatch.setattr(guard, "expected_edges", lambda: 2)
    _claude(monkeypatch, tmp_path, "Eight", ["Lodestar.Text", "Lodestar.Fuzzy"])

    found = guard.findings()

    assert any("the prose says Eight packages" in f for f in found)


def test_the_edge_count_is_checked_against_the_nuspec_gate(monkeypatch, tmp_path):
    monkeypatch.setattr(guard, "source_packages", lambda: {"Lodestar.Text"})
    monkeypatch.setattr(guard, "expected_edges", lambda: 10)
    _claude(monkeypatch, tmp_path, "One", ["Lodestar.Text"], edge_word="four")

    found = guard.findings()

    assert any("says four inter-package edges" in f and "carries 10" in f for f in found)


def test_the_edge_count_comes_from_expected_and_not_from_a_constant():
    # The number in CLAUDE.md is only worth checking if its source is the map the
    # nuspec gate already asserts against the packed .nuspec.
    assert guard.expected_edges() > 0
