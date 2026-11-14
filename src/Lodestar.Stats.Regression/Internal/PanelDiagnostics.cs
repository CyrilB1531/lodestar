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
        // A robust covariance of an exact fit is zero, for every reader below (#1779); the rows as given carry the rounding
        // a transformed fit inherits. The unadjusted one, s² times a fixed matrix, stays the classical F's covariance.
        bool exact = type != PanelCovarianceType.Unadjusted
            && ExactFit.Holds(
                fit.X,
                k,
                fit.Residuals,
                fit.Coefficients,
                fit.Conditioning,
                fit.Floors is { } floors ? new Transform(panel.X, panel.Y, floors) : null);
        if (exact)
        {
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

        (double overall, double within, double between) = Family(panel, fit.Coefficients, hasConstant);

        // A response the transform removes whole leaves the fit's R² and its F undefined (decision 0010, Review A pass 26).
        bool noVariance = PanelCore.Absorbs(panel, fit.ResponseGroups);
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
            ModelTest = ClassicalF(fit, hasConstant, noVariance),
            RobustModelTest = Wald(fit, exact, hasConstant, constantColumn, debiased),
            PoolabilityTest = fit.Poolability,
            ResidualVariance = fit.ResidualVariance,
            EffectsVariance = fit.EffectsVariance,
            Rho = fit.Rho,
            Theta = fit.Theta,
        };
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
    /// response does not vary, read exactly on the rows as given: overall and between where the response is constant (or
    /// zero without a constant), the latter's sufficient case, within where it is constant in each entity (Review A pass 26).
    /// </remarks>
    private static (double Overall, double Within, double Between) Family(PanelLayout panel, double[] b, bool hasConstant)
    {
        int k = panel.K;
        if (hasConstant && k == 1)
        {
            return (0.0, 0.0, 0.0);
        }

        double[] meanY = PanelLayout.GroupMeans(panel.Y, 1, panel.Entity, panel.EntityCount);
        double[] meanX = PanelLayout.GroupMeans(panel.X, k, panel.Entity, panel.EntityCount);
        double between = Explained(meanY, meanX, k, b, hasConstant);
        double overall = Explained(panel.Y, panel.X, k, b, hasConstant);
        double[] withinY = PanelLayout.Demean(panel.Y, 1, panel.Entity, panel.EntityCount);
        double[] withinX = PanelLayout.Demean(panel.X, k, panel.Entity, panel.EntityCount);
        double within = panel.PeriodCount == 1 ? 0.0 : Explained(withinY, withinX, k, b, centred: false);
        // S1244: without a constant the uncentred total is zero only on a response exactly zero.
#pragma warning disable S1244
        bool flat = OrdinaryLeastSquares.IsConstant(panel.Y) && (hasConstant || panel.Y[0] == 0.0);
#pragma warning restore S1244
        if (flat)
        {
            return (double.NaN, double.NaN, double.NaN);
        }

        bool flatWithin = panel.PeriodCount > 1 && PanelCore.ConstantWithin(panel.Y, panel.Entity, panel.EntityCount);
        return (overall, flatWithin ? double.NaN : within, between);
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
    private static WaldTest? ClassicalF(PanelFit fit, bool hasConstant, bool noVariance)
    {
        int numeratorDf = fit.K - (hasConstant ? 1 : 0);
        if (numeratorDf == 0)
        {
            return null;
        }

        double mean = hasConstant ? PanelCore.Mean(fit.Y) : 0.0;
        double total = 0.0;
        foreach (double value in fit.Y)
        {
            total += (value - mean) * (value - mean);
        }

        double rss = PanelCore.Dot(fit.Residuals, fit.Residuals);
        int df = fit.ResidualDegreesOfFreedom;
        if (rss <= 0.0 && ExactFit.Vanishes(fit.Residuals))
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

        double statistic = (total - rss) / numeratorDf / (rss / df);
        return new WaldTest(statistic, Distributions.FisherSf(statistic, numeratorDf, df), numeratorDf, df);
    }

    /// <summary>The same test as a Wald test under the chosen covariance: an F when debiased, else a χ².</summary>
    private static WaldTest? Wald(PanelFit fit, bool exact, bool hasConstant, int constantColumn, bool debiased)
    {
        int k = fit.K;
        int[] tested = [.. Enumerable.Range(0, k).Where(j => !(hasConstant && j == constantColumn))];
        if (tested.Length == 0)
        {
            return null;
        }

        int m = tested.Length;
        int df = fit.ResidualDegreesOfFreedom;
        if (exact)
        {
            // A robust covariance of an exact fit is zero, a singular block, whose test is NaN as on any other (#1779).
            return new WaldTest(double.NaN, double.NaN, m, debiased ? df : null);
        }

        if (ExactFit.Vanishes(fit.Residuals))
        {
            // Residuals exactly zero leave the unadjusted covariance zero: infinite, as the classical F.
            return new WaldTest(double.PositiveInfinity, 0.0, m, debiased ? df : null);
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
}
