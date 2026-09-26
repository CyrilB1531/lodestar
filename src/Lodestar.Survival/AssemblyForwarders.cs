using System.Runtime.CompilerServices;

// Its data types live in Lodestar.Abstractions (decision 0003): those declared here before #1142 bind old
// callers through these, and #1170's four are forwarded so this assembly exports every type its API names.

[assembly: TypeForwardedTo(typeof(Lodestar.Survival.KaplanMeierCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.NelsonAalenCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.SurvivalStep))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankWeighting))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.PairwiseLogRankResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.RestrictedMeanResult))]
