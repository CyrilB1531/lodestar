namespace Lodestar.Survival;

/// <summary>How the log-rank family weighs each event time; lifelines' <c>weightings=</c>.</summary>
public enum LogRankWeighting
{
    /// <summary>Every time weighs one: the log-rank test itself, lifelines' <c>weightings=None</c>.</summary>
    LogRank,

    /// <summary>The pooled number at risk: Gehan-Breslow's generalised Wilcoxon test, <c>"wilcoxon"</c>.</summary>
    Wilcoxon,

    /// <summary>The square root of the pooled number at risk, <c>"tarone-ware"</c>.</summary>
    TaroneWare,

    /// <summary>Peto and Peto's modified survival estimate, <c>"peto"</c>.</summary>
    Peto,

    /// <summary><c>S^p (1 − S)^q</c> of the pooled Kaplan-Meier curve just before each time, <c>"fleming-harrington"</c>.</summary>
    FlemingHarrington,
}
