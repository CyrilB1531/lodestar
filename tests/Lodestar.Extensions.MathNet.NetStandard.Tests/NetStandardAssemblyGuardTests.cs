using System.Reflection;
using System.Runtime.Versioning;
using Xunit;

namespace Lodestar.Extensions.MathNet.Tests;

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
        Assert.Equal(NetStandard, FrameworkOf(typeof(MathNetInterop)));
    }

    /// <summary>The same guarantee for Lodestar.Abstractions, reached through a package
    /// rather than a project reference.</summary>
    /// <remarks>
    /// <c>SetTargetFramework</c> does not cross a <c>PackageReference</c>: NuGet resolves
    /// package assets against this project's own framework, net10.0. Left alone the suite
    /// would convert the netstandard2.0 bridge's output into the net10.0 <c>CsrMatrix</c> —
    /// half a mirror, every test green (#529).
    /// </remarks>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Abstractions()
    {
        Assert.Equal(NetStandard, FrameworkOf(typeof(Lodestar.Abstractions.CsrMatrix)));
    }
}
