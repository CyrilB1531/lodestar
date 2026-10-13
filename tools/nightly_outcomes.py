"""What the nightly's class loop and its comparisons share: a bounded run, and where each outcome goes (#1687).

Its own module, so `run_bounded.py`, which every class goes through, imports nothing it does not
need: a top-level change to the comparisons' runner or to the selection cannot break the class
loop, which writes the page.
"""

from __future__ import annotations

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

    def settle(self, name: str, outcome: str, cut_before: Sequence[str] = ()) -> None:
        """``late`` never started and is owed. ``cut`` is owed and noted as cut, unless the previous
        night cut it too: owed goes first, so one that never fits would be cut first every night
        ahead of the rest, and is failed as too long for the window instead. A night that only
        skipped it does not count, since it never had the window. ``failed`` is failed: owed, it
        would be asked for first every night and crowd out the rest; fatal, it would stop the page
        as a timeout did."""
        if outcome == "ran":
            self.ran.append(name)
        elif outcome == "late":
            self.owed.append(name)
        elif outcome == "cut" and name not in cut_before:
            print(f"::warning::{name} was still running at the stop; it is carried to the next run",
                  flush=True)
            self.owed.append(name)
            self.cut.append(name)
        elif outcome == "cut":
            annotate(f"{name} was cut on two nights running; it does not fit the window")
            self.failed.append(name)
        else:
            self.failed.append(name)

    def append_to(self, ran: pathlib.Path, directory: pathlib.Path = pathlib.Path()) -> None:
        """Appends each list to its file under ``directory``; ``ran`` is the caller's own."""
        for path, names in ((ran, self.ran), (OWED, self.owed), (FAILED, self.failed), (CUT, self.cut)):
            with (directory / path).open("a", encoding=ENCODING) as sink:
                sink.writelines(f"{name}\n" for name in names)
