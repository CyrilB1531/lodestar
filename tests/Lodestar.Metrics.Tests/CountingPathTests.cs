using Lodestar.Metrics.Internal;
using Xunit;

// SonarLint S2245 / CA5394: a seeded Random draws reproducible inputs for
// path-against-path comparisons; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394

namespace Lodestar.Metrics.Tests;

/// <summary>
/// Each shortcut a metric takes on its common input, held bit for bit against the general path it
/// stands in for, on inputs that reach both.
/// </summary>
public sealed class CountingPathTests
{
    private const int Rows = 60;
    private const int LabelCount = 5;

    [Theory]
    [InlineData(false, 0)]
    [InlineData(true, 0)]
    [InlineData(false, 2)]
    [InlineData(true, 2)]
    public void An_unweighted_matrix_is_the_matrix_weighted_by_ones(bool sparseLabels, int extraLabels)
    {
        var rng = new Random(31);
        int[] vocabulary = [.. Enumerable.Range(0, 6).Select(c => sparseLabels ? (c * 100_000_000) - 3 : c)];
        int[] yTrue = [.. Enumerable.Range(0, 500).Select(_ => vocabulary[rng.Next(vocabulary.Length)])];
        int[] yPred = [.. yTrue.Select(label => rng.Next(3) == 0 ? vocabulary[rng.Next(vocabulary.Length)] : label)];
        double[] ones = [.. yTrue.Select(_ => 1.0)];

        // A request that leaves some observed labels out, so the kept total and the supports differ from n.
        int[] labels = extraLabels == 0 ? [] : [vocabulary[1], vocabulary[4], 17];

        ConfusionMatrix counted = ConfusionMatrix.Compute(yTrue, yPred, labels);
        ConfusionMatrix weighted = ConfusionMatrix.Compute(yTrue, yPred, labels, ones);

        Assert.Equal(weighted.ToArray(), counted.ToArray());
        Assert.Equal(weighted.TotalWeight, counted.TotalWeight);
        Assert.Equal(weighted.TrueSum.ToArray(), counted.TrueSum.ToArray());
        Assert.Equal(weighted.NoSampleCorrect, counted.NoSampleCorrect);
        Assert.Equal(weighted.DroppedSamples, counted.DroppedSamples);
    }

    [Fact]
    public void Unweighted_accuracy_counts_what_a_per_sample_comparison_counts_at_every_length()
    {
        var rng = new Random(37);
        for (int length = 1; length <= 70; length++)
        {
            int[] yTrue = [.. Enumerable.Range(0, length).Select(_ => rng.Next(3))];
            int[] yPred = [.. Enumerable.Range(0, length).Select(_ => rng.Next(3))];
            int matches = yTrue.Where((label, i) => label == yPred[i]).Count();

            Assert.Equal((double)matches, Accuracy.Score(yTrue, yPred, normalize: false));
            Assert.Equal(matches / (double)length, Accuracy.Score(yTrue, yPred));
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void A_per_label_stack_is_a_confusion_matrix_per_binarised_column(bool weighted)
    {
        (bool[] yTrue, bool[] yPred, double[] weights) = LabelMatrix(weighted);

        ConfusionMatrix[] perLabel = MultilabelConfusionMatrix.Compute(yTrue, yPred, LabelCount, false, weights);
        for (int label = 0; label < LabelCount; label++)
        {
            int[] binaryTrue = [.. Enumerable.Range(0, Rows).Select(row => yTrue[(row * LabelCount) + label] ? 1 : 0)];
            int[] binaryPred = [.. Enumerable.Range(0, Rows).Select(row => yPred[(row * LabelCount) + label] ? 1 : 0)];
            SameMatrix(ConfusionMatrix.Compute(binaryTrue, binaryPred, [0, 1], weights), perLabel[label]);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void A_per_sample_stack_is_a_confusion_matrix_per_binarised_row(bool weighted)
    {
        (bool[] yTrue, bool[] yPred, double[] weights) = LabelMatrix(weighted);

        ConfusionMatrix[] perSample = MultilabelConfusionMatrix.Compute(yTrue, yPred, LabelCount, true, weights);
        for (int row = 0; row < Rows; row++)
        {
            int[] binaryTrue = [.. Enumerable.Range(0, LabelCount).Select(label => yTrue[(row * LabelCount) + label] ? 1 : 0)];
            int[] binaryPred = [.. Enumerable.Range(0, LabelCount).Select(label => yPred[(row * LabelCount) + label] ? 1 : 0)];
            double[] repeated = weighted ? [.. Enumerable.Repeat(weights[row], LabelCount)] : [];
            SameMatrix(ConfusionMatrix.Compute(binaryTrue, binaryPred, [0, 1], repeated), perSample[row]);
        }
    }

    [Fact]
    public void Coverage_without_a_sort_is_the_worst_max_rank_of_a_relevant_label()
    {
        var rng = new Random(43);
        const int labelCount = 12;
        double[] choices = [0.25, 0.5, -0.0, 0.0, double.PositiveInfinity, double.NegativeInfinity];
        int[] ranks = new int[labelCount];
        for (int trial = 0; trial < 400; trial++)
        {
            bool[] relevant = [.. Enumerable.Range(0, labelCount).Select(_ => rng.Next(3) == 0)];
            double[] scores = [.. Enumerable.Range(0, labelCount).Select(_ => rng.Next(2) == 0 ? rng.NextDouble() : choices[rng.Next(choices.Length)])];
            if (trial % 4 == 0)
            {
                // The ranked path, which a NaN anywhere in the row still takes.
                scores[rng.Next(labelCount)] = double.NaN;
            }

            LabelRanking.MaxRank(scores, ranks);
            int worst = Enumerable.Range(0, labelCount).Where(label => relevant[label]).Select(label => ranks[label]).DefaultIfEmpty(0).Max();

            Assert.Equal((double)worst, CoverageError.Score(relevant, scores, labelCount));
        }
    }

    [Fact]
    public void A_sparse_label_range_numbers_the_clusters_as_a_dense_one_does()
    {
        var rng = new Random(47);
        int[] dense = [.. Enumerable.Range(0, 300).Select(_ => rng.Next(7))];
        int[] sparse = [.. dense.Select(label => (label * 400_000_000) - 1_200_000_000)];
        double[] features = [.. Enumerable.Range(0, 600).Select(_ => rng.NextDouble())];

        int[] denseSizes = Partition.Sizes(dense, out int[] denseOrdinals, out int denseClusters);
        int[] sparseSizes = Partition.Sizes(sparse, out int[] sparseOrdinals, out int sparseClusters);

        Assert.Equal(denseClusters, sparseClusters);
        Assert.Equal(denseSizes, sparseSizes);
        Assert.Equal(denseOrdinals, sparseOrdinals);
        Assert.Equal(DaviesBouldin.Score(dense, features, 2), DaviesBouldin.Score(sparse, features, 2));
    }

    [Fact]
    public void A_one_vs_one_pair_ignores_samples_of_a_label_outside_the_classes()
    {
        int[] yTrue = [0, 1, 2, 3, 0, 1, 2, 3, 2, 1];
        double[] yScore =
        [
            0.6, 0.3, 0.1, 0.2, 0.5, 0.3, 0.1, 0.2, 0.7, 0.3, 0.3, 0.4, 0.5, 0.4, 0.1,
            0.2, 0.3, 0.5, 0.1, 0.1, 0.8, 0.2, 0.2, 0.6, 0.1, 0.1, 0.8, 0.3, 0.4, 0.3,
        ];
        int[] kept = [.. Enumerable.Range(0, yTrue.Length).Where(i => yTrue[i] != 3)];
        int[] keptTrue = [.. kept.Select(i => yTrue[i])];
        double[] keptScore = [.. kept.SelectMany(i => yScore.Skip(i * 3).Take(3))];

        // Macro averaging reads no prevalence, so the extra samples can only reach the pair scans.
        double withExtra = RocAuc.MultiClass(
            yTrue, yScore, 3, new MultiClassRocOptions { Strategy = MultiClassStrategy.OneVsOne, Labels = [0, 1, 2] });
        double without = RocAuc.MultiClass(
            keptTrue, keptScore, 3, new MultiClassRocOptions { Strategy = MultiClassStrategy.OneVsOne });

        Assert.Equal(without, withExtra);
    }

    private static (bool[] YTrue, bool[] YPred, double[] Weights) LabelMatrix(bool weighted)
    {
        var rng = new Random(41);
        bool[] yTrue = [.. Enumerable.Range(0, Rows * LabelCount).Select(_ => rng.Next(3) == 0)];
        bool[] yPred = [.. Enumerable.Range(0, Rows * LabelCount).Select(_ => rng.Next(2) == 0)];
        double[] weights = weighted ? [.. Enumerable.Range(0, Rows).Select(_ => rng.NextDouble() * 3)] : [];
        return (yTrue, yPred, weights);
    }

    private static void SameMatrix(ConfusionMatrix expected, ConfusionMatrix actual)
    {
        Assert.Equal(expected.ToArray(), actual.ToArray());
        Assert.Equal(expected.TotalWeight, actual.TotalWeight);
        Assert.Equal(expected.TrueSum.ToArray(), actual.TrueSum.ToArray());
        Assert.Equal(expected.NoSampleCorrect, actual.NoSampleCorrect);
        Assert.Equal(expected.IsWeighted, actual.IsWeighted);
        Assert.Equal(expected.Labels, actual.Labels);
    }
}
