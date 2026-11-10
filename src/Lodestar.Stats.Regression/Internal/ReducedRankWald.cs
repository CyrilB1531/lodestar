namespace Lodestar.Stats.Regression.Internal;

// long-comment: the target, the route to it and the refusal each need their reason.
// The target is the Moore-Penrose form of the covariance in exact arithmetic, which statsmodels' pinv computes on the
// formed block. Forming V = c·B·M·B lets a column at 1e5 leave rounding of 1e-6 where a real direction at 1e-10 lives
// (Review A pass 3 on #1223), and factoring M on its unit diagonal lifts a column that is only rounding, a cluster
// dummy's, to the scale of the others (pass 9). So the filling is read as FᵀF, F the score rows, and V = AAᵀ with
// A = √c·B·Fᵀ, reduced by reflections to the kept rows of its triangle; then bᵀV⁺b = ‖A⁺b‖², by reflections over
// rows sorted by size with Powell and Reid's pivoting, which Cox and Higham analyse for rows of very different scale.
// Where the residuals' rounding can move the form by more than Tolerance of itself, no digit of it is known in
// doubles, and the form is NaN.
/// <summary>The Wald form <c>bᵀV⁺b</c> of a robust covariance short of full rank, read from its score rows.</summary>
internal static class ReducedRankWald
{
    /// <summary>The first-order rounding bound, relative to the form, past which it is NaN.</summary>
    private const double Tolerance = 1e-6;

    /// <summary><c>bᵀV⁺b</c> over at most <paramref name="rank"/> directions, and how many it kept.</summary>
    /// <param name="sandwich">The covariance and its parts, over every coefficient.</param>
    /// <param name="tested">The tested coefficients.</param>
    /// <param name="offset">Coefficients left out ahead of them, one for an intercept.</param>
    /// <param name="rank">The tested block's rank, read on its unit diagonal and capped.</param>
    public static (double Form, int Rank) Form(SandwichParts sandwich, double[] tested, int offset, int rank)
    {
        int restrictions = tested.Length;
        int order = restrictions + offset;
        ScoreRows scores = sandwich.Scores();
        int count = scores.Count;

        // L = √c·B over the tested rows; Aᵀ = F·Lᵀ, column-major: one column of count values per restriction.
        double root = Math.Sqrt(sandwich.Factor);
        var lift = new double[restrictions * order];
        for (int i = 0; i < lift.Length; i++)
        {
            lift[i] = root * sandwich.Bread[(offset * order) + i];
        }

        var transposed = new double[count * restrictions];
        for (int i = 0; i < restrictions; i++)
        {
            for (int r = 0; r < count; r++)
            {
                double total = 0.0;
                for (int m = 0; m < order; m++)
                {
                    total += lift[(i * order) + m] * scores.Rows[(r * order) + m];
                }

                transposed[(i * count) + r] = total;
            }
        }

        int kept = Math.Min(rank, Math.Min(count, restrictions));
        if (kept < 1)
        {
            return (double.NaN, 0);
        }

        double[] a = Truncated(transposed, count, restrictions, kept);
        return Solve(a, tested, restrictions, kept, new Lifted(lift, scores, order)) is { } form ? (form, kept) : (double.NaN, kept);
    }

    /// <summary>
    /// <c>A ≈ [R₁₁ R₁₂]ᵀQ₁ᵀ</c>, the first <paramref name="kept"/> steps of <c>Aᵀ = QR</c>, as <c>[R₁₁ R₁₂]ᵀ</c>
    /// row-major in the restrictions' own order: each step reflects the column longest relative to its own start.
    /// </summary>
    /// <remarks>
    /// The columns not reached are what the rank cuts. Pivoting on the share left, not the length, picks the same ones
    /// whatever a coefficient's scale, where directions chosen by rotation mixed a small row with a large one's
    /// rounding: 6e-7 of the form on a Newey–West fit whose exact F is determined to 1e-15 (Review A pass 12).
    /// </remarks>
    private static double[] Truncated(double[] transposed, int rows, int columns, int kept)
    {
        var start = new double[columns];
        int[] order = [.. Enumerable.Range(0, columns)];
        for (int j = 0; j < columns; j++)
        {
            ReadOnlySpan<double> column = transposed.AsSpan(j * rows, rows);
            start[j] = Reflections.Dot(column, column);
        }

        var factor = new double[columns * kept];
        for (int k = 0; k < kept; k++)
        {
            SwapIn(transposed, rows, k, Longest(transposed, rows, columns, k, start, order), order);
            int diagonal = (k * rows) + k;
            Span<double> below = transposed.AsSpan(diagonal, rows - k);
            double norm = Math.Sqrt(Reflections.Dot(below, below));
            double alpha = transposed[diagonal] > 0.0 ? -norm : norm;
            transposed[diagonal] -= alpha;
            double scale = norm * (norm + Math.Abs(transposed[diagonal] + alpha));
            if (scale > 0.0)
            {
                for (int j = k + 1; j < columns; j++)
                {
                    Reflections.Reflect(below, scale, transposed.AsSpan((j * rows) + k, rows - k));
                }
            }

            // S4143: the write above is read through `below`, which aliases transposed[diagonal], before this one
            // replaces it.
#pragma warning disable S4143
            transposed[diagonal] = alpha;
#pragma warning restore S4143
            for (int j = k; j < columns; j++)
            {
                factor[(order[j] * kept) + k] = transposed[(j * rows) + k];
            }
        }

        return factor;
    }

    /// <summary>The column from <paramref name="k"/> on with the largest share of its start left below row <paramref name="k"/>.</summary>
    private static int Longest(double[] transposed, int rows, int columns, int k, double[] start, int[] order)
    {
        int longest = k;
        double share = -1.0;
        for (int j = k; j < columns; j++)
        {
            ReadOnlySpan<double> rest = transposed.AsSpan((j * rows) + k, rows - k);
            double left = start[order[j]] > 0.0 ? Reflections.Dot(rest, rest) / start[order[j]] : 0.0;
            if (left > share)
            {
                (longest, share) = (j, left);
            }
        }

        return longest;
    }

    private static void SwapIn(double[] transposed, int rows, int k, int longest, int[] order)
    {
        if (longest == k)
        {
            return;
        }

        for (int r = 0; r < rows; r++)
        {
            (transposed[(k * rows) + r], transposed[(longest * rows) + r]) = (transposed[(longest * rows) + r], transposed[(k * rows) + r]);
        }

        (order[k], order[longest]) = (order[longest], order[k]);
    }

    /// <summary><c>‖A⁺b‖²</c> by reflections over the rows sorted by size, or null where a pivot is exactly zero.</summary>
    private static double? Solve(double[] a, double[] b, int rows, int columns, Lifted lifted)
    {
        int[] sorted = [.. Enumerable.Range(0, rows)];
        var sizes = new double[rows];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                sizes[i] = Math.Max(sizes[i], Math.Abs(a[(i * columns) + j]));
            }
        }

        Array.Sort(sorted, (left, right) => sizes[right].CompareTo(sizes[left]));
        var columnMajor = new double[rows * columns];
        var projected = new double[rows];
        for (int r = 0; r < rows; r++)
        {
            projected[r] = b[sorted[r]];
            for (int j = 0; j < columns; j++)
            {
                columnMajor[(j * rows) + r] = a[(sorted[r] * columns) + j];
            }
        }

        // Sorting the rows alone lets a reflection mix a small row's value into a large one's (pass 12).
        var pivots = new int[columns];
        LeastSquares.Triangularize(columnMajor, rows, columns, projected, pivots: pivots);
        double[] upper = LeastSquares.Upper(columnMajor, rows, columns);
        a = Permuted(a, rows, columns, pivots);
        for (int i = 0; i < columns; i++)
        {
            // S1244: an exact zero pivot is what a rank-deficient A leaves; any other value is a pivot.
#pragma warning disable S1244
            if (upper[(i * columns) + i] == 0.0)
#pragma warning restore S1244
            {
                return null;
            }
        }

        double[] inverseUpper = LeastSquares.InvertUpper(upper, columns);
        double[] y = Dense.Multiply(inverseUpper, projected, columns, columns, 1);
        double form = OrdinaryLeastSquares.Dot(y, y);
        return Bound(a, b, y, inverseUpper, rows, columns, lifted) <= Tolerance * form ? form : null;
    }

    /// <summary>The columns of <paramref name="a"/>, row-major, in the order <paramref name="pivots"/> placed them.</summary>
    private static double[] Permuted(double[] a, int rows, int columns, int[] pivots)
    {
        var permuted = new double[a.Length];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                permuted[(i * columns) + j] = a[(i * columns) + pivots[j]];
            }
        }

        return permuted;
    }

    /// <summary>
    /// The first-order change of <c>bᵀV⁺b</c> under a rounding at <c>kε</c> of the residuals and of <c>A = L·Fᵀ</c>:
    /// <c>2kε Σᵣ(|Fᵣ·Lᵀz|·|Fᵣ|·|L|ᵀ|z| + |Fᵣ·LᵀV⁺z|·|Fᵣ|·|L|ᵀ|b⊥|)</c>, <c>z = V⁺b</c>, <c>b⊥</c> the part of
    /// <c>b</c> outside <c>V</c>'s range, <c>|Fᵣ|</c> row <c>r</c>'s <see cref="ScoreRows.Magnitudes"/> (pass 12).
    /// </summary>
    /// <remarks>
    /// The product's rounding counts: two rows of <c>A</c> parallel in exact arithmetic are parallel in doubles only to
    /// <c>ε</c> of themselves. Where a score row holds two columns of different scale, as a Newey–West window across
    /// them does, that error grows with the square of their ratio, and so does the bound (passes 12 and 13).
    /// </remarks>
    private static double Bound(double[] a, double[] b, double[] y, double[] inverseUpper, int rows, int columns, Lifted lifted)
    {
        (double[] lift, ScoreRows scores, int order) = lifted;
        // (AᵀA)⁻¹ = R⁻¹R⁻ᵀ, so V⁺ = A(AᵀA)⁻²Aᵀ is read through it without forming V.
        double[] z = Dense.Multiply(a, Gram(inverseUpper, y, columns), rows, columns, 1);
        double[] back = Dense.CrossProduct(a, columns, z, 1, rows);
        double[] pseudo = Dense.Multiply(a, Gram(inverseUpper, Gram(inverseUpper, back, columns), columns), rows, columns, 1);
        double[] fitted = Dense.Multiply(a, y, rows, columns, 1);
        var outside = new double[rows];
        for (int i = 0; i < rows; i++)
        {
            outside[i] = b[i] - fitted[i];
        }

        double[] alongZ = Dense.CrossProduct(lift, order, z, 1, rows);
        double[] alongPseudo = Dense.CrossProduct(lift, order, pseudo, 1, rows);
        double[] size = [.. lift.Select(Math.Abs)];
        double[] sizeZ = Dense.CrossProduct(size, order, [.. z.Select(Math.Abs)], 1, rows);
        double[] sizeOutside = Dense.CrossProduct(size, order, [.. outside.Select(Math.Abs)], 1, rows);
        double spread = 0.0;
        for (int r = 0; r < scores.Count; r++)
        {
            spread += (Math.Abs(Along(scores.Rows, r, alongZ, order)) * Along(scores.Magnitudes, r, sizeZ, order))
                + (Math.Abs(Along(scores.Rows, r, alongPseudo, order)) * Along(scores.Magnitudes, r, sizeOutside, order));
        }

        return 2.0 * RankBracket.Tolerance(order, order) * spread;
    }

    /// <summary>Row <paramref name="row"/> of <paramref name="table"/> dotted with <paramref name="direction"/>.</summary>
    private static double Along(double[] table, int row, double[] direction, int order)
    {
        double total = 0.0;
        for (int m = 0; m < order; m++)
        {
            total += table[(row * order) + m] * direction[m];
        }

        return total;
    }

    /// <summary><c>R⁻¹R⁻ᵀv</c>.</summary>
    private static double[] Gram(double[] inverseUpper, double[] vector, int order)
    {
        var inner = new double[order];
        for (int i = 0; i < order; i++)
        {
            double total = 0.0;
            for (int k = 0; k <= i; k++)
            {
                total += inverseUpper[(k * order) + i] * vector[k];
            }

            inner[i] = total;
        }

        return Dense.Multiply(inverseUpper, inner, order, order, 1);
    }

    /// <summary><c>L = √c·B</c> over the tested rows, the score rows it lifts, and the coefficients each row holds.</summary>
    private readonly record struct Lifted(double[] Lift, ScoreRows Scores, int Order);
}
