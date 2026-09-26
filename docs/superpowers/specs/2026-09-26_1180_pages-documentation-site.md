# The documentation served where a search engine reads it

**Issue:** [#1180](https://github.com/CyrilB1531/lodestar/issues/1180).
**Status:** written before the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

GitHub serves the wiki `wiki.yml` publishes with `x-robots-tag: none`, which is `noindex, nofollow`,
measured on 2026-09-26:

```text
$ curl -sI https://github.com/CyrilB1531/lodestar/wiki | grep -i x-robots-tag
x-robots-tag: none
```

So none of the live pages `docs/wiki-map.json` publishes reaches a reader through a search engine —
993 at `706e95f3`, `main` on 2026-09-26, and more with every package page merged: 920 reference entries, 18 package READMEs, 18 performance pages, 16 guides and 21 root
pages (`equivalence.md`, `docs/decisions/`, `docs/migration/`, the performance index and the two
benchmark pages), plus Home. The same files viewed as blobs on github.com carry no such header, but
a blob ranks as a source file: no title of its own, no navigation, no stable reading address.

## What was settled with the maintainer

- **Both publications, from one build.** The wiki stays as it is; a GitHub Pages site is added, fed
  by the same map and the same selection code, in the same workflow.
- **Pages carries the live channel only.** The frozen per-version archives stay on the wiki, which
  keeps 3,281 files today. Without them no page on the site exists twice, which is also what makes
  a canonical tag unnecessary.
- **DocFX, over Markdown only.** No `metadata` section, so no generated API reference: that would
  duplicate `docs/reference/`, which the reference gate already holds to both target frameworks'
  assemblies.

## The generators read, and why four lost

Read on 2026-09-26 from each project's latest release:

| generator | latest release | why it lost, or won |
| --- | --- | --- |
| DocFX | v2.81.0, 2026-09-25 | **chosen**: a .NET tool, so no new toolchain in CI; active and stable in 2.x; search and `sitemap.xml` built in; the `modern` template writes `<meta name="description">` from a page's `description` |
| Starlight (Astro) | 0.42.4, 2026-09-24 | the best SEO defaults, but a Node project and lockfile in a .NET repository, relative `.md` links Astro does not rewrite, and a 0.x that breaks on minor versions |
| Zensical | 0.0.65, 2026-09-24 | Python like `tools/`, and Material's successor, but a pre-release |
| Material for MkDocs | 9.7.7, 2026-07-17 | in maintenance mode, its authors moved to Zensical; MkDocs itself last released 1.6.1 on 2024-08-30 |
| Jekyll, GitHub's own builder | Jekyll 3.10.0, pinned by GitHub | old, and no search |

## Decisions

### One source, two outputs

`docs/wiki-map.json` stays the only statement of what is published. A new `tools/build_site.py`
imports the selection from `tools/build_wiki.py` — `load_map`, `pages_for`, `page_body`,
`title_of` and `lead_sentence` — rather than adding a mode to a 776-line file. It writes a
DocFX source tree to `artifacts/site/`, which the existing `artifacts/` entry in `.gitignore` already
keeps out of git, and that tree's only inputs are the map, the pages it lists and `tools/site/`. A
page listed in the map and absent from the tree exits 1, as `build_wiki.py` already does.

### Addresses

The flat names were a constraint of the wiki, which addresses a page by file name alone. The site
keeps the repository's structure under `https://cyrilb1531.github.io/lodestar/`:

| source | published at |
| --- | --- |
| `docs/wiki/home.md`, followed by a package table | `index.html` |
| `docs/<path>.md` | `<path>.html` |
| `src/Lodestar.<Name>/README.md` | `packages/<slug>/index.html`, served as `packages/<slug>/` |
| `src/Lodestar.<Name>/performance.md` | `packages/<slug>/performance.html` |

`<slug>` is the package id without `Lodestar.`, lowercased, with `.` turned into `-`:
`Lodestar.Stats.TimeSeries` becomes `stats-timeseries`, the same slug `docs/reference/` already
uses for its folders. So `docs/guides/survival-analysis.md` is published at
`guides/survival-analysis.html`, and `docs/reference/text/distances.md` at
`reference/text/distances.html`. `build_site.py` writes each page to its address with a `.md`
suffix, and DocFX turns that into `.html`.

Home's package table has the wiki's three columns — the package, its latest released version, a
link — but is written by `build_site.py` rather than by `build_wiki.py`'s `home`, which links flat
wiki names and reads the wiki's own tree. The released versions come in as the same
`--released <PackageId>=<Version>` arguments `wiki.yml` already computes from the tags.

### What each page carries

A YAML front matter block with two keys, each written as a double-quoted YAML string:

- `title`: the page's H1, from `title_of`. DocFX appends `_appTitle`, so a tab reads
  `Distances | Lodestar`.
- `description`: the first sentence `lead_sentence` finds, stripped of inline Markdown — a link
  keeps its text, backticks and emphasis markers go — and cut at a word boundary to at most 160
  characters, with `…` when cut. A page that opens on a table, a list or a fence has no lead
  sentence, and takes its title instead. The build prints how many pages fell back, so a regression
  in the lead sentences shows as a number. Home is the exception to the first key: its H1 is the
  site's name, which `_appTitle` already appends, so its title is the constant `HOME_TITLE`,
  "Data-science toolkit for C#/.NET at Python parity".

The body follows unchanged, except for its links.

### Links

Every Markdown link is resolved against the page's source path, the way `rewrite_links` does now:

- **a published page**: a relative link from the page's new address to the target's new address,
  with a `.md` suffix DocFX turns into `.html`, and the `#anchor` kept;
- **a repository file the map does not publish** (`CONTRIBUTING.md`, a `.cs` file, a script):
  `https://github.com/CyrilB1531/lodestar/blob/main/<path>`, or `tree/main/<path>` for a directory,
  anchor kept;
- **an absolute URL**: unchanged;
- **a path inside the repository that names no file**: left as written, so DocFX reports it and
  the build fails, rather than a GitHub link to nothing being published;
- **a path that resolves outside the repository**: left as written, so DocFX reports it and the build
  fails, rather than a guess being published.

Assembling the plan's code on a copy of the repository on 2026-09-26, the link sweep found three
links of this kind already broken: the two pages `bench-nightly.yml` regenerates every night,
`docs/guides/nightly_run.md` and `docs/guides/benchmark_latest.md`, link `performance`,
`benchmark_latest` and `nightly_run` by the wiki's flat names, which resolve on the wiki alone.
Their renderers write `performance.md` and its neighbours from now on. The wiki does not see the
change: `build_wiki.py` rewrites a `.md` link to the same flat name, checked on the same copy.

### Navigation

DocFX shows, on a page, the TOC that references it through `href` (its *Table of contents* page,
read on 2026-09-26), so one file can carry pages from several folders. `build_site.py` writes four
TOC files from the map:

- `toc.yml`, the top bar: **Packages**, **Migrating** (`docs/migration/`, its README first),
  **Parity** (`equivalence.md`) and **Project** (the performance index, the two benchmark pages and
  `docs/decisions/`, its README first).
- `packages/toc.yml`: one node per package in the map's order. The node itself opens the package's
  README; under it: its guides, *Reference*, and *Performance*. *Reference* nests by path: a
  published page `X.md` holds the published pages of the directory `X/`, and a page whose parent
  page is not published sits at the top of *Reference*. That places every reference page exactly
  once, including the ones no `covered` entry names, such as `docs/reference/stats/timeseries/`,
  which has no page of its own above it.
- `migration/toc.yml`, the pages of `docs/migration/`, its README first.
- `project/toc.yml`, the Project pages in the order given above.

### DocFX configuration

`tools/site/docfx.json` is committed and copied to the root of the generated tree:

```json
{
  "build": {
    "content": [{ "files": ["**/*.md", "**/toc.yml"] }],
    "resource": [{ "files": ["images/**", "google*.html"] }],
    "output": "_site",
    "template": ["default", "modern"],
    "globalMetadata": {
      "_appTitle": "Lodestar",
      "_appName": "Lodestar",
      "_appLogoPath": "images/logo.png",
      "_appFaviconPath": "images/icon.png",
      "_enableSearch": true,
      "_disableContribution": true
    },
    "sitemap": {
      "baseUrl": "https://cyrilb1531.github.io/lodestar/",
      "changefreq": "weekly"
    }
  }
}
```

- `_disableContribution` is true because DocFX's *Improve this Doc* would point into the generated
  tree, at files that do not exist in the repository.
- `images/icon.png` is `assets/icon.png`, the favicon; `images/logo.png` is `tools/site/logo.png`, the same icon at 40 px, because the modern template shows a logo at its own size and 128 px overflows the navigation bar. `build_site.py` copies both.
- `sitemap.baseUrl` must end with a slash, or DocFX writes no `sitemap.xml`.

DocFX is pinned in a new `.config/dotnet-tools.json` and run with `dotnet tool restore` then
`dotnet docfx artifacts/site/docfx.json --warningsAsErrors`, so a broken link or an orphan TOC entry
fails the build.

### Workflows

- **`wiki.yml` gains a `pages` job**, beside the wiki job and with no `needs:` on it, so either
  publication can fail without stopping the other. It runs on a push to `main` and on
  `workflow_dispatch`, never on a tag: tags produce archives, and archives stay on the wiki. It
  deploys only when the ref is `refs/heads/main`, so a dispatch from another branch builds nothing.
  It builds the tree, runs DocFX, and deploys through `actions/configure-pages`,
  `actions/upload-pages-artifact` and `actions/deploy-pages`, each pinned by SHA like every other
  action here, with `permissions: pages: write, id-token: write` and the `github-pages` environment.
  Its `paths` filter gains `tools/build_site.py`, `tools/site/**`, `.config/dotnet-tools.json`,
  `src/*/README.md`, `src/*/performance.md` and `assets/icon.png`.
- **`ci.yml` gains a `site` job** that builds the tree and runs DocFX without deploying, so a broken
  link is caught by the pull request and not by the deployment after the merge. The `changes` job
  gains a `site_changed` output, computed by a new `tools/site_changed.py` that reads the file list
  from standard input, as `tools/docs_only.py` does, and answers `true` for the paths in the filter
  above plus `docs/**`, `tools/build_wiki.py`, `docs/wiki-map.json` and `ci.yml` itself, since the
  `site` job's own definition lives there. The `site` job is **not a
  required check**: the four required checks on `main` do not change.

### Outside the repository

These carry no commit, and are recorded here and in the pull request:

- the repository's Pages source set to *GitHub Actions*, once, before the first deployment;
- `homepageUrl` moved from the wiki to `https://cyrilb1531.github.io/lodestar/`, after the first
  deployment succeeds;
- in Google Search Console, a *URL prefix* property on `https://cyrilb1531.github.io/lodestar/`,
  verified with the HTML file Google issues, committed under `tools/site/` where the `google*.html`
  resource glob picks it up, then `sitemap.xml` submitted. A `robots.txt` cannot do this: a crawler
  reads only the one at the host's root, `https://cyrilb1531.github.io/robots.txt`, which a project
  site does not own. The verification file is Cyril's to fetch, and the site ships without it until
  he has it.

## Not done

- **Archives on Pages**: settled above; the wiki keeps them.
- **A canonical tag**: the `modern` template writes none, and a site with no duplicated page does
  not need one.
- **A custom domain**: nothing here prevents one later; GitHub then redirects `github.io` to it.
- **Retiring the wiki**: settled above; both stay.
- **Generated API pages**: settled above; `docs/reference/` is the reference.

## Proof

- `tools/tests/test_build_site.py`, which the Lint job already runs through `tools/tests`:
  - the site publishes exactly the pages `build_wiki.py` publishes as live, no more and no fewer;
  - each row of the address table above, including the `stats-timeseries` and `extensions-ai` slugs;
  - the four link cases, each with and without an anchor;
  - front matter: YAML quoting of a title holding a colon and a quote, the Markdown stripping, the
    160-character cut, and the fallback to the title;
  - the four TOC files: every published page appears exactly once, every `href` resolves to a
    generated file.
- `tools/site_changed.py` gets its own tests in the same directory, one per path class, in the style
  of `tools/tests/test_skip_build.py`.
- DocFX with `--warningsAsErrors`, on every pull request the classifier selects and on every
  deployment.
- After the first deployment, measured and written in the pull request:
  `curl -sI https://cyrilb1531.github.io/lodestar/guides/survival-analysis.html` shows no
  `x-robots-tag`, and `sitemap.xml` lists one address per live page plus Home, the count
  `find artifacts/site -name '*.md' -not -path '*/_site/*' | wc -l` gives on the same commit.
