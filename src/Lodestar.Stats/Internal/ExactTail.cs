namespace Lodestar.Stats.Internal;

/// <summary>A tail before its last rounding, and whether a double form gave it, rounded already, its low part then saying
/// nothing. A low part of exactly zero had stood for that, and a 106-bit tail clamped below one, its low part dropped, took
/// StudentIsf(0.49999999999999994, 1e-11) through 123 tails (#1745's Review A). The first order's share is a double form
/// that keeps its low part, its rounding's tie broken.</summary>
/// <remarks>A tail from an exponential below e^-500, about 2^-721, is held times 2^-<see cref="Exponent"/>, and a side
/// below 2^-969's: scaled down, its low part went subnormal, and FisherSnedecorQuantile(3.53e-307, 4, 5) stopped a double
/// short of its target (#1745's Review A).</remarks>
internal readonly struct ExactTail(DoubleDouble value, bool inDouble, int exponent = 0)
{
    private readonly DoubleDouble _scaled = value;

    /// <summary>The power of two the held value is scaled by; zero but for a lifted tail.</summary>
    internal int Exponent { get; } = exponent;

    internal DoubleDouble Value => Exponent == 0 ? _scaled : DoubleDouble.ScaledOnce(_scaled, Exponent);

    internal bool InDouble { get; } = inDouble;

    /// <summary>The tail rounded once: a lifted one's high part scaled back, exact where normal and rounded once where
    /// not; its low part, rounded onto the subnormals first, moved a tie and FisherSnedecorSf(3.14e204, 2, 3) an ulp.</summary>
    internal double Rounded => Exponent == 0 ? _scaled.Rounded : DoubleDouble.ScaledOnce(_scaled, Exponent).High;

    /// <summary>A double form's tail.</summary>
    internal static ExactTail OfDouble(double value) => new(new DoubleDouble(value, 0.0), inDouble: true);

    /// <summary>This tail less <paramref name="target"/> before the last rounding, NaN for a NaN tail, so no comparison
    /// of it holds: the quantiles' tests and Newton residual alike, on which a tail flat to rounding over hundreds of ulps
    /// near one half no longer moved their answer off the root (Cyril, 2026-10-28, #1745's Review A).</summary>
    internal double Less(double target)
    {
        if (Exponent == 0)
        {
            return (_scaled - new DoubleDouble(target, 0.0)).Rounded;
        }
        // At the held scale, and its sign kept where the difference underflows scaled back; a target past the doubles there,
        // above 2^-176 at SmallSecondShape's exponent of −1200, at its own, where the tail is under an ulp of it.
        DoubleDouble lifted = DoubleDouble.ScaledOnce(new DoubleDouble(target, 0.0), -Exponent);
        if (double.IsInfinity(lifted.High))
        {
            return (Value - new DoubleDouble(target, 0.0)).Rounded;
        }
        double held = (_scaled - lifted).Rounded;
        double back = PowerOfTwo.Scale(held, Exponent);
        if (back != 0.0 || held == 0.0 || double.IsNaN(held))
        {
            return back;
        }
        return held > 0.0 ? double.Epsilon : -double.Epsilon;
    }

    /// <summary>Half this tail, at its scale.</summary>
    internal ExactTail Halved() => new(0.5 * _scaled, InDouble, Exponent);

    /// <summary>One less this tail.</summary>
    internal ExactTail Complement() => new(new DoubleDouble(1.0, 0.0) - Value, InDouble);
}
