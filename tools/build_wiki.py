#!/usr/bin/env python3
"""Turn a checkout into the wiki tree, per package and per version.

The pages live in docs/ because that is where markdownlint, the snippet
compiler and pull-request review reach them. This turns them into what a
reader sees: a flat file per page, named for its channel and version -- flat
because a GitHub wiki addresses a page by file name alone, with no directory
context, and two files sharing a base name silently collide (one shadows the
other with no warning).

docs/wiki-map.json is the only place that says which page belongs to which
package. A page listed there and absent from the tree is an error, not a
silence: that is how a renamed guide stops being published without anyone
noticing.

Links are rewritten from repository-relative paths to flat wiki names. A wiki
page has no .md suffix and no directory, so `../reference/text/distances.md`
read in a guide has to become `Text-distances` in the wiki or it 404s.

Each live channel also gets a generated entry page, `<channel>.md` -- `Text.md`
and so on -- linking to every namespace the package covers and every guide it
ships, so Home can send a reader one level down without naming a page that
might not exist. It carries no content of its own to freeze, so an archived
version does not get one: `<channel>-<version>-<stem>` already names every
frozen page a reader can reach from the banner, and the entry page's own name
has no stem to sit in that scheme -- inventing one would mean teaching
_archive_pattern, _live_stems and _archive_stems (which infer "archived" from
a trailing `-<stem>` after the version) to special-case a page that has none,
for a hub whose only content is links the frozen pages already carry to each
other via the banner. A reader who follows a banner back to an archived page
lands on that page's own frozen counterpart, never on an index, and that is
enough.

Home is the map's hand-written `home` page with the package table appended, and
every package page opens on a breadcrumb back to its namespace, its hub and
Home: the wiki has no directories, so that line is where a page sits (#1107).

Usage:
    python3 tools/build_wiki.py --repo <dir> --out <dir>
        --released Lodestar.Text=0.3.0 [--released ...]
        [--archive Lodestar.Text=0.4.0]

Without --archive it refreshes every live channel and the root pages, each
named `<channel>-<stem>`. With it, it first freezes that one package under
`<channel>-<version>-<stem>` and then refreshes the live channels on top of
that copy: the sidebar's version list and the live pages' banner are both read
off the tree, so archiving alone would leave them naming the previous release.

Exit:   0 clean, 1 the map disagrees with the tree, 2 bad usage
"""

from __future__ import annotations

import argparse
import json
import os
import pathlib
import re
import sys

BANNER = (
    "> **Development build.** This page describes `main`, not a released package.\n"
    "> The latest published {package} is **{version}** — read [its documentation]"
    "({target}).\n\n"
)

# The other half of BANNER, and the only thing on a frozen page that says which
# version it is: the name carries it, and the name is in the URL, not in the text.
ARCHIVE_BANNER = (
    "> **{package} {version}.** This page is frozen at that release.{exit}\n"
    "> A link to a decision or a migration page follows `main`, and leaves the archive.\n\n"
)

ARCHIVE_EXIT = " Read [the current documentation]({channel}) for what `main` says now."

# [text](path.md) and [text](path.md#anchor). The target excludes '#' as well as
# ')', so it can't blur into the anchor; no lazy quantifier, so no super-linear backtrack.
LINK = re.compile(r"\[(?P<text>[^\]]*)\]\((?P<target>[^)\s#]+\.md)(?P<anchor>#[^)\s]*)?\)")


class MapError(Exception):
    """The map and the tree disagree — a declared page that is not there."""


def _guard(candidate: pathlib.Path, root: pathlib.Path) -> pathlib.Path:
    """Return candidate's real path, refusing it unless it stays inside root.

    docs/wiki-map.json's patterns and the page names built from them are
    trusted as data, not as paths: a glob or a name carrying `..` would
    otherwise read from, write to, or delete outside the tree this run is
    scoped to (S8707). What comes back is the resolved path and every caller
    uses that, because validating one spelling of a path and then opening
    another is the hole the rule is about. `build` resolves `repo` and `out`
    the same way, so `relative_to` still answers against the root a caller
    holds.
    """
    base = os.path.realpath(root)
    resolved = os.path.realpath(candidate)
    if resolved != base and not resolved.startswith(base + os.sep):
        raise MapError(f"{candidate}: escapes {root}")
    return pathlib.Path(resolved)


def load_map(path: pathlib.Path) -> dict:
    return json.loads(path.read_text(encoding="utf-8"))


def pages_for(patterns: list[str], repo: pathlib.Path) -> list[pathlib.Path]:
    """Every page a pattern list names, in declaration order, deduplicated."""
    found: list[pathlib.Path] = []
    for pattern in patterns:
        if "*" in pattern:
            found.extend(_guard(page, repo) for page in sorted(repo.glob(pattern)))
        else:
            page = _guard(repo / pattern, repo)
            if not page.exists():
                raise MapError(f"{pattern}: declared in wiki-map.json, missing from the tree")
            found.append(page)
    return list(dict.fromkeys(found))


FRONTMATTER = re.compile(r"\A---\n.*?\n---\n", re.DOTALL)


def page_body(page: pathlib.Path) -> str:
    """A page's text with any YAML frontmatter removed.

    Every ADR carries one since tools/regen_adr_index.py, and `docs/decisions/*.md` is
    published here: a wiki renders the block as a horizontal rule and a paragraph
    of keys, so 106 pages would open with their own metadata. The block is for
    docs/decisions/index.yaml, not for a reader.
    """
    return FRONTMATTER.sub("", page.read_text(encoding="utf-8"), count=1)


def page_stem(page: pathlib.Path) -> str:
    """The name a page is known by, which is not always its file name.

    Both `docs/decisions/` and `docs/migration/` call their index `README.md`,
    and a wiki has no directories to keep the two apart -- so an index takes the
    name of the directory it indexes. Left as the file name, one silently
    shadowed the other, which is what the collision guard in `_write` exists to
    make impossible.
    """
    return page.parent.name if page.stem == "README" else page.stem


def covered_pages(declared: str | list[str]) -> list[str]:
    """The pages one covered namespace maps to, whether it declared one or several.

    `Lodestar.Metrics` is why the plural exists: 31 exported types on one page is a
    scrolling exercise rather than a reference, so its classification and regression
    halves are two documents of one namespace. The gate in
    `tests/Shared/ReferenceDocumentation.cs` reads the same field the same way.
    """
    return [declared] if isinstance(declared, str) else list(declared)


def wiki_name(stem: str, channel: str | None = None, version: str | None = None) -> str:
    """The flat file name a page gets in the wiki: it has no directories."""
    if channel is None:
        return stem
    if version is None:
        return f"{channel}-{stem}"
    return f"{channel}-{version}-{stem}"


def _matches(relative: str, pattern: str) -> bool:
    return pathlib.PurePosixPath(relative).match(pattern)


def link_index(repo: pathlib.Path, mapping: dict) -> dict[str, str]:
    """Every publishable page, keyed by its repository-relative path."""
    index: dict[str, str] = {}
    for page in pages_for(mapping["root"], repo):
        index[page.relative_to(repo).as_posix()] = wiki_name(page_stem(page))
    for package in mapping["packages"].values():
        for page in pages_for(package["pages"], repo):
            index[page.relative_to(repo).as_posix()] = wiki_name(page_stem(page), package["wiki"])
    return index


def rewrite_links(text: str, page: pathlib.Path, repo: pathlib.Path, index: dict[str, str]) -> str:
    """Repository-relative Markdown links, as wiki links."""

    def replace(match: re.Match) -> str:
        target = match.group("target")
        # S5332 false positive: string comparison against an already-written link,
        # never a request -- this function makes no network call of any kind.
        if target.startswith(("http://", "https://")):  # NOSONAR S5332
            return match.group(0)
        resolved = (page.parent / target).resolve()
        try:
            key = resolved.relative_to(repo.resolve()).as_posix()
        except ValueError:
            return match.group(0)
        if key not in index:
            return match.group(0)
        return f"[{match.group('text')}]({index[key]}{match.group('anchor') or ''})"

    return LINK.sub(replace, text)


def banner(package: str, wiki: str, version: str, landing: str) -> str:
    return BANNER.format(package=package, version=version, target=wiki_name(landing, wiki, version))


def archive_banner(package: str, wiki: str, version: str, has_entry: bool) -> str:
    """What a frozen page opens on.

    The way out is the channel's entry page rather than the live counterpart of
    this page: a `<channel>-<stem>` link is exactly what an archived page must
    not carry, and the hub reaches the same place in one more click. A package
    whose entry page `entry_page` declines to write gets the sentence without
    the link, for the reason D4 gives about the live banner -- a banner naming a
    page the wiki does not hold is worse than no banner.
    """
    return ARCHIVE_BANNER.format(
        package=package,
        version=version,
        exit=ARCHIVE_EXIT.format(channel=wiki) if has_entry else "",
    )


def _archive_pattern(channel: str) -> re.Pattern:
    return re.compile(rf"^{re.escape(channel)}-(?P<version>\d[^-]*)-")


def _archive_stems(out: pathlib.Path, channel: str) -> dict[str, set[str]]:
    """Every archived stem present for a channel, grouped by version."""
    pattern = _archive_pattern(channel)
    by_version: dict[str, set[str]] = {}
    for page in out.glob(f"{channel}-*.md"):
        page = _guard(page, out)
        match = pattern.match(page.stem)
        if match:
            by_version.setdefault(match.group("version"), set()).add(page.stem[match.end():])
    return by_version


def _live_stems(out: pathlib.Path, channel: str) -> set[str]:
    """Every live (non-archived) stem present for a channel."""
    pattern = _archive_pattern(channel)
    prefix_len = len(channel) + 1
    return {
        page.stem[prefix_len:]
        for page in (_guard(page, out) for page in out.glob(f"{channel}-*.md"))
        if not pattern.match(page.stem)
    }


def sidebar(repo: pathlib.Path, out: pathlib.Path, mapping: dict) -> str:
    """The navigation, read off the tree that was just written."""
    lines = ["### Lodestar", "", "- [Home](Home)", ""]
    for package in mapping["packages"].values():
        channel = package["wiki"]
        landing = _resolve_landing(_live_stems(out, channel), package)
        if landing is None:
            continue
        # The channel's own entry page, not its first guide: D10 makes that page the
        # one place a reader sees the namespaces and the guides together.
        lines.append(f"- [{channel}]({channel})")
        for version, stems in sorted(_archive_stems(out, channel).items(), key=lambda kv: _version_key(kv[0])):
            archived = _resolve_landing(stems, package)
            if archived is not None:
                lines.append(f"  - [{version}]({wiki_name(archived, channel, version)})")
    lines += ["", "### Project", ""]
    # One row per root page a reader starts from: a globbed directory contributes its
    # index alone, which links the rest -- seven ADR slugs listed flat were not navigation.
    for page in _root_entries(repo, mapping):
        lines.append(f"- [{title_of(page)}]({wiki_name(page_stem(page))})")
    return "\n".join(lines) + "\n"


def _root_entries(repo: pathlib.Path, mapping: dict) -> list[pathlib.Path]:
    """The root pages the sidebar lists: each literal page, and each glob's README."""
    entries: list[pathlib.Path] = []
    for pattern in mapping["root"]:
        if "*" not in pattern:
            entries.extend(pages_for([pattern], repo))
            continue
        readme = _guard(repo / pathlib.PurePosixPath(pattern).parent / "README.md", repo)
        if readme.exists():
            entries.append(readme)
    return entries


def title_of(page: pathlib.Path) -> str:
    """A page's H1, without its `#`: the label a hand-written page chose for itself."""
    lines = page_body(page).splitlines()
    first = lines[0] if lines else ""
    return first.lstrip("#").strip() if first.startswith("#") else page_stem(page)


def short_title(page: pathlib.Path) -> str:
    """The topic half of a `Topic — `Namespace`` title, the length a breadcrumb can carry."""
    return title_of(page).split(" — ", 1)[0].replace("`", "")


# A sentence ends at one of these followed by a space or the paragraph's end --
# never inside a code span or a link, where `1.18.0` and `(kmeans.md)` live.
SENTENCE_END = ".?!"


def lead_sentence(body: str) -> str:
    """The first sentence of a page's first paragraph, which is how a hub describes it."""
    return _first_sentence(_first_paragraph(body))


def _first_paragraph(body: str) -> str:
    """The first run of prose lines after the H1: no heading, quote, table, list or fence."""
    paragraph: list[str] = []
    fenced = False
    for line in body.splitlines()[1:]:
        stripped = line.strip()
        if stripped.startswith("```"):
            fenced = not fenced
        prose = stripped and not fenced and not stripped.startswith(
            ("```", "#", ">", "|", "<!--", "- ", "* "))
        if prose:
            paragraph.append(stripped)
        elif paragraph:
            break
    return " ".join(paragraph)


def _first_sentence(text: str) -> str:
    in_code, depth = False, 0
    for position, char in enumerate(text):
        if char == "`":
            in_code = not in_code
        elif in_code:
            continue
        elif char in "[(":
            depth += 1
        elif char in "])":
            depth = max(0, depth - 1)
        elif depth == 0 and char in SENTENCE_END and text[position + 1:position + 2] in ("", " "):
            return text[:position + 1]
    return text


BREADCRUMB_SEPARATOR = " › "


def breadcrumb(
    page: pathlib.Path, repo: pathlib.Path, index: dict[str, str], channel: str | None
) -> str:
    """The line that says where a page sits, so a search landing is not a dead end.

    A GitHub wiki has no directories, so this line is the whole answer to where a
    member page sits: Home, the package's hub, then the namespace page whose
    directory holds it -- `distances/hamming.md` sits under `distances.md`. The
    hub is left out when `channel` is `None`, which is an archive: it has none.
    """
    crumbs = ["[Home](Home)"]
    if channel is not None:
        crumbs.append(f"[{channel}]({channel})")
    parent = page.parent.with_suffix(".md")
    key = parent.relative_to(repo).as_posix() if parent.is_relative_to(repo) else None
    if key in index:
        crumbs.append(f"[{short_title(parent)}]({index[key]})")
    return BREADCRUMB_SEPARATOR.join(crumbs) + "\n\n"


def _landing(stems: set[str]) -> str | None:
    """The page a channel opens on: the first, alphabetically, that it holds."""
    pages = sorted(stems)
    return pages[0] if pages else None


def _declared_landing(package: dict) -> str | None:
    """The guide the map declares first for a package, if it declares one.

    Every package lists its guide pages before its `docs/reference/**/*.md`
    glob (see docs/wiki-map.json) -- a reference page such as `distances.md`
    would otherwise win `_landing`'s alphabetical fallback over `quickstart.md`
    and become the channel's front door instead of the guide.
    """
    for pattern in package["pages"]:
        if "*" not in pattern:
            return pathlib.PurePosixPath(pattern).stem
    return None


def _resolve_landing(stems: set[str], package: dict) -> str | None:
    """The declared guide if these stems actually hold it, else the fallback."""
    declared = _declared_landing(package)
    if declared is not None and declared in stems:
        return declared
    return _landing(stems)


def _version_key(version: str) -> tuple:
    return tuple(int(part) if part.isdigit() else part for part in version.split("."))


def entry_page(
    repo: pathlib.Path,
    package: dict,
    version: str | None = None,
    index: dict[str, str] | None = None,
    strict: bool = True,
) -> str | None:
    """A package's hub: a link per guide it ships, then one per namespace it covers.

    This is the page D10 puts between Home and a reference page -- Home no
    longer links a bare channel name that resolved to nothing. `None` for a
    package that covers no namespace and ships no guide, which is
    `Lodestar.Metrics` before its first reference page lands: a page linking
    nothing is not navigation, and `home` falls back to the same "no pages
    yet" text it already used before this page existed, rather than link to
    an empty one.
    """
    channel = package["wiki"]
    index = index if index is not None else {}
    lines = [f"# {channel}", ""]
    linked = False

    namespace_pages = _namespace_pages(repo, package, strict)
    covered_set = {page for _, page in namespace_pages}
    # A guide that is also a covered reference page -- `docs/reference/stats.md` --
    # is listed once, as a namespace: under both headings it read as two pages.
    guides = [
        path for path in (_guard(repo / pattern, repo) for pattern in package["pages"] if "*" not in pattern)
        if path not in covered_set
    ]
    if guides:
        lines += ["## Start here", ""]
        for path in guides:
            lines.append(f"- [{title_of(path)}]({wiki_name(page_stem(path), channel, version)})")
        lines.append("")
        linked = True

    if namespace_pages:
        lines += ["## Namespaces", ""]
        for _, path in namespace_pages:
            # The page's own H1 is the label and its first sentence the description:
            # `Lodestar.Stats — nanpolicy` was a label a reader could not choose from.
            lead = rewrite_links(lead_sentence(page_body(path)), path, repo, index)
            row = f"- [{title_of(path)}]({wiki_name(page_stem(path), channel, version)})"
            lines.append(f"{row} — {lead}" if lead else row)
        lines.append("")
        linked = True

    return "\n".join(lines) + "\n" if linked else None


def _namespace_pages(
    repo: pathlib.Path, package: dict, strict: bool = True
) -> list[tuple[str, pathlib.Path]]:
    """Each covered namespace's page, checked to name the namespace it documents.

    A covered entry declared as a directory -- `docs/reference/text/distances` --
    is documented by the page beside it, `distances.md`, and that page's H1 is
    what the hub shows. It has to name the namespace in backticks, or the hub
    tells the reader one namespace and links another: three pages named
    `Lodestar.Text` and `Lodestar.Embeddings` for a sub-namespace until #1107.
    An entry declared as a file is a package-level page and names what it likes.
    `strict` is off for an --archive run, which may read a tag cut before the check.
    """
    found: list[tuple[str, pathlib.Path]] = []
    for namespace, declared in sorted(package["covered"].items()):
        for entry in covered_pages(declared):
            path = _namespace_page(repo, namespace, entry, strict)
            if path is not None:
                found.append((namespace, path))
    return found


def _namespace_page(
    repo: pathlib.Path, namespace: str, entry: str, strict: bool
) -> pathlib.Path | None:
    """One covered entry's page, or `None` when a lenient run finds none."""
    is_directory = not entry.endswith(".md")
    path = _guard(repo / (f"{entry}.md" if is_directory else entry), repo)
    if not path.exists():
        if strict:
            raise MapError(f"{entry}: covered in wiki-map.json, and no page documents it")
        return None
    if strict and is_directory and f"`{namespace}`" not in title_of(path):
        raise MapError(
            f"{path.relative_to(repo)}: its title does not name `{namespace}`, "
            "the namespace wiki-map.json says it covers"
        )
    return path


def home(
    repo: pathlib.Path,
    out: pathlib.Path,
    mapping: dict,
    released: dict[str, str],
    index: dict[str, str],
) -> str:
    """The front page: the map's `home` document, then the table read off the tree.

    The document is written by hand because a task-oriented path is prose, and
    prose belongs where markdownlint and review reach it. Its links are
    repository-relative and each must land on a published page: a renamed
    guide breaks the front door loudly here rather than as a 404 on the wiki.

    A package with no entry page -- `entry_page` returned `None`, so nothing
    under this channel is covered or guided yet -- has nothing a reader could
    walk down to, and `sidebar` already drops its row for the same reason.
    Home keeps the row, because the released version is the other half of
    what the table is for, and writes no link rather than one to a page that
    does not exist.
    """
    lines = _home_opening(repo, mapping, index) + [
        "| Package | Latest released | Documentation |",
        "| --- | --- | --- |",
    ]
    for name, package in mapping["packages"].items():
        version = released.get(name, "unreleased")
        channel = package["wiki"]
        entry_target = _guard(out / f"{channel}.md", out)
        target = f"[{channel}]({channel})" if entry_target.exists() else "no pages yet"
        lines.append(f"| `{name}` | {version} | {target} |")
    return "\n".join(lines) + "\n"


def _home_opening(repo: pathlib.Path, mapping: dict, index: dict[str, str]) -> list[str]:
    """What Home says above its package table."""
    declared = mapping.get("home")
    if declared is None:
        return [
            "# Lodestar",
            "",
            "A data-science toolkit for C#/.NET. Each package documents itself, at the version",
            "you installed.",
            "",
        ]
    page = pages_for([declared], repo)[0]
    body = page_body(page)
    for match in LINK.finditer(body):
        target = match.group("target")
        # S5332 false positive: a string comparison, never a request.
        if target.startswith(("http://", "https://")):  # NOSONAR S5332
            continue
        resolved = _guard(page.parent / target, repo)
        if resolved.relative_to(repo).as_posix() not in index:
            raise MapError(f"{declared}: links {target}, which the wiki does not publish")
    return [rewrite_links(body, page, repo, index).rstrip("\n"), "", "## Packages", ""]


def _build_archive(
    repo: pathlib.Path,
    out: pathlib.Path,
    mapping: dict,
    index: dict[str, str],
    names: dict[str, pathlib.Path],
    archive: tuple[str, str],
) -> list[pathlib.Path]:
    """One package's frozen version, and nothing else -- the --archive branch."""
    name, version = archive
    package = mapping["packages"][name]
    written: list[pathlib.Path] = []
    for stale in out.glob(f"{package['wiki']}-{version}-*.md"):
        _guard(stale, out).unlink()
    frozen = _archive_index(index, repo, package, version)
    has_entry = entry_page(repo, package, strict=False) is not None
    prefix = archive_banner(name, package["wiki"], version, has_entry)
    for page in pages_for(package["pages"], repo):
        wname = wiki_name(page_stem(page), package["wiki"], version)
        crumb = breadcrumb(page, repo, frozen, None)
        written.append(_write(page, out, repo, frozen, prefix + crumb, wname, names))
    return written


def _archive_index(
    index: dict[str, str], repo: pathlib.Path, package: dict, version: str
) -> dict[str, str]:
    """The link index as a frozen page sees it: this package's pages, versioned.

    Sharing the live index is what sent a reader of 0.4.0 one click into
    whatever `main` says today, which is the one thing an archive exists to
    prevent. Only the frozen package moves: a link to another package's channel
    is right as it stands, because that channel was not frozen, and so is a link
    to a root page, which follows `main` and has no frozen counterpart to point
    at. The banner is what tells the reader those two leave the archive.
    """
    frozen = dict(index)
    for page in pages_for(package["pages"], repo):
        frozen[page.relative_to(repo).as_posix()] = wiki_name(
            page_stem(page), package["wiki"], version)
    return frozen


def _build_channel(
    repo: pathlib.Path,
    out: pathlib.Path,
    index: dict[str, str],
    names: dict[str, pathlib.Path],
    pkg_name: str,
    package: dict,
    released: dict[str, str],
    strict: bool = True,
) -> list[pathlib.Path]:
    """One package's live channel: its pages refreshed, banner-prefixed once archived."""
    pages = pages_for(package["pages"], repo)
    if not pages:
        return []

    channel = package["wiki"]
    version = released.get(pkg_name)

    # Against the archive pointed into, not against main: a declared guide may
    # postdate the tag, and a banner the wiki cannot hold is worse than none (#256).
    archives = _archive_stems(out, channel)
    landing = _resolve_landing(archives[version], package) if version in archives else None
    prefix = banner(pkg_name, channel, version, landing) if version and landing else ""
    archive_pattern = _archive_pattern(channel)
    for stale in out.glob(f"{channel}-*.md"):
        if not archive_pattern.match(stale.stem):
            _guard(stale, out).unlink()

    entry = entry_page(repo, package, index=index, strict=strict)
    hub = channel if entry is not None else None
    written = [
        _write(
            page, out, repo, index, prefix + breadcrumb(page, repo, index, hub),
            wiki_name(page_stem(page), channel), names,
        )
        for page in pages
    ]

    entry_target = _guard(out / f"{channel}.md", out)
    if entry is not None:
        entry_target.write_text(prefix + entry, encoding="utf-8")
        written.append(entry_target)
    elif entry_target.exists():
        # Covered before, not now: nothing would link here any more, so the
        # stale page from a previous run must not linger and outlive its link.
        entry_target.unlink()

    return written


def missing_archives(
    out: pathlib.Path, mapping: dict, released: dict[str, str]
) -> list[tuple[str, str]]:
    """The released versions the wiki holds no frozen page for, in map order.

    A tag push archives the version that triggered it, and GitHub keeps at most
    one pending run per concurrency group -- so publishing several tags at once
    drops every archive but the last, silently (#199). This is what a later run
    reads to catch up, and it is why a cancelled run costs nothing.

    The caller archives each from *its own tag*, never from the current tree: the
    documentation moves between releases, and freezing today's pages under an
    older number would be worse than the gap it fills.
    """
    out = pathlib.Path(out)
    behind: list[tuple[str, str]] = []
    for name, package in mapping["packages"].items():
        version = released.get(name)
        if version and version not in _archive_stems(out, package["wiki"]):
            behind.append((name, version))
    return behind


def build(
    repo: pathlib.Path,
    out: pathlib.Path,
    mapping: dict,
    released: dict[str, str],
    archive: tuple[str, str] | None = None,
) -> list[pathlib.Path]:
    """Write the wiki tree. Returns the pages written, for the caller to report.

    An archive is written first and the live channels are then rebuilt on top of
    it, never instead of it. Only the named package is frozen -- that split is
    what lets a per-package tag publish one version -- but the sidebar's version
    list and the banner of D4 are both read off the tree, so returning after the
    archive left the sidebar without the new version and every live page still
    naming the previous release. The banner exists so it cannot go stale, and a
    release tag was the one moment it did.
    """
    # Both roots are resolved once, here, so every path derived from them is
    # already real by the time _guard compares one against them.
    repo, out = pathlib.Path(os.path.realpath(repo)), pathlib.Path(os.path.realpath(out))
    if not out.parent.is_dir():
        raise MapError(f"{out}: its parent directory does not exist")
    # S8707 asks for a path validated against a root; here the argument *is* the root,
    # so only the one level below an existing directory can be checked, and is.
    out.mkdir(exist_ok=True)  # NOSONAR S8707
    index = link_index(repo, mapping)
    names: dict[str, pathlib.Path] = {}

    written: list[pathlib.Path] = (
        _build_archive(repo, out, mapping, index, names, archive) if archive is not None else []
    )

    written += [
        _write(page, out, repo, index, "", wiki_name(page_stem(page)), names)
        for page in pages_for(mapping["root"], repo)
    ]
    for pkg_name, package in mapping["packages"].items():
        # An --archive run may read an old tag, whose titles predate the check; the
        # commit a tag names was already checked strictly when main received it.
        written.extend(_build_channel(
            repo, out, index, names, pkg_name, package, released, strict=archive is None))

    _guard(out / "_Sidebar.md", out).write_text(sidebar(repo, out, mapping), encoding="utf-8")
    _guard(out / "Home.md", out).write_text(
        home(repo, out, mapping, released, index), encoding="utf-8")
    return written


def _write(
    page: pathlib.Path,
    out: pathlib.Path,
    repo: pathlib.Path,
    index: dict[str, str],
    prefix: str,
    name: str,
    names: dict[str, pathlib.Path],
) -> pathlib.Path:
    """Write one page under its flat wiki name, refusing a name collision."""
    if name in names and names[name] != page:
        raise MapError(
            f"{name}: {names[name].relative_to(repo)} and {page.relative_to(repo)} "
            "would both publish under this name"
        )
    names[name] = page
    target = _guard(out / f"{name}.md", out)
    target.write_text(
        prefix + rewrite_links(page_body(page), page, repo, index),
        encoding="utf-8",
    )
    return target


# S7494 and S7500 disagree on this exact shape: dict() over a generator of pairs,
# or the equivalent comprehension, each draws the other rule's complaint.
def _pairs(values: list[str]) -> dict[str, str]:
    return dict(value.split("=", 1) for value in values)  # NOSONAR S7494


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repo", required=True, type=pathlib.Path)
    parser.add_argument("--out", required=True, type=pathlib.Path)
    parser.add_argument("--released", action="append", default=[], metavar="PACKAGE=VERSION")
    parser.add_argument("--archive", metavar="PACKAGE=VERSION")
    parser.add_argument(
        "--report-missing-archives",
        action="store_true",
        help="print the released versions the out tree holds no archive for, one per line, and exit",
    )
    arguments = parser.parse_args()

    repo = pathlib.Path(os.path.realpath(arguments.repo))
    archive = tuple(arguments.archive.split("=", 1)) if arguments.archive else None

    try:
        mapping = load_map(_guard(repo / "docs" / "wiki-map.json", repo))
        if arguments.report_missing_archives:
            for name, version in missing_archives(
                arguments.out, mapping, _pairs(arguments.released)
            ):
                print(f"{name}={version}")
            return 0

        written = build(repo, arguments.out, mapping, _pairs(arguments.released), archive)
    except MapError as error:
        print(f"::error::{error}", file=sys.stderr)
        return 1

    print(f"pages written: {len(written)}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
