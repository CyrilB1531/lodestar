using Lodestar.Abstractions;
using Lodestar.Decomposition.Internal;

namespace Lodestar.Decomposition;

/// <summary>A fitted non-negative matrix factorization, <c>X ≈ W H</c>.</summary>
/// <remarks>
/// There is no unfitted state, and <see cref="Transform"/> is a factorization rather than a
/// projection: applying an unseen row to a non-negative basis runs the same multiplicative loop
/// with H held fixed, not the product a name borrowed from the SVD would suggest.
/// </remarks>
public sealed class Nmf
{
    private readonly double[] _weights;
    private readonly double[] _components;

    // long-comment: which settings are kept and which are not is the whole reason Transform
    // needs no options of its own, and a reader will ask why the list is only three long.
    // The three the reference replays in transform(), and no more. Initialization, Seed and
    // RandomMatrix decide where a *fit* starts; a transform's W is the mean fill below, so
    // keeping them would be keeping state nothing reads (#1124).
    private readonly NmfBetaLoss _betaLoss;
    private readonly int _maxIterations;
    private readonly double _tolerance;

    private Nmf(int featureCount, int componentCount, double[] weights, double[] components,
                int iterations, double reconstructionError, NmfOptions settings)
    {
        FeatureCount = featureCount;
        ComponentCount = componentCount;
        _weights = weights;
        _components = components;
        Iterations = iterations;
        ReconstructionError = reconstructionError;
        _betaLoss = settings.BetaLoss;
        _maxIterations = settings.MaxIterations;
        _tolerance = settings.Tolerance;
    }

    /// <summary>How many components were asked for.</summary>
    public int ComponentCount { get; }

    /// <summary>How many columns the factorized matrix had.</summary>
    public int FeatureCount { get; }

    /// <summary>How many multiplicative updates ran — scikit-learn's <c>n_iter_</c>.</summary>
    public int Iterations { get; }

    /// <summary>The beta divergence at the end, square-rooted — scikit-learn's <c>reconstruction_err_</c>.</summary>
    public double ReconstructionError { get; }

    /// <summary><c>W</c>, row-major rows × <see cref="ComponentCount"/>: each row's mix of components.</summary>
    public IReadOnlyList<double> Weights => _weights;

    /// <summary><c>H</c>, row-major <see cref="ComponentCount"/> × <see cref="FeatureCount"/> — scikit-learn's <c>components_</c>.</summary>
    public IReadOnlyList<double> Components => _components;

    /// <summary>Factorizes <paramref name="matrix"/>, initialising it with the NNDSVD family.</summary>
    /// <param name="matrix">The non-negative matrix to factorize, rows as samples and columns as features.</param>
    /// <param name="componentCount">How many components to keep.</param>
    /// <param name="options">The solver's settings, or null for scikit-learn's defaults.</param>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="componentCount"/> is not in <c>[1, min(matrix.RowCount, matrix.ColumnCount)]</c>, or an option is out of range.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> holds a negative value, a NaN or an infinity, or <see cref="NmfOptions.RandomMatrix"/> is not <c>matrix.ColumnCount × (componentCount + 10)</c>.</exception>
    public static Nmf Fit(CsrMatrix matrix, int componentCount, NmfOptions? options = null)
    {
        Guard.NotNull(matrix);
        NmfOptions settings = options ?? new NmfOptions();
        // Before the rank and the initialisation, so a bad option is refused without paying for NNDSVD.
        Validate(settings);
        int maximumRank = Math.Min(matrix.RowCount, matrix.ColumnCount);
        if (componentCount < 1 || componentCount > maximumRank)
        {
            // Past min(rows, columns) a rank survives the range finder and breaks the truncation
            // once the economic factorization narrows the block: a stack trace, not an answer.
            throw new ArgumentOutOfRangeException(
                nameof(componentCount), componentCount,
                $"A factorization keeps between 1 and {maximumRank} components, which is " +
                "scikit-learn's min(n_samples, n_features); above that, only " +
                "Fit(matrix, initialWeights, initialComponents) is defined, reading the rank " +
                "off the blocks it is handed.");
        }

        int size = componentCount + NndSvd.Oversampling;
        if (settings.RandomMatrix is { } omega && omega.Length != (long)matrix.ColumnCount * size)
        {
            throw new ArgumentException(
                $"Ω is {omega.Length} long, not {matrix.ColumnCount} × {size}.", nameof(options));
        }

        RequireNonNegativeMatrix(matrix);
        (double[] w, double[] h) = NndSvd.Initialize(
            matrix, componentCount, settings.Initialization, settings.Seed, settings.RandomMatrix);
        return Fit(matrix, w, h, settings);
    }

    /// <summary>Factorizes <paramref name="matrix"/> from an initialisation you supply.</summary>
    /// <param name="matrix">The non-negative matrix to factorize.</param>
    /// <param name="initialWeights">W₀, row-major <c>matrix.RowCount × componentCount</c> and non-negative.</param>
    /// <param name="initialComponents">H₀, row-major <c>componentCount × matrix.ColumnCount</c> and non-negative.</param>
    /// <param name="options">The solver's settings, or null for scikit-learn's defaults.</param>
    /// <exception cref="ArgumentNullException">An argument is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> has no row or no column or holds a negative value, a NaN or an infinity, or the two blocks do not agree on a component count, do not fit the matrix, or hold a negative number, a NaN or an infinity.</exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is out of range.</exception>
    public static Nmf Fit(
        CsrMatrix matrix, double[] initialWeights, double[] initialComponents,
        NmfOptions? options = null)
    {
        Guard.NotNull(matrix);
        Guard.NotNull(initialWeights);
        Guard.NotNull(initialComponents);
        NmfOptions settings = options ?? new NmfOptions();
        Validate(settings);

        if (matrix.RowCount == 0 || matrix.ColumnCount == 0)
        {
            // W's length is divided by the row count below, and scikit-learn refuses both shapes.
            throw new ArgumentException(
                $"A factorization needs at least one row and one column; this matrix is {matrix.RowCount} × {matrix.ColumnCount}.",
                nameof(matrix));
        }

        int features = matrix.ColumnCount;
        int componentCount = ComponentCountOf(matrix, initialWeights, initialComponents, features);
        RequireNonNegativeMatrix(matrix);
        RequireNonNegative(initialWeights, nameof(initialWeights));
        RequireNonNegative(initialComponents, nameof(initialComponents));

        double[] w = (double[])initialWeights.Clone();

        // Column-major for the loop, as the updates read it, and row-major again for the caller.
        double[] h = DenseBlock.Transpose(initialComponents, componentCount, features);
        var workspace = new MultiplicativeUpdates.Workspace(matrix, componentCount, settings.BetaLoss);

        double initial = BetaDivergence.Compute(matrix, w, h, componentCount, settings.BetaLoss);
        double previous = initial;
        int iteration = 0;
        while (iteration < settings.MaxIterations)
        {
            iteration++;
            MultiplicativeUpdates.UpdateWeights(matrix, w, h, componentCount, settings.BetaLoss, workspace);
            MultiplicativeUpdates.UpdateComponents(matrix, w, h, componentCount, settings.BetaLoss, workspace);

            // scikit-learn checks every tenth iteration, never on the others: checking more
            // often would stop earlier, on the same data, for no reason a caller can see.
            if (settings.Tolerance > 0 && iteration % 10 == 0)
            {
                double error = BetaDivergence.Compute(
                    matrix, w, h, componentCount, settings.BetaLoss);
                if ((previous - error) / initial < settings.Tolerance)
                {
                    break;
                }
                previous = error;
            }
        }

        double final = BetaDivergence.Compute(matrix, w, h, componentCount, settings.BetaLoss);
        return new Nmf(
            features, componentCount, w, DenseBlock.Transpose(h, features, componentCount),
            iteration, final, settings);
    }

    /// <summary>W for an unseen matrix, with this fit's H held fixed.</summary>
    /// <param name="matrix">The matrix to factorize against this fit; it must have <see cref="FeatureCount"/> columns.</param>
    /// <returns>W, row-major <c>matrix.RowCount × <see cref="ComponentCount"/></c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> has no row, does not have <see cref="FeatureCount"/> columns, or holds a negative value, a NaN or an infinity.</exception>
    /// <remarks>
    /// The same multiplicative update the fit ran, with H untouched, from the loss, the cap and
    /// the tolerance that fit was given. It is a factorization and not a projection, which is why
    /// it iterates where <c>TruncatedSvd.Transform</c> multiplies.
    /// </remarks>
    public double[] Transform(CsrMatrix matrix)
    {
        Guard.NotNull(matrix);
        if (matrix.ColumnCount != FeatureCount)
        {
            throw new ArgumentException(
                $"This fit has {FeatureCount} features; the matrix has {matrix.ColumnCount}.",
                nameof(matrix));
        }
        if (matrix.RowCount == 0)
        {
            // The reference refuses it too: "Found array with 0 sample(s) ... a minimum of 1".
            throw new ArgumentException("A transform needs at least one row.", nameof(matrix));
        }

        RequireNonNegativeMatrix(matrix);

        int rows = matrix.RowCount;
        double[] h = DenseBlock.Transpose(_components, ComponentCount, FeatureCount);
        var w = new double[(long)rows * ComponentCount];

        // long-comment: this fill is the reason a transform can be frozen at all, and a reader
        // who replaces it with a random start would break the corpus without breaking a test.
        // The reference's own initialisation when H is held fixed and the solver is the
        // multiplicative one: the mean over *every* cell, structural zeros included, over the
        // rank, square-rooted. No random_state is involved anywhere in it (#1124, measured
        // against scikit-learn 1.9.1 to 1.7e-16).
        w.AsSpan().Fill(Math.Sqrt(Mean(matrix) / ComponentCount));

        var workspace = new MultiplicativeUpdates.Workspace(matrix, ComponentCount, _betaLoss);
        double initial = BetaDivergence.Compute(matrix, w, h, ComponentCount, _betaLoss);
        double previous = initial;
        for (int iteration = 1; iteration <= _maxIterations; iteration++)
        {
            MultiplicativeUpdates.UpdateWeights(matrix, w, h, ComponentCount, _betaLoss, workspace);

            // H never moves here, so only the tenth-iteration check the fit makes is left.
            if (_tolerance > 0 && iteration % 10 == 0)
            {
                double error = BetaDivergence.Compute(matrix, w, h, ComponentCount, _betaLoss);
                if ((previous - error) / initial < _tolerance)
                {
                    break;
                }
                previous = error;
            }
        }

        return w;
    }

    /// <summary>The mean over every cell, which is what <c>X.mean()</c> is on a sparse matrix.</summary>
    private static double Mean(CsrMatrix matrix)
    {
        double sum = 0.0;
        double[] values = matrix.Values;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum / ((double)matrix.RowCount * matrix.ColumnCount);
    }

    /// <summary>The two settings the loop itself cannot survive, refused before it starts.</summary>
    /// <remarks>
    /// The parameter is named <c>options</c> rather than <c>settings</c> so that every
    /// <see cref="ArgumentException.ParamName"/> it produces names a parameter of the
    /// overload the caller wrote, which is the only signature they can read.
    /// </remarks>
    private static void Validate(NmfOptions options)
    {
        if (options.MaxIterations < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.MaxIterations, "MaxIterations is at least one.");
        }
        if (options.Tolerance < 0 || double.IsNaN(options.Tolerance))
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.Tolerance, "Tolerance is not negative.");
        }
    }

    private static int ComponentCountOf(
        CsrMatrix matrix, double[] initialWeights, double[] initialComponents, int features)
    {
        if (initialWeights.Length % matrix.RowCount != 0)
        {
            throw new ArgumentException(
                $"W is {initialWeights.Length} long, which is not a multiple of {matrix.RowCount} rows.",
                nameof(initialWeights));
        }
        int componentCount = initialWeights.Length / matrix.RowCount;
        if (componentCount < 1 || initialComponents.Length != (long)componentCount * features)
        {
            throw new ArgumentException(
                $"W implies {componentCount} components, so H must be {componentCount} × {features}; " +
                $"it is {initialComponents.Length} long.",
                nameof(initialComponents));
        }
        return componentCount;
    }

    private static void RequireNonNegative(double[] block, string name)
    {
        int index = FirstNegative(block);
        if (index >= 0)
        {
            throw new ArgumentException(
                $"A non-negative factorization cannot start from {block[index]}; W₀ and H₀ are finite and non-negative.", name);
        }
    }

    private static void RequireNonNegativeMatrix(CsrMatrix matrix)
    {
        int index = FirstNegative(matrix.Values);
        if (index >= 0)
        {
            throw new ArgumentException(
                $"A non-negative factorization needs a finite, non-negative matrix, and this one holds {matrix.Values[index]}.",
                nameof(matrix));
        }
    }

    /// <summary>Where the first value a non-negative factorization cannot use sits, or -1.</summary>
    /// <remarks>
    /// Infinity included: the updates turn it into NaN rather than refusing it. scikit-learn
    /// refuses it in the matrix and runs W₀ and H₀ holding it to NaN.
    /// </remarks>
    private static int FirstNegative(double[] block) =>
        Array.FindIndex(block, value => value < 0 || double.IsNaN(value) || double.IsInfinity(value));
}
