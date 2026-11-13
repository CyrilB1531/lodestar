namespace Lodestar.Stats.Regression.Internal;

/// <summary>Whether every residual of a fit is zero within the rounding of computing it, so the fit is exact.</summary>
/// <remarks>
/// <c>rᵢ = yᵢ − Σⱼ xᵢⱼbⱼ</c> summed in doubles over <c>k</c> products carries up to about <c>(k + 1)ε</c> of its terms, and
/// the coefficients' rounding reaches every row through the regressors' span; <see cref="Holds"/> removes that part and
/// holds what is left to each row's own bound, with what the span can carry in from the others (#1779, Review A passes
/// 9 to 19).
/// </remarks>
internal static class ExactFit
{
    /// <summary>Machine epsilon for <see cref="double"/>, which <see cref="double.Epsilon"/> is not.</summary>
    private const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary>How many times <c>(k + 1)ε</c> a residual may reach and still be zero.</summary>
    private const double RoundingMultiple = 1.0;

    /// <summary>How far past the screen's bound the raw residual, the span's part kept, may lie before it rejects.</summary>
    private const double RawMultiple = 1048576.0;

    /// <summary>
    /// <see cref="RawMultiple"/> for an OLS, WLS or GLS residual, solved by a QR or by normal equations held to a scaled
    /// condition number of 200: the exact corpora of Review A pass 17 reached 0.74 of the screen's bound, and relative
    /// noise of 1e-10 at 2,000 × 8 reaches 431, where it paid the reflections.
    /// </summary>
    private const double StableRawMultiple = 256.0;

    /// <summary>
    /// How many times <c>κ·(k + 1)ε·M</c> a normal-equations fit's raw residual may reach: its coefficients' forward error,
    /// <c>κ·ε</c>, carried up to 3.1 times <c>κ·ε·M</c> into the exact near-collinear 2SLS fits of Review A pass 16.
    /// </summary>
    private const double ConditionMultiple = 4.0;

    /// <summary><see cref="RoundingMultiple"/> times <c>(k + 1)ε</c>: what a sum of <c>k</c> products may carry, relative to its terms' sizes.</summary>
    public static double Share(int columns) => RoundingMultiple * (columns + 1) * MachineEpsilon;

    /// <summary>Whether every residual, its part in the regressors' span removed, lies within its row's rounding.</summary>
    /// <param name="matrix">The rows the residuals were computed from, row-major, every fitted column included.</param>
    /// <param name="columns">Columns of <paramref name="matrix"/>, the length of <paramref name="coefficients"/>.</param>
    /// <param name="residuals">One residual per row.</param>
    /// <param name="coefficients">The estimate the residuals were computed with.</param>
    /// <param name="conditioning">The condition number of the matrix an IV or panel solve inverted for the coefficients, zero for an OLS, WLS or GLS fit.</param>
    /// <param name="transform">For a transformed fit, its untransformed rows, which bound its floors, and each row's floor.</param>
    /// <remarks>
    /// On an exact fit <c>r = −X·δb + ρ</c>, <c>δb</c> in the span, <c>|ρⱼ| ≤ (k + 1)ε·mⱼ</c>: each row of <c>ρ − Hρ</c>
    /// is held to <c>(k + 1)ε·(mᵢ + Σⱼ|Hᵢⱼ|mⱼ)</c>, exact only where proven within it, the test's rounding counted against
    /// it. With <c>M = max|r| + max|x|·Σ|bⱼ|</c>, an OLS, WLS or GLS residual past 256 times <c>(k + 1)ε·(1 + √n)·M</c>
    /// rejects before anything is built, as relative noise of 1e-10 at 2,000 × 8 does; an IV or panel one past 2²⁰ times
    /// it plus its coefficients' forward error. Then <c>√n·M</c> screens each row, and <see cref="WithinPairs"/> the rest.
    /// </remarks>
    public static bool Holds(
        double[] matrix,
        int columns,
        double[] residuals,
        double[] coefficients,
        double conditioning = 0.0,
        Transform? transform = null)
    {
        int n = residuals.Length;
        double share = Share(columns);
        double weight = 0.0;
        foreach (double coefficient in coefficients)
        {
            weight += Math.Abs(coefficient);
        }

        // Every mⱼ, a floor included, lies under this; a NaN residual fails every test below.
        double raw = LargestMagnitude.Of(residuals);
        double largestSize = raw + (LargestMagnitude.Of(matrix) * weight);
        if (transform is { } source)
        {
            largestSize = Math.Max(largestSize, LargestMagnitude.Of(source.Y) + (LargestMagnitude.Of(source.X) * weight));
        }

        // The raw residual holds X·δb too: an OLS, WLS or GLS fit's rejects past 256 times the screen's bound, an IV or
        // panel fit's past 2²⁰ times it plus what κ·ε of its coefficients carries in (Review A passes 11 to 17).
        double carriedIn = ConditionMultiple * conditioning * share * largestSize;
        double multiple = conditioning > 0.0 ? RawMultiple : StableRawMultiple;
        if (!(raw <= (multiple * share * (1.0 + Math.Sqrt(n)) * largestSize) + carriedIn))
        {
            return false;
        }

        // The span is read from the rows' own reflections past the raw reject only: (XᵀX)⁻¹ squared the condition
        // number into δ and threw on a near-collinear design the fit had solved (Review A pass 16).
        var span = new SpanBasis(matrix, n, columns);
        if (!span.FullRank
            || Screened(matrix, columns, residuals, coefficients, span, largestSize, transform is not null) is not { } screened)
        {
            // A column with nothing past the others, or a span the rounding turns by half, leaves no test: main's answer.
            return false;
        }

        (double[] projected, double[] sizes, double[] errors) = screened;

        if (transform?.Floors() is { } floors)
        {
            for (int row = 0; row < n; row++)
            {
                sizes[row] = Math.Max(sizes[row], floors[row]);
            }
        }

        return WithinPairs(span, projected, sizes, errors, share);
    }

    /// <summary>
    /// Each residual less its span part, each row's terms, each projected residual's rounding; null at the first row
    /// past <c>(k + 1)ε</c> of its terms (or of <paramref name="largestSize"/> if transformed) plus <c>√n</c> of that size.
    /// </summary>
    /// <remarks>
    /// The span's part, <c>X·δ</c> with <c>δ = R⁻¹Qᵀr</c>, comes off row by row before the reflections project what is
    /// left: a normal-equations fit's residual holds <c>κ·ε</c> of its coefficients in the span, whose rounding, read
    /// off the whole residual, widened the rule fourfold (Review A pass 17). Compensated, the subtraction errs by
    /// <c>eᵢ ≤ 2ε|rᵢ′| + (2(k + 1)ε)²·(|rᵢ| + Σⱼ|xᵢⱼδⱼ|)</c>, which projected carries at most <c>eᵢ + ‖e‖₂</c>.
    /// </remarks>
    private static (double[] Projected, double[] Sizes, double[] Errors)? Screened(
        double[] matrix, int columns, double[] residuals, double[] coefficients, SpanBasis span, double largestSize, bool transformed)
    {
        int n = residuals.Length;
        double share = Share(columns);
        double reach = share * Math.Sqrt(n) * largestSize;
        double floor = transformed ? largestSize : 0.0;
        double[] delta = span.Coefficients(residuals);
        var remainder = new double[n];
        var errors = new double[n];
        for (int row = 0; row < n; row++)
        {
            int at = row * columns;
            double value = residuals[row];
            double terms = Math.Abs(value);
            double compensation = 0.0;
            for (int j = 0; j < columns; j++)
            {
                // Ogita, Rump and Oishi's Dot2: each product's and each sum's rounding is carried, not lost.
                double part = matrix[at + j] * delta[j];
                double sum = value - part;
                double back = sum - value;
                compensation += ((value - (sum - back)) - (part + back)) - ExactProduct.ProductError(part, matrix[at + j], delta[j]);
                value = sum;
                terms += Math.Abs(part);
            }

            remainder[row] = value + compensation;
            double gamma = 2.0 * share;
            errors[row] = (2.0 * MachineEpsilon * Math.Abs(remainder[row])) + (gamma * gamma * terms);
        }

        double[] projected = span.Apply(remainder, keepSpan: false);
        double carried = Norm(errors) + (span.Rounding * Norm(remainder));
        var sizes = new double[n];
        for (int row = 0; row < n; row++)
        {
            sizes[row] = Magnitude(matrix, residuals[row], coefficients, row * columns);
            errors[row] += carried;

            // Not negated into a > test: a NaN residual, from an overflow, is no exact fit.
            if (!(Math.Abs(projected[row]) + errors[row] <= (share * Math.Max(sizes[row], floor)) + reach))
            {
                return null;
            }
        }

        return (projected, sizes, errors);
    }

    /// <summary>Whether every row lies within <c>(k + 1)ε·(mᵢ + Σⱼ|Hᵢⱼ|mⱼ)</c>, all its pairs read only between two bounds.</summary>
    /// <param name="span">The rows' reflections, <c>H = QQᵀ</c>.</param>
    /// <param name="projected">Each residual less its part in the span.</param>
    /// <param name="sizes">Each row's terms or floor, <c>mᵢ</c>.</param>
    /// <param name="errors">Each projected residual's rounding.</param>
    /// <param name="share"><c>(k + 1)ε</c>.</param>
    /// <remarks>
    /// <c>|Σⱼ Hᵢⱼmⱼ| ≤ Σⱼ|Hᵢⱼ|mⱼ</c>, and <c>H·m = Q·Qᵀm</c> costs <c>O(n·k)</c>: a row within that lower bound, less its
    /// rounding, passes, almost every row of an exact fit. The others take <see cref="Undecided"/>'s bounds, and only a
    /// row between those reads all its pairs (Review A pass 12: an upper bound taken for the accept passed noise twice the
    /// rule's).
    /// </remarks>
    private static bool WithinPairs(SpanBasis span, double[] projected, double[] sizes, double[] errors, double share)
    {
        int n = projected.Length;
        double[] spread = span.Apply(sizes, keepSpan: true);
        double slack = span.Rounding * Norm(sizes);
        var undecided = new List<int>();
        for (int row = 0; row < n; row++)
        {
            if (!((Math.Abs(projected[row]) + errors[row]) / share <= sizes[row] + Math.Abs(spread[row]) - slack))
            {
                undecided.Add(row);
            }
        }

        return undecided.Count == 0 || Undecided(span, projected, sizes, errors, slack, share, undecided);
    }

    /// <summary>Whether each row <see cref="WithinPairs"/> left open lies within the rule, read between two bounds.</summary>
    /// <remarks>
    /// The rows that each carry a sixteenth of <c>T = Σⱼ√hⱼⱼ·mⱼ</c>, sixteen at most, are read pair by pair; with
    /// <c>|Σⱼ Hᵢⱼmⱼ|</c> over the others they bound <c>Σⱼ|Hᵢⱼ|mⱼ</c> from below. <c>H</c> being a projection,
    /// <c>|Hᵢⱼ| ≤ √(hᵢᵢhⱼⱼ)</c> and <c>Σⱼ Hᵢⱼ² = hᵢᵢ</c> bound the others from above by <c>√hᵢᵢ</c> times the smaller of
    /// their <c>Σ√hⱼⱼ·mⱼ</c> and <c>‖m‖₂</c>, so a row past that fails, and a row between reads all its pairs. Each
    /// bound moves by <paramref name="slack"/>, the rounding of <c>Q</c>'s rows, against the side it decides.
    /// </remarks>
    private static bool Undecided(
        SpanBasis span, double[] projected, double[] sizes, double[] errors, double slack, double share, List<int> rows)
    {
        int n = projected.Length;
        int columns = span.Columns;
        double[] basis = span.Basis();
        var roots = new double[n];
        double total = 0.0;
        for (int row = 0; row < n; row++)
        {
            roots[row] = Math.Sqrt(Math.Min(1.0, Leverage(basis, row * columns, columns) + span.Rounding));
            total += roots[row] * sizes[row];
        }

        var heavy = new List<int>();
        var light = new double[columns];
        double spread = 0.0;
        double norm = 0.0;
        for (int row = 0; row < n; row++)
        {
            if (roots[row] * sizes[row] > total / HeavyShare)
            {
                heavy.Add(row);
                continue;
            }

            spread += roots[row] * sizes[row];
            norm += sizes[row] * sizes[row];
            for (int j = 0; j < columns; j++)
            {
                light[j] += basis[(row * columns) + j] * sizes[row];
            }
        }

        int[] heavyRows = [.. heavy];
        double rest = Math.Min(spread, Math.Sqrt(norm));
        foreach (int row in rows)
        {
            int at = row * columns;
            double residual = Math.Abs(projected[row]) / share;
            double error = errors[row] / share;
            double heavyPart = Carried(basis, sizes, at, columns, heavyRows);
            double carried = 0.0;
            for (int j = 0; j < columns; j++)
            {
                carried += basis[at + j] * light[j];
            }

            if (residual + error <= sizes[row] + heavyPart + Math.Abs(carried) - (2.0 * slack))
            {
                continue;
            }

            // Not negated into > tests: a NaN residual is no exact fit. Exact only where proven within the rule.
            if (!(residual + error <= sizes[row] + heavyPart + (roots[row] * rest) + slack)
                || !(residual + error <= sizes[row] + Carried(basis, sizes, at, columns, null) - slack))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>The share of <c>Σⱼ√hⱼⱼ·mⱼ</c> past which one row's pairs are read apart: at most this many rows.</summary>
    private const double HeavyShare = 16.0;

    /// <summary><c>hᵢᵢ = ‖qᵢ‖²</c>, <c>qᵢ</c> row <c>i</c> of <c>Q</c>.</summary>
    private static double Leverage(double[] basis, int at, int columns)
    {
        double hat = 0.0;
        for (int j = 0; j < columns; j++)
        {
            hat += basis[at + j] * basis[at + j];
        }

        return hat;
    }

    /// <summary><c>Σⱼ|Hᵢⱼ|mⱼ</c> over <paramref name="rows"/>, or every row where null, with <c>Hᵢⱼ = qᵢ·qⱼ</c>.</summary>
    private static double Carried(double[] basis, double[] sizes, int at, int columns, int[]? rows)
    {
        double total = 0.0;
        int count = rows?.Length ?? sizes.Length;
        for (int i = 0; i < count; i++)
        {
            int other = rows is null ? i : rows[i];
            int from = other * columns;
            double hat = 0.0;
            for (int j = 0; j < columns; j++)
            {
                hat += basis[at + j] * basis[from + j];
            }

            total += Math.Abs(hat) * sizes[other];
        }

        return total;
    }

    /// <summary><c>‖v‖₂</c>, scaled by its largest entry so no square overflows.</summary>
    private static double Norm(double[] values)
    {
        double largest = LargestMagnitude.Of(values);
        if (!(largest > 0.0) || double.IsInfinity(largest))
        {
            return largest;
        }

        double total = 0.0;
        foreach (double value in values)
        {
            double scaled = value / largest;
            total += scaled * scaled;
        }

        return largest * Math.Sqrt(total);
    }

    /// long-comment: the bound's derivation, which the test's soundness rests on.
    /// <summary>The Householder reflections of a block of rows, each column scaled to unit size, and what they project.</summary>
    /// <remarks>
    /// Scaling a column leaves its span, so <c>H = QQᵀ</c> is the rows' own and no square overflows. The computed
    /// <c>Q̂</c> lies within <c>√k·γ</c> of an orthonormal <c>Q̃</c> spanning <c>X + ΔX</c>, <c>‖ΔX‖₂ ≤ γ‖R‖_F</c>, with
    /// <c>γ = 4nkε</c> Higham's <c>γ̃ₙₖ</c> (<i>Accuracy and Stability</i>, §19.3, Theorem 19.4) taken with its unstated
    /// constant <c>c</c> as 8; so <c>‖Q̂Q̂ᵀ − Q̃Q̃ᵀ‖₂ ≤ 2√k·γ + kγ²</c>, and by Wedin's theorem <c>‖Q̃Q̃ᵀ − H‖₂ ≤ η/(1 − η)</c>,
    /// <c>η = ‖ΔX‖₂‖R⁻¹‖₂ ≤ γκ</c>, <c>κ = ‖R‖_F‖R⁻¹‖_F ≥ k</c>. Their sum lies under <c>γ(κ + 2√k)/(1 − γκ)</c>, each
    /// read's error per unit of its argument's norm (Review A passes 18, 19); from <c>γκ = ½</c> the test refuses, main's
    /// answer. Through <c>(XᵀX)⁻¹</c> it carried <c>κ²</c>, and from <c>κ ≈ 1e9</c> kept main's answer on 17 of 30 exact
    /// fits (pass 16). A column with nothing left past the earlier ones leaves it short of full rank.
    /// </remarks>
    private sealed class SpanBasis
    {
        private readonly double[] _vectors;
        private readonly double[] _scales;
        private readonly double[] _diagonal;
        private readonly double[] _columnScales;
        private readonly int _rows;

        internal SpanBasis(double[] matrix, int rows, int columns)
        {
            _rows = rows;
            Columns = columns;
            _vectors = new double[rows * columns];
            _scales = new double[columns];
            _diagonal = new double[columns];
            _columnScales = new double[columns];
            for (int j = 0; j < columns; j++)
            {
                double largest = 0.0;
                for (int i = 0; i < rows; i++)
                {
                    largest = Math.Max(largest, Math.Abs(matrix[(i * columns) + j]));
                }

                double scale = largest > 0.0 ? 1.0 / largest : 0.0;
                _columnScales[j] = scale;
                for (int i = 0; i < rows; i++)
                {
                    _vectors[(j * rows) + i] = matrix[(i * columns) + j] * scale;
                }
            }

            int reach = Math.Min(rows, columns);
            for (int j = 0; j < reach; j++)
            {
                Reflect(j);
            }

            double gamma = 4.0 * rows * columns * MachineEpsilon;
            double condition = Condition();
            double turn = gamma * condition;
            Rounding = gamma * (condition + (2.0 * Math.Sqrt(columns))) / (1.0 - turn);
            FullRank = rows >= columns && Array.TrueForAll(_scales, scale => scale > 0.0) && turn < 0.5;
        }

        /// <summary>Whether every column added a direction and the span's turn, <c>γκ</c>, stays under a half.</summary>
        internal bool FullRank { get; }

        /// <summary>Columns of the block, the most directions the span holds.</summary>
        internal int Columns { get; }

        /// <summary>A bound on the error of <see cref="Apply"/> per unit of its argument's norm, and on each <c>qᵢ·qⱼ</c>, the turn of the span included.</summary>
        internal double Rounding { get; }

        /// <summary><c>‖R‖_F·‖R⁻¹‖_F</c> of the scaled columns' triangle, infinite where it has a zero pivot.</summary>
        private double Condition()
        {
            int k = Columns;
            var upper = new double[k * k];
            for (int c = 0; c < k; c++)
            {
                for (int i = 0; i < c; i++)
                {
                    upper[(i * k) + c] = _vectors[(c * _rows) + i];
                }

                upper[(c * k) + c] = _diagonal[c];
            }

            double norm = 0.0;
            double inverseNorm = 0.0;
            var column = new double[k];
            for (int j = 0; j < k; j++)
            {
                column[j] = 1.0 / upper[(j * k) + j];
                for (int i = j - 1; i >= 0; i--)
                {
                    double total = 0.0;
                    for (int l = i + 1; l <= j; l++)
                    {
                        total += upper[(i * k) + l] * column[l];
                    }

                    column[i] = -total / upper[(i * k) + i];
                }

                for (int i = 0; i <= j; i++)
                {
                    norm += upper[(i * k) + j] * upper[(i * k) + j];
                    inverseNorm += column[i] * column[i];
                }
            }

            return Math.Sqrt(norm) * Math.Sqrt(inverseNorm);
        }

        /// <summary><c>δ</c> with <c>Xδ = Hv</c>: <c>R⁻¹Qᵀv</c> on the scaled columns, each then read at its own scale.</summary>
        internal double[] Coefficients(double[] values)
        {
            double[] z = (double[])values.Clone();
            for (int j = 0; j < Columns; j++)
            {
                ApplyOne(j, z);
            }

            var delta = new double[Columns];
            for (int i = Columns - 1; i >= 0; i--)
            {
                double total = z[i];
                for (int c = i + 1; c < Columns; c++)
                {
                    total -= _vectors[(c * _rows) + i] * delta[c];
                }

                delta[i] = total / _diagonal[i];
            }

            for (int j = 0; j < Columns; j++)
            {
                delta[j] *= _columnScales[j];
            }

            return delta;
        }

        /// <summary><c>Hv</c> where <paramref name="keepSpan"/>, <c>(I − H)v</c> otherwise.</summary>
        internal double[] Apply(double[] values, bool keepSpan)
        {
            double[] z = (double[])values.Clone();
            int reach = Math.Min(_rows, Columns);
            for (int j = 0; j < reach; j++)
            {
                ApplyOne(j, z);
            }

            for (int i = 0; i < _rows; i++)
            {
                if ((i < reach) != keepSpan)
                {
                    z[i] = 0.0;
                }
            }

            for (int j = reach - 1; j >= 0; j--)
            {
                ApplyOne(j, z);
            }

            return z;
        }

        /// <summary><c>Q</c>'s rows, row-major: <c>Hᵢⱼ = qᵢ·qⱼ</c>.</summary>
        internal double[] Basis()
        {
            int reach = Math.Min(_rows, Columns);
            var basis = new double[_rows * Columns];
            var column = new double[_rows];
            for (int c = 0; c < reach; c++)
            {
                Array.Clear(column, 0, _rows);
                column[c] = 1.0;

                // A reflection past c leaves e_c, zero at and below its diagonal, as it was.
                for (int j = c; j >= 0; j--)
                {
                    ApplyOne(j, column);
                }

                for (int i = 0; i < _rows; i++)
                {
                    basis[(i * Columns) + c] = column[i];
                }
            }

            return basis;
        }

        /// <summary>Builds reflection <paramref name="j"/> from column j below its diagonal and applies it to the columns after.</summary>
        private void Reflect(int j)
        {
            int at = j * _rows;
            ReadOnlySpan<double> below = _vectors.AsSpan(at + j, _rows - j);
            double norm = Math.Sqrt(Lodestar.Internal.Reflections.Dot(below, below));
            if (!(norm > 0.0))
            {
                return;
            }

            double head = _vectors[at + j];
            double alpha = head >= 0.0 ? -norm : norm;
            _vectors[at + j] = head - alpha;
            _diagonal[j] = alpha;
            _scales[j] = norm * (norm + Math.Abs(head));
            ReadOnlySpan<double> vector = _vectors.AsSpan(at + j, _rows - j);
            for (int c = j + 1; c < Columns; c++)
            {
                Lodestar.Internal.Reflections.Reflect(vector, _scales[j], _vectors.AsSpan((c * _rows) + j, _rows - j));
            }
        }

        /// <summary>Applies reflection <paramref name="j"/>, its own inverse, to <paramref name="z"/>.</summary>
        private void ApplyOne(int j, double[] z)
        {
            if (_scales[j] > 0.0)
            {
                Lodestar.Internal.Reflections.Reflect(_vectors.AsSpan((j * _rows) + j, _rows - j), _scales[j], z.AsSpan(j));
            }
        }
    }

    /// <summary>A residual's size with the terms it was computed from, <c>|rᵢ| + Σⱼ |xᵢⱼbⱼ|</c>, which its rounding scales with.</summary>
    /// <param name="matrix">The rows, row-major, as many columns as <paramref name="coefficients"/>.</param>
    /// <param name="residual">The row's residual.</param>
    /// <param name="coefficients">The estimate.</param>
    /// <param name="at">Where the row starts in <paramref name="matrix"/>.</param>
    public static double Magnitude(double[] matrix, double residual, double[] coefficients, int at)
    {
        double size = Math.Abs(residual);
        for (int j = 0; j < coefficients.Length; j++)
        {
            size += Math.Abs(matrix[at + j] * coefficients[j]);
        }

        return size;
    }

    /// <summary>Whether every residual is exactly zero, so <c>s² = 0</c> and a homoskedastic covariance is zero.</summary>
    /// <remarks>
    /// Each residual, not their sum of squares: residuals below 1e-162 square to zero, and a noisy fit whose response
    /// underflows too, as at 1e-170, read <c>+∞</c> where its arithmetic reads 0/0 (Review A pass 7 on #1779). Stops at the
    /// first that is not zero.
    /// </remarks>
    public static bool Vanishes(double[] residuals) => Array.TrueForAll(residuals, IsZero);

    // S1244: exactly zero, the one value whose square cannot underflow from a nonzero residual.
#pragma warning disable S1244
    private static bool IsZero(double value) => value == 0.0;
#pragma warning restore S1244
}

/// <summary>A transformed fit's untransformed rows, and the rounding floor its transform leaves each fitted row.</summary>
/// <param name="X">The untransformed regressors, row-major.</param>
/// <param name="Y">The untransformed response.</param>
/// <param name="Floors">Each fitted row's floor, built only by a fit that passes the screen.</param>
internal sealed record Transform(double[] X, double[] Y, Func<double[]> Floors);
