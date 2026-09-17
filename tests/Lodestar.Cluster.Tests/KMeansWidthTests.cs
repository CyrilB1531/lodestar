using Xunit;

namespace Lodestar.Cluster.Tests;

// CA5394, S2245: a seeded Random builds a reproducible dataset; no security use.
#pragma warning disable CA5394, S2245

/// <summary>
/// Rows wider than four features are assigned by a second loop; padding a narrow dataset with
/// zero columns adds only +0.0 to every distance, so both loops must give the same fit.
/// </summary>
public sealed class KMeansWidthTests
{
    [Fact]
    public void A_wide_fit_agrees_with_the_narrow_one_it_pads()
    {
        const int Width = 7;
        var random = new Random(933);
        double[] narrow = [.. Enumerable.Range(0, 400).Select(_ => (random.NextDouble() * 10.0) - 5.0)];
        var wide = new double[narrow.Length / 2 * Width];
        for (int row = 0; row < narrow.Length / 2; row++)
        {
            wide[row * Width] = narrow[row * 2];
            wide[(row * Width) + 1] = narrow[(row * 2) + 1];
        }

        // Tolerance 0: the stopping threshold is scaled by the feature count, which padding changes.
        double[] start = [.. narrow.Take(10)];
        KMeans reference = KMeans.Fit(narrow, 2, 5, new KMeansOptions { InitialCentres = start, Tolerance = 0.0 });
        KMeans widened = KMeans.Fit(wide, Width, 5, new KMeansOptions { InitialCentres = Pad(start, Width), Tolerance = 0.0 });

        Assert.Equal(reference.Labels, widened.Labels);
        Assert.Equal(reference.Inertia, widened.Inertia);
        Assert.Equal(reference.Predict(narrow), widened.Predict(wide));
    }

    private static double[] Pad(double[] centres, int width)
    {
        var padded = new double[centres.Length / 2 * width];
        for (int cluster = 0; cluster < centres.Length / 2; cluster++)
        {
            padded[cluster * width] = centres[cluster * 2];
            padded[(cluster * width) + 1] = centres[(cluster * 2) + 1];
        }

        return padded;
    }
}
