using System.Text.Json;

namespace Lodestar.Stats.TimeSeries.Tests.Oracles;

/// <summary>Loads a frozen stats corpus committed under <c>tests/oracles/</c>.</summary>
/// <remarks>
/// Trimmed from <c>Lodestar.Stats.Tests</c>' loader to what the time-series corpora read: the rest names
/// <c>Lodestar.Stats</c> types this suite has no reason to compile against.
/// </remarks>
internal static class StatsCorpus
{
    internal static JsonDocument Load(string fileName)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Oracle '{fileName}' not found at '{path}'. Run tools/generate_oracles.py.", path);
        }

        return JsonDocument.Parse(File.ReadAllText(path));
    }

    /// <summary>One corpus number, decoding the three non-finite spellings.</summary>
    /// <remarks>
    /// The generator writes with <c>allow_nan=False</c>, so a one-sided
    /// confidence bound and an infinite odds ratio arrive as the strings
    /// <c>"Infinity"</c>, <c>"-Infinity"</c> and <c>"NaN"</c> rather than as
    /// tokens no strict JSON reader accepts.
    /// </remarks>
    internal static double Number(JsonElement element) =>
        element.ValueKind == JsonValueKind.String
            ? element.GetString() switch
            {
                "Infinity" => double.PositiveInfinity,
                "-Infinity" => double.NegativeInfinity,
                "NaN" => double.NaN,
                var other => throw new InvalidDataException($"Unknown corpus number '{other}'."),
            }
            : element.GetDouble();

    internal static double[] Doubles(JsonElement element) =>
        [.. element.EnumerateArray().Select(Number)];
}
