# Lodestar.Text.Tests

The suite for [`Lodestar.Text`](../../src/Lodestar.Text/README.md): distances, phonetics, set similarity, stemmers, tokenizers, vectorizers, persistence, BM25 search, keyword extraction and the BK-tree. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Text.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Text.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Text.Tests -c Release
```
