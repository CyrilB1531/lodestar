# Lodestar.Abstractions.Tests

The suite for [`Lodestar.Abstractions`](../../src/Lodestar.Abstractions/README.md): `CsrMatrix`'s construction, validation, row norms and products, and the structural equality of the data types it holds. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Abstractions.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Abstractions.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Abstractions.Tests -c Release
```
