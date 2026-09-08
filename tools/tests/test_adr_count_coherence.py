"""The ADR directory, the index table and the index prose all count the same records.

`docs/decisions/README.md` states three times, in three notations, how many
decisions this repository holds: once as the files themselves, once as the rows of
its table, and once as two numbers spelled out in words under `## What `accepted`
means here` -- a total, and that total less `0004`. Nothing derived any of them
from the others, so they drifted: the prose sat at eighty-two while the directory
held eighty-three, and two sessions in one day re-diagnosed the same gap before
either noticed the other. Adding an ADR means editing all three, which is the kind
of coupling a reader cannot see and a test can.

The words are the half no grep for digits will find, and the half a contributor is
most likely to forget -- the table row is next to the file being added, the
paragraph is fifteen lines below it. #553 added `0084` with its row and left the
prose behind; the same commit had to come back for it.

This guard reads the directory as the truth and holds the other two to it. It does
not check what any ADR says: `check_adr_immutable.py` owns that, and the index is
outside its scope by its own line 41, which is why nothing was watching here.
"""

from __future__ import annotations

import pathlib
import re

ROOT = pathlib.Path(__file__).resolve().parents[2]
DECISIONS = ROOT / "docs" / "decisions"
INDEX = DECISIONS / "README.md"

# `0001-target-framework.md` -- every ADR file, README.md excluded by the shape.
ADR_FILE = re.compile(r"^(\d{4})-.+\.md$")

# `| [`0001`](0001-target-framework.md) | ...` -- one table row per ADR.
INDEX_ROW = re.compile(r"^\|\s*\[`(\d{4})`\]", re.MULTILINE)

# The prose section carrying the spelled-out numbers. The heading holds backticks
# around `accepted`; matching to the next `## ` keeps the section's own bounds.
PROSE_SECTION = re.compile(
    r"^## What `accepted` means here$(.*?)(?=^## )", re.MULTILINE | re.DOTALL)

UNITS = {
    "one": 1, "two": 2, "three": 3, "four": 4, "five": 5, "six": 6, "seven": 7,
    "eight": 8, "nine": 9, "ten": 10, "eleven": 11, "twelve": 12, "thirteen": 13,
    "fourteen": 14, "fifteen": 15, "sixteen": 16, "seventeen": 17,
    "eighteen": 18, "nineteen": 19,
}
TENS = {
    "twenty": 20, "thirty": 30, "forty": 40, "fifty": 50, "sixty": 60,
    "seventy": 70, "eighty": 80, "ninety": 90,
}
NUMBER_WORDS = {**UNITS, **TENS, "hundred": 100}

# A run of number words joined by hyphens, spaces or `and`: `eighty-four`, `one
# hundred and four`. Longest-first so `eight` cannot beat `eighteen` or `eighty`.
_WORD = "|".join(sorted(NUMBER_WORDS, key=len, reverse=True))
SPELLED = re.compile(
    rf"\b(?:{_WORD})(?:[-\s]+(?:and[-\s]+)?(?:{_WORD}))*\b", re.IGNORECASE)

# The two the section states: the total, and the total less 0004 itself. A third
# would mean the paragraph was rewritten, which changes what this test guards.
EXPECTED_SPELLED_COUNT = 2


def spelled_to_int(phrase: str) -> int:
    """`eighty-four` -> 84. Handles the hundreds a future index may reach."""
    total = 0
    current = 0
    for word in re.split(r"[-\s]+", phrase.lower()):
        if word == "and":
            continue
        if word == "hundred":
            current = (current or 1) * 100
            total += current
            current = 0
            continue
        current += NUMBER_WORDS[word]
    return total + current


def adr_numbers() -> list[int]:
    """Every ADR number the directory holds, ascending."""
    found = [
        int(match.group(1))
        for path in DECISIONS.iterdir()
        for match in [ADR_FILE.match(path.name)]
        if match
    ]
    return sorted(found)


def index_numbers() -> list[int]:
    """Every ADR number the index table lists, in the order it lists them."""
    return [int(n) for n in INDEX_ROW.findall(INDEX.read_text(encoding="utf-8"))]


def spelled_numbers() -> list[int]:
    """The numbers written in words under `## What `accepted` means here`."""
    section = PROSE_SECTION.search(INDEX.read_text(encoding="utf-8"))
    assert section, (
        "the `## What `accepted` means here` section is gone from the index, or no "
        "`## ` heading follows it. It carries the counts this test checks.")
    return [spelled_to_int(m.group(0)) for m in SPELLED.finditer(section.group(1))]


def test_the_directory_holds_adrs():
    assert adr_numbers(), "no ADR file found: the directory parse is wrong"


def test_the_adr_numbering_has_no_gap_and_no_duplicate():
    numbers = adr_numbers()
    assert len(numbers) == len(set(numbers)), (
        f"two ADR files share a number: {sorted({n for n in numbers if numbers.count(n) > 1})}")
    expected = list(range(1, len(numbers) + 1))
    assert numbers == expected, (
        f"the ADR numbering is not 1..{len(numbers)} unbroken -- missing "
        f"{sorted(set(expected) - set(numbers))}, unexpected "
        f"{sorted(set(numbers) - set(expected))}")


def test_the_index_lists_every_adr_exactly_once():
    listed, actual = index_numbers(), adr_numbers()
    missing = sorted(set(actual) - set(listed))
    extra = sorted(set(listed) - set(actual))
    duplicated = sorted({n for n in listed if listed.count(n) > 1})
    assert not missing, f"{INDEX.name} has no row for {missing}, which docs/decisions/ holds"
    assert not extra, f"{INDEX.name} lists {extra}, which docs/decisions/ does not hold"
    assert not duplicated, f"{INDEX.name} lists {duplicated} more than once"


def test_the_index_rows_are_in_ascending_order():
    listed = index_numbers()
    assert listed == sorted(listed), (
        "the index table is not in ascending order, so a reader cannot find a row by "
        "counting down to it")


def test_the_prose_states_two_numbers():
    spelled = spelled_numbers()
    assert len(spelled) == EXPECTED_SPELLED_COUNT, (
        f"expected {EXPECTED_SPELLED_COUNT} numbers written in words under "
        f"`## What `accepted` means here`, found {len(spelled)}: {spelled}. Rewriting "
        "that paragraph changes what this test reads, so update the count deliberately.")


def test_the_prose_counts_match_the_directory():
    total = len(adr_numbers())
    spelled = spelled_numbers()
    assert spelled[0] == total, (
        f"the index prose says {spelled[0]} ADRs carry `accepted`, but docs/decisions/ "
        f"holds {total}. Adding an ADR means editing this paragraph too.")
    assert spelled[1] == total - 1, (
        f"the index prose says `0004`'s status reads like the other {spelled[1]}, but "
        f"{total} ADRs less 0004 itself is {total - 1}. The two numbers move together.")
