using System;
#if NET
using System.Diagnostics;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Lodestar.Internal;

/// <summary>Small argument guards shared across the libraries (compiled into each assembly).</summary>
/// <remarks>
/// The range guard calls <c>ArgumentOutOfRangeException.ThrowIfLessThan</c> on net10 and checks by
/// hand on the netstandard targets; the null guard checks by hand everywhere, generic so it need not box
/// a value type, its frames hidden from stack traces on net10 alone, as netstandard lacks the attribute.
/// Those splits, and the one in <c>Guard.Disposal.cs</c>, which <c>Lodestar.Abstractions</c> does
/// not compile (#1490), keep every call site clean.
/// </remarks>
internal static partial class Guard
{
    /// <summary>Throws <see cref="ArgumentNullException"/> if <paramref name="value"/> is null.</summary>
    /// <remarks>
    /// Generic, so a value of a generic type that a value type closes is not boxed into an object, the way
    /// <c>ArgumentNullException.ThrowIfNull</c> takes it. The <c>is null</c> test is a box a JIT folds away; one that
    /// does not, as an old Mono or the .NET Framework x86 JIT may not, still boxes a nullable value type here.
    /// </remarks>
#if NET
    [StackTraceHidden]
#endif
    public static void NotNull<T>(
        [NotNull] T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            ThrowNull(paramName);
        }
    }

    // Out of line, so the exception's construction is in this one method rather than in every instantiation's body.
    [DoesNotReturn]
#if NET
    [StackTraceHidden]
#endif
    private static void ThrowNull(string? paramName) => throw new ArgumentNullException(paramName);

    /// <summary>Throws <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is below <paramref name="min"/>.</summary>
    public static void NotLessThan(
        int value,
        int min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThan(value, min, paramName);
#else
        if (value < min)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Must be at least {min}.");
        }
#endif
    }
}
