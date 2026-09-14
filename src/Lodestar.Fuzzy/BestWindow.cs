namespace Lodestar.Fuzzy;

/// <summary>The best window so far, held as its LCS and its combined length.</summary>
/// <remarks>
/// Compared as the fraction <c>2·lcs/total</c> by cross-multiplying, and turned into a double once,
/// by the expression <c>Indel.NormalizedSimilarity</c> evaluates. That expression is monotone in
/// the fraction, so the maximum it returns is the one scoring every window would have found. The
/// products are taken in <see cref="long"/>: a needle past 46,340 characters overflows an <see cref="int"/>.
/// </remarks>
internal struct BestWindow
{
    private int _lcs;
    private int _total;

    public void Offer(int lcs, int total)
    {
        if (_total == 0 || (long)lcs * _total > (long)_lcs * total)
        {
            _lcs = lcs;
            _total = total;
        }
    }

    /// <summary>Whether the best is a whole needle matched inside a full window, which nothing beats.</summary>
    public readonly bool IsExact => _total != 0 && 2 * _lcs == _total;

    public readonly bool CouldImprove(int lcsCeiling, int total) =>
        _total == 0 || (long)lcsCeiling * _total > (long)_lcs * total;

    public readonly double Ratio() =>
        _lcs == 0 ? 0.0 : 100.0 * (1.0 - ((double)(_total - (2 * _lcs)) / _total));
}
