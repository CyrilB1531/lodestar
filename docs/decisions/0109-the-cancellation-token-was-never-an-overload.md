---
status: accepted
supersedes: []
amends: ["0107"]
applies: ["0074"]
---
# 0109 — The cancellation token was never an overload, and 0107 said it was

**Status:** accepted · **Date:** 2026-09-11

## Context

[Decision 0107](0107-xunit-v3-arrives-whole-and-takes-the-coverage-format-with-it.md) moved the
suites to xunit v3 and suppressed `xUnit1051` — *pass `TestContext.Current.CancellationToken` to a
call that accepts one* — for the whole `tests/` area. It gave a reason, in its evidence section
and again in a consequence:

> Eight of those are `PersistenceOverloadTests`, whose whole subject is that the overload
> *without* a token exists and behaves like the one with it, so a blanket fix deletes what they
> assert.

**That is false**, and it was asserted without reading the file. `PersistenceOverloadTests` says
what it is about in its own summary:

> Every vectorizer ships four ways to persist — stream, file, and an async counterpart of each —
> and the round-trip suite exercised only the stream ones.

The overloads under test are **stream against file** and **synchronous against asynchronous**. The
eight sites are `SaveAsync(stream)` and `LoadAsync(stream)` calls, and the token is not an overload
at all: measured across `src/`, **20** methods take `CancellationToken cancellationToken = default`
and **zero** take it as a separate mandatory overload. Passing it calls the same method by the same
path and asserts exactly the same thing.

So the suppression had no surviving justification, and the follow-up
[#647](https://github.com/CyrilB1531/lodestar/issues/647) was scoped around a fiction: it was
written to fix 46 of 54 sites and leave 8 with a `#pragma` each.

## Decision

**All 54 sites take the token, and `xUnit1051` comes out of `tests/Directory.Build.props`.** Not
one site needed a `#pragma`: the build is clean with the rule on, and warnings are errors here.
6942 tests across 32 assemblies before and after, and the diff is 54 insertions against 54
deletions — one line each, nothing else moved.

The argument is named rather than positional — `cancellationToken: TestContext.Current.CancellationToken`
— because most of these methods take an options parameter before it, so the positional form would
bind the token to `ArtifactLoadOptions?` or `EncodingOptions?` and fail to compile. That is worth
recording as the shape a later site should copy, not rediscover.

## Why it happened, which is the part worth keeping

The claim came from the file's **name**. `PersistenceOverloadTests` reads like a suite about
overloads, the rule was about an overload-shaped thing, and the two were joined without opening
the file — whose first twelve lines say otherwise.

[Decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) exists to stop
exactly this, for external libraries: read the exported surface, never the README. The protocol was
followed for `datasketch`, `Microsoft.ML.TimeSeries`, `Accord.Statistics` and a dozen others, and
not applied to a file in this repository — where it is cheaper, not more expensive.

An ADR is a record of evidence. One stating an unread file's contents as measured is worse than one
stating nothing, because the next reader has no way to tell the two apart. This record is the
correction; 0107's decision — xunit v3 whole, coverage as Visual Studio XML — is untouched and
stands.

## Consequences

- `xUnit1051` is enforced. A new call that accepts a token and is given none fails the build, which
  is where the rule is worth having.
- 0107's evidence section is wrong where it names `PersistenceOverloadTests`, and right everywhere
  else — the churn measurement, the `global.json` constraint, the coverage format and the two
  pinned-name traps were all measured. `docs/decisions/index.yaml` carries the `amends` edge so a
  reader of 0107 reaches this.
- The habit this repository already has for external surfaces — read it, do not infer it — applies
  to its own files. Nothing enforces that and nothing can; it is written here so the next claim
  about a test file's subject is checked against its summary.
