"""check_gpu_tests_force_cpu.py's guard on a test that uses whatever device it finds.

GpuContext.Create() falls back to the CPU accelerator where there is no card, which is
deliberate and is also the trap: a bare Create() passes on a developer machine using the
GPU and on a runner using the processor, so the suite asserts different things in
different places and both are green.

The first test below is the one that would have caught it. The rest cover what the guard
must not report: a forced call, a benchmark (out of scope on purpose), and the exempt
test whose subject is the preferred device.
"""

from __future__ import annotations

import sys
from pathlib import Path

import pytest

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_gpu_tests_force_cpu as guard  # noqa: E402


def _suite(monkeypatch, tmp_path, body, name="Lodestar.Gpu.Tests"):
    """Writes one source file into a fake GPU suite and points the guard at it."""
    suite = tmp_path / "tests" / name
    suite.mkdir(parents=True)
    (suite / "SomeTests.cs").write_text(body, encoding="utf-8")
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    monkeypatch.setattr(guard, "SUITES", tmp_path / "tests")
    return suite


def test_a_bare_create_is_reported(monkeypatch, tmp_path):
    """The case the guard exists for: green either way, asserting different things."""
    _suite(monkeypatch, tmp_path, """
    public void Some_test()
    {
        using var context = GpuContext.Create();
    }
""")

    found = guard.findings()

    assert len(found) == 1
    assert "Some_test" in found[0]
    assert "preferCpu" in found[0]


def test_a_forced_create_is_not_reported(monkeypatch, tmp_path):
    _suite(monkeypatch, tmp_path, """
    public void Some_test()
    {
        using var context = GpuContext.Create(preferCpu: true);
    }
""")

    assert guard.findings() == []


def test_an_exempt_test_is_not_reported(monkeypatch, tmp_path):
    """One test's subject *is* the preferred device; forcing would remove it."""
    name = next(iter(guard.EXEMPT))
    _suite(monkeypatch, tmp_path, f"""
    public void {name}()
    {{
        using var context = GpuContext.Create();
    }}
""")

    assert guard.findings() == []


def test_the_mirror_suite_is_scanned_too(monkeypatch, tmp_path):
    """The netstandard mirror links the same sources, and the glob has to reach it."""
    _suite(monkeypatch, tmp_path, """
    public void Some_test()
    {
        using var context = GpuContext.Create();
    }
""", name="Lodestar.Gpu.NetStandard.Tests")

    assert len(guard.findings()) == 1


def test_a_build_output_is_skipped(monkeypatch, tmp_path):
    """obj/ holds generated copies of the same sources, which would double every finding."""
    suite = _suite(monkeypatch, tmp_path, """
    public void Some_test()
    {
        using var context = GpuContext.Create(preferCpu: true);
    }
""")
    generated = suite / "obj" / "Release"
    generated.mkdir(parents=True)
    (generated / "Copy.cs").write_text("GpuContext.Create();", encoding="utf-8")

    assert guard.findings() == []


def test_no_suite_at_all_is_reported(monkeypatch, tmp_path):
    """A guard watching nothing is worse than no guard: it reads as a pass."""
    (tmp_path / "tests").mkdir(parents=True)
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    monkeypatch.setattr(guard, "SUITES", tmp_path / "tests")

    found = guard.findings()

    assert len(found) == 1
    assert "watching nothing" in found[0]


def test_the_repository_itself_passes():
    """The guard's own subject, which is what CI runs it for."""
    assert guard.findings() == []


@pytest.mark.parametrize("argument", ["--help", "-h"])
def test_help_prints_and_exits_zero(argument, monkeypatch, capsys):
    monkeypatch.setattr(sys, "argv", ["check_gpu_tests_force_cpu.py", argument])

    assert guard.main() == 0
    assert "CPU accelerator" in capsys.readouterr().out


def test_an_unexpected_argument_exits_two(monkeypatch):
    monkeypatch.setattr(sys, "argv", ["check_gpu_tests_force_cpu.py", "--wat"])

    assert guard.main() == 2
