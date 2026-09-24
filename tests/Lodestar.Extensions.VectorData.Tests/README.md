# Lodestar.Extensions.VectorData.Tests

The suite for [`Lodestar.Extensions.VectorData`](../../src/Lodestar.Extensions.VectorData/README.md): the vector store's collections, keyword, vector and hybrid search, and its contract checks. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Extensions.VectorData.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Extensions.VectorData.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Extensions.VectorData.Tests -c Release
```
