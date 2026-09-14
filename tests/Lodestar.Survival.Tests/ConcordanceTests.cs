using Lodestar.Survival.Internal;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>Harrell's concordance on pairs counted by hand, where the corpus has no exact tie.</summary>
/// <remarks>
/// lifelines computes its partial hazard with row-dependent rounding, so two subjects with identical
/// covariates can compare strictly there and count a whole pair; measured on a ten-subject design it
/// reported 33/42 where the tie rule gives 32.5/42. The rule is what is asserted here.
/// </remarks>
public sealed class ConcordanceTests
{
    [Fact]
    public void Identical_predictors_count_one_half()
    {
        // Pairs (0,1) tie, (0,2) and (1,2) are concordant: 2.5 of 3.
        double c = Concordance.Harrell([2.0, 2.0, 0.0], [1.0, 2.0, 3.0], [true, true, true], [1.0]);

        Assert.Equal(2.5 / 3.0, c, 1e-15);
    }

    [Fact]
    public void A_censoring_at_an_event_time_is_comparable_and_two_events_there_are_not()
    {
        // At time 2: an event (subject 0), an event (1) and a censoring (2). Comparable pairs are
        // (0,2), (1,2) and both events against subject 3 at time 5; (0,1) is not comparable.
        double c = Concordance.Harrell(
            [3.0, 1.0, 2.0, 0.0], [2.0, 2.0, 2.0, 5.0], [true, true, false, true], [1.0]);

        // (0,2): 3 > 2 concordant; (1,2): 1 < 2 discordant; (0,3) and (1,3) concordant.
        Assert.Equal(3.0 / 4.0, c, 1e-15);
    }

    /// <summary>
    /// The Fenwick walk against the pairwise definition, on draws with ties in both the durations
    /// and the predictor: small integer ranges make both kinds of tie common.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void The_walk_agrees_with_every_pair_counted_directly(int seed)
    {
        // SonarLint S2245, CA5394: a seeded Random draws reproducible test data; no security use.
#pragma warning disable S2245, CA5394
        Random random = new(seed);
        int count = 60 + random.Next(80);
        double[] design = new double[count];
        double[] durations = new double[count];
        bool[] events = new bool[count];
        for (int i = 0; i < count; i++)
        {
            design[i] = random.Next(6);
            durations[i] = random.Next(1, 15);
            events[i] = random.Next(3) != 0;
        }
#pragma warning restore S2245, CA5394

        double expected = Pairwise(design, durations, events);

        Assert.Equal(expected, Concordance.Harrell(design, durations, events, [0.7]), 1e-15);
    }

    // S1244: the definition being restated compares durations and predictors exactly; a range
    // would count a pair Harrell does not.
#pragma warning disable S1244
    private static double Pairwise(double[] eta, double[] durations, bool[] events)
    {
        double pairs = 0.0;
        double credit = 0.0;
        for (int i = 0; i < eta.Length; i++)
        {
            for (int k = 0; k < eta.Length; k++)
            {
                bool comparable = events[i]
                    && (durations[k] > durations[i] || (durations[k] == durations[i] && !events[k]));
                if (!comparable)
                {
                    continue;
                }

                pairs++;
                if (eta[i] == eta[k])
                {
                    credit += 0.5;
                }
                else if (eta[i] > eta[k])
                {
                    credit += 1.0;
                }
            }
        }

        return credit / pairs;
    }
#pragma warning restore S1244

    [Fact]
    public void A_higher_risk_that_fails_later_is_discordant()
    {
        double c = Concordance.Harrell([0.0, 1.0], [1.0, 2.0], [true, true], [1.0]);

        Assert.Equal(0.0, c);
    }
}
