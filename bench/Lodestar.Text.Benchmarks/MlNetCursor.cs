using Microsoft.ML;
using Microsoft.ML.Data;

namespace Lodestar.Text.Benchmarks;

/// <summary>Reading an <see cref="IDataView"/> to the end, which is where ML.NET does its work.</summary>
/// <remarks>
/// Shared by every incumbent benchmark that times an ML.NET estimator: its transforms are lazy,
/// so a <c>Fit</c> alone measures the plan rather than the transform. One copy, because two
/// identical cursor loops in this directory are what SonarCloud's duplication gate counts.
/// </remarks>
internal static class MlNetCursor
{
    /// <summary>Counts the rows, forcing the view to compute every one of them.</summary>
    internal static int Drain(IDataView view)
    {
        int rows = 0;
        using DataViewRowCursor cursor = view.GetRowCursor(view.Schema);
        while (cursor.MoveNext())
        {
            rows++;
        }

        return rows;
    }
}
