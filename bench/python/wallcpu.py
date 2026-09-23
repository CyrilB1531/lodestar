"""The measure-and-report half every wall/cpu harness in this directory repeats.

`bench_splitters.py`, `bench_transformers.py` and `bench_stats.py` each carry their own
copy of `measure` and `payload_for`. That is the standing backlog rather than this
module's subject: what it exists for is that the *next* harness does not add a fourth,
which SonarCloud's duplication gate on new code is what said out loud (#1123, 16.4% on
`bench_cdist.py` before this).

`harness.py` next door is the other shape -- `time_bucket` and `run`, for the distance
harnesses that read a bucketed corpus. This one is for the harnesses that time a named
operation and report elapsed and processor time side by side.

The methodology is the one the C# side mirrors: auto-scale until a measurement lasts
MIN_TIME, report the best of REPEATS, and record `perf_counter` and `process_time`
together because elapsed time hides a thread pool on either side.
"""

from __future__ import annotations

import json
import platform
from pathlib import Path
from time import perf_counter, process_time
from typing import Callable

MIN_TIME = 0.5
REPEATS = 5


def measure(operation: str, action: Callable[[], object]) -> dict:
    """Time one operation, recording both elapsed time and processor time."""
    best_wall, cpu_of_best = float("inf"), float("nan")
    for _ in range(REPEATS):
        iters = 1
        while True:
            c0, w0 = process_time(), perf_counter()
            for _ in range(iters):
                action()
            dt = perf_counter() - w0
            cpu = process_time() - c0
            if dt >= MIN_TIME:
                break
            iters *= 2
        wall_ms = dt / iters * 1e3
        if wall_ms < best_wall:
            best_wall, cpu_of_best = wall_ms, cpu / iters * 1e3
    print(f"  {operation:<28} {best_wall:10.3f} ms/op  cpu {cpu_of_best:8.3f} ms/op")
    return {"operation": operation, "ms_per_op": best_wall, "cpu_ms_per_op": cpu_of_best}


def payload_for(results: list[dict], libraries: dict[str, str]) -> dict:
    """The document `bench/compare.py` reads, with the versions that produced it."""
    return {
        "metadata": {
            "side": "python",
            "libraries": libraries,
            "python": platform.python_version(),
            "machine": platform.machine(),
            "min_time_s": MIN_TIME,
            "repeats": REPEATS,
        },
        "results": results,
    }


def write(out: Path, results: list[dict], libraries: dict[str, str]) -> None:
    """Writes the payload where `bench/compare.py` looks for it."""
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(json.dumps(payload_for(results, libraries), indent=2) + "\n", encoding="utf-8")
    print(f"-> {out}")
