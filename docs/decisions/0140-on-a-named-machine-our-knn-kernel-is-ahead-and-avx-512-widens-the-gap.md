---
status: accepted
supersedes: []
amends: ["0060"]
applies: []
---
# 0140 — On a named machine our kNN kernel is ahead, and AVX-512 widens the gap

**Status:** accepted · **Date:** 2026-09-16 · **Amends:** [`0060`](0060-tensorprimitives-beats-our-kernel-and-the-knn-is-still-not-redundant.md)

## Context

[Decision 0060](0060-tensorprimitives-beats-our-kernel-and-the-knn-is-still-not-redundant.md) read
`tensor-primitives` on a hosted runner — an AVX2 EPYC, .NET 10.0.11 — and concluded that
`TensorPrimitives` beats our kernel on the kNN's own access pattern, by 1.09–1.23× on the dot. It
refused a container reading of 0.27× as inverted by the runner. [#754](https://github.com/CyrilB1531/lodestar/issues/754)
found a named machine reproducing the refused reading, and proposed one hypothesis: that
`TensorPrimitives` takes a 512-bit path where the hardware reports it, and that its setup costs
more than it saves at 384 floats per call.

## What was measured

`tensor-primitives` on an AMD Ryzen 7 8700G (AVX-512), Ubuntu 26.04.1, .NET 10.0.12, pinned to one
core, three conditions interleaved over five runs of nine. Median of the five medians, in ms, over
10,000 × 384 floats; ratios above 1 mean `TensorPrimitives` is faster.

| row | `Vector512` on (default) | `Vector512` off, AVX-512 kept | AVX-512 off |
| --- | ---: | ---: | ---: |
| `ours_dot_knn` | 0.863 | 1.232 | 0.955 |
| `tp_dot_knn` | 6.222 | 3.860 | 3.946 |
| `ours_cosine_knn` | 1.803 | 2.055 | 1.814 |
| `tp_cosine_knn` | 10.795 | 6.130 | 6.363 |
| `ours_one_sweep` | 0.563 | 0.545 | 0.351 |
| `tp_one_sweep` | 0.456 | 0.387 | 0.369 |
| `index_search` | 3.734 | 3.787 | 3.692 |
| **dot ratio** | **0.14** | **0.32** | **0.24** |
| **cosine ratio** | **0.17** | **0.34** | **0.29** |
| one sweep ratio | 1.23 | 1.41 | 0.95 |

The second column is `DOTNET_PreferredVectorBitWidth=256` and the third `DOTNET_EnableAVX512=0`.
**The knob #754 named, `DOTNET_EnableAVX512F=0`, does nothing on .NET 10**: the diagnostic now
prints `Vector512.IsHardwareAccelerated`, and it stayed `True` under that variable. Agreement holds
before timing in every run: 1.192e-7 on the dot, 5.960e-8 on the cosine.

## Decision

**On this machine our kernel is ahead of `TensorPrimitives` on the kNN pattern, whatever the vector
width.** 0060's sentence *"there is no access pattern here on which we win"* holds for the runner it
measured and does not hold here.

**The hypothesis is half right.** Turning the 512-bit path off cuts `TensorPrimitives`' dot from
6.22 ms to 3.86–3.95 ms and removes the bimodal runs its default showed (one of five at 3.30), so
AVX-512 does cost it at 384 floats per call. **But with that path off it is still 3–4× behind**,
while one long sweep is at parity. What decides the ratio here is the cost per call, which the
512-bit path amplifies rather than causes — and the runner's AVX2 reading is therefore not explained
by the instruction set alone.

**0060's second half stands, and stronger.** The dot is 23–33% of `index_search` here, against 49%
on the runner; top-k selection is most of a query on both machines, and delegating the kernel is no
longer even a candidate saving.

## What was refused

**Declaring either machine the true one.** Two machines disagree in direction, and the cause is
narrowed rather than found. The kernel stays as it is on both readings: behind by at most 23% on one,
ahead by 3–7× on the other.

**Publishing the runner's absolutes beside these.** They are different hardware on different
runtimes, and only ratios taken in one window are comparable.

## Consequences

- **Delegating [`VectorMath.Dot`](../reference/embeddings/search/vectormath-dot.md) to `TensorPrimitives` is off the table** until a machine shows it
  ahead by more than this one shows it behind.
- **A kNN performance lot still starts at top-k**, as 0060 said.
- **`bench/README.md` §14 names the .NET 10 knobs**, so the next reading of this question does not
  set a variable the runtime ignores.
