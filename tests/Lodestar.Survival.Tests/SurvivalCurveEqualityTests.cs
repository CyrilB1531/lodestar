using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>Decision 0112: the curves compare their arrays by value.</summary>
public sealed class SurvivalCurveEqualityTests
{
    private static SurvivalStep[] Steps() =>
        [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];

    private static KaplanMeierCurve Kaplan(double[] survival) =>
        new(Steps(), survival, [0.4, 0.3], [0.9, 0.8], 0.95);

    [Fact]
    public void Two_kaplan_meier_curves_holding_the_same_values_are_equal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = Kaplan([1.0, 0.75]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_one_estimate_is_unequal()
    {
        Assert.NotEqual(Kaplan([1.0, 0.75]), Kaplan([1.0, 0.5]));
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_a_step_is_unequal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = new(
            [new(0.0, 4, 0, 0), new(2.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_its_level_is_unequal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = new(Steps(), [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.90);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Two_nelson_aalen_curves_holding_the_same_values_are_equal()
    {
        NelsonAalenCurve left = new(Steps(), [0.0, 0.25]);
        NelsonAalenCurve right = new(Steps(), [0.0, 0.25]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_nelson_aalen_curve_differing_in_one_hazard_is_unequal()
    {
        Assert.NotEqual(
            new NelsonAalenCurve(Steps(), [0.0, 0.25]),
            new NelsonAalenCurve(Steps(), [0.0, 0.50]));
    }

    [Fact]
    public void A_nelson_aalen_curve_of_a_differing_length_is_unequal()
    {
        Assert.NotEqual(
            new NelsonAalenCurve(Steps(), [0.0, 0.25]),
            new NelsonAalenCurve([new(0.0, 4, 0, 0)], [0.0]));
    }
}
