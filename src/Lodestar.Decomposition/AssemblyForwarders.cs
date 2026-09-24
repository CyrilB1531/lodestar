using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Decomposition still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Decomposition.NmfBetaLoss))]
[assembly: TypeForwardedTo(typeof(Lodestar.Decomposition.NmfInitialization))]
[assembly: TypeForwardedTo(typeof(Lodestar.Decomposition.NmfOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Decomposition.PowerIterationNormalizer))]
[assembly: TypeForwardedTo(typeof(Lodestar.Decomposition.TruncatedSvdOptions))]
