namespace Lodestar.Stats;

/// <summary>The point-biserial correlation: how a continuous variable differs between the two sides of a binary one.</summary>
/// <remarks>
/// <see cref="Pearson"/> with one variable coded 0 and 1, which is all scipy's <c>pointbiserialr</c> is: the
/// coefficient is the standardised gap between the two groups' means, and its p-value is Pearson's two-sided one.
/// </remarks>
public static class PointBiserial
{
    /// <summary>Correlates a binary variable with a continuous one.</summary>
    /// <param name="x">The binary variable.</param>
    /// <param name="y">The continuous variable; the same length as <paramref name="x"/>.</param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c> in <paramref name="y"/>; <see cref="NanPolicy.Omit"/> drops the pair.</param>
    /// <returns>
    /// The coefficient and its two-sided p-value; both <c>NaN</c> when either side is constant or fewer than two pairs
    /// remain, where scipy answers rather than raises.
    /// </returns>
    /// <exception cref="ArgumentException">The samples differ in length, or a <c>NaN</c> under <see cref="NanPolicy.Raise"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside its enumeration.</exception>
    public static TestResult Test(ReadOnlySpan<bool> x, ReadOnlySpan<double> y, NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        var coded = new double[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            coded[i] = x[i] ? 1.0 : 0.0;
        }

        // Below two pairs Pearson.Test answers NaN itself, after refusing a length mismatch or a raised NaN (decision 0010).
        PearsonResult result = Pearson.Test(coded, y, Alternative.TwoSided, nanPolicy);
        return new TestResult(result.Statistic, result.PValue);
    }
}
