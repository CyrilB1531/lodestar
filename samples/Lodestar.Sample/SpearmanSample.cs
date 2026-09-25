using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>Spearman's rho — the same question as Pearson, asked of the ranks.</summary>
internal static class SpearmanSample
{
    // The dose rises with the effect at every step, but the last one jumps: a measure of
    // order sees a perfect relationship where a measure of distance sees two thirds of one.
    private static readonly double[] Dose = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 40.0];
    private static readonly double[] Effect = [2.0, 3.5, 4.0, 5.5, 6.0, 7.5, 8.0, 9.0];

    public static void Run()
    {
        Console.WriteLine("Spearman correlation");

        TestResult monotone = Spearman.Test(Dose, Effect);

        Console.WriteLine($"  rho                   = {Inv.F4(monotone.Statistic)}");
        Console.WriteLine($"  p                     = {Inv.E3(monotone.PValue)}");
        Console.WriteLine($"  Pearson r, same data  = {Inv.F4(Pearson.Test(Dose, Effect).Statistic)}");

        // Every pair of three variables at once: the dose, the effect, and a side effect that falls.
        double[] rows = [.. Dose.Select((dose, i) => new[] { dose, Effect[i], 10.0 - i }).SelectMany(row => row)];
        CorrelationMatrix matrix = Spearman.Matrix(rows, 3, Alternative.TwoSided, NanPolicy.Propagate);
        Console.WriteLine(
            $"  matrix {matrix.VariableCount} x {matrix.VariableCount}          : rho(dose, side) = {Inv.F4(matrix.Statistics[2])}, "
            + $"p = {Inv.E3(matrix.PValues[2])}, equal to itself {matrix.Equals(Spearman.Matrix(rows, 3))}, hash {matrix.GetHashCode()}");
    }
}
