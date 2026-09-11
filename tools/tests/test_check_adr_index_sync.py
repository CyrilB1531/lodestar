"""check_adr_index_sync.py: a generated file nobody regenerates is worse than none.

It is believed, which is the whole problem. `#586`, `#597` and `#610` were each a
hand-maintained copy of something derivable, true when written, and each was found
by the thing it was supposed to prevent. The index is the same shape of risk with
a worse payload: what it holds is the only forward pointer from an amended
decision to its amendment, because the amended record cannot carry one itself.

So the comparison is the whole text rather than a parse. The emitter is
deterministic -- `test_adr_index.py` asserts that -- so any difference at all
means the file was hand-edited or a record landed without a regeneration.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import adr_index  # noqa: E402
import check_adr_index_sync as guard  # noqa: E402
import regen_adr_index  # noqa: E402


def test_the_committed_index_is_in_sync():
    assert guard.findings() == []


def test_the_generator_reports_the_same_thing():
    assert regen_adr_index.main(["prog", "--check"]) == 0


def test_a_missing_index_is_refused(monkeypatch, tmp_path):
    monkeypatch.setattr(adr_index, "INDEX", tmp_path / "index.yaml")

    findings = guard.findings()

    assert len(findings) == 1
    assert "missing" in findings[0]


def test_a_hand_edited_index_is_refused_with_the_lines_that_differ(monkeypatch, tmp_path):
    stale = tmp_path / "index.yaml"
    current = adr_index.generate()
    stale.write_text(current.replace('amended_by: ["0103"]', "amended_by: []", 1),
                     encoding="utf-8")
    monkeypatch.setattr(adr_index, "INDEX", stale)

    findings = guard.findings()

    assert findings
    assert "has drifted" in findings[0]
    assert any('amended_by: ["0103"]' in finding for finding in findings)
    assert any("regen_adr_index.py" in finding for finding in findings)


def test_a_wholesale_rewrite_is_reported_without_printing_the_whole_file(
        monkeypatch, tmp_path):
    stale = tmp_path / "index.yaml"
    stale.write_text("# nothing like it\n", encoding="utf-8")
    monkeypatch.setattr(adr_index, "INDEX", stale)

    findings = guard.findings()

    assert len(findings) <= guard.MAX_DIFF_LINES + 3
    assert any("further diff line" in finding for finding in findings)


def test_a_record_the_index_cannot_be_generated_from_names_the_other_guard(
        monkeypatch, tmp_path):
    broken = tmp_path / "decisions"
    broken.mkdir()
    (broken / "0001-a.md").write_text("---\namends: oops\n---\n# 0001 — A\n", encoding="utf-8")
    monkeypatch.setattr(adr_index, "DECISIONS", broken)

    findings = guard.findings()

    assert any("check_adr_frontmatter.py" in finding for finding in findings)


def test_the_generator_writes_and_then_reports_current(monkeypatch, tmp_path):
    monkeypatch.setattr(adr_index, "INDEX", tmp_path / "index.yaml")

    assert regen_adr_index.main(["prog"]) == 0
    assert regen_adr_index.main(["prog", "--check"]) == 0
    assert (tmp_path / "index.yaml").read_text(encoding="utf-8") == adr_index.generate()


def test_the_generator_refuses_an_argument_it_does_not_know():
    assert regen_adr_index.main(["prog", "--force"]) == 2
    assert regen_adr_index.main(["prog", "--help"]) == 0
