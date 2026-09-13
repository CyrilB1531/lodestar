# Lodestar.Extensions.VectorData Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ship `Lodestar.Extensions.VectorData`, an in-process `Microsoft.Extensions.VectorData` provider whose hybrid search needs no service running anywhere.

**Architecture:** A `Dictionary<TKey, TRecord>` is the collection's state; an `EmbeddingIndex`, a `Bm25Index` and a position-to-key array are derived caches rebuilt on the first read after a write, because `EmbeddingIndex` only appends and `Bm25Index` accepts no document. Filters compile and run before the search so `top` means `top`. Hybrid search fuses the two rankings through `RankFusion.Rrf`.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, `Microsoft.Extensions.VectorData.Abstractions` 10.10.0, `Lodestar.Embeddings` 0.5.0, `Lodestar.Text` 0.6.0.

**Spec:** [`docs/superpowers/specs/2026-09-12_0682_vectordata-satellite.md`](../specs/2026-09-12_0682_vectordata-satellite.md)

**Branch:** `feat/682-vectordata-satellite`

## Global Constraints

- **Everything in English** — code, comments, XML docs, commit messages. No `feat:`/`fix:` prefix on a commit message. Reference the issue with `Closes #682` in the pull request, not in every commit.
- **Both target frameworks, one public API.** `net10.0;netstandard2.0`, no reduced surface on either. Gaps close through PolySharp, then target-only package references, then a hand-written fallback in `src/Shared/`.
- **Warnings are errors**, `AnalysisMode=All`, `AnalysisLevel` 10.0. The rules this code will meet: **S1226** never reassign a parameter — introduce a new local; **S1192** a string literal repeated four times becomes a constant; **CA1819** a property must not return an array; **S3776** cognitive complexity 15.
- **Comment rules:** say why, not what. Two lines inline, eight lines of prose in XML documentation. A longer block carries `long-comment:` **as its very first line** — so it cannot live inside a `<summary>`. `python3 tools/check_comment_length.py` counts them.
- **`$MAIN` is the main checkout's root**, which is where the two dotfiles and
  `.venv-oracles` live. From a worktree it is *not* `--show-toplevel`. Set it once per
  shell before running any command in this plan:

  ```bash
  MAIN="$(cd "$(git rev-parse --git-common-dir)/.." && pwd -P)"
  ```

- **Every dotnet command goes through the lock**: `"$MAIN/.dotnet-guarded" dotnet <args>`.
  A bare `dotnet build`/`test`/`run` during a benchmark corrupts its numbers silently.
- **The ADR number comes from `"$MAIN/.next-adr"`**, never from listing `docs/decisions/`. It reported `0119` on 2026-09-12; run it again when writing the record.
- **Floors, exactly:** `Microsoft.Extensions.VectorData.Abstractions` **10.10.0**; `Lodestar.Embeddings` **0.5.0** (unchanged — `EmbeddingIndex`, `FromOwnedBlock` and `BlockNormalization` are all present there, verified); `Lodestar.Text` **0.4.0 → 0.6.0** (the keyword half arrives at 0.6.0). Central Package Management means that raise is repository-wide and `Lodestar.Fuzzy` restores against 0.6.0 too.
- **No new external dependency beyond the one.** `Microsoft.Extensions.AI.Abstractions` 10.10.0 arrives transitively and is already pinned at that version.
- **New package version is `0.1.0`.** No other package's version moves; no tag is cut.
- **Test count, not colour.** `dotnet test Lodestar.slnx -c Release` must report **34 assemblies** once this package's two suites exist (32 today).
- **The netstandard mirror pins every `Lodestar.*` dependency with its own `ProjectReference`**, never a `PackageReference`: `SetTargetFramework` does not travel across one. `tools/check_netstandard_guards.py` enforces both halves.

---

### Task 1: The package, its edges, and every guard that counts packages

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/Lodestar.Extensions.VectorData.csproj`
- Create: `src/Lodestar.Extensions.VectorData/Version.props`
- Create: `src/Lodestar.Extensions.VectorData/LodestarVectorStoreOptions.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/Lodestar.Extensions.VectorData.Tests.csproj`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/LodestarVectorStoreOptionsTests.cs`
- Create: `tests/Lodestar.Extensions.VectorData.NetStandard.Tests/Lodestar.Extensions.VectorData.NetStandard.Tests.csproj`
- Create: `tests/Lodestar.Extensions.VectorData.NetStandard.Tests/NetStandardAssemblyGuardTests.cs`
- Modify: `src/Directory.Packages.props`
- Modify: `Lodestar.slnx`
- Modify: `tools/check_nuspec_dependencies.py`
- Modify: `tools/check_version_floor.py`
- Modify: `CLAUDE.md`

**Interfaces:**

- Produces: the assembly `Lodestar.Extensions.VectorData`, namespace `Lodestar.Extensions.VectorData`, and `public sealed class LodestarVectorStoreOptions` with `CountVectorizerOptions? Vectorizer { get; init; }`, `Bm25Options? Bm25 { get; init; }`, `int RankFusionK { get; init; } = 60`.

- [ ] **Step 1: Pin the dependency and raise the Text floor**

In `src/Directory.Packages.props`, add the external pin beside the other two externals (line 39–40 today holds `Microsoft.Extensions.AI.Abstractions` and `Microsoft.ML.OnnxRuntime`):

```xml
    <!-- The dependency Lodestar.Extensions.VectorData exists for. Its own single
         dependency is Microsoft.Extensions.AI.Abstractions at this same 10.10.0,
         so the two satellites agree without a range to reconcile. -->
    <PackageVersion Include="Microsoft.Extensions.VectorData.Abstractions" Version="10.10.0" />
```

And raise the `Lodestar.Text` floor on line 31 from `0.4.0` to `0.6.0`:

```xml
    <PackageVersion Include="Lodestar.Text" Version="0.6.0" />
```

- [ ] **Step 2: Write Version.props**

```xml
<Project>

  <!--
    Lodestar.Extensions.VectorData owns its version here, independently of the
    other packages (see docs/decisions/0012-per-package-versioning.md).

    0.1.0 is this package's first release. It carries two edges — to
    Lodestar.Embeddings, whose EmbeddingIndex holds the vectors, and to
    Lodestar.Text, whose Bm25Index and RankFusion are the keyword half — so each
    floor moves when one of those publishes API this package needs.
  -->
  <PropertyGroup>
    <LodestarExtensionsVectorDataVersion>0.1.0</LodestarExtensionsVectorDataVersion>
  </PropertyGroup>

</Project>
```

- [ ] **Step 3: Write the library csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <!-- This package's version, owned here rather than repository-wide. -->
  <Import Project="Version.props" />

  <PropertyGroup>
    <Version>$(LodestarExtensionsVectorDataVersion)</Version>
    <TargetFrameworks>net10.0;netstandard2.0</TargetFrameworks>
    <RootNamespace>Lodestar.Extensions.VectorData</RootNamespace>

    <PackageId>Lodestar.Extensions.VectorData</PackageId>
    <Description>Microsoft.Extensions.VectorData provider for Lodestar: an in-process vector store with hybrid keyword and vector search, over Lodestar's own EmbeddingIndex and BM25 index, so a Semantic Kernel or Microsoft.Extensions.AI consumer gets hybrid retrieval with no database and no service running anywhere.</Description>
    <PackageTags>microsoft-extensions-vectordata;vector-store;hybrid-search;bm25;embeddings;semantic-kernel;interop;lodestar</PackageTags>
  </PropertyGroup>

  <!-- The dependency this package exists for. A core package carries none, so an
       external dependency earns its own satellite named for it — this is the
       third member of the interop tier (decisions 0076 and 0100). -->
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.VectorData.Abstractions" />
  </ItemGroup>

  <!--
    Two Lodestar edges, both load-bearing: Lodestar.Embeddings' EmbeddingIndex is
    the vector half and Lodestar.Text's Bm25Index, CountVectorizer and RankFusion
    are the keyword half. Neither is a transitive convenience — decision 0100
    parked this package until the second was published.

    Expressed as PackageReferences against the published packages, like every
    other src/ edge, so the build graph is the graph consumers see
    (decision 0012).

    The opt-in below exists only so that editing several packages in one branch
    does not require a pack-and-restore cycle per edit:

        export LodestarUseProjectRefs=true

    It is never set in CI, and the packed .nuspec is asserted there.
  -->
  <ItemGroup Condition="'$(LodestarUseProjectRefs)' == 'true'">
    <ProjectReference Include="../Lodestar.Embeddings/Lodestar.Embeddings.csproj" />
    <ProjectReference Include="../Lodestar.Text/Lodestar.Text.csproj" />
  </ItemGroup>

  <ItemGroup Condition="'$(LodestarUseProjectRefs)' != 'true'">
    <PackageReference Include="Lodestar.Embeddings" />
    <PackageReference Include="Lodestar.Text" />
  </ItemGroup>

  <!-- Loud on purpose: a local build that silently differed from the shipped one
       is the failure mode this whole arrangement is designed to avoid. -->
  <Target Name="WarnOnLocalProjectRefs" BeforeTargets="Build"
          Condition="'$(LodestarUseProjectRefs)' == 'true'">
    <Message Importance="high"
             Text="Lodestar.Extensions.VectorData: LodestarUseProjectRefs=true — referencing Lodestar.Embeddings and Lodestar.Text by project, not by package. This is the developer loop, not what ships." />
  </Target>

  <ItemGroup>
    <InternalsVisibleTo Include="Lodestar.Extensions.VectorData.Tests" />
    <!-- Same suite, replayed against the netstandard2.0 build. -->
    <InternalsVisibleTo Include="Lodestar.Extensions.VectorData.NetStandard.Tests" />
  </ItemGroup>

</Project>
```

- [ ] **Step 4: Write the failing test for the options type**

`tests/Lodestar.Extensions.VectorData.Tests/LodestarVectorStoreOptionsTests.cs`:

```csharp
using Lodestar.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class LodestarVectorStoreOptionsTests
{
    [Fact]
    public void Defaults_take_RankFusion_s_own_k()
    {
        var options = new LodestarVectorStoreOptions();

        Assert.Equal(60, options.RankFusionK);
        Assert.Null(options.Vectorizer);
        Assert.Null(options.Bm25);
    }

    [Fact]
    public void A_non_positive_fusion_k_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new LodestarVectorStoreOptions { RankFusionK = 0 });
    }
}
```

- [ ] **Step 5: Write the test csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <IsPackable>false</IsPackable>
    <GenerateDocumentationFile>false</GenerateDocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
    <PackageReference Include="Microsoft.Testing.Extensions.TrxReport" />
    <PackageReference Include="xunit.v3" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="../../src/Lodestar.Extensions.VectorData/Lodestar.Extensions.VectorData.csproj" />
  </ItemGroup>

  <!-- The reference gate reads the published pages and the map beside the assembly,
       the way the oracle corpora already are. -->
  <ItemGroup>
    <Compile Include="../Shared/ReferenceDocumentation.cs" Link="Documentation/ReferenceDocumentation.cs" />
    <None Include="../../docs/reference/extensions-vectordata/**/*.md" CopyToOutputDirectory="PreserveNewest"
          LinkBase="reference" />
    <None Include="../../docs/wiki-map.json" CopyToOutputDirectory="PreserveNewest" />
    <None Include="../../docs/**/*.md" Exclude="../../docs/superpowers/**"
          CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
  </ItemGroup>

</Project>
```

- [ ] **Step 6: Write the mirror csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <!--
    Replays the entire Lodestar.Extensions.VectorData.Tests suite against the
    *netstandard2.0* build of the library, instead of the net10.0 one the original
    project references.

    netstandard2.0 is a contract, not a runtime, so the tests cannot run *on* it.
    They run on net10.0 — identical host — and only the assembly under test
    changes. Without this, the assemblies shipped to .NET Framework, Mono and
    Unity consumers are compile-verified but never executed.

    The test sources are linked, never copied: one suite, two builds.
  -->

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <IsPackable>false</IsPackable>
    <GenerateDocumentationFile>false</GenerateDocumentationFile>
    <AssemblyName>Lodestar.Extensions.VectorData.NetStandard.Tests</AssemblyName>
    <RootNamespace>Lodestar.Extensions.VectorData.Tests</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
    <PackageReference Include="Microsoft.Testing.Extensions.TrxReport" />
    <PackageReference Include="xunit.v3" />
  </ItemGroup>

  <!--
    SetTargetFramework is what pins the reference to the netstandard2.0 build.

    Lodestar.Embeddings and Lodestar.Text are listed explicitly, and that is not
    redundant. Lodestar.Extensions.VectorData reaches both through a
    PackageReference, and SetTargetFramework does not travel across one: NuGet
    resolves package assets against *this* project's target framework (net10.0),
    so the transitive dependencies would come from lib/net10.0 and the mirror
    would stop mirroring — silently, since a guard that only inspected the
    Lodestar.Extensions.VectorData assembly would still pass. A direct
    ProjectReference takes precedence over the package of the same id, which
    restores the netstandard2.0 build; the extended guard proves it (#529).
  -->
  <ItemGroup>
    <ProjectReference Include="../../src/Lodestar.Extensions.VectorData/Lodestar.Extensions.VectorData.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Embeddings/Lodestar.Embeddings.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
    <ProjectReference Include="../../src/Lodestar.Text/Lodestar.Text.csproj"
                      SetTargetFramework="TargetFramework=netstandard2.0" />
  </ItemGroup>

  <ItemGroup>
    <Compile Include="../Lodestar.Extensions.VectorData.Tests/**/*.cs"
             Exclude="../Lodestar.Extensions.VectorData.Tests/bin/**;../Lodestar.Extensions.VectorData.Tests/obj/**"
             Link="%(RecursiveDir)%(Filename)%(Extension)" />
  </ItemGroup>

  <!-- The gate's engine is shared by every package's suite, so it is linked rather
       than copied; the pages and the map are read from the output directory. -->
  <ItemGroup>
    <Compile Include="../Shared/ReferenceDocumentation.cs" Link="Documentation/ReferenceDocumentation.cs" />
    <None Include="../../docs/reference/extensions-vectordata/**/*.md" CopyToOutputDirectory="PreserveNewest"
          LinkBase="reference" />
    <None Include="../../docs/wiki-map.json" CopyToOutputDirectory="PreserveNewest" />
    <None Include="../../docs/**/*.md" Exclude="../../docs/superpowers/**"
          CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
  </ItemGroup>

</Project>
```

- [ ] **Step 7: Write the mirror's assembly guard**

`tests/Lodestar.Extensions.VectorData.NetStandard.Tests/NetStandardAssemblyGuardTests.cs`:

```csharp
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
```

- [ ] **Step 8: Run the test to verify it fails**

```bash
"$MAIN/.dotnet-guarded" dotnet build src/Lodestar.Extensions.VectorData -c Release
```

Expected: FAIL — `LodestarVectorStoreOptions` does not exist.

- [ ] **Step 9: Write the options type**

`src/Lodestar.Extensions.VectorData/LodestarVectorStoreOptions.cs`:

```csharp
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// <summary>How a collection builds its two derived indexes, and how it fuses them.</summary>
/// <remarks>
/// All three are the defaults of the members they configure, so a collection constructed
/// without options behaves as <c>CountVectorizer</c>, <c>Bm25Index</c> and
/// <c>RankFusion.Rrf</c> do on their own.
/// </remarks>
public sealed class LodestarVectorStoreOptions
{
    private readonly int _rankFusionK = RankFusion.DefaultK;

    /// <summary>How the full-text property is tokenized and counted; <see langword="null"/> takes the defaults.</summary>
    public CountVectorizerOptions? Vectorizer { get; init; }

    /// <summary>The BM25 saturation and length-normalisation settings; <see langword="null"/> takes the defaults.</summary>
    public Bm25Options? Bm25 { get; init; }

    /// <summary>The rank offset reciprocal-rank fusion uses; 60 by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not positive.</exception>
    public int RankFusionK
    {
        get => _rankFusionK;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value,
                    "The rank offset is positive; zero would divide by zero at rank zero.");
            }

            _rankFusionK = value;
        }
    }
}
```

- [ ] **Step 10: Add both projects to the solution**

In `Lodestar.slnx`, insert in alphabetical position among the `src/` entries (after `src/Lodestar.Extensions.AI/...`, line 15 today):

```xml
    <Project Path="src/Lodestar.Extensions.VectorData/Lodestar.Extensions.VectorData.csproj" />
```

and among the `tests/` entries (after the two `Lodestar.Extensions.AI` test entries, lines 38–39 today):

```xml
    <Project Path="tests/Lodestar.Extensions.VectorData.NetStandard.Tests/Lodestar.Extensions.VectorData.NetStandard.Tests.csproj" />
    <Project Path="tests/Lodestar.Extensions.VectorData.Tests/Lodestar.Extensions.VectorData.Tests.csproj" />
```

- [ ] **Step 11: Teach the nuspec gate the new package and the raised floor**

In `tools/check_nuspec_dependencies.py`, add the two constants beside the others (near line 76–82):

```python
EXTENSIONS_VECTORDATA = "Lodestar.Extensions.VectorData"
MS_VECTORDATA = "Microsoft.Extensions.VectorData.Abstractions"
```

Change `TEXT_FLOOR` on line 94 from `"0.4.0"` to `"0.6.0"` — the comment above it already says it must equal `Directory.Packages.props`' `PackageVersion`:

```python
TEXT_FLOOR = "0.6.0"
```

And add the package's entry to `EXPECTED`, after `EXTENSIONS_AI`:

```python
    EXTENSIONS_VECTORDATA: {
        # The third satellite. Two Lodestar edges, one per half of hybrid search:
        # Embeddings holds the vectors, Text holds the BM25 index and the fusion.
        # Its one external dependency pins the same Microsoft.Extensions.AI.Abstractions
        # 10.10.0 that Lodestar.Extensions.AI already carries, so the two agree.
        NET: {EMBEDDINGS: EMBEDDINGS_FLOOR, TEXT: TEXT_FLOOR, MS_VECTORDATA: "10.10.0"},
        NETSTANDARD: {
            EMBEDDINGS: EMBEDDINGS_FLOOR,
            TEXT: TEXT_FLOOR,
            MS_VECTORDATA: "10.10.0",
            **POLYFILLS,
        },
    },
```

- [ ] **Step 12: Teach the version-floor gate the new consumers**

In `tools/check_version_floor.py`, extend the two `Floor` entries this package consumes (lines 69 and 72–73):

```python
    Floor("Lodestar.Text", "LodestarTextVersion", "TEXT_FLOOR",
          ("Lodestar.Fuzzy", "Lodestar.Extensions.VectorData")),
```

```python
    Floor("Lodestar.Embeddings", "LodestarEmbeddingsVersion", "EMBEDDINGS_FLOOR",
          ("Lodestar.Onnx", "Lodestar.Extensions.AI", "Lodestar.Extensions.VectorData")),
```

- [ ] **Step 13: Update CLAUDE.md's table, tier sentence and edge count**

Add a row to the package table, after the `Lodestar.Extensions.MathNet` row:

```markdown
| `Lodestar.Extensions.VectorData` | interop | an in-process `VectorStore` with hybrid keyword and vector search over `EmbeddingIndex` and `Bm25Index`; carries `Microsoft.Extensions.VectorData.Abstractions`. |
```

Change the opening architecture sentence from "Sixteen independently versioned packages" to "Seventeen independently versioned packages".

Replace the edge paragraph's count and list — it reads "The edges: **ten**" today:

```markdown
The edges: **twelve**, all asserted per target framework and per version range —
`Text`, `Decomposition` and `Extensions.MathNet` → `Abstractions`; `Fuzzy` → `Text`;
`Onnx` → `Embeddings`; `Extensions.AI` → `Embeddings` and `Onnx`;
`Extensions.VectorData` → `Embeddings` and `Text`; `Stats.Regression` →
`Stats` and `Decomposition`; `Survival` → `Stats`. `tools/check_nuspec_dependencies.py`'s
`EXPECTED` is the authority, and the count above is checked against it.
```

- [ ] **Step 14: Run the guards and the build**

```bash
"$MAIN/.dotnet-guarded" dotnet build Lodestar.slnx -c Release
python3 tools/check_claude_md_packages.py
python3 tools/check_version_floor.py
python3 tools/check_netstandard_guards.py
python3 tools/check_comment_length.py
```

Expected: build succeeds with 0 warnings; every guard prints `ok`. `check_claude_md_packages.py` derives the edge count from the nuspec gate's `EXPECTED`, so a mismatch between Step 11 and Step 13 fails here.

- [ ] **Step 15: Run the two new suites**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.NetStandard.Tests -c Release
```

Expected: 2 tests pass in the first, 5 in the second (its own 3 guards plus the 2 linked).

- [ ] **Step 16: Commit**

```bash
git add src/Lodestar.Extensions.VectorData tests/Lodestar.Extensions.VectorData.Tests \
        tests/Lodestar.Extensions.VectorData.NetStandard.Tests \
        src/Directory.Packages.props Lodestar.slnx tools/check_nuspec_dependencies.py \
        tools/check_version_floor.py CLAUDE.md
git commit -m "Create the VectorData satellite, and raise the one floor its keyword half needs

Decision 0100 parked this package on Lodestar.Text publishing Bm25Index; 0.6.0
did. Central Package Management makes that floor repository-wide, so the raise
from 0.4.0 reaches Lodestar.Fuzzy too and is asserted in both guards. The
Lodestar.Embeddings floor does not move: EmbeddingIndex, FromOwnedBlock and
BlockNormalization are all present at the pinned 0.5.0."
```

---

### Task 2: Reading a record's schema

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/Internal/RecordSchema.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/RecordSchemaTests.cs`

**Interfaces:**

- Consumes: nothing from earlier tasks.
- Produces: `internal sealed class RecordSchema<TKey, TRecord> where TKey : notnull where TRecord : class`, with `static RecordSchema<TKey, TRecord> Create(VectorStoreCollectionDefinition? definition)`, `TKey KeyOf(TRecord record)`, `ReadOnlyMemory<float> VectorOf(TRecord record)`, `string FullTextOf(TRecord record)`, `int Dimension { get; }`, `bool HasFullText { get; }`.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/RecordSchemaTests.cs`:

```csharp
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>A record carrying all three kinds of property, as a consumer writes one.</summary>
public sealed class Document
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>The same, with nothing marked for full-text search.</summary>
public sealed class VectorOnly
{
    [VectorStoreKey]
    public int Id { get; set; }

    [VectorStoreVector(2)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

public sealed class RecordSchemaTests
{
    [Fact]
    public void Attributes_name_the_key_the_vector_and_the_text()
    {
        RecordSchema<string, Document> schema = RecordSchema<string, Document>.Create(null);
        var record = new Document { Id = "a", Text = "the cat sat", Embedding = new float[] { 1f, 0f, 0f } };

        Assert.Equal("a", schema.KeyOf(record));
        Assert.Equal("the cat sat", schema.FullTextOf(record));
        Assert.Equal(3, schema.Dimension);
        Assert.True(schema.HasFullText);
        Assert.Equal(1f, schema.VectorOf(record).Span[0]);
    }

    [Fact]
    public void A_record_with_no_full_text_property_says_so_rather_than_throwing()
    {
        RecordSchema<int, VectorOnly> schema = RecordSchema<int, VectorOnly>.Create(null);

        Assert.False(schema.HasFullText);
        Assert.Equal(2, schema.Dimension);
    }

    [Fact]
    public void A_definition_overrides_the_attributes()
    {
        var definition = new VectorStoreCollectionDefinition
        {
            Properties =
            [
                new VectorStoreKeyProperty("Id", typeof(string)),
                new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3),
            ],
        };

        RecordSchema<string, Document> schema = RecordSchema<string, Document>.Create(definition);

        // The definition names no full-text property, so the collection has no keyword half
        // even though the attribute on Text says otherwise.
        Assert.False(schema.HasFullText);
    }

    [Fact]
    public void A_record_with_no_key_is_refused_by_name()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RecordSchema<string, string>.Create(null));

        Assert.Contains("VectorStoreKey", error.Message, StringComparison.Ordinal);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~RecordSchema"
```

Expected: FAIL — `RecordSchema` does not exist.

- [ ] **Step 3: Write the schema reader**

`src/Lodestar.Extensions.VectorData/Internal/RecordSchema.cs`:

```csharp
using System.Reflection;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>Where a record keeps its key, its vector and the text the keyword half indexes.</summary>
/// <remarks>
/// Read once per collection and held, because reflection per record would dominate every
/// rebuild. A definition, when one is given, is the whole answer: it replaces the attributes
/// rather than adding to them, so a caller who describes a schema at runtime gets exactly
/// what they described.
/// </remarks>
internal sealed class RecordSchema<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private const string KeyAttribute = "VectorStoreKey";

    private readonly PropertyInfo _key;
    private readonly PropertyInfo _vector;
    private readonly PropertyInfo? _fullText;

    private RecordSchema(PropertyInfo key, PropertyInfo vector, PropertyInfo? fullText, int dimension)
    {
        _key = key;
        _vector = vector;
        _fullText = fullText;
        Dimension = dimension;
    }

    /// <summary>The vector length every record in the collection must carry.</summary>
    public int Dimension { get; }

    /// <summary>Whether a property was marked for full-text search, which is what BM25 needs.</summary>
    public bool HasFullText => _fullText is not null;

    /// <summary>Reads the schema from <paramref name="definition"/> when one is given, else from the attributes.</summary>
    /// <param name="definition">An explicit description, or <see langword="null"/> to read the attributes.</param>
    /// <exception cref="ArgumentException">No key property, no vector property, or a vector of a type other than <c>ReadOnlyMemory&lt;float&gt;</c>.</exception>
    public static RecordSchema<TKey, TRecord> Create(VectorStoreCollectionDefinition? definition)
    {
        PropertyInfo[] properties = typeof(TRecord).GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        return definition is null ? FromAttributes(properties) : FromDefinition(properties, definition);
    }

    /// <summary>The record's key, as the dictionary keys it.</summary>
    public TKey KeyOf(TRecord record) => (TKey)_key.GetValue(record)!;

    /// <summary>The record's vector, which the index holds a copy of.</summary>
    public ReadOnlyMemory<float> VectorOf(TRecord record) =>
        (ReadOnlyMemory<float>)_vector.GetValue(record)!;

    /// <summary>The text BM25 indexes, or the empty string when the record leaves it null.</summary>
    /// <remarks>Never called when <see cref="HasFullText"/> is false.</remarks>
    public string FullTextOf(TRecord record) => (string?)_fullText!.GetValue(record) ?? string.Empty;

    private static RecordSchema<TKey, TRecord> FromAttributes(PropertyInfo[] properties)
    {
        PropertyInfo key = Single(properties, p => p.GetCustomAttribute<VectorStoreKeyAttribute>() is not null, KeyAttribute);
        PropertyInfo vector = Single(properties, p => p.GetCustomAttribute<VectorStoreVectorAttribute>() is not null, "VectorStoreVector");
        PropertyInfo? text = Array.Find(properties, p =>
            p.GetCustomAttribute<VectorStoreDataAttribute>() is { IsFullTextIndexed: true });

        int dimension = vector.GetCustomAttribute<VectorStoreVectorAttribute>()!.Dimensions;
        return Build(key, vector, text, dimension);
    }

    private static RecordSchema<TKey, TRecord> FromDefinition(
        PropertyInfo[] properties, VectorStoreCollectionDefinition definition)
    {
        VectorStoreKeyProperty key = definition.Properties.OfType<VectorStoreKeyProperty>().FirstOrDefault()
            ?? throw new ArgumentException(
                $"The definition names no key property; one {KeyAttribute} property or one "
                + "VectorStoreKeyProperty is what a record is addressed by.", nameof(definition));
        VectorStoreVectorProperty vector = definition.Properties.OfType<VectorStoreVectorProperty>().FirstOrDefault()
            ?? throw new ArgumentException(
                "The definition names no vector property, and a vector store searches by vector.",
                nameof(definition));
        VectorStoreDataProperty? text = definition.Properties.OfType<VectorStoreDataProperty>()
            .FirstOrDefault(p => p.IsFullTextIndexed);

        return Build(
            Named(properties, key.Name),
            Named(properties, vector.Name),
            text is null ? null : Named(properties, text.Name),
            (int)(vector.Dimensions ?? 0));
    }

    private static RecordSchema<TKey, TRecord> Build(
        PropertyInfo key, PropertyInfo vector, PropertyInfo? text, int dimension)
    {
        if (vector.PropertyType != typeof(ReadOnlyMemory<float>))
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{vector.Name} is {vector.PropertyType.Name}; this store "
                + "holds ReadOnlyMemory<float>, which is what EmbeddingIndex takes.", nameof(vector));
        }

        if (dimension < 1)
        {
            throw new ArgumentException(
                $"{typeof(TRecord).Name}.{vector.Name} declares no dimension, and the index is "
                + "built to a fixed width.", nameof(vector));
        }

        return new RecordSchema<TKey, TRecord>(key, vector, text, dimension);
    }

    private static PropertyInfo Named(PropertyInfo[] properties, string name) =>
        Array.Find(properties, p => p.Name == name)
        ?? throw new ArgumentException(
            $"The definition names {name}, which {typeof(TRecord).Name} does not declare.", nameof(name));

    private static PropertyInfo Single(PropertyInfo[] properties, Func<PropertyInfo, bool> match, string attribute) =>
        Array.Find(properties, new Predicate<PropertyInfo>(match))
        ?? throw new ArgumentException(
            $"{typeof(TRecord).Name} carries no [{attribute}] property.", nameof(properties));
}
```

- [ ] **Step 4: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~RecordSchema"
```

Expected: PASS, 4 tests.

- [ ] **Step 5: Commit**

```bash
git add src/Lodestar.Extensions.VectorData/Internal/RecordSchema.cs \
        tests/Lodestar.Extensions.VectorData.Tests/RecordSchemaTests.cs
git commit -m "Read a record's key, vector and indexed text once per collection

A definition replaces the attributes rather than adding to them, so a caller who
describes a schema at runtime gets what they described and nothing the type
happens to be decorated with. A record with no full-text property is a
collection with no keyword half, which is a state to report rather than refuse."
```

---

### Task 3: The rebuild

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/Internal/DerivedIndexes.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/DerivedIndexesTests.cs`

**Interfaces:**

- Consumes: `RecordSchema<TKey, TRecord>` from Task 2 — `KeyOf`, `VectorOf`, `FullTextOf`, `Dimension`, `HasFullText`.
- Produces: `internal sealed class DerivedIndexes<TKey> where TKey : notnull`, with `EmbeddingIndex Vectors { get; }`, `Bm25Index? Keywords { get; }`, `CountVectorizer? Vectorizer { get; }`, `IReadOnlyList<TKey> Keys { get; }`, and `static DerivedIndexes<TKey> Build<TRecord>(IReadOnlyCollection<TRecord> records, RecordSchema<TKey, TRecord> schema, LodestarVectorStoreOptions options) where TRecord : class`.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/DerivedIndexesTests.cs`:

```csharp
using Lodestar.Embeddings.Search;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class DerivedIndexesTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    [Fact]
    public void The_block_holds_one_row_per_record_in_key_order()
    {
        Document[] records =
        [
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
        ];

        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(2, built.Vectors.Count);
        Assert.Equal(3, built.Vectors.Dimension);
        Assert.Equal(["a", "b"], built.Keys);
    }

    [Fact]
    public void The_keyword_half_is_built_over_the_same_documents()
    {
        Document[] records =
        [
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
        ];

        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.NotNull(built.Keywords);
        Assert.NotNull(built.Vectorizer);
        Assert.Equal(2, built.Keywords!.DocumentCount);
    }

    [Fact]
    public void A_record_type_with_no_full_text_property_builds_vectors_alone()
    {
        VectorOnly[] records =
        [
            new() { Id = 1, Embedding = new float[] { 1f, 0f } },
        ];

        DerivedIndexes<int> built = DerivedIndexes<int>.Build(
            records, RecordSchema<int, VectorOnly>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(1, built.Vectors.Count);
        Assert.Null(built.Keywords);
        Assert.Null(built.Vectorizer);
    }

    [Fact]
    public void An_empty_collection_builds_an_empty_index_rather_than_throwing()
    {
        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            [], RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(0, built.Vectors.Count);
        Assert.Null(built.Keywords);
        Assert.Empty(built.Keys);
    }

    [Fact]
    public void A_record_whose_vector_is_the_wrong_width_is_refused_by_name()
    {
        Document[] records = [Doc("a", "the cat sat", 1f, 0f)];

        ArgumentException error = Assert.Throws<ArgumentException>(() => DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions()));

        Assert.Contains("a", error.Message, StringComparison.Ordinal);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~DerivedIndexes"
```

Expected: FAIL — `DerivedIndexes` does not exist.

- [ ] **Step 3: Write the rebuild**

`src/Lodestar.Extensions.VectorData/Internal/DerivedIndexes.cs`:

```csharp
using Lodestar.Abstractions;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// <summary>The three structures a collection's records are searched through.</summary>
/// <remarks>
/// <para>
/// Built whole, never mutated. <c>EmbeddingIndex</c> only appends and <c>Bm25Index</c> takes
/// no document at all, so a store that must honour delete and replace keeps its records and
/// rebuilds these from them. One rebuild produces all three, because they share an index:
/// position <c>i</c> of the block, row <c>i</c> of the count matrix and <c>Keys[i]</c> are
/// the same record.
/// </para>
/// </remarks>
internal sealed class DerivedIndexes<TKey>
    where TKey : notnull
{
    private DerivedIndexes(EmbeddingIndex vectors, Bm25Index? keywords, CountVectorizer? vectorizer, TKey[] keys)
    {
        Vectors = vectors;
        Keywords = keywords;
        Vectorizer = vectorizer;
        Keys = keys;
    }

    /// <summary>The vector half.</summary>
    public EmbeddingIndex Vectors { get; }

    /// <summary>The keyword half, or <see langword="null"/> when no property is full-text indexed.</summary>
    public Bm25Index? Keywords { get; }

    /// <summary>The vocabulary the keyword half was fitted on, so a query transforms against it.</summary>
    public CountVectorizer? Vectorizer { get; }

    /// <summary>The key at each index position, which is how a hit becomes a record.</summary>
    public IReadOnlyList<TKey> Keys { get; }

    /// <summary>Builds all three from the records as they stand.</summary>
    /// <param name="records">The collection's records, in the order the indexes will report.</param>
    /// <param name="schema">Where each record keeps its key, vector and text.</param>
    /// <param name="options">How the keyword half is tokenized and scored.</param>
    /// <exception cref="ArgumentException">A record's vector is not <see cref="RecordSchema{TKey, TRecord}.Dimension"/> long.</exception>
    public static DerivedIndexes<TKey> Build<TRecord>(
        IReadOnlyCollection<TRecord> records,
        RecordSchema<TKey, TRecord> schema,
        LodestarVectorStoreOptions options)
        where TRecord : class
    {
        int dimension = schema.Dimension;
        float[] block = new float[records.Count * dimension];
        TKey[] keys = new TKey[records.Count];
        List<string> documents = schema.HasFullText ? new List<string>(records.Count) : [];

        int row = 0;
        foreach (TRecord record in records)
        {
            TKey key = schema.KeyOf(record);
            ReadOnlySpan<float> vector = schema.VectorOf(record).Span;
            if (vector.Length != dimension)
            {
                throw new ArgumentException(
                    $"The record keyed {key} carries a vector of {vector.Length} where this "
                    + $"collection is {dimension} wide.", nameof(records));
            }

            vector.CopyTo(block.AsSpan(row * dimension, dimension));
            keys[row] = key;
            if (schema.HasFullText)
            {
                documents.Add(schema.FullTextOf(record));
            }

            row++;
        }

        // FromOwnedBlock rather than Count calls to Add: the whole block is known here, and
        // the array is this method's own, so handing it over costs no second copy.
        EmbeddingIndex vectors = EmbeddingIndex.FromOwnedBlock(
            block, dimension, BlockNormalization.Normalize);

        if (documents.Count == 0)
        {
            return new DerivedIndexes<TKey>(vectors, null, null, keys);
        }

        var vectorizer = new CountVectorizer(options.Vectorizer);
        CsrMatrix counts = vectorizer.FitTransform(documents);
        return new DerivedIndexes<TKey>(vectors, new Bm25Index(counts, options.Bm25), vectorizer, keys);
    }
}
```

- [ ] **Step 4: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~DerivedIndexes"
```

Expected: PASS, 5 tests. If `new CountVectorizer(options.Vectorizer)` does not compile, read the constructor on `src/Lodestar.Text/Vectorization/CountVectorizer.cs` and pass the options the way it takes them; do not change the `LodestarVectorStoreOptions` surface to suit it.

- [ ] **Step 5: Commit**

```bash
git add src/Lodestar.Extensions.VectorData/Internal/DerivedIndexes.cs \
        tests/Lodestar.Extensions.VectorData.Tests/DerivedIndexesTests.cs
git commit -m "Build the vector index, the BM25 index and the key map in one pass

They share an index by construction -- position i of the block, row i of the
count matrix and Keys[i] are the same record -- which is what lets a hit from
either half become a record without a lookup table per half."
```

---

### Task 4: The collection's state, its writes, and its reads by key

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/CollectionWriteTests.cs`

**Interfaces:**

- Consumes: `RecordSchema<TKey, TRecord>.Create`, `DerivedIndexes<TKey>.Build`, `LodestarVectorStoreOptions`.
- Produces: `public sealed class LodestarVectorStoreCollection<TKey, TRecord> : VectorStoreCollection<TKey, TRecord> where TKey : notnull where TRecord : class`, constructed as `new LodestarVectorStoreCollection<TKey, TRecord>(string name, LodestarVectorStoreOptions? options = null, VectorStoreCollectionDefinition? definition = null)`; `internal int RebuildCount { get; }`; `internal DerivedIndexes<TKey> Current()` returning the caches, rebuilding first if dirty. Task 5 and Task 6 add `SearchAsync` and `HybridSearchAsync` to this same class.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/CollectionWriteTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class CollectionWriteTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    private static LodestarVectorStoreCollection<string, Document> Collection() => new("documents");

    [Fact]
    public async Task An_upserted_record_comes_back_by_key()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));

        Document? found = await collection.GetAsync("a");

        Assert.NotNull(found);
        Assert.Equal("the cat sat", found!.Text);
    }

    [Fact]
    public async Task Upserting_the_same_key_replaces_rather_than_duplicates()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        await collection.UpsertAsync(Doc("a", "the dog ran", 0f, 1f, 0f));

        Document? found = await collection.GetAsync("a");

        Assert.Equal("the dog ran", found!.Text);
        Assert.Equal(1, collection.Current().Vectors.Count);
    }

    [Fact]
    public async Task A_deleted_record_leaves_neither_a_record_nor_a_vector()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        await collection.UpsertAsync(Doc("b", "the dog ran", 0f, 1f, 0f));

        await collection.DeleteAsync("a");

        Assert.Null(await collection.GetAsync("a"));
        Assert.Equal(1, collection.Current().Vectors.Count);
        Assert.Equal(["b"], collection.Current().Keys);
    }

    [Fact]
    public async Task Deleting_a_key_that_is_not_there_is_not_an_error()
    {
        using var collection = Collection();

        await collection.DeleteAsync("missing");

        Assert.Equal(0, collection.Current().Vectors.Count);
    }

    [Fact]
    public async Task A_batch_of_writes_rebuilds_once()
    {
        using var collection = Collection();
        await collection.UpsertAsync([
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
            Doc("c", "the bird flew", 0f, 0f, 1f),
        ]);

        collection.Current();
        collection.Current();

        Assert.Equal(1, collection.RebuildCount);
    }

    [Fact]
    public async Task A_write_after_a_read_rebuilds_again()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));
        collection.Current();

        await collection.UpsertAsync(Doc("b", "the dog ran", 0f, 1f, 0f));
        collection.Current();

        Assert.Equal(2, collection.RebuildCount);
    }

    [Fact]
    public async Task The_collection_exists_once_it_has_been_ensured()
    {
        using var collection = Collection();

        Assert.False(await collection.CollectionExistsAsync());
        await collection.EnsureCollectionExistsAsync();
        Assert.True(await collection.CollectionExistsAsync());

        await collection.EnsureCollectionDeletedAsync();
        Assert.False(await collection.CollectionExistsAsync());
    }

    [Fact]
    public async Task Deleting_the_collection_drops_its_records()
    {
        using var collection = Collection();
        await collection.UpsertAsync(Doc("a", "the cat sat", 1f, 0f, 0f));

        await collection.EnsureCollectionDeletedAsync();

        Assert.Null(await collection.GetAsync("a"));
    }

    [Fact]
    public void The_collection_reports_the_name_it_was_given()
    {
        using var collection = Collection();

        Assert.Equal("documents", collection.Name);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~CollectionWrite"
```

Expected: FAIL — `LodestarVectorStoreCollection` does not exist.

- [ ] **Step 3: Write the collection's state and write path**

`src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs`. This file grows in Tasks 5 and 6; write it now with the members below and nothing else, leaving the abstract search members overridden by throwing `NotSupportedException` so the type compiles — Task 5 replaces `SearchAsync`, Task 6 adds the hybrid interface.

```csharp
using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process collection: the records are the state, the indexes are caches.</summary>
/// <typeparam name="TKey">The key type the record's key property carries.</typeparam>
/// <typeparam name="TRecord">The record type, whose schema is read once on construction.</typeparam>
/// <remarks>
/// <para>
/// A write updates the dictionary and marks the caches stale; the first read after it rebuilds
/// them. So a batch of writes costs one rebuild rather than one per record, and an updated
/// record's superseded vector is gone rather than masked — which is the case an append-with-
/// tombstones store answers wrongly.
/// </para>
/// <para>
/// Not thread-safe for concurrent writes, as an in-memory collection built for one process is
/// not a database.
/// </para>
/// </remarks>
public sealed class LodestarVectorStoreCollection<TKey, TRecord> : VectorStoreCollection<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private readonly Dictionary<TKey, TRecord> _records = [];
    private readonly RecordSchema<TKey, TRecord> _schema;
    private readonly LodestarVectorStoreOptions _options;
    private DerivedIndexes<TKey>? _indexes;
    private bool _exists;

    /// <summary>Creates a collection over a record type the schema is read from.</summary>
    /// <param name="name">The collection's name, which <see cref="Name"/> reports.</param>
    /// <param name="options">How the keyword half is built and fused; <see langword="null"/> takes the defaults.</param>
    /// <param name="definition">An explicit schema, or <see langword="null"/> to read <typeparamref name="TRecord"/>'s attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException"><typeparamref name="TRecord"/> declares no key or no vector property.</exception>
    public LodestarVectorStoreCollection(
        string name,
        LodestarVectorStoreOptions? options = null,
        VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);
        Name = name;
        _options = options ?? new LodestarVectorStoreOptions();
        _schema = RecordSchema<TKey, TRecord>.Create(definition);
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>How many times the caches have been rebuilt, which the suite asserts on.</summary>
    internal int RebuildCount { get; private set; }

    /// <summary>The caches, rebuilt first when a write has happened since the last read.</summary>
    internal DerivedIndexes<TKey> Current()
    {
        if (_indexes is null)
        {
            _indexes = DerivedIndexes<TKey>.Build(_records.Values, _schema, _options);
            RebuildCount++;
        }

        return _indexes;
    }

    /// <summary>The schema this collection reads its records through.</summary>
    internal RecordSchema<TKey, TRecord> Schema => _schema;

    /// <summary>The options the keyword half and the fusion were configured with.</summary>
    internal LodestarVectorStoreOptions Options => _options;

    /// <inheritdoc />
    public override Task<bool> CollectionExistsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_exists);

    /// <inheritdoc />
    public override Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default)
    {
        _exists = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default)
    {
        _exists = false;
        _records.Clear();
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task UpsertAsync(TRecord record, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(record);
        _records[_schema.KeyOf(record)] = record;
        _exists = true;
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task UpsertAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(records);
        foreach (TRecord record in records)
        {
            _records[_schema.KeyOf(record)] = record;
        }

        _exists = true;
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (_records.Remove(key))
        {
            Invalidate();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        bool removed = false;
        foreach (TKey key in keys)
        {
            removed |= _records.Remove(key);
        }

        if (removed)
        {
            Invalidate();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task<TRecord?> GetAsync(
        TKey key, RecordRetrievalOptions? options = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(_records.TryGetValue(key, out TRecord? record) ? record : null);

    /// <inheritdoc />
    public override async IAsyncEnumerable<TRecord> GetAsync(
        IEnumerable<TKey> keys,
        RecordRetrievalOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        foreach (TKey key in keys)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_records.TryGetValue(key, out TRecord? record))
            {
                yield return record;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType == typeof(VectorStoreCollectionMetadata) && serviceKey is null
            ? new VectorStoreCollectionMetadata { VectorStoreSystemName = "lodestar", CollectionName = Name }
            : null;

    /// <inheritdoc />
    public override void Dispose()
    {
        // Nothing owns an unmanaged handle here; the base class declares it and a
        // consumer's `using` has to reach something.
    }

    private void Invalidate() => _indexes = null;
}
```

- [ ] **Step 4: Add the two members the interface still demands, temporarily refusing**

Append to the same class, so the type is concrete and Task 4 can be tested. Task 5 replaces the first and Task 6 supplies the second.

```csharp
    /// <inheritdoc />
    public override IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<TRecord>? options = null,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Vector search arrives in the next task.");

    /// <inheritdoc />
    public override IAsyncEnumerable<TRecord> GetAsync(
        System.Linq.Expressions.Expression<Func<TRecord, bool>> filter,
        int top,
        FilteredRecordRetrievalOptions<TRecord>? options = null,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Filtered retrieval arrives with the filter, in the next task.");
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~CollectionWrite"
```

Expected: PASS, 9 tests. The abstract member list is the eleven read from the assembly on 2026-09-12: `CollectionExistsAsync`, `DeleteAsync` ×2, `EnsureCollectionDeletedAsync`, `EnsureCollectionExistsAsync`, `GetAsync` ×3, `GetService`, `Name`, `SearchAsync`, `UpsertAsync` ×2. If the compiler names one this file does not override, override it — do not change the test.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs \
        tests/Lodestar.Extensions.VectorData.Tests/CollectionWriteTests.cs
git commit -m "Hold the records and rebuild the indexes on the first read after a write

One rebuild per batch rather than per record, because the flag is set and not
counted. Re-upserting a key replaces the record outright, so its superseded
vector is gone rather than masked -- the case a tombstone store gets wrong."
```

---

### Task 5: Filters, and exact vector search

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/Internal/RecordFilter.cs`
- Modify: `src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs` — replace the two members added in Task 4 Step 4
- Create: `tests/Lodestar.Extensions.VectorData.Tests/SearchTests.cs`

**Interfaces:**

- Consumes: `Current()`, `Schema`, `Options` from Task 4; `DerivedIndexes<TKey>.Vectors` and `.Keys` from Task 3.
- Produces: `internal static class RecordFilter` with `static Func<TRecord, bool>? Compile<TRecord>(Expression<Func<TRecord, bool>>? filter)`; a working `SearchAsync` and filtered `GetAsync` on the collection.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/SearchTests.cs`:

```csharp
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class SearchTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    private static async Task<LodestarVectorStoreCollection<string, Document>> Seeded()
    {
        var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
            Doc("c", "a bird flew over the park", 0f, 0f, 1f),
        ]);
        return collection;
    }

    [Fact]
    public async Task The_nearest_vector_comes_first()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection
            .SearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), 2).ToListAsync();

        Assert.Equal(2, hits.Count);
        Assert.Equal("a", hits[0].Record.Id);
    }

    [Fact]
    public async Task A_filter_that_admits_two_of_three_still_returns_two_when_top_is_five()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Text.Contains("park") })
            .ToListAsync();

        Assert.Equal(2, hits.Count);
        Assert.All(hits, hit => Assert.Contains("park", hit.Record.Text, StringComparison.Ordinal));
    }

    [Fact]
    public async Task A_filter_that_admits_nothing_returns_nothing()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            5,
            new VectorSearchOptions<Document> { Filter = d => d.Id == "absent" })
            .ToListAsync();

        Assert.Empty(hits);
    }

    [Fact]
    public async Task Skip_drops_the_leading_hits_rather_than_the_trailing_ones()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<VectorSearchResult<Document>> hits = await collection.SearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            1,
            new VectorSearchOptions<Document> { Skip = 1 })
            .ToListAsync();

        Assert.Single(hits);
        Assert.NotEqual("a", hits[0].Record.Id);
    }

    [Fact]
    public async Task A_string_search_value_is_refused_with_the_reason()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.SearchAsync("the cat", 2).ToListAsync());

        Assert.Contains("vector", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Filtered_retrieval_returns_the_matching_records()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await Seeded();

        List<Document> found = await collection.GetAsync(d => d.Text.Contains("park"), 5).ToListAsync();

        Assert.Equal(2, found.Count);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~SearchTests"
```

Expected: FAIL — `NotSupportedException("Vector search arrives in the next task.")`.

- [ ] **Step 3: Write the filter compiler**

`src/Lodestar.Extensions.VectorData/Internal/RecordFilter.cs`:

```csharp
using System.Linq.Expressions;

namespace Lodestar.Extensions.VectorData;

/// <summary>Turns the abstraction's filter expression into something callable.</summary>
/// <remarks>
/// Compiled once per call rather than per record, and never cached across calls: a caller who
/// builds a fresh closure each time would otherwise hold every one of them alive.
/// <c>Expression.Compile</c> needs dynamic code, which is what makes this package's filter path
/// unavailable under trimming and ahead-of-time compilation.
/// </remarks>
internal static class RecordFilter
{
    /// <summary>Compiles the filter, or answers null when there is none to apply.</summary>
    /// <param name="filter">The caller's predicate over the record type.</param>
    public static Func<TRecord, bool>? Compile<TRecord>(Expression<Func<TRecord, bool>>? filter) =>
        filter?.Compile();
}
```

- [ ] **Step 4: Replace the two refusing members with the real ones**

In `src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs`, delete the two members from Task 4 Step 4 and put these in their place:

```csharp
    /// <inheritdoc />
    /// <exception cref="NotSupportedException"><paramref name="searchValue"/> is not a vector, and this package generates none.</exception>
    /// <remarks>
    /// <para>
    /// With a filter, every record is scored and the filter runs before the cut, so
    /// <paramref name="top"/> means <paramref name="top"/>: a caller asking for five matching
    /// records gets five whenever five match. Post-filtering a top-k would return fewer
    /// without saying why.
    /// </para>
    /// </remarks>
    public override async IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotLessThan(top, 1);
        ReadOnlyMemory<float> query = AsVector(searchValue);
        VectorSearchOptions<TRecord> settings = options ?? new VectorSearchOptions<TRecord>();
        Func<TRecord, bool>? admits = RecordFilter.Compile(settings.Filter);
        DerivedIndexes<TKey> indexes = Current();

        // With a filter the whole collection is scored, because the records the filter keeps
        // are not known before it runs and a short list would silently return too few.
        int wanted = admits is null ? top + settings.Skip : indexes.Vectors.Count;
        int skipped = 0;
        int taken = 0;

        foreach (SearchResult hit in Scored(indexes, query, wanted))
        {
            cancellationToken.ThrowIfCancellationRequested();
            TRecord record = _records[indexes.Keys[hit.Index]];
            if (admits is not null && !admits(record))
            {
                continue;
            }

            if (settings.ScoreThreshold is { } threshold && hit.Score < threshold)
            {
                continue;
            }

            if (skipped < settings.Skip)
            {
                skipped++;
                continue;
            }

            yield return new VectorSearchResult<TRecord>(record, hit.Score);
            if (++taken == top)
            {
                break;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async IAsyncEnumerable<TRecord> GetAsync(
        Expression<Func<TRecord, bool>> filter,
        int top,
        FilteredRecordRetrievalOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotNull(filter);
        Guard.NotLessThan(top, 1);
        Func<TRecord, bool> admits = filter.Compile();
        int taken = 0;

        foreach (TRecord record in _records.Values)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!admits(record))
            {
                continue;
            }

            yield return record;
            if (++taken == top)
            {
                break;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    private static IReadOnlyList<SearchResult> Scored(
        DerivedIndexes<TKey> indexes, ReadOnlyMemory<float> query, int wanted) =>
        indexes.Vectors.Count == 0
            ? []
            : indexes.Vectors.Search(query.Span, Math.Min(wanted, indexes.Vectors.Count));

    private static ReadOnlyMemory<float> AsVector<TInput>(TInput searchValue)
        where TInput : notnull => searchValue switch
    {
        ReadOnlyMemory<float> memory => memory,
        float[] array => array,
        _ => throw new NotSupportedException(
            $"{typeof(TInput).Name} is not a vector, and this package generates none: supply a "
            + "ReadOnlyMemory<float> or a float[], or embed the text with Lodestar.Extensions.AI first."),
    };
```

Add the two usings this needs at the top of the file, beside the existing ones:

```csharp
using System.Linq.Expressions;
using Lodestar.Embeddings.Search;
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~SearchTests"
```

Expected: PASS, 6 tests. If `ToListAsync` is unavailable, the suite needs `System.Linq.Async`; do not add it — write a local `static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)` helper in a `Fixtures/AsyncEnumerableExtensions.cs` in the test project instead, since a test-only dependency would also land in the mirror.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Extensions.VectorData tests/Lodestar.Extensions.VectorData.Tests/SearchTests.cs
git commit -m "Search exactly under a filter, so top means top

A filter makes the search score the whole collection, because which records
survive is not known before it runs. Post-filtering a top-k is cheaper and
returns fewer results than asked for without saying so, which reads as a bug."
```

---

### Task 6: Hybrid search

**Files:**

- Modify: `src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/HybridSearchTests.cs`

**Interfaces:**

- Consumes: `DerivedIndexes<TKey>.Keywords`, `.Vectorizer`, `.Vectors`, `.Keys`; `LodestarVectorStoreOptions.RankFusionK`.
- Produces: `LodestarVectorStoreCollection<TKey, TRecord>` additionally implements `IKeywordHybridSearchable<TRecord>`.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/HybridSearchTests.cs`:

```csharp
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class HybridSearchTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    [Fact]
    public async Task The_fusion_surfaces_a_record_only_the_keyword_half_ranks_first()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            // "a" is nearest the query vector and says nothing about elephants.
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
            // "c" is furthest from the query vector and is the only elephant document.
            Doc("c", "an elephant crossed the river", 0f, 0f, 1f),
        ]);

        IKeywordHybridSearchable<Document> hybrid = collection;
        List<VectorSearchResult<Document>> hits = await hybrid
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 3)
            .ToListAsync();

        // Vector search alone would never put "c" this high; keyword search alone would never
        // rank "a" at all. Both appearing is what says the two rankings were fused.
        Assert.Contains(hits, hit => hit.Record.Id == "c");
        Assert.Contains(hits, hit => hit.Record.Id == "a");
    }

    [Fact]
    public async Task A_keyword_outside_the_vocabulary_contributes_nothing_and_does_not_throw()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "the dog ran in the park", 0f, 1f, 0f),
        ]);

        IKeywordHybridSearchable<Document> hybrid = collection;
        List<VectorSearchResult<Document>> hits = await hybrid
            .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), ["zebra"], 2)
            .ToListAsync();

        Assert.Equal(2, hits.Count);
        Assert.Equal("a", hits[0].Record.Id);
    }

    [Fact]
    public async Task A_collection_with_no_full_text_property_refuses_hybrid_search_by_name()
    {
        using var collection = new LodestarVectorStoreCollection<int, VectorOnly>("vectors");
        await collection.UpsertAsync(new VectorOnly { Id = 1, Embedding = new float[] { 1f, 0f } });

        IKeywordHybridSearchable<VectorOnly> hybrid = collection;
        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await hybrid
                .HybridSearchAsync(new ReadOnlyMemory<float>([1f, 0f]), ["anything"], 1)
                .ToListAsync());

        Assert.Contains("IsFullTextIndexed", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_filter_applies_to_the_fused_ranking_too()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync([
            Doc("a", "the cat sat on the mat", 1f, 0f, 0f),
            Doc("b", "an elephant in the park", 0f, 1f, 0f),
            Doc("c", "an elephant crossed the river", 0f, 0f, 1f),
        ]);

        IKeywordHybridSearchable<Document> hybrid = collection;
        List<VectorSearchResult<Document>> hits = await hybrid.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]),
            ["elephant"],
            3,
            new HybridSearchOptions<Document> { Filter = d => d.Id != "c" })
            .ToListAsync();

        Assert.DoesNotContain(hits, hit => hit.Record.Id == "c");
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~HybridSearch"
```

Expected: FAIL — the collection does not implement `IKeywordHybridSearchable<TRecord>`.

- [ ] **Step 3: Implement the hybrid half**

Change the class declaration to add the interface:

```csharp
public sealed class LodestarVectorStoreCollection<TKey, TRecord>
    : VectorStoreCollection<TKey, TRecord>, IKeywordHybridSearchable<TRecord>
```

Add these usings beside the existing ones:

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
```

And append the member:

```csharp
    /// <summary>Fuses the vector ranking with a BM25 ranking over the keywords.</summary>
    /// <typeparam name="TInput">The search value's type; a vector, since this package generates none.</typeparam>
    /// <param name="searchValue">The query vector.</param>
    /// <param name="keywords">The terms the keyword half scores, taken as one query document.</param>
    /// <param name="top">How many fused results to return.</param>
    /// <param name="options">A filter and a skip, applied to the fused ranking.</param>
    /// <param name="cancellationToken">Checked between results.</param>
    /// <exception cref="NotSupportedException">The record type marks no <c>IsFullTextIndexed</c> property, or the search value is not a vector.</exception>
    /// <remarks>
    /// <para>
    /// The keywords are transformed against the vocabulary the index was fitted on, so a term
    /// the collection has never seen scores nothing rather than failing — which is what BM25
    /// means by an unseen term. Fusion is reciprocal rank at
    /// <see cref="LodestarVectorStoreOptions.RankFusionK"/>.
    /// </para>
    /// </remarks>
    public async IAsyncEnumerable<VectorSearchResult<TRecord>> HybridSearchAsync<TInput>(
        TInput searchValue,
        ICollection<string> keywords,
        int top,
        HybridSearchOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TInput : notnull
    {
        Guard.NotNull(keywords);
        Guard.NotLessThan(top, 1);
        ReadOnlyMemory<float> query = AsVector(searchValue);
        DerivedIndexes<TKey> indexes = Current();

        if (indexes.Keywords is null || indexes.Vectorizer is null)
        {
            throw new NotSupportedException(
                $"{typeof(TRecord).Name} marks no property [VectorStoreData(IsFullTextIndexed = true)], "
                + "so this collection has no keyword half to fuse with.");
        }

        HybridSearchOptions<TRecord> settings = options ?? new HybridSearchOptions<TRecord>();
        Func<TRecord, bool>? admits = RecordFilter.Compile(settings.Filter);

        int[] byVector = [.. Scored(indexes, query, indexes.Vectors.Count).Select(hit => hit.Index)];
        int[] byKeyword = [.. indexes.Keywords
            .Top(QueryTerms(indexes.Vectorizer, keywords), indexes.Keywords.DocumentCount)
            .Select(hit => hit.Document)];

        int skipped = 0;
        int taken = 0;
        foreach (SearchHit fused in RankFusion.Rrf([byVector, byKeyword], Options.RankFusionK))
        {
            cancellationToken.ThrowIfCancellationRequested();
            TRecord record = _records[indexes.Keys[fused.Document]];
            if (admits is not null && !admits(record))
            {
                continue;
            }

            if (skipped < settings.Skip)
            {
                skipped++;
                continue;
            }

            yield return new VectorSearchResult<TRecord>(record, fused.Score);
            if (++taken == top)
            {
                break;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>The keywords as column indices of the fitted vocabulary, unseen terms dropped.</summary>
    private static IEnumerable<int> QueryTerms(CountVectorizer vectorizer, ICollection<string> keywords)
    {
        CsrMatrix row = vectorizer.Transform([string.Join(" ", keywords)]);
        return row.RowCount == 0 ? [] : row.ColumnIndicesOf(0);
    }
```

- [ ] **Step 4: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~HybridSearch"
```

Expected: PASS, 4 tests. `CsrMatrix.ColumnIndicesOf` is the one member in this plan not read from the assembly beforehand — if it does not exist, read `src/Lodestar.Abstractions/CsrMatrix.cs` for the member that enumerates a row's non-zero column indices and use that; if none exists, walk the matrix's row pointers and column indices directly rather than adding a member to `Lodestar.Abstractions`, which is on a published floor and would need a release.

- [ ] **Step 5: Commit**

```bash
git add src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs \
        tests/Lodestar.Extensions.VectorData.Tests/HybridSearchTests.cs
git commit -m "Fuse the vector and keyword rankings, which is what this package is for

The assertion that proves the fusion happened is a record only one half ranks:
a document furthest from the query vector and the only one holding the keyword
has to surface, and a document no keyword matches has to stay."
```

---

### Task 7: The store

**Files:**

- Create: `src/Lodestar.Extensions.VectorData/LodestarVectorStore.cs`
- Create: `tests/Lodestar.Extensions.VectorData.Tests/StoreTests.cs`

**Interfaces:**

- Consumes: `LodestarVectorStoreCollection<TKey, TRecord>`, `LodestarVectorStoreOptions`.
- Produces: `public sealed class LodestarVectorStore : VectorStore`, constructed as `new LodestarVectorStore(LodestarVectorStoreOptions? options = null)`.

- [ ] **Step 1: Write the failing tests**

`tests/Lodestar.Extensions.VectorData.Tests/StoreTests.cs`:

```csharp
using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class StoreTests
{
    [Fact]
    public void The_same_name_returns_the_same_collection()
    {
        using var store = new LodestarVectorStore();

        VectorStoreCollection<string, Document> first = store.GetCollection<string, Document>("documents");
        VectorStoreCollection<string, Document> second = store.GetCollection<string, Document>("documents");

        Assert.Same(first, second);
    }

    [Fact]
    public async Task A_collection_appears_in_the_listing_once_it_exists()
    {
        using var store = new LodestarVectorStore();
        VectorStoreCollection<string, Document> collection = store.GetCollection<string, Document>("documents");

        Assert.Empty(await store.ListCollectionNamesAsync().ToListAsync());

        await collection.EnsureCollectionExistsAsync();

        Assert.Equal(["documents"], await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public async Task The_store_reports_whether_a_collection_exists()
    {
        using var store = new LodestarVectorStore();
        await store.GetCollection<string, Document>("documents").EnsureCollectionExistsAsync();

        Assert.True(await store.CollectionExistsAsync("documents"));
        Assert.False(await store.CollectionExistsAsync("absent"));
    }

    [Fact]
    public async Task Ensuring_a_collection_deleted_removes_it_from_the_listing()
    {
        using var store = new LodestarVectorStore();
        await store.GetCollection<string, Document>("documents").EnsureCollectionExistsAsync();

        await store.EnsureCollectionDeletedAsync("documents");

        Assert.Empty(await store.ListCollectionNamesAsync().ToListAsync());
    }

    [Fact]
    public void A_dynamic_collection_is_refused_with_its_reason()
    {
        using var store = new LodestarVectorStore();
        var definition = new VectorStoreCollectionDefinition
        {
            Properties = [new VectorStoreKeyProperty("Id", typeof(string))],
        };

        NotSupportedException error = Assert.Throws<NotSupportedException>(
            () => store.GetDynamicCollection("documents", definition));

        Assert.Contains("typed", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Asking_for_one_name_under_two_record_types_is_refused()
    {
        using var store = new LodestarVectorStore();
        store.GetCollection<string, Document>("documents");

        Assert.Throws<ArgumentException>(() => store.GetCollection<int, VectorOnly>("documents"));
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release --filter "FullyQualifiedName~StoreTests"
```

Expected: FAIL — `LodestarVectorStore` does not exist.

- [ ] **Step 3: Write the store**

`src/Lodestar.Extensions.VectorData/LodestarVectorStore.cs`:

```csharp
using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process vector store: named collections, held for the store's lifetime.</summary>
/// <remarks>
/// Nothing here reaches a network or a file. A collection exists once something has ensured it
/// or written to it, which is what <see cref="ListCollectionNamesAsync"/> reports — asking for
/// a collection by name does not create it, the way asking a database for a table does not.
/// </remarks>
public sealed class LodestarVectorStore : VectorStore
{
    private readonly Dictionary<string, object> _collections = [];
    private readonly LodestarVectorStoreOptions _options;

    /// <summary>Creates a store whose collections all take the same options.</summary>
    /// <param name="options">How each collection builds its keyword half; <see langword="null"/> takes the defaults.</param>
    public LodestarVectorStore(LodestarVectorStoreOptions? options = null) =>
        _options = options ?? new LodestarVectorStoreOptions();

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="name"/> is already held under a different key or record type.</exception>
    public override VectorStoreCollection<TKey, TRecord> GetCollection<TKey, TRecord>(
        string name, VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);
        if (_collections.TryGetValue(name, out object? existing))
        {
            return existing as LodestarVectorStoreCollection<TKey, TRecord>
                ?? throw new ArgumentException(
                    $"The collection {name} is already held over a different key or record type; "
                    + "one name is one schema.", nameof(name));
        }

        var created = new LodestarVectorStoreCollection<TKey, TRecord>(name, _options, definition);
        _collections[name] = created;
        return created;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Always: this store has no dynamic path.</exception>
    /// <remarks>
    /// A dynamic record is a dictionary, and the filter this store compiles is an expression over
    /// a record's properties — there are none to compile against. Refusing says so; answering
    /// with a collection that then refused every filter would not.
    /// </remarks>
    public override VectorStoreCollection<object, Dictionary<string, object?>> GetDynamicCollection(
        string name, VectorStoreCollectionDefinition definition) =>
        throw new NotSupportedException(
            "This store serves typed records only. Its filters compile an expression over a "
            + "record's properties, which a dictionary does not have.");

    /// <inheritdoc />
    public override async IAsyncEnumerable<string> ListCollectionNamesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (KeyValuePair<string, object> entry in _collections)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry.Value is IExistingCollection { Exists: true })
            {
                yield return entry.Key;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override Task<bool> CollectionExistsAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(_collections.TryGetValue(name, out object? held)
            && held is IExistingCollection { Exists: true });

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_collections.TryGetValue(name, out object? held) && held is IExistingCollection collection)
        {
            return collection.EnsureDeletedAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType == typeof(VectorStoreMetadata) && serviceKey is null
            ? new VectorStoreMetadata { VectorStoreSystemName = "lodestar" }
            : null;

    /// <inheritdoc />
    public override void Dispose()
    {
        foreach (object collection in _collections.Values)
        {
            (collection as IDisposable)?.Dispose();
        }

        _collections.Clear();
    }
}

/// <summary>What the store needs of a collection without knowing its type arguments.</summary>
/// <remarks>
/// The store holds collections as <see cref="object"/>, since each closes the generic
/// differently. This is the narrow surface that lets it answer about existence anyway.
/// </remarks>
internal interface IExistingCollection
{
    /// <summary>Whether the collection has been created or written to.</summary>
    bool Exists { get; }

    /// <summary>Drops the collection and its records.</summary>
    Task EnsureDeletedAsync(CancellationToken cancellationToken);
}
```

- [ ] **Step 4: Implement the bridge on the collection**

In `src/Lodestar.Extensions.VectorData/LodestarVectorStoreCollection.cs`, add the interface to the declaration:

```csharp
public sealed class LodestarVectorStoreCollection<TKey, TRecord>
    : VectorStoreCollection<TKey, TRecord>, IKeywordHybridSearchable<TRecord>, IExistingCollection
```

and the two explicit members at the end of the class:

```csharp
    /// <inheritdoc />
    bool IExistingCollection.Exists => _exists;

    /// <inheritdoc />
    Task IExistingCollection.EnsureDeletedAsync(CancellationToken cancellationToken) =>
        EnsureCollectionDeletedAsync(cancellationToken);
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
"$MAIN/.dotnet-guarded" dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release
```

Expected: PASS, 30 tests in the suite and 33 in the mirror.

- [ ] **Step 6: Commit**

```bash
git add src/Lodestar.Extensions.VectorData tests/Lodestar.Extensions.VectorData.Tests/StoreTests.cs
git commit -m "Hold the collections by name, and refuse the dynamic one with its reason

Asking for a collection does not create it, the way asking a database for a
table does not: it exists once something ensures it or writes to it. The
dynamic path refuses rather than returning a collection that would refuse every
filter, since a dictionary has no properties to compile an expression against."
```

---

### Task 8: The documents this package owes

**Files:**

- Create: `docs/reference/extensions-vectordata/store.md` and one page per public member under `docs/reference/extensions-vectordata/store/`
- Modify: `docs/wiki-map.json`, `docs/equivalence.md`, `docs/migration/semantic-kernel.md`, `CHANGELOG.md`, `README.md`
- Modify: `.github/workflows/release.yml`
- Create: `docs/decisions/<number>-<slug>.md`, then regenerate `docs/decisions/index.yaml` and hand-edit `docs/decisions/README.md`

**Interfaces:**

- Consumes: every public member from Tasks 1 and 4 to 7.
- Produces: the pages the reference gate reads.

- [ ] **Step 1: Add the package to the wiki map**

In `docs/wiki-map.json`, inside `packages`, after `Lodestar.Extensions.AI`:

```json
  "Lodestar.Extensions.VectorData": {
    "wiki": "ExtensionsVectorData",
    "pages": [
      "docs/reference/extensions-vectordata/*.md",
      "docs/reference/extensions-vectordata/*/*.md"
    ],
    "covered": {
      "Lodestar.Extensions.VectorData": "docs/reference/extensions-vectordata/store"
    },
    "exceptionsUnchecked": []
  },
```

- [ ] **Step 2: Write the reference pages**

One page per public member, in the layout the other packages use — read
`docs/reference/extensions-ai/` for the exact heading structure and the
`<!-- docs-declaration -->` block the gate compares against the assembly. The members owed are:
`LodestarVectorStore`, its constructor, `GetCollection`, `GetDynamicCollection`,
`ListCollectionNamesAsync`, `CollectionExistsAsync`, `EnsureCollectionDeletedAsync`, `GetService`,
`Dispose`; `LodestarVectorStoreCollection<TKey, TRecord>`, its constructor, `Name`, both
`UpsertAsync`, both `DeleteAsync`, all three `GetAsync`, `SearchAsync`, `HybridSearchAsync`,
`CollectionExistsAsync`, `EnsureCollectionExistsAsync`, `EnsureCollectionDeletedAsync`,
`GetService`, `Dispose`; `LodestarVectorStoreOptions`, `Vectorizer`, `Bm25`, `RankFusionK`.

Every ` ```csharp ` fence is compiled and a trailing `// =>` is executed as an assertion, so write
the examples against the real API and run the gate in Step 7 rather than trusting them.

Write the declaration blocks by copying what the assembly reports rather than what this plan says:
the gate compares against both target frameworks' assemblies, and it emits neither `?` nor
`override` — a literal `?` in a declaration block is what failed the gate on
[#695](https://github.com/CyrilB1531/lodestar/pull/695).

- [ ] **Step 3: Write the equivalence row**

In `docs/equivalence.md`, in the table this package belongs to, add:

```markdown
| `Microsoft.Extensions.VectorData` provider conformance | — (no Python reference) | [`LodestarVectorStore`](reference/extensions-vectordata/store/lodestarvectorstore.md) | **There is no Python call to map.** This is an interface adapter, so conformance is proven by driving the abstractions as a consumer does — constructing a store, upserting typed records, searching, filtering and fusing — rather than by replaying a frozen corpus. Two deliberate refusals: `GetDynamicCollection` throws, because a dictionary record has no properties for the compiled filter to bind to, and a `string` search value throws, because this package generates no embeddings. `HybridSearchAsync` fuses a vector ranking with a BM25 ranking through reciprocal rank at `k = 60`. |
```

- [ ] **Step 4: Write the migration row**

Create `docs/migration/semantic-kernel.md` if it does not exist, following the shape of
`docs/migration/sklearn.md`, with a row saying what a caller uses today (a hosted vector database
behind a Semantic Kernel connector) and what this replaces it with for the in-process case.

- [ ] **Step 5: Write the changelog entry**

In `CHANGELOG.md`, under `## [Unreleased]`, a new section:

```markdown
### Lodestar.Extensions.VectorData

#### Added

- **The package.** An in-process `Microsoft.Extensions.VectorData` provider: `LodestarVectorStore`,
  `LodestarVectorStoreCollection<TKey, TRecord>` and `LodestarVectorStoreOptions`. Records are the
  collection's state and the vector and BM25 indexes are caches rebuilt on the first read after a
  write, so upsert and delete are exact rather than masked. `HybridSearchAsync` fuses both halves
  through reciprocal rank, which is hybrid retrieval with no database and no service running
  anywhere. `GetDynamicCollection` and a `string` search value are refused, each naming its reason.
  ([#682](https://github.com/CyrilB1531/lodestar/issues/682))
```

- [ ] **Step 6: Write the ADR**

```bash
"$MAIN/.next-adr"
```

Use the number it prints. The record carries Decisions 1 to 5 of the spec and the shared-floor
consequence, with frontmatter `applies: ["0100"]` — 0100 is applied unchanged, not amended, because
the 10.10.0 reading matched its 10.9.0 one on every count. Then:

```bash
python3 tools/regen_adr_index.py
```

and hand-edit `docs/decisions/README.md`: a table row in number order, and **both** spelled-out
counts in the `## What accepted means here` paragraph.

- [ ] **Step 7: Add the package to the README lists and the release workflow**

`README.md`'s package table and its pack loop, and `.github/workflows/release.yml`'s package list.
Run the three guards, which name exactly what is missing:

```bash
python3 tools/check_readme_packages.py
python3 tools/check_readme_pack_loop.py
python3 tools/check_release_workflow_packages.py
```

- [ ] **Step 8: Run the documentation gates**

```bash
python3 tools/extract_doc_snippets.py
"$MAIN/.dotnet-guarded" dotnet build samples/Lodestar.DocSnippets -c Release
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
"$MAIN/.venv-oracles/bin/python" -m pytest tools/tests -q
```

Expected: the snippet project builds, markdownlint reports 0 issues, pytest passes. The reference
gate itself runs inside the two test suites, so Task 9's full run is what proves it.

- [ ] **Step 9: Commit**

```bash
git add docs README.md CHANGELOG.md .github/workflows/release.yml
git commit -m "Document the satellite, and record the decision that built it

Decision 0100 is applied rather than amended: the 10.10.0 reading matched its
10.9.0 one on all seven counts, so its placement argument holds untouched. What
the new record adds is the consequence 0100 did not price -- a floor under
Central Package Management is repository-wide."
```

---

### Task 9: The sample, the packaging gate, and the whole suite

**Files:**

- Create: `samples/Lodestar.Sample/LodestarVectorStoreSample.cs`
- Modify: `samples/Lodestar.Sample/Program.cs`, `samples/Lodestar.Sample/Lodestar.Sample.csproj`
- Modify: `tools/check_sample_coverage.py`

- [ ] **Step 1: Add the package reference to the sample**

In `samples/Lodestar.Sample/Lodestar.Sample.csproj`, after the `Lodestar.Extensions.AI` line:

```xml
    <PackageReference Include="Lodestar.Extensions.VectorData" Version="$(LodestarExtensionsVectorDataVersion)" />
```

- [ ] **Step 2: Add the package to the coverage gate's converted list**

In `tools/check_sample_coverage.py`, add `"Lodestar.Extensions.VectorData"` to `CONVERTED`.

- [ ] **Step 3: Write the sample**

`samples/Lodestar.Sample/LodestarVectorStoreSample.cs`. It must reference **every public member**,
because the gate counts members and not types — the miss that cost
[#695](https://github.com/CyrilB1531/lodestar/pull/695) three CI rounds. Print through `Inv` so
the culture guard passes.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Sample;

/// <summary>An in-process hybrid vector store: no database, no service.</summary>
internal static class LodestarVectorStoreSample
{
    private sealed class Note
    {
        [VectorStoreKey]
        public string Id { get; set; } = string.Empty;

        [VectorStoreData(IsFullTextIndexed = true)]
        public string Text { get; set; } = string.Empty;

        [VectorStoreVector(3)]
        public ReadOnlyMemory<float> Embedding { get; set; }
    }

    public static async Task RunAsync()
    {
        Console.WriteLine("Vector store (Lodestar.Extensions.VectorData)");

        var options = new LodestarVectorStoreOptions { RankFusionK = 60 };
        Console.WriteLine($"  fusion k          : {options.RankFusionK}");
        Console.WriteLine($"  vectorizer / bm25 : {options.Vectorizer is null} / {options.Bm25 is null}");

        using var store = new LodestarVectorStore(options);
        VectorStoreCollection<string, Note> notes = store.GetCollection<string, Note>("notes");
        await notes.EnsureCollectionExistsAsync();

        await notes.UpsertAsync(new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } });
        await notes.UpsertAsync([
            new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
            new Note { Id = "c", Text = "an elephant crossed the river", Embedding = new float[] { 0f, 0f, 1f } },
        ]);

        Console.WriteLine($"  collection        : {notes.Name}, exists {await notes.CollectionExistsAsync()}");
        Console.WriteLine($"  store knows it    : {await store.CollectionExistsAsync("notes")}");

        Note? one = await notes.GetAsync("a");
        Console.WriteLine($"  by key            : {one?.Text}");

        await foreach (Note note in notes.GetAsync(["b", "c"]))
        {
            Console.WriteLine($"  by keys           : {note.Id}");
        }

        await foreach (Note note in notes.GetAsync(n => n.Text.Contains("park"), 2))
        {
            Console.WriteLine($"  by filter         : {note.Id}");
        }

        await foreach (VectorSearchResult<Note> hit in notes.SearchAsync(new ReadOnlyMemory<float>([1f, 0f, 0f]), 2))
        {
            Console.WriteLine($"  nearest           : {hit.Record.Id} at {Inv.F4(hit.Score ?? 0.0)}");
        }

        var hybrid = (IKeywordHybridSearchable<Note>)notes;
        await foreach (VectorSearchResult<Note> hit in hybrid.HybridSearchAsync(
            new ReadOnlyMemory<float>([1f, 0f, 0f]), ["elephant"], 2))
        {
            Console.WriteLine($"  fused             : {hit.Record.Id} at {Inv.F4(hit.Score ?? 0.0)}");
        }

        await foreach (string name in store.ListCollectionNamesAsync())
        {
            Console.WriteLine($"  listed            : {name}");
        }

        Console.WriteLine($"  metadata          : {notes.GetService(typeof(VectorStoreCollectionMetadata)) is not null}");
        Console.WriteLine($"  store metadata    : {store.GetService(typeof(VectorStoreMetadata)) is not null}");

        await notes.DeleteAsync("a");
        await notes.DeleteAsync(["b"]);
        await notes.EnsureCollectionDeletedAsync();
        await store.EnsureCollectionDeletedAsync("notes");
        notes.Dispose();
        Console.WriteLine();
    }
}
```

- [ ] **Step 4: Call it from Program.cs**

Add beside the other lots, in the position matching the package's place in the file's existing
ordering:

```csharp
await LodestarVectorStoreSample.RunAsync();
```

- [ ] **Step 5: Run the packaging gate the way ADR 0009 requires**

A fresh pack **and** an isolated `NUGET_PACKAGES`, or the sample judges the published packages
instead of the working tree:

```bash
cd "$(git rev-parse --show-toplevel)"
rm -rf ./artifacts
for p in src/Lodestar.Abstractions src/Lodestar.Text src/Lodestar.Embeddings src/Lodestar.Fuzzy \
         src/Lodestar.Metrics src/Lodestar.Conformal src/Lodestar.Decomposition src/Lodestar.Onnx \
         src/Lodestar.Cluster src/Lodestar.Preprocessing src/Lodestar.Stats \
         src/Lodestar.Stats.Regression src/Lodestar.Survival src/Lodestar.Extensions.AI \
         src/Lodestar.Extensions.MathNet src/Lodestar.Extensions.VectorData src/Lodestar.Gpu; do
  "$MAIN/.dotnet-guarded" dotnet pack "$p" -c Release -o ./artifacts
done
NUGET_PACKAGES="$(mktemp -d)" "$MAIN/.dotnet-guarded" \
  dotnet build samples/Lodestar.Sample -c Release
python3 tools/check_sample_coverage.py
python3 tools/check_sample_culture.py
```

Expected: `check_sample_coverage.py` reports every member covered. When it names members this
sample does not reference, add a line referencing each; do not add them to an exclusion list unless
the member is genuinely unreachable from a sample, and then give the exclusion a reason in the same
wording the existing `RecordPlumbing` entries use.

- [ ] **Step 6: Run everything**

```bash
cd "$(git rev-parse --show-toplevel)"
sh .githooks/pre-commit
"$MAIN/.dotnet-guarded" dotnet build Lodestar.slnx -c Release
"$MAIN/.dotnet-guarded" dotnet test Lodestar.slnx -c Release
"$MAIN/.venv-oracles/bin/python" -m pytest tools/tests -q
python3 tools/check_nuspec_dependencies.py
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

Expected: the pre-commit hook exits 0; the build reports 0 warnings and 0 errors; `dotnet test`
reports **34 assemblies** and 0 failures; `pytest tools/tests` passes; every guard prints `ok`;
markdownlint reports 0 issues.

Read the assembly count, not the colour. 32 means one of the two new suites did not run.

- [ ] **Step 7: Commit**

```bash
git add samples tools/check_sample_coverage.py
git commit -m "Exercise every member from the sample, which is what the gate counts

The gate counts exported members and not types, so a lot that constructs the
store and searches once leaves most of the surface unreferenced -- the miss that
cost #695 three CI rounds."
```

---

## Self-review

**Spec coverage.** Every section of the spec maps to a task: the two readings and the floor
analysis to Task 1 (the pins and both guards) and Task 8 (the ADR); Decision 1 to Tasks 3 and 4;
Decision 2 to Task 5; Decision 3 to Tasks 2 and 7; Decision 4 to Task 6; Decision 5 to the
`IAsyncEnumerable` members in Tasks 4 to 7; the package layout to Task 1; the testing list to the
suites in Tasks 2 to 7; the documentation list to Task 8; the packaging gate to Task 9.

The spec's eight-item testing list maps to: upsert-replaces and delete (Task 4), the rebuild count
(Task 4), the filter exactness (Task 5), the fusion (Task 6), the unseen keyword (Task 6), the two
refusals (Tasks 5 and 7), and the no-full-text collection (Tasks 2, 3 and 6).

**Two members this plan names without having read them from the assembly**, flagged where they are
used rather than left to surprise an implementer: `CountVectorizer`'s options constructor (Task 3
Step 4) and `CsrMatrix.ColumnIndicesOf` (Task 6 Step 4). Both steps say what to do if the member
differs, and both say not to change the public surface to accommodate it.

**Type consistency.** `RecordSchema<TKey, TRecord>` is spelled the same in Tasks 2, 3 and 4;
`DerivedIndexes<TKey>.Build<TRecord>` takes `(IReadOnlyCollection<TRecord>, RecordSchema<TKey,
TRecord>, LodestarVectorStoreOptions)` in Task 3 and is called that way in Task 4;
`RecordFilter.Compile` returns `Func<TRecord, bool>?` in Task 5 and is used as one in Tasks 5 and 6;
`IExistingCollection` is declared in Task 7 and implemented in the same task. `Current()`,
`Schema`, `Options` and `_records` are introduced in Task 4 and used in Tasks 5 and 6.
