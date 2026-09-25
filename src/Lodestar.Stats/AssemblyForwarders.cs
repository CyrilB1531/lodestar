using System.Runtime.CompilerServices;

// The thirteen data types this package declared until 0.4.x live in Lodestar.Abstractions under the
// same names (decision 0003, #1142); code built against 0.4.x still binds through these forwarders.

[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Alternative))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Center))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Continuity))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.ExactMethod))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.KendallVariant))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.NanPolicy))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.ProportionInterval))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Variance))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.ZeroMethod))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.AndersonResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Chi2ContingencyResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.KsResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TestResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.AndersonKSampleVariant))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.CorrelationMatrix))]
