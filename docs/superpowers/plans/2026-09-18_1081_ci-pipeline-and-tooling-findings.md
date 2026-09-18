# CI pipeline and tooling findings Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Close #1059, #1060, #1061, #1062, #1067 and #1081 — make the docs-only path's guards
match what they claim, make `stage_doc_inputs.py` copy what MSBuild copies, commit the fake-scanner
harness that proved #1072, and write down the next-revision version invariant.

**Architecture:** Nothing under `src/` moves. Two Python tools change behaviour and are pinned by
`tools/tests/`; one new `tools/tests/` file extracts the `End analysis` step's own script out of
`.github/workflows/ci.yml` and runs it against a fake scanner, so the step is tested by reading the
file that runs in CI rather than by a copy of it. The rest is prose in `ci.yml`, `release.yml`,
`bench-nightly.yml`, `CLAUDE.md`, `CONTRIBUTING.md`, `tools/README.md` and three `Version.props`.

**Tech Stack:** Python 3.12 + pytest (`tools/tests/`), PyYAML, bash, GitHub Actions YAML, MSBuild
property files.

**Spec:** [`docs/superpowers/specs/2026-09-18_1081_ci-pipeline-and-tooling-findings.md`](../specs/2026-09-18_1081_ci-pipeline-and-tooling-findings.md)

**Branch:** `fix/1081-ci-pipeline-and-tooling-findings`

## Global Constraints

- One commit for the whole pull request; amend fix-ups rather than stacking them.
- No package version moves: `main` carries the next revision already (this is #1067's subject).
- No `CHANGELOG.md` entry: the changelog is grouped per package and a CI or tooling change has no
  package heading (`#997`, `#1063`, `#1073` are absent from it).
- Every comment says why, not what, and carries what would check its claim; two lines inline is the
  budget `tools/check_comment_length.py` enforces on `.py`, and a longer block opens with
  `long-comment: <reason>`.
- Everything in English; commit message carries no `feat:`/`fix:` prefix and ends with
  `Closes #1059`, `#1060`, `#1061`, `#1062`, `#1067`, `#1081`.
- `python3` on this machine; `dotnet` only under `./.dotnet-guarded`.
- Markdown must pass `npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"`.

---

### Task 1: `stage_doc_inputs.py` copies what MSBuild copies (#1062)

**Files:**

- Modify: `tools/stage_doc_inputs.py:61-94` (`_item_copies`, `plan`) and its docstring
- Test: `tools/tests/test_stage_doc_inputs.py`

**Interfaces:**

- Consumes: nothing from other tasks.
- Produces: `stage_doc_inputs.COPIED = ("Always", "PreserveNewest")`, the tuple of
  `CopyToOutputDirectory` values that stage a file. `plan(project: pathlib.Path) ->
  list[tuple[pathlib.Path, str]]` and `stage(project, output) -> int` keep their signatures.

- [x] **Step 1: Write the failing tests**

Append to `tools/tests/test_stage_doc_inputs.py`, and add the two `Never`/`false` items and the
wrapped `Exclude` to the existing `PROJECT` fixture so both shapes are in the one project the other
tests already read:

```python
NOT_COPIED = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <None Include="../../docs/guides/stats.md" CopyToOutputDirectory="Never" LinkBase="docs" />
    <None Include="../../docs/wiki-map.json" CopyToOutputDirectory="false" />
  </ItemGroup>
</Project>
"""

WRAPPED_EXCLUDE = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <None Include="../../docs/**/*.md"
          Exclude="../../docs/guides/**;
                   ../../docs/superpowers/**"
          CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
  </ItemGroup>
</Project>
"""


def _project(tmp_path: pathlib.Path, text: str, name: str) -> pathlib.Path:
    """One more project beside the fixture's, over the same docs tree."""
    _repository(tmp_path)
    project = tmp_path / f"tests/Lodestar.Stats.Tests/{name}.csproj"
    project.write_text(text, encoding="utf-8")
    return project


def test_never_and_false_stage_nothing(tmp_path):
    # MSBuild's copy targets match Always and PreserveNewest and nothing else; measured on a
    # synthetic project holding both shapes, whose output held neither file (#1062).
    assert plan(_project(tmp_path, NOT_COPIED, "NotCopied")) == []


def test_the_copy_value_is_read_as_msbuild_compares_it(tmp_path):
    # An MSBuild condition compares strings case-insensitively, so `preservenewest` copies (#1062).
    lowered = NOT_COPIED.replace('CopyToOutputDirectory="Never"', 'CopyToOutputDirectory="preservenewest"')
    assert [destination for _, destination in plan(_project(tmp_path, lowered, "Lowered"))] == ["docs/stats.md"]


def test_a_wrapped_exclude_excludes_its_second_pattern(tmp_path):
    # XML attribute-value normalisation turns the newline into a space and MSBuild trims each
    # entry of a `;` list, so `docs/superpowers/**` here is the same pattern as unwrapped (#1062).
    destinations = sorted(destination for _, destination in plan(_project(tmp_path, WRAPPED_EXCLUDE, "Wrapped")))
    assert destinations == [
        "docs/reference/stats.md",
        "docs/reference/stats/tails/normal.md",
        "docs/reference/stats/tests.md",
    ]
```

- [x] **Step 2: Run them to verify they fail**

Run: `python3 -m pytest tools/tests/test_stage_doc_inputs.py -v`
Expected: FAIL — `test_never_and_false_stage_nothing` returns two copies, and
`test_a_wrapped_exclude_excludes_its_second_pattern` also lists `docs/guides/stats.md` and
`docs/superpowers/specs/s.md`.

- [x] **Step 3: Make the two changes**

In `tools/stage_doc_inputs.py`, add the tuple beside `_split` and read it in `plan`:

```python
# The two values MSBuild's copy targets match, lowered because it matches them case-insensitively:
# measured on a built project, `preservenewest` and `ALWAYS` copy, `Never` and `false` do not (#1062).
COPIED = ("always", "preservenewest")
```

```python
        if "docs/" in include and (item.get("CopyToOutputDirectory") or "").strip().lower() in COPIED:
```

and in `_item_copies`, strip each `Exclude` pattern:

```python
    # Stripped because a wrapped attribute arrives with a space where its newline was, and
    # MSBuild trims each entry: unstripped, a second pattern excludes nothing (#1062).
    excludes = [
        (base / pattern.strip().replace("\\", "/")).resolve().as_posix()
        for pattern in (item.get("Exclude") or "").split(";")
        if pattern.strip()
    ]
```

Then replace the docstring's `every ``<None Include=... CopyToOutputDirectory>`` item` with
`every ``<None Include=...>`` item whose ``CopyToOutputDirectory`` is ``Always`` or
``PreserveNewest``` so the module says what it now does.

- [x] **Step 4: Run the tests to verify they pass**

Run: `python3 -m pytest tools/tests/test_stage_doc_inputs.py -v`
Expected: PASS, all of them — including
`test_every_item_lands_at_its_link_or_link_base` and
`test_staging_replaces_what_the_binaries_carried`, whose eight destinations and `count == 8` must
not move, because every item in the original fixture carries `PreserveNewest`.

- [x] **Step 5: Prove it against MSBuild on the real tree**

Run:

```bash
python3 - <<'EOF'
import pathlib, sys
sys.path.insert(0, "tools")
from stage_doc_inputs import plan
for project in sorted(pathlib.Path("tests").glob("Lodestar.*.Tests/*.csproj")):
    if "NetStandard" in project.name:
        continue
    print(project.name, len(plan(project)))
EOF
```

Expected: eighteen lines, each with a non-zero count — the same counts the docs-only path staged
before, since every `None` item in `tests/` carries `PreserveNewest`.

---

### Task 2: the guard's docstring and the summing test (#1081.5, #1081.6)

**Files:**

- Modify: `tools/check_doc_test_counts.py:2-16` (docstring only)
- Test: `tools/tests/test_check_doc_test_counts.py:67-76`

**Interfaces:**

- Consumes: nothing.
- Produces: nothing. `totals`, `report` and `main` keep their signatures and behaviour.

- [x] **Step 1: Strengthen the test that cannot fail for its own reason**

Replace `test_the_totals_of_several_assemblies_in_one_file_are_summed` with:

```python
def test_the_totals_of_several_assemblies_in_one_file_are_summed(tmp_path):
    (tmp_path / "Lodestar.A.Tests").mkdir()
    results = tmp_path / "Lodestar.A.Tests" / "results.xml"
    results.write_text(
        '<assemblies>'
        '<assembly name="one.dll" total="2"></assembly>'
        '<assembly name="two.dll" total="3"></assembly>'
        '</assemblies>',
        encoding="utf-8")

    # The exit code holds for any non-zero count, so `max` or a first-assembly read would pass it:
    # the sum is what this pins.
    assert GUARD.totals(results) == 5
    assert GUARD.main(["guard", str(tmp_path)]) == 0
```

- [x] **Step 2: Run it, and run it against a broken `totals`**

Run: `python3 -m pytest tools/tests/test_check_doc_test_counts.py -v`
Expected: PASS.

Then temporarily change `totals`' `sum(...)` to `max(...)`, run the same command, and expect
`test_the_totals_of_several_assemblies_in_one_file_are_summed` to FAIL with `assert 3 == 5`.
Restore `sum` — and delete `tools/__pycache__` before re-running, since the two edits differ by
three bytes and Python's bytecode cache is keyed on size and mtime to the second.

- [x] **Step 3: Narrow the docstring to what the guard covers**

In `tools/check_doc_test_counts.py`, drop the nested-namespace half of the first paragraph's
claim and say what a per-project zero actually covers, naming the limit in a paragraph of its
own:

```python
"""```

- [x] **Step 4: Run the guard's tests and the comment-length check**

Run: `python3 -m pytest tools/tests/test_check_doc_test_counts.py -q && python3 tools/check_comment_length.py`
Expected: PASS, and the comment check exits 0 (a docstring is not a comment block, so the new
paragraph costs nothing).

---

### Task 3: the `End analysis` step, and the harness that proves it (#1081.1–#1081.4)

**Files:**

- Modify: `.github/workflows/ci.yml:545-611` (the comment above `End analysis` and its `run`)
- Create: `tools/tests/test_end_analysis_retry.py`
- Modify: `tools/README.md` (the `tools/tests` inventory sentence, if it enumerates the files)

**Interfaces:**

- Consumes: nothing.
- Produces: `test_end_analysis_retry.script_of(workflow: pathlib.Path, job: str, step: str) -> str`,
  the step's `run` with `${{ runner.temp }}` substituted, used only inside that module.

- [x] **Step 1: Write the harness, against the step as it stands**

Create `tools/tests/test_end_analysis_retry.py`:

```python
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
import time

import pytest
import yaml

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
        ["bash", "-c", script_of(CI, "build-test", "End analysis")],
        capture_output=True, text=True, check=False,
        env={**os.environ,
             "PATH": f"{stubs}:{os.environ['PATH']}",
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
            ["bash", "-c", script_of(CI, "build-test", "End analysis")],
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
```

- [x] **Step 2: Run it against the current step**

Run: `python3 -m pytest tools/tests/test_end_analysis_retry.py -v`
Expected: PASS for the seven status and grouping tests, FAIL for
`test_the_scanners_output_reaches_the_log_while_it_is_still_running` with "the step printed
nothing until the scanner had exited" — the step redirects to a log and `cat`s it after the
scanner exits, so nothing reaches the group while it runs. The other seven pass on the step as it
stands, which is the point: they are the regression guard, and Step 4 proves they bite.

- [x] **Step 3: Rewrite the step**

In `.github/workflows/ci.yml`, the comment above `End analysis` and the step's `run` become — the
header no longer promises three attempts for a gate failure, the pipeline keeps the scanner's
status under `pipefail`, and the refusal names what else a gate measures:

```yaml
      # Three attempts, two then five minutes apart, and only for an outage: SonarCloud answered
      # `Error 500 ... Please try again` on its own project-data call for over an hour on
      # 2026-09-17, in windows longer than a minute, reddening a required check on green work. A
      # failed quality gate exits non-zero too and is this repository's designed refusal, so it is
      # matched below and fails on the first attempt (#1063).
      - name: End analysis
        if: env.SONAR == 'true'
        env:
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
        run: |
          # pipefail, so the `if` below is worth the scanner's status and not tee's: without it a
          # failed analysis -- a failed quality gate included -- reports as successful on every run,
          # which tools/tests/test_end_analysis_retry.py runs this step against a fake scanner to
          # prove it does not (#1081).
          set -o pipefail
          log="${{ runner.temp }}/end-analysis.log"
          for attempt in 1 2 3; do
            echo "::group::End analysis, attempt $attempt of 3"
            # Through tee rather than redirected to the log and printed after: this step takes up to
            # twenty minutes and cancel-in-progress cancels it on the next push, which left the
            # group holding nothing the scanner had printed (#1081).
            if "${{ runner.temp }}/scanner/dotnet-sonarscanner" end /d:sonar.token="$SONAR_TOKEN" 2>&1 | tee "$log"; then
              echo "::endgroup::"
              exit 0
            fi
            echo "::endgroup::"
            # A failed quality gate is this repository's designed refusal, and `end` exits non-zero
            # for it too, so retrying spent seven minutes of sleep and two more analyses of the same
            # report on every red pull request (#1063). Only the outage is retried.
            if grep -qi "quality gate status: failed" "$log"; then
              echo "::error::The quality gate failed on this analysis: a finding, the new-code coverage or the duplication in what this commit changes is below it."
              exit 1
            fi
            if [ "$attempt" != 3 ]; then
              # 2 then 5 minutes: measured on 2026-09-17, a minute apart left all three attempts
              # inside one outage window, while analyses a few minutes apart went through.
              wait=$([ "$attempt" = 1 ] && echo 120 || echo 300)
              echo "::warning::End analysis failed on attempt $attempt; retrying in ${wait}s."
              sleep "$wait"
            fi
          done
          echo "::error::End analysis failed three times."
          exit 1
```

- [x] **Step 4: Run the harness against the rewritten step, then against a broken one**

Run: `python3 -m pytest tools/tests/test_end_analysis_retry.py -v`
Expected: PASS, all of them.

Then delete the `set -o pipefail` line from the step, run the same command, and expect **five**
failures — `test_a_failed_quality_gate_fails_at_once`,
`test_an_outage_is_retried_and_the_analysis_goes_through`, `test_three_outages_fail` and both
`test_a_failing_analysis_never_exits_zero` cases — every one of them a status of 0 where the
scanner failed. That is the bug the harness exists for. Restore the line.

- [x] **Step 5: Keep the workflow tests and the README inventory green**

Run: `python3 -m pytest tools/tests -q && python3 tools/check_comment_length.py`
Expected: PASS. `tools/tests/test_readme_covers_the_tools.py` asserts `tools/README.md` documents
each tool; if it fails for the new test file, add the sentence it asks for rather than an
exemption.

---

### Task 4: the three counts, the netstandard claim and the two upload comments (#1059, #1060, #1061)

**Files:**

- Modify: `.github/workflows/ci.yml:56-57`, `CONTRIBUTING.md:91`, `tools/README.md:797-798`
- Modify: `CLAUDE.md:34`, `CLAUDE.md:333-336`
- Modify: `.github/workflows/release.yml:175`, `.github/workflows/bench-nightly.yml:403`

**Interfaces:**

- Consumes: nothing.
- Produces: nothing executable; `tools/check_claude_md_packages.py` still has to pass over the
  edited `CLAUDE.md`.

- [x] **Step 1: Say 18, in the three places**

`.github/workflows/ci.yml:56-57`:

```yaml
      # A pull request that skips `build-test` runs its documentation tests here: 18
      # ReferenceDocumentationTests classes, one per package, read docs/**/*.md, and one caught
      # #859's missing link.
```

`CONTRIBUTING.md:91`:

```markdown
instead — 18 `ReferenceDocumentationTests` classes, one per package, read `docs/**/*.md`. It runs them whenever the
```

`tools/README.md:797-798`:

```markdown
([#997](https://github.com/CyrilB1531/lodestar/issues/997)) run on either. Those tests stay because 18
`ReferenceDocumentationTests` classes, one per package, read `docs/**/*.md`: one caught a missing
reference link in
```

- [x] **Step 2: Check the number you just wrote**

Run: `grep -rl "class ReferenceDocumentationTests" tests/*/Documentation/*.cs | grep -vc NetStandard`
Expected: `18`.

Run: `grep -rn "ReferenceDocumentationTests" .github/workflows/ci.yml CONTRIBUTING.md tools/README.md`
Expected: no line holding `21`.

- [x] **Step 3: Make CLAUDE.md's reference-gate claim true on both paths**

`CLAUDE.md:34`, the `docs/reference/` row's source column:

```markdown
| `docs/reference/` | the exported types and public methods of the namespaces `docs/wiki-map.json` declares covered, replayed against both target frameworks' assemblies — against net10.0's alone on a pull request that skips the build, which runs them on the binaries `main` staged for its base commit ([#1059](https://github.com/CyrilB1531/lodestar/issues/1059)) | what each function is for, entry by entry — declaration, parameters, returns, example, remarks |
```

`CLAUDE.md:333-336`, the reference-gate bullet:

```markdown
- **The reference gate.** A new public type or method in a namespace listed in
  `docs/wiki-map.json`'s `covered` table needs an entry in its package's
  reference page under `docs/reference/`, checked against both target
  frameworks' assemblies — a signature that drifts from its documentation fails
  CI rather than a reader. A pull request that skips the build runs the same
  tests against net10.0's assemblies alone, because those are the ones `main`
  staged for it ([#1059](https://github.com/CyrilB1531/lodestar/issues/1059));
  the mirrors judge such a page on the next push to `main`. Only the namespaces
  `covered` names are enforced; the rest of the surface waits on the reference
  page that has not been written yet.
```

- [x] **Step 4: Make the two upload comments say what was measured**

`.github/workflows/release.yml:175` and `.github/workflows/bench-nightly.yml:403`, both replacing
`# A rerun re-uploads into the same run, which the action refuses by default (#997).`:

```yaml
          # A rerun re-uploads into the same run and the default keeps both, leaving two artifacts
          # of one name -- measured on main, two doc-tests-net10 artifacts on one run (#1060).
```

- [x] **Step 5: Check the claim is now single-voiced**

Run: `grep -rn "re-uploads into the same run" .github/workflows/`
Expected: three lines (`ci.yml`, `release.yml`, `bench-nightly.yml`), none of them saying the
action refuses anything.

Run: `python3 tools/check_claude_md_packages.py && npx --yes --ignore-scripts markdownlint-cli2@0.23.2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"`
Expected: both exit 0.

---

### Task 5: the next-revision invariant in writing (#1067)

**Files:**

- Modify: `CONTRIBUTING.md:421-429` (*Releasing*)
- Modify: `src/Lodestar.Cluster/Version.props:7-8`,
  `src/Lodestar.Decomposition/Version.props:22-23`,
  `src/Lodestar.Preprocessing/Version.props:7-8`

**Interfaces:**

- Consumes: nothing.
- Produces: nothing. No `<Lodestar*Version>` value changes.

- [x] **Step 1: State the invariant once, in *Releasing***

`CONTRIBUTING.md`'s *Releasing* opens with the invariant and then the two steps in order:

```markdown
### Releasing

Versions are declared per package in `src/<Package>/Version.props` and nowhere
else, and **`main` carries the next revision rather than the published one**: a
package released at `0.2.0` reads `0.2.1` there, so every branch packs and every
sample restores a number nuget.org does not hold — it is immutable, and a
collision makes two different assemblies answer to one identity. A feature pull
request therefore never touches `Version.props`; it lands on a number already
ahead of the feed.

To release one: set that file to the version being cut — the number `main`
carries when the release is a revision, a larger one when the change earns a
minor or a major — land it on `main`, then tag `<PackageId>/v<Version>` (for
example `Lodestar.Fuzzy/v0.3.0`). The workflow compares the tag against the
declared version and refuses to publish if they disagree. The tag chooses
*which* release to cut; it does not set the number. Add the entry under a
per-package heading in `CHANGELOG.md`, and close the release issue by bumping
the revision again, which puts `main` back ahead of the feed.
```

- [x] **Step 2: Make the three `Version.props` comments agree**

In each of the three, the sentence that said the number "moves only when a release is cut …, never
in the pull request that adds the work" becomes (`0.2.0` in `Lodestar.Decomposition`):

```xml
    0.1.0 is the published version and the number below is the next revision: a
    release pull request sets the version being cut, and the release issue closes
    by bumping the revision again (CONTRIBUTING.md, Releasing). A pull request
    that adds work never touches it.
```

and the sentence that follows — each file's edges, or that it has none — starts with the package's
own name rather than an `It` that now reads as the version.

- [x] **Step 3: Check no version moved and the floors still agree**

Run: `git diff -U0 src/ | grep -E "^[-+].*<Lodestar[A-Za-z.]*Version>"`
Expected: no output.

Run: `python3 tools/check_version_floor.py && python3 tools/check_unreleased.py`
Expected: both exit 0.

- [x] **Step 4: MSBuild still parses the three files**

Run: `./.dotnet-guarded dotnet msbuild src/Lodestar.Cluster -getProperty:LodestarClusterVersion`
Expected: `0.1.1`, with no error — a `--` inside an MSBuild comment is an error in every project
that imports it, so the new prose must carry none.

---

### Task 6: the whole gate, then one commit

**Files:** none.

- [x] **Step 1: Every check script, not a subset**

Run:

```bash
for tool in tools/check_*.py; do echo "== $tool"; python3 "$tool" || echo "FAILED $tool"; done
python3 tools/check_repeated_literals.py --base origin/main
python3 -m pytest tools/tests -q
```

Expected: every script exits 0 (`check_version_floor.py` without `--check-feed` is offline;
`check_sample_coverage.py` and `check_nuspec_dependencies.py` may need an argument — pass what
their `--help` asks for rather than skipping them), and the whole `tools/tests` suite passes.

- [x] **Step 2: The documentation tests, which read the prose this pull request edits**

Run: `./.dotnet-guarded dotnet test Lodestar.slnx -c Release --filter "FullyQualifiedName~.Documentation."`
Expected: a non-zero test count and 0 failures. `LodestarUseProjectRefs` must be unset.

- [x] **Step 3: Markdown lint and the staging plan on the real tree**

Run:

```bash
npx --yes --ignore-scripts markdownlint-cli2@0.23.2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
python3 tools/stage_doc_inputs.py tests/Lodestar.Stats.Tests/Lodestar.Stats.Tests.csproj "$(mktemp -d)"
```

Expected: lint clean, and the stager reports the same file count it reported before Task 1.

- [ ] **Step 4: One commit**

```bash
git add -A .github CLAUDE.md CONTRIBUTING.md tools docs/superpowers src
git commit -m "$(cat <<'EOF'
Retry only an outage with its output live, stage what MSBuild stages, and say 18 where the tree holds 18

Closes #1059
Closes #1060
Closes #1061
Closes #1062
Closes #1067
Closes #1081
EOF
)"
```

- [ ] **Step 5: Sync with main and open the pull request**

```bash
git fetch origin && git rebase origin/main
gh pr create --fill --milestone "Next release"
```

Expected: a clean rebase; re-run `python3 -m pytest tools/tests -q` after it, since a rebase that
merges cleanly can still leave a tool disagreeing with a file it reads.

## Self-Review

**Spec coverage:** #1062 → Task 1. #1081.5 and #1081.6 → Task 2. #1081.1–.4 → Task 3. #1061, #1059
and #1060 → Task 4. #1067 → Task 5. The spec's *Out of scope* asks for nothing.

**Placeholders:** none — every code step carries the text to write, and every verification step
carries its command and its expected output.

**Type consistency:** `COPIED` is defined in Task 1 and read nowhere else; `script_of` and
`harness` are defined and used inside Task 3's one new file; `GUARD.totals` in Task 2 is the
existing function, unchanged.
