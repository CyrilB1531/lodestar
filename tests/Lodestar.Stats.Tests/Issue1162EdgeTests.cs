using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>What #1162's four members refuse, and the edges their corpora do not isolate.</summary>
public sealed class Issue1162EdgeTests
{
    private static readonly double[] First = [1.2, 3.4, 2.2, 5.1, 4.4];
    private static readonly double[] Second = [2.0, 2.1, 2.3, 1.9, 2.2, 2.4];

    [Fact]
    public void Fligner_refuses_one_group_and_answers_nan_for_an_empty_one()
    {
        Assert.Throws<ArgumentException>(() => Fligner.Test(First));
        TestResult empty = Fligner.Test(First, []);
        Assert.True(double.IsNaN(empty.Statistic) && double.IsNaN(empty.PValue));
        Assert.Throws<ArgumentException>(() => Fligner.Test(Center.Median, 0.05, NanPolicy.Raise, First, [1.0, double.NaN]));
    }

    [Fact]
    public void Fligner_answers_nan_where_every_deviation_ties()
    {
        TestResult tied = Fligner.Test([1.0, 3.0], [5.0, 7.0]);

        Assert.True(double.IsNaN(tied.PValue));
    }

    [Fact]
    public void The_k_sample_test_refuses_what_scipy_refuses()
    {
        Assert.Throws<ArgumentException>(() => AndersonDarling.KSample(First));
        Assert.Throws<ArgumentException>(() => AndersonDarling.KSample(First, []));
        Assert.Throws<ArgumentException>(() => AndersonDarling.KSample([1.0, 1.0], [1.0, 1.0, 1.0]));
        Assert.Throws<ArgumentException>(() => AndersonDarling.KSample(First, [1.0, double.NaN]));
        Assert.Throws<ArgumentOutOfRangeException>(() => AndersonDarling.KSample((AndersonKSampleVariant)9, First, Second));
    }

    /// <summary>The p-value is clamped to the table's ends, 0.25 and 0.001, as scipy's is.</summary>
    [Fact]
    public void The_k_sample_p_value_is_clamped_to_the_table()
    {
        double[] far = [.. First.Select(v => v + 100.0)];
        double[] near = [.. Second.Select(v => v + 100.0)];
        AndersonResult alike = AndersonDarling.KSample(First, [1.1, 3.5, 2.3, 5.0, 4.5]);
        AndersonResult apart = AndersonDarling.KSample([.. First, .. Second], [.. far, .. near]);

        Assert.Equal(0.25, alike.PValue);
        Assert.Equal(0.001, apart.PValue);
        Assert.Equal([25.0, 10.0, 5.0, 2.5, 1.0, 0.5, 0.1], apart.SignificanceLevels);
    }

    [Fact]
    public void The_matrix_refuses_a_malformed_shape()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Spearman.Matrix([1.0, 2.0, 3.0], 1));
        Assert.Throws<ArgumentException>(() => Spearman.Matrix([1.0, 2.0, 3.0], 2));
        Assert.Throws<ArgumentException>(() => Spearman.Matrix([1.0, 2.0, double.NaN, 4.0], 2, nanPolicy: NanPolicy.Raise));
    }

    /// <summary>Two variables give the 2 × 2 matrix whose off-diagonal is <see cref="Spearman.Test"/>, where scipy returns the scalar.</summary>
    [Fact]
    public void Two_variables_give_the_matrix_of_the_pairwise_test()
    {
        double[] rows = [.. First.Zip(Second.Take(5), (a, b) => new[] { a, b }).SelectMany(pair => pair)];

        CorrelationMatrix matrix = Spearman.Matrix(rows, 2);
        TestResult pair = Spearman.Test(First, Second.AsSpan(0, 5));

        Assert.Equal(pair.Statistic, matrix.Statistics[1]);
        Assert.Equal(pair.PValue, matrix.PValues[2]);
        Assert.Equal(new CorrelationMatrix(2, [.. matrix.Statistics], [.. matrix.PValues]), matrix);
    }

    /// <summary>A constant variable is NaN in its own row and column only, the other pairs computed, as scipy's matrix is.</summary>
    [Fact]
    public void A_constant_variable_leaves_the_other_pairs_computed()
    {
        double[] rows = [1.0, 5.0, 3.0, 2.0, 5.0, 1.0, 3.0, 5.0, 2.0, 4.0, 5.0, 4.0];

        CorrelationMatrix matrix = Spearman.Matrix(rows, 3);

        Assert.True(double.IsNaN(matrix.Statistics[1]) && double.IsNaN(matrix.Statistics[4]) && double.IsNaN(matrix.PValues[7]));
        Assert.Equal(Spearman.Test([1.0, 2.0, 3.0, 4.0], [3.0, 1.0, 2.0, 4.0]).Statistic, matrix.Statistics[2]);
    }

    [Fact]
    public void The_point_biserial_is_pearson_on_the_coded_variable()
    {
        bool[] x = [true, false, true, true, false];
        PearsonResult pearson = Pearson.Test([1.0, 0.0, 1.0, 1.0, 0.0], First);

        TestResult result = PointBiserial.Test(x, First);

        Assert.Equal(pearson.Statistic, result.Statistic);
        Assert.Equal(pearson.PValue, result.PValue);
        Assert.Throws<ArgumentException>(() => PointBiserial.Test(x, Second));
    }
}
