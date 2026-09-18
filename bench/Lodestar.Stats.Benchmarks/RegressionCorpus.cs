namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The seeded design and response the least-squares benchmarks fit.</summary>
/// <remarks>
/// One draw order for every class that uses it: <c>columns</c> values for a row, then one for that row's
/// noise. Each class kept its own copy of this loop, so a change to one silently moved that corpus alone
/// and the numbers stopped being comparable — which is what a seed is for (#978).
/// </remarks>
internal static class RegressionCorpus
{
    /// <summary>Draws a corpus, row-major and as a jagged copy for the libraries that take one.</summary>
    /// <param name="seed">The seed, which is what makes a class's numbers comparable across runs.</param>
    /// <param name="rows">How many rows to draw.</param>
    /// <param name="columns">How many regressors each row carries.</param>
    /// <param name="draw">What a uniform <c>[0, 1)</c> becomes — the identity, or a shift onto another range.</param>
    /// <param name="contribution">What a regressor adds to its row's signal, by index and value.</param>
    internal static (double[] Design, double[] Response, double[][] Jagged) Build(
        int seed,
        int rows,
        int columns,
        Func<double, double> draw,
        Func<int, double, double> contribution)
    {
        Random random = new(seed);
        var design = new double[rows * columns];
        var response = new double[rows];
        var jagged = new double[rows][];
        for (int row = 0; row < rows; row++)
        {
            var line = new double[columns];
            double signal = 0.0;
            for (int column = 0; column < columns; column++)
            {
                double value = draw(random.NextDouble());
                line[column] = value;
                design[(row * columns) + column] = value;
                signal += contribution(column, value);
            }

            jagged[row] = line;
            response[row] = signal + (random.NextDouble() - 0.5);
        }

        return (design, response, jagged);
    }
}
