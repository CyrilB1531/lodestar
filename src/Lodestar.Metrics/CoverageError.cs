using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// How far down the ranking you must read to have seen every relevant label — the
/// equivalent of <c>sklearn.metrics.coverage_error</c>.
/// </summary>
public static class CoverageError
{
    /// <summary>Scores a boolean label matrix — <c>sklearn.metrics.coverage_error(y_true, y_score, sample_weight=…)</c>.</summary>
    /// <param name="yTrue">Whether each label is relevant, row-major: one row per sample, <paramref name="labelCount"/> values each.</param>
    /// <param name="yScore">The scores the ranking was made from, same shape as <paramref name="yTrue"/>.</param>
    /// <param name="labelCount">How many labels each row holds.</param>
    /// <param name="sampleWeight">One weight per sample, or empty for an unweighted mean.</param>
    /// <returns>The mean position of the worst-ranked relevant label. <c>1</c> is the best a row can do; a row with no relevant label contributes <c>0</c>, so the mean can sit below <c>1</c>.</returns>
    /// <exception cref="ArgumentException">The shapes disagree, <paramref name="labelCount"/> is <c>1</c>, or <paramref name="sampleWeight"/> sums to zero.</exception>
    public static double Score(
        ReadOnlySpan<bool> yTrue,
        ReadOnlySpan<double> yScore,
        int labelCount,
        ReadOnlySpan<double> sampleWeight = default)
    {
        LabelRanking.Validate(yTrue, yScore, labelCount, sampleWeight, singleLabelAllowed: false);

        int rows = yTrue.Length / labelCount;
        double[] perRow = new double[rows];
        int[] ranks = new int[labelCount];
        double[] sorted = new double[labelCount];
        for (int row = 0; row < rows; row++)
        {
            ReadOnlySpan<bool> relevant = yTrue.Slice(row * labelCount, labelCount);
            ReadOnlySpan<double> scores = yScore.Slice(row * labelCount, labelCount);
            perRow[row] = WorstRank(relevant, scores, ranks, sorted);
        }

        return LabelRanking.Weighted(perRow, sampleWeight);
    }

    /// <summary>The rank of the worst-ranked relevant label, or 0 when none is relevant.</summary>
    /// <remarks>
    /// A max rank is the count of scores at or above a label's own, so the worst relevant rank is
    /// that count at the lowest relevant score, with no sort. A NaN answers no comparison the
    /// way a sorted binary search does, so a row holding one keeps the ranked path.
    /// </remarks>
    private static int WorstRank(ReadOnlySpan<bool> relevant, ReadOnlySpan<double> scores, int[] ranks, double[] sorted)
    {
        bool anyRelevant = false;
        bool anyNaN = false;
        double lowest = double.PositiveInfinity;
        for (int label = 0; label < scores.Length; label++)
        {
            double score = scores[label];
            anyNaN |= double.IsNaN(score);
            if (relevant[label] && (!anyRelevant || score < lowest))
            {
                lowest = score;
                anyRelevant = true;
            }
        }

        if (!anyRelevant)
        {
            return 0;
        }

        if (anyNaN)
        {
            return RankedWorst(relevant, scores, ranks, sorted);
        }

        int atOrAbove = 0;
        foreach (double score in scores)
        {
            atOrAbove += score >= lowest ? 1 : 0;
        }

        return atOrAbove;
    }

    private static int RankedWorst(ReadOnlySpan<bool> relevant, ReadOnlySpan<double> scores, int[] ranks, double[] sorted)
    {
        LabelRanking.MaxRank(scores, ranks, sorted);
        int worst = 0;
        for (int label = 0; label < scores.Length; label++)
        {
            if (relevant[label] && ranks[label] > worst)
            {
                worst = ranks[label];
            }
        }

        return worst;
    }
}
