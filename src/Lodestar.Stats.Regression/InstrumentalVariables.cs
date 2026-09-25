using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>Instrumental-variables regression: two-stage least squares, LIML and two-step GMM, at <c>linearmodels</c> parity.</summary>
/// <remarks>
/// Each estimator takes the response and three row-major blocks — the exogenous regressors, the endogenous ones and the
/// excluded instruments — each with its column count, and returns the whole table <c>linearmodels</c>' <c>fit()</c>
/// prints: estimates, standard errors, tests, intervals, R², the model test, the first-stage diagnostics and the
/// overidentification test. The options' defaults are the reference's, a robust covariance among them. Decision 0004
/// has why this is written here rather than delegated.
/// </remarks>
public static class InstrumentalVariables
{
    private enum Estimator
    {
        TwoStageLeastSquares,
        Liml,
        Gmm,
    }

    /// <summary>Fits two-stage least squares, <c>IV2SLS(y, exog, endog, instruments).fit()</c>.</summary>
    /// <param name="design">The response, the exogenous and endogenous regressors and the excluded instruments.</param>
    /// <param name="options">The covariance, its kernel and scaling, and the intercept; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>The fitted model's table and diagnostics.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">A block's length is not its column count times the rows, the model is under-identified, no residual degree of freedom is left, the options ask for a cluster covariance or set a value this estimator does not read, or the regressors or instruments are collinear.</exception>
    public static IvSummary TwoStageLeastSquares(IvDesign design, IvOptions? options = null) =>
        Fit(Estimator.TwoStageLeastSquares, design, default, clustered: false, options ?? new IvOptions());

    /// <summary>Fits two-stage least squares whose rows fall in clusters.</summary>
    /// <param name="design">The response, the regressors and the instruments.</param>
    /// <param name="clusters">One cluster label per row; any integers, at least two distinct.</param>
    /// <param name="options">The options; <see cref="IvOptions.CovarianceType"/> must be <see cref="IvCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table and diagnostics.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">As the unclustered overload, or <paramref name="clusters"/> is of the wrong length or names one cluster, or the options do not ask for it.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static IvSummary TwoStageLeastSquares(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.TwoStageLeastSquares, design, clusters, clustered: true, options);
    }

    /// <summary>Fits limited-information maximum likelihood, <c>IVLIML(…, fuller=α).fit()</c>.</summary>
    /// <param name="design">The response, the regressors and the instruments.</param>
    /// <param name="options">The covariance, Fuller's <c>α</c> and the intercept; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>The fitted model's table and diagnostics, <see cref="IvSummary.Kappa"/> included.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">As <see cref="TwoStageLeastSquares(IvDesign, IvOptions?)"/>.</exception>
    /// <remarks><c>κ</c> is the smallest eigenvalue of the LIML problem, less <c>α/(n − L)</c> with Fuller's <c>α</c>; a just-identified model has <c>κ = 1</c> and fits 2SLS.</remarks>
    public static IvSummary Liml(IvDesign design, IvOptions? options = null) =>
        Fit(Estimator.Liml, design, default, clustered: false, options ?? new IvOptions());

    /// <summary>Fits LIML whose rows fall in clusters.</summary>
    /// <param name="design">The response, the regressors and the instruments.</param>
    /// <param name="clusters">One cluster label per row.</param>
    /// <param name="options">The options; <see cref="IvOptions.CovarianceType"/> must be <see cref="IvCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table and diagnostics.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">As <see cref="TwoStageLeastSquares(IvDesign, ReadOnlySpan{int}, IvOptions)"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static IvSummary Liml(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.Liml, design, clusters, clustered: true, options);
    }

    /// <summary>Fits two-step GMM, <c>IVGMM(…, weight_type=…).fit()</c>.</summary>
    /// <param name="design">The response, the regressors and the instruments.</param>
    /// <param name="options">The covariance, the second step's weight and the intercept; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>The fitted model's table and diagnostics, Hansen's J as <see cref="IvSummary.Overidentification"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">As <see cref="TwoStageLeastSquares(IvDesign, IvOptions?)"/>.</exception>
    /// <remarks>
    /// The first step weights by <c>(ZᵀZ/n)⁻¹</c>, the second by the inverse of <see cref="IvOptions.GmmWeightType"/>'s score
    /// covariance at the first step's residuals: <c>iter_limit=2</c>, the reference's default. A kernel weight or covariance
    /// without a bandwidth takes <c>n − 2</c>, as the reference's does for GMM.
    /// </remarks>
    public static IvSummary Gmm(IvDesign design, IvOptions? options = null) =>
        Fit(Estimator.Gmm, design, default, clustered: false, options ?? new IvOptions());

    /// <summary>Fits two-step GMM whose rows fall in clusters.</summary>
    /// <param name="design">The response, the regressors and the instruments.</param>
    /// <param name="clusters">One cluster label per row.</param>
    /// <param name="options">The options; the covariance or the weight must be <see cref="IvCovarianceType.Clustered"/>.</param>
    /// <returns>The fitted model's table and diagnostics.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A column count is out of range.</exception>
    /// <exception cref="ArgumentException">As <see cref="TwoStageLeastSquares(IvDesign, ReadOnlySpan{int}, IvOptions)"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static IvSummary Gmm(IvDesign design, ReadOnlySpan<int> clusters, IvOptions options)
    {
        Guard.NotNull(options);
        return Fit(Estimator.Gmm, design, clusters, clustered: true, options);
    }

    private static IvSummary Fit(
        Estimator estimator, IvDesign design, ReadOnlySpan<int> clusters, bool clustered, IvOptions options)
    {
        IvProblem problem = Build(design, options.WithIntercept);
        ClusterLabels? labels = CheckOptions(estimator, options, clusters, clustered, problem.N);
        var spec = new IvCovarianceSpec(options.CovarianceType, options.Debiased, options.Kernel, options.Bandwidth, labels);

        IvFit fit;
        if (estimator == Estimator.Gmm)
        {
            var weight = new IvCovarianceSpec(options.GmmWeightType, false, options.GmmWeightKernel, options.GmmWeightBandwidth, labels);
            fit = IvCore.Gmm(problem, weight, spec);
        }
        else
        {
            double kappa = estimator == Estimator.Liml ? IvCore.LimlKappa(problem) : 1.0;
            kappa -= options.Fuller / (problem.N - problem.L);
            fit = IvCore.KClass(problem, kappa, spec);
        }

        return IvDiagnostics.Summarise(problem, fit, spec, options.ConfidenceLevel, estimator == Estimator.Gmm, design.InstrumentCount);
    }

    private static IvProblem Build(IvDesign design, bool withIntercept)
    {
        ReadOnlySpan<double> response = design.Response;
        ReadOnlySpan<double> exogenous = design.Exogenous;
        ReadOnlySpan<double> endogenous = design.Endogenous;
        ReadOnlySpan<double> instruments = design.Instruments;
        int exogenousCount = design.ExogenousCount;
        int endogenousCount = design.EndogenousCount;
        int instrumentCount = design.InstrumentCount;
        RequireCount(exogenousCount, 0, nameof(design));
        RequireCount(endogenousCount, 1, nameof(design));
        RequireCount(instrumentCount, 1, nameof(design));
        int n = response.Length;
        RequireBlock(exogenous.Length, exogenousCount, n, "exogenous", nameof(design));
        RequireBlock(endogenous.Length, endogenousCount, n, "endogenous", nameof(design));
        RequireBlock(instruments.Length, instrumentCount, n, "instruments", nameof(design));
        if (instrumentCount < endogenousCount)
        {
            throw new ArgumentException(
                $"{instrumentCount} instruments for {endogenousCount} endogenous regressors under-identifies the model; "
                + "it needs at least as many.",
                nameof(design));
        }

        int shared = exogenousCount + (withIntercept ? 1 : 0);
        int k = shared + endogenousCount;
        int l = shared + instrumentCount;
        if (n - Math.Max(k, l) < 1)
        {
            throw new ArgumentException(
                $"{n} rows leave no residual degree of freedom for {k} regressors and {l} instruments.", nameof(design));
        }

        var x = new double[n * k];
        var z = new double[n * l];
        for (int row = 0; row < n; row++)
        {
            int at = 0;
            if (withIntercept)
            {
                x[row * k] = 1.0;
                z[row * l] = 1.0;
                at = 1;
            }

            for (int j = 0; j < exogenousCount; j++)
            {
                double value = exogenous[(row * exogenousCount) + j];
                x[(row * k) + at + j] = value;
                z[(row * l) + at + j] = value;
            }

            for (int j = 0; j < endogenousCount; j++)
            {
                x[(row * k) + shared + j] = endogenous[(row * endogenousCount) + j];
            }

            for (int j = 0; j < instrumentCount; j++)
            {
                z[(row * l) + shared + j] = instruments[(row * instrumentCount) + j];
            }
        }

        return new IvProblem(x, k, z, l, response.ToArray(), n, shared);
    }

    private static void RequireCount(int count, int floor, string parameterName)
    {
        if (count < floor)
        {
            throw new ArgumentOutOfRangeException(parameterName, count, $"A column count here is at least {floor}.");
        }
    }

    private static void RequireBlock(int length, int columns, int rows, string block, string parameterName)
    {
        if (length != (long)rows * columns)
        {
            throw new ArgumentException(
                $"The {block} block holds {length} values, which is not {columns} per row for {rows} rows.", parameterName);
        }
    }

    /// <summary>Refuses the options this estimator and overload do not read, and makes the cluster labels dense.</summary>
    private static ClusterLabels? CheckOptions(
        Estimator estimator, IvOptions options, ReadOnlySpan<int> clusters, bool clustered, int rowCount)
    {
        OptionGuards.ConfidenceLevel(options.ConfidenceLevel);
        RequireDefined(options.CovarianceType, nameof(options));
        RequireDefined(options.GmmWeightType, nameof(options));
        if (options.Kernel is < KernelType.Bartlett or > KernelType.QuadraticSpectral
            || options.GmmWeightKernel is < KernelType.Bartlett or > KernelType.QuadraticSpectral)
        {
            throw new ArgumentException("The kernel is not one of KernelType's.", nameof(options));
        }

        if (options.Bandwidth < 0 || options.GmmWeightBandwidth < 0)
        {
            throw new ArgumentException("A bandwidth is a lag count, zero or more.", nameof(options));
        }

        if (double.IsNaN(options.Fuller) || double.IsInfinity(options.Fuller)
            || (estimator != Estimator.Liml && Math.Abs(options.Fuller) > 0.0))
        {
            throw new ArgumentException(
                $"Fuller's α is LIML's, and finite; it was {options.Fuller} for {estimator}.", nameof(options));
        }

        RequireRead(estimator, options);
        bool wantsClusters = options.CovarianceType == IvCovarianceType.Clustered
            || (estimator == Estimator.Gmm && options.GmmWeightType == IvCovarianceType.Clustered);
        if (wantsClusters != clustered)
        {
            throw new ArgumentException(
                clustered
                    ? "Cluster labels were given, and neither the covariance nor the GMM weight is Clustered."
                    : "A Clustered covariance or weight needs the overload taking one cluster label per row.",
                nameof(options));
        }

        return clustered ? ClusterLabels.Dense(clusters, rowCount) : null;
    }

    /// <summary>Refuses a kernel setting without a kernel to read it, and a GMM weight setting outside GMM.</summary>
    private static void RequireRead(Estimator estimator, IvOptions options)
    {
        bool kernelCovariance = options.CovarianceType == IvCovarianceType.Kernel;
        if (!kernelCovariance && (options.Kernel != KernelType.Bartlett || options.Bandwidth.HasValue))
        {
            throw new ArgumentException(
                $"Kernel and Bandwidth are read by a Kernel covariance only, and the covariance is {options.CovarianceType}.",
                nameof(options));
        }

        bool weightSet = options.GmmWeightType != IvCovarianceType.Robust
            || options.GmmWeightKernel != KernelType.Bartlett
            || options.GmmWeightBandwidth.HasValue;
        if (estimator != Estimator.Gmm && weightSet)
        {
            throw new ArgumentException($"The GmmWeight options are read by GMM only, not by {estimator}.", nameof(options));
        }

        if (options.GmmWeightType != IvCovarianceType.Kernel
            && (options.GmmWeightKernel != KernelType.Bartlett || options.GmmWeightBandwidth.HasValue))
        {
            throw new ArgumentException(
                $"GmmWeightKernel and GmmWeightBandwidth are read by a Kernel weight only, and the weight is {options.GmmWeightType}.",
                nameof(options));
        }
    }

    private static void RequireDefined(IvCovarianceType type, string name)
    {
        if (type is < IvCovarianceType.Unadjusted or > IvCovarianceType.Clustered)
        {
            throw new ArgumentException($"{type} is not one of IvCovarianceType's.", name);
        }
    }
}
