using System.Reflection;
using System.Runtime.Versioning;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Guards the premise of this project: that the suite is replaying against the
/// netstandard2.0 assembly and not the net10.0 one.
/// </summary>
/// <remarks>
/// Without this, a reference that quietly resolved back to net10.0 would leave
/// every test passing while proving nothing. One fact per assembly the suite loads,
/// because <c>SetTargetFramework</c> does not cross a <c>PackageReference</c> (#529).
/// </remarks>
public sealed class NetStandardAssemblyGuardTests
{
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build()
    {
        AssertNetStandard(typeof(StandardScaler));
    }

    /// <summary>The same guarantee for Lodestar.Stats, which carries the normal quantile.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Stats()
    {
        AssertNetStandard(typeof(Lodestar.Stats.Distributions));
    }

    /// <summary>The same guarantee for Lodestar.Abstractions, which carries the sparse matrix.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Abstractions()
    {
        AssertNetStandard(typeof(Lodestar.Abstractions.CsrMatrix));
    }

    /// <summary>The same guarantee for Lodestar.Cluster, which carries the kmeans bin strategy's Lloyd.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Cluster()
    {
        AssertNetStandard(typeof(Lodestar.Cluster.KMeans));
    }

    private static void AssertNetStandard(Type witness)
    {
        string? framework = witness.Assembly
            .GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

        Assert.Equal(".NETStandard,Version=v2.0", framework);
    }
}
