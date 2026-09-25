using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>One instrumental-variables problem: the regressors <c>X</c>, the instruments <c>Z</c> and the response, row-major.</summary>
/// <remarks>
/// <c>X = [exog, endog]</c> and <c>Z = [exog, instruments]</c>, the constant first in both when one was asked for, which is
/// the column order <c>linearmodels</c> builds and reports in.
/// </remarks>
internal sealed class IvProblem
{
    public IvProblem(double[] x, int k, double[] z, int l, double[] y, int n, int exogenousCount)
    {
        X = x;
        K = k;
        Z = z;
        L = l;
        Y = y;
        N = n;
        ExogenousCount = exogenousCount;
    }

    public double[] X { get; }

    public int K { get; }

    public double[] Z { get; }

    public int L { get; }

    public double[] Y { get; }

    public int N { get; }

    /// <summary>The leading columns <c>X</c> and <c>Z</c> share, the constant included.</summary>
    public int ExogenousCount { get; }
}

/// <summary>What a fit hands to the summary: the estimates, their covariance and the residuals.</summary>
internal sealed class IvFit
{
    public IvFit(double[] coefficients, double[] covariance, double[] residuals, int? bandwidth)
    {
        Coefficients = coefficients;
        Covariance = covariance;
        Residuals = residuals;
        Bandwidth = bandwidth;
    }

    public double[] Coefficients { get; }

    public double[] Covariance { get; }

    public double[] Residuals { get; }

    /// <summary>The kernel covariance's bandwidth, given or chosen; <see langword="null"/> under any other.</summary>
    public int? Bandwidth { get; }

    public double? Kappa { get; init; }

    /// <summary>GMM's final weight matrix, which its J statistic reads.</summary>
    public double[]? Weight { get; init; }
}

/// <summary>The covariance a fit reports, with everything a score estimator reads.</summary>
internal readonly record struct IvCovarianceSpec(
    IvCovarianceType Type, bool Debiased, IvKernel Kernel, int? Bandwidth, ClusterLabels? Clusters);

/// <summary>The <c>k</c>-class estimators and two-step GMM, with <c>linearmodels</c>' covariances.</summary>
internal static class IvCore
{
    /// <summary>A <c>k</c>-class fit: 2SLS at <c>κ = 1</c>, LIML at its <c>κ</c>, least squares when <c>Z</c> is <c>X</c>.</summary>
    /// <exception cref="ArgumentException">A cross-product matrix is singular: the regressors or instruments are collinear.</exception>
    public static IvFit KClass(IvProblem problem, double kappa, IvCovarianceSpec spec)
    {
        int n = problem.N;
        int k = problem.K;
        double[] xhat = Projected(problem);
        double[] xx = Dense.Gram(problem.X, k, n);
        double[] xhx = Dense.CrossProduct(problem.X, k, xhat, k, n);
        double[] xy = Dense.CrossProduct(problem.X, k, problem.Y, 1, n);
        double[] xhy = Dense.CrossProduct(xhat, k, problem.Y, 1, n);

        var left = new double[k * k];
        var right = new double[k];
        for (int i = 0; i < left.Length; i++)
        {
            left[i] = (xx[i] * (1.0 - kappa)) + (kappa * xhx[i]);
        }

        for (int i = 0; i < k; i++)
        {
            right[i] = (xy[i] * (1.0 - kappa)) + (kappa * xhy[i]);
        }

        double[] coefficients = Dense.Multiply(Invert(left, k), right, k, k, 1);
        double[] residuals = Residuals(problem, coefficients);

        // The bread V: X'P_zX/n, moved towards X'X/n by 1 − κ for LIML.
        var bread = new double[k * k];
        bool unitKappa = Math.Abs(kappa - 1.0) < double.Epsilon;
        for (int i = 0; i < bread.Length; i++)
        {
            bread[i] = unitKappa ? xhx[i] / n : (((1.0 - kappa) * xx[i]) + (kappa * xhx[i])) / n;
        }

        (double[] meat, int? bandwidth) = KClassMeat(problem, xhat, residuals, bread, spec);
        double[] inverseBread = Invert(bread, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(inverseBread, meat, k, k, k), inverseBread, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        return new IvFit(coefficients, covariance, residuals, bandwidth) { Kappa = kappa };
    }

    /// <summary>LIML's <c>κ</c>: the smallest eigenvalue of <c>E₁ᵀE₁</c> against <c>E_zᵀE_z</c>, <c>E = [y, X_endog]</c>.</summary>
    /// <remarks>
    /// <c>E₁</c> is <c>E</c> purged of the exogenous regressors and <c>E_z</c> of the instruments. The reference forms
    /// <c>(E_zᵀE_z)^−½ E₁ᵀE₁ (E_zᵀE_z)^−½</c>; a Cholesky factor <c>L</c> of <c>E_zᵀE_z</c> gives <c>L⁻¹E₁ᵀE₁L⁻ᵀ</c>,
    /// similar to it and symmetric positive semi-definite, so its smallest singular value is the same eigenvalue.
    /// </remarks>
    public static double LimlKappa(IvProblem problem)
    {
        int n = problem.N;
        int endogenous = problem.K - problem.ExogenousCount;
        int width = endogenous + 1;
        var e = new double[n * width];
        for (int row = 0; row < n; row++)
        {
            e[row * width] = problem.Y[row];
            Array.Copy(problem.X, (row * problem.K) + problem.ExogenousCount, e, (row * width) + 1, endogenous);
        }

        double[] ez = Annihilate(e, width, problem.Z, problem.L, n);
        double[] ex = problem.ExogenousCount == 0
            ? e
            : Annihilate(e, width, Columns(problem.X, problem.K, n, problem.ExogenousCount), problem.ExogenousCount, n);
        double[] a = Dense.Gram(ex, width, n);
        double[] b = Dense.Gram(ez, width, n);
        if (!Cholesky.TryFactor(b, width, out double[] lower))
        {
            throw new ArgumentException("The instruments leave the endogenous block singular, so κ is undefined.");
        }

        // C = L⁻¹ A L⁻ᵀ, one triangular solve per side.
        double[] half = (double[])a.Clone();
        for (int column = 0; column < width; column++)
        {
            Cholesky.ForwardSubstitute(lower, width, half, width, column);
        }

        double[] c = Dense.Transpose(half, width, width);
        for (int column = 0; column < width; column++)
        {
            Cholesky.ForwardSubstitute(lower, width, c, width, column);
        }

        double[] singular = JacobiSpectrum.SingularValues(Dense.Transpose(c, width, width), width, width);
        return singular[width - 1];
    }

    /// <summary>Two-step GMM: 2SLS weights first, then the inverse of the chosen score covariance.</summary>
    public static IvFit Gmm(IvProblem problem, IvCovarianceSpec weightSpec, IvCovarianceSpec spec)
    {
        int n = problem.N;
        int l = problem.L;
        double[] zz = Dense.Gram(problem.Z, l, n);
        Scale(zz, 1.0 / n);
        double[] first = GmmEstimate(problem, Invert(zz, l));
        double[] firstResiduals = Residuals(problem, first);

        double[] weightScores = Scores(problem.Z, l, firstResiduals, n);
        (double[] s, _) = GmmScoreCovariance(problem, weightScores, firstResiduals, weightSpec);
        double[] weight = Invert(s, l);
        double[] coefficients = GmmEstimate(problem, weight);
        double[] residuals = Residuals(problem, coefficients);

        int k = problem.K;
        double[] xz = Dense.CrossProduct(problem.X, k, problem.Z, l, n);
        Scale(xz, 1.0 / n);
        double[] xzw = Dense.Multiply(xz, weight, k, l, l);
        double[] outer = Invert(Dense.Multiply(xzw, Dense.Transpose(xz, k, l), k, l, k), k);
        double[] scores = Scores(problem.Z, l, residuals, n);
        (double[] meat, int? bandwidth) = GmmScoreCovariance(problem, scores, residuals, spec);
        double[] middle = Dense.Multiply(Dense.Multiply(xzw, meat, k, l, l), Dense.Transpose(xzw, k, l), k, l, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(outer, middle, k, k, k), outer, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        return new IvFit(coefficients, covariance, residuals, bandwidth) { Weight = weight };
    }

    /// <summary><c>P_zX</c>, row by row, through <c>(ZᵀZ)⁻¹ZᵀX</c>.</summary>
    public static double[] Projected(IvProblem problem)
    {
        int n = problem.N;
        double[] zz = Dense.Gram(problem.Z, problem.L, n);
        double[] zx = Dense.CrossProduct(problem.Z, problem.L, problem.X, problem.K, n);
        double[] pi = Dense.Multiply(Invert(zz, problem.L), zx, problem.L, problem.L, problem.K);
        return Dense.Multiply(problem.Z, pi, n, problem.L, problem.K);
    }

    /// <summary>A block purged of its projection on another: <c>M_B A</c>.</summary>
    public static double[] Annihilate(double[] a, int aColumns, double[] b, int bColumns, int rows)
    {
        double[] bb = Dense.Gram(b, bColumns, rows);
        double[] ba = Dense.CrossProduct(left: b, leftColumns: bColumns, right: a, rightColumns: aColumns, rows);
        double[] fitted = Dense.Multiply(b, Dense.Multiply(Invert(bb, bColumns), ba, bColumns, bColumns, aColumns), rows, bColumns, aColumns);
        var residual = new double[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            residual[i] = a[i] - fitted[i];
        }

        return residual;
    }

    /// <summary>The leading <paramref name="count"/> columns of a row-major block.</summary>
    public static double[] Columns(double[] block, int columns, int rows, int count)
    {
        var leading = new double[rows * count];
        for (int row = 0; row < rows; row++)
        {
            Array.Copy(block, row * columns, leading, row * count, count);
        }

        return leading;
    }

    /// <summary>The inverse, or the refusal a collinear design earns.</summary>
    public static double[] Invert(double[] matrix, int order)
    {
        if (!Dense.TryInvert(matrix, order, out double[] inverse))
        {
            throw new ArgumentException(
                "A cross-product matrix is singular: the regressors or the instruments are collinear.");
        }

        return inverse;
    }

    private static (double[] Meat, int? Bandwidth) KClassMeat(
        IvProblem problem, double[] xhat, double[] residuals, double[] bread, IvCovarianceSpec spec)
    {
        int n = problem.N;
        int k = problem.K;
        double scale = spec.Debiased ? n / (double)(n - k) : 1.0;
        if (spec.Type == IvCovarianceType.Unadjusted)
        {
            double s2 = 0.0;
            foreach (double e in residuals)
            {
                s2 += e * e;
            }

            double[] meat = (double[])bread.Clone();
            Scale(meat, scale * s2 / n);
            return (meat, null);
        }

        double[] scores = Scores(xhat, k, residuals, n);
        if (spec.Type == IvCovarianceType.Robust)
        {
            double[] meat = IvScores.Robust(scores, k, n);
            Scale(meat, scale);
            return (meat, null);
        }

        if (spec.Type == IvCovarianceType.Kernel)
        {
            int bandwidth = spec.Bandwidth ?? AutomaticBandwidth(xhat, scores, k, n, spec.Kernel);
            double[] meat = IvScores.Kernel(scores, k, n, IvScores.KernelWeights(spec.Kernel, bandwidth, n - 1));
            Scale(meat, scale);
            return (meat, bandwidth);
        }

        double[] clustered = IvScores.Clustered(scores, k, n, spec.Clusters!);
        if (spec.Debiased)
        {
            int g = spec.Clusters!.Count;
            Scale(clustered, scale * g / (g - 1.0) * (n - 1.0) / n);
        }

        return (clustered, null);
    }

    /// <summary>The kernel covariance's bandwidth when none is given: Newey and West on the scores' non-constant columns summed.</summary>
    private static int AutomaticBandwidth(double[] xhat, double[] scores, int k, int n, IvKernel kernel)
    {
        (bool found, int constant) = IvScores.FindConstant(xhat, k, n);
        int skip = found && k > 1 ? constant : -1;
        var summed = new double[n];
        for (int row = 0; row < n; row++)
        {
            double sum = 0.0;
            for (int j = 0; j < k; j++)
            {
                sum += j == skip ? 0.0 : scores[(row * k) + j];
            }

            summed[row] = sum;
        }

        return IvScores.OptimalBandwidth(summed, kernel);
    }

    /// <summary>The score covariance GMM weights or reads its covariance through, with that caller's scaling.</summary>
    /// <remarks>
    /// The unadjusted estimator centres the residuals, <c>s² = Σ(eᵢ − ē)²/n</c>; a kernel one without a bandwidth takes
    /// <c>n − 2</c>, the reference's <c>KernelWeightMatrix</c> default; a clustered one is scaled only when debiased,
    /// by <c>(n − 1)/(n − k)·G/(G − 1)</c>.
    /// </remarks>
    private static (double[] S, int? Bandwidth) GmmScoreCovariance(
        IvProblem problem, double[] scores, double[] residuals, IvCovarianceSpec spec)
    {
        int n = problem.N;
        int l = problem.L;
        int k = problem.K;
        double scale = spec.Debiased ? n / (double)(n - k) : 1.0;
        switch (spec.Type)
        {
            case IvCovarianceType.Unadjusted:
                {
                    double mean = 0.0;
                    foreach (double e in residuals)
                    {
                        mean += e;
                    }

                    mean /= n;
                    double s2 = 0.0;
                    foreach (double e in residuals)
                    {
                        s2 += (e - mean) * (e - mean);
                    }

                    double[] s = Dense.Gram(problem.Z, l, n);
                    Scale(s, scale * s2 / n / n);
                    return (s, null);
                }

            case IvCovarianceType.Robust:
                {
                    double[] s = IvScores.Robust(scores, l, n);
                    Scale(s, scale);
                    return (s, null);
                }

            case IvCovarianceType.Kernel:
                {
                    int bandwidth = spec.Bandwidth ?? n - 2;
                    double[] s = IvScores.Kernel(scores, l, n, IvScores.KernelWeights(spec.Kernel, bandwidth, n - 1));
                    Scale(s, scale);
                    return (s, bandwidth);
                }

            default:
                {
                    double[] s = IvScores.Clustered(scores, l, n, spec.Clusters!);
                    if (spec.Debiased)
                    {
                        int g = spec.Clusters!.Count;
                        Scale(s, (n - 1.0) / (n - k) * g / (g - 1.0));
                    }

                    return (s, null);
                }
        }
    }

    private static double[] GmmEstimate(IvProblem problem, double[] weight)
    {
        int n = problem.N;
        int k = problem.K;
        int l = problem.L;
        double[] xz = Dense.CrossProduct(problem.X, k, problem.Z, l, n);
        double[] zy = Dense.CrossProduct(problem.Z, l, problem.Y, 1, n);
        double[] xzw = Dense.Multiply(xz, weight, k, l, l);
        double[] left = Dense.Multiply(xzw, Dense.Transpose(xz, k, l), k, l, k);
        double[] right = Dense.Multiply(xzw, zy, k, l, 1);
        return Dense.Multiply(Invert(left, k), right, k, k, 1);
    }

    private static double[] Residuals(IvProblem problem, double[] coefficients)
    {
        int k = problem.K;
        var residuals = new double[problem.N];
        for (int row = 0; row < problem.N; row++)
        {
            double fitted = 0.0;
            for (int j = 0; j < k; j++)
            {
                fitted += problem.X[(row * k) + j] * coefficients[j];
            }

            residuals[row] = problem.Y[row] - fitted;
        }

        return residuals;
    }

    private static double[] Scores(double[] block, int columns, double[] residuals, int rows)
    {
        var scores = new double[rows * columns];
        for (int row = 0; row < rows; row++)
        {
            for (int j = 0; j < columns; j++)
            {
                scores[(row * columns) + j] = block[(row * columns) + j] * residuals[row];
            }
        }

        return scores;
    }

    private static void Scale(double[] matrix, double factor)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] *= factor;
        }
    }
}
