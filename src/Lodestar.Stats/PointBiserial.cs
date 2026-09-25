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
    public static TestResult Test(ReadOnlySpan<bool> x, ReadOnlySpan<double> y, NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        var coded = new double[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            coded[i] = x[i] ? 1.0 : 0.0;
        }

        int remaining = 0;
        foreach (double value in y)
        {
            remaining += nanPolicy == NanPolicy.Omit && double.IsNaN(value) ? 0 : 1;
        }

        // scipy's wrapper answers (nan, nan) below two pairs before pearsonr could raise; a length mismatch or a
        // refused NaN still reaches Pearson.Test, which raises as the reference does.
        if (remaining < 2 && x.Length == y.Length && (nanPolicy != NanPolicy.Raise || !Internal.Ranks.HasNaN(y)))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        PearsonResult result = Pearson.Test(coded, y, Alternative.TwoSided, nanPolicy);
        return new TestResult(result.Statistic, result.PValue);
    }
}
