using Lodestar.Stats.Regression.Panel;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Every panel estimator over every panel shape, constant and degenerate response, each cell held to the bitwise rule
/// (#1784, #1846-#1848): <c>+∞</c> on residuals exactly zero, NaN on a total the rule reads zero, a named refusal, or the
/// ordinary formula; the unadjusted Wald form to the model F, and the robust or clustered one NaN on its zero block,
/// cell by cell, under the unadjusted, robust and entity-clustered covariances.
/// </summary>
public sealed class PanelEdgeGridTests
{
    /// <summary>The estimators the grid fits, named as the cells report them.</summary>
    private enum Estimator
    {
        Pooled,
        Within,
        Period,
        TwoWay,
        Between,
        Random,
        FirstDifference,
    }

    /// <summary>How the regressors carry a constant: a column of ones the fit prepends, two indicators that span one, a column of ones alone, or none.</summary>
    private enum Constant
    {
        Column,
        Spanned,
        Alone,
        None,
    }

    /// <summary>The degenerate responses, with one that varies as the ordinary case.</summary>
    private enum Response
    {
        Varying,
        PeriodAbsorbed,
        ConstantOverall,
        ConstantInteger,
        ConstantWithinEntity,
        ConstantWithinPeriod,
        EqualEntityMeans,
        NearEqualEntityMeans,
        ZeroEntityMeans,
        AllZero,
    }

    /// <summary>Rows per entity: each entity's periods run from zero, so a short entity misses the last periods.</summary>
    private static readonly (string Name, int[] Sizes)[] Shapes =
    [
        ("balanced", [5, 5, 5, 5, 5, 5, 5, 5]),
        ("one entity", [12]),
        ("one period", [1, 1, 1, 1, 1, 1, 1, 1, 1, 1]),
        ("unbalanced", [3, 5, 4, 6, 2, 5, 3, 4]),
        ("singletons", [1, 5, 1, 5, 5, 1, 5, 5]),
    ];

    /// <summary>One cell: its panel, what the fit returned or the refusal it raised.</summary>
    private sealed record Cell(
        string Shape, Estimator Estimator, Constant Constant, Response Response,
        double[] Y, Readings Read, int[] Entities, int[] Periods, PanelCovarianceType Covariance, PanelSummary? Summary, string? Refusal)
    {
        public override string ToString() => $"{Shape}, {Estimator}, {Constant}, {Response}, {Covariance}";
    }

    /// <summary>
    /// What the rule reads of one response on one shape: constant or zero as given, constant within each entity or
    /// period, and its entity means, by LINQ, bitwise equal or all zero.
    /// </summary>
    private sealed record Readings(bool ResponseFlat, bool ResponseZero, bool FlatWithinEntity, bool FlatWithinPeriod, bool MeansFlat, bool MeansZero);

    /// <summary>Every cell of the grid, fitted under the given covariance, debiased or not.</summary>
    private static IEnumerable<Cell> Cells(bool debiased, PanelCovarianceType covariance)
    {
        foreach ((string shape, int[] sizes) in Shapes)
        {
            (int[] entities, int[] periods) = Layout(sizes);
            int n = entities.Length;
            double[] x = Regressor(n);
            // An indicator that varies within entities and periods wherever the shape lets it; it and 1 − g span 1.
            double[] g = [.. Enumerable.Range(0, n).Select(i => i % 3 == 0 ? 1.0 : 0.0)];
            foreach (Response response in Enum.GetValues<Response>())
            {
                double[] y = Build(response, x, sizes, entities, periods);
                double[] means = Means(y, entities);
                var read = new Readings(
                    Flat(y), AllZero(y), FlatWithin(y, entities), FlatWithin(y, periods), Flat(means), AllZero(means));
                foreach (Constant constant in Enum.GetValues<Constant>())
                {
                    (double[] design, int columns) = constant switch
                    {
                        Constant.Spanned => ([.. Enumerable.Range(0, n).SelectMany(i => new[] { g[i], 1.0 - g[i], x[i] })], 3),
                        Constant.Alone => ([.. Enumerable.Repeat(1.0, n)], 1),
                        _ => (x, 1),
                    };
                    var options = new PanelOptions
                    {
                        WithIntercept = constant == Constant.Column,
                        Debiased = debiased,
                        CovarianceType = covariance,
                    };
                    foreach (Estimator estimator in Enum.GetValues<Estimator>())
                    {
                        // A ref struct, so built per fit: it cannot live across the yield below.
                        var panel = new PanelDesign(y, design, columns, entities, periods);
                        (PanelSummary? summary, string? refusal) = Run(estimator, panel, entities, options);
                        yield return new Cell(
                            shape, estimator, constant, response, y, read, entities, periods, covariance, summary, refusal);
                    }
                }
            }
        }
    }

    private static (PanelSummary? Summary, string? Refusal) Run(Estimator estimator, PanelDesign panel, int[] entities, PanelOptions options)
    {
        try
        {
            return (options.CovarianceType == PanelCovarianceType.Clustered
                ? FitClustered(estimator, panel, entities, options)
                : Fit(estimator, panel, options), null);
        }
        catch (ArgumentException error)
        {
            return (null, error.Message);
        }
    }

    // Clustered by the entity labels, the one clustering every estimator takes, the between fit's included.
    private static PanelSummary FitClustered(Estimator estimator, PanelDesign panel, int[] clusters, PanelOptions options) => estimator switch
    {
        Estimator.Pooled => PanelRegression.FixedEffects(panel, clusters, options),
        Estimator.Within => PanelRegression.FixedEffects(panel, clusters, options with { EntityEffects = true }),
        Estimator.Period => PanelRegression.FixedEffects(panel, clusters, options with { TimeEffects = true }),
        Estimator.TwoWay => PanelRegression.FixedEffects(panel, clusters, options with { EntityEffects = true, TimeEffects = true }),
        Estimator.Between => PanelRegression.Between(panel, clusters, options),
        Estimator.Random => PanelRegression.RandomEffects(panel, clusters, options),
        _ => PanelRegression.FirstDifference(panel, clusters, options),
    };

    private static PanelSummary Fit(Estimator estimator, PanelDesign panel, PanelOptions options) => estimator switch
    {
        Estimator.Pooled => PanelRegression.FixedEffects(panel, options),
        Estimator.Within => PanelRegression.FixedEffects(panel, options with { EntityEffects = true }),
        Estimator.Period => PanelRegression.FixedEffects(panel, options with { TimeEffects = true }),
        Estimator.TwoWay => PanelRegression.FixedEffects(panel, options with { EntityEffects = true, TimeEffects = true }),
        Estimator.Between => PanelRegression.Between(panel, options),
        Estimator.Random => PanelRegression.RandomEffects(panel, options),
        _ => PanelRegression.FirstDifference(panel, options),
    };

    /// <summary>A regressor of irregular values, irrational parts included, that no layout here makes collinear.</summary>
    private static double[] Regressor(int n) =>
        [.. Enumerable.Range(0, n).Select(i => ((i * 37 % 101) / 10.0) + (Math.Sqrt(i + 2) / 7.0))];

    private static (int[] Entities, int[] Periods) Layout(int[] sizes)
    {
        var entities = new List<int>();
        var periods = new List<int>();
        for (int e = 0; e < sizes.Length; e++)
        {
            for (int t = 0; t < sizes[e]; t++)
            {
                entities.Add(e);
                periods.Add(t);
            }
        }

        return ([.. entities], [.. periods]);
    }

    private static double[] Build(Response response, double[] x, int[] sizes, int[] entities, int[] periods) => response switch
    {
        // A pattern of period eleven, which no entity or period effect of these shapes absorbs.
        Response.Varying => [.. x.Select((v, i) => (2.0 * v) + (0.1 * ((i * 7) % 11)))],
        // A regressor and a pattern in the period, which period and two-way effects fit but for rounding.
        Response.PeriodAbsorbed => [.. x.Select((v, i) => (2.0 * v) + (0.1 * periods[i]))],
        Response.ConstantOverall => [.. x.Select(_ => 0.1)],
        Response.ConstantInteger => [.. x.Select(_ => 5.0)],
        Response.ConstantWithinEntity => [.. entities.Select(e => 0.1 + (0.3 * (e % 3)))],
        Response.ConstantWithinPeriod => [.. periods.Select(t => 0.2 + (0.5 * (t % 2)))],
        Response.EqualEntityMeans => [.. entities.Select((e, i) => 3.0 + Offset(sizes[e], e, periods[i]))],
        Response.ZeroEntityMeans => [.. entities.Select((e, i) => Offset(sizes[e], e, periods[i]))],
        // Equal means in exact arithmetic, 0.1 in each entity, which the rotated summation order and the entity sizes
        // compute an ulp apart: the ordinary formulas, with no tolerance.
        Response.NearEqualEntityMeans => [.. entities.Select((e, i) => 0.1 + Offset(sizes[e], e, periods[i]))],
        _ => new double[x.Length],
    };

    // Offsets j − (s − 1)/2, halves and integers that sum to zero exactly, rotated by the entity: each entity's mean is
    // exactly 3, or 0, whatever its size s, so the means are bitwise equal while the response varies in each entity of two rows or more.
    private static double Offset(int size, int entity, int period) => ((period + entity) % size) - ((size - 1) / 2.0);

    /// <summary>Each entity's mean, by LINQ rather than the library's code: the rows are sorted by entity and period already.</summary>
    private static double[] Means(double[] y, int[] entities) =>
        [.. y.Select((value, i) => (Value: value, Entity: entities[i])).GroupBy(row => row.Entity).Select(rows => rows.Average(row => row.Value))];

    private static bool Flat(double[] values) => values.Distinct().Count() == 1;

    private static bool AllZero(double[] values) => values.All(value => value is 0.0);

    private static bool FlatWithin(double[] y, int[] groups) =>
        groups.Distinct().All(g => Flat([.. y.Where((_, i) => groups[i] == g)]));

    /// <summary>
    /// Where the rule reads the fit's total zero, so its R², model F and unadjusted Wald form are NaN unless the residuals
    /// vanish first: pooled on a constant response beside a constant, entity effects and random effects on one constant in
    /// each entity, period effects on one in each period, two-way effects on either, between on entity means bitwise
    /// equal or a constant response beside a constant; first differences never.
    /// </summary>
    private static bool NoVariance(Cell cell, bool hasConstant)
    {
        bool flat = cell.Read.ResponseFlat;
        return cell.Estimator switch
        {
            Estimator.Pooled => hasConstant && flat,
            Estimator.Within or Estimator.Random => cell.Read.FlatWithinEntity,
            Estimator.Period => cell.Read.FlatWithinPeriod,
            Estimator.TwoWay => cell.Read.FlatWithinEntity || cell.Read.FlatWithinPeriod,
            Estimator.Between => hasConstant && (flat || cell.Read.MeansFlat),
            _ => false,
        };
    }

    /// <summary>The between R²'s total: zero beside a constant on a constant response or bitwise equal means, without one on means all exactly zero.</summary>
    private static bool BetweenTotalZero(Cell cell, bool hasConstant) =>
        hasConstant ? cell.Read.ResponseFlat || cell.Read.MeansFlat : cell.Read.MeansZero;

    // 1400 cells less the refusals the layout predicts; one entity is one cluster, which the clustered fits refuse too.
    public static TheoryData<bool, PanelCovarianceType, int> Settings => new()
    {
        { true, PanelCovarianceType.Unadjusted, 862 },
        { false, PanelCovarianceType.Unadjusted, 862 },
        { true, PanelCovarianceType.Robust, 862 },
        { false, PanelCovarianceType.Robust, 862 },
        { true, PanelCovarianceType.Clustered, 812 },
        { false, PanelCovarianceType.Clustered, 812 },
    };

    [Theory]
    [MemberData(nameof(Settings))]
    public void Every_cell_reads_the_bitwise_rule(bool debiased, PanelCovarianceType covariance, int fittedCells)
    {
        int fitted = 0;
        var failures = new List<string>();
        bool nearEqual = false;
        bool equalMeans = false;
        bool equalMeansSpanned = false;
        bool spanned = false;
        foreach (Cell cell in Cells(debiased, covariance))
        {
            string? refusal = ExpectedRefusal(cell);
            if (cell.Summary is not { } summary)
            {
                Check(failures, refusal is not null && cell.Refusal!.Contains(refusal, StringComparison.Ordinal), $"{cell}: {cell.Refusal}, expected {refusal ?? "a fit"}");
                continue;
            }

            Check(failures, refusal is null, $"{cell}: fitted, expected the refusal {refusal}");
            fitted++;
            // Residuals that do not vanish, so a NaN branch rather than the +∞ one answers.
            bool unexact = summary.StandardErrors.Any(error => error is not 0.0);
            bool between = cell.Estimator == Estimator.Between && !cell.Read.ResponseFlat && unexact;
            nearEqual |= between && cell.Constant == Constant.Column && cell.Response == Response.NearEqualEntityMeans && !cell.Read.MeansFlat;
            equalMeans |= between && cell.Constant == Constant.Column && cell.Read.MeansFlat;
            equalMeansSpanned |= between && cell.Constant == Constant.Spanned && cell.Read.MeansFlat;
            spanned |= cell.Constant == Constant.Spanned && cell.Covariance == PanelCovarianceType.Unadjusted && cell.Read.ResponseFlat
                && unexact && NoVariance(cell, hasConstant: true);
            CheckFit(failures, cell, summary);
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        // Some between fit beside a constant holds means an ulp apart on a varying response, the case no tolerance may
        // call equal.
        Assert.True(nearEqual, "no fitted cell has entity means an ulp apart");
        // And the two cases these issues fix: equal means on a varying response beside a constant column or a spanned one
        // (#1847), and, unadjusted, a constant response beside a spanned constant whose residuals do not vanish (#1848).
        Assert.True(equalMeans && equalMeansSpanned, $"equal means {equalMeans}, beside a spanned constant {equalMeansSpanned}");
        Assert.True(covariance != PanelCovarianceType.Unadjusted || spanned, "no unadjusted fit beside a spanned constant on a constant response");
        Assert.Equal(fittedCells, fitted);
    }

    /// <summary>A fitted cell's tests and four R², each against the outcome the rule predicts for it.</summary>
    private static void CheckFit(List<string> failures, Cell cell, PanelSummary summary)
    {
        bool hasConstant = cell.Constant != Constant.None;
        Check(failures, summary.HasConstant == hasConstant, $"{cell}: HasConstant {summary.HasConstant}");
        bool noVariance = NoVariance(cell, hasConstant);
        // Standard errors are zero exactly where the residuals are at these magnitudes: s² under the unadjusted
        // covariance, the cleared block under the robust or clustered one.
        bool vanished = summary.StandardErrors.All(error => error is 0.0);

        // A constant alone leaves nothing to test; every other design holds a regressor, so both tests exist.
        if (cell.Constant == Constant.Alone)
        {
            Check(failures, summary.ModelTest is null && summary.RobustModelTest is null, $"{cell}: F {summary.ModelTest}, Wald {summary.RobustModelTest}, expected neither");
        }
        else if (summary.ModelTest is not { } model || summary.RobustModelTest is not { } wald)
        {
            failures.Add($"{cell}: F {summary.ModelTest}, Wald {summary.RobustModelTest}, expected both");
        }
        else
        {
            string expected = Expected(vanished, noVariance);
            Check(failures, Outcome(model.Statistic) == expected && PValue(model) == expected, $"{cell}: F {model.Statistic}, p {model.PValue}, expected {expected}");
            // The unadjusted Wald form is the classical F; a robust or clustered one is NaN on its zero block and divides by
            // no total.
            string robust = vanished ? "NaN" : "finite";
            string form = cell.Covariance == PanelCovarianceType.Unadjusted ? expected : robust;
            Check(failures, Outcome(wald.Statistic) == form && PValue(wald) == form, $"{cell}: Wald {wald.Statistic}, p {wald.PValue}, expected {form}");
            Check(failures, DegreesHold(summary, model, wald, hasConstant), $"{cell}: df F {model.DegreesOfFreedom}/{model.DenominatorDegreesOfFreedom}, Wald {wald.DegreesOfFreedom}/{wald.DenominatorDegreesOfFreedom}");
        }

        CheckRSquared(failures, cell, summary, noVariance || (vanished && FitTotalZero(cell, hasConstant)), hasConstant);
    }

    /// <summary>
    /// Both tests test every coefficient but the constant, the F over the residual degrees of freedom, the Wald form over
    /// them only when debiased, whichever branch answered.
    /// </summary>
    private static bool DegreesHold(PanelSummary summary, WaldTest model, WaldTest wald, bool hasConstant)
    {
        int tested = summary.Coefficients.Count - (hasConstant ? 1 : 0);
        int df = summary.ResidualDegreesOfFreedom;
        int? denominator = summary.Debiased ? df : null;
        return model.DegreesOfFreedom == tested && model.DenominatorDegreesOfFreedom == df
            && wald.DegreesOfFreedom == tested && wald.DenominatorDegreesOfFreedom == denominator;
    }

    /// <summary>The four R², each NaN where the rule reads its total zero, else finite; the three panel R² exactly 0 for a constant alone, which explains nothing.</summary>
    private static void CheckRSquared(List<string> failures, Cell cell, PanelSummary summary, bool fitUndefined, bool hasConstant)
    {
        string rSquared = fitUndefined ? "NaN" : "finite";
        Check(failures, Outcome(summary.RSquared) == rSquared, $"{cell}: R² {summary.RSquared}, expected {rSquared}");
        // The between total's zero; the overall one on a constant response beside a constant, or a zero one; the within
        // one in each entity.
        (string Name, double Value, bool Undefined)[] family =
        [
            ("between", summary.RSquaredBetween, BetweenTotalZero(cell, hasConstant)),
            ("overall", summary.RSquaredOverall, cell.Read.ResponseFlat && (hasConstant || cell.Read.ResponseZero)),
            ("within", summary.RSquaredWithin, cell.Read.FlatWithinEntity),
        ];
        foreach ((string name, double value, bool undefined) in family)
        {
            string expected = undefined ? "NaN" : "finite";
            bool zeroWhereAlone = cell.Constant != Constant.Alone || undefined || value is 0.0;
            Check(failures, Outcome(value) == expected && zeroWhereAlone, $"{cell}: {name} R² {value}, expected {expected}");
        }
    }

    private static void Check(List<string> failures, bool holds, string where)
    {
        if (!holds)
        {
            failures.Add(where);
        }
    }

    /// <summary>
    /// The refusal a cell meets, in its message's words, or null where it fits: a first difference beside a constant or
    /// over one period, too few rows for the between fit or the effects, too few for random effects' variance components,
    /// random effects' θ of 0/0, on a zero response or on 5 a constant alone fits exactly, or of one beside a spanned
    /// constant, refused as collinear, and one entity's single cluster.
    /// </summary>
    private static string? ExpectedRefusal(Cell cell)
    {
        bool oneEntity = cell.Shape == "one entity";
        bool onePeriod = cell.Shape == "one period";
        return cell.Estimator switch
        {
            Estimator.FirstDifference when cell.Constant == Constant.Column => "WithIntercept",
            Estimator.FirstDifference when cell.Constant is Constant.Spanned or Constant.Alone => "The regressors hold a constant",
            Estimator.FirstDifference when onePeriod => "A first difference needs two periods",
            Estimator.Between when oneEntity => "leave no residual degree of freedom",
            Estimator.Within when oneEntity => "leaves the effects nothing the poolability test can count",
            Estimator.Within when onePeriod => "residual degrees of freedom",
            Estimator.Period when oneEntity => "residual degrees of freedom",
            Estimator.Period when onePeriod => "leaves the effects nothing the poolability test can count",
            Estimator.TwoWay when oneEntity || onePeriod => "residual degrees of freedom",
            Estimator.Random when oneEntity || onePeriod => "The variance components need more rows",
            // θ is 0/0 where both variance components are computed zero, a zero response or 5 fitted exactly by a
            // constant alone, where 0.1 leaves them a rounding; and one beside a spanned constant it then zeroes.
            Estimator.Random when cell.Response == Response.AllZero
                || (cell.Constant == Constant.Alone && cell.Response == Response.ConstantInteger)
                || (cell.Constant == Constant.Spanned && cell.Response == Response.ConstantWithinEntity) => "collinear",
            _ when oneEntity && cell.Covariance == PanelCovarianceType.Clustered => "falls in one cluster",
            _ => null,
        };
    }

    /// <summary>A fit's own total, read only where its residuals vanish, so <c>1 − 0/0</c>: pooled, a constant response beside a constant or a zero one without; between, the means' total; differenced, zero differences.</summary>
    private static bool FitTotalZero(Cell cell, bool hasConstant) => cell.Estimator switch
    {
        Estimator.Pooled => hasConstant ? cell.Read.ResponseFlat : cell.Read.ResponseZero,
        Estimator.Between => BetweenTotalZero(cell, hasConstant),
        Estimator.FirstDifference => Differences(cell).All(d => d is 0.0),
        // The effects and random effects read their total's zero through NoVariance, which the R² reads first.
        _ => false,
    };

    private static IEnumerable<double> Differences(Cell cell) =>
        Enumerable.Range(1, cell.Y.Length - 1)
            .Where(i => cell.Entities[i] == cell.Entities[i - 1] && cell.Periods[i] == cell.Periods[i - 1] + 1)
            .Select(i => cell.Y[i] - cell.Y[i - 1]);

    private static string Expected(bool vanished, bool noVariance)
    {
        if (vanished)
        {
            return "+inf";
        }

        return noVariance ? "NaN" : "finite";
    }

    // A p-value of 0 goes with +∞, NaN with NaN, and one in [0, 1] with a finite statistic, read as its outcome.
    private static string PValue(WaldTest test)
    {
        double p = test.PValue;
        if (double.IsNaN(p))
        {
            return "NaN";
        }

        if (double.IsPositiveInfinity(test.Statistic))
        {
            return p is 0.0 ? "+inf" : "p";
        }

        return p is >= 0.0 and <= 1.0 ? "finite" : "p";
    }

    private static string Outcome(double statistic)
    {
        if (double.IsPositiveInfinity(statistic))
        {
            return "+inf";
        }

        if (double.IsNaN(statistic))
        {
            return "NaN";
        }

        return double.IsFinite(statistic) ? "finite" : "-inf";
    }
}
