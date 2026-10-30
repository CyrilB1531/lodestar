namespace Lodestar.Internal;

/// <summary>The rounding left by a product, for formulas that subtract it from a number it nearly equals.</summary>
internal static class ExactProduct
{
#if !NET
    // Veltkamp's splitting constant, 2^27 + 1: it halves a double's 53 bits into two of 26.
    private const double Splitter = 134217729.0;

    // Past this an operand's product by Splitter overflows; it is scaled by 2^-600 first, exactly.
    private const double SplitLimit = 1e290;

    // Past this Veltkamp's halves, each rounded up, can carry their own product past the largest double.
    private const double LargeProduct = 1e307;

    // Below this a product's error terms, about 2^-106 of it, fall under the smallest normal double.
    private const double TinyProduct = 1e-275;
    private const double Scale = 2.409919865102884e-181;
    private const double Unscale = 4.149515568880993e+180;
#endif

    /// <summary><c>minuend − left·right</c>, exact where the product rounds to a neighbour of <paramref name="minuend"/>.</summary>
    /// <remarks>
    /// A fused multiply-add on net10.0; on netstandard2.0, which has none, Dekker's product splits <paramref name="left"/>
    /// and <paramref name="right"/> into halves whose products a double holds exactly (Dekker, 1971). The two agree but
    /// where the residual is subnormal: rescaled from a product near the subnormals it rounds twice, and can differ in its
    /// last bit, as Residual(7.5e-294, 11.84, 6.34e-295) does (#1690's review).
    /// </remarks>
    internal static double Residual(double minuend, double left, double right)
    {
#if NET
        return Math.FusedMultiplyAdd(-left, right, minuend);
#else
        // A product past the largest double over a finite pair may still leave a finite residual: scaled down with the
        // minuend, the result the fused multiply-add gives.
        double product = left * right;
        if (double.IsInfinity(product) && !double.IsInfinity(left) && !double.IsInfinity(right))
        {
            return Residual(minuend * Scale, left * Scale, right) * Unscale;
        }
        // Infinite or NaN operands, or a product under half the smallest double, leave nothing a split can add.
#pragma warning disable S1244
        if (double.IsNaN(product) || double.IsInfinity(product) || product == 0.0)
#pragma warning restore S1244
        {
            return minuend - product;
        }
        // Within a few ulps of the largest double the halves' product overflowed and Residual(MaxValue, 1, MaxValue) read
        // −∞: a quarter of the larger factor and of the minuend, exactly, and four times that residual.
        if (Math.Abs(product) > LargeProduct)
        {
            return Math.Abs(left) >= Math.Abs(right)
                ? Residual(minuend * 0.25, left * 0.25, right) * 4.0
                : Residual(minuend * 0.25, left, right * 0.25) * 4.0;
        }
        // An operand that would overflow its split trades 2^600 with the other, which is then below 1e19, so the product
        // and the minuend stay as they are: scaling the minuend with it underflowed a small one to zero (#1690's review).
        if (Math.Abs(left) > SplitLimit)
        {
            return Residual(minuend, left * Scale, right * Unscale);
        }
        if (Math.Abs(right) > SplitLimit)
        {
            return Residual(minuend, left * Unscale, right * Scale);
        }
        // A product near the subnormals loses its error terms there: the smaller operand is scaled up, with a minuend as
        // small, the one whose difference those terms decide.
        if (Math.Abs(product) < TinyProduct && Math.Abs(minuend) < TinyProduct)
        {
            return Math.Abs(left) <= Math.Abs(right)
                ? Residual(minuend * Unscale, left * Unscale, right) * Scale
                : Residual(minuend * Unscale, left, right * Unscale) * Scale;
        }
        return (minuend - product) - DekkerError(product, left, right);
#endif
    }

    /// <summary><c>left·right − product</c>, <paramref name="product"/> their rounded product: Residual negated, its guards
    /// passed over in one test where the product and both operands lie inside Dekker's range. Through every guard each
    /// double-double product cost the netstandard2.0 tails, FisherSnedecorIsf(1e-10, 1e-9, 1000) 43 us to scipy's 27.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static double ProductError(double product, double left, double right)
    {
#if NET
        return Math.FusedMultiplyAdd(left, right, -product);
#else
        double magnitude = Math.Abs(product);
        if (magnitude >= TinyProduct && magnitude <= LargeProduct && Math.Abs(left) <= SplitLimit && Math.Abs(right) <= SplitLimit)
        {
            return DekkerError(product, left, right);
        }
        return -Residual(product, left, right);
#endif
    }

#if !NET
    /// <summary><c>left·right − product</c> by Veltkamp's halves, exact where the operands and product are in range.</summary>
    private static double DekkerError(double product, double left, double right)
    {
        (double leftHigh, double leftLow) = Split(left);
        (double rightHigh, double rightLow) = Split(right);
        return (((leftHigh * rightHigh) - product) + (leftHigh * rightLow) + (leftLow * rightHigh)) + (leftLow * rightLow);
    }

    private static (double High, double Low) Split(double value)
    {
        double scaled = Splitter * value;
        double high = scaled - (scaled - value);
        return (high, value - high);
    }
#endif
}
