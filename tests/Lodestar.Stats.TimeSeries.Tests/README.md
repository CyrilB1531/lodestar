# Lodestar.Stats.TimeSeries.Tests

The suite for [`Lodestar.Stats.TimeSeries`](../../src/Lodestar.Stats.TimeSeries/README.md): the autocorrelation functions, Ljung-Box, ADF, KPSS, seasonal decomposition and VAR. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Stats.TimeSeries.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Stats.TimeSeries.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Stats.TimeSeries.Tests -c Release
```
