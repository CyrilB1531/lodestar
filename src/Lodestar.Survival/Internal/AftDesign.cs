namespace Lodestar.Survival.Internal;

/// <summary>The two blocks of an AFT design, lifelines' <c>Xs</c>: the covariates of each parameter, the intercept last.</summary>
internal sealed class AftDesign
{
    public const int Intercept = -1;

    private AftDesign(int featureCount, bool fitIntercept, bool ancillary)
    {
        FeatureCount = featureCount;
        int[] covariates = [.. Enumerable.Range(0, featureCount)];
        PrimaryColumns = fitIntercept ? [.. covariates, Intercept] : covariates;
        AncillaryColumns = ancillary ? PrimaryColumns : [Intercept];
    }

    public int FeatureCount { get; }

    /// <summary>The design column of each primary coefficient, <see cref="Intercept"/> for the intercept.</summary>
    public int[] PrimaryColumns { get; }

    /// <summary>The design column of each ancillary coefficient.</summary>
    public int[] AncillaryColumns { get; }

    public int Size => PrimaryColumns.Length + AncillaryColumns.Length;

    /// <summary>The blocks <paramref name="options"/> asks for over <paramref name="featureCount"/> covariates.</summary>
    /// <param name="featureCount">The covariates each subject carries.</param>
    /// <param name="options">Whether to fit an intercept, and whether the ancillary parameter takes the covariates.</param>
    /// <param name="paramName">The public parameter a parameter count too large for one array is blamed on.</param>
    /// <exception cref="ArgumentException">The parameters are more than one array holds.</exception>
    public static AftDesign For(int featureCount, AftOptions options, string paramName)
    {
        // Where the column indices are built and the scales sized next: a count past one array, or past int wrapping
        // Size, failed there; the Hessian's square is refused later, after the univariate fit, as before (#1614).
        long primary = featureCount + (options.FitIntercept ? 1L : 0L);
        long parameters = primary + (options.Ancillary ? primary : 1L);
        if (parameters > TableLength.MaxLength)
        {
            throw new ArgumentException($"{parameters} parameters are more than one array holds.", paramName);
        }

        return new(featureCount, options.FitIntercept, options.Ancillary);
    }

    /// <summary>The design as an array, checked against the subjects and the blocks it has to fill.</summary>
    public static double[] Validate(ReadOnlySpan<double> design, int rows, int featureCount, bool fitIntercept)
    {
        if (featureCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(featureCount), featureCount, "A design has zero or more covariates.");
        }

        if (featureCount == 0 && !fitIntercept)
        {
            throw new ArgumentException(
                "With no covariate and no intercept the primary parameter has no column to be modelled by.", nameof(featureCount));
        }

        if (design.Length != (long)rows * featureCount)
        {
            throw new ArgumentException(
                $"The design holds {design.Length} values for {rows} subjects of {featureCount} covariates.", nameof(design));
        }

        foreach (double value in design)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException($"A covariate is finite; the design holds {value}.", nameof(design));
            }
        }

        // Where the copy is made: a span over native memory can outgrow one array (#1614).
        _ = TableLength.Of(rows, featureCount, nameof(design));
        return design.ToArray();
    }

    /// <summary>A row's value in design column <paramref name="column"/>, one for the intercept.</summary>
    public double Value(ReadOnlySpan<double> design, int row, int column) =>
        column == Intercept ? 1.0 : design[(row * FeatureCount) + column];

    /// <summary>
    /// Both linear predictors of <paramref name="row"/> as the two variables of a jet: the likelihood is differentiated
    /// in them, and <see cref="Spread"/> carries the result to every coefficient.
    /// </summary>
    public (Jet Primary, Jet Ancillary) Predictors(ReadOnlySpan<double> design, int row, double[] theta)
    {
        (double primary, double ancillary) = Scores(design, row, theta);
        return (Jet.Variable(primary, 0, 2), Jet.Variable(ancillary, 1, 2));
    }

    /// <summary>
    /// Adds <c>weight</c> times a term differentiated in the two predictors to the gradient and Hessian over the
    /// coefficients: <c>∂/∂β = Σ ∂/∂η · x</c>, each predictor linear in its own block.
    /// </summary>
    public void Spread(ReadOnlySpan<double> design, int row, Jet term, double weight, double[] gradient, double[] hessian)
    {
        int k = Size;
        int p = PrimaryColumns.Length;
        Span<double> u = k <= 64 ? stackalloc double[k] : new double[k];
        for (int j = 0; j < p; j++)
        {
            u[j] = Value(design, row, PrimaryColumns[j]);
        }

        for (int j = p; j < k; j++)
        {
            u[j] = Value(design, row, AncillaryColumns[j - p]);
        }

        double g0 = weight * term.Gradient[0];
        double g1 = weight * term.Gradient[1];
        double h00 = weight * term.Hessian[0];
        double h01 = weight * term.Hessian[1];
        double h11 = weight * term.Hessian[3];
        for (int a = 0; a < k; a++)
        {
            bool primaryA = a < p;
            gradient[a] += (primaryA ? g0 : g1) * u[a];
            double within = primaryA ? h00 : h11;
            for (int b = 0; b < k; b++)
            {
                double h = primaryA == (b < p) ? within : h01;
                hessian[(a * k) + b] += h * u[a] * u[b];
            }
        }
    }

    /// <summary>Both linear predictors of <paramref name="row"/> at <paramref name="theta"/>, as numbers.</summary>
    public (double Primary, double Ancillary) Scores(ReadOnlySpan<double> design, int row, IReadOnlyList<double> theta)
    {
        double primary = 0.0;
        double ancillary = 0.0;
        for (int j = 0; j < PrimaryColumns.Length; j++)
        {
            primary += theta[j] * Value(design, row, PrimaryColumns[j]);
        }

        for (int j = 0; j < AncillaryColumns.Length; j++)
        {
            ancillary += theta[PrimaryColumns.Length + j] * Value(design, row, AncillaryColumns[j]);
        }

        return (primary, ancillary);
    }

    /// <summary>
    /// Each coefficient's column deviation, pandas' <c>std</c>, one for a constant column; and which coefficients
    /// lifelines leaves unpenalised: a constant column, the intercept among them, in a block of more than one.
    /// </summary>
    public (double[] Deviations, bool[] Unpenalised) Scales(ReadOnlySpan<double> design, int rows)
    {
        var deviations = new double[Size];
        var unpenalised = new bool[Size];
        for (int j = 0; j < Size; j++)
        {
            bool primary = j < PrimaryColumns.Length;
            int column = primary ? PrimaryColumns[j] : AncillaryColumns[j - PrimaryColumns.Length];
            double deviation = column == Intercept ? 0.0 : Deviation(design, rows, column);
            bool constant = deviation < 1e-8;
            deviations[j] = constant ? 1.0 : deviation;
            unpenalised[j] = constant && (primary ? PrimaryColumns.Length : AncillaryColumns.Length) > 1;
        }

        return (deviations, unpenalised);
    }

    private double Deviation(ReadOnlySpan<double> design, int rows, int column)
    {
        if (rows < 2)
        {
            return 0.0;
        }

        double sum = 0.0;
        for (int i = 0; i < rows; i++)
        {
            sum += design[(i * FeatureCount) + column];
        }

        double mean = sum / rows;
        double squares = 0.0;
        for (int i = 0; i < rows; i++)
        {
            double gap = design[(i * FeatureCount) + column] - mean;
            squares += gap * gap;
        }

        return Math.Sqrt(squares / (rows - 1));
    }
}
