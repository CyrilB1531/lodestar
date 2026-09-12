---
status: accepted
supersedes: []
amends: []
applies: ["0015"]
---
# 0112 — An analyzer rule below warning is raised where it has escaped, not everywhere

**Status:** accepted · **Date:** 2026-09-12 · **Applies:** [`0015`](0015-sonar-rules-in-the-build.md)

## Context

[Decision 0015](0015-sonar-rules-in-the-build.md) put the analyzer findings in the build rather
than only at the quality gate, so that a finding fails **on the machine that wrote the code** and
not three minutes after the push. `.globalconfig` at the repository root carries that out for the
Sonar rules the analyzer package ships disabled, and `TreatWarningsAsErrors` in the root
`Directory.Build.props` is the lever that turns any of them into a failure.

A class of finding slips through both, and [#690](https://github.com/CyrilB1531/lodestar/issues/690)
is what measured it: **ten `xUnit2033` findings were sitting on `main`**, in six files across four
test projects, none of them ever seen by a contributor.

Two mechanisms had to miss for that, and neither was bypassed:

- `xUnit2033` is **Info** severity in `xunit.analyzers`. `TreatWarningsAsErrors` acts on warnings;
  an Info diagnostic is not one, so `dotnet build` never mentions it.
- SonarCloud imports it as an **external Roslyn issue at INFO**, and the quality gate this
  repository runs is on new code. A finding that does not move `new_violations` lands on `main`
  and stays there.

So the build was silent by design and the gate was silent by configuration. What was missing was a
decision about whether this class of finding is worth failing on at all.

The rule itself is not cosmetic. `Assert.Single(x)` already returns the element it proved was the
only one; re-deriving it with `x[0]` indexes a second time, and the two lines can drift — an edit
that changes the assertion to `Assert.Equal(2, …)` leaves the `[0]` reading an element whose
singularity is no longer asserted.

## Decision

**`xUnit2033` is raised to `warning` for `tests/`, and nothing else is raised.**

The mechanism is `tests/analyzers.globalconfig`, a hand-maintained global analyzer config named in
`tests/Directory.Build.props` through `GlobalAnalyzerConfigFiles`. `warning`, not `error`:
`TreatWarningsAsErrors` already decides whether a finding stops the build, and the root
`.globalconfig`'s own header states the reason for keeping one lever rather than two.

**The file cannot be called `.globalconfig`.** `DiscoverGlobalAnalyzerConfigFiles` looks for a file
of exactly that name beside each project, and the root one carrying it is **generated** by
`tools/generate_sonar_globalconfig.py` and diffed by CI with `--check`. A hand-added entry there
would be erased by the next regeneration and fail the diff on the way.

**Scoped to `tests/` because the rule is.** `xunit.analyzers` only runs where xunit is referenced,
so a root-level entry would be a rule that can never fire outside one directory, written where a
reader looks for repository-wide policy.

## Options that lost

- **Raise every Info-severity analyzer diagnostic.** Symmetrical, and wrong. A rule ships at Info
  because its author judged it advisory, and promoting the category wholesale imports that judgment
  in reverse for rules nobody here has read. It also fails at the wrong time: the first unrelated
  analyzer upgrade turns an unknown number of new Info rules into build failures on a morning
  nobody chose, which is the same objection [decision 0019](0019-the-net-analysers-run-in-the-build-too.md)
  raised against letting `AnalysisLevel` float.
- **Suppress it instead, with `NoWarn`.** Consistent with how `xUnit1051` is handled next door, and
  it would make the ten findings disappear from SonarCloud too. Refused because that one has a
  reason this one does not: `xUnit1051` fires at 54 sites of which eight assert the very thing the
  rule wants changed. `xUnit2033` has ten sites and no defensible one.
- **Fix the ten and change nothing else.** The honest objection to this decision — ten findings is
  not an emergency, and a repository can carry advisory findings. Refused on the evidence rather
  than on principle: nobody saw these for as long as they existed, so "we will notice next time" is
  the claim that already failed.
- **A root `.editorconfig`.** It would work for diagnostic severities, and it is the mechanism most
  readers expect. Refused because `dotnet format --verify-no-changes` is a gate here and reads
  `.editorconfig` for style as well as severity, so introducing one puts a formatting surface under
  a gate to solve a severity problem.

## Consequences

- The next `Assert.Single` written this way **fails the build of the project that contains it**,
  which is asserted rather than assumed: reintroducing one of the ten produced
  `error xUnit2033` at `DeduplicatorTests.cs(62,9)`, and removing it again returned the solution to
  zero errors.
- A second analyzer-config file now exists, and the two have different natures: the root one is
  generated and must not be edited, this one is written by hand and has no generator. Both say so
  in their own first lines.
- The rule this sets is **narrow on purpose**: a diagnostic is raised here when it has been
  measured escaping, not when it might. The next one to escape gets its own line and its own
  sentence in this record's successor, not a category promotion.
- SonarCloud's `main` branch goes to zero open issues once the ten sites and
  [#691](https://github.com/CyrilB1531/lodestar/issues/691) land, which makes the next arrival
  visible rather than the eleventh of a crowd.
