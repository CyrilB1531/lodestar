using System;

namespace Lodestar.Internal;

/// <summary>The disposal guard, apart so a library holding nothing disposable does not compile it (#1490).</summary>
internal static partial class Guard
{
    /// <summary>Throws <see cref="ObjectDisposedException"/> if <paramref name="disposed"/> is set.</summary>
    /// <param name="disposed">Whether <paramref name="instance"/> has already been disposed.</param>
    /// <param name="instance">The instance the call was made on, which names the exception.</param>
    public static void NotDisposed(bool disposed, object instance)
    {
#if NET
        ObjectDisposedException.ThrowIf(disposed, instance);
#else
        if (disposed)
        {
            throw new ObjectDisposedException(instance?.GetType().FullName);
        }
#endif
    }
}
