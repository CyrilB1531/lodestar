"""The site builder's own tests: a fake repository in, a DocFX source tree out (#1180).

Every assertion is about something a reader or a search engine would notice: a
page at the wrong address, a link that 404s, a description Google would cut or
print with backticks in it, a navigation entry missing or shown twice. The
fixtures are built in tmp_path; the last tests build the repository's own docs/
and fail when a page it links is renamed and the link is not.
"""
from __future__ import annotations

import json
import re
import subprocess
import sys
from pathlib import Path

import pytest

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

import build_site  # noqa: E402
import build_wiki  # noqa: E402

REPO = Path(__file__).resolve().parents[2]
SCRIPT = REPO / "tools" / "build_site.py"

MAP = {
    "home": "docs/wiki/home.md",
    "root": [
        "docs/equivalence.md",
        "docs/guides/performance.md",
        "docs/decisions/*.md",
        "docs/migration/*.md",
    ],
    "packages": {
        "Lodestar.Stats.TimeSeries": {
            "wiki": "StatsTimeSeries",
            "pages": [
                "docs/guides/time-series.md",
                "docs/reference/stats-timeseries/*.md",
                "docs/reference/stats-timeseries/*/*.md",
                "src/Lodestar.Stats.TimeSeries/README.md",
                "src/Lodestar.Stats.TimeSeries/performance.md",
            ],
            "covered": {},
        },
        "Lodestar.Extensions.AI": {
            "wiki": "ExtensionsAI",
            "pages": ["src/Lodestar.Extensions.AI/README.md"],
            "covered": {},
        },
    },
}

GUIDE = (
    "# Time series: a guide\n\n"
    "Read [Stationarity](../reference/stats-timeseries/stationarity.md#adf), then\n"
    "[the contributing\nguide](../../CONTRIBUTING.md), [the source](../../src/Lodestar.Stats.TimeSeries/),\n"
    "[a missing page](../nowhere.md) and [Python](https://www.statsmodels.org/).\n\n"
    "```markdown\n[not a link](../equivalence.md)\n```\n"
)

FILES = {
    "docs/wiki/home.md": "# Lodestar\n\nStart with [the guide](../guides/time-series.md).\n",
    "docs/equivalence.md": "# Parity with Python\n\n| a | b |\n| --- | --- |\n",
    "docs/guides/performance.md": "# Performance\n\nThe index.\n",
    "docs/decisions/README.md": "# Decisions\n\nThe records.\n",
    "docs/decisions/0001-foundations.md": "---\nstatus: accepted\n---\n# 0001 Foundations\n\nWhy.\n",
    "docs/migration/README.md": "# Migrating\n\nWhere each need goes.\n",
    "docs/migration/statsmodels.md": "# From statsmodels\n\nThe rows.\n",
    "docs/guides/time-series.md": GUIDE,
    "docs/reference/stats-timeseries/stationarity.md":
        "# Stationarity — `Lodestar.Stats.TimeSeries`\n\nTests for a unit root.\n",
    "docs/reference/stats-timeseries/stationarity/adf.md":
        "# AugmentedDickeyFuller\n\nThe **augmented** `Dickey-Fuller` test.\n",
    "docs/reference/stats-timeseries/orphan/lonely.md": "# Lonely\n\n- a list first\n",
    "src/Lodestar.Stats.TimeSeries/README.md": "# Lodestar.Stats.TimeSeries\n\nDiagnostics.\n",
    "src/Lodestar.Stats.TimeSeries/performance.md": "# Performance\n\nMeasured.\n",
    "src/Lodestar.Extensions.AI/README.md": "# Lodestar.Extensions.AI\n\nInterop.\n",
    "CONTRIBUTING.md": "# Contributing\n",
    "tools/site/docfx.json": "{}\n",
    "tools/site/logo.png": "not really a png either\n",
    "assets/icon.png": "not really a png\n",
}


def make_repo(tmp_path: Path) -> Path:
    repo = tmp_path / "repo"
    for relative, text in FILES.items():
        path = repo / relative
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(text, encoding="utf-8")
    (repo / "docs" / "wiki-map.json").write_text(json.dumps(MAP), encoding="utf-8")
    return repo.resolve()


@pytest.mark.parametrize(
    ("package_id", "slug"),
    [
        ("Lodestar.Text", "text"),
        ("Lodestar.Stats.TimeSeries", "stats-timeseries"),
        ("Lodestar.Extensions.AI", "extensions-ai"),
    ],
)
def test_the_slug_is_the_one_docs_reference_uses(package_id: str, slug: str) -> None:
    assert build_site.package_slug(package_id) == slug


@pytest.mark.parametrize(
    ("relative", "address"),
    [
        ("docs/guides/survival-analysis.md", "guides/survival-analysis.md"),
        ("docs/reference/text/distances.md", "reference/text/distances.md"),
        ("docs/equivalence.md", "equivalence.md"),
        ("docs/decisions/README.md", "decisions/README.md"),
        ("src/Lodestar.Text/README.md", "packages/text/index.md"),
        ("src/Lodestar.Stats.TimeSeries/performance.md", "packages/stats-timeseries/performance.md"),
    ],
)
def test_each_page_has_the_address_the_spec_gives_it(relative: str, address: str) -> None:
    assert build_site.site_address(relative) == address


@pytest.mark.parametrize("relative", ["src/Lodestar.Text/CHANGELOG.md", "README.md", "docs"])
def test_a_page_no_rule_covers_is_refused(relative: str) -> None:
    with pytest.raises(build_wiki.MapError):
        build_site.site_address(relative)


def test_addresses_covers_every_published_page_in_map_order(tmp_path: Path) -> None:
    index = build_site.addresses(make_repo(tmp_path), MAP)
    assert list(index)[:2] == ["docs/equivalence.md", "docs/guides/performance.md"]
    assert index["src/Lodestar.Extensions.AI/README.md"] == "packages/extensions-ai/index.md"
    assert "docs/wiki/home.md" not in index
    assert len(index) == len(FILES) - 5  # home, CONTRIBUTING, docfx.json, the icon and the logo are not pages


def test_two_pages_at_one_address_are_refused(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    clash = {**MAP, "root": [*MAP["root"], "docs/guides/performance.md"]}
    # The same page twice is one page; a second source for one address is the error.
    assert build_site.addresses(repo, clash)["docs/guides/performance.md"] == "guides/performance.md"
    with pytest.raises(build_wiki.MapError, match="both publish"):
        build_site._refuse_collisions({"a/x.md": "x.md", "b/x.md": "x.md"})


@pytest.mark.parametrize(
    ("markdown", "plain"),
    [
        ("Read [the guide](x.md), then `code` and **bold**.", "Read the guide, then code and bold."),
        ("`unit_variance` divides by the *quantile*.", "unit_variance divides by the quantile."),
        ("Spread\nover   lines.", "Spread over lines."),
        ("2 * 3 stays a product.", "2 * 3 stays a product."),
    ],
)
def test_plain_text_keeps_the_words_and_drops_the_markup(markdown: str, plain: str) -> None:
    assert build_site.plain_text(markdown) == plain


def test_a_long_lead_is_cut_at_a_word_within_the_limit() -> None:
    body = "# T\n\n" + "measured " * 40 + "end.\n"
    description, fell_back = build_site.description_for(body, "T")
    assert not fell_back
    assert len(description) <= build_site.DESCRIPTION_LIMIT
    assert description.endswith("measured…")


def test_a_page_opening_on_a_list_takes_its_title() -> None:
    assert build_site.description_for("# Lonely\n\n- a list first\n", "Lonely") == ("Lonely", True)


def test_front_matter_quotes_what_yaml_would_misread() -> None:
    block = build_site.front_matter('Say "hi": now', "A — b")
    lines = block.splitlines()
    assert lines[0] == lines[3] == "---"
    assert json.loads(lines[1].removeprefix("title: ")) == 'Say "hi": now'
    assert lines[2] == 'description: "A — b"'
    assert block.endswith("---\n\n")


def rewritten_guide(tmp_path: Path) -> str:
    repo = make_repo(tmp_path)
    index = build_site.addresses(repo, MAP)
    page = repo / "docs" / "guides" / "time-series.md"
    return build_site.rewrite_site_links(GUIDE, page, index["docs/guides/time-series.md"], repo, index)


def test_a_published_page_is_linked_at_its_new_address_with_its_anchor(tmp_path: Path) -> None:
    assert "[Stationarity](../reference/stats-timeseries/stationarity.md#adf)" in rewritten_guide(tmp_path)


def test_an_unpublished_file_is_linked_on_github_even_across_a_wrapped_line(tmp_path: Path) -> None:
    text = rewritten_guide(tmp_path)
    assert "[the contributing\nguide](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md)" in text


def test_a_directory_is_linked_as_a_tree(tmp_path: Path) -> None:
    assert (
        "[the source](https://github.com/CyrilB1531/lodestar/tree/main/src/Lodestar.Stats.TimeSeries)"
        in rewritten_guide(tmp_path)
    )


def test_a_link_to_nothing_and_an_absolute_url_are_left_as_written(tmp_path: Path) -> None:
    text = rewritten_guide(tmp_path)
    assert "[a missing page](../nowhere.md)" in text
    assert "[Python](https://www.statsmodels.org/)" in text


def test_a_link_inside_a_fence_is_code_not_a_link(tmp_path: Path) -> None:
    assert "```markdown\n[not a link](../equivalence.md)\n```" in rewritten_guide(tmp_path)


def test_an_anchor_alone_and_a_link_leaving_the_repository_are_untouched(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    index = build_site.addresses(repo, MAP)
    page = repo / "docs" / "equivalence.md"
    text = "[Up](#top) and [out](../../../elsewhere.md)"
    assert build_site.rewrite_site_links(text, page, "equivalence.md", repo, index) == text


def test_home_links_from_the_site_root(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    index = build_site.addresses(repo, MAP)
    page = repo / "docs" / "wiki" / "home.md"
    text = build_site.rewrite_site_links(FILES["docs/wiki/home.md"], page, "index.md", repo, index)
    assert "[the guide](guides/time-series.md)" in text


@pytest.mark.parametrize(
    ("text", "expected"),
    [
        # A tilde line inside a backtick block is code, and the prose after the block is prose.
        ("prose [a](x) before\n```\ncode\n~~~\nstill code [b](x)\n```\nafter [c](x)\n",
         "PROSE [A](X) BEFORE\n```\ncode\n~~~\nstill code [b](x)\n```\nAFTER [C](X)\n"),
        # Four backticks are closed by four, not by the three a nested example shows.
        ("````markdown\n```\n[x](y)\n```\n````\nafter\n",
         "````markdown\n```\n[x](y)\n```\n````\nAFTER\n"),
        # A line carrying an info string opens a fence but never closes one.
        ("```\n```python\n[x](y)\n```\ntail\n",
         "```\n```python\n[x](y)\n```\nTAIL\n"),
    ],
)
def test_a_fence_closes_only_on_its_own_delimiter(text: str, expected: str) -> None:
    assert build_site.outside_fences(text, str.upper) == expected


HREF = re.compile(r'href: "(?P<href>[^"]+)"')


def test_toc_yaml_nests_items_under_their_parent() -> None:
    nodes = [{"name": "A", "href": "a.md", "items": [{"name": 'B "b"', "href": "b.md"}]}]
    assert build_site.toc_yaml(nodes) == (
        '- name: "A"\n'
        '  href: "a.md"\n'
        "  items:\n"
        '  - name: "B \\"b\\""\n'
        '    href: "b.md"\n'
    )


def test_a_reference_page_holds_the_pages_of_its_own_directory() -> None:
    entries = [
        ("reference/x.md", "X"),
        ("reference/x/a.md", "A"),
        ("reference/y/b.md", "B"),
    ]
    assert build_site.reference_tree(entries, "packages") == [
        {"name": "X", "href": "../reference/x.md", "items": [{"name": "A", "href": "../reference/x/a.md"}]},
        {"name": "B", "href": "../reference/y/b.md"},
    ]


def test_the_top_bar_is_the_one_the_spec_names(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    top = build_site.tocs(repo, MAP, build_site.addresses(repo, MAP))["toc.yml"]
    assert HREF.findall(top) == ["packages/", "migration/", "equivalence.md", "project/"]
    assert re.findall(r'name: "([^"]+)"', top) == ["Packages", "Migrating", "Parity", "Project"]


def test_a_package_opens_on_its_readme_then_guides_reference_performance(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    packages = build_site.tocs(repo, MAP, build_site.addresses(repo, MAP))["packages/toc.yml"]
    assert HREF.findall(packages) == [
        "stats-timeseries/index.md",
        "../guides/time-series.md",
        "../reference/stats-timeseries/stationarity.md",
        "../reference/stats-timeseries/stationarity/adf.md",
        "../reference/stats-timeseries/orphan/lonely.md",
        "stats-timeseries/performance.md",
        "extensions-ai/index.md",
    ]
    assert re.search(r'^\s*- name: "Reference"$', packages, re.MULTILINE)


def test_every_published_page_is_in_exactly_one_toc_entry(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    index = build_site.addresses(repo, MAP)
    listed: list[str] = []
    for path, text in build_site.tocs(repo, MAP, index).items():
        folder = Path(path).parent.as_posix()
        # A folder href opens that folder's own toc.yml, which is counted on its own.
        listed += [_normalise(f"{folder}/{href}") for href in HREF.findall(text) if not href.endswith("/")]
    assert sorted(listed) == sorted(index.values())


def _normalise(entry: str) -> str:
    """A toc-relative href as an output-relative address: `packages/../guides/x.md` is `guides/x.md`."""
    parts: list[str] = []
    for part in entry.split("/"):
        if part == "..":
            parts.pop()
        elif part not in ("", "."):
            parts.append(part)
    return "/".join(parts)


def test_a_decision_record_keeps_its_title_in_the_sidebar(tmp_path: Path) -> None:
    record = tmp_path / "0001-foundations.md"
    record.write_text("# 0001 — The foundations: `net10.0`\n\nWhy.\n", encoding="utf-8")
    overview = tmp_path / "overview.md"
    overview.write_text("# Overview — `Lodestar.Stats`\n\nWhat.\n", encoding="utf-8")
    assert build_site.toc_name(record) == "0001 — The foundations: net10.0"
    assert build_site.toc_name(overview) == "Overview"


SWEPT_LINK = re.compile(r"(?<!!)\[[^\]]*\]\((?P<target>[^)\s]+)\)")


def dangling(out: Path) -> list[str]:
    """Every relative link or TOC href in a built tree naming a file the tree does not hold."""
    found: list[str] = []
    for page in sorted(out.rglob("*.md")):
        def collect(prose: str, page: Path = page) -> str:
            for match in SWEPT_LINK.finditer(prose):
                target = match.group("target").split("#", 1)[0]
                if target and not target.startswith(build_site.OFF_SITE) and not (page.parent / target).exists():
                    found.append(f"{page.relative_to(out).as_posix()}: {target}")
            return prose
        build_site.outside_fences(page.read_text(encoding="utf-8"), collect)
    for toc in sorted(out.rglob("toc.yml")):
        for href in HREF.findall(toc.read_text(encoding="utf-8")):
            target = toc.parent / (href + "toc.yml" if href.endswith("/") else href)
            if not target.exists():
                found.append(f"{toc.relative_to(out).as_posix()}: {href}")
    return found


def test_build_writes_every_page_home_the_tocs_and_the_config(tmp_path: Path) -> None:
    repo, out = make_repo(tmp_path), tmp_path / "site"
    fell_back = build_site.build(repo, out, {"Lodestar.Stats.TimeSeries": "0.1.0"})
    for expected in [
        "index.md", "guides/time-series.md", "reference/stats-timeseries/stationarity.md",
        "packages/stats-timeseries/index.md", "packages/stats-timeseries/performance.md",
        "packages/extensions-ai/index.md", "decisions/README.md", "migration/statsmodels.md",
        "toc.yml", "packages/toc.yml", "migration/toc.yml", "project/toc.yml",
        "docfx.json", "images/icon.png", "images/logo.png",
    ]:
        assert (out / expected).is_file(), expected
    assert fell_back == 2  # equivalence.md opens on a table, lonely.md on a list
    assert dangling(out) == ["guides/time-series.md: ../nowhere.md"]


def test_home_carries_the_package_table(tmp_path: Path) -> None:
    out = tmp_path / "site"
    build_site.build(make_repo(tmp_path), out, {"Lodestar.Stats.TimeSeries": "0.1.0"})
    home = (out / "index.md").read_text(encoding="utf-8")
    assert home.startswith(
        '---\ntitle: "Data-science toolkit for C#/.NET at Python parity"\ndescription: "Start with the guide."\n---\n\n# Lodestar')
    assert ("| `Lodestar.Stats.TimeSeries` | 0.1.0 | "
            "[Lodestar.Stats.TimeSeries](packages/stats-timeseries/index.md) |") in home
    assert "| `Lodestar.Extensions.AI` | unreleased |" in home


def test_a_decision_loses_its_own_front_matter_for_the_sites(tmp_path: Path) -> None:
    out = tmp_path / "site"
    build_site.build(make_repo(tmp_path), out, {})
    record = (out / "decisions" / "0001-foundations.md").read_text(encoding="utf-8")
    assert record == '---\ntitle: "0001 Foundations"\ndescription: "Why."\n---\n\n# 0001 Foundations\n\nWhy.\n'


def test_a_title_in_backticks_is_plain_in_the_front_matter(tmp_path: Path) -> None:
    out = tmp_path / "site"
    build_site.build(make_repo(tmp_path), out, {})
    page = (out / "reference" / "stats-timeseries" / "stationarity.md").read_text(encoding="utf-8")
    assert page.startswith('---\ntitle: "Stationarity — Lodestar.Stats.TimeSeries"\n')


def test_the_output_is_rebuilt_from_scratch(tmp_path: Path) -> None:
    repo, out = make_repo(tmp_path), tmp_path / "site"
    (out / "stale").mkdir(parents=True)
    build_site.build(repo, out, {})
    assert not (out / "stale").exists()


@pytest.mark.parametrize("where", ["repo", "parent", "docs", "tools/site"])
def test_it_refuses_to_clear_the_repository(tmp_path: Path, where: str) -> None:
    repo = make_repo(tmp_path)
    out = {"repo": repo, "parent": repo.parent}.get(where, repo / where)
    with pytest.raises(build_wiki.MapError, match="would delete"):
        build_site.build(repo, out, {})
    assert (repo / "docs" / "equivalence.md").is_file()


def test_it_writes_under_the_checkouts_artifacts(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    build_site.build(repo, repo / "artifacts" / "site", {})
    assert (repo / "artifacts" / "site" / "index.md").is_file()


def test_a_declared_page_missing_from_the_tree_exits_1(tmp_path: Path) -> None:
    repo = make_repo(tmp_path)
    (repo / "src" / "Lodestar.Extensions.AI" / "README.md").unlink()
    result = subprocess.run(
        [sys.executable, str(SCRIPT), "--repo", str(repo), "--out", str(tmp_path / "site")],
        capture_output=True, text=True, check=False,
    )
    assert result.returncode == 1
    assert "missing from the tree" in result.stderr


def test_the_command_line_reports_the_fallbacks(tmp_path: Path) -> None:
    result = subprocess.run(
        [sys.executable, str(SCRIPT), "--repo", str(make_repo(tmp_path)), "--out", str(tmp_path / "site"),
         "--released", "Lodestar.Stats.TimeSeries=0.1.0"],
        capture_output=True, text=True, check=False,
    )
    assert (result.returncode, result.stdout) == (0, "2 page(s) took their title as description\n")


def test_the_site_publishes_exactly_what_the_wiki_publishes_live() -> None:
    mapping = build_wiki.load_map(REPO / "docs" / "wiki-map.json")
    assert set(build_site.addresses(REPO, mapping)) == set(build_wiki.link_index(REPO.resolve(), mapping))


def test_every_link_on_the_real_site_lands(tmp_path: Path) -> None:
    out = tmp_path / "site"
    build_site.build(REPO, out, {})
    mapping = build_wiki.load_map(REPO / "docs" / "wiki-map.json")
    assert len(list(out.rglob("*.md"))) == len(build_site.addresses(REPO, mapping)) + 1
    assert dangling(out) == []
