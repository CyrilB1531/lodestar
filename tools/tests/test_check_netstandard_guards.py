"""The guard's own tests, over the shapes that actually reached this repository.

Issue #529 exists because ``Lodestar.Text`` and ``Lodestar.Decomposition`` ran 832
tests against the net10.0 ``Lodestar.Abstractions`` while reporting green. Both
fixtures below are those two projects' real shapes -- the broken one as committed,
the fixed one as this branch leaves it -- rather than examples someone invented.
"""
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_netstandard_guards as guard  # noqa: E402

GUARD_FILE = "NetStandardAssemblyGuardTests.cs"

# Lodestar.Text's own dependency line, verbatim from src/Lodestar.Text/Lodestar.Text.csproj.
SRC_WITH_DEPENDENCY = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <PackageReference Include="Lodestar.Abstractions" />
  </ItemGroup>
</Project>
"""

SRC_STANDALONE = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <PackageReference Include="PolySharp" />
  </ItemGroup>
</Project>
"""

# The mirror as it stood before #529: its own library pinned, the dependency not.
MIRROR_UNPINNED = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="../../src/Lodestar.Text/Lodestar.Text.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
  </ItemGroup>
</Project>
"""

MIRROR_PINNED = """<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="../../src/Lodestar.Text/Lodestar.Text.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Abstractions/Lodestar.Abstractions.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
  </ItemGroup>
</Project>
"""


def _tree(tmp_path, *, source: str, mirror: str, with_guard: bool) -> Path:
    """Builds a repository shaped the way the guard walks one, and points it there."""
    (tmp_path / "src" / "Lodestar.Text").mkdir(parents=True)
    (tmp_path / "src" / "Lodestar.Text" / "Lodestar.Text.csproj").write_text(source)

    mirror_dir = tmp_path / "tests" / "Lodestar.Text.NetStandard.Tests"
    mirror_dir.mkdir(parents=True)
    (mirror_dir / "Lodestar.Text.NetStandard.Tests.csproj").write_text(mirror)
    if with_guard:
        (mirror_dir / GUARD_FILE).write_text("// guard")

    guard.ROOT = tmp_path
    return mirror_dir


def test_a_pinned_mirror_with_a_guard_passes(tmp_path):
    _tree(tmp_path, source=SRC_WITH_DEPENDENCY, mirror=MIRROR_PINNED, with_guard=True)

    assert guard.main() == 0


def test_an_unpinned_dependency_is_reported(tmp_path):
    mirror = _tree(tmp_path, source=SRC_WITH_DEPENDENCY, mirror=MIRROR_UNPINNED, with_guard=True)

    failures = guard.failures_in(mirror)

    assert len(failures) == 1
    assert "Lodestar.Abstractions" in failures[0]
    assert "SetTargetFramework does not cross a PackageReference" in failures[0]


def test_a_missing_guard_file_is_reported(tmp_path):
    mirror = _tree(tmp_path, source=SRC_WITH_DEPENDENCY, mirror=MIRROR_PINNED, with_guard=False)

    failures = guard.failures_in(mirror)

    assert len(failures) == 1
    assert GUARD_FILE in failures[0]


def test_both_failures_are_reported_together(tmp_path):
    """Stopping at the first would hide the second behind a fix for it."""
    mirror = _tree(tmp_path, source=SRC_WITH_DEPENDENCY, mirror=MIRROR_UNPINNED, with_guard=False)

    assert len(guard.failures_in(mirror)) == 2


def test_a_library_with_no_lodestar_dependency_needs_no_pin(tmp_path):
    mirror = _tree(tmp_path, source=SRC_STANDALONE, mirror=MIRROR_UNPINNED, with_guard=True)

    assert guard.failures_in(mirror) == []


def test_the_real_repository_passes():
    """The check this branch exists to make true, run against the tree itself."""
    guard.ROOT = Path(__file__).resolve().parents[2]

    assert guard.main() == 0


def _library(tmp_path, package, targets, body=""):
    """A src/ csproj declaring a framework list, which the contract checks read."""
    path = tmp_path / "src" / package
    path.mkdir(parents=True, exist_ok=True)
    (path / f"{package}.csproj").write_text(
        f"<Project><PropertyGroup><TargetFrameworks>{targets}"
        f"</TargetFrameworks></PropertyGroup>{body}</Project>", encoding="utf-8")


def _mirror(tmp_path, package, pin):
    """A mirror carrying its guard file and one pinned project reference."""
    path = tmp_path / "tests" / f"{package}.NetStandard.Tests"
    path.mkdir(parents=True, exist_ok=True)
    (path / "NetStandardAssemblyGuardTests.cs").write_text("// guard", encoding="utf-8")
    (path / "mirror.csproj").write_text(
        f'<Project><ItemGroup><ProjectReference Include="../../src/{package}/{package}.csproj" '
        f'SetTargetFramework="TargetFramework={pin}" /></ItemGroup></Project>', encoding="utf-8")
    return path


def test_the_contract_is_read_per_package_not_from_a_constant(monkeypatch, tmp_path):
    """Lodestar.Gpu replays 2.1, because ILGPU ships no 2.0 asset (#444, decision 0103)."""
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    _library(tmp_path, "Lodestar.Gpu", "net10.0;netstandard2.1")
    mirror = _mirror(tmp_path, "Lodestar.Gpu", "netstandard2.1")

    assert guard.contract_of("Lodestar.Gpu") == "netstandard2.1"
    assert guard.failures_in(mirror) == []


def test_a_mirror_pinning_the_wrong_contract_is_reported(monkeypatch, tmp_path):
    """The finding that did not exist before: a 2.1 library mirrored as if it were 2.0.

    This passed vacuously under the old guard. Lodestar.Gpu has no Lodestar dependency
    to pin, so the loop over dependencies had nothing to iterate, nothing was reported,
    and the mirror's own assembly guard asserted a framework the assembly did not carry.
    """
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    _library(tmp_path, "Lodestar.Gpu", "net10.0;netstandard2.1")
    mirror = _mirror(tmp_path, "Lodestar.Gpu", "netstandard2.0")

    failures = guard.failures_in(mirror)

    assert len(failures) == 1
    assert "pins nothing to netstandard2.1" in failures[0]


def test_a_library_that_does_not_ship_its_mirror_contract_is_reported(monkeypatch, tmp_path):
    """The other direction: the table says 2.1 and the library never declares it."""
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    _library(tmp_path, "Lodestar.Gpu", "net10.0")
    mirror = _mirror(tmp_path, "Lodestar.Gpu", "netstandard2.1")

    failures = guard.failures_in(mirror)

    assert len(failures) == 1
    assert "MIRRORED" in failures[0]


def test_a_default_package_still_replays_netstandard2_0(monkeypatch, tmp_path):
    monkeypatch.setattr(guard, "ROOT", tmp_path)
    _library(tmp_path, "Lodestar.Text", "net10.0;netstandard2.0")
    mirror = _mirror(tmp_path, "Lodestar.Text", "netstandard2.0")

    assert guard.contract_of("Lodestar.Text") == "netstandard2.0"
    assert guard.failures_in(mirror) == []
