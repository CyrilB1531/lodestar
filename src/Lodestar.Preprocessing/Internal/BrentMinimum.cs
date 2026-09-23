namespace Lodestar.Preprocessing.Internal;

/// <summary>Brent's bounded minimisation of a smooth function of one variable.</summary>
/// <remarks>
/// Golden-section search with parabolic interpolation taken whenever the parabola behaves —
/// <c>scipy.optimize.fminbound</c>, which <c>sklearn.preprocessing.PowerTransformer</c> reaches
/// for through <c>scipy.stats.yeojohnson</c>. <see cref="Tolerance"/> is scipy's own default and
/// reads as a statement about the answer: it pins the argument to <c>1.48e-8</c> where the
/// log-likelihood determines it to about <c>6e-7</c>, so a reported lambda's last digits belong
/// to the optimiser and not to the data. <c>docs/equivalence.md</c> carries the measurement.
/// </remarks>
internal static class BrentMinimum
{
    /// <summary>The absolute tolerance on the argument; <c>scipy.optimize.fminbound</c>'s <c>xtol</c>.</summary>
    internal const double Tolerance = 1.48e-8;

    private const int MaxIterations = 500;

    private static readonly double Golden = 0.5 * (3.0 - Math.Sqrt(5.0));

    /// <summary>The argument minimising <paramref name="objective"/> over <c>[low, high]</c>.</summary>
    internal static double Locate(Func<double, double> objective, double low, double high)
    {
        var state = Search.Start(objective, low, high);

        for (int i = 0; i < MaxIterations; i++)
        {
            double middle = 0.5 * (state.Low + state.High);
            double tolerance = (Tolerance * Math.Abs(state.Best)) + (Tolerance / 3.0);
            if (Math.Abs(state.Best - middle) <= (2.0 * tolerance) - (0.5 * (state.High - state.Low)))
            {
                return state.Best;
            }

            double step = state.NextStep(middle, tolerance);
            double next = state.Best + Nudged(step, tolerance);
            state = state.Accept(next, objective(next));
        }

        return state.Best;
    }

    /// <summary>A step shorter than the tolerance is taken as one exactly that long, signed.</summary>
    private static double Nudged(double step, double tolerance)
    {
        if (Math.Abs(step) >= tolerance)
        {
            return step;
        }

        return step > 0.0 ? tolerance : -tolerance;
    }

    /// <summary>One evaluated point: where it is, and what the objective reads there.</summary>
    private readonly struct Point
    {
        internal Point(double at, double value)
        {
            At = at;
            Value = value;
        }

        internal double At { get; }

        internal double Value { get; }
    }

    /// <summary>The bracket and the three best points in it, which is the whole of Brent's state.</summary>
    private readonly struct Search
    {
        private Search(double low, double high, Point best, Point second, Point third, double lastStep)
        {
            Low = low;
            High = high;
            BestPoint = best;
            SecondPoint = second;
            ThirdPoint = third;
            LastStep = lastStep;
        }

        internal double Low { get; }

        internal double High { get; }

        /// <summary>Where the smallest value found so far sits.</summary>
        internal double Best => BestPoint.At;

        private Point BestPoint { get; }

        private Point SecondPoint { get; }

        private Point ThirdPoint { get; }

        private double LastStep { get; }

        internal static Search Start(Func<double, double> objective, double low, double high)
        {
            double x = low + (Golden * (high - low));
            var point = new Point(x, objective(x));

            return new Search(low, high, point, point, point, 0.0);
        }

        /// <summary>The parabolic step where it is safe, and the golden-section one otherwise.</summary>
        internal double NextStep(double middle, double tolerance)
        {
            if (Math.Abs(LastStep) > tolerance && TryParabolic(tolerance, out double parabolic))
            {
                return parabolic;
            }

            return Golden * ((Best >= middle ? Low : High) - Best);
        }

        internal Search Accept(double next, double value)
        {
            double step = next - Best;
            var point = new Point(next, value);
            if (value <= BestPoint.Value)
            {
                double low = next >= Best ? Best : Low;
                double high = next >= Best ? High : Best;

                return new Search(low, high, point, BestPoint, SecondPoint, step);
            }

            double narrowedLow = next < Best ? next : Low;
            double narrowedHigh = next < Best ? High : next;

            if (value <= SecondPoint.Value || Same(SecondPoint.At, Best))
            {
                return new Search(narrowedLow, narrowedHigh, BestPoint, point, SecondPoint, step);
            }

            if (value <= ThirdPoint.Value || Same(ThirdPoint.At, Best) || Same(ThirdPoint.At, SecondPoint.At))
            {
                return new Search(narrowedLow, narrowedHigh, BestPoint, SecondPoint, point, step);
            }

            return new Search(narrowedLow, narrowedHigh, BestPoint, SecondPoint, ThirdPoint, step);
        }

        /// <summary>The step through the parabola fitted to the three best points, if it may be taken.</summary>
        /// <remarks>
        /// Refused when it would leave the bracket or fail to shrink: a parabola through three
        /// nearly collinear points can leap out of it, which is what the golden section is for.
        /// </remarks>
        private bool TryParabolic(double tolerance, out double step)
        {
            step = 0.0;
            double r = (Best - SecondPoint.At) * (BestPoint.Value - ThirdPoint.Value);
            double q = (Best - ThirdPoint.At) * (BestPoint.Value - SecondPoint.Value);
            double p = ((Best - ThirdPoint.At) * q) - ((Best - SecondPoint.At) * r);
            q = 2.0 * (q - r);
            if (q > 0.0)
            {
                p = -p;
            }

            q = Math.Abs(q);

            // S1244: a degenerate parabola has an exactly zero denominator, and only that.
#pragma warning disable S1244
            if (q == 0.0)
#pragma warning restore S1244
            {
                return false;
            }

            if (Math.Abs(p) >= Math.Abs(0.5 * q * LastStep)
                || p <= q * (Low - Best)
                || p >= q * (High - Best))
            {
                return false;
            }

            step = p / q;
            double next = Best + step;
            if (next - Low < 2.0 * tolerance || High - next < 2.0 * tolerance)
            {
                step = Best < 0.5 * (Low + High) ? tolerance : -tolerance;
            }

            return true;
        }

        // S1244: identity of the tracked points, which the search assigns from one another, so
        // they are the same double or a different one -- never nearly the same.
#pragma warning disable S1244
        private static bool Same(double left, double right) => left == right;
#pragma warning restore S1244
    }
}
