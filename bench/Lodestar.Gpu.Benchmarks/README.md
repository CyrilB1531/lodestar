# Lodestar.Gpu.Benchmarks

The ILGPU kernels against this repository's own CPU paths: cosine top-k, sparse-dense and
chained products, MinHash signatures and bit-parallel edit distance. A kernel ships only where it
passes this comparison.

## Run one benchmark

```bash
dotnet run -c Release --project bench/Lodestar.Gpu.Benchmarks -- --filter '*TiledCosineTopK*'
```

How to measure (the corpora, the agreement checks run before any timing, the machine lock, and
what each class's numbers mean) is [`bench/README.md`](../README.md)'s subject. What was measured
lives in each package's `src/<Package>/performance.md`.
