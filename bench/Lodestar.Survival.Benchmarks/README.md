# Lodestar.Survival.Benchmarks

Kaplan-Meier, Nelson-Aalen, the log-rank test and the Cox model.

## Run one benchmark

```bash
dotnet run -c Release --project bench/Lodestar.Survival.Benchmarks -- --filter '*Survival*'
```

How to measure (the corpora, the agreement checks run before any timing, the machine lock, and
what each class's numbers mean) is [`bench/README.md`](../README.md)'s subject. What was measured
lives in each package's `src/<Package>/performance.md`.
