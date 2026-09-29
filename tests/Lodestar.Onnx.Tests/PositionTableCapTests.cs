using System.Text;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>
/// The position-table reader stops a repeated field at 64 values, packed or not (#1526): a graph whose table reads six
/// positions reads as unknown once one initializer declares 65 dimensions.
/// </summary>
public sealed class PositionTableCapTests
{
    private const string Table = "embeddings.position_embeddings.weight";

    [Fact]
    public void A_graph_within_the_cap_reads_its_table() =>
        Assert.Equal(6, UsableLength(dims: null, packed: false));

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void An_initializer_past_64_dimensions_reads_as_unknown(bool packed) =>
        Assert.Null(UsableLength(dims: 65, packed));

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Sixty_four_dimensions_are_still_read(bool packed) =>
        Assert.Equal(6, UsableLength(dims: 64, packed));

    private static int? UsableLength(int? dims, bool packed)
    {
        var graph = new List<byte>();
        graph.AddRange(Field(1, Node()));
        graph.AddRange(Field(5, Tensor(Table, [6, 4], packed: true)));
        if (dims is int count)
        {
            graph.AddRange(Field(5, Tensor("wide", [.. Enumerable.Repeat(1L, count)], packed)));
        }

        string path = Path.Combine(Path.GetTempPath(), $"position-cap-{Guid.NewGuid():N}.onnx");
        File.WriteAllBytes(path, [.. Field(7, [.. graph])]);
        try
        {
            return PositionTable.UsableLength(path, "input_ids");
        }
        finally
        {
            File.Delete(path);
        }
    }

    /// <summary>A <c>Gather</c> reading the table at some position ids, which is all the reader looks for.</summary>
    private static byte[] Node() =>
        [.. Text(1, Table), .. Text(1, "position_ids"), .. Text(2, "positions"), .. Text(4, "Gather")];

    private static byte[] Tensor(string name, long[] dims, bool packed)
    {
        var tensor = new List<byte>();
        if (packed)
        {
            tensor.AddRange(Field(1, [.. dims.SelectMany(Varint)]));
        }
        else
        {
            foreach (long dim in dims)
            {
                tensor.AddRange([(1 << 3) | 0, .. Varint(dim)]);
            }
        }

        tensor.AddRange(Text(8, name));
        return [.. tensor];
    }

    private static byte[] Text(int field, string value) => Field(field, Encoding.UTF8.GetBytes(value));

    private static byte[] Field(int field, byte[] payload) =>
        [(byte)((field << 3) | 2), .. Varint(payload.Length), .. payload];

    private static byte[] Varint(long value)
    {
        var bytes = new List<byte>();
        ulong rest = (ulong)value;
        do
        {
            byte low = (byte)(rest & 0x7F);
            rest >>= 7;
            bytes.Add(rest == 0 ? low : (byte)(low | 0x80));
        }
        while (rest != 0);
        return [.. bytes];
    }
}
