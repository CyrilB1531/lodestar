"""classify_change.py, which answers "ships" and "is about" as two different questions.

#628: roughly 750 board placements and 500 milestone assignments were made by hand, and the
hand pass was wrong three times. Each defect below cost a re-run, and each has a test here
rather than a note.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import classify_change  # noqa: E402

REPO = Path(__file__).resolve().parents[2]


def test_src_is_the_only_thing_that_ships():
    ships, about, _ = classify_change.classify(["src/Lodestar.Onnx/OnnxTextEmbedder.cs"])

    assert ships == {"Lodestar.Onnx"}
    assert about == {"Lodestar.Onnx"}


def test_tests_are_about_a_package_and_ship_nothing():
    # Defect 1: `src/` as the only criterion filed every tests-only change as cross-cutting.
    # Measured on a sample of 40 pull requests, 10 were misfiled that way.
    ships, about, _ = classify_change.classify(
        ["tests/Lodestar.Fuzzy.Tests/FuzzTests.cs",
         "tests/Lodestar.Fuzzy.NetStandard.Tests/NetStandardAssemblyGuardTests.cs"])

    assert ships == set()
    assert about == {"Lodestar.Fuzzy"}


def test_benchmarks_are_about_a_package_and_ship_nothing():
    ships, about, _ = classify_change.classify(
        ["bench/Lodestar.Text.Benchmarks/LevenshteinBenchmarks.cs"])

    assert ships == set()
    assert about == {"Lodestar.Text"}


def test_the_rename_is_the_same_package():
    # Defect 2: the repository was renamed, and a filter matching only `src/Lodestar.*`
    # missed everything before it -- 50 pull requests of real work, filed as cross-cutting.
    ships, about, _ = classify_change.classify(["src/DataNet.Text/Levenshtein.cs"])

    assert ships == {"Lodestar.Text"}
    assert about == {"Lodestar.Text"}


def test_a_bare_prefix_names_no_package():
    # A repository-wide suite from before the split: stripping its suffixes yields
    # `DataNet`, which names no package. An unchecked mapper produced a phantom target.
    ships, about, unattributed = classify_change.classify(
        ["tests/DataNet.NetStandard.Tests/Foo.cs"])

    assert ships == set()
    assert about == set()
    assert unattributed == ["tests/DataNet.NetStandard.Tests/Foo.cs"]


def test_documentation_is_attributed_through_the_wiki_map():
    # The mapping is read from docs/wiki-map.json rather than restated, because a second
    # hand-written table is the drift this repository keeps closing.
    ships, about, _ = classify_change.classify(
        ["docs/reference/fuzzy/matching/fuzz-ratio.md"])

    assert ships == set()
    assert about == {"Lodestar.Fuzzy"}


def test_the_worked_example_from_the_issue():
    # #227 "Reference pages for Lodestar.Fuzzy": tests and reference pages, never src/.
    # It ships nothing, and is about two packages -- so Cross-cutting, and both boards.
    ships, about, _ = classify_change.classify([
        "docs/reference/fuzzy/matching.md",
        "docs/reference/text/distances/indel-normalizedsimilarity.md",
        "tests/Lodestar.Fuzzy.Tests/FuzzTests.cs",
        "docs/guides/migrating-from-rapidfuzz.md",
    ])

    assert ships == set()
    assert about == {"Lodestar.Fuzzy", "Lodestar.Text"}


def test_a_shared_build_file_ships_without_naming_a_package():
    # #638: this file carries the pinned versions every package resolves, shipped included.
    # #635 moved three of them and read as shipping nothing.
    ships, about, unattributed = classify_change.classify(["src/Directory.Packages.props"])

    assert ships == {classify_change.SHARED_MARKER}
    assert about == set()
    assert unattributed == []


def test_shared_sources_ship_too():
    # src/Shared/ is compiled into every library, so a change there reaches every nupkg.
    ships, about, _ = classify_change.classify(["src/Shared/Guard.cs"])

    assert ships == {classify_change.SHARED_MARKER}
    assert about == set()


def test_the_shared_marker_is_not_a_package_name():
    # It answers "does this ship", and must never be mistaken for a board or a label target.
    assert classify_change.SHARED_MARKER not in classify_change.known_packages()


def test_a_shared_file_beside_a_package_change_keeps_both():
    ships, about, _ = classify_change.classify(
        ["src/Directory.Packages.props", "src/Lodestar.Onnx/OnnxTextEmbedder.cs"])

    assert ships == {classify_change.SHARED_MARKER, "Lodestar.Onnx"}
    assert about == {"Lodestar.Onnx"}


def test_the_pull_request_that_found_this():
    # #635's own file list: a pin bump plus test fixes in two packages. It shipped three
    # dependencies and was filed Cross-cutting.
    ships, about, _ = classify_change.classify([
        "src/Directory.Packages.props",
        "tests/Directory.Packages.props",
        "tests/Lodestar.Embeddings.Tests/Tokenization/BpeMetaspaceLoaderTests.cs",
        "tests/Lodestar.Metrics.Tests/RocAucRadixTests.cs",
        ".github/dependabot.yml",
    ])

    assert ships, "a change to three shipped dependencies must report that it ships"
    assert about == {"Lodestar.Embeddings", "Lodestar.Metrics"}


def test_a_path_it_cannot_attribute_is_reported_rather_than_swallowed():
    # Defect 3: writing "unknown" as Cross-cutting reads exactly like a decision. Of 158
    # cross-cutting issues, 49 had no evidence at all behind that label.
    _, about, unattributed = classify_change.classify(
        [".github/workflows/ci.yml", "CHANGELOG.md", "tools/check_bench_map.py"])

    assert about == set()
    assert len(unattributed) == 3


def test_a_shared_guide_is_not_attributed_to_one_package():
    # docs/guides/performance.md carries every package's numbers; wiki-map.json does not
    # claim it for one, and neither does this.
    _, about, unattributed = classify_change.classify(["docs/guides/performance.md"])

    assert about == set()
    assert unattributed == ["docs/guides/performance.md"]


def test_the_nightly_pages_belong_to_no_package():
    # The 35 generated `Nightly benchmark run` pull requests touch only these two.
    _, about, _ = classify_change.classify(
        ["docs/guides/benchmark_latest.md", "docs/guides/nightly_run.md"])

    assert about == set()


def test_every_package_the_map_names_is_a_real_package():
    """docs/wiki-map.json's attributions must all resolve, or the mapping has rotted."""
    packages = classify_change.known_packages()
    mapped = set(classify_change.documentation_map().values())

    assert mapped, "docs/wiki-map.json attributed no documentation page at all"
    assert mapped <= packages, f"wiki-map.json names packages that do not exist: {mapped - packages}"


def test_known_packages_matches_the_source_tree():
    packages = classify_change.known_packages()

    assert packages == {p.name for p in (REPO / "src").glob("Lodestar.*") if p.is_dir()}
