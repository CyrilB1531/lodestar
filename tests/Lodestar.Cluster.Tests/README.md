# Lodestar.Cluster.Tests

The suite for [`Lodestar.Cluster`](../../src/Lodestar.Cluster/README.md): k-means, DBSCAN and agglomerative clustering. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Cluster.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Cluster.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Cluster.Tests -c Release
```
