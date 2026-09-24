#!/usr/bin/env python3
"""Report the merged work each package has not published, from the tags and main (#628).

The Milestones page answers "what is left to ship" only while somebody keeps it current, and
an automated view that is wrong is worse than none: 49 issues once sat on `Cross-cutting`
with no evidence behind that label, indistinguishable from 109 that had some. This reads the
three sources that cannot drift -- the tags, `main`, and `src/<Package>/Version.props` -- so
the page can be checked rather than trusted.

Measured on 2026-09-11: `Lodestar.Fuzzy` had two commits merged on 2026-09-01 and never
published, and `Lodestar.Metrics` one. Both had been sitting there for ten days, and nothing
said so.

**Unreleased work is not a fault.** It is the normal state between a merge and a release, so
it is reported and exits zero.

**A dependency moved in `src/Directory.Packages.props` is unpublished work too.** Central
package management keeps every version pin in that one file, so raising `Microsoft.ML.OnnxRuntime`
changes what `Lodestar.Onnx` ships without a commit under `src/Lodestar.Onnx`. Measured on
2026-09-16: counting that directory alone reported nothing to publish for `Lodestar.Onnx` and
`Lodestar.Extensions.AI` while two commits moving their pins waited behind each tag. A pin commit counts
for a package when it changes a pin that package's project references.

**One thing fails**: a package whose declared version is *ahead* of its last tag with nothing
left to publish. That is a release prepared and then never tagged, and it has no innocent
reading.

A missing `## [Unreleased]` entry is **reported, not failed**, and the reason is a measured
one. `701cd987` touches six files under `src/Lodestar.Metrics` and changes nothing but XML
documentation comments -- unpublished work that owes the changelog nothing, since
`CONTRIBUTING.md` scopes the entry to a change in shipped behaviour. Nothing here can tell a
rewrapped comment from a new overload, so failing on the count would refuse a tree that is
correct. The line is printed so a release cut sees it; the judgement stays with the reader.

Usage:  python tools/check_unreleased.py
        python tools/check_unreleased.py --report
        python tools/check_unreleased.py --help

  --report  Print the table and exit 0 whatever it says. What a release cut reads.

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SRC = ROOT / "src"

# Named because it is spelled three times below, which is S1192's threshold.
REPORT = "--report"
ENCODING = "utf-8"

VERSION = re.compile(r"<Lodestar[A-Za-z]*Version>([^<]+)</")
# The heading each src/<Package>/CHANGELOG.md opens its pending entries under (#1133).
UNRELEASED = "## [Unreleased]"
CENTRAL = "src/Directory.Packages.props"
# A changed pin line in a commit's diff, and a project's reference to a pinned package.
PIN = re.compile(r'^[+-]\s*<PackageVersion Include="([^"]+)"')
REFERENCE = re.compile(r'<PackageReference Include="([^"]+)"')


def git(*args: str) -> str:
    return subprocess.run(
        ["git", *args], cwd=ROOT, capture_output=True, text=True, check=False).stdout.strip()


def declared(package: str) -> str:
    """The version src/<Package>/Version.props declares, which is the only place it lives."""
    props = SRC / package / "Version.props"
    match = VERSION.search(props.read_text(encoding=ENCODING)) if props.exists() else None
    return match.group(1) if match else ""


def as_tuple(version: str) -> tuple[int, ...]:
    """`0.10.0` sorts above `0.9.0`, which a string comparison gets backwards."""
    parts = []
    for piece in version.split("."):
        digits = "".join(c for c in piece if c.isdigit())
        parts.append(int(digits) if digits else 0)
    return tuple(parts)


def latest_tag(package: str) -> str:
    """The highest `<Package>/v*` tag, by version rather than by string."""
    tags = [t for t in git("tag", "--list", f"{package}/v*").splitlines() if t]
    return max(tags, key=lambda t: as_tuple(t.rsplit("/v", 1)[1]), default="")


def unreleased_entries() -> set[str]:
    """The packages whose own CHANGELOG.md holds an entry under `## [Unreleased]`."""
    found: set[str] = set()
    for path in SRC.glob("*/CHANGELOG.md"):
        inside = False
        for line in path.read_text(encoding=ENCODING).splitlines():
            if line.startswith("## "):
                inside = line.strip() == UNRELEASED
            elif inside and line.startswith("- "):
                found.add(path.parent.name)
                break
    return found


def has_tags() -> bool:
    """Whether this checkout carries the release tags at all.

    A CI checkout is shallow and fetches none, and every package then reads as though its
    whole history were unpublished. Reporting that is worse than reporting nothing, so the
    caller is told to fetch them instead.
    """
    return bool(git("tag", "--list", "Lodestar.*"))


def references(package: str) -> set[str]:
    """The packages this project references, whose pins live in the central file."""
    project = SRC / package / f"{package}.csproj"
    return set(REFERENCE.findall(project.read_text(encoding=ENCODING))) if project.exists() else set()


def pin_commits(span: str, referenced: set[str]) -> set[str]:
    """Commits in the span that change a central pin one of `referenced` names."""
    moved = set()
    for sha in git("log", "--format=%H", span, "--", CENTRAL).splitlines():
        diff = git("show", "--format=", "--unified=0", sha, "--", CENTRAL).splitlines()
        if any(match.group(1) in referenced for match in map(PIN.match, diff) if match):
            moved.add(sha)
    return moved


def survey() -> list[tuple[str, str, str, int]]:
    """(package, declared version, last tag, unpublished commit count) for every package."""
    rows = []
    for path in sorted(SRC.glob("Lodestar.*")):
        if not (path / "Version.props").exists():
            continue
        package = path.name
        tag = latest_tag(package)
        span = f"{tag}..HEAD" if tag else "HEAD"
        own = set(git("log", "--format=%H", span, "--", f"src/{package}").splitlines())
        waiting = (own | pin_commits(span, references(package))) - {""}
        rows.append((package, declared(package), tag, len(waiting)))
    return rows


def findings(rows: list[tuple[str, str, str, int]]) -> list[str]:
    found = []
    for package, version, tag, commits in rows:
        shipped = tag.rsplit("/v", 1)[1] if tag else ""
        # A version equal to its tag while commits wait is normal: the bump belongs to the
        # release, not to the merge. The docstring has why that is not a finding.
        if not commits and shipped and as_tuple(version) > as_tuple(shipped):
            found.append(
                f"{package}: declares {version} and the highest tag is {tag}, with nothing "
                "unpublished. A release was prepared and never tagged.")
    return found


def notices(rows: list[tuple[str, str, str, int]]) -> list[str]:
    """What a release cut should look at, without any of it being a failure."""
    named = unreleased_entries()
    return [
        f"note  {package}: {commits} commit(s) unpublished and no entry under "
        f"`{UNRELEASED}` in src/{package}/CHANGELOG.md. If any of them changed shipped behaviour, "
        "it owes one "
        "(CONTRIBUTING.md, definition of done, item 7)."
        for package, _, _, commits in rows if commits and package not in named
    ]


def print_table(rows: list[tuple[str, str, str, int]]) -> None:
    """The survey as a table, which is what a release cut reads."""
    print(f"{'package':<30} {'declared':<10} {'last tag':<10} unpublished")
    for package, version, tag, commits in rows:
        shipped = tag.rsplit("/v", 1)[1] if tag else "-"
        print(f"{package:<30} {version:<10} {shipped:<10} {commits or '-'}")
    print()


def main() -> int:
    args = sys.argv[1:]
    if args and args[0] in ("--help", "-h"):
        print(__doc__)
        return 0
    if args and args[0] != REPORT:
        print(__doc__)
        return 2

    if not has_tags():
        print("This checkout carries no Lodestar.* tag, so nothing here can say what has "
              "shipped. Fetch them -- `git fetch --tags`, or `fetch-tags: true` on "
              "actions/checkout -- and run this again.")
        return 0

    rows = survey()
    waiting = [r for r in rows if r[3]]
    if args[:1] == [REPORT] or waiting:
        print_table(rows)

    if args[:1] == [REPORT]:
        return 0

    for notice in notices(rows):
        print(notice)
    found = findings(rows)
    for finding in found:
        print(finding)
    if not found:
        print(f"ok  {len(waiting)} package(s) waiting to publish, and no version declared past "
              "its own tag")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
