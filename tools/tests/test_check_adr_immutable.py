"""check_adr_immutable.py's own tests: an accepted ADR is never rewritten.

Deleting one is allowed since #1103, which keeps the records stating an axis and
deletes the rest; the tests after the frontmatter ones pin both halves. Raising
the numbering epoch in the same diff is the one route to rewriting a record, and
the last three tests pin it: refused without the raise, allowed with it, and a
raise on its own is harmless.

A synthetic repo, not the real one -- the check reads git diffs between two
commits, and issue #399's own findings are the ADRs this guard exists to have
caught, not fixtures to replay it against.
"""
import subprocess
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_adr_immutable as guard  # noqa: E402
from check_adr_immutable import main  # noqa: E402


def make_repo(tmp_path: Path) -> Path:
    repo = tmp_path / "repo"
    (repo / "docs" / "decisions").mkdir(parents=True)
    run = lambda *args: subprocess.run(  # noqa: E731
        args, cwd=repo, check=True, capture_output=True)
    run("git", "init", "-q")
    run("git", "config", "user.email", "t@t")
    run("git", "config", "user.name", "t")
    return repo


def commit(repo: Path, message: str) -> str:
    run = lambda *args: subprocess.run(  # noqa: E731
        args, cwd=repo, check=True, capture_output=True, text=True)
    run("git", "add", "-A")
    # --allow-empty: the "empty" base commit in one test adds no file at all,
    # since git tracks no directory that holds none.
    run("git", "commit", "-q", "--allow-empty", "-m", message)
    return run("git", "rev-parse", "HEAD").stdout.strip()


def check(repo: Path, base: str) -> int:
    """main(), pointed at the synthetic repo rather than this one -- every
    subprocess call in the module under test already takes cwd=ROOT."""
    guard.ROOT = repo
    return main(["prog", "--base", base])


def test_a_brand_new_adr_is_unrestricted(tmp_path):
    repo = make_repo(tmp_path)
    base = commit(repo, "empty")

    (repo / "docs" / "decisions" / "0001-new.md").write_text("# 0001 -- New\n\nFresh.\n")
    commit(repo, "add 0001")

    assert check(repo, base) == 0


def test_a_new_adr_still_being_drafted_in_the_same_pr_is_unrestricted(tmp_path):
    """A file absent at --base stays exempt through further edits in the same PR."""
    repo = make_repo(tmp_path)
    base = commit(repo, "empty")

    adr = repo / "docs" / "decisions" / "0001-new.md"
    adr.write_text("# 0001 -- New\n\nDraft.\n")
    commit(repo, "add 0001")
    adr.write_text("# 0001 -- New\n\nRevised before merge.\n")
    commit(repo, "revise 0001 before it ever reached main")

    assert check(repo, base) == 0


def test_even_a_pure_addition_to_an_existing_adr_is_refused(tmp_path):
    """Superseded convention: an amendment is its own ADR, never a blockquote
    appended to the original -- 'Amend 0004 in a decision of its own instead
    of editing it' is why this is stricter than append-only."""
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text("# 0001 -- Old\n\nOriginal claim.\n")
    base = commit(repo, "add 0001")

    adr.write_text(
        "# 0001 -- Old\n\n"
        "> **#42 update:** the claim below is stale in one place.\n\n"
        "Original claim.\n")
    commit(repo, "append an update blockquote to 0001")

    assert check(repo, base) == 1


def test_a_rewritten_line_in_an_existing_adr_fails(tmp_path, capsys):
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text("# 0001 -- Old\n\nOriginal claim.\n")
    base = commit(repo, "add 0001")

    adr.write_text("# 0001 -- Old\n\nRewritten claim.\n")
    commit(repo, "silently rewrite 0001")

    assert check(repo, base) == 1
    assert "0001-old.md" in capsys.readouterr().out


def test_the_decisions_index_is_not_covered(tmp_path):
    repo = make_repo(tmp_path)
    readme = repo / "docs" / "decisions" / "README.md"
    readme.write_text("# Index\n\n| 0001 | ... |\n")
    base = commit(repo, "add index")

    readme.write_text("# Index\n\n| 0002 | ... |\n")
    commit(repo, "replace the index row")

    assert check(repo, base) == 0


def test_a_pull_request_that_never_touches_decisions_is_unaffected(tmp_path):
    repo = make_repo(tmp_path)
    (repo / "README.md").write_text("hello\n")
    base = commit(repo, "init")

    (repo / "README.md").write_text("hello, updated\n")
    commit(repo, "unrelated change")

    assert check(repo, base) == 0


def test_bad_usage_exits_2():
    assert main(["prog"]) == 2
    assert main(["prog", "--base"]) == 2
    assert main(["prog", "--base", "x", "--extra"]) == 2


def test_a_base_that_looks_like_an_option_is_refused_itself(tmp_path):
    """--base reaches subprocess.run unvalidated otherwise -- the same guard
    tools/select_benchmarks.py's REVISION applies to --since."""
    repo = make_repo(tmp_path)
    commit(repo, "init")
    guard.ROOT = repo
    assert main(["prog", "--base", "-rf"]) == 2


# tools/regen_adr_index.py's one exception: the block is metadata for index.yaml, the body
# is the decision, and immutability is for the reasoning rather than the file.
BLOCK = '---\nstatus: accepted\nsupersedes: []\namends: []\napplies: []\n---\n'
BODY = "# 0001 -- Old\n\nOriginal claim.\n"


def test_a_frontmatter_block_above_an_untouched_body_is_allowed(tmp_path):
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text(BODY)
    base = commit(repo, "add 0001")

    adr.write_text(BLOCK + BODY)
    commit(repo, "give 0001 its frontmatter")

    assert check(repo, base) == 0


def test_a_frontmatter_block_with_one_body_word_changed_is_refused(tmp_path, capsys):
    """The exception's whole content is that the body did not move. A commit that
    inserts a block and edits a sentence is the edit this guard exists for,
    wearing the one change it allows."""
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text(BODY)
    base = commit(repo, "add 0001")

    adr.write_text(BLOCK + BODY.replace("Original", "Revised"))
    commit(repo, "frontmatter, and a word while nobody is looking")

    assert check(repo, base) == 1
    assert "tools/regen_adr_index.py" in capsys.readouterr().err


def test_changing_a_block_that_is_already_there_is_refused(tmp_path):
    """What makes the exception self-limiting: it tests that there was no block
    before, so a record carrying one can never take the path again."""
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text(BLOCK + BODY)
    base = commit(repo, "add 0001 with its frontmatter")

    adr.write_text(BLOCK.replace("amends: []", 'amends: ["0002"]') + BODY)
    commit(repo, "rewrite 0001's declared relations")

    assert check(repo, base) == 1


def test_removing_a_block_is_refused(tmp_path):
    repo = make_repo(tmp_path)
    adr = repo / "docs" / "decisions" / "0001-old.md"
    adr.write_text(BLOCK + BODY)
    base = commit(repo, "add 0001 with its frontmatter")

    adr.write_text(BODY)
    commit(repo, "drop 0001's frontmatter")

    assert check(repo, base) == 1


def test_a_deleted_record_is_allowed(tmp_path):
    """#1103's pass deletes every record that states no axis, and git keeps the body."""
    repo = make_repo(tmp_path)
    (repo / "docs" / "decisions" / "0001-mechanism.md").write_text("# 0001 -- Mechanism\n\nBody.\n")
    base = commit(repo, "add 0001")

    (repo / "docs" / "decisions" / "0001-mechanism.md").unlink()
    commit(repo, "delete 0001")

    assert check(repo, base) == 0


def test_an_edited_record_is_still_refused_beside_a_deleted_one(tmp_path):
    """Deleting one record does not license editing its neighbour in the same pass."""
    repo = make_repo(tmp_path)
    (repo / "docs" / "decisions" / "0001-mechanism.md").write_text("# 0001 -- Mechanism\n\nBody.\n")
    (repo / "docs" / "decisions" / "0002-axis.md").write_text("# 0002 -- Axis\n\nBody.\n")
    base = commit(repo, "add both")

    (repo / "docs" / "decisions" / "0001-mechanism.md").unlink()
    (repo / "docs" / "decisions" / "0002-axis.md").write_text("# 0002 -- Axis\n\nRewritten.\n")
    commit(repo, "delete one, edit the other")

    assert check(repo, base) == 1


def test_a_record_renamed_and_rewritten_is_refused(tmp_path):
    """The deletion allowance is not a way to edit: git reports this pair as a rename."""
    repo = make_repo(tmp_path)
    old = repo / "docs" / "decisions" / "0001-axis.md"
    old.write_text("# 0001 -- Axis\n\nThe body a reader cited.\nLine two.\nLine three.\nLine four.\n")
    base = commit(repo, "add 0001")

    old.unlink()
    (repo / "docs" / "decisions" / "0001-axis-restated.md").write_text(
        "# 0001 -- Axis\n\nThe body a reader cited.\nLine two.\nLine three.\nRewritten.\n")
    commit(repo, "rename and rewrite")

    assert check(repo, base) == 1


def epoch(repo: Path, value: int) -> None:
    (repo / "docs" / "decisions" / ".numbering-epoch").write_text(f"{value}\n# test epoch\n")


def test_a_rewritten_record_is_refused_when_the_epoch_stays(tmp_path):
    """The epoch file being present is not the allowance; raising it is."""
    repo = make_repo(tmp_path)
    epoch(repo, 2)
    adr = repo / "docs" / "decisions" / "0003-layout.md"
    adr.write_text("# 0003 -- Layout\n\nThe exchange rule.\n")
    base = commit(repo, "epoch 2, with 0003")

    adr.write_text("# 0003 -- Layout\n\nThe data-types rule.\n")
    commit(repo, "rewrite 0003 under the same epoch")

    assert check(repo, base) == 1


def test_a_rewritten_record_is_allowed_when_the_diff_raises_the_epoch(tmp_path):
    """#1103's epoch 3: 0003 keeps its number and file, and the raised line says it changed."""
    repo = make_repo(tmp_path)
    epoch(repo, 2)
    adr = repo / "docs" / "decisions" / "0003-layout.md"
    adr.write_text("# 0003 -- Layout\n\nThe exchange rule.\n")
    base = commit(repo, "epoch 2, with 0003")

    epoch(repo, 3)
    adr.write_text("# 0003 -- Layout\n\nThe data-types rule.\n")
    commit(repo, "rewrite 0003 in epoch 3")

    assert check(repo, base) == 0


def test_raising_the_epoch_alone_is_allowed(tmp_path):
    repo = make_repo(tmp_path)
    epoch(repo, 2)
    (repo / "docs" / "decisions" / "0003-layout.md").write_text("# 0003 -- Layout\n\nBody.\n")
    base = commit(repo, "epoch 2, with 0003")

    epoch(repo, 3)
    commit(repo, "raise the epoch, touch nothing")

    assert check(repo, base) == 0
