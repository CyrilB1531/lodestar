using System.Reflection;
using System.Runtime.Versioning;
using Xunit;

namespace Lodestar.Extensions.AI.Tests;

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
        Assert.Equal(NetStandard, FrameworkOf(typeof(OnnxEmbeddingGenerator)));
    }

    /// <summary>The same guarantee for Lodestar.Onnx, reached through a package
    /// rather than a project reference.</summary>
    /// <remarks>
    /// <c>SetTargetFramework</c> does not cross a <c>PackageReference</c>: NuGet resolves
    /// package assets against this project's own framework, net10.0. Left alone the suite
    /// would run the netstandard2.0 adapter against the net10.0 embedder — half a mirror,
    /// every test green (#529).
    /// </remarks>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Onnx()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(Lodestar.Onnx.OnnxTextEmbedder)));
    }

    /// <summary>And for Lodestar.Embeddings, which the constructor names and the batch path runs through.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Embeddings()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(Lodestar.Embeddings.Tokenization.BatchEncoder)));
    }
}
