using System.Runtime.CompilerServices;

namespace Lodestar.Internal;

/// <summary><c>log(1 + x)</c> and <c>eˣ − 1</c> kept accurate near zero, where the plain forms round the answer away.</summary>
/// <remarks>
/// Kahan's corrections, the one implementation the packages share, where five copies each sat a few ulps off glibc in
/// their own way (#1230). <c>numpy.log1p</c> and <c>numpy.expm1</c> are the references, and these land within an ulp
/// of them, not on them: a third to a half of inputs an ulp away, against glibc's correctly rounded forms.
/// </remarks>
internal static class NearZero
{
    /// <summary><c>np.finfo(float).eps</c>, <c>2⁻⁵²</c>; <see cref="double.Epsilon"/> is the smallest subnormal, not this.</summary>
    internal const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary><c>np.finfo(float).smallest_normal</c>, <c>2⁻¹⁰²²</c>.</summary>
    internal const double SmallestNormal = 2.2250738585072014e-308;

    /// <summary><c>log(1 + x)</c>, within an ulp for a small <paramref name="x"/>, where the plain form loses it all.</summary>
    /// <remarks>Inlined, and branch-free past the zero test: Metrics' residual loop ran 1.14 times its own copy's time with a branch, 1.05 without (#1682).</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log1P(double x)
    {
        double u = 1.0 + x;

        // S1244: the sum having rounded back to exactly one is the case the correction would divide by zero in.
#pragma warning disable S1244
        if (u == 1.0)
#pragma warning restore S1244
        {
            return x;
        }

        // Divided before multiplied, as log(u) · x overflows past ~2.5e305; past 1/ε the correction moves nothing, and
        // a select on the factor rather than a branch on the result took Metrics' loop from 1.14 to 1.05 (#1682).
        double correction = x / (u - 1.0);
        return Math.Log(u) * (x > 1.0 / MachineEpsilon ? 1.0 : correction);
    }

    /// <summary><c>eˣ − 1</c>, within an ulp for a small <paramref name="x"/>, where the plain form loses it all.</summary>
    public static double Expm1(double x)
    {
        if (Math.Abs(x) > 0.5)
        {
            return Math.Exp(x) - 1.0;
        }

        double u = Math.Exp(x);

        // S1244: an exponential rounded to exactly one, or a difference to exactly minus one, is where the
        // correction would divide by zero.
#pragma warning disable S1244
        if (u == 1.0)
        {
            return x;
        }

        double um = u - 1.0;
        return um == -1.0 ? -1.0 : um * x / Math.Log(u);
#pragma warning restore S1244
    }
}
