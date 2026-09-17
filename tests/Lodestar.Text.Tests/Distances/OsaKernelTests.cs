using Lodestar.Text.Distances;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

/// <summary>
/// The affix trim and the bit-parallel kernel against the plain three-row dynamic program they
/// replace. The oracle corpus is code-point only, so the UTF-16 path is pinned here.
/// </summary>
public sealed class OsaKernelTests
{
    private const string Narrow = "abc";
    private const string Latin1 = "abcd\u00e9\u00ff ";
    private const string Wide = "ab\u0101\u4e2d";

    public static TheoryData<string, int> Shapes => new()
    {
        { Narrow, 8 },
        { Narrow, 70 },
        { Latin1, 66 },
        { Wide, 70 },
        { Latin1, 140 },
    };

    [Theory]
    [MemberData(nameof(Shapes))]
    public void Distance_equals_the_untrimmed_dynamic_program(string alphabet, int maxLength)
    {
        // CA5394: a seeded generator gives the same pairs on every run; nothing here is secret.
#pragma warning disable CA5394
        var random = new Random(maxLength * 31 + alphabet.Length);
        for (int trial = 0; trial < 2_000; trial++)
        {
            string a = Draw(random, alphabet, maxLength);
            string b = random.Next(2) == 0 ? Transpose(random, a) : Draw(random, alphabet, maxLength);
#pragma warning restore CA5394
            int expected = Reference(a, b);
            Assert.Equal(expected, Osa.Distance(a, b));
            Assert.Equal(expected, Osa.Distance<char>(a.AsSpan(), b.AsSpan()));
        }
    }

    [Theory]
    [InlineData(63)]
    [InlineData(64)]
    [InlineData(65)]
    public void A_pattern_at_the_word_boundary_agrees(int length)
    {
        string shorter = new string('a', length - 2) + "bc";
        string longer = "x" + new string('a', length - 3) + "cb" + "\u4e2d";
        Assert.Equal(Reference(shorter, longer), Osa.Distance(shorter, longer));
        Assert.Equal(Reference(longer, shorter), Osa.Distance(longer, shorter));
    }

#pragma warning disable CA5394
    private static string Draw(Random random, string alphabet, int maxLength)
    {
        char[] chars = new char[random.Next(maxLength + 1)];
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = alphabet[random.Next(alphabet.Length)];
        }

        return new string(chars);
    }

    private static string Transpose(Random random, string value)
    {
        char[] chars = value.ToCharArray();
        for (int edit = 0; edit < 3 && chars.Length > 1; edit++)
        {
            int at = random.Next(chars.Length - 1);
            (chars[at], chars[at + 1]) = (chars[at + 1], chars[at]);
        }

        return new string(chars);
    }
#pragma warning restore CA5394

    private static int Reference(string a, string b)
    {
        int[][] d = new int[a.Length + 1][];
        for (int i = 0; i <= a.Length; i++)
        {
            d[i] = new int[b.Length + 1];
            d[i][0] = i;
        }

        for (int j = 0; j <= b.Length; j++)
        {
            d[0][j] = j;
        }

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                int value = Math.Min(Math.Min(d[i - 1][j] + 1, d[i][j - 1] + 1), d[i - 1][j - 1] + cost);
                if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                {
                    value = Math.Min(value, d[i - 2][j - 2] + 1);
                }

                d[i][j] = value;
            }
        }

        return d[a.Length][b.Length];
    }
}
