using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>Spearman's rho: is there a monotone relationship between two variables?</summary>
/// <remarks>
/// <see cref="Pearson"/> on the mid-ranks rather than on the values, which is the whole
/// definition: it answers whether the two rise and fall together, not whether they do so
/// along a line, and a single outlier moves a rank by one place instead of moving a mean.
/// The p-value is the Student approximation on <c>n - 2</c> degrees of freedom at every
/// sample size — scipy publishes no exact branch for <c>spearmanr</c>, so neither is offered
/// here.
/// </remarks>
public static class Spearman
{
    /// <summary>Correlates the ranks of two paired samples.</summary>
    /// <param name="x">The first sample.</param>
    /// <param name="y">The second sample; the same length as <paramref name="x"/>.</param>
    /// <param name="alternative">Which tail the p-value covers.</param>
    /// <param name="nanPolicy">
    /// What to do with a <c>NaN</c> in either sample. <see cref="NanPolicy.Omit"/> drops the
    /// pair, not the value, so the two samples stay aligned and rank against each other.
    /// </param>
    /// <returns>
    /// Rho and the p-value. Fewer than two pairs, or a constant sample, leaves the
    /// correlation undefined and both numbers are <c>NaN</c>; so is the p-value alone at
    /// exactly two pairs, where the Student distribution has no degrees of freedom left.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The samples differ in length. When <paramref name="nanPolicy"/> is
    /// <see cref="NanPolicy.Raise"/> and either sample holds a <c>NaN</c>.
    /// </exception>
    public static TestResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        PairedSamples.Align(
            x, y, nanPolicy, nameof(x), nameof(y),
            out ReadOnlySpan<double> left, out ReadOnlySpan<double> right);

        // scipy answers rather than refuses below two pairs, where pearsonr raises: the two
        // disagree upstream, and each is matched rather than reconciled here.
        if (left.Length < 2 || Ranks.HasNaN(left) || Ranks.HasNaN(right))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        double rho = Correlation.RankCoefficient(Ranks.Average(left), Ranks.Average(right));
        double dof = left.Length - 2.0;

        return new TestResult(rho, PValue(rho, dof, alternative));
    }

    /// <summary>The Student tail of rho, on <c>n - 2</c> degrees of freedom.</summary>
    /// <remarks>
    /// The ratio under the root is clipped at zero before it is taken: a rho of exactly 1
    /// makes <c>(1 + rho)(1 - rho)</c> zero and the division an infinity, which is the
    /// answer — a p-value of zero — while rounding a shade past 1 would make it negative and
    /// the root a <c>NaN</c>. scipy clips it in the same place.
    /// </remarks>
    private static double PValue(double rho, double dof, Alternative alternative)
    {
        if (dof <= 0.0 || double.IsNaN(rho))
        {
            return double.NaN;
        }

        double ratio = dof / ((rho + 1.0) * (1.0 - rho));
        double t = rho * Math.Sqrt(Math.Max(0.0, ratio));

        return alternative switch
        {
            // Sf(-t) rather than 1 - Sf(t): the far tail is where the complement of a value
            // near one has no bits left to lose.
            Alternative.Less => Beta.StudentSf(-t, dof),
            Alternative.Greater => Beta.StudentSf(t, dof),
            Alternative.TwoSided => 2.0 * Beta.StudentSf(Math.Abs(t), dof),
            _ => throw new ArgumentOutOfRangeException(nameof(alternative), alternative, null),
        };
    }
}
