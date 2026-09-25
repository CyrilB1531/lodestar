using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>A correlation matrix built by hand, and compared with the one Spearman.Matrix returns.</summary>
internal static class CorrelationMatrixSample
{
    public static void Run()
    {
        Console.WriteLine("Correlation matrix");

        double[] rows = [1.0, 2.0, 2.0, 1.0, 3.0, 4.0, 4.0, 3.0];
        CorrelationMatrix computed = Spearman.Matrix(rows, 2);
        var rebuilt = new CorrelationMatrix(computed.VariableCount, [.. computed.Statistics], [.. computed.PValues]);

        Console.WriteLine($"  variables             = {rebuilt.VariableCount}");
        Console.WriteLine($"  rho(0, 1)             = {Inv.F4(rebuilt.Statistics[1])}, p {Inv.F4(rebuilt.PValues[1])}");
        Console.WriteLine($"  equal to the computed = {rebuilt.Equals(computed)}");
    }
}
