using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// The Metrics findings left after #1591, one fact or theory each; the expected values were measured on
/// scikit-learn 1.9.1 with numpy 2.5.3.
/// </summary>
public sealed class MetricsRemainingFindingsTests
{
    [Fact]
    public void A_non_monotone_cumulative_weight_is_searched_as_numpy_searches_it()
    {
        // The cumulative weight runs 0, 1, 1, -1, 1, 0.5, -0.5, -2.5, -4.5, -1.5, -1.5 and the halfway point is -0.75:
        // numpy's branchless search lands on index 4, 2.56; a textbook binary search lands on 0, 0.18 (#1546).
        double[] yTrue = [2.13, -1.37, -5.01, -0.52, -0.07, 4, -3.48, 6.42, -1.6, 4.85, 3.88];
        double[] yPred = [0.67, -8.04, 0.39, -0.34, -0.61, 1.44, 1.79, 2.61, -0.28, -0.35, -5.45];
        double[] weights = [-2, 3, -2, 0, 1, 2, -2, -0.5, 0, -1, 0];

        Assert.Equal(2.56, MedianAbsoluteError.Score(yTrue, yPred, sampleWeight: weights), 1e-12);
    }

    [Theory]
    [InlineData(Averaging.Macro, 1)]
    [InlineData(Averaging.Weighted, 1)]
    [InlineData(Averaging.Macro, 4)]
    public void One_vs_one_over_seventeen_classes_averages_its_136_pairs_as_numpy_does(Averaging average, int workers)
    {
        // 136 pair scores take NumpyAverage's halving and eight-lane branches, which nothing reached (#1595). The
        // scores are integers over 1024, exact in both languages; scikit-learn's bits, which an in-order sum misses.
        const int k = 17;
        const int n = 340;
        int[] yTrue = new int[n];
        double[] yScore = new double[n * k];
        for (int i = 0; i < n; i++)
        {
            yTrue[i] = i * 7 % k;
            int used = 0;
            for (int c = 0; c < k - 1; c++)
            {
                int share = ((i * 31) + (c * 17)) % 57 + 1;
                yScore[(i * k) + c] = share / 1024.0;
                used += share;
            }

            yScore[(i * k) + k - 1] = (1024 - used) / 1024.0;
        }

        double score = RocAuc.MultiClass(yTrue, yScore, k, new MultiClassRocOptions
        {
            Strategy = MultiClassStrategy.OneVsOne,
            Average = average,
            MaxDegreeOfParallelism = workers,
        });

        Assert.Equal(0x3FE01D2389F056BD, BitConverter.DoubleToInt64Bits(score));
    }

    [Fact]
    public void D2_absolute_error_reads_a_negative_weight_as_the_reference_does()
    {
        // The median of yTrue in its denominator is the same weighted percentile; the old reading differed here (#1546).
        double score = D2AbsoluteError.Score(
            [0.3, -1.8, -0.4, -6.0, -3.4, 1.1], [-6.4, 2.5, -5.2, 2.3, -2.5, 2.3], sampleWeight: [-1.0, -0.5, -1.0, 2.0, -0.5, 0.5]);

        Assert.Equal(1.2638297872340427, score, 1e-12);
    }

    [Fact]
    public void D2_pinball_reads_a_negative_weight_as_the_reference_does_away_from_the_middle()
    {
        // d2_pinball_score(..., alpha=0.3): its 30th weighted percentile moved with the port too (#1546).
        double score = D2Pinball.Score(
            [-0.8, 2.3, -1.3, -0.1, 1.0, -2.6], [1.8, -0.3, 1.5, -1.6, 3.3, 1.8], 0.3, sampleWeight: [1.0, -1.0, -0.5, -0.5, 2.0, 1.0]);

        Assert.Equal(-4.429203539823007, score, 1e-12);
    }
}
