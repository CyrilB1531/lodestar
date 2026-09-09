using System.Text;

namespace Lodestar.Text.Stemming;

// CA1308 (normalize to uppercase): Snowball is *defined* on lowercase input —
// the published algorithms, the reference implementations and the oracle corpora
// this suite is checked against all lowercase first. ToUpperInvariant would
// return different stems, which is a wrong answer rather than a differently-cased one.
#pragma warning disable CA1308

/// <summary>
/// The entry point and the two steps the Scandinavian algorithms state the same
/// way, shared by Swedish and Danish.
/// </summary>
/// <remarks>
/// What is shared is the *shape*, never a suffix table: each language keeps its
/// own vowels, its own s-endings and its own rules in its own file, so that file
/// still reads one-to-one against the published description. The Romance
/// algorithms are factored the same way — see <see cref="RomanceSnowballWorker"/>.
/// </remarks>
internal abstract class ScandinavianSnowballWorker : SnowballWorkerBase
{
    /// <summary>The bare <c>s</c> that ends group (b) of each language's first step.</summary>
    internal const string BareS = "s";

    /// <param name="word">The word, already lowercased and composed.</param>
    /// <param name="isVowel">That language's vowel set.</param>
    /// <remarks>The region before R1 must hold at least three letters, as in German.</remarks>
    protected ScandinavianSnowballWorker(string word, Func<char, bool> isVowel)
        : base(word, isVowel, minR1: 3)
    {
    }

    /// <summary>That language's steps, in the order its description states them.</summary>
    protected abstract void Run();

    /// <summary>Stems one word with the worker <paramref name="make"/> builds for it.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    /// <remarks>
    /// Composed (NFC) so an 'ä' or an 'æ' reaches the rules as one code point,
    /// whichever way the caller spelled it. A word of under two letters has no
    /// region to search and comes back lowercased and otherwise untouched.
    /// </remarks>
    internal static string Stem(string word, Func<string, ScandinavianSnowballWorker> make)
    {
        Guard.NotNull(word);
        string s = word.ToLowerInvariant().Normalize(NormalizationForm.FormC);
        if (s.Length < 2)
        {
            return s;
        }
        ScandinavianSnowballWorker worker = make(s);
        worker.Run();
        return worker.S;
    }

    /// <summary>Stems one word by running <paramref name="steps"/>, with no worker of its own.</summary>
    /// <param name="word">The word to stem.</param>
    /// <param name="isVowel">That language's vowel set.</param>
    /// <param name="steps">That language's steps, in the order its description states them.</param>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is null.</exception>
    /// <remarks>
    /// A nested worker is scaffolding repeated once per algorithm — declaration,
    /// vowel field, constructor and <c>Run</c> override, identical every time. A
    /// language whose steps are static methods takes this overload and adds none.
    /// </remarks>
    internal static string Stem(string word, Func<char, bool> isVowel, Action<ScandinavianSnowballWorker> steps) =>
        Stem(word, s => new StepWorker(s, isVowel, steps));

    /// <summary>The worker for a language that supplies its steps rather than a subclass.</summary>
    private sealed class StepWorker : ScandinavianSnowballWorker
    {
        private readonly Action<ScandinavianSnowballWorker> _steps;

        public StepWorker(string word, Func<char, bool> isVowel, Action<ScandinavianSnowballWorker> steps)
            : base(word, isVowel) => _steps = steps;

        protected override void Run() => _steps(this);
    }

    /// <summary>The word as the steps have left it.</summary>
    internal string Word => S;

    /// <summary>The longest of <paramref name="suffixes"/> lying in R1, or null.</summary>
    internal string? LongestInR1(string[] suffixes) => LongestSuffixInR1(suffixes);

    /// <summary>Removes the last <paramref name="count"/> characters.</summary>
    internal void Remove(int count) => Delete(count);

    /// <summary>Replaces a suffix of <paramref name="suffixLength"/> characters.</summary>
    internal void Rewrite(int suffixLength, string replacement) => Replace(suffixLength, replacement);

    /// <summary>Whether the letter at <paramref name="index"/> exists and is a vowel.</summary>
    internal bool IsVowelAt(int index) => index >= 0 && index < S.Length && IsVowel(S[index]);

    /// <summary>Deletes the longest of <paramref name="suffixes"/> that lies in R1.</summary>
    internal void StripInR1(string[] suffixes)
    {
        if (LongestSuffixInR1(suffixes) is { } hit)
        {
            Delete(hit.Length);
        }
    }

    /// <summary>A consonant pair in R1 loses its second letter, for a step supplied as a delegate.</summary>
    internal void StripPair(string[] pairs) => StripConsonantPair(pairs);

    /// <summary>
    /// The first step of both algorithms: delete the longest of
    /// <paramref name="suffixes"/> that lies in R1, except that a bare <c>s</c>
    /// needs a valid s-ending before it.
    /// </summary>
    /// <remarks>
    /// Group (b)'s bare s is searched alongside group (a) rather than after it, and
    /// the letter it tests is the one place in these rules that may sit outside R1.
    /// </remarks>
    protected void StripLongestInR1(string[] suffixes, Func<char, bool> isValidSEnding)
    {
        string? hit = LongestSuffixInR1(suffixes);
        if (hit is null)
        {
            return;
        }
        if (hit == BareS && !(S.Length >= 2 && isValidSEnding(S[S.Length - 2])))
        {
            return;
        }
        Delete(hit.Length);
    }

    /// <summary>A consonant pair in R1 loses its second letter: "friskt" ends at "frisk".</summary>
    protected void StripConsonantPair(string[] pairs)
    {
        if (LongestSuffixInR1(pairs) is not null)
        {
            Delete(1);
        }
    }
}
