using System.Runtime.CompilerServices;

// Fifteen types in Lodestar.Abstractions (decision 0003, #1142): thirteen once declared here, eight of which the 0.4.0
// binary binds to except where #1217 renamed them (Chi2ContingencyResult), and two #1162 declared there that never shipped here.

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
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.ChiSquaredContingencyResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.KsResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.TestResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.AndersonKSampleVariant))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.CorrelationMatrix))]
