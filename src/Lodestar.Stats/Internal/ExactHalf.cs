namespace Lodestar.Stats.Internal;

/// <summary>The halves the chi-squared and F laws take of a df or a statistic, where halving rounds.</summary>
/// <remarks>
/// From 2⁻¹⁰²¹ up halving is exact; below it an odd multiple of the smallest double has no half among the doubles, and
/// FisherSnedecorCdf(1e-5, 1e-310, 1.5e-323) read a third high (#1729's Review A). Density and small tail being of
/// degree one in so small a shape, such a df is lifted by 2^200, exact, the law taken at the lifted shapes' exact
/// halves, and the answer dropped by 2^-200: a subnormal answer rounds its 53 bits once more, off by a granule only
/// within an ulp of a granule's midpoint. Every primitive taking a df, the roots included, lifts through here;
/// Student's reads df itself.
/// </remarks>
internal static class ExactHalf
{
    /// <summary>2⁻¹⁰²¹, from which halving is exact; a hot member tests a df against it first, for speed alone.</summary>
    internal const double Threshold = 2.0 * NearZero.SmallestNormal;

    /// <summary>2^200, which takes a df below <see cref="Threshold"/> into the normal doubles, and still below 2^-821.</summary>
    internal static readonly double Lift = PowerOfTwo.Of(200);

    /// <summary>2^-200, which undoes <see cref="Lift"/> on an answer of degree one in the shape, rounding once.</summary>
    internal static readonly double Drop = PowerOfTwo.Of(-200);

    // 2^-700: an F df below it beside a lifted one is lifted too, so the two stay in the ratio the law reads; above it a
    // lone lifted shape, at most 2^-821, is under 2^-121 of the other, and the small tail is linear in it.
    private static readonly double Companion = PowerOfTwo.Of(-700);

    /// <summary>A df, or a small tail a lifted law solves for, times <see cref="Lift"/>, exactly; a lifted target past
    /// one no tail reaches, which each root reads as its own end.</summary>
    internal static double Lifted(double value) => value * Lift;

    /// <summary>A tail from its small side at the lifted shape, of degree one in it: dropped once, and its complement
    /// one less that; every tail that lifts a shape answers through here.</summary>
    internal static double FromLiftedTail(double liftedSmallTail, bool small)
    {
        double tail = liftedSmallTail * Drop;
        return small ? tail : 1.0 - tail;
    }

    /// <summary>Whether a chi-squared df is lifted: below <see cref="Threshold"/> and positive, since a df of zero or less
    /// would stay one lifted, and recurse; it is refused downstream instead.</summary>
    internal static bool Lifts(double df) => df < Threshold && df > 0.0;

    /// <summary>Whether either F df is lifted, as <see cref="Lifts"/> reads a chi-squared one.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static bool EitherBelow(double numeratorDf, double denominatorDf) =>
        Lifts(numeratorDf) || Lifts(denominatorDf);

    /// <summary>The F shapes at the lifted dfs where <see cref="EitherBelow"/> holds, whether one alone was lifted, and if so
    /// the side of its small tail: the upper for the numerator's, the lower for the denominator's.</summary>
    /// <remarks>
    /// Both lifted, the tails are of degree zero in the shapes and the density of degree one; one lifted, its small tail
    /// and the density are of degree one in it, the other tail one less that.
    /// </remarks>
    internal static (double A, double B, bool Lone, bool SmallUpper) FisherShapes(double numeratorDf, double denominatorDf)
    {
        bool liftsA = Lifts(numeratorDf) || (Lifts(denominatorDf) && numeratorDf < Companion);
        bool liftsB = Lifts(denominatorDf) || (Lifts(numeratorDf) && denominatorDf < Companion);
        return (
            (liftsA ? Lifted(numeratorDf) : numeratorDf) / 2.0,
            (liftsB ? Lifted(denominatorDf) : denominatorDf) / 2.0,
            liftsA != liftsB,
            liftsA);
    }
}
