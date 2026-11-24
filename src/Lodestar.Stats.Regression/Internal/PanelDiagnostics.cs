using Lodestar.Stats.Regression.Panel;

// CS0618: FisherSf is the F tail's name in the published Lodestar.Stats this package builds against, obsolete in the
// working tree (#1217); the call moves to FisherSnedecorSf with the floor.
#pragma warning disable CS0618

namespace Lodestar.Stats.Regression.Internal;

/// <summary>Turns a panel fit into the table <c>linearmodels</c> reports.</summary>
internal static class PanelDiagnostics
{
    /// <summary>The table, the three R² the reference reads off the panel, and the two model tests.</summary>
    public static PanelSummary Summarise(
        PanelLayout panel, PanelFit fit, PanelCovarianceType type, bool debiased, double confidenceLevel,
        bool hasConstant, int constantColumn)
    {
        int k = fit.K;
        int residualDf = fit.ResidualDegreesOfFreedom;
        // Residuals exactly zero leave a robust covariance zero, for every reader below (#1779, #1784). The unadjusted one,
        // s² times a fixed matrix, stays the classical F's covariance, whose tests read +∞ there.
        bool vanishes = ExactFit.Vanishes(fit.Residuals);
        bool exact = type != PanelCovarianceType.Unadjusted && vanishes;
        if (exact)
        {
            // Cleared rather than read: zero whatever the bread and the scaling the estimate multiplied its zero meat by.
            Array.Clear(fit.Covariance, 0, fit.Covariance.Length);
        }

        var errors = new double[k];
        var t = new double[k];
        var p = new double[k];
        var lower = new double[k];
        var upper = new double[k];
        double quantile = debiased ? CriticalValue.TwoSidedStudent(confidenceLevel, residualDf) : CriticalValue.TwoSided(confidenceLevel);
        for (int j = 0; j < k; j++)
        {
            errors[j] = Math.Sqrt(fit.Covariance[(j * k) + j]);
            t[j] = exact ? double.NaN : fit.Coefficients[j] / errors[j];
            p[j] = exact ? double.NaN : TwoSided(t[j], debiased, residualDf);
            lower[j] = fit.Coefficients[j] - (quantile * errors[j]);
            upper[j] = fit.Coefficients[j] + (quantile * errors[j]);
        }

        (bool flat, bool flatWithin, bool noVariance) = Constancy(panel, fit.ResponseGroups);
        (double overall, double within, double between) = Family(panel, fit.Coefficients, hasConstant, flat, flatWithin);

        // The Wald form is the classical F beside a constant column or none, read on the columns as given as the response
        // is, whatever a transform makes of them: one they only span restricts the fit to a column no total divides (#1790).
        bool spannedConstant = noVariance && hasConstant && !IvScores.IsFlat(panel.X, panel.K, panel.N, constantColumn);
        bool undefinedWald = noVariance && !spannedConstant;
        return new PanelSummary
        {
            Coefficients = fit.Coefficients,
            StandardErrors = errors,
            TStatistics = t,
            PValues = p,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            CovarianceType = type,
            Debiased = debiased,
            Bandwidth = type == PanelCovarianceType.Kernel ? fit.Bandwidth : null,
            ConfidenceLevel = confidenceLevel,
            HasConstant = hasConstant,
            ObservationCount = fit.Y.Length,
            EntityCount = panel.EntityCount,
            PeriodCount = panel.PeriodCount,
            ResidualDegreesOfFreedom = residualDf,
            RSquared = noVariance ? double.NaN : RSquared(fit),
            RSquaredWithin = within,
            RSquaredBetween = between,
            RSquaredOverall = overall,
            ModelTest = ClassicalF(fit, hasConstant, vanishes, noVariance),
            RobustModelTest = Wald(fit, type, vanishes, undefinedWald, hasConstant, constantColumn, debiased),
            PoolabilityTest = fit.Poolability,
            ResidualVariance = fit.ResidualVariance,
            EffectsVariance = fit.EffectsVariance,
            Rho = fit.Rho,
            Theta = fit.Theta,
        };
    }

    /// <summary>
    /// Whether the response as given is constant, constant within each entity, and constant on every group
    /// <paramref name="groups"/> names, which leaves the fit's R² and its F undefined (decision 0010, Review A pass 26).
    /// </summary>
    /// <remarks>
    /// Read once, for the R² family and the groups; a constant response is constant within every group, so its scans are
    /// skipped. An entity of one row is constant within itself, so a panel of one period is constant within each entity.
    /// </remarks>
    private static (bool Flat, bool FlatWithin, bool NoVariance) Constancy(PanelLayout panel, ResponseGroups groups)
    {
        bool flat = OrdinaryLeastSquares.IsConstant(panel.Y);
        bool flatWithin = flat || PanelCore.ConstantWithin(panel.Y, panel.Entity, panel.EntityCount);
        bool flatPeriod = flat
            || (groups is ResponseGroups.Period or ResponseGroups.EntityOrPeriod
                && PanelCore.ConstantWithin(panel.Y, panel.Period, panel.PeriodCount));
        bool noVariance = groups switch
        {
            ResponseGroups.Overall => flat,
            ResponseGroups.Entity => flatWithin,
            ResponseGroups.Period => flatPeriod,
            ResponseGroups.EntityOrPeriod => flatWithin || flatPeriod,
            _ => false,
        };

        return (flat, flatWithin, noVariance);
    }

    private static double RSquared(PanelFit fit)
    {
        double total = 0.0;
        foreach (double value in fit.Y)
        {
            total += (value - fit.CentringMean) * (value - fit.CentringMean);
        }

        return 1.0 - (PanelCore.Dot(fit.Residuals, fit.Residuals) / total);
    }

    /// <summary>The overall, within and between R² of the coefficients over the panel as given.</summary>
    /// <remarks>
    /// <c>_rsquared</c>: the rows as given, the entity-demeaned rows and the entity means, each against its own total sum of
    /// squares, centred when the regressors hold a constant; all three are zero for a constant alone. Each is NaN where its
    /// response does not vary, read exactly on the rows as given and before the constant's zeros (#1789): overall and
    /// between where the response is constant (or zero without a constant), the latter's sufficient case, within where it
    /// is constant in each entity (Review A pass 26).
    /// </remarks>
    private static (double Overall, double Within, double Between) Family(
        PanelLayout panel, double[] b, bool hasConstant, bool flat, bool flatWithin)
    {
        int k = panel.K;
        // Without a constant the uncentred total is zero only on a response exactly zero.
        if (flat && (hasConstant || ExactFit.IsZero(panel.Y[0])))
        {
            return (double.NaN, double.NaN, double.NaN);
        }

        // A constant alone explains nothing, so each R² reads 0, the within one NaN where its total is zero.
        if (hasConstant && k == 1)
        {
            return (0.0, flatWithin ? double.NaN : 0.0, 0.0);
        }

        double[] meanY = PanelLayout.GroupMeans(panel.Y, 1, panel.Entity, panel.EntityCount);
        double[] meanX = PanelLayout.GroupMeans(panel.X, k, panel.Entity, panel.EntityCount);
        double between = Explained(meanY, meanX, k, b, hasConstant);
        double overall = Explained(panel.Y, panel.X, k, b, hasConstant);
        double within = flatWithin
            ? double.NaN
            : Explained(
                PanelLayout.Demean(panel.Y, 1, panel.Entity, panel.EntityCount),
                PanelLayout.Demean(panel.X, k, panel.Entity, panel.EntityCount),
                k,
                b,
                centred: false);
        return (overall, within, between);
    }

    private static double Explained(double[] y, double[] x, int k, double[] b, bool centred)
    {
        double[] residuals = PanelCore.Residuals(y, x, k, b);
        double mean = centred ? PanelCore.Mean(y) : 0.0;
        double total = 0.0;
        foreach (double value in y)
        {
            total += (value - mean) * (value - mean);
        }

        return total > 0.0 ? 1.0 - (PanelCore.Dot(residuals, residuals) / total) : 0.0;
    }

    private static double TwoSided(double t, bool debiased, int residualDf) =>
        debiased
            ? Math.Min(1.0, 2.0 * Distributions.StudentSf(Math.Abs(t), residualDf))
            : Distributions.ChiSquaredSf(t * t, 1.0);

    /// <summary>The homoskedastic F of every coefficient but the constant, from the regression's own sums of squares.</summary>
    private static WaldTest? ClassicalF(PanelFit fit, bool hasConstant, bool vanishes, bool noVariance)
    {
        int numeratorDf = fit.K - (hasConstant ? 1 : 0);
        if (numeratorDf == 0)
        {
            return null;
        }

        int df = fit.ResidualDegreesOfFreedom;
        if (vanishes)
        {
            // Residuals exactly zero: the reference's ratio is a division by zero, an infinite F (#1779).
            return new WaldTest(double.PositiveInfinity, 0.0, numeratorDf, df);
        }

        // Nothing to explain, read exactly on the rows as given: its ratio read −df off the rounding, as linearmodels
        // does, and is undefined, as OLS's (decision 0010, Review A passes 24, 25).
        if (noVariance)
        {
            return new WaldTest(double.NaN, double.NaN, numeratorDf, df);
        }

        double mean = hasConstant ? PanelCore.Mean(fit.Y) : 0.0;
        double total = 0.0;
        foreach (double value in fit.Y)
        {
            total += (value - mean) * (value - mean);
        }

        double rss = PanelCore.Dot(fit.Residuals, fit.Residuals);
        double statistic = (total - rss) / numeratorDf / (rss / df);
        return new WaldTest(statistic, Distributions.FisherSf(statistic, numeratorDf, df), numeratorDf, df);
    }

    /// <summary>The same test as a Wald test under the chosen covariance: an F when debiased, else a χ².</summary>
    /// <remarks>
    /// Under any other covariance, residuals exactly zero leave it zero and the test NaN (#1779). Unadjusted, it is the
    /// classical F in Wald form, so it reads as <see cref="ClassicalF"/> does: <c>+∞</c> on residuals exactly zero, then
    /// NaN where <paramref name="undefined"/>: a response the transform removes whole, beside a constant column or none,
    /// where the form is the classical F (decision 0010, #1790).
    /// </remarks>
    private static WaldTest? Wald(
        PanelFit fit, PanelCovarianceType type, bool vanishes, bool undefined, bool hasConstant, int constantColumn, bool debiased)
    {
        int k = fit.K;
        int[] tested = [.. Enumerable.Range(0, k).Where(j => !(hasConstant && j == constantColumn))];
        if (tested.Length == 0)
        {
            return null;
        }

        int m = tested.Length;
        int df = fit.ResidualDegreesOfFreedom;
        if (Degenerate(type, vanishes, undefined, m, debiased ? df : null) is { } degenerate)
        {
            return degenerate;
        }

        var b = new double[m];
        var block = new double[m * m];
        for (int i = 0; i < m; i++)
        {
            b[i] = fit.Coefficients[tested[i]];
            for (int j = 0; j < m; j++)
            {
                block[(i * m) + j] = fit.Covariance[(tested[i] * k) + tested[j]];
            }
        }

        double statistic = CovarianceRank.QuadraticForm(b, block, m, fit.LargestRank);
        return debiased
            ? new WaldTest(statistic / m, Distributions.FisherSf(statistic / m, m, df), m, df)
            : new WaldTest(statistic, Distributions.ChiSquaredSf(statistic, m), m, null);
    }

    /// <summary>The Wald form's answer where its covariance is zero or its response has nothing to explain, else null.</summary>
    private static WaldTest? Degenerate(PanelCovarianceType type, bool vanishes, bool undefined, int m, int? denominatorDf)
    {
        bool unadjusted = type == PanelCovarianceType.Unadjusted;
        if (vanishes)
        {
            // Residuals exactly zero leave every covariance zero: the unadjusted test infinite, as the classical F; any
            // other a singular block, whose test is NaN as on any other (#1779).
            return unadjusted
                ? new WaldTest(double.PositiveInfinity, 0.0, m, denominatorDf)
                : new WaldTest(double.NaN, double.NaN, m, denominatorDf);
        }

        return unadjusted && undefined ? new WaldTest(double.NaN, double.NaN, m, denominatorDf) : null;
    }
}
