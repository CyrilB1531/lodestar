# Lodestar.Extensions.MathNet.Tests

The suite for [`Lodestar.Extensions.MathNet`](../../src/Lodestar.Extensions.MathNet/README.md): the conversion to and from Math.NET's sparse matrix, both ways. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Extensions.MathNet.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Extensions.MathNet.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Extensions.MathNet.Tests -c Release
```
