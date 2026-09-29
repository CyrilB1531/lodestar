using Lodestar.Gpu.Compute;
using Lodestar.Tests.Documentation;
using Xunit;

namespace Lodestar.Gpu.Tests.Documentation;

/// <summary>The reference gate over the pages <c>Lodestar.Gpu</c> declares covered.</summary>
/// <remarks>
/// The engine and its own unit tests live with <c>Lodestar.Text</c>; what is here is this
/// package's half — its namespace against its pages. The netstandard2.1 mirror links this file,
/// so the pages are checked against both of the package's builds.
/// </remarks>
public sealed class ReferenceDocumentationTests
{
    private static string Root => Path.Combine(AppContext.BaseDirectory, "reference");

    private static string Map => Path.Combine(AppContext.BaseDirectory, "wiki-map.json");

    private static string Docs => Path.Combine(AppContext.BaseDirectory, "docs");

    [Fact]
    public void Every_covered_namespace_is_documented()
    {
        IReadOnlyList<string> complaints = ReferenceDocumentation.Check(
            typeof(GpuContext).Assembly, "Lodestar.Gpu", Map, Root);

        Assert.Empty(complaints);
    }

    [Fact]
    public void Every_documented_member_named_in_the_docs_links_to_its_entry()
    {
        IReadOnlyList<string> complaints = ReferenceDocumentation.CheckLinks(
            typeof(GpuContext).Assembly, "Lodestar.Gpu", Map, Docs);

        Assert.Empty(complaints);
    }
}
