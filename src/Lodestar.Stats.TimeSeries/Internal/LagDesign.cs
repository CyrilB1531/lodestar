namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The stacked lags a vector autoregression is fitted on.</summary>
/// <remarks>
/// One row per usable observation, oldest first: the constant when asked, then lag 1's values for every variable, then
/// lag 2's, which is the row order <c>statsmodels</c>' <c>VARResults.params</c> prints.
/// </remarks>
internal static class LagDesign
{
    /// <summary>Builds the design and the responses it explains.</summary>
    /// <param name="series">The observations, row-major in time: <paramref name="variableCount"/> values each, oldest first.</param>
    /// <param name="variableCount">How many variables each observation carries.</param>
    /// <param name="lagOrder">How many lags enter each equation.</param>
    /// <param name="withIntercept">Whether a constant column leads each row.</param>
    /// <param name="responses">One array per variable, holding the values the design explains.</param>
    /// <returns>The design, row-major, <c>n − lagOrder</c> rows of <c>(withIntercept ? 1 : 0) + variableCount·lagOrder</c> columns.</returns>
    public static double[] Stack(
        ReadOnlySpan<double> series,
        int variableCount,
        int lagOrder,
        bool withIntercept,
        out double[][] responses)
    {
        int observations = series.Length / variableCount;
        int usable = observations - lagOrder;
        int columns = (withIntercept ? 1 : 0) + (variableCount * lagOrder);
        var design = new double[usable * columns];
        responses = new double[variableCount][];
        for (int variable = 0; variable < variableCount; variable++)
        {
            responses[variable] = new double[usable];
        }

        for (int row = 0; row < usable; row++)
        {
            int at = row * columns;
            int observation = row + lagOrder;
            if (withIntercept)
            {
                design[at] = 1.0;
            }

            for (int lag = 1; lag <= lagOrder; lag++)
            {
                int source = (observation - lag) * variableCount;
                int into = at + (withIntercept ? 1 : 0) + ((lag - 1) * variableCount);
                for (int variable = 0; variable < variableCount; variable++)
                {
                    design[into + variable] = series[source + variable];
                }
            }

            for (int variable = 0; variable < variableCount; variable++)
            {
                responses[variable][row] = series[(observation * variableCount) + variable];
            }
        }

        return design;
    }
}
