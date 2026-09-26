# The pipeline — what runs, and what protects `main`

The workflows in this directory, and the repository ruleset they satisfy. What a *contributor*
does is in [`CONTRIBUTING.md`](../../CONTRIBUTING.md); this file is the pipeline's own subject —
which workflow answers which question, and what has to be green before the merge button appears.

## The workflows

| file | name | runs on | what it answers |
| --- | --- | --- | --- |
| [`ci.yml`](ci.yml) | `CI` | every pull request, and every push to `main` | everything three of the four required checks stand for: lint, build, test, analyze, pack, the sample, the snippets, the oracles and Windows |
| [`pr-closes.yml`](pr-closes.yml) | `Pull request closes` | every pull request, drafts included, and every edit of its description or labels | whether it closes an issue, each an open issue of this repository assigned to its author, unless a bot opened it or it carries `no-issue` — the fourth required check, `Pull request closes only open issues` |
| [`classify-pull-request.yml`](classify-pull-request.yml) | `Classify a pull request` | a pull request opening, and every push to it | which milestone, labels and boards the changed files earn it. Reconciled on every push, so the attribution follows the diff |
| [`release.yml`](release.yml) | `Release` | a `Lodestar.*/v*` tag | packs and publishes that one package to GitHub Packages, refusing the job when the tag and `Version.props` disagree |
| [`release-nuget-org.yml`](release-nuget-org.yml) | `Publish to nuget.org (Trusted Publishing)` | `workflow_dispatch` | the same package, to nuget.org, over OIDC with no stored key |
| [`wiki.yml`](wiki.yml) | `Wiki` | a push to `main`, a `Lodestar.*/v*` tag, or `workflow_dispatch` | turns `docs/` into the published wiki — a live channel per package on `main`, a frozen archive directory per tag — and, on a branch only, deploys the same live pages to the documentation site at `https://cyrilb1531.github.io/lodestar/`, which a search engine indexes where it does not index the wiki ([#1180](https://github.com/CyrilB1531/lodestar/issues/1180)) |
| [`bench-nightly.yml`](bench-nightly.yml) | `Benchmarks (nightly)` | 02:00 UTC, or `workflow_dispatch` | the night's benchmark run, inside the budget `BUDGET_MINUTES` sets |
| [`bench-ondemand.yml`](bench-ondemand.yml) | `Benchmark (on demand)` | `workflow_dispatch` | one named benchmark, when a `perf/` pull request needs a number |

Only `ci.yml` is read by the pull-request pipeline. That is what makes the other seven skippable for
a pull request that changes nothing else — see [The reduced path](#the-reduced-path) below.

## What protects `main`

The project has one maintainer, who reviews and merges every pull request. That constrains how the
branch can be protected: **GitHub does not let you approve your own pull request**, so a rule
requiring an approving review would block every pull request here — there would be nobody able to
give it.

Protection is therefore built on checks rather than approvals. A repository ruleset named
**`main protected by checks`** targets the default branch, requires a pull request, and requires
four checks to pass before the merge button becomes available: `Lint (markdown + C# format)`,
`Oracles are reproducible`, `Build and analyze` and `Pull request closes only open issues`. The
last comes from `pr-closes.yml`, which runs on every pull request and skips none, since a required
check that is never posted stays pending ([#1152](https://github.com/CyrilB1531/lodestar/issues/1152)).
[`CONTRIBUTING.md`](../../CONTRIBUTING.md#the-four-checks-that-guard-main) has what each one guards,
which is what a contributor reads when one goes red.

`Build and analyze` is a gate job rather than a job that builds. It stands for
`Build, test, analyze`, `Sample consumes the packages` and `Guide snippets compile, reference
snippets run`, and fails when any of them does. The analysis ran in its own workflow until it
shared the first job's build ([#857](https://github.com/CyrilB1531/lodestar/issues/857)); the check
keeps the name the ruleset requires. Standing for the two packaging jobs is what makes the
packaging gate and `check_nuspec_dependencies.py` blocking after
[#1028](https://github.com/CyrilB1531/lodestar/issues/1028) moved them out of the build job
([#1055](https://github.com/CyrilB1531/lodestar/issues/1055)). `Build, test, analyze` — which
packed until #1028 and was named for it until
[#1073](https://github.com/CyrilB1531/lodestar/issues/1073) — is therefore no longer required by
name.

The ruleset has **no bypass list, and it binds the administrator**. That is deliberate: a guard
rail the sole maintainer can step over on a tired evening is a suggestion. Getting past it means
disabling the rule in *Settings → Rules*, which is a visible act with a record, rather than a merge
nobody would have noticed.

"Require approvals" stays off until a second maintainer joins. Self-merging after green checks is
the expected flow here, not a shortcut — the pull request still earns its keep as the place CI runs
against the merge result, and as the record of why a change was made.

### Why the required check is this repository's own job

The analysis steps are skipped on Dependabot and fork pull requests, where `SONAR_TOKEN` is
unreachable; the build and the tests still run there. This is also why the required check is
`Build and analyze` and not SonarQube Cloud's `SonarCloud Code Analysis`. That one is never posted
at all on such a pull request, and a required check that never arrives stays pending for ever.

### The reduced path

A pull request that cannot change what the build proves takes a shorter path. A first job,
`Changed files`, lists its files and asks
[`tools/skip_build.py`](../../tools/README.md#skip_buildpy) whether every one is either Markdown or
a workflow the pull-request pipeline does not read — never `ci.yml`, which is the gate itself and
is tested by running it. When every file qualifies, the build, the sample, the oracles, Windows and
the analysis are skipped. `Lint` then runs the documentation tests **when a Markdown file moved** —
on the net10.0 binaries `main` published for the base commit, with this pull request's docs staged
beside them, and only building the solution when no such binaries exist. A workflow-only pull
request carries no Markdown, so there is nothing for them to judge and they are skipped too.
`tools/README.md` has the rules each of the four scripts applies —
[`skip_build.py`](../../tools/README.md#skip_buildpy),
[`docs_only.py`](../../tools/README.md#docs_onlypy),
[`format_needed.py`](../../tools/README.md#format_neededpy) and
[`stage_doc_inputs.py`](../../tools/README.md#stage_doc_inputspy).

GitHub counts a job skipped by its condition as a passed check, so `Build and analyze` accepts a
skip **only on a classified pull request**, and names the class it accepted in its log rather than
waving a bare `skipped` through. It never accepts a skipped `Guide snippets compile, reference
snippets run` while a Markdown file moved: a documentation change is exactly what breaks a guide
snippet.

`Documentation site builds` renders the site with DocFX, warnings as errors, without deploying it,
so a broken link fails the pull request instead of the deployment after the merge. It runs when
`tools/site_changed.py` says the pull request touches something the site reads, and it is not a
required check.
