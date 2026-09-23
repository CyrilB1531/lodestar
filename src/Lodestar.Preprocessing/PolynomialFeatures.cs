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
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value; or the degree is 0 with no bias, which leaves no term.</exception>
    public static double[] Transform(
        ReadOnlySpan<double> samples, int featureCount, PolynomialFeaturesOptions? options = null)
    {
        PolynomialFeaturesOptions settings = Validated(featureCount, options);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        int[][] terms = Terms(featureCount, settings);
        var expanded = new double[(long)sampleCount * terms.Length <= int.MaxValue
            ? sampleCount * terms.Length
            : throw new ArgumentOutOfRangeException(
                nameof(options), settings.Degree,
                $"Expanding {sampleCount} rows into {terms.Length} terms needs more than int.MaxValue values.")];

        for (int row = 0; row < sampleCount; row++)
        {
            int source = row * featureCount;
            int target = row * terms.Length;
            for (int term = 0; term < terms.Length; term++)
            {
                double product = 1.0;
                foreach (int feature in terms[term])
                {
                    product *= samples[source + feature];
                }

                expanded[target + term] = product;
            }
        }

        return expanded;
    }

    /// <summary>How many terms an expansion produces, without producing one.</summary>
    /// <param name="featureCount">How many values each input row carries.</param>
    /// <param name="options">Degree, interactions and the bias; <see langword="null"/> takes the reference's defaults.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the degree is negative.</exception>
    /// <exception cref="ArgumentException">The degree is 0 with no bias, which leaves no term.</exception>
    public static int OutputFeatureCount(int featureCount, PolynomialFeaturesOptions? options = null) =>
        Terms(featureCount, Validated(featureCount, options)).Length;

    /// <summary>Each term's name, in the order the columns come out.</summary>
    /// <param name="featureCount">How many values each input row carries.</param>
    /// <param name="options">Degree, interactions and the bias; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>Names in the reference's own spelling — <c>1</c>, <c>x0</c>, <c>x0^2</c>, <c>x0 x1</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the degree is negative.</exception>
    /// <exception cref="ArgumentException">The degree is 0 with no bias, which leaves no term.</exception>
    public static string[] FeatureNames(int featureCount, PolynomialFeaturesOptions? options = null)
    {
        int[][] terms = Terms(featureCount, Validated(featureCount, options));
        var names = new string[terms.Length];
        for (int i = 0; i < terms.Length; i++)
        {
            names[i] = Name(terms[i]);
        }

        return names;
    }

    private static PolynomialFeaturesOptions Validated(int featureCount, PolynomialFeaturesOptions? options)
    {
        Guard.NotLessThan(featureCount, 1);
        PolynomialFeaturesOptions settings = options ?? new PolynomialFeaturesOptions();
        Guard.NotLessThan(settings.Degree, 0);
        if (settings.Degree == 0 && !settings.IncludeBias)
        {
            // There is no term at all in that pair, and the reference refuses it in as many
            // words rather than returning a matrix with no columns.
            throw new ArgumentException(
                "Degree 0 without the bias leaves no term to expand into.", nameof(options));
        }

        return settings;
    }

    /// <summary>Every term, as the feature indices it multiplies, in the reference's order.</summary>
    /// <remarks>
    /// Degree by degree, and inside a degree in lexicographic order over the indices — with
    /// repetition unless <see cref="PolynomialFeaturesOptions.InteractionOnly"/> is set, which is
    /// what makes <c>x0²</c> a term of the first and not of the second.
    /// </remarks>
    private static int[][] Terms(int featureCount, PolynomialFeaturesOptions settings)
    {
        var terms = new List<int[]>();
        if (settings.IncludeBias)
        {
            terms.Add([]);
        }

        for (int degree = 1; degree <= settings.Degree; degree++)
        {
            var combination = new int[degree];
            Extend(terms, combination, 0, 0, featureCount, settings.InteractionOnly);
        }

        return [.. terms];
    }

    private static void Extend(
        List<int[]> terms, int[] combination, int position, int first, int featureCount, bool interactionOnly)
    {
        if (position == combination.Length)
        {
            terms.Add((int[])combination.Clone());
            return;
        }

        for (int feature = first; feature < featureCount; feature++)
        {
            combination[position] = feature;
            Extend(terms, combination, position + 1, interactionOnly ? feature + 1 : feature, featureCount, interactionOnly);
        }
    }

    private static string Name(int[] term)
    {
        if (term.Length == 0)
        {
            return "1";
        }

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

            // Formatted invariantly: a name is an identifier a caller matches on, not a number
            // to read, and a culture that groups digits would change it.
            parts.Add(power == 1
                ? "x" + feature.ToString(CultureInfo.InvariantCulture)
                : "x" + feature.ToString(CultureInfo.InvariantCulture)
                    + "^" + power.ToString(CultureInfo.InvariantCulture));
        }

        return string.Join(" ", parts);
    }
}
