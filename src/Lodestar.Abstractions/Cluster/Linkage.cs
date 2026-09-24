namespace Lodestar.Cluster;

/// <summary>How the distance between two clusters is measured when deciding which to merge.</summary>
/// <remarks>
/// scikit-learn's <c>linkage=</c>. <see cref="Single"/> runs a minimum spanning tree and needs
/// no distance matrix; the other three run the nearest-neighbour chain over one, which is
/// <c>n(n − 1)/2</c> doubles.
/// </remarks>
public enum Linkage
{
    /// <summary>The merge that least increases the within-cluster variance; scikit-learn's default.</summary>
    Ward,

    /// <summary>The largest distance between a member of one cluster and a member of the other.</summary>
    Complete,

    /// <summary>The mean distance between the members of one cluster and the members of the other.</summary>
    Average,

    /// <summary>The smallest distance between a member of one cluster and a member of the other.</summary>
    // CA1720 reads "Single" as System.Single. It is scikit-learn's linkage="single" and the
    // field's standard name, and Linkage.SingleLinkage would stutter at every call site.
#pragma warning disable CA1720
    Single,
#pragma warning restore CA1720
}
