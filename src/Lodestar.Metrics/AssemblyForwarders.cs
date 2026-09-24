using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Metrics still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.AverageRow))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.Averaging))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.BinStrategy))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.ClassRow))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.KappaWeighting))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.MultiClassStrategy))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.Normalization))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.UndefinedMetricException))]
[assembly: TypeForwardedTo(typeof(Lodestar.Metrics.ZeroDivision))]
