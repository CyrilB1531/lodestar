"""check_adr_frontmatter.py: what a record has to declare before the index believes it.

Four keys, all required, empty lists included. That last part is the one worth a
test of its own: an absent key and an empty list read the same to a generator and
not at all the same to a reader -- one is a record that declared nothing, the
other a record that declared no relation. Requiring all four is what makes `[]` a
statement rather than a default.

The `status` assertion is the other half. The frontmatter repeats a fact the
`**Status:**` line below it already carries, which is how a fact drifts, so the
rule is that the line opens with the word: `0013` reads "accepted, superseded in
part by `0014`", whose first word is what its block declares.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_adr_frontmatter as guard  # noqa: E402

KNOWN = {"0001", "0002"}


def write(tmp_path: Path, name: str, body: str) -> Path:
    path = tmp_path / name
    path.write_text(body, encoding="utf-8")
    return path


def record(lines: str, *, number: str = "0002", status: str = "accepted") -> str:
    return (
        "---\n" + lines + "---\n"
        f"# {number} — Record {number}\n\n"
        f"**Status:** {status} · **Date:** 2026-09-11\n"
    )


GOOD = """\
status: accepted
supersedes: []
amends: ["0001"]
applies: []
"""


def test_a_complete_record_is_clean(tmp_path):
    path = write(tmp_path, "0002-b.md", record(GOOD))

    assert guard.findings_for(path, KNOWN) == []


def test_a_record_with_no_frontmatter_at_all_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", "# 0002 — Record 0002\n\n**Status:** accepted\n")

    findings = guard.findings_for(path, KNOWN)

    assert len(findings) == 1
    assert "no YAML frontmatter" in findings[0]


def test_an_empty_list_is_accepted_and_a_missing_key_is_not(tmp_path):
    complete = write(tmp_path, "0002-b.md", record(
        'status: accepted\nsupersedes: []\namends: []\napplies: []\n'))
    partial = write(tmp_path, "0001-a.md", record(
        'status: accepted\nsupersedes: []\namends: []\n', number="0001"))

    assert guard.findings_for(complete, KNOWN) == []
    findings = guard.findings_for(partial, KNOWN)
    assert len(findings) == 1
    assert "declares no `applies`" in findings[0]


def test_a_key_outside_the_four_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", record(GOOD + "relates_to: []\n"))

    findings = guard.findings_for(path, KNOWN)

    assert any("`relates_to`" in finding for finding in findings)


def test_a_status_that_disagrees_with_the_status_line_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", record(
        'status: superseded\nsupersedes: []\namends: []\napplies: []\n'))

    findings = guard.findings_for(path, KNOWN)

    assert any("reads 'accepted" in finding for finding in findings)


def test_a_status_line_that_carries_more_than_the_word_still_agrees(tmp_path):
    # 0013's shape: "accepted, superseded in part by `0014`".
    path = write(tmp_path, "0002-b.md", record(
        'status: accepted\nsupersedes: []\namends: []\napplies: []\n',
        status="accepted, superseded in part by `0001`"))

    assert guard.findings_for(path, KNOWN) == []


def test_a_relation_onto_a_record_that_does_not_exist_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", record(
        'status: accepted\nsupersedes: []\namends: ["0999"]\napplies: []\n'))

    findings = guard.findings_for(path, KNOWN)

    assert any("0999" in finding and "no docs/decisions/0999-" in finding
               for finding in findings)


def test_a_record_naming_itself_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", record(
        'status: accepted\nsupersedes: []\namends: ["0002"]\napplies: []\n'))

    findings = guard.findings_for(path, KNOWN)

    assert any("which is itself" in finding for finding in findings)


def test_the_same_decision_named_twice_is_refused(tmp_path):
    path = write(tmp_path, "0002-b.md", record(
        'status: accepted\nsupersedes: []\namends: ["0001", "0001"]\napplies: []\n'))

    findings = guard.findings_for(path, KNOWN)

    assert any("twice" in finding for finding in findings)


def test_an_unreadable_list_is_reported_rather_than_raised(tmp_path):
    path = write(tmp_path, "0002-b.md", record(
        "status: accepted\nsupersedes: []\namends: 0001\napplies: []\n"))

    findings = guard.findings_for(path, KNOWN)

    assert len(findings) == 1
    assert "amends" in findings[0]


def test_the_real_corpus_is_clean():
    # The repository's own records, which is the only corpus that matters here.
    assert guard.findings() == []
