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

BOTH = "bench/Lodestar.Text.Benchmarks bench/Lodestar.Stats.Benchmarks"


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
    assert len(findings) == 1
    assert "bench/Lodestar.Stats.Benchmarks" in findings[0]


def test_naming_the_project_only_in_a_comment_is_not_running_it(monkeypatch, tmp_path):
    # The shape the first draft of this guard accepted, and the reason it reads the loop.
    _nightly(
        monkeypatch,
        tmp_path,
        "# StatsBenchmarks lives in bench/Lodestar.Stats.Benchmarks, so run it too.\n"
        "for project in bench/Lodestar.Text.Benchmarks; do\n",
    )
    findings = check_bench_map.measured_project_findings()
    assert len(findings) == 1
    assert "bench/Lodestar.Stats.Benchmarks" in findings[0]


def test_no_loop_at_all_is_a_finding(monkeypatch, tmp_path):
    # Not silence: a nightly this file cannot read is one it cannot vouch for.
    _nightly(monkeypatch, tmp_path, "dotnet run -c Release --project bench/Whatever -- x\n")
    findings = check_bench_map.measured_project_findings()
    assert len(findings) == 1
    assert "no `for project in ...; do` loop" in findings[0]


def test_both_projects_present_is_clean(monkeypatch, tmp_path):
    _nightly(monkeypatch, tmp_path, f"for project in {BOTH}; do\n")
    assert check_bench_map.measured_project_findings() == []


def test_every_class_dir_the_gate_scans_is_a_real_directory():
    # CLASS_DIRS is what makes the guard above meaningful; a stale path would empty it.
    for directory in check_bench_map.CLASS_DIRS:
        assert directory.is_dir(), f"{directory.relative_to(ROOT)} does not exist"
