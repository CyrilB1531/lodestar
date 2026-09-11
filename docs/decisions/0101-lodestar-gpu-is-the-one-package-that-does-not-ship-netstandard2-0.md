---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075"]
---
# 0101 — `Lodestar.Gpu` is the one package that does not ship `netstandard2.0`

**Status:** accepted · **Date:** 2026-09-10

## Context

[#444](https://github.com/CyrilB1531/lodestar/issues/444) opens Phase 6, a GPU package over ILGPU.
`CLAUDE.md`'s first cross-cutting fact is the rule it breaks:

> Everything ships `net10.0;netstandard2.0` in a single package. `netstandard2.0` reaches
> equivalent behaviour through conditional compilation, **never a reduced API**.

Fifteen packages honour that today, and the rule is not decoration: the `*.NetStandard.Tests`
mirrors exist so the assemblies shipped to .NET Framework, Mono and Unity are *executed* rather
than merely compiled, and [#529](https://github.com/CyrilB1531/lodestar/issues/529) is what
happens when that pin slips.

## The reading

[ADR 0074](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s protocol, applied to
the dependency rather than to an incumbent. `ILGPU` **1.5.3**, read from the package on 2026-09-10:

| | |
| --- | --- |
| `lib/` | `net471`, `net5.0`, `net6.0`, `net7.0`, **`netstandard2.1`** |
| `netstandard2.0` asset | **none** |
| newest modern asset | `net7.0` — consumable by `net10.0`, but ILGPU itself targets nothing newer |
| licence | **University of Illinois/NCSA Open Source License**, read from `LICENSE.txt` *inside the package* |
| declared SPDX expression | **none** — the nuspec carries `<license>LICENSE.txt</license>` and a deprecated `licenseUrl` |
| 1.5.1 → 1.5.2 → 1.5.3 | 2023-09-13, 2025-03-06, 2025-07-12 |

Two of those rows are findings rather than facts in passing.

**The licence had to be read from the artefact.** No SPDX expression is declared, which is the trap
[decision 0075](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md) named and
[0099](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md) met again:
metadata silence is not a licence. NCSA is permissive — a BSD-family grant with a no-endorsement
clause — so [decision 0003](0003-provenance-and-licensing.md) is satisfied. Had `LICENSE.txt` said
GPL, this ADR would be a refusal instead.

**`netstandard2.0` is not a gap to polyfill, it is absent upstream.** The usual order — PolySharp,
then a framework-conditional package reference, then a hand-written fallback — closes *language and
BCL* gaps. None of it produces a CUDA or OpenCL accelerator. There is nothing to write.

## Decision

**`Lodestar.Gpu` targets `net10.0` alone, and is the only package in this repository that does not
ship `netstandard2.0`.**

`net10.0` rather than `net8.0`, which #444 proposed: ILGPU's newest asset is `net7.0` and is
consumed identically by either, so a second target framework would add a matrix dimension and test
nothing ILGPU distinguishes. One target, the repository's own.

### Why this does not weaken the rule

The rule protects a promise to a **consumer**: take `Lodestar.Text` on .NET Framework and the API
is the same API. `Lodestar.Gpu` makes no such promise because it cannot be taken there at all —
and a package absent on a framework breaks no promise about its shape on that framework. The
failure mode the rule prevents is *a package that ships to `netstandard2.0` with less in it*, which
is the opposite of this.

**`Lodestar.Gpu` is therefore additive and never a dependency of a core package.** No `src/`
project may reference it: a core package taking an edge to a `net10.0`-only library would make
`netstandard2.0` unreachable *for that package*, which is the breach this decision is careful not
to be. The edges run the other way — `Lodestar.Gpu` → `Lodestar.Abstractions`, and later
`→ Lodestar.Embeddings` and `→ Lodestar.Text` — so the SIMD path stays the only path a
`netstandard2.0` caller has, and it stays complete.

### What nothing will notice, and therefore has to be written down

`tools/check_netstandard_guards.py` iterates the mirrors that **exist**; it does not require one
per package. So `Lodestar.Gpu` arriving with no mirror is silence, not a finding — indistinguishable
from a mirror somebody forgot. This record is what makes the absence deliberate, and
`CLAUDE.md`'s package table gains a tier column entry saying so in the same change.

## Consequences

- A sixteenth package, `net10.0` only, satellite tier by
  [0076](0076-a-core-package-carries-no-external-dependency.md) and interop-named by
  [0089](0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md)'s reasoning, though
  it adapts hardware rather than another library's types.
- No `*.NetStandard.Tests` mirror, deliberately.
- `THIRD-PARTY-NOTICES.md` gains ILGPU with **NCSA** as read from `LICENSE.txt`, not from the
  nuspec, and a note that the package declares no expression.
- A kernel's baseline is the SIMD path on the same machine, never the scalar loop —
  [0102](0102-the-gpu-gate-is-measured-on-a-named-machine.md) has the protocol.
