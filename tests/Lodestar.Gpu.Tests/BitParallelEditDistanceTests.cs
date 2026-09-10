using Lodestar.Gpu.Compute;
using Lodestar.Text.Distances;
using Xunit;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random builds a reproducible corpus so a
// failing case replays; there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>The Myers kernel against <c>Levenshtein.Distance</c>, the path it replaces.</summary>
/// <remarks>
/// Edit distances are integers, so these compare exactly: there is no tolerance to grant, and a
/// difference of one is a wrong answer rather than a rounding. All on the forced CPU accelerator.
/// </remarks>
public sealed class BitParallelEditDistanceTests
{
    private static int[] Run(string pattern, IReadOnlyList<string> texts)
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var block = DeviceTextBlock.Upload(context, pattern, texts);
        return new BitParallelEditDistance(context).Distance(pattern, block);
    }

    private static void AssertMatchesBaseline(string pattern, string[] texts)
    {
        int[] actual = Run(pattern, texts);

        Assert.Equal(texts.Length, actual.Length);
        for (int i = 0; i < texts.Length; i++)
        {
            Assert.Equal(Levenshtein.Distance(pattern, texts[i]), actual[i]);
        }
    }

    [Fact]
    public void A_batch_of_short_strings_matches_the_baseline()
    {
        AssertMatchesBaseline("kitten", ["sitting", "kitten", "kitchen", "mitten", ""]);
    }

    [Fact]
    public void A_text_sharing_no_character_with_the_pattern_costs_both_lengths()
    {
        // Every character renames to the reserved code, so the equality mask is zero
        // throughout -- the path a wrong reserved slot would silently make cheaper.
        AssertMatchesBaseline("abc", ["xyz", "wxyz", "vwxyz"]);
    }

    [Fact]
    public void An_empty_text_costs_the_pattern_length()
    {
        int[] actual = Run("hello", [""]);

        Assert.Equal([5], actual);
    }

    [Fact]
    public void A_pattern_filling_the_machine_word_matches_the_baseline()
    {
        // 64 characters puts the top bit at position 63, the boundary the single-word
        // formulation is defined up to and not one past it.
        string pattern = new string('a', 63) + "b";
        AssertMatchesBaseline(
            pattern, [pattern, new string('a', 64), new string('a', 32) + new string('b', 32)]);
    }

    [Fact]
    public void A_repeated_alphabet_still_maps_to_one_code_per_character()
    {
        AssertMatchesBaseline("aaaa", ["aaaa", "aaa", "aaaaa", "baaa", ""]);
    }

    [Fact]
    public void A_seeded_batch_matches_the_baseline_row_by_row()
    {
        var random = new Random(4443);
        const string alphabet = "abcdefgh";
        string pattern = new([.. Enumerable.Range(0, 12).Select(_ => alphabet[random.Next(alphabet.Length)])]);
        string[] texts = [.. Enumerable.Range(0, 200).Select(_ =>
            new string([.. Enumerable.Range(0, 1 + random.Next(30))
                .Select(__ => alphabet[random.Next(alphabet.Length)])]))];

        AssertMatchesBaseline(pattern, texts);
    }

    [Fact]
    public void A_pattern_past_the_machine_word_is_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);
        string pattern = new('a', 65);
        using var block = DeviceTextBlock.Upload(context, pattern, ["aaa"]);

        Assert.Throws<ArgumentException>(
            () => new BitParallelEditDistance(context).Distance(pattern, block));
    }

    [Fact]
    public void A_batch_renamed_against_another_pattern_is_refused()
    {
        // The block carries the alphabet it was renamed with, so a pattern holding a
        // character that alphabet never saw is caught rather than silently unmatched.
        using var context = GpuContext.Create(preferCpu: true);
        using var block = DeviceTextBlock.Upload(context, "abc", ["abc"]);

        Assert.Throws<ArgumentException>(
            () => new BitParallelEditDistance(context).Distance("xyz", block));
    }

    [Fact]
    public void An_empty_batch_is_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.Throws<ArgumentException>(() => DeviceTextBlock.Upload(context, "abc", []));
    }
}
