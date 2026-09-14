"""nightly_series.py: the page parses, the series merges, and the comparison catches what it must.

The last test is the acceptance criterion #672 set: replayed night by night over the series
rebuilt from the page's own history, the comparison has to raise the three regressions already
found by hand — each as a new movement, on the run where it appeared.
"""

from __future__ import annotations

import collections
import pathlib
import sys

sys.path.insert(0, str(pathlib.Path(__file__).resolve().parents[1]))

import nightly_series as ns  # noqa: E402

PAGE = """# Nightly benchmark run

## This run

- Commit: `abc1234def5678abc1234def5678abc1234def56`

## Classes re-run

### Lodestar.Text.Benchmarks.IndelBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
```

| Method                     | Length | Mean      | Error    | StdDev   | Ratio | RatioSD | Allocated |
|--------------------------- |------- |----------:|---------:|---------:|------:|--------:|----------:|
| **Distance_Utf16**             | **8**      |  **28.73 ns** | **0.926 ns** | **0.051 ns** |  **1.00** |    **0.00** |         **-** |
| Distance_CodePoint         | 8      | 133.23 ns | 0.867 ns | 0.048 ns |  4.64 |    0.01 |         - |
|                            |        |           |          |          |       |         |           |
| **Distance_Utf16**             | **512**    |  **4,980.71 ns** | **28.02 ns** | **1.54 ns** |  **1.00** |    **0.00** |         **-** |
| Distance_CodePoint         | 512    | 306,139.36 ns | 30,373.94 ns | 1,664.90 ns | 61.47 |    0.29 |         - |

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
```

| Method | Length | Mean | Error | StdDev | Allocated |
|------- |------- |-----:|------:|-------:|----------:|
| Encode | 64     | 1 ms | 0 ms  | 0 ms   | 1 KB      |

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
```

| Method  | Mean      | Error     | StdDev   | Ratio | Allocated |
|-------- |----------:|----------:|---------:|------:|----------:|
| Unigram |  32.11 ms |  4.083 ms | 0.224 ms |  1.00 |   5.43 MB |
| Bpe     | 562.77 ms | 33.381 ms | 1.830 ms |     ? | 112.18 MB |

## Against rapidfuzz, in this same run

| length | rapidfuzz | Lodestar |
| --- | --- | --- |
| 8 | 1 | 2 |
"""


def reading(ratio, date, commit, key=("C", "", "M"), cpu="X"):
    return ns.Reading(date, commit, cpu, key[0], key[1], key[2], ratio)


def test_the_page_yields_every_ratio_with_its_parameters_and_cpu():
    readings = ns.parse_page(PAGE, "2026-09-13")

    by_key = {(r.cls, r.parameters, r.method): r for r in readings}
    assert by_key[("IndelBenchmarks", "Length=512", "Distance_CodePoint")].ratio == 61.47
    assert by_key[("IndelBenchmarks", "Length=8", "Distance_Utf16")].ratio == 1.0
    assert by_key[("IndelBenchmarks", "Length=8", "Distance_CodePoint")].cpu == "AMD EPYC 7763"
    assert all(r.commit.startswith("abc1234") for r in readings)


def test_a_table_without_a_ratio_and_a_cell_that_is_not_a_number_are_skipped():
    classes = {r.cls for r in ns.parse_page(PAGE, "2026-09-13")}
    methods = {(r.cls, r.method) for r in ns.parse_page(PAGE, "2026-09-13")}

    assert "BpeScalingBenchmarks" not in classes
    assert ("BpeBenchmarks", "Bpe") not in methods
    assert ("BpeBenchmarks", "Unigram") in methods


def test_a_table_outside_a_report_section_is_not_read():
    assert not [r for r in ns.parse_page(PAGE, "x") if r.method in {"rapidfuzz", "Lodestar"}]


def test_the_series_round_trips(tmp_path):
    readings = ns.parse_page(PAGE, "2026-09-13")
    path = tmp_path / "ratios.csv"

    ns.write_series(path, readings)

    assert ns.read_series(path) == readings


def test_a_rerun_of_the_same_commit_replaces_its_readings_rather_than_doubling_them():
    first = [reading(1.0, "2026-09-13", "aaa"), reading(2.0, "2026-09-13", "bbb")]
    rerun = [reading(3.0, "2026-09-13", "bbb")]

    merged = ns.merge(first, rerun)

    assert [(r.commit, r.ratio) for r in merged] == [("aaa", 1.0), ("bbb", 3.0)]


def test_a_key_is_not_compared_before_it_has_a_history():
    history = [reading(1.0, "2026-09-01", "a"), reading(1.0, "2026-09-02", "b")]

    assert ns.compare(history, [reading(9.0, "2026-09-03", "c")]) == []


def test_a_step_past_the_floor_is_reported_and_a_small_one_is_not():
    history = [reading(1.0, f"2026-09-0{day}", f"c{day}") for day in range(1, 6)]

    assert ns.compare(history, [reading(1.2, "2026-09-06", "x")]) == []
    moved = ns.compare(history, [reading(1.5, "2026-09-06", "y")])
    assert [(m.kind, round(m.change, 2), m.persisting) for m in moved] == [("step", 0.5, False)]


def test_a_noisy_key_needs_a_larger_step():
    # Readings that alternate by 20% around 1.0 give a noise near 0.2, so the threshold is
    # four times that rather than the 30% floor, and a 50% step no longer counts.
    ratios = [1.0, 1.2, 1.0, 1.2, 1.0, 1.2, 1.0, 1.2]
    history = [reading(r, f"2026-09-{day + 1:02d}", f"c{day}") for day, r in enumerate(ratios)]

    assert ns.compare(history, [reading(1.6, "2026-09-10", "x")]) == []


def test_a_slow_drift_no_single_night_shows_is_reported():
    ratios = [1.0, 1.0, 1.0, 1.05, 1.1, 1.15, 1.2, 1.25]
    dates = ["2026-08-01", "2026-08-02", "2026-08-03", "2026-08-06", "2026-08-08",
             "2026-08-10", "2026-08-12", "2026-08-13"]
    history = [reading(r, d, f"c{i}") for i, (r, d) in enumerate(zip(ratios, dates, strict=True))]

    moved = ns.compare(history, [reading(1.3, "2026-08-14", "x")])

    assert [m.kind for m in moved] == ["drift"]


def test_a_movement_already_reported_on_the_previous_reading_is_marked_persisting():
    history = [reading(1.0, f"2026-09-0{day}", f"c{day}") for day in range(1, 6)]
    history.append(reading(2.0, "2026-09-06", "step"))

    moved = ns.compare(history, [reading(2.0, "2026-09-07", "again")])

    assert [(m.kind, m.persisting) for m in moved] == [("step", True)]


def test_the_section_says_so_when_nothing_moved():
    assert "No ratio stepped" in ns.render([])


def test_the_replayed_history_raises_the_three_regressions_found_by_hand():
    series = ns.read_series(ns.SERIES)
    runs = collections.OrderedDict()
    for r in series:
        runs.setdefault(r.commit, []).append(r)

    expected = {
        # The night of 2026-09-01 reaches the series through the wiki alone: its pull request was
        # never merged. It is the first run after 8de0da96, and the step is there already.
        ("BpeBenchmarks", "", "Bpe"): ("673", "2026-09-01", "step"),
        ("IndelBenchmarks", "Length=512", "Distance_CodePoint"): ("674", "2026-08-31", "drift"),
        ("IndelBenchmarks", "Length=20", "Distance_CodePoint"): ("675", "2026-08-26", "step"),
    }
    first_new = {}
    history: list[ns.Reading] = []
    for tonight in runs.values():
        for movement in ns.compare(history, tonight):
            key = movement.reading.key
            if key in expected and not movement.persisting and key not in first_new:
                first_new[key] = (movement.reading.date, movement.kind)
        history.extend(tonight)

    for key, (issue, date, kind) in expected.items():
        assert first_new.get(key) == (date, kind), f"#{issue}: first new movement {first_new.get(key)}"
