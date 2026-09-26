# SplineFitter and the AFT L1 penalty: measured, and left not written

**Issue:** [#1184](https://github.com/CyrilB1531/lodestar/issues/1184).
**Status:** written with the work, 2026-09-26. The work is a throwaway spike and this record; no code
ships.

## The problem

Issue #1172 moved `lifelines.SplineFitter` and the L1 part of the AFT penalty here, to be matched at
`1e-9` by reproducing the optimiser whose stopping point is lifelines' answer, since neither has an
optimum: `SplineFitter`'s Newton step leaves its likelihood domain, and the L1 term has no second
derivative. The issue named scipy's L-BFGS-B, to be transcribed.

## What was found

- **The optimisers were others.** `SplineFitter` runs scipy's Nelder-Mead (`maxiter=400`), then
  SLSQP from its point, keeping the lower successful result: Nelder-Mead's point where it converges,
  SLSQP's after one iteration where it does not. The AFT L1 fits start from the univariate fit —
  Nelder-Mead's point, continued by L-BFGS-B in 12 of about 55 fits — then run SLSQP for the Weibull
  (`ftol=1e-10`) and the log-logistic (`ftol=1e-6`), 7 to 34 iterations, and scipy's BFGS for the
  log-normal, which sets no optimiser of its own. The spike did not write BFGS.
- **Transcription is excluded** by [decision 0002](../../decisions/0002-provenance-and-the-allowed-references.md),
  for any licence. Nelder-Mead in scipy's parametrisation and unconstrained SLSQP (Kraft 1988, with
  Fletcher and Powell's 1974 `LDLᵀ` update) were written from their papers in a throwaway Python
  spike; scipy's SLSQP source was read to diagnose one divergence, which was lifelines' `ftol`.
- **Where the likelihood is bit-identical, both replay scipy**: Nelder-Mead to `0.0` on 40 spline
  fits, SLSQP to `5e-12` with the same iteration counts on 30 spline and AFT fits.
- **Nelder-Mead does not survive the last bit.** A relative `1e-15` in the likelihood moved its point
  by `6e-6` to `4.7e-3` in 7 of about 140 runs: it stops on a `1e-4` tolerance by comparing values, so
  a comparison the last bit decides changes the path. SLSQP moved by at most `5.7e-10`, but every AFT
  lasso starts from a Nelder-Mead point, and on a non-smooth objective the stopping point follows the
  start.
- **scipy's own answer depends on the CPU.** numpy's `argsort` breaks ties among the simplex's values
  differently under AVX2 than at its baseline, and 2 of 40 spline fits moved by up to `1.3e-4` with the
  SIMD tier.

## Decision

Both stay *not written*, as `CRCSplineFitter` is: a C# likelihood cannot be bit-identical to
autograd's, `exp` and `log` parting in the last bit between runtimes, and at that bit lifelines'
answer is not defined. `docs/equivalence.md` carries the measurements, `AftOptions.Penalizer` stays
the ridge part alone, and no L-BFGS-B, SLSQP or Nelder-Mead is written.

## Rejected

- **Parity on the stable cases only**, the ~95 % whose answer survives an ulp, documenting the rest
  as diverging by up to `5e-3`: a fit whose agreement with the reference depends on the data is not
  parity, and would need L-BFGS-B written as well for the AFT starts it decides.
