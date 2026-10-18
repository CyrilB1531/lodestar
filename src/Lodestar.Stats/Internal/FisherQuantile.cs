namespace Lodestar.Stats.Internal;

/// <summary>The F quantiles from the dfs themselves: the beta inversion at their exact halves, lifted where a half would
/// round, and the root judged on F's own tail.</summary>
internal static class FisherQuantile
{
    // The relative miss of the kept tail within which an f is a root.
    private const double RootTolerance = 1e-9;

    /// <summary>The <em>F</em> root whose lower tail is <paramref name="lowerTarget"/> and upper <paramref name="upperTarget"/>.</summary>
    internal static double Invert(double lowerTarget, double upperTarget, double numeratorDf, double denominatorDf) =>
        ExactHalf.EitherBelow(numeratorDf, denominatorDf)
            ? LiftedFisherInverse(lowerTarget, upperTarget, numeratorDf, denominatorDf)
            : FisherFromTail(lowerTarget, upperTarget, numeratorDf, denominatorDf);

    /// <summary>An <em>F</em> quantile at a df below ExactHalf.Threshold, solved at the lifted shapes.</summary>
    /// <remarks>
    /// Both lifted, the tails are of degree zero and the targets stand; one lifted, its small tail is of degree one in it,
    /// so the lifted law's is the target lifted, and a lifted target past one, which no tail reaches, puts the root at
    /// the support's far end.
    /// </remarks>
    // Out of line, as Densities.UnderflowedFisher: inline, it cost FisherSnedecorQuantile(0.05, 4, 9) 8%, 584 against 538 ns.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LiftedFisherInverse(double lowerTarget, double upperTarget, double numeratorDf, double denominatorDf)
    {
        (double a, double b, bool lone, bool smallUpper) = ExactHalf.FisherShapes(numeratorDf, denominatorDf);
        double lower = lowerTarget;
        double upper = upperTarget;
        if (lone)
        {
            double lifted = ExactHalf.Lifted(smallUpper ? upperTarget : lowerTarget);
            if (lifted > 1.0)
            {
                return smallUpper ? 0.0 : double.PositiveInfinity;
            }
            (lower, upper) = smallUpper ? (1.0 - lifted, lifted) : (lifted, 1.0 - lifted);
        }
        return FisherFromTail(lowerTarget, upperTarget, lower, upper, numeratorDf, denominatorDf, (a, b));
    }

    /// <summary>An <em>F</em> quantile from the incomplete beta's inverse, with <c>y</c> and <c>1 − y</c> both exact.</summary>
    /// <remarks>
    /// With <c>y = d₁f / (d₁f + d₂)</c>, <c>P(F ≤ f) = I_y(d₁/2, d₂/2)</c> and <c>f = d₂y / (d₁(1 − y))</c>.
    /// A root near one would leave <c>1 − y</c> as one minus a number near one, so both come from
    /// <see cref="BetaQuantile.InvertBoth"/>, which solves on whichever side is small.
    /// </remarks>
    private static double FisherFromTail(double lower, double upper, double numeratorDf, double denominatorDf) =>
        FisherFromTail(lower, upper, lower, upper, numeratorDf, denominatorDf, (numeratorDf / 2.0, denominatorDf / 2.0));

    /// <summary><see cref="FisherFromTail(double, double, double, double)"/> at lifted shapes, the solved targets the lifted
    /// law's, the judged ones the caller's.</summary>
    private static double FisherFromTail(
        double lower, double upper, double solvedLower, double solvedUpper, double numeratorDf, double denominatorDf, (double A, double B) shapes)
    {
        (double a, double b) = shapes;
        // Judged on the caller's smaller tail; chosen from the lifted ones, a lifted tail past one half kept the other, one
        // less a tiny target, which rounds to one (#1729's Review A). The inversion and the limit solve the lifted targets.
        var target = new FisherTarget(lower, upper, lower <= upper, numeratorDf, denominatorDf);
        (double y, double complement) = BetaQuantile.InvertBoth(solvedLower, solvedUpper, a, b);
        double f = RootFromSplit(y, complement, numeratorDf, denominatorDf);
        f = target.AtCrossing(f);

        // A side below the smallest normal double keeps too few digits, and the half's limit inverts there instead,
        // kept where it holds the target closer: its leading term divides by a shape, ill-conditioned where it is flat.
        double miss = double.NaN;
        if (Beta.TakesLimit(y, complement))
        {
            miss = target.Miss(f);
            double fromLimit = Beta.FisherRootFromLimit(solvedLower, solvedUpper, numeratorDf, denominatorDf, a, b);
            double limitMiss = double.IsNaN(fromLimit) ? double.PositiveInfinity : target.Miss(fromLimit);
            if (limitMiss <= miss)
            {
                (f, miss) = (fromLimit, limitMiss);
            }
        }

        // Below Beta.SmallShape a root outside the doubles leaves a non-root, and an end strictly past the target is the
        // answer (#1690's review); past it that end's tail is within e^-74 of its limit, so no larger shape needs a look.
        if (numeratorDf / 2.0 >= Beta.SmallShape && denominatorDf / 2.0 >= Beta.SmallShape)
        {
            return f;
        }
        miss = double.IsNaN(miss) ? target.Miss(f) : miss;
        if (miss <= RootTolerance)
        {
            return f;
        }
        return AtTheEnds(f, target, numeratorDf, denominatorDf);
    }

    /// <summary><c>f = d₂y / (d₁(1 − y))</c>, as two products where both are normal; else from the ratios, which a product
    /// below the normal doubles would round, to zero at a df of 5e-324, reading +∞ or NaN (#1729's Review A).</summary>
    private static double RootFromSplit(double y, double complement, double numeratorDf, double denominatorDf)
    {
        double numerator = denominatorDf * y;
        double denominator = numeratorDf * complement;
        if (numerator >= NearZero.SmallestNormal && denominator >= NearZero.SmallestNormal)
        {
            return numerator / denominator;
        }
        // The dfs and the halves as mantissas and one exponent of two, applied last: the dfs' ratio was itself subnormal
        // at a denominator df of 2.8e-317, and the quantile 1.7e-3 off its tail; y over a subnormal complement overflowed.
        return PowerOfTwo.Quotient(stackalloc double[] { denominatorDf, y }, stackalloc double[] { numeratorDf, complement }, 0);
    }

    /// <summary>The root at an end of the support where a small shape puts it there, else <paramref name="f"/>.</summary>
    /// <remarks>
    /// Each end's tail is taken once. A target the tail reaches at the smallest double has its root at 0, and one it
    /// falls short of even at the largest has it at +∞: a root outside the doubles is an end, never a double near one
    /// (#1690). Met exactly at the smallest double, the root lies in (0, ε], read as the end too. At two tiny dfs the
    /// tail is flat to its last ulps: read strictly, such a target answered +∞, and judged on the start's miss alone, a
    /// target past the plateau answered 0 (#1729's Review A).
    /// </remarks>
    private static double AtTheEnds(double f, FisherTarget target, double numeratorDf, double denominatorDf)
    {
        if (numeratorDf / 2.0 < Beta.SmallShape && target.Reaches(target.Kept(double.Epsilon)))
        {
            return 0.0;
        }
        return denominatorDf / 2.0 < Beta.SmallShape && target.RootAbove(double.MaxValue)
            ? double.PositiveInfinity
            : f;
    }

    /// <summary>An F quantile's target, judged on its smaller tail, which a double keeps whole.</summary>
    /// <remarks>
    /// The ends are f's, not the beta's: at small shapes 1 - y at f = MaxValue is far above the smallest double. Judged
    /// on the tail the caller passed, a target of 1 - 1e-12 hid a miss of 1e-12 and FisherSnedecorIsf(1 - 1e-12, 0.001,
    /// 1e-300) read 0 for +inf (#1690's review).
    /// </remarks>
    private readonly struct FisherTarget(double lower, double upper, bool keepsLower, double numeratorDf, double denominatorDf)
        : TailInversion.IUlpStep
    {
        // A log-F standard deviation of 1e-8, squared: below it f's last ulps move a deep tail by more than 1e-6, as at
        // (1.15e-271, 1.44e24, 5.39e23), 0.6% off when the walk began at 1e-12 (#1690's review).
        private const double NarrowLogFSquared = 1e-16;

        // The variance per unit of z^2 under which an ulp moves the tail by 1e-10: z eps / sd > 1e-10.
        private const double DeepTailVariance = 4.9e-12;

        // Its bound at the smallest tail, z^2 = 2 * 745: past it no log is taken, as at any ordinary df.
        private const double DeepTailCeiling = 7.3e-9;

        // The kept target alone, so four fields, the most ShapeLogs' remark found the JIT promotes, a fifth costing 8% (#1690).
        private readonly double _keptTarget = keepsLower ? lower : upper;

        /// <summary>The relative distance of the kept tail at <paramref name="f"/> from its target.</summary>
        internal double Miss(double f) => Math.Abs(Kept(f) - _keptTarget) / _keptTarget;

        /// <summary>Whether the root lies strictly below <paramref name="f"/>: its kept tail there already past the target.</summary>
        internal bool RootBelow(double f) => Passes(Kept(f));

        /// <summary>Whether the root lies strictly above <paramref name="f"/>; a tail equal to the target there is a root,
        /// as at shapes 1e-310, where the tail at the largest double is one half to rounding.</summary>
        internal bool RootAbove(double f)
        {
            // A NaN tail is no evidence either way, as main read it: the root is not above.
            double kept = Kept(f);
            return !double.IsNaN(kept) && !Reaches(kept);
        }

        /// <summary>The f at which F's own tail crosses the target, walked to by up to eight ulps each way, where the law is narrow.</summary>
        /// <remarks>
        /// f = d₂y/(d₁(1 − y)) rounds three times, so the root in y lands up to four ulps of f from where the tail
        /// crosses, which a law narrower than 1e-12 turns into its whole tail: FisherSnedecorQuantile(0.144, 4.7e118,
        /// 4.5e282) read 0.9999999999999998, whose lower tail is 0 (#1690's review). log F's variance is 2/d1 + 2/d2.
        /// </remarks>
        internal double AtCrossing(double f)
        {
            // An ulp of f moves the kept tail by about z eps / sd, z^2 about -2 log of the tail: deep in it a law of width
            // 1e-6 already moved 3e-9 an ulp, past the 1e-9 Miss accepts, at (2e-55, 1.5e12, 9.6e204) (#1690's review).
            double variance = (2.0 / numeratorDf) + (2.0 / denominatorDf);
            if (variance >= DeepTailCeiling
                || (variance >= NarrowLogFSquared && variance >= -2.0 * Math.Log(_keptTarget) * DeepTailVariance))
            {
                return f;
            }
            FisherTarget self = this;
            f = TailInversion.Walk(f, down: true, 8, ref self);
            return TailInversion.Walk(f, down: false, 8, ref self);
        }

        /// <summary>Whether a kept tail of <paramref name="value"/> lies strictly past the target.</summary>
        internal bool Passes(double value) => keepsLower ? value > _keptTarget : value < _keptTarget;

        /// <summary>Whether a kept tail of <paramref name="value"/> has reached the target, equal to it included.</summary>
        internal bool Reaches(double value) => keepsLower ? value >= _keptTarget : value <= _keptTarget;

        /// <summary>A crossing walk's rule: down while the root lies below, up while above.</summary>
        public bool Takes(double from, double to) => to < from ? RootBelow(from) : RootAbove(from);

        /// <summary>The kept tail at <paramref name="f"/>.</summary>
        internal double Kept(double f) => keepsLower
            ? Beta.FisherSnedecorCdf(f, numeratorDf, denominatorDf)
            : Beta.FisherSnedecorSf(f, numeratorDf, denominatorDf);
    }
}
