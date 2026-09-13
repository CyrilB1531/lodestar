using System.Linq.Expressions;

namespace Lodestar.Extensions.VectorData;

/// <summary>Turns the abstraction's filter expression into something callable.</summary>
/// <remarks>
/// Compiled once per call rather than per record, and never cached across calls: a caller who
/// builds a fresh closure each time would otherwise hold every one of them alive.
/// <c>Expression.Compile</c> needs dynamic code, which is what makes this package's filter path
/// unavailable under trimming and ahead-of-time compilation.
/// </remarks>
internal static class RecordFilter
{
    /// <summary>Compiles the filter, or answers null when there is none to apply.</summary>
    /// <param name="filter">The caller's predicate over the record type.</param>
    public static Func<TRecord, bool>? Compile<TRecord>(Expression<Func<TRecord, bool>>? filter) =>
        filter?.Compile();
}
