using System.Globalization;
using Lodestar.Abstractions;
using Lodestar.Internal;
using Lodestar.Preprocessing.Internal;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// The pre-1.0 review findings on <c>Lodestar.Preprocessing</c> (#1226, #1227, #1229, #1230, #1237, #1253, #1254),
/// each against the value scikit-learn 1.9.1, scipy 1.18.1 or numpy 2.5.3 answers, run from <c>/var/tmp</c>.
/// </summary>
public sealed class PreprocessingReviewFindingsTests
{
    private static readonly double[] RoundValues = [0.0, 0.3, 0.6, 0.7, 1.0];

    [Fact]
    public void Box_cox_exponent_is_searched_unbounded_from_its_bracket()
    {
        // stats.boxcox([10, 9.9, 9.8, 9.9, 9.95, 9.99, 5.0, 9.97])[1] == 11.129078030986289; clamped to 2 (#1227).
        double[] samples = [10.0, 9.9, 9.8, 9.9, 9.95, 9.99, 5.0, 9.97];

        PowerTransformer fitted = PowerTransformer.Fit(samples, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox });

        Assert.Equal(11.129078030986289, fitted.Lambdas[0], 1e-5);
    }

    [Theory]
    [InlineData(new[] { 10.0, 9.9, 9.8, 9.9, 9.95, 9.99, 5.0, 9.97 }, 11.129078030986289, 1e6, 6.834720844608326)]
    [InlineData(new[] { 0.5, 0.51, 0.49, 0.52, 0.9 }, -6.943621335084113, 3.0, -3.3786928309598085)]
    [InlineData(new[] { 0.2, 0.21, 0.22, 0.9 }, -2.3147418289761483, 1.5, 9.992007221626409e-16)]
    [InlineData(new[] { 0.3, 2.0, 2.1, 1.9, 2.05 }, 2.3898090221070563, 50.0, 2.3898090221070563)]
    public void An_exponent_that_would_overflow_is_pulled_back_as_boxcox_normmax_pulls_it(
        double[] column, double lambda, double ceiling, double expected)
    {
        // stats.boxcox_normmax(x, method="mle", ymax=ceiling): the default ceiling, max / 1e4, no data of a sane size reaches.
        Assert.Equal(expected, PowerTransformer.OverflowGuarded(column, lambda, ceiling), 1e-9);
    }

    [Fact]
    public void A_column_far_from_one_finds_its_exponent_past_the_bracket()
    {
        // PowerTransformer(method="box-cox").fit(...).lambdas_ == [6.926384921979709], values near 1e10 (#1227).
        double[] samples = [1e10, 1e10 * (1 + 1e-6), 1e10 * (1 - 1e-6), 1e10 * (1 + 2e-6), 5e9];

        PowerTransformer fitted = PowerTransformer.Fit(
            samples, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox, Standardize = false });

        // The likelihood is flat to rounding there, so the system's exp moves the last digits; the point is the bound.
        Assert.Equal(6.926384921979709, fitted.Lambdas[0], 0.05);
    }

    [Theory]
    [InlineData(new[] { 1e10, 1e10 * (1 + 1e-6), 1e10 * (1 - 1e-6), 1e10 * (1 + 2e-6), 5e9 }, 6.92638486096969, 0.05)]
    [InlineData(new[] { -3.0, -1.0, 0.5, 2.0, 7.0 }, 0.7030274833529645, 1e-5)]
    public void Yeo_johnson_s_likelihood_is_scipy_s_in_logarithms(double[] samples, double expected, double tolerance)
    {
        // PowerTransformer().fit(...).lambdas_: the plain variance of 1e10's transform rounded to 0 and the search
        // ran to its bound, -12.9; one-signed data takes its variance in logarithms, mixed data directly (#1227).
        PowerTransformer fitted = PowerTransformer.Fit(samples, 1, new PowerTransformerOptions { Standardize = false });

        // The 1e10 column's likelihood is flat to rounding, so the system's exp moves its last digits; not its bound.
        Assert.Equal(expected, fitted.Lambdas[0], tolerance);
    }

    [Fact]
    public void Quantile_references_and_quantiles_are_numpy_s_to_the_bit()
    {
        // QuantileTransformer(n_quantiles=6) over seven values: references_[3] is linspace's 0x3FE3333333333334, not
        // 3 / 5's ...33, and quantiles_[3] _lerp's 0x4007D70A3D70A3D9, not the textbook's ...D6 (#1230).
        QuantileTransformer fitted = QuantileTransformer.Fit(
            [0.1, 0.7, 1.3, 2.2, 3.5, 4.1, 5.9], 1, new QuantileTransformerOptions { QuantileCount = 6 });

        Assert.Equal(
            [0L, 0x3FC999999999999AL, 0x3FD999999999999AL, 0x3FE3333333333334L, 0x3FE999999999999AL, 0x3FF0000000000000L],
            fitted.References.Select(BitConverter.DoubleToInt64Bits));
        Assert.Equal(
            [0x3FB999999999999AL, 0x3FEA3D70A3D70A3EL, 0x3FFA8F5C28F5C291L, 0x4007D70A3D70A3D9L, 0x400FD70A3D70A3D7L, 0x401799999999999AL],
            fitted.Quantiles[0].Select(BitConverter.DoubleToInt64Bits));
    }

    [Fact]
    public void A_near_constant_box_cox_column_reads_a_finite_likelihood()
    {
        // PowerTransformer(method="box-cox") on three values ulps apart: 0.2241. log1p read -∞ where scipy's
        // logsumexp falls back to the direct difference, and the search was pulled to 0.00207.
        double[] samples = [0.010000000000000153, 0.01000000000000005, 0.010000000000000179];

        PowerTransformer fitted = PowerTransformer.Fit(
            samples, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox, Standardize = false });

        Assert.Equal(0.2241, fitted.Lambdas[0], 1e-3);
    }

    [Fact]
    public void The_bracket_search_refuses_as_scipy_s_bracket_refuses()
    {
        // Run from (-2, 2), scipy's brent raises BracketError on a line falling forever and on NaN, and finds
        // a parabola's minimum at 7.25, outside the starting pair.
        foreach (Func<double, double> objective in new Func<double, double>[] { x => -x, _ => double.NaN })
        {
            ArgumentException error = Assert.Throws<ArgumentException>(
                () => BrentUnbounded.Locate(objective, -2.0, 2.0, "samples"));
            Assert.StartsWith(
                "The algorithm terminated without finding a valid bracket;",
                error.Message, StringComparison.Ordinal);
            Assert.Equal("samples", error.ParamName);
        }

        Assert.Equal(7.25, BrentUnbounded.Locate(x => (x - 7.25) * (x - 7.25), -2.0, 2.0, "samples"), 1e-7);
    }

    [Fact]
    public void A_constant_feature_keeps_yeo_johnson_s_identity_and_is_refused_by_box_cox()
    {
        // PowerTransformer().fit([[3], [3], [3]]).lambdas_ == [1.0]; its transform of 10 is 7, unstandardised 10 (#1253).
        double[] constant = [3.0, 3.0, 3.0];

        PowerTransformer standardised = PowerTransformer.Fit(constant, 1);
        PowerTransformer raw = PowerTransformer.Fit(constant, 1, new PowerTransformerOptions { Standardize = false });

        Assert.Equal(1.0, standardised.Lambdas[0]);
        Assert.Equal(7.0, standardised.Transform([10.0])[0], 1e-12);
        Assert.Equal(10.0, raw.Transform([10.0])[0], 1e-12);
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => PowerTransformer.Fit(constant, 1, new PowerTransformerOptions { Method = PowerMethod.BoxCox }));
        Assert.StartsWith("Data must not be constant.", error.Message, StringComparison.Ordinal);
        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void A_non_positive_box_cox_value_is_refused_before_any_exponent_in_the_previous_release_s_words()
    {
        // Feature by feature before any search, so it wins over a constant feature 0 refused after it.
        var boxCox = new PowerTransformerOptions { Method = PowerMethod.BoxCox };
        ArgumentException error = Assert.Throws<ArgumentException>(() => PowerTransformer.Fit([2.0, 1.0, 2.0, 0.0], 2, boxCox));
        Assert.Equal("Box-Cox needs strictly positive values; feature 1 holds 0. (Parameter 'column')", error.Message);
        error = Assert.Throws<ArgumentException>(() => PowerTransformer.Fit([1.0, 2.0], 1, boxCox).Transform([-1.0]));
        Assert.StartsWith("Box-Cox needs strictly positive values; feature 0 holds -1.", error.Message, StringComparison.Ordinal);
        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void Nan_categories_sort_last_and_the_ordinal_encoder_codes_them_nan()
    {
        // OrdinalEncoder().fit([[nan], [1], [2], [nan]]): categories_ [1, 2, nan], transform [nan, 0, 1, nan] (#1229).
        double[] values = [double.NaN, 1.0, 2.0, double.NaN];

        OrdinalEncoder<double> ordinal = Encoders.Ordinal<double>(values, 1);
        OneHotEncoder<double> oneHot = Encoders.OneHot<double>(values, 1);
        LabelEncoder<double> label = Encoders.Label<double>(values);

        Assert.Equal([1.0, 2.0, double.NaN], ordinal.Categories[0]);
        Assert.Equal([double.NaN, 0.0, 1.0, double.NaN], ordinal.Transform(values));
        Assert.Equal(["x0_1.0", "x0_2.0", "x0_nan"], oneHot.FeatureNames());
        Assert.Equal([0.0, 0.0, 1.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0], oneHot.Transform(values));
        Assert.Equal([2, 0, 1, 2], label.Transform(values));
    }

    [Fact]
    public void String_categories_sort_by_code_point()
    {
        // LabelEncoder().fit(["😀", "！", "a"]).classes_ == ['a', '！', '😀'], codes [2, 1, 0] (#1254).
        string[] labels = ["\U0001F600", "！", "a"];

        LabelEncoder<string> label = Encoders.Label<string>(labels);

        Assert.Equal(["a", "！", "\U0001F600"], label.Classes);
        Assert.Equal([2, 1, 0], label.Transform(labels));
    }

    [Fact]
    public void A_dense_norm_below_ten_epsilon_is_floored_and_a_sparse_non_finite_value_refused()
    {
        // Normalizer().transform([[1e-17, 0]]) == [[1e-17, 0]]; it was [1, 0] (#1230).
        Assert.Equal([1e-17, 0.0], Normalizer.Transform([1e-17, 0.0], 2));

        // normalize(sparse) refuses a NaN the sparse overload scaled, in its words; the dense one keeps its refusal.
        var sparse = new CsrMatrix(1, 2, [double.NaN], [0], [0, 1]);
        ArgumentException error = Assert.Throws<ArgumentException>(() => Normalizer.Transform(sparse));
        Assert.StartsWith("Input contains NaN.", error.Message, StringComparison.Ordinal);
        Assert.Equal("matrix", error.ParamName);
        ArgumentException dense = Assert.Throws<ArgumentException>(() => Normalizer.Transform([double.PositiveInfinity, double.NaN], 2));
        Assert.StartsWith("samples[0] is Infinity", dense.Message, StringComparison.Ordinal);
        Assert.Equal("samples", dense.ParamName);
    }

    [Fact]
    public void The_NaN_a_constant_column_past_the_float_range_transforms_to_is_refused_by_the_inverse()
    {
        // PowerTransformer() on [1.7e308] × 3 transforms to NaN; the inverse refuses it, as the previous release did.
        double[] samples = [1.7e308, 1.7e308, 1.7e308];
        PowerTransformer transformer = PowerTransformer.Fit(samples, 1);
        double[] transformed = transformer.Transform(samples);

        Assert.All(transformed, value => Assert.True(double.IsNaN(value)));
        Assert.StartsWith("samples[0] is NaN", Assert.Throws<ArgumentException>(
            () => transformer.InverseTransform(transformed)).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_column_that_could_overflow_among_others_is_summed_row_by_row_as_numpy_sums_an_axis()
    {
        // StandardScaler on [alternating ±1e308, 0..16]: numpy adds a non-contiguous axis row by row, which cancels to
        // 5.88e306 where the eight accumulators of one column gave NaN; measured on scikit-learn 1.9.1 (#1230).
        double[] samples = [.. Enumerable.Range(0, 17).SelectMany(i => new[] { i % 2 == 0 ? 1e308 : -1e308, i })];

        StandardScaler fitted = StandardScaler.Fit(samples, 2);
        Assert.Equal([5.882352941176471e306, 8.0], fitted.Mean!);
        Assert.True(double.IsNaN(fitted.Variance![0]));
        Assert.Equal(24.0, fitted.Variance[1]);
        Assert.Equal([5.882352941176471e306, 8.0], StandardScaler.Fit(samples, 2, new StandardScalerOptions { WithStd = false }).Mean!);

        StandardScaler folded = StandardScaler.Fit([1.0, 2.0, 2.0, 3.0], 2).PartialFit(samples);
        Assert.Equal([5.263157894736842e306, 7.421052631578948], folded.Mean!);
        Assert.True(double.IsNaN(folded.Variance![0]));
        Assert.Equal(24.34903047091413, folded.Variance[1], 12);
    }

    [Fact]
    public void The_nearest_donors_are_kept_in_a_heap_and_an_exact_match_outweighs_the_rest()
    {
        // KNNImputer(n_neighbors=3) over donors given farthest first keeps 3, 2 and 4 for 2.6: 30; with weights
        // "distance", a donor at distance 0 takes all the weight: 10, where uniform reads 15 (#1230).
        double[] donors = [5.0, 50.0, 4.0, 40.0, 3.0, 30.0, 2.0, 20.0, 1.0, 10.0, 0.5, 5.0];
        Assert.Equal(30.0, KnnImputer.Fit(donors, 2, new KnnImputerOptions { NeighbourCount = 3 }).Transform([2.6, double.NaN])[1], 12);

        double[] ladder = [1.0, 10.0, 2.0, 20.0, 3.0, 30.0, 4.0, 40.0, 5.0, 50.0];
        Assert.Equal(15.0, KnnImputer.Fit(ladder, 2, new KnnImputerOptions { NeighbourCount = 2 }).Transform([1.0, double.NaN])[1], 12);
        Assert.Equal(10.0, KnnImputer.Fit(ladder, 2, new KnnImputerOptions { NeighbourCount = 2, Weights = NeighbourWeights.Distance })
            .Transform([1.0, double.NaN])[1], 12);
    }

    [Fact]
    public void A_partial_fit_squares_the_difference_before_weighting_it()
    {
        // partial_fit([[1e149]]) then a million zeros: the reference's a / b * d ** 2 overflows the square to inf and
        // scales by 1, where ((a / b) * d) * d stayed finite near 1e292.
        StandardScaler folded = StandardScaler.Fit([1e149], 1).PartialFit(new double[1_000_000]);

        Assert.Equal(double.PositiveInfinity, folded.Variance![0]);
        Assert.Equal(1.0, folded.Scale![0]);
    }

    [Fact]
    public void KBinsDiscretizer_keeps_the_previous_release_s_refusal_of_a_NaN()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => KBinsDiscretizer.Fit([double.NaN, 1.0, 2.0], 1));
        Assert.StartsWith("samples[0] is NaN", error.Message, StringComparison.Ordinal);
        Assert.Equal("samples", error.ParamName);
    }

    [Fact]
    public void A_Half_category_is_named_as_numpy_writes_a_float16()
    {
        // OneHotEncoder on float16 [1e-07, 1.0, 65504, nan] names x0_1e-07, x0_1.0, x0_6.55e+04, x0_nan (#1229).
        OneHotEncoder<Half> encoder = Encoders.OneHot<Half>([(Half)1.0, (Half)65504, (Half)1e-7, Half.NaN, (Half)999], 1);

        Assert.Equal(["x0_1e-07", "x0_1.0", "x0_999.0", "x0_6.55e+04", "x0_nan"], encoder.FeatureNames());
    }

    [Fact]
    public void An_infinite_floating_category_is_refused_as_the_reference_s_allow_nan_check_refuses_it()
    {
        // OneHotEncoder().fit([[1.0], [inf]]) and OrdinalEncoder's say "Input contains infinity or a value too large
        // for dtype('float64')."; float16 names its own dtype; LabelEncoder checks nothing and keeps it (#1229).
        Assert.StartsWith("Input contains infinity or a value too large for dtype('float64').",
            Assert.Throws<ArgumentException>(() => Encoders.OneHot<double>([1.0, double.PositiveInfinity], 1)).Message,
            StringComparison.Ordinal);
        Assert.StartsWith("Input contains infinity or a value too large for dtype('float16').",
            Assert.Throws<ArgumentException>(() => Encoders.Ordinal<Half>([(Half)1.0, Half.NegativeInfinity], 1)).Message,
            StringComparison.Ordinal);
        Assert.Throws<ArgumentException>(() => Encoders.Ordinal<double>([1.0, 2.0], 1).Transform([double.PositiveInfinity]));
        Assert.Equal(2, Encoders.Label<double>([1.0, double.PositiveInfinity]).Classes.Count);
    }

    /// <summary>
    /// scipy.optimize.fminbound(f, low, high, xtol=1.48e-8) on objectives whose every operation IEEE fixes, so the
    /// port's path is pinned to the bit where a likelihood flat to rounding would pin the system's exp (#1227).
    /// </summary>
    [Fact]
    public void The_bounded_search_follows_fminbound_step_for_step()
    {
        Assert.Equal(1.3299605279268474, BrentMinimum.Locate(x => ((x - 0.7) * (x - 0.7) * (x - 0.7) * (x - 0.7)) - x, -2.0, 4.0, "samples"));
        Assert.Equal(-0.11059834300040752, BrentMinimum.Locate(x => (x * x * (x - 1) * (x - 1)) + (0.3 * x), -1.0, 2.0, "samples"));
        Assert.Equal(0.24999999997128478, BrentMinimum.Locate(x => Math.Abs(x - 0.25), -1.0, 3.0, "samples"));
    }

    [Fact]
    public void A_float_column_sorts_its_NaN_last_and_names_its_dtype()
    {
        // OrdinalEncoder on float32 [1, nan, 0.5] codes 1, nan, 0; an infinity is refused naming float32 (#1229).
        Assert.Equal([1.0, double.NaN, 0.0], Encoders.Ordinal<float>([1f, float.NaN, 0.5f], 1).Transform([1f, float.NaN, 0.5f]));
        Assert.StartsWith("Input contains infinity or a value too large for dtype('float32').",
            Assert.Throws<ArgumentException>(() => Encoders.OneHot<float>([1f, float.PositiveInfinity], 1)).Message,
            StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(40, "Samples [ 0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21 22 23\n 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39]")]
    [InlineData(1500, "Samples [   0    1    2 ... 1497 1498 1499]")]
    public void All_zero_rows_wrap_and_summarise_as_numpy_prints_them(int rows, string start)
    {
        // KBinsDiscretizer(4, encode="onehot-dense", strategy="uniform") on 0..19: numpy wraps at 75 columns and
        // summarises past 1,000 entries, measured on 2.5.3 (#1230).
        KBinsDiscretizer bins = KBinsDiscretizer.Fit(
            [.. Enumerable.Range(0, 20).Select(i => (double)i)], 1, new KBinsDiscretizerOptions { BinCount = 4, Strategy = BinStrategy.Uniform });

        ArgumentException error = Assert.Throws<ArgumentException>(() => bins.InverseTransform(new double[rows * 4]));
        Assert.StartsWith(start, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_median_is_numpy_s_mean_of_the_two_middle_values()
    {
        // RobustScaler().fit([[-1e308], [1e308]]).center_ == [0.]; interpolating read infinity (#1230).
        RobustScaler scaler = RobustScaler.Fit([-1e308, 1e308], 1);

        Assert.Equal(0.0, scaler.Centre![0]);
    }

    [Theory]
    [InlineData(3, double.PositiveInfinity)]
    [InlineData(601, 1e308)]
    public void The_centre_is_np_nanmedian_s_masked_median_below_six_hundred_rows(int rows, double expected)
    {
        // RobustScaler(with_scaling=False) over rows of 1e308: np.ma.median halves 1e308 + 1e308 below 600 rows,
        // np.median takes the middle value from there (#1230).
        double[] samples = Enumerable.Repeat(1e308, rows).ToArray();

        RobustScaler scaler = RobustScaler.Fit(samples, 1, new RobustScalerOptions { WithScaling = false });

        Assert.Equal(expected, scaler.Centre![0]);
    }

    [Fact]
    public void The_imputed_median_is_np_ma_median_s()
    {
        // SimpleImputer(strategy="median").fit([[1e308], [1e308], [1e308], [nan]]).statistics_ == [inf].
        SimpleImputer imputer = SimpleImputer.Fit(
            [1e308, 1e308, 1e308, double.NaN], 1, new SimpleImputerOptions { Strategy = ImputationStrategy.Median });

        Assert.Equal(double.PositiveInfinity, imputer.Statistics[0]);
    }

    [Fact]
    public void Uniform_edges_are_np_linspace_s()
    {
        // KBinsDiscretizer(10, strategy="uniform") over [0, 0.3, 0.6, 0.7, 1.0] codes 0, 2, 5, 6, 9; it coded 0, 3, 6, 7, 9 (#1226).
        KBinsDiscretizer fitted = KBinsDiscretizer.Fit(RoundValues, 1, new KBinsDiscretizerOptions
        {
            BinCount = 10,
            Strategy = BinStrategy.Uniform,
            Encoding = BinEncoding.Ordinal,
        });

        Assert.Equal([0.0, 2.0, 5.0, 6.0, 9.0], fitted.Transform(RoundValues));
    }

    [Fact]
    public void A_constant_feature_is_one_bin_over_the_whole_line()
    {
        // bin_edges_ [-inf, inf] and an inverse of NaN, where the edges were [c, c] and the inverse c (#1230).
        KBinsDiscretizer fitted = KBinsDiscretizer.Fit([3.0, 3.0, 3.0], 1, new KBinsDiscretizerOptions
        {
            BinCount = 3,
            Strategy = BinStrategy.Uniform,
            Encoding = BinEncoding.Ordinal,
        });

        Assert.Equal([double.NegativeInfinity, double.PositiveInfinity], fitted.BinEdges[0]);
        Assert.Equal([0.0], fitted.Transform([3.0]));
        Assert.True(double.IsNaN(fitted.InverseTransform([0.0])[0]));
    }

    [Theory]
    [InlineData(-1.0, 2.5)]
    [InlineData(2.7, 2.5)]
    [InlineData(-3.0, 0.5)]
    [InlineData(0.9, 0.5)]
    public void An_ordinal_code_is_read_as_numpy_indexes_with_it(double code, double centre)
    {
        // bin_centers[code.astype(np.int64)] over 3 uniform bins of [0, 3]: truncated, a negative from the end (#1230).
        Assert.Equal(centre, UniformThirds(BinEncoding.Ordinal).InverseTransform([code])[0], 12);
    }

    [Theory]
    [InlineData(3.0)]
    [InlineData(-4.0)]
    [InlineData(double.NaN)]
    public void An_ordinal_code_past_either_end_is_refused(double code)
    {
        // IndexError for 3 and -4, "Input contains NaN" for NaN; the cast saturated or wrapped (#1230).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => UniformThirds(BinEncoding.Ordinal).InverseTransform([code]));
        Assert.Equal("encoded", error.ParamName);
    }

    [Fact]
    public void A_one_hot_block_is_read_by_its_argmax_and_refused_when_all_zeros()
    {
        // The one-hot inverse of [0.2, 0.5, 0.1] is 1.5; rows 0 and 2 all zeros raise "Samples [0 2] ..." (#1230).
        KBinsDiscretizer fitted = UniformThirds(BinEncoding.OneHot);

        Assert.Equal(1.5, fitted.InverseTransform([0.2, 0.5, 0.1])[0], 12);
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => fitted.InverseTransform([0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0]));
        Assert.StartsWith("Samples [0 2] can not be inverted", error.Message, StringComparison.Ordinal);
        Assert.Equal("encoded", error.ParamName);
    }

    [Fact]
    public void A_constant_yeo_johnson_feature_is_standardised_as_it_was_fitted()
    {
        // PowerTransformer().fit(five rows of 1e10).transform(...) == -1.33514404296875e-05: the scaler fits the raw
        // 1e10, and scipy's expm1(log1p(x)) maps it to 9999999999.999987, where fitting on that output read 0.
        double[] samples = [1e10, 1e10, 1e10, 1e10, 1e10];

        PowerTransformer fitted = PowerTransformer.Fit(samples, 1);

        // A few ulps of 1e10 below zero, not 0: the last bits are libm's, so the band is asserted, not the value.
        Assert.Equal(1.0, fitted.Lambdas[0]);
        Assert.InRange(fitted.Transform(samples)[0], -1e-4, -1e-6);
    }

    [Fact]
    public void A_variance_that_overflows_scales_by_one_as_the_reference_s_does()
    {
        // Fitted on 1e200, 1, 2 and 3, scikit-learn reads an infinite variance and scales by 1, where infinity minus
        // infinity in the compensated sum made it NaN, and PowerTransformer, standardising through it, NaN (#1230).
        double[] samples = [1e200, 1.0, 2.0, 3.0];

        Assert.Equal(1.0, StandardScaler.Fit(samples, 1).Scale![0]);
        Assert.Equal(1.0, StandardScaler.Fit([0.0], 1).PartialFit(samples).Scale![0]);

        // Where the correction term overflows too, the reference's variance is NaN: [1e200, 1e200, 1], [1e308, 1e308].
        Assert.True(double.IsNaN(StandardScaler.Fit([1e200, 1e200, 1.0], 1).Scale![0]));
        StandardScaler twice = StandardScaler.Fit([1e308, 1e308], 1);
        Assert.Equal(double.PositiveInfinity, twice.Mean![0]);
        Assert.True(double.IsNaN(twice.Variance![0]));
        double[] transformed = PowerTransformer.Fit(samples, 1).Transform(samples);
        Assert.Equal(7.5e199, transformed[0], 7.5e187);
        Assert.Equal(-2.5e199, transformed[1], 2.5e187);
    }

    [Fact]
    public void A_column_some_order_could_overflow_is_summed_in_numpy_s_order_on_every_path()
    {
        // Measured against scikit-learn 1.9.1. Alternating ±1e308 overflows numpy's eight accumulators to NaN where a
        // compensated sum in row order stays finite; the partial fit and the mean-only fit skipped the fallback (#1230).
        double[] alternating = [.. Enumerable.Range(0, 17).Select(i => i % 2 == 0 ? 1e308 : -1e308)];
        var meanOnly = new StandardScalerOptions { WithStd = false };

        Assert.True(double.IsNaN(StandardScaler.Fit(alternating, 1).Mean![0]));
        Assert.True(double.IsNaN(StandardScaler.Fit(alternating, 1, meanOnly).Mean![0]));
        Assert.Equal(double.PositiveInfinity, StandardScaler.Fit([1e308, 1e308, -1e308], 1, meanOnly).Mean![0]);
        Assert.True(double.IsNaN(StandardScaler.Fit([1.0, 2.0], 1, meanOnly).PartialFit(alternating).Mean![0]));

        StandardScaler folded = StandardScaler.Fit([1.0, 2.0], 1).PartialFit(alternating);
        Assert.True(double.IsNaN(folded.Mean![0]));
        Assert.True(double.IsNaN(folded.Variance![0]));

        StandardScaler overflowed = StandardScaler.Fit([1.0, 2.0], 1).PartialFit([1e308, 1e308]);
        Assert.Equal(double.PositiveInfinity, overflowed.Mean![0]);
        Assert.True(double.IsNaN(overflowed.Variance![0]));

        StandardScaler squared = StandardScaler.Fit([1.0, 2.0], 1).PartialFit([1e200, 1e200, 1.0]);
        Assert.Equal(3.9999999999999995e199, squared.Mean![0]);
        Assert.True(double.IsNaN(squared.Variance![0]));

        StandardScaler mixed = StandardScaler.Fit([1.0, 2.0], 1).PartialFit([1e308, -1e308, 5.0]);
        Assert.Equal(1.6, mixed.Mean![0]);
        Assert.Equal(double.PositiveInfinity, mixed.Variance![0]);
        Assert.Equal(1.6666666666666667, StandardScaler.Fit([1e308, -1e308, 5.0], 1).Mean![0]);
    }

    [Fact]
    public void A_neighbour_count_past_the_donors_takes_every_donor_without_allocating_for_the_rest()
    {
        // scikit-learn clips n_neighbors to the donors; the heap was sized by the count asked for (#1230).
        KnnImputer imputer = KnnImputer.Fit([1.0, 2.0, 1.0, 4.0], 2, new KnnImputerOptions { NeighbourCount = 100_000_000 });

        Assert.Equal(3.0, imputer.Transform([1.0, double.NaN])[1], 12);
    }

    [Fact]
    public void Polynomial_terms_are_counted_before_they_are_listed()
    {
        // 1000 features at degree 6 is ~1.4e15 terms: refused by count, where listing them ran out of memory (#1230).
        var options = new PolynomialFeaturesOptions { Degree = 6 };

        ArgumentException error = Assert.Throws<ArgumentException>(() => PolynomialFeatures.OutputFeatureCount(1000, options));
        Assert.Equal("options", error.ParamName);
        Assert.Throws<ArgumentException>(
            () => PolynomialFeatures.OutputFeatureCount(1000, new PolynomialFeaturesOptions { Degree = 1_000_000 }));
        Assert.Equal(84, PolynomialFeatures.OutputFeatureCount(6, new PolynomialFeaturesOptions { Degree = 3 }));
        Assert.Equal(63, PolynomialFeatures.OutputFeatureCount(6, new PolynomialFeaturesOptions { Degree = 6, InteractionOnly = true, IncludeBias = false }));

        // Counted, not listed: 2000 features at degree 3 is 1,337,337,001 terms, under one array and answered at once.
        Assert.Equal(1_337_337_001, PolynomialFeatures.OutputFeatureCount(2000, new PolynomialFeaturesOptions { Degree = 3 }));
        Assert.Equal(64, PolynomialFeatures.OutputFeatureCount(6, new PolynomialFeaturesOptions { Degree = int.MaxValue, InteractionOnly = true }));
        Assert.Throws<ArgumentException>(() => PolynomialFeatures.OutputFeatureCount(1, new PolynomialFeaturesOptions { Degree = int.MaxValue }));
        Assert.Throws<ArgumentException>(
            () => PolynomialFeatures.OutputFeatureCount(2, new PolynomialFeaturesOptions { Degree = 2147483590, IncludeBias = false }));

        // The previous release's refusals keep its order: the options, then the samples, the count only then.
        Assert.Equal("samples", Assert.Throws<ArgumentException>(
            () => PolynomialFeatures.Transform([double.NaN, 1.0], 2, new PolynomialFeaturesOptions { Degree = 5000 })).ParamName);
        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => PolynomialFeatures.Transform([double.NaN], 1, new PolynomialFeaturesOptions { Degree = 0, IncludeBias = false })).ParamName);
        Assert.Equal("settings.Degree", Assert.Throws<ArgumentOutOfRangeException>(
            () => PolynomialFeatures.Transform([double.NaN], 1, new PolynomialFeaturesOptions { Degree = -1 })).ParamName);
        Assert.Equal("settings.Degree", Assert.Throws<ArgumentOutOfRangeException>(
            () => PolynomialFeatures.FeatureNames(1, new PolynomialFeaturesOptions { Degree = -1 })).ParamName);
        Assert.Equal(63, PolynomialFeatures.Transform(new double[6], 6, new PolynomialFeaturesOptions { Degree = int.MaxValue, InteractionOnly = true, IncludeBias = false }).Length);
    }

    [Fact]
    public void A_value_the_previous_release_refused_keeps_its_refusal()
    {
        // The first value that is not finite, in the previous release's words, whatever follows it.
        const string Words = "The reference skips a NaN; a percentile over a sorted column cannot";
        Assert.Contains(Words, Assert.Throws<ArgumentException>(
            () => Normalizer.Transform([double.PositiveInfinity, double.NaN], 1)).Message, StringComparison.Ordinal);
        ArgumentException power = Assert.Throws<ArgumentException>(() => PowerTransformer.Fit([double.NaN, 1.0, double.PositiveInfinity], 1));
        Assert.StartsWith("samples[0] is NaN", power.Message, StringComparison.Ordinal);
        Assert.StartsWith("samples[0] is Infinity", Assert.Throws<ArgumentException>(
            () => KBinsDiscretizer.Fit([double.PositiveInfinity, double.NaN, 1.0, 2.0], 1)).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void An_overflowing_column_gives_the_reference_statistics()
    {
        // StandardScaler().fit and SimpleImputer(strategy='median').fit, scikit-learn 1.9.1.
        StandardScaler wide = StandardScaler.Fit([1e308, 1e308, -1e308], 1);
        Assert.Equal(double.PositiveInfinity, wide.Mean![0]);
        Assert.True(double.IsNaN(wide.Variance![0]));
        StandardScaler cancelled = StandardScaler.Fit([1e200, -1e200], 1);
        Assert.Equal(0.0, cancelled.Mean![0]);
        Assert.Equal(double.PositiveInfinity, cancelled.Variance![0]);
        SimpleImputer median = SimpleImputer.Fit(
            [1e308, 1e308, 1e308], 1, new SimpleImputerOptions { Strategy = ImputationStrategy.Median });
        Assert.Equal(double.PositiveInfinity, median.Statistics[0]);
    }

    [Fact]
    public void Names_walk_the_powers_in_the_reference_order()
    {
        // Against the index-list walk the reference names from, written out here: degree by degree, nondecreasing lists.
        for (int features = 1; features <= 4; features++)
        {
            for (int degree = 1; degree <= 6; degree++)
            {
                var expected = new List<string> { "1" };
                for (int d = 1; d <= degree; d++)
                {
                    ListNames(new int[d], 0, 0, features, expected);
                }

                Assert.Equal(expected, PolynomialFeatures.FeatureNames(features, new PolynomialFeaturesOptions { Degree = degree }));
            }
        }

        // One feature at degree 40,000 named each term from a list of its degree, 1.8 s; by its powers it is linear.
        string[] names = PolynomialFeatures.FeatureNames(1, new PolynomialFeaturesOptions { Degree = 40_000 });
        Assert.Equal("x0^40000", names[^1]);
    }

    [Fact]
    public void Dense_norms_and_masked_means_sum_in_numpy_s_order()
    {
        // Each against scikit-learn 1.9.1 on numpy 2.5.3, where a left-to-right sum lands an ulp away: L1 is numpy's
        // pairwise row sum, L2 row_norms' baseline einsum, a mean np.ma.mean's filled column summed pairwise.
        double[] row = [6.3, -9.9, 7.1, -9.3, 4.6, -6.5, 7.3, 0.8, -4.0, -1.5];
        Assert.Equal(0.1099476439790576, Normalizer.Transform(row, 10, RowNorm.L1)[0]);
        double[] squares = [-9.4, -7.5, 3.4, 2.9, 2.3, -2.3, 9.9, 9.6, 3.7, 3.0];
        Assert.Equal(-0.4772003987919386, Normalizer.Transform(squares, 10, RowNorm.L2)[0]);

        double[] column = [-9.4, -7.5, 3.4, double.NaN, 2.3, -2.3, 9.9, 9.6, 3.7, 3.0];
        Assert.Equal(1.4111111111111114, SimpleImputer.Fit(column, 1).Statistics[0]);
        double[] pairs = new double[20];
        for (int i = 0; i < 10; i++)
        {
            pairs[2 * i] = column[i];
            pairs[(2 * i) + 1] = i;
        }

        Assert.Equal(1.4111111111111114, KnnImputer.Fit(pairs, 2).Transform([double.NaN, double.NaN])[0]);
    }

    [Fact]
    public void A_knn_feature_whose_fallback_mean_is_NaN_is_kept_by_its_mask()
    {
        // ±1e308 in blocks: the pairwise mean is NaN with every value present; scikit-learn keeps the feature (#1230).
        var samples = new double[32];
        for (int row = 0; row < 16; row++)
        {
            samples[2 * row] = (row / 4) % 2 == 0 ? 1e308 : -1e308;
            samples[(2 * row) + 1] = row;
        }

        KnnImputer imputer = KnnImputer.Fit(samples, 2);

        Assert.Equal(2, imputer.OutputFeatureCount);
        Assert.Equal(2, imputer.Transform([double.NaN, 3.0]).Length);
    }

    [Fact]
    public void An_overflowed_mean_is_an_empty_feature()
    {
        // np.ma.mean masks a mean past the float range, and keep_empty_features fills it with 0: statistics_ [0, 2].
        SimpleImputer imputer = SimpleImputer.Fit(
            [1e308, 1.0, 1e308, double.NaN, -1e308, 3.0], 2, new SimpleImputerOptions { KeepEmptyFeatures = true });

        Assert.Equal([0.0, 2.0], imputer.Statistics);

        // Without it the reference drops the feature; this refuses it, as it refuses one with no value at all.
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => SimpleImputer.Fit([1e308, 1.0, 1e308, double.NaN, -1e308, 3.0], 2));
        Assert.Equal("samples", error.ParamName);
        Assert.StartsWith("feature 0's mean is past the float range", error.Message, StringComparison.Ordinal);

        // A feature with no value at all keeps the previous release's sentence under the mean too.
        error = Assert.Throws<ArgumentException>(() => SimpleImputer.Fit([double.NaN, 1.0, double.NaN, 2.0], 2));
        Assert.StartsWith("feature 0 has no value at all", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_sum_of_negative_zeros_is_positive_zero_as_numpy_s_reduction_is()
    {
        // np.sum([]) and np.sum([-0.0] * n) are +0.0: numpy adds the pairwise sum to its +0.0 identity.
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(NumpyReduction.Sum([])));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(NumpyReduction.Sum([-0.0, -0.0, -0.0])));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(NumpyReduction.Sum(new double[200].Select(_ => -0.0).ToArray())));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(NumpyReduction.AbsSum([])));
    }

    [Fact]
    public void Medians_and_the_top_percentile_of_negative_zeros_are_positive_zero_as_numpy_s()
    {
        // np.median and np.ma.median go through a sum from +0.0, and np.percentile(q=100) lerps the top at gamma 0.
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(RobustScaler.Fit([-0.0, -0.0, -0.0], 1).Centre![0]));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(RobustScaler.Fit(new double[601].Select(_ => -0.0).ToArray(), 1).Centre![0]));
        SimpleImputer median = SimpleImputer.Fit(
            [-0.0, -0.0, double.NaN], 1, new SimpleImputerOptions { Strategy = ImputationStrategy.Median });
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(median.Statistics[0]));
        Assert.Equal(0L, BitConverter.DoubleToInt64Bits(NumpyGrid.Percentile([-1.0, -0.0], 100.0)));

        // One value is returned as stored: np.percentile([-0.0], q) is -0.0, and quantiles_ keeps it at one row.
        foreach (double q in new[] { 0.0, 50.0, 100.0 })
        {
            Assert.True(double.IsNegative(NumpyGrid.Percentile([-0.0], q)));
        }

        Assert.True(double.IsNegative(QuantileTransformer.Fit([-0.0, 1.0], 2).Quantiles[0][0]));
    }

    [Fact]
    public void Refusals_print_their_value_in_the_invariant_culture()
    {
        // Under fr-FR the value read "2,5" and "∞", so a message changed with the machine's locale (#1683).
        CultureInfo saved = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        try
        {
            OrdinalEncoder<double> ordinal = Encoders.Ordinal<double>([1.5, 3.5], 1);
            Assert.StartsWith("feature 0 has no category 2.5.", Assert.Throws<ArgumentException>(() => ordinal.Transform([2.5])).Message, StringComparison.Ordinal);
            OneHotEncoder<double> oneHot = Encoders.OneHot<double>([1.5, 3.5], 1);
            Assert.StartsWith("feature 0 has no category 2.5.", Assert.Throws<ArgumentException>(() => oneHot.Transform([2.5])).Message, StringComparison.Ordinal);
            var boxCox = new PowerTransformerOptions { Method = PowerMethod.BoxCox };
            Assert.StartsWith("Box-Cox needs strictly positive values; feature 0 holds -1.5.", Assert.Throws<ArgumentException>(() => PowerTransformer.Fit([-1.5, 2.0], 1, boxCox)).Message, StringComparison.Ordinal);
            Assert.StartsWith("samples[0] is Infinity.", Assert.Throws<ArgumentException>(() => StandardScaler.Fit([double.PositiveInfinity], 1)).Message, StringComparison.Ordinal);
            var sparse = new CsrMatrix(1, 1, [double.PositiveInfinity], [0], [0, 1]);
            Assert.Contains("stores Infinity at position 0", Assert.Throws<ArgumentException>(() => StandardScaler.Fit(sparse)).Message, StringComparison.Ordinal);
            Assert.Contains("(2.5, 1.5)", Assert.Throws<ArgumentOutOfRangeException>(
                () => MinMaxScaler.Fit([1.0, 2.0], 1, new MinMaxScalerOptions { Low = 2.5, High = 1.5 })).Message, StringComparison.Ordinal);
            Assert.Contains("(75.5, 25.5)", Assert.Throws<ArgumentOutOfRangeException>(
                () => RobustScaler.Fit([1.0, 2.0], 1, new RobustScalerOptions { LowerPercentile = 75.5, UpperPercentile = 25.5 })).Message, StringComparison.Ordinal);
            Assert.StartsWith("samples[0] is Infinity.", Assert.Throws<ArgumentException>(() => SimpleImputer.Fit([double.PositiveInfinity], 1)).Message, StringComparison.Ordinal);
            PowerTransformer fitted = PowerTransformer.Fit([1.5, 2.0], 1, boxCox);
            Assert.StartsWith("Box-Cox needs strictly positive values; feature 0 holds -1.5.", Assert.Throws<ArgumentException>(() => fitted.Transform([-1.5])).Message, StringComparison.Ordinal);

            // Swedish writes a negative integer with U+2212, so the order's offending row was culture-bound too.
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE");
            Assert.StartsWith("order[0] is -1,", Assert.Throws<ArgumentException>(() => Splitters.KFold(2, 2, [-1, 0])).Message, StringComparison.Ordinal);
            Assert.Contains("a gap of -5 leave", Assert.Throws<ArgumentException>(() => Splitters.TimeSeries(10, 2, testSize: 100, gap: -5)).Message, StringComparison.Ordinal);
            Assert.Contains("the -5 rows there are", Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(-5, 2)).Message, StringComparison.Ordinal);
        }
        finally
        {
            CultureInfo.CurrentCulture = saved;
        }
    }

    [Fact]
    public void A_time_series_split_count_at_int_max_is_refused_not_wrapped()
    {
        // Adding one to the split count wrapped to int.MinValue and skipped the refusal, then the fold array failed
        // to allocate, where scikit-learn refuses too many folds for ten samples.
        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(() => Splitters.TimeSeries(10, int.MaxValue));
        Assert.Contains("2147483648 blocks is more than the 10 rows there are.", error.Message, StringComparison.Ordinal);
    }

    private static void ListNames(int[] indices, int position, int from, int features, List<string> names)
    {
        if (position == indices.Length)
        {
            names.Add(string.Join(" ", indices.GroupBy(i => i).Select(
                g => g.Count() == 1 ? $"x{g.Key}" : $"x{g.Key}^{g.Count()}")));
            return;
        }

        for (int feature = from; feature < features; feature++)
        {
            indices[position] = feature;
            ListNames(indices, position + 1, feature, features, names);
        }
    }

    [Fact]
    public void A_high_degree_costs_its_output_not_an_index_array_per_term()
    {
        // Two features at degree 3000 are C(3002, 2) = 4,504,501 terms; listing each term's 3000 indices first needed
        // 36 GB, where the reference builds each column from one of the degree before (#1230).
        double[] expanded = PolynomialFeatures.Transform([1.0, 1.0], 2, new PolynomialFeaturesOptions { Degree = 3000 });

        Assert.Equal(4_504_501, expanded.Length);
        Assert.All(expanded, value => Assert.Equal(1.0, value));
    }

    [Fact]
    public void Tied_neighbours_are_taken_in_row_order_whatever_the_donor_count()
    {
        // Twenty donors at one distance: the unstable sort past sixteen picked among them by its own
        // order (#1230); the reference's argpartition varies with the CPU tier, so row order is kept.
        const int donors = 20;
        var samples = new double[(donors + 1) * 2];
        for (int row = 0; row < donors; row++)
        {
            samples[row * 2] = 1.0;
            samples[(row * 2) + 1] = row;
        }

        samples[donors * 2] = 1.0;
        samples[(donors * 2) + 1] = double.NaN;

        KnnImputer imputer = KnnImputer.Fit(samples, 2, new KnnImputerOptions { NeighbourCount = 3 });

        Assert.Equal(1.0, imputer.Transform([1.0, double.NaN])[1], 12);
    }

    [Fact]
    public void The_power_transforms_stay_finite_where_scipy_s_do()
    {
        // stats.yeojohnson(1e306, 1), special.boxcox(exp(355), 2) and special.inv_boxcox(1e308, 2): each finite in
        // scipy, where log1p's correction and expm1 overflowed here.
        Assert.Equal(1.0000000000000326e306, PowerLikelihood.YeoJohnson(1e306, 1.0), 1e294);
        Assert.Equal(1.116997383080917e308, PowerLikelihood.BoxCox(Math.Exp(355.0), 2.0), 1e296);
        Assert.Equal(1.414213562373066e154, PowerLikelihood.InverseBoxCox(1e308, 2.0), 1e142);
    }

    [Fact]
    public void An_ordinal_code_past_int64_is_printed_as_numpy_casts_it()
    {
        // astype(np.int64) of 1e20 reads the minimum: "index -9223372036854775808 is out of bounds for axis 0 with size 3".
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => UniformThirds(BinEncoding.Ordinal).InverseTransform([1e20]));
        Assert.StartsWith("index -9223372036854775808 is out of bounds for axis 0 with size 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_half_precision_nan_sorts_last_as_a_double_one_does()
    {
        // OrdinalEncoder().fit(float16 [[nan], [1], [2]]).categories_ == [1, 2, nan] (#1229).
        Half[] values = [Half.NaN, (Half)1.0, (Half)2.0];

        OrdinalEncoder<Half> ordinal = Encoders.Ordinal<Half>(values, 1);

        Assert.Equal([(Half)1.0, (Half)2.0, Half.NaN], ordinal.Categories[0]);
        Assert.Equal([double.NaN, 0.0, 1.0], ordinal.Transform(values));
    }

    [Fact]
    public void A_power_that_overflows_is_refused_when_standardised_and_returned_when_not()
    {
        // PowerTransformer().fit([[1], [8], [9], [10]]).transform([[1e300]]) raises StandardScaler's "Input X contains
        // infinity"; unstandardised it answers inf.
        double[] samples = [1.0, 8.0, 9.0, 10.0];

        ArgumentException error = Assert.Throws<ArgumentException>(() => PowerTransformer.Fit(samples, 1).Transform([1e300]));
        Assert.StartsWith("Input X contains infinity or a value too large for dtype('float64').", error.Message, StringComparison.Ordinal);
        Assert.Equal("samples", error.ParamName);
        Assert.Equal(double.PositiveInfinity, PowerTransformer.Fit(samples, 1, new PowerTransformerOptions { Standardize = false }).Transform([1e300])[0]);
    }

    [Fact]
    public void Ordinal_codes_are_checked_feature_by_feature()
    {
        // inverse_transform([[0, 5], [7, 0]]) over two features of 3 bins: "index 7 is out of bounds", the first feature's.
        KBinsDiscretizer fitted = KBinsDiscretizer.Fit([0.0, 0.0, 3.0, 3.0], 2, new KBinsDiscretizerOptions
        {
            BinCount = 3,
            Strategy = BinStrategy.Uniform,
            Encoding = BinEncoding.Ordinal,
        });

        ArgumentException error = Assert.Throws<ArgumentException>(() => fitted.InverseTransform([0.0, 5.0, 7.0, 0.0]));
        Assert.StartsWith("index 7 is out of bounds for axis 0 with size 3", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void All_zero_rows_are_listed_as_numpy_prints_them()
    {
        // Ten all-zero rows of twelve: "Samples [ 1  2  3  4  6  7  8  9 10 11] can not be inverted ...".
        KBinsDiscretizer fitted = KBinsDiscretizer.Fit([0.0, 1.0], 1, new KBinsDiscretizerOptions
        {
            BinCount = 2,
            Strategy = BinStrategy.Uniform,
            Encoding = BinEncoding.OneHot,
        });
        var encoded = new double[24];
        encoded[0] = 1.0;
        encoded[11] = 1.0;

        ArgumentException error = Assert.Throws<ArgumentException>(() => fitted.InverseTransform(encoded));
        Assert.StartsWith(
            "Samples [ 1  2  3  4  6  7  8  9 10 11] can not be inverted when drop=None and handle_unknown='error' because they contain all zeros",
            error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Numpy_s_averaged_inverted_cdf_averages_through_its_lerp()
    {
        // np.percentile([1, 2, 3, 4, 5, 6], 50, method="averaged_inverted_cdf") == 3.5, at a whole virtual index.
        Assert.Equal(3.5, NumpyGrid.AveragedInvertedCdf([1.0, 2.0, 3.0, 4.0, 5.0, 6.0], 50.0));
        Assert.Equal(1.0, NumpyGrid.AveragedInvertedCdf([1.0, 2.0, 3.0], 0.0));
        Assert.Equal(3.0, NumpyGrid.AveragedInvertedCdf([1.0, 2.0, 3.0], 100.0));
        Assert.Equal(2.0, NumpyGrid.Median([1.0, 2.0, 3.0]));
        Assert.Equal(2.5, NumpyGrid.Median([1.0, 2.0, 3.0, 4.0]));
    }

    private static KBinsDiscretizer UniformThirds(BinEncoding encoding) =>
        KBinsDiscretizer.Fit([0.0, 1.0, 2.0, 3.0], 1, new KBinsDiscretizerOptions
        {
            BinCount = 3,
            Strategy = BinStrategy.Uniform,
            Encoding = encoding,
        });
}
