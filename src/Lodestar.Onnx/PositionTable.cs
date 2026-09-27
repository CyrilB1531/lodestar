using System.Text;

namespace Lodestar.Onnx;

/// <summary>
/// Reads, from an ONNX file, how many positions its learned position-embedding table can index.
/// </summary>
/// <remarks>
/// ONNX Runtime exposes no initializer, so the protobuf is walked here, field by field, seeking
/// past every tensor payload. The table is the two-dimensional initializer a <c>Gather</c> reads
/// whose name holds <c>position_embeddings</c>, as Hugging Face's exports name it — all nine of
/// all-MiniLM-L6-v2's, quantized included, read 512. No such table, two, a fused embedding op or
/// a file this reader cannot follow answers <see langword="null"/>: nothing is truncated (#1214).
/// </remarks>
internal static class PositionTable
{
    private const string TableMarker = "position_embeddings";

    // Wire types, from the protobuf encoding specification.
    private const int Varint = 0;
    private const int Fixed64 = 1;
    private const int LengthDelimited = 2;
    private const int Fixed32 = 5;

    // A scalar is eight bytes at most; a payload longer than this is seeked past, never read.
    private const int ScalarBytes = 8;

    /// <summary>How deep the walk from a position index back to a <c>CumSum</c> may go.</summary>
    private const int AncestryBudget = 64;

    /// <summary>The longest name, op type or domain read; a graph's are a few dozen bytes.</summary>
    private const int MaxStringBytes = 1 << 16;

    private static readonly HashSet<string> PassThrough =
        new(StringComparer.Ordinal) { "Cast", "Identity", "Reshape", "Squeeze", "Unsqueeze" };

    /// <summary>The positions the table can index, net of a padding offset; null when unknown.</summary>
    /// <param name="modelPath">The <c>.onnx</c> file ONNX Runtime has already opened.</param>
    /// <param name="inputIdsName">The token-ids input, whose own lookup is never the position table.</param>
    public static int? UsableLength(string modelPath, string inputIdsName)
    {
        Graph graph;
        try
        {
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 16);
            graph = Graph.Read(new WireReader(stream));
        }
        catch (Exception exception) when (exception is IOException or InvalidDataException or UnauthorizedAccessException)
        {
            // ONNX Runtime accepted the file, so this reader, not the model, is what failed.
            return null;
        }
        return UsableLength(graph, inputIdsName);
    }

    private static int? UsableLength(Graph graph, string inputIdsName)
    {
        string? table = null;
        string? index = null;
        foreach (Node node in graph.Nodes)
        {
            if (node.OpType != "Gather" || node.Inputs.Length < 2 || node.Inputs[1] == inputIdsName
                || node.Inputs[0].IndexOf(TableMarker, StringComparison.OrdinalIgnoreCase) < 0
                || !graph.Initializers.TryGetValue(node.Inputs[0], out Tensor? candidate)
                || candidate.Dims.Length != 2)
            {
                continue;
            }
            if (table is not null && table != node.Inputs[0])
            {
                return null;
            }
            table = node.Inputs[0];
            index = node.Inputs[1];
        }
        if (table is null || index is null)
        {
            return null;
        }

        long rows = graph.Initializers[table].Dims[0];
        int? offset = PaddingOffset(graph, index);
        long usable = rows - (offset ?? 0);
        return offset is null || usable <= 0 || usable > int.MaxValue ? null : (int)usable;
    }

    /// <summary>
    /// Zero for positions counted from 0; <c>padding_idx + 1</c> for the RoBERTa family, whose
    /// positions are a <c>CumSum</c> of the mask plus <c>padding_idx</c>; null when a <c>CumSum</c>
    /// is there and the constant it is offset by cannot be read.
    /// </summary>
    private static int? PaddingOffset(Graph graph, string index)
    {
        if (!HasCumSumAncestor(graph, index))
        {
            return 0;
        }

        string current = index;
        for (int step = 0; step < AncestryBudget && graph.Producers.TryGetValue(current, out Node? node); step++)
        {
            if (PassThrough.Contains(node.OpType) && node.Inputs.Length > 0)
            {
                current = node.Inputs[0];
                continue;
            }
            if (node.OpType != "Add" || node.Inputs.Length != 2)
            {
                return null;
            }
            for (int side = 0; side < 2; side++)
            {
                if (ScalarOf(graph, node.Inputs[side]) is long padding && padding >= 0 && padding < int.MaxValue
                    && HasCumSumAncestor(graph, node.Inputs[1 - side]))
                {
                    return (int)padding + 1;
                }
            }
            return null;
        }
        return null;
    }

    private static bool HasCumSumAncestor(Graph graph, string name)
    {
        var pending = new Stack<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        pending.Push(name);
        while (pending.Count > 0 && seen.Count < AncestryBudget)
        {
            string current = pending.Pop();
            if (!seen.Add(current) || !graph.Producers.TryGetValue(current, out Node? node))
            {
                continue;
            }
            if (node.OpType == "CumSum")
            {
                return true;
            }
            foreach (string input in node.Inputs)
            {
                pending.Push(input);
            }
        }
        return false;
    }

    /// <summary>An integer scalar held by an initializer or a <c>Constant</c>, looked through casts.</summary>
    private static long? ScalarOf(Graph graph, string name)
    {
        for (int step = 0; step < AncestryBudget; step++)
        {
            if (graph.Initializers.TryGetValue(name, out Tensor? tensor))
            {
                return tensor.Scalar;
            }
            if (!graph.Producers.TryGetValue(name, out Node? node))
            {
                return null;
            }
            if (node.OpType == "Constant")
            {
                return node.Value?.Scalar;
            }
            if (!PassThrough.Contains(node.OpType) || node.Inputs.Length == 0)
            {
                return null;
            }
            name = node.Inputs[0];
        }
        return null;
    }

    /// <summary>The parts of a <c>GraphProto</c> the search reads.</summary>
    private sealed class Graph
    {
        public List<Node> Nodes { get; } = [];

        public Dictionary<string, Tensor> Initializers { get; } = new(StringComparer.Ordinal);

        public Dictionary<string, Node> Producers { get; } = new(StringComparer.Ordinal);

        /// <summary>ModelProto field 7 is the graph; GraphProto fields 1 and 5 its nodes and initializers.</summary>
        public static Graph Read(WireReader reader)
        {
            var graph = new Graph();
            long end = reader.Length;
            while (reader.Position < end)
            {
                (int field, int wire) = reader.ReadTag();
                if (field == 7 && wire == LengthDelimited)
                {
                    graph.ReadBody(reader, reader.ReadLength());
                }
                else
                {
                    reader.Skip(wire);
                }
            }
            return graph;
        }

        private void ReadBody(WireReader reader, long length)
        {
            long end = reader.Position + length;
            while (reader.Position < end)
            {
                (int field, int wire) = reader.ReadTag();
                if (field == 1 && wire == LengthDelimited)
                {
                    var node = Node.Read(reader, reader.ReadLength());
                    Nodes.Add(node);
                    foreach (string output in node.Outputs)
                    {
                        Producers[output] = node;
                    }
                }
                else if (field == 5 && wire == LengthDelimited)
                {
                    var tensor = Tensor.Read(reader, reader.ReadLength());
                    Initializers[tensor.Name] = tensor;
                }
                else
                {
                    reader.Skip(wire);
                }
            }
        }
    }

    /// <summary>A <c>NodeProto</c>: inputs 1, outputs 2, op_type 4, attributes 5, domain 7.</summary>
    private sealed class Node
    {
        public string OpType { get; private set; } = "";

        public string[] Inputs { get; private set; } = [];

        public string[] Outputs { get; private set; } = [];

        /// <summary>The <c>value</c> attribute, which is what a <c>Constant</c> holds.</summary>
        public Tensor? Value { get; private set; }

        public static Node Read(WireReader reader, long length)
        {
            var node = new Node();
            var inputs = new List<string>();
            var outputs = new List<string>();
            string domain = "";
            long end = reader.Position + length;
            while (reader.Position < end)
            {
                (int field, int wire) = reader.ReadTag();
                switch (field)
                {
                    case 1 when wire == LengthDelimited: inputs.Add(reader.ReadString()); break;
                    case 2 when wire == LengthDelimited: outputs.Add(reader.ReadString()); break;
                    case 4 when wire == LengthDelimited: node.OpType = reader.ReadString(); break;
                    case 5 when wire == LengthDelimited: node.ReadAttribute(reader, reader.ReadLength()); break;
                    case 7 when wire == LengthDelimited: domain = reader.ReadString(); break;
                    default: reader.Skip(wire); break;
                }
            }
            // An operator of a custom domain may share a name with a standard one and mean something else.
            if (domain.Length > 0 && domain != "ai.onnx")
            {
                node.OpType = domain + "." + node.OpType;
            }
            node.Inputs = [.. inputs];
            node.Outputs = [.. outputs];
            return node;
        }

        /// <summary>An <c>AttributeProto</c>: name 1, tensor 5.</summary>
        private void ReadAttribute(WireReader reader, long length)
        {
            string name = "";
            Tensor? tensor = null;
            long end = reader.Position + length;
            while (reader.Position < end)
            {
                (int field, int wire) = reader.ReadTag();
                if (field == 1 && wire == LengthDelimited)
                {
                    name = reader.ReadString();
                }
                else if (field == 5 && wire == LengthDelimited)
                {
                    tensor = Tensor.Read(reader, reader.ReadLength());
                }
                else
                {
                    reader.Skip(wire);
                }
            }
            if (name == "value")
            {
                Value = tensor;
            }
        }
    }

    /// <summary>A <c>TensorProto</c>: dims 1, data_type 2, int32_data 5, int64_data 7, name 8, raw_data 9.</summary>
    private sealed class Tensor
    {
        private const int Int32Type = 6;
        private const int Int64Type = 7;

        public string Name { get; private set; } = "";

        public long[] Dims { get; private set; } = [];

        /// <summary>The value of a one-element integer tensor; null for anything else.</summary>
        public long? Scalar { get; private set; }

        public static Tensor Read(WireReader reader, long length)
        {
            var tensor = new Tensor();
            var dims = new List<long>();
            int dataType = 0;
            long? typed = null;
            byte[]? raw = null;
            long end = reader.Position + length;
            while (reader.Position < end)
            {
                (int field, int wire) = reader.ReadTag();
                switch (field)
                {
                    case 1: reader.ReadVarints(wire, dims); break;
                    case 2 when wire == Varint: dataType = (int)reader.ReadVarint(); break;
                    case 5 or 7: typed = reader.ReadFirstVarint(wire); break;
                    case 8 when wire == LengthDelimited: tensor.Name = reader.ReadString(); break;
                    case 9 when wire == LengthDelimited: raw = reader.ReadSmallBytes(ScalarBytes); break;
                    default: reader.Skip(wire); break;
                }
            }
            tensor.Dims = [.. dims];
            if (dims.TrueForAll(d => d == 1) && dataType is Int32Type or Int64Type)
            {
                tensor.Scalar = raw is null ? Signed(typed, dataType) : FromRaw(raw, dataType);
            }
            return tensor;
        }

        private static long? Signed(long? value, int dataType) =>
            value is long v && dataType == Int32Type ? (int)v : value;

        private static long? FromRaw(byte[] raw, int dataType) => (dataType, raw.Length) switch
        {
            // ONNX stores raw_data little-endian whatever the host.
            (Int64Type, 8) => (long)ReadLittleEndian(raw, 8),
            (Int32Type, 4) => (int)(uint)ReadLittleEndian(raw, 4),
            _ => null,
        };

        private static ulong ReadLittleEndian(byte[] raw, int count)
        {
            ulong value = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                value = (value << 8) | raw[i];
            }
            return value;
        }
    }

    /// <summary>The protobuf wire format over a seekable stream, reading nothing it can seek past.</summary>
    private sealed class WireReader(Stream stream)
    {
        public long Position => stream.Position;

        public long Length => stream.Length;

        public (int Field, int Wire) ReadTag()
        {
            ulong tag = ReadVarint();
            return ((int)(tag >> 3), (int)(tag & 7));
        }

        public ulong ReadVarint()
        {
            ulong value = 0;
            for (int shift = 0; shift < 64; shift += 7)
            {
                int b = stream.ReadByte();
                if (b < 0)
                {
                    throw new InvalidDataException("The file ends inside a varint.");
                }
                value |= (ulong)(b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                {
                    return value;
                }
            }
            throw new InvalidDataException("A varint runs past ten bytes.");
        }

        public long ReadLength()
        {
            ulong length = ReadVarint();
            if (length > (ulong)(stream.Length - stream.Position))
            {
                throw new InvalidDataException("A length-delimited field runs past the end of the file.");
            }
            return (long)length;
        }

        public string ReadString()
        {
            // A name or an op type is short; a longer one is a file this reader misparses,
            // and allocating it could fail with something no catch above expects.
            long length = ReadLength();
            if (length > MaxStringBytes)
            {
                throw new InvalidDataException("A string field is longer than any name a graph holds.");
            }
            return Encoding.UTF8.GetString(ReadExactly(length));
        }

        /// <summary>The field's bytes when it holds at most <paramref name="limit"/>; else it is skipped.</summary>
        public byte[]? ReadSmallBytes(int limit)
        {
            long length = ReadLength();
            if (length <= limit)
            {
                return ReadExactly(length);
            }
            stream.Seek(length, SeekOrigin.Current);
            return null;
        }

        /// <summary>Appends a repeated integer field, packed or not.</summary>
        public void ReadVarints(int wire, List<long> into)
        {
            if (wire == Varint)
            {
                into.Add((long)ReadVarint());
                return;
            }
            if (wire != LengthDelimited)
            {
                Skip(wire);
                return;
            }
            long length = ReadLength();
            long end = stream.Position + length;
            while (stream.Position < end)
            {
                into.Add((long)ReadVarint());
            }
        }

        /// <summary>The first element of a repeated integer field; a longer packed run is not a scalar.</summary>
        public long? ReadFirstVarint(int wire)
        {
            var values = new List<long>();
            if (wire == LengthDelimited && PeekLength() > ScalarBytes * 2)
            {
                Skip(wire);
                return null;
            }
            ReadVarints(wire, values);
            return values.Count == 1 ? values[0] : null;
        }

        public void Skip(int wire)
        {
            switch (wire)
            {
                case Varint: ReadVarint(); break;
                case Fixed64: stream.Seek(8, SeekOrigin.Current); break;
                case LengthDelimited: stream.Seek(ReadLength(), SeekOrigin.Current); break;
                case Fixed32: stream.Seek(4, SeekOrigin.Current); break;
                default: throw new InvalidDataException($"Wire type {wire} is not one ONNX writes.");
            }
        }

        private long PeekLength()
        {
            long start = stream.Position;
            long length = ReadLength();
            stream.Seek(start, SeekOrigin.Begin);
            return length;
        }

        private byte[] ReadExactly(long length)
        {
            var buffer = new byte[length];
            int read = 0;
            while (read < buffer.Length)
            {
                int n = stream.Read(buffer, read, buffer.Length - read);
                if (n == 0)
                {
                    throw new InvalidDataException("The file ends inside a field.");
                }
                read += n;
            }
            return buffer;
        }
    }
}
