# 0099 — Survival has no .NET incumbent, and `scikit-survival` is refused on its licence

**Status:** accepted · **Date:** 2026-09-10

## Context

[#442](https://github.com/CyrilB1531/lodestar/issues/442) called survival analysis **the largest
void it surveyed** and held it in reserve, because largest void is not the same as best next move.
[#569](https://github.com/CyrilB1531/lodestar/issues/569) takes it, and two questions had to be
settled before any code: what the incumbent is, and which Python library may serve as the oracle.

[ADR 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s amendment, restated
by #442's closing note, requires an incumbent's **exported surface** to be read through a
`MetadataLoadContext` rather than taken from its README. [Decision 0096](0096-ordinary-least-squares-earns-its-own-package.md)
is why that matters: reading `Accord.Statistics` that way **replaced** #566's gap claim instead of
confirming it.

## The reading, and what it found

There was no assembly to load. A NuGet capability search on **2026-09-09** returned:

| query | packages |
| --- | --- |
| `survival analysis` | **0** |
| `kaplan meier` | **0** |

The only domain in #442's survey where every search comes back empty. And the sharing claim held
too: nothing in the packages shipped before this one is a survival primitive.

**Where there is no assembly, the protocol is discharged by recording the searches** — their
queries, their date, their counts — and not by claiming a `MetadataLoadContext` run that had nothing
to open. Per #442's own instruction, that absence **is** the benchmark section, and
the benchmark harness's own README says so in its section 20 rather than leaving it blank.

## Decision

**`lifelines` 0.30.3 is the oracle. `scikit-survival` is refused.**

`scikit-survival` is the nearest reference in any language, and its PyPI `license_expression` is
**GPL-3.0-or-later**. [Decision 0003](0003-provenance-and-licensing.md)'s rule is unambiguous —
permissive references only, never copyleft — and it is the rule that already excluded `abydos` and
removed `python-Levenshtein`. The refusal is recorded here so nobody rediscovers it mid-lot, and so
that the reason is on file as *licence*, not *unavailability*.

`lifelines` is **MIT**, read from the wheel rather than from the PyPI classifier. Its transitive
graph was read the same way, because two of its dependencies report a licence pip cannot parse:

| package | what pip reports | what the wheel carries |
| --- | --- | --- |
| `lifelines` 0.30.3 | MIT | MIT |
| `autograd` 1.9.1 | *(blank)*, `License: UNKNOWN` | `License-Expression: MIT` |
| `autograd-gamma` 0.4.2 | `License: UNKNOWN` | MIT, in `LICENSE` |
| `formulaic` 1.2.2 | *(blank)* | `License-Expression: MIT`, MIT classifier |
| `interface-meta` 2.0.1 | MIT | MIT |

That is the trap [decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md) named
when it read `doublemetaphone`'s licence out of its own wheel: a `License: UNKNOWN` in legacy
metadata is not a licence, and is not a refusal either.

## Two costs the dependency carried, both measured

**`pandas` drops a major version.** `lifelines` requires `pandas<3.0`, and the lock pinned
`pandas==3.0.5`, which arrived with `statsmodels` in #566. The resolver moves it to **2.3.3**.
CONTRIBUTING's rule is to regenerate the corpora and confirm they have not moved: **all 127 agree**,
`stats_ols.json` — statsmodels' own output over that pandas — byte for byte included.

**`autograd-gamma` 0.5.0 publishes no wheel.** `pip-compile` selects it, and `--only-binary :all:`
then refuses the whole install at every one of this repository's install sites. It is pinned to
**0.4.2** in `tools/requirements.txt`, the newest that ships a wheel, which leaves the rest of the
resolved graph unchanged.

`lifelines`' own Python floor is 3.11, **below** the 3.12
[decision 0065](0065-the-oracle-generators-floor-is-the-ci-interpreter.md) sets, so it does not
raise ours — checked before adding it rather than after a generator failed.

## Consequences

- `Lodestar.Survival` 0.1.0 ships `KaplanMeier`, `NelsonAalen` and `LogRank`, right-censored only.
  Left truncation, interval censoring, Cox regression and the accelerated-failure-time models are
  each their own lot.
- The benchmark project compares the three estimators against each other and against
  input shape, because there is no package to race. `tools/check_bench_map.py`'s `CLASS_DIRS` gains
  the directory, which is what makes the nightly obliged to run it (#586).
- If a .NET survival package appears later, this record is what a reader compares it against: the
  searches are dated, so the claim can be re-run rather than argued about.
