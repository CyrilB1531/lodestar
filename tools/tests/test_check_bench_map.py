"""check_bench_map.py's guards on what the nightly measures.

#586: `Run them` named bench/Lodestar.Text.Benchmarks alone, so `*StatsBenchmarks*` was passed as a
filter to a project that does not declare the class; BenchmarkDotNet matched nothing, exited 0, and
the page listed the class as re-run. The gate now scans run_bounded.PROJECTS, which the nightly
resolves each class against, and holds the conventions `--filter "*.Name.*"` needs (#1695).
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_bench_map  # noqa: E402

ROOT = Path(__file__).resolve().parents[2]



def test_every_class_dir_the_gate_scans_is_a_real_directory():
    # run_bounded.PROJECTS is what the gate scans; a stale path would empty it.
    import run_bounded

    for directory in (check_bench_map.ROOT / project for project in run_bounded.PROJECTS):
        assert directory.is_dir(), f"{directory.relative_to(ROOT)} does not exist"


def test_every_bench_project_is_in_the_solution():
    """#649: two of the five were not, and an analyser bump broke one of them unseen.

    `dotnet build Lodestar.slnx` is the only thing that compiles a benchmark project
    between nightly runs, so a project the solution does not list is one nothing checks
    until the night a change happens to select it.
    """
    assert check_bench_map.solution_findings() == []


def test_a_project_the_solution_omits_is_a_finding(monkeypatch, tmp_path):
    solution = tmp_path / "Lodestar.slnx"
    solution.write_text(
        '<Solution>\n  <Project Path="bench/Lodestar.Text.Benchmarks/'
        'Lodestar.Text.Benchmarks.csproj" />\n</Solution>\n', encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "SOLUTION", solution)
    findings = check_bench_map.solution_findings()

    # One per project under bench/ the solution leaves out -- all of them but the one
    # named above, whatever that set grows to.
    projects = sorted((check_bench_map.ROOT / "bench").glob("*/*.csproj"))
    assert len(findings) == len(projects) - 1
    assert any("Lodestar.Gpu.Benchmarks" in f for f in findings)


def test_a_missing_solution_is_a_finding(monkeypatch, tmp_path):
    monkeypatch.setattr(check_bench_map, "SOLUTION", tmp_path / "absent.slnx")
    assert check_bench_map.solution_findings() == ["absent.slnx: missing"]


def test_the_nightly_commands_built_in_tools_are_judged_again(monkeypatch, tmp_path):
    """#1695: #1689 moved them out of the workflow, where the #478 gate looked for them."""
    shipped = check_bench_map.program_text()
    assert check_bench_map.nightly_command_findings(_shipped_harnesses(), shipped) == []

    broken = 'switch (args[0]) { case "compare-x": break; }'
    findings = check_bench_map.nightly_command_findings(_shipped_harnesses(), broken)

    assert any("tools/run_bounded.py: passes '--filter' first" in f for f in findings), findings
    assert any("does not dispatch" in f for f in findings), findings


def test_a_harness_without_a_key_the_runner_reads_is_a_finding():
    harnesses = {"broken": {"python": "bench/python/bench_rapidfuzz.py", "sources": ["src/**"]}}

    findings, good, _ = check_bench_map.partition(harnesses)

    assert findings == ["bench/bench-map.json: harness 'broken' has no 'subcommand' string, which "
                        "tools/run_comparisons.py reads"]
    assert good == {}, "a missing key reached the rules that read it"
    null = {"broken": {"python": "bench/python/bench_rapidfuzz.py", "subcommand": None, "sources": ["src/**"]}}
    assert check_bench_map.partition(null)[1] == {}, "a null subcommand reached the rules that read it"


def test_every_shipped_class_can_be_matched_by_the_nightly_filter():
    import json

    mapped = json.loads(check_bench_map.MAP.read_text(encoding="utf-8"))["benchmarks"]
    assert check_bench_map.class_shape_findings(mapped) == []


def _shipped_harnesses():
    import json

    return json.loads(check_bench_map.MAP.read_text(encoding="utf-8"))["harnesses"]


def test_a_malformed_harness_ends_the_gate_in_a_finding(monkeypatch, tmp_path, capsys):
    import json

    data = json.loads(check_bench_map.MAP.read_text(encoding="utf-8"))
    data["harnesses"]["broken"] = "python subcommand"
    bench_map = tmp_path / "bench-map.json"
    bench_map.write_text(json.dumps(data), encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "MAP", bench_map)
    monkeypatch.setattr(check_bench_map.sys, "argv", ["check_bench_map.py"])

    assert check_bench_map.main() == 1
    assert "harness 'broken' is not an object" in capsys.readouterr().out


def test_a_program_that_reads_args_besides_handing_them_over_is_a_finding(monkeypatch, tmp_path):
    project = tmp_path / "bench" / "P"
    project.mkdir(parents=True)
    (project / "Program.cs").write_text(
        'if (args.FirstOrDefault() == "x") { }\nBenchmarkSwitcher.FromAssembly(a).Run(args.Skip(1).ToArray());',
        encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "ROOT", tmp_path)

    assert check_bench_map.forwarding_findings("tools/run_bounded.py", "bench/P")


def _classes(monkeypatch, tmp_path, files):
    import run_bounded

    project = tmp_path / "bench" / "P"
    project.mkdir(parents=True)
    for name, text in files.items():
        (project / name).write_text(text, encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "ROOT", tmp_path)
    monkeypatch.setattr(run_bounded, "ROOT", tmp_path)
    monkeypatch.setattr(run_bounded, "PROJECTS", ("bench/P",))


def test_a_class_the_nightly_filter_cannot_match_is_a_finding(monkeypatch, tmp_path):
    _classes(monkeypatch, tmp_path, {
        "Loose.cs": "public sealed class LooseBenchmarks {\n    [Benchmark] public void A() {}\n}",
        "Generic.cs": "namespace X;\npublic abstract class GenericBenchmarks<T> {\n    [Benchmark] public void A() {}\n}",
        "Block.cs": "namespace X\n{\n    public class BlockBenchmarks {\n    [Benchmark] public void A() {}\n}\n}\n",
        "Fine.cs": "namespace X;\npublic class FineBenchmarks {\n    [Benchmark] public void A() {}\n}\n"})

    findings = check_bench_map.class_shape_findings(
        ["LooseBenchmarks", "GenericBenchmarks", "BlockBenchmarks", "FineBenchmarks"])

    assert any("LooseBenchmarks is in no file-scoped namespace" in f for f in findings), findings
    assert any("GenericBenchmarks is generic" in f for f in findings), findings
    assert any("BlockBenchmarks is in no file-scoped namespace" in f for f in findings), findings
    assert not any("FineBenchmarks" in f for f in findings), findings


def test_a_malformed_harness_is_a_finding_and_the_rest_are_still_judged():
    shape, good, _ = check_bench_map.partition(
        {"bad": {"python": 5, "subcommand": ["a"], "generate": 5},
         "good": {"python": "bench/python/nope.py", "subcommand": "compare-x", "sources": ["src/**"]},
         "text": "bench/foo.py"})
    findings = shape + check_bench_map.script_findings(good)

    assert "bench/bench-map.json: harness 'bad' has no 'python' string, which tools/run_comparisons.py reads" in findings
    assert "bench/bench-map.json: harness 'bad' has a 'generate' that is not a list of relative paths" in findings
    assert "bench/bench-map.json: harness 'text' is not an object" in findings
    assert "bench/bench-map.json: harness 'good' names bench/python/nope.py, which does not exist" in findings
    assert check_bench_map.partition([]) == (["bench/bench-map.json: 'harnesses' is not an object"], {}, {})


def test_another_project_is_held_to_the_one_switcher_statement(monkeypatch, tmp_path):
    project = tmp_path / "bench" / "P"
    project.mkdir(parents=True)
    monkeypatch.setattr(check_bench_map, "ROOT", tmp_path)
    cases = {
        "using BenchmarkDotNet.Running;\n\nBenchmarkSwitcher.FromAssembly(typeof(A.B).Assembly)\n    .Run(args);\n": False,
        "// only a comment\nBenchmarkSwitcher.FromAssembly(a).Run(args);": False,
        'switch (args[0]) { case "x": return; }\nBenchmarkSwitcher.FromAssembly(a).Run(args);': True,
        "var s = BenchmarkSwitcher.FromAssembly(a); Dispatcher.Run(args);": True,
        "BenchmarkSwitcher.FromAssembly(a).Run(args.Skip(1).ToArray());": True,
    }
    for program, refused in cases.items():
        (project / "Program.cs").write_text(program, encoding="utf-8")
        assert bool(check_bench_map.forwarding_findings("w", "bench/P")) is refused, program



def test_a_compare_mode_that_is_not_a_string_is_a_finding():
    entry = {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "compare", "compare": 5, "sources": ["src/**"]}

    assert check_bench_map.shape_of("k", entry) == ["bench/bench-map.json: harness 'k' has a 'compare' that is not a string"]


def test_an_empty_absolute_or_directory_path_is_a_finding():
    entry = {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "compare", "sources": ["", "/etc/*"]}
    assert check_bench_map.shape_of("k", entry) == [
        "bench/bench-map.json: harness 'k' has a 'sources' that is not a list of relative paths"]

    findings = check_bench_map.script_findings(
        {"k": {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "compare", "generate": ["bench"],
               "sources": ["src/**"]}})
    assert "bench/bench-map.json: harness 'k' generates with bench, which does not exist" in findings


def test_a_path_that_is_not_below_the_root_the_posix_way_is_a_finding():
    base = {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "compare", "sources": ["src/**"]}
    for sources in (["."], ["./"], ["../x"], ["C:/x"], ["a\\b"]):
        assert check_bench_map.shape_of("k", {**base, "sources": sources}), sources
    assert check_bench_map.shape_of("k", {**base, "python": "/usr/bin/env"}) == [
        "bench/bench-map.json: harness 'k' has a 'python' that is not a relative path"]


def test_a_class_or_case_mentioned_in_a_comment_is_not_a_declaration(monkeypatch, tmp_path):
    _classes(monkeypatch, tmp_path, {"Noisy.cs": (
        "// every class FooBenchmarks<T> declares\n/* class FooBenchmarks<T> */\nnamespace A;\n"
        "public class FooBenchmarks {\n    [Benchmark] public void A() {}\n}\n")})

    assert check_bench_map.class_shape_findings(["FooBenchmarks"]) == []


def test_a_case_arm_in_a_comment_dispatches_nothing():
    assert check_bench_map.judge_first("w", "ghost", '// case "ghost":\nswitch (a) { }')
    assert check_bench_map.judge_first("w", "real", 'switch (a)\n{\n    case "real":\n        break;\n}') == []



def test_an_option_check_in_a_comment_forwards_nothing(monkeypatch, tmp_path):
    program = tmp_path / "Program.cs"
    program.write_text("// option-shaped args (args[0].StartsWith('-')) reach BenchmarkSwitcher\n", encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "PROGRAM", program)

    assert check_bench_map.judge_first("w", "--filter", check_bench_map.program_text())


def test_a_harness_without_sources_and_a_malformed_diagnostic_are_findings():
    entry = {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "compare"}

    assert check_bench_map.shape_of("k", entry) == [
        "bench/bench-map.json: harness 'k' has a 'sources' that is not a list of relative paths"]
    shape, good, _ = check_bench_map.partition_diagnostics(
        [{"python": "bench/python/x.py"}, {"subcommand": "ghost-subcommand"}, {"subcommand": "s", "python": "/etc/x"}])
    assert len(shape) == 2 and good == [{"subcommand": "ghost-subcommand"}]
    assert check_bench_map.diagnostic_findings(good, "") == [
        "bench/bench-map.json: diagnostic 'ghost-subcommand' is not dispatched by bench/Lodestar.Text.Benchmarks/Program.cs"]
    assert check_bench_map.partition_diagnostics(7) == (["bench/bench-map.json: 'diagnostics' is not a list"], [], [])


def test_a_malformed_entry_ends_the_gate_in_findings_and_its_names_still_count(monkeypatch, tmp_path, capsys):
    import json

    data = json.loads(check_bench_map.MAP.read_text(encoding="utf-8"))
    data["harnesses"]["cdist"]["sources"] = "bench/**"
    data["harnesses"]["levenshtein"]["subcommand"] = ["not", "a", "string"]
    data.setdefault("diagnostics", []).append({"subcommand": "roc-parallel", "python": 5})
    bench_map = tmp_path / "bench-map.json"
    bench_map.write_text(json.dumps(data), encoding="utf-8")
    monkeypatch.setattr(check_bench_map, "MAP", bench_map)
    monkeypatch.setattr(check_bench_map.sys, "argv", ["check_bench_map.py"])

    assert check_bench_map.main() == 1
    out = capsys.readouterr().out
    assert "harness 'cdist' has a 'sources' that is not a list" in out and "diagnostic {" in out
    assert "compare-cdist' is dispatched and no harness" not in out and "bench_cdist.py: no harness" not in out


def test_a_helper_of_the_same_name_without_a_benchmark_is_not_the_class(monkeypatch, tmp_path):
    import run_bounded

    for project, text in (("A", "internal sealed class DupBenchmarks { }\n"),
                          ("B", "namespace X;\npublic class DupBenchmarks {\n    [Benchmark] public void A() {}\n}\n")):
        (tmp_path / "bench" / project).mkdir(parents=True)
        (tmp_path / "bench" / project / "F.cs").write_text(text, encoding="utf-8")
    monkeypatch.setattr(run_bounded, "ROOT", tmp_path)
    monkeypatch.setattr(run_bounded, "PROJECTS", ("bench/A", "bench/B"))
    monkeypatch.setattr(check_bench_map, "ROOT", tmp_path)

    assert run_bounded.project_of("DupBenchmarks") == "bench/B"
    assert check_bench_map.declared_classes()["DupBenchmarks"].parent.name == "B"


def test_a_harness_subcommand_must_be_a_dispatched_comparison():
    program = check_bench_map.program_text()
    harnesses = {"k": {"python": "bench/python/bench_rapidfuzz.py", "subcommand": "--list",
                       "sources": ["src/**"]}}

    assert any("runs '--list'" in f for f in check_bench_map.nightly_command_findings(harnesses, program))


def test_a_malformed_diagnostic_still_names_its_script():
    named = [{"subcommand": 5, "python": "bench/python/bench_heap_warmth.py"}]

    findings = check_bench_map.harness_findings({}, named, check_bench_map.program_text())

    assert not any("bench_heap_warmth.py: no harness" in f for f in findings)


def test_the_benchmark_class_is_the_one_declared_before_the_first_benchmark(monkeypatch, tmp_path):
    """One a file, as the runner finds it: a helper beside it is not one, whatever its name."""
    _classes(monkeypatch, tmp_path, {
        "A.cs": "namespace X;\npublic sealed class FooRow { }\npublic class FooComparison {\n    [Benchmark] public void A() {}\n}\n",
        "B.cs": "namespace X;\npublic class BucketRouteDiagnostics {\n    [Benchmark] public void A() {}\n}\npublic class Helper { }\n"})

    assert sorted(check_bench_map.declared_classes()) == ["BucketRouteDiagnostics", "FooComparison"]
    findings = check_bench_map.coverage_findings({"FooRow": []}, check_bench_map.declared_classes())
    assert "bench/bench-map.json: FooRow is mapped and no longer declares a [Benchmark]" in findings
    assert any("FooComparison carries [Benchmark] and is not in" in f for f in findings)


def test_a_class_benchmarkdotnet_will_not_run_is_a_finding(monkeypatch, tmp_path):
    declarations = {"Internal": "internal class", "Sealed": "public sealed class",
                    "Abstract": "public abstract class", "Static": "public static class"}
    _classes(monkeypatch, tmp_path, {
        f"{name}.cs": f"namespace X;\n{declaration} {name}Benchmarks {{\n    [Benchmark] public void A() {{}}\n}}\n"
        for name, declaration in declarations.items()})

    findings = check_bench_map.class_shape_findings([f"{name}Benchmarks" for name in declarations])

    assert sum("is not a plain public class" in f for f in findings) == 4, findings


def test_a_category_attribute_or_a_doc_mention_is_not_the_first_benchmark(monkeypatch, tmp_path):
    _classes(monkeypatch, tmp_path, {"Foo.cs": (
        "namespace Lodestar.Bench;\ninternal static class Rows { }\n/// runs each [Benchmark] below\n"
        "[MemoryDiagnoser]\n[BenchmarkCategory(\"text\")]\npublic class FooBenchmarks\n{\n"
        "    [Benchmark(Baseline = true)]\n    public int Run() => 1;\n}\n")})

    assert list(check_bench_map.declared_classes()) == ["FooBenchmarks"]


def test_every_legal_spelling_of_the_attribute_is_seen(monkeypatch, tmp_path):
    _classes(monkeypatch, tmp_path, {
        "A.cs": "namespace X;\npublic class ABenchmarks\n{\n    [Benchmark, Arguments(1)] public void A(int x) {}\n}\n",
        "B.cs": "namespace X;\npublic class BBenchmarks\n{\n    [BenchmarkDotNet.Attributes.Benchmark] public void A() {}\n}\n",
        "C.cs": "namespace X;\npublic class CBenchmarks\n{\n    [BenchmarkAttribute] public void A() {}\n}\n"})

    assert sorted(check_bench_map.declared_classes()) == ["ABenchmarks", "BBenchmarks", "CBenchmarks"]
