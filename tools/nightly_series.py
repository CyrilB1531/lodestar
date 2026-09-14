#!/usr/bin/env python3
"""Give the nightly benchmark a memory: a committed series of ratios, and a comparison (#672).

`docs/guides/nightly_run.md` is overwritten every night and `benchmark_latest.md` carries old
sections forward, so nothing compared one night with the next. A ratio moved from 0.97 to 1.72
between two nights (#673) and turned nothing red. This keeps every BenchmarkDotNet `Ratio` the
page publishes in `bench/nightly/ratios.csv`, one row per reading, and reports the ratios that
stepped or drifted past the thresholds decision 0126 measured.

Only the `Ratio` column is kept. The page says why: a hosted runner is a different VM every
night, so an absolute mean is not comparable across nights, while a ratio against a baseline
measured in the same minute is. The runner's CPU is kept beside it, since five processor
models served the first 26 days and a new one moves some ratios on its own (#675).

Usage:
    python tools/nightly_series.py backfill [--wiki DIR]
    python tools/nightly_series.py compare [--branch] [--date YYYY-MM-DD] [--wiki DIR]
    python tools/nightly_series.py record --date YYYY-MM-DD [--wiki DIR]

`backfill` rebuilds `bench/nightly/ratios.csv` from the git history of the page on the current
branch. `compare` reads tonight's page against the series, appends a section to the page, and
prints one `::warning::` per new movement; `--branch` reads and writes the branch page instead.
`record` appends tonight's main page to the series. The nightly calls `compare` before `record`,
so tonight is never compared with itself, and only a run on `main` records.
"""

from __future__ import annotations

import argparse
import csv
import datetime
import io
import pathlib
import re
import statistics
import subprocess
import sys
from dataclasses import dataclass

ROOT = pathlib.Path(__file__).resolve().parent.parent
PAGE = pathlib.Path("docs") / "guides" / "nightly_run.md"
# Every path this writes is one of these constants, chosen by a flag, never read from an
# argument: the same rule render_nightly.py and render_benchmark_latest.py keep.
MAIN_PAGE = ROOT / PAGE
BRANCH_PAGE = ROOT / "docs" / "guides" / "branch" / "nightly_run.md"
SERIES = ROOT / "bench" / "nightly" / "ratios.csv"

ENCODING = "utf-8"
# The series' own column names, spelled once: a row is written and read by them.
DATE, COMMIT_FIELD, CPU, CLASS, PARAMETERS, METHOD_FIELD, RATIO_FIELD = (
    "date", "commit", "cpu", "class", "parameters", "method", "ratio")
FIELDS = [DATE, COMMIT_FIELD, CPU, CLASS, PARAMETERS, METHOD_FIELD, RATIO_FIELD]

# BenchmarkDotNet's own column names; everything between Method and Mean is a parameter.
MEAN = "Mean"
RATIO = "Ratio"
METHOD = "Method"

SECTION = re.compile(r"^### (?:[\w.]+\.)?(?P<cls>\w+)-report-github\s*$")
COMMIT = re.compile(r"^- Commit: `(?P<sha>[0-9a-f]{7,40})`")
# The line after BenchmarkDotNet's banner names the processor, e.g. "AMD EPYC 7763, 1 CPU, ...".
PROCESSOR = re.compile(r"^(?P<cpu>[^,|`]+), \d+ CPU\b")

# Decision 0126, measured by replaying the series rebuilt from the page's history.
WINDOW = 5  # readings a median is taken over
MINIMUM_HISTORY = 3  # earlier readings a key needs before it is compared at all
STEP_FLOOR = 0.30  # a step smaller than this is never reported
STEP_NOISE_MULTIPLE = 4.0  # ... nor one smaller than this many times the key's own noise
DRIFT = 0.20  # the recent median against the median of readings at least DRIFT_DAYS older
DRIFT_DAYS = 10
RECENT = 3

ADR = "../decisions/0126-the-nightly-reports-a-ratio-that-steps-past-its-noise-or-drifts-over-ten-days.md"
MOVED_HEADING = "## Ratios that moved"


@dataclass(frozen=True)
class Reading:
    """One `Ratio` cell: a method, at one set of parameters, in one class, on one run."""

    date: str
    commit: str
    cpu: str
    cls: str
    parameters: str
    method: str
    ratio: float

    @property
    def key(self) -> tuple[str, str, str]:
        return (self.cls, self.parameters, self.method)

    def row(self) -> dict[str, str]:
        return {
            DATE: self.date, COMMIT_FIELD: self.commit, CPU: self.cpu, CLASS: self.cls,
            PARAMETERS: self.parameters, METHOD_FIELD: self.method, RATIO_FIELD: repr(self.ratio),
        }


def _cells(line: str) -> list[str]:
    return [cell.strip().strip("*").strip() for cell in line.strip().strip("|").split("|")]


def _parse_table(lines: list[str], start: int) -> tuple[list[tuple[str, str, float]], int]:
    """One BenchmarkDotNet table, from its header row: (parameters, method, ratio) per row."""
    header = _cells(lines[start])
    if RATIO not in header or MEAN not in header:
        return [], start + 1
    method_at, mean_at, ratio_at = header.index(METHOD), header.index(MEAN), header.index(RATIO)
    names = header[method_at + 1:mean_at]
    rows = []
    index = start + 2  # past the header and its |---| separator
    while index < len(lines) and lines[index].startswith("|"):
        cells = _cells(lines[index])
        index += 1
        if len(cells) != len(header) or not cells[method_at]:
            continue  # the blank separator row BenchmarkDotNet puts between parameter groups
        try:
            ratio = float(cells[ratio_at].replace(",", ""))
        except ValueError:
            continue  # "?" or "NA": no ratio could be formed for this row
        values = cells[method_at + 1:mean_at]
        parameters = ", ".join(f"{name}={value}" for name, value in zip(names, values, strict=True))
        rows.append((parameters, cells[method_at], ratio))
    return rows, index


def parse_page(text: str, date: str) -> list[Reading]:
    """Every `Ratio` cell on a nightly page, with its class, parameters, method and CPU."""
    lines = text.splitlines()
    commit, cls, cpu = "", None, ""
    readings: list[Reading] = []
    index = 0
    while index < len(lines):
        line = lines[index]
        if match := COMMIT.match(line):
            commit = match.group("sha")
        elif match := SECTION.match(line):
            cls, cpu = match.group("cls"), ""
        elif line.startswith("## "):
            cls = None
        elif cls and (match := PROCESSOR.match(line)):
            cpu = match.group("cpu").strip()
        elif cls and line.startswith(f"| {METHOD}"):
            rows, index = _parse_table(lines, index)
            readings.extend(Reading(date, commit, cpu, cls, p, m, r) for p, m, r in rows)
            continue
        index += 1
    return readings


def read_series(path: pathlib.Path) -> list[Reading]:
    if not path.exists():
        return []
    with path.open(encoding=ENCODING, newline="") as handle:
        return [
            Reading(row[DATE], row[COMMIT_FIELD], row[CPU], row[CLASS], row[PARAMETERS],
                    row[METHOD_FIELD], float(row[RATIO_FIELD]))
            for row in csv.DictReader(handle)
        ]


def write_series(path: pathlib.Path, readings: list[Reading]) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    buffer = io.StringIO()
    writer = csv.DictWriter(buffer, fieldnames=FIELDS, lineterminator="\n")
    writer.writeheader()
    for reading in readings:
        writer.writerow(reading.row())
    path.write_text(buffer.getvalue(), encoding=ENCODING)


def merge(existing: list[Reading], tonight: list[Reading]) -> list[Reading]:
    """Appends tonight, replacing an earlier reading of the same key on the same measured commit."""
    replaced = {(r.commit, *r.key) for r in tonight}
    return [r for r in existing if (r.commit, *r.key) not in replaced] + tonight


@dataclass(frozen=True)
class Movement:
    reading: Reading
    kind: str  # "step" or "drift"
    reference: float
    change: float
    threshold: float
    persisting: bool = False


def _median(readings: list[Reading]) -> float:
    return statistics.median(r.ratio for r in readings)


def _noise(earlier: list[Reading]) -> float | None:
    """How far this key's readings usually sit from the median of the readings before them."""
    deviations = []
    for index in range(MINIMUM_HISTORY, len(earlier)):
        reference = _median(earlier[max(0, index - WINDOW):index])
        if reference > 0.0:
            deviations.append(abs(earlier[index].ratio / reference - 1.0))
    return statistics.median(deviations) if len(deviations) >= MINIMUM_HISTORY else None


def _days(later: str, earlier: str) -> int:
    return (datetime.date.fromisoformat(later) - datetime.date.fromisoformat(earlier)).days


def assess(earlier: list[Reading], reading: Reading) -> Movement | None:
    """Whether `reading` stepped away from its recent median, or its recent median drifted.

    A step compares the reading with the median of the key's last WINDOW readings, against a
    threshold of STEP_FLOOR or STEP_NOISE_MULTIPLE times the key's own noise, whichever is larger:
    a nanosecond-scale kernel moves more between VMs than a whole-corpus encode does. A drift
    compares the median of the last RECENT readings, tonight's included, with the median of the
    readings at least DRIFT_DAYS older, and catches what no single night shows (#674).
    """
    if len(earlier) < MINIMUM_HISTORY:
        return None
    reference = _median(earlier[-WINDOW:])
    noise = _noise(earlier)
    threshold = STEP_FLOOR if noise is None else max(STEP_FLOOR, STEP_NOISE_MULTIPLE * noise)
    if reference > 0.0 and abs(reading.ratio / reference - 1.0) > threshold:
        return Movement(reading, "step", reference, reading.ratio / reference - 1.0, threshold)
    if not reading.date:
        return None
    old = [r for r in earlier if r.date and _days(reading.date, r.date) >= DRIFT_DAYS][-WINDOW:]
    if len(old) < MINIMUM_HISTORY:
        return None
    before, recent = _median(old), _median((earlier + [reading])[-RECENT:])
    if before > 0.0 and abs(recent / before - 1.0) > DRIFT:
        return Movement(reading, "drift", before, recent / before - 1.0, DRIFT)
    return None


def compare(history: list[Reading], tonight: list[Reading]) -> list[Movement]:
    """Every reading tonight that stepped or drifted, marked persisting when its key's previous
    reading had already moved, so a step is headlined once rather than every night the median lags."""
    by_key: dict[tuple[str, str, str], list[Reading]] = {}
    for reading in history:
        by_key.setdefault(reading.key, []).append(reading)
    moved = []
    for reading in tonight:
        earlier = [r for r in by_key.get(reading.key, []) if r.commit != reading.commit]
        movement = assess(earlier, reading)
        if movement is None:
            continue
        persisting = bool(earlier) and assess(earlier[:-1], earlier[-1]) is not None
        moved.append(Movement(movement.reading, movement.kind, movement.reference, movement.change,
                              movement.threshold, persisting))
    return sorted(moved, key=lambda m: (m.persisting, -abs(m.change)))


def _table(movements: list[Movement]) -> list[str]:
    lines = [
        "| class | parameters | method | kind | tonight | against | change | threshold |",
        "| --- | --- | --- | --- | ---: | ---: | ---: | ---: |",
    ]
    for m in movements:
        r = m.reading
        lines.append(
            f"| `{r.cls}` | {r.parameters or '—'} | `{r.method}` | {m.kind} | {r.ratio:g} | "
            f"{m.reference:g} | {m.change:+.0%} | {m.threshold:.0%} |")
    return lines


def render(moved: list[Movement]) -> str:
    fresh = [m for m in moved if not m.persisting]
    still = [m for m in moved if m.persisting]
    lines = [
        MOVED_HEADING, "",
        f"Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds of "
        f"[decision 0126]({ADR}). A ratio moves when either side of it does: read its baseline before "
        "calling a movement a regression.", "",
    ]
    if not moved:
        lines.append("No ratio stepped past its noise or drifted over ten days tonight.")
        return "\n".join(lines) + "\n"
    # Bold rather than '### ': render_benchmark_latest.py takes every level-3 heading on an old
    # nightly page for a benchmark class, and would carry these forward as one.
    lines += [f"**New tonight: {len(fresh)}.** Each of these had not moved on its previous reading.", ""]
    lines += _table(fresh) if fresh else ["None."]
    if still:
        lines += ["", f"**Still away from their median: {len(still)}.** These moved on an earlier run, "
                  "and their median has not caught up yet.", ""]
        lines += _table(still)
    return "\n".join(lines) + "\n"


def _on_this_history(commit: str) -> bool:
    found = subprocess.run(
        ["git", "-C", str(ROOT), "merge-base", "--is-ancestor", commit, "HEAD"],
        capture_output=True, check=False)
    return found.returncode == 0


def wiki_readings(wiki: pathlib.Path, known: set[str]) -> list[Reading]:
    """The nightly pages the wiki holds for runs the series does not, oldest first.

    The wiki is pushed by every run on main whether or not that night's pull request is merged,
    so a night left unmerged is still here. Reading it keeps the series from losing that night,
    which the committed file alone would. A page whose measured commit is not in this checkout's
    history is left out: before #367 a dispatched branch published too, and two of the wiki's runs
    measured branch commits that no longer exist.
    """
    # Imported here rather than at the top: a sibling tool, only needed when a wiki is given.
    import render_benchmark_latest as latest  # noqa: PLC0415

    readings: list[Reading] = []
    for sha in reversed(latest.commits(wiki, 10_000)):
        body = latest.show(wiki, sha)
        source = latest.source_commit(body) if body else ""
        if not body or source in known or not _on_this_history(source):
            continue
        readings = merge(readings, parse_page(body, latest.commit_date(wiki, sha)))
    return readings


def backfill(series: pathlib.Path, wiki: pathlib.Path | None) -> int:
    log = subprocess.run(
        ["git", "-C", str(ROOT), "log", "--reverse", "--format=%H %cs", "--", PAGE.as_posix()],
        check=True, capture_output=True, text=True).stdout.split()
    readings: list[Reading] = []
    revisions = 0
    for sha, date in zip(log[0::2], log[1::2], strict=True):
        text = subprocess.run(
            ["git", "-C", str(ROOT), "show", f"{sha}:{PAGE.as_posix()}"],
            check=True, capture_output=True, text=True).stdout
        readings = merge(readings, parse_page(text, date))
        revisions += 1
    if wiki:
        readings = merge(wiki_readings(wiki, {r.commit for r in readings}), readings)
        readings.sort(key=lambda r: r.date)
    write_series(series, readings)
    print(f"{len(readings)} readings from {revisions} page revisions -> {series}")
    return 0


def main(argv: list[str]) -> int:
    parser = argparse.ArgumentParser(description=__doc__.split("\n", 1)[0])
    wiki_flag = "--wiki"
    wiki_help = "a wiki clone, to fold in runs the series or the repository's history lacks"
    sub = parser.add_subparsers(dest="command", required=True)
    b = sub.add_parser("backfill")
    b.add_argument(wiki_flag, type=pathlib.Path, help=wiki_help)
    c = sub.add_parser("compare")
    c.add_argument("--branch", action="store_true", help="the branch page, not main's")
    c.add_argument("--date", default=datetime.date.today().isoformat())
    c.add_argument(wiki_flag, type=pathlib.Path, help=wiki_help)
    r = sub.add_parser("record")
    r.add_argument("--date", required=True)
    r.add_argument(wiki_flag, type=pathlib.Path, help=wiki_help)
    args = parser.parse_args(argv[1:])

    if args.command == "backfill":
        return backfill(SERIES, args.wiki)
    page = BRANCH_PAGE if getattr(args, "branch", False) else MAIN_PAGE
    text = page.read_text(encoding=ENCODING)
    tonight = parse_page(text, args.date)
    history = read_series(SERIES)
    if args.wiki:
        history = merge(history, wiki_readings(args.wiki, {r.commit for r in history}))
    if args.command == "record":
        write_series(SERIES, merge(history, tonight))
        print(f"recorded {len(tonight)} readings for {args.date}")
        return 0
    moved = compare(history, tonight)
    if MOVED_HEADING in text:
        text = text[:text.index(MOVED_HEADING)]
    page.write_text(text.rstrip("\n") + "\n\n" + render(moved), encoding=ENCODING)
    for m in (m for m in moved if not m.persisting):
        r = m.reading
        print(f"::warning::{r.cls} {r.method} {r.parameters} {m.kind} {m.change:+.0%}: "
              f"{r.ratio:g} against {m.reference:g}")
    print(f"{sum(not m.persisting for m in moved)} new, {sum(m.persisting for m in moved)} persisting")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
