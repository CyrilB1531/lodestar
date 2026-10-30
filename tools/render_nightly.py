#!/usr/bin/env python3
"""Render the nightly benchmark page from what the run produced (#11).

The page is generated, published to the wiki, and never hand-edited. It carries
the baseline commit the next run reads, from the night's wiki commit or this file,
whichever is further along: the series has to remember where it stopped.

What it deliberately does NOT do is pretend to be docs/guides/performance.md.
That page's subject is what was measured on a named machine, and its numbers are
comparable to each other because the machine and its load are stated. A GitHub
hosted runner is a shared VM whose hardware differs between runs, so its absolute
figures are not comparable to that table nor, strictly, to last night's. What
survives is the ratio inside one run -- a baseline and its comparands measured in
the same minute on the same VM -- which is what this page is for.

Usage:  python tools/render_nightly.py --commit <sha> --baseline <sha> --runner <label> \\
            [--selected Class ...] [--harnesses Name ...] [--owed Name ...] [--cut Name ...] \\
            [--failed Name ...] [--reason text] [--branch] [--stdout]
        python tools/render_nightly.py --markers [--wiki <clone>]
"""

from __future__ import annotations

import argparse
import json
import re
import pathlib
import subprocess
import sys

import select_benchmarks

ROOT = pathlib.Path(__file__).resolve().parent.parent

# long-comment: two homes, not an argument -- a workflow input still cannot aim a
# write anywhere but one of these two constants. PAGE is the one docs/wiki-map.json
# declares and only a main run may write; BRANCH_PAGE is where a workflow_dispatch on
# another ref writes instead, so that branch keeps its own run's numbers without ever
# touching the page a merge into main could carry along (#379). BRANCH_PAGE is not in
# wiki-map.json's page lists, unlike PAGE, so it never reaches the wiki even if it
# somehow reached main. --stdout is what a local check uses.
PAGE = ROOT / "docs" / "guides" / "nightly_run.md"
BRANCH_PAGE = ROOT / "docs" / "guides" / "branch" / "nightly_run.md"

# BenchmarkDotNet writes here by its own convention, so the reports are found rather
# than passed in: no path this script touches comes from an argument.
ARTIFACTS = ROOT / "BenchmarkDotNet.Artifacts" / "results"

# What bench/compare.py prints, captured per comparison. This is where the claim against
# rapidfuzz is measured; a BenchmarkDotNet class only measures us against ourselves.
COMPARISONS = ROOT / "bench" / "results"

HEADER = "# Nightly benchmark run"

MARKER = "<!-- nightly-baseline: {sha} -->"

# What the run selected and did not reach. The baseline above advances to the commit that
# was measured, so without this a truncated night loses the classes it skipped (#650).
OWED = "<!-- nightly-owed: {classes} -->"

# Among those, what the next night asks for last: cut at a stop and not run since, or left unsettled
# by a step that died, so one that does not fit the window takes only what the rest leave (#1694).
CUT = "<!-- nightly-cut: {names} -->"

REVISION = re.compile(r"[0-9a-f]{7,40}")

# By file name, not the wiki's flat name: build_wiki.py rewrites it for the wiki, and github.com and
# the documentation site resolve it as written (#1180).
PERFORMANCE = "performance.md"

PREAMBLE = """
> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance]({performance}). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.
"""


def split_report(body: str) -> tuple[str, str] | None:
    """BenchmarkDotNet's own preamble fence, and the table that follows it.

    A *-report-github.md opens a bare, unlabelled ``` around its host/job
    summary, closes it, then prints the results as a plain GFM table -- one
    following the other, never nested. None when a report does not have that
    shape, so the caller can fall back rather than guess where a table starts.
    """
    lines = body.splitlines()
    fences = [i for i, line in enumerate(lines) if line.strip() == "```"]
    if len(fences) < 2:
        return None
    preamble = "\n".join(lines[fences[0] + 1:fences[1]]).strip()
    table = "\n".join(lines[fences[1] + 1:]).strip()
    return preamble, table


def heading_for(stem: str, occurrence: int) -> str:
    """The section heading for one report, unique when a stem repeats within a run."""
    return stem if occurrence == 1 else f"{stem} (run {occurrence})"


def included_reports(paths: list[pathlib.Path]) -> list[str]:
    """Each report's own section: its preamble fenced, its results table native.

    The preamble is prose, always safe to fence. The table is real GFM (a
    `---:` header row, `**bold**` baseline cells) and renders as a table only
    when nothing wraps it -- but BenchmarkDotNet pads its own pipes to content
    width, which this repo's MD060 alignment style does not match, so that one
    rule is switched off around it: generated content, never hand-edited, has
    no convention of its own to keep.

    A stem can arrive twice -- the same class measured more than once in one run --
    and two identical `###` lines fail markdownlint's MD024 and sink the whole
    lint job. Later occurrences are numbered rather than dropped: the second
    measurement is real, it is usually not wanted, and a reader who can see it is
    the one who can go and remove its cause.
    """
    lines: list[str] = []
    seen: dict[str, int] = {}
    for path in paths:
        body = path.read_text(encoding="utf-8").strip() if path.exists() else ""
        if not body:
            continue
        seen[path.stem] = seen.get(path.stem, 0) + 1
        occurrence = seen[path.stem]
        lines += [f"### {heading_for(path.stem, occurrence)}", ""]
        split = split_report(body)
        if split is None:
            lines += ["````text", body, "````", ""]
            continue
        preamble, table = split
        if preamble:
            lines += ["```text", preamble, "```", ""]
        if table:
            lines += ["<!-- markdownlint-disable MD060 -->", "", table, "",
                       "<!-- markdownlint-enable MD060 -->", ""]
    return lines


def has_content(paths: list[pathlib.Path]) -> bool:
    """Whether at least one path exists and holds more than whitespace."""
    return any(path.exists() and path.read_text(encoding="utf-8").strip() for path in paths)


def listing(title: str, note: str, names: list[str]) -> list[str]:
    """One section headed by its own reason, then the names it covers."""
    return ["", f"## {title}", "", note, ""] + [f"- `{name}`" for name in names] + [""]


def nothing_ran(reason: str) -> list[str]:
    return ["## Nothing was re-run", "",
            reason or "No source a benchmark measures changed since the previous run, so "
            "nothing needed measuring. `bench/bench-map.json` decides that, and "
            "`tools/check_bench_map.py` refuses an entry it does not name.", ""]


def render(args: argparse.Namespace,
           reports: list[pathlib.Path],
           comparisons: list[pathlib.Path] | None = None) -> str:
    lines = [HEADER, "", MARKER.format(sha=args.baseline or "none"),
             OWED.format(classes=" ".join(args.owed)), CUT.format(names=" ".join(args.cut)), ""]
    lines.append(PREAMBLE.format(performance=PERFORMANCE).strip())
    lines += ["", "## This run", "", f"- Commit: `{args.commit}`",
              f"- Previous run: `{args.baseline or 'none — every entry was selected'}`",
              f"- Runner: {args.runner}", ""]

    if not args.selected and not args.harnesses and not args.owed and not args.failed:
        return collapse(lines + nothing_ran(args.reason))

    if args.selected:
        lines += listing(
            "Classes re-run",
            "Selected by `tools/select_benchmarks.py` from the sources that changed since "
            "the previous run, and what earlier nights owed:", args.selected)
        lines += included_reports(reports)

    if args.harnesses:
        lines += listing(
            "Against rapidfuzz, in this same run",
            "Both sides on this VM in these minutes, which is what makes the ratio readable "
            "where the absolutes are not.", args.harnesses)
        lines += included_reports(comparisons or [])

    lines += owed_sections(args)

    if args.failed:
        lines += listing(
            "Failed tonight",
            "These exited with an error, raised, shared a script that failed tonight, or are named by "
            "the map and declared by no project; the log's annotation on each says which. The run "
            "went on, so the "
            "rest are on this page. A failure is not owed: it is selected again only when something it "
            "reads changes.",
            args.failed)

    return collapse(lines)


def owed_sections(args: argparse.Namespace) -> list[str]:
    """What is carried to tomorrow: classes and comparisons not reached, and apart, what was cut."""
    lines: list[str] = []
    if not args.owed:
        return lines
    comparisons_owed = set(harness_names())
    cut = set(args.cut)
    classes = [name for name in args.owed if name not in comparisons_owed and name not in cut]
    owed_comparisons = [name for name in args.owed if name in comparisons_owed and name not in cut]
    reason = ("The run stops starting them when the time left cannot hold one, so these were "
              "carried to the next run rather than measured badly or killed mid-flight. "
              "They are asked for first tomorrow whether or not anything else changes.")
    if classes:
        lines += listing("Classes selected, and not reached tonight", reason, classes)
    if owed_comparisons:
        lines += listing("Comparisons selected, and not reached tonight", reason, owed_comparisons)
    if args.cut:
        lines += listing(
            "Asked for last",
            "Each was cut at its phase's stop, tonight or on an earlier night and not run since, or "
            "left unsettled by a step that died. They are asked for after everything else, so one that "
            "does not fit the window takes only what the rest leave.", args.cut)
    return lines


def harness_names() -> list[str]:
    """The comparisons the map knows, to tell an owed comparison from an owed class (#1687)."""
    if not select_benchmarks.MAP.exists():
        return []
    return list(json.loads(select_benchmarks.MAP.read_text(encoding="utf-8")).get("harnesses", {}))


def collapse(lines: list[str]) -> str:
    """One blank line where the assembly left several: the page is linted like any other."""
    out: list[str] = []
    for line in lines:
        if line == "" and out and out[-1] == "":
            continue
        out.append(line)
    return "\n".join(out).strip() + "\n"


def marker(text: str, template: str) -> str | None:
    """A marker's value as the page holds it, or None when the page has no such marker."""
    head, tail = template.split("{", 1)[0], template.split("}", 1)[1]
    for line in text.splitlines():
        if line.startswith(head) and line.endswith(tail):
            return line[len(head):len(line) - len(tail)].strip()
    return None


# The subject of the wiki commit each night on main makes, before its pull request is merged;
# test_nightly_budget ties it to the workflow's own `git commit -m`.
NIGHT_COMMIT = "Nightly benchmark run for "

# How many of the wiki's nights the baseline looks back over: a run of quiet nights is far shorter.
NIGHTS_READ = 30


def wiki_show(wiki: pathlib.Path, sha: str) -> str:
    """The page as it stood at one wiki commit, or empty when that revision lacked it."""
    if not REVISION.fullmatch(sha):
        return ""
    out = subprocess.run(["git", "show", f"{sha}:{PAGE.name}"], cwd=wiki, capture_output=True, check=False)
    return out.stdout.decode("utf-8", errors="replace") if out.returncode == 0 else ""


def descends(older: str, newer: str) -> bool:
    """Whether newer is a strict descendant of older in this repository's history."""
    if older == newer or not (REVISION.fullmatch(older) and REVISION.fullmatch(newer)):
        return False
    return subprocess.run(["git", "merge-base", "--is-ancestor", older, newer],
                          cwd=ROOT, capture_output=True, check=False).returncode == 0


def latest_page(wiki: pathlib.Path, checked_out: str) -> str:
    """The page of the latest night measured, from its wiki commit or the checked-out page (#1754).

    The checked-out page moves only when a night's pull request is merged, so while one was open the
    next night re-measured its classes. The newest night commit is not always it: a quiet night
    republishes main's page under the same subject. So the page whose baseline is furthest along
    main's history wins, the newest of those sharing it, and the checked-out one where none is ahead.
    """
    try:
        shas = subprocess.run(
            ["git", "log", f"-{NIGHTS_READ}", "--format=%H", "--fixed-strings", f"--grep={NIGHT_COMMIT}", "--", PAGE.name],
            cwd=wiki, capture_output=True, text=True, check=True).stdout.split()
    except (OSError, subprocess.CalledProcessError) as error:
        print(f"::warning::the wiki's nights were unreadable ({error}); reading the checked-out page", file=sys.stderr)
        return checked_out
    best, best_sha, from_wiki = checked_out, marker(checked_out, MARKER) or "", False
    for sha in shas:
        page = wiki_show(wiki, sha)
        night = marker(page, MARKER) or ""
        if REVISION.fullmatch(night) and (
                not best_sha or descends(best_sha, night) or (night == best_sha and not from_wiki)):
            best, best_sha, from_wiki = page, night, True
    return best


def markers(text: str) -> list[str]:
    """The three values the next night reads, as GITHUB_OUTPUT lines, through one parser."""
    sha = marker(text, MARKER) or ""
    return [f"sha={sha if REVISION.fullmatch(sha) else ''}",
            f"owed={' '.join((marker(text, OWED) or '').split())}",
            f"cut={' '.join((marker(text, CUT) or '').split())}"]


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    flag = "store_true"
    parser.add_argument("--commit")
    parser.add_argument("--baseline", default="")
    parser.add_argument("--runner", default="ubuntu-latest")
    parser.add_argument("--selected", nargs="*", default=[])
    parser.add_argument("--harnesses", nargs="*", default=[])
    parser.add_argument("--owed", nargs="*", default=[],
                        help="classes and comparisons selected and not reached, carried to the next run")
    parser.add_argument("--failed", nargs="*", default=[],
                        help="classes and comparisons that exited with an error, raised, or name no project")
    parser.add_argument("--cut", nargs="*", default=[],
                        help="among the owed, those asked for last: cut, or left by a step that died")
    parser.add_argument("--reason", default="")
    parser.add_argument("--branch", action=flag,
                        help="write BRANCH_PAGE instead of PAGE (a workflow_dispatch off main)")
    parser.add_argument("--stdout", action=flag, help="print instead of writing")
    parser.add_argument("--markers", action=flag,
                        help="print the page's baseline, owed and cut as GITHUB_OUTPUT lines")
    parser.add_argument("--wiki", type=pathlib.Path,
                        help="with --markers, read the newest night's page from this wiki clone first")
    args = parser.parse_args()

    if args.markers:
        # An unreadable byte costs that marker, not the night: an unknown baseline measures everything.
        text = PAGE.read_text(encoding="utf-8", errors="replace") if PAGE.exists() else ""
        print("\n".join(markers(latest_page(args.wiki, text) if args.wiki else text)))
        return
    if not args.commit:
        parser.error("--commit is required to render the page")

    reports = sorted(ARTIFACTS.glob("*-report-github.md")) if ARTIFACTS.is_dir() else []
    comparisons = sorted(COMPARISONS.glob("compare-*.md")) if COMPARISONS.is_dir() else []
    page = render(args, reports, comparisons)
    if args.stdout:
        print(page, end="")
        return

    target = BRANCH_PAGE if args.branch else PAGE

    # Untouched only when nothing was selected: the markers travel through the page alone, and one
    # left from an earlier night would ask for what tonight settled (#1687).
    if (not has_content(reports) and not has_content(comparisons)
            and not (args.selected or args.harnesses or args.owed or args.failed)):
        print(f"nothing worth publishing; {target.relative_to(ROOT)} left unchanged")
        return

    target.parent.mkdir(parents=True, exist_ok=True)
    # S2083 and S8707 read a tainted path here. target is one of two fixed constants
    # chosen by a boolean flag, not an argument: dropping --branch did not convince.
    target.write_text(page, encoding="utf-8")  # NOSONAR
    print(f"-> {target.relative_to(ROOT)}")


if __name__ == "__main__":
    main()
