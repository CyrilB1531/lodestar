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

    /// <summary>Each coefficient's column mean, and the index of its block's intercept, −1 for an intercept itself or a
    /// block without one.</summary>
    public (double[] Means, int[] Intercepts) Centres(double[] columnMeans)
    {
        var means = new double[Size];
        var intercepts = new int[Size];
        for (int j = 0; j < Size; j++)
        {
            bool primary = j < PrimaryColumns.Length;
            int[] block = primary ? PrimaryColumns : AncillaryColumns;
            int offset = primary ? 0 : PrimaryColumns.Length;
            int column = block[j - offset];
            int intercept = Array.IndexOf(block, Intercept);
            intercepts[j] = column == Intercept || intercept < 0 ? -1 : offset + intercept;
            means[j] = intercepts[j] >= 0 ? columnMeans[column] : 0.0;
        }

        return (means, intercepts);
    }

    /// <summary>Each design column's mean, which the deviations and the centring both read.</summary>
    public double[] ColumnMeans(ReadOnlySpan<double> design, int rows)
    {
        var means = new double[FeatureCount];
        for (int column = 0; column < FeatureCount; column++)
        {
            double sum = 0.0;
            for (int i = 0; i < rows; i++)
            {
                sum += design[(i * FeatureCount) + column];
            }
            means[column] = sum / rows;
        }

        return means;
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
    /// Adds <c>weight</c> times a term differentiated in the two predictors to the gradient and the Hessian's upper triangle
    /// over the coefficients, the Hessian skipped where null: <c>∂/∂β = Σ ∂/∂η · x</c>, each predictor linear in its own block.
    /// </summary>
    public void Spread(ReadOnlySpan<double> design, int row, Jet term, double weight, double[] gradient, double[]? hessian)
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
        for (int a = 0; a < k; a++)
        {
            gradient[a] += (a < p ? g0 : g1) * u[a];
        }

        if (hessian is null)
        {
            return;
        }

        // The upper triangle alone, each block's run apart: Mirror fills the rest once an evaluation, where every cell and
        // a branch per cell took a 300-covariate fit past lifelines' time (Review A, pass 45).
        double h00 = weight * term.Hessian[0];
        double h01 = weight * term.Hessian[1];
        double h11 = weight * term.Hessian[3];
        for (int a = 0; a < k; a++)
        {
            int offset = a * k;
            double ua = u[a];
            if (a < p)
            {
                AddRun(hessian.AsSpan(offset + a, p - a), h00 * ua, u.Slice(a, p - a));
                AddRun(hessian.AsSpan(offset + p, k - p), h01 * ua, u.Slice(p));
            }
            else
            {
                AddRun(hessian.AsSpan(offset + a, k - a), h11 * ua, u.Slice(a));
            }
        }
    }

    private static void AddRun(Span<double> target, double factor, ReadOnlySpan<double> u)
    {
        for (int b = 0; b < target.Length; b++)
        {
            target[b] += factor * u[b];
        }
    }

    /// <summary>Copies the upper triangle <see cref="Spread"/> accumulates onto the lower one.</summary>
    public static void Mirror(double[] hessian, int k)
    {
        for (int a = 0; a < k; a++)
        {
            for (int b = a + 1; b < k; b++)
            {
                hessian[(b * k) + a] = hessian[(a * k) + b];
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
    public (double[] Deviations, bool[] Unpenalised) Scales(ReadOnlySpan<double> design, int rows, double[] columnMeans)
    {
        var deviations = new double[Size];
        var unpenalised = new bool[Size];
        for (int j = 0; j < Size; j++)
        {
            bool primary = j < PrimaryColumns.Length;
            int column = primary ? PrimaryColumns[j] : AncillaryColumns[j - PrimaryColumns.Length];
            double deviation = column == Intercept ? 0.0 : Deviation(design, rows, column, columnMeans[column]);
            bool constant = deviation < 1e-8;
            deviations[j] = constant ? 1.0 : deviation;
            unpenalised[j] = constant && (primary ? PrimaryColumns.Length : AncillaryColumns.Length) > 1;
        }

        return (deviations, unpenalised);
    }

    private double Deviation(ReadOnlySpan<double> design, int rows, int column, double mean)
    {
        if (rows < 2)
        {
            return 0.0;
        }

        double squares = 0.0;
        for (int i = 0; i < rows; i++)
        {
            double gap = design[(i * FeatureCount) + column] - mean;
            squares += gap * gap;
        }

        return Math.Sqrt(squares / (rows - 1));
    }
}
