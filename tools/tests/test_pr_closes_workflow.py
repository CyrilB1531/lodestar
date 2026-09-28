"""pr-closes.yml never leaves a run of its required check cancelled on a pull request (#1317).

#1316 showed every check green while the pull request stayed blocked: `gh pr create` with labels
fired several `pull_request` events within a second, the workflow's concurrency group cancelled all
but the last, and a cancelled run of a required check -- even one cancelled before its job began,
which no check listing shows -- counts against the merge. These tests pin the three changes that
keep it from happening again: no concurrency group, no condition that skips the job (a skipped
job posts the check as a success, so a label would clear a failure), and a pull request read as it
stands rather than as each event carried it, so the newest run judges the newest state.
"""

from __future__ import annotations

from pathlib import Path

import yaml

WORKFLOW = Path(__file__).resolve().parents[2] / ".github" / "workflows" / "pr-closes.yml"
REQUIRED_CHECK = "Pull request closes only open issues"


def load() -> dict:
    return yaml.safe_load(WORKFLOW.read_text(encoding="utf-8"))


def job() -> dict:
    return load()["jobs"]["closes"]


def test_the_workflow_declares_no_concurrency_group():
    """A group cancels its pending run whatever cancel-in-progress says, so there is none."""
    workflow = load()
    assert "concurrency" not in workflow
    assert "concurrency" not in job()


def test_the_required_check_keeps_its_name():
    """The ruleset waits on this name; renaming the job would leave it pending for ever."""
    assert job()["name"] == REQUIRED_CHECK


def test_no_event_skips_the_job():
    """A job skipped by its condition posts the required check as a success, so a label would clear a failure."""
    assert "if" not in job()


def test_the_label_events_still_trigger_the_workflow():
    """Every label event re-judges the pull request, no-issue being the one that can change it."""
    # PyYAML reads the key `on` as True.
    types = load()[True]["pull_request"]["types"]
    for event in ("opened", "edited", "synchronize", "labeled", "unlabeled"):
        assert event in types


def test_the_pull_request_is_read_as_it_stands_not_as_the_event_carried_it():
    """Runs for two quick edits must judge the same body and labels."""
    step = job()["steps"][-1]
    text = step["run"] + str(step["env"])
    assert "github.event.pull_request.body" not in text
    assert "github.event.pull_request.labels" not in text
    assert "gh pr view" in step["run"]


def test_a_failed_read_fails_the_check():
    """Without pipefail a failed gh call would pipe an empty body, which no-issue lets pass unchecked."""
    run = job()["steps"][-1]["run"]
    assert "set -euo pipefail" in run
    assert run.count("gh pr view") == 1
