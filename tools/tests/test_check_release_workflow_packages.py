"""check_release_workflow_packages.py's guard on the two hand-written release lists.

#610: cutting 0.6.0 (#609) found that four of the eight packages the milestone was
meant to publish could not be published at all. Lodestar.Cluster, Lodestar.Preprocessing,
Lodestar.Extensions.AI and Lodestar.Extensions.MathNet were absent from both
release-nuget-org.yml's `options:` and release.yml's `case` allow-list, so the first
refused the dispatch before the job started and the second answered the tag with
"Unknown package". Neither list is read by anything, which is why both rotted.
"""

from __future__ import annotations

import sys
from pathlib import Path

import pytest

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_release_workflow_packages as guard  # noqa: E402

TEXT = "Lodestar.Text"
FUZZY = "Lodestar.Fuzzy"
SURVIVAL = "Lodestar.Survival"

# The nesting the reader has to descend, `on:` included -- the key PyYAML would read as
# the boolean True under YAML 1.1, which is why this file has a reader of its own.
DISPATCH = """\
name: Publish to nuget.org (Trusted Publishing)

on:
  workflow_dispatch:
    inputs:
      package:
        description: 'Package to publish'
        required: true
        type: choice
        options:
{items}
      version:
        description: 'Version to publish'
        required: true

jobs:
  publish:
    runs-on: ubuntu-latest
"""

TAG = """\
name: Release

on:
  push:
    tags: ['Lodestar.*/v*']

jobs:
  publish:
    steps:
      - name: The tag matches the declared version
        run: |
          case "$PACKAGE" in
{arm}
            *) echo "::error::Unknown package '$PACKAGE'."; exit 1 ;;
          esac
"""


def _write(monkeypatch, tmp_path, name, attribute, body):
    path = tmp_path / name
    path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(guard, attribute, path)
    return path


@pytest.fixture
def workflows(monkeypatch, tmp_path):
    """A pair of release workflows and the src/ they are compared against.

    Call it with the two lists and what src/ declares; leaving a name out of one list
    is the #610 drift, and putting one in that src/ has no directory for is the other
    direction. Both halves are written here rather than pointed at the real tree, so a
    test says what it is about instead of depending on which packages exist today.
    """

    def build(options, accepted, declared):
        _write(monkeypatch, tmp_path, "release-nuget-org.yml", "DISPATCH",
               DISPATCH.format(items="\n".join(f"          - {name}" for name in options)))
        _write(monkeypatch, tmp_path, "release.yml", "TAG",
               TAG.format(arm=f"            {'|'.join(accepted)}) ;;"))
        monkeypatch.setattr(guard, "declared_packages", lambda: set(declared))

    return build


def test_the_shipped_workflows_list_what_the_shipped_src_declares():
    assert guard.findings() == []


def test_every_package_src_declares_is_in_both_lists():
    # The guard's own subject, stated against the real tree: the count in the ok line
    # is the number of directories, so a package added without a release entry moves it.
    declared = guard.declared_packages()
    assert declared, "src/ declares no package at all"
    options = guard.block_sequence(
        guard.DISPATCH.read_text(encoding="utf-8"), guard.OPTIONS_PATH)
    accepted, shape = guard.case_packages(guard.TAG.read_text(encoding="utf-8"))

    assert shape == []
    assert declared <= set(options)
    assert declared <= set(accepted)


def test_a_package_the_dispatch_list_omits_is_a_finding(workflows):
    workflows([TEXT, FUZZY], [TEXT, FUZZY, SURVIVAL], [TEXT, FUZZY, SURVIVAL])

    found = guard.findings()

    assert len(found) == 1
    assert "release-nuget-org.yml" in found[0]
    assert SURVIVAL in found[0]


def test_a_package_the_case_allow_list_omits_is_a_finding(workflows):
    workflows([TEXT, FUZZY, SURVIVAL], [TEXT, FUZZY], [TEXT, FUZZY, SURVIVAL])

    found = guard.findings()

    assert len(found) == 1
    assert "release.yml" in found[0]
    assert SURVIVAL in found[0]


def test_a_name_in_a_list_with_no_src_directory_is_a_finding(workflows):
    # The other direction: a list offering a release that cannot be packed, which is
    # what a renamed or removed package leaves behind.
    workflows([TEXT, FUZZY, SURVIVAL], [TEXT, FUZZY, SURVIVAL], [TEXT, FUZZY])

    found = guard.findings()

    assert len(found) == 2
    assert all(SURVIVAL in finding for finding in found)


def test_the_four_packages_0_6_0_could_not_publish_are_reported(workflows):
    # #610 itself: both lists stopped at twelve while src/ had grown to sixteen.
    declared = [TEXT, FUZZY, "Lodestar.Cluster", "Lodestar.Preprocessing"]
    workflows([TEXT, FUZZY], [TEXT, FUZZY], declared)

    found = guard.findings()

    assert len(found) == 2
    for finding in found:
        assert "Lodestar.Cluster" in finding
        assert "Lodestar.Preprocessing" in finding


def test_order_is_not_compared(workflows):
    # Both lists read in a presentation order of their own, and neither workflow cares.
    workflows([SURVIVAL, TEXT, FUZZY], [FUZZY, SURVIVAL, TEXT], [TEXT, FUZZY, SURVIVAL])

    assert guard.findings() == []


def test_a_backslash_wrapped_case_pattern_is_a_finding(monkeypatch, tmp_path, workflows):
    # The continuation keeps the indentation instead of joining the alternatives, so the
    # arm holds `|            Lodestar.Fuzzy`, which no tag can ever equal.
    workflows([TEXT, FUZZY], [TEXT, FUZZY], [TEXT, FUZZY])
    _write(monkeypatch, tmp_path, "release.yml", "TAG",
           TAG.format(arm=f"            {TEXT}| \\\n            {FUZZY}) ;;"))

    found = guard.findings()

    assert len(found) == 1
    assert "backslash" in found[0]


def test_no_options_list_at_all_is_a_finding(monkeypatch, tmp_path, workflows):
    # Not silence: a list this file cannot read is one it cannot vouch for.
    workflows([TEXT], [TEXT], [TEXT])
    _write(monkeypatch, tmp_path, "release-nuget-org.yml", "DISPATCH",
           "name: Publish\non:\n  workflow_dispatch:\n")

    found = guard.findings()

    assert len(found) == 1
    assert "on.workflow_dispatch.inputs.package.options" in found[0]


def test_no_case_allow_list_at_all_is_a_finding(monkeypatch, tmp_path, workflows):
    workflows([TEXT], [TEXT], [TEXT])
    _write(monkeypatch, tmp_path, "release.yml", "TAG",
           "name: Release\njobs:\n  publish:\n    steps:\n      - run: dotnet pack\n")

    found = guard.findings()

    assert len(found) == 1
    assert 'no `case "$PACKAGE" in` allow-list' in found[0]


def test_a_missing_workflow_is_a_finding(monkeypatch, tmp_path):
    monkeypatch.setattr(guard, "DISPATCH", tmp_path / "gone.yml")

    found = guard.findings()

    assert len(found) == 1
    assert found[0].endswith("gone.yml: missing")


def test_an_options_key_elsewhere_in_the_file_does_not_answer_for_this_one():
    # The reader descends the key path at exact indentation. A grep for `options:`
    # would take the first one and report the wrong list as the dispatch's.
    text = (
        "on:\n"
        "  workflow_call:\n"
        "    inputs:\n"
        "      package:\n"
        "        options:\n"
        "          - Lodestar.Wrong\n"
        "  workflow_dispatch:\n"
        "    inputs:\n"
        "      package:\n"
        "        options:\n"
        f"          - {TEXT}\n"
    )

    assert guard.block_sequence(text, guard.OPTIONS_PATH) == [TEXT]


def test_a_sequence_stops_at_the_end_of_its_own_block():
    # `version:` is the sibling key that follows `options:`; reading past it would
    # report the input's own settings as package names.
    text = DISPATCH.format(items=f"          - {TEXT}\n          - {FUZZY}")

    assert guard.block_sequence(text, guard.OPTIONS_PATH) == [TEXT, FUZZY]


def test_a_key_path_that_is_not_there_reads_as_absent():
    assert guard.block_sequence("on:\n  push:\n    tags: ['v*']\n", guard.OPTIONS_PATH) is None


def test_the_guard_imports_nothing_outside_the_standard_library():
    # The hook runs the guards through the contributor's own interpreter, not through
    # .venv-oracles, so a third-party import turns a missing package into a failed commit.
    source = Path(guard.__file__).read_text(encoding="utf-8")
    imported = {
        line.split()[1].split(".")[0]
        for line in source.splitlines()
        if line.startswith(("import ", "from ")) and "__future__" not in line
    }
    assert imported <= set(sys.stdlib_module_names)
