# Lodestar.Stats.Benchmarks

The hypothesis tests, distribution tails, regressions (OLS, WLS, GLS, GLM, multinomial logit)
and time-series diagnostics, against Accord.Statistics, Math.NET Numerics, Meta.Numerics and
Cortex.TimeSeries where each has the function.

## Run one benchmark

```bash
dotnet run -c Release --project bench/Lodestar.Stats.Benchmarks -- --filter '*Ols*'
```

How to measure (the corpora, the agreement checks run before any timing, the machine lock, and
what each class's numbers mean) is [`bench/README.md`](../README.md)'s subject. What was measured
lives in each package's `src/<Package>/performance.md`.
