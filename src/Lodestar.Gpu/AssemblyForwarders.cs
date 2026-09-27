using System.Runtime.CompilerServices;

// MinHashScheme, declared here before #1142, lives in Lodestar.Abstractions (decision 0003); code
// built against an earlier Lodestar.Gpu binds through this. GpuSearchResult left in #1214.

[assembly: TypeForwardedTo(typeof(Lodestar.Gpu.Compute.MinHashScheme))]
