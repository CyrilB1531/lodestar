"""Every `tools/*.py` is named somewhere in `tools/README.md`.

CLAUDE.md's *Where a fact belongs* table gives that README one subject -- "what
each tool does and how to run it" -- and one source: "the scripts under tools/,
hand-maintained". A script it never names is therefore a gap in the document's
own contract rather than a matter of taste, and hand-maintained against a source
that moves is exactly the pairing this repository asserts instead of trusting
(check_claude_md_packages.py, check_release_workflow_packages.py,
test_packaging_lists_cover_the_solution.py).

Nothing warned, and eight of the forty-two had drifted out by #652. Four of them
were the nightly benchmark machinery -- the part a reader is least able to
reconstruct from the filenames, and `check_bench_map.py` had been missing from
the README's own enumeration of the pre-commit set while running in it.

What this asks for is a mention, not a row in any particular list: four scripts
are documented only by their own `## ` section, and a guard that demanded a
bullet as well would be dictating the shape of the prose rather than its
coverage. Whether the mention says anything useful is a review's call.

A script deliberately left undocumented belongs in UNDOCUMENTED below, named and
with its reason, rather than silently absent. It is empty today -- every script
under tools/ is documented, module or command line -- and an exemption list that
grows is a guard being switched off one file at a time.

AT_LEAST is a floor rather than a pin. The count rises whenever a tool is added,
so pinning it would fail every such commit for no finding; a floor catches only
the glob breaking and the guard passing vacuously.
"""

from __future__ import annotations

import pathlib
import re

import pytest

ROOT = pathlib.Path(__file__).resolve().parents[2]
TOOLS = ROOT / "tools"
README = TOOLS / "README.md"

# Scripts the README deliberately does not name; an entry needs its reason
# beside it. Empty today, and the docstring above has why it should stay so.
UNDOCUMENTED: frozenset[str] = frozenset()

# A floor on what the glob finds, not a pin on what tools/ holds.
AT_LEAST = 40


def scripts() -> list[str]:
    """The scripts the README owns: tools/*.py, not tools/tests/."""
    return sorted(p.name for p in TOOLS.glob("*.py") if p.name not in UNDOCUMENTED)


def names(name: str) -> bool:
    """Whether the README names this file, rather than one whose name ends in it.

    `tools/python_floor.py` and a bare `seeded_random.py` both count; a mention
    of `test_check_bench_map.py` does not count as one of `check_bench_map.py`.
    A leading `/` or backtick is how the README actually spells these, so only a
    word character or a dash before the name disqualifies the match.
    """
    text = README.read_text(encoding="utf-8")
    return re.search(rf"(?<![\w-]){re.escape(name)}", text) is not None


def test_the_tools_directory_was_found():
    found = scripts()
    assert len(found) >= AT_LEAST, (
        f"found only {len(found)} scripts under {TOOLS}: the glob is wrong, and a "
        "guard that finds nothing passes everything")


def test_every_exemption_names_a_script_that_exists():
    gone = sorted(name for name in UNDOCUMENTED if not (TOOLS / name).exists())
    assert not gone, (
        f"UNDOCUMENTED names {gone}, which no longer exist. An exemption outlives "
        "what it exempted only by being forgotten; drop it.")


@pytest.mark.parametrize("script", scripts())
def test_the_readme_names_every_script(script):
    # Bound first: asserting on the call directly makes pytest print the whole
    # README as the expression's operand, which buries the finding.
    documented = names(script)
    assert documented, (
        f"tools/README.md never names {script}. Its subject is what each tool does "
        "and how to run it, so write the row from the script's own module docstring "
        "-- or, if it genuinely should not have one, add it to UNDOCUMENTED in "
        f"{pathlib.Path(__file__).name} with the reason.")
