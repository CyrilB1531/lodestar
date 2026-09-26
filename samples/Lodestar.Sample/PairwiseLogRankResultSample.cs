using Lodestar.Survival;

namespace Lodestar.Sample;

/// <summary>A row of the pairwise table, compared by value against one built by hand.</summary>
internal static class PairwiseLogRankResultSample
{
    public static void Run()
    {
        Console.WriteLine("PairwiseLogRankResult (Lodestar.Survival)");

        double[] durations = [1.0, 2.0, 3.0, 4.0];
        bool[] observed = [true, true, true, true];
        PairwiseLogRankResult row = LogRank.Pairwise(durations, [7, 7, 2, 2], observed)[0];

        // The labels come back ascending, so group 2 (durations 3 and 4) is A.
        LogRankResult direct = LogRank.Test([3.0, 4.0], [true, true], [1.0, 2.0], [true, true]);
        var expected = new PairwiseLogRankResult(2, 7, direct);

        Console.WriteLine($"  groups           : {row.GroupA} and {row.GroupB}, statistic {Inv.F3(row.Result.Statistic)}");
        Console.WriteLine($"  same as by hand  : {row == expected}");
        Console.WriteLine();
    }
}
