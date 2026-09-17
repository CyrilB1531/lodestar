"""The docs-only answer takes the reduced CI path only when it is safe (#857)."""

from __future__ import annotations

import pathlib
import subprocess
import sys

import pytest

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parents[1]))

from docs_only import docs_only  # noqa: E402

SCRIPT = pathlib.Path(__file__).resolve().parents[1] / "docs_only.py"


@pytest.mark.parametrize(
    ("paths", "expected"),
    [
        (["docs/guides/performance.md"], True),
        (["CHANGELOG.md", "docs/decisions/0142-x.md", "README.md"], True),
        # A rename lists both names, so a source file renamed to Markdown is not docs-only.
        (["docs/moved.md", "src/Lodestar.Text/Moved.cs"], False),
        (["docs/guides/performance.md", "docs/wiki-map.json"], False),
        (["assets/icon.png"], False),
        (["docs/README.MD"], False),
        (["docs/notes.md.bak"], False),
        ([], False),
        (["", "  "], False),
    ],
)
def test_docs_only(paths, expected):
    assert docs_only(paths) is expected


def test_the_script_reads_one_path_per_line():
    result = subprocess.run(
        [sys.executable, str(SCRIPT)],
        input="docs/a.md\nCHANGELOG.md\n",
        capture_output=True,
        text=True,
        check=True,
    )
    assert result.stdout.strip() == "true"


def test_the_script_answers_false_on_no_input():
    result = subprocess.run(
        [sys.executable, str(SCRIPT)], input="", capture_output=True, text=True, check=True
    )
    assert result.stdout.strip() == "false"
