using Lodestar.Fuzzy;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

// CA5394, S2245: a seeded Random builds reproducible score ties; no security use.
#pragma warning disable CA5394, S2245

/// <summary>A bounded limit keeps a heap rather than sorting every hit; these pin it to the full sort.</summary>
public sealed class ProcessSelectionTests
{
    [Fact]
    public void A_bounded_limit_keeps_what_sorting_every_hit_and_truncating_keeps()
    {
        var random = new Random(821);
        for (int round = 0; round < 200; round++)
        {
            string[] choices = [.. Enumerable.Range(0, random.Next(0, 300)).Select(_ => random.Next(12).ToString(System.Globalization.CultureInfo.InvariantCulture))];

            // Few distinct scores, so almost every eviction is decided by a tie on the index.
            double Coarse(string query, string choice) => choice.Length == 1 ? double.Parse(choice, System.Globalization.CultureInfo.InvariantCulture) % 4 : 0.0;
            double cutoff = random.Next(3);
            ExtractResult[] everything = [.. Process.Extract("q", choices, Coarse, limit: null, scoreCutoff: cutoff)];

            foreach (int limit in new[] { 0, 1, 2, 7, 50, 1000 })
            {
                IReadOnlyList<ExtractResult> bounded = Process.Extract("q", choices, Coarse, limit, cutoff);
                Assert.Equal(everything.Take(limit), bounded);
            }

            ExtractResult? one = Process.ExtractOne("q", choices, Coarse, cutoff);
            Assert.Equal(everything.Length == 0 ? null : everything[0], one);
        }
    }
}
