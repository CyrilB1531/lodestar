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
