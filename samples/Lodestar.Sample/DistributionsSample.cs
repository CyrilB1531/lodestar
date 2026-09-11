using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>The tails a caller holding its own statistic reaches for.</summary>
internal static class DistributionsSample
{
    public static void Run()
    {
        Console.WriteLine("Distribution tails (Lodestar.Stats)");

        // A coefficient with t = 2.0 on twelve residual degrees of freedom.
        double twoSided = 2.0 * Distributions.StudentSf(2.0, 12.0);
        double multiplier = Distributions.StudentQuantile(0.975, 12.0);

        Console.WriteLine($"  two-sided p      = {Inv.F5(twoSided)}");
        Console.WriteLine($"  95% multiplier   = {Inv.F5(multiplier)}");

        // The overall F test of a model with two regressors and twenty residual df.
        Console.WriteLine($"  overall F tail   = {Inv.F5(Distributions.FisherSf(4.0, 2.0, 20.0))}");

        // A log-rank test's p-value, on one degree of freedom.
        Console.WriteLine($"  log-rank tail    = {Inv.F5(Distributions.ChiSquaredSf(3.84, 1.0))}");

        // The large-sample multiplier, which a survival curve's bounds take.
        Console.WriteLine($"  95% normal z     = {Inv.F5(Distributions.NormalQuantile(0.975))}");

        // Far into the tail, where an absolute tolerance would accept a zero.
        Console.WriteLine($"  far tail         = {Inv.E3(Distributions.StudentSf(30.0, 30.0))}");
        Console.WriteLine();
    }
}
