using Lodestar.Cluster;

namespace Lodestar.Sample;

/// <summary>The two ways a fit can start, and which of them travels.</summary>
internal static class KMeansOptionsSample
{
    public static void Run()
    {
        Console.WriteLine("k-means options (Lodestar.Cluster)");

        double[] samples = [0.0, 0.0, 0.0, 1.0, 10.0, 10.0, 10.0, 11.0, 5.0, 5.0];

        var given = new KMeansOptions { InitialCentres = [0.0, 0.0, 10.0, 10.0, 5.0, 5.0] };
        var drawn = new KMeansOptions { Seed = 11, MaxIterations = 50, Tolerance = 1e-6 };

        // A seed reproduces a run of Lodestar; only InitialCentres reproduces a run of Python.
        Console.WriteLine($"  from given centres : inertia {Inv.F4(KMeans.Fit(samples, 2, 3, given).Inertia)}");
        Console.WriteLine($"  from a drawn start : inertia {Inv.F4(KMeans.Fit(samples, 2, 3, drawn).Inertia)}");
        Console.WriteLine($"  max {drawn.MaxIterations} iterations, tolerance {Inv.E3(drawn.Tolerance)}");

        // Several starts, scikit-learn's n_init: the lowest inertia on a new partition wins.
        var sets = new KMeansOptions
        {
            InitialCentreSets = [[0.0, 0.0, 0.0, 1.0, 10.0, 10.0], [0.0, 0.0, 10.0, 10.0, 5.0, 5.0]],
        };
        var restarts = new KMeansOptions { Seed = 11, Restarts = 4 };
        Console.WriteLine($"  best of {sets.InitialCentreSets.Count} starts : inertia {Inv.F4(KMeans.Fit(samples, 2, 3, sets).Inertia)}");
        Console.WriteLine($"  best of {restarts.Restarts} draws  : inertia {Inv.F4(KMeans.Fit(samples, 2, 3, restarts).Inertia)}");
        Console.WriteLine();
    }
}
