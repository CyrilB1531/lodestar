"""check_readme_pack_loop.py's guard on the command the README calls runnable.

#597: the loop packed nine of the fifteen packages samples/Lodestar.Sample references,
and five of the six missing had been missing for whole lots. samples/NuGet.config maps
Lodestar.* to ./artifacts, so the documented command could not restore -- and nothing
read the loop, which is why it rotted where the six lists something reads did not.
"""

from __future__ import annotations

import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import check_readme_pack_loop  # noqa: E402

ROOT = Path(__file__).resolve().parents[2]

LOOP = (
    "for p in src/Lodestar.Text src/Lodestar.Fuzzy; do\n"
    '  dotnet pack "$p" -c Release -o ./artifacts\n'
    "done\n"
)


def _readme(monkeypatch, tmp_path, body):
    path = tmp_path / "README.md"
    path.write_text(body, encoding="utf-8")
    monkeypatch.setattr(check_readme_pack_loop, "README", path)
    return path


def _sample(monkeypatch, tmp_path, packages):
    path = tmp_path / "Lodestar.Sample.csproj"
    refs = "\n".join(f'    <PackageReference Include="{p}" Version="1.0.0" />' for p in packages)
    path.write_text(f"<Project>\n  <ItemGroup>\n{refs}\n  </ItemGroup>\n</Project>", encoding="utf-8")
    monkeypatch.setattr(check_readme_pack_loop, "SAMPLE", path)
    return path


def test_the_shipped_readme_packs_what_the_shipped_sample_references():
    assert check_readme_pack_loop.findings() == []


def test_a_package_the_sample_needs_and_the_loop_omits_is_a_finding(monkeypatch, tmp_path):
    _readme(monkeypatch, tmp_path, LOOP)
    _sample(monkeypatch, tmp_path, ["Lodestar.Text", "Lodestar.Fuzzy", "Lodestar.Survival"])

    found = check_readme_pack_loop.findings()

    assert len(found) == 1
    assert "Lodestar.Survival" in found[0]


def test_a_package_the_loop_packs_and_the_sample_ignores_is_a_finding(monkeypatch, tmp_path):
    # The other direction: instructions that pack something the sample never consumes.
    _readme(monkeypatch, tmp_path, LOOP)
    _sample(monkeypatch, tmp_path, ["Lodestar.Text"])

    found = check_readme_pack_loop.findings()

    assert len(found) == 1
    assert "Lodestar.Fuzzy" in found[0]


def test_the_wrapped_form_the_readme_actually_uses_is_read(monkeypatch, tmp_path):
    # The shipped loop is wrapped across five lines with backslash continuations; a
    # regex that only matched one line would pass on a truncated list.
    _readme(
        monkeypatch,
        tmp_path,
        "for p in src/Lodestar.Text \\\n        src/Lodestar.Fuzzy; do\n"
        '  dotnet pack "$p" -c Release -o ./artifacts\ndone\n',
    )
    _sample(monkeypatch, tmp_path, ["Lodestar.Text", "Lodestar.Fuzzy"])

    assert check_readme_pack_loop.findings() == []


def test_no_loop_at_all_is_a_finding(monkeypatch, tmp_path):
    # Not silence: a README this file cannot read is one it cannot vouch for.
    _readme(monkeypatch, tmp_path, "dotnet pack src/Lodestar.Text -c Release\n")
    _sample(monkeypatch, tmp_path, ["Lodestar.Text"])

    found = check_readme_pack_loop.findings()

    assert len(found) == 1
    assert "no `for p in src/Lodestar...; do` loop" in found[0]
