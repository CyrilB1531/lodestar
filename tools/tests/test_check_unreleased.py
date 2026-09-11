"""check_unreleased.py, which reads what each package has merged and not published.

#628: `Lodestar.Fuzzy` carried two commits merged on 2026-09-01 and never released, and
nothing said so for ten days. The Milestones page can answer that question, but only while
somebody keeps it current -- this reads the tags and `main`, which cannot drift.

The line between reporting and failing is the whole design, and it was drawn from a measured
case: `701cd987` touches six files under `src/Lodestar.Metrics` and changes nothing but XML
comments. Unpublished work that owes the changelog nothing. So a missing entry is a notice,
and only a version declared past its own tag is a failure.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_unreleased  # noqa: E402

REPO = Path(__file__).resolve().parents[2]


def rows(*specs):
    """(package, declared, tag, unpublished) tuples, the shape survey() returns."""
    return list(specs)


def test_a_version_ahead_of_its_tag_with_nothing_to_publish_fails():
    # A release prepared and then never tagged. No innocent reading: the number is declared,
    # the tag does not exist, and nothing is waiting that would explain the bump.
    found = check_unreleased.findings(rows(("Lodestar.Text", "0.7.0", "Lodestar.Text/v0.6.0", 0)))

    assert len(found) == 1
    assert "never tagged" in found[0]


def test_a_version_equal_to_its_tag_with_commits_waiting_is_not_a_fault():
    # The normal state between a merge and a release. Failing here would demand a version
    # bump per pull request, which is the opposite of what per-package versioning is for.
    assert check_unreleased.findings(
        rows(("Lodestar.Fuzzy", "0.4.0", "Lodestar.Fuzzy/v0.4.0", 2))) == []


def test_a_version_ahead_with_commits_waiting_is_not_a_fault():
    # A release in preparation: the bump is made, the entries are being written, the tag
    # comes last. Reporting that as an error would fail every release branch.
    assert check_unreleased.findings(
        rows(("Lodestar.Fuzzy", "0.5.0", "Lodestar.Fuzzy/v0.4.0", 2))) == []


def test_a_missing_changelog_entry_is_a_notice_rather_than_a_failure(monkeypatch):
    monkeypatch.setattr(check_unreleased, "unreleased_entries", lambda: set())
    waiting = rows(("Lodestar.Fuzzy", "0.4.0", "Lodestar.Fuzzy/v0.4.0", 2))

    assert check_unreleased.findings(waiting) == []
    notices = check_unreleased.notices(waiting)

    assert len(notices) == 1
    assert notices[0].startswith("note ")


def test_a_package_named_under_unreleased_draws_no_notice(monkeypatch):
    monkeypatch.setattr(check_unreleased, "unreleased_entries", lambda: {"Lodestar.Fuzzy"})

    assert check_unreleased.notices(
        rows(("Lodestar.Fuzzy", "0.4.0", "Lodestar.Fuzzy/v0.4.0", 2))) == []


def test_versions_compare_numerically_not_as_strings():
    # `0.10.0` is above `0.9.0`; a string comparison puts it below, which would report a
    # released package as never tagged.
    assert check_unreleased.as_tuple("0.10.0") > check_unreleased.as_tuple("0.9.0")
    assert check_unreleased.findings(
        rows(("Lodestar.Text", "0.9.0", "Lodestar.Text/v0.10.0", 0))) == []


def test_the_latest_tag_is_the_highest_version_not_the_last_string():
    tag = check_unreleased.latest_tag("Lodestar.Text")

    assert tag.startswith("Lodestar.Text/v"), f"no tag found for Lodestar.Text: {tag!r}"


def test_the_shipped_tree_declares_no_version_past_its_own_tag():
    """The one rule that fails, asserted against the repository as it stands."""
    assert check_unreleased.findings(check_unreleased.survey()) == []


def test_the_survey_covers_every_package():
    surveyed = {row[0] for row in check_unreleased.survey()}
    expected = {p.name for p in (REPO / "src").glob("Lodestar.*") if (p / "Version.props").exists()}

    assert surveyed == expected
