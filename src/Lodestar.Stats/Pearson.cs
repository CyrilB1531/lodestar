using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Pearson correlation: is there a linear relationship between two variables?</summary>
/// <remarks>
/// The parametric member of the three correlation tests, and the only one that measures a
/// *linear* relationship rather than a monotone one — <see cref="Spearman"/> and
/// <see cref="KendallTau"/> rank first and so answer a weaker question about stronger data.
/// The null it tests is that the two samples are uncorrelated and normally distributed, which
/// is where the normality assumption enters; on a heavy-tailed sample the rank tests are the
/// safer reading.
/// </remarks>
public static class Pearson
{
    /// <summary>Correlates two paired samples.</summary>
    /// <param name="x">The first sample; at least two values.</param>
    /// <param name="y">The second sample; the same length as <paramref name="x"/>.</param>
    /// <param name="alternative">Which tail the p-value covers.</param>
    /// <param name="nanPolicy">
    /// What to do with a <c>NaN</c> in either sample. <see cref="NanPolicy.Omit"/> drops the
    /// pair, not the value, so the two samples stay aligned.
    /// </param>
    /// <returns>
    /// The correlation coefficient in <c>[-1, 1]</c>, the p-value, and a
    /// <see cref="PearsonResult.ConfidenceInterval"/> the result can be asked for afterwards.
    /// A sample that is constant has no correlation defined, and both numbers are <c>NaN</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The samples differ in length, or hold fewer than two values. When
    /// <paramref name="nanPolicy"/> is <see cref="NanPolicy.Raise"/> and either sample holds a
    /// <c>NaN</c>.
    /// </exception>
    public static PearsonResult Test(
        ReadOnlySpan<double> x,
        ReadOnlySpan<double> y,
        Alternative alternative = Alternative.TwoSided,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        PairedSamples.Align(
            x, y, nanPolicy, nameof(x), nameof(y),
            out ReadOnlySpan<double> left, out ReadOnlySpan<double> right);
        if (left.Length < 2)
        {
            throw new ArgumentException(
                $"A correlation needs at least two pairs; got {left.Length}.", nameof(x));
        }

        int n = left.Length;
        if (Ranks.HasNaN(left) || Ranks.HasNaN(right))
        {
            return new PearsonResult(double.NaN, double.NaN) { N = n, Alternative = alternative };
        }

        double coefficient = Correlation.Coefficient(left, right);
        if (double.IsNaN(coefficient))
        {
            // A constant sample has no correlation, so there is no statistic for a tail to be
            // taken of -- and the beta below is not defined at a NaN argument.
            return new PearsonResult(double.NaN, double.NaN) { N = n, Alternative = alternative };
        }

        // Two pairs admit only +1 and -1, so the two-sided p-value is 1: scipy documents that
        // limit of the beta rather than evaluating a density undefined at shape zero.
        double pValue = n == 2 ? 1.0 : PValue(coefficient, n, alternative);

        return new PearsonResult(coefficient, pValue) { N = n, Alternative = alternative };
    }

    /// <summary>The tail of the null distribution of <c>r</c>: a beta on <c>[-1, 1]</c>.</summary>
    /// <remarks>
    /// The shape parameters are equal, so the distribution is symmetric about zero and each
    /// tail is the other's reflection: <c>greater</c> reads the upper tail directly instead of
    /// subtracting the lower one from one, which is where the far tail's digits would go.
    /// The algebraically equal Student form loses them earlier still, in <c>1 - r²</c>.
    /// </remarks>
    private static double PValue(double r, int n, Alternative alternative)
    {
        double shape = (n / 2.0) - 1.0;

        return alternative switch
        {
            Alternative.Less => Beta.RegularizedIncomplete(shape, shape, (1.0 + r) / 2.0),
            Alternative.Greater => Beta.RegularizedIncomplete(shape, shape, (1.0 - r) / 2.0),
            Alternative.TwoSided =>
                2.0 * Beta.RegularizedIncomplete(shape, shape, (1.0 - Math.Abs(r)) / 2.0),
            _ => throw new ArgumentOutOfRangeException(nameof(alternative), alternative, null),
        };
    }
}
