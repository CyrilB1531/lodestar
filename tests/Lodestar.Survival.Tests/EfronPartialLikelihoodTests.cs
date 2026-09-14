using System.Text.Json;
using Lodestar.Survival.Internal;
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>
/// The Efron partial likelihood, its score and its observed information, against the Cox corpus
/// frozen from <c>lifelines</c> 0.30.3.
/// </summary>
/// <remarks>
/// Evaluated at the frozen coefficients, so these check the three quantities without any Newton
/// iteration in between: the log-likelihood and the null log-likelihood directly, the score by
/// vanishing at the maximum, and the information by inverting to the frozen standard errors.
/// The corpus holds the maximum to within 3.1e-13 of an independent Newton-Raphson.
/// </remarks>
public sealed class EfronPartialLikelihoodTests
{
    private const double Tolerance = 1e-9;

    /// <summary>
    /// The score at the frozen maximum: the information times a coefficient error below 1.4e-13,
    /// measured at 5e-15 on the reference and 4.6e-12 at worst on lifelines' frozen β.
    /// </summary>
    private const double ScoreTolerance = 1e-10;

    private static readonly JsonDocument Corpus = OracleLoader.Load("survival_cox.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(value => value.GetDouble())];

    private static (EfronPartialLikelihood Likelihood, int FeatureCount) Build(JsonElement fixture)
    {
        int featureCount = fixture.GetProperty("featureCount").GetInt32();
        bool[] events =
            [.. fixture.GetProperty("eventObserved").EnumerateArray().Select(value => value.GetInt32() == 1)];
        return (new EfronPartialLikelihood(
            Doubles(fixture, "design"), Doubles(fixture, "durations"), events, featureCount),
            featureCount);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_log_likelihood_at_the_frozen_coefficients_matches_lifelines(int index)
    {
        JsonElement fixture = Cases[index];
        (EfronPartialLikelihood likelihood, int p) = Build(fixture);

        double logLikelihood = likelihood.Evaluate(
            Doubles(fixture, "coefficients"), new double[p], new double[p * p]);

        Assert.Equal(fixture.GetProperty("logLikelihood").GetDouble(), logLikelihood, Tolerance);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_log_likelihood_at_zero_is_the_null_log_likelihood(int index)
    {
        JsonElement fixture = Cases[index];
        (EfronPartialLikelihood likelihood, int p) = Build(fixture);

        double logLikelihood = likelihood.Evaluate(new double[p], new double[p], new double[p * p]);

        Assert.Equal(fixture.GetProperty("nullLogLikelihood").GetDouble(), logLikelihood, Tolerance);
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_score_vanishes_at_the_frozen_maximum(int index)
    {
        JsonElement fixture = Cases[index];
        (EfronPartialLikelihood likelihood, int p) = Build(fixture);
        double[] score = new double[p];

        likelihood.Evaluate(Doubles(fixture, "coefficients"), score, new double[p * p]);

        foreach (double component in score)
        {
            Assert.InRange(component, -ScoreTolerance, ScoreTolerance);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_inverse_information_gives_the_frozen_standard_errors(int index)
    {
        JsonElement fixture = Cases[index];
        (EfronPartialLikelihood likelihood, int p) = Build(fixture);
        double[] information = new double[p * p];

        likelihood.Evaluate(Doubles(fixture, "coefficients"), new double[p], information);

        Assert.True(Cholesky.TryFactor(information, p, out double[] lower));
        double[] covariance = Cholesky.Inverse(lower, p);
        double[] expected = Doubles(fixture, "standardErrors");
        for (int j = 0; j < p; j++)
        {
            double standardError = Math.Sqrt(covariance[(j * p) + j]);
            Assert.True(
                Math.Abs(standardError - expected[j]) <= Tolerance * Math.Abs(expected[j]),
                $"standardErrors[{j}]: expected {expected[j]:R}, got {standardError:R}");
        }
    }

    [Fact]
    public void The_information_is_symmetric()
    {
        (EfronPartialLikelihood likelihood, int p) = Build(Cases[^1]);
        double[] information = new double[p * p];

        likelihood.Evaluate(new double[p], new double[p], information);

        for (int a = 0; a < p; a++)
        {
            for (int b = a + 1; b < p; b++)
            {
                Assert.Equal(information[(a * p) + b], information[(b * p) + a]);
            }
        }
    }

    [Fact]
    public void Order_of_the_subjects_does_not_change_the_answer()
    {
        // Two events and a censoring tied at 2: the tied sums and the risk set depend on the
        // grouping, not on the order the rows arrive in.
        double[] design = [0.5, -1.0, 1.5, 0.0, 2.0];
        double[] durations = [2.0, 1.0, 2.0, 3.0, 2.0];
        bool[] events = [true, true, false, true, true];
        int[] reversed = [4, 3, 2, 1, 0];

        double forward = new EfronPartialLikelihood(design, durations, events, 1)
            .Evaluate([0.3], new double[1], new double[1]);
        double backward = new EfronPartialLikelihood(
                [.. reversed.Select(i => design[i])],
                [.. reversed.Select(i => durations[i])],
                [.. reversed.Select(i => events[i])],
                1)
            .Evaluate([0.3], new double[1], new double[1]);

        Assert.Equal(forward, backward, 1e-14);
    }
}
