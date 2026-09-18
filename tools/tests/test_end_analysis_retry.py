"""The `End analysis` step's own script, run against a fake scanner (#1081).

#1072 replaced `scanner ... | tee "$log"` because a pipeline is worth its last command's status,
so that form reported a failed analysis -- a failed quality gate included -- as successful on
every run. The review pass before the push caught it, no check did, and the fake scanner that
proved the replacement was never committed. This is that scanner, reading the step out of the
workflow so that what runs here is what runs in CI rather than a copy of it.
"""

from __future__ import annotations

import os
import pathlib
import shutil
import subprocess
import sys
import time

import pytest
import yaml

# The step is a bash script with a POSIX runner's semantics, and the executable bits below are
# POSIX too. `Tool tests` runs this file on Windows as well, where it has nothing to assert.
pytestmark = pytest.mark.skipif(sys.platform == "win32", reason="the step runs on ubuntu-latest")

ROOT = pathlib.Path(__file__).resolve().parents[2]
CI = ROOT / ".github" / "workflows" / "ci.yml"
# By absolute path, because the harness shadows `sleep` with a stub that returns at once.
SLEEP = shutil.which("sleep") or "/bin/sleep"

OUTAGE = (1, "ERROR Error 500 on https://sonarcloud.io/api/project_badges/measure Please try again")
GATE = (1, "INFO QUALITY GATE STATUS: FAILED - View details on https://sonarcloud.io/dashboard")
FINE = (0, "INFO QUALITY GATE STATUS: PASSED")


def script_of(workflow: pathlib.Path, job: str, step: str) -> str:
    """One step's `run`, with the runner's temp directory left as a shell variable."""
    data = yaml.safe_load(workflow.read_text(encoding="utf-8"))
    for declared in data["jobs"][job]["steps"]:
        if declared.get("name") == step:
            return declared["run"].replace("${{ runner.temp }}", "$RUNNER_TEMP")
    raise AssertionError(f"{workflow.name}: no step named {step!r} in job {job!r}")


def harness(tmp_path: pathlib.Path, *exits: tuple[int, str]) -> tuple[int, str, int]:
    """Run the step with a scanner answering `exits` in order, and no real sleeping.

    long-comment: the shape of the fake, which is what a reader has to trust here.
    Each attempt appends one byte to a counter file and answers the matching entry, so the
    script's own loop decides how many attempts happen -- the test asserts that number rather
    than assuming it. Returns the step's status, everything it printed, and the attempts made.
    """
    scanner = tmp_path / "scanner"
    scanner.mkdir()
    attempts = tmp_path / "attempts"
    attempts.write_text("", encoding="utf-8")
    answers = "\n".join(
        f'  {index}) printf "%s\\n" {message!r}; exit {code} ;;'
        for index, (code, message) in enumerate(exits, start=1))
    fake_scanner = scanner / "dotnet-sonarscanner"
    fake_scanner.write_text(
        "#!/usr/bin/env bash\n"
        f'printf "x" >> "{attempts}"\n'
        f'case $(wc -c < "{attempts}") in\n{answers}\n'
        '  *) echo "unexpected attempt"; exit 99 ;;\nesac\n',
        encoding="utf-8")
    fake_scanner.chmod(0o755)

    # The step sleeps 120 then 300 seconds between attempts, which a test must not.
    stubs = tmp_path / "bin"
    stubs.mkdir()
    (stubs / "sleep").write_text("#!/usr/bin/env bash\nexit 0\n", encoding="utf-8")
    (stubs / "sleep").chmod(0o755)

    done = subprocess.run(
        # `bash -e`, which is what a `run:` step without a `shell:` gets from the runner; the
        # workflow sets no `defaults.shell`, so `pipefail` is the step's own to declare.
        ["bash", "-e", "-c", script_of(CI, "build-test", "End analysis")],
        capture_output=True, text=True, check=False,
        env={**os.environ,
             "PATH": os.pathsep.join([str(stubs), os.environ["PATH"]]),
             "RUNNER_TEMP": str(tmp_path),
             "SONAR_TOKEN": "fake"})
    return done.returncode, done.stdout + done.stderr, len(attempts.read_text(encoding="utf-8"))


def test_a_successful_analysis_runs_the_scanner_once(tmp_path):
    status, output, attempts = harness(tmp_path, FINE)

    assert (status, attempts) == (0, 1)
    assert "QUALITY GATE STATUS: PASSED" in output


def test_a_failed_quality_gate_fails_at_once(tmp_path):
    status, output, attempts = harness(tmp_path, GATE, FINE)

    # `| tee` without pipefail passes this as 0, and before #1063 it spent seven minutes of sleep
    # and two more analyses of the same report on every red pull request.
    assert (status, attempts) == (1, 1)
    assert "::error::The quality gate failed" in output


def test_an_outage_is_retried_and_the_analysis_goes_through(tmp_path):
    status, output, attempts = harness(tmp_path, OUTAGE, FINE)

    assert (status, attempts) == (0, 2)
    assert "::warning::End analysis failed on attempt 1" in output


def test_three_outages_fail(tmp_path):
    status, output, attempts = harness(tmp_path, OUTAGE, OUTAGE, OUTAGE)

    assert (status, attempts) == (1, 3)
    assert "::error::End analysis failed three times." in output


def test_every_attempt_prints_the_scanner_inside_its_own_group(tmp_path):
    _, output, _ = harness(tmp_path, OUTAGE, FINE)

    # What a cancelled run keeps: cancel-in-progress is on for pull requests and this step can
    # take twenty minutes, so the output belongs in the group as the scanner writes it (#1081).
    first = output.index("Error 500")
    assert output.index("::group::End analysis, attempt 1 of 3") < first < output.index("::endgroup::")


@pytest.mark.parametrize("scenario", [(GATE,), (OUTAGE, OUTAGE, OUTAGE)], ids=["gate", "outage"])
def test_a_failing_analysis_never_exits_zero(tmp_path, scenario):
    assert harness(tmp_path, *scenario)[0] != 0


def test_the_scanners_output_reaches_the_log_while_it_is_still_running(tmp_path):
    """A cancelled run keeps what was printed, and this step can take twenty minutes (#1081)."""
    scanner = tmp_path / "scanner"
    scanner.mkdir()
    release = tmp_path / "release"
    fake_scanner = scanner / "dotnet-sonarscanner"
    fake_scanner.write_text(
        "#!/usr/bin/env bash\n"
        'printf "%s\\n" "INFO Sensor C# analyzer"\n'
        f'until [ -e "{release}" ]; do "{SLEEP}" 0.05; done\n'
        'printf "%s\\n" "INFO QUALITY GATE STATUS: PASSED"\n',
        encoding="utf-8")
    fake_scanner.chmod(0o755)
    printed = tmp_path / "step.out"

    with printed.open("w", encoding="utf-8") as sink:
        step = subprocess.Popen(
            ["bash", "-e", "-c", script_of(CI, "build-test", "End analysis")],
            stdout=sink, stderr=subprocess.STDOUT,
            env={**os.environ, "RUNNER_TEMP": str(tmp_path), "SONAR_TOKEN": "fake"})
        deadline = time.monotonic() + 10
        while "Sensor C# analyzer" not in printed.read_text(encoding="utf-8"):
            if time.monotonic() > deadline or step.poll() is not None:
                break
            time.sleep(0.05)
        live = "Sensor C# analyzer" in printed.read_text(encoding="utf-8")
        release.touch()
        assert step.wait(timeout=30) == 0

    assert live, "the step printed nothing until the scanner had exited"
