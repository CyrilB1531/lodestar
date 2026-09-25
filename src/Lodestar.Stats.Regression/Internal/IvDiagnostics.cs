using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>Turns an instrumental-variables fit into the table <c>linearmodels</c> reports, diagnostics included.</summary>
internal static class IvDiagnostics
{
    /// <summary>The table, the R², the model test, the first stage and the overidentification test.</summary>
    /// <param name="problem">The problem the fit solved.</param>
    /// <param name="fit">The fit.</param>
    /// <param name="spec">The covariance it was reported under.</param>
    /// <param name="confidenceLevel">The intervals' level.</param>
    /// <param name="gmm">Whether GMM produced it, which selects Hansen's J over Sargan's test.</param>
    /// <param name="instrumentCount">The excluded instruments, the trailing columns of <c>Z</c>.</param>
    public static IvSummary Summarise(
        IvProblem problem, IvFit fit, IvCovarianceSpec spec, double confidenceLevel, bool gmm, int instrumentCount)
    {
        int n = problem.N;
        int k = problem.K;
        int residualDf = n - k;
        var errors = new double[k];
        var t = new double[k];
        var p = new double[k];
        var lower = new double[k];
        var upper = new double[k];
        double half = 1.0 - ((1.0 - confidenceLevel) / 2.0);
        double quantile = spec.Debiased
            ? Distributions.StudentQuantile(half, residualDf)
            : Distributions.NormalQuantile(half);
        for (int j = 0; j < k; j++)
        {
            errors[j] = Math.Sqrt(fit.Covariance[(j * k) + j]);
            t[j] = fit.Coefficients[j] / errors[j];
            p[j] = TwoSided(t[j], spec.Debiased, residualDf);
            lower[j] = fit.Coefficients[j] - (quantile * errors[j]);
            upper[j] = fit.Coefficients[j] + (quantile * errors[j]);
        }

        (bool hasConstant, _) = IvScores.FindConstant(problem.X, k, n);
        double r2 = RSquared(problem.Y, fit.Residuals, hasConstant);
        int c = hasConstant ? 1 : 0;
        return new IvSummary
        {
            Coefficients = fit.Coefficients,
            StandardErrors = errors,
            TStatistics = t,
            PValues = p,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            CovarianceType = spec.Type,
            Debiased = spec.Debiased,
            Bandwidth = spec.Type == IvCovarianceType.Kernel ? fit.Bandwidth : null,
            ConfidenceLevel = confidenceLevel,
            HasConstant = hasConstant,
            RSquared = r2,
            AdjustedRSquared = 1.0 - ((n - c) / (double)(n - k) * (1.0 - r2)),
            ModelTest = ModelTest(problem, fit, spec.Debiased),
            ResidualDegreesOfFreedom = residualDf,
            Kappa = gmm ? null : fit.Kappa,
            FirstStage = FirstStage(problem, spec with { Bandwidth = fit.Bandwidth ?? spec.Bandwidth }, instrumentCount),
            Overidentification = gmm ? HansenJ(problem, fit) : Sargan(problem, fit),
        };
    }

    /// <summary>The joint Wald test that every coefficient but the first constant column is zero.</summary>
    private static IvTest? ModelTest(IvProblem problem, IvFit fit, bool debiased)
    {
        int k = problem.K;
        int constant = IvScores.FirstFlatColumn(problem.X, k, problem.N);
        int[] tested = [.. Enumerable.Range(0, k).Where(j => j != constant)];
        if (tested.Length == 0)
        {
            return null;
        }

        double statistic = Quadratic(fit.Coefficients, fit.Covariance, k, tested);
        int df = tested.Length;
        int residualDf = problem.N - k;
        return debiased
            ? new IvTest(statistic / df, Distributions.FisherSf(statistic / df, df, residualDf), df, residualDf)
            : new IvTest(statistic, Distributions.ChiSquaredSf(statistic, df), df, null);
    }

    /// <summary>One row per endogenous regressor, each regression reported under the fit's own covariance.</summary>
    /// <remarks>
    /// The fit's covariance configuration travels whole, the bandwidth it chose included, as the reference's does; the
    /// instruments' test is an F under an unadjusted covariance and a χ² under the others.
    /// </remarks>
    private static IvFirstStage[] FirstStage(IvProblem problem, IvCovarianceSpec spec, int instrumentCount)
    {
        int n = problem.N;
        int l = problem.L;
        int shared = problem.ExogenousCount;
        int endogenousCount = problem.K - shared;
        var ols = new IvProblem(problem.Z, l, problem.Z, l, problem.Y, n, l);
        (bool zConstant, _) = IvScores.FindConstant(problem.Z, l, n);

        double[] instruments = Trailing(problem.Z, l, n, instrumentCount);
        double[]? exogenous = shared == 0 ? null : IvCore.Columns(problem.X, problem.K, n, shared);
        double[] partialInstruments = exogenous is null
            ? instruments
            : IvCore.Annihilate(instruments, instrumentCount, exogenous, shared, n);
        (bool partialConstant, _) = IvScores.FindConstant(partialInstruments, instrumentCount, n);

        double[] shea = SheaFactors(problem);
        var rows = new IvFirstStage[endogenousCount];
        int[] tested = [.. Enumerable.Range(l - instrumentCount, instrumentCount)];
        for (int j = 0; j < endogenousCount; j++)
        {
            double[] endogenous = Column(problem.X, problem.K, n, shared + j);
            IvFit full = IvCore.KClass(Retarget(ols, endogenous), 1.0, spec);
            double statistic = Quadratic(full.Coefficients, full.Covariance, l, tested);
            IvTest test = spec.Type == IvCovarianceType.Unadjusted
                ? new IvTest(
                    statistic / instrumentCount,
                    Distributions.FisherSf(statistic / instrumentCount, instrumentCount, n - l),
                    instrumentCount,
                    n - l)
                : new IvTest(statistic, Distributions.ChiSquaredSf(statistic, instrumentCount), instrumentCount, null);

            double[] partialResponse = exogenous is null ? endogenous : IvCore.Annihilate(endogenous, 1, exogenous, shared, n);
            double[] partialResiduals = IvCore.Annihilate(partialResponse, 1, partialInstruments, instrumentCount, n);
            rows[j] = new IvFirstStage
            {
                RSquared = RSquared(endogenous, full.Residuals, zConstant),
                PartialRSquared = RSquared(partialResponse, partialResiduals, partialConstant),
                SheaRSquared = shea[j],
                InstrumentTest = test,
            };
        }

        return rows;
    }

    /// <summary>Shea's partial R², from unadjusted OLS and unadjusted 2SLS on the same regressors.</summary>
    private static double[] SheaFactors(IvProblem problem)
    {
        int n = problem.N;
        int k = problem.K;
        var unadjusted = new IvCovarianceSpec(IvCovarianceType.Unadjusted, false, IvKernel.Bartlett, null, null);
        IvFit twoStage = IvCore.KClass(problem, 1.0, unadjusted);
        IvFit ols = IvCore.KClass(new IvProblem(problem.X, k, problem.X, k, problem.Y, n, k), 1.0, unadjusted);
        (bool constant, _) = IvScores.FindConstant(problem.X, k, n);
        double r2TwoStage = RSquared(problem.Y, twoStage.Residuals, constant);
        double r2Ols = RSquared(problem.Y, ols.Residuals, constant);
        var shea = new double[k - problem.ExogenousCount];
        for (int j = 0; j < shea.Length; j++)
        {
            int column = problem.ExogenousCount + j;
            double ratio = Math.Sqrt(ols.Covariance[(column * k) + column]) / Math.Sqrt(twoStage.Covariance[(column * k) + column]);
            shea[j] = ratio * ratio * (1.0 - r2TwoStage) / (1.0 - r2Ols);
        }

        return shea;
    }

    /// <summary>Sargan's <c>n·(1 − ûᵀû/eᵀe)</c>, <c>û</c> the residuals purged of the instruments; <see langword="null"/> when just identified.</summary>
    private static IvTest? Sargan(IvProblem problem, IvFit fit)
    {
        int df = problem.L - problem.K;
        if (df == 0)
        {
            return null;
        }

        double[] purged = IvCore.Annihilate(fit.Residuals, 1, problem.Z, problem.L, problem.N);
        double statistic = problem.N * (1.0 - (Dot(purged, purged) / Dot(fit.Residuals, fit.Residuals)));
        return new IvTest(statistic, Distributions.ChiSquaredSf(statistic, df), df, null);
    }

    /// <summary>Hansen's <c>n·ḡᵀWḡ</c> at the second step's weight; <see langword="null"/> when just identified.</summary>
    private static IvTest? HansenJ(IvProblem problem, IvFit fit)
    {
        int df = problem.L - problem.K;
        if (df == 0)
        {
            return null;
        }

        int l = problem.L;
        var mean = new double[l];
        for (int row = 0; row < problem.N; row++)
        {
            for (int j = 0; j < l; j++)
            {
                mean[j] += problem.Z[(row * l) + j] * fit.Residuals[row];
            }
        }

        for (int j = 0; j < l; j++)
        {
            mean[j] /= problem.N;
        }

        double statistic = problem.N * Dot(mean, Dense.Multiply(fit.Weight!, mean, l, l, 1));
        return new IvTest(statistic, Distributions.ChiSquaredSf(statistic, df), df, null);
    }

    private static double TwoSided(double t, bool debiased, int residualDf) =>
        debiased
            ? Math.Min(1.0, 2.0 * Distributions.StudentSf(Math.Abs(t), residualDf))
            : Distributions.ChiSquaredSf(t * t, 1.0);

    /// <summary><c>bᵀC⁻¹b</c> over the selected coefficients.</summary>
    private static double Quadratic(double[] coefficients, double[] covariance, int k, int[] selected)
    {
        int m = selected.Length;
        var b = new double[m];
        var block = new double[m * m];
        for (int i = 0; i < m; i++)
        {
            b[i] = coefficients[selected[i]];
            for (int j = 0; j < m; j++)
            {
                block[(i * m) + j] = covariance[(selected[i] * k) + selected[j]];
            }
        }

        return Dot(b, Dense.Multiply(IvCore.Invert(block, m), b, m, m, 1));
    }

    /// <summary><c>1 − RSS/TSS</c>, the total sum of squares centred when the regressors hold a constant.</summary>
    private static double RSquared(double[] response, double[] residuals, bool centred)
    {
        double mean = 0.0;
        if (centred)
        {
            foreach (double value in response)
            {
                mean += value;
            }

            mean /= response.Length;
        }

        double total = 0.0;
        foreach (double value in response)
        {
            total += (value - mean) * (value - mean);
        }

        return 1.0 - (Dot(residuals, residuals) / total);
    }

    private static IvProblem Retarget(IvProblem problem, double[] response) =>
        new(problem.X, problem.K, problem.Z, problem.L, response, problem.N, problem.ExogenousCount);

    private static double[] Column(double[] block, int columns, int rows, int column)
    {
        var values = new double[rows];
        for (int row = 0; row < rows; row++)
        {
            values[row] = block[(row * columns) + column];
        }

        return values;
    }

    private static double[] Trailing(double[] block, int columns, int rows, int count)
    {
        var values = new double[rows * count];
        for (int row = 0; row < rows; row++)
        {
            Array.Copy(block, (row * columns) + columns - count, values, row * count, count);
        }

        return values;
    }

    private static double Dot(double[] a, double[] b)
    {
        double sum = 0.0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += a[i] * b[i];
        }

        return sum;
    }
}
