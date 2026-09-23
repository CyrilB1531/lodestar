using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>What a binomial result carries past its two numbers: three intervals.</summary>
internal static class BinomialResultSample
{
    public static void Run()
    {
        Console.WriteLine("Binomial confidence intervals");

        BinomialResult result = Binomial.Test(7, 20, 0.5);
        (double exactLow, double exactHigh) = result.ProportionConfidenceInterval();
        (double wilsonLow, double wilsonHigh) =
            result.ProportionConfidenceInterval(0.95, ProportionInterval.Wilson);
        (double correctedLow, double correctedHigh) =
            result.ProportionConfidenceInterval(0.95, ProportionInterval.WilsonCorrected);

        // Clopper-Pearson never covers less than the level asked for and usually
        // covers more, which is why it is the widest of the three.
        Console.WriteLine($"  Clopper-Pearson       = [{Inv.F4(exactLow)}, {Inv.F4(exactHigh)}]");
        Console.WriteLine($"  Wilson                = [{Inv.F4(wilsonLow)}, {Inv.F4(wilsonHigh)}]");
        Console.WriteLine($"  Wilson, corrected     = [{Inv.F4(correctedLow)}, {Inv.F4(correctedHigh)}]");
    }
}
