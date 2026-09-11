"""No workflow step may read its own outputs, which are empty while it runs.

#628, third attempt: a scripted rename turned the resolve step's inputs into references to
its own outputs --

    EVENT_PR: ${{ steps.target.outputs.pr }}      # in the step whose id *is* target

-- so the pull-request number arrived empty and every `pull_request_target` run failed at the
first step. The substring being replaced ended the same way as the line that was meant to be
the source, and nothing objected: it is valid YAML and a valid expression, worth exactly the
empty string.

The dispatch run that was supposed to prove the change did not catch it either, because that
path sets the number from the workflow input and never reads the broken line. A test that
reads the file is what covers both paths at once.
"""

from __future__ import annotations

import re
from pathlib import Path

import pytest
import yaml

WORKFLOWS = Path(__file__).resolve().parents[2] / ".github" / "workflows"
STEP_OUTPUT = re.compile(r"steps\.([A-Za-z0-9_-]+)\.outputs\.")


def steps_of(workflow: Path):
    """Every (job, step) pair a workflow declares, whatever shape its jobs take."""
    data = yaml.safe_load(workflow.read_text(encoding="utf-8")) or {}
    for job_name, job in (data.get("jobs") or {}).items():
        for step in (job or {}).get("steps", []) or []:
            yield job_name, step


@pytest.mark.parametrize("workflow", sorted(WORKFLOWS.glob("*.yml")), ids=lambda p: p.name)
def test_no_step_reads_its_own_outputs(workflow):
    for job_name, step in steps_of(workflow):
        own = step.get("id")
        if not own:
            continue
        body = yaml.safe_dump({k: v for k, v in step.items() if k != "name"})
        for referenced in STEP_OUTPUT.findall(body):
            assert referenced != own, (
                f"{workflow.name}: step '{own}' in job '{job_name}' reads "
                f"steps.{own}.outputs, which is empty while that step runs."
            )


@pytest.mark.parametrize("workflow", sorted(WORKFLOWS.glob("*.yml")), ids=lambda p: p.name)
def test_every_referenced_step_exists_and_comes_first(workflow):
    """A reference to a step that does not exist, or runs later, is the same empty string."""
    for job_name, _ in steps_of(workflow):
        break
    data = yaml.safe_load(workflow.read_text(encoding="utf-8")) or {}
    for job_name, job in (data.get("jobs") or {}).items():
        seen: set[str] = set()
        for step in (job or {}).get("steps", []) or []:
            body = yaml.safe_dump({k: v for k, v in step.items() if k != "name"})
            for referenced in STEP_OUTPUT.findall(body):
                assert referenced in seen, (
                    f"{workflow.name}: job '{job_name}' reads steps.{referenced}.outputs "
                    f"before that step has run, or from a step that does not exist."
                )
            if step.get("id"):
                seen.add(step["id"])
