# Lodestar.NetStandard.Benchmarks

The same benchmark sources, linked rather than copied, run against the `netstandard2.0`
builds of the libraries on the same host. It answers what a .NET Framework, Mono or Unity caller
pays for the portable build, and it asserts in-process that the assemblies it loaded really are
the `netstandard2.0` ones.

## Run one benchmark

```bash
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*VectorMath*'
```

How to measure (the corpora, the agreement checks run before any timing, the machine lock, and
what each class's numbers mean) is [`bench/README.md`](../README.md)'s subject. What was measured
lives in each package's `src/<Package>/performance.md`.
