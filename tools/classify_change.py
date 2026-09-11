#!/usr/bin/env python3
"""Say which packages a change ships and which it is about, from its file paths (#628).

Two different questions, two different answers, and conflating them is what made the manual
pass wrong three times.

**Ships** is `src/<Package>/` and nothing else: what reaches a `.nupkg`. It decides the
milestone, because a milestone names a release.

**About** also counts `tests/`, `bench/` and the documentation pages `docs/wiki-map.json`
already attributes to a package. It decides the boards and the labels, because those answer
"what is this work about" rather than "when did it ship". #227 is the worked example --
"Reference pages for Lodestar.Fuzzy" touches `tests/Lodestar.Fuzzy.Tests` and
`docs/reference/fuzzy/`, never `src/`, so it ships nothing and is entirely about Fuzzy.

Three traps this encodes, each paid for once:

- **The rename.** `src/DataNet.Text/` is `Lodestar.Text` history. A filter matching only
  `src/Lodestar.*` misses every change before the rename -- 50 pull requests, measured.
- **The bare prefix.** `tests/DataNet.NetStandard.Tests` is a repository-wide suite from
  before the split, not a package. Stripping its suffixes yields `DataNet`, which names
  nothing; every result is checked against the directories under `src/` and dropped if it
  is not one of them.
- **Silence.** A path this cannot attribute is reported as unattributed rather than quietly
  folded into the cross-cutting bucket, which is how 49 issues ended up there with no
  evidence behind them.

The documentation mapping is read from `docs/wiki-map.json` rather than restated here: that
file already says which pages belong to which package, and a second hand-written table is
the drift this repository keeps closing.

Usage:  git diff --name-only <base>..<head> | python tools/classify_change.py
        python tools/classify_change.py --files a.cs b.cs
        python tools/classify_change.py --help

Exit:   0 always when it can read its inputs, 2 bad usage. Attribution is an answer, not a
        verdict; the caller decides what an empty one means.
"""

from __future__ import annotations

import json
import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
WIKI_MAP = ROOT / "docs" / "wiki-map.json"
SRC = ROOT / "src"

# `src/Lodestar.Text/...`, `src/DataNet.Text/...` -- the publishable half.
SRC_DIR = re.compile(r"^src/((?:DataNet|Lodestar)\.[A-Za-z.]+)/")
# `tests/Lodestar.Text.Tests/...`, `tests/Lodestar.Text.NetStandard.Tests/...`,
# `bench/Lodestar.Text.Benchmarks/...` -- the half that is about a package without shipping.
SIDE_DIR = re.compile(
    r"^(?:tests|bench)/((?:DataNet|Lodestar)\.[A-Za-z.]+?)(?:\.NetStandard)?"
    r"(?:\.Tests|\.Benchmarks)/")
REFERENCE = re.compile(r"^docs/reference/([^/]+)/")
GUIDE = re.compile(r"^docs/guides/([^/]+)\.md$")


def known_packages() -> set[str]:
    """The directories under src/ that are packages, which is the only valid answer."""
    return {p.name for p in SRC.glob("Lodestar.*") if (p / "Version.props").exists()}


def documentation_map() -> dict[str, str]:
    """`reference/<area>` and `guide/<page>` to package, read from docs/wiki-map.json."""
    if not WIKI_MAP.exists():
        return {}
    data = json.loads(WIKI_MAP.read_text(encoding="utf-8"))
    found = {}
    for package, entry in data.get("packages", {}).items():
        for page in entry.get("pages", []):
            match = REFERENCE.match(page)
            if match:
                found[f"reference/{match.group(1)}"] = package
            match = GUIDE.match(page.replace("*", "x"))
            if match:
                found[f"guide/{match.group(1)}"] = package
    return found


def _rename(name: str) -> str:
    """`DataNet.Text` is `Lodestar.Text` before the rename, and the same package after."""
    return "Lodestar." + name[len("DataNet."):] if name.startswith("DataNet.") else name


def attribute(path: str, docs: dict[str, str]) -> tuple[bool, str | None]:
    """(does it ship, which package) for one path, from its shape alone.

    Four shapes, in the order a path can only match one of them: the publishable tree, the
    suites beside it, a reference directory, a guide page. The last two are looked up rather
    than parsed, because docs/wiki-map.json owns that mapping.
    """
    match = SRC_DIR.match(path)
    if match:
        return True, _rename(match.group(1))
    match = SIDE_DIR.match(path)
    if match:
        return False, _rename(match.group(1))
    match = REFERENCE.match(path)
    if match:
        return False, docs.get(f"reference/{match.group(1)}")
    match = GUIDE.match(path)
    if match:
        return False, docs.get(f"guide/{match.group(1)}")
    return False, None


def classify(paths: list[str]) -> tuple[set[str], set[str], list[str]]:
    """(ships, about, unattributed) for a list of repository-relative paths."""
    packages = known_packages()
    docs = documentation_map()
    ships: set[str] = set()
    about: set[str] = set()
    unattributed: list[str] = []

    for raw in paths:
        path = raw.strip()
        if not path:
            continue
        publishes, named = attribute(path, docs)
        # `named` is checked against src/ rather than trusted: `tests/DataNet.NetStandard.Tests`
        # strips to a prefix that names no package, and an unchecked mapper acted on it.
        if named is None or named not in packages:
            unattributed.append(path)
            continue
        about.add(named)
        if publishes:
            ships.add(named)

    return ships, about, unattributed


def main() -> int:
    args = sys.argv[1:]
    if args and args[0] in ("--help", "-h"):
        print(__doc__)
        return 0
    if args and args[0] == "--files":
        paths = args[1:]
    elif args:
        print(__doc__)
        return 2
    else:
        paths = sys.stdin.read().splitlines()

    ships, about, unattributed = classify(paths)
    print(f"ships={','.join(sorted(ships))}")
    print(f"about={','.join(sorted(about))}")
    print(f"unattributed={len(unattributed)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
