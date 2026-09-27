using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// A fitted statistic is a read-only view: casting one back to its array and writing through it
/// used to edit the fitted object (#1232).
/// </summary>
public sealed class FittedViewTests
{
    private static readonly double[] Samples = [1.0, -2.0, 3.0, 4.0, -5.0, 6.0, 7.0, 8.0, -9.0, 10.0, 11.0, 12.0];

    [Fact]
    public void Scaler_statistics_are_not_arrays()
    {
        StandardScaler standard = StandardScaler.Fit(Samples, 3);
        MinMaxScaler minMax = MinMaxScaler.Fit(Samples, 3);
        MaxAbsScaler maxAbs = MaxAbsScaler.Fit(Samples, 3);
        RobustScaler robust = RobustScaler.Fit(Samples, 3);

        AssertViews(
            standard.Mean, standard.Variance, standard.Scale,
            minMax.DataMinimum, minMax.DataMaximum, minMax.DataRange, minMax.Scale, minMax.Minimum,
            maxAbs.MaximumAbsolute, maxAbs.Scale, robust.Centre, robust.Scale);
    }

    [Fact]
    public void Transformer_and_imputer_statistics_are_not_arrays()
    {
        KBinsDiscretizer bins = KBinsDiscretizer.Fit(Samples, 3, new KBinsDiscretizerOptions { BinCount = 2 });
        QuantileTransformer quantiles = QuantileTransformer.Fit(Samples, 3);
        PowerTransformer power = PowerTransformer.Fit(Samples, 3);
        SimpleImputer imputer = SimpleImputer.Fit(Samples, 3);
        KnnImputer knn = KnnImputer.Fit(Samples, 3);

        AssertViews(quantiles.References, power.Lambdas, imputer.Statistics);
        AssertViews(bins.BinEdges[0], quantiles.Quantiles[0]);
        Assert.IsNotType<IReadOnlyList<double>[]>(bins.BinEdges, exactMatch: false);
        Assert.IsNotType<IReadOnlyList<double>[]>(quantiles.Quantiles, exactMatch: false);
        Assert.IsNotType<int[]>(bins.BinCounts, exactMatch: false);
        Assert.IsNotType<int[]>(knn.KeptFeatures, exactMatch: false);
    }

    [Fact]
    public void Encoder_categories_are_not_arrays()
    {
        string[] values = ["a", "b", "a", "c"];

        OneHotEncoder<string> oneHot = Encoders.OneHot<string>(values, 1);
        OrdinalEncoder<string> ordinal = Encoders.Ordinal<string>(values, 1);
        LabelEncoder<string> label = Encoders.Label<string>(values);

        Assert.IsNotType<string[]>(oneHot.Categories[0], exactMatch: false);
        Assert.IsNotType<IReadOnlyList<string>[]>(oneHot.Categories, exactMatch: false);
        Assert.IsNotType<IReadOnlyList<string>?[]>(oneHot.InfrequentCategories, exactMatch: false);
        Assert.IsNotType<string[]>(ordinal.Categories[0], exactMatch: false);
        Assert.IsNotType<string[]>(label.Classes, exactMatch: false);
    }

    [Fact]
    public void Split_indices_are_not_arrays()
    {
        FoldSplit fold = Splitters.KFold(6, 3)[0];
        TrainTestSplit split = Splitters.TrainTest(6, 0.5);

        Assert.IsNotType<int[]>(fold.TrainIndices, exactMatch: false);
        Assert.IsNotType<int[]>(fold.TestIndices, exactMatch: false);
        Assert.IsNotType<int[]>(split.TrainIndices, exactMatch: false);
        Assert.IsNotType<int[]>(split.TestIndices, exactMatch: false);
    }

    [Fact]
    public void Sparse_fits_read_the_consolidated_matrix_once_and_agree_with_the_dense_ones()
    {
        // Row 0 stores column 1 twice; the sparse fits must read 2 + 3 = 5, as ToDense does.
        CsrMatrix sparse = new(2, 2, [2.0, 3.0, 1.0, 4.0], [1, 1, 0, 1], [0, 2, 4]);
        double[] dense = [.. sparse.ToDense().Cast<double>()];

        Assert.Equal(
            StandardScaler.Fit(dense, 2, new StandardScalerOptions { WithMean = false }).Scale,
            StandardScaler.Fit(sparse, new StandardScalerOptions { WithMean = false }).Scale);
        Assert.Equal(MaxAbsScaler.Fit(dense, 2).Scale, MaxAbsScaler.Fit(sparse).Scale);
        Assert.Equal(
            RobustScaler.Fit(dense, 2, new RobustScalerOptions { WithCentring = false }).Scale,
            RobustScaler.Fit(sparse).Scale);

        MaxAbsScaler clipping = MaxAbsScaler.Fit(new CsrMatrix(1, 2, [1.0, 1.0], [0, 1], [0, 2]), new MaxAbsScalerOptions { Clip = true });
        CsrMatrix clipped = clipping.Transform(sparse);
        Assert.Equal([0.0, 1.0, 1.0, 1.0], clipped.ToDense().Cast<double>());
        Assert.Equal(3, clipped.Values.Length);
    }

    private static void AssertViews(params IReadOnlyList<double>?[] statistics)
    {
        foreach (IReadOnlyList<double>? statistic in statistics)
        {
            Assert.NotNull(statistic);
            Assert.IsNotType<double[]>(statistic, exactMatch: false);
        }
    }
}
