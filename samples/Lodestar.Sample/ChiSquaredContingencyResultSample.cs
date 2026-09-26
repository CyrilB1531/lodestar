using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>
/// <see cref="ChiSquaredContingencyResult"/> — the statistic, the p-value, the degrees
/// of freedom and the table independence would have produced.
/// </summary>
internal static class ChiSquaredContingencyResultSample
{
    private static readonly double[][] Observed =
    [
        [30.0, 20.0],
        [15.0, 35.0],
    ];

    public static void Run()
    {
        Console.WriteLine("ChiSquaredContingencyResult");

        ChiSquaredContingencyResult result = ChiSquared.Contingency(Observed, Continuity.None);

        Console.WriteLine($"  statistic             = {Inv.F3(result.Statistic)}");
        Console.WriteLine($"  p-value               = {Inv.E3(result.PValue)}");
        Console.WriteLine($"  dof                   = {result.DegreesOfFreedom}");
        Console.WriteLine($"  expected row 0        = {Inv.List(result.ExpectedFrequencies[0])}");
    }
}
