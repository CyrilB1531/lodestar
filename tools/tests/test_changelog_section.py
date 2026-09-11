"""changelog_section.py, which turns a changelog entry into a GitHub Release body.

#626: 35 release tags and 4 GitHub Releases, because `release.yml` never made one. The notes
were already written -- CHANGELOG.md carries a `### <Package> — <Version>` heading per release
-- so the fix is extraction rather than authorship, and this is what keeps the extraction honest
about its boundaries.

The two that matter are the ends of a section. `#### Added` belongs to the section and a `###`
heading ends it; getting that backwards either truncates every release body at its first
sub-heading or runs one release's notes into the next one's.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import changelog_section  # noqa: E402

REPO = Path(__file__).resolve().parents[2]

CHANGELOG = """\
# Changelog

## Released — 2026-09-10

### Lodestar.Gpu — 0.1.0

#### Added

- A sixteenth package.

#### Fixed

- Something else.

### Lodestar.Text — 0.6.0

#### Added

- Double metaphone.

## Released — 2026-09-01

### DataNet.Text — 0.3.0

#### Added

- The name before the rename.
"""


def _changelog(monkeypatch, tmp_path, body=CHANGELOG):
    path = tmp_path / "CHANGELOG.md"
    path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(changelog_section, "CHANGELOG", path)
    return path


def test_the_shipped_changelog_carries_a_section_for_every_heading_it_declares():
    found = changelog_section.sections()
    assert found, "the shipped CHANGELOG.md carries no release section at all"
    assert all(body for body in found.values()), "a shipped section is empty"


def test_a_section_keeps_its_own_sub_headings(monkeypatch, tmp_path):
    # `#### Added` is part of the release, not a boundary. A regex that ended a section at any
    # heading would truncate every body at its first sub-heading.
    _changelog(monkeypatch, tmp_path)

    body = changelog_section.sections()[("Lodestar.Gpu", "0.1.0")]

    assert "#### Added" in body
    assert "#### Fixed" in body
    assert "Something else." in body


def test_a_section_stops_at_the_next_release(monkeypatch, tmp_path):
    # The other end: running one release's notes into the next is how a Release body ends up
    # claiming work it did not ship.
    _changelog(monkeypatch, tmp_path)

    body = changelog_section.sections()[("Lodestar.Gpu", "0.1.0")]

    assert "Double metaphone" not in body
    assert "Lodestar.Text" not in body


def test_a_section_stops_at_the_next_dated_block(monkeypatch, tmp_path):
    # `## Released — <date>` is above `###` and ends a section too.
    _changelog(monkeypatch, tmp_path)

    body = changelog_section.sections()[("Lodestar.Text", "0.6.0")]

    assert "Double metaphone" in body
    assert "2026-09-01" not in body
    assert "before the rename" not in body


def test_the_pre_rename_name_is_read_too(monkeypatch, tmp_path):
    # Four release tags are `DataNet.*`, from before the rename. They are part of the record.
    _changelog(monkeypatch, tmp_path)

    assert ("DataNet.Text", "0.3.0") in changelog_section.sections()


def test_a_missing_section_is_an_error_rather_than_an_empty_body(monkeypatch, tmp_path, capsys):
    # CONTRIBUTING.md's item 7 says nothing gates the changelog entry, "precisely because four
    # lots shipped without it". A Release with a blank body is that silence one step downstream.
    _changelog(monkeypatch, tmp_path)
    monkeypatch.setattr(sys, "argv", ["changelog_section.py", "Lodestar.Text", "9.9.9"])

    assert changelog_section.main() == 1
    assert "no `### Lodestar.Text — 9.9.9` section" in capsys.readouterr().err


def test_list_prints_every_release_the_changelog_carries(monkeypatch, tmp_path, capsys):
    _changelog(monkeypatch, tmp_path)
    monkeypatch.setattr(sys, "argv", ["changelog_section.py", "--list"])

    assert changelog_section.main() == 0
    lines = capsys.readouterr().out.split()

    assert "Lodestar.Gpu" in lines
    assert "DataNet.Text" in lines


def test_bad_usage_exits_two(monkeypatch, tmp_path, capsys):
    _changelog(monkeypatch, tmp_path)
    monkeypatch.setattr(sys, "argv", ["changelog_section.py", "Lodestar.Gpu"])

    assert changelog_section.main() == 2
    capsys.readouterr()


def test_help_exits_zero(monkeypatch, capsys):
    monkeypatch.setattr(sys, "argv", ["changelog_section.py", "--help"])

    assert changelog_section.main() == 0
    capsys.readouterr()


def test_every_lodestar_release_tag_has_a_section_to_publish():
    """The tags are the release record; a tag with no section cannot get a Release body.

    This is the coupling #626 exists to close, asserted rather than remembered: a tag pushed
    without its changelog entry is a release the workflow will now refuse.
    """
    import subprocess

    tags = subprocess.run(
        ["git", "tag", "--list", "Lodestar.*", "DataNet.*"],
        cwd=REPO, capture_output=True, text=True, check=True).stdout.split()
    found = changelog_section.sections()

    missing = [t for t in tags if tuple(t.rsplit("/v", 1)) not in found]

    assert not missing, (
        f"{len(missing)} release tags have no `### <Package> — <Version>` section in "
        f"CHANGELOG.md, so they cannot be published with notes: {sorted(missing)}")
