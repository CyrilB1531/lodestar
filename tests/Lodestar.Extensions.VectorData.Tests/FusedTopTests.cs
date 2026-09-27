using Lodestar.Text.Search;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>
/// <see cref="FusedTop"/> against <see cref="RankFusion.Rrf"/> over both rankings in full, on collections
/// deep enough, and rank offsets small enough, that the threshold is read past its first depth (#1214).
/// </summary>
public sealed class FusedTopTests
{
    [Fact]
    public void The_fused_top_is_the_head_of_the_whole_fusion()
    {
        // S2245 / CA5394: seeded so a failing trial replays; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(1214);
        for (int trial = 0; trial < 3000; trial++)
        {
            var held = new HeldRecords<int, string>(2);
            int size = random.Next(1, 300);
            for (int key = 0; key < size; key++)
            {
                held.Put(key, $"r{key}", [random.Next(-2, 3), random.Next(-2, 3)]);
            }

            // Deletes leave free slots, which neither ranking may count.
            for (int key = 0; key < size; key++)
            {
                if (random.Next(5) == 0)
                {
                    held.Remove(key);
                }
            }

            if (held.Count == 0)
            {
                continue;
            }

            int[] live = [.. Enumerable.Range(0, held.SlotCount).Where(slot => held.At(slot) is not null)];
            int[] matchedSlots = [.. live.Where(_ => random.Next(4) != 0 || random.Next(2) == 0).Where(_ => random.Next(3) != 0)];
            double[] bm25 = [.. matchedSlots.Select(_ => random.Next(4) - 1.5)];
            int k = new[] { 1, 2, 3, 5, 60 }[random.Next(5)];
            int wanted = random.Next(1, held.Count + 1);
            double admitted = new[] { 1.0, 0.5, 0.1 }[random.Next(3)];
            HashSet<string> passes = [.. live.Where(_ => random.NextDouble() < admitted).Select(slot => held.At(slot)!)];
            Func<string, bool>? admits = admitted < 1.0 ? passes.Contains : null;
            float[] query = [random.Next(-2, 3), random.Next(-2, 3)];

            var matched = new KeywordMatches(matchedSlots, bm25);
            List<(int, double)> expected = Reference(held, query, matched, k, wanted, admits);
            List<(int Slot, double Score)> actual = FusedTop.Select(held, query, matched, k, wanted, admits);

            Assert.True(
                expected.SequenceEqual(actual),
                $"trial {trial}: n={held.Count} k={k} wanted={wanted}\n expected {string.Join(", ", expected)}\n actual   {string.Join(", ", actual)}");
        }
#pragma warning restore S2245, CA5394
    }

    private static List<(int, double)> Reference(
        HeldRecords<int, string> held,
        float[] query,
        KeywordMatches matched,
        int k,
        int wanted,
        Func<string, bool>? admits)
    {
        int[] live = [.. Enumerable.Range(0, held.SlotCount).Where(slot => held.At(slot) is not null)];
        int[] matchedSlots = matched.Slots;
        double[] bm25 = matched.Scores;
        var scores = new float[held.SlotCount];
        held.Score(query, scores);
        int[] byVector = [.. live.OrderByDescending(slot => scores[slot]).ThenBy(slot => slot)];
        int[] byKeyword = [.. Enumerable.Range(0, matchedSlots.Length)
            .OrderByDescending(i => bm25[i]).ThenBy(i => matchedSlots[i]).Select(i => matchedSlots[i])];
        return [.. RankFusion.Rrf([byVector, byKeyword], k)
            .Where(hit => admits is null || admits(held.At(hit.Document)!))
            .Take(wanted)
            .Select(hit => (hit.Document, hit.Score))];
    }
}
