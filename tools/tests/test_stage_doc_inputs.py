"""The staged docs land where MSBuild copies them (#997)."""

from __future__ import annotations

import pathlib
import sys

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parents[1]))

from stage_doc_inputs import plan, stage  # noqa: E402

PROJECT = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <None Include="../../docs/reference/stats.md" CopyToOutputDirectory="PreserveNewest" Link="reference/stats.md" />
    <None Include="../../docs/reference/stats/**/*.md" CopyToOutputDirectory="PreserveNewest" LinkBase="reference" />
    <None Include="../../docs/wiki-map.json" CopyToOutputDirectory="PreserveNewest" />
    <None Include="../../docs/**/*.md" Exclude="../../docs/superpowers/**" CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
    <None Include="../../tests/oracles/x.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
</Project>
"""


def _repository(tmp_path: pathlib.Path) -> pathlib.Path:
    for relative in [
        "docs/reference/stats.md",
        "docs/reference/stats/tests.md",
        "docs/reference/stats/tails/normal.md",
        "docs/reference/stats/tails/normal.json",
        "docs/guides/stats.md",
        "docs/superpowers/specs/s.md",
        "docs/wiki-map.json",
    ]:
        path = tmp_path / relative
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(relative, encoding="utf-8")
    project = tmp_path / "tests/Lodestar.Stats.Tests/Lodestar.Stats.Tests.csproj"
    project.parent.mkdir(parents=True)
    project.write_text(PROJECT, encoding="utf-8")
    return project


def test_every_item_lands_at_its_link_or_link_base(tmp_path):
    destinations = sorted(destination for _, destination in plan(_repository(tmp_path)))
    assert destinations == [
        "docs/guides/stats.md",
        "docs/reference/stats.md",
        "docs/reference/stats/tails/normal.md",
        "docs/reference/stats/tests.md",
        "reference/stats.md",
        "reference/tails/normal.md",
        "reference/tests.md",
        "wiki-map.json",
    ]


def test_staging_replaces_what_the_binaries_carried(tmp_path):
    project = _repository(tmp_path)
    output = tmp_path / "out"
    stale = output / "reference/removed.md"
    stale.parent.mkdir(parents=True)
    stale.write_text("stale", encoding="utf-8")

    count = stage(project, output)

    assert count == 8
    assert not stale.exists()
    assert (output / "reference/stats.md").read_text(encoding="utf-8") == "docs/reference/stats.md"
