using System.Reflection;
using System.Runtime.Loader;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>
/// <c>Lodestar.Text</c> 0.6.0, as published, still vectorizes against this build (#1142).
/// </summary>
/// <remarks>
/// That release was compiled against <see cref="CsrMatrix.CreateUnchecked"/> while it was internal and
/// reached through an <c>InternalsVisibleTo</c> this package no longer grants. The signature is what
/// the runtime binds, so it keeps working because the member is public now; made internal again, the
/// call fails here with <see cref="MethodAccessException"/> rather than in a consumer's vectorizer.
/// </remarks>
public sealed class PublishedTextCompatibilityTests
{
    private static readonly string[] Documents = ["the cat eats", "the dog eats"];

    [Fact]
    public void The_published_Text_0_6_0_vectorizes_against_this_build()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "published", "Lodestar.Text.dll");
        Assert.True(File.Exists(path), $"the published Lodestar.Text 0.6.0 was not copied to {path}");

        // The context holds no Lodestar.Abstractions, so it defers to the default one, which already
        // holds the build under test: that is the resolution a consumer's process makes.
        var context = new AssemblyLoadContext("published-lodestar-text", isCollectible: true);
        try
        {
            Assembly text = context.LoadFromAssemblyPath(path);
            Assert.Equal(new Version(0, 6, 0, 0), text.GetName().Version);

            Type vectorizer = text.GetType("Lodestar.Text.Vectorization.CountVectorizer", throwOnError: true)!;
            MethodInfo fitTransform = vectorizer.GetMethods()
                .Single(m => m.Name == "FitTransform" && m.GetParameters().Length == 1);
            // Its one constructor takes optional options; reflection passes the default explicitly.
            ConstructorInfo constructor = vectorizer.GetConstructors().Single(c => c.GetParameters().Length == 1);
            object instance = constructor.Invoke([Type.Missing]);

            object result = fitTransform.Invoke(instance, [Documents])!;

            CsrMatrix matrix = Assert.IsType<CsrMatrix>(result);
            Assert.Equal(2, matrix.RowCount);
            Assert.Equal(3.0, matrix.RowL1Norm(0));
        }
        finally
        {
            context.Unload();
        }
    }
}
