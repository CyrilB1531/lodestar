using Meta.Numerics.Statistics;
using NumFlat;
using LodestarKMeans = Lodestar.Cluster.KMeans;
using NumFlatKMeans = NumFlat.Clustering.KMeans;

namespace Lodestar.Text.Benchmarks;

/// <summary>The agreement both k-means benchmarks require before anything is timed.</summary>
internal static class KMeansAgreement
{
    private const double Tolerance = 1e-9;

    public static void RequireSameCentres(LodestarKMeans ours, NumFlatKMeans theirs, bool identity, ClusterBlobs blobs)
    {
        var centres = new double[theirs.ClassCount][];
        for (int cluster = 0; cluster < centres.Length; cluster++)
        {
            Vec<double> centroid = theirs.Centroids[cluster];
            centres[cluster] = [.. Enumerable.Range(0, centroid.Count).Select(i => centroid[i])];
        }

        Require(ours, centres, identity, blobs, "NumFlat");
    }

    public static void RequireSameCentres(LodestarKMeans ours, MeansClusteringResult theirs, ClusterBlobs blobs)
    {
        var centres = new double[theirs.Count][];
        for (int cluster = 0; cluster < centres.Length; cluster++)
        {
            Meta.Numerics.Matrices.ColumnVector centroid = theirs.Centroid(cluster);
            centres[cluster] = [.. Enumerable.Range(0, centroid.Dimension).Select(i => centroid[i])];
        }

        Require(ours, centres, identity: false, blobs, "Meta.Numerics");
    }

    private static void Require(LodestarKMeans ours, double[][] theirs, bool identity, ClusterBlobs blobs, string name)
    {
        int features = blobs.FeatureCount;
        var used = new bool[theirs.Length];
        for (int cluster = 0; cluster < ours.ClusterCount; cluster++)
        {
            // Same order when both started from the same centres; nearest unused otherwise.
            int match = identity ? cluster : Nearest(ours, cluster, theirs, used, features);
            used[match] = true;
            for (int feature = 0; feature < features; feature++)
            {
                double a = ours.Centres[(cluster * features) + feature];
                double b = theirs[match][feature];
                if (Math.Abs(a - b) > Tolerance * Math.Max(1.0, Math.Abs(a)))
                {
                    throw new InvalidOperationException(
                        $"{name} disagrees on centre {cluster}, feature {feature}: {a:R} against {b:R}.");
                }
            }
        }
    }

    private static int Nearest(LodestarKMeans ours, int cluster, double[][] theirs, bool[] used, int features)
    {
        int best = -1;
        double bestDistance = double.PositiveInfinity;
        for (int candidate = 0; candidate < theirs.Length; candidate++)
        {
            if (used[candidate])
            {
                continue;
            }

            double distance = 0.0;
            for (int feature = 0; feature < features; feature++)
            {
                double d = ours.Centres[(cluster * features) + feature] - theirs[candidate][feature];
                distance += d * d;
            }

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = candidate;
            }
        }

        return best;
    }
}
