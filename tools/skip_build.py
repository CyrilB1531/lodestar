#!/usr/bin/env python3
"""Answer whether a pull request changes nothing the build, the tests or Windows can judge (#1073).

`docs_only.py` answers the narrower question, and four jobs hung off it alone —
`build-test`, `sample`, `oracles-fresh` and `windows`. A pull request touching one
workflow file therefore paid all four. #1072 measured what that costs, on the one
workflow this rule excludes: 6 minutes of build and test, 2.1 of sample, 1.1 of
oracles, and a Windows job still running after the required checks had gone green.
A one-line change to any other workflow paid the same, and now costs about 100 s.

Two classes answer ``true`` here:

* **Markdown only**, which is `docs_only.py`'s answer — the reduced path #857 added.
* **Workflow only**, meaning every path is one of the workflow-ish files ``UNREAD``
  names: nothing in the pull-request pipeline reads `release.yml`,
  `release-nuget-org.yml`, `bench-nightly.yml`, `bench-ondemand.yml`,
  `classify-pull-request.yml`, `wiki.yml` or `dependabot.yml`, so a build, a test run
  and a platform check cannot say anything about a change to them. The list is
  enumerated and asserted against the directory, so a workflow added later takes the
  full path until someone classifies it.

`ci.yml` is excluded **by the rule, not by a convention**: that file *is* the gate, so
running the pipeline is the only way to know the change to it works. A pull request
mixing it with anything else takes the full path too, because every path must qualify.

Anything else — a source file, a project file, an image, a JSON map, an action under
`.github/actions/`, `CODEOWNERS` — makes the answer ``false``, and so does an empty
list: a pull request whose files could not be listed takes the full path, never the
reduced one.

Usage:  gh api repos/OWNER/REPO/pulls/N/files --paginate \\
            --jq '.[] | .filename, (.previous_filename // empty)' \\
          | python tools/skip_build.py

With ``--workflows-only``, answers the narrower question instead: every path such a
workflow and **no Markdown at all**, which is what lets `Guide snippets compile,
reference snippets run` be skipped — a documentation change is exactly what breaks a
snippet, so that job runs whenever one Markdown file moved.

Prints ``true`` or ``false``; exits 0 either way.
"""

from __future__ import annotations

import sys
from collections.abc import Iterable

# The gate itself. A change here runs everything, because the pipeline is what is under test.
GATE = ".github/workflows/ci.yml"

# Enumerated rather than globbed, and asserted against the directory in test_skip_build.py:
# a workflow added later has to be classified on purpose rather than inherit the reduced path.
UNREAD = frozenset({
    ".github/workflows/bench-nightly.yml",
    ".github/workflows/bench-ondemand.yml",
    ".github/workflows/classify-pull-request.yml",
    ".github/workflows/release.yml",
    ".github/workflows/release-nuget-org.yml",
    ".github/workflows/wiki.yml",
    ".github/dependabot.yml",
})


def workflow_only(path: str) -> bool:
    """Whether one path is a workflow-ish file the pull-request pipeline does not read."""
    return path in UNREAD


def skip_build(paths: Iterable[str]) -> bool:
    """Whether every non-blank path is Markdown or a workflow the pipeline does not read."""
    listed = [path.strip() for path in paths if path.strip()]
    return bool(listed) and all(
        path.endswith(".md") or workflow_only(path) for path in listed)


def workflows_only(paths: Iterable[str]) -> bool:
    """Whether every non-blank path is such a workflow, with no Markdown among them."""
    listed = [path.strip() for path in paths if path.strip()]
    return bool(listed) and all(workflow_only(path) for path in listed)


def main(argv: list[str] | None = None) -> int:
    argv = sys.argv if argv is None else argv
    # --workflows-only is the narrower question the snippets job asks: with no Markdown in the
    # pull request, no ` ```csharp ` fence can have moved, so compiling them proves nothing.
    answer = workflows_only(sys.stdin) if "--workflows-only" in argv[1:] else skip_build(sys.stdin)
    print("true" if answer else "false")
    return 0


if __name__ == "__main__":
    sys.exit(main())
