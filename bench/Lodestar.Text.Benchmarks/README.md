# Lodestar.Text.Benchmarks

Every benchmark of a `net10.0` library that is not the GPU's, the survival
package's or the statistics packages', whatever package it measures: distances, fuzzy matching,
vectorizers, tokenizers, embeddings search, metrics, clustering, preprocessing and decomposition,
each against the .NET incumbent where one exists.

## Run one benchmark

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Levenshtein*'
```

How to measure (the corpora, the agreement checks run before any timing, the machine lock, and
what each class's numbers mean) is [`bench/README.md`](../README.md)'s subject. What was measured
lives in each package's `src/<Package>/performance.md`.
