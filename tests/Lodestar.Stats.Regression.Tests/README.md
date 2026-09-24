# Lodestar.Stats.Regression.Tests

The suite for [`Lodestar.Stats.Regression`](../../src/Lodestar.Stats.Regression/README.md): OLS, WLS, GLS, the GLM families and links, and the multinomial logit, with their inference tables. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Stats.Regression.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Stats.Regression.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Stats.Regression.Tests -c Release
```
