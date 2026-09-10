namespace Lodestar.Text.Similarity;

/// <summary>How a MinHash signature is cut into bands for locality-sensitive hashing.</summary>
/// <param name="Bands">How many bands, <c>b</c>.</param>
/// <param name="RowsPerBand">How many signature slots each band holds, <c>r</c>.</param>
/// <remarks>
/// <strong><c>b</c> and <c>r</c> are inputs, and the solve that picks them is a function you
/// can call.</strong> The reference hides the same solve inside a constructor, which is what
/// makes the trade-off hard to see: the threshold does not select a banding on its own —
/// it selects one given how much a caller minds a false positive against a false negative.
/// </remarks>
public readonly record struct LshBanding(int Bands, int RowsPerBand)
{
    /// <summary>The signature length this banding consumes, <c>b × r</c>.</summary>
    public int Permutations => Bands * RowsPerBand;

    /// <summary>The chance two sets of this similarity land in the same band.</summary>
    /// <param name="jaccard">Their Jaccard similarity, in <c>[0, 1]</c>.</param>
    /// <returns><c>1 - (1 - s^r)^b</c>, the S-curve this banding produces.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="jaccard"/> is outside <c>[0, 1]</c>.</exception>
    public double CollisionProbability(double jaccard)
    {
        if (jaccard < 0.0 || jaccard > 1.0 || double.IsNaN(jaccard))
        {
            throw new ArgumentOutOfRangeException(
                nameof(jaccard), jaccard, "A Jaccard similarity lies in [0, 1].");
        }

        return 1.0 - Math.Pow(1.0 - Math.Pow(jaccard, RowsPerBand), Bands);
    }

    /// <summary>The banding that minimizes the weighted error at a threshold.</summary>
    /// <param name="threshold">The similarity a caller wants found, in <c>(0, 1)</c>.</param>
    /// <param name="permutations">The signature length available, <c>b × r</c> at most.</param>
    /// <param name="falsePositiveWeight">How much a false positive costs, non-negative.</param>
    /// <param name="falseNegativeWeight">How much a false negative costs, non-negative.</param>
    /// <returns>The <c>(b, r)</c> pair with the lowest weighted error.</returns>
    /// <exception cref="ArgumentOutOfRangeException">An argument is outside its range.</exception>
    /// <remarks>
    /// The two errors are the areas under the S-curve on the wrong side of the threshold, as
    /// the published LSH analysis states them. Integrated by the midpoint rule at a step of
    /// <c>0.001</c>: measured, that reproduces the reference's own choice on all fifteen
    /// threshold and length pairs the corpus freezes, and the answer is a pair of integers,
    /// so a small difference in the quadrature does not move it.
    /// </remarks>
    public static LshBanding Solve(
        double threshold, int permutations,
        double falsePositiveWeight = 0.5, double falseNegativeWeight = 0.5)
    {
        if (threshold <= 0.0 || threshold >= 1.0 || double.IsNaN(threshold))
        {
            throw new ArgumentOutOfRangeException(
                nameof(threshold), threshold, "A threshold lies strictly inside (0, 1).");
        }

        Guard.NotLessThan(permutations, 1);
        RequireWeight(falsePositiveWeight, nameof(falsePositiveWeight));
        RequireWeight(falseNegativeWeight, nameof(falseNegativeWeight));

        double best = double.PositiveInfinity;
        var chosen = new LshBanding(1, permutations);
        for (int bands = 1; bands <= permutations; bands++)
        {
            int widest = permutations / bands;
            for (int rows = 1; rows <= widest; rows++)
            {
                var candidate = new LshBanding(bands, rows);
                double error = (falsePositiveWeight * candidate.FalsePositiveArea(threshold))
                    + (falseNegativeWeight * candidate.FalseNegativeArea(threshold));
                if (error < best)
                {
                    best = error;
                    chosen = candidate;
                }
            }
        }

        return chosen;
    }

    /// <summary>The area under the S-curve below the threshold: pairs found that should not be.</summary>
    /// <remarks>
    /// The banding is copied into a local because a lambda in a struct cannot close over
    /// <c>this</c>, which is a C# rule rather than a design choice here.
    /// </remarks>
    private double FalsePositiveArea(double threshold)
    {
        LshBanding banding = this;
        return Integrate(banding.CollisionProbability, 0.0, threshold);
    }

    /// <summary>The area above it that the curve misses: pairs that should be found and are not.</summary>
    private double FalseNegativeArea(double threshold)
    {
        LshBanding banding = this;
        return Integrate(similarity => 1.0 - banding.CollisionProbability(similarity), threshold, 1.0);
    }

    /// <summary>The midpoint rule at a step of 0.001, which is what the reference's choice needs.</summary>
    private static double Integrate(Func<double, double> curve, double from, double to)
    {
        const double step = 0.001;
        double total = 0.0;
        for (double at = from; at < to; at += step)
        {
            total += curve(Math.Min(at + (step / 2.0), 1.0)) * step;
        }

        return total;
    }

    private static void RequireWeight(double weight, string name)
    {
        if (weight < 0.0 || double.IsNaN(weight))
        {
            throw new ArgumentOutOfRangeException(name, weight, "An error weight is not negative.");
        }
    }
}
