#!/usr/bin/env python3
"""Compare two directories of oracle corpora the way the test suites do.

The `Oracles are reproducible` gate used to regenerate the corpora on the
runner and `git diff --quiet` them. That asserts byte-identity, which is a
stronger property than anything the suites check and one no machine can hold:
the singular values in `decomposition_svd.json` disagree between hosts in
their twelfth significant digit (`-0.0026268786319` against
`-0.00262687863191`), while the suites compare at `1e-9` absolute. Rounding
cannot close that gap from either side -- at 12 significant digits the
disagreement is still in the last digit kept, and at 10 the rounding itself
breaks 13 tests (`1.13e-8` of error on a singular value of 22.606). So the
gate compares what the tests compare, and this is what does it.

Numerically means floats only, at `TOLERANCE` -- except for the corpora
`WIDER` names, held to the wider tolerance their own suite holds them to, and the
ones `RELATIVE` names, compared relatively as their suite compares them, each with
the reason beside the entry. Everything else -- integers, strings,
booleans, nulls, the set and order of an object's keys, an array's length and
order, and the set of files -- is compared exactly, because a corpus that
gained a case, lost a field or reordered one has changed in a way no
floating-point unit can explain. A non-finite value is compared exactly too:
a tolerance around an infinity means nothing.

tools/compare_oracles.py records why the gate asserts this rather than byte-identity.

Usage:  python tools/compare_oracles.py <expected-dir> <actual-dir>
        python tools/compare_oracles.py --help

  <expected-dir>  The corpora as committed.
  <actual-dir>    The corpora as regenerated.
  --help, -h      Print this message to stdout and exit 0.

Exit:   0 clean, 1 differences printed, 2 bad usage
"""

from __future__ import annotations

import json
import math
import pathlib
import sys
from collections import namedtuple
from typing import NamedTuple

# The absolute tolerance the oracle-replaying suites compare floats at, per CLAUDE.md:
# moving one without the other leaves the gate asserting what the tests do not.
TOLERANCE = 1e-9

# long-comment: a wider tolerance on one corpus is the kind of loosening that hides a
# regression, so the reason belongs beside it rather than in a commit message.
# One corpus is held wider, by the same number its own suite holds it to. The exponent
# `preprocessing_power.json` freezes is found by maximising a log-likelihood whose curvature at
# the optimum is about 176 against a value near 443: a double carries that to roughly 3e-11, so
# the data pins the exponent only to about 6e-7 and two hosts' BLAS reductions land on different
# sides of it -- measured, 6.7e-9 apart on lambda and 5e-9 on the values it transforms.
# docs/equivalence.md carries the arithmetic, and PreprocessingOracleAsserts.PowerTolerance is
# the same 1e-5. Everything else is 1e-9.
WIDER: dict[str, float] = {"preprocessing_power.json": 1e-5}


class Relative(NamedTuple):
    """A tolerance proportional to the expected value, with an absolute floor for values near zero."""

    rate: float
    floor: float


# stats_iv.json is compared relatively, as IvOracleTests is: two hosts' BLAS moved a Wald statistic
# of 6.8e6 by 4e-4, 6e-11 relative, which an absolute 1e-9 refuses and the suite accepts.
RELATIVE: dict[str, Relative] = {"stats_iv.json": Relative(1e-9, 1e-15)}

class CaseRule(NamedTuple):
    """Wider tolerances for some fields of the cases a field's value selects, as their suite holds them."""

    field: str
    value: str
    tolerances: dict[str, float]


# long-comment: why the generalized gamma's cases are held wider, field by field.
# ParametricOracleTests holds the generalized gamma at 2e-9 and its inference at 1e-6, lifelines' own
# standard errors at 1e-3: its incomplete gamma's shape derivative is a finite difference, and the
# corpus's Hessian is one of values, both amplifying the last bit gammaincc rounds differently on two
# hosts -- measured, 4e-8 on a standard error and 3e-5 on lifelines' (#1172). Every other case of the
# corpus, and every other field, stays at 1e-9.
CASE_WIDER: dict[str, CaseRule] = {
    "survival_parametric.json": CaseRule("model", "GeneralizedGamma", {
        "parameters": 2e-9, "logLikelihood": 2e-9, "aic": 2e-9, "survival": 2e-9, "cumulativeHazard": 2e-9,
        "hazard": 2e-9, "median": 1e-8, "quartile": 1e-8, "standardErrors": 1e-6, "zStatistics": 1e-6,
        "pValues": 1e-6, "survivalBounds": 1e-6, "cumulativeHazardBounds": 1e-6, "lifelinesStandardErrors": 1e-3}),
}

# What reaches the log. The workflow uploads both directories when this fails,
# so the cap costs a reader nothing and keeps a wholesale mismatch readable.
MAX_REPORTED = 40

# Differences past this are counted but not kept: the count stays exact while
# the memory a pathological run needs stays bounded.
COLLECT_LIMIT = 1000

# How long a value may print before it is elided -- a vocabulary entry or a
# tokenized sentence would otherwise take the line over on its own.
MAX_RENDERED = 60

# Ranks, low first: a corpus that lost a field explains the value differences
# under it, so it is the line a reader wants at the top.
STRUCTURAL = 0
NUMERIC = 1

Difference = namedtuple("Difference", "rank path detail")


class Differences:
    """Every difference found, of which the first `COLLECT_LIMIT` are kept.

    Two directories that share nothing produce a difference per value, and
    building a list of those is neither useful to a reader nor kind to the
    runner. The total is still counted exactly, so the summary line is
    honest about how much was not printed.
    """

    def __init__(self) -> None:
        self.kept: list[Difference] = []
        self.total = 0

    def add(self, rank: int, path: str, detail: str) -> None:
        self.total += 1
        if len(self.kept) < COLLECT_LIMIT:
            self.kept.append(Difference(rank, path, detail))

    def ordered(self) -> list[Difference]:
        """The kept differences, structural ones first, insertion order within a rank."""
        return sorted(self.kept, key=lambda difference: difference.rank)


def kind_of(value) -> str:
    """The JSON kind of `value`, as a word a difference line can carry.

    `bool` is checked before `int` because it is a subclass of it in Python,
    so `True` would otherwise be reported -- and compared -- as the integer 1.
    """
    if isinstance(value, bool):
        return "boolean"
    if isinstance(value, int):
        return "integer"
    if isinstance(value, float):
        return "float"
    if isinstance(value, str):
        return "string"
    if value is None:
        return "null"
    if isinstance(value, list):
        return "array"
    return "object"


def floats_agree(expected: float, actual: float, tolerance: float | Relative = TOLERANCE) -> bool:
    """Whether two floats agree at `tolerance`, with non-finite values compared exactly.

    A tolerance around an infinity or a NaN asserts nothing: `inf - inf` is
    NaN and every comparison against a NaN is false, so both are settled by
    identity instead. Two NaNs agree here, which `==` would not say.
    """
    if math.isnan(expected) or math.isnan(actual):
        return math.isnan(expected) and math.isnan(actual)
    if math.isinf(expected) or math.isinf(actual):
        return expected == actual
    if isinstance(tolerance, Relative):
        return abs(expected - actual) <= tolerance.rate * abs(expected) + tolerance.floor
    return abs(expected - actual) <= tolerance


def render(value) -> str:
    """`value` as a difference line prints it, elided past `MAX_RENDERED`."""
    text = json.dumps(value) if isinstance(value, (str, bool, type(None))) else repr(value)
    return text if len(text) <= MAX_RENDERED else text[:MAX_RENDERED] + "…"


def _child(path: str, step: str) -> str:
    return step if not path else f"{path}.{step}"


def compare_values(path: str, expected, actual, found: Differences,
                   tolerance: float | Relative = TOLERANCE, rule: CaseRule | None = None) -> None:
    """Walk two parsed values in step, recording what differs and where."""
    expected_kind = kind_of(expected)
    actual_kind = kind_of(actual)
    if expected_kind != actual_kind:
        found.add(STRUCTURAL, path,
                  f"{expected_kind} {render(expected)} vs {actual_kind} {render(actual)}")
        return

    if expected_kind == "object":
        _compare_objects(path, expected, actual, found, tolerance, rule)
    elif expected_kind == "array":
        _compare_arrays(path, expected, actual, found, tolerance, rule)
    elif expected_kind == "float":
        if not floats_agree(expected, actual, tolerance):
            found.add(NUMERIC, path, f"{render(expected)} vs {render(actual)}")
    elif expected != actual:
        found.add(STRUCTURAL, path, f"{render(expected)} vs {render(actual)}")


def _compare_objects(path: str, expected: dict, actual: dict, found: Differences,
                     tolerance: float | Relative, rule: CaseRule | None = None) -> None:
    for key in expected:
        if key not in actual:
            found.add(STRUCTURAL, _child(path, key), "in the expected corpus only")
    for key in actual:
        if key not in expected:
            found.add(STRUCTURAL, _child(path, key), "in the actual corpus only")

    # The generator writes its keys in a fixed order, so a reordering is a
    # change to the generator and not to the machine that ran it.
    shared_expected = [key for key in expected if key in actual]
    shared_actual = [key for key in actual if key in expected]
    if shared_expected != shared_actual:
        found.add(STRUCTURAL, path,
                  f"keys reordered: {render(shared_expected)} vs {render(shared_actual)}")

    selected = rule is not None and expected.get(rule.field) == rule.value
    for key in shared_expected:
        wider = rule.tolerances.get(key, tolerance) if selected else tolerance
        compare_values(_child(path, key), expected[key], actual[key], found, wider, rule)


def _compare_arrays(path: str, expected: list, actual: list, found: Differences,
                    tolerance: float | Relative, rule: CaseRule | None = None) -> None:
    if len(expected) != len(actual):
        found.add(STRUCTURAL, path, f"{len(expected)} elements vs {len(actual)}")

    for index in range(min(len(expected), len(actual))):
        compare_values(f"{path}[{index}]", expected[index], actual[index], found, tolerance, rule)


def corpus_files(directory: pathlib.Path) -> list[str]:
    """Every file under `directory`, as sorted repository-style relative paths."""
    return sorted(path.relative_to(directory).as_posix()
                  for path in directory.rglob("*") if path.is_file())


def compare_file(name: str, expected_path: pathlib.Path, actual_path: pathlib.Path,
                 found: Differences) -> None:
    """One corpus: parsed and walked when it is JSON, compared byte for byte otherwise.

    The fixtures that are not JSON -- the SentencePiece models, the ONNX
    graphs, the GPT-2 merge table -- hold no float this script could round, so
    for them byte-identity is both what the tests need and all there is.
    """
    if not name.endswith(".json"):
        if expected_path.read_bytes() != actual_path.read_bytes():
            found.add(STRUCTURAL, name, "the bytes differ")
        return

    try:
        expected = json.loads(expected_path.read_text(encoding="utf-8"))
        actual = json.loads(actual_path.read_text(encoding="utf-8"))
    except (UnicodeDecodeError, json.JSONDecodeError) as error:
        found.add(STRUCTURAL, name, f"could not be read as JSON: {error}")
        return

    nested = Differences()
    compare_values("", expected, actual, nested, RELATIVE.get(name) or WIDER.get(name, TOLERANCE), CASE_WIDER.get(name))
    for difference in nested.kept:
        found.add(difference.rank, f"{name}: {difference.path or '<root>'}", difference.detail)
    found.total += nested.total - len(nested.kept)


def compare_directories(expected_dir: pathlib.Path, actual_dir: pathlib.Path) -> Differences:
    """Both file sets, then each file the two have in common."""
    found = Differences()
    expected_names = corpus_files(expected_dir)
    actual_names = corpus_files(actual_dir)
    expected_set = frozenset(expected_names)
    actual_set = frozenset(actual_names)

    for name in expected_names:
        if name not in actual_set:
            found.add(STRUCTURAL, name, "in the expected corpora only")
    for name in actual_names:
        if name not in expected_set:
            found.add(STRUCTURAL, name, "in the actual corpora only")

    for name in expected_names:
        if name in actual_set:
            compare_file(name, expected_dir / name, actual_dir / name, found)

    return found


def _parse_arguments(arguments: list[str]) -> tuple[int | None, list[str]]:
    """Handle `--help`/`-h` and demand exactly two directory arguments.

    Returns the exit code main() should return immediately, or None to mean
    "keep going", alongside the two paths.
    """
    if "--help" in arguments or "-h" in arguments:
        print(__doc__)
        return 0, []

    if len(arguments) != 2 or any(argument.startswith("-") for argument in arguments):
        print(__doc__, file=sys.stderr)
        return 2, []

    return None, arguments


def main(argv: list[str]) -> int:
    early_exit, arguments = _parse_arguments(argv[1:])
    if early_exit is not None:
        return early_exit

    expected_dir, actual_dir = (pathlib.Path(argument) for argument in arguments)
    for directory in (expected_dir, actual_dir):
        if not directory.is_dir():
            print(f"::error::{directory} is not a directory", file=sys.stderr)
            return 2

    found = compare_directories(expected_dir, actual_dir)

    if not found.total:
        wider = ", ".join([f"{name} within {value:g}" for name, value in sorted(WIDER.items())]
                          + [f"{name} within {value.rate:g} relative" for name, value in sorted(RELATIVE.items())]
                          + [f"{name}'s {rule.value} cases wider by field" for name, rule in sorted(CASE_WIDER.items())])
        print(f"ok  {len(corpus_files(expected_dir))} corpora agree: floats within "
              f"{TOLERANCE:g} ({wider}), everything else exactly")
        return 0

    for difference in found.ordered()[:MAX_REPORTED]:
        print(f"::error::{difference.path}: {difference.detail}")

    remaining = found.total - min(len(found.kept), MAX_REPORTED)
    if remaining:
        print(f"::error::and {remaining} further difference(s), not printed")
    return 1


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
