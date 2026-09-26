using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>The weighted log-rank family over any number of groups, <c>lifelines.statistics.multivariate_logrank_test</c>.</summary>
/// <remarks>
/// Each group's weighted events against its share of the pooled ones, at every distinct duration; the statistic reads
/// that vector, every group but the last, through a pseudo-inverse of its covariance. lifelines' choices are kept: the
/// factor <c>(n − d)/(n − 1)</c> is one where infinite or undefined, a negative one (a fractional risk set below one)
/// adds nothing, Peto's weight includes its own time, and Fleming-Harrington reads the pooled unweighted curve before it.
/// </remarks>
internal static class LogRankFamily
{
    /// <summary>Runs the test.</summary>
    /// <param name="durations">One duration per subject, validated.</param>
    /// <param name="observed">One event flag per subject, validated.</param>
    /// <param name="weights">One positive weight per subject, or <see langword="null"/> for ones.</param>
    /// <param name="group">Each subject's group, <c>0</c> to <paramref name="groupCount"/> − 1 in first-appearance order.</param>
    /// <param name="groupCount">How many groups, at least two.</param>
    /// <param name="options">The weighting and truncation, validated.</param>
    internal static LogRankResult Test(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> observed,
        double[]? weights,
        int[] group,
        int groupCount,
        LogRankOptions options)
    {
        Table table = Table.Build(durations, observed, weights, group, groupCount, options.Truncation);
        var sums = new Accumulator(table, options);
        for (int row = 0; row < table.Rows; row++)
        {
            sums.Add(row);
        }

        return Statistic(sums.ObservedTotals, sums.ExpectedTotals, sums.Products, sums.CrossTotal, groupCount);
    }

    /// <summary>The quadratic form over every group but the last, and its chi-squared tail on <c>k − 1</c>.</summary>
    private static LogRankResult Statistic(
        double[] observedTotals, double[] expectedTotals, double[] products, double[] crossTotal, int k)
    {
        int size = k - 1;
        var difference = new double[size];
        var covariance = new double[size * size];
        for (int g = 0; g < size; g++)
        {
            difference[g] = observedTotals[g] - expectedTotals[g];
            for (int h = 0; h < size; h++)
            {
                covariance[(g * size) + h] = -products[(g * k) + h];
            }

            covariance[(g * size) + g] += crossTotal[g];
        }

        double statistic = SymmetricPseudoInverse.QuadraticForm(covariance, difference, size);
        return new LogRankResult(statistic, Distributions.ChiSquaredSf(statistic, size), size);
    }

    /// <summary>The weighted sums the statistic reads, accumulated one distinct duration at a time.</summary>
    private sealed class Accumulator
    {
        private readonly Table _table;
        private readonly LogRankOptions _options;
        private readonly int _k;
        private readonly double[] _atRisk;
        private readonly double[] _scaled;
        private readonly double[] _removedBefore;
        private double _pooledRemovedBefore;
        private double _peto = 1.0;
        private double _leftSurvival = 1.0;
        private int _countBefore;

        public Accumulator(Table table, LogRankOptions options)
        {
            _table = table;
            _options = options;
            _k = table.GroupTotals.Length;
            _atRisk = new double[_k];
            _scaled = new double[_k];
            _removedBefore = new double[_k];
            ObservedTotals = new double[_k];
            ExpectedTotals = new double[_k];
            Products = new double[_k * _k];
            CrossTotal = new double[_k];
        }

        public double[] ObservedTotals { get; }

        public double[] ExpectedTotals { get; }

        public double[] Products { get; }

        public double[] CrossTotal { get; }

        public void Add(int row)
        {
            double pooledAtRisk = _table.Total - _pooledRemovedBefore;
            double events = 0.0;
            for (int g = 0; g < _k; g++)
            {
                _atRisk[g] = _table.GroupTotals[g] - _removedBefore[g];
                events += _table.Events[(row * _k) + g];
            }

            double share = events / pooledAtRisk;
            double expectedSum = 0.0;
            for (int g = 0; g < _k; g++)
            {
                expectedSum += _atRisk[g] * share;
            }

            _peto *= 1.0 - (expectedSum / (pooledAtRisk + 1.0));
            double weight = Weight(_options, pooledAtRisk, _peto, _leftSurvival);
            double root = Math.Sqrt(VarianceFactor(pooledAtRisk, events));

            // lifelines' fillna(0): a negative factor, from a fractional risk set below one, contributes nothing.
            double totalScaled = double.IsNaN(root) ? 0.0 : pooledAtRisk * weight * root;
            for (int g = 0; g < _k; g++)
            {
                ObservedTotals[g] += _table.Events[(row * _k) + g] * weight;
                ExpectedTotals[g] += _atRisk[g] * share * weight;
                _scaled[g] = double.IsNaN(root) ? 0.0 : _atRisk[g] * weight * root;
                CrossTotal[g] += totalScaled * _scaled[g];
            }

            AddProducts();
            Advance(row);
        }

        /// <summary>The weight of one time under the chosen member of the family.</summary>
        private static double Weight(LogRankOptions options, double pooledAtRisk, double peto, double leftSurvival) =>
            options.Weighting switch
            {
                LogRankWeighting.Wilcoxon => pooledAtRisk,
                LogRankWeighting.TaroneWare => Math.Sqrt(pooledAtRisk),
                LogRankWeighting.Peto => peto,
                LogRankWeighting.FlemingHarrington =>
                    Math.Pow(leftSurvival, options.P) * Math.Pow(1.0 - leftSurvival, options.Q),
                _ => 1.0,
            };

        /// <summary><c>(n − d)/(n − 1) · d/n²</c>, with lifelines' one where the ratio is infinite or undefined.</summary>
        private static double VarianceFactor(double pooledAtRisk, double events)
        {
            double ratio = (pooledAtRisk - events) / (pooledAtRisk - 1.0);
            if (double.IsNaN(ratio) || double.IsPositiveInfinity(ratio))
            {
                ratio = 1.0;
            }

            return ratio * events / (pooledAtRisk * pooledAtRisk);
        }

        private void AddProducts()
        {
            for (int g = 0; g < _k; g++)
            {
                for (int h = 0; h < _k; h++)
                {
                    Products[(g * _k) + h] += _scaled[g] * _scaled[h];
                }
            }
        }

        /// <summary>Moves the risk sets past this time; the pooled curve is the unweighted one, as lifelines fits it.</summary>
        private void Advance(int row)
        {
            int countAtRisk = _table.Count - _countBefore;
            _leftSurvival *= 1.0 - ((double)_table.CountEvents[row] / countAtRisk);
            _countBefore += _table.CountRemoved[row];
            _pooledRemovedBefore += _table.Removed(row);
            for (int g = 0; g < _k; g++)
            {
                _removedBefore[g] += _table.RemovedByGroup[(row * _k) + g];
            }
        }
    }

    /// <summary>The weighted removals and events per distinct duration and group, and the unweighted counts.</summary>
    private sealed class Table
    {
        private Table(int rows, int groupCount, int count)
        {
            Rows = rows;
            Count = count;
            GroupTotals = new double[groupCount];
            RemovedByGroup = new double[rows * groupCount];
            Events = new double[rows * groupCount];
            CountRemoved = new int[rows];
            CountEvents = new int[rows];
        }

        public int Rows { get; }

        public int Count { get; }

        public double Total { get; private set; }

        public double[] GroupTotals { get; }

        public double[] RemovedByGroup { get; }

        public double[] Events { get; }

        public int[] CountRemoved { get; }

        public int[] CountEvents { get; }

        public double Removed(int row)
        {
            int k = GroupTotals.Length;
            double total = 0.0;
            for (int g = 0; g < k; g++)
            {
                total += RemovedByGroup[(row * k) + g];
            }

            return total;
        }

        public static Table Build(
            ReadOnlySpan<double> durations,
            ReadOnlySpan<bool> observed,
            double[]? weights,
            int[] group,
            int groupCount,
            double? truncation)
        {
            int n = durations.Length;
            double[] keys = durations.ToArray();
            int[] order = new int[n];
            for (int i = 0; i < n; i++)
            {
                order[i] = i;
            }

            Array.Sort(keys, order);
            int rows = DistinctCount(keys);
            var table = new Table(rows, groupCount, n);
            int row = -1;
            for (int at = 0; at < n; at++)
            {
                // S1244: a tie is the same recorded duration, as in RiskTable.
#pragma warning disable S1244
                if (at == 0 || keys[at] != keys[at - 1])
#pragma warning restore S1244
                {
                    row++;
                }

                int subject = order[at];
                double w = weights is null ? 1.0 : weights[subject];
                int cell = (row * groupCount) + group[subject];
                bool isEvent = observed[subject] && !(keys[at] > truncation);
                table.RemovedByGroup[cell] += w;
                table.GroupTotals[group[subject]] += w;
                table.Total += w;
                table.CountRemoved[row]++;
                if (isEvent)
                {
                    table.Events[cell] += w;
                    table.CountEvents[row]++;
                }
            }

            return table;
        }

        private static int DistinctCount(double[] sorted)
        {
            int count = 0;
            for (int i = 0; i < sorted.Length; i++)
            {
#pragma warning disable S1244
                if (i == 0 || sorted[i] != sorted[i - 1])
#pragma warning restore S1244
                {
                    count++;
                }
            }

            return count;
        }
    }
}
