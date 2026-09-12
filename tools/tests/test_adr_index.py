"""The ADR index: what it reads, what it reverses, and the chain it reconstitutes.

The index exists because an ADR is immutable, so the decision that gets amended
can never be edited to name its amendment -- seventeen records declared `amends`
or `supersedes` onto fifteen decisions and not one of the fifteen pointed
forward. Two properties carry that, and both are asserted below: the reverse
edges are computed rather than written, and the emitter is deterministic, which
is what lets `check_adr_index_sync.py` compare whole bytes instead of a parse.

The last tests read the real `docs/decisions/`. That is the point of them: a
synthetic corpus would go on passing while the repository's own chains rotted,
and `0095` -> `0097`/`0098` is the case the distinction between *amended* and
*applied* was written for. `0097` says it in its own words -- "This record does
not amend 0095 so much as apply it" -- so a reader following `amended_by` alone
finds nothing and concludes 0095 was never revisited.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import adr_index  # noqa: E402
import pytest  # noqa: E402

BLOCK = """\
---
status: accepted
supersedes: []
amends: ["0004"]
applies: ["0037", "0074"]
---
# 0043 — A title with a colon: and a `backtick`

**Status:** accepted · **Date:** 2026-08-20
"""


def write(directory: Path, name: str, body: str) -> Path:
    path = directory / name
    path.write_text(body, encoding="utf-8")
    return path


def record(number: str, *, amends=(), applies=(), supersedes=(), status="accepted") -> str:
    def listing(values):
        return "[" + ", ".join(f'"{value}"' for value in values) + "]"

    return (
        "---\n"
        f"status: {status}\n"
        f"supersedes: {listing(supersedes)}\n"
        f"amends: {listing(amends)}\n"
        f"applies: {listing(applies)}\n"
        "---\n"
        f"# {number} — Record {number}\n\n"
        f"**Status:** {status} · **Date:** 2026-09-11\n"
    )


def test_a_block_is_read_as_its_four_keys():
    block, body = adr_index.split_frontmatter(BLOCK)
    found = adr_index.read_frontmatter(block)

    assert found == {
        "status": "accepted",
        "supersedes": [],
        "amends": ["0004"],
        "applies": ["0037", "0074"],
    }
    assert body.startswith("# 0043 —")


def test_a_record_with_no_block_reads_as_none():
    assert adr_index.split_frontmatter("# 0001 — No block\n") == (
        None, "# 0001 — No block\n")


@pytest.mark.parametrize("line", [
    "amends: 0004",
    'amends: ["4"]',
    "amends: [0004]",
    'amends: ["0004","0005"]',
    "status accepted",
])
def test_a_shape_the_reader_does_not_know_raises(line):
    # A frontmatter this cannot read is one nobody should trust, so it is a
    # finding rather than a silent default.
    with pytest.raises(adr_index.AdrError):
        adr_index.read_frontmatter(line + "\n")


def test_a_key_declared_twice_raises():
    with pytest.raises(adr_index.AdrError):
        adr_index.read_frontmatter('amends: ["0004"]\namends: []\n')


def test_the_title_comes_from_the_heading_and_not_the_frontmatter():
    _, body = adr_index.split_frontmatter(BLOCK)

    assert adr_index.title_of("0043", body) == "A title with a colon: and a `backtick`"


def test_a_heading_for_another_number_is_not_this_record_s_title():
    with pytest.raises(adr_index.AdrError):
        adr_index.title_of("0044", "# 0043 — Someone else's record\n")


def test_every_forward_edge_comes_back_reversed(tmp_path):
    write(tmp_path, "0001-a.md", record("0001"))
    write(tmp_path, "0002-b.md", record("0002", amends=["0001"]))
    write(tmp_path, "0003-c.md", record("0003", applies=["0001"]))
    write(tmp_path, "0004-d.md", record("0004", supersedes=["0001"]))

    entries = adr_index.build(adr_index.read_all(tmp_path))

    assert entries["0001"]["amended_by"] == ["0002"]
    assert entries["0001"]["applied_by"] == ["0003"]
    assert entries["0001"]["superseded_by"] == ["0004"]
    assert entries["0001"]["amends"] == []


def test_an_edge_onto_a_record_that_does_not_exist_is_kept_and_not_reversed(tmp_path):
    # check_adr_frontmatter.py is where a dangling reference is reported; the
    # index says what the records say rather than arguing with them.
    write(tmp_path, "0001-a.md", record("0001", amends=["0999"]))

    entries = adr_index.build(adr_index.read_all(tmp_path))

    assert entries["0001"]["amends"] == ["0999"]
    assert "0999" not in entries


def test_two_records_amending_one_are_listed_in_number_order(tmp_path):
    write(tmp_path, "0001-a.md", record("0001"))
    write(tmp_path, "0009-c.md", record("0009", amends=["0001"]))
    write(tmp_path, "0002-b.md", record("0002", amends=["0001"]))

    entries = adr_index.build(adr_index.read_all(tmp_path))

    assert entries["0001"]["amended_by"] == ["0002", "0009"]


def test_the_emitter_quotes_a_title_that_would_otherwise_break_the_yaml(tmp_path):
    write(tmp_path, "0043-a.md", BLOCK)

    emitted = adr_index.generate(tmp_path)

    assert '"0043":' in emitted
    assert '  title: "A title with a colon: and a `backtick`"' in emitted


def test_emitting_twice_gives_the_same_bytes(tmp_path):
    write(tmp_path, "0001-a.md", record("0001"))
    write(tmp_path, "0002-b.md", record("0002", amends=["0001"]))

    assert adr_index.generate(tmp_path) == adr_index.generate(tmp_path)


def test_the_committed_index_is_what_the_records_generate():
    # The guard's own subject, asserted here too: a generated file nobody
    # regenerates is worse than no file, because it is believed.
    assert adr_index.INDEX.read_text(encoding="utf-8") == adr_index.generate()


def test_the_0095_chain_reconstitutes_from_the_index_alone():
    """Containment rather than equality: `applied_by` is a list that grows.

    It was written as equality and broke on the first record to apply 0095 after
    0098 -- which is the index working, not the chain changing. What the test is
    about is that following `applied_by` from 0095 finds the two records that
    exercised its escape hatch, and that `amended_by` stays empty because neither
    of them changed it.
    """
    entries = adr_index.build(adr_index.read_all())

    assert {"0097", "0098"} <= set(entries["0095"]["applied_by"])
    assert entries["0095"]["amended_by"] == []
    assert entries["0097"]["applies"] == ["0095"]
    assert entries["0098"]["applies"] == ["0095"]
    # 0095 is itself an application, so the chain runs 0081 <- 0095 <- 0097/0098.
    assert entries["0095"]["applies"] == ["0081"]
    assert "0095" in entries["0081"]["applied_by"]


def test_the_0101_amendment_is_reachable_from_the_record_that_cannot_name_it():
    entries = adr_index.build(adr_index.read_all())

    assert entries["0101"]["amended_by"] == ["0103"]
    assert entries["0103"]["amends"] == ["0101"]
