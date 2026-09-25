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

        // The rest of each law (#1158): density, lower tail and inverse upper tail.
        Console.WriteLine($"  normal pdf, cdf, sf, isf  = {Inv.F5(Distributions.NormalPdf(1.0))}, "
            + $"{Inv.F5(Distributions.NormalCdf(-1.96))}, {Inv.F5(Distributions.NormalSf(3.0))}, "
            + $"{Inv.F5(Distributions.NormalIsf(0.025))}");
        Console.WriteLine($"  t pdf, cdf, isf           = {Inv.F5(Distributions.StudentPdf(2.0, 5.0))}, "
            + $"{Inv.F5(Distributions.StudentCdf(-2.0, 10.0))}, {Inv.F5(Distributions.StudentIsf(0.025, 12.0))}");
        Console.WriteLine($"  F pdf, cdf, ppf, isf      = {Inv.F5(Distributions.FisherPdf(1.0, 2.0, 20.0))}, "
            + $"{Inv.F5(Distributions.FisherCdf(4.0, 2.0, 20.0))}, {Inv.F5(Distributions.FisherQuantile(0.95, 2.0, 20.0))}, "
            + $"{Inv.F5(Distributions.FisherIsf(0.05, 2.0, 20.0))}");
        Console.WriteLine($"  chi2 pdf, cdf, ppf, isf   = {Inv.F5(Distributions.ChiSquaredPdf(2.0, 4.0))}, "
            + $"{Inv.F5(Distributions.ChiSquaredCdf(3.84, 1.0))}, {Inv.F5(Distributions.ChiSquaredQuantile(0.95, 1.0))}, "
            + $"{Inv.F5(Distributions.ChiSquaredIsf(0.05, 4.0))}");
        Console.WriteLine();
    }
}
