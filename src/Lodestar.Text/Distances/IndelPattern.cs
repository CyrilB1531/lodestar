using System.Buffers;

namespace Lodestar.Text.Distances;

/// <summary>
/// One pattern's bit-parallel equality table, built once and scanned against many texts — the
/// bulk form of <see cref="Indel"/>, at the same answers.
/// </summary>
/// <remarks>
/// <see cref="Indel"/> rebuilds the pattern's table on every call; a caller holding one pattern
/// and a list pays it once here instead — 0.43 to 0.46 of the pairwise loop over 64 unrelated
/// texts, and at worst 1.18× where a long shared affix leaves the pairwise call almost nothing to
/// scan. UTF-16 units only; scanning is thread-safe, disposal is not.
/// </remarks>
public sealed class IndelPattern : IDisposable
{
    /// <summary>How many units must agree at one end before a pair goes to the pairwise path.</summary>
    /// <remarks>
    /// The table spans the whole pattern, so the handle cannot drop a shared prefix and suffix the
    /// way <see cref="Indel"/> does: over two words that measured 1.56× to 2.14× on 90 shared
    /// units, and the guard brings it to 1.04× to 1.18×. Over one word it is not taken — scanning
    /// the affix costs about what the trim saves, 1.08× at worst unguarded, and a probe that fired
    /// there made a shared suffix 0.78 → 1.02 (#1138).
    /// </remarks>
    private const int Probe = 16;

    private readonly string pattern;
    private readonly int words;
    private ulong[]? table;
    private bool disposed;

    private IndelPattern(string pattern, ulong[]? table, int words)
    {
        this.pattern = pattern;
        this.table = table;
        this.words = words;
    }

    /// <summary>Builds a handle over <paramref name="pattern"/>.</summary>
    /// <param name="pattern">The pattern every scan compares against.</param>
    /// <returns>A handle to dispose when the last text has been scanned.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null.</exception>
    public static IndelPattern For(string pattern)
    {
        Guard.NotNull(pattern);
        return Build(pattern);
    }

    /// <summary>Builds a handle over <paramref name="pattern"/>, which is copied.</summary>
    /// <param name="pattern">The pattern every scan compares against.</param>
    /// <returns>A handle to dispose when the last text has been scanned.</returns>
    /// <remarks>The span cannot be held, so this allocates the string the other overload is given.</remarks>
    public static IndelPattern For(ReadOnlySpan<char> pattern) => Build(pattern.ToString());

    private static IndelPattern Build(string pattern)
    {
        int m = pattern.Length;
        if (m is 0 or > BitParallelLcs.MaxInterleavedPattern)
        {
            return new IndelPattern(pattern, null, 0);
        }

        ulong[] rented = ArrayPool<ulong>.Shared.Rent(BitParallelLcs.InterleavedEntries);
        Span<ulong> peq = rented.AsSpan(0, BitParallelLcs.InterleavedEntries);
        peq.Clear();
        if (!BitParallelLcs.TryFillInterleaved(pattern.AsSpan(), peq))
        {
            // A pattern above Latin-1 needs the side table the pairwise kernel carries, which is
            // established per pair. Nothing to hold, so nothing is held.
            ArrayPool<ulong>.Shared.Return(rented);
            return new IndelPattern(pattern, null, 0);
        }

        return new IndelPattern(pattern, rented, m <= 64 ? 1 : 2);
    }

    /// <summary>The Indel distance between the pattern and <paramref name="text"/>.</summary>
    /// <param name="text">The text to scan.</param>
    /// <returns>The number of insertions and deletions, as <see cref="Indel.Distance"/> gives it.</returns>
    /// <exception cref="ObjectDisposedException">The handle has been disposed.</exception>
    public int Distance(ReadOnlySpan<char> text)
    {
        Guard.NotDisposed(this.disposed, this);

        ReadOnlySpan<char> held = this.pattern.AsSpan();
        ulong[]? peq = this.table;
        if (peq is null || (this.words == 2 && Shares(held, text)))
        {
            return Pairwise(held, text);
        }

        Span<ulong> span = peq.AsSpan(0, BitParallelLcs.InterleavedEntries);
        int common = this.words == 1
            ? BitParallelLcs.ScanOneWordInterleaved(span, held.Length, text)
            : BitParallelLcs.ScanTwoWordsInterleaved(span, held.Length, text);

        return held.Length + text.Length - (2 * common);
    }

    /// <summary>Normalized distance in <c>[0, 1]</c>: <c>distance / (len(pattern) + len(text))</c>.</summary>
    /// <param name="text">The text to scan.</param>
    /// <returns>Zero when both are empty, as <see cref="Indel.NormalizedDistance"/> gives it.</returns>
    /// <exception cref="ObjectDisposedException">The handle has been disposed.</exception>
    public double NormalizedDistance(ReadOnlySpan<char> text)
    {
        // Before the empty-pair shortcut, so two empty strings refuse on a disposed handle
        // rather than answering where every other pair throws.
        Guard.NotDisposed(this.disposed, this);

        int total = this.pattern.Length + text.Length;
        return total == 0 ? 0.0 : (double)this.Distance(text) / total;
    }

    /// <summary>Normalized similarity in <c>[0, 1]</c>. This ×100 is <c>fuzz.ratio</c>.</summary>
    /// <param name="text">The text to scan.</param>
    /// <returns>One minus <see cref="NormalizedDistance"/>.</returns>
    /// <exception cref="ObjectDisposedException">The handle has been disposed.</exception>
    public double NormalizedSimilarity(ReadOnlySpan<char> text) => 1.0 - this.NormalizedDistance(text);

    /// <summary>Returns the table to the pool. Scanning afterwards throws.</summary>
    /// <remarks>
    /// Idempotent, which is why this is a class: a struct holding a rented array is copyable, and
    /// two copies disposed would hand one array to two callers with no exception to show for it.
    /// </remarks>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;
        ulong[]? rented = this.table;
        this.table = null;
        if (rented is not null)
        {
            ArrayPool<ulong>.Shared.Return(rented);
        }
    }

    /// <summary>The pairwise call, with its comparison unit named rather than defaulted.</summary>
    /// <remarks>
    /// <c>Indel.Distance(a, b)</c> over two <c>ReadOnlySpan&lt;char&gt;</c> does <em>not</em> bind
    /// to the character overload: C# prefers a candidate whose parameters all have arguments, so
    /// the generic <c>Distance&lt;char&gt;</c> wins and takes the dynamic program. Same answer,
    /// measured 3× to 9× slower on a pattern past the table's width (#1130).
    /// </remarks>
    private static int Pairwise(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text) =>
        Indel.Distance(pattern, text, TextElement.Utf16Unit);

    /// <summary>Whether enough units agree at either end that the pairwise path's trim would pay.</summary>
    /// <remarks>Both ends, because <c>Affixes.Trim</c> strips both (#1138).</remarks>
    private static bool Shares(ReadOnlySpan<char> pattern, ReadOnlySpan<char> text)
    {
        if (pattern.Length < Probe || text.Length < Probe)
        {
            return false;
        }

        return pattern.Slice(0, Probe).SequenceEqual(text.Slice(0, Probe))
            || pattern.Slice(pattern.Length - Probe).SequenceEqual(text.Slice(text.Length - Probe));
    }
}
