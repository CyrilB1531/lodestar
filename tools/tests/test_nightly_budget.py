"""The nightly stops on its own budget rather than on the runner's timeout (#650).

Measured on 2026-09-11: a shared build file moved, `bench-map.json` correctly selected all
33 classes, and the sweep needs about three hours -- Text 40 minutes, Stats and Survival 8
between them, and Gpu past 70 and half done when `timeout-minutes: 120` killed the job.
GitHub reports a timeout as *cancelled*, which reads like a person did it.

The part that made it a trap rather than a slow night: a killed job writes no page, the
baseline marker only advances when a page is written, so the next night selected the same
33 and died the same way. The failure fed itself and no further change was needed to keep
it failing.

Measured again on 2026-09-16, with the budget honoured and the job killed anyway: the run
loop stopped itself at 61 minutes as told, and the steps *after* it took the remaining 59 --
"Compare against Python" alone 58. The deadline bounds one step, and the margin beneath it
has to hold every step that has none.

Measured once more on 2026-10-12: the classes stopped on time and 21 comparisons with no
deadline ran the job into its timeout every night from 2026-09-26 (#1687).

So three things have to stay true, and none is visible from reading one line:
  * every phase -- classes, comparisons -- has a start deadline and a cut on the job's clock,
    each cut before the next phase's last start, the last one before the page's minutes
  * what a night does not reach is written down, carried, and asked for first, save what it cut,
    which goes last
  * a failure is reported and the run goes on, since a step that stops costs the page
"""

from __future__ import annotations

import pathlib
import re
import sys

import pytest
import yaml

ROOT = pathlib.Path(__file__).resolve().parents[2]
NIGHTLY = ROOT / ".github" / "workflows" / "bench-nightly.yml"

sys.path.insert(0, str(ROOT / "tools"))

import render_nightly  # noqa: E402
import run_comparisons  # noqa: E402
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


def test_the_deadlines_follow_one_another_inside_the_timeout():
    """Classes, then comparisons, then the page: each phase's cut is the next one's guaranteed start (#1687)."""
    data = yaml.safe_load(NIGHTLY.read_text(encoding="utf-8"))
    job = next(iter(data["jobs"].values()))
    timeout = int(job["timeout-minutes"])
    budget = int(steps()["Run them"]["env"]["BUDGET_MINUTES"])
    class_stop = int(steps()["Run them"]["env"]["CLASS_STOP_BY_MINUTES"])
    start_by = int(steps()["Compare against Python"]["env"]["COMPARISON_START_BY_MINUTES"])
    stop_by = int(job["env"]["STOP_BY_MINUTES"])

    assert budget <= class_stop < start_by <= stop_by < timeout - PAGE_TAIL_MINUTES, (
        f"classes start until {budget} and are cut at {class_stop}, comparisons start until {start_by} "
        f"and are cut at {stop_by}, the page needs {PAGE_TAIL_MINUTES} of the {timeout}: one phase "
        "can run into the next or the page into the timeout")


def test_what_the_run_did_not_reach_is_written_down():
    run = steps()["Run them"]["run"]

    assert "owed.txt" in run, "Run them reaches no budget and records nothing"
    # One `dotnet run` per class, not per project: a deadline checked between projects
    # cannot stop Gpu, which alone is longer than the whole budget.
    assert 'tools/run_bounded.py --name "$name"' in run and "while read -r name" in run, (
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


def test_what_is_owed_goes_first_so_a_short_night_does_not_drop_it_again():
    """#1687: in the map's order the owed tail was the part every deadline cut, night after night."""
    data = {"harnesses": {"levenshtein": {}, "metrics": {}, "survival": {}, "ols": {}}}

    carried = select_benchmarks.with_owed(
        data, "harnesses", "ols survival", ["levenshtein", "metrics", "survival"])

    assert carried == ["survival", "ols", "levenshtein", "metrics"]


def _args(**overrides):
    import argparse
    values = {"commit": "", "baseline": "", "runner": "test", "selected": [],
              "harnesses": [], "owed": [], "failed": [], "cut": [], "reason": "", "branch": False, "stdout": True}
    values.update(overrides)
    return argparse.Namespace(**values)


# The steps after the comparisons -- page, wiki, aggregate, push -- took about one minute on 2026-09-25's
# run, the last one to reach them; nine allowed. The pull request is the separate publish job.
PAGE_TAIL_MINUTES = 9


def test_the_comparisons_stop_on_the_job_clock_and_leave_the_page_its_minutes():
    """#1687: the classes stopped on time, then 21 comparisons ran with no deadline into the timeout."""
    data = yaml.safe_load(NIGHTLY.read_text(encoding="utf-8"))
    job = next(iter(data["jobs"].values()))
    timeout = int(job["timeout-minutes"])
    first = job["steps"][0]
    compare = steps()["Compare against Python"]
    env = compare.get("env") or {}

    assert "JOB_STARTED" in first.get("run", ""), (
        "the job's first step does not note its start, so no deadline counts the minutes before it")
    start_by = int(env["COMPARISON_START_BY_MINUTES"])
    stop_by = int(job["env"]["STOP_BY_MINUTES"])
    assert start_by <= stop_by and stop_by + PAGE_TAIL_MINUTES < timeout, (
        f"a comparison may run until {stop_by} minutes and the page needs {PAGE_TAIL_MINUTES} more: "
        f"past the {timeout} the runner allows, which kills the job with nothing published")
    run = compare["run"]
    for flag in ("--started \"$JOB_STARTED\"", "--start-by", '--stop-by "$STOP_BY_MINUTES"'):
        assert flag in run, f"Compare against Python does not pass {flag} to run_comparisons.py"
    classes = steps()["Run them"]["run"]
    assert "JOB_STARTED" in classes, (
        "the classes count their budget from their own step while the comparisons count from the "
        "job, so the setup's minutes belong to neither")
    assert "CLASS_STOP_BY_MINUTES" in classes and "tools/run_bounded.py" in classes, (
        "a class started just inside the budget is never cut, so a long one still runs the job "
        "into its timeout")
    class_stop = int(steps()["Run them"]["env"]["CLASS_STOP_BY_MINUTES"])
    assert int(steps()["Run them"]["env"]["BUDGET_MINUTES"]) <= class_stop < start_by, (
        "the class cut is not before the comparisons' last start, so a long class still takes "
        "every comparison's minutes")


def test_the_comparisons_selection_is_handed_what_is_owed():
    select = steps()["Which classes and comparisons that makes worth re-running"]["run"]

    assert re.search(r'--owed="\$OWED" --cut="\$CUT" --harnesses', select), (
        "the comparisons' selection is not handed the owed list, so a comparison a short night "
        "skipped is never asked for again")


def test_the_page_lists_only_what_ran():
    page = steps()["Write the page"]["run"]

    assert "harnesses-run.txt" in page and "cat harnesses.txt" not in page, (
        "the page lists every selected comparison as run, a skipped one included")
    assert "classes-run.txt" in page and "cat selected.txt" not in page, (
        "the page lists every selected class as re-run, an owed one included")


def _harnesses(*names, generate=None):
    return {name: {"python": f"bench/python/{name}.py", "subcommand": f"compare-{name}",
                   "compare": name, "generate": (generate or {}).get(name, [])} for name in names}


def test_no_comparison_starts_past_the_deadline_and_the_rest_are_owed(tmp_path):
    now = [0.0]
    calls = []

    def runner(command, limit, output):
        calls.append(command)
        now[0] += 60.0
        return True

    outcomes = run_comparisons.run(
        ["a", "b", "c"], _harnesses("a", "b", "c"), started=0.0, start_by=300.0, stop_by=600.0,
        results=tmp_path, clock=lambda: now[0], runner=runner)

    # Four commands a comparison at a minute each: "a" ends at 240 s, "b" starts before 300 and
    # ends at 480, "c" would start past 300.
    assert outcomes.ran == ["a", "b"]
    assert outcomes.owed == ["c"]
    assert outcomes.cut == [], "one never started was recorded as cut"


def test_a_comparison_running_at_the_hard_stop_is_cut_and_owed(tmp_path):
    now = [0.0]

    def runner(command, limit, output):
        if command[0] == "dotnet" and "compare-slow" in command:
            assert limit == 600.0 - now[0], "the cut is not the time left to the hard stop"
            now[0] = 600.0
            return False
        now[0] += 10.0
        return True

    (tmp_path / "compare-slow.txt").write_text("stale", encoding="utf-8")
    outcomes = run_comparisons.run(
        ["slow", "next"], _harnesses("slow", "next"), started=0.0, start_by=500.0, stop_by=600.0,
        results=tmp_path, clock=lambda: now[0], runner=runner)

    assert outcomes.ran == []
    assert outcomes.owed == ["slow", "next"]
    assert outcomes.cut == ["slow"], "only the one running at the stop was cut"
    assert not (tmp_path / "compare-slow.txt").exists(), "a cut comparison left a result to publish"


def test_a_shared_corpus_is_generated_once_and_only_for_what_runs(tmp_path):
    now = [0.0]
    commands = []

    def runner(command, limit, output):
        commands.append(tuple(command))
        now[0] += 100.0
        return True

    generate = {"a": ["gen/shared.py"], "b": ["gen/shared.py"], "c": ["gen/late.py"]}
    run_comparisons.run(
        ["a", "b", "c"], _harnesses("a", "b", "c", generate=generate), started=0.0, start_by=900.0,
        stop_by=2000.0, results=tmp_path, clock=lambda: now[0], runner=runner)

    assert commands.count(("python", "gen/shared.py")) == 1
    assert ("python", "gen/late.py") not in commands, "a skipped comparison's corpus was generated"


def test_a_failing_comparison_is_reported_and_the_rest_still_run(tmp_path):
    """A failure that ended the step would stop the page as surely as the timeout did (#1687)."""
    import subprocess

    def runner(command, limit, output):
        if command[0] == "dotnet" and "compare-broken" in command:
            raise subprocess.CalledProcessError(1, list(command))
        return True

    outcomes = run_comparisons.run(
        ["broken", "fine"], _harnesses("broken", "fine"), started=0.0, start_by=900.0, stop_by=999.0,
        results=tmp_path, clock=lambda: 0.0, runner=runner)

    assert (outcomes.ran, outcomes.owed, outcomes.failed) == (["fine"], [], ["broken"])


@pytest.mark.skipif(sys.platform == "win32", reason="process groups are POSIX; the nightly runs on ubuntu-latest")
def test_the_bounded_runner_kills_the_group_and_settles_the_class(tmp_path):
    """Its settling, driven with commands of the test's own; main builds the real one."""
    import subprocess
    import time

    import run_bounded

    files = {"ran": "classes-run.txt", "owed": "owed.txt", "failed": "failed.txt", "cut": "cut.txt"}

    def bounded(name, seconds, *command):
        run_bounded.settle_class(name, seconds, list(command), directory=tmp_path)

    def read(kind):
        path = tmp_path / files[kind]
        return path.read_text(encoding="utf-8").split() if path.exists() else []

    started = time.time()
    bounded("slow", 1, "bash", "-c", "sleep 31.69 & sleep 31.69")
    assert time.time() - started < 10
    left = subprocess.run(["pgrep", "-x", "-f", "sleep 31.69"], capture_output=True, text=True).stdout
    assert left.strip() == "", "a grandchild outlived the cut"
    # Exit 124 of its own is a failure, not a cut: the outcome is never read from the exit code.
    bounded("own124", 5, sys.executable, "-c", "raise SystemExit(124)")
    bounded("fine", 5, sys.executable, "-c", "pass")
    bounded("late", 0, sys.executable, "-c", "pass")
    # stdin is the loop's list of names; a child must not swallow it.
    bounded("reader", 5, sys.executable, "-c", "import sys; assert sys.stdin.read() == ''")

    assert read("ran") == ["fine", "reader"]
    assert read("owed") == ["slow", "late"]
    assert read("cut") == ["slow"]
    assert read("failed") == ["own124"]


def test_the_bounded_runner_builds_its_command_and_refuses_what_it_does_not_know(monkeypatch, tmp_path):
    """Nothing from its command line is run: the class comes from the map, the project from a list."""
    import run_bounded

    assert run_bounded.command_for("XBenchmarks", "bench/Lodestar.Text.Benchmarks")[-6:] == [
        "--filter", "*.XBenchmarks.*", "--job", "short", "--exporters", "github"]
    assert run_bounded.main(["--name", "NotAClass; rm -rf /", "--seconds", "1"]) == 2

    # A mention in a comment is not a declaration: the class's own project is the one found.
    project = tmp_path / "bench" / "P"
    project.mkdir(parents=True)
    (project / "A.cs").write_text("// see class FooBenchmarks\n", encoding="utf-8")
    (project / "B.cs").write_text("namespace X;\npublic class FooBenchmarks {\n    [Benchmark] public void A() {}\n}\n",
                                  encoding="utf-8")
    monkeypatch.setattr(run_bounded, "ROOT", tmp_path)
    monkeypatch.setattr(run_bounded, "PROJECTS", ("bench/P",))
    assert run_bounded.project_of("FooBenchmarks") == "bench/P"
    (project / "B.cs").unlink()
    assert run_bounded.project_of("FooBenchmarks") is None


def test_a_shared_python_side_runs_once_a_night(tmp_path):
    commands = []

    def runner(command, limit, output):
        commands.append(tuple(command))
        return True

    harnesses = _harnesses("stats", "ols")
    harnesses["ols"]["python"] = harnesses["stats"]["python"]
    run_comparisons.run(["stats", "ols"], harnesses, started=0.0, start_by=900.0, stop_by=999.0,
                        results=tmp_path, clock=lambda: 0.0, runner=runner)

    assert commands.count(("python", harnesses["stats"]["python"])) == 1


def test_a_broken_shared_side_fails_its_sharers_without_running_again(tmp_path):
    import subprocess

    commands = []

    def runner(command, limit, output):
        commands.append(tuple(command))
        if command == ["python", "bench/python/stats.py"]:
            raise subprocess.CalledProcessError(1, list(command))
        return True

    harnesses = _harnesses("stats", "ols")
    harnesses["ols"]["python"] = harnesses["stats"]["python"]
    outcomes = run_comparisons.run(["stats", "ols"], harnesses, started=0.0, start_by=900.0,
                                   stop_by=999.0, results=tmp_path, clock=lambda: 0.0, runner=runner)

    assert commands.count(("python", "bench/python/stats.py")) == 1
    assert outcomes.failed == ["stats", "ols"]


def test_cut_names_go_after_everything_else():
    """#1694: owed first, one that does not fit would be cut first every night ahead of the rest."""
    data = {"benchmarks": {"A": [], "B": [], "C": [], "D": []}}

    order = select_benchmarks.with_owed(data, "benchmarks", "A C", ["B", "C", "D"], cut="C")

    assert order == ["A", "B", "D", "C"]


def test_everything_selected_still_asks_for_the_owed_first(capsys):
    """--all after a short night: in map order the owed tail met the deadline again and was dropped."""
    import argparse

    data = {"harnesses": {"a": {}, "b": {}, "c": {}}}
    args = argparse.Namespace(owed="c", cut="")
    select_benchmarks.print_selection(data, "harnesses", args, data["harnesses"])

    assert capsys.readouterr().out.split() == ["c", "a", "b"]


def test_the_page_carries_what_was_cut():
    page = render_nightly.render(_args(commit="a", baseline="b", owed=["X", "Y"], cut=["Y"]), reports=[])

    assert re.search(r"<!-- nightly-cut: ([^>]*) -->", page).group(1).split() == ["Y"]


def test_the_page_derived_lists_reach_no_script_by_interpolation():
    """A quote in the owed marker, read from a page file, would otherwise run as shell code."""
    for name, step in steps().items():
        run = step.get("run", "")
        assert "steps.baseline.outputs.owed" not in run and "steps.baseline.outputs.cut" not in run, (
            f"{name} interpolates a page-derived list into its script")


def test_a_comparison_is_late_past_the_stop_whatever_the_start_deadline_says(tmp_path):
    """Started past the stop it would be "cut" before it ran, and asked for last though it never had the window."""
    outcomes = run_comparisons.run(
        ["a"], _harnesses("a"), started=0.0, start_by=900.0, stop_by=600.0, results=tmp_path,
        clock=lambda: 700.0, runner=lambda command, limit, output: True)

    assert (outcomes.owed, outcomes.cut) == (["a"], [])


def test_the_page_names_what_was_cut_apart_from_what_was_not_reached():
    page = render_nightly.render(
        _args(commit="a", baseline="b", owed=["XBenchmarks", "YBenchmarks"], cut=["YBenchmarks"]),
        reports=[])
    not_reached = page.split("## Classes selected, and not reached tonight")[1].split("##")[0]
    cut = page.split("## Asked for last")[1]

    assert "XBenchmarks" in not_reached and "YBenchmarks" not in not_reached
    assert "YBenchmarks" in cut


def test_a_failure_turns_the_run_red_after_the_pages_are_handed_over():
    data = yaml.safe_load(NIGHTLY.read_text(encoding="utf-8"))
    measure = data["jobs"]["measure"]
    names = [step.get("name") for step in measure["steps"]]

    assert names[-1] == "Fail on what failed tonight", (
        "a failure only annotates a green run, and the next page drops it")
    assert names.index("Hand the pages to the publish job") < names.index(names[-1])
    assert "failed.txt" in measure["steps"][-1]["run"]
    assert "!cancelled()" in data["jobs"]["publish"]["if"], (
        "measure failing on its last step would skip the pull request for numbers it measured")


def test_the_workflow_reads_every_marker_through_one_parser():
    baseline = steps()["The commit the previous run stopped at"]["run"]

    assert "python tools/render_nightly.py --markers" in baseline
    assert "sed " not in baseline, "two parsers read one page"
    page = render_nightly.render(_args(commit="a", baseline="b" * 40, owed=["Beta"], cut=["Beta"]), reports=[])
    assert render_nightly.markers(page) == ["sha=" + "b" * 40, "owed=Beta", "cut=Beta"]
    assert render_nightly.markers("") == ["sha=", "owed=", "cut="]


def test_a_name_left_unsettled_is_owed_last_and_a_cut_name_stays_last_until_it_runs():
    """#1694/#1695: owed first, a name that kills its step or never fits would starve the rest."""
    import nightly_outcomes

    # B was selected and never settled; X was cut last night and is late tonight; A was cut and ran.
    owed_add, cut_add = nightly_outcomes.carry(
        selected=["A", "B", "C", "h1", "X"], settled={"A", "C", "h1", "X"}, owed={"X"}, cut=set(),
        cut_before=["X", "A", "-v", "*"])

    assert owed_add == ["B"]
    assert cut_add == ["B", "X"], "a name taken from the page reached a list it was not in"


def test_the_page_step_carries_through_the_tested_rule():
    run = steps()["Write the page"]["run"]

    assert 'python tools/nightly_outcomes.py carry --cut-before="$CUT_BEFORE"' in run
    assert "for name in $CUT_BEFORE" not in run, "the rule is back in shell, where no test runs it"


def test_the_class_loop_fails_a_class_its_runner_left_unsettled(tmp_path):
    """The `||` handler, run: failed unless run_bounded settled the class before it fell over."""
    import subprocess
    import sys

    if sys.platform == "win32":
        pytest.skip("the step runs on ubuntu-latest")
    run = steps()["Run them"]["run"]
    handler = run[run.index("python tools/run_bounded.py"):run.index("}", run.index("|| {")) + 1]
    stub = tmp_path / "bin"
    stub.mkdir()
    (stub / "python").write_text("#!/bin/sh\n[ -n \"$SETTLE\" ] && echo \"$3\" >> \"$SETTLE\"\nexit 3\n",
                                 encoding="utf-8")
    (stub / "python").chmod(0o755)
    for settle, failed in (("", ["Lost"]), ("owed.txt", [])):
        for name in ("classes-run.txt", "owed.txt", "failed.txt"):
            (tmp_path / name).write_text("", encoding="utf-8")
        script = f"set -euo pipefail\nname=Lost\nleft=10\n{handler}\n"
        out = subprocess.run(["bash", "-c", script], cwd=tmp_path, check=True, capture_output=True, text=True,
                             env={"PATH": f"{stub}:/usr/bin:/bin", "SETTLE": settle}).stdout
        assert "exited 3" in out
        assert (tmp_path / "failed.txt").read_text(encoding="utf-8").split() == failed


def test_the_page_is_written_after_a_failed_step():
    """#1695: a measuring step that raised cost the page, which is the run's product."""
    for name in ("Run them", "Compare against Python"):
        assert steps()[name].get("continue-on-error") is True, name
    fail = steps()["Fail on what failed tonight"]
    assert "steps.run.outcome" in str(fail["env"]) and "steps.compare.outcome" in str(fail["env"]), (
        "a raised measuring step leaves the run green")
    for name in ("Push what changed", "Hand the pages to the publish job", "Write the page"):
        assert "cancelled" not in str(steps()[name].get("if", "")), (
            f"{name} runs after a failed step it depends on")


def test_a_comparison_that_raises_fails_alone_and_the_others_are_recorded_as_they_settle(tmp_path):
    """#1695: settled only at the end, a raise lost every result and the page owed them all."""
    recorded = []

    def runner(command, limit, output):
        if command[0] == "dotnet" and "compare-boom" in command:
            raise FileNotFoundError("dotnet")
        return True

    harnesses = _harnesses("fine", "boom", "after")
    outcomes = run_comparisons.run(["fine", "boom", "after"], harnesses, started=0.0, start_by=900.0,
                                   stop_by=999.0, results=tmp_path, clock=lambda: 0.0, runner=runner,
                                   record=lambda one: recorded.append((one.ran, one.failed)))

    assert (outcomes.ran, outcomes.failed) == (["fine", "after"], ["boom"])
    assert recorded == [(["fine"], []), ([], ["boom"]), (["after"], [])]


def test_the_page_steps_carry_runs_over_the_files_the_measuring_steps_write(monkeypatch, tmp_path):
    """The call the page step makes, end to end: a mistyped file name would owe what ran."""
    import nightly_outcomes

    files = {"selected.txt": "A\nB\n", "harnesses.txt": "h1\nh2\n", "classes-run.txt": "A\n",
             "harnesses-run.txt": "h1\n", "failed.txt": "", "owed.txt": "X\n", "cut.txt": ""}
    for name, text in files.items():
        (tmp_path / name).write_text(text, encoding="utf-8")
    monkeypatch.chdir(tmp_path)

    assert nightly_outcomes.main(["carry", "--cut-before", "X A"]) == 0
    assert (tmp_path / "owed.txt").read_text(encoding="utf-8").split() == ["X", "B", "h2"]
    assert (tmp_path / "cut.txt").read_text(encoding="utf-8").split() == ["B", "h2", "X"]


def test_markers_read_a_missing_or_unreadable_page_as_unknown(monkeypatch, tmp_path, capsys):
    page = tmp_path / "nightly_run.md"
    monkeypatch.setattr(render_nightly, "PAGE", page)
    monkeypatch.setattr(render_nightly.sys, "argv", ["render_nightly.py", "--markers"])

    render_nightly.main()
    assert capsys.readouterr().out.split() == ["sha=", "owed=", "cut="]
    page.write_bytes(b"\xff\xfe<!-- nightly-baseline: abcdef1 -->\n")
    render_nightly.main()
    assert capsys.readouterr().out.split()[0] == "sha="


def test_a_page_value_that_looks_like_an_option_is_still_a_value(monkeypatch, tmp_path):
    """The step passes `--cut-before=...`: a marker of "-v" must not read as an option."""
    import nightly_outcomes

    monkeypatch.chdir(tmp_path)
    assert nightly_outcomes.main(["carry", "--cut-before=-v"]) == 0
