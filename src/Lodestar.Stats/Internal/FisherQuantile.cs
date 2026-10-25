namespace Lodestar.Stats.Internal;

/// <summary>The F quantiles from the dfs themselves: the beta inversion at their exact halves, lifted where a half would
/// round, and the root judged on F's own tail; below dfs of 6.2e-9, near the mode, a search of the doubles.</summary>
internal static class FisherQuantile
{
    // The relative miss of the kept tail within which an f is a root.
    private const double RootTolerance = 1e-9;

    // 2^20: past this many doubles per ulp of the target's tail the search takes some 40 tails, where 1,300 at dfs
    // (10, 0.001) take a few; an unknown width, the front zero or NaN, is searched.
    private const double FarRunWidth = 1048576.0;

    /// <summary>The <em>F</em> root whose lower tail, where <paramref name="givenLower"/> holds, else upper, is
    /// <paramref name="target"/>, the other side one less it.</summary>
    internal static double Invert(double target, bool givenLower, double numeratorDf, double denominatorDf)
    {
        // The median of equal dfs is one, the exact tail's one half there and below it only, answered without a search.
        if (IsEqualMedian(target, numeratorDf, denominatorDf))
        {
            return 1.0;
        }
        if (numeratorDf < Beta.FirstOrderAtModeDf && denominatorDf < Beta.FirstOrderAtModeDf)
        {
            double small = SmallShapesRoot(target, givenLower, numeratorDf, denominatorDf);
            if (!double.IsNaN(small))
            {
                return small;
            }
        }
        double lowerTarget = givenLower ? target : 1.0 - target;
        double upperTarget = givenLower ? 1.0 - target : target;
        // A root of 0 searched from ε, whose exact tail decides it: the inversion read the double forms' there, which can pass
        // a target the exact tail does not (#1745's Review A); AboveZero answers 0 where ε's tail is past the target.
        double root = Inverted(lowerTarget, upperTarget, numeratorDf, denominatorDf, out RootTail measured);
        if (!(root <= double.MaxValue))
        {
            return root;
        }
        bool keepsLower = KeepsLower(lowerTarget, upperTarget, givenLower);
        // The smallest double whose smaller tail before rounding, one less a target past ½ exactly, reaches it; monotone in f
        // (#1745). On the caller's side, whose ulps near one are 1e-4 of a tail of 1e-12, f.ppf(1 - 1e-12, 5, 5) read 2.2e-5 off.
        var tail = new FisherTarget(lowerTarget, upperTarget, keepsLower, numeratorDf, denominatorDf);
        root = StartAboveZero(root, tail, keepsLower, ref measured, out bool startsAtEpsilon);
        if (root <= 0.0)
        {
            return 0.0;
        }
        ExactTail exact = measured.Or(root, keepsLower, tail);
        if (KeepsRoot(Math.Min(lowerTarget, upperTarget), exact, numeratorDf, denominatorDf))
        {
            return AboveZero(BitConverter.DoubleToInt64Bits(root), tail, startsAtEpsilon);
        }
        // Newton steps on that tail unrounded land within an ulp or two, so the search takes two or three tails, not eight.
        // The root's front once, for the run's width and the first Newton step: twice it cost each quantile a split.
        var reaches = new Meeting(tail);
        double front = tail.Front(root);
        bool missed = tail.MissOf(exact.Rounded) > RootTolerance;
        bool searchesFar = SearchesFar(exact, tail.RunWidth(root, front), missed, numeratorDf, denominatorDf);
        double firstStep = tail.Stepped(root, exact, front);
        long nearest = TailInversion.Settled(root, exact, tail.Reaches(exact), ref tail, ref reaches, searchesFar, firstStep);
        if (nearest == TailInversion.NotSearched)
        {
            return AboveZero(BitConverter.DoubleToInt64Bits(root), tail, startsAtEpsilon);
        }
        // No double reaching the target, the root lies past them; else the search tested the bits it returns, and leaves ε's
        // end alone.
        return nearest < 0 ? double.PositiveInfinity : AboveZero(nearest, tail, startsAtEpsilon);
    }

    /// <summary>Whether the quantile judges the lower tail: the smaller target's, and at a tie, one half, the caller's side;
    /// on the lower, FisherSnedecorIsf(0.5, 0.05, 0.3) read 11 ulps high. Distributions' end check reads it too.</summary>
    internal static bool KeepsLower(double lowerTarget, double upperTarget, bool givenLower) =>
        lowerTarget < upperTarget || (givenLower && !(lowerTarget > upperTarget));

    /// <summary>Whether the inversion's root stands, as main's, its search too costly for what it would find.</summary>
    /// <remarks>A subnormal target with a subnormal tail at the root: below the granule the exact tail tells nothing, and the
    /// search for a run's end took 199 µs at FisherSnedecorIsf(5e-324, 1e-300, 2000), scipy's 26. And past a harmonic shape
    /// of 1000 below Temme's, where the fraction's double jumps thousands of ulps between runs of f: the search for the
    /// run the root lands beside took 59 us at dfs (165557, 8597663), scipy's 29 (Cyril, 2026-10-27, #1745's Review A).</remarks>
    private static bool KeepsRoot(double smallerTarget, ExactTail exact, double numeratorDf, double denominatorDf)
    {
        // A tail of 0 is no granule but a root short of a law narrower than an ulp: FisherSnedecorQuantile(1e-310, 1e40,
        // 1e41) kept 1 - 2^-53, whose tail is 0 and its neighbour's one half (#1746's Review A).
        if (smallerTarget < NearZero.SmallestNormal && exact.Rounded > 0.0 && exact.Rounded < NearZero.SmallestNormal)
        {
            return true;
        }
        return exact.InDouble && Beta.FisherFractionIsDouble(numeratorDf, denominatorDf);
    }

    /// <summary>Whether the search goes past the windows about the Newton steps' start and the root, or answers the root
    /// where the crossing is at neither nor their neighbours (Cyril, 2026-10-28, #1745's Review A).</summary>
    /// <remarks>Not where an ulp of the target spans over FarRunWidth doubles: a tail some ulp off moves a step 1e8 ulps
    /// there, and the search took 133 us at dfs (3e-9, 7e-9), scipy's 26. Nor at a lifted df's
    /// double-form tail, whose front at a subnormal half tells no width: 40 tails, 85 us at FisherSnedecorIsf(2^-1022,
    /// 1e-310, 2000), scipy's 26. A wide run where the root's tail <paramref name="missed"/> the target past RootTolerance
    /// is no flat tail but a front read far off a law narrower than an ulp: FisherSnedecorQuantile(0.49999999999977596,
    /// 7.49e34, 1.17e35) answered a root two ulps short, a tail of 1.2e-246 (#1745's Review A).</remarks>
    private static bool SearchesFar(ExactTail exact, double width, bool missed, double numeratorDf, double denominatorDf) =>
        (missed || !(width > FarRunWidth && width < double.PositiveInfinity))
            && !(exact.InDouble && ExactHalf.EitherBelow(numeratorDf, denominatorDf));

    /// <summary>The inversion's root; for a root of 0, ε, whose exact tail the settle decides from, or 0 where that tail
    /// already passes the target. Read once into <paramref name="measured"/>: the settle and its end read it twice more.</summary>
    private static double StartAboveZero(double root, in FisherTarget tail, bool keepsLower, ref RootTail measured, out bool startsAtEpsilon)
    {
        startsAtEpsilon = !(root > 0.0);
        if (!startsAtEpsilon)
        {
            return root;
        }
        ExactTail atEpsilon = tail.Exact(double.Epsilon);
        if (tail.PassedAtEpsilon(atEpsilon))
        {
            return 0.0;
        }
        measured = new RootTail(double.Epsilon, keepsLower, atEpsilon);
        return double.Epsilon;
    }

    /// <summary>The double of the bits <paramref name="high"/> a search found, or 0 where the search could not test ε.</summary>
    /// <remarks>From a guess the search does not test ε, which is 0 where its tail is past the target, as SmallShapesEnd has
    /// it: unchecked, Q(F(ε)⁻) at dfs (1e-12, 1e-12) read ε. The largest double it answers only where it tested it.</remarks>
    private static double SearchedEnds(long high, FisherTarget tail, bool endsTested) =>
        endsTested ? BitConverter.Int64BitsToDouble(high) : AboveZero(high, tail, epsilonKept: false);

    /// <summary>The double of the bits <paramref name="high"/>, or 0 where they are ε's and its tail is past the target;
    /// <paramref name="epsilonKept"/> where ε's tail has been read already and does not pass it.</summary>
    private static double AboveZero(long high, FisherTarget tail, bool epsilonKept) =>
        high == 1L && !epsilonKept && tail.PassedAtEpsilon() ? 0.0 : BitConverter.Int64BitsToDouble(high);

    /// <summary>The beta inversion's root, lifted where a df's half is not a double.</summary>
    private static double Inverted(double lowerTarget, double upperTarget, double numeratorDf, double denominatorDf, out RootTail measured) =>
        ExactHalf.EitherBelow(numeratorDf, denominatorDf)
            ? LiftedFisherInverse(lowerTarget, upperTarget, numeratorDf, denominatorDf, out measured)
            : FisherFromTail(lowerTarget, upperTarget, numeratorDf, denominatorDf, out measured);

    /// <summary>The exact kept tail an inversion read at its root, on the side <c>Lower</c> says, for Invert to take as
    /// read: read again, at dfs (1e-9, 1000), it cost a tail, 7 us of FisherSnedecorIsf(1e-10, …)'s 30, scipy's 27.</summary>
    private readonly record struct RootTail(double F, bool Lower, ExactTail Exact)
    {
        internal static RootTail None => new(double.NaN, false, default);

        /// <summary>This tail where it is the one at <paramref name="f"/>'s bits on that side, else <paramref name="tail"/>'s.</summary>
        internal ExactTail Or(double f, bool lower, in FisherTarget tail) =>
            Lower == lower && BitConverter.DoubleToInt64Bits(F) == BitConverter.DoubleToInt64Bits(f) ? Exact : tail.Exact(f);
    }

    /// <summary>An inversion's f, unless it overflowed to +∞: then, where the smaller tail at the largest double reaches its
    /// target, that double, which Invert settles to the smallest reaching it; else +∞.</summary>
    /// <remarks>
    /// Judged as FisherTarget judges it, on the smaller tail, which keeps the target whole: near one the caller's own side
    /// rounds, and a root past the doubles would read as one below them. An inversion overflowed past MaxValue came back
    /// unchecked where that tail reaches the target there: Isf(Sf(MaxValue, 1.07e-5, 0.215)) of 1.06e-37 answered +∞
    /// (#1746). A NaN tail there reaches nothing, and leaves +∞. Not where an end test has already answered +∞ from that
    /// tail: read again at every +∞, it cost FisherSnedecorIsf(0.01, 3e-9, 4e-9) 18%.
    /// </remarks>
    private static double OrBelowTheLargest(double f, FisherTarget tail) =>
        double.IsPositiveInfinity(f) && tail.ReachedAtLargest() ? double.MaxValue : f;

    /// <summary>Below dfs of 6.2e-9 where the first order holds at its root, where the caller's tail crosses the target,
    /// by steps doubling from that root then halving the doubles' bits; below 1e-11, where it holds at every f, an end for
    /// a target past the tail; one for the median of equal dfs; NaN else, for the inversion.</summary>
    /// <remarks>
    /// The inversion, within RootTolerance of a tail moving by a few ulps, read one ulp below a flat tail 1.0000007 for 0
    /// (#1743). Below 1e-11 the tail is monotone and the crossing the smallest double reaching the target; past it the tail
    /// falls by ulps (#1745). The caller's side: on one less a target near one, Q(F(x)) read an end at 651 of 69,620.
    /// </remarks>
    // Out of line, as LiftedFisherInverse, for Invert's size.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double SmallShapesRoot(double target, bool givenLower, double numeratorDf, double denominatorDf)
    {
        double end = LoneEnd(target, givenLower, numeratorDf, denominatorDf, out double twiceA, out double twiceB);
        if (!double.IsNaN(end))
        {
            return end;
        }
        // FisherTarget kept on the caller's side, not its smaller one: it keeps the target alone.
        var tail = new FisherTarget(target, target, givenLower, numeratorDf, denominatorDf);
        // Non-negative doubles order as their bit patterns: 0 never reaches the target, and the largest double, searched
        // for, does.
        long largest = BitConverter.DoubleToInt64Bits(double.MaxValue);
        long high;
        // From the first order's root, steps doubling until they bracket the target: halving the doubles from their ends
        // took 3.4 us at dfs (1e-9, 1e-9) and targets near one half, the inversion 1.3.
        double guess = FirstOrderGuess(target, givenLower, twiceA, twiceB, numeratorDf, denominatorDf);
        bool endsTested = false;
        if (guess > 0.0 && guess < double.MaxValue)
        {
            high = FromGuess(guess, tail);
            // None reaching it, the largest double tested by the search: read again, it cost a tail (#1745's Review A).
            if (high < 0)
            {
                return double.PositiveInfinity;
            }
        }
        else if (twiceA < 2.0 * Beta.SmallShapesEverywhere && twiceB < 2.0 * Beta.SmallShapesEverywhere)
        {
            // No first-order guess at dfs this small: the target is past the tail at an end, which SmallShapesEnd reads.
            end = SmallShapesEnd(tail);
            if (!double.IsNaN(end))
            {
                return end;
            }
            endsTested = true;
            var shortOf = new TailInversion.Sided<Meeting>(new Meeting(tail), false);
            high = TailInversion.HalveBits(0, largest, ref shortOf).Failed;
        }
        else
        {
            // Where it holds near the mode alone, the fraction's slow tail would be searched over a run of doubles too
            // wide: FisherSnedecorQuantile(Cdf(1e-100, 5e-9, 5e-9)) took 11.7 us for the inversion's 0.62 (#1745).
            return double.NaN;
        }
        return SearchedEnds(high, tail, endsTested);
    }

    /// <summary>The bits of the smallest double reaching the target, searched from <paramref name="guess"/> toward the
    /// side its tail is short of; −1 where none does.</summary>
    private static long FromGuess(double guess, FisherTarget tail)
    {
        // Settled as the inversion's root: searched from the guess alone, a crossing 1e8 ulps off took 160 us at dfs (5e-9,
        // 5e-9) and p an ulp past one half, scipy's 27 (#1745's Review A).
        ExactTail exact = tail.Exact(guess);
        var reaches = new Meeting(tail);
        return TailInversion.Settled(guess, exact, tail.Reaches(exact), ref tail, ref reaches);
    }

    /// <summary>Whether the target is one half at equal dfs, where the median is exactly one.</summary>
    private static bool IsEqualMedian(double target, double numeratorDf, double denominatorDf)
    {
        // S1244: equal dfs make the law of 1/F the law of F, whose tail at one is exactly one half, and so is the exact one's.
#pragma warning disable S1244
        return numeratorDf == denominatorDf && target == 0.5;
#pragma warning restore S1244
    }

    /// <summary>Twice the shapes the tail is taken at, <paramref name="twiceA"/> and <paramref name="twiceB"/>, and the end
    /// where one df lifted alone puts a small tail past 2^-200 out of the law's reach, as LiftedFisherInverse reads it; NaN
    /// else.</summary>
    /// <remarks>Searched for, FisherSnedecorQuantile(0.5, 1e-310, 1e-9) took 302 ns, 33 here, for main's 30. Both lifted, the tail is
    /// the lifted shapes' first order; one alone, the dfs' own, whose halves a subnormal df's rounding would move.</remarks>
    private static double LoneEnd(double target, bool givenLower, double numeratorDf, double denominatorDf, out double twiceA, out double twiceB)
    {
        twiceA = numeratorDf;
        twiceB = denominatorDf;
        if (!ExactHalf.EitherBelow(numeratorDf, denominatorDf))
        {
            return double.NaN;
        }
        (double liftedA, double liftedB, bool lone, bool smallUpper) = ExactHalf.FisherShapes(numeratorDf, denominatorDf);
        if (!lone)
        {
            (twiceA, twiceB) = (2.0 * liftedA, 2.0 * liftedB);
            return double.NaN;
        }
        // The small tail's target as given where it is the caller's: one less one less it lost a target below 2^-54.
        return LiftedEnd(ExactHalf.Lifted(smallUpper == givenLower ? 1.0 - target : target), smallUpper);
    }

    /// <summary>The end a lone lifted df's small tail answers, its target lifted to <paramref name="lifted"/>, past one
    /// out of the law's reach; NaN where it is a target: LoneEnd's and LiftedFisherInverse's one rule.</summary>
    private static double LiftedEnd(double lifted, bool smallUpper)
    {
        if (!(lifted > 1.0))
        {
            return double.NaN;
        }
        return smallUpper ? 0.0 : double.PositiveInfinity;
    }

    /// <summary>SmallShapesRoot's answer where no search is needed: 0 only where ε's tail is strictly past the target, +∞
    /// where the largest double's falls short; NaN where a search finds the root.</summary>
    /// <remarks>A target ε's tail meets is ε here, the smallest double reaching it, as AtTheEnds has it: Q(F(ε)) is then ε,
    /// F(ε) the target, as the generalized inverse has it.</remarks>
    private static double SmallShapesEnd(FisherTarget tail)
    {
        if (tail.PassedAtEpsilon())
        {
            return 0.0;
        }
        return tail.ReachedAtLargest() ? double.NaN : double.PositiveInfinity;
    }

    /// <summary>The f at which the first order's tail on the caller's side, at shapes half <paramref name="twiceA"/> and
    /// <paramref name="twiceB"/>, is the target, where the settling starts.</summary>
    /// <remarks>Taken from twice the shapes, the dfs where one is lifted alone: its rounded subnormal half put the log-odds
    /// at 84383 for 987 (#1743's Review A). NaN where the first order does not hold there.</remarks>
    private static double FirstOrderGuess(double target, bool givenLower, double twiceA, double twiceB, double numeratorDf, double denominatorDf)
    {
        double logOdds = 2.0 * Beta.SmallShapesLogOdds(twiceA, twiceB, target, upperTail: !givenLower);
        return Beta.SmallShapesHoldAt(twiceA / 2.0, twiceB / 2.0, logOdds)
            ? Math.Exp(logOdds + Math.Log(denominatorDf) - Math.Log(numeratorDf))
            : double.NaN;
    }

    /// <summary>Whether the kept tail at a probe's double has reached the target, as the bit search's test.</summary>
    private readonly struct Meeting(FisherTarget tail) : TailInversion.IBitTest
    {
        private readonly FisherTarget _tail = tail;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Holds(long bits) => _tail.Reaches(_tail.Exact(BitConverter.Int64BitsToDouble(bits)));
    }

    /// <summary>An <em>F</em> quantile at a df below ExactHalf.Threshold, solved at the lifted shapes.</summary>
    /// <remarks>
    /// Both lifted, the tails are of degree zero and the targets stand; one lifted, its small tail is of degree one in it,
    /// so the lifted law's is the target lifted, and a lifted target past one, which no tail reaches, puts the root at
    /// the support's far end.
    /// </remarks>
    // Out of line, as Densities.UnderflowedFisher: inline, it cost FisherSnedecorQuantile(0.05, 4, 9) 8%, 584 against 538 ns.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LiftedFisherInverse(double lowerTarget, double upperTarget, double numeratorDf, double denominatorDf, out RootTail measured)
    {
        measured = RootTail.None;
        (double a, double b, bool lone, bool smallUpper) = ExactHalf.FisherShapes(numeratorDf, denominatorDf);
        double lower = lowerTarget;
        double upper = upperTarget;
        if (lone)
        {
            double lifted = ExactHalf.Lifted(smallUpper ? upperTarget : lowerTarget);
            double end = LiftedEnd(lifted, smallUpper);
            if (!double.IsNaN(end))
            {
                return end;
            }
            (lower, upper) = smallUpper ? (1.0 - lifted, lifted) : (lifted, 1.0 - lifted);
        }
        return FisherFromTail(lowerTarget, upperTarget, (lower, upper), numeratorDf, denominatorDf, (a, b), out measured);
    }

    /// <summary>An <em>F</em> quantile from the incomplete beta's inverse, with <c>y</c> and <c>1 − y</c> both exact.</summary>
    /// <remarks>
    /// With <c>y = d₁f / (d₁f + d₂)</c>, <c>P(F ≤ f) = I_y(d₁/2, d₂/2)</c> and <c>f = d₂y / (d₁(1 − y))</c>.
    /// A root near one would leave <c>1 − y</c> as one minus a number near one, so both come from
    /// <see cref="BetaQuantile.InvertBoth"/>, which solves on whichever side is small.
    /// </remarks>
    private static double FisherFromTail(double lower, double upper, double numeratorDf, double denominatorDf, out RootTail measured) =>
        FisherFromTail(lower, upper, (lower, upper), numeratorDf, denominatorDf, (numeratorDf / 2.0, denominatorDf / 2.0), out measured);

    /// <summary><see cref="FisherFromTail(double, double, double, double, out RootTail)"/> at lifted shapes, the solved targets the lifted
    /// law's, the judged ones the caller's.</summary>
    private static double FisherFromTail(
        double lower, double upper, (double Lower, double Upper) solved, double numeratorDf, double denominatorDf, (double A, double B) shapes,
        out RootTail measured)
    {
        measured = RootTail.None;
        (double solvedLower, double solvedUpper) = solved;
        (double a, double b) = shapes;
        // Judged on the caller's smaller tail; chosen from the lifted ones, a lifted tail past one half kept the other, one
        // less a tiny target, which rounds to one (#1729's Review A). The inversion and the limit solve the lifted targets.
        var target = new FisherTarget(lower, upper, lower <= upper, numeratorDf, denominatorDf);
        (double y, double complement) = BetaQuantile.InvertBoth(solvedLower, solvedUpper, a, b);
        double f = RootFromSplit(y, complement, numeratorDf, denominatorDf);

        // A side below the smallest normal double keeps too few digits, and the half's limit inverts there instead,
        // kept where it holds the target closer: its leading term divides by a shape, ill-conditioned where it is flat.
        double miss = double.NaN;
        if (Beta.TakesLimit(y, complement))
        {
            ExactTail atRoot = target.Exact(f);
            miss = target.MissOf(atRoot.Rounded);
            double fromLimit = Beta.FisherRootFromLimit(solvedLower, solvedUpper, numeratorDf, denominatorDf, a, b);
            if (!double.IsNaN(fromLimit))
            {
                ExactTail atLimit = target.Exact(fromLimit);
                double limitMiss = target.MissOf(atLimit.Rounded);
                if (limitMiss <= miss)
                {
                    (f, miss, atRoot) = (fromLimit, limitMiss, atLimit);
                }
            }
            measured = new RootTail(f, lower <= upper, atRoot);
        }

        // Below Beta.SmallShape a root outside the doubles leaves a non-root, and an end strictly past the target is the
        // answer (#1690's review); past it that end's tail is within e^-74 of its limit, so no larger shape needs a look.
        if (numeratorDf / 2.0 >= Beta.SmallShape && denominatorDf / 2.0 >= Beta.SmallShape)
        {
            return OrBelowTheLargest(f, target);
        }
        if (double.IsNaN(miss))
        {
            ExactTail atRoot = target.Exact(f);
            measured = new RootTail(f, lower <= upper, atRoot);
            miss = target.MissOf(atRoot.Rounded);
        }
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
    /// Each end's tail is taken once. A target the tail passes at the smallest double has its root at 0, and one it
    /// falls short of even at the largest has it at +∞: a root outside the doubles is an end, never a double near one
    /// (#1690). Met exactly there, ε is the smallest double reaching it, as SmallShapesEnd has it. At two tiny dfs the
    /// tail is flat to its last ulps: read strictly, such a target answered +∞, and judged on the start's miss alone, a
    /// target past the plateau answered 0 (#1729's Review A). Dfs below 6.2e-9 are SmallShapesRoot's first (#1743).
    /// </remarks>
    private static double AtTheEnds(double f, FisherTarget target, double numeratorDf, double denominatorDf)
    {
        if (numeratorDf / 2.0 < Beta.SmallShape)
        {
            ExactTail atSmallest = target.Exact(double.Epsilon);
            if (target.Reaches(atSmallest))
            {
                return target.Passes(atSmallest) ? 0.0 : double.Epsilon;
            }
        }
        return denominatorDf / 2.0 < Beta.SmallShape && target.RootAbove(double.MaxValue)
            ? double.PositiveInfinity
            : OrBelowTheLargest(f, target);
    }

    /// <summary>An F quantile's target, judged on its smaller tail, which a double keeps whole.</summary>
    /// <remarks>
    /// The ends are f's, not the beta's: at small shapes 1 - y at f = MaxValue is far above the smallest double. Judged
    /// on the tail the caller passed, a target of 1 - 1e-12 hid a miss of 1e-12 and FisherSnedecorIsf(1 - 1e-12, 0.001,
    /// 1e-300) read 0 for +inf (#1690's review).
    /// </remarks>
    private readonly struct FisherTarget(double lower, double upper, bool keepsLower, double numeratorDf, double denominatorDf)
        : TailInversion.INewtonTail
    {
        // The kept target alone, so four fields, the most ShapeLogs' remark found the JIT promotes, a fifth costing 8% (#1690).
        private readonly double _keptTarget = keepsLower ? lower : upper;

        /// <summary>The relative distance of a kept tail of <paramref name="kept"/> from the target.</summary>
        internal double MissOf(double kept) => Math.Abs(kept - _keptTarget) / _keptTarget;

        /// <summary>Whether the root lies strictly above <paramref name="f"/>; a tail equal to the target there is a root,
        /// as at shapes 1e-310, where the tail at the largest double is one half to rounding.</summary>
        internal bool RootAbove(double f)
        {
            // A NaN tail is no evidence either way, as main read it: the root is not above.
            ExactTail exact = Exact(f);
            return !double.IsNaN(exact.Rounded) && !Reaches(exact);
        }

        /// <summary>Whether the tail at ε lies strictly past the target, where the quantile is 0: the one test of that end.</summary>
        internal bool PassedAtEpsilon() => PassedAtEpsilon(Exact(double.Epsilon));

        /// <summary><see cref="PassedAtEpsilon()"/> on ε's tail <paramref name="atEpsilon"/>, read already.</summary>
        internal bool PassedAtEpsilon(ExactTail atEpsilon) =>
            // A subnormal target on ε's rounded tail: below the granule the exact one tells nothing, and at dfs (2, 5) its
            // 106-bit noise passed ε's own tail, answering 0 for main's ε (#1745's Review A).
            _keptTarget < NearZero.SmallestNormal ? Passes(ExactTail.OfDouble(atEpsilon.Rounded)) : Passes(atEpsilon);

        /// <summary>Whether the tail at the largest double reaches the target, where a double answers: the one test of that end.</summary>
        internal bool ReachedAtLargest() => Reaches(Exact(double.MaxValue));

        /// <summary>Whether a kept tail before its last rounding lies strictly past the target.</summary>
        internal bool Passes(ExactTail exact) => keepsLower ? exact.Less(_keptTarget) > 0.0 : exact.Less(_keptTarget) < 0.0;

        /// <summary>One Newton step from <paramref name="statistic"/>, whose kept tail is <paramref name="exact"/> unrounded,
        /// on the double forms' density, toward the target.</summary>
        public double Stepped(double statistic, ExactTail exact) => Stepped(statistic, exact, Front(statistic));

        /// <summary><see cref="Stepped(double, ExactTail)"/> on the front at <paramref name="statistic"/> already taken.</summary>
        internal double Stepped(double statistic, ExactTail exact, double front) =>
            TailInversion.EdgeStep(statistic, exact.Less(_keptTarget), front, keepsLower);

        /// <summary>About how many doubles of f about <paramref name="statistic"/> the tail takes to move an ulp of the
        /// target: that ulp over the tail's slope in log f, <paramref name="front"/>, in ulps of f; NaN where the front is.</summary>
        internal double RunWidth(double statistic, double front) =>
            (TailInversion.NextUp(_keptTarget) - _keptTarget) / front / (TailInversion.NextUp(statistic) - statistic) * statistic;

        /// <summary>f times the density by the double forms, the tail's slope in log f; NaN where a df's half rounds to zero.</summary>
        internal double Front(double statistic)
        {
            // A df whose half rounds to zero has no density to step on: log B(0, b) threw where main answered (#1745's
            // Review A), and a NaN step leaves the root.
            if (!(numeratorDf / 2.0 > 0.0 && denominatorDf / 2.0 > 0.0))
            {
                return double.NaN;
            }
            (double point, double complement) = Beta.FisherSplit(statistic, numeratorDf, denominatorDf);
            // Over f times the density, the front, which stays a double where the density underflows near the largest
            // double: Isf(Sf(1.7e308, 400, 0.5)) kept a root 177 ulps short of the target (#1745's Review A).
            double a = numeratorDf / 2.0;
            double b = denominatorDf / 2.0;
            // A split with a subnormal half takes the density's limit: its own front read 0, and with no Newton step
            // FisherSnedecorQuantile(0.025, 1.7e308, 0.001) searched 45 tails, 33 us to scipy's 26 (#1745's Review A).
            if (Beta.TakesLimit(point, complement))
            {
                return Beta.FisherFrontFromLimit(statistic, numeratorDf, denominatorDf, a, b, point);
            }

            // log B from DoubleDouble's cache of the last shapes, which only a shape below Stirling's reads: as NaN, each step
            // took its three log-gammas again.
            double logBeta = Math.Min(a, b) < Beta.StirlingMinimumShape ? DoubleDouble.LogBeta(a, b).High : double.NaN;
            return Beta.FastFront(a, b, point, complement, logBeta);
        }

        /// <summary>Whether a kept tail before its last rounding has reached the target, equal to it included.</summary>
        public bool Reaches(ExactTail exact) => keepsLower ? exact.Less(_keptTarget) >= 0.0 : exact.Less(_keptTarget) <= 0.0;

        /// <summary>The kept tail at <paramref name="statistic"/> before its last rounding.</summary>
        public ExactTail Exact(double statistic) => Beta.ExactFisherTail(statistic, numeratorDf, denominatorDf, upperTail: !keepsLower);
    }
}
