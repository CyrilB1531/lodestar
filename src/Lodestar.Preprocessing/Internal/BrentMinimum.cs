namespace Lodestar.Preprocessing.Internal;

/// <summary>Brent's bounded minimisation of a smooth function of one variable.</summary>
/// <remarks>
/// <c>scipy.optimize._optimize._minimize_scalar_bounded</c>, which <c>scipy.optimize.fminbound</c> runs and
/// <c>sklearn.preprocessing.PowerTransformer</c> reaches through <c>scipy.stats.yeojohnson_normmax</c>, ported
/// statement for statement: its <c>sqrt(2.2e-16)</c> beside the <c>1.48e-8</c> tolerance, its lag of the step
/// through <c>e</c> and <c>rat</c>, its tests on the parabola and its count of calls. Review A found a hand-written
/// Brent parting from it by 6e-2 on a column near 1e10, every likelihood matching to the bit (#1227).
/// </remarks>
internal static class BrentMinimum
{
    /// <summary><c>yeojohnson_normmax</c>'s <c>tol_brent</c>, the <c>xtol</c> it passes <c>fminbound</c>.</summary>
    internal const double Tolerance = 1.48e-8;

    /// <summary><c>fminbound</c>'s default <c>maxfun</c>.</summary>
    private const int MaxCalls = 500;

    private static readonly double SquareRootEpsilon = Math.Sqrt(2.2e-16);

    private static readonly double GoldenMean = 0.5 * (3.0 - Math.Sqrt(5.0));

    /// <summary>The argument minimising <paramref name="objective"/> over <c>[low, high]</c>.</summary>
    /// <exception cref="ArgumentException">The lower bound exceeds the upper, as scipy refuses it.</exception>
    internal static double Locate(Func<double, double> objective, double low, double high, string paramName)
    {
        // scipy's own check, in its words: a mixed-sign column past about 1e145 inverts the Yeo-Johnson bounds. Its
        // finite-bounds check has no input to reach here, a column small enough to make one being constant first.
        if (low > high)
        {
            throw new ArgumentException("The lower bound exceeds the upper bound.", paramName);
        }

        var search = new Search(objective, low, high);
        while (!search.Converged && search.Calls < MaxCalls)
        {
            search.Step();
        }

        return search.Best;
    }

    /// <summary>scipy's loop state, the names its own: <c>xf</c> the best, <c>nfc</c> and <c>fulc</c> the two before.</summary>
    private sealed class Search
    {
        private readonly Func<double, double> _objective;
        private double _a;
        private double _b;
        private double _fulc;
        private double _nfc;
        private double _xf;
        private double _rat;
        private double _e;
        private double _fx;
        private double _ffulc;
        private double _fnfc;
        private double _xm;
        private double _tol1;
        private double _tol2;

        internal Search(Func<double, double> objective, double low, double high)
        {
            _objective = objective;
            _a = low;
            _b = high;
            _fulc = _a + (GoldenMean * (_b - _a));
            _nfc = _fulc;
            _xf = _fulc;
            _fx = objective(_xf);
            Calls = 1;
            _ffulc = _fx;
            _fnfc = _fx;
            Bounds();
        }

        internal int Calls { get; private set; }

        internal double Best => _xf;

        internal bool Converged => Math.Abs(_xf - _xm) <= _tol2 - (0.5 * (_b - _a));

        /// <summary>One pass of scipy's <c>while</c>: a parabolic step where the parabola behaves, golden otherwise.</summary>
        internal void Step()
        {
            if (!(Math.Abs(_e) > _tol1 && TryParabola()))
            {
                _e = _xf >= _xm ? _a - _xf : _b - _xf;
                _rat = GoldenMean * _e;
            }

            double u = _xf + (SignOrOne(_rat) * Math.Max(Math.Abs(_rat), _tol1));
            double fu = _objective(u);
            Calls++;
            Accept(u, fu);
            Bounds();
        }

        /// <summary>scipy's parabolic branch; false where it falls back to a golden step, <c>e</c> already lagged.</summary>
        private bool TryParabola()
        {
            double r = (_xf - _nfc) * (_fx - _ffulc);
            double q = (_xf - _fulc) * (_fx - _fnfc);
            double p = ((_xf - _fulc) * q) - ((_xf - _nfc) * r);
            q = 2.0 * (q - r);
            if (q > 0.0)
            {
                p = -p;
            }

            q = Math.Abs(q);
            r = _e;
            _e = _rat;
            if (!(Math.Abs(p) < Math.Abs(0.5 * q * r) && p > q * (_a - _xf) && p < q * (_b - _xf)))
            {
                return false;
            }

            _rat = (p + 0.0) / q;
            double x = _xf + _rat;
            if ((x - _a) < _tol2 || (_b - x) < _tol2)
            {
                _rat = _tol1 * SignOrOne(_xm - _xf);
            }

            return true;
        }

        private void Accept(double u, double fu)
        {
            if (fu <= _fx)
            {
                if (u >= _xf)
                {
                    _a = _xf;
                }
                else
                {
                    _b = _xf;
                }

                (_fulc, _ffulc) = (_nfc, _fnfc);
                (_nfc, _fnfc) = (_xf, _fx);
                (_xf, _fx) = (u, fu);
                return;
            }

            if (u < _xf)
            {
                _a = u;
            }
            else
            {
                _b = u;
            }

            // S1244: scipy compares the points exactly, as here.
#pragma warning disable S1244
            if (fu <= _fnfc || _nfc == _xf)
            {
                (_fulc, _ffulc) = (_nfc, _fnfc);
                (_nfc, _fnfc) = (u, fu);
            }
            else if (fu <= _ffulc || _fulc == _xf || _fulc == _nfc)
            {
                (_fulc, _ffulc) = (u, fu);
            }
#pragma warning restore S1244
        }

        private void Bounds()
        {
            _xm = 0.5 * (_a + _b);
            _tol1 = (SquareRootEpsilon * Math.Abs(_xf)) + (Tolerance / 3.0);
            _tol2 = 2.0 * _tol1;
        }

        /// <summary><c>np.sign(v) + (v == 0)</c>: the sign, and 1 for a zero.</summary>
        private static double SignOrOne(double value) => value < 0.0 ? -1.0 : 1.0;
    }
}
