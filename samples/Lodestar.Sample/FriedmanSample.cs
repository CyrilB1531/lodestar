using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>The Friedman test — three judges scoring the same six wines.</summary>
internal static class FriedmanSample
{
    // Each judge is harsh or generous in their own way; ranking within a wine
    // removes that before the judges are compared.
    private static readonly double[] Strict = [7.0, 5.0, 8.0, 6.0, 9.0, 7.0];
    private static readonly double[] Stricter = [6.0, 4.0, 7.0, 5.0, 8.0, 6.0];
    private static readonly double[] Generous = [8.0, 7.0, 9.0, 8.0, 9.0, 8.0];

    public static void Run()
    {
        Console.WriteLine("Friedman test");

        TestResult paired = Friedman.Test(Strict, Stricter, Generous);
        TestResult unpaired = KruskalWallis.Test(Strict, Stricter, Generous);

        Console.WriteLine($"  Q                     = {Inv.F4(paired.Statistic)}");
        Console.WriteLine($"  p, paired             = {Inv.E3(paired.PValue)}");
        Console.WriteLine($"  p, unpaired           = {Inv.E3(unpaired.PValue)}");
    }
}
