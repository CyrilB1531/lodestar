using Aglomera;

namespace Lodestar.Text.Benchmarks;

/// <summary>A sample as <c>Aglomera</c> 1.1.1 needs one: comparable, and carrying its row.</summary>
/// <remarks>
/// The comparison is by row alone, which is what lets the merge tree be read back as sets of rows
/// and compared with this package's, cluster ids being arbitrary across libraries.
/// </remarks>
internal sealed class AgglomeraPoint : IComparable<AgglomeraPoint>, IEquatable<AgglomeraPoint>
{
    public AgglomeraPoint(int row, double[] values)
    {
        Row = row;
        Values = values;
    }

    public int Row { get; }

    // CA1819: the one caller reads the values once per distance; copying them out would price
    // an allocation into the incumbent's column that it does not make itself.
#pragma warning disable CA1819
    public double[] Values { get; }
#pragma warning restore CA1819

    public static bool operator ==(AgglomeraPoint? left, AgglomeraPoint? right) => Equals(left, right);

    public static bool operator !=(AgglomeraPoint? left, AgglomeraPoint? right) => !Equals(left, right);

    public static bool operator <(AgglomeraPoint left, AgglomeraPoint right) => left.CompareTo(right) < 0;

    public static bool operator <=(AgglomeraPoint left, AgglomeraPoint right) => left.CompareTo(right) <= 0;

    public static bool operator >(AgglomeraPoint left, AgglomeraPoint right) => left.CompareTo(right) > 0;

    public static bool operator >=(AgglomeraPoint left, AgglomeraPoint right) => left.CompareTo(right) >= 0;

    public int CompareTo(AgglomeraPoint? other) => other is null ? 1 : Row.CompareTo(other.Row);

    public bool Equals(AgglomeraPoint? other) => other is not null && Row == other.Row;

    public override bool Equals(object? obj) => obj is AgglomeraPoint other && Equals(other);

    public override int GetHashCode() => Row;
}

/// <summary>Euclidean distance summed in feature order, the same arithmetic this package uses.</summary>
internal sealed class AgglomeraEuclidean : IDissimilarityMetric<AgglomeraPoint>
{
    public double Calculate(AgglomeraPoint instance1, AgglomeraPoint instance2)
    {
        double total = 0.0;
        for (int feature = 0; feature < instance1.Values.Length; feature++)
        {
            double gap = instance1.Values[feature] - instance2.Values[feature];
            total += gap * gap;
        }

        return Math.Sqrt(total);
    }
}
