"""check_project_docs.py: every project carries the documents #1133 gave it.

A fixture tree stands in for the repository, so each rule is shown failing on its own; the last
test runs the guard on the tree itself.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_project_docs as guard  # noqa: E402

PACKAGE = "Lodestar.A"


def _write(path: Path, text: str = "text\n") -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text, encoding="utf-8")


def _tree(root: Path) -> Path:
    src = root / "src" / PACKAGE
    _write(src / "Version.props", "<Project />\n")
    _write(src / "README.md")
    _write(src / "performance.md")
    _write(src / "CHANGELOG.md", f"# Changelog — {PACKAGE}\n\n## [Unreleased]\n")
    _write(root / "samples" / "S" / "S.csproj", "<Project />\n")
    _write(root / "samples" / "S" / "README.md")
    _write(root / "bench" / "B" / "B.csproj", "<Project />\n")
    _write(root / "bench" / "B" / "README.md")
    _write(root / "tests" / "README.md")
    _write(root / "tests" / f"{PACKAGE}.Tests" / f"{PACKAGE}.Tests.csproj", "<Project />\n")
    _write(root / "tests" / f"{PACKAGE}.Tests" / "README.md")
    _write(root / "tests" / f"{PACKAGE}.NetStandard.Tests" / "mirror.csproj", "<Project />\n")
    _write(root / "README.md", f"[{PACKAGE}](src/{PACKAGE}/README.md)\n")
    _write(root / "CHANGELOG.md", f"# Changelog\n\n- [`{PACKAGE}`](src/{PACKAGE}/CHANGELOG.md)\n")
    _write(root / "docs" / "guides" / "performance.md",
           f"| [`{PACKAGE}`](../../src/{PACKAGE}/performance.md) | none yet |\n")
    return root


def test_a_complete_tree_passes(tmp_path):
    assert guard.findings(_tree(tmp_path)) == []


def test_each_missing_package_document_is_named(tmp_path):
    root = _tree(tmp_path)
    for name in ("README.md", "CHANGELOG.md", "performance.md"):
        (root / "src" / PACKAGE / name).unlink()

    missing = [f for f in guard.findings(root) if "is missing" in f]

    assert missing == [f"src/{PACKAGE}/{name}: is missing"
                       for name in ("README.md", "CHANGELOG.md", "performance.md")]


def test_a_sample_a_benchmark_and_a_suite_need_a_readme(tmp_path):
    root = _tree(tmp_path)
    for path in ("samples/S/README.md", "bench/B/README.md", f"tests/{PACKAGE}.Tests/README.md",
                 "tests/README.md"):
        (root / path).unlink()

    assert sorted(guard.findings(root)) == sorted([
        "samples/S/README.md: is missing", "bench/B/README.md: is missing",
        f"tests/{PACKAGE}.Tests/README.md: is missing", "tests/README.md: is missing"])


def test_a_mirror_needs_no_readme(tmp_path):
    root = _tree(tmp_path)
    assert not (root / "tests" / f"{PACKAGE}.NetStandard.Tests" / "README.md").exists()
    assert guard.findings(root) == []


def test_each_index_must_link_the_package(tmp_path):
    root = _tree(tmp_path)
    for index in ("README.md", "CHANGELOG.md", "docs/guides/performance.md"):
        _write(root / index, "# nothing linked\n")

    assert sorted(guard.findings(root)) == sorted([
        f"README.md: does not link src/{PACKAGE}/README.md",
        f"CHANGELOG.md: does not link src/{PACKAGE}/CHANGELOG.md",
        f"docs/guides/performance.md: does not link src/{PACKAGE}/performance.md"])


def test_the_root_changelog_holds_no_entry(tmp_path):
    root = _tree(tmp_path)
    _write(root / "CHANGELOG.md",
           f"# Changelog\n\n- [`{PACKAGE}`](src/{PACKAGE}/CHANGELOG.md)\n- Fixed a bug. (#1)\n")

    assert guard.findings(root) == [
        "CHANGELOG.md:4: an entry; it belongs in its package's src/<Package>/CHANGELOG.md"]


def test_a_package_changelog_opens_with_its_title_and_unreleased(tmp_path):
    root = _tree(tmp_path)
    _write(root / "src" / PACKAGE / "CHANGELOG.md", "# Changelog\n\n## [0.1.0] — 2026-09-24\n")

    assert sorted(guard.findings(root)) == sorted([
        f"src/{PACKAGE}/CHANGELOG.md:1: must open '# Changelog — {PACKAGE}'",
        f"src/{PACKAGE}/CHANGELOG.md: has no '## [Unreleased]' section"])


def test_the_repository_holds(capsys):
    assert guard.main([]) == 0
    assert "ok" in capsys.readouterr().out
