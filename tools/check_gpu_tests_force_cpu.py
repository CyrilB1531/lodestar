#!/usr/bin/env python3
"""Refuse a GPU correctness test that does not force ILGPU's CPU accelerator (#444).

Lodestar.Gpu's suites run in CI on a runner with no graphics hardware, and
GpuContext.Create() falls back to the CPU accelerator rather than throwing. That
fallback is deliberate and it is also the trap: a test written with a bare Create()
passes on a developer machine *using the GPU* and passes on the runner *using the
processor*, so the suite silently asserts different things in different places.

Nothing else can catch it. Both outcomes are green, the difference is invisible in a
log, and the bug it hides is the kind that only appears on a card -- a group size, a
memory limit, a synchronisation the CPU accelerator happens to serialise.

So a correctness test opens its context with `preferCpu: true`, and the few that
deliberately probe the preferred device are named below with the reason. Same shape as
tools/check_bench_map.py: read the thing itself and compare, so a divergence fails on
the commit that introduces it.

Benchmarks are deliberately out of scope. Their whole purpose is the device a machine
actually has, and decision 0102 requires them to report which one produced a figure
rather than to force one.

Usage:  python tools/check_gpu_tests_force_cpu.py
        python tools/check_gpu_tests_force_cpu.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SUITES = ROOT / "tests"
PATTERN = "Lodestar.Gpu*.Tests"

# A call that opens a context. The argument list is captured so the check reads what was
# passed rather than whether the word appears somewhere on the line.
CREATE = re.compile(r"GpuContext\.Create\((?P<arguments>[^)]*)\)")

# The enclosing declaration, so a finding names the test a reader has to open.
DECLARATION = re.compile(r"^\s*(?:public|private|internal)[\w\s<>\[\],?]*?\s(?P<name>\w+)\s*\(",
                         re.MULTILINE)

# Tests whose subject IS the preferred device, with the reason each is exempt.
EXEMPT = {
    # Asserts that an OpenCL runtime over a processor is not reported as graphics
    # hardware. Forcing the CPU accelerator would remove the thing being tested.
    "A_runtime_over_a_processor_is_not_reported_as_graphics_hardware",
}


def enclosing(text: str, position: int) -> str:
    """The name of the member a character offset sits inside."""
    name = "(unknown)"
    for found in DECLARATION.finditer(text):
        if found.start() > position:
            break
        name = found.group("name")

    return name


def findings() -> list[str]:
    """Every bare Create() outside the exemptions, so one fix does not hide the next."""
    found: list[str] = []
    suites = sorted(SUITES.glob(PATTERN))
    if not suites:
        return [f"tests/: no {PATTERN} directory, so this guard is watching nothing."]

    for suite in suites:
        for source in sorted(suite.rglob("*.cs")):
            if "obj" in source.parts or "bin" in source.parts:
                continue
            text = source.read_text(encoding="utf-8")
            for call in CREATE.finditer(text):
                if "preferCpu" in call.group("arguments"):
                    continue
                member = enclosing(text, call.start())
                if member in EXEMPT:
                    continue
                line = text.count("\n", 0, call.start()) + 1
                found.append(
                    f"{source.relative_to(ROOT)}:{line}: {member} opens a context without "
                    f"preferCpu: true, so it uses whatever device the machine has. Pass "
                    f"preferCpu: true, or add the test to EXEMPT in "
                    f"tools/check_gpu_tests_force_cpu.py with the reason its subject is "
                    f"the preferred device.")

    return found


def main() -> int:
    if len(sys.argv) > 1:
        if sys.argv[1] in ("--help", "-h"):
            print(__doc__)
            return 0

        print(f"unexpected argument: {sys.argv[1]}", file=sys.stderr)
        return 2

    found = findings()
    for finding in found:
        print(f"::error::{finding}")
    if found:
        return 1

    print(f"ok  every Lodestar.Gpu test forces the CPU accelerator, bar {len(EXEMPT)} that "
          f"probe the preferred device on purpose")
    return 0


if __name__ == "__main__":
    sys.exit(main())
