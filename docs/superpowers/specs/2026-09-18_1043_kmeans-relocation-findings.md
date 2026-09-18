# Three relocation findings in Lodestar.Cluster — design

**Status:** accepted — **retrospective**, written the same day from the work, which was done
before the spec; no plan, since a plan is for work not yet started.
**Issues:** [#1043](https://github.com/CyrilB1531/lodestar/issues/1043),
[#1047](https://github.com/CyrilB1531/lodestar/issues/1047),
[#1066](https://github.com/CyrilB1531/lodestar/issues/1066).
**Date:** 2026-09-18.

## Why the three travel together

All three are about `KMeans.Relocate` and the paragraph of the `KMeans.Fit` page that describes
it. #1043 and #1066 are two sentences of that paragraph, and #1047 is the loop at the top of the
method the paragraph describes.

## What is measured

On numpy 2.5.3 / scikit-learn 1.9.0, on a machine whose numpy reports `X86_V3`, `X86_V4` and
`AVX512_ICL`. `NPY_DISABLE_CPU_FEATURES` switches off the upper tiers one at a time.

| Measurement | Result |
| --- | --- |
| `np.argpartition(d, -k)[:-k-1:-1]`, 900 arrays × k ∈ {1, 3, 20, 50}, hashed | three different hashes under AVX-512, AVX2 and baseline |
| scikit-learn against itself, 1,000 tied fits (seed 990), AVX-512 vs baseline | 35 centre sets, 6 inertias differ |
| one tied input (seed 2026, case 33) | `inertia_` `2.5e-31` under AVX-512, `193.26` under the baseline |
| `KMeans.Fit` vs scikit-learn, 1,000 fits with distinct distances | 0 centre sets, 0 inertias differ; 3 label sets (AVX-512) and 725 (baseline), the pairing |
| the same, 1,000 tied fits | 214 centre sets and 58 inertias differ (AVX-512), 213 and 57 (baseline) |
| the same tied fits, ties taken highest row first | 210 and 216: no row rule closes it |
| 1,000 tied fits in which no cluster empties | 0 differ |
| small tied arrays, which subset `argpartition` returns | the highest rows 74% of the time under AVX-512 and 63% under the baseline, the lowest 23% and 33%, otherwise neither |

## Decisions

**#1043 is documented, not aligned.** The standing rule is to make a found divergence iso in
code. It cannot apply here, because there is no single reference behaviour to match: the same
scikit-learn run returns different centres depending on the CPU tier numpy dispatches
`argpartition` to. Porting one tier's kernel (x86-simd-sort's AVX-512 `argselect`, or numpy's
generic introselect) would match one class of machine and still differ from the others. The
last row shows why a cheaper rule does not help either. The page now says the reference's choice
varies with the CPU tier and, with ties, carries the centres and `Inertia` with it. It quotes the
counts above, and "no frozen case turns on it" becomes "not yet".

**#1066 is dropped, not reworded.** "On none with 20 or fewer" was a property of seed 0. The
tier finding makes any such bound machine-dependent as well, so the clause goes rather than being
pinned to a seed.

**#1047 folds the count into the listing.** One walk over `counts`: the first empty cluster
allocates `counts.Length - cluster` slots, which bounds every empty cluster after it, and no
empty cluster means no allocation. The array stays because it is what fixes #975.

## Rejected

- **Port numpy's introselect.** It is BSD and portable, but it is only the baseline tier. On the
  measuring machine scikit-learn runs the AVX-512 kernel, so the port would still disagree there.
- **Ties highest row first.** It matches numpy's lean on small arrays and moves nothing on fits:
  210 against 214.
