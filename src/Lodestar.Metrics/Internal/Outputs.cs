using System.Numerics;
#if NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Lodestar.Metrics.Internal;

/// <summary>
/// What one metric does to one <c>(truth, prediction)</c> pair, and nothing
/// else — the only part of a weighted mean that differs between eight of the
/// seventeen regression metrics.
/// </summary>
/// <remarks>
/// Implemented by <see langword="struct"/>s, not a
/// <see cref="Func{T1, T2, TResult}"/>: specialized per kernel and resolved
/// statically, where a delegate would cost an indirect call per element.
/// </remarks>
internal interface IResidualKernel
{
    /// <summary>The metric's own per-pair quantity, before weighting.</summary>
    /// <param name="truth">One true value.</param>
    /// <param name="prediction">The prediction for the same sample and output.</param>
    double Apply(double truth, double prediction);
}

/// <summary>A kernel whose per-pair quantity also has a lane-wise form.</summary>
/// <remarks>
/// Separate from <see cref="IResidualKernel"/> because most kernels cannot have one:
/// the Tweedie deviances reach <c>Math.Pow</c> and the log errors
/// <c>Math.Log</c>, neither of which <see cref="Vector{T}"/> offers. Only the
/// two that are arithmetic all the way down implement it, and only they take the
/// vectorized walk (#321).
/// </remarks>
internal interface IVectorResidualKernel : IResidualKernel
{
    /// <summary>The same quantity, one lane per element.</summary>
    /// <param name="truth">One true value per lane.</param>
    /// <param name="prediction">The prediction for the same sample, per lane.</param>
    Vector<double> Apply(Vector<double> truth, Vector<double> prediction);
}

/// <summary>
/// What the regression metrics in this package share: agreeing that a flat span
/// really is <c>n × outputCount</c>, walking it once under a per-pair kernel,
/// and reducing the per-output array to a scalar.
/// </summary>
/// <remarks>
/// Eight kernels differ in one expression and share the rest; what is not the
/// same keeps its own code — the median sorts, R² and explained variance need
/// two passes, and <see cref="MaxError"/> takes no weights.
/// </remarks>
internal static class Outputs
{
    /// <summary>Checks the shape and returns the sample count.</summary>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, row-major.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty.</param>
    /// <param name="outputWeights">A weight per output, or empty for a plain mean.</param>
    /// <returns>The number of samples, <c>yTrue.Length / outputCount</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outputCount"/> is below one.</exception>
    /// <exception cref="ArgumentException">A length disagrees with the shape.</exception>
    public static int Validate(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        ReadOnlySpan<double> outputWeights)
    {
        Inputs.Validate(yTrue, yPred, sampleWeight);
        Guard.NotLessThan(outputCount, 1);

        if (yTrue.Length % outputCount != 0)
        {
            throw new ArgumentException(
                $"yTrue holds {yTrue.Length} values, which is not a whole number of rows of {outputCount} outputs.",
                nameof(outputCount));
        }

        int samples = yTrue.Length / outputCount;
        if (!sampleWeight.IsEmpty && sampleWeight.Length != samples)
        {
            throw new ArgumentException(
                $"sampleWeight has {sampleWeight.Length} entries but there are {samples} samples.",
                nameof(sampleWeight));
        }
        if (!outputWeights.IsEmpty)
        {
            if (outputWeights.Length != outputCount)
            {
                throw new ArgumentException(
                    $"outputWeights has {outputWeights.Length} entries but there are {outputCount} outputs.",
                    nameof(outputWeights));
            }

            RequireNormalizable(outputWeights);
        }

        return samples;
    }

    /// <summary>
    /// Reproduces <c>numpy.average</c>'s refusal of weights it cannot normalize,
    /// with its message — the error scikit-learn surfaces for
    /// <c>multioutput=[0, 0]</c>.
    /// </summary>
    /// <remarks>
    /// The test is the <em>sum</em>, not all-zero, unlike the sample weight's
    /// own check: <c>[1, -1]</c> is refused here though not all zero, while
    /// <c>[-1, -1]</c> scores, since its sum normalizes fine.
    /// </remarks>
    private static void RequireNormalizable(ReadOnlySpan<double> outputWeights)
    {
        double total = 0.0;
        foreach (double weight in outputWeights)
        {
            total += weight;
        }

        // S1244: the question is whether the sum normalizes at all, which is a
        // division by exactly zero and nothing else. A tolerance would refuse a
        // legitimately tiny sum that numpy divides by without complaint.
#pragma warning disable S1244
        if (total == 0.0)
#pragma warning restore S1244
        {
            throw new ArgumentException(
                "Weights sum to zero, can't be normalized.", nameof(outputWeights));
        }
    }

    /// <summary>
    /// The whole of a weighted-mean metric's <c>Score</c>: validate, walk under
    /// <typeparamref name="TKernel"/>, reduce.
    /// </summary>
    /// <typeparam name="TKernel">The per-pair quantity this metric averages.</typeparam>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty.</param>
    /// <param name="outputWeights">A weight per output, or empty for a plain mean.</param>
    /// <param name="kernel">The kernel instance, for a kernel that carries state such as an <c>alpha</c>.</param>
    public static double Score<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        ReadOnlySpan<double> outputWeights,
        TKernel kernel = default)
        where TKernel : struct, IResidualKernel
    {
        int samples = Validate(yTrue, yPred, outputCount, sampleWeight, outputWeights);
        return Reduce(WeightedMean(yTrue, yPred, outputCount, sampleWeight, samples, kernel), outputWeights);
    }

    /// <summary>
    /// <see cref="Score{TKernel}"/> for a kernel that has a lane-wise form, which takes
    /// the vectorized walk where the shape allows it. A distinct name rather than an
    /// overload: C# does not resolve on the constraint, and the two differ in nothing else.
    /// </summary>
    /// <typeparam name="TKernel">The per-pair quantity this metric averages.</typeparam>
    /// <param name="yTrue">The true values, row-major when there is more than one output.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty.</param>
    /// <param name="outputWeights">A weight per output, or empty for a plain mean.</param>
    /// <param name="kernel">The kernel instance.</param>
    public static double ScoreVectorized<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        ReadOnlySpan<double> outputWeights,
        TKernel kernel = default)
        where TKernel : struct, IVectorResidualKernel
    {
        if (outputCount == 1 && sampleWeight.IsEmpty && OnlyTargetsNeedScanning(yTrue, yPred, outputWeights)
            && TryUnweightedMean(yTrue, yPred, kernel, out double mean))
        {
            return Reduce([mean], outputWeights);
        }

        int samples = Validate(yTrue, yPred, outputCount, sampleWeight, outputWeights);
        return Reduce(WeightedMeanVectorized(yTrue, yPred, outputCount, sampleWeight, samples, kernel), outputWeights);
    }

    /// <summary><see cref="PerOutput{TKernel}"/> for a kernel that has a lane-wise form.</summary>
    /// <typeparam name="TKernel">The per-pair quantity this metric averages.</typeparam>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty.</param>
    /// <param name="kernel">The kernel instance.</param>
    public static double[] PerOutputVectorized<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        TKernel kernel = default)
        where TKernel : struct, IVectorResidualKernel
    {
        if (outputCount == 1 && sampleWeight.IsEmpty && OnlyTargetsNeedScanning(yTrue, yPred, default)
            && TryUnweightedMean(yTrue, yPred, kernel, out double mean))
        {
            return [mean];
        }

        int samples = Validate(yTrue, yPred, outputCount, sampleWeight, default);
        return WeightedMeanVectorized(yTrue, yPred, outputCount, sampleWeight, samples, kernel);
    }

    /// <summary>
    /// Whether an unweighted single-output input passes every check <see cref="Validate"/> makes
    /// that does not read the targets, answered without throwing.
    /// </summary>
    /// <param name="yTrue">The true values.</param>
    /// <param name="yPred">The predicted values.</param>
    /// <param name="outputWeights">A weight per output, or empty.</param>
    /// <remarks>
    /// The targets' finiteness is then folded into the metric's own walk, so the data is read once
    /// rather than three times. Whatever this or that walk refuses goes to <see cref="Validate"/>,
    /// which throws in its own order, so a caller sees the exception it always saw.
    /// </remarks>
    public static bool OnlyTargetsNeedScanning(
        ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPred, ReadOnlySpan<double> outputWeights) =>
        yTrue.Length == yPred.Length && !yTrue.IsEmpty
        && (outputWeights.IsEmpty || (outputWeights.Length == 1 && IsNormalizable(outputWeights[0])));

    // S1244: RequireNormalizable's own exact-zero test, over one weight; a NaN passes both.
#pragma warning disable S1244
    private static bool IsNormalizable(double weight) => weight != 0.0;
#pragma warning restore S1244

    /// <summary>
    /// The unweighted mean of <typeparamref name="TKernel"/> over one output, which also reports
    /// whether every target was finite.
    /// </summary>
    /// <typeparam name="TKernel">The per-pair quantity to average.</typeparam>
    /// <param name="yTrue">The true values, as long as <paramref name="yPred"/> and not empty.</param>
    /// <param name="yPred">The predicted values.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="mean">The mean, meaningful only when the method returns <see langword="true"/>.</param>
    /// <remarks>
    /// Accumulated per lane on <c>net10.0</c> and in four stripes elsewhere, which
    /// <see cref="StripedCompensatedSum"/> relates.
    /// </remarks>
    private static bool TryUnweightedMean<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        TKernel kernel,
        out double mean)
        where TKernel : struct, IVectorResidualKernel
    {
#if NET5_0_OR_GREATER
        bool finite = Vector.IsHardwareAccelerated
            ? TryLaneSum(yTrue, yPred, kernel, out CompensatedSum sum, out int i)
            : TryStripeSum(yTrue, yPred, kernel, out sum, out i);
#else
        bool finite = TryStripeSum(yTrue, yPred, kernel, out CompensatedSum sum, out int i);
#endif
        int samples = yTrue.Length;
        long nonFinite = 0;
        for (; i < samples; i++)
        {
            nonFinite |= Inputs.NonFiniteBits(yTrue[i]) | Inputs.NonFiniteBits(yPred[i]);
            sum.Add(kernel.Apply(yTrue[i], yPred[i]));
        }

        // Exact: n additions of 1.0 land on n for every n below 2^53.
        mean = sum.Value / samples;
        return finite && nonFinite == 0;
    }

#if NET5_0_OR_GREATER
    /// <summary>
    /// <see cref="TryUnweightedMean{TKernel}"/>'s whole blocks, one <see cref="Vector{T}"/> lane per element.
    /// </summary>
    /// <typeparam name="TKernel">The per-pair quantity to add.</typeparam>
    /// <param name="yTrue">The true values.</param>
    /// <param name="yPred">The predicted values, as long as <paramref name="yTrue"/>.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="sum">The lanes, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the blocks covered.</param>
    /// <returns>Whether every element the blocks covered was finite.</returns>
    private static bool TryLaneSum<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        TKernel kernel,
        out CompensatedSum sum,
        out int consumed)
        where TKernel : struct, IVectorResidualKernel
    {
        ReadOnlySpan<Vector<double>> truths = MemoryMarshal.Cast<double, Vector<double>>(yTrue);
        ReadOnlySpan<Vector<double>> predictions = MemoryMarshal.Cast<double, Vector<double>>(yPred);
        VectorCompensatedSum acc = default;
        Vector<long> laneBits = Vector<long>.Zero;
        for (int block = 0; block < truths.Length; block++)
        {
            Vector<double> truth = truths[block];
            Vector<double> prediction = predictions[block];
            laneBits |= Inputs.NonFiniteBits(truth) | Inputs.NonFiniteBits(prediction);
            acc.Add(kernel.Apply(truth, prediction));
        }

        sum = acc.Reduce();
        consumed = truths.Length * Vector<double>.Count;
        return laneBits == Vector<long>.Zero;
    }
#endif

    /// <summary><see cref="TryUnweightedMean{TKernel}"/>'s whole groups of four, one stripe per element.</summary>
    /// <typeparam name="TKernel">The per-pair quantity to add.</typeparam>
    /// <param name="yTrue">The true values.</param>
    /// <param name="yPred">The predicted values, as long as <paramref name="yTrue"/>.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="sum">The stripes, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the groups covered.</param>
    /// <returns>Whether every element the groups covered was finite.</returns>
    /// <remarks>Internal so the <c>net10.0</c> tests, where the lanes route around it, reach it.</remarks>
    internal static bool TryStripeSum<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        TKernel kernel,
        out CompensatedSum sum,
        out int consumed)
        where TKernel : struct, IResidualKernel
    {
        StripedCompensatedSum acc = default;
        long nonFinite = 0;
        int i = 0;
        for (; i <= yTrue.Length - 4; i += 4)
        {
            nonFinite |= Inputs.NonFiniteBits(yTrue[i]) | Inputs.NonFiniteBits(yPred[i])
                | Inputs.NonFiniteBits(yTrue[i + 1]) | Inputs.NonFiniteBits(yPred[i + 1])
                | Inputs.NonFiniteBits(yTrue[i + 2]) | Inputs.NonFiniteBits(yPred[i + 2])
                | Inputs.NonFiniteBits(yTrue[i + 3]) | Inputs.NonFiniteBits(yPred[i + 3]);
            acc.Add(
                kernel.Apply(yTrue[i], yPred[i]),
                kernel.Apply(yTrue[i + 1], yPred[i + 1]),
                kernel.Apply(yTrue[i + 2], yPred[i + 2]),
                kernel.Apply(yTrue[i + 3], yPred[i + 3]));
        }

        sum = acc.Reduce();
        consumed = i;
        return nonFinite == 0;
    }

    /// <summary>The same walk without the reduction — <c>multioutput="raw_values"</c>.</summary>
    /// <typeparam name="TKernel">The per-pair quantity this metric averages.</typeparam>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty.</param>
    /// <param name="kernel">The kernel instance, for a kernel that carries state.</param>
    public static double[] PerOutput<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        TKernel kernel = default)
        where TKernel : struct, IResidualKernel
    {
        int samples = Validate(yTrue, yPred, outputCount, sampleWeight, default);
        return WeightedMean(yTrue, yPred, outputCount, sampleWeight, samples, kernel);
    }

    /// <summary>
    /// The weighted mean of <typeparamref name="TKernel"/> over each output
    /// column, on input already validated.
    /// </summary>
    /// <typeparam name="TKernel">The per-pair quantity to average.</typeparam>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, row-major.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty for weight 1 each.</param>
    /// <param name="samples">The sample count <see cref="Validate"/> returned.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <returns>A fresh array of <paramref name="outputCount"/> entries, in column order.</returns>
    /// <remarks>
    /// Separate from <see cref="Score{TKernel}"/> and <see cref="PerOutput{TKernel}"/> so that a
    /// metric with a validation rule of its own — <see cref="MeanSquaredLogError"/> refuses a
    /// target at or below −1 — can run it between the shared check and the walk, rather than
    /// paying for a second validation pass to get in front of it.
    /// </remarks>
    public static double[] WeightedMean<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        int samples,
        TKernel kernel = default)
        where TKernel : struct, IResidualKernel
    {
        CompensatedSum[] sums = new CompensatedSum[outputCount];
        double total;

        if (sampleWeight.IsEmpty)
        {
            for (int row = 0; row < samples; row++)
            {
                int offset = row * outputCount;
                for (int col = 0; col < outputCount; col++)
                {
                    sums[col].Add(kernel.Apply(yTrue[offset + col], yPred[offset + col]));
                }
            }

            // Exact: n additions of 1.0 land on n for every n below 2^53, and this
            // repository's array-length guard is far below that.
            total = samples;
        }
        else
        {
            CompensatedSum totalWeight = default;
            for (int row = 0; row < samples; row++)
            {
                double weight = sampleWeight[row];
                totalWeight.Add(weight);
                int offset = row * outputCount;
                for (int col = 0; col < outputCount; col++)
                {
                    sums[col].Add(weight * kernel.Apply(yTrue[offset + col], yPred[offset + col]));
                }
            }
            total = totalWeight.Value;
        }

        double[] result = new double[outputCount];
        for (int col = 0; col < outputCount; col++)
        {
            result[col] = sums[col].Value / total;
        }

        return result;
    }

    /// <summary><see cref="WeightedMean{TKernel}"/> with a SIMD walk where the shape permits one.</summary>
    /// <typeparam name="TKernel">The per-pair quantity to average.</typeparam>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, row-major.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample, or empty for weight 1 each.</param>
    /// <param name="samples">The sample count <see cref="Validate"/> returned.</param>
    /// <param name="kernel">The kernel instance.</param>
    /// <remarks>
    /// Both conditions are <c>decisions/0027</c>'s, settled there for R² and explained
    /// variance: <c>outputCount == 1</c> is the only contiguous shape, a strided column
    /// being what a <see cref="Vector{T}"/> load cannot gather, and
    /// <see cref="Vector.IsHardwareAccelerated"/> is checked apart from it so a runtime
    /// emulating the type keeps the scalar loop.
    /// </remarks>
    private static double[] WeightedMeanVectorized<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        int samples,
        TKernel kernel)
        where TKernel : struct, IVectorResidualKernel
    {
#if NET5_0_OR_GREATER
        // Unweighted single-output input does not reach here unless Validate refused it:
        // ScoreVectorized and PerOutputVectorized walk it with TryUnweightedMean instead.
        if (outputCount == 1 && !sampleWeight.IsEmpty && Vector.IsHardwareAccelerated)
        {
            return [SingleOutputWeightedVectorized(yTrue, yPred, sampleWeight, samples, kernel)];
        }
#endif
        return WeightedMean(yTrue, yPred, outputCount, sampleWeight, samples, kernel);
    }

#if NET5_0_OR_GREATER
    /// <summary>The weighted mean over one contiguous output, accumulated per lane.</summary>
    /// <remarks>
    /// Not guaranteed bit-identical with the scalar loop, for the reason
    /// <see cref="VectorCompensatedSum"/> gives: each lane is Neumaier-exact on its own
    /// terms and the lanes are combined in a different order. Both sides pass the
    /// oracle corpus at its 1e-9 comparison.
    /// </remarks>
    private static double SingleOutputWeightedVectorized<TKernel>(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        ReadOnlySpan<double> sampleWeight,
        int samples,
        TKernel kernel)
        where TKernel : struct, IVectorResidualKernel
    {
        int width = Vector<double>.Count;
        VectorCompensatedSum acc = default;
        VectorCompensatedSum weightAcc = default;
        int i = 0;
        for (; i <= samples - width; i += width)
        {
            var weights = new Vector<double>(sampleWeight.Slice(i, width));
            weightAcc.Add(weights);
            acc.Add(weights * kernel.Apply(
                new Vector<double>(yTrue.Slice(i, width)),
                new Vector<double>(yPred.Slice(i, width))));
        }

        CompensatedSum weighted = acc.Reduce();
        CompensatedSum totalWeight = weightAcc.Reduce();
        for (; i < samples; i++)
        {
            double weight = sampleWeight[i];
            totalWeight.Add(weight);
            weighted.Add(weight * kernel.Apply(yTrue[i], yPred[i]));
        }

        return weighted.Value / totalWeight.Value;
    }
#endif

    /// <summary>Roots every entry in place, and hands the same array back.</summary>
    /// <param name="perOutput">One value per output, replaced by its square root.</param>
    /// <remarks>
    /// In place because the caller has just allocated it: the two rooted metrics
    /// each ask their squared counterpart for a fresh array and own it outright,
    /// so rooting a copy would allocate a second array for nothing.
    /// </remarks>
    public static double[] SquareRoots(double[] perOutput)
    {
        for (int i = 0; i < perOutput.Length; i++)
        {
            perOutput[i] = Math.Sqrt(perOutput[i]);
        }

        return perOutput;
    }

    /// <summary>
    /// <c>multioutput="uniform_average"</c> when <paramref name="outputWeights"/>
    /// is empty, and the weighted average scikit-learn computes for an array
    /// otherwise.
    /// </summary>
    /// <param name="perOutput">One value per output.</param>
    /// <param name="outputWeights">One weight per output, or empty.</param>
    public static double Reduce(double[] perOutput, ReadOnlySpan<double> outputWeights)
    {
        if (outputWeights.IsEmpty)
        {
            double sum = 0.0;
            foreach (double value in perOutput)
            {
                sum += value;
            }
            return sum / perOutput.Length;
        }

        double weighted = 0.0;
        double total = 0.0;
        for (int i = 0; i < perOutput.Length; i++)
        {
            weighted += perOutput[i] * outputWeights[i];
            total += outputWeights[i];
        }
        return weighted / total;
    }

    /// <summary>
    /// <c>multioutput="variance_weighted"</c>: each output counted in proportion
    /// to the variance of its own truth.
    /// </summary>
    /// <param name="perOutput">One score per output.</param>
    /// <param name="variances">The weighted variance of the truth, per output.</param>
    /// <remarks>
    /// When every variance is zero there is nothing to weight by, and
    /// scikit-learn falls back to the plain mean rather than dividing by zero:
    /// <c>if not xp.any(nonzero_denominator): avg_weights = None</c>,
    /// <c>sklearn/metrics/_regression.py:982-986</c> (scikit-learn 1.9.1).
    /// </remarks>
    public static double ReduceByVariance(double[] perOutput, double[] variances)
    {
        double total = 0.0;
        foreach (double variance in variances)
        {
            total += variance;
        }

        // S1244: the question is whether any variance accumulated at all, not
        // whether two computed quantities are close. A tolerance would reroute a
        // legitimately tiny variance into the fallback and change the value.
#pragma warning disable S1244
        if (total == 0.0)
#pragma warning restore S1244
        {
            return Reduce(perOutput, default);
        }

        double weighted = 0.0;
        for (int i = 0; i < perOutput.Length; i++)
        {
            weighted += perOutput[i] * variances[i];
        }
        return weighted / total;
    }
}
