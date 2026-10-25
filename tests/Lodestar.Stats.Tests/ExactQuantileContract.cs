using Lodestar.Stats.Internal;

namespace Lodestar.Stats.Tests;

/// <summary>The F and Student quantiles' contract, judged as the quantiles judge it: the smallest double whose smaller tail
/// before its last rounding reaches the target, one less the target past one half (#1745's Review A, Cyril 2026-10-28).</summary>
internal static class ExactQuantileContract
{
    /// <summary>Whether F's lower tail before rounding at <paramref name="f"/> reaches <paramref name="target"/>.</summary>
    internal static bool FisherLowerReaches(double f, double dfn, double dfd, double target) =>
        Beta.ExactFisherTail(f, dfn, dfd, upperTail: false).Less(target) >= 0.0;

    /// <summary>Whether F's upper tail before rounding at <paramref name="f"/> has fallen to <paramref name="target"/>.</summary>
    internal static bool FisherUpperReaches(double f, double dfn, double dfd, double target) =>
        Beta.ExactFisherTail(f, dfn, dfd, upperTail: true).Less(target) <= 0.0;

    /// <summary>Whether Student's upper tail before rounding at a positive <paramref name="t"/> has fallen to
    /// <paramref name="target"/>.</summary>
    internal static bool StudentUpperReaches(double t, double df, double target) =>
        new Beta.StudentTail(df).ExactUpperTail(t).Less(target) <= 0.0;

    /// <summary>Whether Student's lower tail before rounding at a negative <paramref name="t"/> reaches
    /// <paramref name="target"/>, the upper tail at −t by symmetry.</summary>
    internal static bool StudentLowerReaches(double t, double df, double target) =>
        new Beta.StudentTail(df).ExactUpperTail(-t).Less(target) >= 0.0;
}
