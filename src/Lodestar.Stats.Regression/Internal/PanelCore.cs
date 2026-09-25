using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>One panel regression as estimated: the rows it ran on, its estimates and what the summary adds for it.</summary>
internal sealed class PanelFit
{
    public PanelFit(double[] y, double[] x, int k, double[] coefficients, double[] residuals, int residualDf)
    {
        Y = y;
        X = x;
        K = k;
        Coefficients = coefficients;
        Residuals = residuals;
        ResidualDegreesOfFreedom = residualDf;
    }

    /// <summary>The regression's response: transformed, averaged or differenced.</summary>
    public double[] Y { get; }

    /// <summary>The regression's regressors, row-major, transformed the same way.</summary>
    public double[] X { get; }

    public int K { get; }

    public double[] Coefficients { get; }

    public double[] Residuals { get; }

    public int ResidualDegreesOfFreedom { get; }

    public double[] Covariance { get; set; } = [];

    public int? Bandwidth { get; set; }

    /// <summary>The mean the regression's R² centres on, or zero without a constant.</summary>
    public double CentringMean { get; init; }

    public WaldTest? Poolability { get; init; }

    public double? ResidualVariance { get; init; }

    public double? EffectsVariance { get; init; }

    public double? Rho { get; init; }

    public double[]? Theta { get; init; }
}

/// <summary>The four panel estimators, each reduced to least squares on transformed rows.</summary>
internal static class PanelCore
{
    /// <summary><c>PanelOLS</c>: least squares on the rows with the chosen effects projected out.</summary>
    public static PanelFit FixedEffects(
        PanelLayout panel, bool entityEffects, bool timeEffects, bool hasConstant, Func<int, int[]?, int, PanelCovarianceSpec> spec)
    {
        int n = panel.N;
        int k = panel.K;
        double[] joined = Join(panel.Y, panel.X, k);
        double[] transformed = Within(panel, joined, k + 1, entityEffects, timeEffects);
        bool effects = entityEffects || timeEffects;
        if (effects && hasConstant)
        {
            // The grand means go back in, so the constant survives the projection.
            AddGrandMeans(transformed, joined, k + 1, n);
        }

        (double[] y, double[] x) = Split(transformed, k, n);
        double[] coefficients = LeastSquares(x, y, k);
        double[] residuals = Residuals(y, x, k, coefficients);
        int absorbed = Absorbed(panel, entityEffects, timeEffects, hasConstant);
        int residualDf = n - k - absorbed;
        int[]? effect = null;
        if (entityEffects != timeEffects)
        {
            effect = entityEffects ? panel.Entity : panel.Period;
        }

        var fit = new PanelFit(y, x, k, coefficients, residuals, residualDf)
        {
            CentringMean = hasConstant ? Mean(panel.Y) : 0.0,
            Poolability = effects ? Poolability(panel, residuals, absorbed, residualDf, hasConstant) : null,
        };

        (double residualVariance, double effectsVariance, double rho) = Decomposition(panel, coefficients, residuals);
        PanelFit decomposed = With(fit, residualVariance, effectsVariance, rho, null);
        (decomposed.Covariance, decomposed.Bandwidth) =
            PanelCovariance.Estimate(x, k, residuals, spec(absorbed, effect, n));
        return decomposed;
    }

    /// <summary>The effects a fixed-effects fit absorbs, the first of them dropped when a constant stays in.</summary>
    public static int Absorbed(PanelLayout panel, bool entityEffects, bool timeEffects, bool hasConstant)
    {
        int absorbed = 0;
        bool dropFirst = hasConstant;
        if (entityEffects)
        {
            absorbed += panel.EntityCount - (dropFirst ? 1 : 0);
            dropFirst = true;
        }

        if (timeEffects)
        {
            absorbed += panel.PeriodCount - (dropFirst ? 1 : 0);
        }

        return absorbed;
    }

    private static void AddGrandMeans(double[] transformed, double[] original, int columns, int n)
    {
        double[] means = PanelLayout.GroupMeans(original, columns, new int[n], 1);
        for (int row = 0; row < n; row++)
        {
            for (int j = 0; j < columns; j++)
            {
                transformed[(row * columns) + j] += means[j];
            }
        }
    }

    /// <summary><c>BetweenOLS</c>: least squares on the entity means.</summary>
    public static PanelFit Between(PanelLayout panel, bool hasConstant, PanelCovarianceSpec spec)
    {
        int k = panel.K;
        double[] y = PanelLayout.GroupMeans(panel.Y, 1, panel.Entity, panel.EntityCount);
        double[] x = PanelLayout.GroupMeans(panel.X, k, panel.Entity, panel.EntityCount);
        double[] coefficients = LeastSquares(x, y, k);
        double[] residuals = Residuals(y, x, k, coefficients);
        var fit = new PanelFit(y, x, k, coefficients, residuals, panel.EntityCount - k)
        {
            CentringMean = hasConstant ? Mean(y) : 0.0,
        };
        (fit.Covariance, fit.Bandwidth) = PanelCovariance.Estimate(x, k, residuals, spec);
        return fit;
    }

    /// <summary><c>FirstDifferenceOLS</c>: least squares on the differences between adjacent periods of each entity.</summary>
    /// <param name="panel">The sorted panel.</param>
    /// <param name="spec">The covariance, from the differenced rows' entities, periods and cluster labels.</param>
    public static PanelFit FirstDifference(PanelLayout panel, Func<int[], int[], int[]?, PanelCovarianceSpec> spec)
    {
        int k = panel.K;
        var ys = new List<double>();
        var xs = new List<double>();
        var entities = new List<int>();
        var periods = new List<int>();
        var clusters = panel.Clusters is null ? null : new List<int>();
        for (int row = 1; row < panel.N; row++)
        {
            if (panel.Entity[row] != panel.Entity[row - 1] || panel.Period[row] != panel.Period[row - 1] + 1)
            {
                continue;
            }

            ys.Add(panel.Y[row] - panel.Y[row - 1]);
            for (int j = 0; j < k; j++)
            {
                xs.Add(panel.X[(row * k) + j] - panel.X[((row - 1) * k) + j]);
            }

            entities.Add(panel.Entity[row]);
            periods.Add(panel.Period[row]);
            if (clusters is not null)
            {
                if (panel.Clusters![row] != panel.Clusters[row - 1])
                {
                    throw new ArgumentException(
                        "A cluster label changes between two rows that are differenced; the reference refuses it too.");
                }

                clusters.Add(panel.Clusters[row]);
            }
        }

        double[] y = [.. ys];
        double[] x = [.. xs];
        if (y.Length <= k)
        {
            throw new ArgumentException($"{y.Length} differenced rows leave no residual degree of freedom for {k} regressors.");
        }

        double[] coefficients = LeastSquares(x, y, k);
        double[] residuals = Residuals(y, x, k, coefficients);
        var fit = new PanelFit(y, x, k, coefficients, residuals, y.Length - k);
        (fit.Covariance, fit.Bandwidth) =
            PanelCovariance.Estimate(x, k, residuals, spec([.. entities], [.. periods], clusters is null ? null : [.. clusters]));
        return fit;
    }

    /// <summary><c>RandomEffects</c>: Swamy and Arora's variance components, then least squares on the quasi-demeaned rows.</summary>
    public static PanelFit RandomEffects(PanelLayout panel, bool hasConstant, PanelCovarianceSpec spec)
    {
        int n = panel.N;
        int k = panel.K;
        int entities = panel.EntityCount;
        double[] joined = Join(panel.Y, panel.X, k);
        double[] within = PanelLayout.Demean(joined, k + 1, panel.Entity, entities);
        if (hasConstant)
        {
            AddGrandMeans(within, joined, k + 1, n);
        }

        (double[] wy, double[] wx) = Split(within, k, n);
        double[] withinResiduals = Residuals(wy, wx, k, LeastSquares(wx, wy, k));
        double[] meanY = PanelLayout.GroupMeans(panel.Y, 1, panel.Entity, entities);
        double[] meanX = PanelLayout.GroupMeans(panel.X, k, panel.Entity, entities);
        double[] betweenResiduals = Residuals(meanY, meanX, k, LeastSquares(meanX, meanY, k));

        double sigmaE = Dot(withinResiduals, withinResiduals) / (n - k - entities + 1);
        int[] sizes = panel.EntitySizes();
        double inverseSum = 0.0;
        foreach (int size in sizes)
        {
            inverseSum += 1.0 / size;
        }

        double harmonic = entities / inverseSum;
        double sigmaU = Math.Max(0.0, (Dot(betweenResiduals, betweenResiduals) / (entities - k)) - (sigmaE / harmonic));
        var theta = new double[entities];
        for (int e = 0; e < entities; e++)
        {
            theta[e] = 1.0 - Math.Sqrt(sigmaE / ((sizes[e] * sigmaU) + sigmaE));
        }

        var y = new double[n];
        var x = new double[n * k];
        for (int row = 0; row < n; row++)
        {
            int e = panel.Entity[row];
            y[row] = panel.Y[row] - (theta[e] * meanY[e]);
            for (int j = 0; j < k; j++)
            {
                x[(row * k) + j] = panel.X[(row * k) + j] - (theta[e] * meanX[(e * k) + j]);
            }
        }

        double[] coefficients = LeastSquares(x, y, k);
        double[] residuals = Residuals(y, x, k, coefficients);
        var fit = new PanelFit(y, x, k, coefficients, residuals, n - k)
        {
            CentringMean = hasConstant ? Mean(y) : 0.0,
            ResidualVariance = sigmaE,
            EffectsVariance = sigmaU,
            Rho = sigmaU / (sigmaU + sigmaE),
            Theta = theta,
        };
        (fit.Covariance, fit.Bandwidth) = PanelCovariance.Estimate(x, k, residuals, spec);
        return fit;
    }

    /// <summary>The joined block with the effects projected out: one-way by demeaning, two-way exactly.</summary>
    /// <remarks>
    /// Two ways are the reference's default algorithm: demean by the larger dimension, then purge the demeaned dummies of
    /// the other, its first category dropped — the exact within transform on a balanced or an unbalanced panel.
    /// </remarks>
    private static double[] Within(PanelLayout panel, double[] block, int columns, bool entityEffects, bool timeEffects)
    {
        if (!entityEffects && !timeEffects)
        {
            return (double[])block.Clone();
        }

        if (!timeEffects)
        {
            return PanelLayout.Demean(block, columns, panel.Entity, panel.EntityCount);
        }

        if (!entityEffects)
        {
            return PanelLayout.Demean(block, columns, panel.Period, panel.PeriodCount);
        }

        bool byEntity = panel.EntityCount > panel.PeriodCount;
        (int[] group, int groupCount) = byEntity ? (panel.Entity, panel.EntityCount) : (panel.Period, panel.PeriodCount);
        (int[] other, int otherCount) = byEntity ? (panel.Period, panel.PeriodCount) : (panel.Entity, panel.EntityCount);
        double[] demeaned = PanelLayout.Demean(block, columns, group, groupCount);
        if (otherCount < 2)
        {
            return demeaned;
        }

        int dummies = otherCount - 1;
        var indicator = new double[panel.N * dummies];
        for (int row = 0; row < panel.N; row++)
        {
            if (other[row] > 0)
            {
                indicator[(row * dummies) + other[row] - 1] = 1.0;
            }
        }

        double[] purged = PanelLayout.Demean(indicator, dummies, group, groupCount);
        return IvCore.Annihilate(demeaned, columns, purged, dummies, panel.N);
    }

    /// <summary>The F test that the effects are zero: pooled least squares against the fit with them.</summary>
    private static WaldTest Poolability(PanelLayout panel, double[] residuals, int absorbed, int residualDf, bool hasConstant)
    {
        int n = panel.N;
        int k = panel.K;
        double[] y = (double[])panel.Y.Clone();
        double[] x = (double[])panel.X.Clone();
        int numeratorDf = absorbed;
        if (!hasConstant)
        {
            y = PanelLayout.Demean(y, 1, new int[n], 1);
            x = PanelLayout.Demean(x, k, new int[n], 1);
            numeratorDf--;
        }

        double[] pooled = Residuals(y, x, k, LeastSquares(x, y, k));
        double rss = Dot(residuals, residuals);
        if (rss <= 0.0)
        {
            return new WaldTest(double.PositiveInfinity, 0.0, numeratorDf, residualDf);
        }

        double statistic = ((Dot(pooled, pooled) - rss) / numeratorDf) / (rss / residualDf);
        return new WaldTest(statistic, Distributions.FisherSf(statistic, numeratorDf, residualDf), numeratorDf, residualDf);
    }

    /// <summary>The idiosyncratic and effects variances fixed effects reports, and the effects' share.</summary>
    private static (double Residual, double Effects, double Rho) Decomposition(
        PanelLayout panel, double[] coefficients, double[] residuals)
    {
        int n = panel.N;
        double[] total = Residuals(panel.Y, panel.X, panel.K, coefficients);
        double sigmaTotal = Dot(total, total) / n;
        double sigmaResidual = Dot(residuals, residuals) / n;
        double effects = sigmaTotal - sigmaResidual;
        return (sigmaResidual, effects, sigmaTotal > 0.0 ? effects / sigmaTotal : 0.0);
    }

    private static PanelFit With(PanelFit fit, double residual, double effects, double rho, double[]? theta) =>
        new(fit.Y, fit.X, fit.K, fit.Coefficients, fit.Residuals, fit.ResidualDegreesOfFreedom)
        {
            CentringMean = fit.CentringMean,
            Poolability = fit.Poolability,
            ResidualVariance = residual,
            EffectsVariance = effects,
            Rho = rho,
            Theta = theta,
        };

    /// <summary><c>(XᵀX)⁻¹Xᵀy</c>.</summary>
    public static double[] LeastSquares(double[] x, double[] y, int k)
    {
        int n = y.Length;
        double[] xx = Dense.Gram(x, k, n);
        double[] xy = Dense.CrossProduct(x, k, y, 1, n);
        return Dense.Multiply(IvCore.Invert(xx, k), xy, k, k, 1);
    }

    public static double[] Residuals(double[] y, double[] x, int k, double[] coefficients)
    {
        var residuals = new double[y.Length];
        for (int row = 0; row < y.Length; row++)
        {
            double fitted = 0.0;
            for (int j = 0; j < k; j++)
            {
                fitted += x[(row * k) + j] * coefficients[j];
            }

            residuals[row] = y[row] - fitted;
        }

        return residuals;
    }

    public static double Dot(double[] a, double[] b)
    {
        double sum = 0.0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += a[i] * b[i];
        }

        return sum;
    }

    public static double Mean(double[] values)
    {
        double sum = 0.0;
        foreach (double value in values)
        {
            sum += value;
        }

        return sum / values.Length;
    }

    private static double[] Join(double[] y, double[] x, int k)
    {
        int n = y.Length;
        var joined = new double[n * (k + 1)];
        for (int row = 0; row < n; row++)
        {
            joined[row * (k + 1)] = y[row];
            Array.Copy(x, row * k, joined, (row * (k + 1)) + 1, k);
        }

        return joined;
    }

    private static (double[] Y, double[] X) Split(double[] joined, int k, int n)
    {
        var y = new double[n];
        var x = new double[n * k];
        for (int row = 0; row < n; row++)
        {
            y[row] = joined[row * (k + 1)];
            Array.Copy(joined, (row * (k + 1)) + 1, x, row * k, k);
        }

        return (y, x);
    }
}
