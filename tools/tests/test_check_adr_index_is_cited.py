"""check_adr_index_is_cited.py: the index solves nothing unread.

The failure it exists for is a reader citing a decision that was later amended --
`0101` without `0103`, which says the opposite -- and that reader is not looking
for an index. So the two documents that say how to work here have to send them.

What is asserted is substance, not wording: a paragraph naming the index and both
reverse edges. Rewording prose is normal and a guard that forbids it gets deleted.
The paragraph is the unit on purpose -- a sentence naming the index fifty lines
from one naming the edges is two statements a reader meets separately, and the one
that matters is whichever they find first.

Both edges or neither, because following only `amended_by` reads `0095` as never
revisited when `0097` and `0098` both exercise it.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_adr_index_is_cited as guard  # noqa: E402

COMPLETE = """\
# A document

Before citing decision NNNN, read `docs/decisions/index.yaml` and follow its
`amended_by` and `applied_by` entries.

Something else entirely.
"""


def test_a_document_that_sends_the_reader_is_clean():
    assert guard.findings_for("CLAUDE.md", COMPLETE) == []


def test_a_document_that_never_names_the_index_is_refused():
    findings = guard.findings_for("CLAUDE.md", "# A document\n\nNothing about decisions.\n")

    assert len(findings) == 1
    assert "no paragraph names" in findings[0]


def test_naming_the_index_without_both_edges_is_refused():
    text = COMPLETE.replace("`amended_by` and `applied_by` entries", "`amended_by` entries")

    findings = guard.findings_for("CLAUDE.md", text)

    assert len(findings) == 1
    assert "applied_by" in findings[0]


def test_the_two_halves_in_different_paragraphs_are_refused():
    split = """\
# A document

Read `docs/decisions/index.yaml` first.

It holds `amended_by` and `applied_by` for every record.
"""

    findings = guard.findings_for("CLAUDE.md", split)

    assert len(findings) == 1
    assert "no one paragraph" in findings[0]


def test_rewording_the_sentence_is_allowed():
    reworded = """\
# A document

The index at `docs/decisions/index.yaml` is the first thing to read about any
decision: its `applied_by` list says the rule was used again, and `amended_by`
says the decision itself moved.
"""

    assert guard.findings_for("CLAUDE.md", reworded) == []


def test_both_real_documents_send_the_reader():
    assert guard.findings() == []
