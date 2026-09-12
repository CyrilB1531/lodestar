using System.Reflection;
using System.Runtime.Versioning;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>
/// Guards the premise of this project: that the suite is replaying against the
/// netstandard2.0 assembly and not the net10.0 one.
/// </summary>
/// <remarks>
/// Without this, a reference that quietly resolved back to net10.0 would leave
/// every test passing while proving nothing. The assertion is cheap; the false
/// confidence it prevents is not.
/// </remarks>
public sealed class NetStandardAssemblyGuardTests
{
    private const string NetStandard = ".NETStandard,Version=v2.0";

    private static string? FrameworkOf(Type type) =>
        type.Assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(LodestarVectorStoreOptions)));
    }

    /// <summary>The same guarantee for Lodestar.Embeddings, which holds the vectors.</summary>
    /// <remarks>
    /// <c>SetTargetFramework</c> does not cross a <c>PackageReference</c>: NuGet resolves
    /// package assets against this project's own framework, net10.0. Left alone the suite
    /// would run the netstandard2.0 store against the net10.0 index — half a mirror,
    /// every test green (#529).
    /// </remarks>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Embeddings()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(Lodestar.Embeddings.Search.EmbeddingIndex)));
    }

    /// <summary>And for Lodestar.Text, whose BM25 index is the keyword half.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Text()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(Lodestar.Text.Search.Bm25Index)));
    }
}
