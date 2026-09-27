using Lodestar.Internal;
using Xunit;

namespace Lodestar.Text.Tests.Vectorization;

// CA5394/S2245 (insecure randomness): a seeded Random draws the cases so that a failure
// reproduces; nothing here is a secret.
#pragma warning disable CA5394, S2245

/// <summary>The string order the vocabulary and Rake's tie-break share, Python's order for a <c>str</c> (#1264).</summary>
public sealed class CodePointOrderTests
{
    // Every case a surrogate can decide: a pair against the BMP above and below it, a pair against
    // a lone half, two pairs sharing their leading unit, and a lone half against ordinary text.
    private static readonly string[] Pieces =
    [
        "a", "z", "\u00E9", "\uD7FF", "\uE000", "\uFF76", "\uFFFF",
        "\U00010000", "\U0001F600", "\U0001F601", "\U00020B9F",
        "\uD83D", "\uDE00", "\uD800", "\uDFFF",
    ];

    [Fact]
    public void Random_strings_sort_as_their_code_points_do()
    {
        var rng = new Random(1264);
        for (int k = 0; k < 20_000; k++)
        {
            string prefix = Draw(rng, 3);
            string x = prefix + Draw(rng, 4);
            string y = prefix + Draw(rng, 4);

            Assert.True(
                Math.Sign(Reference(x, y)) == Math.Sign(CodePointOrder.Instance.Compare(x, y)),
                $"{Escape(x)} against {Escape(y)}");
        }
    }

    // Built in code: an attribute stores its strings as UTF-8, where a lone surrogate becomes U+FFFD.
    private static readonly (string X, string Y, int Expected)[] Decisive =
    [
        ("\uFF76", "\U00020B9F", -1),
        ("\U0001F600", "\uD83D", 1),
        ("\U0001F600", "\uD83Dz", 1),
        ("\U0001F600", "\uD83D\uE000", 1),
        ("\U0001F600", "\U0001F601", -1),
        ("\uDE00", "\uE000", -1),
        ("ab", "abc", -1),
        ("abc", "abc", 0),
    ];

    [Fact]
    public void The_first_differing_code_point_decides()
    {
        foreach ((string x, string y, int expected) in Decisive)
        {
            Assert.Equal(expected, Math.Sign(CodePointOrder.Instance.Compare(x, y)));
            // S2234: the arguments are swapped on purpose, to check the order is antisymmetric.
#pragma warning disable S2234
            Assert.Equal(-expected, Math.Sign(CodePointOrder.Instance.Compare(y, x)));
#pragma warning restore S2234
        }
    }

    [Fact]
    public void Null_sorts_first_and_equals_only_itself()
    {
        Assert.Equal(0, CodePointOrder.Instance.Compare(null, null));
        Assert.True(CodePointOrder.Instance.Compare(null, "") < 0);
        Assert.True(CodePointOrder.Instance.Compare("", null) > 0);
    }

    private static string Draw(Random rng, int max)
    {
        int n = rng.Next(max + 1);
        var parts = new string[n];
        for (int i = 0; i < n; i++)
        {
            parts[i] = Pieces[rng.Next(Pieces.Length)];
        }
        return string.Concat(parts);
    }

    // Decodes both sides as Python holds a str, a pair as one scalar and a lone half as itself,
    // then compares the scalars lexicographically.
    private static int Reference(string x, string y)
    {
        List<int> a = Scalars(x);
        List<int> b = Scalars(y);
        for (int i = 0; i < Math.Min(a.Count, b.Count); i++)
        {
            if (a[i] != b[i])
            {
                return a[i].CompareTo(b[i]);
            }
        }
        return a.Count.CompareTo(b.Count);
    }

    private static List<int> Scalars(string s)
    {
        var scalars = new List<int>();
        int i = 0;
        while (i < s.Length)
        {
            bool pair = char.IsHighSurrogate(s[i]) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]);
            scalars.Add(pair ? char.ConvertToUtf32(s[i], s[i + 1]) : s[i]);
            i += pair ? 2 : 1;
        }
        return scalars;
    }

    private static string Escape(string s) =>
        string.Concat(s.Select(c => c < 0x80 ? c.ToString() : $"\\u{(int)c:X4}"));
}
