namespace Lodestar.Preprocessing;

/// <summary>Which category <c>OneHotEncoder{T}</c> drops, and what it does with an unseen value.</summary>
/// <remarks>
/// <c>drop</c> and <c>handle_unknown</c>, same defaults. <strong>The two interact</strong>: an
/// ignored unknown encodes to all zeros, and so does a dropped first category, so a row of zeros
/// means either. The reference accepts that collision and this does too.
/// </remarks>
public sealed record OneHotEncoderOptions
{
    /// <summary>Which category loses its column, if any.</summary>
    public CategoryDrop Drop { get; init; }

    /// <summary>What to do with a value the fit never saw.</summary>
    public UnknownCategory Unknown { get; init; }

    /// <summary>
    /// A category seen fewer times than this is infrequent, and shares one column with the others — the reference's
    /// <c>min_frequency</c> as an integer; <see langword="null"/>, the default, groups none.
    /// </summary>
    public int? MinFrequency { get; init; }

    /// <summary>
    /// A category seen in fewer than this share of the rows is infrequent — <c>min_frequency</c> as a float in
    /// <c>(0, 1)</c>. It and <see cref="MinFrequency"/> are refused together.
    /// </summary>
    public double? MinFrequencyShare { get; init; }

    /// <summary>
    /// At most this many columns per feature, the infrequent one included: the least frequent categories beyond it
    /// are grouped — the reference's <c>max_categories</c>; <see langword="null"/>, the default, sets no limit.
    /// </summary>
    public int? MaxCategories { get; init; }
}
