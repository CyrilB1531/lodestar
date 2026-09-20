"""check_performance_sections.py: the guide holds comparisons, not before/afters.

The rule this guard exists for is the fourth one. A before/after is not recognisable from its
prose — every optimisation writes "faster" — but it is recognisable from its columns: a table whose
header names a branch or a revision is comparing this repository to itself at another commit, which
is an argument about a pull request and ages the day after it merges
([#1106](https://github.com/CyrilB1531/lodestar/issues/1106)). The tests below pin that the column
test reads the header row and only the header row, so a data cell saying "main" is not a breach and
a header saying it is.

The other three rules are structural: the package heading, the machine, the window and the
incumbent. Each has a test for the breach rather than for the pass, because the pass is the file in
the tree and CI reads that directly.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_performance_sections as guard  # noqa: E402

PACKAGES = {"Lodestar.Text", "Lodestar.Stats"}

GOOD = """# Performance

## Lodestar.Text

### `Levenshtein.Distance` against rapidfuzz

Machine: AMD Ryzen 7 8700G. Window: one run on 2026-09-14.

| Length | rapidfuzz | Lodestar |
| ---: | ---: | ---: |
| 128 | 994.2 ns | 536.4 ns |
"""


def lines(text: str) -> list[str]:
    return text.split("\n")


def test_a_comparison_with_its_machine_window_and_incumbent_passes():
    assert guard.check(lines(GOOD), PACKAGES) == []


def test_a_header_naming_a_branch_is_refused():
    broken = GOOD.replace("| Length | rapidfuzz | Lodestar |", "| Length | `main` | fix |")
    branch = [b for b in guard.check(lines(broken), PACKAGES) if "belongs in the pull request" in b]
    assert len(branch) == 2


def test_a_data_cell_naming_a_branch_is_not_a_header():
    """Only the row above the delimiter is a header; `main` inside the table is data."""
    body = GOOD.replace("| 128 | 994.2 ns | 536.4 ns |", "| main | 994.2 ns | 536.4 ns |")
    assert guard.check(lines(body), PACKAGES) == []


def test_the_a_b_a_column_the_regression_sections_used_is_refused():
    broken = GOOD.replace("| Length | rapidfuzz | Lodestar |",
                          "| Length | `main`, A1 / A2 | Lodestar |")
    breaches = guard.check(lines(broken), PACKAGES)
    assert [breach for breach in breaches if "A1 / A2" in breach]


def test_a_section_with_no_machine_is_refused():
    broken = GOOD.replace("Machine: AMD Ryzen 7 8700G. ", "")
    assert any("names no machine" in breach for breach in guard.check(lines(broken), PACKAGES))


def test_a_section_with_no_window_is_refused():
    broken = GOOD.replace(" Window: one run on 2026-09-14.", "")
    assert any("names no window" in breach for breach in guard.check(lines(broken), PACKAGES))


def test_a_section_naming_no_incumbent_is_refused():
    broken = GOOD.replace("rapidfuzz", "the old path")
    assert any("names no incumbent" in breach for breach in guard.check(lines(broken), PACKAGES))


def test_an_exempt_section_may_name_no_incumbent():
    exempt = next(iter(guard.EXEMPT))
    broken = GOOD.replace("### `Levenshtein.Distance` against rapidfuzz", f"### {exempt}")
    broken = broken.replace("rapidfuzz", "the CPU path")
    assert guard.check(lines(broken), PACKAGES) == []


def test_every_exemption_carries_its_reason():
    assert all(reason.strip() for reason in guard.EXEMPT.values())


def test_a_heading_that_is_not_a_package_is_refused():
    broken = GOOD.replace("## Lodestar.Text", "## Principles applied")
    assert any("neither a package" in breach for breach in guard.check(lines(broken), PACKAGES))


def test_a_table_under_a_package_heading_is_checked_too():
    """A before/after placed before the first `###` is still a before/after."""
    broken = GOOD.replace("## Lodestar.Text\n",
                          "## Lodestar.Text\n\n| n | before | after |\n| ---: | ---: | ---: |\n| 1 | 2 | 3 |\n")
    branch = [b for b in guard.check(lines(broken), PACKAGES) if "belongs in the pull request" in b]
    assert len(branch) == 2


def test_a_table_in_the_preamble_is_checked_too():
    broken = GOOD.replace("# Performance\n",
                          "# Performance\n\n| n | main | fix |\n| ---: | ---: | ---: |\n| 1 | 2 | 3 |\n")
    branch = [b for b in guard.check(lines(broken), PACKAGES) if "belongs in the pull request" in b]
    assert len(branch) == 2


def test_a_machine_inherited_from_a_neighbour_is_refused():
    """"Same machine as above" is what pointed one section at the wrong machine."""
    broken = GOOD.replace("Machine: AMD Ryzen 7 8700G.", "Same machine as above.")
    assert any("names no machine" in breach for breach in guard.check(lines(broken), PACKAGES))


def test_a_capability_outside_a_package_is_refused():
    broken = GOOD.replace("## Lodestar.Text", "## How to read a row")
    assert any("does not sit under a package" in breach
               for breach in guard.check(lines(broken), PACKAGES))


def test_the_guide_in_the_tree_holds():
    assert guard.GUIDE.exists()
    text = guard.GUIDE.read_text(encoding="utf-8")
    assert guard.check(text.split("\n"), guard.packages()) == []
