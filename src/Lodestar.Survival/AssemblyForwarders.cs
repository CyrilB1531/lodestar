using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Survival still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Survival.KaplanMeierCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.LogRankResult))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.NelsonAalenCurve))]
[assembly: TypeForwardedTo(typeof(Lodestar.Survival.SurvivalStep))]
