using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Stats.Regression still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.CovarianceType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.GlmFamily))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.GlmLink))]

// The instrumental-variables data types never lived here: they were declared in Lodestar.Abstractions (#1155).
// Forwarding them anyway makes this package, which names them, the one that documents them, as for the moved types.
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Instrumental.IvCovarianceType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Instrumental.IvDesign))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Instrumental.IvFirstStage))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.KernelType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Instrumental.IvOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Instrumental.IvSummary))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.WaldTest))]

// The panel data types, declared in Lodestar.Abstractions (#1156) and forwarded for the same reason.
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Panel.PanelCovarianceType))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Panel.PanelDesign))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Panel.PanelOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Stats.Regression.Panel.PanelSummary))]
