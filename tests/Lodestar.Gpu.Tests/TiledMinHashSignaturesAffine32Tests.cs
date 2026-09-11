using System.Security.Cryptography;
using System.Text;
using Lodestar.Gpu.Compute;
using Lodestar.Text.Similarity;
using Xunit;
using GpuScheme = Lodestar.Gpu.Compute.MinHashScheme;
using TextScheme = Lodestar.Text.Similarity.MinHashScheme;

namespace Lodestar.Gpu.Tests;

/// <summary>The MinHash kernel's second permutation family, against the CPU path it replaces.</summary>
/// <remarks>
/// Compared <strong>exactly</strong>, for the reason the legacy suite gives: a signature is a list
/// of hashes, so any difference means the arithmetic diverged rather than rounded. The interest
/// here is that <c>affine32</c> mixes in the kernel — as the shared tile fills, once per token per
/// group — while the CPU path mixes once per token, and the two have to agree anyway (#645).
/// </remarks>
public sealed class TiledMinHashSignaturesAffine32Tests
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

    /// <summary>Odd and inside 32 bits, which is what the family requires of a multiplier.</summary>
    private static readonly ulong[] Multipliers = [3582191691UL, 4270784983UL, 1892572953UL, 3715639441UL];

    private static readonly ulong[] Addends = [982527UL, 1100580627UL, 2597016983UL, 4286725387UL];

    private static void AssertMatchesCpu(params string[][] documents)
    {
        var hasher = new MinHash(new MinHashPermutations(Multipliers, Addends, TextScheme.Affine32));
        uint[][] hashes = [.. documents.Select(d => d.Select(Hash32).ToArray())];

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, hashes);
        IReadOnlyList<uint[]> actual = new TiledMinHashSignatures(context)
            .Signatures(resident, Multipliers, Addends, GpuScheme.Affine32);

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
    public void A_document_longer_than_one_tile_matches_the_cpu_path()
    {
        // The tile is refilled per pass and the finalizer runs on each fill, so a document that
        // spans more than one pass is what proves the mix is not applied twice to a value.
        string[] long_document = [.. Enumerable.Range(0, 700).Select(i => $"token-{i}")];

        AssertMatchesCpu(long_document);
    }

    [Fact]
    public void An_empty_document_gives_every_slot_its_maximum()
    {
        AssertMatchesCpu([], ["alpha"]);
    }

    /// <summary>One residency, both families — which is why the mix is in the kernel.</summary>
    [Fact]
    public void The_same_resident_batch_serves_both_schemes()
    {
        string[][] documents = [["the", "quick", "brown", "fox"], ["entirely", "different"]];
        uint[][] hashes = [.. documents.Select(d => d.Select(Hash32).ToArray())];
        ulong[] legacyA = [775169054918279404UL, 2109959069025162UL, 401325382989534145UL,
            1130051441076870728UL];
        ulong[] legacyB = [1758426461858698312UL, 965365488286768773UL, 1703346441743126657UL,
            1762784241922636284UL];

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, hashes);
        var kernel = new TiledMinHashSignatures(context);

        IReadOnlyList<uint[]> affine =
            kernel.Signatures(resident, Multipliers, Addends, GpuScheme.Affine32);
        IReadOnlyList<uint[]> legacy =
            kernel.Signatures(resident, legacyA, legacyB, GpuScheme.Legacy);

        Assert.Equal(
            new MinHash(new MinHashPermutations(Multipliers, Addends, TextScheme.Affine32))
                .Signature(documents[0]),
            affine[0]);
        Assert.Equal(
            new MinHash(new MinHashPermutations(legacyA, legacyB)).Signature(documents[0]),
            legacy[0]);
        Assert.NotEqual(affine[0], legacy[0]);
    }

    [Fact]
    public void Naming_no_scheme_is_naming_legacy()
    {
        string[][] documents = [["the", "quick", "brown", "fox"]];
        uint[][] hashes = [.. documents.Select(d => d.Select(Hash32).ToArray())];
        ulong[] legacyA = [775169054918279404UL, 2109959069025162UL, 401325382989534145UL,
            1130051441076870728UL];
        ulong[] legacyB = [1758426461858698312UL, 965365488286768773UL, 1703346441743126657UL,
            1762784241922636284UL];

        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, hashes);
        var kernel = new TiledMinHashSignatures(context);

        Assert.Equal(
            kernel.Signatures(resident, legacyA, legacyB, GpuScheme.Legacy)[0],
            kernel.Signatures(resident, legacyA, legacyB)[0]);
    }

    [Theory]
    [InlineData(4294967296UL)]   // past 32 bits: the other family's width
    [InlineData(2UL)]            // even: collapses the range instead of permuting it
    public void A_coefficient_affine32_cannot_read_is_refused(ulong multiplier)
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, [[1u]]);
        var kernel = new TiledMinHashSignatures(context);

        Assert.Throws<ArgumentException>(() =>
            kernel.Signatures(resident, [multiplier], [0UL], GpuScheme.Affine32));
    }

    [Fact]
    public void A_scheme_that_is_not_a_member_is_refused()
    {
        using var context = GpuContext.Create(preferCpu: true);
        using var resident = DeviceTokenHashes.Upload(context, [[1u]]);
        var kernel = new TiledMinHashSignatures(context);

        Assert.Throws<ArgumentException>(() =>
            kernel.Signatures(resident, [1UL], [0UL], (GpuScheme)7));
    }
}
