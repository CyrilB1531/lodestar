using System.Security.Cryptography;
using System.Text;
using Lodestar.Gpu.Compute;
using Lodestar.Text.Similarity;
using Xunit;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random builds reproducible permutation
// coefficients so a failing case replays; there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>The MinHash kernel against <c>MinHash.Signature</c>, the path it replaces.</summary>
/// <remarks>
/// Compared <strong>exactly</strong>: a signature is a list of hashes, so any difference at all
/// means the permutation arithmetic diverged rather than rounded. All on the forced CPU
/// accelerator, because correctness has to be answerable where there is no graphics hardware.
/// </remarks>
public sealed class TiledMinHashSignaturesTests
{
    // CA5350 (weak cryptographic algorithm): SHA-1 is the hash Lodestar.Text.Similarity.MinHash
    // uses, and this suite has to supply the same hashes to compare against it. Never a signature.
#pragma warning disable CA5350
    /// <summary>The hash the CPU path uses: the first four bytes of SHA-1, little-endian.</summary>
    private static uint Hash32(string token)
    {
        Span<byte> digest = stackalloc byte[20];
        SHA1.HashData(Encoding.UTF8.GetBytes(token), digest);
        return digest[0] | ((uint)digest[1] << 8) | ((uint)digest[2] << 16) | ((uint)digest[3] << 24);
    }
#pragma warning restore CA5350

    private static readonly ulong[] Multipliers =
        [775169054918279404UL, 2109959069025162UL, 401325382989534145UL, 1130051441076870728UL];

    private static readonly ulong[] Addends =
        [1758426461858698312UL, 965365488286768773UL, 1703346441743126657UL, 1762784241922636284UL];

    /// <summary>One slot's worth of coefficients, for the length-mismatch refusal.</summary>
    private static readonly ulong[] TooShort = [1UL];

    /// <summary>One document of one hash, for the refusals that never reach a kernel.</summary>
    private static readonly uint[] OneHash = [1u];

    /// <summary>An empty document, which must give every slot its maximum.</summary>
    private static readonly uint[] Empty = [];

    private static MinHashPermutations Permutations() => new(Multipliers, Addends);

    private static void AssertMatchesCpu(params string[][] documents)
    {
        var hasher = new MinHash(Permutations());
        uint[][] hashes = [.. documents.Select(d => d.Select(Hash32).ToArray())];

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, hashes);
        IReadOnlyList<uint[]> actual =
            new TiledMinHashSignatures(context).Signatures(resident, Multipliers, Addends);

        Assert.Equal(documents.Length, actual.Count);
        for (int i = 0; i < documents.Length; i++)
        {
            Assert.Equal(hasher.Signature(documents[i]), actual[i]);
        }
    }

    [Fact]
    public void A_batch_of_documents_matches_the_cpu_path()
    {
        AssertMatchesCpu(
            ["the", "quick", "brown", "fox"],
            ["the", "quick", "brown", "dog"],
            ["entirely", "different", "words", "here"]);
    }

    [Fact]
    public void An_empty_document_gives_every_slot_its_maximum()
    {
        // The identity a minimum starts from, not a sentinel -- so two empty documents
        // estimate a similarity of 1, exactly as they do on the CPU path.
        AssertMatchesCpu([], ["alpha"]);

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, [Empty]);
        IReadOnlyList<uint[]> actual =
            new TiledMinHashSignatures(context).Signatures(resident, Multipliers, Addends);

        Assert.Equal([uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue], actual[0]);
    }

    [Fact]
    public void A_repeated_token_changes_nothing()
    {
        // A minimum is idempotent, which is what makes this a set sketch and not a bag one.
        AssertMatchesCpu(["the", "the", "quick"], ["quick", "the"]);
    }

    [Fact]
    public void A_document_longer_than_one_tile_still_matches()
    {
        // The CPU accelerator's group is 16 wide, so 50 tokens reload the shared tile several
        // times with a partial last pass -- the loop a single-tile document never exercises.
        string[] many = [.. Enumerable.Range(0, 50).Select(i => $"token{i:D3}")];

        AssertMatchesCpu(many);
    }

    [Fact]
    public void More_permutations_than_one_group_still_matches()
    {
        // 40 permutations outruns the CPU accelerator's 16-wide group, so the grid carries
        // three tiles and the last one has threads with no permutation to own.
        var random = new Random(5501);
        ulong[] a = [.. Enumerable.Range(0, 40).Select(_ => (ulong)random.NextInt64(1, long.MaxValue))];
        ulong[] b = [.. Enumerable.Range(0, 40).Select(_ => (ulong)random.NextInt64(0, long.MaxValue))];
        string[] tokens = ["alpha", "bravo", "charlie", "delta"];

        var hasher = new MinHash(new MinHashPermutations(a, b));
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, [tokens.Select(Hash32).ToArray()]);

        IReadOnlyList<uint[]> actual =
            new TiledMinHashSignatures(context).Signatures(resident, a, b);

        Assert.Equal(hasher.Signature(tokens), actual[0]);
    }

    [Fact]
    public void The_estimate_agrees_with_the_cpu_one()
    {
        string[] left = ["the", "quick", "brown", "fox"];
        string[] right = ["the", "quick", "brown", "dog"];
        var hasher = new MinHash(Permutations());

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(
            context, [left.Select(Hash32).ToArray(), right.Select(Hash32).ToArray()]);
        IReadOnlyList<uint[]> actual =
            new TiledMinHashSignatures(context).Signatures(resident, Multipliers, Addends);

        Assert.Equal(
            MinHash.Jaccard(hasher.Signature(left), hasher.Signature(right)),
            MinHash.Jaccard(actual[0], actual[1]));
    }

    [Fact]
    public void Mismatched_coefficient_lengths_are_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, [OneHash]);
        var kernel = new TiledMinHashSignatures(context);

        Assert.Throws<ArgumentException>(() => kernel.Signatures(resident, Multipliers, TooShort));
    }

    [Fact]
    public void An_empty_batch_is_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);

        Assert.Throws<ArgumentException>(() => DeviceTokenHashes.Upload(context, []));
    }
}
