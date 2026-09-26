namespace Lodestar.Survival;

/// <summary>The time scale the proportional hazards test correlates the scaled Schoenfeld residuals with; lifelines' <c>time_transform</c>.</summary>
public enum CoxTimeTransform
{
    /// <summary>The rank of each event among the fit's sorted subjects, lifelines' default <c>"rank"</c>.</summary>
    Rank,

    /// <summary>One less the Kaplan-Meier estimate at each event time, <c>"km"</c>, R's default.</summary>
    KaplanMeier,

    /// <summary>The event time itself, <c>"identity"</c>.</summary>
    Identity,

    /// <summary>Its logarithm, <c>"log"</c>.</summary>
    Log,
}
