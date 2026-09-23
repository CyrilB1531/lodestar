using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>Kendall's tau: do two rankings agree, pair by pair?</summary>
/// <remarks>
/// Where <see cref="Spearman"/> correlates the ranks themselves, this counts the pairs the two
/// samples order the same way against the pairs they order oppositely, so it reads as a
/// probability rather than as a correlation and is the steadier of the two on a short sample.
/// Both normalisations scipy publishes are offered — see <see cref="KendallVariant"/> — and
/// they share a p-value, because the count they normalise is the same count.
/// </remarks>
public static class KendallTau
{
    // Measured against scipy 1.18.1: Auto is exact up to and including 33 observations, or
    // whenever one tail holds at most one pair, and only when nothing is tied.
    private const int AutoExactThreshold = 33;

    /// <summary>Correlates two paired samples by counting concordant and discordant pairs.</summary>
    /// <param name="x">The first sample.</param>
    /// <param name="y">The second sample; the same length as <paramref name="x"/>.</param>
    /// <param name="alternative">Which tail the p-value covers.</param>
    /// <param name="variant">Which normalisation the statistic carries; the p-value is the same either way.</param>
    /// <param name="method">Exact, asymptotic, or chosen by sample size and ties; <see cref="ExactMethod.Auto"/> never throws.</param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; <see cref="NanPolicy.Omit"/> drops the pair, not the value.</param>
    /// <returns>Tau and the p-value; both <c>NaN</c> below two pairs or on a fully tied sample.</returns>
    /// <exception cref="ArgumentException">
    /// The samples differ in length. When <paramref name="method"/> is
    /// <see cref="ExactMethod.Exact"/> and either sample holds a tie -- the one input the exact
    /// distribution is not defined over, unlike <see cref="MannWhitney"/>. When
    /// <paramref name="nanPolicy"/> is <see cref="NanPolicy.Raise"/> and a <c>NaN</c> is present.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="method"/> is <see cref="ExactMethod.Exact"/> and the null distribution's
    /// table would exceed twenty million cells; its cost is proportional to that count. Pass
    /// <see cref="ExactMethod.Asymptotic"/> instead.
    /// </exception>
    public static TestResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        KendallVariant variant = KendallVariant.TauB,
        ExactMethod method = ExactMethod.Auto,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        PairedSamples.Align(
            x, y, nanPolicy, nameof(x), nameof(y),
            out ReadOnlySpan<double> left, out ReadOnlySpan<double> right);

        // As for Spearman, scipy answers rather than refuses below two pairs.
        if (left.Length < 2 || Ranks.HasNaN(left) || Ranks.HasNaN(right))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        int n = left.Length;
        long total = (long)n * (n - 1) / 2;
        ConcordanceCounts counts = Concordance.Compute(left, right);

        // A fully tied sample orders no pair, so the normalisation would divide by zero.
        // scipy answers NaN here rather than raising, and so does this.
        if (counts.XTies == total || counts.YTies == total)
        {
            return new TestResult(double.NaN, double.NaN);
        }

        double excess = total - counts.XTies - counts.YTies + counts.JointTies
            - (2.0 * counts.Discordant);

        return new TestResult(
            Statistic(excess, counts, n, total, variant),
            PValue(excess, counts, n, total, alternative, method));
    }

    /// <summary>Tau itself: the same concordance excess, under whichever scale was asked for.</summary>
    private static double Statistic(
        double excess, ConcordanceCounts counts, int n, long total, KendallVariant variant)
    {
        double tau;
        if (variant == KendallVariant.TauB)
        {
            // Two divisions rather than one root of a product: the product of two pair counts
            // overflows a double past about 1.3e9 observations, and each root does not.
            tau = excess / Math.Sqrt(total - counts.XTies) / Math.Sqrt(total - counts.YTies);
        }
        else
        {
            double classes = Math.Min(counts.DistinctX, counts.DistinctY);
            tau = 2.0 * excess / ((double)n * n * ((classes - 1.0) / classes));
        }

        // Rounding can carry either scale a bit past its own range.
        return Math.Min(1.0, Math.Max(-1.0, tau));
    }

    private static double PValue(
        double excess,
        ConcordanceCounts counts,
        int n,
        long total,
        Alternative alternative,
        ExactMethod method)
    {
        bool tied = counts.XTies != 0L || counts.YTies != 0L;

        if (method == ExactMethod.Exact && tied)
        {
            throw new ArgumentException(
                "The exact Kendall distribution is defined over untied samples only, and one of "
                + "these holds a tie. Pass ExactMethod.Asymptotic instead.",
                nameof(method));
        }

        long concordant = total - counts.Discordant;
        bool wantsExact = method switch
        {
            ExactMethod.Exact => true,
            ExactMethod.Asymptotic => false,
            _ => !tied
                && (n <= AutoExactThreshold
                    || Math.Min(counts.Discordant, total - counts.Discordant) <= 1L),
        };

        if (!wantsExact)
        {
            return AsymptoticPValue(excess, counts, n, alternative);
        }

        bool tableTooLarge = KendallExact.Cells(n, concordant) > KendallExact.MaxCells;
        if (method == ExactMethod.Exact && tableTooLarge)
        {
            throw new ArgumentOutOfRangeException(
                nameof(method),
                method,
                $"An exact Kendall p-value here would walk {KendallExact.Cells(n, concordant)} table "
                + $"cells, past the {KendallExact.MaxCells} this refuses at. Pass "
                + "ExactMethod.Asymptotic instead.");
        }

        // Auto never throws: past the bound it takes the asymptotic answer instead, since
        // nothing the caller wrote asked for an exact one.
        return tableTooLarge
            ? AsymptoticPValue(excess, counts, n, alternative)
            : KendallExact.PValue(n, concordant, alternative);
    }

    /// <summary>The normal approximation, whose variance the tie groups shrink.</summary>
    private static double AsymptoticPValue(
        double excess, ConcordanceCounts counts, int n, Alternative alternative)
    {
        double pairs = (double)n * (n - 1.0);
        double variance = (((pairs * ((2.0 * n) + 5.0)) - counts.X1 - counts.Y1) / 18.0)
            + (2.0 * counts.XTies * counts.YTies / pairs)
            + (counts.X0 * counts.Y0 / (9.0 * pairs * (n - 2.0)));
        double z = excess / Math.Sqrt(variance);

        return alternative switch
        {
            // Sf(-z), not 1 - Sf(z): the far tail is where the complement of a value near one
            // has no bits left to give.
            Alternative.Less => Normal.Sf(-z),
            Alternative.Greater => Normal.Sf(z),
            Alternative.TwoSided => 2.0 * Normal.Sf(Math.Abs(z)),
            _ => throw new ArgumentOutOfRangeException(nameof(alternative), alternative, null),
        };
    }
}
