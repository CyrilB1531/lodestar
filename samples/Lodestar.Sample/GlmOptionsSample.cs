using Lodestar.Stats.Regression;

namespace Lodestar.Sample;

/// <summary>The confidence level a fit is read at, and inspecting one that did not converge.</summary>
internal static class GlmOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("GLM fitting options (Lodestar.Stats.Regression)");

        double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
        double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

        GlmSummary ninetyFive = GeneralizedLinearModel.Fit(design, response, 1, GlmFamily.Binomial);
        GlmSummary ninetyNine = GeneralizedLinearModel.Fit(
            design, response, 1, GlmFamily.Binomial, new GlmOptions { ConfidenceLevel = 0.99 });

        Console.WriteLine(
            $"  95%              : [{Inv.F4(ninetyFive.ConfidenceLower[1])}, {Inv.F4(ninetyFive.ConfidenceUpper[1])}]");
        Console.WriteLine(
            $"  99%              : [{Inv.F4(ninetyNine.ConfidenceLower[1])}, {Inv.F4(ninetyNine.ConfidenceUpper[1])}]");

        // A perfectly separable design: every y = 1 sits above every y = 0, so the logit
        // slope keeps growing and IRLS exhausts its budget rather than reaching Tolerance.
        double[] separableDesign = [-2.0, -1.0, 1.0, 2.0];
        double[] separableResponse = [0.0, 0.0, 1.0, 1.0];

        GlmSummary notConverged = GeneralizedLinearModel.Fit(
            separableDesign, separableResponse, 1, GlmFamily.Binomial,
            new GlmOptions { MaximumIterations = 15, ThrowOnNonConvergence = false });

        Console.WriteLine($"  converged        : {notConverged.Converged} after {notConverged.Iterations} iterations");
        Console.WriteLine($"  last change      : {Inv.E3(notConverged.DevianceChange)}");

        // A looser Tolerance accepts a larger last step, so IRLS stops sooner on the same fit.
        GlmSummary loose = GeneralizedLinearModel.Fit(
            design, response, 1, GlmFamily.Binomial, new GlmOptions { Tolerance = 1e-2 });
        Console.WriteLine($"  iterations       : {ninetyFive.Iterations} at 1e-8, {loose.Iterations} at 1e-2");

        // Without the intercept the slope is the only coefficient, so the lists shrink by one.
        GlmSummary throughOrigin = GeneralizedLinearModel.Fit(
            design, response, 1, GlmFamily.Binomial, new GlmOptions { WithIntercept = false });
        Console.WriteLine($"  no intercept     : {throughOrigin.Coefficients.Count} coefficient");

        // Over-dispersed counts: the negative binomial's alpha widens the slope's standard error.
        double[] x = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0];
        double[] counts = [1.0, 0.0, 2.0, 3.0, 4.0, 3.0, 7.0, 6.0, 9.0, 11.0];
        GlmSummary poisson = GeneralizedLinearModel.Fit(x, counts, 1, GlmFamily.Poisson);
        GlmSummary negativeBinomial = GeneralizedLinearModel.Fit(
            x, counts, 1, GlmFamily.NegativeBinomial, new GlmOptions { NegativeBinomialAlpha = 0.5 });
        Console.WriteLine($"  slope s.e.       : Poisson {Inv.F4(poisson.StandardErrors[1])}, negative binomial {Inv.F4(negativeBinomial.StandardErrors[1])}");

        // A positive, skewed response: Gamma through the log link, with its estimated dispersion.
        double[] cost = [2.1, 1.8, 3.5, 2.9, 4.8, 5.5, 4.9, 7.8, 6.9, 9.4];
        GlmSummary gamma = GeneralizedLinearModel.Fit(
            x, cost, 1, GlmFamily.Gamma, new GlmOptions { Link = GlmLink.Log });
        Console.WriteLine($"  gamma, log link  : growth {Inv.F4(gamma.Coefficients[1])}, dispersion {Inv.F4(gamma.Dispersion)}");
        Console.WriteLine();
    }
}
