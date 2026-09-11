---
status: accepted
supersedes: []
amends: []
applies: ["0074"]
---
# 0093 — An exact tie between two centres is not part of k-means parity

**Status:** accepted · **Date:** 2026-09-09

## Context

[`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) replays
`sklearn.cluster.KMeans(algorithm="lloyd")` entry by entry: the starting centres are an input, as
[decision 0072](0072-omega-is-an-input-not-a-seed.md) made Ω one, so Lloyd's algorithm is an
ordinary parity target rather than a distribution to compare.

Lloyd's assignment step gives each sample the nearest centre. **When two centres are exactly the
same distance away, which one wins is a choice**, and the reference's choice was measured on two
configurations of five and four samples. It went both ways:

| samples | starting centres | the tie | scikit-learn assigns |
| --- | --- | --- | --- |
| `(0,0) (0,1) (10,10) (10,11) (5,5)` | `(0,0) (1,1) (2,2)` | `(0,1)` is at distance 1 from both `(0,0)` and `(1,1)` | the **second** centre |
| `(1,1) ×3, (9,9)` | `(1,1) (1,1)` | every sample is equidistant from two identical centres | the **first** centre |

Both are exact ties in IEEE 754, in the plain squared distance and in the
`‖c‖² − 2·x·c` form the reference computes. Neither `<` nor `<=` reproduces both: keeping the first
minimum matches the second row and fails the first, keeping the last matches the first row and
fails the second. The choice is made inside a Cython kernel driven by a BLAS product, and it is not
reachable from the installed package — only the compiled extension ships in the wheel.

## Decision

**A sample exactly equidistant from two or more centres takes the lowest-indexed one**, which is
`numpy.argmin`'s rule and the one a reader guesses. It is implemented as a strict `<` over the
plain squared distance.

**That tie is not part of the parity promise, and no frozen case turns on one.** The oracle corpus
was written with two fixtures that hinged on an exact tie; both were replaced with centres that are
close to the samples but not equidistant from any of them, so every assertion in
`tests/oracles/cluster_kmeans.json` rests on a comparison the arithmetic decides rather than on a
convention.

## Why not reproduce it

**Because it cannot be read, only guessed at.** ADR 0074's rule for an incumbent is to read what it
exports; the same standard applied here says the relocation and assignment kernels are compiled
Cython reached from compiled Cython, so a rule inferred from two observations would be a guess
dressed as parity. Two more measurements could send it a third way, and the corpus would then be
freezing this machine's BLAS rather than the algorithm.

**And because the tie is arbitrary in the algorithm itself.** Lloyd converges to the same partition
from either choice on any configuration where the tie is not sustained; where it *is* sustained —
two identical centres — the two answers differ only by a relabelling, which every clustering metric
in `Lodestar.Metrics` is invariant to by construction. What a caller can act on is unchanged.

## Options refused

**Match `<=` and accept the other case failing.** Refused: it trades one wrong row for another and
buys nothing, since neither rule is the reference's.

**Freeze both observed outcomes as special cases.** Refused for the reason above — two data points
do not establish a rule, and a corpus that encoded them would assert something this repository has
not verified.

**Refuse a tie at run time.** Refused: an exact tie is a normal state of a k-means run, not a
caller error, and throwing would turn a well-defined partition into an exception for no gain.

## What enforces it

`KMeans.Assign` carries the rule and points here. The reference page for
[`KMeans.Fit`](../reference/cluster/partitioning/kmeans-fit.md) states the divergence where a
reader meets it, which is what [decision 0036](0036-a-member-may-ship-without-an-oracle-if-it-says-so.md)
asks of any behaviour that ships without an oracle behind it.
