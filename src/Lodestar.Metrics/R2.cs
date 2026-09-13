#if NET5_0_OR_GREATER
using System.Numerics;
using System.Runtime.InteropServices;
#endif
using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// The coefficient of determination — the equivalent of
/// <c>sklearn.metrics.r2_score</c>.
/// </summary>
/// <remarks>
/// <c>forceFinite</c> and <see cref="ZeroDivision"/> answer two different
/// undefined cases and must not be merged into one. See
/// docs/decisions/0026.
/// </remarks>
public static class R2
{
    /// <summary>
    /// One number for the whole prediction —
    /// <c>r2_score(y_true, y_pred, sample_weight=…, multioutput=…, force_finite=…)</c>.
    /// </summary>
    /// <param name="yTrue">The true values, row-major when there is more than one output.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds. One, the default, is the ordinary case.</param>
    /// <param name="sampleWeight">A weight per sample — per <em>row</em>, not per value. Omit to weight every sample by 1.</param>
    /// <param name="outputWeights">A weight per output (<c>multioutput=[…]</c>). Omit for <c>multioutput="uniform_average"</c>.</param>
    /// <param name="forceFinite">
    /// scikit-learn's <c>force_finite</c>, which answers a truth of zero variance over two
    /// or more samples and nothing else. Pass <see langword="false"/> for the unclamped <c>nan</c> and <c>-inf</c>.
    /// </param>
    /// <param name="zeroDivision">
    /// What to answer when there are fewer than two samples, which is the only case
    /// scikit-learn leaves undefined regardless of <paramref name="forceFinite"/>. The default reproduces its <c>nan</c>.
    /// </param>
    /// <exception cref="ArgumentException">A length disagrees with the shape, the input is empty, or it holds a non-finite value.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outputCount"/> is below one.</exception>
    /// <exception cref="UndefinedMetricException">
    /// There are fewer than two samples and <paramref name="zeroDivision"/> is
    /// <see cref="ZeroDivision.Throw"/>.
    /// </exception>
    public static double Score(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount = 1,
        ReadOnlySpan<double> sampleWeight = default,
        ReadOnlySpan<double> outputWeights = default,
        bool forceFinite = true,
        ZeroDivision zeroDivision = ZeroDivision.NaN)
    {
        (double[] scores, _) =
            ValidateAndCompute(yTrue, yPred, outputCount, sampleWeight, outputWeights, forceFinite, zeroDivision);
        return Outputs.Reduce(scores, outputWeights);
    }

    /// <summary>
    /// One number per output — <c>multioutput="raw_values"</c>.
    /// </summary>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample. Omit to weight every sample by 1.</param>
    /// <param name="forceFinite">scikit-learn's <c>force_finite</c>. See <see cref="Score"/>.</param>
    /// <param name="zeroDivision">The answer for fewer than two samples. See <see cref="Score"/>.</param>
    /// <returns>A fresh array of <paramref name="outputCount"/> entries, in column order.</returns>
    /// <exception cref="ArgumentException">A length disagrees with the shape, the input is empty, or it holds a non-finite value.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outputCount"/> is below one.</exception>
    /// <exception cref="UndefinedMetricException">
    /// There are fewer than two samples and <paramref name="zeroDivision"/> is
    /// <see cref="ZeroDivision.Throw"/>.
    /// </exception>
    public static double[] PerOutput(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount = 1,
        ReadOnlySpan<double> sampleWeight = default,
        bool forceFinite = true,
        ZeroDivision zeroDivision = ZeroDivision.NaN)
    {
        return ValidateAndCompute(yTrue, yPred, outputCount, sampleWeight, default, forceFinite, zeroDivision).Scores;
    }

    /// <summary>
    /// One number, each output counted in proportion to the variance of its own
    /// truth — <c>multioutput="variance_weighted"</c>.
    /// </summary>
    /// <param name="yTrue">The true values, row-major.</param>
    /// <param name="yPred">The predicted values, same length as <paramref name="yTrue"/>.</param>
    /// <param name="outputCount">How many outputs each row holds.</param>
    /// <param name="sampleWeight">A weight per sample. Omit to weight every sample by 1.</param>
    /// <param name="forceFinite">scikit-learn's <c>force_finite</c>. See <see cref="Score"/>.</param>
    /// <param name="zeroDivision">The answer for fewer than two samples. See <see cref="Score"/>.</param>
    /// <exception cref="ArgumentException">A length disagrees with the shape, the input is empty, or it holds a non-finite value.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outputCount"/> is below one.</exception>
    /// <exception cref="UndefinedMetricException">
    /// There are fewer than two samples and <paramref name="zeroDivision"/> is
    /// <see cref="ZeroDivision.Throw"/>.
    /// </exception>
    /// <remarks>
    /// A method rather than a member of an averaging enum: the weights are this computation's
    /// own per-output variances, produced by the same pass as the scores and not recoverable from them. See docs/decisions/0021.
    /// </remarks>
    public static double VarianceWeighted(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight = default,
        bool forceFinite = true,
        ZeroDivision zeroDivision = ZeroDivision.NaN)
    {
        (double[] scores, double[] denominators) =
            ValidateAndCompute(yTrue, yPred, outputCount, sampleWeight, default, forceFinite, zeroDivision);
        return Outputs.ReduceByVariance(scores, denominators);
    }

    private static (double[] Scores, double[] Denominators) ValidateAndCompute(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        ReadOnlySpan<double> outputWeights,
        bool forceFinite,
        ZeroDivision zeroDivision)
    {
        // A single contiguous output is the only shape that vectorizes. See docs/decisions/0027.
        if (outputCount == 1 && sampleWeight.IsEmpty && Outputs.OnlyTargetsNeedScanning(yTrue, yPred, outputWeights)
            && TryAccumulateSingleOutput(yTrue, yPred, out CompensatedSum numerator, out CompensatedSum centredSquare))
        {
            double denominator = centredSquare.Value;
            return ([Resolve(numerator.Value, denominator, yTrue.Length, forceFinite, zeroDivision)], [denominator]);
        }

        int samples = Outputs.Validate(yTrue, yPred, outputCount, sampleWeight, outputWeights);
        return Compute(yTrue, yPred, outputCount, sampleWeight, samples, forceFinite, zeroDivision);
    }

    // One pass returns both arrays: VarianceWeighted needs the denominators,
    // which cannot be recovered from the scores alone.
    private static (double[] Scores, double[] Denominators) Compute(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int outputCount,
        ReadOnlySpan<double> sampleWeight,
        int samples,
        bool forceFinite,
        ZeroDivision zeroDivision)
    {
        double[] scores = new double[outputCount];
        double[] denominators = new double[outputCount];
        CompensatedSum[] numerators = new CompensatedSum[outputCount];
        CompensatedSum[] centredSquares = new CompensatedSum[outputCount];

        if (sampleWeight.IsEmpty)
        {
            AccumulateUnweighted(yTrue, yPred, samples, numerators, centredSquares);
        }
        else
        {
            AccumulateWeighted(yTrue, yPred, sampleWeight, samples, numerators, centredSquares);
        }

        for (int col = 0; col < outputCount; col++)
        {
            denominators[col] = centredSquares[col].Value;
            scores[col] = Resolve(numerators[col].Value, denominators[col], samples, forceFinite, zeroDivision);
        }
        return (scores, denominators);
    }

    // meanSums stays local: Compute never sees it, so threading it through as
    // a parameter would only spend S107's budget for nothing.
    private static void AccumulateUnweighted(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        int samples,
        CompensatedSum[] numerators,
        CompensatedSum[] centredSquares)
    {
        int outputCount = numerators.Length;

        CompensatedSum[] meanSums = new CompensatedSum[outputCount];
        for (int row = 0; row < samples; row++)
        {
            int offset = row * outputCount;
            for (int col = 0; col < outputCount; col++)
            {
                meanSums[col].Add(yTrue[offset + col]);
            }
        }

        // No total to accumulate: the sum of n ones is exactly n below 2^53,
        // so the mean divides by samples directly.
        double[] means = new double[outputCount];
        for (int col = 0; col < outputCount; col++)
        {
            means[col] = meanSums[col].Value / samples;
        }

        for (int row = 0; row < samples; row++)
        {
            int offset = row * outputCount;
            for (int col = 0; col < outputCount; col++)
            {
                double residual = yTrue[offset + col] - yPred[offset + col];
                double centred = yTrue[offset + col] - means[col];
                numerators[col].Add(residual * residual);
                centredSquares[col].Add(centred * centred);
            }
        }
    }

    /// <summary>
    /// R²'s two sums over one unweighted output, which also report whether every target was finite.
    /// </summary>
    /// <param name="yTrue">The true values, as long as <paramref name="yPred"/> and not empty.</param>
    /// <param name="yPred">The predicted values.</param>
    /// <param name="numerator">The sum of squared residuals.</param>
    /// <param name="centredSquare">The sum of squared deviations of the truth from its mean.</param>
    /// <returns>Whether both spans were finite; when not, the sums mean nothing.</returns>
    /// <remarks>
    /// The mean pass reads the truth and the second pass both spans, each testing what it reads, in
    /// place of two more passes spent only on validation. Per lane on <c>net10.0</c> and in four
    /// stripes elsewhere; not guaranteed bit-identical with the multi-output loop, for
    /// <c>docs/decisions/0033</c>'s reason.
    /// </remarks>
    private static bool TryAccumulateSingleOutput(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        out CompensatedSum numerator,
        out CompensatedSum centredSquare)
    {
#if NET5_0_OR_GREATER
        bool truthFinite = Vector.IsHardwareAccelerated
            ? TryLaneTotal(yTrue, out CompensatedSum meanSum, out int i)
            : TryStripeTotal(yTrue, out meanSum, out i);
#else
        bool truthFinite = TryStripeTotal(yTrue, out CompensatedSum meanSum, out int i);
#endif
        int samples = yTrue.Length;
        long nonFinite = 0;
        for (; i < samples; i++)
        {
            nonFinite |= Inputs.NonFiniteBits(yTrue[i]);
            meanSum.Add(yTrue[i]);
        }

        // No total to accumulate: the sum of n ones is exactly n below 2^53.
        double mean = meanSum.Value / samples;
#if NET5_0_OR_GREATER
        bool predictionFinite = Vector.IsHardwareAccelerated
            ? TryLaneSquares(yTrue, yPred, mean, out numerator, out centredSquare, out i)
            : TryStripeSquares(yTrue, yPred, mean, out numerator, out centredSquare, out i);
#else
        bool predictionFinite = TryStripeSquares(yTrue, yPred, mean, out numerator, out centredSquare, out i);
#endif
        for (; i < samples; i++)
        {
            nonFinite |= Inputs.NonFiniteBits(yPred[i]);
            double residual = yTrue[i] - yPred[i];
            double centred = yTrue[i] - mean;
            numerator.Add(residual * residual);
            centredSquare.Add(centred * centred);
        }

        return truthFinite && predictionFinite && nonFinite == 0;
    }

#if NET5_0_OR_GREATER
    /// <summary>The whole blocks of <paramref name="values"/>, added per lane.</summary>
    /// <param name="values">The values to add.</param>
    /// <param name="sum">The lanes, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the blocks covered.</param>
    /// <returns>Whether every element the blocks covered was finite.</returns>
    private static bool TryLaneTotal(ReadOnlySpan<double> values, out CompensatedSum sum, out int consumed)
    {
        ReadOnlySpan<Vector<double>> blocks = MemoryMarshal.Cast<double, Vector<double>>(values);
        VectorCompensatedSum acc = default;
        Vector<long> laneBits = Vector<long>.Zero;
        foreach (Vector<double> block in blocks)
        {
            laneBits |= Inputs.NonFiniteBits(block);
            acc.Add(block);
        }

        sum = acc.Reduce();
        consumed = blocks.Length * Vector<double>.Count;
        return laneBits == Vector<long>.Zero;
    }

    /// <summary>The second pass's whole blocks, per lane; only the prediction is still untested.</summary>
    /// <param name="yTrue">The true values, already tested by the mean pass.</param>
    /// <param name="yPred">The predicted values, as long as <paramref name="yTrue"/>.</param>
    /// <param name="mean">The truth's mean.</param>
    /// <param name="numerator">The squared residuals, reduced to one sum.</param>
    /// <param name="centredSquare">The squared deviations from the mean, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the blocks covered.</param>
    /// <returns>Whether every prediction the blocks covered was finite.</returns>
    private static bool TryLaneSquares(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        double mean,
        out CompensatedSum numerator,
        out CompensatedSum centredSquare,
        out int consumed)
    {
        ReadOnlySpan<Vector<double>> truths = MemoryMarshal.Cast<double, Vector<double>>(yTrue);
        ReadOnlySpan<Vector<double>> predictions = MemoryMarshal.Cast<double, Vector<double>>(yPred);
        var meanVec = new Vector<double>(mean);
        VectorCompensatedSum numeratorAcc = default;
        VectorCompensatedSum centredSquareAcc = default;
        Vector<long> laneBits = Vector<long>.Zero;
        for (int block = 0; block < truths.Length; block++)
        {
            Vector<double> truth = truths[block];
            Vector<double> prediction = predictions[block];
            laneBits |= Inputs.NonFiniteBits(prediction);
            Vector<double> residual = truth - prediction;
            Vector<double> centred = truth - meanVec;
            numeratorAcc.Add(residual * residual);
            centredSquareAcc.Add(centred * centred);
        }

        numerator = numeratorAcc.Reduce();
        centredSquare = centredSquareAcc.Reduce();
        consumed = truths.Length * Vector<double>.Count;
        return laneBits == Vector<long>.Zero;
    }
#endif

    /// <summary>The whole groups of four of <paramref name="values"/>, one stripe per element.</summary>
    /// <param name="values">The values to add.</param>
    /// <param name="sum">The stripes, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the groups covered.</param>
    /// <returns>Whether every element the groups covered was finite.</returns>
    /// <remarks>Internal so the <c>net10.0</c> tests, where the lanes route around it, reach it.</remarks>
    internal static bool TryStripeTotal(ReadOnlySpan<double> values, out CompensatedSum sum, out int consumed)
    {
        StripedCompensatedSum acc = default;
        long nonFinite = 0;
        int i = 0;
        for (; i <= values.Length - 4; i += 4)
        {
            nonFinite |= Inputs.NonFiniteBits(values[i]) | Inputs.NonFiniteBits(values[i + 1])
                | Inputs.NonFiniteBits(values[i + 2]) | Inputs.NonFiniteBits(values[i + 3]);
            acc.Add(values[i], values[i + 1], values[i + 2], values[i + 3]);
        }

        sum = acc.Reduce();
        consumed = i;
        return nonFinite == 0;
    }

    /// <summary>The second pass's whole groups of four, one stripe per element.</summary>
    /// <param name="yTrue">The true values, already tested by the mean pass.</param>
    /// <param name="yPred">The predicted values, as long as <paramref name="yTrue"/>.</param>
    /// <param name="mean">The truth's mean.</param>
    /// <param name="numerator">The squared residuals, reduced to one sum.</param>
    /// <param name="centredSquare">The squared deviations from the mean, reduced to one sum.</param>
    /// <param name="consumed">How many leading elements the groups covered.</param>
    /// <returns>Whether every prediction the groups covered was finite.</returns>
    /// <remarks>Internal so the <c>net10.0</c> tests, where the lanes route around it, reach it.</remarks>
    internal static bool TryStripeSquares(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        double mean,
        out CompensatedSum numerator,
        out CompensatedSum centredSquare,
        out int consumed)
    {
        StripedCompensatedSum numeratorAcc = default;
        StripedCompensatedSum centredSquareAcc = default;
        long nonFinite = 0;
        int i = 0;
        for (; i <= yTrue.Length - 4; i += 4)
        {
            nonFinite |= Inputs.NonFiniteBits(yPred[i]) | Inputs.NonFiniteBits(yPred[i + 1])
                | Inputs.NonFiniteBits(yPred[i + 2]) | Inputs.NonFiniteBits(yPred[i + 3]);
            double r0 = yTrue[i] - yPred[i];
            double r1 = yTrue[i + 1] - yPred[i + 1];
            double r2 = yTrue[i + 2] - yPred[i + 2];
            double r3 = yTrue[i + 3] - yPred[i + 3];
            numeratorAcc.Add(r0 * r0, r1 * r1, r2 * r2, r3 * r3);
            double c0 = yTrue[i] - mean;
            double c1 = yTrue[i + 1] - mean;
            double c2 = yTrue[i + 2] - mean;
            double c3 = yTrue[i + 3] - mean;
            centredSquareAcc.Add(c0 * c0, c1 * c1, c2 * c2, c3 * c3);
        }

        numerator = numeratorAcc.Reduce();
        centredSquare = centredSquareAcc.Reduce();
        consumed = i;
        return nonFinite == 0;
    }

    private static void AccumulateWeighted(
        ReadOnlySpan<double> yTrue,
        ReadOnlySpan<double> yPred,
        ReadOnlySpan<double> sampleWeight,
        int samples,
        CompensatedSum[] numerators,
        CompensatedSum[] centredSquares)
    {
        int outputCount = numerators.Length;
        CompensatedSum[] meanSums = new CompensatedSum[outputCount];
        CompensatedSum totalWeight = default;
        for (int row = 0; row < samples; row++)
        {
            double weight = sampleWeight[row];
            totalWeight.Add(weight);
            int offset = row * outputCount;
            for (int col = 0; col < outputCount; col++)
            {
                meanSums[col].Add(weight * yTrue[offset + col]);
            }
        }

        double total = totalWeight.Value;
        double[] means = new double[outputCount];
        for (int col = 0; col < outputCount; col++)
        {
            means[col] = meanSums[col].Value / total;
        }

        for (int row = 0; row < samples; row++)
        {
            double weight = sampleWeight[row];
            int offset = row * outputCount;
            for (int col = 0; col < outputCount; col++)
            {
                double residual = yTrue[offset + col] - yPred[offset + col];
                double centred = yTrue[offset + col] - means[col];
                numerators[col].Add(weight * residual * residual);
                centredSquares[col].Add(weight * centred * centred);
            }
        }
    }

    /// <summary>
    /// The two undefined cases, which do not overlap and must not be merged.
    /// </summary>
    /// <remarks>
    /// Fewer than two samples is <c>nan</c> in scikit-learn under either setting of
    /// <c>force_finite</c>, so it is <see cref="ZeroDivision"/>'s case alone. A denominator of
    /// zero over two or more samples is <paramref name="forceFinite"/>'s alone: 1 when the
    /// numerator vanished too, 0 otherwise, or <c>nan</c> and <c>-inf</c> for the unclamped values.
    /// </remarks>
    private static double Resolve(
        double numerator, double denominator, int samples, bool forceFinite, ZeroDivision zeroDivision)
    {
        if (samples < 2)
        {
            return Prf.Undefined(zeroDivision, "R²");
        }

        // S1244: whether the variance collapsed at all, not whether two computed
        // quantities are close. scikit-learn tests the same quantity against
        // exact zero, and a tolerance would reroute a legitimately tiny variance.
#pragma warning disable S1244
        if (denominator != 0.0)
        {
#pragma warning restore S1244
            return 1.0 - (numerator / denominator);
        }

        // S1244: same question one line down — whether the residuals vanished
        // exactly, which is what separates scikit-learn's 1 from its 0 (and its
        // nan from its -inf). A tolerance would call a small-but-real error
        // perfect.
#pragma warning disable S1244
        bool perfect = numerator == 0.0;
#pragma warning restore S1244
        if (forceFinite)
        {
            return perfect ? 1.0 : 0.0;
        }
        return perfect ? double.NaN : double.NegativeInfinity;
    }
}
