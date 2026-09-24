namespace Lodestar.Embeddings.Tokenization;

// CA1008: these values mirror SentencePiece's own ModelProto.SentencePiece.Type
// numbering (from 1, below); a synthetic None = 0 would carry no model file's value.
#pragma warning disable CA1008

/// <summary>
/// What a piece is for, as recorded in the <c>spiece.model</c> proto.
/// </summary>
/// <remarks>
/// The numeric values are the ones in <c>sentencepiece_model.proto</c>
/// (<c>ModelProto.SentencePiece.Type</c>) and are part of the file format, not an
/// internal detail. <c>Control</c> and <c>Unknown</c> pieces must never match
/// real text: <c>&lt;s&gt;</c> is a marker the model emits, not a string a user
/// can type.
/// </remarks>
public enum SentencePieceType
{
    /// <summary>An ordinary piece, matched against text.</summary>
    Normal = 1,

    /// <summary>The unknown piece — surfaced only for characters nothing covers.</summary>
    Unknown = 2,

    /// <summary>A control marker such as <c>&lt;s&gt;</c> or <c>&lt;/s&gt;</c>; never matched against text.</summary>
    Control = 3,

    /// <summary>A user-defined piece; matched against text like a normal one.</summary>
    UserDefined = 4,

    /// <summary>A piece dropped during training.</summary>
    Unused = 5,

    /// <summary>A single byte, used by byte-fallback models.</summary>
    Byte = 6,
}
#pragma warning restore CA1008
