"""No workflow may test for a bot by globbing `[bot]`, which is a character class.

#642. `classify-pull-request.yml` decided whether a pull request was generated with

    case "$AUTHOR" in
      github-actions*|*[bot]) ...

and `*[bot]` does not mean "ends with the literal [bot]". In a shell glob, brackets open a
character class, so the pattern means "ends with b, o or t". `dependabot[bot]` ends with `]`
and never matched; `github-actions*` was carrying the whole test alone, and Dependabot's four
pull requests were classified as if a person had opened them.

Nothing objects to it: it is a valid pattern that matches the wrong set, so the only visible
symptom was four bumps arriving on a milestone. The glob is banned outright rather than
corrected, because the spelling to match is not stable either -- REST calls the same author
`dependabot[bot]` where GraphQL calls it `app/dependabot`. Both APIs publish a flag saying so
(`.user.type == "Bot"`, `.author.is_bot`), and reading it is what the workflow does now.

Only an *unquoted* occurrence is refused. `git config user.name "github-actions[bot]"`, which
`bench-nightly.yml` and `wiki.yml` both run, is the literal suffix and is correct -- quoting is
exactly what separates the two, and nothing else does.
"""

from __future__ import annotations

import re
from pathlib import Path

import pytest
import yaml

WORKFLOWS = Path(__file__).resolve().parents[2] / ".github" / "workflows"
QUOTED = re.compile(r"'[^']*'" + r'|"[^"]*"')


def unquoted(line: str) -> str:
    """The line with its quoted spans removed, because a glob only globs unquoted.

    Backslash-escaped quotes are not handled: no workflow here has one, and a span this
    misses can only lose a finding, never invent one.
    """
    return QUOTED.sub("", line)


def shell_lines(workflow: Path):
    """Every executable line of every `run:` block, with its job and step named."""
    data = yaml.safe_load(workflow.read_text(encoding="utf-8")) or {}
    for job_name, job in (data.get("jobs") or {}).items():
        for step in (job or {}).get("steps", []) or []:
            script = step.get("run")
            if not isinstance(script, str):
                continue
            for line in script.splitlines():
                # A comment may name the trap; only what the shell executes is refused.
                if line.lstrip().startswith("#"):
                    continue
                yield job_name, step.get("name", step.get("id", "?")), line


@pytest.mark.parametrize("workflow", sorted(WORKFLOWS.glob("*.yml")), ids=lambda p: p.name)
def test_no_run_block_globs_the_bot_suffix(workflow):
    for job_name, step_name, line in shell_lines(workflow):
        assert "[bot]" not in unquoted(line), (
            f"{workflow.name}: step '{step_name}' in job '{job_name}' runs\n"
            f"    {line.strip()}\n"
            f"`[bot]` unquoted is the character class b/o/t, so it matches a name ending in "
            f"one of those letters and never the literal suffix. Read the API's own bot flag "
            f"instead -- `.user.type` on the event, `.author.is_bot` through gh."
        )
