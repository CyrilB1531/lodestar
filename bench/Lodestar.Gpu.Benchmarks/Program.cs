using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Lodestar.Gpu.Benchmarks.TiledCosineTopKBenchmarks).Assembly)
    .Run(args);
