using System.Text.Json;
using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// Macro and weighted averages over a class whose score is undefined, replayed from
/// scikit-learn's <c>_nanaverage</c> under all three of its zero-division modes (#861).
/// </summary>
public sealed class UndefinedAverageTests
{
    [Theory]
    [MemberData(nameof(MetricsCorpus.UndefinedAverageIndices), MemberType = typeof(MetricsCorpus))]
    public void Scores_match_sklearn(int index)
    {
        JsonElement c = MetricsCorpus.UndefinedAverages[index];
        int[] yTrue = MetricsCorpus.Ints(c, "y_true");
        int[] yPred = MetricsCorpus.Ints(c, "y_pred");
        int[] labels = MetricsCorpus.OptionalInts(c, "labels");
        double[] sampleWeight = MetricsCorpus.OptionalDoubles(c, "sample_weight");
        ConfusionMatrix cm = ConfusionMatrix.Compute(yTrue, yPred, labels, sampleWeight);

        foreach (JsonProperty entry in c.GetProperty("scores").EnumerateObject())
        {
            string[] parts = entry.Name.Split('|');
            Averaging average = parts[0] == "macro" ? Averaging.Macro : Averaging.Weighted;
            ZeroDivision zero = ParseZeroDivision(parts[1]);
            string what = $"{c.GetProperty("fixture").GetString()} {entry.Name}";
            JsonElement want = entry.Value;

            AssertClose(want, "precision", Precision.Score(cm, average, zeroDivision: zero), what);
            AssertClose(want, "precision",
                Precision.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            AssertClose(want, "recall", Recall.Score(cm, average, zeroDivision: zero), what);
            AssertClose(want, "f1", F1.Score(cm, average, zeroDivision: zero), what);
            AssertClose(want, "fbeta_0.5", FBeta.Score(cm, 0.5, average, zeroDivision: zero), what);
            AssertClose(want, "fbeta_2.0", FBeta.Score(cm, 2.0, average, zeroDivision: zero), what);

            // jaccard_score refuses zero_division=nan, so that mode has no reference value.
            if (want.GetProperty("jaccard").ValueKind != JsonValueKind.Null)
            {
                AssertClose(want, "jaccard",
                    JaccardScore.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                    what);
            }
        }
    }

    [Theory]
    [MemberData(nameof(MetricsCorpus.UndefinedAverageIndices), MemberType = typeof(MetricsCorpus))]
    public void Report_average_rows_match_sklearn(int index)
    {
        JsonElement c = MetricsCorpus.UndefinedAverages[index];
        ConfusionMatrix cm = MetricsCorpus.Matrix(c);

        foreach (JsonProperty entry in c.GetProperty("report").EnumerateObject())
        {
            ClassificationReport report = ClassificationReport.Compute(cm, zeroDivision: ParseZeroDivision(entry.Name));
            string what = $"{c.GetProperty("fixture").GetString()} {entry.Name}";

            AssertRow(entry.Value.GetProperty("macro avg"), report.MacroAverage, what);
            AssertRow(entry.Value.GetProperty("weighted avg"), report.WeightedAverage, what);
        }
    }

    [Fact]
    public void Jaccard_under_NaN_averages_only_the_defined_classes()
    {
        int[] yTrue = [0, 1, 1];
        int[] yPred = [0, 1, 0];
        int[] labels = [0, 1, 2];

        double[] perClass = JaccardScore.PerClass(yTrue, yPred, ZeroDivision.NaN, labels);
        double macro = JaccardScore.Score(yTrue, yPred, Averaging.Macro, zeroDivision: ZeroDivision.NaN, labels: labels);

        Assert.True(double.IsNaN(perClass[2]));
        Assert.Equal((perClass[0] + perClass[1]) / 2.0, macro, MetricsCorpus.Tolerance);
    }

    private static void AssertRow(JsonElement want, AverageRow actual, string what)
    {
        AssertClose(want, "precision", actual.Precision, $"{what} {actual.Name}");
        AssertClose(want, "recall", actual.Recall, $"{what} {actual.Name}");
        AssertClose(want, "f1-score", actual.F1, $"{what} {actual.Name}");
        AssertClose(want, "support", actual.Support, $"{what} {actual.Name}");
    }

    private static void AssertClose(JsonElement want, string name, double actual, string what)
    {
        double expected = OracleLoader.Number(want.GetProperty(name));
        bool same = double.IsNaN(expected)
            ? double.IsNaN(actual)
            : Math.Abs(expected - actual) < MetricsCorpus.Tolerance;
        Assert.True(same, $"{what}: {name} expected {expected}, got {actual}");
    }

    private static ZeroDivision ParseZeroDivision(string name) => name switch
    {
        "0" => ZeroDivision.Zero,
        "1" => ZeroDivision.One,
        "nan" => ZeroDivision.NaN,
        _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Unknown zero_division in the corpus."),
    };
}
