---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0096", "0105"]
---
# 0110 — The surveyor is a file-based app, and it names which members it counted

**Status:** accepted · **Date:** 2026-09-11

## Context

[Decision 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md) requires a gap
claim to be checked against an incumbent's **exported surface**, read through a
`MetadataLoadContext` — never against its README. Five decisions have run that protocol: 0074,
[0096](0096-ordinary-least-squares-earns-its-own-package.md),
[0099](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md), 0100, and
the pair [0104](0104-generalized-linear-models-are-written-natively.md) and
[0105](0105-the-time-series-forecast-is-delegated-and-the-diagnostics-are-the-gap.md).

Every run was a throwaway console project written again from scratch
([#619](https://github.com/CyrilB1531/lodestar/issues/619)). The numbers reached the records and
the means of producing them did not — 0096 reports "336 exported types, 5 333 members" with no
command a reviewer can re-run — so the traps were rediscovered each time rather than paid once.

## Decision

**`tools/survey.cs`, a file-based app.** No `.csproj`, no entry in `Lodestar.slnx`, nothing for CI
to build:

```bash
dotnet run tools/survey.cs -- Microsoft.ML.TimeSeries 5.0.0 'Arima|Acf|Stationar'
```

The alternative was a project under `tools/`, and it costs more than it looks. `tools/` is
otherwise Python and the pre-commit hook iterates it as scripts; a `.csproj` there is either in the
solution — where `dotnet build` and `dotnet test` reach it for no reason, and every analyzer bump
has one more thing to break — or outside it, which is the arrangement
[#649](https://github.com/CyrilB1531/lodestar/issues/649) has just finished paying for on a
benchmark project nothing compiled. A file whose project file is three directives at the top of it
avoids the choice.

The trim, single-file and AOT analysers are disabled there, by `#:property`. Reading an arbitrary
assembly's surface cannot satisfy them by construction — naming the types it will reflect over is
the one thing this must not do — so the suppression is the program rather than four expressions in
it.

**Publishing before reading is the point, not an implementation detail.** A bare `lib/*.dll`
cannot be opened: `GetExportedTypes` needs every assembly its signatures mention. That trap has
been paid twice, on `Mosaik.Core` (0074) and on `Microsoft.ML.TimeSeries`, which drags
`Microsoft.ML`, an MKL redistributable and `Newtonsoft.Json` behind it. Resolving the closure with
the SDK is what makes the reading reproducible rather than lucky.

## The counting basis, and what measuring it found

The type counts across five records reproduce exactly. The member counts did not, and #619 says
why it thought so:

> the member count depends on whether property accessors are counted

**That is not what happened.** Measured with the tool, on `MathNet.Numerics` 5.0.0 — whose 336
types 0096 reproduces on the nose:

| basis | members |
| --- | --- |
| declared, accessors and operators excluded, constructors kept | 5 707 |
| declared, everything | 6 938 |
| declared, accessors, operators **and constructors** excluded | **5 335** |

0096's "5 333" is the third line, two apart on a different SDK — so the difference was
constructors *and* special names, not accessors alone. And 0105's "124 members" for
`Microsoft.ML.TimeSeries` is the **first** line, reproducing exactly.

So two records used two different bases and neither said which. All three are printed, and **a new
record quotes the first**: declared public members with accessors and operators excluded and
constructors kept, because a caller calls a constructor and does not call `get_Length`.

## Consequences

- `tools/README.md` carries the command, the three counts and the two reproductions above, and
  `test_readme_covers_the_tools.py` now globs `*.cs` beside `*.py` so the next non-Python tool
  cannot arrive undocumented.
- **Older figures stay as they are.** 0096 and 0105 are immutable and both are correct on their own
  basis; this record is where a reader learns which. Nothing is restated into them.
- A package that installs no assembly is reported with the closure's actual contents rather than as
  a blank — the finding 0104 recorded for `cs-glm` 1.0.1, which this reproduces as an NU1202 naming
  `net10.0`.
- The reading is now cheap enough that a claim can be re-checked rather than trusted, which is what
  0074 asks for and what five ad-hoc runs made expensive. [#616](https://github.com/CyrilB1531/lodestar/issues/616)
  and [#617](https://github.com/CyrilB1531/lodestar/issues/617) are the next two to need it.
