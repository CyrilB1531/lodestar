# ParametricModel

Which of lifelines' parametric univariate models [`ParametricSurvival`](parametricsurvival.md) fits.

<!-- docs-declaration -->

```csharp
public enum ParametricModel { Exponential, Weibull, LogNormal, LogLogistic, PiecewiseExponential, GeneralizedGamma }
```

**Members** — each with its parameters in [`ParametricFit.ParameterNames`](parametricfit.md)' order:

- `Exponential`, `H(t) = t / λ`: `lambda_`. lifelines' `ExponentialFitter`.
- `Weibull`, `H(t) = (t / λ)^ρ`: `lambda_`, `rho_`. `WeibullFitter`.
- `LogNormal`, `log T` normal with mean `μ` and deviation `σ`: `mu_`, `sigma_`. `LogNormalFitter`.
- `LogLogistic`, `S(t) = 1 / (1 + (t / α)^β)`: `alpha_`, `beta_`. `LogLogisticFitter`.
- `PiecewiseExponential`, a constant hazard `1 / λᵢ` between consecutive breakpoints: `lambda_0_`,
  `lambda_1_` and so on, one more than there are breakpoints. `PiecewiseExponentialFitter`.
- `GeneralizedGamma`, in `(μ, log σ, λ)`: `mu_`, `ln_sigma_`, `lambda_`. `GeneralizedGammaFitter`.

**Example** — the same ten patients under three models, by AIC.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

double exponential = Math.Round(ParametricSurvival.Fit(ParametricModel.Exponential, months, died).Aic, 4);   // => 53.6435
double weibull = Math.Round(ParametricSurvival.Fit(ParametricModel.Weibull, months, died).Aic, 4);           // => 53.4005
double logNormal = Math.Round(ParametricSurvival.Fit(ParametricModel.LogNormal, months, died).Aic, 4);       // => 52.3648
```

**Remarks** — **the exponential is a Weibull with `ρ = 1`, and both are generalized gammas**, as the
log-normal is at `λ = 0`: the generalized gamma nests them, and its fit is the check that a simpler
one is not forcing its shape on the data. A difference of one in AIC, as above, is no evidence either
way on ten subjects.

**The piecewise exponential needs its breakpoints** in
[`ParametricOptions.Breakpoints`](parametricoptions.md), and is refused without them.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival`](parametricsurvival.md), [`AftModel`](aftmodel.md).
