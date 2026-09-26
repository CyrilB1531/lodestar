#!/usr/bin/env python3
"""Turn a checkout into the source tree of the documentation site (#1180).

GitHub serves the wiki with `x-robots-tag: none`, so no search engine indexes
it. This writes the same live pages -- docs/wiki-map.json stays the only list of
what is published, read through build_wiki.py's own selection -- as a DocFX
source tree at readable addresses: the flat names were a constraint of the wiki,
which has no directories. Archives stay on the wiki alone, so no page exists
twice here and no canonical tag is needed.

Each page gets `title` and `description` front matter, which DocFX's modern
template writes into <title> and <meta name="description">, the line a search
result shows. Links are rewritten to the page's new address; one to a file the
map does not publish goes to GitHub, and one naming nothing is left for DocFX,
run with --warningsAsErrors, to refuse.

Usage:
    python3 tools/build_site.py --repo <dir> --out <dir>
        [--released Lodestar.Text=0.7.0 ...]

Exit:   0 clean, 1 the map disagrees with the tree, 2 bad usage
"""

from __future__ import annotations

import argparse
import json
import pathlib
import posixpath
import re
import shutil
import sys
from collections.abc import Callable

from build_wiki import MapError, lead_sentence, load_map, page_body, pages_for, short_title, title_of

REPOSITORY = "https://github.com/CyrilB1531/lodestar"
PREFIX = "Lodestar."
DESCRIPTION_LIMIT = 160

# Home's <title>: its H1 is the site's own name, which DocFX's _appTitle already appends.
HOME_TITLE = "Data-science toolkit for C#/.NET at Python parity"

# The map's key for its packages, a TOC node's children, and the site folder of the migration pages.
MAP_PACKAGES = "packages"
ITEMS = "items"
MIGRATION = "migration/"

# Where the build reads its own committed assets from, and where it writes generated ones to.
TOOLS = "tools"
IMAGES = "images"

# The two pages a package ships beside its reference, and the name each takes in packages/<slug>/.
PACKAGE_PAGES = {"README.md": "index.md", "performance.md": "performance.md"}

INLINE_LINK = re.compile(r"!?\[([^\]]*)\]\([^)]*\)")
# `*` and `**` only: `_` is inside identifiers such as unit_variance far more often than emphasis.
EMPHASIS = re.compile(r"(\*\*|\*)(?=\S)(.+?)(?<=\S)\1")


def package_slug(package_id: str) -> str:
    """`Lodestar.Stats.TimeSeries` -> `stats-timeseries`, the slug docs/reference/ already uses."""
    name = package_id[len(PREFIX):] if package_id.startswith(PREFIX) else package_id
    return name.lower().replace(".", "-")


def site_address(relative: str) -> str:
    """Where a published repository page lands on the site, `.md` suffix kept for DocFX."""
    path = pathlib.PurePosixPath(relative)
    if path.parts[0] == "docs" and len(path.parts) > 1 and path.suffix == ".md":
        return path.relative_to("docs").as_posix()
    if path.parts[0] == "src" and len(path.parts) == 3 and path.name in PACKAGE_PAGES:
        return f"packages/{package_slug(path.parts[1])}/{PACKAGE_PAGES[path.name]}"
    raise MapError(f"{relative}: no rule gives it an address on the site")


def addresses(repo: pathlib.Path, mapping: dict) -> dict[str, str]:
    """Every published page, by repository-relative path, with its address, in the map's order."""
    root = repo.resolve()
    found: dict[str, str] = {}
    for patterns in [mapping["root"], *(package["pages"] for package in mapping[MAP_PACKAGES].values())]:
        for page in pages_for(patterns, root):
            relative = page.relative_to(root).as_posix()
            found[relative] = site_address(relative)
    _refuse_collisions(found)
    return found


def _refuse_collisions(found: dict[str, str]) -> None:
    taken: dict[str, str] = {}
    for relative, address in found.items():
        if address in taken:
            raise MapError(f"{taken[address]} and {relative} would both publish {address}")
        taken[address] = relative


def plain_text(markdown: str) -> str:
    """Inline Markdown as the words a search result prints: link text kept, markers dropped."""
    text = INLINE_LINK.sub(r"\1", markdown)
    text = EMPHASIS.sub(r"\2", text).replace("`", "")
    return " ".join(text.split())


def description_for(body: str, title: str) -> tuple[str, bool]:
    """The page's meta description, and whether it had to fall back to the title."""
    lead = plain_text(lead_sentence(body))
    if not lead:
        return title, True
    if len(lead) <= DESCRIPTION_LIMIT:
        return lead, False
    cut = lead[:DESCRIPTION_LIMIT - 1]
    if " " in cut:
        cut = cut[:cut.rfind(" ")]
    return cut.rstrip(" ,;:") + "…", False


def front_matter(title: str, description: str) -> str:
    """A YAML header DocFX reads; a JSON string is a valid YAML double-quoted scalar."""
    return (
        "---\n"
        f"title: {json.dumps(title, ensure_ascii=False)}\n"
        f"description: {json.dumps(description, ensure_ascii=False)}\n"
        "---\n\n"
    )


# Any inline link but an image. Wider than build_wiki.LINK, which rewrites `.md` targets only:
# here a link to an unpublished file has to become a GitHub URL. Link text may wrap lines.
SITE_LINK = re.compile(r"(?<!!)\[(?P<text>[^\]]*)\]\((?P<target>[^)\s#]*)(?P<anchor>#[^)\s]*)?\)")
# S5332 false positive: prefixes a written link is compared against, never a request made.
OFF_SITE = ("http://", "https://", "mailto:")  # NOSONAR S5332
FENCE = re.compile(r"(`{3,}|~{3,})")


def outside_fences(text: str, rewrite: Callable[[str], str]) -> str:
    """`rewrite` applied to each run of lines outside a code fence, fenced lines passed through.

    Runs rather than lines, because a link's text wraps across lines in these pages.
    """
    chunks: list[str] = []
    prose: list[str] = []
    opener: str | None = None
    for line in text.splitlines(keepends=True):
        fence = FENCE.match(line.lstrip())
        if opener is None and fence:
            chunks.append(rewrite("".join(prose)))
            prose = []
            opener = fence.group(1)
            chunks.append(line)
        elif opener is not None:
            chunks.append(line)
            # Closed only by the opener's character, at least as long, alone on its line (CommonMark).
            if fence and fence.group(1)[0] == opener[0] and len(fence.group(1)) >= len(opener) \
                    and line.strip() == fence.group(1):
                opener = None
        else:
            prose.append(line)
    chunks.append(rewrite("".join(prose)))
    return "".join(chunks)


def rewrite_site_links(
    text: str, page: pathlib.Path, address: str, repo: pathlib.Path, index: dict[str, str]
) -> str:
    """Repository-relative links, as links the page can follow from its address on the site."""
    root = repo.resolve()
    base = posixpath.dirname(address) or "."

    def replace(match: re.Match) -> str:
        target, anchor = match.group("target"), match.group("anchor") or ""
        if not target or target.startswith(OFF_SITE):
            return match.group(0)
        resolved = (page.parent / target).resolve()
        try:
            key = resolved.relative_to(root).as_posix()
        except ValueError:
            return match.group(0)
        if key in index:
            link = posixpath.relpath(index[key], base)
        elif resolved.is_dir():
            link = REPOSITORY if key == "." else f"{REPOSITORY}/tree/main/{key}"
        elif resolved.is_file():
            link = f"{REPOSITORY}/blob/main/{key}"
        else:
            return match.group(0)
        return f"[{match.group('text')}]({link}{anchor})"

    return outside_fences(text, lambda prose: SITE_LINK.sub(replace, prose))


TOP_BAR = [
    {"name": "Packages", "href": "packages/"},
    {"name": "Migrating", "href": MIGRATION},
    {"name": "Parity", "href": "equivalence.md"},
    {"name": "Project", "href": "project/"},
]


def toc_name(page: pathlib.Path) -> str:
    """A sidebar label: a title's topic half, unless that half is only a record number.

    `Overview — `Lodestar.Stats`` reads as `Overview`; `0001 — The foundations` would read as `0001`.
    """
    short = short_title(page)
    return plain_text(title_of(page)) if short.isdigit() else short


def toc_yaml(nodes: list[dict]) -> str:
    """A DocFX toc.yml; every scalar is JSON-quoted, which YAML reads as a double-quoted string."""
    lines: list[str] = []

    def emit(level: list[dict], depth: int) -> None:
        pad = "  " * depth
        for node in level:
            lines.append(f"{pad}- name: {json.dumps(node['name'], ensure_ascii=False)}")
            if "href" in node:
                lines.append(f"{pad}  href: {json.dumps(node['href'], ensure_ascii=False)}")
            if node.get(ITEMS):
                lines.append(f"{pad}  items:")
                emit(node[ITEMS], depth + 1)

    emit(nodes, 0)
    return "\n".join(lines) + "\n"


def reference_tree(entries: list[tuple[str, str]], base: str) -> list[dict]:
    """Reference pages nested by path: `X.md` holds the pages of `X/`; the rest sit at the top.

    That places every page exactly once, including those whose directory has no page above it.
    """
    published = {address for address, _ in entries}
    nodes = {address: {"name": name, "href": posixpath.relpath(address, base)} for address, name in entries}
    roots: list[dict] = []
    for address, _ in entries:
        parent = posixpath.dirname(address) + ".md"
        if parent in published:
            nodes[parent].setdefault(ITEMS, []).append(nodes[address])
        else:
            roots.append(nodes[address])
    return roots


def _readme_first(addresses_in: list[str]) -> list[str]:
    return sorted(addresses_in, key=lambda address: (not address.endswith("/README.md"), address))


def _package_node(root: pathlib.Path, name: str, package: dict, index: dict[str, str]) -> dict:
    base = "packages"
    readme, performance = f"src/{name}/README.md", f"src/{name}/performance.md"
    pages = [page.relative_to(root).as_posix() for page in pages_for(package["pages"], root)]
    guides = [p for p in pages if p.startswith("docs/") and not p.startswith("docs/reference/")]
    reference = [(index[p], toc_name(root / p)) for p in pages if p.startswith("docs/reference/")]
    items = [{"name": toc_name(root / p), "href": posixpath.relpath(index[p], base)} for p in guides]
    if reference:
        items.append({"name": "Reference", ITEMS: reference_tree(reference, base)})
    if performance in index:
        items.append({"name": "Performance", "href": posixpath.relpath(index[performance], base)})
    node: dict = {"name": name}
    if readme in index:
        node["href"] = posixpath.relpath(index[readme], base)
    if items:
        node[ITEMS] = items
    return node


def tocs(repo: pathlib.Path, mapping: dict, index: dict[str, str]) -> dict[str, str]:
    """The four TOC files, by output-relative path."""
    root = repo.resolve()
    root_pages = [page.relative_to(root).as_posix() for page in pages_for(mapping["root"], root)]
    root_addresses = [index[page] for page in root_pages]
    migration = _readme_first([a for a in root_addresses if a.startswith(MIGRATION)])
    decisions = _readme_first([a for a in root_addresses if a.startswith("decisions/")])
    project = [a for a in root_addresses
               if a != "equivalence.md" and not a.startswith((MIGRATION, "decisions/"))]
    by_address = {address: root / relative for relative, address in index.items()}

    def leaf(address: str, base: str) -> dict:
        return {"name": toc_name(by_address[address]), "href": posixpath.relpath(address, base)}

    project_base = "project"
    project_nodes = [leaf(a, project_base) for a in project]
    if decisions:
        head = leaf(decisions[0], project_base)
        head[ITEMS] = [leaf(a, project_base) for a in decisions[1:]]
        project_nodes.append(head)
    return {
        "toc.yml": toc_yaml(TOP_BAR),
        "packages/toc.yml": toc_yaml(
            [_package_node(root, name, package, index) for name, package in mapping[MAP_PACKAGES].items()]),
        "migration/toc.yml": toc_yaml([leaf(a, "migration") for a in migration]),
        "project/toc.yml": toc_yaml(project_nodes),
    }


def home_table(mapping: dict, released: dict[str, str]) -> str:
    """The wiki's three columns, linking each package's page on this site."""
    lines = ["## Packages", "", "| Package | Latest released | Documentation |", "| --- | --- | --- |"]
    for name in mapping[MAP_PACKAGES]:
        version = released.get(name, "unreleased")
        lines.append(f"| `{name}` | {version} | [{name}](packages/{package_slug(name)}/index.md) |")
    return "\n".join(lines) + "\n"


def _write_page(
    page: pathlib.Path, address: str, root: pathlib.Path, index: dict[str, str], out: pathlib.Path,
    appendix: str = "", title: str | None = None,
) -> bool:
    body = page_body(page)
    title = title or plain_text(title_of(page))
    description, fell_back = description_for(body, title)
    text = front_matter(title, description) + rewrite_site_links(body, page, address, root, index)
    if appendix:
        text = text.rstrip("\n") + "\n\n" + appendix
    target = out / address
    target.parent.mkdir(parents=True, exist_ok=True)
    target.write_text(text, encoding="utf-8")
    return fell_back


def build(repo: pathlib.Path, out: pathlib.Path, released: dict[str, str]) -> int:
    """Write the site's source tree to `out`, replacing it; return how many titles stood in."""
    root, target = repo.resolve(), out.resolve()
    # `out` is emptied first: not the checkout, nothing holding it, nothing inside it but artifacts/.
    inside = target.is_relative_to(root) and not target.is_relative_to(root / "artifacts")
    if root == target or root.is_relative_to(target) or inside:
        raise MapError(f"{out}: clearing it would delete {repo} or part of it")
    mapping = load_map(root / "docs" / "wiki-map.json")
    index = addresses(root, mapping)
    if target.exists():
        shutil.rmtree(target)
    target.mkdir(parents=True)
    fell_back = sum(_write_page(root / relative, address, root, index, target)
                    for relative, address in index.items())
    home = pages_for([mapping["home"]], root)[0]
    fell_back += _write_page(home, "index.md", root, index, target, home_table(mapping, released), title=HOME_TITLE)
    for path, text in tocs(root, mapping, index).items():
        (target / path).parent.mkdir(parents=True, exist_ok=True)
        (target / path).write_text(text, encoding="utf-8")
    shutil.copyfile(root / TOOLS / "site" / "docfx.json", target / "docfx.json")
    for verification in sorted((root / TOOLS / "site").glob("google*.html")):
        shutil.copyfile(verification, target / verification.name)
    (target / IMAGES).mkdir()
    shutil.copyfile(root / "assets" / "icon.png", target / IMAGES / "icon.png")
    shutil.copyfile(root / TOOLS / "site" / "logo.png", target / IMAGES / "logo.png")
    return fell_back


def _released(values: list[str]) -> dict[str, str]:
    pairs: dict[str, str] = {}
    for value in values:
        package, separator, version = value.partition("=")
        if not separator or not package or not version:
            # Bad usage, so the same exit argparse gives: 2.
            print(f"error: --released {value}: expected PACKAGE=VERSION", file=sys.stderr)
            raise SystemExit(2)
        pairs[package] = version
    return pairs


def main() -> int:
    parser = argparse.ArgumentParser(description="Write the documentation site's DocFX source tree.")
    parser.add_argument("--repo", type=pathlib.Path, required=True)
    parser.add_argument("--out", type=pathlib.Path, required=True)
    parser.add_argument("--released", action="append", default=[], metavar="PACKAGE=VERSION")
    args = parser.parse_args()
    try:
        fell_back = build(args.repo, args.out, _released(args.released))
    except MapError as error:
        print(f"error: {error}", file=sys.stderr)
        return 1
    print(f"{fell_back} page(s) took their title as description")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
