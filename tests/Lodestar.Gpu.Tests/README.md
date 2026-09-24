# Lodestar.Gpu.Tests

The suite for [`Lodestar.Gpu`](../../src/Lodestar.Gpu/README.md): each kernel on ILGPU's CPU accelerator, which CI forces, against the CPU path it replaces. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Gpu.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Gpu.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Gpu.Tests -c Release
```
