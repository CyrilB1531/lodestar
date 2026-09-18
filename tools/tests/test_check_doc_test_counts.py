"""`check_doc_test_counts.py` fails a documentation run that tested nothing.

The runner the docs-only path uses exits 0 when its namespace filter matches nothing
(#1054), so these assert on the count the result file records rather than on an exit code.
"""

from __future__ import annotations

import importlib.util
import pathlib

import pytest

ROOT = pathlib.Path(__file__).resolve().parents[2]

SPEC = importlib.util.spec_from_file_location("check_doc_test_counts", ROOT / "tools" / "check_doc_test_counts.py")
GUARD = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(GUARD)


def results(total: int) -> str:
    """One assembly element in the shape xunit v3's `-xml` writes."""
    return (
        '<?xml version="1.0" encoding="utf-8"?>\n'
        '<assemblies>\n'
        f'  <assembly name="X.Tests.dll" total="{total}" passed="{total}" failed="0" skipped="0">\n'
        '  </assembly>\n'
        '</assemblies>\n'
    )


def project(directory: pathlib.Path, name: str, total: int | None) -> None:
    (directory / name).mkdir()
    if total is not None:
        (directory / name / "results.xml").write_text(results(total), encoding="utf-8")


def test_a_run_with_tests_in_every_project_passes(tmp_path):
    project(tmp_path, "Lodestar.A.Tests", 4)
    project(tmp_path, "Lodestar.B.Tests", 1)

    assert GUARD.main(["guard", str(tmp_path)]) == 0


def test_a_suite_that_ran_nothing_fails(tmp_path):
    project(tmp_path, "Lodestar.A.Tests", 4)
    project(tmp_path, "Lodestar.B.Tests", 0)

    assert GUARD.main(["guard", str(tmp_path)]) == 1


def test_a_missing_result_file_fails(tmp_path):
    project(tmp_path, "Lodestar.A.Tests", 4)
    project(tmp_path, "Lodestar.B.Tests", None)

    assert GUARD.main(["guard", str(tmp_path)]) == 1


def test_a_directory_holding_no_project_fails(tmp_path):
    assert GUARD.main(["guard", str(tmp_path)]) == 1


def test_a_directory_that_does_not_exist_fails(tmp_path):
    assert GUARD.main(["guard", str(tmp_path / "nope")]) == 1


def test_the_totals_of_several_assemblies_in_one_file_are_summed(tmp_path):
    (tmp_path / "Lodestar.A.Tests").mkdir()
    (tmp_path / "Lodestar.A.Tests" / "results.xml").write_text(
        '<assemblies>'
        '<assembly name="one.dll" total="2"></assembly>'
        '<assembly name="two.dll" total="3"></assembly>'
        '</assemblies>',
        encoding="utf-8")

    assert GUARD.main(["guard", str(tmp_path)]) == 0


def test_no_argument_prints_the_usage(tmp_path, capsys):
    assert GUARD.main(["guard"]) == 2
    assert "documentation suite" in capsys.readouterr().out


def test_a_truncated_result_file_fails(tmp_path):
    (tmp_path / "Lodestar.A.Tests").mkdir()
    (tmp_path / "Lodestar.A.Tests" / "results.xml").write_text('<assemblies><assembly total="2"', encoding="utf-8")

    assert GUARD.main(["guard", str(tmp_path)]) == 1


@pytest.mark.parametrize("total", [0, 1, 250])
def test_totals_reads_the_attribute(tmp_path, total):
    path = tmp_path / "results.xml"
    path.write_text(results(total), encoding="utf-8")

    assert GUARD.totals(path) == total
