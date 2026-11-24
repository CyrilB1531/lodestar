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
    public IvFit(double[] coefficients, double[] covariance, double[] residuals, int? bandwidth, int largestRank)
    {
        Coefficients = coefficients;
        Covariance = covariance;
        Residuals = residuals;
        Bandwidth = bandwidth;
        LargestRank = largestRank;
    }

    public double[] Coefficients { get; }

    public double[] Covariance { get; }

    public double[] Residuals { get; }

    /// <summary>The kernel covariance's bandwidth, given or chosen; <see langword="null"/> under any other.</summary>
    public int? Bandwidth { get; }

    public double? Kappa { get; init; }

    /// <summary>
    /// Whether every residual is exactly zero, which leaves every covariance read from them zero: a robust, kernel or
    /// clustered one is then returned as a zero block, whose Wald tests are undefined, and the unadjusted one's are +∞.
    /// </summary>
    public bool Vanishes { get; init; }

    /// <summary>GMM's final weight matrix, which its J statistic reads.</summary>
    public double[]? Weight { get; init; }

    /// <summary>
    /// The most rank the covariance can hold: the score groups that are not zero, clusters or rows, less one where the
    /// estimator zeroes their total. 2SLS and GMM do; a <c>κ</c> other than one leaves <c>X̂ᵀe = (κ − 1)XᵀM_Z e</c>.
    /// </summary>
    public int LargestRank { get; }
}

/// <summary>The covariance a fit reports, with everything a score estimator reads.</summary>
internal readonly record struct IvCovarianceSpec(
    IvCovarianceType Type, bool Debiased, KernelType Kernel, int? Bandwidth, ClusterLabels? Clusters);

/// <summary>The <c>k</c>-class estimators and two-step GMM, with <c>linearmodels</c>' covariances.</summary>
internal static class IvCore
{
    /// <summary>How every refusal of GMM's second-step weight opens.</summary>
    private const string WeightRefused = "The score covariance the second GMM step weights by is ";

    /// <summary>A <c>k</c>-class fit: 2SLS at <c>κ = 1</c>, LIML at its <c>κ</c>, least squares when <c>Z</c> is <c>X</c>.</summary>
    /// <exception cref="ArgumentException">A cross-product matrix is singular: the regressors or instruments are collinear; or a kernel's bandwidth is refused, past the sample's lags or automatic and 0/0, as on residuals exactly zero.</exception>
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
        // Residuals exactly zero leave a robust covariance zero, and its automatic bandwidth 0/0 (#1779, #1784); the
        // unadjusted one, s² times a fixed matrix, stays the homoskedastic covariance, whose tests read +∞ there.
        bool vanishes = ExactFit.Vanishes(residuals);
        if (spec.Type != IvCovarianceType.Unadjusted && vanishes)
        {
            // Zero for every reader, errors, t and Wald tests; a kernel still refuses a bandwidth past the sample's lags, and
            // an automatic one, 0/0 here.
            return ZeroCovariance(coefficients, residuals, spec, fallbackBandwidth: null, kappa, weight: null);
        }

        // The bread V: X'P_zX/n, moved towards X'X/n by 1 − κ for LIML.
        var bread = new double[k * k];
        // S1244: 2SLS and a just-identified LIML pass one exactly, and only one zeroes the scores' total.
#pragma warning disable S1244
        bool unitKappa = kappa == 1.0;
#pragma warning restore S1244
        for (int i = 0; i < bread.Length; i++)
        {
            bread[i] = unitKappa ? xhx[i] / n : (((1.0 - kappa) * xx[i]) + (kappa * xhx[i])) / n;
        }

        (double[] meat, int? bandwidth, int groups) = KClassMeat(problem, xhat, residuals, bread, spec);

        double[] inverseBread = Invert(bread, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(inverseBread, meat, k, k, k), inverseBread, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        int zeroedTotal = unitKappa ? 1 : 0;
        return new IvFit(coefficients, covariance, residuals, bandwidth, Math.Max(0, groups - zeroedTotal)) { Kappa = kappa, Vanishes = vanishes };
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
        // [y, X_endog] is one column wider than the regressors when no exogenous one is shared (#1614).
        var e = new double[TableLength.Of(n, width, IvScores.DesignParameter)];
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
    /// <param name="problem">The fit's arrays.</param>
    /// <param name="weightSpec">The second step's weight.</param>
    /// <param name="spec">The reported covariance.</param>
    /// <param name="clustersName">The public parameter a clustered weight with too few clusters is refused under.</param>
    /// <param name="designName">The public parameter the weight's other refusals are under, zero, singular or not finite; a kernel bandwidth's names none.</param>
    /// <exception cref="ArgumentException">A clustered weight has too few clusters, under <paramref name="clustersName"/>; or, under <paramref name="designName"/>, the weight is zero, on first-step residuals exactly zero or an unadjusted <c>s²</c> of zero, singular, or not finite; a normal matrix is singular; or a kernel's bandwidth runs past the sample's lags.</exception>
    public static IvFit Gmm(IvProblem problem, IvCovarianceSpec weightSpec, IvCovarianceSpec spec, string clustersName, string designName)
    {
        int n = problem.N;
        int l = problem.L;

        // 2SLS leaves Zᵀe = 0 just identified, so its score groups span one direction fewer, which rounding must not lift.
        int zeroedTotal = l == problem.K ? 1 : 0;

        double[] zz = Dense.Gram(problem.Z, l, n);
        Scale(zz, 1.0 / n);
        double[] first = GmmEstimate(problem, Invert(zz, l));

        // Too few clusters leave a clustered weight singular whatever the response, so they are named after the design's
        // collinearity refusals and before the residuals'; given enough, the scores answer for a capped rank (#1786).
        if (weightSpec.Type == IvCovarianceType.Clustered && weightSpec.Clusters!.Count - zeroedTotal < l)
        {
            throw new ArgumentException(
                WeightRefused + "singular: fewer clusters than instruments, every "
                + "column counted, exogenous ones and any constant, or as many in a just-identified model, whose first-step scores sum to zero, "
                + "leave it without an inverse.",
                clustersName);
        }

        double[] firstResiduals = Residuals(problem, first);

        // Residuals exactly zero leave every weight zero (#1779, #1784).
        if (ExactFit.Vanishes(firstResiduals))
        {
            throw new ArgumentException(
                WeightRefused + "zero: the first-step fit is exact, every residual "
                + "exactly zero, which leaves the weight without an inverse.",
                designName);
        }

        (double[] s, _, int weightGroups) = GmmScoreCovariance(problem, firstResiduals, weightSpec, designName);
        (int measured, double[]? inverse) = CovarianceRank.Inverse(s, l);
        int rank = Math.Min(measured, weightGroups - zeroedTotal);
        if (inverse is null && measured == l)
        {
            // Only a block that is not finite comes back of full rank without an inverse: the scores overflowed.
            throw new ArgumentException(
                WeightRefused + "not finite: the first-step scores overflow.",
                designName);
        }

        if (rank < l)
        {
            throw new ArgumentException(SingularWeight(weightSpec.Type), designName);
        }

        double[] weight = inverse!;
        double[] coefficients = GmmEstimate(problem, weight);
        double[] residuals = Residuals(problem, coefficients);

        int k = problem.K;
        bool vanishes = ExactFit.Vanishes(residuals);
        if (spec.Type != IvCovarianceType.Unadjusted && vanishes)
        {
            // As KClass: zero for every reader, a kernel taking n − 2 without a bandwidth, the reference's GMM default.
            return ZeroCovariance(coefficients, residuals, spec, fallbackBandwidth: GmmDefaultBandwidth(n), kappa: null, weight);
        }

        double[] xz = Dense.CrossProduct(problem.X, k, problem.Z, l, n);
        Scale(xz, 1.0 / n);
        double[] xzw = Dense.Multiply(xz, weight, k, l, l);
        double[] outer = Invert(Dense.Multiply(xzw, Dense.Transpose(xz, k, l), k, l, k), k);
        (double[] meat, int? bandwidth, int groups) = GmmScoreCovariance(problem, residuals, spec);

        double[] middle = Dense.Multiply(Dense.Multiply(xzw, meat, k, l, l), Dense.Transpose(xzw, k, l), k, l, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(outer, middle, k, k, k), outer, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        return new IvFit(coefficients, covariance, residuals, bandwidth, Math.Max(0, groups - 1)) { Weight = weight, Vanishes = vanishes };
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

    /// <summary>The filling, the bandwidth a kernel used, and the score groups that are not zero (<see cref="IvFit.LargestRank"/>).</summary>
    private static (double[] Meat, int? Bandwidth, int Groups) KClassMeat(
        IvProblem problem, double[] xhat, double[] residuals, double[] bread, IvCovarianceSpec spec)
    {
        int n = problem.N;
        int k = problem.K;
        double scale = Debiasing(spec, n, k);
        if (spec.Type == IvCovarianceType.Unadjusted)
        {
            double s2 = 0.0;
            foreach (double e in residuals)
            {
                s2 += e * e;
            }

            double[] meat = (double[])bread.Clone();
            Scale(meat, scale * s2 / n);
            return (meat, null, n);
        }

        double[] scores = Scores(xhat, k, residuals, n);
        if (spec.Type == IvCovarianceType.Robust)
        {
            (double[] meat, int rows) = IvScores.RobustSpanned(scores, k, n);
            Scale(meat, scale);
            return (meat, null, rows);
        }

        if (spec.Type == IvCovarianceType.Kernel)
        {
            int bandwidth = spec.Bandwidth ?? AutomaticBandwidth(xhat, scores, k, n, spec.Kernel);
            double[] meat = IvScores.Kernel(scores, k, n, IvScores.KernelWeights(spec.Kernel, bandwidth, n - 1));
            Scale(meat, scale);
            return (meat, bandwidth, n);
        }

        (double[] clustered, int spanning) = IvScores.ClusteredSpanned(scores, k, n, spec.Clusters!);
        if (spec.Debiased)
        {
            int g = spec.Clusters!.Count;
            Scale(clustered, scale * g / (g - 1.0) * (n - 1.0) / n);
        }

        return (clustered, null, spanning);
    }

    /// <summary>The kernel covariance's bandwidth when none is given: Newey and West on the scores' non-constant columns summed.</summary>
    private static int AutomaticBandwidth(double[] xhat, double[] scores, int k, int n, KernelType kernel)
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

    /// <summary>The robust, kernel or clustered score covariance GMM weights or reads its covariance through.</summary>
    /// <remarks>
    /// The unadjusted one is <see cref="UnadjustedScoreCovariance"/>'s, which its callers take first. A kernel without a
    /// bandwidth takes <c>n − 2</c>, the reference's <c>KernelWeightMatrix</c> default; a clustered one is scaled only
    /// when debiased, by <c>(n − 1)/(n − k)·G/(G − 1)</c>.
    /// </remarks>
    /// <param name="problem">The fit's arrays.</param>
    /// <param name="residuals">The residuals the scores are formed from.</param>
    /// <param name="spec">The covariance to estimate.</param>
    /// <param name="zeroWeightName">For the weight, the public parameter an unadjusted weight of zero is refused under; null for the reported covariance.</param>
    private static (double[] S, int? Bandwidth, int Groups) GmmScoreCovariance(
        IvProblem problem, double[] residuals, IvCovarianceSpec spec, string? zeroWeightName = null)
    {
        if (spec.Type == IvCovarianceType.Unadjusted)
        {
            return UnadjustedScoreCovariance(problem, residuals, spec, zeroWeightName);
        }

        int n = problem.N;
        int l = problem.L;
        int k = problem.K;
        double[] scores = Scores(problem.Z, l, residuals, n);
        double scale = Debiasing(spec, n, k);
        switch (spec.Type)
        {
            case IvCovarianceType.Robust:
                {
                    (double[] s, int rows) = IvScores.RobustSpanned(scores, l, n);
                    Scale(s, scale);
                    return (s, null, rows);
                }

            case IvCovarianceType.Kernel:
                {
                    int bandwidth = spec.Bandwidth ?? GmmDefaultBandwidth(n);
                    double[] s = IvScores.Kernel(scores, l, n, IvScores.KernelWeights(spec.Kernel, bandwidth, n - 1));
                    Scale(s, scale);
                    return (s, bandwidth, n);
                }

            case IvCovarianceType.Clustered:
                {
                    (double[] s, int spanning) = IvScores.ClusteredSpanned(scores, l, n, spec.Clusters!);
                    if (spec.Debiased)
                    {
                        int g = spec.Clusters!.Count;
                        Scale(s, (n - 1.0) / (n - k) * g / (g - 1.0));
                    }

                    return (s, null, spanning);
                }

            default:
                throw new InvalidOperationException($"No GMM score covariance is written for {spec.Type}.");
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

    /// <summary>
    /// The unadjusted score covariance, <c>s²·ZᵀZ/n</c>, the estimator centring the residuals, <c>s² = Σ(eᵢ − ē)²/n</c>;
    /// as the weight, refused under <paramref name="zeroWeightName"/> where that <c>s²</c> is exactly zero.
    /// </summary>
    /// <remarks>
    /// Zero as residuals all one value whose mean rounds back to it leave it, or residuals whose squares underflow, and no
    /// cluster is to blame (#1786); one rounding of <c>s²</c> is the formula's to invert, as linearmodels' is. An
    /// <c>s²</c> that is not finite, from residuals or squares that overflowed, is refused as the weight by
    /// <see cref="Gmm"/>'s test of a block that is not finite, and reads errors that are not finite as the reported one.
    /// </remarks>
    private static (double[] S, int? Bandwidth, int Groups) UnadjustedScoreCovariance(
        IvProblem problem, double[] residuals, IvCovarianceSpec spec, string? zeroWeightName)
    {
        int n = problem.N;
        double squares = CentredSquares(residuals);
        // Exactly zero, the one s² that leaves the weight without an inverse whatever ZᵀZ is.
        if (zeroWeightName is not null && ExactFit.IsZero(squares))
        {
            throw new ArgumentException(
                WeightRefused + "zero: the first-step residuals' squares about "
                + "their mean sum to zero, so the unadjusted weight's s² is zero and the weight has no inverse.",
                zeroWeightName);
        }

        double[] s = Dense.Gram(problem.Z, problem.L, n);
        Scale(s, Debiasing(spec, n, problem.K) * squares / n / n);
        return (s, null, n);
    }

    /// <summary>
    /// An exact fit under a robust, kernel or clustered covariance: zero for every reader. A kernel's bandwidth is refused
    /// without the kernel sum its zero scores would discard: past the sample's lags, and, automatic where no fallback is
    /// given, as 0/0 on scores all zero.
    /// </summary>
    private static IvFit ZeroCovariance(
        double[] coefficients, double[] residuals, IvCovarianceSpec spec, int? fallbackBandwidth, double? kappa, double[]? weight)
    {
        int? bandwidth = null;
        if (spec.Type == IvCovarianceType.Kernel)
        {
            int chosen = spec.Bandwidth ?? fallbackBandwidth ?? throw IvScores.UndefinedBandwidth();
            IvScores.RequireBandwidth(spec.Kernel, chosen, residuals.Length - 1);
            bandwidth = chosen;
        }

        int k = coefficients.Length;
        return new IvFit(coefficients, new double[k * k], residuals, bandwidth, 0) { Kappa = kappa, Weight = weight, Vanishes = true };
    }

    /// <summary>Why a weight with clusters enough, or of another type, is singular: an unadjusted one is built from no scores, its rank ZᵀZ's.</summary>
    private static string SingularWeight(IvCovarianceType type) => type == IvCovarianceType.Unadjusted
        ? WeightRefused + "singular: the unadjusted weight's s² times ZᵀZ/n "
            + "falls short of full rank, as an s² that underflows to a subnormal or instruments near collinear leave it."
        : WeightRefused + "singular: its first-step scores span fewer directions than the instrument columns, "
            + "exogenous ones and any constant counted, or underflow, or the instruments are near collinear, which leaves it "
            + "without an inverse.";

    /// <summary>The bandwidth a GMM kernel takes without one, <c>n − 2</c>, the reference's <c>KernelWeightMatrix</c> default.</summary>
    private static int GmmDefaultBandwidth(int n) => n - 2;

    /// <summary>The debiased covariances' small-sample factor, <c>n/(n − k)</c>, or one.</summary>
    private static double Debiasing(IvCovarianceSpec spec, int n, int k) => spec.Debiased ? n / (double)(n - k) : 1.0;

    /// <summary>
    /// <c>Σ(eᵢ − ē)²</c>, the unadjusted weight's <c>s²</c> times <c>n</c>: its own loop beside the R²'s total, which
    /// oracles at 1e-9 pin apart, so a change for either's sake cannot move the other.
    /// </summary>
    private static double CentredSquares(double[] residuals)
    {
        double mean = 0.0;
        foreach (double e in residuals)
        {
            mean += e;
        }

        mean /= residuals.Length;
        double total = 0.0;
        foreach (double e in residuals)
        {
            total += (e - mean) * (e - mean);
        }

        return total;
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
