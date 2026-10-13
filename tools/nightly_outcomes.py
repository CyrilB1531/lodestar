"""What the nightly's class loop and its comparisons share: a bounded run, and where each outcome goes (#1687).

Its own module, so `run_bounded.py`, which every class goes through, imports nothing it does not
need: a top-level change to the comparisons' runner or to the selection cannot break the class
loop, which writes the page.
"""

from __future__ import annotations

import argparse
import dataclasses
import os
import pathlib
import signal
import subprocess
from collections.abc import Sequence

ENCODING = "utf-8"

# Where a night's outcomes are appended, relative to the job's working directory: fixed here rather
# than passed in, so no path a step writes comes from its command line.
OWED, FAILED, CUT = pathlib.Path("owed.txt"), pathlib.Path("failed.txt"), pathlib.Path("cut.txt")


def annotate(message: str) -> None:
    """One GitHub error annotation, flushed so it lands inside the group it belongs to."""
    print(f"::error::{message}", flush=True)


def run_command(command: Sequence[str], limit: float, output: pathlib.Path | None) -> bool:
    """Run one command for at most ``limit`` seconds; False when it had to be cut.

    In a session of its own, so a cut kills the whole group: `dotnet run` starts the benchmark
    as a child of its own, which killing `dotnet` alone would leave running past the job.
    """
    if limit <= 0:
        return False
    try:
        # No stdin: the class loop reads its names from one, which a child reading it would swallow.
        process = subprocess.Popen(command, start_new_session=True, stdin=subprocess.DEVNULL,
                                   stdout=subprocess.PIPE if output is not None else None, text=True)
    except OSError as error:
        # A command that cannot start is a failure like one that exits nonzero, not a crash of the step.
        raise subprocess.CalledProcessError(127, list(command)) from error
    with process:
        try:
            captured, _ = process.communicate(timeout=limit)
        except subprocess.TimeoutExpired:
            try:
                os.killpg(process.pid, signal.SIGKILL)
            except ProcessLookupError:
                pass  # the group finished between the timeout and the kill
            process.communicate()
            return False
    if output is not None:
        output.write_text(captured, encoding=ENCODING)
        # The plain table is what a human reads in the log, as `tee` showed it; the gfm copy is for
        # the page alone. Shown even when the command failed, so its diagnostics are not lost.
        if output.suffix == ".txt" or process.returncode != 0:
            print(captured, end="", flush=True)
    if process.returncode != 0:
        raise subprocess.CalledProcessError(process.returncode, list(command))
    return True


@dataclasses.dataclass
class Outcomes:
    """Where each entry of a night went: the class loop and the comparisons settle through this one
    rule, so the two cannot drift apart (#1687)."""

    ran: list[str] = dataclasses.field(default_factory=list)
    owed: list[str] = dataclasses.field(default_factory=list)
    failed: list[str] = dataclasses.field(default_factory=list)
    cut: list[str] = dataclasses.field(default_factory=list)

    def settle(self, name: str, outcome: str) -> None:
        """``late`` never started and is owed. ``cut`` is owed and noted as cut, which the next night
        asks for last rather than first (select_benchmarks.with_owed), so one that never fits uses
        only what the window has left (#1694). ``failed`` is failed: owed, it would be asked for first
        every night and crowd out the rest; fatal, it would stop the page as a timeout did."""
        if outcome == "ran":
            self.ran.append(name)
        elif outcome == "late":
            self.owed.append(name)
        elif outcome == "cut":
            print(f"::warning::{name} was still running at the stop; it is carried to the next run",
                  flush=True)
            self.owed.append(name)
            self.cut.append(name)
        else:
            self.failed.append(name)

    def extend(self, other: Outcomes) -> None:
        for field in dataclasses.fields(self):
            getattr(self, field.name).extend(getattr(other, field.name))

    def append_to(self, ran: pathlib.Path, directory: pathlib.Path = pathlib.Path()) -> None:
        """Appends each list to its file under ``directory``; ``ran`` is the caller's own."""
        for path, names in ((ran, self.ran), (OWED, self.owed), (FAILED, self.failed), (CUT, self.cut)):
            with (directory / path).open("a", encoding=ENCODING) as sink:
                sink.writelines(f"{name}\n" for name in names)


def carry(selected: list[str], settled: set[str], owed: set[str], cut: set[str],
          cut_before: list[str]) -> tuple[list[str], list[str]]:
    """What the page step adds to owed.txt and cut.txt (#1694, #1695).

    A name the night selected and a dying step left unsettled is owed, as the baseline advances past
    it (#650), and asked for last, so one that kills its step every night cannot starve the rest. A
    name cut on an earlier night and still owed stays last until it runs; otherwise it would go
    first every other night.
    """
    unsettled = sorted(set(selected) - settled)
    kept = sorted((set(cut_before) & (owed | set(unsettled))) - cut - set(unsettled))
    return unsettled, unsettled + kept


def read_names(path: pathlib.Path) -> list[str]:
    """One name a line, as every step writes them; the one reader of those files."""
    return path.read_text(encoding=ENCODING).split() if path.exists() else []


def main(argv: Sequence[str] | None = None) -> int:
    """`carry`: settle what tonight left unsettled, from the fixed files the measuring steps wrote."""
    parser = argparse.ArgumentParser(description=main.__doc__)
    parser.add_argument("command", choices=["carry"])
    parser.add_argument("--cut-before", default="", help="what the previous page cut, space-separated")
    args = parser.parse_args(argv)

    selected = read_names(pathlib.Path("selected.txt")) + read_names(pathlib.Path("harnesses.txt"))
    owed = read_names(OWED)
    settled = {name for path in (pathlib.Path("classes-run.txt"), pathlib.Path("harnesses-run.txt"), FAILED)
               for name in read_names(path)} | set(owed)
    owed_add, cut_add = carry(selected, settled, set(owed), set(read_names(CUT)), args.cut_before.split())
    for path, names in ((OWED, owed_add), (CUT, cut_add)):
        with path.open("a", encoding=ENCODING) as sink:
            sink.writelines(f"{name}\n" for name in names)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
