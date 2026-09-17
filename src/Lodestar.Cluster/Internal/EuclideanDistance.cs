namespace Lodestar.Cluster.Internal;

/// <summary>The distance between two rows of a row-major matrix.</summary>
internal static class EuclideanDistance
{
    /// <summary>The square root of the squared gaps, summed in feature order.</summary>
    /// <remarks>
    /// Feature order is not incidental. A tie between two merge distances decides the tree, and
    /// a sum taken in another order can differ in the last bit; this one is bit-identical to
    /// <c>scipy.spatial.distance.pdist</c>, measured on 780 pairs (#760).
    /// </remarks>
    public static double Between(ReadOnlySpan<double> samples, int featureCount, int left, int right)
    {
        ReadOnlySpan<double> a = samples.Slice(left * featureCount, featureCount);
        ReadOnlySpan<double> b = samples.Slice(right * featureCount, featureCount);
        double total = 0.0;
        for (int feature = 0; feature < a.Length; feature++)
        {
            double gap = a[feature] - b[feature];
            total += gap * gap;
        }

        return Math.Sqrt(total);
    }
}
