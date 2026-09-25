using Lodestar.Stats.Regression.Internal;
using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Stats.Regression;

/// <summary>Panel regression: fixed effects, between, first difference and random effects, at <c>linearmodels</c> parity.</summary>
/// <remarks>
/// Each estimator takes a <see cref="PanelDesign"/> — the response, the regressors, and each row's entity and period in any
/// order — and returns the whole table <c>linearmodels</c> prints: estimates, standard errors, tests, intervals, the
/// within, between and overall R², the model tests, and what the estimator adds: the poolability test of fixed effects,
/// the variance components and <c>θ</c> of random effects. The options' defaults are the reference's, an unadjusted
/// covariance and debiased tests among them.
/// </remarks>
public static class PanelRegression
{
    private enum Estimator
    {
        FixedEffects,
        Between,
        FirstDifference,
        RandomEffects,
    }

    /// <summary>Fits least squares with entity effects, period effects, both or neither: <c>PanelOLS(…).fit()</c>.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="options">The effects, the covariance and the intercept; <see langword="null"/> takes the reference's defaults, pooled least squares.</param>
    /// <returns>The fitted model's table, with its poolability test when it absorbs effects.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">A span's length does not match the rows, two rows share an entity and a period, no residual degree of freedom is left, the options set a value this fit does not read, or the regressors are collinear.</exception>
    public static PanelSummary FixedEffects(PanelDesign design, PanelOptions? options = null) =>
        Fit(Estimator.FixedEffects, design, default, clustered: false, options ?? new PanelOptions());

    /// <summary>Fits fixed effects with a covariance clustered by the caller's labels.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="clusters">One cluster label per row, combined with <see cref="PanelOptions.ClusterEntity"/> or <see cref="PanelOptions.ClusterTime"/> for two ways.</param>
    /// <param name="options">The options; <see cref="PanelOptions.CovarianceType"/> must be <see cref="PanelCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As the unclustered overload, or the labels are of the wrong length, name one cluster, or make more than two ways.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static PanelSummary FixedEffects(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.FixedEffects, design, clusters, clustered: true, options);
    }

    /// <summary>Fits least squares on the entity means: <c>BetweenOLS(…).fit()</c>.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="options">The covariance and the intercept; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>The fitted model's table, one observation per entity.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As <see cref="FixedEffects(PanelDesign, PanelOptions?)"/>, or the options ask for a kernel covariance, for effects, or for entity or period clusters.</exception>
    public static PanelSummary Between(PanelDesign design, PanelOptions? options = null) =>
        Fit(Estimator.Between, design, default, clustered: false, options ?? new PanelOptions());

    /// <summary>Fits the between estimator with a covariance clustered by labels constant within each entity.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="clusters">One cluster label per row, the same on every row of an entity.</param>
    /// <param name="options">The options; <see cref="PanelOptions.CovarianceType"/> must be <see cref="PanelCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As the unclustered overload, or a label varies inside an entity.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static PanelSummary Between(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.Between, design, clusters, clustered: true, options);
    }

    /// <summary>Fits least squares on first differences between adjacent periods: <c>FirstDifferenceOLS(…).fit()</c>.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="options">The covariance; <see cref="PanelOptions.WithIntercept"/> must be <see langword="false"/>, since a difference removes a constant.</param>
    /// <returns>The fitted model's table, one observation per differenced pair.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As <see cref="FixedEffects(PanelDesign, PanelOptions?)"/>, or the options ask for an intercept, effects or period clusters, the regressors hold a constant, or the panel has one period.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>: the default asks for an intercept.</exception>
    public static PanelSummary FirstDifference(PanelDesign design, PanelOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.FirstDifference, design, default, clustered: false, options);
    }

    /// <summary>Fits first differences with a covariance clustered by labels that do not change across a differenced pair.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="clusters">One cluster label per row.</param>
    /// <param name="options">The options; <see cref="PanelOptions.CovarianceType"/> must be <see cref="PanelCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As the unclustered overload, or a label changes across a differenced pair.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static PanelSummary FirstDifference(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.FirstDifference, design, clusters, clustered: true, options);
    }

    /// <summary>Fits random effects with Swamy and Arora's variance components: <c>RandomEffects(…).fit()</c>.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="options">The covariance and the intercept; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>The fitted model's table, with the variance components and each entity's <c>θ</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As <see cref="FixedEffects(PanelDesign, PanelOptions?)"/>, or the options ask for effects.</exception>
    public static PanelSummary RandomEffects(PanelDesign design, PanelOptions? options = null) =>
        Fit(Estimator.RandomEffects, design, default, clustered: false, options ?? new PanelOptions());

    /// <summary>Fits random effects with a covariance clustered by the caller's labels.</summary>
    /// <param name="design">The response, the regressors, and each row's entity and period.</param>
    /// <param name="clusters">One cluster label per row.</param>
    /// <param name="options">The options; <see cref="PanelOptions.CovarianceType"/> must be <see cref="PanelCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The regressor count is below one.</exception>
    /// <exception cref="ArgumentException">As the unclustered overload, or the labels are of the wrong length, name one cluster, or make more than two ways.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static PanelSummary RandomEffects(PanelDesign design, ReadOnlySpan<int> clusters, PanelOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.RandomEffects, design, clusters, clustered: true, options);
    }

    private static PanelSummary Fit(
        Estimator estimator, PanelDesign design, ReadOnlySpan<int> clusters, bool clustered, PanelOptions options)
    {
        CheckShape(design, clusters, clustered);
        CheckOptions(estimator, options, clustered);
        PanelLayout panel = PanelLayout.Sort(design, clusters, options.WithIntercept, nameof(design));
        (bool hasConstant, int constantColumn) = IvScores.FindConstant(panel.X, panel.K, panel.N);
        RequireRows(estimator, panel, hasConstant, nameof(design));
        RequireDegreesOfFreedom(estimator, panel, options, hasConstant, nameof(design));

        PanelFit fit = estimator switch
        {
            Estimator.FixedEffects => PanelCore.FixedEffects(
                panel, options.EntityEffects, options.TimeEffects, hasConstant,
                (absorbed, effect, _) => Spec(options, CountedEffects(options, panel, absorbed, effect), panel.Entity, panel.Period, panel.Clusters)),
            Estimator.Between => PanelCore.Between(panel, hasConstant, BetweenSpec(options, panel)),
            Estimator.FirstDifference => PanelCore.FirstDifference(
                panel, (entities, periods, labels) => Spec(options, 0, entities, periods, labels)),
            _ => PanelCore.RandomEffects(panel, hasConstant, Spec(options, 0, panel.Entity, panel.Period, panel.Clusters)),
        };

        return PanelDiagnostics.Summarise(
            panel, fit, options.CovarianceType, options.Debiased, options.ConfidenceLevel, hasConstant, constantColumn);
    }

    /// <summary>The effects counted out of the covariance's degrees of freedom: all of them, unless one effect is nested in the clusters.</summary>
    /// <remarks>
    /// <c>auto_df</c>: with one effect and a clustered covariance, the effect is dropped from the count when each of its
    /// groups falls in a single cluster — judged, as the reference judges it, on the last cluster column.
    /// </remarks>
    private static int CountedEffects(PanelOptions options, PanelLayout panel, int absorbed, int[]? effect)
    {
        if (options.CovarianceType != PanelCovarianceType.Clustered || effect is null)
        {
            return absorbed;
        }

        List<int[]> columns = ClusterColumns(options, panel.Entity, panel.Period, panel.Clusters);
        if (columns.Count == 0)
        {
            return absorbed;
        }

        int[] last = columns[columns.Count - 1];
        int effectCount = effect.Distinct().Count();
        int pairs = Enumerable.Range(0, effect.Length).Select(row => (effect[row], last[row])).Distinct().Count();
        return pairs == effectCount ? 0 : absorbed;
    }

    private static PanelCovarianceSpec Spec(PanelOptions options, int extraDf, int[] entities, int[] periods, int[]? labels) =>
        new(options.CovarianceType, options.Debiased, extraDf, ClusterColumns(options, entities, periods, labels), periods,
            options.Kernel, options.Bandwidth);

    /// <summary>The between estimator's covariance: one row per entity, the caller's clusters read off each entity's rows.</summary>
    private static PanelCovarianceSpec BetweenSpec(PanelOptions options, PanelLayout panel)
    {
        var columns = new List<int[]>();
        if (panel.Clusters is { } labels)
        {
            var perEntity = new int[panel.EntityCount];
            var seen = new bool[panel.EntityCount];
            for (int row = 0; row < panel.N; row++)
            {
                int e = panel.Entity[row];
                if (seen[e] && perEntity[e] != labels[row])
                {
                    throw new ArgumentException("A cluster label varies inside an entity; the between estimator needs one per entity.");
                }

                perEntity[e] = labels[row];
                seen[e] = true;
            }

            columns.Add(perEntity);
        }

        return new PanelCovarianceSpec(
            options.CovarianceType, options.Debiased, 0, columns, new int[panel.EntityCount], options.Kernel, options.Bandwidth);
    }

    /// <summary>The cluster columns in the reference's order: the caller's labels, then entity, then period.</summary>
    private static List<int[]> ClusterColumns(PanelOptions options, int[] entities, int[] periods, int[]? labels)
    {
        var columns = new List<int[]>();
        if (options.CovarianceType != PanelCovarianceType.Clustered)
        {
            return columns;
        }

        if (labels is not null)
        {
            columns.Add(labels);
        }

        if (options.ClusterEntity)
        {
            columns.Add(entities);
        }

        if (options.ClusterTime)
        {
            columns.Add(periods);
        }

        return columns;
    }

    private static void CheckShape(PanelDesign design, ReadOnlySpan<int> clusters, bool clustered)
    {
        int n = design.Response.Length;
        if (design.ExogenousCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(design), design.ExogenousCount, "A panel regression needs at least one regressor.");
        }

        if (design.Exogenous.Length != (long)n * design.ExogenousCount
            || design.Entities.Length != n || design.Periods.Length != n || (clustered && clusters.Length != n))
        {
            throw new ArgumentException(
                $"{n} responses need {n * (long)design.ExogenousCount} regressor values and {n} entity, period and cluster labels.",
                nameof(design));
        }
    }

    /// <summary>Refuses the options this estimator and overload do not read.</summary>
    private static void CheckOptions(Estimator estimator, PanelOptions options, bool clustered)
    {
        OptionGuards.ConfidenceLevel(options.ConfidenceLevel);
        if (options.CovarianceType is < PanelCovarianceType.Unadjusted or > PanelCovarianceType.Kernel
            || options.Kernel is < KernelType.Bartlett or > KernelType.QuadraticSpectral || options.Bandwidth < 0)
        {
            throw new ArgumentException("The covariance, the kernel or the bandwidth is not one this package defines.", nameof(options));
        }

        CheckEstimatorOptions(estimator, options, clustered);
    }

    /// <summary>Refuses a kernel, cluster or effect setting the chosen covariance or estimator does not read.</summary>
    private static void CheckEstimatorOptions(Estimator estimator, PanelOptions options, bool clustered)
    {
        bool kernel = options.CovarianceType == PanelCovarianceType.Kernel;
        bool clusteredCovariance = options.CovarianceType == PanelCovarianceType.Clustered;
        Refuse(!kernel && (options.Kernel != KernelType.Bartlett || options.Bandwidth.HasValue),
            "Kernel and Bandwidth are read by a Kernel covariance only.", nameof(options));
        Refuse(!clusteredCovariance && (options.ClusterEntity || options.ClusterTime || clustered),
            "Cluster settings and labels are read by a Clustered covariance only.", nameof(options));
        Refuse(estimator != Estimator.FixedEffects && (options.EntityEffects || options.TimeEffects),
            $"Entity and time effects are FixedEffects' only, not {estimator}'s.", nameof(options));
        int ways = (clustered ? 1 : 0) + (options.ClusterEntity ? 1 : 0) + (options.ClusterTime ? 1 : 0);
        Refuse(ways > 2, "Clustering runs one or two ways, not three.", nameof(options));
        Refuse(estimator == Estimator.Between && (kernel || options.ClusterEntity || options.ClusterTime),
            "Between takes the unadjusted, robust or clustered covariance, clustered by labels constant within each entity.", nameof(options));
        Refuse(estimator == Estimator.FirstDifference && options.WithIntercept,
            "A first difference removes a constant: set WithIntercept to false, as the reference refuses one.", nameof(options));
        Refuse(estimator == Estimator.FirstDifference && options.ClusterTime,
            "First differences cluster by entity or by labels, not by period.", nameof(options));
    }

    private static void RequireRows(Estimator estimator, PanelLayout panel, bool hasConstant, string designName)
    {
        Refuse(estimator == Estimator.FirstDifference && hasConstant,
            "The regressors hold a constant, which a first difference removes; the reference refuses it too.", designName);
        Refuse(estimator == Estimator.FirstDifference && panel.PeriodCount < 2, "A first difference needs two periods.", designName);
        int rows = estimator == Estimator.Between ? panel.EntityCount : panel.N;
        Refuse(rows <= panel.K, $"{rows} rows leave no residual degree of freedom for {panel.K} coefficients.", designName);
    }

    /// <summary>Refuses a fit whose effects or variance components leave no degree of freedom, where the reference divides by zero.</summary>
    private static void RequireDegreesOfFreedom(
        Estimator estimator, PanelLayout panel, PanelOptions options, bool hasConstant, string designName)
    {
        if (estimator == Estimator.FixedEffects)
        {
            int absorbed = PanelCore.Absorbed(panel, options.EntityEffects, options.TimeEffects, hasConstant);
            int left = panel.N - panel.K - absorbed;
            Refuse(left <= 0, $"The effects and {panel.K} coefficients leave {left} residual degrees of freedom.", designName);
            bool effects = options.EntityEffects || options.TimeEffects;
            Refuse(effects && absorbed - (hasConstant ? 0 : 1) <= 0,
                "One entity or one period leaves the effects nothing the poolability test can count.", designName);
        }
        else if (estimator == Estimator.RandomEffects)
        {
            Refuse(panel.N - panel.K - panel.EntityCount + 1 <= 0 || panel.EntityCount <= panel.K,
                "The variance components need more rows than entities plus coefficients, and more entities than coefficients.",
                designName);
        }
    }

    private static void Refuse(bool condition, string message, string parameterName)
    {
        if (condition)
        {
            throw new ArgumentException(message, parameterName);
        }
    }
}
