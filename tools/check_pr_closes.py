#!/usr/bin/env python3
"""Refuse a pull request whose closing keyword targets an issue that is not open (#1152).

A closing keyword promises that merging the pull request finishes an issue, and nothing
checked that the issue was still there to finish. Of the 200 pull requests merged before
2026-09-24, two closed an issue already closed when they opened: #785 wrote ``Closes #617``
three days after #617 closed, and #766 wrote ``Closes #568`` five days after #568 did. Both
landed with no open issue tracking the work.

Every closing keyword GitHub honours is read, case-insensitive and with an optional colon --
close, closes, closed, fix, fixes, fixed, resolve, resolves, resolved -- followed by ``#N``,
``owner/repo#N`` or an issue URL. Each target must be an **open issue of this repository**.
A target fails when it is closed, missing, a pull request, locked, or in another repository.

``Refs #N`` is not read: it links without promising a close, and #1148 and #1151 used it on
purpose for an issue closed by then. Code spans, fenced blocks and HTML comments are not read
either, since GitHub does not act on a keyword there, and a body quoting ``Closes #12`` as an
example must pass.

Two rules follow from the same promise. **A pull request closes an issue**: a body with no
closing keyword fails, and ``Refs`` alone does not count. **The issue is assigned to the pull
request's author**: an issue with another assignee, or none, fails. Both are waived for a bot
author in BOTS -- Dependabot, and bench-nightly.yml's github-actions[bot] -- and for a pull
request carrying the ``no-issue`` label, which only a maintainer can set. The first rule, that
a closing keyword names an open issue here, is never waived.

Usage:  python tools/check_pr_closes.py [--repo OWNER/NAME] [--author LOGIN] [--labels A,B] < BODY

The body is read from stdin only, never from a path an argument names. Each option falls back
to $GITHUB_REPOSITORY, $PR_AUTHOR and $PR_LABELS; the repository and the author are required.
$GH_TOKEN or $GITHUB_TOKEN, when set, authenticates the REST calls; without one GitHub allows
60 an hour, enough by hand.

Exit:   0 every target is an open issue here, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import json
import os
import re
import sys
import urllib.error
import urllib.request
from dataclasses import dataclass
from typing import Callable

API = "https://api.github.com/repos/{repo}/issues/{number}"

# Authors whose pull requests nobody describes: Dependabot, Renovate, and bench-nightly.yml,
# which opens its pull request as github-actions[bot]. A closing keyword they write is checked.
BOTS = frozenset({"dependabot[bot]", "github-actions[bot]", "renovate[bot]"})

# The label a maintainer sets on a pull request that deliberately closes no issue.
NO_ISSUE = "no-issue"

NO_CLOSE = ("::error::This pull request closes no issue. Add Closes #N to its description, "
            f"or ask a maintainer for the {NO_ISSUE} label if no issue should track it.")

# The regular expression's group for an issue number written as #N.
NUMBER = "number"

# What GitHub does not act on: an HTML comment, a fenced block, an inline code span.
UNREAD = re.compile(r"<!--.*?-->|```.*?```|~~~.*?~~~|`[^`\n]*`", re.DOTALL)

KEYWORD = re.compile(
    r"\b(?P<keyword>close[sd]?|fix(?:e[sd])?|resolve[sd]?)\s*:?\s+"
    r"(?:(?P<url>https?://github\.com/(?P<url_repo>[\w.-]+/[\w.-]+)/"
    r"(?P<kind>issues|pull)/(?P<url_number>\d+))"
    r"|(?P<repo>[\w.-]+/[\w.-]+)?#(?P<number>\d+))\b",
    re.IGNORECASE)


@dataclass(frozen=True)
class Target:
    """One closing keyword and the issue it names, as written."""

    keyword: str
    written: str
    repo: str
    number: int
    is_pull_url: bool = False


def targets(body: str, repo: str) -> list[Target]:
    """Every closing keyword in the body, once per issue, in the order written."""
    found: dict[tuple[str, int], Target] = {}
    for match in KEYWORD.finditer(UNREAD.sub(" ", body)):
        if match.group("url"):
            target = Target(match.group("keyword"), match.group("url"), match.group("url_repo"),
                            int(match.group("url_number")), match.group("kind").lower() == "pull")
        else:
            named = match.group("repo")
            number = match.group(NUMBER)
            written = f"{named}#{number}" if named else f"#{number}"
            target = Target(match.group("keyword"), written, named or repo, int(number))
        found.setdefault((target.repo.lower(), target.number), target)
    return list(found.values())


def fetch(repo: str, number: int) -> dict | None:
    """The issue as the REST API returns it, or None when there is no such issue."""
    request = urllib.request.Request(API.format(repo=repo, number=number),
                                     headers={"Accept": "application/vnd.github+json"})
    token = os.environ.get("GH_TOKEN") or os.environ.get("GITHUB_TOKEN")
    if token:
        request.add_header("Authorization", f"Bearer {token}")
    try:
        with urllib.request.urlopen(request, timeout=30) as response:
            return json.load(response)
    except urllib.error.HTTPError as error:
        if error.code in (404, 410):
            return None
        raise SystemExit(f"GitHub answered {error.code} for {repo}#{number}") from error
    except urllib.error.URLError as error:
        raise SystemExit(f"could not reach GitHub: {error.reason}") from error


def problem(target: Target, repo: str, issue: dict | None) -> str | None:
    """Why the target is not an open issue of this repository, or None when it is."""
    if target.repo.lower() != repo.lower():
        return f"is in {target.repo}, not in {repo}"
    if target.is_pull_url or (issue is not None and issue.get("pull_request") is not None):
        return "is a pull request, not an issue"
    if issue is None:
        return "does not exist"
    if issue.get("state") != "open":
        reason = issue.get("state_reason") or "closed"
        closed = (issue.get("closed_at") or "")[:10]
        return f"is closed ({reason.replace('_', ' ')}, {closed})" if closed else "is closed"
    if issue.get("locked"):
        return "is locked"
    return None


def unassigned(issue: dict, author: str) -> str | None:
    """Why the author is not among the issue's assignees, or None when they are."""
    assignees = [person.get("login", "") for person in issue.get("assignees") or []]
    if not assignees:
        return f"is not assigned to {author} (it has no assignee)"
    if author.lower() not in (login.lower() for login in assignees):
        return f"is not assigned to {author}"
    return None


def exemption(author: str, labels: frozenset[str]) -> str | None:
    """Why the pull request need not close an issue assigned to its author, if it need not."""
    if author.lower() in BOTS:
        return f"{author} is a bot"
    if NO_ISSUE in labels:
        return f"the {NO_ISSUE} label is set"
    return None


def error(target: Target, why: str, advice: str, refs_when: str = "") -> str:
    """One finding, in the shape every rule shares."""
    return (f"::error::{target.keyword.capitalize()} {target.written}, but {target.written} "
            f"{why}. {advice}, or write Refs {target.written}{refs_when}.")


def findings(body: str, repo: str, author: str, labels: frozenset[str] = frozenset(),
             lookup: Callable[[str, int], dict | None] | None = None) -> list[str]:
    """One error line per rule a closing keyword, or its absence, breaks."""
    lookup = lookup or fetch
    exempt = exemption(author, labels)
    found = targets(body, repo)
    if not found and exempt is None:
        return [NO_CLOSE]

    lines = []
    for target in found:
        issue = lookup(repo, target.number) if target.repo.lower() == repo.lower() else None
        why = problem(target, repo, issue)
        if why is not None:
            advice = ("Reopen it if the work is unfinished" if why.startswith("is closed")
                      else f"Point it at an open issue of {repo}")
            lines.append(error(target, why, advice))
        elif exempt is None and issue is not None and (who := unassigned(issue, author)):
            lines.append(error(target, who, "Ask a maintainer to assign it",
                               " if you are not the one finishing it"))
    return lines


# Each option, and the environment variable it falls back to.
OPTIONS = {"--repo": "GITHUB_REPOSITORY", "--author": "PR_AUTHOR", "--labels": "PR_LABELS"}


def options(arguments: list[str]) -> tuple[str, str, frozenset[str]] | None:
    """The repository, the author and the labels; None on bad usage."""
    values = {option: os.environ.get(variable, "") for option, variable in OPTIONS.items()}
    while arguments:
        if len(arguments) < 2 or arguments[0] not in OPTIONS:
            return None
        values[arguments[0]] = arguments[1]
        arguments = arguments[2:]
    repo, author, labels = (values[option] for option in OPTIONS)
    if not re.fullmatch(r"[\w.-]+/[\w.-]+", repo) or not author:
        return None
    return repo, author, frozenset(label.strip() for label in labels.split(",") if label.strip())


def main(argv: list[str]) -> int:
    if argv[1:] in (["--help"], ["-h"]):
        print(__doc__)
        return 0
    parsed = options(argv[1:])
    if parsed is None:
        print(__doc__, file=sys.stderr)
        return 2

    repo, author, labels = parsed
    body = sys.stdin.read()

    lines = findings(body, repo, author, labels)
    for line in lines:
        print(line)
    if lines:
        return 1
    exempt = exemption(author, labels)
    if exempt is not None:
        print(f"ok  {exempt}: no issue needs closing or assigning, and every closing keyword "
              f"names an open issue of {repo}")
    else:
        print(f"ok  every closing keyword names an open issue of {repo} assigned to {author}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
