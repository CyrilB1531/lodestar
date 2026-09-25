using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>A panel sorted by entity, then period, with both labels renumbered from zero in ascending order.</summary>
/// <remarks>
/// The reference keeps the rows in the order given and reads a period axis in the order periods are first seen;
/// sorting here is what lets a first difference be taken between adjacent periods whatever order the rows came in.
/// </remarks>
internal sealed class PanelLayout
{
    /// <summary>The response, sorted.</summary>
    public double[] Y { get; private init; } = [];

    /// <summary>The regressors, sorted and row-major, the constant first when one was asked for.</summary>
    public double[] X { get; private init; } = [];

    /// <summary>Columns in <see cref="X"/>.</summary>
    public int K { get; private init; }

    /// <summary>Each row's entity, from zero in ascending label order.</summary>
    public int[] Entity { get; private init; } = [];

    /// <summary>Each row's period, from zero in ascending label order.</summary>
    public int[] Period { get; private init; } = [];

    /// <summary>The number of entities, <c>N</c>.</summary>
    public int EntityCount { get; private init; }

    /// <summary>The number of periods, <c>T</c>.</summary>
    public int PeriodCount { get; private init; }

    /// <summary>The caller's cluster labels, sorted with the rows; <see langword="null"/> when none were given.</summary>
    public int[]? Clusters { get; private init; }

    /// <summary>Rows in the panel.</summary>
    public int N => Y.Length;

    /// <summary>Sorts and renumbers a panel, refusing a row that repeats another's entity and period.</summary>
    /// <exception cref="ArgumentException">Two rows share an entity and a period.</exception>
    public static PanelLayout Sort(PanelDesign design, ReadOnlySpan<int> clusters, bool withIntercept, string parameterName)
    {
        ReadOnlySpan<double> response = design.Response;
        ReadOnlySpan<double> exogenous = design.Exogenous;
        ReadOnlySpan<int> entities = design.Entities;
        ReadOnlySpan<int> periods = design.Periods;
        int exogenousCount = design.ExogenousCount;
        int n = response.Length;
        (int[] entity, int entityCount) = Codes(entities);
        (int[] period, int periodCount) = Codes(periods);
        int[] order = [.. Enumerable.Range(0, n).OrderBy(row => entity[row]).ThenBy(row => period[row])];
        int k = exogenousCount + (withIntercept ? 1 : 0);
        var y = new double[n];
        var x = new double[n * k];
        var sortedEntity = new int[n];
        var sortedPeriod = new int[n];
        int[]? sortedClusters = clusters.IsEmpty ? null : new int[n];
        for (int at = 0; at < n; at++)
        {
            int row = order[at];
            y[at] = response[row];
            sortedEntity[at] = entity[row];
            sortedPeriod[at] = period[row];
            if (at > 0 && sortedEntity[at] == sortedEntity[at - 1] && sortedPeriod[at] == sortedPeriod[at - 1])
            {
                throw new ArgumentException(
                    $"Two rows share entity {entities[row]} and period {periods[row]}; a panel holds one row for each.",
                    parameterName);
            }

            int offset = 0;
            if (withIntercept)
            {
                x[at * k] = 1.0;
                offset = 1;
            }

            for (int j = 0; j < exogenousCount; j++)
            {
                x[(at * k) + offset + j] = exogenous[(row * exogenousCount) + j];
            }

            if (sortedClusters is not null)
            {
                sortedClusters[at] = clusters[row];
            }
        }

        return new PanelLayout
        {
            Y = y,
            X = x,
            K = k,
            Entity = sortedEntity,
            Period = sortedPeriod,
            EntityCount = entityCount,
            PeriodCount = periodCount,
            Clusters = sortedClusters,
        };
    }

    /// <summary>Each row's value less its group's mean, column by column: <c>demean</c> for one effect.</summary>
    public static double[] Demean(double[] block, int columns, int[] group, int groupCount)
    {
        double[] means = GroupMeans(block, columns, group, groupCount);
        var demeaned = new double[block.Length];
        for (int row = 0; row < group.Length; row++)
        {
            int at = row * columns;
            int from = group[row] * columns;
            for (int j = 0; j < columns; j++)
            {
                demeaned[at + j] = block[at + j] - means[from + j];
            }
        }

        return demeaned;
    }

    /// <summary>Each group's mean of each column, row-major by group.</summary>
    public static double[] GroupMeans(double[] block, int columns, int[] group, int groupCount)
    {
        var sums = new double[groupCount * columns];
        var counts = new int[groupCount];
        for (int row = 0; row < group.Length; row++)
        {
            counts[group[row]]++;
            int at = row * columns;
            int to = group[row] * columns;
            for (int j = 0; j < columns; j++)
            {
                sums[to + j] += block[at + j];
            }
        }

        for (int g = 0; g < groupCount; g++)
        {
            for (int j = 0; j < columns; j++)
            {
                sums[(g * columns) + j] /= counts[g];
            }
        }

        return sums;
    }

    /// <summary>How many rows each entity holds.</summary>
    public int[] EntitySizes()
    {
        var sizes = new int[EntityCount];
        foreach (int e in Entity)
        {
            sizes[e]++;
        }

        return sizes;
    }

    private static (int[] Codes, int Count) Codes(ReadOnlySpan<int> labels)
    {
        int[] distinct = labels.ToArray();
        Array.Sort(distinct);
        int unique = 0;
        for (int i = 0; i < distinct.Length; i++)
        {
            if (i == 0 || distinct[i] != distinct[i - 1])
            {
                distinct[unique++] = distinct[i];
            }
        }

        var codes = new int[labels.Length];
        for (int row = 0; row < labels.Length; row++)
        {
            codes[row] = Array.BinarySearch(distinct, 0, unique, labels[row]);
        }

        return (codes, unique);
    }
}
