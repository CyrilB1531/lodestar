#!/usr/bin/env python3
"""Refuse README prose that counts or lists fewer packages than src/ holds (#688).

Three hand-written texts understated this repository by seven packages, and each was
true when it was written:

  README.md   "Publishing"      "Nine NuGet packages are produced ... Eight of them
                                are core tier", against sixteen in three tiers
  README.md   "Structure"       nine src/ directories of sixteen
  CLAUDE.md   quick commands    a pack loop naming eight

The shape is the one this repository has now found four times -- #586 in a workflow,
#597 in the README's other pack loop, #610 in a release list. What makes it keep
happening here is that the guards already in place sit *next to* the wrong text:
tools/check_readme_pack_loop.py gates the README's runnable loop, and
tools/check_claude_md_packages.py gates CLAUDE.md's architecture table. Both stayed
current. The prose beside each did not, because nothing read it.

src/ is the source of truth for which packages exist, and CLAUDE.md's architecture
table -- itself held to src/ by check_claude_md_packages.py -- is the source for
which tier each is in. This reads both and holds the three texts to them.

Order is not compared anywhere: all three read in a presentation order of their own,
and none of them is a command whose sequence matters.

The counts are spelled out, which is deliberate and is the half a contributor
forgets: a grep for digits finds nothing, and the sentence is a paragraph away from
the list it counts. tools/tests/test_pre_commit_hook.py and
test_adr_count_coherence.py make the same argument about their own prose.

Usage:  python tools/check_readme_packages.py
        python tools/check_readme_packages.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

# PYTHONSAFEPATH=1 keeps this script's own directory off sys.path, the way
# generate_oracles.py documents -- appended, so nothing here shadows a package.
sys.path.append(str(pathlib.Path(__file__).resolve().parent))

import check_claude_md_packages as tiers  # noqa: E402

ROOT = tiers.ROOT
README = ROOT / "README.md"
CLAUDE = ROOT / "CLAUDE.md"

# `Sixteen NuGet packages are produced: `A`, `B` ... and `P`.` -- the count and the list
# are one sentence, so they are read as one and reported apart.
PUBLISHING = re.compile(
    r"^(\w+) NuGet packages are produced:(.*?)\.\s", re.MULTILINE | re.DOTALL)
# `Twelve are **core tier**` -- the tier sentence that follows it.
CORE_COUNT = re.compile(r"(\w+) are \*\*core tier\*\*")
# A line of the Structure tree: `├── src/Lodestar.Text/    distances, ...`
TREE_LINE = re.compile(r"^[^\n]*?src/(Lodestar\.[A-Za-z.]+)/", re.MULTILINE)
# The quick-commands pack loop in CLAUDE.md, which is not the README's runnable one.
PACK_LOOP = re.compile(r"for p in([^;]*);\s*do")
PACKAGE = re.compile(r"src/(Lodestar\.[A-Za-z.]+)")
NAME = re.compile(r"`(Lodestar\.[A-Za-z.]+)`")

# Its own table rather than the sibling guard's, which starts at eight because that is
# the smallest package count this repository can reach. A tier count has no such floor.
WORDS = (
    "zero one two three four five six seven eight nine ten eleven twelve thirteen "
    "fourteen fifteen sixteen seventeen eighteen nineteen twenty").split()


def spelled(word: str) -> int | None:
    """`sixteen` -> 16, for a count a sentence states in words."""
    lowered = word.lower()
    return WORDS.index(lowered) if lowered in WORDS else None


def in_words(count: int) -> str:
    """16 -> `sixteen`, so a failure message can quote the correction."""
    return WORDS[count] if 0 <= count < len(WORDS) else str(count)


def count_finding(word: str, real: int, where: str) -> list[str]:
    """One sentence's spelled count against the number src/ holds."""
    said = spelled(word)
    if said == real:
        return []
    expected = in_words(real)
    if said is None:
        return [f"{where} states its count as {word!r}, which is not a number word this "
                f"file knows. Spell it out: {expected}."]
    return [f"{where} says {word} and src/ holds {real}. Say `{expected}`."]


def names_finding(found: set[str], packages: set[str], where: str) -> list[str]:
    """One list of package names against the set src/ holds."""
    missing = sorted(packages - found)
    extra = sorted(found - packages)
    out = []
    if missing:
        out.append(f"{where} does not name {', '.join(missing)}, which src/ holds.")
    if extra:
        out.append(f"{where} names {', '.join(extra)}, and there is no src/<name> for it.")
    return out


def publishing_findings(text: str, packages: set[str], core: int) -> list[str]:
    """The Publishing paragraph: its count, its list, and its core-tier count."""
    where = "README.md's Publishing section"
    match = PUBLISHING.search(text)
    if match is None:
        return [f"{where} no longer opens `<count> NuGet packages are produced:`. It is what "
                "a reader believes about what ships, so it states the count and this holds "
                "it to src/."]

    found = count_finding(match.group(1), len(packages), where)
    found += names_finding(set(NAME.findall(match.group(2))), packages, where)

    tier = CORE_COUNT.search(text, match.end())
    if tier is None:
        found.append(f"{where} no longer says how many packages are core tier. The tier split "
                     "is what decision 0076 is about, so the sentence stays.")
    else:
        found += count_finding(tier.group(1), core, f"{where}'s core-tier sentence")
    return found


def tree_findings(text: str, packages: set[str]) -> list[str]:
    """The Structure tree: one line per package under src/."""
    where = "README.md's Structure tree"
    section = text.split("## Structure", 1)
    if len(section) != 2:
        return [f"{where} is gone, or its heading changed."]
    tree = section[1].split("```", 2)
    if len(tree) < 3:
        return [f"{where} has no fenced block to read."]
    # `src/*/Version.props` is a line about every package rather than one of them.
    named = {name for name in TREE_LINE.findall(tree[1])}
    return names_finding(named, packages, where)


def loop_findings(text: str, packages: set[str]) -> list[str]:
    """CLAUDE.md's quick-commands pack loop."""
    where = "CLAUDE.md's pack loop"
    match = PACK_LOOP.search(text)
    if match is None:
        return [f"{where} is gone, or no longer reads `for p in ... ; do`. README.md's "
                "runnable loop is a different list with a different source of truth, and "
                "tools/check_readme_pack_loop.py is what gates that one."]
    return names_finding(set(PACKAGE.findall(match.group(1))), packages, where)


def findings() -> list[str]:
    for path in (README, CLAUDE):
        if not path.exists():
            return [f"{tiers.label(path)}: missing"]

    packages = tiers.source_packages()
    if not packages:
        return ["src/: no package directory at all, so there is nothing to compare."]

    claude = CLAUDE.read_text(encoding="utf-8")
    rows = dict(tiers.ROW.findall(claude))
    core = sum(1 for package in packages if rows.get(package) == "core")

    readme = README.read_text(encoding="utf-8")
    return (publishing_findings(readme, packages, core)
            + tree_findings(readme, packages)
            + loop_findings(claude, packages))


def main() -> int:
    if len(sys.argv) > 1:
        if sys.argv[1] in ("--help", "-h"):
            print(__doc__)
            return 0
        print(__doc__)
        return 2

    found = findings()
    for finding in found:
        print(finding)
    if not found:
        print(f"ok  README.md's prose and CLAUDE.md's pack loop name the "
              f"{len(tiers.source_packages())} packages src/ holds")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
