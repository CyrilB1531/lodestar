"""The format check is skipped only when no C# input can have changed (#997)."""

from __future__ import annotations

import pathlib
import subprocess
import sys

import pytest

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parents[1]))

from format_needed import format_needed  # noqa: E402

SCRIPT = pathlib.Path(__file__).resolve().parents[1] / "format_needed.py"


@pytest.mark.parametrize(
    ("paths", "expected"),
    [
        (["docs/guides/performance.md"], False),
        (["CHANGELOG.md", "README.md"], False),
        (["tools/docs_only.py", "tools/tests/test_docs_only.py"], False),
        (["tests/oracles/fuzz.json"], False),
        ([".github/workflows/ci.yml"], False),
        (["src/Lodestar.Fuzzy/Fuzz.cs"], True),
        (["docs/guides/fuzzy.md", "src/Lodestar.Fuzzy/Fuzz.cs"], True),
        (["src/Lodestar.Text/Lodestar.Text.csproj"], True),
        (["src/Directory.Build.props"], True),
        (["Directory.Build.targets"], True),
        (["Lodestar.slnx"], True),
        ([".editorconfig"], True),
        (["tests/analyzers.globalconfig"], True),
        # A rename lists both names, so a C# file renamed away still counts.
        (["docs/moved.md", "src/Lodestar.Text/Moved.cs"], True),
        (["docs/Notes.cs.md"], False),
        ([], True),
        (["", "  "], True),
    ],
)
def test_format_needed(paths, expected):
    assert format_needed(paths) is expected


def test_the_script_reads_one_path_per_line():
    result = subprocess.run(
        [sys.executable, str(SCRIPT)],
        input="docs/a.md\ntools/x.py\n",
        capture_output=True,
        text=True,
        check=True,
    )
    assert result.stdout.strip() == "false"


def test_the_script_answers_true_on_no_input():
    result = subprocess.run(
        [sys.executable, str(SCRIPT)], input="", capture_output=True, text=True, check=True
    )
    assert result.stdout.strip() == "true"
