using System.Text.Json;
using Lodestar.Preprocessing.Internal;
using Xunit;

namespace Lodestar.Preprocessing.Tests.Internal;

/// <summary>Replays numpy's legacy <c>RandomState</c> over <c>tests/oracles/numpy_random_state.json</c> (#1157).</summary>
/// <remarks>
/// Before any splitter: a generator one bit wrong fails here by name, where a splitter would only return other folds
/// (decision 0008).
/// </remarks>
public sealed class NumpyRandomStateOracleTests
{
    [Fact]
    public void Every_draw_matches_numpy()
    {
        using JsonDocument corpus = OracleLoader.Load("numpy_random_state.json");
        int replayed = 0;

        foreach (JsonElement frozen in corpus.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = frozen.GetProperty("name").GetString()!;
            var generator = new NumpyRandomState(frozen.GetProperty("seed").GetInt64());
            JsonElement values = frozen.GetProperty("values");

            switch (frozen.GetProperty("call").GetString())
            {
                case "raw":
                    foreach (JsonElement expected in values.EnumerateArray())
                    {
                        Assert.True(expected.GetUInt32() == generator.NextUInt32(), $"{name}: raw output {replayed}");
                    }

                    break;
                case "permutation":
                    int[] counts = Integers(frozen.GetProperty("counts"));
                    JsonElement[] permutations = [.. values.EnumerateArray()];
                    for (int draw = 0; draw < counts.Length; draw++)
                    {
                        Assert.Equal(Integers(permutations[draw]), generator.Permutation(counts[draw]));
                    }

                    break;
                default:
                    int[] pool = Integers(frozen.GetProperty("pool"));
                    int[] sizes = Integers(frozen.GetProperty("counts"));
                    JsonElement[] choices = [.. values.EnumerateArray()];
                    for (int draw = 0; draw < sizes.Length; draw++)
                    {
                        Assert.Equal(Integers(choices[draw]), generator.Choice(pool, sizes[draw]));
                    }

                    break;
            }

            replayed++;
        }

        Assert.Equal(corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static int[] Integers(JsonElement element) => [.. element.EnumerateArray().Select(v => v.GetInt32())];
}
