using System.Text.Json;
using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// Macro and weighted averages over a class whose score is undefined, replayed from
/// scikit-learn's <c>_nanaverage</c> under all three of its zero-division modes (#861),
/// and from <c>jaccard_score</c>'s own <c>numpy.average</c> where the two part (#988).
/// </summary>
public sealed class UndefinedAverageTests
{
    /// <summary>What the corpus holds where the reference raises rather than scoring.</summary>
    private const string Refusal = "ZeroDivisionError";

    [Theory]
    [MemberData(nameof(MetricsCorpus.UndefinedAverageIndices), MemberType = typeof(MetricsCorpus))]
    public void Scores_match_sklearn(int index)
    {
        JsonElement c = MetricsCorpus.UndefinedAverages[index];
        int[] yTrue = MetricsCorpus.Ints(c, "y_true");
        int[] yPred = MetricsCorpus.Ints(c, "y_pred");
        int[] labels = MetricsCorpus.OptionalInts(c, "labels");
        double[] sampleWeight = MetricsCorpus.OptionalDoubles(c, "sample_weight");
        ConfusionMatrix? cm = MatrixOrRefusal(yTrue, yPred, labels, sampleWeight);

        foreach (JsonProperty entry in c.GetProperty("scores").EnumerateObject())
        {
            string[] parts = entry.Name.Split('|');
            Averaging average = parts[0] == "macro" ? Averaging.Macro : Averaging.Weighted;
            ZeroDivision zero = ParseZeroDivision(parts[1]);
            string what = $"{c.GetProperty("fixture").GetString()} {entry.Name}";
            JsonElement want = entry.Value;

            AssertClose(want, "precision",
                Precision.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            AssertClose(want, "recall",
                Recall.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            AssertClose(want, "f1",
                F1.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            AssertClose(want, "fbeta_0.5",
                FBeta.Score(yTrue, yPred, 0.5, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            AssertClose(want, "fbeta_2.0",
                FBeta.Score(yTrue, yPred, 2.0, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                what);
            if (cm is not null)
            {
                AssertClose(want, "precision", Precision.Score(cm, average, zeroDivision: zero), what);
                AssertClose(want, "recall", Recall.Score(cm, average, zeroDivision: zero), what);
                AssertClose(want, "f1", F1.Score(cm, average, zeroDivision: zero), what);
                AssertClose(want, "fbeta_0.5", FBeta.Score(cm, 0.5, average, zeroDivision: zero), what);
                AssertClose(want, "fbeta_2.0", FBeta.Score(cm, 2.0, average, zeroDivision: zero), what);
            }

            // jaccard_score refuses zero_division=nan, so that mode has no reference
            // value; a weighted average whose supports cancel has a refusal instead (#988).
            JsonElement jaccard = want.GetProperty("jaccard");
            if (jaccard.ValueKind == JsonValueKind.String && jaccard.GetString() == Refusal)
            {
                ArgumentException error = Assert.Throws<ArgumentException>(() =>
                    JaccardScore.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight));
                Assert.StartsWith("Weights sum to zero", error.Message, StringComparison.Ordinal);
                Assert.Equal("sampleWeight", error.ParamName);
            }
            else if (jaccard.ValueKind != JsonValueKind.Null)
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
        int[] yTrue = MetricsCorpus.Ints(c, "y_true");
        int[] yPred = MetricsCorpus.Ints(c, "y_pred");
        int[] labels = MetricsCorpus.OptionalInts(c, "labels");
        double[] sampleWeight = MetricsCorpus.OptionalDoubles(c, "sample_weight");
        ConfusionMatrix? cm = MatrixOrRefusal(yTrue, yPred, labels, sampleWeight);

        foreach (JsonProperty entry in c.GetProperty("report").EnumerateObject())
        {
            ZeroDivision zero = ParseZeroDivision(entry.Name);
            string what = $"{c.GetProperty("fixture").GetString()} {entry.Name}";
            ClassificationReport fromSamples = ClassificationReport.Compute(
                yTrue, yPred, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight);
            AssertRow(entry.Value.GetProperty("macro avg"), fromSamples.MacroAverage, what);
            AssertRow(entry.Value.GetProperty("weighted avg"), fromSamples.WeightedAverage, what);

            if (cm is not null)
            {
                ClassificationReport report = ClassificationReport.Compute(cm, zeroDivision: zero);
                AssertRow(entry.Value.GetProperty("macro avg"), report.MacroAverage, what);
                AssertRow(entry.Value.GetProperty("weighted avg"), report.WeightedAverage, what);
            }
        }
    }

    /// <summary>The matrix, or <see langword="null"/> where <c>confusion_matrix</c> refuses labels absent from y_true.</summary>
    /// <remarks>Only the matrix refuses them: the scores and the report read the samples (#1202).</remarks>
    private static ConfusionMatrix? MatrixOrRefusal(int[] yTrue, int[] yPred, int[] labels, double[] sampleWeight)
    {
        if (labels.Length == 0 || labels.Any(yTrue.Contains))
        {
            return ConfusionMatrix.Compute(yTrue, yPred, labels, sampleWeight);
        }

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => ConfusionMatrix.Compute(yTrue, yPred, labels, sampleWeight));
        Assert.StartsWith("At least one supplied label must occur in yTrue", error.Message, StringComparison.Ordinal);
        return null;
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
