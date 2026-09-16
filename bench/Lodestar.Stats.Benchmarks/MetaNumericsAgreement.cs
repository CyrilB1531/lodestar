using System.Globalization;

namespace Lodestar.Stats.Benchmarks;

/// <summary>
/// What the two sides must agree on before either is timed, and what they are allowed to differ on (#756).
/// </summary>
/// <remarks>
/// The statistic is the same quantity in both libraries — a t, a U, an H — so a disagreement there
/// means the row would be meaningless, and it stops the run. A p-value is that statistic read
/// through a distribution, where the two deliberately differ on continuity corrections and on the
/// exact/asymptotic switch: a fact to report, so it is recorded and printed instead.
/// </remarks>
internal static class MetaNumericsAgreement
{
    /// <summary>Relative tolerance for a quantity both libraries compute the same way.</summary>
    private const double Tolerance = 1e-9;

    private static readonly List<string> Differences = [];

    /// <summary>Refuses to time a pair whose statistic disagrees; records a p-value that does.</summary>
    public static void Require(string family, double ourStatistic, double theirStatistic, double ourP, double theirP)
    {
        if (!Close(ourStatistic, theirStatistic))
        {
            throw new InvalidOperationException(
                $"{family}: the statistics disagree ({Text(ourStatistic)} against {Text(theirStatistic)}), so the two "
                + "libraries are not computing the same quantity and timing them under one name would say nothing.");
        }

        if (!Close(ourP, theirP))
        {
            Differences.Add(
                $"{family}: statistic agrees, p-value {Text(ourP)} against {Text(theirP)} "
                + $"(relative {Text(Math.Abs(ourP - theirP) / Math.Max(Math.Abs(ourP), double.Epsilon))})");
        }
    }

    /// <summary>The same, for a pair whose statistic this package does not expose comparably.</summary>
    public static void Record(string family, double ours, double theirs)
    {
        if (!Close(ours, theirs))
        {
            Differences.Add($"{family}: {Text(ours)} against {Text(theirs)}");
        }
    }

    /// <summary>Prints what disagreed, which is what the write-up quotes.</summary>
    public static void Report(string title)
    {
        // console-print: the report is the deliverable, and bench/README.md §45 quotes these lines.
        Console.WriteLine($"// {title}: {Differences.Count} recorded difference(s)");
        foreach (string difference in Differences)
        {
            Console.WriteLine($"//   {difference}");  // console-print: see above
        }

        Differences.Clear();
    }

    private static bool Close(double ours, double theirs)
    {
        if (double.IsNaN(ours) || double.IsNaN(theirs))
        {
            return double.IsNaN(ours) && double.IsNaN(theirs);
        }

        return Math.Abs(ours - theirs) <= Tolerance * Math.Max(1.0, Math.Abs(ours));
    }

    private static string Text(double value) => value.ToString("G6", CultureInfo.InvariantCulture);
}
