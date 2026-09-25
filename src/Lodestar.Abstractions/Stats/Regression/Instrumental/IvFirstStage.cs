namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>How strongly the instruments predict one endogenous regressor: <c>first_stage.diagnostics</c>' row.</summary>
public sealed class IvFirstStage
{
    /// <summary>The R² of the endogenous regressor on the exogenous regressors and the instruments.</summary>
    public double RSquared { get; init; }

    /// <summary>The R² of the instruments alone, once both sides are purged of the exogenous regressors.</summary>
    public double PartialRSquared { get; init; }

    /// <summary>Shea's partial R², which accounts for the other endogenous regressors.</summary>
    public double SheaRSquared { get; init; }

    /// <summary>The joint test that the instruments' first-stage coefficients are zero.</summary>
    /// <remarks>An F under an unadjusted covariance and a χ² under the others, whatever <c>IvOptions.Debiased</c> says, as in the reference.</remarks>
    public WaldTest InstrumentTest { get; init; } = new(0.0, 1.0, 0, null);
}
