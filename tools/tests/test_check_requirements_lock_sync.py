"""check_requirements_lock_sync.py's guard on the txt-to-lock drift #604 met.

#604 added datasketch and simhash to tools/requirements.txt and not to the lock. CI
installs from the lock, so `Oracles are reproducible` stopped on ModuleNotFoundError
while every local check had passed -- the developer venv already held both.

The first test below is the one that would have caught it. The rest cover what the
guard must *not* report: a transitive pin the lock carries and requirements.txt does
not, and a name the two files spell differently, which PEP 503 says is one name.
"""

from __future__ import annotations

import sys
from pathlib import Path

import pytest

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_requirements_lock_sync as guard  # noqa: E402


def _files(monkeypatch, tmp_path, requirements: str, lock: str):
    """Points the guard at a pair of files written for one case."""
    requirements_path = tmp_path / "requirements.txt"
    lock_path = tmp_path / "requirements.lock.txt"
    requirements_path.write_text(requirements, encoding="utf-8")
    lock_path.write_text(lock, encoding="utf-8")
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    monkeypatch.setattr(guard, "REQUIREMENTS", requirements_path)
    monkeypatch.setattr(guard, "LOCK", lock_path)


HASHED = """\
alpha==1.0.0 \\
    --hash=sha256:0000000000000000000000000000000000000000000000000000000000000000
"""


def test_a_pin_missing_from_the_lock_is_reported(monkeypatch, tmp_path):
    """The #604 case: requirements.txt gained a pin and the lock never heard of it."""
    _files(monkeypatch, tmp_path, "alpha==1.0.0\nbeta==2.0.0\n", HASHED)

    found = guard.findings()

    assert len(found) == 1
    assert "beta" in found[0]
    assert "2.0.0" in found[0]


def test_a_version_the_two_files_disagree_on_is_reported(monkeypatch, tmp_path):
    _files(monkeypatch, tmp_path, "alpha==1.1.0\n", HASHED)

    found = guard.findings()

    assert len(found) == 1
    assert "1.0.0" in found[0] and "1.1.0" in found[0]


def test_an_agreeing_pair_reports_nothing(monkeypatch, tmp_path):
    _files(monkeypatch, tmp_path, "alpha==1.0.0\n", HASHED)

    assert guard.findings() == []


def test_a_transitive_pin_the_lock_adds_is_not_reported(monkeypatch, tmp_path):
    """The lock holds far more than requirements.txt names, and that is why it exists."""
    lock = HASHED + """\
transitive==9.9.9 \\
    --hash=sha256:1111111111111111111111111111111111111111111111111111111111111111
    # via alpha
"""
    _files(monkeypatch, tmp_path, "alpha==1.0.0\n", lock)

    assert guard.findings() == []


def test_a_name_spelled_two_ways_is_one_name(monkeypatch, tmp_path):
    """pip-compile writes rake_nltk where requirements.txt says rake-nltk (PEP 503)."""
    lock = """\
rake_nltk==1.0.6 \\
    --hash=sha256:2222222222222222222222222222222222222222222222222222222222222222
"""
    _files(monkeypatch, tmp_path, "rake-nltk==1.0.6\n", lock)

    assert guard.findings() == []


def test_a_comment_naming_a_package_is_not_a_pin(monkeypatch, tmp_path):
    """requirements.txt carries prose above most pins, and it names versions."""
    requirements = """\
# beta==2.0.0 was considered and refused; see the issue.
alpha==1.0.0
"""
    _files(monkeypatch, tmp_path, requirements, HASHED)

    assert guard.findings() == []


def test_a_lock_continuation_line_is_not_read_as_a_pin(monkeypatch, tmp_path):
    """A hash line is indented, so an anchored match cannot mistake it for a pin."""
    _files(monkeypatch, tmp_path, "alpha==1.0.0\n", HASHED)

    assert guard.read_pins(guard.LOCK, guard.LOCK_PIN) == {"alpha": "1.0.0"}


def test_an_unpinned_dependency_is_reported_rather_than_skipped(monkeypatch, tmp_path):
    """A range cannot be compared against a lock, so it is named instead of ignored."""
    _files(monkeypatch, tmp_path, "alpha==1.0.0\nloose>=3\n", HASHED)

    found = guard.findings()

    assert any("not pinned" in finding for finding in found)


def test_an_include_line_is_not_an_unpinned_dependency(monkeypatch, tmp_path):
    _files(monkeypatch, tmp_path, "-r other.txt\nalpha==1.0.0\n", HASHED)

    assert guard.findings() == []


def test_a_missing_file_is_reported_before_anything_else(monkeypatch, tmp_path):
    requirements_path = tmp_path / "requirements.txt"
    requirements_path.write_text("alpha==1.0.0\n", encoding="utf-8")
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    monkeypatch.setattr(guard, "REQUIREMENTS", requirements_path)
    monkeypatch.setattr(guard, "LOCK", tmp_path / "absent.txt")

    found = guard.findings()

    assert len(found) == 1
    assert "missing" in found[0]


def test_the_repository_itself_is_in_sync():
    """The guard's own subject, which is what CI runs it for."""
    assert guard.findings() == []


@pytest.mark.parametrize("argument", ["--help", "-h"])
def test_help_prints_and_exits_zero(argument, monkeypatch, capsys):
    monkeypatch.setattr(sys, "argv", ["check_requirements_lock_sync.py", argument])

    assert guard.main() == 0
    assert "requirements.lock.txt" in capsys.readouterr().out


def test_an_unexpected_argument_exits_two(monkeypatch):
    monkeypatch.setattr(sys, "argv", ["check_requirements_lock_sync.py", "--wat"])

    assert guard.main() == 2
