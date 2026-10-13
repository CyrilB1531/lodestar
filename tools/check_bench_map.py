#!/usr/bin/env python3
"""Refuse a benchmark that no night selects, or that no build reaches.

Two silences, and they end the same way: a benchmark that exists, compiles in
someone's editor, and is never measured, with nothing going red. That is the gap
#11 exists to close.

The first is a class bench/bench-map.json does not name. The nightly run (#11)
executes only the benchmark classes whose sources changed since the previous
run, and it reads that map to decide which those are, so a class missing from it
is never selected. The map cannot be derived: FuzzBenchmarks names only Fuzz,
which reaches Indel, then Lcs, then Affixes; LevenshteinCodePointBenchmarks
depends on the decoder in Text/. Naming conventions do not carry that, so the
map is hand-written, and this keeps it honest about the one thing it can check
there: completeness.

The second is a project Lodestar.slnx does not list, which is the same silence
one layer down -- no build reaches it, so it is never compiled rather than never
measured. That is what #649 cost, and solution_findings() below has the
reasoning and the case.

What it does NOT check is whether a class's globs are *right*. Being too narrow
is invisible here and is why the map is written at directory granularity: a
benchmark run for nothing costs minutes, one not run hides a regression.

Usage:  python tools/check_bench_map.py

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import json
import pathlib
import re
import sys

import run_bounded

ROOT = pathlib.Path(__file__).resolve().parent.parent
MAP = ROOT / "bench" / "bench-map.json"
BENCH_DIR = ROOT / "bench" / "Lodestar.Text.Benchmarks"
PROGRAM = BENCH_DIR / "Program.cs"
PYTHON_DIR = ROOT / "bench" / "python"
WORKFLOWS = ROOT / ".github" / "workflows"
SOLUTION = ROOT / "Lodestar.slnx"
# Three findings say a file this gate reads is not there, and S1192 counts the third.
MISSING = ": missing"
# Every finding here names a workflow by its repository-relative path, and S1192
# fires on the third spelling of one literal. `label(path)` is that one spelling.
WORKFLOW_DIR = ".github/workflows"

# Program.cs dispatches with `case "name":` arms, each starting its line, so one in a comment
# does not count; a chain of `args[0] == "name"` passed the analyser's complexity bar at nine.
SUBCOMMAND = re.compile(r'^\s*case "(compare[a-z-]*)"', re.MULTILINE)
ANY_SUBCOMMAND = re.compile(r'^\s*case "([a-z][a-z-]*)"', re.MULTILINE)
# `dotnet run --project ... -- <first>`: the space before the bare `--` is what tells it
# apart from `--project`, whose dashes are not followed by one.
INVOCATION = re.compile(r"dotnet run\b.*?\s--\s+(\S+)")
CONTINUATION = re.compile(r"\\\n\s*")
# Whether Program.cs's default arm lets an option through to BenchmarkSwitcher (#478).
FORWARDS_OPTIONS = re.compile(r"StartsWith\('-'\)")
FILE_NAMESPACE = re.compile(r"^\s*namespace\s+[\w.]+\s*;", re.MULTILINE)
# What run_comparisons.plan reads from every harness, as strings; a missing one raises mid-night (#1695).
HARNESS_FIELDS = ("python", "subcommand")
HARNESSES = "harnesses"
SOURCE = run_bounded.SOURCE
TEXT_PROJECT = BENCH_DIR.relative_to(ROOT).as_posix()
# The globs select_benchmarks reads on every harness.
SOURCES = "sources"
# The corpus generators a harness may run first, optional.
GENERATE = "generate"
# The optional mode run_comparisons.plan hands bench/compare.py.
MODE = "compare"
# The one statement a Program.cs other than Lodestar.Text.Benchmarks' may hold, and its using directives.
SWITCHER_ONLY = re.compile(r"\s*BenchmarkSwitcher\s*\.\s*FromAssembly\s*\((?:[^;()]|\([^;()]*\))*\)"
                           r"\s*\.\s*Run\s*\(\s*args\s*\)\s*;\s*")
# What else a Program.cs may hold: using directives and whole-line comments.
USINGS = re.compile(r"^\s*(?:using\s+[\w.]+\s*;|//.*)$", re.MULTILINE)
COMMENT_LINE = re.compile(r"^\s*//.*$", re.MULTILINE)
BENCHMARKS = "benchmarks"
MAP_HARNESS = "bench/bench-map.json: harness"


def label(path: pathlib.Path) -> str:
    """A workflow named the way a finding has to name it: relative to the repository."""
    return f"{WORKFLOW_DIR}/{path.name}"


def declared_classes() -> dict[str, pathlib.Path]:
    """Every benchmark class, as run_bounded.declarations finds them: the gate and the runner count
    the same classes, one a file, and a helper declared beside one is not one."""
    return {match.group(2): path for match, path, _ in run_bounded.declarations()}


def declared_harnesses(program: str) -> set[str]:
    """Every cross-language subcommand Program.cs dispatches, and every bench_*.py."""
    found = set(SUBCOMMAND.findall(program))
    found |= {path.name for path in PYTHON_DIR.glob("bench_*.py")}
    return found


def diagnostic_findings(diagnostics: list, program: str | None = None) -> list[str]:
    """A diagnostic is exempt from the nightly, not from existing.

    roc-parallel, save-phases and heap-warmth answer a question a lot asks once, so no
    harness names them and the nightly never runs one. That is deliberate. What is not
    deliberate is a renamed subcommand or a deleted script leaving an entry pointing at
    nothing, which is the same rot the harness rules catch.
    """
    dispatched = set(ANY_SUBCOMMAND.findall(program_text() if program is None else program))
    findings = [
        f"bench/bench-map.json: diagnostic '{entry['subcommand']}' is not dispatched by "
        f"bench/Lodestar.Text.Benchmarks/Program.cs"
        for entry in diagnostics if entry.get("subcommand") not in dispatched
    ]
    findings += [
        f"bench/bench-map.json: diagnostic '{entry['subcommand']}' names {entry['python']}, "
        f"which does not exist"
        for entry in diagnostics
        if "python" in entry and not (ROOT / entry["python"]).is_file()
    ]
    return findings


def partition_diagnostics(diagnostics) -> tuple[list[str], list[dict], list[dict]]:
    """The shape findings, the well-formed diagnostics every other rule judges -- an object with a
    `subcommand` string and, if it names one, a `python` script given as a relative path -- and every
    diagnostic object, which still names its script when a field is malformed."""
    if not isinstance(diagnostics, list):
        return ["bench/bench-map.json: 'diagnostics' is not a list"], [], []
    findings: list[str] = []
    good = []
    for entry in diagnostics:
        if (isinstance(entry, dict) and isinstance(entry.get("subcommand"), str)
                and ("python" not in entry or (isinstance(entry["python"], str) and relative(entry["python"])))):
            good.append(entry)
        else:
            findings.append(f"bench/bench-map.json: diagnostic {entry!r} is not an object with a 'subcommand' "
                            f"string and, if any, a relative 'python' path")
    return findings, good, [entry for entry in diagnostics if isinstance(entry, dict)]


def harness_findings(named: dict, named_diagnostics: list, program: str) -> list[str]:
    """A comparison the map does not carry is one the nightly never runs. ``named`` and
    ``named_diagnostics`` are every harness and diagnostic object, malformed ones included, since an
    entry with a bad field still names its scripts; the well-formed ones are judged elsewhere."""
    mapped_subcommands = {entry["subcommand"] for entry in named.values()
                          if isinstance(entry.get("subcommand"), str)}
    mapped_python = {pathlib.Path(entry["python"]).name for entry in named.values()
                     if isinstance(entry.get("python"), str)}
    # A diagnostic's Python half is named here rather than by a harness, on purpose; a malformed
    # diagnostic still names its script, as a malformed harness does.
    mapped_python |= {pathlib.Path(entry["python"]).name for entry in named_diagnostics
                      if isinstance(entry.get("python"), str)}
    declared = declared_harnesses(program)

    findings = [
        f"bench/Lodestar.Text.Benchmarks/Program.cs: '{name}' is dispatched and no harness in "
        f"bench/bench-map.json names it, so the nightly would never run that comparison"
        for name in sorted(declared & {n for n in declared if n.startswith("compare")})
        if name not in mapped_subcommands
    ]
    findings += [
        f"bench/python/{name}: no harness in bench/bench-map.json names it"
        for name in sorted(n for n in declared if n.endswith(".py")) if name not in mapped_python
    ]
    return findings


def script_findings(harnesses: dict) -> list[str]:
    """A well-formed harness whose Python side or corpus generator is not a file."""
    findings = [
        f"{MAP_HARNESS} '{key}' names {entry['python']}, which does not exist"
        for key, entry in sorted(harnesses.items())
        if not (ROOT / entry["python"]).is_file()
    ]
    findings += [
        f"{MAP_HARNESS} '{key}' generates with {script}, which does not exist"
        for key, entry in sorted(harnesses.items())
        for script in entry.get(GENERATE, []) if not (ROOT / script).is_file()
    ]
    return findings


def shape_of(key: str, entry) -> list[str]:
    """What is wrong with one harness's shape: the gate's rules and run_comparisons read every field."""
    if not isinstance(entry, dict):
        return [f"{MAP_HARNESS} '{key}' is not an object"]
    findings = [f"{MAP_HARNESS} '{key}' has no '{field}' string, which tools/run_comparisons.py reads"
                for field in HARNESS_FIELDS if not isinstance(entry.get(field), str)]
    if isinstance(entry.get("python"), str) and not relative(entry["python"]):
        findings.append(f"{MAP_HARNESS} '{key}' has a 'python' that is not a relative path")
    if MODE in entry and not isinstance(entry[MODE], str):
        findings.append(f"{MAP_HARNESS} '{key}' has a '{MODE}' that is not a string")
    # select_benchmarks reads `sources` on every harness; `generate` is optional.
    fields = [SOURCES] + ([GENERATE] if GENERATE in entry else [])
    findings += [f"{MAP_HARNESS} '{key}' has a '{field}' that is not a list of relative paths"
                 for field in fields if not relative_paths(entry.get(field))]
    return findings


def relative_paths(value) -> bool:
    """A list of non-empty relative paths or globs, which pathlib's glob and ROOT / path both take."""
    return isinstance(value, list) and all(isinstance(x, str) and relative(x) for x in value)


def relative(path: str) -> bool:
    """Below the repository root and spelled the POSIX way, so it reads the same on every machine."""
    parts = pathlib.PurePosixPath(path).parts
    return bool(parts) and parts[0] != "/" and ".." not in parts and not any(c in path for c in "\\:")


def partition(harnesses) -> tuple[list[str], dict, dict]:
    """The shape findings, the well-formed harnesses every other rule judges, and every harness
    object, which still names its scripts when a field is malformed."""
    if not isinstance(harnesses, dict):
        return ["bench/bench-map.json: 'harnesses' is not an object"], {}, {}
    findings: list[str] = []
    good = {}
    for key, entry in sorted(harnesses.items()):
        problems = shape_of(key, entry)
        findings += problems
        if not problems:
            good[key] = entry
    return findings, good, {key: entry for key, entry in harnesses.items() if isinstance(entry, dict)}


def coverage_findings(mapped: dict, declared: dict) -> list[str]:
    """A [Benchmark] class the map forgot, and a mapped name that no longer is one."""
    findings = [
        f"{path.relative_to(ROOT)}: {name} carries [Benchmark] and is not in "
        f"bench/bench-map.json, so the nightly run would never select it"
        for name, path in declared.items() if name not in mapped
    ]
    findings += [
        f"bench/bench-map.json: {name} is mapped and no longer declares a [Benchmark]"
        for name in sorted(set(mapped) - set(declared))
    ]
    return findings


def glob_findings(data: dict) -> list[str]:
    """A glob matching nothing is a rename nobody carried through."""
    findings = [
        f"bench/bench-map.json: {name}'s '{glob}' matches nothing"
        for name, globs in sorted(data.get(BENCHMARKS, {}).items())
        for glob in globs if not any(ROOT.glob(glob))
    ]
    findings += [
        f"bench/bench-map.json: harness {key}'s '{glob}' matches nothing"
        for key, entry in sorted(data.get(HARNESSES, {}).items())
        for glob in entry.get(SOURCES, []) if not any(ROOT.glob(glob))
    ]
    findings += [
        f"bench/bench-map.json: 'always' entry '{glob}' matches nothing"
        for glob in data.get("always", []) if not any(ROOT.glob(glob))
    ]
    return findings


def dispatch_only_findings(data: dict) -> list[str]:
    """A 'dispatch_only' key whose file or whose construct has moved.

    select_benchmarks.py exempts the named construct from the "always" rule, and every
    failure mode there falls to "not exempt" -- safely, but silently, so a key left
    pointing at a renamed file or at a switch that has become something else costs the
    nightly its whole benchmark selection without a word. That is the rot the harness
    and glob rules exist to break, and this key was outside them.

    One occurrence, anchored the way strip_construct anchors it: none means the file has
    stopped having the construct, more than one means the exemption is ambiguous, and
    neither is a mapping worth keeping.
    """
    findings = []
    for path, keyword in sorted(data.get("dispatch_only", {}).items()):
        source = ROOT / path
        if not source.exists():
            findings.append(
                f"bench/bench-map.json: 'dispatch_only' names {path}, which does not exist")
            continue
        anchor = re.compile(rf"(?m)^[ \t]*{re.escape(keyword)}\s*\(")
        found = len(anchor.findall(source.read_text(encoding="utf-8")))
        if found != 1:
            findings.append(
                f"bench/bench-map.json: 'dispatch_only' names '{keyword}' in {path}, which "
                f"holds {found} of them; tools/select_benchmarks.py exempts nothing but one")
    return findings


def invocation_findings(program: str) -> list[str]:
    """A workflow's own arguments, against what Program.cs accepts.

    #478: the refusal added for a mistyped subcommand also caught `--filter`, which is
    BenchmarkDotNet's and not a subcommand at all, so every scheduled nightly exited 2
    before its first measurement. Nothing saw it because the nightly is the only caller
    that leads with an option, and it is the one workflow no pull request runs -- so the
    check for it belongs where a pull request does run, which is here.

    Only the first argument, which is the one Program.cs judges. A mistyped option later
    in the line is a hazard this does not cover and #470's refusal never reached either:
    BenchmarkDotNet answers an unknown option with its help screen and exit 0, so such a
    nightly is green having measured nothing. Verified, not assumed -- `--nonsense-option`
    exits 0 here.
    """
    findings = []
    for path in sorted(WORKFLOWS.glob("*.yml")):
        text = CONTINUATION.sub(" ", path.read_text(encoding="utf-8"))
        for first in INVOCATION.findall(text):
            token = first.strip('"\'')
            if "$" in token:
                continue  # A subcommand read from bench-map.json; the map rules check it.
            findings += judge_first(label(path), token, program)
    return findings


def program_text() -> str:
    """Program.cs without its whole-line comments, so neither a `case` arm nor the option check
    is found in one."""
    text = PROGRAM.read_text(encoding=SOURCE) if PROGRAM.exists() else ""
    return COMMENT_LINE.sub("", text)


def judge_first(where: str, token: str, program: str, project: str = TEXT_PROJECT) -> list[str]:
    """One command's first argument after `--`, against the Program.cs of the project it runs.

    Only Lodestar.Text.Benchmarks dispatches subcommands; the other projects hand every argument
    to BenchmarkSwitcher, which takes an option and refuses nothing.
    """
    if project != TEXT_PROJECT:
        return forwarding_findings(where, project)
    if token.startswith("-"):
        if FORWARDS_OPTIONS.search(program):
            return []
        return [f"{where}: passes '{token}' first, which bench/Lodestar.Text.Benchmarks/Program.cs "
                f"refuses as an unknown subcommand rather than forwarding to BenchmarkDotNet"]
    if token in set(ANY_SUBCOMMAND.findall(program)):
        return []
    return [f"{where}: passes '{token}' first, which bench/Lodestar.Text.Benchmarks/Program.cs "
            f"does not dispatch"]


def forwarding_findings(where: str, project: str) -> list[str]:
    """A project other than Lodestar.Text.Benchmarks has to hand its arguments to BenchmarkSwitcher
    untouched: one dispatching its own first argument would refuse `--filter`, as #478's did. Its
    Program.cs is held to the one statement all three are, using directives and whole-line comments
    aside; any other shape is a finding, and widening this rule is the change reviewed with it."""
    program = ROOT / project / "Program.cs"
    code = program.read_text(encoding=SOURCE) if program.exists() else ""
    if SWITCHER_ONLY.fullmatch(USINGS.sub("", code)):
        return []
    return [f"{where}: {project}/Program.cs is not the one statement "
            f"BenchmarkSwitcher.FromAssembly(...).Run(args), so the nightly's leading --filter may be refused"]


def first_after_separator(command: list[str]) -> str:
    """What `dotnet run ... -- <first>` hands the program first."""
    return command[command.index("--") + 1]


def project_arg(command: list[str]) -> str:
    return command[command.index("--project") + 1]


def nightly_command_findings(harnesses: dict, program: str) -> list[str]:
    """The commands the nightly builds in tools/, judged as the workflow's own once were (#1695);
    ``harnesses`` are the well-formed ones, as main hands them over.

    #1689 moved them out of bench-nightly.yml into run_bounded.command_for and run_comparisons.plan,
    where `invocation_findings`' scan of the workflows no longer reaches; #478's refusal would have
    failed every class again and no pull request would have said so.
    """
    import run_comparisons

    findings = []
    for project in run_bounded.PROJECTS:
        command = run_bounded.command_for("AnyBenchmarks", project)
        findings += judge_first("tools/run_bounded.py", first_after_separator(command), program,
                                project_arg(command))
    comparisons = set(SUBCOMMAND.findall(program))
    for key, entry in sorted(harnesses.items()):
        for command, _ in run_comparisons.plan(entry, key, ROOT):
            token = first_after_separator(command) if command[0] == "dotnet" else None
            # A comparison's first argument is its subcommand, never an option BenchmarkDotNet would take.
            if token is not None and token not in comparisons:
                findings.append(f"tools/run_comparisons.py, harness '{key}': runs '{token}', which "
                                f"bench/Lodestar.Text.Benchmarks/Program.cs does not dispatch as a comparison")
    return findings


def class_shape_findings(mapped) -> list[str]:
    """A mapped class the nightly's `--filter "*.Name.*"` cannot match (#1695).

    BenchmarkDotNet matches the filter against Namespace.Type.Method: a class outside a namespace
    has no leading segment, and a generic one is named Name<T>. Either matches nothing, and
    BenchmarkDotNet exits 0, so the class settles as run with no report -- #586's symptom. Every
    benchmark file declares a file-scoped namespace, and that convention is what is held: proving a
    block namespace encloses a class needs a C# parser.
    """
    wanted = set(mapped)
    findings = []
    for match, path, text in run_bounded.declarations():
        modifiers, name = match.group(1).split(), match.group(2)
        if name not in wanted:
            continue
        where = path.relative_to(ROOT).as_posix()
        unmatched = f"so the nightly's --filter \"*.{name}.*\" matches none of its benchmarks"
        if match.group(3):
            findings.append(f"{where}: {name} is generic, {unmatched}")
        if not FILE_NAMESPACE.search(text):
            findings.append(f"{where}: {name} is in no file-scoped namespace, {unmatched}")
        # BenchmarkDotNet runs public types only, and none sealed, abstract or static.
        if "public" not in modifiers or {"sealed", "abstract", "static"} & set(modifiers):
            findings.append(f"{where}: {name} is not a plain public class, which BenchmarkDotNet "
                            f"runs, {unmatched}")
    return findings


def solution_findings() -> list[str]:
    """Every project under bench/, against the solution that would compile it.

    #649: Lodestar.slnx listed three of the four benchmark projects, and
    bench/Lodestar.Gpu.Benchmarks was the fourth. So `dotnet build Lodestar.slnx` never
    touched it, and when #622 raised SonarAnalyzer.CSharp from 10.20 to 10.34 the new
    S4790 broke it silently -- a bump validated against every project the solution knows
    about, which was all of them but that one. The nightly is what would have found it,
    on whichever night a change first touched src/Lodestar.Gpu or Similarity.

    A project outside the solution is not forbidden -- samples/ is deliberately outside
    (CONTRIBUTING.md's Definition of done) -- but a benchmark project is compiled by the nightly and so has to stay
    compilable, which only a build reaching it can promise.
    """
    if not SOLUTION.exists():
        return [f"{SOLUTION.name}{MISSING}"]

    listed = SOLUTION.read_text(encoding="utf-8")
    return [
        f"{project.relative_to(ROOT).as_posix()} is not in {SOLUTION.name}, so no build "
        f"reaches it and an analyser bump can break it unseen (#649)"
        for project in sorted((ROOT / "bench").glob("*/*.csproj"))
        if project.relative_to(ROOT).as_posix() not in listed
    ]


def main() -> int:
    if len(sys.argv) > 1:
        print(__doc__)
        return 0 if sys.argv[1] in ("--help", "-h") else 2

    if not MAP.exists():
        print(f"{MAP.relative_to(ROOT)}{MISSING}")
        return 1

    data = json.loads(MAP.read_text(encoding="utf-8"))
    program = program_text()
    findings = coverage_findings(data.get(BENCHMARKS, {}), declared_classes())
    findings += class_shape_findings(data.get(BENCHMARKS, {}))
    # Every rule after the shapes judges well-formed entries alone; the malformed are named, and
    # still count as naming their scripts.
    shape, data[HARNESSES], named = partition(data.get(HARNESSES, {}))
    diagnostic_shape, diagnostics, named_diagnostics = partition_diagnostics(data.get("diagnostics", []))
    findings += shape + diagnostic_shape
    findings += harness_findings(named, named_diagnostics, program)
    findings += script_findings(data[HARNESSES])
    findings += diagnostic_findings(diagnostics, program)
    findings += invocation_findings(program)
    findings += nightly_command_findings(data.get(HARNESSES, {}), program)
    findings += solution_findings()
    findings += glob_findings(data)
    findings += dispatch_only_findings(data)

    for finding in findings:
        print(finding)
    return 1 if findings else 0


if __name__ == "__main__":
    raise SystemExit(main())
