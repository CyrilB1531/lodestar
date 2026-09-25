namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>An instrumental-variables problem's data: the response and its three row-major blocks with their widths.</summary>
/// <remarks>
/// <c>linearmodels</c>' <c>IV2SLS(dependent, exog, endog, instruments)</c>, as spans rather than data frames. A
/// <see langword="ref struct"/>, so the spans are only read, never copied, and the design lives no longer than the call
/// it is passed to.
/// </remarks>
public readonly ref struct IvDesign
{
    /// <summary>Describes the data.</summary>
    /// <param name="response">One value per row.</param>
    /// <param name="exogenous">The exogenous regressors, row-major, <paramref name="exogenousCount"/> per row, with no constant column of your own; empty when there are none.</param>
    /// <param name="exogenousCount">Columns in <paramref name="exogenous"/>, zero or more.</param>
    /// <param name="endogenous">The endogenous regressors, row-major, <paramref name="endogenousCount"/> per row.</param>
    /// <param name="endogenousCount">Columns in <paramref name="endogenous"/>, at least one.</param>
    /// <param name="instruments">The excluded instruments, row-major, <paramref name="instrumentCount"/> per row.</param>
    /// <param name="instrumentCount">Columns in <paramref name="instruments"/>, at least <paramref name="endogenousCount"/>.</param>
    public IvDesign(
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> exogenous,
        int exogenousCount,
        ReadOnlySpan<double> endogenous,
        int endogenousCount,
        ReadOnlySpan<double> instruments,
        int instrumentCount)
    {
        Response = response;
        Exogenous = exogenous;
        ExogenousCount = exogenousCount;
        Endogenous = endogenous;
        EndogenousCount = endogenousCount;
        Instruments = instruments;
        InstrumentCount = instrumentCount;
    }

    /// <summary>One value per row.</summary>
    public ReadOnlySpan<double> Response { get; }

    /// <summary>The exogenous regressors, row-major.</summary>
    public ReadOnlySpan<double> Exogenous { get; }

    /// <summary>Columns in <see cref="Exogenous"/>.</summary>
    public int ExogenousCount { get; }

    /// <summary>The endogenous regressors, row-major.</summary>
    public ReadOnlySpan<double> Endogenous { get; }

    /// <summary>Columns in <see cref="Endogenous"/>.</summary>
    public int EndogenousCount { get; }

    /// <summary>The excluded instruments, row-major.</summary>
    public ReadOnlySpan<double> Instruments { get; }

    /// <summary>Columns in <see cref="Instruments"/>.</summary>
    public int InstrumentCount { get; }
}
