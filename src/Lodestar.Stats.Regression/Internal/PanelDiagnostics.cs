using Lodestar.Stats.Regression.Panel;

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
        var errors = new double[k];
        var t = new double[k];
        var p = new double[k];
        var lower = new double[k];
        var upper = new double[k];
        double half = 1.0 - ((1.0 - confidenceLevel) / 2.0);
        double quantile = debiased ? Distributions.StudentQuantile(half, residualDf) : Distributions.NormalQuantile(half);
        for (int j = 0; j < k; j++)
        {
            errors[j] = Math.Sqrt(fit.Covariance[(j * k) + j]);
            t[j] = fit.Coefficients[j] / errors[j];
            p[j] = debiased
                ? Math.Min(1.0, 2.0 * Distributions.StudentSf(Math.Abs(t[j]), residualDf))
                : Distributions.ChiSquaredSf(t[j] * t[j], 1.0);
            lower[j] = fit.Coefficients[j] - (quantile * errors[j]);
            upper[j] = fit.Coefficients[j] + (quantile * errors[j]);
        }

        (double overall, double within, double between) = Family(panel, fit.Coefficients, hasConstant);
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
            RSquared = RSquared(fit),
            RSquaredWithin = within,
            RSquaredBetween = between,
            RSquaredOverall = overall,
            ModelTest = ClassicalF(fit, hasConstant),
            RobustModelTest = Wald(fit, hasConstant, constantColumn, debiased),
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
    /// squares, centred when the regressors hold a constant; all three are zero for a constant alone.
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

    /// <summary>The homoskedastic F of every coefficient but the constant, from the regression's own sums of squares.</summary>
    private static WaldTest? ClassicalF(PanelFit fit, bool hasConstant)
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
        if (rss <= 0.0)
        {
            // An exact fit: the reference's ratio is a division by zero, an infinite F.
            return new WaldTest(double.PositiveInfinity, 0.0, numeratorDf, df);
        }

        double statistic = (total - rss) / numeratorDf / (rss / df);
        return new WaldTest(statistic, Distributions.FisherSf(statistic, numeratorDf, df), numeratorDf, df);
    }

    /// <summary>The same test as a Wald test under the chosen covariance: an F when debiased, else a χ².</summary>
    private static WaldTest? Wald(PanelFit fit, bool hasConstant, int constantColumn, bool debiased)
    {
        int k = fit.K;
        int[] tested = [.. Enumerable.Range(0, k).Where(j => !(hasConstant && j == constantColumn))];
        if (tested.Length == 0)
        {
            return null;
        }

        int m = tested.Length;
        int df = fit.ResidualDegreesOfFreedom;
        if (PanelCore.Dot(fit.Residuals, fit.Residuals) <= 0.0)
        {
            // An exact fit's covariance is zero, which no Wald statistic inverts: infinite, as the classical F.
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

        double statistic = PanelCore.Dot(b, Dense.Multiply(IvCore.Invert(block, m), b, m, m, 1));
        return debiased
            ? new WaldTest(statistic / m, Distributions.FisherSf(statistic / m, m, df), m, df)
            : new WaldTest(statistic, Distributions.ChiSquaredSf(statistic, m), m, null);
    }
}
