using System.Buffers;
using System.Text;
using System.Text.Json;
using Lodestar.Internal.Persistence;
using Xunit;

namespace Lodestar.Text.Tests.Persistence;

/// <summary>The escaping that lets a lone surrogate through <c>Save</c> and <c>Load</c> (#1263).</summary>
public sealed class JsonTextEscapingTests
{
    private const string Bs = "\\";

    [Fact]
    public void Every_string_of_units_reads_back_as_written()
    {
        // Lone halves of both kinds, pairs, quotes, backslashes and controls, in every order.
        char[] pool = ['a', '"', '\\', '\n', (char)0x01, (char)0xE9, (char)0xD800, (char)0xDBFF, (char)0xDC00, (char)0xDFFF, (char)0xFFFD];
#pragma warning disable CA5394, S2245 // seeded, so a failure reproduces; nothing here is a secret
        var rng = new Random(1263);
        for (int k = 0; k < 5_000; k++)
        {
            var sb = new StringBuilder();
            for (int n = rng.Next(8); n > 0; n--)
            {
                sb.Append(pool[rng.Next(pool.Length)]);
            }
#pragma warning restore CA5394, S2245
            string value = sb.ToString();

            Assert.Equal(value, RoundTrip(value));
        }
    }

    [Fact]
    public void Text_without_a_lone_surrogate_is_written_as_the_writer_writes_it()
    {
        string value = "caf" + (char)0xE9 + " " + char.ConvertFromUtf32(0x1F600) + " " + (char)0x2028 + Bs;

        Assert.Equal(Write(w => w.WriteStringValue(value)), Write(w => JsonArtifact.WriteText(w, value)));
    }

    [Fact]
    public void The_text_around_a_lone_surrogate_is_escaped_as_the_writer_escapes_it()
    {
        string around = char.ConvertFromUtf32(0x1F600) + (char)0x2028;
        string lone = Write(w => JsonArtifact.WriteText(w, around + (char)0xD800 + around));
        string plain = Write(w => w.WriteStringValue(around));
        string inner = plain.Substring(2, plain.Length - 4);

        Assert.Equal("[\"" + inner + Bs + "uD800" + inner + "\"]", lone);
    }

    [Fact]
    public void An_escaped_backslash_before_u_d800_is_text_not_an_escape()
    {
        Assert.Equal(Bs + "ud800", Read("\"" + Bs + Bs + "ud800\""));
    }

    [Fact]
    public void Invalid_utf8_beside_a_lone_escape_raises_the_readers_exception()
    {
        byte[] json = [(byte)'"', 0xFF, 0xFE, (byte)'\\', (byte)'u', (byte)'d', (byte)'8', (byte)'0', (byte)'0', (byte)'"'];

        Assert.Throws<InvalidOperationException>(() => Read(json));
    }

    [Fact]
    public void A_token_split_across_segments_reads_its_lone_surrogate()
    {
        byte[] json = Encoding.UTF8.GetBytes("\"x" + Bs + "ud800y" + Bs + "ud83d" + Bs + "ude00\"");
        for (int cut = 1; cut < json.Length; cut++)
        {
            var first = new Segment(json.AsMemory(0, cut));
            Segment last = first.Append(json.AsMemory(cut));
            var reader = new Utf8JsonReader(new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length));
            Assert.True(reader.Read());

            Assert.Equal("x" + (char)0xD800 + "y" + char.ConvertFromUtf32(0x1F600), JsonArtifact.GetText(ref reader));
        }
    }

    private static string RoundTrip(string value)
    {
        string json = Write(w => JsonArtifact.WriteText(w, value));
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        Assert.True(reader.Read() && reader.Read());
        return JsonArtifact.GetText(ref reader);
    }

    private static string Read(string json) => Read(Encoding.UTF8.GetBytes(json));

    private static string Read(byte[] json)
    {
        var reader = new Utf8JsonReader(json);
        Assert.True(reader.Read());
        return JsonArtifact.GetText(ref reader);
    }

    private static string Write(Action<Utf8JsonWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, JsonArtifact.WriterOptions))
        {
            writer.WriteStartArray();
            write(writer);
            writer.WriteEndArray();
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public Segment(ReadOnlyMemory<byte> memory) => Memory = memory;

        public Segment Append(ReadOnlyMemory<byte> memory)
        {
            var next = new Segment(memory) { RunningIndex = RunningIndex + Memory.Length };
            Next = next;
            return next;
        }
    }
}
