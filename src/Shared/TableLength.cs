namespace Lodestar.Internal;

/// <summary>The length of a row-major table sized by the caller, and the largest array it may be.</summary>
/// <remarks>
/// .NET 6 and later cap every array at <c>Array.MaxLength</c>, <c>0x7FFFFFC7</c>. Earlier runtimes the
/// netstandard builds reach — .NET Framework, .NET Core, .NET 5 — cap an array of elements wider than one
/// byte lower, at <c>0x7FEFFFFF</c> (.NET Framework past 2 GB only with <c>gcAllowVeryLargeObjects</c>).
/// Legacy Mono, which names itself "Mono" (Unity, classic Xamarin), is held there unmeasured; the Mono behind
/// .NET 6 and later reports ".NET". The bound is read from the runtime once, so a count between the two is
/// refused rather than left to fail in the allocation (#1614).
/// </remarks>
internal static class TableLength
{
    /// <summary><c>Array.MaxLength</c> from .NET 6 on, and the bound every refusal read on any runtime before #1614.</summary>
    /// <remarks>
    /// A refusal that runs ahead of others keeps checking this where it did, so an input refused for another reason
    /// stays refused for it, and checks <see cref="MaxLength"/> once those have run.
    /// </remarks>
    public const long ArrayMaxLength = 0x7FFFFFC7;

    /// <summary>The most elements one <see cref="byte"/> array holds: <see cref="ArrayMaxLength"/> before and after .NET 6.</summary>
    /// <remarks>.NET Framework, .NET Core and .NET 6 agree on it; legacy Mono is unmeasured.</remarks>
    public const long MaxByteLength = ArrayMaxLength;

    /// <summary>The most elements one array of a type wider than one byte holds on this runtime.</summary>
#if NET6_0_OR_GREATER
    public static readonly long MaxLength = Array.MaxLength;
#else
    public static readonly long MaxLength = Bound(SafeDescription());
#endif

    /// <summary>The cell count, refused rather than wrapped in <c>int</c> past the largest array (#1314).</summary>
    /// <exception cref="ArgumentException">The table holds more cells than one array can.</exception>
    public static int Of(int rows, int columns, string paramName)
    {
        long length = (long)rows * columns;
        if (length > MaxLength)
        {
            throw new ArgumentException(
                $"{rows} rows of {columns} is {length} cells, more than one array holds.", paramName);
        }

        return (int)length;
    }

    /// <summary>The bound the runtime <paramref name="frameworkDescription"/> names puts on a wider array.</summary>
    /// <param name="frameworkDescription">
    /// <c>RuntimeInformation.FrameworkDescription</c>, or a test's stand-in; null reads as unknown.
    /// </param>
    internal static long Bound(string? frameworkDescription) =>
        IsNet6OrLater(frameworkDescription) ? ArrayMaxLength : 0x7FEFFFFF;

#if !NET6_0_OR_GREATER
    /// <summary>The runtime's description, or null where a host cannot give one, which takes the lower bound.</summary>
    /// <remarks>
    /// Around a separate, non-inlined call: a facade that fails to load — an old .NET Framework with a broken
    /// binding redirect — fails when that call is compiled, which a <c>try</c> in the same method would not
    /// see, and would otherwise leave every package's bound a <c>TypeInitializationException</c>.
    /// </remarks>
    private static string? SafeDescription()
    {
        try
        {
            return Description();
        }
        // CA1031: any failure to describe the runtime reads as unknown, whose lower bound refuses nothing it holds.
#pragma warning disable CA1031
        catch (Exception)
#pragma warning restore CA1031
        {
            return null;
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static string Description() => System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#endif

    /// <summary>Whether the description is ".NET" at major version 6 or more, with <c>Array.MaxLength</c>.</summary>
    /// <remarks>".NET Framework 4.8", ".NET Core 3.1" and 2.x's "4.6", ".NET 5.0" and "Mono 6.12" are not.</remarks>
    private static bool IsNet6OrLater(string? frameworkDescription)
    {
        const string Prefix = ".NET ";
        if (frameworkDescription is null || !frameworkDescription.StartsWith(Prefix, StringComparison.Ordinal))
        {
            return false;
        }

        // The major version's digits, read until they reach 6, so a long run of them cannot overflow.
        int major = 0;
        int end = Prefix.Length;
        while (end < frameworkDescription.Length && frameworkDescription[end] is >= '0' and <= '9' && major < 6)
        {
            major = (major * 10) + (frameworkDescription[end] - '0');
            end++;
        }

        return major >= 6;
    }
}
