---
status: accepted
supersedes: []
amends: []
applies: ["0072", "0076", "0102"]
---
# 0108 — MinHash ships both permutation families, and neither replaces the other

**Status:** accepted · **Date:** 2026-09-11

## Context

[#645](https://github.com/CyrilB1531/lodestar/issues/645) was opened while doing
[#643](https://github.com/CyrilB1531/lodestar/issues/643), which took `datasketch` from 1.6.5 to
2.0.0. That release named three permutation schemes and **changed the default**:

```python
_VALID_SCHEMES = (_SCHEME_AFFINE32, _SCHEME_AFFINE64, _SCHEME_LEGACY)
...
    scheme = _SCHEME_AFFINE32          # the new default
```

`Lodestar.Text.Similarity.MinHash` computes what 2.0.0 now calls `legacy`, and #643 kept the
corpus still by naming it — `scheme="legacy"` reproduces the frozen coefficients exactly. That was
the right move for a dependency bump and it settled nothing about the package.

What it left is a claim in `docs/equivalence.md` that is no longer true as written:

> Exact parity, **bit for bit**: a signature is a list of hashes, so any difference means the
> algorithm diverged.

A caller who computes a MinHash with a current `datasketch` today gets `affine32`, and this
package could not produce it. So the promise held only against a spelling the reference no longer
defaults to, and the page did not say so.

## What was measured

On an AMD Ryzen 7 8700G (16 cores), 24 tokens per document, BenchmarkDotNet:

| | hashing | minimise, `legacy` | minimise, `affine32` | end to end |
| --- | --- | --- | --- | --- |
| 64 permutations | 9 372 ns | 1 297 ns | 575 ns | **−6.8%** |
| 128 permutations | 9 300 ns | 2 576 ns | 1 159 ns | **−11.9%** |

The second family costs **less than half** the first in the loop that runs once per token per
permutation — no division by a prime, just a multiply that wraps. And it barely moves what a
caller pays, because SHA-1 hashing is **74% to 78%** of `MinHash.Signature` and neither family
touches it. That is the same finding `bench/README.md` §26 recorded for the GPU kernel, arriving
from the other side: the minimisation is not where the time is.

`affine32` is not merely "the same thing without the modulo". It applies the MurmurHash3 finalizer
to each hash first — `fmix32`, fixed constants — so that a weakly hashed input cannot ride its own
structure through an affine map, which the prime modulus used to absorb. Reproducing it bit for
bit means reproducing that too.

## Decision

**Both families ship, and `Legacy` stays the default.**

Replacing it was the option a 2× figure invites, and 6.8% to 11.9% is not what it buys. `MinHash`
is public API in a shipped package, signatures are persisted, and every one of them would stop
comparing to a freshly computed one — for a gain the hashing pass swallows.

Keeping only `Legacy` was the other option, and it is what leaves the parity claim quietly false.
The package exists to match the reference; the reference moved its default; following it is in
thesis rather than scope creep.

**The scheme travels with the coefficients**, on `MinHashPermutations`, not on `MinHash` or the
call. They are chosen together: an `affine32` multiplier is odd and fits 32 bits where a `legacy`
one spans 61. A pair that cannot mean what it says is refused at construction —
[decision 0072](0072-omega-is-an-input-not-a-seed.md) makes the coefficients an input, so unlike
the reference, which generates and therefore trusts them, this has a caller to protect. An even
multiplier collapses the value range instead of permuting it, and shows up as an estimate that is
merely wrong.

**The corpus grows a second block rather than moving one.** `tests/oracles/text_similarity.json`
keeps every value it had and gains an `affine32` object beside them: 370 lines added, none changed.

**`affine64` is not offered.** The reference pairs it with `sha1_hash64` rather than
`sha1_hash32`, so it is a second parity surface — a different hash and a different width — and not
a third value of this enum. It waits for a use, not for a release.

## What the satellite tier costs here, visibly

`Lodestar.Gpu` carries no edge to any Lodestar package
([decision 0076](0076-a-core-package-carries-no-external-dependency.md),
[0102](0102-the-gpu-gate-is-measured-on-a-named-machine.md)), so it declares its own
`MinHashScheme` beside its own `MersennePrime` and mask. The duplication is two members, and it is
visible to a caller:

```csharp
using Lodestar.Gpu.Compute;
using Lodestar.Text.Similarity;   // 'MinHashScheme' is an ambiguous reference
```

That caller needs a `using` alias. Creating an edge to remove it would put a dependency on the
satellite tier for an enum, which is the thing the tier exists to refuse — so the alias is the
price, named here rather than discovered.

The finalizer runs **inside the kernel**, as the shared tile fills: once per token per group
rather than once per token per thread. Mixing before upload would be cheaper still and would make
a `DeviceTokenHashes` belong to one family, which is what residency exists not to do.

## Consequences

- `Lodestar.Text` 0.6.0 → **0.7.0** and `Lodestar.Gpu` 0.1.0 → **0.2.0**: new public surface, no
  behaviour changed for anyone who names no scheme.
- `docs/equivalence.md`'s MinHash row says which family it claims parity with, and that the other
  is available — the claim narrows to something true rather than staying broad and false.
- **The oracle now freezes two families from one reference version.** A future `datasketch` that
  moves its default again costs a third block, not a rewrite, and the shape is set here.
- `bench/README.md` §26's split is what makes this a small decision rather than a large one. If
  the hashing ever moves to the accelerator — §26 names it as the obvious next kernel — the
  permutation arithmetic becomes a larger share of the total and this measurement is worth
  retaking, not re-deriving.
