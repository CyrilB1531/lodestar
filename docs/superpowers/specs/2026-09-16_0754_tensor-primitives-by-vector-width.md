# 0754 — `TensorPrimitives` against our kNN kernel, by vector width

**Status:** **retrospective** — written 2026-09-16, after the measurement it records.

Issue: [#754](https://github.com/CyrilB1531/lodestar/issues/754). Reading:
[decision 0060](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0060-tensorprimitives-beats-our-kernel-and-the-knn-is-still-not-redundant.md).

## Problem

0060 found `TensorPrimitives` 1.09–1.23× ahead on the kNN dot on an AVX2 hosted runner. A named
AVX-512 machine read the opposite, 0.26×. The issue's hypothesis: the 512-bit path costs more than
it saves at 384 floats per call.

## Measured

`tensor-primitives`, pinned to one core, three conditions interleaved over five runs of nine:
default, `DOTNET_PreferredVectorBitWidth=256`, `DOTNET_EnableAVX512=0`. The issue's
`DOTNET_EnableAVX512F=0` was tried first and ignored by .NET 10, which is why the diagnostic now
prints `Vector512.IsHardwareAccelerated`.

| | `Vector512` on | `Vector512` off | AVX-512 off |
| --- | ---: | ---: | ---: |
| dot ratio | 0.14 | 0.32 | 0.24 |
| cosine ratio | 0.17 | 0.34 | 0.29 |

## Conclusion

The 512-bit path halves `TensorPrimitives`' speed here, but it is 3–4× behind without it: the cost
per call decides the direction and the width only its size. [Decision 0140](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0140-on-a-named-machine-our-knn-kernel-is-ahead-and-avx-512-widens-the-gap.md)
amends 0060, and the rows are in `docs/guides/performance.md`.

## Rejected

- **Delegating `VectorMath.Dot`.** Behind by at most 23% on one machine, ahead 3–7× on another.
- **Re-running on the hosted runner to settle it.** Its hardware changes night to night, which is
  why #679 moved these comparisons to a named machine.
