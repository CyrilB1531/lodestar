using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays <c>partial_fit</c> over batches against <c>fit</c> on the concatenation, for the three
/// scalers that have one (#765).
/// </summary>
/// <remarks>
/// Every case carries both answers, because the claim is not that the incremental statistics are
/// right on their own but that they are the ones a single fit gives. The batches are uneven on
/// purpose: equal ones would let an implementation that averages two batch means pass.
/// </remarks>
public sealed class PartialFitOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("preprocessing_partial_fit.json");

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

    [Theory]
    [MemberData(nameof(Indices))]
    public void Every_case_matches_scikit_learn(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        int featureCount = frozen.GetProperty("featureCount").GetInt32();
        double[][] batches =
            [.. frozen.GetProperty("batches").EnumerateArray().Select(b => Doubles(b))];

        switch (frozen.GetProperty("scaler").GetString())
        {
            case "minmax":
                MinMaxScaler minMax = MinMaxScaler.Fit(batches[0], featureCount);
                foreach (double[] batch in batches.Skip(1))
                {
                    minMax = minMax.PartialFit(batch);
                }

                Assert.Equal(frozen.GetProperty("samplesSeen").GetInt32(), minMax.SampleCount);
                AssertSame(Doubles(frozen, "dataMinimum"), minMax.DataMinimum, $"{name}: minimum");
                AssertSame(Doubles(frozen, "dataMaximum"), minMax.DataMaximum, $"{name}: maximum");
                AssertSame(Doubles(frozen, "incrementalScale"), minMax.Scale, $"{name}: scale");
                AssertSame(Doubles(frozen, "wholeScale"), minMax.Scale, $"{name}: against the whole fit");
                break;

            case "maxabs":
                MaxAbsScaler maxAbs = MaxAbsScaler.Fit(batches[0], featureCount);
                foreach (double[] batch in batches.Skip(1))
                {
                    maxAbs = maxAbs.PartialFit(batch);
                }

                Assert.Equal(frozen.GetProperty("samplesSeen").GetInt32(), maxAbs.SampleCount);
                AssertSame(Doubles(frozen, "maximumAbsolute"), maxAbs.MaximumAbsolute, $"{name}: maximum absolute");
                AssertSame(Doubles(frozen, "incrementalScale"), maxAbs.Scale, $"{name}: scale");
                AssertSame(Doubles(frozen, "wholeScale"), maxAbs.Scale, $"{name}: against the whole fit");
                break;

            default:
                StandardScaler standard = StandardScaler.Fit(batches[0], featureCount);
                foreach (double[] batch in batches.Skip(1))
                {
                    standard = standard.PartialFit(batch);
                }

                Assert.Equal(frozen.GetProperty("samplesSeen").GetInt32(), standard.SampleCount);
                AssertSame(Doubles(frozen, "incrementalMean"), standard.Mean!, $"{name}: mean");
                AssertSame(Doubles(frozen, "incrementalVariance"), standard.Variance!, $"{name}: variance");
                AssertSame(Doubles(frozen, "incrementalScale"), standard.Scale!, $"{name}: scale");

                // The property the lot exists for: the same statistics a single fit would give.
                AssertSame(Doubles(frozen, "wholeMean"), standard.Mean!, $"{name}: mean against the whole fit");
                AssertSame(Doubles(frozen, "wholeVariance"), standard.Variance!, $"{name}: variance against the whole fit");
                break;
        }
    }

    [Fact]
    public void Every_frozen_case_is_replayed()
    {
        Assert.Equal(Corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), Cases.Count);
    }

    private static double[] Doubles(JsonElement element) =>
        [.. element.EnumerateArray().Select(v => v.GetDouble())];

    private static double[] Doubles(JsonElement element, string name) =>
        Doubles(element.GetProperty(name));

    private static void AssertSame(double[] expected, IReadOnlyList<double> actual, string what)
    {
        Assert.True(expected.Length == actual.Count, $"{what}: {expected.Length} values expected, {actual.Count} reported");
        for (int i = 0; i < expected.Length; i++)
        {
            double allowed = Math.Max(Tolerance, Math.Abs(expected[i]) * Tolerance);
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= allowed,
                $"{what}[{i}]: expected {expected[i]}, got {actual[i]}");
        }
    }
}
