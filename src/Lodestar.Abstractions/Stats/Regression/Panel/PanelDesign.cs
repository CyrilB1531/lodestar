namespace Lodestar.Stats.Regression.Panel;

/// <summary>A panel's data: the response, the regressors, and which entity and period each row belongs to.</summary>
/// <remarks>
/// Rows may come in any order and the panel need not be balanced: the estimators sort by entity, then period. A
/// <see langword="ref struct"/>, so the spans are read in place and the design lives no longer than the call.
/// </remarks>
public readonly ref struct PanelDesign
{
    /// <summary>Describes the panel.</summary>
    /// <param name="response">One value per row.</param>
    /// <param name="exogenous">The regressors, row-major, <paramref name="exogenousCount"/> per row, with no constant column of your own.</param>
    /// <param name="exogenousCount">Columns in <paramref name="exogenous"/>, at least one.</param>
    /// <param name="entities">One entity label per row; any integers.</param>
    /// <param name="periods">One period label per row; any integers, ordered as time is.</param>
    public PanelDesign(
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> exogenous,
        int exogenousCount,
        ReadOnlySpan<int> entities,
        ReadOnlySpan<int> periods)
    {
        Response = response;
        Exogenous = exogenous;
        ExogenousCount = exogenousCount;
        Entities = entities;
        Periods = periods;
    }

    /// <summary>One value per row.</summary>
    public ReadOnlySpan<double> Response { get; }

    /// <summary>The regressors, row-major.</summary>
    public ReadOnlySpan<double> Exogenous { get; }

    /// <summary>Columns in <see cref="Exogenous"/>.</summary>
    public int ExogenousCount { get; }

    /// <summary>One entity label per row.</summary>
    public ReadOnlySpan<int> Entities { get; }

    /// <summary>One period label per row.</summary>
    public ReadOnlySpan<int> Periods { get; }
}
