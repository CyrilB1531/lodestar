"""The nightly stops on its own budget rather than on the runner's timeout (#650).

Measured on 2026-09-11: a shared build file moved, `bench-map.json` correctly selected all
33 classes, and the sweep needs about three hours -- Text 40 minutes, Stats and Survival 8
between them, and Gpu past 70 and half done when `timeout-minutes: 120` killed the job.
GitHub reports a timeout as *cancelled*, which reads like a person did it.

The part that made it a trap rather than a slow night: a killed job writes no page, the
baseline marker only advances when a page is written, so the next night selected the same
33 and died the same way. The failure fed itself and no further change was needed to keep
it failing.

So two things have to stay true, and neither is visible from reading one line:
  * the run loop has a deadline of its own, under the job's timeout
  * what it does not reach is written down, carried, and asked for again
"""

from __future__ import annotations

import pathlib
import re
import sys

import yaml

ROOT = pathlib.Path(__file__).resolve().parents[2]
NIGHTLY = ROOT / ".github" / "workflows" / "bench-nightly.yml"

sys.path.insert(0, str(ROOT / "tools"))

import render_nightly  # noqa: E402
import select_benchmarks  # noqa: E402


def steps() -> dict:
    data = yaml.safe_load(NIGHTLY.read_text(encoding="utf-8"))
    job = next(iter(data["jobs"].values()))
    return {step.get("name"): step for step in job["steps"] if step.get("name")}


def test_the_run_loop_has_a_deadline_under_the_job_timeout():
    data = yaml.safe_load(NIGHTLY.read_text(encoding="utf-8"))
    job = next(iter(data["jobs"].values()))
    run = steps()["Run them"]

    assert "BUDGET_MINUTES" in (run.get("env") or {}), (
        "Run them declares no BUDGET_MINUTES, so nothing stops it short of the runner's "
        "timeout -- which kills the job before it can write down what it did not reach."
    )
    budget = int(run["env"]["BUDGET_MINUTES"])
    timeout = int(job["timeout-minutes"])

    assert budget < timeout, (
        f"the run loop's budget is {budget} minutes and the job's timeout {timeout}: a "
        "deadline at or past the timeout cannot stop anything, because the runner gets "
        "there first."
    )
    assert "deadline" in run["run"], "Run them starts classes without checking a deadline"


def test_what_the_run_did_not_reach_is_written_down():
    run = steps()["Run them"]["run"]

    assert "owed.txt" in run, "Run them reaches no budget and records nothing"
    # One `dotnet run` per class, not per project: a deadline checked between projects
    # cannot stop Gpu, which alone is longer than the whole budget.
    assert re.search(r'filters="\*\$name\*"', run), (
        "Run them still batches a project's classes into one invocation, so its deadline "
        "can only be checked before an hour of work rather than before each class."
    )


def test_the_page_carries_the_owed_list_and_the_baseline_reads_it_back():
    page = render_nightly.render(
        _args(commit="abc123", baseline="def456", owed=["OneBenchmarks", "TwoBenchmarks"]),
        reports=[])

    assert "<!-- nightly-owed: OneBenchmarks TwoBenchmarks -->" in page

    # The expression the workflow recovers it with, applied to what the page actually holds.
    recovered = re.search(r"<!-- nightly-owed: ([^>]*) -->", page)
    assert recovered is not None
    assert recovered.group(1).split() == ["OneBenchmarks", "TwoBenchmarks"]


def test_a_night_that_reached_everything_still_writes_the_marker():
    """Empty, not absent: the workflow's sed finds nothing rather than the previous list."""
    page = render_nightly.render(_args(commit="abc", baseline="def", owed=[]), reports=[])

    assert "<!-- nightly-owed:  -->" in page
    assert re.search(r"<!-- nightly-owed: ([^>]*) -->", page).group(1).strip() == ""


def test_the_owed_classes_are_selected_again_even_when_nothing_changed():
    data = {"benchmarks": {"AlphaBenchmarks": [], "BetaBenchmarks": [], "GammaBenchmarks": []}}

    carried = select_benchmarks.with_owed(data, "benchmarks", "GammaBenchmarks AlphaBenchmarks", [])

    # The map's order, not the owed string's, so two nights owing the same set agree.
    assert carried == ["AlphaBenchmarks", "GammaBenchmarks"]


def test_a_class_the_map_no_longer_knows_is_dropped_rather_than_owed_forever():
    data = {"benchmarks": {"AlphaBenchmarks": []}}

    assert select_benchmarks.with_owed(data, "benchmarks", "DeletedBenchmarks", []) == []


def test_owed_and_selected_are_one_set_rather_than_two_runs():
    data = {"benchmarks": {"AlphaBenchmarks": [], "BetaBenchmarks": []}}

    carried = select_benchmarks.with_owed(
        data, "benchmarks", "AlphaBenchmarks", ["AlphaBenchmarks", "BetaBenchmarks"])

    assert carried == ["AlphaBenchmarks", "BetaBenchmarks"]


def _args(**overrides):
    import argparse
    values = {"commit": "", "baseline": "", "runner": "test", "selected": [],
              "harnesses": [], "owed": [], "reason": "", "branch": False, "stdout": True}
    values.update(overrides)
    return argparse.Namespace(**values)
