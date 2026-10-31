namespace Lodestar.Survival.Internal;

/// <summary>The step halving both Newton fits search with.</summary>
internal static class NewtonSearch
{
    // A fall within this share of the likelihood is rounding: the last steps of a converging fit wander there.
    private const double RoundingFall = 1e-12;

    // Past this many halvings the step is 2^-80 of the first.
    private const int MaximumHalvings = 80;

    /// <summary>The first of 1, ½, ¼, … whose value is a number no lower than <paramref name="current"/> past rounding,
    /// or NaN where none of <see cref="MaximumHalvings"/> is, or where a step no longer moves the point, which
    /// <paramref name="valueAt"/> reports as null: the point is then a maximum to rounding, or the search is stuck, which
    /// each fit tells apart by its own rule.</summary>
    internal static double Scale(double current, Func<double, double?> valueAt)
    {
        double floor = current - (RoundingFall * Math.Abs(current));
        double scale = 1.0;
        for (int halving = 0; halving < MaximumHalvings; halving++)
        {
            double? value = valueAt(scale);
            if (value is null)
            {
                return double.NaN;
            }

            if (value.Value >= floor)
            {
                return scale;
            }

            scale /= 2.0;
        }

        return double.NaN;
    }
}
