using System.Runtime.CompilerServices;

// Its data types live in Lodestar.Abstractions (decision 0003): those declared here before #1142 bind old
// callers through these, and those added since are forwarded so this assembly exports every type its API names.

[assembly: TypeForwardedTo(typeof(Lodestar.Survival.KaplanMeierCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.NelsonAalenCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.SurvivalStep))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankWeighting))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.PairwiseLogRankResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.RestrictedMeanResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.CoxBaseline))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.CoxTimeTransform))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.ParametricModel))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.AftModel))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.SurvivalCurve))]
