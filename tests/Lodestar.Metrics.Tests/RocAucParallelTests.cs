using System.Text.Json;
using Xunit;

namespace Lodestar.Metrics.Tests;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible six-class score
// matrix for a bit-identity fixture; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// The guarantee issue #86 rests on: parallelising the per-class and per-pair
/// loops must not move a single bit. Not "within 1e-9" — identically. Every
/// class writes its own slot and the averaging runs afterwards on the calling
/// thread in array order, so if a value moves, the parallelisation is unsound
/// and the change is wrong.
/// </summary>
public sealed class RocAucParallelTests
{
    private static readonly int[] WorkerCounts = [2, 3, 8];

    [Theory]
    [MemberData(nameof(RocCorpus.MulticlassIndices), MemberType = typeof(RocCorpus))]
    public void Replays_the_frozen_corpus_bit_identically_in_parallel(int index)
    {
        JsonElement c = RocCorpus.Cases[index];
        int[] yTrue = RocCorpus.YTrue(c);
        double[] scores = RocCorpus.RowMajorScores(c);
        double[] weight = RocCorpus.SampleWeight(c);
        int classCount = c.GetProperty("class_count").GetInt32();

        foreach (JsonProperty entry in c.GetProperty("values").EnumerateObject())
        {
            string[] parts = entry.Name.Split('|');
            MultiClassStrategy strategy = parts[0] == "ovr"
                ? MultiClassStrategy.OneVsRest
                : MultiClassStrategy.OneVsOne;
            Averaging average = parts[1] == "macro" ? Averaging.Macro : Averaging.Weighted;

            double sequential = RocAuc.MultiClass(yTrue, scores, classCount, new MultiClassRocOptions
            {
                Strategy = strategy,
                Average = average,
                SampleWeight = weight,
            });

            foreach (int workers in WorkerCounts)
            {
                double parallel = RocAuc.MultiClass(yTrue, scores, classCount, new MultiClassRocOptions
                {
                    Strategy = strategy,
                    Average = average,
                    SampleWeight = weight,
                    MaxDegreeOfParallelism = workers,
                });

                Assert.Equal(
                    BitConverter.DoubleToInt64Bits(sequential),
                    BitConverter.DoubleToInt64Bits(parallel));
            }
        }
    }

    [Fact]
    public void A_NaN_score_is_refused_up_front_the_same_way_in_parallel()
    {
        // check_array's sentence before any class is scored, on either path (#1569).
        int[] yTrue = [0, 1, 2, 0, 1, 2];
        double[] scores =
        [
            0.5, 0.3, 0.2,
            0.2, double.NaN, 0.3,
            0.1, 0.2, double.NaN,
            0.6, 0.2, 0.2,
            0.2, double.NaN, 0.3,
            0.1, 0.3, double.NaN,
        ];

        ArgumentException sequential = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(yTrue, scores, 3));
        ArgumentException parallel = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(yTrue, scores, 3, new MultiClassRocOptions { MaxDegreeOfParallelism = 8 }));

        Assert.StartsWith("Input contains NaN.", sequential.Message, StringComparison.Ordinal);
        Assert.Equal(sequential.Message, parallel.Message);
        Assert.Equal(sequential.ParamName, parallel.ParamName);
    }

    [Fact]
    public void A_refused_weight_is_reported_the_same_way_in_parallel()
    {
        // All-zero weights under macro are refused once, before any worker starts: the parallel path throws the same
        // ArgumentException as the sequential one, not an AggregateException.
        int[] yTrue = [0, 1, 2, 0, 1, 2];
        double[] scores = [.5, .3, .2, .2, .5, .3, .1, .2, .7, .6, .2, .2, .2, .5, .3, .1, .3, .6];
        double[] weights = new double[6];

        ArgumentException sequential = Assert.Throws<ArgumentException>(
            () => RocAuc.MultiClass(yTrue, scores, 3, new MultiClassRocOptions { SampleWeight = weights }));
        ArgumentException parallel = Assert.Throws<ArgumentException>(() => RocAuc.MultiClass(
            yTrue, scores, 3, new MultiClassRocOptions { SampleWeight = weights, MaxDegreeOfParallelism = 8 }));

        Assert.Equal(sequential.Message, parallel.Message);
        Assert.Equal(sequential.ParamName, parallel.ParamName);
    }

    [Fact]
    public void A_class_absent_from_y_true_scores_NaN_the_same_way_in_parallel()
    {
        int[] yTrue = [0, 0, 1, 1];
        double[] scores = [0.9, 0.05, 0.05, 0.8, 0.1, 0.1, 0.1, 0.8, 0.1, 0.2, 0.7, 0.1];
        int[] labels = [0, 1, 2];

        // roc_auc_score(..., multi_class='ovr', labels=[0, 1, 2]) answers nan for the absent class; it threw (#1277).
        double sequential = RocAuc.MultiClass(yTrue, scores, 3, new MultiClassRocOptions { Labels = labels });
        double parallel = RocAuc.MultiClass(yTrue, scores, 3, new MultiClassRocOptions
        {
            Labels = labels,
            MaxDegreeOfParallelism = 8,
        });

        Assert.True(double.IsNaN(sequential));
        Assert.True(double.IsNaN(parallel));
    }

    /// <summary>
    /// k=4, n=10 is an <c>ArrayPool</c> collision like the one docs/guides/performance.md's
    /// <c>ScoreSource</c> section measures (<c>Rent(10).Length * 4 == Rent(40).Length</c>): the
    /// corpus's class counts, 3 and 5, do not collide, so only this fixture would catch a span
    /// sliced to the rented length reading the wrong column. It was k=2 until #1605 refused two
    /// columns as scikit-learn does. One-vs-one also runs with two of the four classes present,
    /// the one shape giving <c>OneVsOneParallel</c> a single pair and collapsing
    /// <c>Math.Min(workers, count)</c> to one worker whatever the caller asked for.
    /// </summary>
    [Fact]
    public void A_power_of_two_class_count_is_bit_identical_in_parallel()
    {
        int[] allPresent = [0, 1, 2, 3, 0, 1, 2, 3, 0, 1];
        int[] twoPresent = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
        double[] scores =
        [
            0.4, 0.3, 0.2, 0.1, 0.1, 0.5, 0.2, 0.2, 0.2, 0.2, 0.5, 0.1, 0.1, 0.2, 0.3, 0.4, 0.35, 0.25, 0.2, 0.2,
            0.15, 0.45, 0.25, 0.15, 0.25, 0.25, 0.3, 0.2, 0.2, 0.1, 0.3, 0.4, 0.45, 0.2, 0.2, 0.15, 0.3, 0.3, 0.2, 0.2,
        ];

        foreach ((MultiClassStrategy strategy, int[] yTrue) in new[]
        {
            (MultiClassStrategy.OneVsRest, allPresent),
            (MultiClassStrategy.OneVsOne, allPresent),
            (MultiClassStrategy.OneVsOne, twoPresent),
        })
        {
            double sequential = RocAuc.MultiClass(yTrue, scores, 4,
                new MultiClassRocOptions { Strategy = strategy, Labels = [0, 1, 2, 3] });

            foreach (int workers in WorkerCounts)
            {
                double parallel = RocAuc.MultiClass(yTrue, scores, 4, new MultiClassRocOptions
                {
                    Strategy = strategy,
                    Labels = [0, 1, 2, 3],
                    MaxDegreeOfParallelism = workers,
                });

                Assert.Equal(BitConverter.DoubleToInt64Bits(sequential), BitConverter.DoubleToInt64Bits(parallel));
            }
        }
    }

    /// <summary>
    /// The parallel body must pass the <em>label</em> where <c>ClassScore</c>
    /// wants a label and the <em>column</em> where it wants a column. With labels
    /// 0..k-1 they are the same number, so every other test in this file passes
    /// with the two swapped. <see cref="RocAucMultiClassTests"/>'s sequential
    /// twin never sets <c>MaxDegreeOfParallelism</c>, so it guarded only the
    /// sequential driver — this one closes the gap on the parallel path.
    /// </summary>
    [Fact]
    public void Shifted_labels_are_bit_identical_in_parallel_too()
    {
        int[] shifted = [10, 20, 30, 30, 20, 10];
        double[] scores =
        [
            0.70, 0.20, 0.10,
            0.10, 0.60, 0.30,
            0.15, 0.25, 0.60,
            0.20, 0.20, 0.60,
            0.30, 0.50, 0.20,
            0.55, 0.30, 0.15,
        ];
        int[] labels = [10, 20, 30];

        foreach (MultiClassStrategy strategy in new[] { MultiClassStrategy.OneVsRest, MultiClassStrategy.OneVsOne })
        {
            double sequential = RocAuc.MultiClass(shifted, scores, 3, new MultiClassRocOptions
            {
                Strategy = strategy,
                Labels = labels,
            });

            foreach (int workers in WorkerCounts)
            {
                double parallel = RocAuc.MultiClass(shifted, scores, 3, new MultiClassRocOptions
                {
                    Strategy = strategy,
                    Labels = labels,
                    MaxDegreeOfParallelism = workers,
                });

                Assert.Equal(
                    BitConverter.DoubleToInt64Bits(sequential),
                    BitConverter.DoubleToInt64Bits(parallel));
            }
        }
    }

    [Fact]
    public void One_vs_one_over_six_classes_is_bit_identical_in_parallel()
    {
        // 15 pairs and 30 curves, more pairs than workers and more workers than
        // any single pair needs: the shape where a per-pair race would show.
        const int k = 6;
        const int n = 240;
        int[] yTrue = new int[n];
        double[] scores = new double[n * k];
        var random = new Random(20260808);

        for (int i = 0; i < n; i++)
        {
            yTrue[i] = i % k;
            double total = 0.0;
            for (int c = 0; c < k; c++)
            {
                double draw = random.NextDouble() + (c == yTrue[i] ? 0.75 : 0.0);
                scores[(i * k) + c] = draw;
                total += draw;
            }
            for (int c = 0; c < k; c++)
            {
                scores[(i * k) + c] /= total;
            }
        }

        foreach (Averaging average in new[] { Averaging.Macro, Averaging.Weighted })
        {
            double sequential = RocAuc.MultiClass(yTrue, scores, k, new MultiClassRocOptions
            {
                Strategy = MultiClassStrategy.OneVsOne,
                Average = average,
            });

            // Bit equality alone would hold if both paths degenerated to the same
            // NaN; pin the value to a separable problem's band first.
            Assert.InRange(sequential, 0.5, 1.0);

            foreach (int workers in WorkerCounts)
            {
                double parallel = RocAuc.MultiClass(yTrue, scores, k, new MultiClassRocOptions
                {
                    Strategy = MultiClassStrategy.OneVsOne,
                    Average = average,
                    MaxDegreeOfParallelism = workers,
                });

                Assert.Equal(
                    BitConverter.DoubleToInt64Bits(sequential),
                    BitConverter.DoubleToInt64Bits(parallel));
            }
        }
    }

    [Fact]
    public void More_workers_than_classes_is_not_an_error()
    {
        int[] yTrue = [0, 1, 2, 1];
        double[] scores = [0.7, 0.2, 0.1, 0.2, 0.6, 0.2, 0.1, 0.3, 0.6, 0.3, 0.4, 0.3];

        double sequential = RocAuc.MultiClass(yTrue, scores, 3);
        double parallel = RocAuc.MultiClass(yTrue, scores, 3,
            new MultiClassRocOptions { MaxDegreeOfParallelism = 64 });

        Assert.Equal(BitConverter.DoubleToInt64Bits(sequential), BitConverter.DoubleToInt64Bits(parallel));
    }
}
