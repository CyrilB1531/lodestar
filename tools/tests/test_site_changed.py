"""The site is rebuilt on a pull request only when something it reads moved (#1180)."""

from __future__ import annotations

import pathlib
import subprocess
import sys

import pytest

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parents[1]))

from site_changed import site_changed  # noqa: E402

SCRIPT = pathlib.Path(__file__).resolve().parents[1] / "site_changed.py"


@pytest.mark.parametrize(
    ("paths", "expected"),
    [
        (["docs/guides/survival-analysis.md"], True),
        (["docs/wiki-map.json"], True),
        (["docs/wiki/home.md"], True),
        (["src/Lodestar.Text/README.md"], True),
        (["src/Lodestar.Stats.TimeSeries/performance.md"], True),
        (["tools/build_site.py"], True),
        (["tools/build_wiki.py"], True),
        (["tools/site/docfx.json"], True),
        ([".config/dotnet-tools.json"], True),
        (["assets/icon.png"], True),
        ([".github/workflows/ci.yml"], True),
        (["src/Lodestar.Text/Levenshtein.cs"], False),
        (["src/Lodestar.Text/CHANGELOG.md"], False),
        (["src/Lodestar.Text/Sub/README.md"], False),
        (["tests/Lodestar.Text.Tests/LevenshteinTests.cs", "CHANGELOG.md"], False),
        (["tools/check_spec_status.py"], False),
        # A pull request whose files could not be listed is checked, never waved through.
        ([], True),
        (["", "  "], True),
    ],
)
def test_site_changed(paths: list[str], expected: bool) -> None:
    assert site_changed(paths) is expected


def test_the_command_line_reads_stdin_and_prints_the_answer() -> None:
    result = subprocess.run(
        [sys.executable, str(SCRIPT)],
        input="src/Lodestar.Text/Levenshtein.cs\ndocs/guides/quickstart.md\n",
        capture_output=True,
        text=True,
        check=False,
    )
    assert (result.returncode, result.stdout) == (0, "true\n")
