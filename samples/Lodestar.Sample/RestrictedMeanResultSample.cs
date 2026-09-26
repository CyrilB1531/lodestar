using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>The restricted mean of a curve that closes, against the value it must have.</summary>
internal static class RestrictedMeanResultSample
{
    public static void Run()
    {
        Console.WriteLine("RestrictedMeanResult (Lodestar.Survival)");

        // One subject, dead at time two: two units of certain survival, and no spread.
        RestrictedMeanResult result = KaplanMeier.RestrictedMean(KaplanMeier.Estimate([2.0], [true]));
        var expected = new RestrictedMeanResult(2.0, 0.0);

        Console.WriteLine($"  mean / variance  : {Inv.F3(result.Mean)} / {Inv.F3(result.Variance)}");
        Console.WriteLine($"  as expected      : {result == expected}");
        Console.WriteLine();
    }
}
