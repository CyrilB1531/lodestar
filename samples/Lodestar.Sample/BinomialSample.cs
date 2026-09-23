using Lodestar.Stats;

namespace Lodestar.Sample;

/// <summary>The binomial test — exact at any number of trials.</summary>
internal static class BinomialSample
{
    public static void Run()
    {
        Console.WriteLine("Binomial test");

        BinomialResult twoSided = Binomial.Test(successes: 7, trials: 20, probability: 0.5);
        BinomialResult greater = Binomial.Test(7, 20, 0.5, Alternative.Greater);

        Console.WriteLine($"  proportion            = {Inv.F4(twoSided.Statistic)}");
        Console.WriteLine($"  two-sided p           = {Inv.F4(twoSided.PValue)}");

        // Not half the two-sided value: the binomial is discrete, and asymmetric
        // whenever the hypothesised probability is not one half.
        Console.WriteLine($"  one-sided p           = {Inv.F4(greater.PValue)}");
    }
}
