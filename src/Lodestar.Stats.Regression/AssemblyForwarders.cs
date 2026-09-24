using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Stats.Regression still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.CovarianceType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.GlmFamily))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.GlmLink))]
