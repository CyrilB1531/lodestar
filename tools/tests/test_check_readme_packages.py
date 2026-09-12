"""check_readme_packages.py: the prose beside a gated list is not itself gated.

That is the whole finding behind #688. `check_readme_pack_loop.py` holds the README's
runnable loop to the sample, and `check_claude_md_packages.py` holds CLAUDE.md's
architecture table to `src/`. Both stayed current at sixteen packages. The Publishing
paragraph two screens away said nine, the Structure tree listed nine directories, and
CLAUDE.md's quick-commands pack loop named eight -- each true when written, each read by
nobody.

The tests below are written against fixture text rather than the repository, except the
last: a guard whose only assertion is "the tree passes today" passes tomorrow for the
wrong reason.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_readme_packages as guard  # noqa: E402

PACKAGES = {"Lodestar.Text", "Lodestar.Gpu", "Lodestar.Onnx"}

PUBLISHING = """\
## Publishing

Three NuGet packages are produced: `Lodestar.Text`, `Lodestar.Gpu` and `Lodestar.Onnx`.
One are **core tier** and carry no external dependency.
"""

TREE = """\
## Structure

```text
Lodestar.slnx
├── src/Lodestar.Text/     distances
├── src/Lodestar.Gpu/      kernels
├── src/Lodestar.Onnx/     inference
├── src/*/Version.props    one version per publishable package
```
"""

LOOP = """\
```bash
for p in src/Lodestar.Text src/Lodestar.Gpu src/Lodestar.Onnx; do
  dotnet pack "$p" -c Release -o ./artifacts
done
```
"""


def test_a_complete_publishing_paragraph_is_clean():
    assert guard.publishing_findings(PUBLISHING, PACKAGES, 1) == []


def test_a_count_that_disagrees_with_src_is_refused():
    text = PUBLISHING.replace("Three NuGet", "Nine NuGet")

    findings = guard.publishing_findings(text, PACKAGES, 1)

    assert len(findings) == 1
    assert "says Nine and src/ holds 3" in findings[0]
    assert "`three`" in findings[0]


def test_a_count_that_is_not_a_number_word_is_refused():
    # The shape the stale text actually had: "Eight of them are **core tier**".
    text = PUBLISHING.replace("One are **core tier**", "One of them are **core tier**")

    findings = guard.publishing_findings(text, PACKAGES, 1)

    assert any("'them'" in finding and "Spell it out: one" in finding
               for finding in findings)


def test_a_package_missing_from_the_list_is_refused():
    text = PUBLISHING.replace("`Lodestar.Gpu` and ", "")

    findings = guard.publishing_findings(text, PACKAGES, 1)

    assert any("does not name Lodestar.Gpu" in finding for finding in findings)


def test_a_package_in_the_list_that_src_does_not_hold_is_refused():
    findings = guard.publishing_findings(PUBLISHING, {"Lodestar.Text", "Lodestar.Gpu"}, 1)

    assert any("names Lodestar.Onnx" in finding and "no src/<name>" in finding
               for finding in findings)


def test_a_core_tier_count_that_disagrees_is_refused():
    findings = guard.publishing_findings(PUBLISHING, PACKAGES, 2)

    assert any("core-tier sentence says One and src/ holds 2" in finding
               for finding in findings)


def test_a_publishing_section_that_no_longer_states_a_count_is_refused():
    text = PUBLISHING.replace("Three NuGet packages are produced:", "We publish:")

    findings = guard.publishing_findings(text, PACKAGES, 1)

    assert len(findings) == 1
    assert "no longer opens" in findings[0]


def test_a_complete_tree_is_clean():
    assert guard.tree_findings(TREE, PACKAGES) == []


def test_a_package_missing_from_the_tree_is_refused():
    text = TREE.replace("├── src/Lodestar.Gpu/      kernels\n", "")

    findings = guard.tree_findings(text, PACKAGES)

    assert len(findings) == 1
    assert "Structure tree does not name Lodestar.Gpu" in findings[0]


def test_the_version_props_line_is_not_read_as_a_package():
    # `src/*/Version.props` is a line about every package, not one of them.
    assert guard.tree_findings(TREE, PACKAGES) == []


def test_a_missing_structure_section_is_refused():
    findings = guard.tree_findings("# A README with no tree\n", PACKAGES)

    assert len(findings) == 1
    assert "is gone" in findings[0]


def test_a_complete_pack_loop_is_clean():
    assert guard.loop_findings(LOOP, PACKAGES) == []


def test_a_package_missing_from_the_pack_loop_is_refused():
    text = LOOP.replace(" src/Lodestar.Gpu", "")

    findings = guard.loop_findings(text, PACKAGES)

    assert len(findings) == 1
    assert "pack loop does not name Lodestar.Gpu" in findings[0]


def test_a_pack_loop_that_is_gone_is_refused():
    findings = guard.loop_findings("```bash\ndotnet build\n```\n", PACKAGES)

    assert len(findings) == 1
    assert "check_readme_pack_loop.py" in findings[0]


def test_the_repository_as_it_stands_is_clean():
    # The real texts, because a fixture cannot notice a seventeenth package arriving.
    assert guard.findings() == []


def test_a_dot_inside_a_package_name_does_not_end_the_sentence():
    """`Lodestar.Stats.Regression` carries two periods, and neither closes the list.

    What tells them apart is the character after: a period inside a name is followed by
    a letter, the one that ends the sentence by whitespace. The sentence also ends at a
    newline rather than a space, which is how the real README wraps it.
    """
    packages = {"Lodestar.Stats", "Lodestar.Stats.Regression"}
    text = (
        "Two NuGet packages are produced: `Lodestar.Stats` and\n"
        "`Lodestar.Stats.Regression`.\n"
        "Two are **core tier** and carry no external dependency.\n"
    )

    assert guard.publishing_findings(text, packages, 2) == []


def test_a_publishing_section_with_no_core_tier_sentence_is_refused():
    text = PUBLISHING.replace("One are **core tier**", "All of them are core")

    findings = guard.publishing_findings(text, PACKAGES, 1)

    assert len(findings) == 1
    assert "no longer says how many packages are core tier" in findings[0]
