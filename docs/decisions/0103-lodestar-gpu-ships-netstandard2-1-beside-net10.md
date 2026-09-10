# 0103 — `Lodestar.Gpu` ships `netstandard2.1` beside `net10.0`

**Status:** accepted · **Date:** 2026-09-10 · **Amends:** [0101](0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md)

## Context

[Decision 0101](0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md) gave
`Lodestar.Gpu` a single target framework. It argued two things, read from the package: that ILGPU
1.5.3 publishes **no `netstandard2.0` asset**, so that target is absent upstream rather than a gap
a polyfill could close; and that `net10.0` was preferable to the `net8.0`
[#444](https://github.com/CyrilB1531/lodestar/issues/444) proposed, because ILGPU's newest asset is
`net7.0` and both consume it identically.

Both remain true. **Neither is an argument about `netstandard2.1`, which ILGPU also publishes**, and
0101 never weighed it — the record went from "not `netstandard2.0`" straight to "`net10.0` alone"
with nothing in between. This amends that step.

## What the omission cost

`net10.0` alone excludes every consumer that is not on .NET 10. That includes **.NET 8, which is
LTS until November 2026**, and Mono and Unity. ILGPU ships `net6.0` and `net7.0` assets precisely
to serve those callers, and a package wrapping it that reaches none of them narrows the dependency
rather than adapting it.

0101's reasoning for `net10.0` over `net8.0` was about **testing** — a second moniker ILGPU does
not distinguish adds a matrix dimension and proves nothing. That is still right, and it does not
transfer: `netstandard2.1` is not another flavour of the same reach, it is a contract ILGPU
declares and a strictly wider audience.

## The measurement

Adding `netstandard2.1` produced **50 compile errors, every one of them the same cause**:
`ArgumentNullException.ThrowIfNull` and `ArgumentOutOfRangeException.ThrowIfLessThan`, static
helpers that exist on .NET 6 and 8 and not on `netstandard2.1`. No algorithm, no ILGPU API, no
kernel.

`src/Shared/Guard.cs` already answers exactly that, and has since the first package: `Guard.NotNull`
and `Guard.NotLessThan` carry the one `#if` so no call site does. The other fifteen packages use
them **because** they target `netstandard2.0`. Swapping the call sites took the build to zero
errors, and it leaves `Lodestar.Gpu` consistent with its siblings rather than the only package
reaching for BCL helpers the rest cannot.

## Decision

**`Lodestar.Gpu` targets `net10.0;netstandard2.1`.**

`net10.0` stays, so a caller on the newest runtime gets the assets compiled against it.
`netstandard2.1` is the floor ILGPU itself declares, so the package reaches what its dependency
reaches and no further.

### The mirror is the half that makes this honest

A second target framework that nothing executes is the failure
[#529](https://github.com/CyrilB1531/lodestar/issues/529) was about: compile-verified, never run,
every test green and half of them proving nothing. So `tests/Lodestar.Gpu.NetStandard.Tests`
replays the whole suite against the `netstandard2.1` build — **the first mirror in this repository
pinned to 2.1 rather than 2.0** — and its `NetStandardAssemblyGuardTests` asserts the loaded
assembly's `TargetFrameworkAttribute` is `.NETStandard,Version=v2.1`.

`tools/check_netstandard_guards.py` was taught the difference rather than given an exception. It
now reads the contract **per package** from a `MIRRORED` table instead of a constant, checks that
the library actually declares the framework its mirror claims to replay, and reports a mirror
pinning *nothing* as a finding. That last one matters: before the change, a 2.1 mirror passed the
2.0 guard vacuously — `Lodestar.Gpu` has no Lodestar dependency to pin, so there was nothing to
fail on, and the guard printed "16 netstandard2.0 mirrors" while one of them was not.

## What 0101 keeps

Everything except the framework list.

- **No `src/` project may reference `Lodestar.Gpu`.** A wider target makes this easier to forget,
  not less necessary: the SIMD path is still the only path a `netstandard2.0` caller has.
- Satellite tier by [0076](0076-a-core-package-carries-no-external-dependency.md), ILGPU's
  **NCSA** licence read from `LICENSE.txt` inside the package because no SPDX expression is
  declared, and the device-selection and measurement rules in
  [0102](0102-the-gpu-gate-is-measured-on-a-named-machine.md).
- `netstandard2.0` is still absent upstream and still not offered.

## Consequences

- `tools/check_nuspec_dependencies.py` gains a `.NETStandard2.1` group for this package alone.
- `CLAUDE.md`'s table says two frameworks rather than one.
- The repository now has **sixteen** mirrors, fifteen on 2.0 and one on 2.1.
