using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Lodestar.Survival.Benchmarks.SurvivalBenchmarks).Assembly)
    .Run(args);
