"""`skip_build.py` answers true only for a pull request the build cannot judge.

The adversarial inputs are the ones the delta review used against `docs_only.py` and
`format_needed.py`: renames, deletions, no extension, case, paths with spaces, a
directory that looks like a file, and a long list (#1073).
"""

from __future__ import annotations

import importlib.util
import pathlib
import re

import pytest

ROOT = pathlib.Path(__file__).resolve().parents[2]
SPEC = importlib.util.spec_from_file_location("skip_build", ROOT / "tools" / "skip_build.py")
TOOL = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(TOOL)


@pytest.mark.parametrize("paths", [
    ["README.md"],
    ["docs/guides/quickstart.md", "CHANGELOG.md"],
    [".github/workflows/release.yml"],
    [".github/workflows/bench-nightly.yml", ".github/workflows/wiki.yml"],
    [".github/workflows/classify-pull-request.yml", "docs/a.md"],
    [".github/dependabot.yml"],
    [".github/workflows/wiki.yml", "README.md"],
])
def test_a_pull_request_the_build_cannot_judge(paths):
    assert TOOL.skip_build(paths) is True


@pytest.mark.parametrize("paths", [
    [".github/workflows/ci.yml"],
    [".github/workflows/ci.yml", "docs/a.md"],
    [".github/workflows/ci.yml", ".github/workflows/release.yml"],
    ["src/Lodestar.Text/Distances/Levenshtein.cs"],
    ["src/Directory.Packages.props"],
    ["tools/docs_only.py"],
    [".github/actions/setup/action.yml"],
    [".github/workflows/nested/thing.yml"],
    [".github/CODEOWNERS"],
    ["assets/icon.png"],
    ["docs/wiki-map.json"],
    ["Lodestar.slnx"],
    ["tests/oracles/snowball_fr.json"],
])
def test_a_pull_request_the_build_must_judge(paths):
    assert TOOL.skip_build(paths) is False


def test_an_empty_list_takes_the_full_path():
    assert TOOL.skip_build([]) is False
    assert TOOL.skip_build(["", "   ", "\n"]) is False


def test_a_rename_counts_both_of_its_names():
    # The `changes` job prints the old name too, so a workflow renamed into a source file fails.
    assert TOOL.skip_build([".github/workflows/wiki.yml", "src/New.cs"]) is False
    assert TOOL.skip_build([".github/workflows/wiki.yml", ".github/workflows/release.yml"]) is True


def test_whitespace_around_a_path_is_ignored():
    assert TOOL.skip_build(["  docs/a.md  ", "\t.github/workflows/release.yml\n"]) is True


@pytest.mark.parametrize("path", ["docs/a name with spaces.md", "docs/naïve.md"])
def test_a_path_with_spaces_or_accents_is_read_by_its_suffix(path):
    assert TOOL.skip_build([path]) is True


def test_a_workflow_nobody_classified_takes_the_full_path():
    # The point of enumerating: a CodeQL gate added tomorrow must not inherit the reduced path.
    assert TOOL.skip_build([".github/workflows/codeql.yml"]) is False


def test_the_enumeration_names_no_workflow_that_is_gone():
    # A subset, deliberately: a workflow nobody classified takes the full path, which is the safe
    # answer. What must not survive is a name the directory no longer holds.
    directory = ROOT / ".github" / "workflows"
    found = {f".github/workflows/{path.name}" for path in directory.glob("*.y*ml")}
    listed = {path for path in TOOL.UNREAD if path.startswith(".github/workflows/")}

    assert listed <= found, (
        f"skip_build.py's UNREAD names {sorted(listed - found)}, which .github/workflows/ no longer "
        "holds. A stale name there classifies nothing; a renamed workflow needs its new name.")


def test_no_unread_path_would_need_dotnet_format():
    # Lint skips setup-dotnet on a workflow-only pull request: safe only while no UNREAD path is
    # one `dotnet format` reads, or the format check would run with no SDK.
    spec = importlib.util.spec_from_file_location("format_needed", ROOT / "tools" / "format_needed.py")
    formats = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(formats)

    assert not formats.format_needed(sorted(TOOL.UNREAD)), (
        "an UNREAD path is one dotnet format reads, so Lint would run the format check without an "
        "SDK. Either drop it from UNREAD, or give actions/setup-dotnet a wider condition.")


def test_no_workflow_the_gate_calls_is_classified_as_unread():
    # The hole this closes: `ci.yml` calling a reusable workflow makes that file part of the gate,
    # and a pull request touching it would otherwise skip the pipeline it drives.
    gate = (ROOT / TOOL.GATE).read_text(encoding="utf-8")
    called = {f".github/workflows/{name}" for name in re.findall(r"uses:\s+\./\.github/workflows/(\S+)", gate)}

    assert not (called & TOOL.UNREAD), (
        f"{sorted(called & TOOL.UNREAD)} is called by ci.yml and classified as unread. A change to "
        "it changes the gate, so it belongs on the full path.")


@pytest.mark.parametrize("path", ["README.MD", ".github/workflows/RELEASE.YML"])
def test_the_suffix_is_matched_as_written(path):
    # Case is not folded: git reports the path as it is tracked, so an upper-case
    # suffix is a file this repository does not have and the safe answer is the full path.
    assert TOOL.skip_build([path]) is False


def test_a_long_list_of_qualifying_paths_still_qualifies():
    assert TOOL.skip_build([f"docs/page-{i}.md" for i in range(150)]) is True


def test_one_source_file_among_many_documents_takes_the_full_path():
    paths = [f"docs/page-{i}.md" for i in range(150)] + ["src/Lodestar.Text/X.cs"]

    assert TOOL.skip_build(paths) is False


def test_the_gate_itself_is_named_rather_than_guessed():
    assert TOOL.GATE == ".github/workflows/ci.yml"
    assert TOOL.workflow_only(TOOL.GATE) is False


@pytest.mark.parametrize("paths", [
    [".github/workflows/release.yml"],
    [".github/workflows/bench-nightly.yml", ".github/dependabot.yml"],
])
def test_a_pull_request_with_no_markdown_at_all(paths):
    assert TOOL.workflows_only(paths) is True
    assert TOOL.skip_build(paths) is True


@pytest.mark.parametrize("paths", [
    ["docs/a.md"],
    ["docs/a.md", ".github/workflows/release.yml"],
    [".github/workflows/ci.yml"],
    ["src/X.cs"],
    [],
])
def test_one_markdown_file_is_enough_to_need_the_snippets(paths):
    # The narrower question: a guide's fence is compiled and run, so any Markdown runs that job.
    assert TOOL.workflows_only(paths) is False


def test_the_flag_selects_the_narrower_question(monkeypatch, capsys):
    monkeypatch.setattr("sys.stdin", iter(["docs/a.md\n", ".github/workflows/wiki.yml\n"]))
    assert TOOL.main(["skip_build.py", "--workflows-only"]) == 0
    assert capsys.readouterr().out.strip() == "false"

    monkeypatch.setattr("sys.stdin", iter([".github/workflows/wiki.yml\n"]))
    assert TOOL.main(["skip_build.py", "--workflows-only"]) == 0
    assert capsys.readouterr().out.strip() == "true"


def test_main_prints_the_answer(monkeypatch, capsys):
    monkeypatch.setattr("sys.stdin", iter(["docs/a.md\n", ".github/workflows/wiki.yml\n"]))

    assert TOOL.main(["skip_build.py"]) == 0
    assert capsys.readouterr().out.strip() == "true"
