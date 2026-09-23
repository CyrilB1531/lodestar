using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Kendall's tau — agreement counted pair by pair, under either normalisation.</summary>
internal static class KendallTauSample
{
    // Two judges ranking eight entries, with ties on both cards: the tie is what
    // makes the two variants differ, and what sends `Auto` to the asymptotic route.
    private static readonly double[] JudgeA = [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0];
    private static readonly double[] JudgeB = [1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 4.0, 5.0];

    public static void Run()
    {
        Console.WriteLine("Kendall's tau");

        TestResult tauB = KendallTau.Test(JudgeA, JudgeB);
        TestResult tauC = KendallTau.Test(JudgeA, JudgeB, variant: KendallVariant.TauC);

        Console.WriteLine($"  tau-b                 = {Inv.F4(tauB.Statistic)}");
        Console.WriteLine($"  tau-c                 = {Inv.F4(tauC.Statistic)}");

        // The variant normalises the concordance excess; the null distribution is read
        // against that excess, so both variants answer the same p-value.
        Console.WriteLine($"  p, either variant     = {Inv.E3(tauB.PValue)}");
        Console.WriteLine($"  same p under tau-c    = {Inv.E3(tauC.PValue)}");
    }
}
