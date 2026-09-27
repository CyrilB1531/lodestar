namespace Lodestar.Text.Stemming;

/// <summary>Which of the Porter algorithm's two published forms <c>PorterStemmer</c> applies.</summary>
public enum PorterStemmerMode
{
    /// <summary>
    /// The 1980 paper's rules as printed: <c>abli</c> becomes <c>able</c>, <c>logi</c> is kept, and
    /// words of one or two letters are stemmed like any other. nltk's <c>ORIGINAL_ALGORITHM</c>.
    /// </summary>
    OriginalAlgorithm,

    /// <summary>
    /// The departures Martin Porter's own reference implementation makes from the paper: <c>bli</c>
    /// becomes <c>ble</c>, <c>logi</c> becomes <c>log</c>, and words of one or two letters come back
    /// lowercased and unstemmed. nltk's <c>MARTIN_EXTENSIONS</c>.
    /// </summary>
    MartinExtensions,
}
