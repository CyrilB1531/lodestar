using Lodestar.Stats.TimeSeries;

namespace Lodestar.Sample;

/// <summary>A Ljung-Box test cumulated lag by lag, indexed from lag 1.</summary>
internal static class LjungBoxResultSample
{
    private static readonly double[] Series =
        [1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0, 13.0];

    public static void Run()
    {
        Console.WriteLine("The Ljung-Box result (Lodestar.Stats.TimeSeries)");

        LjungBoxResult result = SerialCorrelation.LjungBox(Series, lagCount: 4);

        for (int i = 0; i < result.Statistics.Count; i++)
        {
            Console.WriteLine(
                $"  lag {i + 1} (df {result.DegreesOfFreedom[i]}): "
                + $"Q = {Inv.F3(result.Statistics[i])}, p = {Inv.F3(result.PValues[i])}");
        }

        Console.WriteLine();
    }
}
