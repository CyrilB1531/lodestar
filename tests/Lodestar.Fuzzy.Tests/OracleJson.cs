using System.Text.Json;

namespace Lodestar.Fuzzy.Tests;

/// <summary>What the rapidfuzz corpora share in how they are read.</summary>
internal static class OracleJson
{
    /// <summary>The corpus's strings, a JSON null — rapidfuzz's <c>None</c> — kept as null (#1233).</summary>
    public static string?[] Strings(JsonElement array) =>
        [.. array.EnumerateArray().Select(v => v.GetString())];
}
