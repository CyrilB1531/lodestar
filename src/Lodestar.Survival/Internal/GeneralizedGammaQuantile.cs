using Lodestar.Stats;

namespace Lodestar.Survival.Internal;

/// <summary>The generalized gamma law's log time, standardized: <c>w = log(λ² G) / λ</c>, G the gamma quantile at shape
/// 1/λ², whose percentile is <c>exp(μ + σ w)</c>.</summary>
/// <remarks>
/// Below a shape of <see cref="UniformShape"/> G holds only <c>ε</c> of a spread of <c>1/|λ|</c>, so w took <c>ε/|λ|</c>:
/// at λ = 1e-10 7.5e-7 off its log-normal limit, at 1e-50 +∞ (#1729's Review A). There w solves Temme's uniform
/// expansion of the tail, <c>Φ̄(η/λ) + λ φ(η/λ) Σ C_k(η) λ^(2k)</c>, with <c>η²/2 = e^t − 1 − t</c> at <c>t = λ w</c>, in
/// which no G is formed; its C_k, TemmeSeries' exact series of degree 60, hold to η = 1.9, 0.05 times 38 standard
/// deviations, with 1e-17 to spare.
/// </remarks>
internal static class GeneralizedGammaQuantile
{
    // Below it the expansion's terms past C6 fall under 1e-16; past it the gamma quantile's ε/|λ| held w to 1.3e-14 at
    // λ = 0.05, p = 0.01, the step across it, and its |η| would pass SeriesReach in the deep tails.
    private const double UniformShape = 0.05;

    // The |η| to which TemmeSeries' degree-60 rows hold 1e-13 and their sign: the roots reach 1.93, at 0.05 times 38.
    internal const double SeriesReach = 2.2;

    // The rows of TemmeSeries read: past C6 a term falls under 1e-16 below UniformShape.
    private const int Rows = 7;

    // Newton's steps in the log of the tail, or halvings of the bracket, which gain one bit each.
    private const int MaxSteps = 200;

    // Past Special.MillsFrom Mills' ratio's asymptotic series, Special.MillsTerms long, holds 3e-26.
    private const double MillsFrom = Special.MillsFrom;

    /// <summary>w for the survival probability <paramref name="probability"/> at shape <paramref name="lambda"/>.</summary>
    internal static double Standardized(double lambda, double probability)
    {
        if (!(Math.Abs(lambda) < UniformShape))
        {
            return FromChiSquared(lambda, probability);
        }
        double w = Distributions.NormalIsf(probability);
        // S1244: the log-normal limit itself, where the expansion's every correction is zero.
#pragma warning disable S1244
        if (lambda == 0.0 || double.IsInfinity(w))
#pragma warning restore S1244
        {
            return w;
        }
        double start = w - (lambda * ((w * w) + 2.0) / 6.0);
        // S1244: a first correction under half an ulp leaves the normal quantile, which the solve would leave by the
        // normal tail's rounding instead: at λ = 1e-300 and p = 0.1 it read 3 ulps from it.
#pragma warning disable S1244
        return start == w ? w : Solve(lambda, probability, w, start);
#pragma warning restore S1244
    }

    /// <summary>w from the chi-squared quantile at df 2/λ², which holds it to ε/|λ|; a NaN shape is refused there, as
    /// main refused it.</summary>
    private static double FromChiSquared(double lambda, double probability)
    {
        double square = lambda * lambda;
        double chiSquared = lambda > 0.0
            ? Distributions.ChiSquaredIsf(probability, 2.0 * (1.0 / square))
            : Distributions.ChiSquaredQuantile(probability, 2.0 * (1.0 / square));
        // The gamma quantile is half the chi-squared one, halved with λ², exactly.
        return Math.Log(chiSquared * (square / 2.0)) / lambda;
    }

    /// <summary>The w whose tail is <paramref name="probability"/>, from <paramref name="w"/>: Newton within a bracket the
    /// tail narrows, a step leaving it, as one from the normal quantile at p = 1e-300 did to −∞, halving it instead.</summary>
    private static double Solve(double lambda, double probability, double anchor, double w)
    {
        // Past one half on the lower side, against 1 − p, exact there: the upper tail near one held only ε of it, and w
        // missed by ε/φ, 3e-9 at p = 1 − 3.5e-10.
        bool lower = probability > 0.5;
        double target = lower ? 1.0 - probability : probability;
        double below = double.NegativeInfinity;
        double above = double.PositiveInfinity;
        // A subnormal target is met in logs throughout: its tail kept a few bits, and w at p = 5e-324 was 6e-3 off.
        bool deep = !lower && target < NearZero.SmallestNormal;
        double logTarget = Math.Log(target);
        double previous = anchor;
        for (int i = 0; i < MaxSteps; i++)
        {
            // In logs, the tail's own scale: on the tail itself a step from 2.8 off at p = 1e-250 gained a nat, and 200
            // ran out at w = 39.8 for 26.5 (#1729's Review A). A tail of zero is −∞ there, which the bracket takes.
            (double miss, double scale) = deep ? DeepMiss(lambda, w, logTarget) : Miss(lambda, w, lower, target);
            // A NaN tail, φ underflowing or η past the series' reach, says nothing of the side: back halfway, the bracket
            // kept, where reading it as "below" had cut off the root.
            if (double.IsNaN(miss))
            {
                w = Back(w, previous, anchor);
                continue;
            }
            // S1244: the tail met exactly, which no step improves on.
#pragma warning disable S1244
            if (miss == 0.0)
#pragma warning restore S1244
            {
                return w;
            }
            previous = w;
            (double next, below, above) = Advance(w, miss, scale, below, above);
            double step = next - w;
            w = next;
            // Two ulps of w past one, 1e-16 |w| being under one past |w| = 32; below, 4.5e-16 absolute, as near one half the
            // tail resolves w to ε/φ, 1e-13 of w = 3e-4 at λ = ±0.001, about ε of the percentile at σ = 1.
            if (!(Math.Abs(step) > 4.5e-16 * Math.Max(1.0, Math.Abs(w))))
            {
                return w;
            }
        }
        // Unsettled within MaxSteps: the bracket's middle where it is closed, else the last w, never the chi-squared
        // quantile's, which overflows below 1.2e-154 and holds only ε/|λ| above.
        return double.IsInfinity(below) || double.IsInfinity(above) ? w : (below + above) / 2.0;
    }

    /// <summary>The log of the tail over the target, and the w a unit of it moves.</summary>
    private static (double Miss, double Scale) Miss(double lambda, double w, bool lower, double target)
    {
        (double tail, double slope) = Tail(lambda, w, lower);
        return (lower ? Math.Log(target / tail) : Math.Log(tail / target), tail / slope);
    }

    /// <summary><see cref="Miss"/> on the upper side against a target's log, the tail taken in logs.</summary>
    private static (double Miss, double Scale) DeepMiss(double lambda, double w, double logTarget)
    {
        (double logTail, double scale) = LogTail(lambda, w);
        return (logTail - logTarget, scale);
    }

    /// <summary>The next w and the bracket narrowed by w's side: Newton's step in the tail's log, <paramref name="miss"/>
    /// times <paramref name="scale"/>, where it stays inside, else the bracket's own step.</summary>
    private static (double Next, double Below, double Above) Advance(double w, double miss, double scale, double below, double above)
    {
        bool rootAbove = miss > 0.0;
        below = rootAbove ? w : below;
        above = rootAbove ? above : w;
        double newton = w + (miss * scale);
        // S1244: a step under half an ulp lands on w, the bracket's new end; read as leaving it, it jumped by |w|, and
        // w at λ = 1e-100, p = 2^-1022 settled 3 ulps off.
#pragma warning disable S1244
        bool inside = newton == w || (newton > below && newton < above);
#pragma warning restore S1244
        return (inside ? newton : Bracketed(w, below, above, rootAbove), below, above);
    }

    /// <summary>Halfway back from w toward the last w, or where there is none toward the anchor, the normal quantile.</summary>
    private static double Back(double w, double previous, double anchor)
    {
        // S1244: the first step's own w, which has no last one to fall back to.
#pragma warning disable S1244
        return (w + (previous == w ? anchor : previous)) / 2.0;
#pragma warning restore S1244
    }

    /// <summary>The bracket's middle, or past an open end a step of the larger of one and |w| toward the root.</summary>
    private static double Bracketed(double w, double below, double above, bool rootAbove)
    {
        if (double.IsInfinity(below) || double.IsInfinity(above))
        {
            double reach = Math.Max(1.0, Math.Abs(w));
            return rootAbove ? w + reach : w - reach;
        }
        return (below + above) / 2.0;
    }

    /// <summary>The survival probability at w, or its complement where <paramref name="lower"/>, and the magnitude of its
    /// slope, <c>φ(s) (dη/dt) (1 + λ s Σ − λ² Σ′)</c>, s being η/λ and Σ the series in λ².</summary>
    /// <remarks>η is t √h, h = 2(e^t − 1 − t)/t², so s = w √h forms no t²: at λ = −1e-160 t² was subnormal and the
    /// percentile 6e-5 off its limit.</remarks>
    private static (double Tail, double Slope) Tail(double lambda, double w, bool lower)
    {
        double t = lambda * w;
        double root = Math.Sqrt(GeneralizedGammaUniform.RatioValue(t));
        return TailAt(lambda, t, root, w * root, lower);
    }

    /// <summary><see cref="Tail"/> from t, √h and s = w √h already formed, which <see cref="LogTail"/> forms too.</summary>
    private static (double Tail, double Slope) TailAt(double lambda, double t, double root, double s, bool lower)
    {
        double eta = t * root;
        // Past it the series of degree 60 lose their digits, toward their radius 2√π; no root lies past 1.93.
        if (!(Math.Abs(eta) <= SeriesReach))
        {
            return (double.NaN, double.NaN);
        }
        double density = Distributions.NormalPdf(s);
        (double sum, double sumSlope) = Series(lambda, eta);
        double correction = lambda * density * sum;
        double tail = lower ? Distributions.NormalCdf(s) - correction : Distributions.NormalSf(s) + correction;
        // The correction's own slope as well: at λ = −0.049 and p = 1e-300 it is the size of Φ̄'s, and a step on Φ̄'s alone
        // overshot twice over and never settled (#1729's Review A).
        return (tail, density * Growth(t) / root * (1.0 + (lambda * s * sum) - ((lambda * lambda) * sumSlope)));
    }

    /// <summary>The upper tail's log at w and the w a unit of it moves: past <see cref="MillsFrom"/> as
    /// <c>log φ(s) + log(R(s) + λ Σ)</c>, R Mills' ratio, where neither φ nor the tail is formed.</summary>
    private static (double LogTail, double Inverse) LogTail(double lambda, double w)
    {
        double t = lambda * w;
        double root = Math.Sqrt(GeneralizedGammaUniform.RatioValue(t));
        double s = w * root;
        if (!(s >= MillsFrom) || !(Math.Abs(t * root) <= SeriesReach))
        {
            (double tail, double slope) = TailAt(lambda, t, root, s, false);
            return (Math.Log(tail), tail / slope);
        }
        (double sum, double sumSlope) = Series(lambda, t * root);
        double ratio = Mills(s) + (lambda * sum);
        double slopeFactor = Growth(t) / root * (1.0 + (lambda * s * sum) - ((lambda * lambda) * sumSlope));
        return (Special.NormalLogPdf(s) + Math.Log(ratio), ratio / slopeFactor);
    }

    /// <summary>Σ C_k(η) λ^(2k) over <see cref="Rows"/> rows, and its derivative in η.</summary>
    private static (double Sum, double Slope) Series(double lambda, double eta)
    {
        double square = lambda * lambda;
        double sum = 0.0;
        double sumSlope = 0.0;
        double power = 1.0;
        int terms = Terms(eta);
        for (int k = 0; k < Rows; k++)
        {
            // Past a λ^(2k) of zero, at λ = 1e-200 from C1 on, the series would add nothing.
            if (!(power > 0.0))
            {
                break;
            }
            (double value, double derivative) = TemmeSeries.Evaluate(k, terms, eta);
            sum += value * power;
            sumSlope += derivative * power;
            power *= square;
        }
        return (sum, sumSlope);
    }

    /// <summary>The terms of each row an |η| needs: its series fall as (|η|/2√π)^j, so a quarter, a half and one leave
    /// under 1e-21 of them past 20, 28 and 40, where the whole row cost each Newton step 427 multiply-adds.</summary>
    internal static int Terms(double eta)
    {
        double size = Math.Abs(eta);
        if (size <= 0.25)
        {
            return 20;
        }
        if (size <= 0.5)
        {
            return 28;
        }
        return size <= 1.0 ? 40 : TemmeSeries.Degree;
    }

    /// <summary>Mills' ratio Φ̄(s)/φ(s) for s past <see cref="MillsFrom"/>: <c>(1/s) Σ (−1)^k (2k−1)!! / s^(2k)</c>.</summary>
    private static double Mills(double s) => MillsSeries.Sum(1.0 / (s * s), Special.MillsTerms) / s;

    /// <summary><c>(e^t − 1)/t</c>, by expm1, and one at zero; NaN at NaN, so a NaN iterate is not hidden.</summary>
    private static double Growth(double t) => Math.Abs(t) > 0.0 || double.IsNaN(t) ? NearZero.Expm1(t) / t : 1.0;
}
