"""check_bench_map.py's guard on which projects the nightly actually measures.

#586: `Run them` named bench/Lodestar.Text.Benchmarks alone, so `*StatsBenchmarks*` was
passed as a filter to a project that does not declare the class. BenchmarkDotNet matches
nothing and exits 0, and the page still listed the class under "Classes re-run" -- run
34198050949 published a measurement that never happened.

The first test below is the one that would have caught it. The second is the reason the
guard reads the loop rather than the file: a substring test over the whole text is
satisfied by the comment above `Run them`, which names both projects in prose. Written
that way first, it passed with the loop emptied.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_bench_map  # noqa: E402

ROOT = Path(__file__).resolve().parents[2]

# Every directory CLASS_DIRS scans, which is what the loop has to name (#586, #569).
ALL = " ".join(d.relative_to(ROOT).as_posix() for d in check_bench_map.CLASS_DIRS)


def _nightly(monkeypatch, tmp_path, body):
    path = tmp_path / "bench-nightly.yml"
    path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "NIGHTLY", path)
    return path


def test_the_shipped_nightly_measures_every_project_that_can_declare_a_benchmark():
    assert check_bench_map.measured_project_findings() == []


def test_a_forgotten_project_is_a_finding(monkeypatch, tmp_path):
    _nightly(monkeypatch, tmp_path, "for project in bench/Lodestar.Text.Benchmarks; do\n")
    findings = check_bench_map.measured_project_findings()

    # One per directory the loop leaves out, so adding a benchmark project without
    # adding it to the nightly is a finding on the commit that adds it.
    assert len(findings) == len(check_bench_map.CLASS_DIRS) - 1
    assert any("bench/Lodestar.Stats.Benchmarks" in f for f in findings)
    assert any("bench/Lodestar.Survival.Benchmarks" in f for f in findings)


def test_naming_the_project_only_in_a_comment_is_not_running_it(monkeypatch, tmp_path):
    # The shape the first draft of this guard accepted, and the reason it reads the loop.
    _nightly(
        monkeypatch,
        tmp_path,
        "# StatsBenchmarks lives in bench/Lodestar.Stats.Benchmarks, so run it too.\n"
        "for project in bench/Lodestar.Text.Benchmarks; do\n",
    )
    findings = check_bench_map.measured_project_findings()

    assert len(findings) == len(check_bench_map.CLASS_DIRS) - 1
    assert any("bench/Lodestar.Stats.Benchmarks" in f for f in findings)


def test_no_loop_at_all_is_a_finding(monkeypatch, tmp_path):
    # Not silence: a nightly this file cannot read is one it cannot vouch for.
    _nightly(monkeypatch, tmp_path, "dotnet run -c Release --project bench/Whatever -- x\n")
    findings = check_bench_map.measured_project_findings()
    assert len(findings) == 1
    assert "no `for project in ...; do` loop" in findings[0]


def test_every_project_present_is_clean(monkeypatch, tmp_path):
    _nightly(monkeypatch, tmp_path, f"for project in {ALL}; do\n")
    assert check_bench_map.measured_project_findings() == []


def test_every_class_dir_the_gate_scans_is_a_real_directory():
    # CLASS_DIRS is what makes the guard above meaningful; a stale path would empty it.
    for directory in check_bench_map.CLASS_DIRS:
        assert directory.is_dir(), f"{directory.relative_to(ROOT)} does not exist"


def test_every_bench_project_is_in_the_solution():
    """#649: two of the five were not, and an analyser bump broke one of them unseen.

    `dotnet build Lodestar.slnx` is the only thing that compiles a benchmark project
    between nightly runs, so a project the solution does not list is one nothing checks
    until the night a change happens to select it.
    """
    assert check_bench_map.solution_findings() == []


def test_a_project_the_solution_omits_is_a_finding(monkeypatch, tmp_path):
    solution = tmp_path / "Lodestar.slnx"
    solution.write_text(
        '<Solution>\n  <Project Path="bench/Lodestar.Text.Benchmarks/'
        'Lodestar.Text.Benchmarks.csproj" />\n</Solution>\n', encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "SOLUTION", solution)
    findings = check_bench_map.solution_findings()

    # One per project under bench/ the solution leaves out -- all of them but the one
    # named above, whatever that set grows to.
    projects = sorted((check_bench_map.ROOT / "bench").glob("*/*.csproj"))
    assert len(findings) == len(projects) - 1
    assert any("Lodestar.Gpu.Benchmarks" in f for f in findings)


def test_a_missing_solution_is_a_finding(monkeypatch, tmp_path):
    monkeypatch.setattr(check_bench_map, "SOLUTION", tmp_path / "absent.slnx")
    assert check_bench_map.solution_findings() == ["absent.slnx: missing"]
