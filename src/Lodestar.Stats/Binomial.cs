using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The binomial test: is this proportion of successes consistent with a stated one?</summary>
/// <remarks>
/// The exact counterpart of what <see cref="ChiSquare.GoodnessOfFit"/> answers asymptotically
/// over two categories. Exact means the p-value is a sum of binomial probabilities rather than a
/// chi-squared approximation of one, so it is right at any number of trials — which is the whole
/// point, because the approximation is worst exactly where a proportion is most often tested:
/// few trials, or a probability near zero or one.
/// </remarks>
public static class Binomial
{
    // Two outcomes differing only in the last bits are equally extreme here, and a bare <=
    // would include or exclude one by rounding. FisherExact guards its own sum the same way.
    private const double ProbabilityTolerance = 1e-7;

    /// <summary>Tests a count of successes against a stated probability.</summary>
    /// <param name="successes">The number of successes; between zero and <paramref name="trials"/>.</param>
    /// <param name="trials">The number of trials; at least one.</param>
    /// <param name="probability">The probability of success under the null, in <c>[0, 1]</c>.</param>
    /// <param name="alternative">Which tail the p-value covers.</param>
    /// <returns>
    /// The observed proportion, the p-value, and a
    /// <see cref="BinomialResult.ProportionConfidenceInterval"/> the result can be asked for.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="trials"/> is not positive, <paramref name="successes"/> lies outside
    /// <c>[0, trials]</c>, or <paramref name="probability"/> lies outside <c>[0, 1]</c>.
    /// </exception>
    public static BinomialResult Test(
        int successes,
        int trials,
        double probability = 0.5,
        Alternative alternative = Alternative.TwoSided)
    {
        if (trials < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(trials), trials, "The number of trials must be at least one.");
        }
        if (successes < 0 || successes > trials)
        {
            throw new ArgumentOutOfRangeException(
                nameof(successes), successes, $"The successes must lie in [0, {trials}].");
        }
        if (double.IsNaN(probability) || probability < 0.0 || probability > 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(probability), probability, "The probability must lie in [0, 1].");
        }

        double pValue = alternative switch
        {
            Alternative.Less => BinomialTail.Cumulative(successes, trials, probability),
            Alternative.Greater => BinomialTail.Survival(successes, trials, probability),
            Alternative.TwoSided => TwoSided(successes, trials, probability),
            _ => throw new ArgumentOutOfRangeException(nameof(alternative), alternative, null),
        };

        return new BinomialResult((double)successes / trials, pValue)
        {
            Successes = successes,
            Trials = trials,
            Alternative = alternative,
        };
    }

    /// <summary>Every outcome no more likely than the observed one, from both sides.</summary>
    /// <remarks>
    /// The far side's boundary is found by binary search rather than by summing: the mass is
    /// unimodal, so the outcomes at least as extreme form one run from each end, and the search
    /// only has to find where that run starts. scipy's own contract for it is the index <c>i</c>
    /// with <c>mass(i) &lt;= d &lt; mass(i+1)</c>.
    /// </remarks>
    private static double TwoSided(int successes, int trials, double probability)
    {
        double observed = BinomialTail.Mass(successes, trials, probability) * (1.0 + ProbabilityTolerance);
        double mode = probability * trials;

        double pValue;
        if (successes < mode)
        {
            int index = SearchDescending(observed, trials, probability, (int)Math.Ceiling(mode), trials);

            // S1244: the tie the tolerance above was added to catch -- an outcome exactly as
            // likely as the observed one belongs to the sum, and only exact equality says so.
#pragma warning disable S1244
            int count = trials - index + (observed == BinomialTail.Mass(index, trials, probability) ? 1 : 0);
#pragma warning restore S1244
            pValue = BinomialTail.Cumulative(successes, trials, probability)
                + BinomialTail.Survival(trials - count + 1, trials, probability);
        }
        else
        {
            int index = SearchAscending(observed, trials, probability, 0, (int)Math.Floor(mode));
            pValue = BinomialTail.Cumulative(index, trials, probability)
                + BinomialTail.Survival(successes, trials, probability);
        }

        return Math.Min(1.0, pValue);
    }

    /// <summary>The first index in the falling half whose mass has dropped to <paramref name="bound"/>.</summary>
    private static int SearchDescending(double bound, int n, double p, int low, int high)
        => Search(bound, n, p, low, high, ascending: false);

    /// <summary>The last index in the rising half whose mass is still at most <paramref name="bound"/>.</summary>
    private static int SearchAscending(double bound, int n, double p, int low, int high)
        => Search(bound, n, p, low, high, ascending: true);

    private static int Search(double bound, int n, double p, int low, int high, bool ascending)
    {
        double Value(int i) => ascending
            ? BinomialTail.Mass(i, n, p)
            : -BinomialTail.Mass(i, n, p);
        double target = ascending ? bound : -bound;

        while (low < high)
        {
            int middle = low + ((high - low) / 2);
            double value = Value(middle);
            if (value < target)
            {
                low = middle + 1;
            }
            else if (value > target)
            {
                high = middle - 1;
            }
            else
            {
                return middle;
            }
        }

        return Value(low) <= target ? low : low - 1;
    }
}
