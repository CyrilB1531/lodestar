using BenchmarkDotNet.Attributes;
using Lodestar.Text.Phonetics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible name list; no security use.
#pragma warning disable S2245, CA5394

/// <summary><see cref="DoubleMetaphone"/> over a thousand surname-shaped strings.</summary>
[MemoryDiagnoser]
public class DoubleMetaphoneBenchmarks
{
    private static readonly string[] Syllables =
        ["SCH", "MID", "KOW", "AL", "CZYK", "WITZ", "GH", "ACH", "TH", "ER", "SON", "PH", "IL", "EAU", "GN", "ORE", "ANO", "WSKI"];

    private string[] _names = [];

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(1_000);
        _names = new string[1_000];
        for (int i = 0; i < _names.Length; i++)
        {
            int parts = 2 + random.Next(3);
            var name = new System.Text.StringBuilder();
            for (int p = 0; p < parts; p++)
            {
                name.Append(Syllables[random.Next(Syllables.Length)]);
            }

            _names[i] = name.ToString();
        }
    }

    [Benchmark]
    public int Encode()
    {
        int total = 0;
        foreach (string name in _names)
        {
            total += DoubleMetaphone.Encode(name).Primary.Length;
        }

        return total;
    }
}
