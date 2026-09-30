using System.Globalization;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>Expands each row into its polynomial terms, at <c>sklearn.preprocessing.PolynomialFeatures</c> parity.</summary>
/// <remarks>
/// Fits nothing — the terms are decided by the feature count and the degree, not by the data — so
/// this is static, as <see cref="Normalizer"/> is.
/// <strong>The column order is the contract.</strong> A caller reading a coefficient back needs
/// to know which term it belongs to, and the only useful answer is the reference's own order: the
/// bias, degree one in feature order, then <c>x0², x0·x1, x1²</c>, and so on.
/// <see cref="FeatureNames"/> returns exactly that order, in the reference's own spelling.
/// </remarks>
public static class PolynomialFeatures
{
    /// <summary>Expands a row-major matrix into its polynomial terms.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Degree, interactions and the bias; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A new matrix, <see cref="OutputFeatureCount"/> values per row.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the degree is negative.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value; or the degree is 0 with no bias, which leaves no term; or the terms, or the rows times the terms, are more than one array holds.</exception>
    /// <remarks>
    /// Term by term as the reference builds them, a row at a time: each term of degree <c>d</c> is a term of degree
    /// <c>d − 1</c> times one feature, so the products round as the reference's do and no term is listed first — two
    /// features at degree 3000 cost their output, not an index array per term (#1230).
    /// </remarks>
    public static double[] Transform(
        ReadOnlySpan<double> samples, int featureCount, PolynomialFeaturesOptions? options = null)
    {
        // The previous release's refusals in its order: the options, then the rows and their values; the term count,
        // past one array, only then, where that release ran out of memory listing the terms.
        PolynomialFeaturesOptions settings = Checked(featureCount, options);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));
        int count = Counted(featureCount, settings, nameof(options));

        // Bounded by the largest array rather than int.MaxValue, and blamed on the rows it expands (#1314).
        var expanded = new double[TableLength.Of(sampleCount, count, nameof(samples))];
        int first = settings.IncludeBias ? 1 : 0;
        List<Segment> segments = settings.Degree == 0 ? [] : Plan(featureCount, settings, first);

        // Row by row, as the previous release wrote them: the plan is the same for every row, and a row's terms sit
        // together in memory where a column's are a row apart.
        for (int row = 0; row < sampleCount; row++)
        {
            Span<double> output = expanded.AsSpan(row * count, count);
            if (settings.IncludeBias)
            {
                output[0] = 1.0;
            }

            if (settings.Degree == 0)
            {
                continue;
            }

            ReadOnlySpan<double> input = samples.Slice(row * featureCount, featureCount);
            input.CopyTo(output.Slice(first));
            foreach (Segment segment in segments)
            {
                double value = input[segment.Feature];
                Span<double> target = output.Slice(segment.Target, segment.Length);
                ReadOnlySpan<double> source = output.Slice(segment.Source, segment.Length);
                for (int k = 0; k < target.Length; k++)
                {
                    target[k] = source[k] * value;
                }
            }
        }

        return expanded;
    }

    /// <summary>A run of terms one degree up: the terms at <c>Source</c> times feature <c>Feature</c>, written at <c>Target</c>.</summary>
    private readonly record struct Segment(int Feature, int Source, int Target, int Length);

    /// <summary>Every degree's runs past the first, in the order scikit-learn writes them, one plan for every row.</summary>
    private static List<Segment> Plan(int featureCount, PolynomialFeaturesOptions settings, int first)
    {
        var segments = new List<Segment>();
        var index = new int[featureCount + 1];
        for (int feature = 0; feature <= featureCount; feature++)
        {
            index[feature] = first + feature;
        }

        int column = first + featureCount;
        for (int degree = 2; degree <= MaxDegreeWithTerms(featureCount, settings); degree++)
        {
            column = NextDegree(settings.InteractionOnly, index, column, segments);
        }

        return segments;
    }

    /// <summary>
    /// One degree more, scikit-learn's loop: the terms of the last degree that start at feature <c>f</c> or later — past
    /// it, interactions only — times feature <c>f</c>, for each <c>f</c> until none remain.
    /// </summary>
    /// <returns>The first column past the degree planned, as <paramref name="index"/> is rewritten to its starts.</returns>
    private static int NextDegree(bool interactionOnly, int[] index, int column, List<Segment> segments)
    {
        int featureCount = index.Length - 1;
        int end = index[featureCount];
        var starts = new int[featureCount + 1];
        int feature = 0;
        for (; feature < featureCount; feature++)
        {
            int start = index[feature];
            starts[feature] = column;
            if (interactionOnly)
            {
                start += index[feature + 1] - index[feature];
            }

            int next = column + end - start;
            if (next <= column)
            {
                break;
            }

            segments.Add(new Segment(feature, start, column, end - start));
            column = next;
        }

        for (int rest = Math.Min(feature + 1, featureCount); rest <= featureCount; rest++)
        {
            starts[rest] = column;
        }

        Array.Copy(starts, index, featureCount + 1);
        return column;
    }

    /// <summary>How many terms an expansion produces, without producing one.</summary>
    /// <param name="featureCount">How many values each input row carries.</param>
    /// <param name="options">Degree, interactions and the bias; <see langword="null"/> takes the reference's defaults.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the degree is negative.</exception>
    /// <exception cref="ArgumentException">The degree is 0 with no bias, which leaves no term; or the terms are more than one array holds.</exception>
    public static int OutputFeatureCount(int featureCount, PolynomialFeaturesOptions? options = null) =>
        Validated(featureCount, options).Count;

    /// <summary>Each term's name, in the order the columns come out.</summary>
    /// <param name="featureCount">How many values each input row carries.</param>
    /// <param name="options">Degree, interactions and the bias; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>Names in the reference's own spelling — <c>1</c>, <c>x0</c>, <c>x0^2</c>, <c>x0 x1</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the degree is negative.</exception>
    /// <exception cref="ArgumentException">The degree is 0 with no bias, which leaves no term; or the terms are more than one array holds.</exception>
    /// <remarks>
    /// Degree by degree, and inside a degree in lexicographic order over the indices — with repetition unless
    /// <see cref="PolynomialFeaturesOptions.InteractionOnly"/> is set, which is what makes <c>x0²</c> a term of the
    /// first and not of the second. Each term is named as it is reached, none kept but its name.
    /// </remarks>
    public static string[] FeatureNames(int featureCount, PolynomialFeaturesOptions? options = null)
    {
        (PolynomialFeaturesOptions settings, int count) = Validated(featureCount, options);
        var names = new string[count];
        int written = 0;
        if (settings.IncludeBias)
        {
            names[written++] = "1";
        }

        for (int degree = 1; degree <= MaxDegreeWithTerms(featureCount, settings); degree++)
        {
            // A term costs its index list or its power vector, whichever is shorter: the degree or the features.
            if (!settings.InteractionOnly && degree > featureCount)
            {
                written = NamePowers(names, written, degree, featureCount);
                continue;
            }

            int[]? combination = First(degree, settings.InteractionOnly);
            while (combination is not null)
            {
                names[written++] = Name(combination);
                combination = Next(combination, featureCount, settings.InteractionOnly);
            }
        }

        return names;
    }

    /// <summary>
    /// Names a degree's terms, repetition allowed, by each feature's power rather than by the degree's index list:
    /// in the same order <see cref="Next"/> walks them, at a cost per term of the features, not of the degree. Taken
    /// past the feature count only, where two features at degree 3000 would build a 3000-index list per term (#1230).
    /// </summary>
    private static int NamePowers(string[] names, int written, int degree, int featureCount)
    {
        var powers = new int[featureCount];
        powers[0] = degree;
        do
        {
            names[written++] = PowersName(powers);
        }
        while (NextPowers(powers));

        return written;
    }

    /// <summary><see cref="Next"/>'s step on the powers: the last index rises, or the last below the top rises and takes the run above it.</summary>
    private static bool NextPowers(int[] powers)
    {
        int top = powers.Length - 1;
        int last = top;
        while (powers[last] == 0)
        {
            last--;
        }

        if (last < top)
        {
            powers[last]--;
            powers[last + 1]++;
            return true;
        }

        int below = top - 1;
        while (below >= 0 && powers[below] == 0)
        {
            below--;
        }

        if (below < 0)
        {
            return false;
        }

        int run = powers[top];
        powers[below]--;
        powers[top] = 0;
        powers[below + 1] += run + 1;
        return true;
    }

    /// <summary>A term named from its powers, as <see cref="Name(int[])"/> names it from its index list.</summary>
    private static string PowersName(int[] powers)
    {
        var parts = new List<string>();
        for (int feature = 0; feature < powers.Length; feature++)
        {
            if (powers[feature] > 0)
            {
                parts.Add(Part(feature, powers[feature]));
            }
        }

        return string.Join(" ", parts);
    }

    /// <summary>The last degree with a term: past the feature count, interactions only, there is none.</summary>
    private static int MaxDegreeWithTerms(int featureCount, PolynomialFeaturesOptions settings) =>
        settings.InteractionOnly ? Math.Min(settings.Degree, featureCount) : settings.Degree;

    /// <summary>The first combination of a degree in lexicographic order; the caller asks only for a degree with terms.</summary>
    private static int[] First(int degree, bool interactionOnly)
    {
        var combination = new int[degree];
        for (int i = 0; i < degree; i++)
        {
            combination[i] = interactionOnly ? i : 0;
        }

        return combination;
    }

    /// <summary>The next combination in lexicographic order, advanced in place, or none past the last.</summary>
    private static int[]? Next(int[] combination, int featureCount, bool interactionOnly)
    {
        int degree = combination.Length;
        for (int i = degree - 1; i >= 0; i--)
        {
            int ceiling = interactionOnly ? featureCount - degree + i : featureCount - 1;
            if (combination[i] < ceiling)
            {
                combination[i]++;
                for (int j = i + 1; j < degree; j++)
                {
                    combination[j] = interactionOnly ? combination[j - 1] + 1 : combination[i];
                }

                return combination;
            }
        }

        return null;
    }

    private static (PolynomialFeaturesOptions Settings, int Count) Validated(int featureCount, PolynomialFeaturesOptions? options)
    {
        PolynomialFeaturesOptions settings = Checked(featureCount, options);
        return (settings, Counted(featureCount, settings, nameof(options)));
    }

    private static PolynomialFeaturesOptions Checked(int featureCount, PolynomialFeaturesOptions? options)
    {
        PolynomialFeaturesOptions settings = Parameters(featureCount, options);
        RequireSomeTerm(settings, nameof(options));
        return settings;
    }

    private static PolynomialFeaturesOptions Parameters(int featureCount, PolynomialFeaturesOptions? options)
    {
        Guard.NotLessThan(featureCount, 1);
        PolynomialFeaturesOptions settings = options ?? new PolynomialFeaturesOptions();
        Guard.NotLessThan(settings.Degree, 0);
        return settings;
    }

    private static void RequireSomeTerm(PolynomialFeaturesOptions settings, string paramName)
    {
        if (settings.Degree == 0 && !settings.IncludeBias)
        {
            // There is no term at all in that pair, and the reference refuses it in as many
            // words rather than returning a matrix with no columns.
            throw new ArgumentException("Degree 0 without the bias leaves no term to expand into.", paramName);
        }
    }

    /// <summary>The term count, refused past one array before anything is built (#1230).</summary>
    private static int Counted(int featureCount, PolynomialFeaturesOptions settings, string paramName)
    {
        System.Numerics.BigInteger count = TermCount(featureCount, settings);
        if (count > TableLength.MaxLength)
        {
            throw new ArgumentException(
                $"The expansion would have more terms than one array holds ({TableLength.MaxLength}).", paramName);
        }

        return (int)count;
    }

    /// <summary>
    /// How many terms there are, in closed form: <c>C(n + D, D)</c> with repetition, <c>Σ C(n, d)</c> for <c>d ≤ min(n, D)</c>
    /// interactions only, less the bias when it is off — stopped as soon as it passes what an array holds.
    /// </summary>
    private static System.Numerics.BigInteger TermCount(int featureCount, PolynomialFeaturesOptions settings)
    {
        int steps = Math.Min(featureCount, settings.Degree);
        long top = (long)featureCount + settings.Degree;
        System.Numerics.BigInteger total = 1;
        System.Numerics.BigInteger subsets = 1;
        for (int k = 1; k <= steps; k++)
        {
            if (settings.InteractionOnly)
            {
                subsets = subsets * (featureCount - k + 1) / k;
                total += subsets;
            }
            else
            {
                // C(n + D, k) climbs with k up to min(n, D), so it may stop once past the bound.
                total = total * (top - k + 1) / k;
            }

            if (total > TableLength.MaxLength + 1)
            {
                // A partial count this far past the bound stays past it with the bias taken off: refused as it is.
                return total;
            }
        }

        return settings.IncludeBias ? total : total - 1;
    }

    private static string Name(int[] term)
    {
        var parts = new List<string>();
        int index = 0;
        while (index < term.Length)
        {
            int feature = term[index];
            int power = 0;
            while (index < term.Length && term[index] == feature)
            {
                power++;
                index++;
            }

            parts.Add(Part(feature, power));
        }

        return string.Join(" ", parts);
    }

    /// <summary>One feature's part of a name, formatted invariantly: an identifier a caller matches on.</summary>
    private static string Part(int feature, int power) => power == 1
        ? "x" + feature.ToString(CultureInfo.InvariantCulture)
        : "x" + feature.ToString(CultureInfo.InvariantCulture) + "^" + power.ToString(CultureInfo.InvariantCulture);
}
