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
        Console.WriteLine();
    }
}
