# Lodestar.Embeddings.Tests

The suite for [`Lodestar.Embeddings`](../../src/Lodestar.Embeddings/README.md): the WordPiece, SentencePiece and BPE tokenizers and their loaders, batch encoding, pooling, the embedding index and `.npy` persistence. Where a Python reference
exists, the tests replay its frozen values from [`tests/oracles/`](../oracles); the files a suite
reads are the ones its sources name:

```bash
grep -rhoE "[a-z0-9_]+\.json" tests/Lodestar.Embeddings.Tests --include=*.cs | sort -u
```

`Documentation/` holds the tests that read this package's reference pages under `docs/reference/`
against the assembly, which the reference gate relies on.

Its mirror, `tests/Lodestar.Embeddings.NetStandard.Tests`, runs these same sources against the
`netstandard` build; [`tests/README.md`](../README.md) explains the pair.

```bash
dotnet test tests/Lodestar.Embeddings.Tests -c Release
```
