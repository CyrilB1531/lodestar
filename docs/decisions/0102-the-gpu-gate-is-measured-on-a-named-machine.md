# 0102 — The GPU gate is measured on a named machine, and the kernels are ordered by what they need

**Status:** accepted · **Date:** 2026-09-10

## Context

[#444](https://github.com/CyrilB1531/lodestar/issues/444) puts one sentence in front of every
kernel it lists:

> **No kernel ships without a measured 5–10× gain at realistic sizes, transfers included, baseline
> our own SIMD path.**

That is the right gate and it has no runner. GitHub Actions hosts no GPU, so the number cannot come
from CI, and a gate that cannot be evaluated is a gate that gets waived. This record says where it
*is* evaluated, and in what order the six kernels are attempted, because two of them are blocked on
code that does not exist.

## Decision 1 — the gate is measured locally, on a machine named beside the number

The repository already splits this cleanly and nothing new is needed:

| document | its subject |
| --- | --- |
| `bench/README.md` | **how to measure** — the harness, the corpus, the commands |
| `docs/guides/performance.md` | **what was measured** — every number, with its machine and its window |

A GPU number lands the same way a nightly number does: the harness and the protocol in
`bench/README.md`, the figure and the hardware in `docs/guides/performance.md`. **The machine's
specification is recorded when the first measurement is taken, not now** — inventing it here would
put a fact in a decision record that nobody has observed, which is the failure
[0059](0059-phase-0-verifications-two-confirmed-voids-do-not-survive-nuget.md) was written about.

**The protocol, which is what a maintainer can follow without a GPU present:**

1. **The baseline is the SIMD path in this repository**, on the same machine, in the same process —
   never a scalar loop and never a figure taken elsewhere. `[Benchmark(Baseline = true)]` on the
   CPU row, so BenchmarkDotNet computes the ratio rather than a reader.
2. **Transfers are inside the measured region.** Host-to-device, kernel, device-to-host. A row that
   times the kernel alone is not this gate; it may exist *beside* the gate row, labelled, to show
   where the time goes — the split [#480](https://github.com/CyrilB1531/lodestar/issues/480) used
   for the `.npy` ingest.
3. **Warm-up is explicit and separate.** ILGPU compiles a kernel on first launch, so the first
   call measures a compiler. `[GlobalSetup]` launches each kernel once on the real corpus before
   any iteration is timed, and the bench section says it does.
4. **Sizes bracket the crossing point.** At least one size where the CPU path wins and one where
   the GPU path does, so the report shows *where* the gain starts rather than asserting it. A
   single flattering size is not a measurement.
5. **A failed gate is published, not deleted.** A kernel that reaches 2× is a row in
   `performance.md` and a kernel that does not ship — which is worth more to the next reader than
   an absence.

`CPU accelerator forced in CI` stays in #444's scope and is not the gate: it proves the kernels are
*correct* where there is no GPU, which is a different question from whether they are *faster*
where there is one. Correctness runs everywhere and blocks a merge; the gain is measured on the
named machine and blocks a ship.

## Decision 2 — the six kernels are ordered by what they need, not by what they promise

| # | kernel | needs | state |
| --- | --- | --- | --- |
| 1 | **Tiled cosine + top-k** | `Lodestar.Abstractions` (published), a dense block | **ready** |
| 2 | CSR SpMM | `CsrMatrix`, published in `Lodestar.Abstractions` 0.1.1 | ready |
| 3 | Bit-parallel Myers | the existing Myers path as its baseline | ready |
| 4 | Device-resident types | kernels 1–3, since chainability is only visible across two of them | after 1–3 |
| 5 | MinHash | an algorithm this repository **does not have** | **blocked on [#602](https://github.com/CyrilB1531/lodestar/issues/602)** |
| 6 | CPU accelerator in CI | kernel 1 | with kernel 1 |

**Tiled cosine + top-k is first**, and not only because it is the simplest. It is the one workload
that #444's own survey marks ✅ without qualification — *"resident matrix swept by millions of
queries"* — so it is where the gate is most likely to be met, and a first kernel that fails the
gate teaches nothing about the harness.

**The MinHash kernel is blocked and the block is not new.** #444 names it as depending on
[#440](https://github.com/CyrilB1531/lodestar/issues/440) lot 1, which closed without being built.
[#602](https://github.com/CyrilB1531/lodestar/issues/602) reopens that on the ground lot 1's own
reading did not weigh — all three incumbents are unavailable on `netstandard2.0` — and it must land
before a kernel can be written for it. This is the third time this repository has met the shape:
[#573](https://github.com/CyrilB1531/lodestar/issues/573) before
[#572](https://github.com/CyrilB1531/lodestar/issues/572), and now #602 before #444's kernel 5.

**Device-resident types come after three kernels, not before one.** #444 is right that per-call
transfer makes a GPU package slower than the SIMD path it replaced — which is an argument for
`DeviceCsrMatrix` and `DeviceEmbeddingMatrix` existing, not for designing them first. Chainability
is a claim about two operations sharing a residency, and it cannot be measured until two operations
exist.

## Consequences

- `bench/Lodestar.Gpu.Benchmarks`, its own project, registered in `bench/bench-map.json`. The
  nightly runs it on a runner with no GPU, so it must select the CPU accelerator there and say so
  in its output rather than reporting a GPU figure taken from nothing.
- Every kernel PR carries its own `performance.md` rows with the machine named, per
  `CONTRIBUTING.md`'s existing rule that a `perf/` pull request carries before/after numbers and
  names the machine.
- Kernel 5 waits on #602. Kernels 1–3 do not wait on anything.
