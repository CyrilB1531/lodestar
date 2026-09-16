using Lodestar.Cluster;

namespace Lodestar.Sample;

/// <summary>Density clustering, where the cluster count is an output and noise is allowed.</summary>
internal static class DbscanSample
{
    public static void Run()
    {
        Console.WriteLine("DBSCAN (Lodestar.Cluster)");

        // Row-major, two features per row: three points near the origin, two near (5, 5),
        // and one out at (9, 9) that no dense region reaches.
        double[] samples = [0.0, 0.0, 0.0, 0.3, 0.3, 0.0, 5.0, 5.0, 5.0, 5.2, 9.0, 9.0];

        Dbscan model = Dbscan.Fit(samples, featureCount: 2, epsilon: 0.5, minimumSamples: 2);

        Console.WriteLine($"  {model.ClusterCount} clusters x {model.FeatureCount} features");
        Console.WriteLine($"  labels           : [{string.Join(", ", model.Labels)}] ({Dbscan.Noise} is noise)");
        Console.WriteLine($"  core samples     : [{string.Join(", ", model.CoreSampleIndices)}]");

        // The same answer from the distances alone, which is what metric="precomputed" is for.
        double[] distances = Pairwise(samples, featureCount: 2, sampleCount: 6);
        Dbscan fromDistances = Dbscan.FitPrecomputed(distances, sampleCount: 6, epsilon: 0.5, minimumSamples: 2);

        Console.WriteLine($"  from distances   : [{string.Join(", ", fromDistances.Labels)}]");
        Console.WriteLine();
    }

    private static double[] Pairwise(double[] samples, int featureCount, int sampleCount)
    {
        var distances = new double[sampleCount * sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            for (int other = 0; other < sampleCount; other++)
            {
                double total = 0.0;
                for (int feature = 0; feature < featureCount; feature++)
                {
                    double gap = samples[(row * featureCount) + feature]
                        - samples[(other * featureCount) + feature];
                    total += gap * gap;
                }

                distances[(row * sampleCount) + other] = Math.Sqrt(total);
            }
        }

        return distances;
    }
}
