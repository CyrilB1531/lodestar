# 0837 — The incomplete gamma and beta at large shapes

**Status:** accepted, 2026-09-17. Written after the measurement it records.

Issue: [#837](https://github.com/CyrilB1531/lodestar/issues/837), found by the performance review of `main` (2026-09-16).

## Change

- **Gamma.** Past `a = 20` and within `|x - a| / a < 0.3`, `P` and `Q` come from Temme's uniform
  asymptotic expansion (Temme 1979, DLMF 8.12), twelve terms in `1/a` of sixteen in `eta`, the
  coefficients expanded from DLMF 8.12.9's recursion by exact rational arithmetic. Outside it the
  series or continued fraction converges within a few hundred terms, and from `a = 10` its prefactor
  `x^a e^-x / Gamma(a)` is formed through Stirling's `Gamma*` and `log(1 + t) - t` instead of
  subtracting Lanczos log-gammas of order `a log a`.
- **Beta.** The continued fraction keeps its switch at the mean and loses its 300-term cap, now a
  guard at a million; from both shapes of 10 its prefactor is formed the same way, relative to the
  mean, which is how boost's `ibeta` (scipy's `betainc`) reaches large shapes too.

## Measured

Against scipy 1.18.1 on the issue's 168 points, within three standard deviations of the mean:

| function | `main` worst relative | fix worst relative |
| --- | ---: | ---: |
| `P`, `Q`, `a` from 1e3 to 1e8 | 9.8e-1 | 1.2e-15 |
| `I_x(a, b)`, shapes from 10 to 1e8 | 7.4e-1 | 9.6e-12 |

On 2,922 further points (shapes 0.1 to 1e5, both tails) no point moved away from scipy past `1e-10` relative.
Eight cases join `stats_distributions.json`; no existing corpus moved.

## Rejected

- **Only raising the cap.** The series needs about `7 sqrt(a)` terms at `x = a` — 72,000 at
  `a = 1e8` — and the log-gamma prefactor alone is `7e-8` off there, even from an exact `lgamma`.
- **scipy's narrower region past `a = 200`.** It spans 4.5 standard deviations, and past it scipy's
  series is cut at 2,000 terms and 60% wrong at `a = 1e8`; `docs/equivalence.md` records the gap.
- **Temme's expansion for the beta.** The fraction already converges in 4,600 terms at
  `a = b = 1e8` once its prefactor is exact, and the reference takes the same route.
