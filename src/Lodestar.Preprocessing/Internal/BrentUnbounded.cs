namespace Lodestar.Preprocessing.Internal;

/// <summary>
/// <c>scipy.optimize.brent</c> with a two-point <c>brack</c>: <c>scipy.optimize.bracket</c> walks downhill
/// from the two points until it holds a minimum, then Brent's method searches that bracket unbounded.
/// </summary>
/// <remarks>
/// What <c>scipy.stats.boxcox_normmax</c> runs from <c>(-2, 2)</c>, so a Box-Cox exponent may land outside
/// that interval, where the bounded search this package used clamped it (#1227). Written from scipy 1.18.1's
/// <c>bracket</c> and <c>Brent.optimize</c> (BSD-3, allowed by decision 0002), constants included.
/// </remarks>
internal static class BrentUnbounded
{
    private const double Golden = 1.618034;
    private const double VerySmall = 1e-21;
    private const double GrowLimit = 110.0;
    private const int BracketIterations = 1000;
    private const double Tolerance = 1.48e-8;
    private const double MinimumTolerance = 1e-11;
    private const double GoldenSection = 0.3819660;
    private const int MaxIterations = 500;

    /// <summary>The argument minimising <paramref name="objective"/>, bracketed from <paramref name="first"/> and <paramref name="second"/>.</summary>
    /// <exception cref="ArgumentException">No valid bracket is found, as <c>bracket</c> raises.</exception>
    public static double Locate(Func<double, double> objective, double first, double second, string paramName)
    {
        (double a, double b, double c, double fb) = Bracket(objective, first, second, paramName);
        return Minimise(objective, a, b, c, fb);
    }

    // S3776: scipy's bracket, branch for branch, for the same reason as Minimise below.
#pragma warning disable S3776
    private static (double A, double B, double C, double Fb) Bracket(
        Func<double, double> objective, double xa, double xb, string paramName)
#pragma warning restore S3776
    {
        double fa = objective(xa);
        double fb = objective(xb);
        if (fa < fb)
        {
            (xa, xb) = (xb, xa);
            (fa, fb) = (fb, fa);
        }

        double xc = xb + (Golden * (xb - xa));
        double fc = objective(xc);
        int iteration = 0;
        while (fc < fb)
        {
            double tmp1 = (xb - xa) * (fb - fc);
            double tmp2 = (xb - xc) * (fb - fa);
            double val = tmp2 - tmp1;
            double denominator = Math.Abs(val) < VerySmall ? 2.0 * VerySmall : 2.0 * val;
            double w = xb - ((((xb - xc) * tmp2) - ((xb - xa) * tmp1)) / denominator);
            double wlim = xb + (GrowLimit * (xc - xb));
            if (iteration > BracketIterations)
            {
                throw new ArgumentException(
                    "No valid bracket was found before the iteration limit was reached; the Box-Cox search starts from (-2, 2) and allows 1000 steps, as scipy.stats.boxcox does.",
                    paramName);
            }

            iteration++;
            double fw;
            if ((w - xc) * (xb - w) > 0.0)
            {
                fw = objective(w);
                if (fw < fc)
                {
                    return Checked(xb, w, xc, fb, fw, fc, paramName);
                }

                if (fw > fb)
                {
                    return Checked(xa, xb, w, fa, fb, fw, paramName);
                }

                w = xc + (Golden * (xc - xb));
                fw = objective(w);
            }
            else if ((w - wlim) * (wlim - xc) >= 0.0)
            {
                w = wlim;
                fw = objective(w);
            }
            else if ((w - wlim) * (xc - w) > 0.0)
            {
                fw = objective(w);
                if (fw < fc)
                {
                    xb = xc;
                    xc = w;
                    w = xc + (Golden * (xc - xb));
                    fb = fc;
                    fc = fw;
                    fw = objective(w);
                }
            }
            else
            {
                w = xc + (Golden * (xc - xb));
                fw = objective(w);
            }

            xa = xb;
            xb = xc;
            xc = w;
            fa = fb;
            fb = fc;
            fc = fw;
        }

        return Checked(xa, xb, xc, fa, fb, fc, paramName);
    }

    /// <summary>The three conditions <c>bracket</c> checks before it returns.</summary>
    private static (double A, double B, double C, double Fb) Checked(
        double xa, double xb, double xc, double fa, double fb, double fc, string paramName)
    {
        bool lowest = (fb < fc && fb <= fa) || (fb < fa && fb <= fc);
        bool ordered = (xa < xb && xb < xc) || (xc < xb && xb < xa);
        bool finite = !double.IsInfinity(xa) && !double.IsNaN(xa) && !double.IsInfinity(xb) && !double.IsNaN(xb)
            && !double.IsInfinity(xc) && !double.IsNaN(xc);
        if (!(lowest && ordered && finite))
        {
            throw new ArgumentException(
                "The algorithm terminated without finding a valid bracket; the Box-Cox search starts from (-2, 2), as scipy.stats.boxcox does.", paramName);
        }

        return (xa, xb, xc, fb);
    }

    // long-comment: a line-for-line port of scipy's Brent.optimize, whose variable names it keeps so
    // the two can be read side by side; splitting it would hide the correspondence that checks it.
    // S3776: the reference's own control flow, kept whole for that reason.
#pragma warning disable S3776
    private static double Minimise(Func<double, double> objective, double xa, double xb, double xc, double fb)
#pragma warning restore S3776
    {
        double x = xb;
        double w = xb;
        double v = xb;
        double fx = fb;
        double fw = fb;
        double fv = fb;
        double a = xa < xc ? xa : xc;
        double b = xa < xc ? xc : xa;
        double deltax = 0.0;
        double rat = 0.0;

        for (int iteration = 0; iteration < MaxIterations; iteration++)
        {
            double tol1 = (Tolerance * Math.Abs(x)) + MinimumTolerance;
            double tol2 = 2.0 * tol1;
            double xmid = 0.5 * (a + b);
            if (Math.Abs(x - xmid) < tol2 - (0.5 * (b - a)))
            {
                break;
            }

            if (Math.Abs(deltax) <= tol1)
            {
                deltax = x >= xmid ? a - x : b - x;
                rat = GoldenSection * deltax;
            }
            else
            {
                double tmp1 = (x - w) * (fx - fv);
                double tmp2 = (x - v) * (fx - fw);
                double p = ((x - v) * tmp2) - ((x - w) * tmp1);
                tmp2 = 2.0 * (tmp2 - tmp1);
                if (tmp2 > 0.0)
                {
                    p = -p;
                }

                tmp2 = Math.Abs(tmp2);
                double previous = deltax;
                deltax = rat;
                if (p > tmp2 * (a - x) && p < tmp2 * (b - x) && Math.Abs(p) < Math.Abs(0.5 * tmp2 * previous))
                {
                    rat = p * 1.0 / tmp2;
                    double trial = x + rat;
                    if (trial - a < tol2 || b - trial < tol2)
                    {
                        rat = xmid - x >= 0 ? tol1 : -tol1;
                    }
                }
                else
                {
                    deltax = x >= xmid ? a - x : b - x;
                    rat = GoldenSection * deltax;
                }
            }

            double u;
            if (Math.Abs(rat) < tol1)
            {
                u = rat >= 0 ? x + tol1 : x - tol1;
            }
            else
            {
                u = x + rat;
            }

            double fu = objective(u);
            if (fu > fx)
            {
                if (u < x)
                {
                    a = u;
                }
                else
                {
                    b = u;
                }

                // S1244: identity of tracked points, assigned from one another, as scipy compares them.
#pragma warning disable S1244
                if (fu <= fw || w == x)
                {
                    v = w;
                    w = u;
                    fv = fw;
                    fw = fu;
                }
                else if (fu <= fv || v == x || v == w)
                {
                    v = u;
                    fv = fu;
                }
#pragma warning restore S1244
            }
            else
            {
                if (u >= x)
                {
                    a = x;
                }
                else
                {
                    b = x;
                }

                v = w;
                w = x;
                x = u;
                fv = fw;
                fw = fx;
                fx = fu;
            }
        }

        return x;
    }
}
