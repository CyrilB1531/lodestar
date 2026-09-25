using System.Text.Json;
using Xunit;

namespace Lodestar.Conformal.Tests;

/// <summary>
/// Replays MAPIE 1.5.0's <c>CrossConformalRegressor</c>, <c>JackknifeAfterBootstrapRegressor</c> and the gamma score
/// over <c>tests/oracles/conformal_cross.json</c> (#1159).
/// </summary>
/// <remarks>
/// Each case carries every model's predictions and MAPIE's own held-out mask, so the C# is handed exactly what MAPIE
/// computed with. Bounds and scores at <c>1e-9</c> relative: a mean over several models sums in another order.
/// </remarks>
public sealed class CrossConformalOracleTests
{
    private const double Relative = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("conformal_cross.json");

    private static IReadOnlyList<JsonElement> Cross => [.. Corpus.RootElement.GetProperty("cross").EnumerateArray()];

    private static IReadOnlyList<JsonElement> GammaSplit => [.. Corpus.RootElement.GetProperty("gammaSplit").EnumerateArray()];

    public static TheoryData<int> CrossIndices() => Indices(Cross.Count);

    public static TheoryData<int> GammaSplitIndices() => Indices(GammaSplit.Count);

    [Theory]
    [MemberData(nameof(CrossIndices))]
    public void The_out_of_sample_scores_match_the_reference(int index)
    {
        JsonElement c = Cross[index];
        double[] scores = Scores(c);
        JsonElement[] expected = [.. c.GetProperty("scores").EnumerateArray()];

        Assert.Equal(expected.Length, scores.Length);
        for (int i = 0; i < scores.Length; i++)
        {
            if (expected[i].ValueKind == JsonValueKind.Null)
            {
                Assert.True(double.IsNaN(scores[i]), $"{Name(c)}: score {i} should have no held-out model");
            }
            else
            {
                Close(expected[i].GetDouble(), scores[i], $"{Name(c)}: score {i}");
            }
        }
    }

    [Theory]
    [MemberData(nameof(CrossIndices))]
    public void The_intervals_match_the_reference(int index)
    {
        JsonElement c = Cross[index];
        double[] lower = ConformalCorpus.Doubles(c, "lower");
        double[] upper = ConformalCorpus.Doubles(c, "upper");
        for (int t = 0; t < lower.Length; t++)
        {
            (double low, double high) = Interval(c, t, useFolds: false);
            Close(lower[t], low, $"{Name(c)}: lower {t}");
            Close(upper[t], high, $"{Name(c)}: upper {t}");
        }
    }

    /// <summary>K-fold and leave-one-out hold each sample out once; the fold overload must give the mask's interval.</summary>
    [Theory]
    [MemberData(nameof(CrossIndices))]
    public void The_fold_overload_matches_the_reference(int index)
    {
        JsonElement c = Cross[index];
        if (c.GetProperty("kind").GetString() == "jackknife-after-bootstrap" || Method(c) == "base")
        {
            return;
        }

        double[] lower = ConformalCorpus.Doubles(c, "lower");
        double[] upper = ConformalCorpus.Doubles(c, "upper");
        for (int t = 0; t < lower.Length; t++)
        {
            (double low, double high) = Interval(c, t, useFolds: true);
            Close(lower[t], low, $"{Name(c)}: lower {t}");
            Close(upper[t], high, $"{Name(c)}: upper {t}");
        }
    }

    [Theory]
    [MemberData(nameof(GammaSplitIndices))]
    public void The_gamma_split_interval_matches_the_reference(int index)
    {
        JsonElement c = GammaSplit[index];
        double alpha = ConformalCorpus.Alpha(c);
        double[] scores = SplitConformal.GammaScores(ConformalCorpus.Doubles(c, "y"), ConformalCorpus.Doubles(c, "predictions"));
        double[] test = ConformalCorpus.Doubles(c, "testPredictions");
        double[] lower = ConformalCorpus.Doubles(c, "lower");
        double[] upper = ConformalCorpus.Doubles(c, "upper");
        for (int t = 0; t < test.Length; t++)
        {
            (double low, double high) = SplitConformal.GammaInterval(test[t], scores, alpha);
            Close(lower[t], low, $"{Name(c)}: lower {t}");
            Close(upper[t], high, $"{Name(c)}: upper {t}");
        }
    }

    private static (double Lower, double Upper) Interval(JsonElement c, int t, bool useFolds)
    {
        double alpha = ConformalCorpus.Alpha(c);
        int models = c.GetProperty("modelCount").GetInt32();
        bool gamma = c.GetProperty("score").GetString() == "gamma";
        double[] scores = Scores(c);
        if (Method(c) == "base")
        {
            double full = ConformalCorpus.Doubles(c, "fullPredictions")[t];
            return gamma
                ? SplitConformal.GammaInterval(full, scores, alpha)
                : SplitConformal.Interval(full, SplitConformal.Quantile(scores, alpha));
        }

        ReadOnlySpan<double> test = ConformalCorpus.Row(ConformalCorpus.Doubles(c, "testPredictions"), t, models);
        CrossConformalMethod method = Method(c) == "minmax" ? CrossConformalMethod.MinMax : CrossConformalMethod.Plus;
        if (useFolds)
        {
            int[] folds = Folds(HeldOut(c), models);
            return gamma
                ? CrossConformal.GammaInterval(test, folds, scores, alpha, method)
                : CrossConformal.Interval(test, folds, scores, alpha, method);
        }

        bool[] heldOut = HeldOut(c);
        CrossConformalAggregation aggregation = Aggregation(c);
        return gamma
            ? CrossConformal.GammaInterval(test, heldOut, scores, alpha, method, aggregation)
            : CrossConformal.Interval(test, heldOut, scores, alpha, method, aggregation);
    }

    private static double[] Scores(JsonElement c)
    {
        int models = c.GetProperty("modelCount").GetInt32();
        double[] outOfSample = CrossConformal.OutOfSample(
            ConformalCorpus.Doubles(c, "trainPredictions"), HeldOut(c), models, Aggregation(c));
        double[] y = ConformalCorpus.Doubles(c, "y");
        if (c.GetProperty("score").GetString() != "gamma")
        {
            return SplitConformal.AbsoluteResiduals(y, outOfSample);
        }

        // A sample no model holds out has a NaN prediction, which the gamma score refuses; its score stays NaN.
        var scores = new double[y.Length];
        for (int i = 0; i < y.Length; i++)
        {
            scores[i] = double.IsNaN(outOfSample[i]) ? double.NaN : SplitConformal.GammaScores([y[i]], [outOfSample[i]])[0];
        }

        return scores;
    }

    private static int[] Folds(bool[] heldOut, int models)
    {
        var folds = new int[heldOut.Length / models];
        for (int i = 0; i < folds.Length; i++)
        {
            folds[i] = Array.IndexOf(heldOut, true, i * models, models) - (i * models);
        }

        return folds;
    }

    private static bool[] HeldOut(JsonElement c) => [.. c.GetProperty("heldOut").EnumerateArray().Select(v => v.GetBoolean())];

    private static CrossConformalAggregation Aggregation(JsonElement c) =>
        c.GetProperty("aggregation").GetString() == "median" ? CrossConformalAggregation.Median : CrossConformalAggregation.Mean;

    private static string Method(JsonElement c) => c.GetProperty("method").GetString()!;

    private static string Name(JsonElement c) => c.GetProperty("name").GetString()!;

    private static void Close(double expected, double actual, string what) =>
        Assert.True(
            Math.Abs(actual - expected) <= (Relative * Math.Abs(expected)) + 1e-12,
            $"{what}: {actual:R} against {expected:R}");

    private static TheoryData<int> Indices(int count)
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < count; i++)
        {
            data.Add(i);
        }

        return data;
    }
}
