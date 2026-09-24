"""Tests for tools/check_pr_closes.py (#1152).

The REST lookup is replaced by a table, so these run offline; the two closed issues are the
cases that motivated the guard, #785 closing #617 and #766 closing #568.
"""

from __future__ import annotations

import io
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_pr_closes as guard  # noqa: E402

REPO = "CyrilB1531/lodestar"

AUTHOR = "CyrilB1531"
MINE = [{"login": "cyrilb1531"}]

ISSUES = {
    1152: {"state": "open", "locked": False, "assignees": MINE},
    1160: {"state": "open", "locked": False, "assignees": [{"login": "someone-else"}]},
    1161: {"state": "open", "locked": False, "assignees": []},
    617: {"state": "closed", "state_reason": "completed", "closed_at": "2026-09-12T13:50:45Z"},
    568: {"state": "closed", "state_reason": "not_planned", "closed_at": "2026-09-09T21:11:55Z"},
    900: {"state": "open", "locked": True, "assignees": MINE},
    1151: {"state": "closed", "pull_request": {"url": "…"}},
}


def lookup(repo, number):
    assert repo == REPO, "a target in another repository is never looked up"
    return ISSUES.get(number)


def run(body, author=AUTHOR, labels=frozenset()):
    return guard.findings(body, REPO, author, labels, lookup)


def test_an_open_issue_passes():
    assert run("Closes #1152.") == []


def test_a_closed_issue_fails_with_its_reason_and_date():
    assert run("Closes #617") == [
        "::error::Closes #617, but #617 is closed (completed, 2026-09-12). "
        "Reopen it if the work is unfinished, or write Refs #617."]


def test_a_missing_issue_fails():
    (line,) = run("Fixes #99999")
    assert "#99999 does not exist" in line
    assert "or write Refs #99999." in line


def test_refs_is_not_read():
    assert run("Closes #1152. Refs #617, and see #568.") == []


def test_a_body_closing_no_issue_fails():
    assert run("A change with no issue.") == [guard.NO_CLOSE]


def test_refs_alone_does_not_count_as_closing():
    assert run("Refs #1152") == [guard.NO_CLOSE]


def test_an_issue_assigned_to_someone_else_fails():
    assert run("Closes #1160") == [
        f"::error::Closes #1160, but #1160 is not assigned to {AUTHOR}. Ask a maintainer to "
        f"assign it, or write Refs #1160 if you are not the one finishing it."]


def test_an_issue_with_no_assignee_fails():
    (line,) = run("Closes #1161")
    assert f"#1161 is not assigned to {AUTHOR} (it has no assignee)" in line


def test_the_assignee_is_compared_without_regard_to_case():
    assert run("Closes #1152", author="CYRILB1531") == []


def test_a_bot_author_needs_no_issue_nor_an_assignment():
    for bot in ("dependabot[bot]", "github-actions[bot]", "renovate[bot]"):
        assert run("Bump a dependency.", author=bot) == [], bot
        assert run("Closes #1160", author=bot) == [], bot


def test_a_bot_author_still_may_not_close_a_closed_issue():
    (line,) = run("Closes #617", author="github-actions[bot]")
    assert "#617 is closed" in line


def test_the_no_issue_label_waives_closing_and_assigning_only():
    labels = frozenset({"no-issue"})
    assert run("Refs #1142", labels=labels) == []
    assert run("Closes #1160", labels=labels) == []
    (line,) = run("Closes #617", labels=labels)
    assert "#617 is closed" in line


def test_another_bot_is_not_exempt():
    assert run("Nothing to close.", author="some-app[bot]") == [guard.NO_CLOSE]


def test_each_broken_target_gets_its_own_line():
    lines = run("Closes #1152\nCloses #617\nResolves #568\nFixes #99999")

    assert len(lines) == 3
    assert "#617 is closed (completed" in lines[0]
    assert "#568 is closed (not planned" in lines[1]
    assert "#99999 does not exist" in lines[2]


def test_every_keyword_is_read_in_any_case_with_an_optional_colon():
    for keyword in ("close", "CLOSES", "Closed", "fix", "Fixes", "fixed",
                    "resolve", "Resolves", "RESOLVED", "Closes:"):
        assert len(run(f"{keyword} #617")) == 1, keyword


def test_a_url_and_an_owner_repo_reference_are_read():
    assert len(run(f"Closes https://github.com/{REPO}/issues/617")) == 1
    assert len(run(f"Closes {REPO}#617")) == 1
    assert run(f"Closes https://github.com/{REPO}/issues/1152") == []


def test_one_issue_named_twice_is_reported_once():
    assert len(run(f"Closes #617, and fixes {REPO}#617")) == 1


def test_an_issue_in_another_repository_fails_without_a_lookup():
    (line,) = run("Closes octo/other#5")
    assert f"octo/other#5 is in octo/other, not in {REPO}" in line


def test_a_pull_request_fails():
    (line,) = run("Closes #1151")
    assert "#1151 is a pull request, not an issue" in line
    (line,) = run(f"Closes https://github.com/{REPO}/pull/1151")
    assert "is a pull request" in line


def test_a_locked_issue_fails():
    (line,) = run("Closes #900")
    assert "#900 is locked" in line


def test_a_keyword_in_code_or_a_comment_is_not_read():
    body = ("Closes #1152. Write `Closes #617` in the body.\n```text\nCloses #568\n```\n"
            "<!-- Closes #617 -->")
    assert run(body) == []


def test_a_word_containing_a_keyword_is_not_read():
    assert run("Closes #1152. It encloses #617 and prefixes #568.") == []


def test_main_reads_the_body_from_stdin(monkeypatch, capsys):
    monkeypatch.setattr(guard, "fetch", lookup)
    monkeypatch.setattr(sys, "stdin", io.StringIO("Closes #617"))

    assert guard.main(["check_pr_closes.py", "--repo", REPO, "--author", AUTHOR]) == 1
    assert "#617 is closed" in capsys.readouterr().out


def test_main_reads_the_author_and_labels_from_the_environment(monkeypatch, capsys):
    monkeypatch.setattr(guard, "fetch", lookup)
    monkeypatch.setattr(sys, "stdin", io.StringIO("No issue here."))
    monkeypatch.setenv("GITHUB_REPOSITORY", REPO)
    monkeypatch.setenv("PR_AUTHOR", AUTHOR)
    monkeypatch.setenv("PR_LABELS", "bench,no-issue")

    assert guard.main(["check_pr_closes.py"]) == 0
    assert "the no-issue label is set" in capsys.readouterr().out


def test_main_takes_no_path():
    assert guard.main(["check_pr_closes.py", "--repo", REPO, "--author", AUTHOR, "body.md"]) == 2


def test_main_requires_an_author(monkeypatch):
    monkeypatch.delenv("PR_AUTHOR", raising=False)

    assert guard.main(["check_pr_closes.py", "--repo", REPO]) == 2


def test_main_refuses_a_missing_repository(monkeypatch, capsys):
    monkeypatch.delenv("GITHUB_REPOSITORY", raising=False)

    assert guard.main(["check_pr_closes.py"]) == 2
