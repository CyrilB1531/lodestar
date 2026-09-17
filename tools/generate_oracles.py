#!/usr/bin/env python3
"""Generate frozen oracle corpora for the Lodestar.Text test suite.

Per section 4 of the project brief, correctness is proven by replaying reference
values captured from the canonical Python libraries — never by trusting that the
C# "compiles and passes". This script produces those references as versioned
JSON under ``tests/oracles/``. Python is thus a *development* dependency only;
the committed JSON is what the C# suite consumes at test time.

Design rules:
  * Deterministic. A fixed seed and no wall-clock timestamps, so regenerating on
    another machine yields a byte-identical file (clean diffs, real review).
  * Code-point semantics. rapidfuzz operates on Python ``str`` (code points), so
    the C# side must compare with ``TextElement.CodePoint`` to match. Lone
    surrogates are never emitted (they cannot round-trip through JSON).
  * Broad coverage. Empty / identical / ASCII typos / accents / BMP mix / CJK /
    supplementary-plane emoji / long strings.

Usage: CONTRIBUTING.md's *Oracle validation* has the virtualenv's creation step,
the interpreter it needs, and the neutral working directory this must run from.
"""

from __future__ import annotations

import base64
import contextlib
import json
import math
import os
import sys
import tempfile
import warnings
from collections.abc import Callable
from importlib.metadata import version
from pathlib import Path

# PYTHONSAFEPATH=1 (CONTRIBUTING.md) keeps this script's own directory off
# sys.path, so seeded_random is appended -- never prepended, so nothing here shadows an installed package.
sys.path.append(str(Path(__file__).resolve().parent))

from python_floor import require_supported_python  # noqa: E402

# Before the seeded_random import, never after: that module is PEP 695, so an
# interpreter below the floor fails parsing it first (issue #486).
require_supported_python("tools/generate_oracles.py")

from seeded_random import SeededRandom  # noqa: E402

from difflib import SequenceMatcher

import jellyfish
import numpy as np
import textdistance as td
from doublemetaphone import doublemetaphone
from rapidfuzz.distance import DamerauLevenshtein, Indel, Levenshtein, OSA
from sklearn import metrics as skm
from sklearn.feature_extraction.text import CountVectorizer as SkCountVectorizer
from sklearn.feature_extraction.text import TfidfVectorizer as SkTfidfVectorizer

SEED = 20260801
ORACLE_DIR = Path(__file__).resolve().parent.parent / "tests" / "oracles"

# The metadata key every numeric corpus carries, named once because S1192 counts a
# JSON key like any other literal and the decomposition corpora made it a fourth.
TOLERANCE_KEY = "tolerance"
# The second MinHash permutation family, named where datasketch names it and where the
# corpus block that freezes it does -- three spellings of one string past S1192 (#645).
AFFINE32 = "affine32"
# Which call produced a block, beside the library version that produced it.
VARIANT = "variant"

# Fixture strings reused across several corpora.
QUICK_FOX = "the quick brown fox"
METS = "new york mets"
UNK_TOKEN = "[UNK]"
# WordPiece's spelling; the sentencepiece-based families (Unigram, XLM-R) spell
# the same concept in angle brackets below.
UNK_TOKEN_LOWER = "<unk>"
# The sequence marker the sentencepiece families declare, named once for the same
# reason THE_CAT below is.
BOS_TOKEN = "<s>"
# Two spellings of one fixture phrase. THE_CAT is separate because four corpora
# reach for it and Sonar's S1192 counts them together (issue #487's quality gate).
THE_CAT = "the cat"
# The accented word three corpora reach for: the two transliteration pairs and the
# Double Metaphone input contract, which is what took it to S1192's threshold (#177).
NAIVE = "naïve"
# "the house", definite and genitive, which all three Scandinavian corpora reach
# for -- and that is what takes both spellings to S1192's threshold (#308).
HUSET = "huset"
HUSETS = "husets"
# The conformal corpus's own key names. S1192 counts a JSON key like any other
# literal, and these are written once per case in three places each (#441).
CALIB_SIZE = "calib"
# The normalised score's two extra columns, named for the same reason (#683).
CALIB_SIGMA = "calibResidualEstimates"
TEST_SIGMA = "testResidualEstimates"
# The two classification rules' keys, named for the same reason (#866).
CEILING_QUANTILE = "ceiling_quantile"
BETWEEN = "between"
# The sparse-dense corpus's fixture keys, named for the same reason the conformal
# ones above are: S1192 counts a dict key like any other literal (#440).
COLUMNS = "columns"

# The three keys every StandardScaler case carries (#568). Named here rather than
# repeated, which is what S1192 asks once a literal reaches three uses in a file.
FEATURE_COUNT = "feature_count"
SAMPLES = "samples"
MAX_ITER = "max_iter"
T_PPF = "t.ppf"
CHI2_SF = "chi2.sf"
# A bound key three corpora write, which is what takes it to S1192's threshold (#569).
UPPER = "upper"
LOWER = "lower"
NORM_PPF = "norm.ppf"
FAMILY = "family"
# The OLS corpus repeats its own field names once per fixture and once per emitted case.
CONFIDENCE_LEVEL = "confidenceLevel"
COVARIANCE_TYPE = "covarianceType"
# The HAC and cluster keys of the linear corpora, echoed into each case they configure (#775).
HAC_LAGS = "hacLags"
USE_CORRECTION = "useCorrection"
# statsmodels' cov_type string for one-way clusters, used by the OLS and WLS fixtures (#775).
CLUSTER = "cluster"
DESIGN = "design"
OLS_FEATURE_COUNT = "featureCount"
RESPONSE = "response"
WITH_INTERCEPT = "withIntercept"
WEIGHTS = "weights"
ERROR_COVARIANCE = "covariance"
# The inference table's own field names, shared by the OLS, GLM and Cox corpora (#684).
COEFFICIENTS = "coefficients"
STANDARD_ERRORS = "standardErrors"
P_VALUES = "pValues"
CONFIDENCE_LOWER = "confidenceLower"
CONFIDENCE_UPPER = "confidenceUpper"
# scikit-learn's metric name and lifelines' stopping option, spelled alike.
PRECISION = "precision"
# Metric names several corpora write as keys, named once they reached S1192 (#861).
RECALL = "recall"
JACCARD = "jaccard"
# The GLM corpus beside the OLS one above, past the same threshold (#616): a family
# literal, the library name, and "iterations", which NMF and k-means already write.
STATSMODELS = "statsmodels"
BINOMIAL = "binomial"
POISSON = "poisson"
ITERATIONS = "iterations"
NEGATIVE_BINOMIAL = "negativeBinomial"
ALPHA = "alpha"
GAMMA = "gamma"
INVERSE = "inverse"
LINK = "link"
# The per-row terms a GLM predictor carries with no coefficient, echoed into each case (#787).
# Fields several corpora report, each named once (#788): S1192 counts a JSON key like any other literal.
CONVERGED = "converged"
LOG_LIKELIHOOD = "logLikelihood"
AKAIKE = "akaike"
RESIDUAL_DEGREES_OF_FREEDOM = "residualDegreesOfFreedom"
Z_STATISTICS = "zStatistics"
OFFSET = "offset"
EXPOSURE = "exposure"
WITH_MEAN = "with_mean"
WITH_STD = "with_std"
DENSE = "dense"
WIDTH = "width"
QUANTILE = "quantile"
Y_CALIB = "y_calib"
Y_CALIB_PRED = "y_calib_pred"
Y_TEST_PRED = "y_test_pred"
CAT_SENTENCE = THE_CAT + " sat on the mat"
HELLO_WORLD = "hello world"
END_OF_TEXT = "<|endoftext|>"
TINY_SP_MODEL = "tiny_sp.model"
EMBEDDING_SENTENCE = "tokenization is embedding embeddings"
XLMR_FAIRSEQ_MODEL = "xlmr_fairseq.model"
# XLM-R's mask marker, and the added token issue #104 was opened for: roberta-base
# declares lstrip on this one.
MASK_TOKEN = "<mask>"

# Named rather than repeated per corpus: three metadata blocks carry it, which is
# python:S1192's threshold -- confirmed CRITICAL by the analyzer at that count.
BYTE_LEVEL_NO_MERGES = (
    "hand-built: the byte-level alphabet with no merges, defined in tools/generate_oracles.py")

# Ordered: whitespace first (" a" vs "a " must stay distinct), then multi-byte
# scripts, then the special-token strings written out literally.
BPE_TEXTS = [
    "",
    " ",
    "   ",
    "Hello, world!",
    " leading space",
    "trailing space ",
    "double  space",
    "a\tb\nc\r\nd",
    "Il était une fois, à Paris — déjà vu.",
    "naïve café résumé",
    "東京都から来ました",
    "中文分词测试",
    "emoji 👋🏽 family 👨‍👩‍👧‍👦 flag 🇫🇷",
    "<|endoftext|> is written here literally",
    "[UNK] [CLS] [SEP] as text",
    "123 4567 89.01 -42",
    "https://example.com/path?q=1&r=2",
    "snake_case camelCase kebab-case SCREAMING_CASE",
    "the quick brown fox jumps over the lazy dog",
    "tokenization is embedding embeddings",
]

# Code-point ranges per category. Surrogates (0xD800..0xDFFF) are filtered out.
RANGES = {
    "ascii": [(0x20, 0x7E)],
    "latin": [(0x20, 0x7E), (0xC0, 0x17F)],
    "bmp": [(0x20, 0x7E), (0xC0, 0x2AF), (0x370, 0x52F), (0x4E00, 0x9FFF)],
    "supplementary": [(0x1F300, 0x1FAFF), (0x10000, 0x1052F)],
}


# Not the full float64 repr; see stable()'s docstring for why twelve.
STABLE_DIGITS = 12


def stable(value) -> float:
    """A float the corpus can commit: rounded away from the host's last bits.

    numpy and scikit-learn sum in whatever order the SIMD kernel scipy-openblas
    selects for the host CPU, so the last bits of anything a BLAS kernel reduced
    describe the machine that ran the generator rather than the metric --
    committing them turns the drift gate into a hardware check (issue #97).

    Significant digits, not decimal places, because the spread is always at the
    last bit and scales with the value: measured ~1e-13 on accuracy_count (~413)
    and ~1e-16 on the knn scores (~0.4), the same sixteenth digit in both. Twelve
    leaves four orders of margin above it, costing at most 5e-13 against the
    tolerances the tests compare with -- 1e-9 for the metrics corpus, 1e-4f for
    the knn one.
    """
    return float(f"{float(value):.{STABLE_DIGITS}g}")


# Below this a value is cancellation residue rather than a quantity: three orders under
# the 1e-9 the suites compare at, its last bits set by the host's reduction order (see stable).
NEGLIGIBLE = 1e-12


def settled(value) -> float:
    """stable(), with anything the comparison cannot distinguish from zero flushed to it."""
    number = float(value)
    return 0.0 if abs(number) < NEGLIGIBLE else stable(number)


def rand_string(rng: SeededRandom, length: int, ranges) -> str:
    out = []
    for _ in range(length):
        lo, hi = rng.choice(ranges)
        cp = rng.randint(lo, hi)
        while 0xD800 <= cp <= 0xDFFF:
            cp = rng.randint(lo, hi)
        out.append(chr(cp))
    return "".join(out)


def mutate(rng: SeededRandom, s: str, edits: int, ranges) -> str:
    """Apply `edits` random insert/delete/substitute operations to `s`."""
    chars = list(s)
    for _ in range(edits):
        op = rng.choice(("ins", "del", "sub")) if chars else "ins"
        lo, hi = rng.choice(ranges)
        cp = rng.randint(lo, hi)
        while 0xD800 <= cp <= 0xDFFF:
            cp = rng.randint(lo, hi)
        if op == "ins":
            chars.insert(rng.randint(0, len(chars)), chr(cp))
        elif op == "del":
            del chars[rng.randint(0, len(chars) - 1)]
        else:
            chars[rng.randint(0, len(chars) - 1)] = chr(cp)
    return "".join(chars)


def build_pairs(rng: SeededRandom):
    """Yield (category, a, b) tuples covering the corpus design.

    long_ascii/long_latin/long_supplementary are appended last so every existing
    case keeps its id and value -- the RNG is consumed in order.

    The first two exist because "long" draws from BMP ranges, so its patterns
    contain CJK and never reach the Latin-1 bit-parallel path: the blocked Myers
    code had no coverage at all until long ASCII/Latin pairs were added.

    long_supplementary is the same hole one plane up (#208). "supplementary"
    draws 2-10 characters, and the fast path opens at 16, so measured over the
    1425 cases before it: 283 reached the Myers gate, 194 of those held a
    character above U+00FF, and *none* held a supplementary one. Surrogate
    decoding is what the code-point mode is for, and it was the one part of that
    path with no case long enough to exercise it.
    """
    # Deterministic edge cases first.
    edge = [
        ("", ""),
        ("a", ""),
        ("", "a"),
        ("abc", "abc"),
        ("kitten", "sitting"),
        ("flaw", "lawn"),
        ("Levenshtein", "Levenstein"),
        ("café", "cafe"),
        ("Straße", "Strasse"),
        (NAIVE, "naive"),
        ("😀", "😀"),
        ("😀", "😁"),
        ("a😀b", "ab"),
        ("👨‍👩‍👧", "👨‍👩‍👦"),  # ZWJ sequences: differ by code points
        ("中文测试", "中文考试"),
        # Transposition-focused: exercise OSA vs unrestricted Damerau-Levenshtein.
        ("ab", "ba"),
        ("abcd", "acbd"),
        ("CA", "ABC"),   # OSA gives 3, unrestricted Damerau gives 2
        ("ca", "abc"),
        ("a cat", "an act"),
        ("converse", "conserve"),
    ]
    for a, b in edge:
        yield "edge", a, b

    # Randomized families.
    plans = [
        ("ascii", RANGES["ascii"], 200, (3, 12), (0, 5)),
        ("latin", RANGES["latin"], 200, (3, 14), (0, 6)),
        ("bmp", RANGES["bmp"], 200, (3, 16), (0, 7)),
        ("supplementary", RANGES["supplementary"], 150, (2, 10), (0, 6)),
        ("long", RANGES["bmp"], 60, (120, 400), (5, 40)),
        # See this function's docstring for why these three are appended last.
        ("long_ascii", RANGES["ascii"], 60, (80, 400), (5, 40)),
        ("long_latin", RANGES["latin"], 60, (80, 400), (5, 40)),
        # 20, not the 80 its neighbours use: at 80 every case cleared the 64-code-point
        # word and the single-word kernel got none. Measured on the first attempt.
        ("long_supplementary", RANGES["supplementary"], 60, (20, 400), (5, 40)),
    ]
    for name, ranges, count, (lo_len, hi_len), (lo_edit, hi_edit) in plans:
        for _ in range(count):
            length = rng.randint(lo_len, hi_len)
            a = rand_string(rng, length, ranges)
            b = mutate(rng, a, rng.randint(lo_edit, hi_edit), ranges)
            # Half the time, compare against a fully independent string too.
            yield name, a, b
            if rng.random() < 0.5:
                yield name, a, rand_string(rng, rng.randint(lo_len, hi_len), ranges)


def generate_levenshtein() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        cases.append(
            {
                "id": idx,
                "category": category,
                "a": a,
                "b": b,
                "distance": Levenshtein.distance(a, b),
                "normalized_distance": Levenshtein.normalized_distance(a, b),
                "normalized_similarity": Levenshtein.normalized_similarity(a, b),
            }
        )
    return {
        "metadata": {
            "algorithm": "Levenshtein",
            "library": "rapidfuzz",
            "library_version": version("rapidfuzz"),
            "reference_calls": [
                "rapidfuzz.distance.Levenshtein.distance",
                "rapidfuzz.distance.Levenshtein.normalized_distance",
                "rapidfuzz.distance.Levenshtein.normalized_similarity",
            ],
            "weights": [1, 1, 1],
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def _edit_distance_corpus(module, algorithm: str, library: str, calls: list[str]) -> dict:
    """Build an oracle for any rapidfuzz edit-distance module (Levenshtein/OSA/DL)."""
    rng = SeededRandom(SEED)
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        cases.append(
            {
                "id": idx,
                "category": category,
                "a": a,
                "b": b,
                "distance": module.distance(a, b),
                "normalized_distance": module.normalized_distance(a, b),
                "normalized_similarity": module.normalized_similarity(a, b),
            }
        )
    return {
        "metadata": {
            "algorithm": algorithm,
            "library": library,
            "library_version": version(library),
            "reference_calls": calls,
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_osa() -> dict:
    return _edit_distance_corpus(
        OSA, "OSA", "rapidfuzz",
        ["rapidfuzz.distance.OSA.distance",
         "rapidfuzz.distance.OSA.normalized_distance",
         "rapidfuzz.distance.OSA.normalized_similarity"],
    )


def generate_damerau() -> dict:
    return _edit_distance_corpus(
        DamerauLevenshtein, "DamerauLevenshtein", "rapidfuzz",
        ["rapidfuzz.distance.DamerauLevenshtein.distance",
         "rapidfuzz.distance.DamerauLevenshtein.normalized_distance",
         "rapidfuzz.distance.DamerauLevenshtein.normalized_similarity"],
    )


def _hamming_reference(a: str, b: str) -> int:
    """Standard Hamming distance over code points: positional mismatches over the
    common prefix, plus the length difference.

    Note: jellyfish.hamming_distance matches this for all normal inputs but
    diverges on ~5% of degenerate combining-mark strings (an unexplained quirk of
    its Rust core — not NFC normalization, not byte-level). Lodestar implements the
    standard definition; see docs/decisions/0005-hamming-jellyfish-divergence.md.
    """
    m = min(len(a), len(b))
    return sum(1 for i in range(m) if a[i] != b[i]) + abs(len(a) - len(b))


def generate_hamming() -> dict:
    rng = SeededRandom(SEED)
    diverge = 0
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        ref = _hamming_reference(a, b)
        if jellyfish.hamming_distance(a, b) != ref:
            diverge += 1
        cases.append({"id": idx, "category": category, "a": a, "b": b, "distance": ref})
    return {
        "metadata": {
            "algorithm": "Hamming",
            "library": "reference-standard",
            "reference_calls": ["standard code-point Hamming (see decision 0005)"],
            "jellyfish_version": version("jellyfish"),
            "jellyfish_divergences": diverge,
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_indel() -> dict:
    return _edit_distance_corpus(
        Indel, "Indel", "rapidfuzz",
        ["rapidfuzz.distance.Indel.distance",
         "rapidfuzz.distance.Indel.normalized_distance",
         "rapidfuzz.distance.Indel.normalized_similarity"],
    )


def _jaro_reference(a: str, b: str) -> float:  # NOSONAR S3776
    """Standard Jaro similarity over code points (matches Lodestar's Jaro core).

    Cognitive complexity is deliberately left above the threshold. This is a
    transcription of the published Jaro algorithm — match window, then
    transposition count — and its C# counterpart, Jaro.SimilarityCore, carries the
    same suppression for the same reason: decomposing it would break the
    one-to-one mapping with the reference that makes any divergence auditable.

    The argument is stronger here than in the C#. This function GENERATES the
    reference data every other component is validated against, so "the tests still
    pass" would be circular: the tests compare against exactly this output.

    jellyfish agrees for normal inputs but diverges on the same degenerate
    combining-mark / emoji strings as its Hamming (decision 0005). We therefore
    generate from this reference and record the jellyfish divergence count.
    """
    l1, l2 = len(a), len(b)
    if l1 == 0 or l2 == 0:
        return 0.0
    window = max(0, max(l1, l2) // 2 - 1)
    m1 = [False] * l1
    m2 = [False] * l2
    matches = 0
    for i in range(l1):
        lo = max(0, i - window)
        hi = min(i + window + 1, l2)
        for j in range(lo, hi):
            if not m2[j] and a[i] == b[j]:
                m1[i] = m2[j] = True
                matches += 1
                break
    if matches == 0:
        return 0.0
    t = 0
    k = 0
    for i in range(l1):
        if m1[i]:
            while not m2[k]:
                k += 1
            if a[i] != b[k]:
                t += 1
            k += 1
    t //= 2
    m = matches
    return (m / l1 + m / l2 + (m - t) / m) / 3.0


def _jaro_winkler_reference(a: str, b: str, p: float = 0.1) -> float:
    jaro = _jaro_reference(a, b)
    if jaro <= 0.7:
        return jaro
    limit = min(min(len(a), len(b)), 4)
    prefix = 0
    while prefix < limit and a[prefix] == b[prefix]:
        prefix += 1
    return jaro + prefix * p * (1.0 - jaro)


def _similarity_reference_corpus(reference, jelly, algorithm: str) -> dict:
    rng = SeededRandom(SEED)
    diverge = 0
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        ref = reference(a, b)
        if abs(jelly(a, b) - ref) > 1e-9:
            diverge += 1
        cases.append({"id": idx, "category": category, "a": a, "b": b, "similarity": ref})
    return {
        "metadata": {
            "algorithm": algorithm,
            "library": "reference-standard",
            "reference_calls": [f"standard {algorithm} over code points (see decision 0005)"],
            "jellyfish_version": version("jellyfish"),
            "jellyfish_divergences": diverge,
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_jaro() -> dict:
    return _similarity_reference_corpus(_jaro_reference, jellyfish.jaro_similarity, "Jaro")


def generate_jaro_winkler() -> dict:
    return _similarity_reference_corpus(
        _jaro_winkler_reference, jellyfish.jaro_winkler_similarity, "JaroWinkler")


def generate_lcs() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        subsequence = (len(a) + len(b) - Indel.distance(a, b)) // 2
        substring = SequenceMatcher(None, a, b, autojunk=False).find_longest_match(0, len(a), 0, len(b)).size
        cases.append({
            "id": idx, "category": category, "a": a, "b": b,
            "subsequence": subsequence, "substring": substring,
        })
    return {
        "metadata": {
            "algorithm": "Lcs",
            "library": "rapidfuzz+difflib",
            "reference_calls": [
                "subsequence: (len(a)+len(b)-rapidfuzz.distance.Indel.distance)//2",
                "substring: difflib.SequenceMatcher(autojunk=False).find_longest_match(...).size",
            ],
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_ratcliff() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        similarity = SequenceMatcher(None, a, b, autojunk=False).ratio()
        cases.append({"id": idx, "category": category, "a": a, "b": b, "similarity": similarity})
    return {
        "metadata": {
            "algorithm": "RatcliffObershelp",
            "library": "difflib",
            "reference_calls": ["difflib.SequenceMatcher(None, a, b, autojunk=False).ratio()"],
            "autojunk": False,
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_set_similarity() -> dict:
    """qval=1 (textdistance default), multiset (bag) semantics, over non-empty pairs.

    textdistance raises on empty operands, which is its own edge quirk; Lodestar
    defines those separately and covers them via unit tests.
    """
    rng = SeededRandom(SEED)
    cases = []
    for idx, (category, a, b) in enumerate(build_pairs(rng)):
        if a == "" or b == "":
            continue
        cases.append({
            "id": idx, "category": category, "a": a, "b": b,
            JACCARD: td.Jaccard(qval=1).normalized_similarity(a, b),
            "dice": td.Sorensen(qval=1).normalized_similarity(a, b),
            "overlap": td.Overlap(qval=1).normalized_similarity(a, b),
            "tversky": td.Tversky(qval=1).normalized_similarity(a, b),
            "cosine": td.Cosine(qval=1).normalized_similarity(a, b),
        })
    return {
        "metadata": {
            "algorithm": "SetSimilarity",
            "library": "textdistance",
            "library_version": version("textdistance"),
            "reference_calls": [
                "textdistance.{Jaccard,Sorensen,Overlap,Tversky,Cosine}(qval=1).normalized_similarity",
            ],
            "qval": 1,
            "semantics": "code_point",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


CURATED_WORDS = [
    "Robert", "Rupert", "Rubin", "Ashcraft", "Ashcroft", "Tymczak", "Pfister",
    "Honeyman", "Washington", "Lee", "Gutierrez", "Jackson", "VanDeusen", "Deusen",
    "Knuth", "Euler", "Gauss", "Kant", "Lloyd", "Bob", "a", "MacDonald", "Christina",
    "Catherine", "Smith", "Smyth", "Schmidt", "Jefferson", "Adams", "Wojcik",
    "Nguyen", "Johnson", "Williams", "Brown", "Garcia", "Martinez", "Anderson",
    "Thompson", "Phillip", "Xavier", "Yvonne", "Zachary", "Quinn", "Wright",
    "Knight", "Gnome", "Psalm", "Thomas", "Theodore", "Czar", "Pizza", "Aegean",
]


def phonetic_words(rng: SeededRandom):
    for w in CURATED_WORDS:
        yield w
    # Random pronounceable-ish alphabetic words, deterministic.
    letters = "abcdefghijklmnopqrstuvwxyz"
    for _ in range(350):
        length = rng.randint(2, 11)
        w = "".join(rng.choice(letters) for _ in range(length))
        # Occasionally capitalize to exercise case handling.
        yield w.capitalize() if rng.random() < 0.5 else w


METAPHONE_WORDS = [
    "Thomas", "Theodore", "Catherine", "Christina", "Christopher", "Character",
    "Chemistry", "School", "Schmidt", "Knight", "Knife", "Knuth", "Gnome", "Sign",
    "Design", "Gnat", "Wright", "Write", "Wrong", "Psalm", "Pneumonia", "Phone",
    "Phoenix", "Philip", "Elephant", "Rough", "Though", "Through", "Laugh", "Ghost",
    "Judge", "Bridge", "Edge", "Dodge", "Special", "Social", "Musician", "Nation",
    "Action", "Mission", "Passion", "Ocean", "Ancient", "Efficient", "Thumb",
    "Climb", "Lamb", "Comb", "Dumb", "Xavier", "Xylophone", "Box", "Fox", "Exam",
    "Cent", "City", "Cycle", "Cat", "Cool", "Music", "Quick", "Queen", "Square",
    "Zero", "Zone", "Buzz", "Vision", "Version", "Washington", "Jackson", "Jefferson",
    "Robert", "Rupert", "Rubin", "Ashcraft", "Ashcroft", "Tymczak", "Pfister",
    "Honeyman", "Gutierrez", "MacDonald", "Anderson", "Williams", "Thompson",
    "Nicholas", "Vaughan", "Hugh", "Leigh", "Callaghan", "Gough", "Naughton",
    "Aegean", "Caesar", "Scene", "Science", "Scissors", "Fascinate", "Discipline",
    "Yellow", "Yes", "Young", "Beyond", "Layer", "Player", "Day", "Boy", "Guy",
    "Whale", "White", "Where", "Which", "Whisper", "Hour", "Honest", "Heir", "Herb",
    "Ghana", "Spaghetti", "Bologna", "Lasagna", "Champagne", "Foreign", "Reign",
]


def generate_metaphone() -> dict:
    cases = []
    for idx, word in enumerate(METAPHONE_WORDS):
        cases.append({"id": idx, "word": word, "metaphone": jellyfish.metaphone(word)})
    return {
        "metadata": {
            "algorithm": "Metaphone",
            "library": "jellyfish",
            "library_version": version("jellyfish"),
            "reference_calls": ["jellyfish.metaphone"],
            "corpus": "real English words/names (see decision 0007)",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_phonetics() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, word in enumerate(phonetic_words(rng)):
        cases.append({
            "id": idx,
            "word": word,
            "soundex": jellyfish.soundex(word),
            "metaphone": jellyfish.metaphone(word),
            "nysiis": jellyfish.nysiis(word),
        })
    return {
        "metadata": {
            "algorithm": "Phonetics",
            "library": "jellyfish",
            "library_version": version("jellyfish"),
            "reference_calls": ["jellyfish.soundex", "jellyfish.metaphone", "jellyfish.nysiis"],
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


# phonetic_words is ASCII-alphabetic throughout, so it pins the encoder and nothing
# about what reaches it. These fix the input contract instead, from the reference.
DOUBLE_METAPHONE_WORDS = [
    "", " ", "  ", "123", "a1b2", "O'Brien", "Smith-Jones", "Zzzz zzzz",
    "élan", "Ünal", NAIVE, "ç", "日本",
    "A", "x", "aeiou", "McDonald", "van der Berg",
    "Constantinople", "Bhattacharya", "Schwarzenegger",
]


# One-letter words, and the W rules at either end of a two-letter one: "W" threw (#838).
# Yielded last so the ids of the cases before them do not move.
DOUBLE_METAPHONE_SHORT_WORDS = [
    *"BCDEFGHIJKLMNOPQRSTUVWXYZ",
    "AW", "OW", "WA", "WH", "WR", "SW",
]


def double_metaphone_words(rng: SeededRandom):
    yield from DOUBLE_METAPHONE_WORDS
    yield from phonetic_words(rng)
    yield from DOUBLE_METAPHONE_SHORT_WORDS


def generate_double_metaphone() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, word in enumerate(double_metaphone_words(rng)):
        primary, secondary = doublemetaphone(word)
        # doublemetaphone repeats the primary where there is no alternate; '' is what the
        # siblings return and what the C# API exposes -- decision 0075, normalised on the way in.
        cases.append({
            "id": idx,
            "word": word,
            "primary": primary,
            "secondary": "" if secondary == primary else secondary,
        })
    return {
        "metadata": {
            "algorithm": "DoubleMetaphone",
            "library": "doublemetaphone",
            "library_version": version("doublemetaphone"),
            "reference_calls": ["doublemetaphone.doublemetaphone"],
            "corpus": "the input-contract fixed points, plus phonetic_words -- the words decision 0075 compared the candidates over",
            "secondary_convention": "'' for no alternate, unwrapped from the reference's repeated primary",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


# Every Unicode entry is kept under 7 UTF-8 bytes so jellyfish's own codex does not
# corrupt itself on the truncation bug decision 0080 records: parity cases, not divergences.
MRA_WORDS = [
    "Smith", "Smyth", "Byrne", "Boern", "Catherine", "Kathryn", "aeiou",
    "Mississippi", "Bhattacharya", "Schwarzenegger", "Constantinople", "",
    "Zzzz zzzz", "  ", " ", "élan", "Ünal", "日本",
]

# "abcdefghi", not the issue's "abcdefgh": that literal already sits at S1192's
# threshold below, and the pair only needs *a* five-character gap, not that word.
MRA_PAIRS = [
    ("Smith", "Smyth"), ("Byrne", "Boern"), ("Catherine", "Kathryn"),
    ("Smith", "Smith"), ("", ""), ("Sm", "Sm"), ("Tim", "Timothy"),
    ("abc", "abcdefghi"), ("Smith", "Smithsonian"), ("ab", "abcde"),
    ("Smith", ""),
]


def match_rating_words(rng: SeededRandom):
    yield from MRA_WORDS
    yield from phonetic_words(rng)


def generate_match_rating_codex() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for idx, word in enumerate(match_rating_words(rng)):
        cases.append({"id": idx, "word": word, "codex": jellyfish.match_rating_codex(word)})
    return {
        "metadata": {
            "algorithm": "MatchRatingApproach.Codex",
            "library": "jellyfish",
            "library_version": version("jellyfish"),
            "reference_calls": ["jellyfish.match_rating_codex"],
            "corpus": "the issue's fixed points, plus phonetic_words' real names and random words",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_match_rating_comparison() -> dict:
    # phonetic_words alone, not match_rating_words: its words are ASCII, so character
    # and UTF-8 byte length always agree and decision 0080's divergence cannot enter.
    words = list(phonetic_words(SeededRandom(SEED)))
    pairs = list(MRA_PAIRS)
    # Consecutive words from that list, deterministically -- covers every bucket
    # of the combined-codex-length -> minimum-rating table.
    for i in range(0, len(words) - 1, 2):
        pairs.append((words[i], words[i + 1]))

    cases = []
    for idx, (a, b) in enumerate(pairs):
        cases.append({"id": idx, "a": a, "b": b, "comparison": jellyfish.match_rating_comparison(a, b)})
    return {
        "metadata": {
            "algorithm": "MatchRatingApproach.Compare",
            "library": "jellyfish",
            "library_version": version("jellyfish"),
            "reference_calls": ["jellyfish.match_rating_comparison"],
            "corpus": "the issue's own worked pairs, plus consecutive pairs from the codex corpus",
            "seed": SEED,
            "count": len(cases),
        },
        "cases": cases,
    }


CORPUS_A = [
    CAT_SENTENCE,
    "a cat and a dog",
    "the dog barked loudly",
    "cats and dogs are friends",
    QUICK_FOX,
]
CORPUS_ACCENTS = ["Café crème", "Cafe creme", "Élève à l'école", "eleve a l ecole"]
CORPUS_WHITESPACE = ["a\tb c", "x\n\ny  z\r\n", "p\x1cq\x1c\x1dr", "u\u2003v\u00a0\u00a0w"]


def _build_count_vectorizer(cfg: dict):
    return SkCountVectorizer(
        analyzer=cfg.get("analyzer", "word"),
        ngram_range=(cfg.get("ngram_min", 1), cfg.get("ngram_max", 1)),
        min_df=cfg.get("min_df", 1),
        max_df=cfg.get("max_df", 1.0),
        binary=cfg.get("binary", False),
        lowercase=cfg.get("lowercase", True),
        strip_accents="unicode" if cfg.get("strip_accents", False) else None,
        stop_words=cfg.get("stop_words", None),
    )


COUNT_CASES = [
    {"config": {}, "docs": CORPUS_A},
    {"config": {"ngram_min": 1, "ngram_max": 2}, "docs": CORPUS_A},
    {"config": {"min_df": 2}, "docs": CORPUS_A},
    {"config": {"max_df": 0.5}, "docs": CORPUS_A},
    {"config": {"binary": True}, "docs": CORPUS_A},
    {"config": {"stop_words": ["the", "a", "and"]}, "docs": CORPUS_A},
    {"config": {"stop_words": "english"}, "docs": CORPUS_A},
    {"config": {"lowercase": False}, "docs": CORPUS_A},
    {"config": {"strip_accents": True}, "docs": CORPUS_ACCENTS},
    {"config": {"analyzer": "char", "ngram_min": 2, "ngram_max": 3}, "docs": CORPUS_A[:3]},
    {"config": {"analyzer": "char_wb", "ngram_min": 2, "ngram_max": 3}, "docs": CORPUS_A[:3]},
    # #879: only runs of two or more whitespace collapse (\s\s+), and U+001C..U+001F are whitespace.
    {"config": {"analyzer": "char", "ngram_min": 1, "ngram_max": 2}, "docs": CORPUS_WHITESPACE},
    {"config": {"analyzer": "char_wb", "ngram_min": 1, "ngram_max": 2}, "docs": CORPUS_WHITESPACE},
]


def generate_countvectorizer() -> dict:
    cases = []
    for idx, case in enumerate(COUNT_CASES):
        cv = _build_count_vectorizer(case["config"])
        x = cv.fit_transform(case["docs"])
        cases.append({
            "id": idx,
            "config": case["config"],
            "docs": case["docs"],
            "feature_names": cv.get_feature_names_out().tolist(),
            "matrix": x.toarray().astype(int).tolist(),
        })
    return {
        "metadata": {
            "algorithm": "CountVectorizer",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.feature_extraction.text.CountVectorizer"],
            "count": len(cases),
        },
        "cases": cases,
    }


TFIDF_CASES = [
    {"config": {}, "docs": CORPUS_A},
    {"config": {"sublinear_tf": True}, "docs": CORPUS_A},
    {"config": {"smooth_idf": False}, "docs": CORPUS_A},
    {"config": {"norm": None}, "docs": CORPUS_A},
    {"config": {"use_idf": False}, "docs": CORPUS_A},
    {"config": {"ngram_min": 1, "ngram_max": 2}, "docs": CORPUS_A},
    {"config": {"norm": "l1"}, "docs": CORPUS_A},
]


def generate_tfidfvectorizer() -> dict:
    cases = []
    for idx, case in enumerate(TFIDF_CASES):
        cfg = case["config"]
        tv = SkTfidfVectorizer(
            ngram_range=(cfg.get("ngram_min", 1), cfg.get("ngram_max", 1)),
            use_idf=cfg.get("use_idf", True),
            smooth_idf=cfg.get("smooth_idf", True),
            sublinear_tf=cfg.get("sublinear_tf", False),
            norm=cfg.get("norm", "l2") if "norm" in cfg else "l2",
        )
        x = tv.fit_transform(case["docs"])
        cases.append({
            "id": idx,
            "config": cfg,
            "docs": case["docs"],
            "feature_names": tv.get_feature_names_out().tolist(),
            "idf": tv.idf_.tolist() if cfg.get("use_idf", True) else None,
            "matrix": x.toarray().tolist(),
        })
    return {
        "metadata": {
            "algorithm": "TfidfVectorizer",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.feature_extraction.text.TfidfVectorizer"],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_hashingvectorizer() -> dict:
    from sklearn.feature_extraction.text import HashingVectorizer as SkHV
    from sklearn.utils.murmurhash import murmurhash3_32

    hashes = {t: int(murmurhash3_32(t.encode("utf-8"), seed=0)) for t in ["cat", "the", "dog", "hello", "a", "mat"]}
    configs = [
        {"n_features": 16, "alternate_sign": True, "norm": None},
        {"n_features": 16, "alternate_sign": True, "norm": "l2"},
        {"n_features": 16, "alternate_sign": False, "norm": None},
        {"n_features": 8, "ngram_min": 1, "ngram_max": 2, "norm": None},
    ]
    cases = []
    for idx, cfg in enumerate(configs):
        hv = SkHV(
            n_features=cfg["n_features"],
            alternate_sign=cfg.get("alternate_sign", True),
            norm=cfg.get("norm", "l2"),
            ngram_range=(cfg.get("ngram_min", 1), cfg.get("ngram_max", 1)),
        )
        x = hv.fit_transform(CORPUS_A)
        cases.append({"id": idx, "config": cfg, "docs": CORPUS_A, "matrix": x.toarray().tolist()})
    return {
        "metadata": {
            "algorithm": "HashingVectorizer",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.feature_extraction.text.HashingVectorizer", "sklearn.utils.murmurhash.murmurhash3_32"],
            "murmur3": hashes,
            "count": len(cases),
        },
        "cases": cases,
    }


PORTER_WORDS = [
    # step 1a
    "caresses", "ponies", "ties", "caress", "cats",
    # step 1b
    "feed", "agreed", "plastered", "bled", "motoring", "sing", "conflated",
    "troubled", "sized", "hopping", "tanned", "falling", "hissing", "fizzed",
    "failing", "filing",
    # step 1c
    "happy", "sky",
    # step 2
    "relational", "conditional", "rational", "valenci", "hesitanci", "digitizer",
    "conformabli", "radicalli", "differentli", "vileli", "analogousli",
    "vietnamization", "predication", "operator", "feudalism", "decisiveness",
    "hopefulness", "callousness", "formaliti", "sensitiviti", "sensibiliti",
    # step 3
    "triplicate", "formative", "formalize", "electriciti", "electrical",
    "hopeful", "goodness",
    # step 4
    "revival", "allowance", "inference", "airliner", "gyroscopic", "adjustable",
    "defensible", "irritant", "replacement", "adjustment", "dependent",
    "adoption", "homologous", "communism", "activate", "angulariti",
    "homologou", "effective", "bowdlerize",
    # step 5
    "probate", "rate", "cease", "controll", "roll",
    # common words
    "running", "runner", "easily", "fairly", "national", "generalization",
    "organization", "happiness", "argument", "arguing", "meetings",
]


def generate_porter() -> dict:
    from nltk.stem.porter import PorterStemmer  # noqa: PLC0415 (lazy: sandbox import guard)

    stemmer = PorterStemmer(mode=PorterStemmer.ORIGINAL_ALGORITHM)
    cases = [{"id": i, "word": w, "stem": stemmer.stem(w)} for i, w in enumerate(PORTER_WORDS)]
    return {
        "metadata": {
            "algorithm": "PorterStemmer",
            "library": "nltk",
            "library_version": version("nltk"),
            "mode": "ORIGINAL_ALGORITHM",
            "reference_calls": ["nltk.stem.porter.PorterStemmer(mode=ORIGINAL_ALGORITHM)"],
            "count": len(cases),
        },
        "cases": cases,
    }


SNOWBALL_EN_WORDS = PORTER_WORDS + [
    "generous", "generously", "generation", "communism", "communication", "arsenic",
    "national", "nationally", "rationalization", "sensational", "consciously",
    "beautiful", "beautifully", "happily", "quickly", "slowly", "friendly",
    "management", "development", "government", "measurement", "achievement",
    "creation", "relation", "position", "decision", "television", "discussion",
    "activity", "sensitivity", "productivity", "ability", "possibility",
    "organize", "realize", "recognize", "characterize", "modernize",
    "connected", "connecting", "connection", "connects", "connect",
    "studies", "studied", "studying", "study", "cries", "cried", "crying",
    "agreement", "agreed", "agreeing", "agrees", "agree",
    "controlling", "controlled", "controls", "control", "rolling", "rolled",
    "flying", "denying", "trying", "buying", "playing", "enjoying",
    "hopeful", "careful", "wonderful", "powerful", "successful",
    "goodness", "happiness", "kindness", "darkness", "weakness",
    "faithfully", "hopefully", "carefully", "exactly", "absolutely",
    "biology", "psychology", "technology", "apology", "analogy",
    "universities", "abilities", "cities", "parties", "countries",
    "running", "runner", "runs", "swimmer", "swimming", "beginner", "beginning",
    "european", "america", "france", "england", "computer", "internet",
    "walking", "talked", "jumped", "wanted", "needed", "worked", "looked",
]


def generate_snowball_en() -> dict:
    from nltk.stem.snowball import SnowballStemmer  # noqa: PLC0415

    stemmer = SnowballStemmer("english")
    seen = set()
    words = [w for w in SNOWBALL_EN_WORDS if not (w in seen or seen.add(w))]
    cases = [{"id": i, "word": w, "stem": stemmer.stem(w)} for i, w in enumerate(words)]
    return {
        "metadata": {
            "algorithm": "EnglishSnowballStemmer",
            "library": "nltk",
            "library_version": version("nltk"),
            "reference_calls": ["nltk.stem.snowball.SnowballStemmer('english')"],
            "count": len(cases),
        },
        "cases": cases,
    }


SNOWBALL_FR_WORDS = [
    "continuellement", "amoureusement", "national", "nationale", "nationaux", "finalement",
    "rapidement", "organisation", "organiser", "organisé", "développement", "information",
    "maison", "maisons", "cheval", "chevaux", "journal", "journaux", "heureuse", "heureux",
    "finir", "finissait", "finissant", "mangé", "mangée", "mangées", "manger", "mangez",
    "parlions", "parlait", "parlerons", "chanter", "chantez", "chantait", "chanteront",
    "beauté", "activité", "possibilité", "capacité", "réalité", "société", "qualité",
    "important", "importante", "importants", "différent", "différence", "présidence",
    "gentiment", "vraiment", "seulement", "notamment", "évidemment", "constamment",
    "production", "création", "administration", "communication", "génération",
    "technologie", "psychologie", "biologie", "logique", "musique", "physique",
    "grandeur", "chaleur", "couleur", "douleur", "bonheur", "malheur",
    "premier", "première", "dernier", "dernière", "policier", "policière",
    "voiture", "nature", "culture", "structure", "aventure", "peinture",
    "national", "rationnel", "personnel", "naturel", "culturel", "actuel",
    "grandir", "choisir", "réussir", "réfléchir", "établir", "accomplir",
    "aimer", "aimé", "aimait", "aimeront", "aimerais", "donner", "donné", "donnait",
    "petit", "petite", "petits", "petites", "grand", "grande", "grands",
    "rouge", "rouges", "jaune", "jaunes", "libre", "libres", "riche", "riches",
    "connaissance", "puissance", "naissance", "croissance", "assurance",
    "facilement", "difficilement", "heureusement", "malheureusement", "certainement",
    "utiliser", "utilisé", "utilisation", "réalisation", "réaliser", "réalisé",
    "gouvernement", "changement", "mouvement", "sentiment", "moment", "document",
    "belle", "belles", "vieille", "nouvelle", "nouvelles", "ancienne", "ancien",
    "manière", "matière", "lumière", "rivière", "prière", "carrière",
    # A word-final é or è keeps its accent; only one followed by a non-vowel loses it (#948)
    "thé", "né", "fée", "été", "idée", "café", "pré", "dé", "blé", "clé", "abbé", "bébé",
    "année", "armée", "épée", "entrée", "allée", "musée", "créé", "agréé", "côté", "procédé",
    "ès", "père", "élève", "progrès", "succès", "problème", "fidèle", "chèvre",
]


def generate_snowball_fr() -> dict:
    from nltk.stem.snowball import SnowballStemmer  # noqa: PLC0415

    stemmer = SnowballStemmer("french")
    seen = set()
    words = [w for w in SNOWBALL_FR_WORDS if not (w in seen or seen.add(w))]
    cases = [{"id": i, "word": w, "stem": stemmer.stem(w)} for i, w in enumerate(words)]
    return {
        "metadata": {
            "algorithm": "FrenchSnowballStemmer",
            "library": "nltk",
            "library_version": version("nltk"),
            "reference_calls": ["nltk.stem.snowball.SnowballStemmer('french')"],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Additional Snowball languages, see _snowball_corpus's docstring ----------

SNOWBALL_ES_WORDS = [
    # step 0: attached object pronouns
    "damelo", "dámelo", "haciéndola", "haciendolo", "vámonos", "escribirle", "decirles",
    "comprarlo", "cantándome", "construyendolo", "dárselo", "mostrárselas",
    # step 1: nominal / adjectival suffixes
    "esperanza", "esperanzas", "musico", "musica", "musicos", "musicas",
    "realismo", "realismos", "amable", "amables", "posible", "posibles",
    "artista", "artistas", "hermoso", "hermosa", "hermosos", "hermosas",
    "conocimiento", "conocimientos", "sentimiento", "sentimientos",
    "computadora", "computador", "generación", "generaciones", "trabajador", "trabajadores",
    "importante", "importantes", "distancia", "distancias",
    "biología", "biologías", "solución", "soluciones", "revolución", "revoluciones",
    "existencia", "existencias", "paciencia",
    "rapidamente", "rápidamente", "claramente", "efectivamente", "activamente",
    "realmente", "generalmente", "posiblemente", "amablemente",
    "ciudad", "ciudades", "capacidad", "capacidades", "actividad", "actividades",
    "activa", "activo", "activas", "activos", "creativo", "creativa",
    # step 2: verb endings
    "cantar", "canto", "cantas", "cantamos", "cantaron", "cantaban", "cantaría",
    "cantarían", "cantaremos", "cantase", "cantaste", "cantando",
    "comer", "comes", "comemos", "comieron", "comería", "comiendo", "comido",
    "vivir", "vives", "vivimos", "vivieron", "viviría", "viviendo", "vivido",
    "construyendo", "construyeron", "leyendo", "leyeron", "oyendo",
    "distinguen", "distinguir", "sigue", "siguen", "pague", "paguen",
    # short / residual
    "casa", "casas", "libro", "libros", "papel", "papeles", "sol", "mar",
    "país", "países", "café", "bebé", "and", "yo", "el", "la",
]


SNOWBALL_PT_WORDS = [
    "esperança", "esperanças", "musico", "musica", "musicos", "musicas",
    "realismo", "amável", "amáveis", "possível", "possíveis",
    "artista", "artistas", "formoso", "formosa", "formosos", "formosas",
    "conhecimento", "conhecimentos", "sentimento", "sentimentos",
    "computador", "computadores", "geração", "gerações", "trabalhador", "trabalhadores",
    "importante", "importantes", "distância", "distâncias",
    "biologia", "biologias", "solução", "soluções", "revolução", "revoluções",
    "existência", "existências", "paciência",
    "rapidamente", "claramente", "efetivamente", "ativamente",
    "realmente", "geralmente", "possivelmente",
    "cidade", "cidades", "capacidade", "capacidades", "atividade", "atividades",
    "ativa", "ativo", "ativas", "ativos", "criativo", "criativa",
    "cantar", "canto", "cantas", "cantamos", "cantaram", "cantava", "cantaria",
    "cantariam", "cantaremos", "cantasse", "cantaste", "cantando",
    "comer", "comes", "comemos", "comeram", "comeria", "comendo", "comido",
    "partir", "partes", "partimos", "partiram", "partiria", "partindo", "partido",
    "casa", "casas", "livro", "livros", "papel", "papéis", "sol", "mar",
    "país", "países", "café", "bebê", "coração", "corações",
    "nação", "nações", "irmã", "irmãs", "logia", "logias",
]


SNOWBALL_IT_WORDS = [
    "speranza", "speranze", "musico", "musica", "musici", "musiche",
    "realismo", "realismi", "amabile", "amabili", "possibile", "possibili",
    "artista", "artisti", "formoso", "formosa", "formosi", "formose",
    "conoscimento", "sentimento", "sentimenti",
    "computatore", "computatori", "generazione", "generazioni",
    "lavoratore", "lavoratori", "importante", "importanti", "distanza", "distanze",
    "biologia", "biologie", "soluzione", "soluzioni", "rivoluzione", "rivoluzioni",
    "esistenza", "esistenze", "pazienza",
    "rapidamente", "chiaramente", "effettivamente", "attivamente",
    "realmente", "generalmente", "possibilmente",
    "citta", "città", "capacita", "capacità", "attivita", "attività",
    "attiva", "attivo", "attive", "attivi", "creativo", "creativa",
    "cantare", "canto", "canti", "cantiamo", "cantarono", "cantava", "canterebbe",
    "cantando", "cantato", "cantata", "cantate", "cantati",
    "credere", "credi", "crediamo", "credono", "credendo", "creduto",
    "finire", "finisci", "finiamo", "finirono", "finendo", "finito",
    "casa", "case", "libro", "libri", "carta", "carte", "sole", "mare",
    "paese", "paesi", "caffè", "abbandonare", "abbandonato",
]


SNOWBALL_DE_WORDS = [
    # German preprocessing: sharp s, u/y between vowels, umlauts
    "straße", "strasse", "größe", "grosse", "fuß", "füße",
    "kraut", "kräuter", "haus", "häuser", "baum", "bäume",
    # -heit / -keit / -ung / -nis / -isch / -lich / -ig / -end
    "schönheit", "schönheiten", "freiheit", "möglichkeit", "möglichkeiten",
    "wohnung", "wohnungen", "zeitung", "zeitungen", "rechnung",
    "ergebnis", "ergebnisse", "geheimnis", "kenntnis",
    "praktisch", "praktische", "politisch", "politischen",
    "freundlich", "freundliche", "freundlichen", "wirklich", "wirkliche",
    "wichtig", "wichtige", "wichtigen", "richtig", "richtiges",
    "lachend", "singend", "arbeitend",
    # inflectional endings -e -en -es -em -er -ern -est
    "kinder", "kindern", "kindes", "kinde", "kind",
    "guten", "gutes", "gutem", "guter", "gute", "gut",
    "schnellsten", "schnellste", "schnellst", "schnell",
    "männer", "männern", "frauen", "frau", "mann",
    # verbs
    "arbeiten", "arbeitet", "arbeitete", "gearbeitet", "arbeite",
    "spielen", "spielt", "spielte", "gespielt",
    "laufen", "läuft", "lief", "gelaufen",
    "sprechen", "spricht", "sprach", "gesprochen",
    # short / residual
    "der", "die", "das", "und", "ist", "ein", "eine", "einen",
]


SNOWBALL_NL_WORDS = [
    # Dutch preprocessing: umlauts and acutes folded, grave kept, y and i marked
    "café", "cafés", "coördinatie", "reünie", "één", "privé",
    "ijs", "ijsje", "lijn", "lijnen", "mijn", "mijnen", "zijde", "wijn",
    "draaien", "draaide", "zaaien", "gooien", "yoga", "yoghurt", "typisch",
    # step 1: -heden / -ene / -en / -se / -s
    "mogelijkheden", "mogelijkheid", "gelegenheden", "gelegenheid",
    "waarheden", "waarheid", "schoonheden", "schoonheid", "vrijheden", "vrijheid",
    "overheden", "overheid", "gezondheid", "eenheden", "eenheid",
    "huizen", "huis", "boeken", "boek", "kinderen", "kind", "mensen", "mens",
    "deuren", "deur", "ramen", "raam", "tafels", "tafel",
    "jongens", "jongen", "meisjes", "meisje", "huisjes", "huisje",
    # step 1 undoubling: a kk / dd / tt the deletion uncovers
    "bakken", "bakt", "pakken", "redden", "redt", "zitten", "zat", "likken",
    # step 2: a final e in R1 after a non-vowel
    "grote", "groot", "kleine", "klein", "goede", "goed", "mooie", "mooi",
    "snelle", "snel", "oude", "oud", "nieuwe", "nieuw", "ziekte", "ziektes",
    # step 3a: -heid in R2, and the -en that can follow it out
    "moeilijkheid", "moeilijkheden", "wetenschappelijkheid",
    # step 3b: -end / -ing / -ig / -lijk / -baar / -bar
    "lopend", "lopende", "zittend", "wandelend",
    "opening", "openingen", "regering", "regeringen", "woning", "woningen",
    "koning", "koningen", "wandeling", "wandelingen", "rekening", "tekening",
    "verandering", "veranderingen", "behandeling", "verzameling",
    "machtig", "machtige", "prachtig", "gelukkig", "nodig", "bezig", "vorig",
    "vriendelijk", "vriendelijke", "natuurlijk", "eindelijk", "duidelijk",
    "werkelijke", "persoonlijk", "wetenschappelijk",
    "dankbaar", "dankbare", "zichtbaar", "leesbaar", "openbaar", "openbare",
    "houdbaar", "wonderbaar",
    # step 4: the undoubled vowel
    "maan", "manen", "boot", "boten", "brood", "broden", "uur", "uren",
    "zaak", "zaken", "paard", "paarden", "kaas", "kazen", "been", "benen",
    "muur", "muren", "aap", "apen", "baas", "bazen",
    # verbs and their participles
    "lopen", "loopt", "liep", "gelopen", "werken", "werkt", "gewerkt",
    "maken", "maakt", "gemaakt", "spreken", "spreekt", "gesproken",
    "bouwen", "bouwde", "gebouwd", "gebouw", "gebouwen",
    # short / residual
    "de", "het", "een", "en", "van", "in", "op", "is", "zijn", "niet", "gem",
]


SNOWBALL_SV_WORDS = [
    # step 1(a), the -het family: -het -heten -heter -heterna -hetens
    "möjlighet", "möjligheten", "möjligheter", "möjligheterna", "möjlighetens",
    "verklighet", "verkligheten", "skyldighet", "oskyldighet", "sanningen",
    "härlighet", "härligheten", "nyhet", "nyheter", "nyheterna",
    "enhet", "enheter", "enheten", "enhetens",
    # step 1(a), the -ande family: -ande -anden -andes -andet
    "vandrande", "vandranden", "vandrandes", "vandrandet",
    "skrivande", "skrivandet", "leende", "boende", "glädjande",
    # step 1(a), the -ar / -are / -aren / -arens / -arna / -arnas family
    "högtalare", "högtalaren", "högtalarens", "högtalarna", "högtalarnas",
    "läsare", "läsaren", "läsarens", "läsarna", "artiklar", "vandringar",
    "vandringarna", "pojkarna", "fiskarne", "spelare", "spelarna",
    # step 1(a), the -er / -ern / -erns / -erna / -ernas family
    "fängelser", "vintern", "vinterns", "länderna", "ländernas", "systern",
    "bäckens", "kanalen", "kapitlet", "kapitlets",
    # step 1(a), the -or / -orna / -ornas family
    "flickor", "flickorna", "flickornas", "stjärnorna", "människorna",
    # step 1(a), the plain endings: -a -e -ad -ade -ades -as -at -ast -aste -en -es -ens
    "människa", "flicka", "glädje", "begynnelse", "fängelse",
    "älskad", "älskade", "älskades", "kallad", "kallat", "bilas",
    "snabbast", "snabbaste", "sjukhusen", "byggnaderna", "glädjens",
    "tjänstemännens", "tjänstemän", "heres", "andes", "arens",
    # step 1(b): a bare s, kept unless the letter before it is a valid s-ending
    HUSETS, HUSET, "hus", "bordets", "landets", "fartygets", "kappsäcks",
    "radios", "fotos", "artikels", "kapitels", "tidsels",
    "chefs", "arkivs", "partys", "chips", "bajs", "picknicks", "bergs", "hems",
    # the letters outside the s-ending set, which leave the s in place
    "ovanpås", "sjös", "sås", "cirkus", "hos", "ros", "os",
    # a suffix that reaches past R1 while a shorter one inside it does not
    "arne", "aste", "ades", "ernas", "ornas", "arnas", "hetens", "erns",
    # step 2: the doubled consonant a deletion or an inflection leaves behind
    "friskt", "frisk", "byggd", "byggt", "bygg", "stängd", "stängt", "stänga",
    "sagt", "lagt", "nätt", "vitt", "glad", "gladd", "vann", "vinna",
    "kändt", "blandat", "fullständigt", "fullständig",
    "otäckt", "otäck", "perfekt", "gott", "brett", "trött",
    # verbs, whose participles are where the consonant pairs come from
    "springa", "springer", "sprang", "sprungit", "köpa", "köper", "köpte", "köpt",
    "säga", "säger", "sade", "göra", "gjorde", "gjort", "läsa", "läser", "läste",
    # step 3: -lig -ig -els deleted, -löst and -fullt replaced
    "härlig", "härligt", "vänlig", "vänligt", "vänligheten",
    "möjlig", "möjliga", "verklig", "verkliga", "rolig", "roliga", "roligt",
    "händelse", "händelsen", "händelser", "rörelse", "rörelser", "medels",
    "tvivelaktigt", "tvivelaktig", "viktig", "viktigt", "viktigare",
    "hemlöst", "hjälplöst", "ändlöst", "trolös", "löst",
    "kärleksfullt", "kärleksfull", "fullt", "full",
    # words that must come back whole: too short for R1, or nothing to strip
    "kärlek", "hus", "sjö", "träd", "bok", "och", "att", "det", "som",
    "med", "för", "inte", "han", "hon", "den", "var", "ett", "jag",
    "a", "ab", "abc", "abcd",
]


SNOWBALL_RU_WORDS = [
    # step 1, perfective gerund: -в -вши -вшись after а/я, -ив -ыв -ивши -ывши
    # -ившись -ывшись unconditionally
    "прочитав", "сделав", "написав", "написавши", "написавшись",
    "закрыв", "открыв", "забыв", "покрыв", "закрывши", "забывшись",
    "проводив", "получив", "изучив", "решив", "получивши", "получившись",
    # step 1, reflexive: -ся -сь, removed before the three that follow
    "учиться", "учится", "смеяться", "смеялся", "смеялась", "смеялись",
    "находится", "находятся", "находились", "казался", "казалась", "казалось",
    # step 1, adjectival: an adjective ending, optionally preceded by a participle
    "красивый", "красивая", "красивое", "красивые", "красивых", "красивым",
    "красивому", "красивого", "красивую", "красивой", "красивом", "красивыми",
    "новый", "новая", "новое", "новые", "нового", "новому", "новых", "новыми",
    "новою", "синюю", "синяя", "синею", "большой", "большая", "большие",
    "большого", "хороший", "хорошая", "хорошее", "хорошие", "любимый", "любимая",
    # the participle groups: -ющ -щ -вш -ем -нн after а/я, -ивш -ывш -ующ without
    "читающий", "читающая", "читающие", "читающего", "читающему",
    "делающий", "делающая", "горящий", "горящая", "блестящий", "блестящая",
    "читавший", "читавшая", "читавшего", "написавший",
    "читаемый", "читаемая", "читаемого",
    "сделанный", "сделанная", "сделанного", "сделанным",
    # the one pair nltk's table spells wrong, decision 0086: -ующая keeps its ующ
    "рискующая", "рискующий", "танцующая", "танцующий",
    "существующая", "существующий", "действующая", "действующий",
    # step 1, verb: two groups again, the first only after а or я
    "читать", "читаю", "читаешь", "читает", "читаем", "читаете", "читают",
    "читал", "читала", "читали", "читало", "читайте",
    "говорить", "говорю", "говоришь", "говорит", "говорим", "говорите",
    "говорят", "говорил", "говорила", "говорили",
    "делать", "делаю", "делаешь", "делает", "делаем", "делаете", "делают",
    "делал", "делала", "делали", "написать", "написал", "написали",
    "видеть", "видел", "видели", "любить", "люблю", "любит", "любят",
    "закрыть", "закрыл", "закрыт", "закрыта", "закрыто", "закрыты",
    "сделан", "сделана", "сделано", "сделаны",
    "рисуйте", "танцуйте", "рисует", "рисуют", "рисую",
    # step 1, noun: the longest of thirty-six, all of them inside RV
    "город", "города", "городе", "городу", "городом", "городов", "городах",
    "городами", "городам", "книга", "книги", "книге", "книгу", "книгой",
    "книгам", "книгах", "книгами", "стол", "стола", "столы", "столов",
    "столам", "столах", "время", "времени", "временем",
    "здание", "здания", "зданию", "зданием", "зданиями", "зданиях", "зданий",
    "статья", "статьи", "статье", "статью", "статьей", "статьями", "статьях",
    "дверь", "двери", "дверью", "дверям", "дверях", "путь", "пути", "путем",
    "путей", "лошадь", "лошади", "лошадью",
    "земля", "земли", "земле", "землю", "землей", "землями",
    # step 2, a final и in RV, whatever step 1 did
    "мыши", "ножи", "враги", "руки", "ноги",
    # step 3, the two derivational endings, and only inside R2
    "радость", "радости", "радостью", "новость", "новости", "возможность",
    "возможности", "молодость", "скорость", "скорости", "гордость",
    "честность", "ясность", "сложность", "полезность",
    # step 4, the three that end the algorithm: нн undoubled, a superlative
    # ending removed, or a final ь dropped
    "странный", "странная", "длинный", "длинная", "военный", "современный",
    "туманный", "туманна", "туманно", "весенний",
    "красивейший", "красивейшая", "сильнейший", "новейший", "интереснейший",
    "важнейший", "умнейший",
    "конь", "день", "тень", "лень", "жизнь", "мысль", "часть", "ночь", "дочь",
    "речь", "печь", "власть",
    # ё is not a letter of the algorithm's alphabet and reads as е
    "ёлка", "ёлки", "ёж", "всё", "лёгкий", "чёрный", "тёплый",
    # ъ inside a word, which the transliteration the reference uses splits in two
    "объявление", "объявления", "подъезд", "съезд",
    # uppercase, which the Cyrillic alphabet carries and the algorithm does not
    "Москва", "ГОРОДА", "Книгами",
    # words that come back whole: no vowel, an empty RV, or nothing to strip
    "он", "она", "оно", "они", "мы", "вы", "ты", "я", "не", "но", "и", "в",
    "с", "к", "по", "из", "до", "за", "hello", "café",
]


SNOWBALL_DA_WORDS = [
    # step 1(a), the -hed family: -hed -heden -heder -hedens, and -ethed
    "kærlighed", "kærligheden", "kærligheder", "kærlighedens",
    "sandhed", "sandheden", "sandheder", "sandhedens",
    "mulighed", "muligheden", "muligheder", "mulighedens",
    "virkelighed", "virkeligheden", "hemmelighed", "hemmeligheder",
    "frihed", "friheden", "sundhed", "sundheden", "nyhed", "nyheder",
    "enkelthed", "enkeltheden", "offentlighed", "offentligheden",
    # step 1(a), the -er family: -ere -eren -erens -erne -ernes -erer -ered
    "lærere", "læreren", "lærerens", "lærerne", "lærernes", "lærer",
    "arbejdere", "arbejderen", "arbejderne", "arbejdernes", "arbejder",
    "spillere", "spilleren", "spillerne", "spiller",
    "undervisere", "underviseren", "underviser",
    # step 1(a), the -erede / -erende / -erendes / -eret / -erets family
    "leverede", "leverendes", "leveret", "leverets", "levere", "leverer",
    "markerede", "markeret", "markerer", "noterede", "noteret",
    "studerende", "studerendes", "regerende", "regerendes", "beregnede",
    # step 1(a), the plain endings: -e -en -ende -ene -ens -er -ers -es -et -ets
    "husene", HUSETS, HUSET, "huse",
    "bilerne", "bilernes", "bilen", "biler", "bilens", "bile",
    "bogen", "bøger", "bøgerne", "bogens",
    "barnet", "barnets", "børnene", "børns",
    "manden", "mandens", "mændene", "mænds",
    "landet", "landets", "landene", "lande",
    "kvinden", "kvinder", "kvinderne", "kvindens", "kvinde",
    "dagen", "dagene", "dagens", "tiden", "tider", "tiderne", "tidens",
    "løbende", "siddende", "stående", "gående",
    # step 1(b): a bare s, kept unless the letter before it is a valid s-ending
    "hjems", "folks", "bords", "hunds", "bjergs", "kiosks", "chefs",
    "radios", "fotos", "taxas", "banks", "films", "sofas", "korts", "jobs",
    # the letters outside the s-ending set, which leave the s in place
    "virus", "kursus", "bonus", "campus", "paradis", "nervøs", "religiøs",
    "kaos", "gris", "hus",
    # step 2: a gd / dt / gt / kt in R1 loses its last letter
    "sagt", "lagt", "bragt", "vagt", "magt", "søgt", "vægt", "godt",
    "perfekt", "punkt", "projekt", "tænkt", "friskt", "frisk",
    # step 3, the igst rule: the final st goes before the suffix search runs
    "hurtigst", "vigtigst", "billigst", "dejligst", "venligst", "tidligst",
    # step 3(a): -ig -lig -elig -els deleted, then step 2 runs again
    "hurtig", "hurtigt", "vigtig", "vigtigt", "billig", "billigt",
    "rigtig", "rigtigt", "tidlig", "dejlig", "dejligt", "venlig", "venligt",
    "kærlig", "kærligt", "synlig", "synligt", "farlig", "farligt",
    "endelig", "virkelig", "egentlig", "forskellig", "sandsynlig",
    "middels", "handels", "fjendtligt",
    # step 3(b): -løst rewritten to -løs, so the stem stays a Danish word
    "hjælpeløst", "meningsløst", "endeløst", "trådløst", "arbejdsløst",
    "hjælpeløs", "løst", "løs",
    # step 4: the double consonant a deletion or a rewrite uncovers
    "bestemmelse", "bestemmelsen", "bestemmelser", "bestemme", "bestemmer",
    "stemme", "stemmer", "stemmen", "hammeren", "hammer",
    "nummeret", "nummer", "sommeren", "sommer", "vinteren", "vinter",
    "villig", "villige", "hyggelig", "hyggeligt", "kaffen", "kaffe",
    "oplevelse", "oplevelser", "bevægelse", "bevægelser",
    "forbindelse", "forbindelser", "afgørelse", "ændringer",
    # the apostrophe the published description gives R1 to and nltk does not --
    # decision 0087 follows nltk, and these are what pin that
    "pc'er", "cd'er", "tv'et", "bil'er", "computer'en", "a'ere",
    # words that must come back whole: too short for R1, or nothing to strip
    "og", "at", "det", "en", "et", "er", "som", "på", "med", "for",
    "ikke", "han", "hun", "den", "var", "jeg", "til", "af",
    "a", "ab", "abc", "abcd",
]


# Hungarian is oracled by snowballstemmer rather than nltk -- decision 0090 has
# the two omissions in nltk's Hungarian that made it unusable as a reference.
SNOWBALL_HU_WORDS = [
    # step 2, remove frequent cases: the case endings, both vowel harmonies
    "ház", "házban", "házba", "házból", "házra", "házról", "háztól", "háznál",
    "házhoz", "házig", "házért", "házként", "házkor", "házzal", "házon", "házat",
    "kert", "kertben", "kertbe", "kertből", "kertre", "kertről", "kerttől",
    "kertnél", "kerthez", "kertig", "kerten", "kertet", "kertnek", "kertté",
    "könyv", "könyvben", "könyvből", "könyvhöz", "könyvet", "könyvvel", "könyvön",
    "város", "városban", "városból", "városra", "városról", "városnak", "várossá",
    "ember", "embernek", "emberrel", "embert", "emberként", "emberül", "emberré",
    # the front-vowel suffixes nltk omits, which is why this corpus exists
    "erdőből", "vízből", "tejből", "időtől", "mezőről", "kőről", "fűtől",
    # step 1 and step 5, the double consonant undoubled
    "vassal", "ésszel", "hússal", "kézzel", "tejjel", "lábbal", "nappal",
    "hússá", "vízzé", "kézzé", "tejjé", "kővé", "sóvá", "fává",
    # step 3, the special cases án / ánként / én
    "házán", "kertjén", "barátján", "házánként",
    # step 4, astul / estül / stul / stül / ástul / éstül
    "házastul", "kertestül", "családostul", "mindenestül",
    # steps 6 to 9, the possessive and plural families
    "házé", "házaké", "házéi", "kerté", "kertéi", "könyvé",
    "házam", "házad", "háza", "házunk", "házatok", "házuk",
    "kertem", "kerted", "kertje", "kertünk", "kertetek", "kertjük",
    "házaim", "házaid", "házai", "házaink", "házaitok", "házaik",
    "kertjeim", "kertjeid", "kertjei", "kertjeink", "kertjeitek", "kertjeik",
    "házak", "kertek", "könyvek", "városok", "emberek", "ablakok", "üstök",
    "almák", "körték", "fák", "nők", "kövek", "erdők", "idők", "tetők",
    # the two vowels nltk does not carry, in ordinary words
    "erdő", "idő", "mező", "első", "tető", "felhő", "szőlő", "szőlők",
    "nő", "kő", "tűz", "fű", "gyűrű", "gyűrűk", "tükör", "tükrök",
    "hő", "hőben", "bőr", "bőrök", "tő", "tövek", "sző", "nőtt",
    # digraphs, which the alphabet counts as single consonants
    "csoport", "csoportok", "gyerek", "gyerekek", "nyelv", "nyelvek",
    "szoba", "szobák", "zseb", "zsebek", "tyúk", "tyúkok", "lyuk", "lyukak",
    "dzsungel", "dzsungelek", "gyertya", "gyertyák",
    # verbs and their persons
    "olvas", "olvasok", "olvasunk", "olvasnak", "olvasott",
    "tanul", "tanulok", "tanulunk", "tanulnak", "tanult",
    "beszél", "beszélek", "beszélünk", "beszélnek", "beszélt",
    "ír", "írok", "írunk", "írnak", "írt", "lát", "látok", "látunk", "látják",
    # short and residual
    "és", "de", "hogy", "nem", "igen", "ez", "az", "itt", "ott", "már",
    "én", "te", "ő", "mi", "ti", "ők", "egy", "két", "három", "öt",
]


def _snowball_corpus(language: str, algorithm: str, words: list[str]) -> dict:
    """Freeze nltk's Snowball output for one language into an oracle payload.

    Each word list targets that language's own suffix families, plus short and
    irregular words that exercise its region (RV/R1/R2) boundaries.
    """
    from nltk.stem.snowball import SnowballStemmer  # noqa: PLC0415

    stemmer = SnowballStemmer(language)
    seen = set()
    unique = [w for w in words if not (w in seen or seen.add(w))]
    cases = [{"id": i, "word": w, "stem": stemmer.stem(w)} for i, w in enumerate(unique)]
    return {
        "metadata": {
            "algorithm": algorithm,
            "library": "nltk",
            "library_version": version("nltk"),
            "reference_calls": [f"nltk.stem.snowball.SnowballStemmer('{language}')"],
            "count": len(cases),
        },
        "cases": cases,
    }


# Arabic is oracled by snowballstemmer rather than nltk -- decision 0094: nltk's
# Arabic carries state between calls, which no thread-safe stemmer can reproduce.
SNOWBALL_AR_WORDS = [
    # normalisation, which the algorithm does before any stripping: the
    # vocalisation marks go, and so does the kasheeda that stretches a word
    "مُحَمَّد", "محمد", "طَيِّب", "طيب", "كِتَاب", "الْكِتَابُ", "مـــدرسة",
    # normalisation: the hamza forms and the alef madda fold onto a bare alef
    "إسلام", "اسلام", "أحمد", "احمد", "آمن", "امن", "أول", "اول",
    "مسؤول", "مسئول", "شيء", "سماء", "قرأ", "بدأ",
    # normalisation: alef maksura, teh marbuta, and the Arabic-Indic digits
    "على", "مصطفى", "ليلى", "مدرسة", "مدينة", "شجرة", "١٢٣", "٤٥٦",
    # the definite article, alone and behind a preposition or a conjunction
    "كتاب", "الكتاب", "بالكتاب", "كالكتاب", "للكتاب", "والكتاب", "فالكتاب",
    "المدرسة", "بالمدرسة", "للمدرسة",
    "بيت", "البيت", "بالبيت", "والبيت", "المدينة", "بالمدينة",
    # the conjunction prefixes on their own
    "وكتاب", "فكتاب", "وبيت", "فبيت", "ومدرسة", "وأحمد",
    # the possessive suffixes: -y -k -h -ha -hm -hn -km -kn -hma -kma -na
    "كتابي", "كتابك", "كتابه", "كتابها", "كتابهم", "كتابهن",
    "كتابكم", "كتابكن", "كتابهما", "كتابكما", "كتابنا",
    "بيتي", "بيتك", "بيته", "بيتها", "بيتهم", "بيتنا",
    "مدرستي", "مدرستك", "مدرسته", "مدرستها", "مدرستهم", "مدرستنا",
    # the sound plurals, and the feminine plural -at
    "كتب", "الكتب", "كاتبون", "كاتبين", "معلمون", "معلمين",
    "معلمات", "مدرسات", "طالبات", "مكتبات", "سيارات", "لغات",
    # the nisba adjective, which the last noun step removes
    "عربي", "عربية", "مصري", "مصرية", "علمي", "علمية", "وطني", "وطنية",
    # the verb prefixes: the imperfect markers and the future s-
    "يكتب", "تكتب", "نكتب", "أكتب", "سيكتب", "ستكتب", "سنكتب", "سأكتب",
    "يستخدم", "نستخدم", "تستخدم", "استخدم",
    # the verb suffixes: person, number and the object pronouns
    "كتبت", "كتبنا", "كتبوا", "كتبتم", "كتبتن", "كتبا", "كتبتا",
    "يكتبون", "يكتبان", "تكتبين", "تكتبون", "يكتبن",
    "كتبه", "كتبها", "كتبهم", "كتبني", "كتبك", "كتبكم",
    # words the length guards are supposed to protect: too short to strip
    "من", "في", "إلى", "عن", "هو", "هي", "هم", "لا", "ما",
    "يد", "دم", "أب", "أم", "ابن", "بنت", "علم", "قلم", "باب",
    # text the algorithm has no rule for, which must come back unchanged
    "hello", "café", "123", "abc",
]


def _snowball_reference_corpus(language: str, algorithm: str, words: list[str]) -> dict:
    """Freeze snowballstemmer's output for one language into an oracle payload.

    The Snowball project's own generated package, used where nltk's transcription
    of an algorithm is incomplete -- decision 0090 has the measurement that took
    Hungarian off nltk, and why no other language moved with it.
    """
    import snowballstemmer  # noqa: PLC0415

    stemmer = snowballstemmer.stemmer(language)
    seen = set()
    unique = [w for w in words if not (w in seen or seen.add(w))]
    cases = [{"id": i, "word": w, "stem": stemmer.stemWord(w)} for i, w in enumerate(unique)]
    return {
        "metadata": {
            "algorithm": algorithm,
            "library": "snowballstemmer",
            "library_version": version("snowballstemmer"),
            "reference_calls": [f"snowballstemmer.stemmer('{language}').stemWord(w)"],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_snowball_es() -> dict:
    return _snowball_corpus("spanish", "SpanishSnowballStemmer", SNOWBALL_ES_WORDS)


def generate_snowball_pt() -> dict:
    return _snowball_corpus("portuguese", "PortugueseSnowballStemmer", SNOWBALL_PT_WORDS)


def generate_snowball_it() -> dict:
    return _snowball_corpus("italian", "ItalianSnowballStemmer", SNOWBALL_IT_WORDS)


def generate_snowball_de() -> dict:
    return _snowball_corpus("german", "GermanSnowballStemmer", SNOWBALL_DE_WORDS)


def generate_snowball_nl() -> dict:
    return _snowball_corpus("dutch", "DutchSnowballStemmer", SNOWBALL_NL_WORDS)


SNOWBALL_FI_WORDS = [
    # step 1, particles: -kin -kaan -kään -ko -kö -han -hän -pa -pä, after n, t or a vowel
    "talokin", "kirjakin", "onkin", "kaikkikin", "sittenkin",
    "eikään", "mikään", "kukaan", "taloonkaan", "kenenkään",
    "menetkö", "tuletko", "onko", "eikö", "sinäkö",
    "tulehan", "otahan", "mikähän", "kukahan", "otapa", "menepä", "tulepa",
    # step 1, -sti, the one particle that has to sit in R2
    "nopeasti", "hitaasti", "kauniisti", "varmasti", "erityisesti",
    "helposti", "vaikeasti", "hyvästi", "kovasti",
    # step 2, the possessives: -si -ni -nsa -nsä -mme -nne
    "talosi", "kirjasi", "kotisi", "äitisi", "isäsi", "kätesi",
    "taloni", "kirjani", "kotini", "äitini", "käteni",
    "talonsa", "kirjansa", "kotinsa", "äitinsä", "kätensä", "lapsensa",
    "talomme", "kirjamme", "kotimme", "talonne", "kotinne", "lapsenne",
    # step 2, -an / -än / -en, each admitted only after its own case endings
    "talossaan", "talostaan", "talollaan", "taloltaan", "kotonaan",
    "kädessään", "kädestään", "kädellään", "kädeltään", "äitinään",
    "talolleen", "äidilleen", "kirjoineen", "lapsineen",
    # step 3, hXn: the vowel repeats across the h
    "maahan", "puuhun", "työhön", "tiehen", "päähän", "suuhun", "yöhön",
    # step 3, -siin -den -tten, admitted after Vi
    "vapaisiin", "korkeisiin", "maiden", "töiden", "teiden",
    "maitten", "töitten", "teitten", "taloihin",
    # step 3, -seen after a long vowel
    "vapaaseen", "huoneeseen", "maaseen",
    # step 3, the partitive -a / -ä after a consonant and a vowel
    "taloa", "kirjaa", "kissaa", "koiraa", "päätä", "kättä", "vettä",
    # step 3, the plain case endings
    "talossa", "talosta", "talolla", "talolta", "talolle", "talona",
    "taloksi", "taloine", "kädessä", "kädestä", "kädellä", "kädeltä",
    "kädelle", "kätenä", "kädeksi", "kaupungissa", "kaupungista",
    # step 3, -tta / -ttä, admitted only after e
    "huonetta", "perhettä", "aluetta", "osoitetta",
    # step 3, the genitive -n, and the long vowel or ie it uncovers
    "talon", "kirjan", "kissan", "koiran", "miehen", "kaupungin",
    "maan", "puun", "tien", "työn", "suun", "pään", "veden", "lapsen",
    # step 4, the comparative and superlative families, both in R2
    "isompi", "isompaa", "suurempi", "vanhempi", "nuorempi", "parempi",
    "kauniimpi", "kauniimpaa", "pienempi", "kylmempi",
    "vanhimpia", "isoimpia", "kauneimpia", "suurimmat", "vanhimmat",
    # step 5, the plurals: a final i or j after step 3, a final t otherwise
    "talot", "kirjat", "kissat", "miehet", "lapset", "kaupungit",
    "taloissa", "kirjoissa", "taloista", "kirjoista",
    # step 6, tidying: a long vowel, a consonant and one of a ä e i, -oj -uj -jo
    "vapaa", "korkea", "huone", "perhe", "alue", "kirje",
    "matkoja", "poikia", "taloja", "kirjoja",
    # the same rules again on longer stems, where the suffix reaches R1 or R2 --
    # a short word leaves several steps untested, which is what these are for
    "todellisesti", "yleisesti", "luonnollisesti", "yksinkertaisesti",
    "kotimaahan", "isänmaahan", "kirjoihin", "ihmisiin", "ystäviin",
    "taloiden", "kirjoiden", "asioiden", "taloitten", "kirjoitten",
    "taloon", "kirjaan", "kotiin", "kaupunkiin", "kaupunkien", "huoneen",
    "perheiden", "mielenkiintoisempi", "kansainvälisempi", "hyödyllisempi",
    "ymmärtäväisempi", "ihmiset", "opiskelijat", "kirjastot",
    # constructed forms pinning where a failed condition ends the search, from
    # both sides -- decision 0090; "ihmisiin" above is the one real word for it
    "ihmisden", "ihmistten", "ihmiseseen", "kotimaahon", "kissatta",
    # words that must come back whole, or nearly: too short, or nothing to strip
    "ja", "on", "ei", "se", "ne", "me", "te", "hän", "minä", "sinä",
    "kun", "niin", "mutta", "myös", "vain", "kuin",
    "a", "ab", "abc", "abcd",
]


SNOWBALL_NO_WORDS = [
    # step 1 (a): the definite, plural and genitive families
    "hus", HUSET, "husene", HUSETS, "huses",
    "bok", "boken", "bokens", "bøkene",
    "gutt", "gutten", "gutter", "guttene", "guttenes",
    "jente", "jenta", "jenter", "jentene",
    "barn", "barnet", "barna", "barnets",
    "dag", "dagen", "dager", "dagene", "dagens",
    "vei", "veien", "veier", "veiene",
    "lærer", "læreren", "lærere", "lærerne",
    "arbeider", "arbeidere", "arbeiderne",
    "regjering", "regjeringen", "regjeringer", "regjeringene",
    # step 1 (a): the -het family, including hetene / hetens / hetenes
    "mulighet", "muligheten", "muligheter", "mulighetene", "mulighetens", "mulighetenes",
    "sannhet", "sannheten", "sannheter", "sannhetene", "sannhetens",
    "frihet", "friheten", "friheter", "frihetene",
    "kjærlighet", "kjærligheten", "virkelighet", "virkeligheten",
    # step 1 (a): comparatives and superlatives
    "større", "størst", "største", "mindre", "minst",
    "raskere", "raskest", "raskeste", "sterkere", "sterkest",
    "vakrere", "vakrest", "billigere", "dårligere",
    # step 1 (a): the endings Bokmal produces rarely, which the algorithm carries anyway
    "elskede", "elskedes", "elskande", "dansande", "guttane", "bilane",
    "bilar", "dagar", "husas", "raskast", "lærers", "arbeiders",
    "kommendes", "sittendes", "arbeidendes",
    # step 1 (b): the bare s, and the k that is valid only after a non-vowel
    "folks", "boks", "bokser", "norsks", "fisks", "melks", "kalks", "sjokks",
    # step 1 (c): ert / erte rewritten to er
    "servert", "serverte", "sortert", "sorterte", "importert", "importerte",
    "studert", "studerte", "reservert", "konsentrert", "kontrollert",
    # step 2: a dt or vt in R1 loses the t
    "halvt", "levt", "godt", "bredt", "hardt", "kaldt", "rundt", "blindt", "vondt",
    # step 3: the -lig family and its neighbours
    "viktig", "viktige", "riktig", "ferdig", "mulig", "umulig",
    "naturlig", "naturlige", "lykkelig", "lykkelige", "tydelig", "farlig",
    "ærlig", "lovlig", "ulovlig", "forferdelig", "vanskelig", "alvorlig",
    "følelse", "følelsen", "følelser", "hendelse", "hendelsen", "bevegelse",
    "frelse", "frelsen", "tilgivelse",
    # step 3: lov / elov / slov / hetslov / leg / eleg / eig, the seven without a -lig
    "lov", "loven", "lover", "lovene", "kjærlighetslov", "hetslov",
    "grunnlov", "grunnloven", "straffelov", "avtalelov",
    "kollega", "kollegaer", "venleg", "fyrsteleg", "pizzadeig", "kjempedeig",
    # verbs and their participles
    "snakke", "snakker", "snakket", "snakkende", "snakkes",
    "lese", "leser", "leste", "lest",
    "skrive", "skriver", "skrevet", "kjøre", "kjører", "kjørte", "kjørt",
    "høre", "hører", "hørte", "hørt", "lære", "lærte", "lært",
    "bygge", "bygger", "bygde", "bygget", "kjenne", "kjenner", "kjente", "kjent",
    "elske", "elsker", "elsket", "elskende", "arbeide", "arbeidet", "arbeidende",
    "begynne", "begynner", "begynte", "begynt",
    # the letters æ, å and ø, which are letters here rather than accented forms
    "hånd", "hånden", "hender", "øye", "øyet", "øyne", "øynene",
    "år", "året", "årene", "årets", "dør", "døra", "døren", "dører",
    "sjø", "sjøen", "vår", "våren", "måned", "måneden",
    # short / residual
    "og", "i", "det", "at", "en", "et", "den", "til", "er", "som", "på", "de",
    "med", "av", "ikke", "var", "har", "kan", "vil", "ble",
]


SNOWBALL_RO_WORDS = [
    # step 0: the enclitic article and the plurals it attaches to
    "băiatul", "băiatului", "omul", "omului", "copilul", "copilului",
    "drumul", "drumului", "lucrul", "lucrului", "timpul", "timpului",
    "fata", "fetele", "fetelor", "casele", "caselor", "florile", "florilor",
    "cartea", "cărţile", "cărţilor", "familiei", "familiile", "familiilor",
    "steaua", "stelele", "cafeaua", "zilele", "zilelor",
    "băieţii", "copiii", "fiii", "ochii", "oamenii", "oamenilor",
    "elevilor", "studenţilor", "prietenii", "prietenilor",
    "informaţie", "naţie", "naţia", "relaţie", "relaţia",
    "abilitatei", "cetatei",
    # step 1: the combining derivational suffixes, which loop
    "abilitate", "abilităţi", "responsabilitate", "sensibilitate",
    "posibilitate", "imposibilitate", "stabilitate", "mobilitate",
    "activitate", "activităţi", "creativitate", "productivitate",
    "obiectivitate", "electricitate", "publicitate", "simplicitate",
    "autenticitate", "capacitate", "educativ", "educativă", "educative",
    "informativ", "informativă", "decorativ", "operativ", "operativă",
    "creator", "creatori", "creatoare", "cititor", "cititori", "cititoare",
    "muncitor", "muncitori", "muncitoare", "vânzător", "vânzători",
    "vânzătoare", "învăţător", "învăţătoare", "conducător", "conducători",
    "definitiv", "definitivă", "primitiv", "primitivă", "pozitiv", "pozitivă",
    "negativ", "negativă", "administraţiune", "poziţiune",
    "comunicativ", "clasical", "clasicale", "practicală",
    # step 2: the standard suffixes, all of them measured against R2
    "naţiune", "naţiuni", "raţiune", "raţiuni", "lecţiune",
    "comunism", "comunisme", "comunist", "comunista", "comuniste",
    "comunişti", "comunistă", "artist", "artista", "artiste", "artişti",
    "jurnalist", "jurnalişti", "realism", "realist", "realişti",
    "frumoasa", "frumoasă", "frumoase", "credincioasă", "bucuroasă",
    "frumos", "bucuros", "bucuroşi", "curajos", "curajoşi",
    "important", "importanta", "importante", "importanţi", "importantă",
    "elegant", "eleganta", "elegante", "elegantă",
    "cântat", "cântată", "cântaţi", "cântate", "lucrat", "lucrată",
    "născut", "născută", "născuţi", "născute", "cunoscut", "cunoscută",
    "citit", "citită", "cititi", "citite", "dormit", "dormită",
    "politic", "politica", "politice", "politici", "politică",
    "istoric", "istorica", "istorice", "istorici", "istorică",
    "public", "publica", "publice", "publici", "publică",
    "activa", "active", "activi", "activă", "pasiv", "pasivă",
    "capabil", "capabile", "capabili", "capabilă",
    "posibil", "posibile", "posibili", "posibilă",
    "vizibil", "vizibilă", "flexibil", "flexibilă",
    # step 3: the verb endings, the largest table and the only one with a
    # condition on the letter before the suffix
    "cânta", "cântare", "cântam", "cântai", "cântau", "cântăm",
    "cântase", "cântasem", "cântaseşi", "cântaseră", "cântaserăm",
    "cântaserăţi", "cântară", "cântarăm", "cântarăţi", "cântaşi",
    "cântând", "cântându", "cântează", "cântezi", "cânteze",
    "vorbeşte", "vorbeşti", "vorbesc", "vorbeam", "vorbeai", "vorbeau",
    "vorbeaţi", "vorbind", "vorbire", "vorbiră", "vorbirăm",
    "citeşte", "citeşti", "citesc", "citeaţi", "citind", "citire",
    "citiră", "citirăm", "citirăţi", "citiseşi", "citiseră", "citiserăm",
    "dormim", "dormiţi", "dormind", "dormire", "dormiră", "dormeam",
    "iubesc", "iubeşti", "iubeşte", "iubim", "iubiţi", "iubind", "iubire",
    "hotărăsc", "hotărăşte", "hotărăşti", "vorbiţi", "vorbim",
    "coborâm", "coborâţi", "coborând", "coborâre", "coborâră", "coborâşi",
    "coborâsem", "coborâse", "hotărâm", "hotărâţi", "hotărând",
    # step 4: the final vowel, and words too short for RV to reach
    "carte", "cărţi", "floare", "flori", "masă", "mese", "casă", "case",
    "apă", "ape", "ţară", "ţări", "şcoală", "şcoli", "viaţă", "vieţi",
    "mare", "mari", "bine", "rău", "nou", "noi", "vechi", "greu", "uşor",
    # i and u between vowels, which the algorithm marks before the rules
    "aceia", "aceea", "băiat", "băieţi", "ziua", "zeii", "voiau", "beau",
    "continuare", "continuu", "individual", "actual", "actuală",
    "ploaie", "ploaia", "femeie", "femeia", "femei", "cheie", "cheia",
    # the two Unicode spellings of the same two letters, cedilla and comma
    "ştiinţă", "știință", "ştiinţe", "științe", "mulţumesc", "mulțumesc",
    "aceştia", "aceștia", "româneşte", "românește", "naţiunea", "națiunea",
    "informaţia", "informația", "frumoşi", "frumoși", "greşeală", "greșeală",
    # short words and words with nothing to strip
    "om", "an", "zi", "el", "ea", "eu", "voi", "şi", "și", "cu",
    "de", "la", "un", "o", "nu", "da", "mai", "sau", "dar",
]


def generate_snowball_sv() -> dict:
    return _snowball_corpus("swedish", "SwedishSnowballStemmer", SNOWBALL_SV_WORDS)


def generate_snowball_ru() -> dict:
    return _snowball_corpus("russian", "RussianSnowballStemmer", SNOWBALL_RU_WORDS)


def generate_snowball_da() -> dict:
    return _snowball_corpus("danish", "DanishSnowballStemmer", SNOWBALL_DA_WORDS)
def generate_snowball_no() -> dict:
    return _snowball_corpus("norwegian", "NorwegianSnowballStemmer", SNOWBALL_NO_WORDS)


def generate_snowball_fi() -> dict:
    return _snowball_corpus("finnish", "FinnishSnowballStemmer", SNOWBALL_FI_WORDS)
def generate_snowball_hu() -> dict:
    return _snowball_reference_corpus("hungarian", "HungarianSnowballStemmer", SNOWBALL_HU_WORDS)
def generate_snowball_ro() -> dict:
    return _snowball_corpus("romanian", "RomanianSnowballStemmer", SNOWBALL_RO_WORDS)


def generate_snowball_ar() -> dict:
    return _snowball_reference_corpus("arabic", "ArabicSnowballStemmer", SNOWBALL_AR_WORDS)


WORDPIECE_VOCAB = [
    UNK_TOKEN, "the", "cat", "dog", "play", "un", "love", "run", "quick", "brown",
    "fox", "jump", "hello", "world", "token", "embed", "semantic", "search", "is",
    "are", "and", "this", "big", "a", ".", "!", "?",
    "##s", "##ing", "##ed", "##er", "##aff", "##able", "##ly", "##ner", "##ning",
    "##ization", "##ize", "##ding", "##dings", "##ger", "##gest", "##a", "##b", "##c",
]


# Issue #887: what Oniguruma's \w holds and .NET's does not, plus an astral So and a No that split.
# Appended after the two Whitespace() corpora's own texts so no earlier case id moves.
WHITESPACE_PRE_TOKENIZER_TEXTS = [
    "\u0939\u093f\u0902\u0926\u0940 the",
    "the\u093fcat",
    "the\u20ddcat",
    "\u216b the \u216bcat",
    "cat\u200ddog cat\u200cdog",
    "the\u24b6cat \u24b6",
    "the\U00020000cat \U00020000\U0002a6d6",
    "the\U0001f600cat \U0001f600!",
    "the\u00b2 cat",
]

WORDPIECE_TEXTS = [
    "the cats playing",
    "unaffable",
    "unknownxyz",
    "quick brown fox jumps.",
    EMBEDDING_SENTENCE,
    "hello world!",
    "the dog runs and the cat plays",
    "bigger biggest",
    "lovely love loved lover",
]


def generate_wordpiece() -> dict:
    from tokenizers import Tokenizer  # noqa: PLC0415
    from tokenizers.models import WordPiece
    from tokenizers.pre_tokenizers import Whitespace

    vocab = {tok: i for i, tok in enumerate(WORDPIECE_VOCAB)}
    wp = WordPiece(vocab, unk_token=UNK_TOKEN, max_input_chars_per_word=100)
    tokenizer = Tokenizer(wp)
    tokenizer.pre_tokenizer = Whitespace()

    cases = []
    for i, text in enumerate(WORDPIECE_TEXTS + WHITESPACE_PRE_TOKENIZER_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({"id": i, "text": text, "tokens": enc.tokens, "ids": enc.ids})
    return {
        "metadata": {
            "algorithm": "WordPiece",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "vocab": vocab,
            "unk_token": UNK_TOKEN,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_pooling() -> dict:
    rng = SeededRandom(SEED)
    cases = []
    for cid, (seq, dim) in enumerate([(4, 6), (5, 8), (3, 4), (6, 5)]):
        emb = [[rng.uniform(-1, 1) for _ in range(dim)] for _ in range(seq)]
        mask = [1 if rng.random() < 0.7 or t == 0 else 0 for t in range(seq)]
        active = sum(mask) or 1
        pooled = [sum(emb[t][d] for t in range(seq) if mask[t]) / active for d in range(dim)]
        norm = sum(v * v for v in pooled) ** 0.5
        normalized = [v / norm for v in pooled] if norm > 0 else pooled
        cases.append({
            "id": cid, "seq": seq, "dim": dim,
            "embeddings": emb, "mask": mask, "pooled_normalized": normalized,
        })
    return {
        "metadata": {
            "algorithm": "MeanPooling",
            "library": "reference",
            "reference_calls": ["mean pool with attention mask + L2 normalize (sentence-transformers recipe)"],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_knn() -> dict:
    import numpy as np  # noqa: PLC0415

    rng = SeededRandom(SEED)
    n_items, dim, n_queries, k = 60, 16, 6, 5
    corpus = [[rng.uniform(-1, 1) for _ in range(dim)] for _ in range(n_items)]
    queries = [[rng.uniform(-1, 1) for _ in range(dim)] for _ in range(n_queries)]

    c = np.array(corpus, dtype=np.float64)
    c = c / np.linalg.norm(c, axis=1, keepdims=True)
    cases = []
    for i, raw in enumerate(queries):
        q = np.array(raw, dtype=np.float64)
        q = q / np.linalg.norm(q)
        sims = c @ q
        order = np.argsort(-sims, kind="stable")[:k]
        results = [{"index": int(j), "score": stable(sims[j])} for j in order]
        cases.append({"id": i, "query": raw, "k": k, "results": results})

    return {
        "metadata": {
            "algorithm": "CosineKnn",
            "library": "numpy",
            "library_version": version("numpy"),
            "reference_calls": ["brute-force cosine similarity + argsort"],
            "dim": dim,
            "count": len(cases),
        },
        "corpus": corpus,
        "cases": cases,
    }


FUZZ_PAIRS = [
    ("fuzzy wuzzy was a bear", "wuzzy fuzzy was a bear"),
    (METS, METS),
    (METS, "the wonderful new york mets"),
    ("mariners vs angels", "los angeles angels of anaheim at seattle mariners"),
    (HELLO_WORLD, "world hello"),
    ("a", "ab"), ("", ""), ("abc", "abcd"),
    ("Hello", "hello"), ("New York!", "york new"),
    (QUICK_FOX, "the brown quick fox"),
    ("apple", "apple pie"), ("apple pie", "apple"),
    ("data science", "science of data"), ("machine learning", "learning machine models"),
    ("kitten", "sitting"), ("levenshtein", "levenstein"),
    ("this is a test", "this is a test!"),
    ("one two three four", "four three two one"),
    ("café", "cafe"), (NAIVE, "naive"),
    ("abcdefgh", "abcdefgh"), ("abcdefgh", "hgfedcba"),
    ("python programming", "programming in python"),
    (THE_CAT, "cat"), ("supercalifragilistic", "super"),
    ("john smith", "smith, john"), ("jonathan", "john"),
    ("123 main st", "123 main street"), ("dr smith", "doctor smith"),
    # One side with no words: rapidfuzz scores the token-set ratios 0, not the 100 a prefix gives.
    ("", "alpha beta"), ("alpha beta", ""), (" ", "a"), ("a", " "), (" ", " "), (" \t ", "x y"),
]


# Issue #892: where code points, rapidfuzz's whitespace and a code-point token sort part from UTF-16.
# Replayed with TextElement.CodePoint only; appended so no earlier case id moves.
FUZZ_CODE_POINT_PAIRS = [
    ("\U0001f600", "\U0001f601"),
    ("\uffff a", "\U0001f600 a"),
    ("na\u00efve \U0001f600 caf\u00e9", "caf\u00e9 \U0001f600"),
    ("\U0001f600\U0001f601 hello", "hello \U0001f601"),
    ("a\u001cb", "b a"), ("a\u00a0b", "b a"), ("a\u0085b", "b a"), ("a\u3000b", "b a"),
    ("\U00020000\U00020001\U00020002 x", "x \U00020001"),
    # Past 64 code points, where PartialRatio takes the long-needle path.
    ("\U0001f600" * 70 + "x", "\U0001f600" * 66),
    ("q" + "\U0001f600" * 80, "\U0001f600" * 30 + "q"),
]


def generate_fuzz() -> dict:
    from rapidfuzz import fuzz  # noqa: PLC0415

    cases = []
    pairs = [(a, b, False) for a, b in FUZZ_PAIRS] + [(a, b, True) for a, b in FUZZ_CODE_POINT_PAIRS]
    for i, (a, b, code_point_only) in enumerate(pairs):
        cases.append({
            "id": i, "a": a, "b": b, "codePointOnly": code_point_only,
            "ratio": fuzz.ratio(a, b),
            "partial_ratio": fuzz.partial_ratio(a, b),
            "token_sort_ratio": fuzz.token_sort_ratio(a, b),
            "token_set_ratio": fuzz.token_set_ratio(a, b),
            "wratio": fuzz.WRatio(a, b),
            "partial_token_sort_ratio": fuzz.partial_token_sort_ratio(a, b),
            "partial_token_set_ratio": fuzz.partial_token_set_ratio(a, b),
        })
    return {
        "metadata": {
            "algorithm": "Fuzz",
            "library": "rapidfuzz",
            "library_version": version("rapidfuzz"),
            "reference_calls": [
                "rapidfuzz.fuzz.{ratio,partial_ratio,token_sort_ratio,token_set_ratio,WRatio,"
                "partial_token_sort_ratio,partial_token_set_ratio}",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


PROCESS_CHOICES = [
    METS, "new york yankees", "boston red sox", "atlanta braves",
    "new york knicks", "brooklyn nets", "los angeles lakers", "chicago bulls",
]
PROCESS_CASES = [
    {"query": "new york", "limit": 5, "cutoff": 0.0},
    {"query": "new york", "limit": 3, "cutoff": 0.0},
    {"query": METS, "limit": 5, "cutoff": 80.0},
    {"query": "brooklyn", "limit": 2, "cutoff": 0.0},
    {"query": "lakers", "limit": 5, "cutoff": 50.0},
    # A blank query as long as METS: its token-set ratio used to score 100 and put METS first (#860).
    {"query": " " * len(METS), "limit": 3, "cutoff": 0.0},
    {"query": "brooklyn", "limit": 0, "cutoff": 0.0},
    {"query": "zzz", "limit": 5, "cutoff": 90.0},
]


def _process_hit(choice: str, score: float, index: int) -> dict:
    return {"choice": choice, "score": score, "index": index}


def generate_process() -> dict:
    from rapidfuzz import process  # noqa: PLC0415

    cases = []
    for i, case in enumerate(PROCESS_CASES):
        res = process.extract(case["query"], PROCESS_CHOICES, limit=case["limit"], score_cutoff=case["cutoff"])
        one = process.extractOne(case["query"], PROCESS_CHOICES, score_cutoff=case["cutoff"])
        cases.append({
            "id": i, "query": case["query"], "limit": case["limit"], "cutoff": case["cutoff"],
            "results": [_process_hit(*hit) for hit in res],
            "extract_one": None if one is None else _process_hit(*one),
        })
    return {
        "metadata": {
            "algorithm": "Process",
            "library": "rapidfuzz",
            "library_version": version("rapidfuzz"),
            "reference_calls": ["rapidfuzz.process.{extract,extractOne} (default scorer WRatio)"],
            "choices": PROCESS_CHOICES,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_sentencepiece() -> dict:
    import sentencepiece as spm  # noqa: PLC0415

    sp = spm.SentencePieceProcessor(model_file=str(ORACLE_DIR / TINY_SP_MODEL))
    vocab = [{"piece": sp.id_to_piece(i), "score": sp.get_score(i), "id": i} for i in range(sp.get_piece_size())]
    texts = [
        QUICK_FOX, "tokenization", HELLO_WORLD,
        "machine learning and data science", CAT_SENTENCE,
        "natural language processing", "xyzabc", "a b c",
        "unigram models find the best segmentation", "programming",
    ]
    cases = [
        {"id": k, "text": t, "pieces": sp.encode(t, out_type=str), "ids": sp.encode(t, out_type=int)}
        for k, t in enumerate(texts)
    ]
    return {
        "metadata": {
            "algorithm": "SentencePiece",
            "library": "sentencepiece",
            "library_version": version("sentencepiece"),
            "model": "tiny_sp.model (self-trained unigram, identity normalizer)",
            "unk_id": sp.unk_id(),
            "vocab": vocab,
            "count": len(cases),
        },
        "cases": cases,
    }


LOADER_TEXTS = [
    QUICK_FOX, "tokenization", HELLO_WORLD,
    "machine learning and data science", CAT_SENTENCE,
    "natural language processing", "xyzabc", "a b c",
    "unigram models find the best segmentation", "programming",
]


def _wordpiece_tokenizer(vocab: dict[str, int], lowercase: bool):
    """A HuggingFace WordPiece tokenizer with the pipeline Lodestar reproduces."""
    from tokenizers import Tokenizer  # noqa: PLC0415
    from tokenizers.models import WordPiece  # noqa: PLC0415
    from tokenizers.normalizers import Lowercase  # noqa: PLC0415
    from tokenizers.pre_tokenizers import Whitespace  # noqa: PLC0415

    tokenizer = Tokenizer(WordPiece(vocab, unk_token=UNK_TOKEN, max_input_chars_per_word=100))
    tokenizer.pre_tokenizer = Whitespace()
    if lowercase:
        tokenizer.normalizer = Lowercase()
    return tokenizer


def generate_vocab_txt() -> dict:
    """Freeze a vocab.txt and what transformers' loader makes of it.

    The file content is embedded so the C# side replays the exact bytes rather
    than a second fixture that could drift away from this one.
    """
    tokens = list(WORDPIECE_VOCAB)
    # transformers reads the file in text mode and does token.rstrip("\n"); the
    # trailing newline of the last line therefore adds no entry.
    content = "".join(f"{token}\n" for token in tokens)
    vocab = {token: index for index, token in enumerate(tokens)}

    tokenizer = _wordpiece_tokenizer(vocab, lowercase=False)
    cases = []
    for i, text in enumerate(WORDPIECE_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({"id": i, "text": text, "tokens": enc.tokens, "ids": enc.ids})

    return {
        "metadata": {
            "algorithm": "VocabTxtLoader",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "reference_calls": [
                "transformers.BertTokenizer vocab.txt loading: rstrip('\\n') then vocab[token] = index",
                "tokenizers.Tokenizer(WordPiece(vocab, unk_token)).encode",
            ],
            "vocab_txt": content,
            "vocab": vocab,
            "unk_token": UNK_TOKEN,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_tokenizer_json() -> dict:
    """Freeze two tokenizer.json documents — WordPiece and Unigram — and their encodings."""
    import json as _json  # noqa: PLC0415
    from tokenizers import Tokenizer  # noqa: PLC0415
    from tokenizers.models import Unigram  # noqa: PLC0415
    from tokenizers.pre_tokenizers import Metaspace  # noqa: PLC0415
    from sentencepiece import sentencepiece_model_pb2 as model_pb2  # noqa: PLC0415

    wordpiece_vocab = {token: index for index, token in enumerate(WORDPIECE_VOCAB)}
    wordpiece = _wordpiece_tokenizer(wordpiece_vocab, lowercase=True)
    wordpiece_cases = []
    for i, text in enumerate(WORDPIECE_TEXTS):
        enc = wordpiece.encode(text)
        wordpiece_cases.append({"id": i, "model": "WordPiece", "text": text, "tokens": enc.tokens, "ids": enc.ids})

    proto = model_pb2.ModelProto()
    proto.ParseFromString((ORACLE_DIR / TINY_SP_MODEL).read_bytes())
    unigram_pieces = [(p.piece, p.score) for p in proto.pieces]
    unigram = Tokenizer(Unigram(unigram_pieces, unk_id=proto.trainer_spec.unk_id, byte_fallback=False))
    unigram.pre_tokenizer = Metaspace()
    unigram.add_special_tokens([UNK_TOKEN_LOWER, BOS_TOKEN, "</s>"])
    unigram_cases = []
    for i, text in enumerate(LOADER_TEXTS):
        enc = unigram.encode(text)
        unigram_cases.append({"id": i, "model": "Unigram", "text": text, "tokens": enc.tokens, "ids": enc.ids})

    return {
        "metadata": {
            "algorithm": "TokenizerJsonLoader",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "reference_calls": [
                "tokenizers.Tokenizer.from_file('tokenizer.json') then .encode",
            ],
            "wordpiece_tokenizer_json": _json.loads(wordpiece.to_str()),
            "wordpiece_vocab": wordpiece_vocab,
            "wordpiece_unk_token": UNK_TOKEN,
            "wordpiece_lowercase": True,
            "unigram_tokenizer_json": _json.loads(unigram.to_str()),
            "unigram_unk_id": proto.trainer_spec.unk_id,
            "count": len(wordpiece_cases) + len(unigram_cases),
        },
        "cases": wordpiece_cases + unigram_cases,
    }


def generate_spiece_model() -> dict:
    """Freeze what sentencepiece's own parser reads out of tests/oracles/tiny_sp.model.

    Piece *types* come from the protobuf rather than from the IsControl/IsUnknown
    helpers: the proto is the format Lodestar's loader claims to read, so it is the
    right reference for it.
    """
    import sentencepiece as spm  # noqa: PLC0415
    from sentencepiece import sentencepiece_model_pb2 as model_pb2  # noqa: PLC0415

    proto = model_pb2.ModelProto()
    proto.ParseFromString((ORACLE_DIR / TINY_SP_MODEL).read_bytes())
    pieces = [
        {"piece": p.piece, "score": p.score, "type": int(p.type), "id": i}
        for i, p in enumerate(proto.pieces)
    ]

    sp = spm.SentencePieceProcessor(model_file=str(ORACLE_DIR / TINY_SP_MODEL))
    cases = [
        {"id": k, "text": t, "pieces": sp.encode(t, out_type=str), "ids": sp.encode(t, out_type=int)}
        for k, t in enumerate(LOADER_TEXTS)
    ]

    return {
        "metadata": {
            "algorithm": "SentencePieceModelLoader",
            "library": "sentencepiece",
            "library_version": version("sentencepiece"),
            "model": "tiny_sp.model (self-trained unigram, identity normalizer)",
            "reference_calls": [
                "sentencepiece_model_pb2.ModelProto().ParseFromString(open('spiece.model','rb').read())",
                "sentencepiece.SentencePieceProcessor(model_file=…).encode",
            ],
            "normalizer_name": proto.normalizer_spec.name,
            "add_dummy_prefix": proto.normalizer_spec.add_dummy_prefix,
            "remove_extra_whitespaces": proto.normalizer_spec.remove_extra_whitespaces,
            "escape_whitespaces": proto.normalizer_spec.escape_whitespaces,
            "unk_id": proto.trainer_spec.unk_id,
            "bos_id": proto.trainer_spec.bos_id,
            "eos_id": proto.trainer_spec.eos_id,
            "pad_id": proto.trainer_spec.pad_id,
            "pieces": pieces,
            "count": len(cases),
        },
        "cases": cases,
    }


# Names the markers literally, plus ordinary multilingual text; see
# generate_xlmr_fairseq's docstring.
XLMR_TEXTS = [
    "le renard brun rapide saute par-dessus le chien paresseux",
    "el zorro marron rapido salta sobre el perro perezoso",
    "der schnelle braune Fuchs springt uber den faulen Hund",
    "быстрая коричневая лиса прыгает через ленивую собаку",
    "速い茶色のキツネが怠け者の犬を飛び越える",
    "a <unk> b",
    "le chat <mask> sur le tapis",
    "<s> hello </s>",
    "<pad><pad> padding",
    MASK_TOKEN,
    "un texte avec <s>, </s>, <pad>, <unk> et <mask> dedans",
    # Since #75 these are rewritten by XLM-R's own nmt_nfkc charsmap before
    # segmentation; escaped so no editor can normalise them by accident.
    "\uff2c\uff25 \uff32\uff25\uff2e\uff21\uff32\uff24 \uff52\uff41\uff50\uff49\uff44\uff45",  # full-width LE RENARD rapide
    "\ufb01nancier, \ufb02amme et \u0153uvre",  # fi and fl ligatures
    "cafe\u0301 de\u0301ja\u0300 vu",  # decomposed accents, which nmt_nfkc recomposes
    "\u2168 siecles, \u2460\u2461\u2462 etapes",  # roman numeral IX, circled digits
    "espace\u00a0insecable et espace\u3000ideographique",
    "un\u0001texte\u0002avec\u0007des controles",
]

# The five strings a vocabulary in this layout must never segment onto.
XLMR_MARKERS = [BOS_TOKEN, "<pad>", "</s>", UNK_TOKEN_LOWER, MASK_TOKEN]


def generate_xlmr_fairseq() -> dict:
    """Freeze sentencepiece's encoding of the XLM-R vocabulary in fairseq layout.

    The fixture is built by tools/fetch_xlmr_vocab.py: XLM-R's own 250 000
    pieces and scores, at the ids HuggingFace gives them, with <s>=0, <pad>=1,
    </s>=2, <unk>=3 and <mask>=250001 typed CONTROL/UNKNOWN, and the normalizer
    set to identity — the pipeline Lodestar reproduces. See that script for why
    the stock sentencepiece.bpe.model cannot be replayed directly.

    This is the corpus the id-based control filter could not have passed: every
    marker sits outside 0-2 except <s>, and <mask> sits 250 000 ids away from
    where the guess looked.

    XLMR_TEXTS names the marker strings literally -- a piece only ever matches
    where its literal characters occur, so an input without "<" in it cannot tell
    a tokenizer that excludes the control pieces from one that does not -- plus
    ordinary multilingual text over XLM-R's own vocabulary, so a fixture that only
    ever saw Latin script does not leave most of it unexercised.
    """
    import sentencepiece as spm  # noqa: PLC0415
    from sentencepiece import sentencepiece_model_pb2 as model_pb2  # noqa: PLC0415

    path = ORACLE_DIR / XLMR_FAIRSEQ_MODEL
    proto = model_pb2.ModelProto()
    proto.ParseFromString(path.read_bytes())
    sp = spm.SentencePieceProcessor(model_file=str(path))

    markers = [
        {
            "piece": piece,
            "id": next(i for i, p in enumerate(proto.pieces) if p.piece == piece),
            "type": int(next(p.type for p in proto.pieces if p.piece == piece)),
        }
        for piece in XLMR_MARKERS
    ]
    # Spot-checked rather than all 250 002: the vocabulary is the committed
    # .model, and repeating it as JSON would double a 5 MB fixture for nothing.
    sampled = [
        {"id": i, "piece": sp.id_to_piece(i), "score": sp.get_score(i), "type": int(proto.pieces[i].type)}
        for i in (0, 1, 2, 3, 4, 5, 1000, 100_000, 250_000, 250_001)
    ]
    cases = [
        {"id": k, "text": t, "pieces": sp.encode(t, out_type=str), "ids": sp.encode(t, out_type=int)}
        for k, t in enumerate(XLMR_TEXTS)
    ]

    return {
        "metadata": {
            "algorithm": "SentencePieceTokenizer",
            "library": "sentencepiece",
            "library_version": version("sentencepiece"),
            "model": (
                "xlmr_fairseq.model (xlm-roberta-base vocabulary, fairseq layout, "
                "identity normalizer — see tools/fetch_xlmr_vocab.py)"
            ),
            "reference_calls": [
                "sentencepiece.SentencePieceProcessor(model_file='xlmr_fairseq.model').encode",
            ],
            "normalizer_name": proto.normalizer_spec.name,
            "vocab_size": len(proto.pieces),
            "unk_id": proto.trainer_spec.unk_id,
            "bos_id": proto.trainer_spec.bos_id,
            "eos_id": proto.trainer_spec.eos_id,
            "pad_id": proto.trainer_spec.pad_id,
            "markers": markers,
            "sampled_pieces": sampled,
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Batch encoding and batched embedding (issue #60) --------------------------
# See generate_batch_encoding's docstring for why the chain is frozen in two halves.

# Appended after the WordPiece vocabulary; see _batch_tokenizer's docstring for why.
CLS_TOKEN = "[CLS]"
SEP_TOKEN = "[SEP]"
PAD_TOKEN = "[PAD]"
BATCH_VOCAB = [*WORDPIECE_VOCAB, CLS_TOKEN, SEP_TOKEN, PAD_TOKEN]

# Mirrors tools/build_tiny_models.py; see _batch_embedding_table's docstring for
# why it is duplicated rather than imported.
EMBEDDING_ROWS = 64
EMBEDDING_DIM = 4

BATCH_MAX_LENGTH = 8

# The four documented edges (see _assert_batch_edges) fall out of one batch
# under this limit.
BATCH_EDGE_TEXTS = [
    "",
    "the",
    "quick brown fox jumps.",
    EMBEDDING_SENTENCE,
]

BATCH_MIXED_TEXTS = [
    "hello world!",
    "the",
    "the dog runs and the cat plays",
    "unaffable",
    "the cats playing",
    EMBEDDING_SENTENCE,
    "",
    "lovely love loved lover",
]

BATCH_UNKNOWN_TEXTS = [
    "unknownxyz",
    "the unknownxyz cat",
    "zzz qqq",
]


def _batch_embedding_table():
    """The synthetic embedding matrix `tiny_embedder.onnx` gathers from.

    Every entry is a multiple of 1/64 with magnitude below 1/2, so a sum of a few
    dozen rows is exact in float32 and the only inexactness in the whole pipeline
    is the final division and the normalization.

    EMBEDDING_ROWS/EMBEDDING_DIM mirror tools/build_tiny_models.py, duplicated
    rather than imported: that script runs in a virtualenv carrying `onnx`, this
    one in a virtualenv carrying scikit-learn, and neither has the other's
    dependency. The table is frozen into the corpus and a C# test gathers a row
    through the ONNX model and compares it, so the two copies cannot drift apart
    in silence.
    """
    import numpy as np  # noqa: PLC0415

    table = np.zeros((EMBEDDING_ROWS, EMBEDDING_DIM), dtype=np.float64)
    for i in range(EMBEDDING_ROWS):
        for d in range(EMBEDDING_DIM):
            table[i, d] = (((7 * i + 13 * d) % 64) - 32) / 64.0
    return table


def _batch_tokenizer(vocab: dict[str, int], max_length: int | None):
    """A HuggingFace tokenizer configured the way `BatchEncoder` configures itself.

    CLS_TOKEN/SEP_TOKEN/PAD_TOKEN are appended after the WordPiece vocabulary
    rather than placed at the front, where BERT keeps them: nothing may assume
    [CLS] is id 101, or id 0, or that the special tokens are contiguous with each
    other -- the template names a token and the vocabulary is what answers with
    an id.
    """
    from tokenizers.processors import TemplateProcessing  # noqa: PLC0415

    tokenizer = _wordpiece_tokenizer(vocab, lowercase=False)
    tokenizer.post_processor = TemplateProcessing(
        single=f"{CLS_TOKEN} $A {SEP_TOKEN}",
        special_tokens=[(CLS_TOKEN, vocab[CLS_TOKEN]), (SEP_TOKEN, vocab[SEP_TOKEN])],
    )
    # padding="longest": to the longest row of this batch, never to max_length.
    tokenizer.enable_padding(pad_id=vocab[PAD_TOKEN], pad_token=PAD_TOKEN)
    if max_length is None:
        tokenizer.no_truncation()
    else:
        tokenizer.enable_truncation(max_length=max_length)
    return tokenizer


def _batch_reference(ids, mask, table):
    """Mean-pool the gathered rows behind the mask, then L2-normalize.

    The sentence-transformers recipe, in float64: `sum(E[ids] * mask) /
    clamp(sum(mask), min=1e-9)`, scaled to unit length.
    """
    import numpy as np  # noqa: PLC0415

    gathered = table[np.array(ids, dtype=np.int64)]
    weights = np.array(mask, dtype=np.float64)[:, :, None]
    active = np.maximum(np.array(mask, dtype=np.float64).sum(axis=1), 1e-9)[:, None]
    pooled = (gathered * weights).sum(axis=1) / active
    norm = np.sqrt((pooled * pooled).sum(axis=1))[:, None]
    normalized = np.divide(pooled, norm, out=pooled.copy(), where=norm > 0)
    return pooled.tolist(), normalized.tolist()


def _batch_case(cid: int, name: str, texts: list[str], vocab: dict[str, int],
                max_length: int | None, table) -> dict:
    encodings = _batch_tokenizer(vocab, max_length).encode_batch(texts)
    ids = [enc.ids for enc in encodings]
    mask = [enc.attention_mask for enc in encodings]
    pooled, normalized = _batch_reference(ids, mask, table)
    return {
        "id": cid,
        "name": name,
        "texts": texts,
        "max_length": max_length,
        "sequence_length": len(ids[0]),
        "input_ids": ids,
        "attention_mask": mask,
        "pooled": pooled,
        "pooled_normalized": normalized,
    }


def _assert_batch_edges(case: dict) -> None:
    """Fail generation if the edge fixture has stopped exercising its four edges.

    BATCH_EDGE_TEXTS is chosen so the four edges checked below -- nothing, one
    token, exactly BATCH_MAX_LENGTH, one over it -- fall out of the same batch.
    A vocabulary or template change can leave these texts encoding to lengths
    that no longer straddle the limit. The test replaying them would still pass,
    having quietly become a test of nothing, which is why each edge is asserted
    here rather than left to this docstring.
    """
    lengths = [sum(row) for row in case["attention_mask"]]
    limit = case["max_length"]
    template_tokens = 2  # [CLS] and [SEP]
    if lengths[0] != template_tokens:
        raise AssertionError(f"the empty text should encode to the template alone, got {lengths[0]}")
    if lengths[1] != template_tokens + 1:
        raise AssertionError(f"'the' should encode to one token plus the template, got {lengths[1]}")
    if lengths[2] != limit:
        raise AssertionError(f"the exactly-at-the-limit text encodes to {lengths[2]}, not {limit}")
    if lengths[3] != limit:
        raise AssertionError(f"the over-the-limit text should truncate to {limit}, got {lengths[3]}")
    untruncated = _batch_tokenizer(
        {tok: i for i, tok in enumerate(BATCH_VOCAB)}, None).encode(case["texts"][3])
    if len(untruncated.ids) != limit + 1:
        raise AssertionError(
            f"the over-the-limit text encodes to {len(untruncated.ids)} tokens; "
            f"it must be exactly one over {limit} for the fixture to test the boundary")


def generate_batch_encoding() -> dict:
    """Freeze `tokenize -> insert specials -> truncate -> pad -> infer -> pool`, in two halves.

    Tokenization is integers taken from HuggingFace `tokenizers` with the
    post-processor and padding enabled -- the library the C# reproduces -- and
    replayed for exact equality: an id is right or it is not, and a tolerance
    would only hide an off-by-one in the template.

    The embedding half is float64 arithmetic over the same table
    `tools/build_tiny_models.py` bakes into `tiny_embedder.onnx` (a lone Gather
    node), worked out independently rather than as a second copy of the C# code
    -- the only version of it worth freezing. ONNX Runtime hands back float32
    and normalizes in float32, so agreement with this exact reference is bounded
    by the float32 epsilon (~1e-7 relative) and by nothing this repository can
    improve; demanding 1e-9 would mean reproducing the C# rounding sequence in
    numpy, at which point the corpus mirrors the code and catches nothing. What
    *is* asserted exactly lives in the C# suite: the ids and mask here, a
    batched vector against the single-sequence one for the same text, and the
    vectorized net10.0 result against the scalar netstandard2.0 one.
    """
    vocab = {tok: i for i, tok in enumerate(BATCH_VOCAB)}
    table = _batch_embedding_table()

    cases = [
        _batch_case(0, "mixed_lengths", BATCH_MIXED_TEXTS, vocab, None, table),
        _batch_case(1, "edges", BATCH_EDGE_TEXTS, vocab, BATCH_MAX_LENGTH, table),
        _batch_case(2, "unknown_tokens", BATCH_UNKNOWN_TEXTS, vocab, None, table),
        _batch_case(3, "single_text", [CAT_SENTENCE], vocab, None, table),
        # Every row the same length, so the batch is already rectangular and no
        # padding is written at all — the control the padded cases are read against.
        _batch_case(4, "no_padding_needed", [THE_CAT, "the dog", "the fox"], vocab, None, table),
        _batch_case(5, "truncated", BATCH_MIXED_TEXTS, vocab, BATCH_MAX_LENGTH, table),
    ]
    _assert_batch_edges(cases[1])

    if max(max(row) for case in cases for row in case["input_ids"]) >= EMBEDDING_ROWS:
        raise AssertionError(
            f"an id falls outside the {EMBEDDING_ROWS}-row table tiny_embedder.onnx gathers from")

    return {
        "metadata": {
            "algorithm": "BatchEncoding",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "reference_calls": [
                "tokenizers.Tokenizer.encode_batch, TemplateProcessing(single='[CLS] $A [SEP]'), "
                "enable_padding(longest), enable_truncation(max_length)",
                "mean pool with attention mask + L2 normalize (sentence-transformers recipe)",
            ],
            "vocab": vocab,
            "unk_token": UNK_TOKEN,
            "template": {"prefix": [CLS_TOKEN], "suffix": [SEP_TOKEN], "pad": PAD_TOKEN},
            "embedding_rows": EMBEDDING_ROWS,
            "embedding_dim": EMBEDDING_DIM,
            "embedding_table": table.tolist(),
            "count": len(cases),
        },
        "cases": cases,
    }


# What normalization changes and identity hides (#75); see generate_normalizer's
# docstring.
NORMALIZER_TEXTS = [
    "",
    "already normal text",
    "\uff2c\uff25 \uff32\uff25\uff2e\uff21\uff32\uff24",       # full-width letters
    "\uff11\uff12\uff13",                                       # full-width digits
    "\uff71\uff92\uff98\uff76",                                # half-width katakana
    "cafe\u0301",                                                # decomposed
    "caf\u00e9",                                                 # composed
    "\ufb01nancier \ufb02amme \ufb03n",                           # ligatures
    "\u2168 \u2460\u2461 \u3231",                                # roman numeral, circled, squared
    "a\u00a0b",                                                  # non-breaking space
    "a\u3000b",                                                  # ideographic space
    "a\tb",                                                      # tab
    "a\nb",                                                      # newline
    "a\u200bb",                                                  # zero-width space
    "\ufeffbom",                                                 # byte-order mark
    "a\u0001b\u0007c",                                           # control characters
    "  spaced   out  ",
    "MiXeD CaSe TeXt",                                          # only the _cf rules fold this
    "\u00df \u2460 \u00a4",                                       # the custom rules: sharp s, circled one, currency sign
    "\u2581 already escaped",                                    # the meta symbol itself
    "\u0130stanbul",                                             # dotted capital I
    "\u1e9b\u0323",                                              # long s with dot, plus dot below
]

# Each carries a different charsmap (or none); see generate_normalizer's docstring.
NORMALIZER_FIXTURES = [
    ("xlmr_fairseq.model", "xlm-roberta-base, nmt_nfkc (the spm_train default)"),
    ("nmt_nfkc_cf.model", "self-trained, nmt_nfkc_cf (case folding)"),
    ("custom_norm.model", "self-trained, three hand-written rules from a normalization_rule_tsv"),
    (TINY_SP_MODEL, "self-trained, identity: no charsmap at all"),
]


def generate_normalizer() -> dict:
    """Freeze what each fixture's precompiled_charsmap does to text.

    Two references per case, because they answer different questions:

    * ``normalized`` is the charsmap alone. It is produced from a copy of the
      model with add_dummy_prefix, remove_extra_whitespaces and
      escape_whitespaces turned off, so nothing but the map speaks — that is
      exactly the boundary of ``PrecompiledNormalizer.Normalize``.
    * ``pieces``/``ids`` are the whole pipeline on the stock flags, which is what
      ``SentencePieceTokenizer.Encode`` reproduces.

    A test that only replayed the second could pass with the normalization and
    the whitespace handling wrong in compensating ways.

    NORMALIZER_TEXTS covers what normalization changes and identity hides, per
    the acceptance criteria of #75: width forms, composition, ligatures,
    whitespace of every flavour, control characters, case -- plus the three
    rules only custom_norm.model performs.

    NORMALIZER_FIXTURES: each entry carries a different charsmap, which is the
    point -- the same interpreter must handle all of them. tiny_sp.model is the
    control case, with no charsmap at all.

    custom_norm.model alone also carries charsmap_base64, the same blob
    `tokenizers` writes into a tokenizer.json, in the same encoding, so the JSON
    loader can be tested against a real map without a hand-pasted constant.
    Base64 of the much larger nmt_nfkc map would add 300 KB to the corpus to say
    nothing new.
    """
    import sentencepiece as spm  # noqa: PLC0415
    from sentencepiece import sentencepiece_model_pb2 as model_pb2  # noqa: PLC0415

    models = []
    cases = []
    for filename, description in NORMALIZER_FIXTURES:
        path = ORACLE_DIR / filename
        proto = model_pb2.ModelProto()
        proto.ParseFromString(path.read_bytes())

        bare = model_pb2.ModelProto()
        bare.CopyFrom(proto)
        bare.normalizer_spec.add_dummy_prefix = False
        bare.normalizer_spec.remove_extra_whitespaces = False
        bare.normalizer_spec.escape_whitespaces = False

        stock = spm.SentencePieceProcessor(model_file=str(path))
        charsmap_only = spm.SentencePieceProcessor(model_proto=bare.SerializeToString())

        entry = {
            "model": filename,
            "description": description,
            "normalizer_name": proto.normalizer_spec.name,
            "charsmap_bytes": len(proto.normalizer_spec.precompiled_charsmap),
            "vocab_size": len(proto.pieces),
        }
        if filename == "custom_norm.model":
            # Only for this fixture, so the JSON loader has a real map to test
            # against; see this function's docstring for why not the others.
            entry["charsmap_base64"] = base64.b64encode(proto.normalizer_spec.precompiled_charsmap).decode("ascii")
        models.append(entry)
        for text in NORMALIZER_TEXTS:
            cases.append({
                "id": len(cases),
                "model": filename,
                "text": text,
                "normalized": charsmap_only.normalize(text),
                "pieces": stock.encode(text, out_type=str),
                "ids": stock.encode(text, out_type=int),
            })

    return {
        "metadata": {
            "algorithm": "PrecompiledNormalizer",
            "library": "sentencepiece",
            "library_version": version("sentencepiece"),
            "reference_calls": [
                "sentencepiece.SentencePieceProcessor(model_file=…).normalize  (charsmap only)",
                "sentencepiece.SentencePieceProcessor(model_file=…).encode     (whole pipeline)",
            ],
            "models": models,
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Classification metrics (issue #61), see _metric_fixtures's docstring -----

METRIC_SEED = SEED + 61
ZERO_DIVISIONS = (0, 1)
BETAS = (0.5, 2.0)
REPORT_DIGITS = (2, 3)


def _finite_or_name(value: float) -> float | str:
    """Encode a non-finite oracle value as a string, and round a finite one.

    JSON has no literal for NaN or infinity, and this repository's loader reads
    strict JSON. Every other oracle value in the corpus is a plain number; these
    are the first that cannot be.

    A finite value goes through stable() for the reason STABLE_DIGITS gives:
    balanced accuracy, Matthews correlation and Cohen's kappa all come out of
    np.dot/np.outer/xp.mean reductions, so their last bits describe this host's
    BLAS kernel rather than the metric. The three name strings are returned
    untouched — "NaN" must stay "NaN", not become a rounded number.
    """
    if math.isnan(value):
        return "NaN"
    if math.isinf(value):
        return "Infinity" if value > 0 else "-Infinity"
    return stable(value)


def _metric_fixtures() -> list[dict]:
    """Fixtures chosen for where implementations diverge, not average behaviour.

    A class never predicted, a class absent from the truth, a labels= subset
    (which drops samples and turns the report's accuracy row into a micro-avg
    row), and non-contiguous label values that catch any implementation
    assuming 0..k-1. Each fixture is emitted twice, unweighted and weighted,
    because sample_weight changes the dtype of every count upstream.

    class_only_in_pred is small enough to move balanced accuracy off the naive
    per-sample average: it is scored only over the classes present in y_true
    (0.75), not over every class either array mentions (0.5), and the adjusted
    form follows the same restriction.
    """
    rng = SeededRandom(METRIC_SEED)
    fixtures: list[dict] = []

    def noisy(truth: list[int], classes: list[int], flip: float) -> list[int]:
        return [
            t if rng.random() >= flip else rng.choice([c for c in classes if c != t])
            for t in truth
        ]

    def add(name, y_true, y_pred, labels=None, target_names=None, pos_label=1):
        fixtures.append({
            "name": name,
            "y_true": [int(v) for v in y_true],
            "y_pred": [int(v) for v in y_pred],
            "labels": labels,
            "target_names": target_names,
            "pos_label": pos_label,
            "sample_weight": [round(rng.uniform(0.1, 3.0), 3) for _ in y_true],
        })

    balanced = [rng.randint(0, 1) for _ in range(200)]
    add("binary_balanced", balanced, noisy(balanced, [0, 1], 0.2),
        target_names=["negative", "positive"])

    imbalanced = [0] * 190 + [1] * 10
    add("binary_imbalanced", imbalanced, noisy(imbalanced, [0, 1], 0.3))

    three = [rng.randint(0, 2) for _ in range(300)]
    add("multiclass_3", three, noisy(three, [0, 1, 2], 0.35),
        target_names=["alpha", "beta", "gamma"])

    ten = [rng.randint(0, 9) for _ in range(500)]
    add("multiclass_10", ten, noisy(ten, list(range(10)), 0.5))

    # Class 2 is in y_true and never predicted: its precision divides by zero.
    add("class_never_predicted", [0, 0, 1, 1, 2, 2, 1, 0], [0, 1, 1, 1, 0, 1, 1, 0])

    # Class 3 is predicted and absent from y_true: its recall divides by zero.
    add("class_absent_from_truth", [0, 0, 1, 1, 0, 1], [0, 3, 1, 3, 0, 1])

    perfect = [rng.randint(0, 2) for _ in range(50)]
    add("perfect", perfect, list(perfect))
    add("all_wrong", perfect, [(v + 1) % 3 for v in perfect])

    add("single_sample", [1], [1])
    add("single_class", [1, 1, 1, 1], [1, 1, 1, 1])

    subset = [rng.randint(0, 3) for _ in range(120)]
    add("labels_subset", subset, noisy(subset, [0, 1, 2, 3], 0.4), labels=[0, 2])

    sparse = [rng.choice([-1, 5, 42]) for _ in range(120)]
    add("non_contiguous_labels", sparse, noisy(sparse, [-1, 5, 42], 0.4), pos_label=5)

    # A class predicted but never true; see this function's docstring for why.
    add("class_only_in_pred", [0, 0, 1], [0, 2, 1])

    return fixtures


def _binary_average_applies(observed: list[int], pos_label: int) -> bool:
    """Mirror scikit-learn's own admissibility rule for average="binary"."""
    if len(observed) > 2:
        return False
    return pos_label in observed or len(observed) < 2


def _metric_case(fx: dict, weighted: bool) -> dict:
    """One fixture's full metric surface, unweighted or weighted.

    ``case["normalized"]`` passes labels=labels so every mode's matrix keeps the
    same shape and label ordering as ``case["confusion_matrix"]`` above it,
    rather than falling back to the full observed label set for
    labels_subset-style fixtures. Each entry goes through stable(), not bare
    float(): normalize= divides by a row, column or grand sum that numpy
    reduced, so it carries the same host-dependent last bits as every other
    reduced value here. confusion_matrix's own nan_to_num keeps every entry
    finite, so none needs _finite_or_name's string encoding.
    """
    y_true, y_pred = fx["y_true"], fx["y_pred"]
    labels, pos_label = fx["labels"], fx["pos_label"]
    sw = fx["sample_weight"] if weighted else None
    observed = sorted(set(y_true) | set(y_pred))
    effective = labels if labels is not None else observed
    averages = ["micro", "macro", "weighted"]
    if _binary_average_applies(observed, pos_label):
        averages.append("binary")

    cm = skm.confusion_matrix(y_true, y_pred, labels=labels, sample_weight=sw)
    case = {
        "fixture": fx["name"],
        "weighted": weighted,
        "y_true": y_true,
        "y_pred": y_pred,
        "sample_weight": sw,
        "labels": labels,
        "target_names": fx["target_names"],
        "pos_label": pos_label,
        "expected_labels": [int(v) for v in effective],
        "confusion_matrix": [[stable(v) for v in row] for row in cm.tolist()],
        "accuracy": stable(skm.accuracy_score(y_true, y_pred, sample_weight=sw)),
        "accuracy_count": stable(
            skm.accuracy_score(y_true, y_pred, normalize=False, sample_weight=sw)),
        "averaged": {},
        "per_class": {},
        "fbeta": {},
        "reports": {},
    }

    for zd in ZERO_DIVISIONS:
        for avg in averages:
            p, r, f, _ = skm.precision_recall_fscore_support(
                y_true, y_pred, labels=labels, average=avg, pos_label=pos_label,
                sample_weight=sw, zero_division=zd)
            case["averaged"][f"{avg}|{zd}"] = {
                PRECISION: stable(p), RECALL: stable(r), "f1": stable(f)}
            for beta in BETAS:
                case["fbeta"][f"{beta}|{avg}|{zd}"] = stable(skm.fbeta_score(
                    y_true, y_pred, beta=beta, labels=labels, average=avg,
                    pos_label=pos_label, sample_weight=sw, zero_division=zd))
        p, r, f, s = skm.precision_recall_fscore_support(
            y_true, y_pred, labels=labels, average=None, sample_weight=sw,
            zero_division=zd)
        case["per_class"][str(zd)] = {
            PRECISION: [stable(v) for v in p],
            RECALL: [stable(v) for v in r],
            "f1": [stable(v) for v in f],
            "support": [stable(v) for v in s],
        }

    for digits in REPORT_DIGITS:
        case["reports"][str(digits)] = skm.classification_report(
            y_true, y_pred, labels=labels, target_names=fx["target_names"],
            digits=digits, sample_weight=sw, zero_division=0)

    case["balanced_accuracy"] = _finite_or_name(
        skm.balanced_accuracy_score(y_true, y_pred, sample_weight=sw))
    case["balanced_accuracy_adjusted"] = _finite_or_name(
        skm.balanced_accuracy_score(y_true, y_pred, sample_weight=sw, adjusted=True))
    case["matthews"] = _finite_or_name(skm.matthews_corrcoef(y_true, y_pred, sample_weight=sw))
    for suffix, w in (("", None), ("_linear", "linear"), ("_quadratic", "quadratic")):
        case["kappa" + suffix] = _finite_or_name(
            skm.cohen_kappa_score(y_true, y_pred, weights=w, sample_weight=sw))
    # See this function's docstring for labels=labels and stable() here.
    case["normalized"] = {
        mode: [[stable(x) for x in row]
               for row in skm.confusion_matrix(
                   y_true, y_pred, labels=labels, sample_weight=sw, normalize=mode)]
        for mode in ("true", "pred", "all")
    }
    return case


# zero_division=np.nan joins the modes for the undefined-average cases only: the
# existing cases keep their keys, and a new mode there would move every one (#861).
UNDEFINED_ZERO_DIVISIONS = (0, 1, math.nan)


def _undefined_average_fixtures() -> list[dict]:
    """Inputs where a macro or weighted average meets an undefined class (#861).

    scikit-learn averages through _nanaverage, which drops the NaN classes and
    falls back to an unweighted mean when the remaining support sums to zero.
    A zero sample weight is what reaches a zero total support while a requested
    label still occurs in y_true, which the confusion matrix requires.
    """
    return [
        {"name": "one_class_never_predicted", "y_true": [0, 0, 1], "y_pred": [0, 0, 0],
         "labels": None, "sample_weight": None},
        {"name": "one_class_never_predicted_weighted",
         "y_true": [0, 0, 1, 1, 2, 2, 1, 0], "y_pred": [0, 1, 1, 1, 0, 1, 1, 0],
         "labels": None, "sample_weight": [1.5, 0.25, 2.0, 1.0, 3.0, 0.5, 1.25, 2.5]},
        {"name": "label_absent_from_both", "y_true": [0, 1, 1], "y_pred": [0, 1, 0],
         "labels": [0, 1, 2], "sample_weight": None},
        {"name": "every_class_undefined", "y_true": [0, 1], "y_pred": [1, 1],
         "labels": [0], "sample_weight": [0.0, 1.0]},
        {"name": "zero_total_support", "y_true": [0, 2, 1, 1], "y_pred": [1, 2, 0, 1],
         "labels": [0, 2], "sample_weight": [0.0, 0.0, 1.0, 1.0]},
    ]


def _undefined_average_case(fx: dict) -> dict:
    """Macro and weighted scores, and the report's two average rows, per zero_division.

    jaccard_score refuses zero_division=np.nan, so its entry is null under that
    mode; classification_report's rows come from precision_recall_fscore_support,
    the same _nanaverage the scores use.
    """
    y_true, y_pred = fx["y_true"], fx["y_pred"]
    labels, sw = fx["labels"], fx["sample_weight"]
    case = {
        "fixture": fx["name"],
        "y_true": y_true,
        "y_pred": y_pred,
        "labels": labels,
        "sample_weight": sw,
        "scores": {},
        "report": {},
    }
    for zd in UNDEFINED_ZERO_DIVISIONS:
        mode = "nan" if math.isnan(zd) else str(zd)
        for avg in ("macro", "weighted"):
            kw = {"labels": labels, "average": avg, "sample_weight": sw, "zero_division": zd}
            p, r, f, _ = skm.precision_recall_fscore_support(y_true, y_pred, **kw)
            entry = {
                PRECISION: _finite_or_name(p),
                RECALL: _finite_or_name(r),
                "f1": _finite_or_name(f),
            }
            for beta in BETAS:
                entry[f"fbeta_{beta}"] = _finite_or_name(
                    skm.fbeta_score(y_true, y_pred, beta=beta, **kw))
            entry[JACCARD] = None if math.isnan(zd) else _finite_or_name(
                skm.jaccard_score(y_true, y_pred, **kw))
            case["scores"][f"{avg}|{mode}"] = entry
        report = skm.classification_report(
            y_true, y_pred, labels=labels, sample_weight=sw, zero_division=zd,
            output_dict=True)
        case["report"][mode] = {
            row: {name: _finite_or_name(report[row][name])
                  for name in (PRECISION, RECALL, "f1-score", "support")}
            for row in ("macro avg", "weighted avg")
        }
    return case


def generate_classification_metrics() -> dict:
    with warnings.catch_warnings():
        # scikit-learn warns on every undefined metric; the corpus records the
        # value it returns, which is the thing under test.
        warnings.simplefilter("ignore")
        cases = [
            _metric_case(fx, weighted)
            for fx in _metric_fixtures()
            for weighted in (False, True)
        ]
        undefined_averages = [_undefined_average_case(fx) for fx in _undefined_average_fixtures()]
    return {
        "metadata": {
            "algorithm": "ClassificationMetrics",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.accuracy_score",
                "sklearn.metrics.confusion_matrix",
                "sklearn.metrics.confusion_matrix(normalize=...)",
                "sklearn.metrics.precision_recall_fscore_support",
                "sklearn.metrics.fbeta_score",
                "sklearn.metrics.classification_report",
                "sklearn.metrics.balanced_accuracy_score",
                "sklearn.metrics.balanced_accuracy_score(adjusted=True)",
                "sklearn.metrics.matthews_corrcoef",
                "sklearn.metrics.cohen_kappa_score(weights=None|'linear'|'quadratic')",
            ],
            "count": len(cases),
        },
        "cases": cases,
        "undefined_averages": undefined_averages,
    }


# --- Clustering agreement (issue #172) ---------------------------------------


def _clustering_fixtures() -> list[tuple[str, list[int], list[int]]]:
    """Partitions whose agreement is the thing under test, degenerate ones included.

    The last four are the cases the reference answers surprisingly: an empty
    input and a single sample are perfect agreement rather than an error, and a
    single cluster on one side splits homogeneity from completeness.
    """
    return [
        ("identical", [0, 0, 1, 1, 2, 2], [0, 0, 1, 1, 2, 2]),
        ("renamed", [0, 0, 1, 1, 2, 2], [2, 2, 0, 0, 1, 1]),
        ("one moved", [0, 0, 1, 1, 2, 2], [0, 0, 1, 2, 2, 2]),
        ("independent", [0, 0, 1, 1], [0, 1, 0, 1]),
        ("split in two", [0, 0, 0, 1, 1, 1], [0, 1, 0, 1, 0, 1]),
        ("merged", [0, 0, 1, 1, 2, 2], [0, 0, 0, 0, 1, 1]),
        ("unbalanced", [0] * 8 + [1, 2], [0] * 7 + [1, 1, 2]),
        ("negative labels", [-1, -1, 3, 3], [7, 7, -2, -2]),
        ("one cluster predicted", [0, 0, 1, 1], [0, 0, 0, 0]),
        ("one class in truth", [0, 0, 0, 0], [0, 0, 1, 1]),
        ("every sample alone", [0, 0, 1, 1], [0, 1, 2, 3]),
        ("single sample", [0], [0]),
        ("empty", [], []),
    ]


def generate_clustering_agreement() -> dict:
    from sklearn import metrics as skmetrics

    cases = []
    with warnings.catch_warnings():
        # An undefined case warns and still returns the value under test.
        warnings.simplefilter("ignore")
        for name, true, pred in _clustering_fixtures():
            homogeneity, completeness, v_measure = (
                skmetrics.homogeneity_completeness_v_measure(true, pred))
            cases.append({
                "name": name,
                "labels_true": true,
                "labels_pred": pred,
                "adjusted_rand": skmetrics.adjusted_rand_score(true, pred),
                "normalized_mutual_information": skmetrics.normalized_mutual_info_score(true, pred),
                "fowlkes_mallows": skmetrics.fowlkes_mallows_score(true, pred),
                # scikit-learn 1.9.0 raises on an empty input here -- log(0) inside
                # mutual_info_score -- where every other metric in this corpus returns.
                "mutual_information": skmetrics.mutual_info_score(true, pred) if true else None,
                "rand": skmetrics.rand_score(true, pred),
                "pair_confusion": [
                    int(v) for v in
                    skmetrics.cluster.pair_confusion_matrix(true, pred).ravel()
                ] if true else [0, 0, 0, 0],
                "adjusted_mutual_information": skmetrics.adjusted_mutual_info_score(true, pred),
                "homogeneity": homogeneity,
                "completeness": completeness,
                "v_measure": v_measure,
            })

    return {
        "metadata": {
            "algorithm": "ClusteringAgreement",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.adjusted_rand_score",
                "sklearn.metrics.normalized_mutual_info_score",
                "sklearn.metrics.homogeneity_completeness_v_measure",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Ranking, ordered list (issue #173, first lot) ---------------------------


def _ranking_fixtures() -> list[dict]:
    """Relevance and score pairs whose *ties* are the thing under test.

    A tie is where a plausible implementation agrees on the easy cases and
    disagrees where it matters: scikit-learn averages the discounted gain over
    the permutations of equal scores, which is a different number from ranking
    them arbitrarily -- 0.807 against 0.614 on the all-tied case, measured.
    """
    return [
        {"name": "perfectly ordered", "true": [3.0, 2.0, 1.0, 0.0], "score": [0.9, 0.5, 0.4, 0.1]},
        {"name": "reversed", "true": [3.0, 2.0, 1.0, 0.0], "score": [0.1, 0.4, 0.5, 0.9]},
        {"name": ALL_TIED, "true": [3.0, 2.0, 1.0, 0.0], "score": [0.5, 0.5, 0.5, 0.5]},
        {"name": "two tied among distinct", "true": [3.0, 2.0, 1.0, 0.0], "score": [0.9, 0.5, 0.5, 0.1]},
        {"name": "a tie across the k boundary", "true": [3.0, 2.0, 1.0, 0.0], "score": [0.9, 0.5, 0.5, 0.2]},
        {"name": "all-zero relevance", "true": [0.0, 0.0, 0.0, 0.0], "score": [0.9, 0.5, 0.4, 0.1]},
        {"name": "one relevant document", "true": [0.0, 0.0, 1.0, 0.0], "score": [0.9, 0.5, 0.4, 0.1]},
        {"name": "six documents", "true": [2.0, 0.0, 3.0, 1.0, 0.0, 2.0],
         "score": [0.8, 0.7, 0.6, 0.5, 0.4, 0.3]},
    ]


def generate_ranking() -> dict:
    import math

    import numpy as np
    from sklearn.metrics import dcg_score, ndcg_score

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _ranking_fixtures():
            true = np.array([fixture["true"]])
            score = np.array([fixture["score"]])
            cases.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_score": fixture["score"],
                "dcg": float(dcg_score(true, score)),
                # ignore_ties goes through a bare np.argsort, an unstable quicksort, so
                # on a tied row this value is numpy's order rather than a defined one.
                "dcg_ignore_ties": float(dcg_score(true, score, ignore_ties=True)),
                "dcg_log_e": float(dcg_score(true, score, log_base=math.e)),
                "dcg_at_2": float(dcg_score(true, score, k=2)),
                "ndcg": float(ndcg_score(true, score)),
                "ndcg_ignore_ties": float(ndcg_score(true, score, ignore_ties=True)),
                "ndcg_at_2": float(ndcg_score(true, score, k=2)),
                "ndcg_at_99": float(ndcg_score(true, score, k=99)),
            })

    return {
        "metadata": {
            "algorithm": "Ranking",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.dcg_score",
                "sklearn.metrics.dcg_score(k=..., log_base=..., ignore_ties=...)",
                "sklearn.metrics.ndcg_score",
                "sklearn.metrics.ndcg_score(k=..., ignore_ties=...)",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# Three corpora open with the pair their own page works through, and the name is
# what ties a failing case back to the prose that explains it.
WORKED_CASE = "the worked case"
WORKED_CASE_WEIGHTED = "the worked case, weighted"
SIGNED_LABELS = "labels are -1 and 1"
ALL_TIED = "every score tied"


def _label_ranking_fixtures() -> list[dict]:
    """Rows where a plausible implementation and the reference part company."""
    wide = [0] * 20
    for j in (0, 9, 19):
        wide[j] = 1
    return [
        {"name": WORKED_CASE, "true": [[1, 0, 0], [0, 0, 1]],
         "score": [[0.75, 0.5, 1.0], [1.0, 0.2, 0.1]], "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [[1, 0, 0], [0, 0, 1]],
         "score": [[0.75, 0.5, 1.0], [1.0, 0.2, 0.1]], "weight": [1.0, 2.0]},
        {"name": "every label relevant", "true": [[1, 1, 1]],
         "score": [[0.7, 0.2, 0.1]], "weight": None},
        {"name": "no label relevant", "true": [[0, 0, 0]],
         "score": [[0.7, 0.2, 0.1]], "weight": None},
        {"name": "an empty row beside a scoring one", "true": [[0, 0, 0], [1, 0, 0]],
         "score": [[0.7, 0.2, 0.1], [0.7, 0.2, 0.1]], "weight": None},
        {"name": "every score equal, two of three relevant", "true": [[1, 1, 0]],
         "score": [[0.5, 0.5, 0.5]], "weight": None},
        {"name": "negative scores", "true": [[1, 0, 0]],
         "score": [[-0.7, -0.2, -0.1]], "weight": None},
        {"name": "relevant on top", "true": [[1, 1, 0, 0]],
         "score": [[0.9, 0.8, 0.2, 0.1]], "weight": None},
        {"name": "relevant at the bottom", "true": [[0, 0, 1, 1]],
         "score": [[0.9, 0.8, 0.2, 0.1]], "weight": None},
        # 20 columns: the width at which lot 1's Array.Sort stopped being stable. The
        # tie order is unobservable here, and a case is worth more than the claim.
        {"name": "twenty columns, every score tied", "true": [wide],
         "score": [[0.5] * 20], "weight": None},
        {"name": "twenty columns, strictly ordered", "true": [wide],
         "score": [[(20 - j) / 20 for j in range(20)]], "weight": None},
        # The relevant labels are at indices 0 and 2 but score 0.2 and 0.8, so their
        # index order and score order disagree -- the only shape a pairing bug shows in.
        {"name": "relevant labels out of score order", "true": [[1, 0, 1]],
         "score": [[0.2, 0.9, 0.8]], "weight": None},
    ]


def generate_label_ranking() -> dict:
    import numpy as np
    from sklearn.metrics import coverage_error, label_ranking_loss
    from sklearn.metrics import label_ranking_average_precision_score as lrap

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _label_ranking_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}
            cases.append({
                "name": fixture["name"],
                "y_true": [v for row in fixture["true"] for v in row],
                "y_score": [v for row in fixture["score"] for v in row],
                "label_count": true.shape[1],
                "sample_weight": fixture["weight"],
                "lrap": float(lrap(true, score, **kw)),
                "coverage": float(coverage_error(true, score, **kw)),
                "ranking_loss": float(label_ranking_loss(true, score, **kw)),
            })

    return {
        "metadata": {
            "algorithm": "LabelRanking",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.label_ranking_average_precision_score",
                "sklearn.metrics.coverage_error",
                "sklearn.metrics.label_ranking_loss",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


def _average_precision_binary_fixtures() -> list[dict]:
    """Binary cases, chosen where the step sum and the trapezoid part company."""
    return [
        {"name": WORKED_CASE, "true": [0, 0, 1, 1],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 1, "weight": None},
        {"name": "the worked case, pos_label 0", "true": [0, 0, 1, 1],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 0, "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [0, 0, 1, 1],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 1, "weight": [1.0, 2.0, 3.0, 4.0]},
        # Every score tied: the sum takes one step of the full recall at the group's
        # precision, where the trapezoid interpolates a diagonal that is not there.
        {"name": ALL_TIED, "true": [0, 1, 0, 1],
         "score": [0.5, 0.5, 0.5, 0.5], "pos_label": 1, "weight": None},
        {"name": "perfectly ranked", "true": [0, 0, 1, 1],
         "score": [0.1, 0.2, 0.3, 0.4], "pos_label": 1, "weight": None},
        {"name": "perfectly inverted", "true": [1, 1, 0, 0],
         "score": [0.1, 0.2, 0.3, 0.4], "pos_label": 1, "weight": None},
        {"name": "one positive, ranked last", "true": [0, 0, 0, 1],
         "score": [0.9, 0.8, 0.7, 0.1], "pos_label": 1, "weight": None},
        # scikit-learn warns here and returns a value rather than refusing.
        {"name": "no positive sample", "true": [0, 0, 0, 0],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 1, "weight": None},
        {"name": "every sample positive", "true": [1, 1, 1, 1],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 1, "weight": None},
        {"name": "negative scores", "true": [0, 0, 1, 1],
         "score": [-0.9, -0.6, -0.65, -0.2], "pos_label": 1, "weight": None},
        {"name": SIGNED_LABELS, "true": [-1, -1, 1, 1],
         "score": [0.1, 0.4, 0.35, 0.8], "pos_label": 1, "weight": None},
        {"name": "a tie spanning both classes", "true": [1, 0, 1, 0, 1],
         "score": [0.9, 0.5, 0.5, 0.5, 0.1], "pos_label": 1, "weight": None},
    ]


def generate_average_precision() -> dict:
    """average_precision_score: the binary sum, and the label matrix it averages over."""
    import numpy as np
    from sklearn.metrics import average_precision_score, auc, precision_recall_curve

    binary = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _average_precision_binary_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            kw = {"pos_label": fixture["pos_label"]}
            if fixture["weight"] is not None:
                kw["sample_weight"] = np.array(fixture["weight"])

            # The trapezoid is carried beside the sum so the corpus itself records the
            # difference the metric exists to avoid; nothing in C# reproduces this column.
            precision, recall, _ = precision_recall_curve(true, score, **kw)
            binary.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_score": fixture["score"],
                "pos_label": fixture["pos_label"],
                "sample_weight": fixture["weight"],
                "average_precision": float(average_precision_score(true, score, **kw)),
                "trapezoid": float(auc(recall, precision)),
            })

    multilabel = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _label_ranking_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}
            multilabel.append({
                "name": fixture["name"],
                "y_true": [int(v) for row in fixture["true"] for v in row],
                "y_score": [v for row in fixture["score"] for v in row],
                "label_count": int(true.shape[1]),
                "sample_weight": fixture["weight"],
                "macro": float(average_precision_score(true, score, average="macro", **kw)),
                "micro": float(average_precision_score(true, score, average="micro", **kw)),
                "weighted": float(average_precision_score(true, score, average="weighted", **kw)),
                "per_label": [float(v) for v in np.atleast_1d(
                    average_precision_score(true, score, average=None, **kw))],
            })

    return {
        "metadata": {
            "algorithm": "AveragePrecision",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.average_precision_score",
                "sklearn.metrics.auc",
                "sklearn.metrics.precision_recall_curve",
            ],
            "count": len(binary) + len(multilabel),
        },
        "binary_cases": binary,
        "multilabel_cases": multilabel,
    }


def _deviance_fixtures() -> list[dict]:
    """Pairs chosen so every Tweedie regime is reached, and each one's domain edge."""
    return [
        {"name": WORKED_CASE, "true": [1.0, 2.0, 3.0, 4.0],
         "pred": [1.5, 2.5, 2.0, 4.5], "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [1.0, 2.0, 3.0, 4.0],
         "pred": [1.5, 2.5, 2.0, 4.5], "weight": [1.0, 2.0, 3.0, 4.0]},
        {"name": "a perfect prediction", "true": [1.5, 2.5, 2.0],
         "pred": [1.5, 2.5, 2.0], "weight": None},
        # y_true at zero is the boundary between the [1, 2) regime, which allows it,
        # and the >= 2 regimes, which do not -- so it is scored only where it is legal.
        {"name": "a zero truth", "true": [0.0, 2.0, 3.0],
         "pred": [1.0, 2.0, 3.0], "weight": None},
        {"name": "far apart", "true": [1.0, 10.0, 2.0],
         "pred": [8.0, 1.0, 9.0], "weight": None},
        # long-comment: the exclusion below is a reproducibility claim, and a reader
        # who does not know why will delete the flag and re-break the drift gate.
        # At power -2 this pair's deviance is a sum of three terms an order of
        # magnitude larger than the result, so its last bits follow the machine's
        # reduction order -- measured, a CI runner and this one disagree by one ulp
        # on it. Freezing either answer makes the gate a lottery. Only this fixture
        # is affected, and the other five still cover the negative regimes.
        {"name": "small values", "true": [0.01, 0.5, 0.25],
         "pred": [0.02, 0.4, 0.3], "weight": None, "skip_negative_powers": True},
    ]


def _tweedie_powers() -> list[float]:
    """One power per regime, plus the two the named deviances are."""
    return [-2.0, -1.0, 0.0, 1.0, 1.5, 2.0, 3.0]


def _tweedie_admits(power: float, true, pred) -> bool:
    """Whether the regime admits this pair, which is what the C# side refuses on."""
    if power < 0:
        return min(pred) > 0
    if power == 0:
        return True
    if power < 2:
        return min(true) >= 0 and min(pred) > 0
    return min(true) > 0 and min(pred) > 0


def _tweedie_row(fixture: dict, true, pred, kw: dict) -> list[dict]:
    """One entry per power the fixture is legal at, with its D2 where that is defined."""
    from sklearn.metrics import d2_tweedie_score, mean_tweedie_deviance

    # A D2 needs two samples and a truth that varies; where it does not, the
    # reference divides by zero and the C# side refuses instead.
    scored = len(true) >= 2 and len(set(fixture["true"])) > 1

    rows = []
    for power in _tweedie_powers():
        if power < 0 and fixture.get("skip_negative_powers"):
            continue
        if not _tweedie_admits(power, fixture["true"], fixture["pred"]):
            continue
        entry = {
            "power": power,
            "deviance": float(mean_tweedie_deviance(true, pred, power=power, **kw)),
        }
        if scored:
            entry["d2"] = float(d2_tweedie_score(true, pred, power=power, **kw))
        rows.append(entry)
    return rows


def _deviance_case(fixture: dict) -> dict:
    """Every number one fixture contributes, across the powers its values allow."""
    import numpy as np
    from sklearn.metrics import (
        d2_absolute_error_score,
        d2_pinball_score,
        mean_gamma_deviance,
        mean_poisson_deviance,
    )

    true = np.array(fixture["true"])
    pred = np.array(fixture["pred"])
    kw = {} if fixture["weight"] is None else {"sample_weight": np.array(fixture["weight"])}

    case = {
        "name": fixture["name"],
        "y_true": fixture["true"],
        "y_pred": fixture["pred"],
        "sample_weight": fixture["weight"],
        "tweedie": _tweedie_row(fixture, true, pred, kw),
        "d2_absolute_error": float(d2_absolute_error_score(true, pred, **kw)),
        "pinball": [
            {"alpha": alpha, "d2": float(d2_pinball_score(true, pred, alpha=alpha, **kw))}
            for alpha in (0.1, 0.25, 0.5, 0.75, 0.9)
        ],
    }
    if _tweedie_admits(1.0, fixture["true"], fixture["pred"]):
        case[POISSON] = float(mean_poisson_deviance(true, pred, **kw))
    if _tweedie_admits(2.0, fixture["true"], fixture["pred"]):
        case[GAMMA] = float(mean_gamma_deviance(true, pred, **kw))
    return case


def _deviance_multioutput() -> dict:
    """Two outputs, the shape only the two pinball D2 scores accept."""
    import numpy as np
    from sklearn.metrics import d2_absolute_error_score, d2_pinball_score

    true = [[0.5, 1.0], [1.0, 1.0], [7.0, -6.0]]
    pred = [[0.0, 2.0], [-1.0, 2.0], [8.0, -5.0]]
    mt = np.array(true)
    mp = np.array(pred)
    return {
        "y_true": [v for row in true for v in row],
        "y_pred": [v for row in pred for v in row],
        "output_count": 2,
        "uniform_average": float(d2_absolute_error_score(mt, mp)),
        "raw_values": [float(v) for v in d2_absolute_error_score(mt, mp, multioutput="raw_values")],
        "pinball_uniform_average": float(d2_pinball_score(mt, mp, alpha=0.75)),
        "pinball_raw_values": [
            float(v) for v in d2_pinball_score(mt, mp, alpha=0.75, multioutput="raw_values")],
    }


def generate_regression_deviance() -> dict:
    """The three GLM deviances and the three D2 scores -- regression lot 2 (#202)."""
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        cases = [_deviance_case(fixture) for fixture in _deviance_fixtures()]
        multioutput = _deviance_multioutput()

    return {
        "metadata": {
            "algorithm": "RegressionDeviance",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.mean_tweedie_deviance",
                "sklearn.metrics.mean_poisson_deviance",
                "sklearn.metrics.mean_gamma_deviance",
                "sklearn.metrics.d2_tweedie_score",
                "sklearn.metrics.d2_pinball_score",
                "sklearn.metrics.d2_absolute_error_score",
            ],
            "count": len(cases),
        },
        "cases": cases,
        "multioutput": multioutput,
    }


# --- Split conformal prediction (#441) ------------------------------------


def _frozen_estimators():
    """MAPIE-compatible estimators whose predictions are a frozen table.

    prefit=True means MAPIE only ever calls predict / predict_proba, so a table
    indexed by X[:, 0] is a complete estimator here -- and unlike a fitted
    regressor it puts no BLAS reduction between the fixture and the corpus.
    """
    from sklearn.base import BaseEstimator, ClassifierMixin, RegressorMixin

    class FrozenRegressor(RegressorMixin, BaseEstimator):
        def __init__(self, table=None):
            self.table = table

        def fit(self, X):
            # No y: the table is the model, so fitting only records the shape and
            # the marker sklearn's check_is_fitted looks for.
            self.n_features_in_ = np.asarray(X).shape[1]
            self.is_fitted_ = True
            return self

        def predict(self, X):
            return np.asarray(self.table, dtype=float)[np.asarray(X)[:, 0].astype(int)]

    class FrozenClassifier(ClassifierMixin, BaseEstimator):
        def __init__(self, table=None, n_classes=0):
            self.table = table
            self.n_classes = n_classes

        def fit(self, X):
            # classes_ comes from n_classes rather than from a y: see FrozenRegressor.fit.
            self.classes_ = np.arange(self.n_classes)
            self.n_features_in_ = np.asarray(X).shape[1]
            self.is_fitted_ = True
            return self

        def predict_proba(self, X):
            return np.asarray(self.table, dtype=float)[np.asarray(X)[:, 0].astype(int)]

        def predict(self, X):
            return self.classes_[self.predict_proba(X).argmax(axis=1)]

    return FrozenRegressor, FrozenClassifier


def _conformal_quantile(scores: list[float], alpha: float) -> tuple[int, float]:
    """The rule under test, computed here so the corpus can assert MAPIE against it."""
    n = len(scores)
    k = math.ceil((n + 1) * (1.0 - alpha))
    if k > n:
        raise ValueError(f"k={k} exceeds n={n}; this corpus holds only cases MAPIE answers")
    return k, sorted(scores)[k - 1]


def _mapie_classification_quantile(scores: list[float], alpha: float) -> tuple[int, float]:
    """MAPIE's prediction-set rule: np.quantile at (n + 1)(1 - alpha)/n, method="higher" (#866).

    numpy indexes ceil((n - 1) * level), 0-based; the arithmetic follows its order so a level
    near an integer rounds the way numpy rounds it.
    """
    n = len(scores)
    level = ((n + 1) * (1 - alpha)) / n
    if level > 1:
        raise ValueError(f"level {level} exceeds 1; this corpus holds only cases MAPIE answers")
    k = math.ceil((n - 1) * level) + 1
    return k, float(np.quantile(scores, level, method="higher"))


def _conformal_regression_fixtures() -> list[dict]:
    """Calibration/test splits for the absolute-residual score.

    Every alpha satisfies MAPIE's own precondition -- 1/alpha and 1/(1 - alpha)
    both below the calibration size -- because below it MAPIE refuses to answer at
    all, which is the same region decision 0070 is about.
    """
    rng = SeededRandom(SEED + 441)
    y_calib = [round(rng.gauss(10.0, 3.0), 6) for _ in range(30)]
    predicted = [round(v + rng.gauss(0.0, 1.5), 6) for v in y_calib]
    test = [round(rng.gauss(10.0, 3.0), 6) for _ in range(6)]
    return [
        {"name": "thirty calibration points at 90 %",
         "alpha": 0.1, Y_CALIB: y_calib, Y_CALIB_PRED: predicted, Y_TEST_PRED: test},
        {"name": "the same points at 50 %",
         "alpha": 0.5, Y_CALIB: y_calib, Y_CALIB_PRED: predicted, Y_TEST_PRED: test},
        # (n + 1)(1 - alpha) = 20 * 0.9 = 18 exactly, so the ceiling must not round
        # up: k is 18, not 19. An implementation carrying an epsilon gets this wrong.
        {"name": "an exact integer at the ceiling",
         "alpha": 0.1, Y_CALIB: y_calib[:19], Y_CALIB_PRED: predicted[:19],
         Y_TEST_PRED: test},
        # Repeated scores: the k-th smallest is a position, not a distinct value.
        {"name": "ties in the calibration scores",
         "alpha": 0.3,
         Y_CALIB: [float(v) for v in range(1, 13)],
         Y_CALIB_PRED: [1.5, 2.5, 4.0, 5.0, 4.0, 5.75, 9.0, 8.5, 8.0, 9.75, 13.0, 11.5],
         Y_TEST_PRED: [0.0, 6.25, 100.0]},
    ]


def _peaked_rows(rng: SeededRandom, rows: int, classes: int, sharpness: float) -> list[list[float]]:
    """Probability rows, normalised in pure Python so the corpus holds exact doubles.

    A numpy row-sum would put a reduction between the fixture and the file; these
    values are committed, so they are computed the way they are written.
    """
    out = []
    for _ in range(rows):
        raw = [rng.random() ** sharpness + 1e-3 for _ in range(classes)]
        total = math.fsum(raw)
        out.append([v / total for v in raw])
    return out


def _conformal_classification_fixtures() -> list[dict]:
    """LAC fixtures, including the one whose point is an empty prediction set."""
    rng = SeededRandom(SEED + 4410)
    flat = _peaked_rows(rng, 88, 4, 1.0)
    confident = _peaked_rows(rng, 64, 3, 6.0)
    # A deliberately flat test row under a confident model: no class clears 1 - q,
    # and LAC's answer is the empty set rather than the arg-max.
    confident[-1] = [0.34, 0.33, 0.33]
    binary = _peaked_rows(rng, 40, 2, 2.0)
    between = _peaked_rows(SeededRandom(SEED + 866), 128, 3, 2.0)
    return [
        {"name": "eighty calibration points, four classes, at 80 %",
         "alpha": 0.2, "class_count": 4, CALIB_SIZE: 80, "proba": flat, "empty": False},
        {"name": "a confident model, where a flat row gets an empty set",
         "alpha": 0.25, "class_count": 3, CALIB_SIZE: 60, "proba": confident, "empty": True},
        {"name": "two classes at 75 %",
         "alpha": 0.25, "class_count": 2, CALIB_SIZE: 36, "proba": binary, "empty": False},
        # The two rules read different order statistics here, the 19th against the 18th and
        # the 91st against the 90th, and the last row sits between their thresholds (#866).
        {"name": "nineteen points at 90 %, where MAPIE reads one rank higher",
         "alpha": 0.1, "class_count": 3, CALIB_SIZE: 19, "proba": between[:24],
         "empty": False, BETWEEN: True},
        {"name": "ninety-nine points at 90 %, where MAPIE reads one rank higher",
         "alpha": 0.1, "class_count": 3, CALIB_SIZE: 99, "proba": between[24:],
         "empty": False, BETWEEN: True},
    ]


def _conformal_labels(rng: SeededRandom, rows: list[list[float]]) -> list[int]:
    """A label per row, drawn from that row's own distribution.

    Sampling rather than arg-max is what makes the model calibrated, which is what
    keeps 1 - p(true) small enough for the threshold to leave a flat row empty.
    """
    labels = []
    for row in rows:
        draw = rng.random()
        cumulative = 0.0
        chosen = len(row) - 1
        for index, probability in enumerate(row):
            cumulative += probability
            if draw < cumulative:
                chosen = index
                break
        labels.append(chosen)
    return labels


def _conformal_classification_case(fx: dict, frozen_classifier, split_classifier) -> dict:
    """One LAC case: MAPIE's prediction sets, asserted against the threshold rule."""
    proba = fx["proba"]
    classes = fx["class_count"]
    n = fx[CALIB_SIZE]
    labels = _conformal_labels(SeededRandom(SEED + 44100), proba[:n])

    scores = [1.0 - proba[i][labels[i]] for i in range(n)]
    ceiling_k, ceiling_q = _conformal_quantile(scores, fx["alpha"])
    k, q = _mapie_classification_quantile(scores, fx["alpha"])
    if fx.get(BETWEEN):
        assert q > ceiling_q, f"{fx['name']}: the two rules agree, so the case proves nothing"
        # A row whose first class clears MAPIE's threshold and not the ceiling rule's.
        first = 1.0 - ((q + ceiling_q) / 2.0)
        proba = proba[:-1] + [[first, (1.0 - first) / 2.0, (1.0 - first) / 2.0]]
    else:
        assert q == ceiling_q, f"{fx['name']}: the rules disagree on a case frozen as agreeing"

    estimator = frozen_classifier(table=np.array(proba), n_classes=classes).fit(np.zeros((1, 1)))
    mapie = split_classifier(
        estimator=estimator, confidence_level=1.0 - fx["alpha"],
        conformity_score="lac", prefit=True)
    mapie.conformalize(np.arange(n).reshape(-1, 1), np.array(labels))
    _, sets = mapie.predict_set(np.arange(n, len(proba)).reshape(-1, 1))

    test = proba[n:]
    mine = [[1 if p >= 1.0 - q else 0 for p in row] for row in test]
    assert np.array_equal(sets[:, :, 0].astype(int), np.array(mine)), fx["name"]
    if fx.get(BETWEEN):
        ceiling = [[1 if p >= 1.0 - ceiling_q else 0 for p in row] for row in test]
        assert ceiling != mine, f"{fx['name']}: no test row separates the two rules"
    if fx["empty"]:
        assert any(sum(row) == 0 for row in mine), f"{fx['name']}: no empty set to freeze"

    return {
        "name": fx["name"], "alpha": fx["alpha"], "class_count": classes,
        "calib_proba": [p for row in proba[:n] for p in row],
        "calib_labels": labels,
        "scores": scores, "k": k, QUANTILE: q,
        "ceiling_k": ceiling_k, CEILING_QUANTILE: ceiling_q,
        "test_count": len(test),
        "test_proba": [p for row in test for p in row],
        "sets": [flag for row in mine for flag in row],
    }


def _conformal_normalised_fixtures() -> list[dict]:
    """Calibration splits for the normalised score, where the width has to vary.

    The residual estimates are the point of every case: a constant one reduces the
    score to the absolute residual divided by a number, which would pass while
    testing nothing. These grow with the index, so the interval a point gets is
    visibly its own.
    """
    rng = SeededRandom(SEED + 683)
    y_calib = [round(rng.gauss(10.0, 3.0), 6) for _ in range(30)]
    predicted = [round(v + rng.gauss(0.0, 1.5), 6) for v in y_calib]
    # A spread that grows across the calibration set, floored well above zero so the
    # fixture says nothing about the refusal at zero -- that is an edge test's job.
    sigma = [round(0.5 + (0.15 * i), 6) for i in range(30)]
    test = [round(rng.gauss(10.0, 3.0), 6) for _ in range(6)]
    test_sigma = [0.5, 1.25, 2.0, 3.5, 5.0, 8.0]
    return [
        {"name": "thirty points, widths growing with the index, at 90 %",
         "alpha": 0.1, Y_CALIB: y_calib, Y_CALIB_PRED: predicted, CALIB_SIGMA: sigma,
         Y_TEST_PRED: test, TEST_SIGMA: test_sigma},
        {"name": "the same points and estimates at 50 %",
         "alpha": 0.5, Y_CALIB: y_calib, Y_CALIB_PRED: predicted, CALIB_SIGMA: sigma,
         Y_TEST_PRED: test, TEST_SIGMA: test_sigma},
        # Estimates on both sides of one: the score divides by r, so a small estimate
        # narrows and a large one widens, which is the direction readers get backwards.
        {"name": "estimates on both sides of one",
         "alpha": 0.2,
         Y_CALIB: [float(v) for v in range(1, 13)],
         Y_CALIB_PRED: [1.5, 2.5, 4.0, 5.0, 4.0, 5.75, 9.0, 8.5, 8.0, 9.75, 13.0, 11.5],
         CALIB_SIGMA: [0.25, 0.5, 0.75, 1.0, 1.5, 2.0, 0.25, 0.5, 0.75, 1.0, 1.5, 2.0],
         Y_TEST_PRED: [0.0, 6.25, 100.0], TEST_SIGMA: [0.1, 1.0, 10.0]},
    ]


def generate_conformal() -> dict:
    """Split conformal prediction, against MAPIE 1.5.0 (#441)."""
    from mapie.classification import SplitConformalClassifier
    from mapie.conformity_scores import ResidualNormalisedScore
    from mapie.regression import SplitConformalRegressor

    frozen_regressor, frozen_classifier = _frozen_estimators()
    quantile_cases: list[dict] = []
    regression_cases: list[dict] = []
    classification_cases: list[dict] = []

    for fx in _conformal_regression_fixtures():
        y_calib = fx[Y_CALIB]
        calib_pred = fx[Y_CALIB_PRED]
        test_pred = fx[Y_TEST_PRED]
        n = len(y_calib)
        scores = [abs(t - p) for t, p in zip(y_calib, calib_pred)]
        k, q = _conformal_quantile(scores, fx["alpha"])

        # No numpy cross-check here: np.quantile at level (1 - alpha)(n + 1)/n is a
        # different rule (see decision 0070). MAPIE below is the reference.
        estimator = frozen_regressor(table=np.array(calib_pred + test_pred)).fit(np.zeros((1, 1)))
        mapie = SplitConformalRegressor(
            estimator=estimator, confidence_level=1.0 - fx["alpha"], prefit=True)
        mapie.conformalize(np.arange(n).reshape(-1, 1), np.array(y_calib))
        _, interval = mapie.predict_interval(np.arange(n, n + len(test_pred)).reshape(-1, 1))

        lower = [p - q for p in test_pred]
        upper = [p + q for p in test_pred]
        assert np.allclose(interval[:, 0, 0], lower, rtol=0, atol=1e-12), fx["name"]
        assert np.allclose(interval[:, 1, 0], upper, rtol=0, atol=1e-12), fx["name"]

        quantile_cases.append({
            "name": fx["name"], "alpha": fx["alpha"], "scores": scores, "k": k, QUANTILE: q})
        regression_cases.append({
            "name": fx["name"], "alpha": fx["alpha"], Y_CALIB: y_calib,
            Y_CALIB_PRED: calib_pred, QUANTILE: q, Y_TEST_PRED: test_pred,
            LOWER: lower, UPPER: upper})

    normalised_cases: list[dict] = []
    for fx in _conformal_normalised_fixtures():
        y_calib = fx[Y_CALIB]
        calib_pred = fx[Y_CALIB_PRED]
        test_pred = fx[Y_TEST_PRED]
        calib_sigma = fx[CALIB_SIGMA]
        test_sigma = fx[TEST_SIGMA]
        n = len(y_calib)
        scores = [abs(y - p) / s for y, p, s in zip(y_calib, calib_pred, calib_sigma)]
        k, q = _conformal_quantile(scores, fx["alpha"])

        # Two frozen tables. The residual estimator's predict returns the residual and
        # not its log, which is MAPIE's prefit contract and its own warning's subject.
        estimator = frozen_regressor(table=np.array(calib_pred + test_pred)).fit(np.zeros((1, 1)))
        residual = frozen_regressor(table=np.array(calib_sigma + test_sigma)).fit(np.zeros((1, 1)))
        mapie = SplitConformalRegressor(
            estimator=estimator,
            confidence_level=1.0 - fx["alpha"],
            conformity_score=ResidualNormalisedScore(
                residual_estimator=residual, prefit=True, sym=True),
            prefit=True)
        mapie.conformalize(np.arange(n).reshape(-1, 1), np.array(y_calib))
        _, interval = mapie.predict_interval(np.arange(n, n + len(test_pred)).reshape(-1, 1))

        lower = [p - (q * s) for p, s in zip(test_pred, test_sigma)]
        upper = [p + (q * s) for p, s in zip(test_pred, test_sigma)]
        assert np.allclose(interval[:, 0, 0], lower, rtol=0, atol=1e-12), fx["name"]
        assert np.allclose(interval[:, 1, 0], upper, rtol=0, atol=1e-12), fx["name"]

        quantile_cases.append({
            "name": fx["name"], "alpha": fx["alpha"], "scores": scores, "k": k, QUANTILE: q})
        normalised_cases.append({
            "name": fx["name"], "alpha": fx["alpha"], Y_CALIB: y_calib,
            Y_CALIB_PRED: calib_pred, CALIB_SIGMA: calib_sigma, QUANTILE: q,
            Y_TEST_PRED: test_pred, TEST_SIGMA: test_sigma, LOWER: lower, UPPER: upper})

    for fx in _conformal_classification_fixtures():
        case = _conformal_classification_case(fx, frozen_classifier, SplitConformalClassifier)
        classification_cases.append(case)
        # The quantile section is the ceiling rule's, which is what Quantile answers by default.
        quantile_cases.append({
            "name": case["name"], "alpha": case["alpha"], "scores": case["scores"],
            "k": case["ceiling_k"], QUANTILE: case[CEILING_QUANTILE]})

    return {
        "metadata": {"library": "mapie", "version": version("mapie"),
                     "count": len(regression_cases) + len(normalised_cases)
                     + len(classification_cases)},
        QUANTILE: quantile_cases,
        "regression": regression_cases,
        "normalised": normalised_cases,
        "classification": classification_cases,
    }


# --- Sparse-dense products (#440) -----------------------------------------


def _sparse_matmul_fixtures() -> list[dict]:
    """CSR matrices paired with a dense block, including the shapes that hide bugs."""
    rng = SeededRandom(SEED + 440)
    cases = [
        # Row-major by hand, so the layout the C# reads is visible in the fixture.
        {"name": "a gap in the first row", "rows": 2, COLUMNS: 3, WIDTH: 2,
         DENSE: [[1.0, 0.0, 2.0], [0.0, 3.0, 0.0]]},
        # An empty row: the row pointers repeat, and that result row must stay zero.
        {"name": "an empty row", "rows": 3, COLUMNS: 3, WIDTH: 4,
         DENSE: [[1.0, 2.0, 0.0], [0.0, 0.0, 0.0], [0.0, 0.0, 5.0]]},
        # An all-zero column: the transposed product must still produce its row.
        {"name": "a column nothing touches", "rows": 2, COLUMNS: 4, WIDTH: 3,
         DENSE: [[1.0, 0.0, 0.0, 2.0], [0.0, 0.0, 3.0, 0.0]]},
        {"name": "one column of block", "rows": 3, COLUMNS: 3, WIDTH: 1,
         DENSE: [[1.0, 0.0, 2.0], [0.0, 3.0, 0.0], [4.0, 0.0, 5.0]]},
        {"name": "no non-zeros at all", "rows": 2, COLUMNS: 2, WIDTH: 2,
         DENSE: [[0.0, 0.0], [0.0, 0.0]]},
    ]
    # One larger, denser case, so the small hand-written ones are not the whole corpus.
    rows, columns = 12, 9
    dense = [[round(rng.uniform(-4.0, 4.0), 6) if rng.random() < 0.35 else 0.0
              for _ in range(columns)] for _ in range(rows)]
    cases.append({"name": "twelve by nine at a third dense", "rows": rows,
                  COLUMNS: columns, WIDTH: 5, DENSE: dense})
    return cases


def generate_sparse_matmul() -> dict:
    """The two sparse-dense products, against scipy (#440)."""
    from scipy import sparse

    rng = SeededRandom(SEED + 4400)
    cases = []
    for fx in _sparse_matmul_fixtures():
        matrix = sparse.csr_matrix(np.array(fx[DENSE], dtype=float))
        width = fx[WIDTH]
        block = np.array([[round(rng.uniform(-3.0, 3.0), 6) for _ in range(width)]
                          for _ in range(fx[COLUMNS])])
        transpose_block = np.array([[round(rng.uniform(-3.0, 3.0), 6) for _ in range(width)]
                                    for _ in range(fx["rows"])])
        cases.append({
            "name": fx["name"],
            "rows": fx["rows"], COLUMNS: fx[COLUMNS],
            "values": [float(v) for v in matrix.data],
            "column_indices": [int(i) for i in matrix.indices],
            "row_pointers": [int(i) for i in matrix.indptr],
            "block_columns": width,
            "block": [stable(v) for v in block.ravel()],
            "product": [stable(v) for v in (matrix @ block).ravel()],
            "transpose_block": [stable(v) for v in transpose_block.ravel()],
            "transpose_product": [stable(v) for v in (matrix.T @ transpose_block).ravel()],
        })

    return {"metadata": {"library": "scipy", "version": version("scipy"),
                         "count": len(cases)},
            "cases": cases}


# --- Dense kernels for Lodestar.Decomposition (#440) -----------------------

# Reused by three corpora below. S1192 counts a repeated JSON key like any other
# literal, and these are written once for that reason as much as for clarity.
MATRIX_KEY = "matrix"
ROWS_KEY = "rows"
COLUMNS_KEY = "columns"
# The SVD, PCA and regression corpora all name an explained variance; one spelling for three keys.
EXPLAINED_VARIANCE_KEY = "explained_variance"


def _dense_fixtures() -> list[dict]:
    """Tall-and-skinny blocks, the shape a range finder actually produces.

    Every one is full-rank on purpose. A rank-deficient block has no reference factors
    to freeze: past a vanished pivot the reflector is built from rounding noise, so
    scipy's own Q and R are a property of the host rather than of the input, and a
    corpus that stored them broke the drift gate across runners (#440). The property
    that block was there for — that the factor stays finite — is asserted directly in
    ``HouseholderQrTests`` and ``PartialPivotLuTests``, where it needs no oracle.
    """
    rng = SeededRandom(SEED + 44300)
    shapes = [(6, 3), (12, 4), (25, 10), (40, 10), (9, 9), (5, 1)]
    fixtures = []
    for rows, columns in shapes:
        values = [rng.gauss(0.0, 1.0) for _ in range(rows * columns)]
        fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: values})
    return fixtures


def generate_decomposition_qr() -> dict:
    """Economic QR, against scipy (#440).

    The factors are unique only up to a per-column sign, so the corpus freezes what
    is actually invariant: that Q has orthonormal columns, that R is upper
    triangular, and that Q @ R reproduces the input. scipy's own factors ride along
    so a divergence can be looked at, never asserted on.
    """
    from scipy import linalg

    cases = []
    for fixture in _dense_fixtures():
        rows, columns = fixture[ROWS_KEY], fixture[COLUMNS_KEY]
        a = np.array(fixture[MATRIX_KEY]).reshape(rows, columns)
        q, r = linalg.qr(a, mode="economic")
        cases.append({
            **fixture,
            "scipy_q": [settled(v) for v in q.ravel()],
            "scipy_r": [settled(v) for v in r.ravel()],
        })
    return {"metadata": {"library": "scipy", "version": version("scipy"),
                         "reference_calls": ["scipy.linalg.qr"],
                         "seed": SEED, "count": len(cases), TOLERANCE_KEY: 1e-9},
            "cases": cases}


def generate_decomposition_lu() -> dict:
    """LU with partial pivoting, against scipy (#440).

    ``permute_l=True`` is the form scikit-learn's power iteration uses: it asks for
    ``P @ L`` and throws ``U`` away. That product is unique for the full-rank blocks
    this corpus carries, so unlike the QR corpus this one asserts the factor itself as
    well as the reconstruction.
    """
    from scipy import linalg

    cases = []
    for fixture in _dense_fixtures():
        rows, columns = fixture[ROWS_KEY], fixture[COLUMNS_KEY]
        a = np.array(fixture[MATRIX_KEY]).reshape(rows, columns)
        pl, u = linalg.lu(a, permute_l=True)
        cases.append({
            **fixture,
            "permuted_lower": [settled(v) for v in pl.ravel()],
            UPPER: [settled(v) for v in u.ravel()],
        })
    return {"metadata": {"library": "scipy", "version": version("scipy"),
                         "reference_calls": ["scipy.linalg.lu"],
                         "seed": SEED, "count": len(cases), TOLERANCE_KEY: 1e-9},
            "cases": cases}


def _dense_svd_fixtures() -> list[dict]:
    """Both orientations, plus the wide-and-short shape B actually has."""
    rng = SeededRandom(SEED + 44400)
    shapes = [(6, 3), (3, 6), (10, 10), (14, 4), (4, 14), (1, 5), (5, 1)]
    fixtures = []
    for rows, columns in shapes:
        values = [rng.gauss(0.0, 1.0) for _ in range(rows * columns)]
        fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: values})
    return fixtures


def _dense_svd_cases() -> list[dict]:
    """The dense factorization on its own, so a failure in the composed algorithm
    has somewhere smaller to land."""
    from scipy import linalg

    cases = []
    for fixture in _dense_svd_fixtures():
        rows, columns = fixture[ROWS_KEY], fixture[COLUMNS_KEY]
        a = np.array(fixture[MATRIX_KEY]).reshape(rows, columns)
        u, s, vt = linalg.svd(a, full_matrices=False)
        cases.append({
            **fixture,
            "singular_values": [settled(v) for v in s],
            "scipy_u": [settled(v) for v in u.ravel()],
            "scipy_vt": [settled(v) for v in vt.ravel()],
        })
    return cases


# The randomized corpus's own keys, written once for S1192 and for the two
# readers who have to agree on them -- this generator and the C# theory.
OMEGA_KEY = "omega"
COMPONENT_COUNT_KEY = "component_count"
# Every assertion below opens with it, and S1192 counts an f-string's literal head
# like any other -- six occurrences across these generators, so it gets a name.
CASE = "case "


def _sparse_fixture(rng: SeededRandom, rows: int, columns: int, density: float) -> dict:
    """A CSR fixture, in the field names the C# side already reads elsewhere."""
    values, column_indices, row_pointers = [], [], [0]
    for _ in range(rows):
        for column in range(columns):
            if rng.random() < density:
                values.append(rng.uniform(0.1, 4.0))
                column_indices.append(column)
        row_pointers.append(len(values))
    return {
        ROWS_KEY: rows,
        COLUMNS_KEY: columns,
        "values": values,
        "column_indices": column_indices,
        "row_pointers": row_pointers,
    }


def _randomized_settings() -> list[tuple[int, int, float, int, int, int, str]]:
    """rows, columns, density, k, oversampling, power iterations, normalizer.

    Every matrix is at least as tall as it is wide, because TruncatedSVD's own
    ``transpose="auto"`` resolves to False exactly there -- and transpose is the one
    knob this package does not offer, so a wide fixture would compare two different
    factorizations. One case per normalizer, and two where k + p reaches the rank, so
    the normalizer's own factorization narrows the block the way scipy's does.
    """
    return [
        (40, 25, 0.30, 4, 6, 3, "QR"),
        (40, 25, 0.30, 4, 6, 1, "none"),
        (40, 25, 0.30, 4, 6, 5, "LU"),
        (60, 30, 0.20, 8, 10, 5, "auto"),
        (30, 12, 0.50, 3, 10, 4, "auto"),
        (25, 8, 0.60, 2, 10, 7, "QR"),
    ]


def _randomized_cases() -> list[dict]:
    """randomized_svd and TruncatedSVD over a frozen Omega.

    Omega is drawn from ``np.random.RandomState(seed)`` and the *same* seed is handed
    to scikit-learn, so the matrix stored here is bit-for-bit the one it draws first:
    ``_randomized_range_finder``'s opening call is
    ``random_state.normal(size=(n_features, k + p))``. Nothing is monkey-patched, and
    the C# side starts from the same Omega instead of reproducing MT19937.

    The signs are TruncatedSVD's, not ``randomized_svd``'s. Since 1.6 the estimator
    asks for ``flip_sign=False`` and then flips on the *right* vectors
    (``svd_flip(..., u_based_decision=False)``), while the bare function still flips on
    the left ones -- so the two disagree by a sign on four of these six fixtures.
    Re-flipping the bare function's pair on the right reproduces
    ``components_`` and ``U`` exactly, which is what is asserted below and stored here.
    """
    from scipy.sparse import csr_matrix
    from sklearn.decomposition import TruncatedSVD
    from sklearn.utils.extmath import randomized_svd, svd_flip

    rng = SeededRandom(SEED + 44500)
    cases = []
    for index, (rows, columns, density, k, p, iterations, normalizer) in enumerate(
            _randomized_settings()):
        fixture = _sparse_fixture(rng, rows, columns, density)
        a = csr_matrix(
            (fixture["values"], fixture["column_indices"], fixture["row_pointers"]),
            shape=(rows, columns))

        seed = SEED + 44600 + index
        # check_random_state takes None, an int or a RandomState and rejects a Generator,
        # and the first draw off this one has to be the Omega scikit-learn itself draws.
        omega = np.random.RandomState(seed).normal(size=(columns, k + p))  # NOSONAR S6711

        u, s, vt = randomized_svd(
            a, n_components=k, n_oversamples=p, n_iter=iterations,
            power_iteration_normalizer=normalizer, transpose=False, random_state=seed)
        u, vt = svd_flip(u, vt, u_based_decision=False)

        svd = TruncatedSVD(
            n_components=k, algorithm="randomized", n_oversamples=p, n_iter=iterations,
            power_iteration_normalizer=normalizer, random_state=seed)
        svd.fit(a)
        assert np.array_equal(vt, svd.components_), f"{CASE}{index}: components diverged"
        assert np.array_equal(s, svd.singular_values_), f"{CASE}{index}: sigma diverged"

        cases.append({
            **fixture,
            COMPONENT_COUNT_KEY: k,
            "oversampling": p,
            "power_iterations": iterations,
            "normalizer": normalizer,
            OMEGA_KEY: omega.ravel().tolist(),
            # Rides along for diagnosis the way the dense half's scipy factors do:
            # nothing in the C# suite asserts on it, because U is not reported.
            "left_singular_vectors": [settled(v) for v in u.ravel()],
            "singular_values": [settled(v) for v in s],
            "components": [settled(v) for v in vt.ravel()],
            EXPLAINED_VARIANCE_KEY: [settled(v) for v in svd.explained_variance_],
            "explained_variance_ratio": [settled(v) for v in svd.explained_variance_ratio_],
            "transform": [settled(v) for v in svd.transform(a).ravel()],
        })
    return cases


def _randomized_wide_settings() -> list[tuple[int, int, float, int, int, int, str]]:
    """rows, columns, density, k, oversampling, power iterations, normalizer.

    The mirror of _randomized_settings: every matrix here has fewer rows than columns,
    which is the shape a term-document matrix actually has and the one the corpus above
    cannot carry. The last case draws k + p past the row count, so the range finder has
    to narrow the block on the short side.
    """
    return [
        (12, 40, 0.30, 3, 6, 2, "auto"),
        (20, 50, 0.20, 5, 10, 4, "LU"),
        (8, 30, 0.40, 2, 10, 1, "QR"),
    ]


def _randomized_wide_cases() -> list[dict]:
    """randomized_svd on a wide matrix, without the estimator.

    TruncatedSVD is deliberately not called: its ``transpose="auto"`` resolves to True
    exactly here, so it would factorize the transpose and the comparison would be against
    a different factorization rather than against this package. ``randomized_svd`` with
    ``transpose=False`` is what Lodestar computes, and the right-based ``svd_flip`` the
    estimator applies is reapplied here so the signs are the ones a caller sees.

    Only the singular values and the components are frozen. The estimator's own outputs --
    explained variance, the projection -- have no reference to compare against once the
    estimator is out of the loop, and U is not reported by the C# side either.
    """
    from scipy.sparse import csr_matrix
    from sklearn.utils.extmath import randomized_svd, svd_flip

    rng = SeededRandom(SEED + 44700)
    cases = []
    for index, (rows, columns, density, k, p, iterations, normalizer) in enumerate(
            _randomized_wide_settings()):
        fixture = _sparse_fixture(rng, rows, columns, density)
        assert rows < columns, f"{CASE}{index}: the wide corpus needs rows < columns"
        a = csr_matrix(
            (fixture["values"], fixture["column_indices"], fixture["row_pointers"]),
            shape=(rows, columns))

        seed = SEED + 44800 + index
        omega = np.random.RandomState(seed).normal(size=(columns, k + p))  # NOSONAR S6711

        u, s, vt = randomized_svd(
            a, n_components=k, n_oversamples=p, n_iter=iterations,
            power_iteration_normalizer=normalizer, transpose=False, random_state=seed)
        _, vt = svd_flip(u, vt, u_based_decision=False)

        cases.append({
            **fixture,
            COMPONENT_COUNT_KEY: k,
            "oversampling": p,
            "power_iterations": iterations,
            "normalizer": normalizer,
            OMEGA_KEY: omega.ravel().tolist(),
            "singular_values": [settled(v) for v in s],
            "components": [settled(v) for v in vt.ravel()],
        })
    return cases


def generate_decomposition_svd() -> dict:
    """The dense SVD, randomized_svd composed on top of it, and the wide shape
    TruncatedSVD's transpose="auto" puts out of the estimator's reach (#440)."""
    dense, randomized = _dense_svd_cases(), _randomized_cases()
    wide = _randomized_wide_cases()
    return {"metadata": {"library": "scipy and scikit-learn",
                         "version": version("scipy"),
                         "sklearn_version": version("scikit-learn"),
                         "reference_calls": ["scipy.linalg.svd",
                                             "sklearn.utils.extmath.randomized_svd",
                                             "sklearn.decomposition.TruncatedSVD"],
                         "seed": SEED,
                         "count": len(dense) + len(randomized) + len(wide),
                         TOLERANCE_KEY: 1e-9},
            "dense": dense,
            "randomized": randomized,
            "randomized_wide": wide}


# --- NMF for Lodestar.Decomposition (#440) ---------------------------------

INITIAL_W_KEY = "initial_w"
INITIAL_H_KEY = "initial_h"
# scikit-learn's own spelling of the sparse variant, named once because the settings
# table reaches for it four times and S1192 counts those together.
NNDSVD = "nndsvd"
NNDSVDA = "nndsvda"
# scikit-learn's own spelling of the two losses solver="mu" supports, named for the
# same reason: the update settings reach for them five times between them.
FROBENIUS = "frobenius"
KULLBACK_LEIBLER = "kullback-leibler"


def _nmf_settings() -> list[tuple[int, int, float, int, str]]:
    """rows, columns, density, k, init. Tall again, for transpose='auto'.

    The last row is the only one that resolves ``n_iter='auto'`` to seven rather than
    four: 3 < 0.1 * min(60, 40). That is the ordinary shape of the data this package
    targets -- a term-document matrix is far taller and wider than the rank asked of it
    -- and the branch is unselectable from the public surface, so a caller who met a
    wrong constant there would have no knob to work around it. Without this row the
    corpus pins only the branch small fixtures happen to take.
    """
    return [
        (30, 12, 0.45, 3, NNDSVD),
        (30, 12, 0.45, 3, NNDSVDA),
        (48, 20, 0.30, 5, NNDSVD),
        (48, 20, 0.30, 5, NNDSVDA),
        (16, 6, 0.70, 2, NNDSVD),
        (60, 40, 0.20, 3, NNDSVD),
    ]


def _nmf_initialization_cases() -> list[dict]:
    """_initialize_nmf over a frozen Omega.

    It calls randomized_svd internally, so W0 and H0 depend on the seed -- measured,
    seeds 7 and 99 give different matrices. Freezing Omega is what decouples the
    initialisation from the update loop, and lets each fail on its own.

    Its randomized_svd call takes its own defaults and not TruncatedSVD's: ten
    oversamples, ``n_iter='auto'``, and ``flip_sign`` left at True -- so the
    initialisation inherits the *left*-based ``svd_flip(U, Vt)`` while the estimator
    opts out and flips on the right vectors. Both facts are asserted below rather than
    asserted in prose: the leading triplet is rebuilt from a randomized_svd called with
    the resolved iteration count and the default flip, put through the same eps snap and
    nndsvda fill, and compared to what _initialize_nmf returned. Nothing is monkey-patched.
    """
    from scipy.sparse import csr_matrix
    from sklearn.decomposition._nmf import _initialize_nmf
    from sklearn.utils.extmath import randomized_svd

    rng = SeededRandom(SEED + 44700)
    cases = []
    for index, (rows, columns, density, k, init) in enumerate(_nmf_settings()):
        fixture = _sparse_fixture(rng, rows, columns, density)
        a = csr_matrix(
            (fixture["values"], fixture["column_indices"], fixture["row_pointers"]),
            shape=(rows, columns))

        seed = SEED + 44800 + index
        # _initialize_nmf's own randomized_svd call takes n_oversamples=10 and
        # n_iter='auto'; the first draw off this RandomState is the same Omega.
        omega = np.random.RandomState(seed).normal(size=(columns, k + 10))  # NOSONAR S6711
        w, h = _initialize_nmf(a, k, init=init, random_state=seed)

        iterations = 7 if k < 0.1 * min(a.shape) else 4
        u, s, vt = randomized_svd(
            a, n_components=k, n_oversamples=10, n_iter=iterations,
            power_iteration_normalizer="auto", transpose=False, random_state=seed)
        for expected, rebuilt in ((w[:, 0], np.sqrt(s[0]) * np.abs(u[:, 0])),
                                  (h[0, :], np.sqrt(s[0]) * np.abs(vt[0, :]))):
            rebuilt[rebuilt < 1e-6] = 0
            if init == NNDSVDA:
                # rebuilt is an NNDSVD factor, non-negative, so <= 0 is the == 0 mask.
                rebuilt[rebuilt <= 0] = a.mean()
            assert np.array_equal(expected, rebuilt), f"{CASE}{index}: leading triplet diverged"

        cases.append({
            **fixture,
            COMPONENT_COUNT_KEY: k,
            "initialization": init,
            OMEGA_KEY: omega.ravel().tolist(),
            INITIAL_W_KEY: [settled(v) for v in w.ravel()],
            INITIAL_H_KEY: [settled(v) for v in h.ravel()],
        })
    return cases


def _nmf_update_settings() -> list[tuple[int, int, float, int, str, int, float, int]]:
    """rows, columns, density, k, beta loss, max_iter, tol, column of W0 to zero (-1: none).

    tol=0.0 disables the early stop, which makes the iteration count an input rather
    than a result -- asserted below, NMF then reports n_iter_ = max_iter and returns the
    identical W on two runs. One case keeps scikit-learn's default tol so the
    stopping rule itself is compared, not just the arithmetic.

    The last row is k == columns with rows >= columns, the rank scikit-learn admits and
    a bound taken from TruncatedSVD used to refuse (#519): the edge is frozen against NMF
    itself rather than argued from the C# code staying in range there.

    The row before it zeroes a column of W0 under Kullback-Leibler, which is the one place
    the two implementations are written differently: _multiplicative_update_h replaces a
    zero column sum of W with 1.0 before dividing, while this package floors every zero
    denominator to EPSILON. Both reach zero because the numerator is zero there too, and
    that is an argument rather than a measurement -- so it is measured here. The path is
    selectable from the public surface, since Fit(matrix, W0, H0) takes the W0 it is given.
    """
    return [
        (30, 12, 0.45, 3, FROBENIUS, 60, 0.0, -1),
        (30, 12, 0.45, 3, KULLBACK_LEIBLER, 60, 0.0, -1),
        (48, 20, 0.30, 5, FROBENIUS, 40, 0.0, -1),
        (48, 20, 0.30, 5, KULLBACK_LEIBLER, 40, 0.0, -1),
        (16, 6, 0.70, 2, FROBENIUS, 200, 1e-4, -1),
        (30, 12, 0.45, 3, KULLBACK_LEIBLER, 40, 0.0, 1),
        (24, 8, 0.50, 8, FROBENIUS, 40, 0.0, -1),
    ]


def _nmf_update_cases() -> list[dict]:
    """The multiplicative updates, from a frozen W0 and H0.

    The initialisation is passed in as ``init="custom"`` so this half and the
    initialisation half fail independently: a wrong W0 breaks one corpus, not both.

    Two claims the C# side depends on are asserted rather than written down: with the
    stop disabled the iteration count is the input and not a result, and the solve is
    deterministic -- a second fit from the same W0 and H0 returns the identical W. The
    reconstruction error is confronted with _beta_divergence over the reported
    components_, which is the same number the C# suite compares. Where a column of W0 is
    zeroed, that it survives every update as an exact zero is asserted too: that is what
    makes the 1.0 / EPSILON difference in the H denominator unobservable.
    """
    from scipy.sparse import csr_matrix
    from sklearn.decomposition import NMF
    from sklearn.decomposition._nmf import _beta_divergence, _initialize_nmf

    rng = SeededRandom(SEED + 44900)
    cases = []
    for index, (rows, columns, density, k, loss, iterations, tol, zeroed) in enumerate(
            _nmf_update_settings()):
        fixture = _sparse_fixture(rng, rows, columns, density)
        a = csr_matrix(
            (fixture["values"], fixture["column_indices"], fixture["row_pointers"]),
            shape=(rows, columns))

        seed = SEED + 45000 + index
        w0, h0 = _initialize_nmf(a, k, init=NNDSVDA, random_state=seed)
        # Rounded before the fit, not on the way out: W0 and H0 are the input both
        # sides start from, so the estimator has to see what the corpus carries.
        w0 = np.array([[settled(v) for v in row] for row in w0])
        h0 = np.array([[settled(v) for v in row] for row in h0])
        if zeroed >= 0:
            w0[:, zeroed] = 0.0

        model = NMF(n_components=k, init="custom", solver="mu", beta_loss=loss,
                    tol=tol, max_iter=iterations, random_state=seed)
        w = model.fit_transform(a, W=w0.copy(), H=h0.copy())

        assert np.array_equal(
            w, NMF(n_components=k, init="custom", solver="mu", beta_loss=loss, tol=tol,
                   max_iter=iterations, random_state=seed).fit_transform(
                       a, W=w0.copy(), H=h0.copy())), f"{CASE}{index}: two runs disagreed"
        if tol <= 0.0:
            assert model.n_iter_ == iterations, f"{CASE}{index}: n_iter_ is not max_iter"
        # Two independently computed floats, so the claim is the corpus's own tolerance.
        assert math.isclose(
            model.reconstruction_err_,
            _beta_divergence(a, w, model.components_, loss, square_root=True),
            rel_tol=1e-12), \
            f"{CASE}{index}: reconstruction_err_ is not the beta divergence"
        if zeroed >= 0:
            assert not w[:, zeroed].any(), f"{CASE}{index}: the zeroed column came back"
            assert not model.components_[zeroed, :].any(), \
                f"{CASE}{index}: the zeroed column's H row came back"

        cases.append({
            **fixture,
            COMPONENT_COUNT_KEY: k,
            "beta_loss": loss,
            "max_iterations": iterations,
            "tolerance": tol,
            INITIAL_W_KEY: [settled(v) for v in w0.ravel()],
            INITIAL_H_KEY: [settled(v) for v in h0.ravel()],
            "weights": [settled(v) for v in w.ravel()],
            "components": [settled(v) for v in model.components_.ravel()],
            ITERATIONS: int(model.n_iter_),
            "reconstruction_error": settled(model.reconstruction_err_),
        })
    return cases


def generate_decomposition_nmf() -> dict:
    """NNDSVD, and the multiplicative updates on top of it (#440)."""
    initialization, updates = _nmf_initialization_cases(), _nmf_update_cases()
    return {"metadata": {"library": "scikit-learn", "version": version("scikit-learn"),
                         "reference_calls": ["sklearn.decomposition._nmf._initialize_nmf",
                                             "sklearn.decomposition.NMF"],
                         "seed": SEED, "count": len(initialization) + len(updates),
                         TOLERANCE_KEY: 1e-9},
            "initialization": initialization,
            "updates": updates}


# --- The variance each principal component explains (#701) -------------------


def _pca_fixtures() -> list[dict]:
    """Tall, square and wide blocks, plus the three shapes a scree plot is read on.

    ``(5, 12)`` and ``(2, 3)`` are the ``n < p`` edge: scikit-learn keeps
    ``min(n_samples, n_features)`` components, and centring leaves the last of them at
    zero. The correlated block has a steep curve, the scaled one is dominated by its last
    column, and the one with a constant column shows that column contributing nothing.
    """
    rng = SeededRandom(SEED + 70100)
    fixtures = []
    for rows, columns in [(20, 4), (50, 10), (8, 8), (5, 12), (30, 1), (2, 3)]:
        values = [rng.gauss(0.0, 1.0) for _ in range(rows * columns)]
        fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: values})

    rows, columns = 40, 6
    correlated = []
    for _ in range(rows):
        first, second = rng.gauss(0.0, 2.0), rng.gauss(0.0, 1.0)
        loadings = [first, second, first + second, first - second, 0.5 * first, 2.0 * second]
        correlated.extend(value + rng.gauss(0.0, 0.05) for value in loadings)
    fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: correlated})

    rows, columns = 25, 5
    scaled = [rng.gauss(0.0, 1.0) * 3.0 ** (index % columns) for index in range(rows * columns)]
    fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: scaled})

    rows, columns = 15, 4
    constant = [7.0 if index % columns == 2 else rng.gauss(0.0, 1.0)
                for index in range(rows * columns)]
    fixtures.append({ROWS_KEY: rows, COLUMNS_KEY: columns, MATRIX_KEY: constant})
    return fixtures


def generate_decomposition_pca() -> dict:
    """PCA's explained variance, its ratio and the cumulative curve, against scikit-learn.

    ``svd_solver="full"`` is pinned rather than left to ``"auto"``, which picks
    ``covariance_eigh`` for tall blocks and would freeze a different computation of the
    same numbers. Only variances are frozen: a component's sign is a convention
    (``svd_flip``), and nothing this corpus asserts depends on one.
    """
    from sklearn.decomposition import PCA

    cases = []
    for fixture in _pca_fixtures():
        rows, columns = fixture[ROWS_KEY], fixture[COLUMNS_KEY]
        a = np.array(fixture[MATRIX_KEY]).reshape(rows, columns)
        model = PCA(svd_solver="full", random_state=0).fit(a)
        ratio = model.explained_variance_ratio_
        cases.append({
            **fixture,
            COMPONENT_COUNT_KEY: int(model.n_components_),
            EXPLAINED_VARIANCE_KEY: [settled(v) for v in model.explained_variance_],
            "explained_variance_ratio": [settled(v) for v in ratio],
            "cumulative_explained_variance_ratio": [settled(v) for v in np.cumsum(ratio)],
            "total_variance": settled(np.sum(model.explained_variance_)),
        })
    return {"metadata": {"library": "scikit-learn", "version": version("scikit-learn"),
                         "reference_calls": ["sklearn.decomposition.PCA"],
                         "seed": SEED, "count": len(cases), TOLERANCE_KEY: 1e-9},
            "cases": cases}

def _internal_validity_fixtures() -> list[dict]:
    """Clusterings chosen where a plausible implementation and the reference part company."""
    two_by_two = [[1.0, 2.0], [1.5, 1.8], [5.0, 8.0], [8.0, 8.0], [1.0, 0.6], [9.0, 11.0]]
    return [
        {"name": WORKED_CASE, "features": two_by_two, "labels": [0, 0, 1, 1, 0, 1]},
        # Every cluster but one holds a single sample: the widest label count either
        # metric admits, n - 1, and the one where a singleton's zero spread shows.
        {"name": "one cluster of two, the rest singletons", "features": two_by_two,
         "labels": [0, 1, 2, 3, 4, 4]},
        {"name": "a singleton beside a large cluster", "features": two_by_two,
         "labels": [0, 0, 0, 0, 0, 1]},
        {"name": "three clusters", "features": two_by_two, "labels": [0, 1, 2, 0, 1, 2]},
        # No spread at all: Calinski-Harabasz answers 1 rather than dividing by zero,
        # and Davies-Bouldin 0 because the centroids coincide.
        {"name": "four identical points", "features": [[1.0, 1.0]] * 4, "labels": [0, 0, 1, 1]},
        {"name": "two points, far apart, duplicated",
         "features": [[0.0, 0.0], [0.0, 0.0], [10.0, 10.0], [10.0, 10.0]], "labels": [0, 0, 1, 1]},
        # One feature, and five: the shape is a flat span either way.
        {"name": "one feature", "features": [[1.0], [1.2], [8.0], [8.4], [1.1], [9.0]],
         "labels": [0, 0, 1, 1, 0, 1]},
        {"name": "five features",
         "features": [[1.0, 2.0, 3.0, 4.0, 5.0], [1.1, 2.1, 3.1, 4.1, 5.1],
                      [9.0, 8.0, 7.0, 6.0, 5.0], [9.1, 8.1, 7.1, 6.1, 5.1]],
         "labels": [0, 0, 1, 1]},
    ]


def _standard_scaler_fixtures() -> list[dict]:
    """Sample matrices, each chosen for a branch of the fit rather than for variety."""
    return [
        # A constant second feature: variance is exactly zero, so the near-constant rule
        # fires on its easy end and the feature scales by 1 rather than by nothing.
        {"name": "a constant feature beside a varying one",
         "rows": [[1.0, 10.0], [2.0, 10.0], [4.0, 10.0]],
         WITH_MEAN: True, WITH_STD: True},
        # The other three option pairs on the same matrix: mean_ survives with_mean=False
        # and disappears only when both are off.
        {"name": "with_mean off, with_std on",
         "rows": [[1.0, 10.0], [2.0, 10.0], [4.0, 10.0]],
         WITH_MEAN: False, WITH_STD: True},
        {"name": "with_mean on, with_std off",
         "rows": [[1.0, 10.0], [2.0, 10.0], [4.0, 10.0]],
         WITH_MEAN: True, WITH_STD: False},
        {"name": "both off",
         "rows": [[1.0, 10.0], [2.0, 10.0], [4.0, 10.0]],
         WITH_MEAN: False, WITH_STD: False},
        # 1e8 +/- 1e-8: variance 1.48e-16, not zero, yet under the Chan-Golub-LeVeque bound.
        # An implementation testing `variance == 0` disagrees here by eight decades.
        {"name": "near constant under the two-pass error bound",
         "rows": [[1e8], [1e8 + 1e-8], [1e8 - 1e-8]],
         WITH_MEAN: True, WITH_STD: True},
        # 1e8 +/- 1e-7, one decade up: the same shape, above the bound, scaled normally.
        # The pair is what pins the threshold rather than the direction.
        {"name": "just above the same bound",
         "rows": [[1e8], [1e8 + 1e-7], [1e8 - 1e-7]],
         WITH_MEAN: True, WITH_STD: True},
        # One row: every variance is zero, so every feature is constant.
        {"name": "a single sample",
         "rows": [[3.0, -4.0, 0.0]],
         WITH_MEAN: True, WITH_STD: True},
        # Negative values and three very different spreads in one matrix.
        {"name": "mixed signs and three scales",
         "rows": [[-1.0, 100.0, 0.001], [3.0, -250.0, 0.002],
                  [-7.0, 40.0, 0.0015], [11.0, 0.0, 0.0025]],
         WITH_MEAN: True, WITH_STD: True},
    ]


def generate_preprocessing_standard_scaler() -> dict:
    """StandardScaler: the fitted statistics, the transform and its inverse (#568)."""
    import numpy as np
    from sklearn.preprocessing import StandardScaler

    def column(values) -> list | None:
        return None if values is None else [float(v) for v in values]

    cases = []
    for fixture in _standard_scaler_fixtures():
        matrix = np.array(fixture["rows"], dtype=np.float64)
        scaler = StandardScaler(
            with_mean=fixture[WITH_MEAN], with_std=fixture[WITH_STD]).fit(matrix)
        transformed = scaler.transform(matrix)
        cases.append({
            "name": fixture["name"],
            SAMPLES: [float(v) for row in fixture["rows"] for v in row],
            FEATURE_COUNT: int(matrix.shape[1]),
            WITH_MEAN: fixture[WITH_MEAN],
            WITH_STD: fixture[WITH_STD],
            "n_samples_seen": int(scaler.n_samples_seen_),
            "mean": column(scaler.mean_),
            "var": column(scaler.var_),
            "scale": column(scaler.scale_),
            "transformed": [float(v) for row in transformed for v in row],
            "inverse_transformed": [
                float(v) for row in scaler.inverse_transform(transformed) for v in row],
        })

    return {
        "metadata": {
            "algorithm": "StandardScaler",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.preprocessing.StandardScaler.fit",
                "sklearn.preprocessing.StandardScaler.transform",
                "sklearn.preprocessing.StandardScaler.inverse_transform",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# The remaining scalers' corpus (#763): the keys its cases carry beyond SAMPLES and FEATURE_COUNT,
# the three scalers it freezes, and the matrices they are fitted on.
MINMAX = "minmax"
MAXABS = "maxabs"
ROBUST = "robust"
MIXED = "mixed"
OUTLIER = "outlier"
NEAR_CONSTANT = "near_constant"
ZEROS = "zeros"
ELEVEN = "eleven"
SCALE_KEY = "scale"
UNIT_VARIANCE = "unitVariance"
SCALER = "scaler"
CLIP = "clip"
FEATURE_LOW = "featureLow"
FEATURE_HIGH = "featureHigh"
WITH_CENTRING = "withCentring"
WITH_SCALING = "withScaling"
LOWER_PERCENTILE = "lowerPercentile"
UPPER_PERCENTILE = "upperPercentile"
FITTED = "fitted"
TRANSFORMED = "transformed"
INVERSE_TRANSFORMED = "inverseTransformed"
UNSEEN = "unseen"
UNSEEN_TRANSFORMED = "unseenTransformed"


def _scaler_matrices() -> dict[str, list[list[float]]]:
    """The matrices the scaler cases are fitted on, each chosen for a branch rather than for variety."""
    return {
        # A constant second feature (range 0) beside a varying one, and a negative column so
        # MaxAbs has something whose maximum absolute value is not its maximum.
        MIXED: [[1.0, 10.0, -4.0], [2.0, 10.0, -1.0], [4.0, 10.0, -9.0], [8.0, 10.0, -2.0]],
        # One outlier three decades out: the whole reason RobustScaler exists, and the case
        # where its answer and StandardScaler's diverge by more than rounding.
        OUTLIER: [[1.0], [2.0], [3.0], [4.0], [5.0], [6.0], [7.0], [8.0], [9.0], [5000.0]],
        # The pair that pins the near-constant rule: a range of 1.11e-15, below 10*eps and not
        # zero, floored to 1; and one of 4.00e-15, just above it, divided by (2.5e14).
        NEAR_CONSTANT: [[1.0, 1.0], [1.0 + 1e-15, 1.0 + 4e-15], [1.0, 1.0 + 2e-15]],
        # Eleven rows, so the quartiles of 0..10 fall exactly on an index (h = 2.5 and 7.5
        # interpolate, h at the median is 5 and does not) -- both branches of type 7.
        ELEVEN: [[float(v)] for v in range(11)],
        # All zeros: MaxAbs divides by 1, MinMax puts the feature on the bottom of the range.
        ZEROS: [[0.0, 3.0], [0.0, 6.0], [0.0, 9.0]],
    }


def _scaler_fixtures() -> list[dict]:
    """One fixture per (scaler, option) branch the three scalers have."""
    fixtures = []
    for name in (MIXED, NEAR_CONSTANT, "zeros"):
        fixtures.append({"name": f"minmax [0, 1] on {name}", SCALER: MINMAX, "matrix": name,
                         FEATURE_LOW: 0.0, FEATURE_HIGH: 1.0, CLIP: False})
    fixtures += [
        {"name": "minmax [-5, 3] on mixed", SCALER: MINMAX, "matrix": MIXED,
         FEATURE_LOW: -5.0, FEATURE_HIGH: 3.0, CLIP: False},
        # Clipping only shows on a value the fit never saw, which is why every case carries
        # an unseen row beside the fitted matrix.
        {"name": "minmax [0, 1] clipped on mixed", SCALER: MINMAX, "matrix": MIXED,
         FEATURE_LOW: 0.0, FEATURE_HIGH: 1.0, CLIP: True},
        {"name": "minmax [-5, 3] clipped on outlier", SCALER: MINMAX, "matrix": OUTLIER,
         FEATURE_LOW: -5.0, FEATURE_HIGH: 3.0, CLIP: True},
        {"name": "maxabs on mixed", SCALER: MAXABS, "matrix": MIXED, CLIP: False},
        {"name": "maxabs on zeros", SCALER: MAXABS, "matrix": ZEROS, CLIP: False},
        {"name": "maxabs on near constant", SCALER: MAXABS, "matrix": NEAR_CONSTANT, CLIP: False},
        {"name": "maxabs clipped on outlier", SCALER: MAXABS, "matrix": OUTLIER, CLIP: True},
        {"name": "robust quartiles on outlier", SCALER: ROBUST, "matrix": OUTLIER,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust quartiles on eleven", SCALER: ROBUST, "matrix": ELEVEN,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust deciles on outlier", SCALER: ROBUST, "matrix": OUTLIER,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 10.0, UPPER_PERCENTILE: 90.0},
        {"name": "robust without centring on mixed", SCALER: ROBUST, "matrix": MIXED,
         WITH_CENTRING: False, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust without scaling on mixed", SCALER: ROBUST, "matrix": MIXED,
         WITH_CENTRING: True, WITH_SCALING: False, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust neither on mixed", SCALER: ROBUST, "matrix": MIXED,
         WITH_CENTRING: False, WITH_SCALING: False, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust quartiles on near constant", SCALER: ROBUST, "matrix": NEAR_CONSTANT,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0},
        {"name": "robust on a single sample", SCALER: ROBUST, "matrix": ZEROS,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 0.0, UPPER_PERCENTILE: 100.0},
        # unit_variance divides the range by the normal quantiles of the percentile pair: 1.3489795
        # at the quartiles, 2.5631031 at the deciles.
        {"name": "robust quartiles with unit variance", SCALER: ROBUST, "matrix": OUTLIER,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0,
         UNIT_VARIANCE: True},
        {"name": "robust deciles with unit variance", SCALER: ROBUST, "matrix": OUTLIER,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 10.0, UPPER_PERCENTILE: 90.0,
         UNIT_VARIANCE: True},
        # The floor and unit variance together: floored to 1 first, then divided, so the feature
        # the floor caught comes out at 1/1.3489795 rather than at 1.
        {"name": "robust with unit variance on near constant", SCALER: ROBUST, "matrix": NEAR_CONSTANT,
         WITH_CENTRING: True, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0,
         UNIT_VARIANCE: True},
        {"name": "robust with unit variance and no centring", SCALER: ROBUST, "matrix": MIXED,
         WITH_CENTRING: False, WITH_SCALING: True, LOWER_PERCENTILE: 25.0, UPPER_PERCENTILE: 75.0,
         UNIT_VARIANCE: True},
    ]
    return fixtures


def generate_preprocessing_scalers() -> dict:
    """MinMaxScaler, MaxAbsScaler and RobustScaler: the statistics, the transform and its inverse (#763).

    long-comment: why each case carries an unseen row as well as the fitted matrix.
    Clipping cannot be seen on the matrix a scaler was fitted on -- every fitted value is inside the
    range by construction. The unseen row is one decade outside it on each feature, so the clipped
    and unclipped cases differ in the corpus rather than only in the prose.
    """
    import numpy as np
    from sklearn.preprocessing import MaxAbsScaler, MinMaxScaler, RobustScaler

    def column(values) -> list | None:
        return None if values is None else [float(v) for v in values]

    def flat(rows) -> list:
        return [float(v) for row in rows for v in row]

    matrices = _scaler_matrices()
    cases = []
    for fixture in _scaler_fixtures():
        rows = matrices[fixture["matrix"]]
        matrix = np.array(rows, dtype=np.float64)
        # Ten times the largest value of each feature, and the negative of it: outside every
        # fitted range on both sides, whatever the matrix.
        unseen = np.array([[10.0 * abs(v) + 1.0 for v in matrix.max(axis=0)],
                           [-10.0 * abs(v) - 1.0 for v in matrix.max(axis=0)]], dtype=np.float64)

        if fixture[SCALER] == MINMAX:
            scaler = MinMaxScaler(
                feature_range=(fixture[FEATURE_LOW], fixture[FEATURE_HIGH]), clip=fixture[CLIP]).fit(matrix)
            fitted = {
                "dataMinimum": column(scaler.data_min_),
                "dataMaximum": column(scaler.data_max_),
                "dataRange": column(scaler.data_range_),
                SCALE_KEY: column(scaler.scale_),
                "minimum": column(scaler.min_),
            }
        elif fixture[SCALER] == MAXABS:
            scaler = MaxAbsScaler(clip=fixture[CLIP]).fit(matrix)
            fitted = {"maximumAbsolute": column(scaler.max_abs_), SCALE_KEY: column(scaler.scale_)}
        else:
            scaler = RobustScaler(
                with_centering=fixture[WITH_CENTRING],
                with_scaling=fixture[WITH_SCALING],
                quantile_range=(fixture[LOWER_PERCENTILE], fixture[UPPER_PERCENTILE]),
                unit_variance=fixture.get(UNIT_VARIANCE, False)).fit(matrix)
            fitted = {"centre": column(scaler.center_), SCALE_KEY: column(scaler.scale_)}

        transformed = scaler.transform(matrix)
        case = {
            "name": fixture["name"],
            SCALER: fixture[SCALER],
            SAMPLES: flat(rows),
            FEATURE_COUNT: int(matrix.shape[1]),
            FITTED: fitted,
            TRANSFORMED: flat(transformed),
            INVERSE_TRANSFORMED: flat(scaler.inverse_transform(transformed)),
            UNSEEN: flat(unseen),
            UNSEEN_TRANSFORMED: flat(scaler.transform(unseen)),
        }
        for key in (CLIP, FEATURE_LOW, FEATURE_HIGH, WITH_CENTRING, WITH_SCALING,
                    LOWER_PERCENTILE, UPPER_PERCENTILE, UNIT_VARIANCE):
            if key in fixture:
                case[key] = fixture[key]
        cases.append(case)

    return {
        "metadata": {
            "algorithm": "MinMaxScaler, MaxAbsScaler, RobustScaler",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.preprocessing.MinMaxScaler.fit",
                "sklearn.preprocessing.MaxAbsScaler.fit",
                "sklearn.preprocessing.RobustScaler.fit",
                "sklearn.preprocessing.MinMaxScaler.transform",
                "sklearn.preprocessing.MinMaxScaler.inverse_transform",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# The encoder and imputer corpus (#764): the keys its cases carry, and the calls it freezes.
ONEHOT = "onehot"
ORDINAL = "ordinal"
IMPUTE = "impute"
DROP_FIRST = "first"
DROP_IF_BINARY = "if_binary"
HANDLE_IGNORE = "ignore"
HANDLE_ERROR = "error"
ENCODER = "encoder"
VALUES = "values"
CATEGORIES = "categories"
ENCODED = "encoded"
UNSEEN = "unseen"
UNSEEN_VALUES = "unseenValues"
UNSEEN_ENCODED = "unseenEncoded"
ELEMENT_TYPE = "elementType"
ELEMENT_STRING = "string"
ELEMENT_INT = "int"
DROP = "drop"
HANDLE_UNKNOWN = "handleUnknown"
STRATEGY = "strategy"
FILL_VALUE = "fillValue"
KEEP_EMPTY_FEATURES = "keepEmptyFeatures"
STATISTICS = "statistics"
IMPUTED = "imputed"
STRATEGY_MEAN = "mean"
STRATEGY_MEDIAN = "median"
STRATEGY_MOST_FREQUENT = "most_frequent"
STRATEGY_CONSTANT = "constant"


def _encoder_fixtures() -> list[dict]:
    """One fixture per branch the two encoders have, rather than per shape."""
    letters = [["b", "x"], ["a", "y"], ["c", "x"], ["a", "y"]]
    binary = [["y"], ["n"], ["y"], ["y"]]
    three = [["a"], ["b"], ["c"], ["a"]]
    return [
        # Two features at once, so the column layout is pinned: the second feature's
        # columns follow the first's, each in its own sorted order.
        {"name": "two string features", ENCODER: ONEHOT, VALUES: letters,
         DROP: None, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["a", "x"]]},
        {"name": "two string features, drop first", ENCODER: ONEHOT, VALUES: letters,
         DROP: DROP_FIRST, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["a", "x"]]},
        # if_binary drops the two-category feature and keeps the three-category one whole,
        # which is the pair that separates it from `first`.
        {"name": "binary feature, drop if binary", ENCODER: ONEHOT, VALUES: binary,
         DROP: DROP_IF_BINARY, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["n"]]},
        {"name": "three categories, drop if binary keeps them", ENCODER: ONEHOT, VALUES: three,
         DROP: DROP_IF_BINARY, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["b"]]},
        # An unknown encodes to all zeros, the row a dropped first category also gives.
        {"name": "unknown ignored, all zeros", ENCODER: ONEHOT, VALUES: three,
         DROP: None, HANDLE_UNKNOWN: HANDLE_IGNORE, UNSEEN: [["zzz"], ["a"]]},
        # numpy sorts strings by code point where .NET's default comparison is culture-sensitive:
        # ordinal puts 'B' before 'a', a culture the reverse, and the columns differ.
        {"name": "mixed case sorts by code point", ENCODER: ONEHOT,
         VALUES: [["B"], ["a"], ["b"], ["A"]],
         DROP: None, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["a"]]},
        {"name": "integers sort as numbers", ENCODER: ONEHOT, VALUES: [[10], [2], [33], [2]],
         DROP: None, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [[33]]},
        {"name": "ordinal over two string features", ENCODER: ORDINAL, VALUES: letters,
         DROP: None, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [["c", "y"]]},
        {"name": "ordinal over integers", ENCODER: ORDINAL, VALUES: [[10], [2], [33], [2]],
         DROP: None, HANDLE_UNKNOWN: HANDLE_ERROR, UNSEEN: [[10]]},
    ]


def _imputer_fixtures() -> list[dict]:
    """One fixture per strategy, plus the two the strategies disagree about."""
    nan = float("nan")
    return [
        {"name": "mean over two features", STRATEGY: STRATEGY_MEAN,
         VALUES: [[1.0, 10.0], [2.0, nan], [nan, 30.0], [5.0, 40.0]]},
        # An even count, where the median is the average of the two middle values.
        {"name": "median of an even count", STRATEGY: STRATEGY_MEDIAN,
         VALUES: [[1.0], [2.0], [3.0], [4.0], [nan]]},
        {"name": "median of an odd count", STRATEGY: STRATEGY_MEDIAN,
         VALUES: [[1.0], [2.0], [3.0], [nan]]},
        # 1 and 2 both appear twice: the reference fills with the smaller.
        {"name": "most frequent breaks a tie downward", STRATEGY: STRATEGY_MOST_FREQUENT,
         VALUES: [[1.0], [1.0], [2.0], [2.0], [nan]]},
        {"name": "most frequent without a tie", STRATEGY: STRATEGY_MOST_FREQUENT,
         VALUES: [[3.0], [3.0], [3.0], [7.0], [nan]]},
        {"name": "constant at its default of zero", STRATEGY: STRATEGY_CONSTANT,
         VALUES: [[1.0], [nan], [3.0]]},
        {"name": "constant at a value the caller chose", STRATEGY: STRATEGY_CONSTANT,
         FILL_VALUE: -1.0, VALUES: [[1.0], [nan], [3.0]]},
        # #894: a kept empty feature is filled with zero, except under constant, which uses its fill value.
        {"name": "mean keeps an empty feature at zero", STRATEGY: STRATEGY_MEAN, KEEP_EMPTY_FEATURES: True,
         VALUES: [[1.0, nan], [nan, nan], [3.0, nan]]},
        {"name": "constant keeps an empty feature at its fill value", STRATEGY: STRATEGY_CONSTANT,
         FILL_VALUE: 7.0, KEEP_EMPTY_FEATURES: True, VALUES: [[1.0, nan], [nan, nan], [3.0, nan]]},
    ]


def generate_preprocessing_encoders() -> dict:
    """OneHotEncoder, OrdinalEncoder and SimpleImputer: categories, layout and fills (#764).

    long-comment: why every encoder case carries a row the fit never saw.
    The branches that matter -- an unknown value, a dropped category -- are invisible on the
    matrix the encoder was fitted on, where every value is known and every category present.
    The unseen rows are where `handle_unknown` and `drop` differ from each other.
    """
    import numpy as np
    from sklearn.impute import SimpleImputer
    from sklearn.preprocessing import OneHotEncoder, OrdinalEncoder

    cases = []
    for fixture in _encoder_fixtures():
        rows = fixture[VALUES]
        matrix = np.array(rows)
        unseen = np.array(fixture[UNSEEN])
        if fixture[ENCODER] == ONEHOT:
            encoder = OneHotEncoder(
                sparse_output=False, drop=fixture[DROP], handle_unknown=fixture[HANDLE_UNKNOWN]).fit(matrix)
        else:
            encoder = OrdinalEncoder().fit(matrix)

        encoded = encoder.transform(matrix)
        # An integer column sorts as numbers and a string column by code point, which is why
        # the corpus keeps the type rather than stringifying it.
        integral = isinstance(rows[0][0], int)
        flatten = (lambda grid: [int(v) for row in grid for v in row]) if integral else (
            lambda grid: [str(v) for row in grid for v in row])
        cases.append({
            "name": fixture["name"],
            ENCODER: fixture[ENCODER],
            ELEMENT_TYPE: ELEMENT_INT if integral else ELEMENT_STRING,
            VALUES: flatten(rows),
            FEATURE_COUNT: int(matrix.shape[1]),
            DROP: fixture[DROP],
            HANDLE_UNKNOWN: fixture[HANDLE_UNKNOWN],
            CATEGORIES: [[int(v) if integral else str(v) for v in feature] for feature in encoder.categories_],
            ENCODED: [float(v) for row in encoded for v in row],
            "encodedFeatureCount": int(encoded.shape[1]),
            UNSEEN_VALUES: flatten(fixture[UNSEEN]),
            UNSEEN_ENCODED: [float(v) for row in encoder.transform(unseen) for v in row],
        })

    for fixture in _imputer_fixtures():
        rows = fixture[VALUES]
        matrix = np.array(rows, dtype=np.float64)
        keep = fixture.get(KEEP_EMPTY_FEATURES, False)
        imputer = SimpleImputer(
            strategy=fixture[STRATEGY], fill_value=fixture.get(FILL_VALUE), keep_empty_features=keep).fit(matrix)
        # The key is written only where it is set, so the cases frozen before #894 stay byte-identical.
        kept = {KEEP_EMPTY_FEATURES: True} if keep else {}
        cases.append({
            "name": fixture["name"],
            ENCODER: IMPUTE,
            # A literal NaN marks a missing value and main() writes with allow_nan=False, so it
            # takes the nan_policy fixtures' spelling: "NaN" as a string, which StatsCorpus reads.
            SAMPLES: _stats_nan_list([v for row in rows for v in row]),
            FEATURE_COUNT: int(matrix.shape[1]),
            STRATEGY: fixture[STRATEGY],
            FILL_VALUE: fixture.get(FILL_VALUE),
            STATISTICS: [float(v) for v in imputer.statistics_],
            IMPUTED: [float(v) for row in imputer.transform(matrix) for v in row],
            **kept,
        })

    return {
        "metadata": {
            "algorithm": "OneHotEncoder, OrdinalEncoder, SimpleImputer",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.preprocessing.OneHotEncoder.fit",
                "sklearn.preprocessing.OrdinalEncoder.fit",
                "sklearn.impute.SimpleImputer.fit",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# The incremental-fit corpus (#765): the keys its cases carry, and the scalers it freezes.
STANDARD = "standard"
BATCHES = "batches"
WHOLE_MEAN = "wholeMean"
WHOLE_VARIANCE = "wholeVariance"
WHOLE_SCALE = "wholeScale"
INCREMENTAL_MEAN = "incrementalMean"
INCREMENTAL_VARIANCE = "incrementalVariance"
INCREMENTAL_SCALE = "incrementalScale"
SEEN = "samplesSeen"
DATA_MINIMUM = "dataMinimum"
DATA_MAXIMUM = "dataMaximum"
MAXIMUM_ABSOLUTE = "maximumAbsolute"


def _partial_fit_fixtures() -> list[dict]:
    """Batch splits, each chosen for a branch rather than for size."""
    rng = SeededRandom(SEED + 765)
    wide = [[round(rng.gauss(5.0, 2.0), 6), round(rng.gauss(-3.0, 1.0), 6)] for _ in range(24)]
    return [
        # Uneven batches: the update weights each by its own count, where equal ones would
        # hide an implementation that averaged the two means.
        {"name": "two uneven batches", SCALER: STANDARD, BATCHES: [wide[:7], wide[7:]]},
        {"name": "three uneven batches", SCALER: STANDARD, BATCHES: [wide[:5], wide[5:17], wide[17:]]},
        # One row at a time is where a variance update divides by a count of one.
        {"name": "single-row batches", SCALER: STANDARD, BATCHES: [[row] for row in wide[:4]]},
        {"name": "two uneven batches, min-max", SCALER: MINMAX, BATCHES: [wide[:7], wide[7:]]},
        # A second batch inside the first's range leaves every statistic where it was.
        {"name": "a batch inside the range", SCALER: MINMAX,
         BATCHES: [wide[:12], [[1.0, -3.0], [2.0, -3.5]]]},
        {"name": "two uneven batches, max-abs", SCALER: MAXABS, BATCHES: [wide[:7], wide[7:]]},
    ]


def generate_preprocessing_partial_fit() -> dict:
    """partial_fit over batches against fit on the concatenation, for the three scalers (#765).

    long-comment: why every case carries both answers rather than one.
    The claim is not that the incremental statistics are right on their own; it is that they are the
    ones a single fit over the concatenated batches gives. Freezing both and comparing them is what
    states that, and an implementation that averages batch means passes neither.
    """
    import numpy as np
    from sklearn.preprocessing import MaxAbsScaler, MinMaxScaler, StandardScaler

    def column(values) -> list:
        return [float(v) for v in values]

    cases = []
    for fixture in _partial_fit_fixtures():
        batches = [np.array(batch, dtype=np.float64) for batch in fixture[BATCHES]]
        whole = np.vstack(batches)
        kind = fixture[SCALER]
        incremental = {STANDARD: StandardScaler, MINMAX: MinMaxScaler, MAXABS: MaxAbsScaler}[kind]()
        for batch in batches:
            incremental.partial_fit(batch)

        fitted = {STANDARD: StandardScaler, MINMAX: MinMaxScaler, MAXABS: MaxAbsScaler}[kind]().fit(whole)
        case = {
            "name": fixture["name"],
            SCALER: kind,
            BATCHES: [[float(v) for row in batch for v in row] for batch in fixture[BATCHES]],
            FEATURE_COUNT: int(whole.shape[1]),
            SEEN: int(np.asarray(incremental.n_samples_seen_).reshape(-1)[0]),
            INCREMENTAL_SCALE: column(incremental.scale_),
            WHOLE_SCALE: column(fitted.scale_),
        }
        if kind == STANDARD:
            case[INCREMENTAL_MEAN] = column(incremental.mean_)
            case[WHOLE_MEAN] = column(fitted.mean_)
            case[INCREMENTAL_VARIANCE] = column(incremental.var_)
            case[WHOLE_VARIANCE] = column(fitted.var_)
        elif kind == MINMAX:
            case[DATA_MINIMUM] = column(incremental.data_min_)
            case[DATA_MAXIMUM] = column(incremental.data_max_)
        else:
            case[MAXIMUM_ABSOLUTE] = column(incremental.max_abs_)

        cases.append(case)

    return {
        "metadata": {
            "algorithm": "StandardScaler, MinMaxScaler, MaxAbsScaler partial_fit",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.preprocessing.StandardScaler.partial_fit",
                "sklearn.preprocessing.MinMaxScaler.partial_fit",
                "sklearn.preprocessing.MaxAbsScaler.partial_fit",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# The sparse-fit corpus (#765): the keys its cases carry beyond the dense ones.
SPARSE_VALUES = "sparseValues"
COLUMN_INDICES = "columnIndices"
ROW_POINTERS = "rowPointers"
ROW_COUNT = "rowCount"
COLUMN_COUNT = "columnCount"
DENSE_SCALE = "denseScale"
SPARSE_SCALE = "sparseScale"
TRANSFORMED_VALUES = "transformedValues"


def _sparse_fixtures() -> list[dict]:
    """Matrices whose zeros are the point, each with a column chosen for a branch."""
    return [
        # Column 1 stores nothing and column 2 exactly one value: the floor fires on the first,
        # and the second is where a mostly-zero column's percentiles are zero.
        {"name": "a column of zeros and a column with one entry",
         "rows": [[1.0, 0.0, 2.0], [0.0, 0.0, 0.0], [3.0, 0.0, 0.0], [-4.0, 0.0, 0.0]]},
        # Dense enough that the quartiles are not all zero, so the robust scale is not floored.
        {"name": "half the entries stored",
         "rows": [[1.0, 5.0], [0.0, 6.0], [3.0, 0.0], [4.0, 8.0], [0.0, 9.0], [7.0, 0.0]]},
    ]


def generate_preprocessing_sparse() -> dict:
    """The three scalers scikit-learn fits on a sparse matrix, against the dense answer (#765).

    long-comment: why each case carries the dense answer as well as the sparse one.
    A sparse fit is not a different statistic: it is the same one, read without visiting the zeros.
    Freezing both states that, and an implementation that skipped the absent zeros in a mean -- the
    easy mistake -- would match neither.
    """
    import numpy as np
    from scipy import sparse
    from sklearn.preprocessing import MaxAbsScaler, RobustScaler, StandardScaler

    def column(values) -> list:
        return [float(v) for v in values]

    cases = []
    for fixture in _sparse_fixtures():
        dense = np.array(fixture["rows"], dtype=np.float64)
        csr = sparse.csr_matrix(dense)
        for kind, make in (
                (STANDARD, lambda: StandardScaler(with_mean=False)),
                (MAXABS, MaxAbsScaler),
                (ROBUST, lambda: RobustScaler(with_centering=False))):
            fitted = make().fit(csr)
            # Scaling alone keeps the stored positions, so the values are the whole answer (#895).
            transformed = fitted.transform(csr)
            restored = fitted.inverse_transform(transformed)
            for out in (transformed, restored):
                assert (out.indices == csr.indices).all() and (out.indptr == csr.indptr).all(), kind
            cases.append({
                "name": f"{kind}, {fixture['name']}",
                SCALER: kind,
                SPARSE_VALUES: column(csr.data),
                COLUMN_INDICES: [int(v) for v in csr.indices],
                ROW_POINTERS: [int(v) for v in csr.indptr],
                ROW_COUNT: int(dense.shape[0]),
                COLUMN_COUNT: int(dense.shape[1]),
                SAMPLES: [float(v) for row in fixture["rows"] for v in row],
                SPARSE_SCALE: column(fitted.scale_),
                DENSE_SCALE: column(make().fit(dense).scale_),
                TRANSFORMED_VALUES: column(transformed.data),
                "inverseValues": column(restored.data),
            })

    return {
        "metadata": {
            "algorithm": "StandardScaler, MaxAbsScaler, RobustScaler over a sparse matrix",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.preprocessing.StandardScaler.fit",
                "sklearn.preprocessing.MaxAbsScaler.fit",
                "sklearn.preprocessing.RobustScaler.fit",
                "transform and inverse_transform on the same CSR matrix",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


def _kmeans_fixtures() -> list[dict]:
    """Sample matrices with their starting centres, each chosen for a branch of Lloyd."""
    blobs = [[0.0, 0.0], [0.0, 1.0], [10.0, 10.0], [10.0, 11.0], [5.0, 5.0]]
    return [
        # Three separated groups from centres already on them: strict convergence, fast.
        {"name": "three blobs from centres on them", "rows": blobs,
         "init": [[0.0, 0.0], [10.0, 10.0], [5.0, 5.0]], MAX_ITER: 300, "tol": 1e-4},
        # A centre no sample is nearest to, so its cluster starts empty and the reference
        # relocates it onto the sample furthest from its own centre.
        {"name": "a cluster that starts empty", "rows": blobs,
         "init": [[0.0, 0.0], [10.0, 10.0], [-500.0, -500.0]], MAX_ITER: 300, "tol": 1e-4},
        # One iteration only, so the final assignment is what makes labels and centres
        # agree. Centres unequal and off the groups: no tie to decide (decision 0090).
        {"name": "stopped at one iteration", "rows": blobs,
         "init": [[0.3, 0.2], [1.4, 1.1], [2.6, 2.3]], MAX_ITER: 1, "tol": 1e-4},
        # tol = 0 keeps going until the labels stop moving, with no shift test at all.
        {"name": "zero tolerance runs to strict convergence", "rows": blobs,
         "init": [[0.3, 0.2], [1.4, 1.1], [2.6, 2.3]], MAX_ITER: 300, "tol": 0.0},
        # A tolerance large enough to stop on the shift rather than on the labels.
        {"name": "stopped by the scaled tolerance", "rows": blobs,
         "init": [[0.3, 0.2], [1.4, 1.1], [2.6, 2.3]], MAX_ITER: 300, "tol": 0.5},
        # Every sample its own cluster: inertia is zero and nothing moves.
        {"name": "as many clusters as samples", "rows": [[1.0], [2.0], [3.0]],
         "init": [[1.0], [2.0], [3.0]], MAX_ITER: 300, "tol": 1e-4},
        # Repeated points: a centre lands exactly on three identical samples, so that
        # cluster's inertia is zero. The two starting centres differ, so nothing ties.
        {"name": "duplicate samples", "rows": [[1.0, 1.0], [1.0, 1.0], [1.0, 1.0], [9.0, 9.0]],
         "init": [[0.5, 0.5], [8.0, 8.0]], MAX_ITER: 300, "tol": 1e-4},
        # Four features, negative values, centres deliberately off the groups.
        {"name": "four features, centres off the groups",
         "rows": [[-1.0, 2.0, -3.0, 4.0], [-1.2, 2.1, -2.9, 4.2], [8.0, -7.0, 6.0, -5.0],
                  [8.3, -6.8, 6.1, -5.2], [0.0, 0.0, 0.0, 0.0]],
         "init": [[5.0, 4.0, 5.0, 6.0], [-5.0, -4.0, -6.0, -5.0]], MAX_ITER: 300, "tol": 1e-4},
        # Two clusters empty at once (#862): each takes a distinct one of the two furthest
        # samples, the labels are left alone, and both are subtracted from the one donor.
        {"name": "two clusters empty at once", "rows": [[0.0], [1.0], [3.0], [10.0]],
         "init": [[1.0], [100.0], [200.0]], MAX_ITER: 1, "tol": 1e-4},
        # Every sample on its centre: the reference relocates nothing, and the two empty
        # clusters take the largest cluster's averaged centre.
        {"name": "every sample on one centre", "rows": [[1.0], [1.0], [1.0], [1.0]],
         "init": [[1.0], [1.0], [1.0]], MAX_ITER: 300, "tol": 1e-4},
        # The emptied donor comes before the largest cluster, so it takes that cluster's centred
        # sum. One iteration: the second relocation ties two samples, left out as in 0093.
        {"name": "a relocation that empties its donor", "rows": [[60.0], [0.0], [1.0], [2.5]],
         "init": [[50.0], [1.0], [500.0], [600.0]], MAX_ITER: 1, "tol": 1e-4},
    ]


def generate_cluster_kmeans() -> dict:
    """KMeans by Lloyd, from centres given rather than drawn (#567).

    ``inertia_`` is recomputed here from the centres and labels the fit returns, rather
    than read off the estimator. It is the same quantity -- the summed squared distance
    from each sample to its centre -- but scikit-learn accumulates it in an OpenMP
    reduction whose order varies run to run: measured on the four-feature fixture, four
    regenerations of this file gave 20.756666666666668 twice, ...675 once and ...67 once.
    Every value is within 2e-15 of the others, so the numeric gate of decision 0073 would
    never have failed on it -- but the committed file would have changed on every run for
    no reason, which is the friction that gate was written to remove rather than to hide.
    """
    import numpy as np
    from sklearn.cluster import KMeans as SkKMeans

    cases = []
    for fixture in _kmeans_fixtures():
        matrix = np.array(fixture["rows"], dtype=np.float64)
        init = np.array(fixture["init"], dtype=np.float64)
        # Nothing draws from random_state here -- init is an array and n_init=1 -- but
        # stating it says the run is deterministic rather than leaving that inferred.
        model = SkKMeans(
            n_clusters=init.shape[0], init=init, n_init=1, random_state=0,
            max_iter=fixture[MAX_ITER], tol=fixture["tol"], algorithm="lloyd").fit(matrix)
        cases.append({
            "name": fixture["name"],
            SAMPLES: [float(v) for row in fixture["rows"] for v in row],
            FEATURE_COUNT: int(matrix.shape[1]),
            "cluster_count": int(init.shape[0]),
            "initial_centres": [float(v) for row in fixture["init"] for v in row],
            MAX_ITER: fixture[MAX_ITER],
            "tol": fixture["tol"],
            "centres": [float(v) for row in model.cluster_centers_ for v in row],
            "labels": [int(v) for v in model.labels_],
            "inertia": float(((matrix - model.cluster_centers_[model.labels_]) ** 2).sum()),
            ITERATIONS: int(model.n_iter_),
        })

    return {
        "metadata": {
            "algorithm": "KMeans",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ['sklearn.cluster.KMeans(init=..., n_init=1, algorithm="lloyd")'],
            "count": len(cases),
        },
        "cases": cases,
    }


# The border fixture of #759: two dense groups whose nearest cores are 2.0 apart, and one
# point at 1.3 -- within eps of exactly one core on each side, and of too few to be core.
DBSCAN_LEFT = [0.0, 0.1, 0.2, 0.3]
DBSCAN_RIGHT = [2.3, 2.4, 2.5, 2.6]
DBSCAN_BORDER = 1.3
DBSCAN_TWO_BLOBS = "two features, two blobs and one outlier"
MIN_SAMPLES = "min_samples"
EUCLIDEAN = "euclidean"
PRECOMPUTED = "precomputed"


def _dbscan_fixtures() -> list[dict]:
    """Point sets chosen for a branch of the growth loop rather than for a picture."""
    return [
        {"name": "eps below the gap leaves every point noise",
         "rows": [[0.0], [1.0], [2.0]], "eps": 0.999, MIN_SAMPLES: 2},
        # The same three points at an eps exactly equal to the gap: `<=` makes one cluster
        # where `<` leaves three noise points. The pair is the whole test of the boundary.
        {"name": "eps exactly at the gap is inclusive",
         "rows": [[0.0], [1.0], [2.0]], "eps": 1.0, MIN_SAMPLES: 2},
        {"name": "min_samples counts the point itself",
         "rows": [[0.0], [0.25]], "eps": 1.0, MIN_SAMPLES: 2},
        {"name": "min_samples one past the neighbourhood",
         "rows": [[0.0], [0.25]], "eps": 1.0, MIN_SAMPLES: 3},
        # Four orderings of one point set. The border point takes the first-grown cluster's
        # label in all four, wherever it sits -- which no single ordering could establish.
        {"name": "a border point between two clusters",
         "rows": [[v] for v in DBSCAN_LEFT + [DBSCAN_BORDER] + DBSCAN_RIGHT],
         "eps": 1.0, MIN_SAMPLES: 4},
        {"name": "the same border point, listed first",
         "rows": [[v] for v in [DBSCAN_BORDER] + DBSCAN_LEFT + DBSCAN_RIGHT],
         "eps": 1.0, MIN_SAMPLES: 4},
        {"name": "the same border point, listed last",
         "rows": [[v] for v in DBSCAN_LEFT + DBSCAN_RIGHT + [DBSCAN_BORDER]],
         "eps": 1.0, MIN_SAMPLES: 4},
        {"name": "the same border point, the far group first",
         "rows": [[v] for v in DBSCAN_RIGHT + [DBSCAN_BORDER] + DBSCAN_LEFT],
         "eps": 1.0, MIN_SAMPLES: 4},
        # Numbering follows the first core point's index, not the group's position or size.
        {"name": "clusters numbered by first appearance",
         "rows": [[9.0], [9.1], [0.0], [0.1]], "eps": 0.5, MIN_SAMPLES: 2},
        {"name": DBSCAN_TWO_BLOBS,
         "rows": [[0.0, 0.0], [0.0, 0.3], [0.3, 0.0], [5.0, 5.0], [5.0, 5.2], [9.0, 9.0]],
         "eps": 0.5, MIN_SAMPLES: 2},
        {"name": "four features",
         "rows": [[-1.0, 2.0, -3.0, 4.0], [-1.2, 2.1, -2.9, 4.2], [-0.9, 1.9, -3.1, 3.8],
                  [8.0, -7.0, 6.0, -5.0], [8.3, -6.8, 6.1, -5.2], [0.0, 0.0, 0.0, 0.0]],
         "eps": 1.0, MIN_SAMPLES: 3},
        # Every row identical, so every point is core. An implementation that skips the
        # self-distance gets a count one short here and nowhere else.
        {"name": "duplicate rows",
         "rows": [[1.0, 1.0], [1.0, 1.0], [1.0, 1.0], [1.0, 1.0]], "eps": 0.1, MIN_SAMPLES: 3},
        {"name": "one cluster holding every row",
         "rows": [[0.0], [0.1], [0.2], [0.3]], "eps": 1.0, MIN_SAMPLES: 2},
        {"name": "a single sample, core when min_samples is one",
         "rows": [[4.0]], "eps": 1.0, MIN_SAMPLES: 1},
    ]


def generate_cluster_dbscan() -> dict:
    """DBSCAN labels, compared exactly (#759).

    A label is an integer and the algorithm is deterministic given the neighbourhood
    order, so there is nothing here for decision 0073's numeric comparison to soften: a
    case whose labels move has changed answer rather than drifted, and the corpus says so
    by carrying no tolerance at all.

    The precomputed case is the two-blob case's own distance matrix, so the corpus
    compares this package's two entry points against each other as well as against the
    reference -- which is the claim the spec makes about them.
    """
    import numpy as np
    from sklearn.cluster import DBSCAN as SkDbscan
    from sklearn.metrics import pairwise_distances

    def case(name, values, columns, fixture, model, metric):
        return {
            "name": name,
            SAMPLES: [float(v) for v in values],
            FEATURE_COUNT: columns,
            "eps": fixture["eps"],
            MIN_SAMPLES: fixture[MIN_SAMPLES],
            "metric": metric,
            "labels": [int(v) for v in model.labels_],
            "core_sample_indices": [int(v) for v in model.core_sample_indices_],
        }

    cases = []
    for fixture in _dbscan_fixtures():
        matrix = np.array(fixture["rows"], dtype=np.float64)
        model = SkDbscan(
            eps=fixture["eps"], min_samples=fixture[MIN_SAMPLES], metric=EUCLIDEAN).fit(matrix)
        cases.append(case(
            fixture["name"], [v for row in fixture["rows"] for v in row],
            int(matrix.shape[1]), fixture, model, EUCLIDEAN))

    twin = next(f for f in _dbscan_fixtures() if f["name"] == DBSCAN_TWO_BLOBS)
    distances = pairwise_distances(np.array(twin["rows"], dtype=np.float64))
    precomputed = SkDbscan(
        eps=twin["eps"], min_samples=twin[MIN_SAMPLES], metric=PRECOMPUTED).fit(distances)
    cases.append(case(
        "the same two blobs from a precomputed distance matrix",
        [v for row in distances for v in row], int(distances.shape[0]),
        twin, precomputed, PRECOMPUTED))

    return {
        "metadata": {
            "algorithm": "DBSCAN",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [f'sklearn.cluster.DBSCAN(metric="{EUCLIDEAN}")',
                                f'sklearn.cluster.DBSCAN(metric="{PRECOMPUTED}")'],
            "count": len(cases),
        },
        "cases": cases,
    }


# The agglomerative corpus of #760. Integer data is kept because it ties, and ties are the point.
SINGLE_LINKAGE = "single"
AGGLOMERATIVE_LINKAGES = ("ward", "complete", "average", SINGLE_LINKAGE)
N_CLUSTERS = "n_clusters"
DISTANCE_THRESHOLD = "distance_threshold"


def _agglomerative_fixtures() -> list[dict]:
    """Point sets chosen for how they tie, each cut at more than one count."""
    import numpy as np

    rng = np.random.default_rng(760)
    blobs = np.vstack([rng.normal(centre, 0.6, (10, 3))
                       for centre in ([0, 0, 0], [6, 6, 6], [0, 8, -4])])
    return [
        # One tie, at the first two merges: the reference takes the lower pair first.
        {"name": "a line of five, one tie", "rows": [[0.0], [1.0], [5.0], [6.0], [20.0]],
         "cuts": [2, 3]},
        # Every side tied: a different first merge is a different tree, not a different order.
        {"name": "a unit square, every side tied",
         "rows": [[0.0, 0.0], [1.0, 0.0], [0.0, 1.0], [1.0, 1.0]], "cuts": [2, 3]},
        {"name": "an evenly spaced line, every gap tied",
         "rows": [[float(i)] for i in range(6)], "cuts": [2, 4]},
        {"name": "duplicate rows", "rows": [[1.0, 1.0], [1.0, 1.0], [5.0, 5.0], [5.0, 5.0]],
         "cuts": [2, 3]},
        {"name": "a three-by-three grid", "rows": [[float(i), float(j)] for i in range(3) for j in range(3)],
         "cuts": [2, 5]},
        # Tie-free, where every implementation should agree: the control.
        {"name": "three blobs in three dimensions", "rows": blobs.tolist(), "cuts": [3, 7]},
        # Random integers dense with ties. A distance update written in the right algebra and
        # the wrong floating-point order passed every fixture above and failed these.
        {"name": "random integers, fifteen points in two dimensions",
         "rows": rng.integers(0, 5, (15, 2)).astype(float).tolist(), "cuts": [3, 6]},
        {"name": "random integers, twelve points in three dimensions",
         "rows": rng.integers(0, 4, (12, 3)).astype(float).tolist(), "cuts": [2, 4]},
    ]


def _agglomerative_case(name, rows, linkage, model, mode, value) -> dict:
    return {
        "name": name,
        SAMPLES: [float(v) for row in rows for v in row],
        FEATURE_COUNT: len(rows[0]),
        "linkage": linkage,
        "mode": mode,
        mode: value,
        "labels": [int(v) for v in model.labels_],
        "children": [int(v) for pair in model.children_ for v in pair],
        "distances": [float(v) for v in model.distances_],
        "cluster_count": int(model.n_clusters_),
    }


def generate_cluster_agglomerative() -> dict:
    """AgglomerativeClustering, labels and children compared exactly (#760).

    ``labels_`` and ``children_`` are integers and the tree is deterministic, so they carry no
    tolerance; ``distances_`` is compared at 1e-9 like every other float here. Every fixture is
    run under all four linkages, because ward, complete and average go to scipy's
    nearest-neighbour chain and single to scikit-learn's own spanning tree, and the two break
    ties differently.
    """
    import numpy as np
    from sklearn.cluster import AgglomerativeClustering as SkAgglomerative

    cases = []
    for fixture in _agglomerative_fixtures():
        matrix = np.array(fixture["rows"], dtype=np.float64)
        for linkage in AGGLOMERATIVE_LINKAGES:
            for cut in fixture["cuts"]:
                model = SkAgglomerative(
                    n_clusters=cut, linkage=linkage, compute_distances=True).fit(matrix)
                cases.append(_agglomerative_case(
                    f"{fixture['name']}, {linkage}, cut at {cut}",
                    fixture["rows"], linkage, model, N_CLUSTERS, cut))

    # The threshold is exclusive: points at 0, 1 and 3 stay three clusters at exactly 1.0 under
    # single linkage and become two just above it. Zero is allowed and splits every sample.
    steps = [[0.0], [1.0], [3.0]]
    for threshold in (0.0, 1.0, 1.001, 2.0, 2.001):
        model = SkAgglomerative(
            n_clusters=None, distance_threshold=threshold, linkage=SINGLE_LINKAGE).fit(np.array(steps))
        cases.append(_agglomerative_case(
            f"a distance threshold of {threshold}, single", steps, SINGLE_LINKAGE, model,
            DISTANCE_THRESHOLD, threshold))

    blobs = next(f for f in _agglomerative_fixtures() if f["name"] == "three blobs in three dimensions")
    for threshold in (5.0, 40.0):
        model = SkAgglomerative(
            n_clusters=None, distance_threshold=threshold, linkage="ward").fit(np.array(blobs["rows"]))
        cases.append(_agglomerative_case(
            f"a distance threshold of {threshold} on three blobs, ward", blobs["rows"], "ward", model,
            DISTANCE_THRESHOLD, threshold))

    return {
        "metadata": {
            "algorithm": "AgglomerativeClustering",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                'sklearn.cluster.AgglomerativeClustering(n_clusters=..., linkage=..., compute_distances=True)',
                'sklearn.cluster.AgglomerativeClustering(n_clusters=None, distance_threshold=...)'],
            "count": len(cases),
        },
        "cases": cases,
    }


def _distribution_fixtures() -> list[dict]:
    """Points chosen for the range a general-purpose caller reaches, not the one the tests do."""
    return [
        {"name": "the body, where every hypothesis test here already lives",
         "call": "t.sf", "args": {"x": 2.0, "df": 10.0}},
        {"name": "the median of a Cauchy", "call": "t.sf", "args": {"x": 0.0, "df": 1.0}},
        {"name": "the lower half, by symmetry", "call": "t.sf", "args": {"x": -1.5, "df": 7.0}},
        # The far tail: the closed forms the internal tests use do not reach it, and no
        # corpus of p-values near 0.05 would ever have exercised it.
        {"name": "the far tail at 1e-24", "call": "t.sf", "args": {"x": 30.0, "df": 30.0}},
        {"name": "a heavy tail, three degrees of freedom",
         "call": "t.sf", "args": {"x": 200.0, "df": 3.0}},
        {"name": "a million degrees of freedom, all but normal",
         "call": "t.sf", "args": {"x": 8.0, "df": 1000000.0}},
        {"name": "the multiplier a 95% interval asks for",
         "call": T_PPF, "args": {"x": 0.975, "df": 12.0}},
        {"name": "the multiplier a 99% interval asks for",
         "call": T_PPF, "args": {"x": 0.995, "df": 5.0}},
        {"name": "the median", "call": T_PPF, "args": {"x": 0.5, "df": 3.0}},
        {"name": "a quantile far into the lower tail",
         "call": T_PPF, "args": {"x": 1e-08, "df": 4.0}},
        {"name": "a regression's overall test",
         "call": "f.sf", "args": {"x": 4.0, "dfn": 2.0, "dfd": 20.0}},
        {"name": "one and one degree of freedom",
         "call": "f.sf", "args": {"x": 1.0, "dfn": 1.0, "dfd": 1.0}},
        {"name": "far into the upper tail",
         "call": "f.sf", "args": {"x": 500.0, "dfn": 3.0, "dfd": 100.0}},
        {"name": "near zero, where the tail is all but one",
         "call": "f.sf", "args": {"x": 0.001, "dfn": 5.0, "dfd": 5.0}},
        # Large shapes near the mean (#837), where a fraction cut at 300 terms stopped short.
        {"name": "an ANOVA on half a million observations, one standard deviation up",
         "call": "f.sf", "args": {"x": 1.004, "dfn": 200000.0, "dfd": 300000.0}},
        {"name": "two million degrees of freedom each, just below the mean",
         "call": "f.sf", "args": {"x": 0.999, "dfn": 2000000.0, "dfd": 2000000.0}},
        {"name": "shapes of 1e8 each, at the median",
         "call": "f.sf", "args": {"x": 1.0, "dfn": 200000000.0, "dfd": 200000000.0}},
        {"name": "shapes of 1e8 each, one and a half standard deviations up",
         "call": "f.sf", "args": {"x": 1.0002, "dfn": 200000000.0, "dfd": 200000000.0}},
        # Chi-squared, published for the log-rank test (#569). One degree of freedom is
        # the two-sample case; the far tail is where a closed form would stop agreeing.
        {"name": "a log-rank test on two groups", "call": CHI2_SF, "args": {"x": 3.84, "df": 1.0}},
        {"name": "one degree of freedom, at the median", "call": CHI2_SF, "args": {"x": 0.4549, "df": 1.0}},
        {"name": "a k-sample test, four degrees of freedom", "call": CHI2_SF, "args": {"x": 9.488, "df": 4.0}},
        {"name": "the far tail at 1e-23", "call": CHI2_SF, "args": {"x": 120.0, "df": 3.0}},
        {"name": "below the support, where the tail is one", "call": CHI2_SF, "args": {"x": 0.0, "df": 2.0}},
        {"name": "a hundred degrees of freedom, near its mean", "call": CHI2_SF, "args": {"x": 100.0, "df": 100.0}},
        # Large shapes near the mean (#837): a Ljung-Box statistic this wide was about 0.0014 off.
        {"name": "twenty thousand degrees of freedom, at its mean",
         "call": CHI2_SF, "args": {"x": 20000.0, "df": 20000.0}},
        {"name": "two million degrees of freedom, two standard deviations up",
         "call": CHI2_SF, "args": {"x": 2004000.0, "df": 2000000.0}},
        {"name": "a shape of 1e8, three standard deviations down",
         "call": CHI2_SF, "args": {"x": 199940000.0, "df": 200000000.0}},
        {"name": "a shape of 1e8, one standard deviation up",
         "call": CHI2_SF, "args": {"x": 200020000.0, "df": 200000000.0}},
        # The normal quantile (#569): a Student one at a huge degrees of freedom reaches
        # it only to about 9e-9, which is why decision 0098 publishes its own member.
        {"name": "the multiplier a 95% large-sample interval asks for", "call": NORM_PPF, "args": {"x": 0.975}},
        {"name": "the multiplier a 99% one asks for", "call": NORM_PPF, "args": {"x": 0.995}},
        {"name": "the median, which is exactly zero", "call": NORM_PPF, "args": {"x": 0.5}},
        {"name": "the lower half, by symmetry", "call": NORM_PPF, "args": {"x": 0.025}},
        {"name": "far into the lower tail", "call": NORM_PPF, "args": {"x": 1e-08}},
        {"name": "far into the upper tail", "call": NORM_PPF, "args": {"x": 0.99999999}},
        # One shape large and one small (#841), where the fraction was up to 4e-7 off. f.sf stays at
        # dfn = 2 or dfd = 2e5: past that scipy's own argument rounding reaches 1e-9 (docs/equivalence.md).
        {"name": "two hundred million degrees of freedom, one unit out",
         "call": "t.sf", "args": {"x": 1.0, "df": 200000000.0}},
        {"name": "two hundred million degrees of freedom, below the median",
         "call": "t.sf", "args": {"x": -0.7, "df": 200000000.0}},
        {"name": "twenty million degrees of freedom, one and a half units out",
         "call": "t.sf", "args": {"x": 1.5, "df": 20000000.0}},
        {"name": "two numerator degrees of freedom over two hundred million",
         "call": "f.sf", "args": {"x": 1.0005, "dfn": 2.0, "dfd": 200000000.0}},
        {"name": "a numerator shape of 0.1 over a denominator of 1e5",
         "call": "f.sf", "args": {"x": 4.0, "dfn": 0.2, "dfd": 200000.0}},
        {"name": "shapes of 1e5 and one half, below the mean",
         "call": "f.sf", "args": {"x": 0.9, "dfn": 1.0, "dfd": 200000.0}},
    ]


def generate_stats_distributions() -> dict:
    """The tails and quantiles Lodestar.Stats publishes, at the range a caller reaches."""
    from scipy import stats as sps

    cases = []
    for fixture in _distribution_fixtures():
        args = fixture["args"]
        call = fixture["call"]
        if call == "t.sf":
            value = float(sps.t.sf(args["x"], args["df"]))
        elif call == T_PPF:
            value = float(sps.t.ppf(args["x"], args["df"]))
        elif call == CHI2_SF:
            value = float(sps.chi2.sf(args["x"], args["df"]))
        elif call == NORM_PPF:
            value = float(sps.norm.ppf(args["x"]))
        else:
            value = float(sps.f.sf(args["x"], args["dfn"], args["dfd"]))
        cases.append({
            "name": fixture["name"], "call": call, "args": args, "value": value})

    return {
        "metadata": {
            "library": "scipy",
            "version": version("scipy"),
            FAMILY: "distributions",
            "count": len(cases),
        },
        "cases": cases,
    }


def _ols_fixtures() -> list[dict]:
    """Designs chosen for what an inference table can get wrong, not for what a solve can."""
    return [
        {
            "name": "simple regression, intercept fitted",
            DESIGN: [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0],
            RESPONSE: [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Three regressors on twelve rows: the residual degrees of freedom (8) are
            # small enough that the t multiplier is visibly not 1.96.
            "name": "three regressors, twelve rows",
            DESIGN: [
                1.0, 4.0, 0.5, 2.0, 3.0, 1.5, 3.0, 5.0, 2.5, 4.0, 2.0, 0.5,
                5.0, 6.0, 3.5, 6.0, 1.0, 1.0, 7.0, 7.0, 4.5, 8.0, 3.0, 2.0,
                9.0, 8.0, 5.5, 10.0, 4.0, 2.5, 11.0, 9.0, 6.5, 12.0, 5.0, 3.0,
            ],
            RESPONSE: [
                7.2, 9.1, 12.4, 10.8, 16.3, 14.1, 20.7, 18.2, 24.9, 22.4, 29.1, 26.8,
            ],
            OLS_FEATURE_COUNT: 3, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # No constant column. R-squared is then the uncentred one, which statsmodels
            # reports without saying so, and the F test loses a degree of freedom.
            "name": "two regressors, no intercept",
            DESIGN: [1.0, 1.0, 2.0, 1.0, 3.0, 2.0, 4.0, 2.0, 5.0, 3.0, 6.0, 3.0, 7.0, 4.0, 8.0, 4.0],
            RESPONSE: [3.1, 5.2, 8.4, 10.1, 13.3, 15.2, 18.4, 20.1],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # x2 is x1 plus a hundredth: a five-figure VIF, which is both what the
            # diagnostic exists to report and what normal equations would round away.
            "name": "near-collinear regressors, a VIF near 6e4",
            DESIGN: [
                1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01,
            ],
            RESPONSE: [2.2, 4.1, 6.3, 7.9, 10.2, 12.1, 14.3, 15.9, 18.2, 20.1],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "a 99% interval, where the multiplier is the wider one",
            DESIGN: [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0],
            RESPONSE: [1.9, 4.2, 5.8, 8.3, 9.7, 12.4, 13.9],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            # 9.4 is load-bearing: at 9.0 the third row is exactly the sum of the first
            # two, the fit is exact, and every standard error below divides by zero.
            "name": "one residual degree of freedom",
            DESIGN: [1.0, 3.0, 2.0, 1.0, 3.0, 4.0],
            RESPONSE: [5.0, 4.0, 9.4],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        *_robust_fixtures(),
        *_hac_cluster_fixtures(),
    ]


def _robust_fixtures() -> list[dict]:
    """One heteroskedastic design, fitted four ways, and two boundaries (#686).

    The spread of the response grows with the regressor -- residuals of roughly
    +-0.2 at the first row and +-3 at the last -- which is the shape the ordinary
    standard errors get wrong and the whole reason these estimators exist. Fitting
    the same rows under all four types is deliberate: HC0 to HC3 differ only in the
    weight each row's squared residual carries, so one design tells them apart where
    four designs would confound the estimator with the data.

    Hand-written rather than drawn, like the fixtures above, so the funnel is visible
    in the literal.
    """
    funnel = {
        DESIGN: [
            1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0,
        ],
        RESPONSE: [
            2.1, 4.3, 5.7, 8.4, 9.6, 13.1, 13.4, 17.9, 17.2, 22.8, 20.9, 26.4, 24.1, 31.6,
        ],
        OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
    }
    fixtures = [
        {"name": f"heteroskedastic funnel, {kind}", COVARIANCE_TYPE: kind, **funnel}
        for kind in ("HC0", "HC1", "HC2", "HC3")
    ]
    fixtures.append({
        # long-comment: what this case is for, and why its response is its own.
        # No intercept: the Wald test then restricts every coefficient rather than all
        # but one, which is where the block the statistic inverts changes shape.
        # The design is shared with the nonrobust case above, the response is not. On the
        # near-noiseless response above it (R2 = 0.99995) the Wald statistic reaches
        # 97 533, and tools/compare_oracles.py compares floats at 1e-9 *absolute*
        # (decision 0073) -- so a last-bit BLAS disagreement of 7e-14 relative is
        # 7e-9 absolute and the reproducibility gate fails on a different machine.
        # The ordinary F on the same rows is safe at 60 493 only because it is read
        # off R-squared rather than by inverting a covariance block. Visible residuals
        # bring the statistic to 1 113 and the headroom back to two orders of magnitude.
        "name": "two regressors, no intercept, HC1",
        DESIGN: [1.0, 1.0, 2.0, 1.0, 3.0, 2.0, 4.0, 2.0, 5.0, 3.0, 6.0, 3.0, 7.0, 4.0, 8.0, 4.0],
        RESPONSE: [3.4, 4.8, 8.9, 9.6, 12.7, 15.9, 19.2, 19.4],
        OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        COVARIANCE_TYPE: "HC1",
    })
    fixtures.append({
        # A 99% interval under a robust covariance, where the multiplier is the normal
        # one rather than Student's -- 2.5758 rather than 3.4995 on five rows.
        "name": "a 99% interval under HC3, where the multiplier is the normal one",
        DESIGN: [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0],
        RESPONSE: [1.9, 4.2, 5.8, 8.3, 9.7, 12.4, 13.9],
        OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        COVARIANCE_TYPE: "HC3",
    })
    return fixtures


# long-comment: where the literal below came from, so it can be drawn again.
# Twenty rows of y = 1.5 + 0.8 x plus AR(1) errors at 0.6, drawn once from numpy's
# default_rng(775) with a noise scale of 1.2 and rounded to two decimals: the serial
# correlation HAC exists for, frozen as a literal like the fixtures above (#775).
SERIAL = {
    DESIGN: [float(v) for v in range(1, 21)],
    RESPONSE: [
        2.43, 3.16, 4.05, 4.97, 5.77, 4.73, 5.77, 7.8, 8.67, 9.68,
        9.7, 10.86, 11.12, 10.9, 11.98, 14.32, 15.33, 14.54, 15.6, 19.15,
    ],
    OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
}

# Five clusters of uneven size (4, 4, 5, 4, 3), labels neither dense nor sorted: the
# reference hands int64 labels to np.bincount as they are, so the gaps are empty bins.
SERIAL_GROUPS = [7, 7, 3, 7, 0, 3, 3, 12, 0, 12, 7, 5, 5, 3, 0, 12, 5, 5, 5, 0]


def _hac_cluster_fixtures() -> list[dict]:
    """HAC at two lag counts and past the row count, and one-way clusters, on OLS (#775)."""
    return [
        {"name": "serially correlated errors, HAC 2", COVARIANCE_TYPE: "HAC", HAC_LAGS: 2, **SERIAL},
        {
            "name": "serially correlated errors, HAC 4, corrected",
            COVARIANCE_TYPE: "HAC", HAC_LAGS: 4, USE_CORRECTION: True, **SERIAL,
        },
        {
            # long-comment: what the case pins, and why its response is its own.
            # Ten lags on eight rows: the lags past the seventh have no pairs, and the
            # Bartlett weights of the ones that do still divide by eleven. The response
            # alternates hard: on the HC1 case's own the Wald statistic reaches 14 955,
            # past where decision 0073's absolute 1e-9 holds across machines.
            "name": "two regressors, no intercept, HAC 10 on eight rows",
            DESIGN: [1.0, 1.0, 2.0, 1.0, 3.0, 2.0, 4.0, 2.0, 5.0, 3.0, 6.0, 3.0, 7.0, 4.0, 8.0, 4.0],
            RESPONSE: [7.5, 1.2, 13.1, 5.1, 17.9, 9.6, 25.1, 13.5],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
            COVARIANCE_TYPE: "HAC", HAC_LAGS: 10,
        },
        {"name": "five uneven clusters", COVARIANCE_TYPE: CLUSTER, GROUPS: SERIAL_GROUPS, **SERIAL},
        {
            "name": "five uneven clusters, uncorrected",
            COVARIANCE_TYPE: CLUSTER, GROUPS: SERIAL_GROUPS, USE_CORRECTION: False, **SERIAL,
        },
        {
            # Ten clusters of two at 99%: the F test reads 9 denominator degrees of
            # freedom where df_resid is 18.
            "name": "ten clusters of two, 99%",
            COVARIANCE_TYPE: CLUSTER, GROUPS: [row // 2 for row in range(20)],
            **{**SERIAL, CONFIDENCE_LEVEL: 0.99},
        },
    ]


# --- Lodestar.Survival, oracled by lifelines rather than scipy (#569) -----------

DURATIONS = "durations"
EVENTS = "eventObserved"
# The two-arm keys, named because each appears once per fixture and once per case, and
# S1192 counts a JSON key like any other literal (#569).
DURATIONS_A = "durationsA"
DURATIONS_B = "durationsB"
EVENTS_A = "eventsA"
EVENTS_B = "eventsB"
# The library that oracles this family, named for the same reason as the keys above.
LIFELINES = "lifelines"
OBSERVED = "observed"
# How many subjects each row stands for, where a sample is too large to store one row each.
COPIES = "copies"


def _survival_fixtures() -> list[dict]:
    """Right-censored samples, chosen for what ties and censoring can get wrong.

    The first is Freireich's leukaemia trial, the data every survival text opens
    with, and the rest exist for one boundary each: a time carrying both an event
    and a censoring, a sample with no event at all, and the shortest input a curve
    can be drawn from.
    """
    return [
        {
            "name": "Freireich, the treatment arm",
            DURATIONS: [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35],
            EVENTS: [1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        },
        {
            "name": "Freireich, the control arm, every duration observed",
            DURATIONS: [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23],
            EVENTS: [1] * 21,
        },
        {
            # An event and a censoring at one time: the step Kaplan-Meier is most
            # often got wrong on, the censoring moving the risk set and not the curve.
            "name": "events and censorings tied at the same time",
            DURATIONS: [2, 2, 2, 5, 5, 5, 8, 8, 11],
            EVENTS: [1, 0, 1, 0, 1, 0, 1, 1, 0],
        },
        {
            "name": "every observation censored, so the curve never falls",
            DURATIONS: [3, 5, 5, 9, 12],
            EVENTS: [0, 0, 0, 0, 0],
        },
        {
            "name": "a single observed duration",
            DURATIONS: [4],
            EVENTS: [1],
        },
        {
            "name": "a single censored duration",
            DURATIONS: [4],
            EVENTS: [0],
        },
        {
            # The last duration observed takes the curve to zero, where the log-log
            # transform's interval is degenerate and lifelines reports both bounds as zero.
            "name": "the curve reaches zero at the last time",
            DURATIONS: [1, 2, 3],
            EVENTS: [1, 1, 1],
        },
        {
            "name": "heavy ties, four events at one time",
            DURATIONS: [7, 7, 7, 7, 9, 9, 14, 20, 20],
            EVENTS: [1, 1, 1, 1, 0, 1, 1, 0, 1],
        },
        {
            # long-comment: why the risk set is this large, and why it is stored as copies.
            # 70,000 subjects, the first event at 70,000 at risk and the third at 50,000:
            # n(n - d) overflows int at both, wrapping positive at the first and negative
            # at the third, where Greenwood's sum was computed in int (#865). Each row
            # stands for COPIES of itself, so the corpus holds six rows rather than 70,000.
            "name": "70,000 at risk, past where n(n - d) overflows int",
            DURATIONS: [1, 2, 3, 4, 5, 5],
            EVENTS: [1, 0, 1, 1, 1, 0],
            COPIES: [1000, 19000, 2000, 40000, 4000, 4000],
        },
    ]


def _logrank_fixtures() -> list[dict]:
    """Two-arm comparisons, including the pair every text uses and two degenerate ones."""
    return [
        {
            "name": "Freireich, treatment against control",
            DURATIONS_A: [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35],
            EVENTS_A: [1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            DURATIONS_B: [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23],
            EVENTS_B: [1] * 21,
        },
        {
            "name": "two arms with tied event times across groups",
            DURATIONS_A: [4, 4, 6, 6, 9],
            EVENTS_A: [1, 1, 1, 0, 1],
            DURATIONS_B: [4, 6, 6, 10, 10],
            EVENTS_B: [1, 1, 1, 1, 0],
        },
        {
            "name": "identical arms, where the statistic is zero",
            DURATIONS_A: [2, 4, 6, 8],
            EVENTS_A: [1, 1, 1, 1],
            DURATIONS_B: [2, 4, 6, 8],
            EVENTS_B: [1, 1, 1, 1],
        },
        {
            "name": "one arm entirely censored",
            DURATIONS_A: [3, 5, 7, 9],
            EVENTS_A: [1, 1, 1, 1],
            DURATIONS_B: [3, 5, 7, 9],
            EVENTS_B: [0, 0, 0, 0],
        },
        {
            "name": "arms of very different size",
            DURATIONS_A: [1, 2, 3],
            EVENTS_A: [1, 1, 1],
            DURATIONS_B: [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15],
            EVENTS_B: [1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1],
        },
    ]


def generate_survival_curves() -> dict:
    """Kaplan-Meier and Nelson-Aalen over the same samples, frozen from lifelines.

    The confidence interval is the one lifelines reports by default: built on the
    log-log transform of the estimate rather than on the estimate itself, verified
    numerically and stated on the reference page, because the two differ visibly.
    """
    from lifelines import KaplanMeierFitter, NelsonAalenFitter  # noqa: PLC0415

    cases = []
    for fixture in _survival_fixtures():
        copies = fixture.get(COPIES, [1] * len(fixture[DURATIONS]))
        durations = [d for d, c in zip(fixture[DURATIONS], copies, strict=True) for _ in range(c)]
        events = [e for e, c in zip(fixture[EVENTS], copies, strict=True) for _ in range(c)]

        kmf = KaplanMeierFitter()
        kmf.fit(durations, event_observed=events)
        naf = NelsonAalenFitter()
        naf.fit(durations, event_observed=events)

        table = kmf.event_table
        lower, upper = kmf.confidence_interval_.columns
        case = {
            "name": fixture["name"],
            DURATIONS: fixture[DURATIONS],
            EVENTS: fixture[EVENTS],
            "timeline": [float(t) for t in kmf.survival_function_.index],
            "survival": [float(v) for v in kmf.survival_function_.iloc[:, 0]],
            "cumulativeHazard": [float(v) for v in naf.cumulative_hazard_.iloc[:, 0]],
            "atRisk": [int(v) for v in table["at_risk"]],
            OBSERVED: [int(v) for v in table[OBSERVED]],
            "censored": [int(v) for v in table["censored"]],
            LOWER: [float(v) for v in kmf.confidence_interval_[lower]],
            UPPER: [float(v) for v in kmf.confidence_interval_[upper]],
        }
        if COPIES in fixture:
            case[COPIES] = fixture[COPIES]
        cases.append(case)

    return {
        "metadata": {
            "library": LIFELINES,
            "version": version(LIFELINES),
            FAMILY: "survival-curves",
            "confidenceInterval": "log-log transform at 0.95, lifelines' default",
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_survival_logrank() -> dict:
    """The two-sample log-rank test, frozen from lifelines.statistics."""
    from lifelines.statistics import logrank_test  # noqa: PLC0415

    cases = []
    for fixture in _logrank_fixtures():
        result = logrank_test(
            fixture[DURATIONS_A], fixture[DURATIONS_B],
            event_observed_A=fixture[EVENTS_A], event_observed_B=fixture[EVENTS_B])
        cases.append({
            "name": fixture["name"],
            DURATIONS_A: fixture[DURATIONS_A], EVENTS_A: fixture[EVENTS_A],
            DURATIONS_B: fixture[DURATIONS_B], EVENTS_B: fixture[EVENTS_B],
            "statistic": float(result.test_statistic),
            "pValue": float(result.p_value),
            "degreesOfFreedom": int(result.degrees_of_freedom),
        })

    return {
        "metadata": {
            "library": LIFELINES,
            "version": version(LIFELINES),
            FAMILY: "survival-logrank",
            "count": len(cases),
        },
        "cases": cases,
    }


# The default precision stops lifelines 8.6e-6 relative from the maximum; at 1e-20 all five fixtures
# are within 3.1e-13 of an independent Newton-Raphson (#684, the specification has the table).
COX_FIT_OPTIONS = {PRECISION: 1e-20, "r_precision": 0.0}


NORMAL_COVARIATE = "normal"
BINARY_COVARIATE = "binary"
COX_DURATION_COLUMN = "duration"
COX_EVENT_COLUMN = "event"


def _cox_fixture(rng: SeededRandom, name: str, rows: int, columns: list[str],
                 event_rate: float, time_scale: float, tied: bool) -> dict:
    """One seeded design. A binary column is a 0/1 group, a normal one a standard draw.

    Durations are exponential with a log hazard that depends on the design, so every
    coefficient is away from zero; `tied` rounds them up to whole units, which is where
    Efron's handling of ties differs from Breslow's.
    """
    effects = [0.6, -0.8, 0.4]
    design, durations, events = [], [], []
    seen = set()
    for _ in range(rows):
        row = [float(rng.random() < 0.5) if kind == BINARY_COVARIATE else rng.gauss(0.0, 1.0)
               for kind in columns]
        rate = math.exp(sum(effect * value for effect, value in zip(effects, row, strict=False)))
        duration = -math.log(1.0 - rng.random()) / rate * time_scale
        duration = float(math.ceil(duration)) if tied else round(duration, 6)
        while not tied and duration in seen:
            duration = round(duration + 1e-6, 6)
        seen.add(duration)
        design.extend(row)
        durations.append(duration)
        events.append(1 if rng.random() < event_rate else 0)
    return {"name": name, OLS_FEATURE_COUNT: len(columns), DESIGN: design,
            DURATIONS: durations, EVENTS: events}


def _cox_fixtures() -> list[dict]:
    """Five designs, each catching what the others would not (the #684 specification)."""
    rng = SeededRandom(SEED + 68400)
    return [
        # Ties are where Efron and Breslow part company, so this one proves which is written.
        _cox_fixture(rng, "heavy ties, one continuous and one binary covariate",
                     80, [NORMAL_COVARIATE, BINARY_COVARIATE], 0.8, 5.0, tied=True),
        # Where Efron and Breslow coincide: pins the untied path rather than discriminating.
        _cox_fixture(rng, "no ties at all", 40, [NORMAL_COVARIATE, BINARY_COVARIATE], 0.75, 10.0, tied=False),
        # Around 70% censored: the risk sets move and the events do not.
        _cox_fixture(rng, "heavy censoring", 60, [NORMAL_COVARIATE, BINARY_COVARIATE], 0.3, 5.0, tied=True),
        # The p x p algebra degenerates to a scalar, where a Cholesky bug would hide.
        _cox_fixture(rng, "a single covariate", 50, [NORMAL_COVARIATE], 0.8, 5.0, tied=True),
        # The smallest case where the inverse is not a closed formula.
        _cox_fixture(rng, "three covariates", 90, [NORMAL_COVARIATE, BINARY_COVARIATE, NORMAL_COVARIATE], 0.8, 5.0,
                     tied=True),
    ]


def generate_survival_cox() -> dict:
    """The Cox proportional hazards table, frozen from lifelines' CoxPHFitter (#684).

    lifelines standardises the design before its Newton-Raphson and reports the answer
    rescaled, so nothing here depends on that; what does is the stopping rule, which
    COX_FIT_OPTIONS tightens. Efron is the only tie handling CoxPHFitter offers.
    """
    import pandas as pd  # noqa: PLC0415
    from lifelines import CoxPHFitter  # noqa: PLC0415
    from lifelines.utils import concordance_index  # noqa: PLC0415

    cases = []
    for fixture in _cox_fixtures():
        width = fixture[OLS_FEATURE_COUNT]
        names = [f"x{index}" for index in range(width)]
        frame = pd.DataFrame(np.array(fixture[DESIGN]).reshape(-1, width), columns=names)
        frame[COX_DURATION_COLUMN] = fixture[DURATIONS]
        frame[COX_EVENT_COLUMN] = fixture[EVENTS]

        model = CoxPHFitter().fit(frame, COX_DURATION_COLUMN, COX_EVENT_COLUMN, fit_options=COX_FIT_OPTIONS)
        summary = model.summary
        ratio = model.log_likelihood_ratio_test()
        # lifelines' concordance_index_ is this call on the negated partial hazard; checked
        # here so the frozen value names what it is rather than which attribute it came from.
        concordance = concordance_index(
            frame[COX_DURATION_COLUMN], -model.predict_partial_hazard(frame), frame[COX_EVENT_COLUMN])
        assert concordance == model.concordance_index_, fixture["name"]

        cases.append({
            **fixture,
            CONFIDENCE_LEVEL: 1.0 - model.alpha,
            COEFFICIENTS: [float(v) for v in summary["coef"]],
            STANDARD_ERRORS: [float(v) for v in summary["se(coef)"]],
            Z_STATISTICS: [float(v) for v in summary["z"]],
            P_VALUES: [float(v) for v in summary["p"]],
            CONFIDENCE_LOWER: [float(v) for v in summary["coef lower 95%"]],
            CONFIDENCE_UPPER: [float(v) for v in summary["coef upper 95%"]],
            "hazardRatios": [float(v) for v in summary["exp(coef)"]],
            "hazardRatioLower": [float(v) for v in summary["exp(coef) lower 95%"]],
            "hazardRatioUpper": [float(v) for v in summary["exp(coef) upper 95%"]],
            LOG_LIKELIHOOD: float(model.log_likelihood_),
            "nullLogLikelihood": float(model.log_likelihood_ - ratio.test_statistic / 2.0),
            "likelihoodRatioStatistic": float(ratio.test_statistic),
            "likelihoodRatioPValue": float(ratio.p_value),
            "likelihoodRatioDegreesOfFreedom": int(ratio.degrees_freedom),
            "concordanceIndex": float(model.concordance_index_),
        })

    return {
        "metadata": {
            "library": LIFELINES,
            "version": version(LIFELINES),
            FAMILY: "survival-cox",
            "ties": "Efron, the only handling CoxPHFitter offers",
            "fitOptions": COX_FIT_OPTIONS,
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Lodestar.Text.Similarity, oracled by datasketch and simhash (#602) -------

SIM_TOKENS = "tokens"
SIM_SIGNATURE = "signature"
HAMMING = "hamming"
# The one sentence four documents are variations on; spelled once so its words stay
# under S1192 rather than reaching three occurrences apiece.
SIM_SENTENCE = ["the", "quick", "brown", "fox"]


def _similarity_documents() -> list[dict]:
    """Token sets chosen for what a sketch gets wrong, not for what a hash does."""
    return [
        {"key": "a", SIM_TOKENS: SIM_SENTENCE},
        # One token apart from "a": the pair a near-duplicate detector exists for.
        {"key": "b", SIM_TOKENS: SIM_SENTENCE[:3] + ["dog"]},
        {"key": "c", SIM_TOKENS: SIM_SENTENCE + ["jumps"]},
        # Disjoint from every other set, so its estimate must be exactly zero.
        {"key": "d", SIM_TOKENS: ["entirely", "different", "words", "here"]},
        # The same set as "a" written in another order: a set sketch cannot see the order.
        {"key": "e", SIM_TOKENS: list(reversed(SIM_SENTENCE))},
        # A repeated token, which MinHash ignores and SimHash weighs.
        {"key": "f", SIM_TOKENS: [SIM_SENTENCE[0]] + SIM_SENTENCE},
        {"key": "g", SIM_TOKENS: ["\u00e9clair", "na\u00efve", "caf\u00e9"]},
        {"key": "h", SIM_TOKENS: []},
    ]


def generate_text_similarity() -> dict:
    """Freeze MinHash signatures, SimHash fingerprints and the LSH banding solve.

    The permutation coefficients are frozen with the signatures rather than derived
    from the seed: reproducing numpy's generator stream in C# would make the parity
    claim depend on a random number generator instead of on the algorithm, which is
    the call decision 0072 already made for randomized SVD's omega.
    """
    from datasketch import MinHash as DsMinHash
    from datasketch.lsh import _optimal_param
    from simhash import Simhash

    documents = _similarity_documents()
    permutation_count = 32
    # long-comment: which permutation family the corpus freezes, and why it is now named.
    # scheme="legacy" is (a*h + b) mod (2^61 - 1), what Lodestar.Text's MinHash and
    # Lodestar.Gpu's TiledMinHashSignatures compute. It was datasketch's only scheme through
    # 1.6.5 and arrived as the default; 2.0.0 added affine32 and affine64 and made affine32
    # the default, so this same call silently changed families (#643). Named rather than
    # inherited -- #645 holds whether the shipped packages should follow the new default.
    scheme = "legacy"
    reference = DsMinHash(num_perm=permutation_count, seed=1, scheme=scheme)
    multipliers, addends = reference.permutations

    signatures: dict[str, list[int]] = {}
    cases = []
    for document in documents:
        sketch = DsMinHash(num_perm=permutation_count, seed=1, scheme=scheme)
        for token in document[SIM_TOKENS]:
            sketch.update(token.encode("utf-8"))
        signature = [int(value) for value in sketch.hashvalues]
        signatures[document["key"]] = signature
        cases.append({
            "key": document["key"],
            SIM_TOKENS: document[SIM_TOKENS],
            SIM_SIGNATURE: signature,
            "simHash": int(Simhash(document[SIM_TOKENS]).value),
        })

    pairs = []
    keys = [document["key"] for document in documents]
    for left in range(len(keys)):
        for right in range(left + 1, len(keys)):
            first, second = keys[left], keys[right]
            estimate = sum(
                1 for a, b in zip(signatures[first], signatures[second]) if a == b
            ) / permutation_count
            pairs.append({
                "left": first,
                "right": second,
                JACCARD: float(estimate),
                HAMMING: bin(
                    int(Simhash(documents[left][SIM_TOKENS]).value)
                    ^ int(Simhash(documents[right][SIM_TOKENS]).value)
                ).count("1"),
            })

    bandings = []
    for threshold in (0.5, 0.7, 0.8, 0.9, 0.95):
        for length in (32, 64, 128):
            bands, rows = _optimal_param(threshold, length, 0.5, 0.5)
            bandings.append({
                "threshold": threshold,
                "permutations": length,
                "bands": int(bands),
                "rowsPerBand": int(rows),
            })

    # long-comment: why the corpus grows a block instead of moving one (#645).
    # datasketch 2.0.0 made `affine32` its default, so the block above -- which names `legacy`
    # -- is what every existing assertion replays, and this one is what a caller comparing
    # against a current datasketch needs. The two share the documents and nothing else:
    # different coefficients, different widths, different values, by construction.
    affine_reference = DsMinHash(num_perm=permutation_count, seed=1, scheme=AFFINE32)
    affine_a, affine_b = affine_reference.permutations
    affine_cases = []
    for document in documents:
        sketch = DsMinHash(num_perm=permutation_count, seed=1, scheme=AFFINE32)
        for token in document[SIM_TOKENS]:
            sketch.update(token.encode("utf-8"))
        affine_cases.append({
            "key": document["key"],
            SIM_SIGNATURE: [int(value) for value in sketch.hashvalues],
        })

    return {
        "metadata": {
            "library": "datasketch + simhash",
            "version": f'{version("datasketch")} + {version("simhash")}',
            FAMILY: "text-similarity",
            VARIANT: "MinHash(seed=1, sha1_hash32, scheme=legacy), Simhash(f=64, md5), "
                       "MinHashLSH optimal banding",
            "count": len(cases),
        },
        "permutationCount": permutation_count,
        "multipliers": [int(value) for value in multipliers],
        "addends": [int(value) for value in addends],
        "cases": cases,
        "pairs": pairs,
        "bandings": bandings,
        AFFINE32: {
            VARIANT: "MinHash(seed=1, sha1_hash32, scheme=affine32)",
            "multipliers": [int(value) for value in affine_a],
            "addends": [int(value) for value in affine_b],
            "cases": affine_cases,
        },
    }


# --- Lodestar.Text.Search, oracled by rank_bm25 (#573) -------------------------

BM25_QUERY = "query"
BM25_DOCUMENTS = "documents"
BM25_FILLER = "filler"
BM25_LEARNING = "learning"
# One vocabulary, each word spelled once: the fixtures index into it rather than
# repeating a literal, which is what keeps them under S1192's threshold.
BM25_WORDS = ["alpha", "bravo", "charlie", "delta"]
BM25_CHAIN = [BM25_WORDS[i:i + 2] for i in range(3)]


def _bm25_fixtures() -> list[dict]:
    """Corpora chosen for what BM25 gets wrong, not for what a sum does.

    The first carries a term in every document, which drives Robertson's IDF
    negative -- rank_bm25 floors those at epsilon * average_idf rather than letting
    a match subtract, and that floor is the single place a plain reading of the
    paper disagrees with this reference.
    """
    animals = [
        ["the", "cat", "sat"],
        ["the", "dog", "sat", "sat"],
        ["the", "bird", "flew", "far", "away", "today"],
    ]
    return [
        {
            "name": "a term in every document, where Robertson's IDF goes negative",
            BM25_DOCUMENTS: animals,
            BM25_QUERY: ["sat"],
        },
        {
            "name": "the same corpus, a query term that is rare",
            BM25_DOCUMENTS: animals,
            BM25_QUERY: ["cat"],
        },
        {
            "name": "a query term absent from the corpus, scoring zero everywhere",
            BM25_DOCUMENTS: BM25_CHAIN,
            BM25_QUERY: [OMEGA_KEY],
        },
        {
            "name": "several query terms, one of them absent",
            BM25_DOCUMENTS: BM25_CHAIN,
            BM25_QUERY: [BM25_WORDS[1], OMEGA_KEY, BM25_WORDS[2]],
        },
        {
            # A repeated query term counts twice: rank_bm25 sums per occurrence rather
            # than per distinct term, and a set-based implementation would halve this.
            "name": "a repeated query term, which counts twice",
            BM25_DOCUMENTS: BM25_CHAIN,
            BM25_QUERY: [BM25_WORDS[1], BM25_WORDS[1]],
        },
        {
            # Length normalization is the whole point of b: the same term frequency in
            # a short and a long document must not score the same.
            "name": "documents of very different length",
            BM25_DOCUMENTS: [
                ["term"],
                ["term"] + [BM25_FILLER] * 5,
                [BM25_FILLER] * 20 + ["term"],
                ["term"] * 3,
            ],
            BM25_QUERY: ["term"],
        },
        {
            "name": "a larger corpus with repeated vocabulary",
            BM25_DOCUMENTS: [
                ["machine", BM25_LEARNING, "is", "fun"],
                ["deep", BM25_LEARNING, "is", "machine", BM25_LEARNING],
                ["the", "cat", "sat", "on", "the", "mat"],
                [BM25_LEARNING, "to", "rank", BM25_DOCUMENTS],
                ["information", "retrieval", "and", "ranking"],
                ["ranking", BM25_DOCUMENTS, "by", "relevance"],
            ],
            BM25_QUERY: [BM25_LEARNING, BM25_DOCUMENTS],
        },
        {
            "name": "one document only, where every term is in every document",
            BM25_DOCUMENTS: [["solo", "document", "here"]],
            BM25_QUERY: ["solo"],
        },
    ]


def generate_search_bm25() -> dict:
    """Freeze rank_bm25's Okapi scores, the vocabulary, and the parameters it used.

    The corpus carries the column order so the C# side scores the same matrix: the
    vocabulary is sorted, which is what CountVectorizer's feature order is, and the
    query is frozen as indices into it as well as as terms.
    """
    from rank_bm25 import BM25Okapi  # noqa: PLC0415

    cases = []
    for fixture in _bm25_fixtures():
        documents = fixture[BM25_DOCUMENTS]
        okapi = BM25Okapi(documents)
        vocabulary = sorted({term for document in documents for term in document})
        column = {term: index for index, term in enumerate(vocabulary)}
        counts = [[document.count(term) for term in vocabulary] for document in documents]
        query = fixture[BM25_QUERY]

        cases.append({
            "name": fixture["name"],
            "vocabulary": vocabulary,
            "counts": counts,
            BM25_QUERY: query,
            "queryColumns": [column[term] for term in query if term in column],
            "queryHasUnknownTerm": any(term not in column for term in query),
            "k1": float(okapi.k1),
            "b": float(okapi.b),
            "epsilon": float(okapi.epsilon),
            "averageDocumentLength": float(okapi.avgdl),
            "scores": [float(s) for s in okapi.get_scores(query)],
        })

    return {
        "metadata": {
            "library": "rank_bm25",
            "version": version("rank_bm25"),
            FAMILY: "search-bm25",
            VARIANT: "BM25Okapi: Robertson IDF, negatives floored at epsilon * average_idf",
            "count": len(cases),
        },
        "cases": cases,
    }


def _linear_case(fixture: dict, model) -> dict:
    """One OLS or WLS case: the fixture echoed, then every number the summary table holds.

    Shared by the two corpora because statsmodels returns the same RegressionResults from
    both, and the C# returns the same OlsSummary (#768).
    """
    import numpy as np
    from statsmodels.stats.outliers_influence import variance_inflation_factor

    kind = fixture.get(COVARIANCE_TYPE, "nonrobust")
    settings = {}
    if HAC_LAGS in fixture:
        settings["maxlags"] = fixture[HAC_LAGS]
    if GROUPS in fixture:
        settings[GROUPS] = np.array(fixture[GROUPS], dtype=np.int64)
    if USE_CORRECTION in fixture:
        settings["use_correction"] = fixture[USE_CORRECTION]
    if kind == "nonrobust":
        fitted = model.fit()
    elif settings:
        fitted = model.fit(cov_type=kind, cov_kwds=settings)
    else:
        fitted = model.fit(cov_type=kind)
    interval = fitted.conf_int(alpha=1.0 - fixture[CONFIDENCE_LEVEL])

    # variance_inflation_factor adds no constant and takes no weights: the model's own
    # constant centres the auxiliary fit, and a WLS case freezes the design's own VIF.
    exog = model.exog
    first = 1 if fixture[WITH_INTERCEPT] else 0
    vif = [float(variance_inflation_factor(exog, i))
           for i in range(first, exog.shape[1])]

    case = {
        "name": fixture["name"],
        DESIGN: fixture[DESIGN],
        RESPONSE: fixture[RESPONSE],
    }
    for echoed in (WEIGHTS, ERROR_COVARIANCE, GROUPS):
        if echoed in fixture:
            case[echoed] = fixture[echoed]
    case.update({
        OLS_FEATURE_COUNT: fixture[OLS_FEATURE_COUNT],
        WITH_INTERCEPT: fixture[WITH_INTERCEPT],
        CONFIDENCE_LEVEL: fixture[CONFIDENCE_LEVEL],
        COVARIANCE_TYPE: kind,
    })
    for echoed in (HAC_LAGS, USE_CORRECTION):
        if echoed in fixture:
            case[echoed] = fixture[echoed]
    case.update({
        COEFFICIENTS: [float(v) for v in fitted.params],
        STANDARD_ERRORS: [float(v) for v in fitted.bse],
        "tStatistics": [float(v) for v in fitted.tvalues],
        P_VALUES: [float(v) for v in fitted.pvalues],
        CONFIDENCE_LOWER: [float(v) for v in interval[:, 0]],
        CONFIDENCE_UPPER: [float(v) for v in interval[:, 1]],
        "rSquared": float(fitted.rsquared),
        "adjustedRSquared": float(fitted.rsquared_adj),
        "fStatistic": float(fitted.fvalue),
        "fPValue": float(fitted.f_pvalue),
        RESIDUAL_DEGREES_OF_FREEDOM: int(fitted.df_resid),
        "residualStandardError": float(np.sqrt(fitted.mse_resid)),
        "varianceInflationFactors": vif,
    })
    return case


def _linear_exog(fixture: dict):
    """The fixture's design as statsmodels takes it, with the constant prepended when asked."""
    import numpy as np
    import statsmodels.api as sm

    design = np.array(fixture[DESIGN]).reshape(-1, fixture[OLS_FEATURE_COUNT])
    return sm.add_constant(design, prepend=True) if fixture[WITH_INTERCEPT] else design


def generate_stats_ols() -> dict:
    """OLS with the inference table statsmodels prints and MathNet does not have (#566)."""
    import numpy as np
    import statsmodels.api as sm

    cases = [
        _linear_case(fixture, sm.OLS(np.array(fixture[RESPONSE]), _linear_exog(fixture)))
        for fixture in _ols_fixtures()
    ]

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "ols",
            "count": len(cases),
        },
        "cases": cases,
    }


def _wls_fixtures() -> list[dict]:
    """Weights chosen for what whitening can get wrong, over designs the OLS corpus already trusts (#768).

    Hand-written like `_ols_fixtures`. The responses carry visible noise on purpose: the
    reproducibility gate compares at 1e-9 absolute (decision 0073), and a near-exact fit
    drives the robust Wald statistic to where a last-bit BLAS difference crosses that.
    """
    line = {
        DESIGN: [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0],
        RESPONSE: [2.4, 3.6, 6.9, 7.1, 10.8, 11.2, 15.1, 14.6, 19.3, 19.9],
        OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
    }
    uneven = [1.0, 2.0, 0.5, 1.5, 3.0, 1.0, 0.25, 2.0, 0.75, 1.25]
    # Twelve rows of three regressors, the OLS corpus' own, with weights that fall as the
    # response grows: the shape a variance proportional to the level asks for.
    three = {
        DESIGN: [
            1.0, 4.0, 0.5, 2.0, 3.0, 1.5, 3.0, 5.0, 2.5, 4.0, 2.0, 0.5,
            5.0, 6.0, 3.5, 6.0, 1.0, 1.0, 7.0, 7.0, 4.5, 8.0, 3.0, 2.0,
            9.0, 8.0, 5.5, 10.0, 4.0, 2.5, 11.0, 9.0, 6.5, 12.0, 5.0, 3.0,
        ],
        RESPONSE: [7.2, 9.1, 12.4, 10.8, 16.3, 14.1, 20.7, 18.2, 24.9, 22.4, 29.1, 26.8],
        WEIGHTS: [
            1.0 / 7.2, 1.0 / 9.1, 1.0 / 12.4, 1.0 / 10.8, 1.0 / 16.3, 1.0 / 14.1,
            1.0 / 20.7, 1.0 / 18.2, 1.0 / 24.9, 1.0 / 22.4, 1.0 / 29.1, 1.0 / 26.8,
        ],
        OLS_FEATURE_COUNT: 3, WITH_INTERCEPT: True,
    }
    fixtures = [
        {"name": "one regressor, uneven weights", WEIGHTS: uneven, **line},
        {
            # Every weight one: the corpus then holds OLS numbers, which is the identity
            # the C# pins bit for bit against OrdinaryLeastSquares.Fit.
            "name": "one regressor, unit weights",
            WEIGHTS: [1.0] * 10, **line,
        },
        {
            # A constant weight moves the scale and nothing else: the estimates, their
            # errors and R-squared are OLS's, the residual standard error is doubled.
            "name": "one regressor, every weight four",
            WEIGHTS: [4.0] * 10, **line,
        },
        {
            # The row the weight zeroes still counts: df_resid is 8 on ten rows, not 7.
            "name": "one regressor, one zero weight",
            WEIGHTS: [1.0, 2.0, 0.5, 0.0, 3.0, 1.0, 0.25, 2.0, 0.75, 1.25], **line,
        },
        {
            # No intercept: R-squared is then the whitened uncentred one, sum of w y^2.
            "name": "two regressors, no intercept, uneven weights",
            DESIGN: [1.0, 1.0, 2.0, 1.0, 3.0, 2.0, 4.0, 2.0, 5.0, 3.0, 6.0, 3.0, 7.0, 4.0, 8.0, 4.0],
            RESPONSE: [3.4, 4.8, 8.9, 9.6, 12.7, 15.9, 19.2, 19.4],
            WEIGHTS: [2.0, 1.0, 1.5, 0.5, 1.0, 0.75, 0.5, 0.25],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {"name": "three regressors, inverse-level weights, 99%", CONFIDENCE_LEVEL: 0.99, **three},
        {
            # x2 is x1 plus a hundredth, as in the OLS corpus: the VIF is the design's own,
            # which is the number variance_inflation_factor returns for this exog.
            "name": "near-collinear regressors, uneven weights",
            DESIGN: [
                1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01,
            ],
            RESPONSE: [2.4, 3.6, 6.9, 7.1, 10.8, 11.2, 15.1, 14.6, 19.3, 19.9],
            WEIGHTS: uneven,
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
    ]
    # long-comment: what the robust cases catch, and why they stay on one regressor.
    # The model's constant stays out of the robust Wald test, where OLS on the whitened
    # rows would put it in -- 505 against 619 under HC0 here, so these four also tell
    # WLS from that shortcut. Kept to one regressor: a Wald statistic in the tens of
    # thousands is where decision 0073's absolute 1e-9 stops holding across machines.
    fixtures.extend(
        {"name": f"one regressor, uneven weights, {kind}", COVARIANCE_TYPE: kind,
         WEIGHTS: uneven, **line}
        for kind in ("HC0", "HC1", "HC2", "HC3")
    )
    fixtures.append({
        # A zero weight under HC3: the whitened row has leverage zero, so its 1 - h is
        # one rather than a division by zero, and HC1's n still counts it.
        "name": "one regressor, one zero weight, HC3",
        WEIGHTS: [1.0, 2.0, 0.5, 0.0, 3.0, 1.0, 0.25, 2.0, 0.75, 1.25],
        COVARIANCE_TYPE: "HC3", **line,
    })
    # long-comment: what the two cases catch, and why they do not use the line's response.
    # The reference's scores are the whitened rows times the whitened residuals, so
    # these two fail on a C# that clusters or lags the unweighted ones (#775).
    # A zigzag response rather than the line's: on the line the slope's z reaches 38,
    # where the reference's two-sided p-value underflows to zero and asserts nothing.
    zigzag = {**line, RESPONSE: [3.1, 1.9, 7.4, 5.2, 12.3, 8.1, 16.8, 10.9, 21.2, 14.7]}
    fixtures.append({
        "name": "one regressor, uneven weights, zigzag, HAC 3",
        COVARIANCE_TYPE: "HAC", HAC_LAGS: 3, WEIGHTS: uneven, **zigzag,
    })
    fixtures.append({
        "name": "one regressor, uneven weights, zigzag, four clusters",
        COVARIANCE_TYPE: CLUSTER, GROUPS: [2, 2, 4, 4, 4, 9, 9, 1, 1, 1],
        WEIGHTS: uneven, **zigzag,
    })
    return fixtures


def generate_stats_wls() -> dict:
    """WLS with the same table as OLS, rows scaled by the square root of a weight (#768)."""
    import numpy as np
    import statsmodels.api as sm

    cases = [
        _linear_case(
            fixture,
            sm.WLS(np.array(fixture[RESPONSE]), _linear_exog(fixture),
                   weights=np.array(fixture[WEIGHTS])),
        )
        for fixture in _wls_fixtures()
    ]

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "wls",
            "count": len(cases),
        },
        "cases": cases,
    }


def _autoregressive(rho: float, n: int) -> list[float]:
    """An AR(1) error covariance, rho^|i-j|, row-major: the correlated-neighbour case GLS exists for."""
    return [rho ** abs(i - j) for i in range(n) for j in range(n)]


def _gls_fixtures() -> list[dict]:
    """Error covariances chosen for what whitening by a full Cholesky factor can get wrong (#771).

    The responses carry visible noise, as the WLS corpus's do, so the robust Wald statistics stay in the
    hundreds where decision 0073's absolute 1e-9 holds across machines.
    """
    line = {
        DESIGN: [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0],
        RESPONSE: [2.4, 3.6, 6.9, 7.1, 10.8, 11.2, 15.1, 14.6, 19.3, 19.9],
        OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True,
    }
    # Two blocks of five equicorrelated rows at 0.5, uncorrelated across blocks, with unequal variances:
    # a covariance that is neither diagonal nor banded, so no shortcut through WLS reaches it.
    block = [
        (1.0 + 0.1 * i) * (1.0 + 0.1 * j) * (1.0 if i == j else 0.5) if (i < 5) == (j < 5) else 0.0
        for i in range(10) for j in range(10)
    ]
    fixtures = [
        {"name": "AR(1) errors, rho 0.6", ERROR_COVARIANCE: _autoregressive(0.6, 10),
         CONFIDENCE_LEVEL: 0.95, **line},
        {"name": "AR(1) errors, rho 0.3, 99%", ERROR_COVARIANCE: _autoregressive(0.3, 10),
         CONFIDENCE_LEVEL: 0.99, **line},
        {
            # The line's response lifted by 0.9: on the unlifted one this covariance fits an intercept of
            # -8.4e-5, where a relative 1e-9 would ask for agreement to 8e-14 and assert rounding instead.
            **line, "name": "two equicorrelated blocks, unequal variances", ERROR_COVARIANCE: block,
            RESPONSE: [3.3, 4.5, 7.8, 8.0, 11.7, 12.1, 16.0, 15.5, 20.2, 20.8], CONFIDENCE_LEVEL: 0.95,
        },
        {
            # No intercept: R-squared is then the whitened response's own uncentred square.
            "name": "two regressors, no intercept, AR(1) errors",
            DESIGN: [1.0, 1.0, 2.0, 1.0, 3.0, 2.0, 4.0, 2.0, 5.0, 3.0, 6.0, 3.0, 7.0, 4.0, 8.0, 4.0],
            RESPONSE: [3.4, 4.8, 8.9, 9.6, 12.7, 15.9, 19.2, 19.4],
            ERROR_COVARIANCE: _autoregressive(0.5, 8),
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "three regressors, twelve rows, AR(1) errors",
            DESIGN: [
                1.0, 4.0, 0.5, 2.0, 3.0, 1.5, 3.0, 5.0, 2.5, 4.0, 2.0, 0.5,
                5.0, 6.0, 3.5, 6.0, 1.0, 1.0, 7.0, 7.0, 4.5, 8.0, 3.0, 2.0,
                9.0, 8.0, 5.5, 10.0, 4.0, 2.5, 11.0, 9.0, 6.5, 12.0, 5.0, 3.0,
            ],
            RESPONSE: [7.9, 8.4, 13.1, 10.1, 17.2, 13.3, 21.9, 17.1, 26.0, 21.2, 30.3, 25.7],
            ERROR_COVARIANCE: _autoregressive(0.4, 12),
            OLS_FEATURE_COUNT: 3, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
    ]
    fixtures.extend(
        {"name": f"AR(1) errors, rho 0.6, {kind}", COVARIANCE_TYPE: kind,
         ERROR_COVARIANCE: _autoregressive(0.6, 10), CONFIDENCE_LEVEL: 0.95, **line}
        for kind in ("HC0", "HC1", "HC2", "HC3")
    )
    return fixtures


def generate_stats_gls() -> dict:
    """GLS with the same table as OLS, rows whitened by the error covariance's Cholesky factor (#771)."""
    import numpy as np
    import statsmodels.api as sm

    cases = []
    for fixture in _gls_fixtures():
        rows = len(fixture[RESPONSE])
        sigma = np.array(fixture[ERROR_COVARIANCE]).reshape(rows, rows)
        model = sm.GLS(np.array(fixture[RESPONSE]), _linear_exog(fixture), sigma=sigma)
        cases.append(_linear_case(fixture, model))

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "gls",
            "count": len(cases),
        },
        "cases": cases,
    }


def _glm_fixtures() -> list[dict]:
    """Designs chosen for what a link can get wrong, not for what a solve can.

    Hand-written rather than drawn: `_ols_fixtures` beside this is the idiom, and a
    generator sharing the module's random stream makes an unrelated corpus move when a
    case is added here.
    """
    return [
        {
            "name": "logistic, one regressor, intercept fitted",
            FAMILY: BINOMIAL,
            DESIGN: [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0],
            RESPONSE: [0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 0.0, 1.0, 1.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Two regressors at 99%, so a wrong multiplier fails on the interval rather than
            # on the coefficient it wraps; well conditioned, and y is not monotone in either x.
            "name": "logistic, two regressors, 99%",
            FAMILY: BINOMIAL,
            DESIGN: [
                2.0, 1.0, 5.0, 0.5, 1.0, 2.0, 4.0, 1.5, 3.0, 0.5, 6.0, 2.5,
                2.5, 1.5, 5.5, 1.0, 1.5, 0.5, 4.5, 2.0, 3.5, 1.0, 6.5, 2.5,
            ],
            RESPONSE: [
                0.0, 1.0, 0.0, 1.0, 1.0, 0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 0.0,
            ],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            # No intercept, the arm of the fit nothing else reaches -- and whose null
            # deviance is still the constant-only model's, exactly as the reference's is.
            "name": "logistic, no intercept",
            FAMILY: BINOMIAL,
            DESIGN: [-2.0, -1.5, -0.5, 0.5, 1.0, 1.5, 2.0, 2.5],
            RESPONSE: [0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 1.0, 1.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "poisson, one regressor, intercept fitted",
            FAMILY: POISSON,
            DESIGN: [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0],
            RESPONSE: [1.0, 0.0, 2.0, 3.0, 4.0, 3.0, 7.0, 6.0, 9.0, 11.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # A zero response, which is where the deviance's x-log-y term is 0 * -inf and
            # NaN in floating point if the limit is not taken.
            "name": "poisson, zeros in the response",
            FAMILY: POISSON,
            DESIGN: [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5],
            RESPONSE: [0.0, 0.0, 1.0, 0.0, 2.0, 1.0, 3.0, 4.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Two independent regressors: the second is not a multiple of the first, so the
            # design and its intercept span three directions and one solve reaches the fit.
            "name": "poisson, two regressors",
            FAMILY: POISSON,
            DESIGN: [
                1.0, 0.5, 2.0, 2.0, 3.0, 1.0, 4.0, 3.0, 5.0, 1.5,
                6.0, 2.5, 7.0, 0.5, 8.0, 3.5, 9.0, 2.0, 10.0, 1.0,
            ],
            RESPONSE: [1.0, 2.0, 2.0, 4.0, 5.0, 7.0, 8.0, 12.0, 15.0, 20.0],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        *_negative_binomial_fixtures(),
        *_gamma_fixtures(),
        *_glm_offset_fixtures(),
    ]


def _gamma_fixtures() -> list[dict]:
    """Positive skewed responses under the inverse link, statsmodels' default, and the log link (#770).

    The responses carry visible noise so the Pearson scale is not near zero: the log-likelihood reads
    1/scale, and a near-exact fit would push it where the reproducibility gate's absolute 1e-9 fails.
    """
    rising = {
        DESIGN: [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0],
        RESPONSE: [2.1, 1.8, 3.5, 2.9, 4.8, 5.5, 4.9, 7.8, 6.9, 9.4],
        OLS_FEATURE_COUNT: 1,
    }
    fixtures = [
        {"name": f"gamma, {link} link", FAMILY: GAMMA, LINK: link,
         WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95, **rising}
        for link in (INVERSE, "log")
    ]
    fixtures.extend([
        {
            # Falling: under the inverse link 1/mu rises with x, the direction that keeps mu positive.
            "name": "gamma, inverse link, a falling response at 99%",
            FAMILY: GAMMA, LINK: INVERSE,
            DESIGN: [0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0],
            RESPONSE: [8.2, 5.9, 5.1, 3.2, 3.6, 2.4, 2.7, 1.9, 2.2, 1.5],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            "name": "gamma, log link, no intercept",
            FAMILY: GAMMA, LINK: "log",
            DESIGN: [0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6],
            RESPONSE: [1.3, 1.1, 2.0, 1.6, 3.1, 2.4, 4.2, 3.3],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Two orders of magnitude, the spread a Gamma model exists for.
            "name": "gamma, log link, two regressors, a wide response",
            FAMILY: GAMMA, LINK: "log",
            DESIGN: [
                1.0, 0.5, 2.0, 2.0, 3.0, 1.0, 4.0, 3.0, 5.0, 1.5,
                6.0, 2.5, 7.0, 0.5, 8.0, 3.5, 9.0, 2.0, 10.0, 1.0,
            ],
            RESPONSE: [0.8, 3.1, 1.9, 9.5, 4.2, 12.8, 5.1, 44.0, 21.5, 30.2],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
    ])
    return fixtures


def _negative_binomial_fixtures() -> list[dict]:
    """The Poisson designs again, under a given alpha (#769).

    The same rows as the Poisson cases, so a reader can set a negative binomial table beside
    the Poisson one and see alpha widen it. Counts stay small on purpose: a large-count
    log-likelihood regenerates past the gate's absolute 1e-9 between hosts, which
    generate_regression_log_factorial's docstring measured.
    """
    counts = {
        DESIGN: [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0],
        RESPONSE: [1.0, 0.0, 2.0, 3.0, 4.0, 3.0, 7.0, 6.0, 9.0, 11.0],
        OLS_FEATURE_COUNT: 1,
    }
    fixtures = [
        {"name": f"negative binomial, alpha {alpha}", FAMILY: NEGATIVE_BINOMIAL, ALPHA: alpha,
         WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95, **counts}
        for alpha in (0.5, 1.0, 2.0)
    ]
    fixtures.extend([
        {
            # No intercept: the null deviance is still the constant-only model's.
            "name": "negative binomial, no intercept",
            FAMILY: NEGATIVE_BINOMIAL, ALPHA: 1.0,
            DESIGN: [0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0],
            RESPONSE: [1.0, 2.0, 1.0, 4.0, 3.0, 6.0, 5.0, 9.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Zeros: the deviance's y log(y/mu) is clipped rather than 0 * -inf.
            "name": "negative binomial, zeros in the response, 99%",
            FAMILY: NEGATIVE_BINOMIAL, ALPHA: 0.5,
            DESIGN: [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5],
            RESPONSE: [0.0, 0.0, 1.0, 0.0, 2.0, 1.0, 3.0, 4.0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            "name": "negative binomial, two regressors",
            FAMILY: NEGATIVE_BINOMIAL, ALPHA: 0.25,
            DESIGN: [
                1.0, 0.5, 2.0, 2.0, 3.0, 1.0, 4.0, 3.0, 5.0, 1.5,
                6.0, 2.5, 7.0, 0.5, 8.0, 3.5, 9.0, 2.0, 10.0, 1.0,
            ],
            RESPONSE: [1.0, 2.0, 2.0, 4.0, 5.0, 7.0, 8.0, 12.0, 15.0, 20.0],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # Near Poisson: 1/alpha is 1000, so the deviance and lnGamma(y + 1/alpha) -
            # lnGamma(1/alpha) both cancel large terms, the arithmetic a small alpha stresses.
            "name": "negative binomial, alpha 1e-3, near Poisson",
            FAMILY: NEGATIVE_BINOMIAL, ALPHA: 1e-3,
            WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95, **counts,
        },
    ])
    return fixtures


def _glm_offset_fixtures() -> list[dict]:
    """Offsets and exposures on each family, over hand-written rates (#787).

    Twelve rows of claims over exposures between half a unit and four, the shape a rate
    model is for: the count grows with the exposure, and the regressor moves the rate.
    """
    design = [0.0, 1.0, 2.0, 3.0, 0.5, 1.5, 2.5, 3.5, 0.2, 1.2, 2.2, 3.2]
    exposure = [1.0, 2.5, 0.5, 4.0, 3.0, 1.5, 2.0, 0.75, 3.5, 1.25, 2.75, 1.0]
    offset = [0.1, -0.2, 0.3, 0.0, -0.1, 0.2, -0.3, 0.15, 0.05, -0.25, 0.1, -0.05]
    claims = [1.0, 4.0, 1.0, 12.0, 3.0, 4.0, 7.0, 3.0, 2.0, 3.0, 9.0, 5.0]
    rates = {
        DESIGN: design, OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
    }
    return [
        {"name": "poisson, exposure", FAMILY: POISSON, RESPONSE: claims, EXPOSURE: exposure, **rates},
        {"name": "poisson, offset", FAMILY: POISSON, RESPONSE: claims, OFFSET: offset, **rates},
        {"name": "poisson, offset and exposure", FAMILY: POISSON, RESPONSE: claims,
         OFFSET: offset, EXPOSURE: exposure, **rates},
        {
            # An offset of zeros is not "no offset" to the reference: the null deviance
            # comes from refitting the intercept-only model, not from the response mean.
            "name": "poisson, an offset of zeros",
            FAMILY: POISSON, RESPONSE: claims, OFFSET: [0.0] * 12, **rates,
        },
        {"name": "negative binomial, exposure", FAMILY: NEGATIVE_BINOMIAL, ALPHA: 0.5,
         RESPONSE: claims, EXPOSURE: exposure, **rates},
        {
            "name": "gamma, log link, offset and exposure",
            FAMILY: GAMMA, LINK: "log",
            RESPONSE: [1.3, 5.2, 0.9, 14.8, 4.1, 3.9, 8.7, 2.2, 3.1, 2.8, 11.5, 4.4],
            OFFSET: offset, EXPOSURE: exposure, **rates,
        },
        {
            # No exposure: the logit is not a log link, and the reference refuses one there.
            "name": "logistic, offset",
            FAMILY: BINOMIAL, RESPONSE: [0.0, 0.0, 1.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 1.0, 1.0],
            OFFSET: offset, **rates,
        },
    ]


def generate_regression_log_factorial() -> dict:
    """``scipy.special.gammaln(k + 1)``, the ``log(y!)`` statsmodels' Poisson log-likelihood reads (#665).

    Internal to Lodestar.Stats.Regression, so compared relatively at the tolerance the GLM corpus
    uses: the counts reach 2**53, where ``log(k!)`` is 3e17 and an absolute 1e-9 would be meaningless.
    The table-to-series boundary at 256 is covered on both sides. This corpus, and not a GLM fit with
    counts that large, is the oracle: such a fit's log-likelihood cancels terms near ``y log y`` and
    regenerates 3.7e-9 apart between two hosts, past the absolute 1e-9 the reproducibility gate reads.
    """
    import scipy
    from scipy.special import gammaln

    counts = sorted({
        *range(0, 31), 99, 100, 254, 255, 256, 257, 1_000, 65_535, 65_536,
        999_999, 1_000_000, 1_000_001, 1_234_567, 10_000_000, 123_456_789,
        2**31 - 1, 2**31, 10**12, 2**40 + 1, 10**15, 2**53,
    })
    return {
        "metadata": {
            "library": "scipy",
            "version": scipy.__version__,
            FAMILY: "gammaln",
            VARIANT: "gammaln(k + 1)",
            "count": len(counts),
        },
        "cases": [{"count": float(k), "logFactorial": float(gammaln(k + 1.0))} for k in counts],
    }


def generate_regression_log_gamma() -> dict:
    """``scipy.special.gammaln(x)`` at non-integer x, the lnGamma(y + 1/alpha) the negative binomial reads (#769).

    Compared relatively, like the log-factorial corpus beside it; the points straddle the shift to 40
    the C# takes before its Stirling series, and reach below one, where lnGamma changes sign.
    """
    import scipy
    from scipy.special import gammaln

    points = [
        1e-3, 0.01, 0.1, 0.5, 1.5, 2.5, 10.0 / 3.0, 7.25, 19.5, 20.5, 39.5, 40.5,
        101.0 / 3.0, 1000.5, 12345.678, 1e6 + 0.25,
    ]
    return {
        "metadata": {
            "library": "scipy",
            "version": scipy.__version__,
            FAMILY: "gammaln",
            VARIANT: "gammaln(x)",
            "count": len(points),
        },
        "cases": [{"x": x, "logGamma": float(gammaln(x))} for x in points],
    }


# The multinomial logit corpus (#788): each fixture's labels and the categories they sort into.
LABELS = "labels"
CATEGORIES = "categories"


def _mnlogit_fixtures() -> list[dict]:
    """Designs drawn once from numpy's default_rng and frozen, each with a likelihood-ratio statistic above 25.

    long-comment: why the likelihood-ratio statistic has to be large.
    The pseudo-R-squared and the likelihood-ratio test read `llnull`, which statsmodels reaches by a
    Nelder-Mead and BFGS refit that lands up to 3e-10 off the closed form the C# computes (decision
    0136). The statistic is a difference of log-likelihoods, so a small one amplifies that gap past
    the corpus tolerance; above 25 it stays under 1e-10. Seeds 5, 2, 10 and 8, noted so they can be
    drawn again: category probabilities from normal coefficients, regressors rounded to two decimals.
    """
    one = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23, -0.96, 1.6, 0.2, -1.73,
           -0.08, -1.16, -0.63, -0.49, -0.71, 0.55, -0.06, -0.59, 0.41, 0.83, -1.64, -0.26, -0.98, -0.17, -1.29,
           0.02, -0.04, -0.3, -1.05, -0.4]
    one_labels = [2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2, 0, 0, 2, 2, 2, 0, 2, 2,
                  1, 2, 2, 2]
    return [
        {"name": "three categories, one regressor", DESIGN: one, LABELS: one_labels,
         OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95},
        {
            # The same rows relabelled -7, 0, 12: the categories sort by value, so the fit is the one above.
            "name": "labels negative and not contiguous",
            DESIGN: one, LABELS: [(-7, 0, 12)[v] for v in one_labels],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            "name": "four categories, two regressors, 99%",
            DESIGN: [
                0.19, -0.52, -0.41, -2.44, 1.8, 1.14, -0.33, 0.77, 0.28, -0.55, 0.98, -0.31, -0.33, -0.79, 0.45,
                -0.1, 0.55, -0.61, 0.13, -0.89, 0.84, 0.19, 0.33, 0.41, -1.01, 0.78, 2.06, -1.64, -1.73, -1.5, 0.84,
                0.13, 1.08, 0.72, 0.21, 0.28, -0.17, 0.87, -1.13, -0.42, 0.24, 1.8, -0.76, -1.08, -0.56, 0.97, -0.24,
                1.32, -1.87, 1.13, 1.03, -1.42, 0.15, 1.22, 0.09, 1.0, 2.37, 0.27, -0.28, -0.77, 0.65, -0.2, -0.18,
                -0.11, 0.65, -1.07, -1.53, -2.43, 1.2, 0.07, 1.51, -0.01, -0.74, 0.48, -0.08, -1.25, -0.89, 1.77,
                0.35, 0.42, -0.28, -0.69, 0.89, -0.1, -0.76, -0.13, -0.91, 0.19, 1.13, -0.84, 1.43, -0.67, 0.15,
                -0.84, -0.22, 0.05,
            ],
            LABELS: [0, 1, 3, 0, 0, 3, 2, 3, 3, 3, 0, 3, 2, 0, 1, 3, 3, 3, 3, 2, 3, 2, 2, 3, 2, 3, 0, 3, 3, 2, 0, 2,
                     0, 1, 0, 3, 2, 0, 2, 3, 2, 0, 3, 3, 1, 3, 1, 3],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.99,
        },
        {
            # Two categories: the binary logit, which GeneralizedLinearModel's binomial family also fits.
            "name": "two categories",
            DESIGN: [-1.1, -0.73, -0.78, 0.27, -0.25, 0.13, 0.84, 0.86, 0.48, -0.45, -0.75, -0.81, -0.34, -0.05,
                     -0.97, -1.13, 0.31, -1.85, -0.18, 0.43, -0.99, -1.11, -0.76, 0.65, -0.13, -1.87, -0.42, 1.01,
                     0.98, 0.63],
            LABELS: [1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0],
            OLS_FEATURE_COUNT: 1, WITH_INTERCEPT: True, CONFIDENCE_LEVEL: 0.95,
        },
        {
            # No intercept: df_model is still (K - 1)(J - 1), and llnull is still the constant-only model's.
            "name": "three categories, two regressors, no intercept",
            DESIGN: [
                -1.74, -1.34, -1.36, -0.35, -2.31, -0.19, -0.96, 0.89, 0.96, 1.39, 0.77, -0.05, 0.86, 1.51, -0.65,
                0.61, -0.04, 1.44, -0.84, -0.3, 0.36, 0.26, -1.64, 0.36, -0.12, -0.24, -0.16, 0.22, -1.82, 1.55,
                -0.86, -2.24, -0.08, 1.46, -0.52, 1.55, 1.56, -0.86, -2.47, -1.24, 1.19, -0.82, -1.51, -1.34, 0.0,
                -0.03, 0.87, 0.99, -0.93, -0.16, -1.13, 0.07, -1.15, -1.2, 2.12, 0.03, 0.64, 2.54, 0.79, -0.11, 0.05,
                -0.74, 1.09, -0.33, 1.76, -0.34, -0.35, -0.42, 0.63, 0.19, 1.37, -0.55,
            ],
            LABELS: [0, 2, 0, 0, 2, 2, 2, 2, 2, 0, 1, 0, 2, 0, 0, 0, 2, 2, 1, 0, 1, 0, 2, 2, 2, 0, 0, 1, 2, 2, 1, 1,
                     1, 0, 1, 2],
            OLS_FEATURE_COUNT: 2, WITH_INTERCEPT: False, CONFIDENCE_LEVEL: 0.95,
        },
    ]


def generate_stats_mnlogit() -> dict:
    """statsmodels' MNLogit, Newton from zeros with its default budget and tolerance (#788)."""
    import numpy as np
    import statsmodels.api as sm
    from scipy.stats import chi2
    from statsmodels.discrete.discrete_model import MNLogit

    def closed_null(labels: list[int]) -> float:
        counts = np.unique(np.array(labels), return_counts=True)[1]
        return float(np.sum(counts * np.log(counts / len(labels))))

    cases = []
    for fixture in _mnlogit_fixtures():
        design = np.array(fixture[DESIGN]).reshape(-1, fixture[OLS_FEATURE_COUNT])
        exog = sm.add_constant(design, prepend=True) if fixture[WITH_INTERCEPT] else design
        fit = MNLogit(np.array(fixture[LABELS]), exog).fit(disp=0)
        interval = np.asarray(fit.conf_int(alpha=1.0 - fixture[CONFIDENCE_LEVEL]))
        # params and the columns built from it are K x (J - 1); the C# reads equation first, so each is transposed.
        by_equation = lambda values: np.asarray(values).T.tolist()
        cases.append({
            "name": fixture["name"],
            DESIGN: fixture[DESIGN],
            LABELS: fixture[LABELS],
            OLS_FEATURE_COUNT: fixture[OLS_FEATURE_COUNT],
            WITH_INTERCEPT: fixture[WITH_INTERCEPT],
            CONFIDENCE_LEVEL: fixture[CONFIDENCE_LEVEL],
            CATEGORIES: sorted(set(fixture[LABELS])),
            COEFFICIENTS: by_equation(fit.params),
            STANDARD_ERRORS: by_equation(fit.bse),
            Z_STATISTICS: by_equation(fit.tvalues),
            P_VALUES: by_equation(fit.pvalues),
            CONFIDENCE_LOWER: interval[:, :, 0].tolist(),
            CONFIDENCE_UPPER: interval[:, :, 1].tolist(),
            LOG_LIKELIHOOD: float(fit.llf),
            "nullLogLikelihood": float(fit.llnull),
            "pseudoRSquared": float(fit.prsquared),
            "likelihoodRatio": float(fit.llr),
            "likelihoodRatioPValue": float(fit.llr_pvalue),
            # The same tail read on the closed-form null: statsmodels' refit null moves its own p-value by up
            # to 1.3e-8 relative, and this is the number the C# computes (decision 0136).
            "likelihoodRatioPValueClosedNull": float(chi2.sf(
                -2.0 * (closed_null(fixture[LABELS]) - fit.llf), fit.df_model)),
            AKAIKE: float(fit.aic),
            "bayesian": float(fit.bic),
            "modelDegreesOfFreedom": int(fit.df_model),
            RESIDUAL_DEGREES_OF_FREEDOM: int(fit.df_resid),
            ITERATIONS: int(fit.mle_retvals["iterations"]),
            CONVERGED: bool(fit.mle_retvals[CONVERGED]),
        })

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "mnlogit",
            "count": len(cases),
        },
        "cases": cases,
    }


# The vector autoregression corpus (#786): three series drawn once and frozen, and the keys its cases carry.
SERIES = "series"
VARIABLE_COUNT = "variableCount"
LAG_ORDER = "lagOrder"

TWO_LAG1 = [
    0.1968, -0.1307, 0.2167, -0.8291, 0.2534, 0.0921, 0.2934, -0.0748, -0.2623, 0.2421,
    0.4235, -0.3523, -0.2909, 0.4475, -0.2417, -0.9916, -0.3567, -0.3337, -1.0869, -0.5651,
    -0.5519, -1.0074, -1.2172, -0.1457, 0.2603, -0.7061, -0.7272, 0.3055, 0.3998, -0.6067,
    -0.0985, 0.6436, 0.1524, -0.6287, -0.2005, 0.1835, 0.5198, -0.8023, -0.7329, -0.1165,
    -0.9041, -0.3419, 0.0044, -0.7644, 0.2002, 0.4028, 0.4168, 0.0892, 0.4179, -0.5421,
    -0.0363, 0.4417, -0.6453, -0.0438, -0.2008, -0.0226, -0.2703, -0.1957, -0.0017, -0.5807,
    -0.0666, -0.0497, 0.1376, -0.36, 0.2595, 0.2901, 0.0015, 0.2383, 0.6207, 0.6586,
    -0.4468, 0.4404, 0.3539, -0.4179, -0.7072, 0.6866, -0.3091, -0.2761,
]
TWO_LAG2 = [
    -0.0314, 0.4229, -0.7265, 0.3649, -0.4906, 0.714, 0.0746, 0.6713, -0.3812, 0.4426,
    0.1965, -0.2687, -0.7469, 0.0101, -0.0639, -0.1779, -0.6415, -0.0305, 0.3526, -0.5965,
    -0.2208, 0.0135, 0.1897, -0.2637, -0.3142, 0.1129, 0.3254, -0.0131, -0.2106, 1.1165,
    0.7617, 0.1045, -0.1467, -0.1039, 0.5955, 0.1957, -0.1666, -0.3845, -0.0636, 0.0861,
    -0.093, 0.6169, 0.573, 0.0612, 0.2406, 0.8291, 1.0658, 0.8427, 0.5645, 1.2685,
    0.7735, 0.0452, 0.0492, -0.7069, -0.5603, -0.5541, 0.2707, 0.4428, -0.6351, -0.0863,
    -0.2273, -0.3356, -0.4894, 0.537, 0.742, 0.5177, -0.062, -0.1101, -0.3891, -0.1401,
    0.2386, 0.4546, 0.2497, 0.8799, 0.6078, -0.5634, -0.7895, -0.0419, 0.8156, -0.5691,
    -0.1336, 0.2624, 0.6725, 0.4389, 0.2885, -0.2514, 0.527, -0.1047,
]
THREE_LAG2 = [
    0.5237, 0.8299, 0.9592, 0.2414, 1.1466, 0.0107, -0.0631, 0.2254, -0.2168, 0.6802,
    0.3995, -0.0925, -0.0048, -0.0019, 1.3157, 1.3871, 0.0326, -0.6446, 1.1091, -0.4112,
    0.2016, 1.5921, -0.1166, 1.2056, 2.2295, 0.0756, 0.4952, 2.2421, 0.8768, 0.5655,
    1.702, 0.3822, 1.3635, 2.287, 0.9172, 0.4125, 1.9195, 0.6123, 0.3097, 1.2928,
    0.7359, 0.3915, 0.6547, 0.7707, 0.9481, 0.8255, -0.5828, -0.4715, 0.9815, -0.5295,
    -0.1214, 2.4496, -0.7544, 0.8107, 3.4356, -0.4409, 1.7087, 3.5538, -0.1367, 1.5075,
    2.8186, -0.5296, 1.1366, 3.9822, -0.2655, -0.1503, 3.3847, -0.7249, 1.5475, 5.3305,
    -0.4771, 2.1101, 5.2501, 0.2219, 2.4431, 4.4188, 0.2975, 0.9211, 4.0714, 1.2273,
    0.7577, 2.0245, 0.5205, 0.9612, 1.8964, 1.4289, -0.425, 0.3426, 0.4742, 0.9356,
    1.1713, 0.4568, 0.0455, 1.1443, 0.1017, -0.0256, 0.7482, 0.6519, 0.5828, 0.0915,
    0.0845, 1.578, 0.9619, 0.8349, -0.3307, -0.2309, 0.4098, -0.2234, -0.1247, -0.8403,
    1.2656, 2.0449, -0.1443, -0.0134, 1.1285, 0.4751, 0.8214, 1.4318, -0.5283, 0.6899,
    3.1505, -0.0665, 0.637, 2.7762, 0.4977, 2.031, 2.8339, 0.1423, 0.8716, 2.6911,
    0.3152, 0.301, 2.5008, -0.1879, 0.8281, 4.2862, 0.3294, 1.063, 4.5841, 0.0905,
    0.7739, 3.3585, -0.4967, 1.7218, 4.8835, -1.1196, 1.308, 4.9428, -0.1418, 1.7451,
    5.0881, -0.1331, 1.6613, 4.5413, 0.3338, 1.7209, 4.6797, 1.5635, 1.439, 3.2109,
    1.8486, 1.2099, 1.2299, 0.385, 0.7772, 2.0044, 1.1348, -0.79, 0.4961, 1.4501,
    1.1017, -0.3644, 0.5083, 1.0321,
]


def _var_fixtures() -> list[dict]:
    """Stable systems drawn once from numpy's default_rng, seeds 11, 12 and 13, rounded to four decimals.

    long-comment: why the systems are stable and what that buys the corpus.
    A vector autoregression whose companion matrix has an eigenvalue outside the unit circle explodes, and
    a frozen explosive series is a corpus of overflow rather than of a fit. The coefficient matrices are
    drawn at a scale of 0.35, which kept every draw here stable, and the largest t statistic per fixture
    is between 5 and 9 -- large enough that the replay reads as a test rather than as noise.
    """
    return [
        {"name": "two variables, one lag", SERIES: TWO_LAG1, VARIABLE_COUNT: 2, LAG_ORDER: 1, WITH_INTERCEPT: True},
        {"name": "two variables, two lags", SERIES: TWO_LAG2, VARIABLE_COUNT: 2, LAG_ORDER: 2, WITH_INTERCEPT: True},
        {"name": "two variables, two lags, no intercept", SERIES: TWO_LAG2, VARIABLE_COUNT: 2, LAG_ORDER: 2,
         WITH_INTERCEPT: False},
        {"name": "three variables, two lags", SERIES: THREE_LAG2, VARIABLE_COUNT: 3, LAG_ORDER: 2,
         WITH_INTERCEPT: True},
    ]


def generate_stats_var() -> dict:
    """statsmodels' VAR: least squares on the stacked lags, with the table it prints (#786)."""
    import numpy as np
    from statsmodels.tsa.api import VAR

    cases = []
    for fixture in _var_fixtures():
        variables = fixture[VARIABLE_COUNT]
        series = np.array(fixture[SERIES]).reshape(-1, variables)
        fit = VAR(series).fit(fixture[LAG_ORDER], trend="c" if fixture[WITH_INTERCEPT] else "n")
        # params and the columns built from it are (1 + K*p) x K, one column per equation; the C# reads
        # equation first, so each is transposed.
        cases.append({
            "name": fixture["name"],
            SERIES: fixture[SERIES],
            VARIABLE_COUNT: variables,
            LAG_ORDER: fixture[LAG_ORDER],
            WITH_INTERCEPT: fixture[WITH_INTERCEPT],
            COEFFICIENTS: np.asarray(fit.params).T.tolist(),
            STANDARD_ERRORS: np.asarray(fit.stderr).T.tolist(),
            "tStatistics": np.asarray(fit.tvalues).T.tolist(),
            P_VALUES: np.asarray(fit.pvalues).T.tolist(),
            "residualCovariance": np.asarray(fit.sigma_u).ravel().tolist(),
            "residualCovarianceMaximumLikelihood": np.asarray(fit.sigma_u_mle).ravel().tolist(),
            LOG_LIKELIHOOD: float(fit.llf),
            AKAIKE: float(fit.aic),
            "bayesian": float(fit.bic),
            "hannanQuinn": float(fit.hqic),
            "finalPredictionError": float(fit.fpe),
            "observationsUsed": int(fit.nobs),
            "modelDegreesOfFreedom": int(fit.df_model),
            RESIDUAL_DEGREES_OF_FREEDOM: int(fit.df_resid),
        })

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "var",
            "count": len(cases),
        },
        "cases": cases,
    }


# The splitter corpus (#762): its keys, and the calls it freezes.
FOLD_COUNT = "foldCount"
SAMPLE_COUNT = "sampleCount"
LABELS_KEY = "labels"
ORDER = "order"
TEST_FRACTION = "testFraction"
TRAIN_INDICES = "trainIndices"
TEST_INDICES = "testIndices"
FOLDS = "folds"
CALL_STRATIFIED = "stratified"


def generate_preprocessing_splitters() -> dict:
    """scikit-learn's KFold, StratifiedKFold and train_test_split, as index lists (#762).

    long-comment: why one shuffled case of each is frozen with its permutation.
    Unshuffled, the three are deterministic and this package matches them exactly. Shuffled, the
    reference draws its permutation from `random_state` through numpy's generator, which nothing
    here reproduces -- so the permutation is frozen beside the folds and handed to the C# as the
    input it is (decision 0132), which pins the allocation rule rather than the draw.
    """
    import numpy as np
    import sklearn
    from sklearn.model_selection import KFold, StratifiedKFold, train_test_split

    cases: list[dict] = []
    for samples, folds in ((10, 3), (12, 4), (11, 4), (7, 7), (12, 5)):
        # NOSONAR S6709: unshuffled, and the reference refuses a random_state here --
        # "Setting a random_state has no effect since shuffle is False" is a ValueError.
        splits = list(KFold(n_splits=folds).split(np.zeros((samples, 1))))  # NOSONAR S6709
        cases.append({
            "name": f"kfold, {samples} rows in {folds} folds",
            "call": "kfold", SAMPLE_COUNT: samples, FOLD_COUNT: folds,
            FOLDS: [{TRAIN_INDICES: train.tolist(), TEST_INDICES: test.tolist()} for train, test in splits],
        })

    labelled = {
        "balanced, three classes": [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2],
        "unbalanced, a class of two": [1, 1, 1, 1, 1, 0, 0, 0, 2, 2],
        "two classes, odd counts": [0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0],
        "labels that are not contiguous": [7, 7, 7, -3, -3, -3, 40, 40, 40, 7],
    }
    for name, labels in labelled.items():
        for folds in (2, 3):
            splits = list(  # NOSONAR S6709: unshuffled, see above
                StratifiedKFold(n_splits=folds)  # NOSONAR S6709
                .split(np.zeros((len(labels), 1)), np.array(labels)))
            cases.append({
                "name": f"stratified {folds} folds, {name}",
                "call": CALL_STRATIFIED, LABELS_KEY: labels, FOLD_COUNT: folds,
                FOLDS: [{TRAIN_INDICES: train.tolist(), TEST_INDICES: test.tolist()} for train, test in splits],
            })

    for samples, fraction in ((10, 0.2), (11, 0.25), (8, 0.5), (9, 0.34)):
        train, test = train_test_split(  # NOSONAR S6709: shuffle=False ignores a random_state
            np.arange(samples), test_size=fraction, shuffle=False)
        cases.append({
            "name": f"train/test, {samples} rows at {fraction}",
            "call": "trainTest", SAMPLE_COUNT: samples, TEST_FRACTION: fraction,
            TRAIN_INDICES: train.tolist(), TEST_INDICES: test.tolist(),
        })

    # One shuffled case of each, the permutation frozen beside the folds.
    rng = np.random.default_rng(762)
    order = rng.permutation(12)
    shuffled = np.asarray(order)
    kfold_shuffled = [
        {TRAIN_INDICES: sorted(shuffled[train].tolist()), TEST_INDICES: sorted(shuffled[test].tolist())}
        # NOSONAR S6709: the permutation is applied above and frozen in the case; the split itself
        # is unshuffled, which is what makes it reproducible from the permutation alone.
        for train, test in KFold(n_splits=3).split(np.zeros((12, 1)))  # NOSONAR S6709
    ]
    cases.append({
        "name": "kfold, 12 rows in 3 folds, permuted",
        "call": "kfold", SAMPLE_COUNT: 12, FOLD_COUNT: 3, ORDER: shuffled.tolist(), FOLDS: kfold_shuffled,
    })

    labels = [0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2]
    permuted_labels = [labels[i] for i in shuffled.tolist()]
    folds_of = list(  # NOSONAR S6709: unshuffled, see above
        StratifiedKFold(n_splits=3)  # NOSONAR S6709
        .split(np.zeros((12, 1)), np.array(permuted_labels)))
    cases.append({
        "name": "stratified 3 folds, balanced, permuted",
        "call": CALL_STRATIFIED, LABELS_KEY: labels, FOLD_COUNT: 3, ORDER: shuffled.tolist(),
        FOLDS: [
            {TRAIN_INDICES: sorted(shuffled[train].tolist()), TEST_INDICES: sorted(shuffled[test].tolist())}
            for train, test in folds_of
        ],
    })

    # #893: classes numbered over the reading, and a shuffled train/test split drawn by the reference
    # itself, whose RandomState(seed).permutation is the order ShuffleSplit reads.
    first_seen_labels = [1, 1, 1, 1, 0, 0, 2, 2, 0, 2]
    # Written out rather than drawn: any order that meets class 1 after 0 and 2 serves.
    first_seen_order = np.array([1, 9, 5, 8, 0, 7, 4, 3, 6, 2])
    folds_of = list(  # NOSONAR S6709: unshuffled, see above
        StratifiedKFold(n_splits=2)  # NOSONAR S6709
        .split(np.zeros((10, 1)), np.array(first_seen_labels)[first_seen_order]))
    cases.append({
        "name": "stratified 2 folds, classes first seen in the permuted order",
        "call": CALL_STRATIFIED, LABELS_KEY: first_seen_labels, FOLD_COUNT: 2, ORDER: first_seen_order.tolist(),
        FOLDS: [
            {TRAIN_INDICES: sorted(first_seen_order[train].tolist()),
             TEST_INDICES: sorted(first_seen_order[test].tolist())}
            for train, test in folds_of
        ],
    })
    for samples, fraction in ((10, 0.25), (11, 0.34)):
        train, test = train_test_split(np.arange(samples), test_size=fraction, random_state=893)
        cases.append({
            "name": f"train/test, {samples} rows at {fraction}, permuted",
            "call": "trainTest", SAMPLE_COUNT: samples, TEST_FRACTION: fraction,
            # ShuffleSplit permutes with RandomState, so the order it reads has to come from one too.
            ORDER: np.random.RandomState(893).permutation(samples).tolist(),  # NOSONAR S6711
            TRAIN_INDICES: sorted(train.tolist()), TEST_INDICES: sorted(test.tolist()),
        })

    return {
        "metadata": {
            "library": "scikit-learn",
            "version": sklearn.__version__,
            FAMILY: "splitters",
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_stats_glm() -> dict:
    """statsmodels' GLM, one block per family (#616).

    Separate blocks rather than one flat list, the shape #645 settled for two MinHash
    schemes: a family added later grows the file instead of rewriting it.

    The separable block's budget is 15, not the 25 the plan first reached for: at 25,
    and from 21 on, statsmodels' own deviance criterion is satisfied even though the
    coefficients have not stabilised (measured, not assumed — see the task report).
    Fifteen is where statsmodels' own IRLS genuinely exhausts its budget rather than
    reaching a criterion, so the non-converged branch this fixture exists to exercise
    is the one actually replayed.
    """
    import numpy as np
    import statsmodels.api as sm

    families = {
        BINOMIAL: lambda _: sm.families.Binomial(),
        POISSON: lambda _: sm.families.Poisson(),
        NEGATIVE_BINOMIAL: lambda fixture: sm.families.NegativeBinomial(alpha=fixture[ALPHA]),
        GAMMA: lambda fixture: sm.families.Gamma(
            link=sm.families.links.InversePower() if fixture[LINK] == INVERSE else sm.families.links.Log()),
    }
    blocks: dict = {name: {"cases": []} for name in families}
    for fixture in _glm_fixtures():
        feature_count = fixture[OLS_FEATURE_COUNT]
        design = np.array(fixture[DESIGN]).reshape(-1, feature_count)
        response = np.array(fixture[RESPONSE])
        exog = sm.add_constant(design, prepend=True) if fixture[WITH_INTERCEPT] else design
        # OFFSET and EXPOSURE are spelled as GLM's own keyword arguments, so they name both.
        terms = {key: np.array(fixture[key]) for key in (OFFSET, EXPOSURE) if key in fixture}
        fit = sm.GLM(response, exog, family=families[fixture[FAMILY]](fixture), **terms).fit()
        interval = fit.conf_int(alpha=1.0 - fixture[CONFIDENCE_LEVEL])
        blocks[fixture[FAMILY]]["cases"].append({
            "name": fixture["name"],
            DESIGN: [float(v) for v in fixture[DESIGN]],
            RESPONSE: [float(v) for v in fixture[RESPONSE]],
            OLS_FEATURE_COUNT: feature_count,
            WITH_INTERCEPT: fixture[WITH_INTERCEPT],
            CONFIDENCE_LEVEL: fixture[CONFIDENCE_LEVEL],
            **({ALPHA: fixture[ALPHA]} if ALPHA in fixture else {}),
            **({LINK: fixture[LINK]} if LINK in fixture else {}),
            **({OFFSET: fixture[OFFSET]} if OFFSET in fixture else {}),
            **({EXPOSURE: fixture[EXPOSURE]} if EXPOSURE in fixture else {}),
            COEFFICIENTS: [float(v) for v in fit.params],
            STANDARD_ERRORS: [float(v) for v in fit.bse],
            Z_STATISTICS: [float(v) for v in fit.tvalues],
            P_VALUES: [float(v) for v in fit.pvalues],
            CONFIDENCE_LOWER: [float(v) for v in interval[:, 0]],
            CONFIDENCE_UPPER: [float(v) for v in interval[:, 1]],
            "deviance": float(fit.deviance),
            "nullDeviance": float(fit.null_deviance),
            "dispersion": float(fit.scale),
            LOG_LIKELIHOOD: float(fit.llf),
            AKAIKE: float(fit.aic),
            RESIDUAL_DEGREES_OF_FREEDOM: int(fit.df_resid),
            CONVERGED: bool(fit.converged),
            ITERATIONS: int(fit.fit_history["iteration"]),
        })

    # One separable design, which does not converge at this budget. Frozen like any
    # other case so the non-converged branch is replayed rather than asserted by hand.
    separable_x = np.array([[-2.0], [-1.0], [1.0], [2.0]])
    separable_y = np.array([0.0, 0.0, 1.0, 1.0])
    separable_maxiter = 15
    separable = sm.GLM(
        separable_y, sm.add_constant(separable_x), family=sm.families.Binomial()
    ).fit(maxiter=separable_maxiter)
    blocks["separable"] = {
        DESIGN: [float(v) for v in separable_x.ravel()],
        RESPONSE: [float(v) for v in separable_y],
        OLS_FEATURE_COUNT: 1,
        "maximumIterations": separable_maxiter,
        CONVERGED: bool(separable.converged),
        ITERATIONS: int(separable.fit_history["iteration"]),
    }

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "glm",
            VARIANT: "GLM(family=Binomial|Poisson|NegativeBinomial(alpha)|Gamma(link)).fit(), IRLS",
            "count": sum(len(b["cases"]) for b in blocks.values() if "cases" in b),
        },
        **blocks,
    }


def generate_internal_validity() -> dict:
    """Calinski-Harabasz and Davies-Bouldin, which score a clustering with no reference (#192)."""
    import numpy as np
    from sklearn.metrics import calinski_harabasz_score, davies_bouldin_score

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _internal_validity_fixtures():
            features = np.array(fixture["features"])
            labels = np.array(fixture["labels"])
            cases.append({
                "name": fixture["name"],
                "features": [v for row in fixture["features"] for v in row],
                FEATURE_COUNT: int(features.shape[1]),
                "labels": fixture["labels"],
                "calinski_harabasz": float(calinski_harabasz_score(features, labels)),
                "davies_bouldin": float(davies_bouldin_score(features, labels)),
            })

    return {
        "metadata": {
            "algorithm": "InternalValidity",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.calinski_harabasz_score",
                "sklearn.metrics.davies_bouldin_score",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


def _calibration_fixtures() -> list[dict]:
    """Binary probability vectors, including the ones the clip decides."""
    return [
        {"name": WORKED_CASE, "true": [0, 1, 1, 0], "proba": [0.1, 0.9, 0.8, 0.3],
         "pos_label": 1, "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [0, 1, 1, 0], "proba": [0.1, 0.9, 0.8, 0.3],
         "pos_label": 1, "weight": [1.0, 2.0, 3.0, 4.0]},
        {"name": "the same, scored about the other class", "true": [0, 1, 1, 0],
         "proba": [0.1, 0.9, 0.8, 0.3], "pos_label": 0, "weight": None},
        # A predicted 0 for the true class is where log loss would be infinite and the
        # clip decides the number instead; brier reads the same input as an ordinary 1.
        {"name": "certain and wrong", "true": [1, 0], "proba": [0.0, 0.5],
         "pos_label": 1, "weight": None},
        # long-comment: the exclusion below is a reproducibility claim, and a reader
        # who does not know why will delete the flag and re-break the drift gate.
        # A perfect prediction scores -log(1 - eps), whose last bits follow the libm
        # that computed it -- measured, a CI runner and this one disagree by one ulp
        # on 2.220446049250313e-16. The brier score on the same fixture is exactly 0
        # and is kept; what the log loss does at the clip is asserted by a test with
        # a tolerance rather than frozen here.
        {"name": "certain and right", "true": [0, 1], "proba": [0.0, 1.0],
         "pos_label": 1, "weight": None, "skip_log_loss": True},
        {"name": "just above the clip", "true": [1, 0], "proba": [1e-15, 0.5],
         "pos_label": 1, "weight": None},
        {"name": "just below the clip", "true": [1, 0], "proba": [1e-20, 0.5],
         "pos_label": 1, "weight": None},
        {"name": "uninformative, every probability one half", "true": [0, 1, 1, 0],
         "proba": [0.5, 0.5, 0.5, 0.5], "pos_label": 1, "weight": None},
        {"name": SIGNED_LABELS, "true": [-1, 1, 1, -1], "proba": [0.1, 0.9, 0.8, 0.3],
         "pos_label": 1, "weight": None},
    ]


def generate_calibration() -> dict:
    """Brier score and log loss -- the calibration family (#174)."""
    import numpy as np
    from sklearn.metrics import brier_score_loss, log_loss

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _calibration_fixtures():
            true = np.array(fixture["true"])
            proba = np.array(fixture["proba"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}
            labels = sorted(set(fixture["true"]))
            # log_loss has no pos_label -- a 1-D column always describes the greater
            # label -- so the other class is the same call on the complement.
            log_proba = proba if fixture["pos_label"] == labels[-1] else 1.0 - proba
            cases.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_proba": fixture["proba"],
                "pos_label": fixture["pos_label"],
                "sample_weight": fixture["weight"],
                "brier": float(brier_score_loss(
                    true, proba, pos_label=fixture["pos_label"], **kw)),
                "brier_unscaled": float(brier_score_loss(
                    true, proba, pos_label=fixture["pos_label"], **{"scale_by_half": False}, **kw)),
            })
            if not fixture.get("skip_log_loss"):
                cases[-1]["log_loss"] = float(log_loss(true, log_proba, labels=labels, **kw))
                cases[-1]["log_loss_total"] = float(
                    log_loss(true, log_proba, labels=labels, normalize=False, **kw))

    # A probability matrix, where scale_by_half's 'auto' resolves the other way.
    multi_true = [0, 1, 2, 1]
    multi_proba = [[0.7, 0.2, 0.1], [0.1, 0.8, 0.1], [0.2, 0.2, 0.6], [0.3, 0.4, 0.3]]
    mt = np.array(multi_true)
    mp = np.array(multi_proba)
    weight = np.array([1.0, 2.0, 3.0, 4.0])
    multiclass = {
        "y_true": multi_true,
        "y_proba": [v for row in multi_proba for v in row],
        "class_count": 3,
        "sample_weight": [1.0, 2.0, 3.0, 4.0],
        "log_loss": float(log_loss(mt, mp)),
        "log_loss_total": float(log_loss(mt, mp, normalize=False)),
        "log_loss_weighted": float(log_loss(mt, mp, sample_weight=weight)),
        "brier": float(brier_score_loss(mt, mp)),
        # scale_by_half arrives through a dict: it is new in scikit-learn 1.9, and the
        # analyser's stub for this function is older and reports it as unexpected.
        "brier_scaled": float(brier_score_loss(mt, mp, **{"scale_by_half": True})),
        # Rows summing to one half: the reference warns and scores them as given
        # rather than renormalising, which is the number reproduced here.
        "half_rows_log_loss": float(log_loss(mt, mp * 0.5)),
        "half_rows_brier": float(brier_score_loss(mt, mp * 0.5)),
    }

    return {
        "metadata": {
            "algorithm": "Calibration",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.brier_score_loss",
                "sklearn.metrics.log_loss",
            ],
            "count": len(cases),
        },
        "cases": cases,
        "multiclass": multiclass,
    }


def _calibration_curve_fixtures() -> list[dict]:
    """Probability vectors chosen so a bin comes out empty on some of them."""
    return [
        # Four points over five uniform bins: three bins are empty, which is what makes
        # the returned arrays shorter than n_bins and their length depend on the data.
        {"name": WORKED_CASE, "true": [0, 1, 1, 0], "proba": [0.1, 0.9, 0.8, 0.3],
         "pos_label": 1, "n_bins": 5, "strategy": "uniform"},
        # The same points binned by quantile: every bin holds one, so nothing is dropped
        # and the edges come from np.percentile rather than from a linear split.
        {"name": "the same, binned by quantile", "true": [0, 1, 1, 0],
         "proba": [0.1, 0.9, 0.8, 0.3], "pos_label": 1, "n_bins": 4, "strategy": "quantile"},
        {"name": "scored about the other class", "true": [0, 1, 1, 0],
         "proba": [0.1, 0.9, 0.8, 0.3], "pos_label": 0, "n_bins": 5, "strategy": "uniform"},
        # Every probability in one bin: one point out, and prob_true is the bin's rate.
        {"name": "every probability in one bin", "true": [0, 1, 1, 0],
         "proba": [0.42, 0.44, 0.46, 0.48], "pos_label": 1, "n_bins": 5,
         "strategy": "uniform"},
        {"name": "the endpoints, zero and one", "true": [0, 1, 0, 1],
         "proba": [0.0, 1.0, 0.0, 1.0], "pos_label": 1, "n_bins": 5, "strategy": "uniform"},
        {"name": SIGNED_LABELS, "true": [-1, 1, 1, -1],
         "proba": [0.1, 0.9, 0.8, 0.3], "pos_label": 1, "n_bins": 5, "strategy": "uniform"},
        # Ties under quantile: repeated values collapse bin edges, so bins come out empty
        # on the strategy where the uniform split would have filled them.
        {"name": "ties collapse quantile edges", "true": [0, 0, 1, 1, 1, 0],
         "proba": [0.2, 0.2, 0.2, 0.8, 0.8, 0.8], "pos_label": 1, "n_bins": 4,
         "strategy": "quantile"},
        {"name": "ten points over three bins", "true": [0, 0, 1, 0, 1, 1, 0, 1, 1, 1],
         "proba": [0.05, 0.15, 0.25, 0.35, 0.45, 0.55, 0.65, 0.75, 0.85, 0.95],
         "pos_label": 1, "n_bins": 3, "strategy": "uniform"},
    ]


def generate_calibration_curve() -> dict:
    """The reliability curve -- sklearn.calibration, not sklearn.metrics (#286)."""
    import numpy as np
    from sklearn.calibration import calibration_curve

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _calibration_curve_fixtures():
            prob_true, prob_pred = calibration_curve(
                np.array(fixture["true"]),
                np.array(fixture["proba"]),
                pos_label=fixture["pos_label"],
                n_bins=fixture["n_bins"],
                strategy=fixture["strategy"])
            cases.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_proba": fixture["proba"],
                "pos_label": fixture["pos_label"],
                "n_bins": fixture["n_bins"],
                "strategy": fixture["strategy"],
                "prob_true": [float(v) for v in prob_true],
                "prob_pred": [float(v) for v in prob_pred],
            })

    return {
        "metadata": {
            "algorithm": "CalibrationCurve",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            # Not sklearn.metrics: the reliability curve lives in sklearn.calibration,
            # and naming the other module in the equivalence table would be false.
            "reference_calls": ["sklearn.calibration.calibration_curve"],
            "count": len(cases),
        },
        "cases": cases,
    }


def _curve_fixtures() -> list[dict]:
    """Score vectors chosen so drop_intermediate changes a length on some of them."""
    return [
        {"name": WORKED_CASE, "true": [0, 0, 1, 1], "score": [0.1, 0.4, 0.35, 0.8],
         "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [0, 0, 1, 1], "score": [0.1, 0.4, 0.35, 0.8],
         "weight": [1.0, 2.0, 3.0, 4.0]},
        # Ten samples with a long collinear run: the one fixture where every curve's
        # drop_intermediate actually drops, and by a different amount on each.
        {"name": "a run drop_intermediate shortens",
         "true": [0, 0, 0, 0, 1, 1, 1, 1, 0, 1],
         "score": [0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.05], "weight": None},
        {"name": "perfectly separated", "true": [0, 0, 1, 1], "score": [0.1, 0.2, 0.8, 0.9],
         "weight": None},
        {"name": "perfectly inverted", "true": [1, 1, 0, 0], "score": [0.1, 0.2, 0.8, 0.9],
         "weight": None},
        {"name": ALL_TIED, "true": [0, 1, 0, 1], "score": [0.5, 0.5, 0.5, 0.5],
         "weight": None},
        {"name": "a tie spanning both classes", "true": [1, 0, 1, 0, 1],
         "score": [0.9, 0.5, 0.5, 0.5, 0.1], "weight": None},
    ]


def generate_curves() -> dict:
    """roc_curve, precision_recall_curve and det_curve as plot data (#212)."""
    import numpy as np
    from sklearn.metrics import auc, det_curve, precision_recall_curve, roc_auc_score, roc_curve

    def arrays(triple) -> dict:
        first, second, thresholds = triple
        # JSON has no infinity, so the threshold at +inf -- where the model always
        # answers negative -- travels as a string the C# side reads back.
        return {
            "first": [float(v) for v in first],
            "second": [float(v) for v in second],
            "thresholds": ["Infinity" if np.isinf(v) else float(v) for v in thresholds],
        }

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _curve_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}

            case = {
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_score": fixture["score"],
                "sample_weight": fixture["weight"],
                "roc_auc": float(roc_auc_score(true, score, **kw)),
            }
            for drop in (True, False):
                case[f"roc_{drop}"] = arrays(roc_curve(true, score, drop_intermediate=drop, **kw))
                case[f"pr_{drop}"] = arrays(
                    precision_recall_curve(true, score, drop_intermediate=drop, **kw))
                case[f"det_{drop}"] = arrays(det_curve(true, score, drop_intermediate=drop, **kw))

            # The area the curve's own points integrate to, which is what the trapezoid
            # must reproduce -- and which roc_auc equals, an invariant no oracle states.
            fpr, tpr, _ = roc_curve(true, score, **kw)
            case["roc_trapezoid"] = float(auc(fpr, tpr))
            cases.append(case)

    return {
        "metadata": {
            "algorithm": "Curves",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.roc_curve",
                "sklearn.metrics.precision_recall_curve",
                "sklearn.metrics.det_curve",
                "sklearn.metrics.auc",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_label_losses() -> dict:
    """hamming_loss, zero_one_loss and jaccard_score -- #93's cheap leftovers (#211)."""
    import numpy as np
    from sklearn.metrics import hamming_loss, jaccard_score, zero_one_loss

    single = [
        {"name": WORKED_CASE, "true": [0, 1, 2, 1], "pred": [0, 2, 2, 1], "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [0, 1, 2, 1], "pred": [0, 2, 2, 1],
         "weight": [1.0, 2.0, 3.0, 4.0]},
        {"name": "every prediction right", "true": [0, 1, 2], "pred": [0, 1, 2], "weight": None},
        {"name": "every prediction wrong", "true": [0, 1, 2], "pred": [1, 2, 0], "weight": None},
        {"name": "two classes", "true": [0, 1, 1, 0], "pred": [0, 1, 0, 0], "weight": None},
    ]

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in single:
            true = np.array(fixture["true"])
            pred = np.array(fixture["pred"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}
            case = {
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_pred": fixture["pred"],
                "sample_weight": fixture["weight"],
                HAMMING: float(hamming_loss(true, pred, **kw)),
                "zero_one": float(zero_one_loss(true, pred, **kw)),
                "zero_one_count": float(zero_one_loss(true, pred, normalize=False, **kw)),
                "jaccard_per_class": [
                    float(v) for v in jaccard_score(true, pred, average=None, zero_division=0.0, **kw)],
            }
            for average in ("macro", "micro", "weighted"):
                case[f"jaccard_{average}"] = float(
                    jaccard_score(true, pred, average=average, zero_division=0.0, **kw))
            cases.append(case)

    # A label matrix, where hamming counts labels and zero-one counts rows.
    multi_true = [[1, 0, 1], [0, 1, 1]]
    multi_pred = [[1, 0, 0], [1, 1, 1]]
    mt = np.array(multi_true)
    mp = np.array(multi_pred)
    weight = np.array([1.0, 3.0])
    multilabel = {
        "y_true": [v for row in multi_true for v in row],
        "y_pred": [v for row in multi_pred for v in row],
        "label_count": 3,
        "sample_weight": [1.0, 3.0],
        HAMMING: float(hamming_loss(mt, mp)),
        "hamming_weighted": float(hamming_loss(mt, mp, sample_weight=weight)),
        "zero_one": float(zero_one_loss(mt, mp)),
        "zero_one_count": float(zero_one_loss(mt, mp, normalize=False)),
        "zero_one_weighted": float(zero_one_loss(mt, mp, sample_weight=weight)),
    }

    # A label neither side carries: the zero-division case, which needs an explicit
    # label set to reach at all.
    undefined = {
        "y_true": [0, 1],
        "y_pred": [0, 1],
        "labels": [0, 1, 2],
        "jaccard_zero": [float(v) for v in jaccard_score(
            np.array([0, 1]), np.array([0, 1]), average=None, labels=[0, 1, 2], zero_division=0.0)],
        "jaccard_one": [float(v) for v in jaccard_score(
            np.array([0, 1]), np.array([0, 1]), average=None, labels=[0, 1, 2], zero_division=1.0)],
        "jaccard_macro_zero": float(jaccard_score(
            np.array([0, 1]), np.array([0, 1]), average="macro", labels=[0, 1, 2], zero_division=0.0)),
    }

    return {
        "metadata": {
            "algorithm": "LabelLosses",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.hamming_loss",
                "sklearn.metrics.zero_one_loss",
                "sklearn.metrics.jaccard_score",
            ],
            "count": len(cases),
        },
        "cases": cases,
        "multilabel": multilabel,
        "undefined": undefined,
    }


def generate_multilabel_confusion() -> dict:
    """multilabel_confusion_matrix: one 2x2 per label, or per sample (#211)."""
    import numpy as np
    from sklearn.metrics import multilabel_confusion_matrix

    def stack(matrices) -> list[list[float]]:
        """Each 2x2 flattened as [tn, fp, fn, tp], which is its reading order."""
        return [[float(v) for v in m.reshape(-1)] for m in matrices]

    multi_true = [[1, 0, 1], [0, 1, 1]]
    multi_pred = [[1, 0, 0], [1, 1, 1]]
    mt = np.array(multi_true)
    mp = np.array(multi_pred)
    weight = np.array([1.0, 3.0])

    single_true = [0, 1, 2, 1]
    single_pred = [0, 2, 2, 1]
    st = np.array(single_true)
    sp = np.array(single_pred)

    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        return {
            "metadata": {
                "algorithm": "MultilabelConfusion",
                "library": "scikit-learn",
                "library_version": version("scikit-learn"),
                "reference_calls": ["sklearn.metrics.multilabel_confusion_matrix"],
                "count": 5,
            },
            "multilabel": {
                "y_true": [v for row in multi_true for v in row],
                "y_pred": [v for row in multi_pred for v in row],
                "label_count": 3,
                "sample_weight": [1.0, 3.0],
                "per_label": stack(multilabel_confusion_matrix(mt, mp)),
                "samplewise": stack(multilabel_confusion_matrix(mt, mp, samplewise=True)),
                "per_label_weighted": stack(
                    multilabel_confusion_matrix(mt, mp, sample_weight=weight)),
                "samplewise_weighted": stack(
                    multilabel_confusion_matrix(mt, mp, samplewise=True, sample_weight=weight)),
            },
            "multiclass": {
                "y_true": single_true,
                "y_pred": single_pred,
                "labels": [0, 1],
                "sample_weight": [1.0, 2.0, 3.0, 4.0],
                "per_class": stack(multilabel_confusion_matrix(st, sp)),
                "selected_labels": stack(multilabel_confusion_matrix(st, sp, labels=[0, 1])),
                "per_class_weighted": stack(multilabel_confusion_matrix(
                    st, sp, sample_weight=np.array([1.0, 2.0, 3.0, 4.0]))),
            },
        }


def _likelihood_ratio_fixtures() -> list[dict]:
    """Binary cases, chosen so each of the four undefined shapes is reached."""
    return [
        {"name": WORKED_CASE, "true": [0, 1, 1, 0, 1, 0], "pred": [0, 1, 0, 0, 1, 1],
         "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [0, 1, 1, 0, 1, 0], "pred": [0, 1, 0, 0, 1, 1],
         "weight": [1.0, 2.0, 3.0, 4.0, 5.0, 6.0]},
        # Nothing true-negative: the negative ratio divides by a specificity of zero.
        {"name": "no true negative", "true": [0, 1, 1], "pred": [1, 1, 1], "weight": None},
        # Nothing false-positive: the positive ratio divides by zero instead.
        {"name": "a perfect prediction", "true": [0, 1], "pred": [0, 1], "weight": None},
        {"name": "nothing predicted positive", "true": [0, 1], "pred": [0, 0], "weight": None},
        # A class missing from the truth leaves neither ratio a value.
        {"name": "no positive sample", "true": [0, 0], "pred": [0, 1], "weight": None},
        {"name": "no negative sample", "true": [1, 1], "pred": [0, 1], "weight": None},
        {"name": "an ordinary imbalanced case", "true": [0, 0, 0, 0, 1, 1],
         "pred": [0, 0, 0, 1, 1, 0], "weight": None},
    ]


def generate_likelihood_ratios() -> dict:
    """class_likelihood_ratios: a pair, and four ways for one of them to have no value (#211)."""
    import numpy as np
    from sklearn.metrics import class_likelihood_ratios

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _likelihood_ratio_fixtures():
            true = np.array(fixture["true"])
            pred = np.array(fixture["pred"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}

            default = class_likelihood_ratios(true, pred, **kw)
            # replace_undefined_by takes a scalar or a per-ratio mapping; both are
            # two explicit parameters on the C# side, so both are pinned here.
            scalar = class_likelihood_ratios(true, pred, replace_undefined_by=1.0, **kw)
            mapping = class_likelihood_ratios(
                true, pred, replace_undefined_by={"LR+": 10.0, "LR-": 0.5}, **kw)

            # JSON has no nan, so an undefined ratio travels as null. Some stay
            # undefined even under replace_undefined_by, which is itself a measurement.
            def maybe(value):
                return None if np.isnan(value) else float(value)

            cases.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_pred": fixture["pred"],
                "sample_weight": fixture["weight"],
                "positive": maybe(default[0]),
                "negative": maybe(default[1]),
                "positive_replaced_by_one": maybe(scalar[0]),
                "negative_replaced_by_one": maybe(scalar[1]),
                "positive_replaced_apart": maybe(mapping[0]),
                "negative_replaced_apart": maybe(mapping[1]),
            })

    return {
        "metadata": {
            "algorithm": "LikelihoodRatios",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.metrics.class_likelihood_ratios"],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_hinge_loss() -> dict:
    """hinge_loss: the one metric that reads a decision function (#211)."""
    import numpy as np
    from sklearn.metrics import hinge_loss

    binary = [
        {"name": WORKED_CASE, "true": [-1, 1, 1, -1],
         "decision": [-0.5, 1.2, 0.3, 0.8], "weight": None},
        {"name": WORKED_CASE_WEIGHTED, "true": [-1, 1, 1, -1],
         "decision": [-0.5, 1.2, 0.3, 0.8], "weight": [1.0, 2.0, 3.0, 4.0]},
        # The same decisions under 0/1 labels: the mapping is to the sign, so the
        # number does not move.
        {"name": "labels zero and one", "true": [0, 1, 1, 0],
         "decision": [-0.5, 1.2, 0.3, 0.8], "weight": None},
        # Every sample right by a margin of at least one, which is where the loss
        # stops charging anything at all.
        {"name": "every margin past one", "true": [-1, 1],
         "decision": [-2.0, 2.0], "weight": None},
        {"name": "a margin exactly one", "true": [-1, 1],
         "decision": [-1.0, 1.0], "weight": None},
        {"name": "every sample on the wrong side", "true": [-1, 1],
         "decision": [1.5, -1.5], "weight": None},
    ]

    multiclass_true = [0, 1, 2, 1]
    multiclass_decision = [[1.2, 0.3, -0.5], [0.1, 0.9, 0.2], [0.4, 0.2, 0.7], [0.3, 0.1, 0.6]]
    mt = np.array(multiclass_true)
    md = np.array(multiclass_decision)

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in binary:
            true = np.array(fixture["true"])
            decision = np.array(fixture["decision"])
            kw = {} if fixture["weight"] is None else {
                "sample_weight": np.array(fixture["weight"])}
            cases.append({
                "name": fixture["name"],
                "y_true": fixture["true"],
                "pred_decision": fixture["decision"],
                "sample_weight": fixture["weight"],
                "hinge": float(hinge_loss(true, decision, **kw)),
            })

        multiclass = {
            "y_true": multiclass_true,
            "pred_decision": [v for row in multiclass_decision for v in row],
            "class_count": 3,
            "sample_weight": [1.0, 2.0, 3.0, 4.0],
            "hinge": float(hinge_loss(mt, md)),
            "hinge_weighted": float(hinge_loss(mt, md, sample_weight=np.array([1.0, 2.0, 3.0, 4.0]))),
        }

    return {
        "metadata": {
            "algorithm": "HingeLoss",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.metrics.hinge_loss"],
            "count": len(cases),
        },
        "cases": cases,
        "multiclass": multiclass,
    }


def _top_k_fixtures() -> list[dict]:
    """Multiclass score matrices, where k is the question rather than ties.

    Every weight vector puts a value other than 1 on a sample that HITS. A
    vector weighting only the misses cannot separate "sums the weights" from
    "counts the samples": both give the unweighted count, which is what a first
    measurement of normalize=False reported before the fixture was fixed (#216).
    """
    return [
        {"name": "three classes", "true": [0, 1, 2, 2],
         "score": [[0.7, 0.2, 0.1], [0.3, 0.5, 0.2], [0.2, 0.3, 0.5], [0.5, 0.3, 0.2]],
         "weight": [5.0, 1.0, 1.0, 1.0]},
        {"name": "every prediction wrong at k=1", "true": [1, 2, 0],
         "score": [[0.6, 0.3, 0.1], [0.5, 0.4, 0.1], [0.2, 0.7, 0.1]],
         "weight": [2.0, 3.0, 4.0]},
        # scikit-learn infers the label set from y_true and refuses a wider score row,
        # so every class appears here; our own surface takes the count as a parameter.
        {"name": "ties in the score row", "true": [0, 1, 2],
         "score": [[0.5, 0.5, 0.0], [0.4, 0.4, 0.2], [0.1, 0.2, 0.7]],
         "weight": [1.0, 4.0, 1.0]},
        # A negative weight is accepted and takes the fraction out of [0, 1],
        # which the reference does too rather than refusing it.
        {"name": "a negative weight", "true": [0, 1, 2, 2],
         "score": [[0.7, 0.2, 0.1], [0.3, 0.5, 0.2], [0.2, 0.3, 0.5], [0.5, 0.3, 0.2]],
         "weight": [-1.0, 1.0, 1.0, 1.0]},
    ]


def _ranking_weighted_fixtures() -> list[dict]:
    """Multi-row queries, because a weight over one row cancels.

    Every fixture in _ranking_fixtures is a single query, so sample_weight
    there multiplies the numerator and the denominator alike and no vector can
    change a value. Weights need at least two rows that score differently,
    which is what these are.
    """
    return [
        # The corpus' perfect and reversed rows together: the pair whose mean the
        # weights move furthest, and whose unweighted mean is already asserted.
        {"name": "perfect and reversed", "true": [[3.0, 2.0, 1.0, 0.0], [3.0, 2.0, 1.0, 0.0]],
         "score": [[0.9, 0.5, 0.4, 0.1], [0.1, 0.4, 0.5, 0.9]], "weight": [1.0, 3.0]},
        {"name": "perfect and reversed, weight on the good row",
         "true": [[3.0, 2.0, 1.0, 0.0], [3.0, 2.0, 1.0, 0.0]],
         "score": [[0.9, 0.5, 0.4, 0.1], [0.1, 0.4, 0.5, 0.9]], "weight": [3.0, 1.0]},
        # Equal weights must give the unweighted mean back, which is the check
        # that the weighting is a mean rather than a sum.
        {"name": "equal weights are the plain mean",
         "true": [[3.0, 2.0, 1.0, 0.0], [3.0, 2.0, 1.0, 0.0]],
         "score": [[0.9, 0.5, 0.4, 0.1], [0.1, 0.4, 0.5, 0.9]], "weight": [2.0, 2.0]},
        # A row whose relevance is all zero scores 0 for ndcg and 0 for dcg, and
        # still carries its weight into the denominator.
        {"name": "a nothing-relevant row carries its weight",
         "true": [[3.0, 2.0, 1.0, 0.0], [0.0, 0.0, 0.0, 0.0]],
         "score": [[0.9, 0.5, 0.4, 0.1], [0.9, 0.5, 0.4, 0.1]], "weight": [1.0, 4.0]},
        # Accepted on both sides, and it takes the result outside the range the
        # page promises -- recorded rather than smoothed.
        {"name": "a negative weight", "true": [[3.0, 2.0, 1.0, 0.0], [3.0, 2.0, 1.0, 0.0]],
         "score": [[0.9, 0.5, 0.4, 0.1], [0.1, 0.4, 0.5, 0.9]], "weight": [-1.0, 2.0]},
        {"name": "three rows", "true": [[3.0, 2.0, 1.0], [1.0, 2.0, 3.0], [0.0, 1.0, 0.0]],
         "score": [[0.9, 0.5, 0.1], [0.9, 0.5, 0.1], [0.5, 0.5, 0.5]], "weight": [1.0, 2.0, 5.0]},
    ]


def generate_ranking_weighted() -> dict:
    """dcg_score and ndcg_score with a sample_weight, which needs several rows."""
    import math

    import numpy as np
    from sklearn.metrics import dcg_score, ndcg_score

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _ranking_weighted_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            weight = np.array(fixture["weight"])
            cases.append({
                "name": fixture["name"],
                "y_true": [v for row in fixture["true"] for v in row],
                "y_score": [v for row in fixture["score"] for v in row],
                "label_count": true.shape[1],
                "sample_weight": fixture["weight"],
                "dcg": float(dcg_score(true, score)),
                "dcg_weighted": float(dcg_score(true, score, sample_weight=weight)),
                "dcg_weighted_log_e": float(
                    dcg_score(true, score, log_base=math.e, sample_weight=weight)),
                "dcg_weighted_at_2": float(dcg_score(true, score, k=2, sample_weight=weight)),
                "ndcg": float(ndcg_score(true, score)),
                "ndcg_weighted": float(ndcg_score(true, score, sample_weight=weight)),
                "ndcg_weighted_at_2": float(ndcg_score(true, score, k=2, sample_weight=weight)),
                "ndcg_weighted_ignore_ties": float(
                    ndcg_score(true, score, ignore_ties=True, sample_weight=weight)),
            })

    return {
        "metadata": {
            "algorithm": "RankingWeighted",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.dcg_score(sample_weight=...)",
                "sklearn.metrics.ndcg_score(sample_weight=...)",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_top_k_accuracy() -> dict:
    import numpy as np
    from sklearn.metrics import top_k_accuracy_score

    cases = []
    with warnings.catch_warnings():
        warnings.simplefilter("ignore")
        for fixture in _top_k_fixtures():
            true = np.array(fixture["true"])
            score = np.array(fixture["score"])
            classes = score.shape[1]
            entry = {
                "name": fixture["name"],
                "y_true": fixture["true"],
                "y_score": [value for row in fixture["score"] for value in row],
                "class_count": classes,
            }
            weight = np.array(fixture["weight"])
            entry["sample_weight"] = fixture["weight"]
            for k in (1, 2, classes):
                entry[f"top_{k}"] = float(top_k_accuracy_score(true, score, k=k))
                entry[f"top_{k}_count"] = float(
                    top_k_accuracy_score(true, score, k=k, normalize=False))
                entry[f"top_{k}_weighted"] = float(
                    top_k_accuracy_score(true, score, k=k, sample_weight=weight))
                # normalize=False sums the weights of the hits rather than counting
                # them, and never divides -- so it alone survives a zero-sum vector.
                entry[f"top_{k}_weighted_count"] = float(
                    top_k_accuracy_score(true, score, k=k, normalize=False,
                                         sample_weight=weight))
            cases.append(entry)

    return {
        "metadata": {
            "algorithm": "TopKAccuracy",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.top_k_accuracy_score",
                "sklearn.metrics.top_k_accuracy_score(normalize=False)",
                "sklearn.metrics.top_k_accuracy_score(sample_weight=...)",
                "sklearn.metrics.top_k_accuracy_score(normalize=False, sample_weight=...)",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Silhouette (issue #172, second lot) --------------------------------------


def _silhouette_fixtures() -> list[dict]:
    """Feature matrices whose clustering is the thing under test.

    Two well-separated blobs, two that overlap, a cluster holding one sample, and
    a one-dimensional case -- the shapes whose per-sample values differ in kind
    rather than in scale.
    """
    return [
        {
            "name": "two separated blobs",
            "features": [[0.0, 0.0], [0.1, 0.1], [5.0, 5.0], [5.1, 5.2], [5.0, 4.9]],
            "labels": [0, 0, 1, 1, 1],
        },
        {
            "name": "two overlapping blobs",
            "features": [[0.0, 0.0], [1.0, 0.5], [1.2, 0.4], [2.0, 1.0], [0.4, 1.4]],
            "labels": [0, 0, 1, 1, 0],
        },
        {
            "name": "a singleton cluster",
            "features": [[0.0, 0.0], [0.1, 0.1], [5.0, 5.0], [5.1, 5.2], [9.0, 9.0]],
            "labels": [0, 0, 1, 1, 2],
        },
        {
            "name": "one dimension",
            "features": [[0.0], [1.0], [10.0], [11.0], [12.0]],
            "labels": [0, 0, 1, 1, 1],
        },
        {
            "name": "three clusters",
            "features": [[0.0, 0.0], [0.2, 0.1], [4.0, 4.0], [4.2, 3.9], [8.0, 0.0], [8.1, 0.2]],
            "labels": [0, 0, 1, 1, 2, 2],
        },
        {
            "name": "coincident samples",
            "features": [[1.0, 1.0], [1.0, 1.0], [1.0, 1.0], [1.0, 1.0]],
            "labels": [0, 0, 1, 1],
        },
        {
            "name": "a misplaced sample",
            "features": [[0.0, 0.0], [0.2, 0.1], [4.0, 4.0], [4.2, 3.9], [0.1, 0.3]],
            "labels": [0, 0, 1, 1, 1],
        },
    ]


def generate_silhouette() -> dict:
    import numpy as np
    from sklearn.metrics import pairwise_distances, silhouette_samples, silhouette_score

    cases = []
    for fixture in _silhouette_fixtures():
        features = np.array(fixture["features"], dtype=float)
        labels = fixture["labels"]
        distances = pairwise_distances(features, metric="euclidean")
        cases.append({
            "name": fixture["name"],
            "features": [value for row in fixture["features"] for value in row],
            FEATURE_COUNT: features.shape[1],
            "labels": labels,
            "distances": [float(value) for row in distances for value in row],
            # random_state is inert without sample_size, and named anyway: S6709 asks
            # every caller of a seeded API to say which seed, and silence is not an answer.
            "score": float(silhouette_score(features, labels, random_state=0)),
            "score_precomputed": float(
                silhouette_score(distances, labels, metric="precomputed", random_state=0)),
            "per_sample": [float(value) for value in silhouette_samples(features, labels)],
        })

    return {
        "metadata": {
            "algorithm": "Silhouette",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.silhouette_score",
                "sklearn.metrics.silhouette_score(metric='precomputed')",
                "sklearn.metrics.silhouette_samples",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- ROC-AUC (issue #61) ------------------------------------------------------


def _softmax(row: list[float]) -> list[float]:
    top = max(row)
    exps = [math.exp(v - top) for v in row]
    total = sum(exps)
    return [v / total for v in exps]


def _roc_fixtures() -> list[dict]:
    rng = SeededRandom(METRIC_SEED + 1)
    fixtures: list[dict] = []

    def weights(n: int) -> list[float]:
        return [round(rng.uniform(0.1, 3.0), 3) for _ in range(n)]

    def informative(truth: list[int]) -> list[float]:
        # Overlapping but separable: an AUC around 0.8 rather than 0.5 or 1.0.
        return [round(rng.random() * 0.6 + 0.4 * t, 12) for t in truth]

    balanced = [rng.randint(0, 1) for _ in range(300)]
    fixtures.append({"name": "binary_balanced", "kind": "binary", "y_true": balanced,
                     "scores": informative(balanced), "class_count": 2,
                     "sample_weight": weights(len(balanced))})

    imbalanced = [0] * 280 + [1] * 20
    fixtures.append({"name": "binary_imbalanced", "kind": "binary", "y_true": imbalanced,
                     "scores": informative(imbalanced), "class_count": 2,
                     "sample_weight": weights(len(imbalanced))})

    tied = [rng.randint(0, 1) for _ in range(200)]
    fixtures.append({"name": "binary_heavy_ties", "kind": "binary", "y_true": tied,
                     # One decimal: many samples share a score, which is where a
                     # rank-based shortcut and a real ROC curve part company.
                     "scores": [round(v, 1) for v in informative(tied)], "class_count": 2,
                     "sample_weight": weights(len(tied))})

    for k, size in ((3, 240), (5, 400)):
        truth = [rng.randint(0, k - 1) for _ in range(size)]
        rows = []
        for t in truth:
            logits = [rng.gauss(0.0, 1.0) for _ in range(k)]
            logits[t] += 1.5
            rows.append([round(v, 12) for v in _softmax(logits)])
        fixtures.append({"name": f"multiclass_{k}", "kind": "multiclass", "y_true": truth,
                         "scores": rows, "class_count": k,
                         "sample_weight": weights(size)})

    return fixtures


def _roc_case(fx: dict, weighted: bool) -> dict:
    sw = fx["sample_weight"] if weighted else None
    y_true = fx["y_true"]
    case = {
        "fixture": fx["name"],
        "kind": fx["kind"],
        "weighted": weighted,
        "y_true": y_true,
        "scores": fx["scores"],
        "class_count": fx["class_count"],
        "sample_weight": sw,
        "values": {},
    }
    if fx["kind"] == "binary":
        case["values"]["binary"] = stable(
            skm.roc_auc_score(y_true, fx["scores"], sample_weight=sw))
        return case

    scores = np.array(fx["scores"], dtype=float)
    classes = list(range(fx["class_count"]))
    for strategy in ("ovr", "ovo"):
        # scikit-learn refuses sample_weight for one-vs-one, and so do we.
        if strategy == "ovo" and weighted:
            continue
        for average in ("macro", "weighted"):
            case["values"][f"{strategy}|{average}"] = stable(skm.roc_auc_score(
                y_true, scores, multi_class=strategy, average=average,
                labels=classes, sample_weight=sw))
    return case


def generate_roc_auc() -> dict:
    cases = [
        _roc_case(fx, weighted)
        for fx in _roc_fixtures()
        for weighted in (False, True)
    ]
    return {
        "metadata": {
            "algorithm": "RocAuc",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": ["sklearn.metrics.roc_auc_score"],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- BPE and byte-level BPE tokenizers (issue #59) ---------------------------

GPT2_VOCAB = "gpt2_vocab.json"
GPT2_MERGES = "gpt2_merges.txt"


def _gpt2_tokenizer(pattern: str | None = None, add_prefix_space: bool = False):
    """GPT-2's byte-level BPE, optionally with another model's split pattern.

    `pattern=None` is stock GPT-2: `ByteLevel` does its own splitting. A pattern
    reproduces the Llama-3 / Qwen2 shape, where a `Split` runs first and
    `ByteLevel` is reduced to the byte mapping.

    `add_prefix_space` is HuggingFace's own `ByteLevel` default (True), left off
    here because every corpus that predates issue #59's final review was
    generated without it; `generate_bpe_added_tokens` is the one that turns it on.
    """
    from tokenizers import Regex, Tokenizer  # noqa: PLC0415
    from tokenizers.decoders import ByteLevel as ByteLevelDecoder  # noqa: PLC0415
    from tokenizers.models import BPE  # noqa: PLC0415
    from tokenizers.pre_tokenizers import ByteLevel, Sequence, Split  # noqa: PLC0415

    tokenizer = Tokenizer(BPE.from_file(
        str(ORACLE_DIR / GPT2_VOCAB), str(ORACLE_DIR / GPT2_MERGES)))
    if pattern is None:
        tokenizer.pre_tokenizer = ByteLevel(add_prefix_space=add_prefix_space)
    else:
        tokenizer.pre_tokenizer = Sequence([
            Split(Regex(pattern), behavior="isolated"),
            ByteLevel(add_prefix_space=add_prefix_space, use_regex=False),
        ])
    tokenizer.decoder = ByteLevelDecoder()
    return tokenizer


def generate_bytelevel_bpe() -> dict:
    from tokenizers.pre_tokenizers import ByteLevel  # noqa: PLC0415

    tokenizer = _gpt2_tokenizer()
    # The published bytes_to_unicode construction: the three printable ranges map
    # to themselves, and the 68 bytes left over take 256, 257, ... in byte order.
    printable = (list(range(0x21, 0x7F)) + list(range(0xA1, 0xAD)) + list(range(0xAE, 0x100)))
    table = [None] * 256
    for byte in printable:
        table[byte] = chr(byte)
    spare = 0
    for byte in range(256):
        if table[byte] is None:
            table[byte] = chr(256 + spare)
            spare += 1
    # Coerced to sets: ByteLevel.alphabet() returns a list here, so a bare
    # set(table) == ByteLevel.alphabet() is False regardless of contents.
    assert set(table) == set(ByteLevel.alphabet()), "derived alphabet disagrees with tokenizers"

    # ignore_merges: emits a piece that is itself a vocab entry whole, rather
    # than merged up to. Set on the deserialized JSON -- the flag lives on the model.
    import json as _json  # noqa: PLC0415
    from tokenizers import Tokenizer as _Tokenizer  # noqa: PLC0415

    spec = _json.loads(tokenizer.to_str())
    spec["model"]["ignore_merges"] = True
    ignoring = _Tokenizer.from_str(_json.dumps(spec))

    cases = []
    for i, text in enumerate(BPE_TEXTS):
        enc = tokenizer.encode(text)
        enc_ignoring = ignoring.encode(text)
        cases.append({
            "id": i,
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
            "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            "decoded_skip_specials": tokenizer.decode(enc.ids, skip_special_tokens=True),
            "tokens_ignore_merges": enc_ignoring.tokens,
            "ids_ignore_merges": enc_ignoring.ids,
        })
    return {
        "metadata": {
            "algorithm": "ByteLevelBPE",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "gpt2 (vendored by tools/fetch_gpt2_bpe.py)",
            "alphabet": table,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_bpe() -> dict:
    """Classic character-level BPE over the small self-trained model."""
    from tokenizers import Tokenizer  # noqa: PLC0415

    tokenizer = Tokenizer.from_file(str(ORACLE_DIR / "tiny_bpe.json"))
    cases = []
    for i, text in enumerate(BPE_TEXTS + WHITESPACE_PRE_TOKENIZER_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({"id": i, "text": text, "tokens": enc.tokens, "ids": enc.ids})
    return {
        "metadata": {
            "algorithm": "BPE",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "tiny_bpe.json (self-trained, end_of_word_suffix </w>)",
            "count": len(cases),
        },
        "cases": cases,
    }


# GPT-2's own vocabulary cannot exercise ignore_merges; see generate_orphan_bpe's
# docstring for why this corpus exists instead.
ORPHAN_BPE_TEXTS = [
    "abc",       # the orphan itself: ['ab', 'c'] normally, ['abc'] with the flag
    "x abc y",   # the orphan as one piece among ordinary ones
    "x y",       # no piece here is the orphan
    "ab c",      # "ab" is a legitimately reachable entry, not the orphan
    "",
]


def generate_orphan_bpe() -> dict:
    """Classic BPE over a model with one vocabulary entry the merge table cannot reach.

    The vendored GPT-2 model cannot exercise ignore_merges: checked over all
    50 257 of its vocabulary entries, none diverges, because a natively-trained
    merge table always retraces to its own entries (see the ignore_merges
    task's amended plan for the argument in full). The flag only rescues
    *orphaned* entries -- present in a model's vocabulary but unreachable by
    replaying its merges -- which is what a tiktoken-to-tokenizer.json
    conversion produces and what training never does. orphan_bpe_model.json
    (tools/build_tiny_models.py) holds exactly one such entry, on purpose, so
    this corpus is the only one in the suite that can prove the flag does
    anything.
    """
    import json as _json  # noqa: PLC0415
    from tokenizers import Tokenizer  # noqa: PLC0415

    tokenizer = Tokenizer.from_file(str(ORACLE_DIR / "orphan_bpe_model.json"))
    spec = _json.loads(tokenizer.to_str())
    spec["model"]["ignore_merges"] = True
    ignoring = Tokenizer.from_str(_json.dumps(spec))

    cases = []
    for i, text in enumerate(ORPHAN_BPE_TEXTS):
        enc = tokenizer.encode(text)
        enc_ignoring = ignoring.encode(text)
        cases.append({
            "id": i,
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
            "tokens_ignore_merges": enc_ignoring.tokens,
            "ids_ignore_merges": enc_ignoring.ids,
        })
    return {
        "metadata": {
            "algorithm": "BPE",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "orphan_bpe_model.json (hand-constructed, one orphaned entry)",
            "count": len(cases),
        },
        "cases": cases,
    }


# long-comment: kept at length because the material imposes it -- two mirror URLs and
# what they agree on are the evidence that stands in for a gated original nobody can open
# Transcribe each of these from the model's own tokenizer.json rather than from
# memory: they differ from GPT-2 in newline handling and in the case-insensitive
# contraction group, and from each other only in a quantifier on \p{N}.
#
# Provenance:
#   gpt2   - tests/oracles/gpt2_vocab.json / gpt2_merges.txt (vendored by
#            tools/fetch_gpt2_bpe.py); ByteLevel does its own splitting, so
#            this is the pattern GPT-2's own pre-tokenizer is equivalent to.
#   qwen2  - https://huggingface.co/Qwen/Qwen2-0.5B/resolve/main/tokenizer.json
#            (Apache-2.0, ungated), `pre_tokenizer.pretokenizers[0].pattern.Regex`.
#   llama3 - meta-llama/Meta-Llama-3-8B is gated and returns HTTP 401 without
#            an authorized token, so this was read from two independent
#            mirrors instead and cross-checked byte-for-byte:
#              https://huggingface.co/NousResearch/Meta-Llama-3-8B/resolve/main/tokenizer.json
#              https://huggingface.co/unsloth/llama-3-8b/resolve/main/tokenizer.json
#            Both carry the same `pre_tokenizer.pretokenizers[0].pattern.Regex`,
#            the same `model.ignore_merges = true`, and the same 128 000-entry
#            vocabulary, which is what stands in for reading the gated
#            original. It differs from qwen2 in exactly one place: `\p{N}{1,3}`
#            where qwen2 has `\p{N}`.
BPE_PATTERNS = {
    "gpt2": r"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
    "llama3": r"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}{1,3}| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+",
    "qwen2": r"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+",
}


# Astral-plane letters and digits, for issue #341. Kept apart from BPE_TEXTS:
# that list also feeds generators this fixture has no reason to touch.
BPE_PRETOKENIZE_ASTRAL_TEXTS = [
    "\U0001D400\U0001D401\U0001D402 \U0001D7CF\U0001D7D0\U0001D7D1 abc123",
    "\U0001D504lphabet mixes \U0001D7DEigits with ascii",
]


def generate_bpe_pretokenize() -> dict:
    """Prove the split, not the vocabulary.

    The Llama-3 and Qwen2 rows of the parity table are claimed at the split
    level only (ADR 0017). Running their patterns over GPT-2's vocabulary is
    what proves the C# regex behaves as HuggingFace's does, without vendoring a
    second and third 150 000-entry vocabulary to prove a merge loop the GPT-2
    corpus already proves.
    """
    cases = []
    case_id = 0
    for name, pattern in BPE_PATTERNS.items():
        tokenizer = _gpt2_tokenizer(None if name == "gpt2" else pattern)
        for text in BPE_TEXTS + BPE_PRETOKENIZE_ASTRAL_TEXTS:
            pieces = [piece for piece, _ in tokenizer.pre_tokenizer.pre_tokenize_str(text)]
            cases.append({"id": case_id, "pattern": name, "text": text, "pieces": pieces})
            case_id += 1
    return {
        "metadata": {
            "algorithm": "BPE pre-tokenization",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "patterns": BPE_PATTERNS,
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_bpe_tokenizer_json() -> dict:
    """The tokenizer.json shapes TokenizerJsonLoader.LoadBpe must read.

    Each case carries the file itself, so the C# side parses the exact bytes
    HuggingFace was handed rather than a second fixture that could drift.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    text = "Hello, world! déjà 東京 👋"
    cases = []
    for i, (name, tokenizer) in enumerate([
        ("bytelevel", _gpt2_tokenizer()),
        ("split_sequence", _gpt2_tokenizer(BPE_PATTERNS["qwen2"])),
        ("classic", Tokenizer.from_file(str(ORACLE_DIR / "tiny_bpe.json"))),
    ]):
        enc = tokenizer.encode(text)
        cases.append({
            "id": i,
            "name": name,
            "tokenizer_json": tokenizer.to_str(),
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
        })
    return {
        "metadata": {
            "algorithm": "BPE tokenizer.json",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


# Combining sequence, singleton (U+212B -> U+00C5) and two compatibility characters
# only the K forms touch -- as \u escapes, so none arrives already normalized.
BPE_NORMALIZER_TEXTS = [
    "école",            # e + COMBINING ACUTE
    "école",             # the precomposed form of the same word
    "Ångstrom unit",     # ANGSTROM SIGN
    "ﬁve o￦clock",  # the fi ligature, and a fullwidth macron
    "① ② café",
    HELLO_WORLD,              # unchanged by every form: the control
]

# Code points chosen to expose a disagreement between .NET's Unicode tables
# and Rust's crate, if the four forms have one -- see the spec's D5.
UNICODE_FORM_PROBES = BPE_NORMALIZER_TEXTS + [
    "ẛ̣",   # LATIN SMALL LETTER LONG S WITH DOT ABOVE + DOT BELOW
    "İ",         # LATIN CAPITAL LETTER I WITH DOT ABOVE
    "Ω",         # OHM SIGN
    "̈́",         # COMBINING GREEK DIALYTIKA TONOS, a singleton decomposition
    "가한",   # Hangul syllables, algorithmic composition
    "豈",         # a CJK compatibility ideograph
    "ᾂ",         # a Greek letter with three stacked marks
    "ﷺ",         # ARABIC LIGATURE SALLALLAHOU..., expands to 18 characters
]


def generate_unicode_forms() -> dict:
    """What tokenizers' four normalization forms produce, character for character.

    The C# side asserts String.Normalize gives the same answer. Nothing in this
    corpus involves BPE: it isolates the one question the tokenizer corpus cannot
    answer, which is whether the two runtimes' Unicode tables agree at all.
    """
    from tokenizers import normalizers  # noqa: PLC0415

    forms = [("NFC", normalizers.NFC()), ("NFKC", normalizers.NFKC()),
             ("NFD", normalizers.NFD()), ("NFKD", normalizers.NFKD())]
    cases = []
    for text in UNICODE_FORM_PROBES:
        for name, normalizer in forms:
            cases.append({
                "id": len(cases),
                "form": name,
                "text": text,
                "normalized": normalizer.normalize_str(text),
            })
    return {
        "metadata": {
            "algorithm": "Unicode normalization forms",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


def _small_bytelevel_bpe_tokenizer(add_prefix_space: bool = False):
    """A byte-level BPE with full coverage and no merges.

    Every one of the 256 alphabet characters is its own token, so any input
    encodes with no unknown id -- all this corpus needs, since its subject is
    the normalizer, not the size of the vocabulary the merges reach. Reuses the
    bytes_to_unicode-derived alphabet generate_bytelevel_bpe already proves
    against ByteLevel.alphabet(), rather than deriving it a second time.
    """
    from tokenizers import Tokenizer, decoders, models, pre_tokenizers  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    tokenizer = Tokenizer(models.BPE(vocab, []))
    tokenizer.pre_tokenizer = pre_tokenizers.ByteLevel(add_prefix_space=add_prefix_space)
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def generate_bpe_normalizer() -> dict:
    """A BPE pipeline with a normalizer, which LoadBpe refused wholesale.

    Ten pipelines. Seven cover D1-D4 of the spec: one per form, a Sequence of
    two, an empty Sequence -- the deepseek shape, which does nothing and was
    refused for nothing -- and a normalizer beside both halves of the added
    token table, the gpt-neox shape (23 of its 25 entries are normalized:
    true). Three more, added by the branch review of #121: two where a raw
    and a normalized added token compete for the same span (finding 1), and
    one measuring add_prefix_space against a normalizer for the first time
    (finding 2) -- see the comments at each for what they pin down.

    Only "nfc" -- the form four of the five surveyed models actually declare --
    runs on real GPT-2, so the corpus still proves the case that exists in the
    wild against a real 50 257-entry vocabulary. Every other pipeline runs on
    _small_bytelevel_bpe_tokenizer(): each case already carries its own whole
    tokenizer.json (so the C# side parses the exact bytes HuggingFace was
    handed), and ten copies of GPT-2's 1.8 MB model would have made this
    corpus over 100 MB for a question that has nothing to do with vocabulary
    size.
    """
    from tokenizers import AddedToken, normalizers  # noqa: PLC0415

    pipelines = [
        ("nfc", normalizers.NFC(), False, True),
        ("nfkc", normalizers.NFKC(), False, False),
        ("nfd", normalizers.NFD(), False, False),
        ("nfkd", normalizers.NFKD(), False, False),
        ("sequence", normalizers.Sequence([normalizers.NFD(), normalizers.NFC()]), False, False),
        ("empty_sequence", normalizers.Sequence([]), False, False),
        ("added_tokens", normalizers.NFC(), True, False),
    ]

    cases = []
    for i, (name, normalizer, with_added, real_gpt2) in enumerate(pipelines):
        tokenizer = _gpt2_tokenizer() if real_gpt2 else _small_bytelevel_bpe_tokenizer()
        tokenizer.normalizer = normalizer
        if with_added:
            # One of each half. The normalized entry is written decomposed, so it
            # can only match once its own content has been normalized too.
            tokenizer.add_tokens([AddedToken("café", normalized=True)])
            tokenizer.add_special_tokens([AddedToken(END_OF_TEXT, special=True, normalized=False)])
        texts = BPE_NORMALIZER_TEXTS + (["a café<|endoftext|>b", "café tail"] if with_added else [])
        text_cases = []
        for text in texts:
            enc = tokenizer.encode(text)
            text_cases.append({
                "text": text,
                "tokens": enc.tokens,
                "ids": enc.ids,
                "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            })
        cases.append({
            "id": i,
            "name": name,
            "tokenizer_json": tokenizer.to_str(),
            "texts": text_cases,
        })

    # Branch review of #121, finding 1: a raw entry now beats an earlier or
    # longer normalized one for the same span, HuggingFace's own precedence -- see spec D2.
    precedence_pipelines = [
        # "ab" (normalized) starts earlier but loses: the raw pass claims "b"
        # first, leaving no gap that still contains "ab".
        ("precedence_raw_beats_normalized", [("ab", True), ("b", False)], ["xaby"]),
        # "abc" (normalized, earlier and longer) still loses: the raw pass
        # claims "cy" first, removing the 'c' it needed.
        ("precedence_raw_beats_earlier_longer_normalized", [("abc", True), ("cy", False)], ["abcy"]),
    ]
    for name, tokens, texts in precedence_pipelines:
        tokenizer = _small_bytelevel_bpe_tokenizer()
        for content, normalized in tokens:
            tokenizer.add_tokens([AddedToken(content, normalized=normalized)])
        text_cases = []
        for text in texts:
            enc = tokenizer.encode(text)
            text_cases.append({
                "text": text,
                "tokens": enc.tokens,
                "ids": enc.ids,
                "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            })
        cases.append({
            "id": len(cases),
            "name": name,
            "tokenizer_json": tokenizer.to_str(),
            "texts": text_cases,
        })

    # Branch review of #121, finding 2: normalization runs before
    # add_prefix_space is decided -- U+3000 only becomes a literal space post-NFKC; see spec D2.
    prefix_space_tokenizer = _small_bytelevel_bpe_tokenizer(add_prefix_space=True)
    prefix_space_tokenizer.normalizer = normalizers.NFKC()
    prefix_space_texts = [
        "　café",  # IDEOGRAPHIC SPACE + café: a leading space only after NFKC
        " café",       # control: already begins with a literal space
    ]
    prefix_space_text_cases = []
    for text in prefix_space_texts:
        enc = prefix_space_tokenizer.encode(text)
        prefix_space_text_cases.append({
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
            "decoded": prefix_space_tokenizer.decode(enc.ids, skip_special_tokens=False),
        })
    cases.append({
        "id": len(cases),
        "name": "add_prefix_space_after_normalize",
        "tokenizer_json": prefix_space_tokenizer.to_str(),
        "texts": prefix_space_text_cases,
    })

    return {
        "metadata": {
            "algorithm": "BPE normalizer",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


# The SentencePiece meta symbol, U+2581 LOWER ONE EIGHTH BLOCK, which both
# spellings of the whitespace escape substitute for a literal space.
META_SYMBOL = "▁"

# One text per place the three prepend schemes and the two spellings can part;
# each line below says which place it is.
BPE_METASPACE_TEXTS = [
    THE_CAT,                        # the control: a space at neither end
    " " + THE_CAT,                  # leading space -- where the two spellings part
    THE_CAT + " ",                  # trailing space: a lone symbol ends the stream
    "the  cat",                     # an interior run, which nothing collapses here
    META_SYMBOL + THE_CAT,          # already begins with the symbol, not with a space
    "\t" + THE_CAT,                 # a tab, which neither spelling rewrites
    "café",                    # non-ASCII, and no space to escape at all
    "the café ",               # non-ASCII beside a trailing space
    "",                             # nothing to escape and nothing to prepend
    # BOS_TOKEN is an added token, so it is a piece of its own -- and `first`
    # prepends to the opening piece, so where the token stands decides.
    BOS_TOKEN + THE_CAT,                # the token takes the opening piece
    THE_CAT + BOS_TOKEN + THE_CAT,      # a gap on either side of it
    BOS_TOKEN + " " + THE_CAT,          # the token, then a guarded gap
]

# Merges in rank order. Each right-hand side is a single character, so the
# vocabulary below is the alphabet plus what every merge produces.
BPE_METASPACE_MERGES = [
    (META_SYMBOL, "t"), (META_SYMBOL + "t", "h"), (META_SYMBOL + "th", "e"),
    (META_SYMBOL, "c"), (META_SYMBOL + "c", "a"), (META_SYMBOL + "ca", "t"),
    (META_SYMBOL + "ca", "f"), (META_SYMBOL + "caf", "é"),
    (META_SYMBOL, "a"),
    ("t", "h"), ("th", "e"),
    ("c", "a"), ("ca", "t"), ("ca", "f"), ("caf", "é"),
]

# Every character BPE_METASPACE_TEXTS can present after the escape has run, the
# tab included: an uncovered one would be dropped, and drop the case with it.
BPE_METASPACE_ALPHABET = ["a", "c", "e", "f", "h", "t", "é", "\t", META_SYMBOL]


def _small_metaspace_bpe_tokenizer():
    """A SentencePiece-BPE model whose merges only fire once the escape has run.

    Not _small_bytelevel_bpe_tokenizer(): that one is byte-level, and this
    lineage -- Llama-2, Mistral v0.1 -- is not. Half of these merges are spelled
    with the meta symbol, so "the cat" reaches a whole-word token only when a
    space has become one, and a corpus built on a model without them would pass
    with the escape switched off. byte_fallback stays off: it is refused until
    issue #317, and a fixture declaring it would not load.

    Carries no pre_tokenizer, no normalizer and no decoder -- each case sets the
    first two itself, and the third is left out because neither `tokenizers` nor
    `BpeTokenizer` would undo the escape the same way; see the corpus docstring.

    It does carry one special token, which both target models declare: an added
    token is a piece of its own, so it is what tells `first` apart from `always`
    on a model that splits at nothing else.
    """
    from tokenizers import Tokenizer, models  # noqa: PLC0415

    vocab = {}
    for symbol in BPE_METASPACE_ALPHABET:
        vocab[symbol] = len(vocab)
    for left, right in BPE_METASPACE_MERGES:
        vocab.setdefault(left + right, len(vocab))
    tokenizer = Tokenizer(models.BPE(vocab, BPE_METASPACE_MERGES))
    tokenizer.pre_tokenizer = None
    tokenizer.normalizer = None
    tokenizer.decoder = None
    tokenizer.add_special_tokens([BOS_TOKEN])
    return tokenizer


def _metaspace_pipeline(base: str, pre_tokenizer, normalizer) -> str:
    """Puts one pipeline's two blocks into the model's own serialized file."""
    raw = json.loads(base)
    raw["pre_tokenizer"] = pre_tokenizer
    raw["normalizer"] = normalizer
    return json.dumps(raw, ensure_ascii=False)


def generate_bpe_metaspace() -> dict:
    """The whitespace escape a SentencePiece-BPE file writes, in both spellings.

    Six pipelines over one model. Mistral v0.1 writes a `Metaspace`
    pre-tokenizer with `split` off, Llama-2 a `Prepend` + `Replace` normalizer
    with a null pre-tokenizer, and decision 0050 §2 calls those two writings of
    one value -- so the first two cases carry the same texts and the equality is
    the corpus's subject. `never` and `always` are here because the loader maps
    three prepend schemes and the two model files between them exercise one.

    The last two carry the pre-0.14 spelling, `add_prefix_space` where a newer
    file writes `prepend_scheme`, which nothing else measures: `tokenizers`
    0.23.1 still reads it, mapping a bare `true` onto `always` and letting an
    explicit `prepend_scheme` win over it. A bare `false` is not here because
    `tokenizers` refuses that file outright -- see docs/equivalence.md, which
    records that Lodestar reads it as `never` instead.

    Each case carries its own whole tokenizer_json so the C# side parses the
    exact bytes `tokenizers` was handed, the two legacy blocks above all: they
    are written by hand and read back, since `to_str()` would rewrite them into
    the modern spelling and measure nothing. No `decoded` column: with no
    decoder `tokenizers` joins the tokens with a space, with a `Metaspace` one it
    undoes the escape, and `BpeTokenizer` does neither -- the escape is an
    encode-side transform in this lot, and a decode column would claim a
    reproduction that is not there.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    base = _small_metaspace_bpe_tokenizer().to_str()
    metaspace = {"type": "Metaspace", "replacement": META_SYMBOL, "split": False}
    prepend_replace = {
        "type": "Sequence",
        "normalizers": [
            {"type": "Prepend", "prepend": META_SYMBOL},
            {"type": "Replace", "pattern": {"String": " "}, "content": META_SYMBOL},
        ],
    }
    pipelines = [
        # The two spellings, first: same texts, and the same stream is the point.
        ("metaspace_first", {**metaspace, "prepend_scheme": "first"}, None),
        ("prepend_replace_normalizer", None, prepend_replace),
        ("metaspace_never", {**metaspace, "prepend_scheme": "never"}, None),
        ("metaspace_always", {**metaspace, "prepend_scheme": "always"}, None),
        # The pre-0.14 spelling alone, then beside the field that supersedes it.
        ("legacy_add_prefix_space", {**metaspace, "add_prefix_space": True}, None),
        ("legacy_add_prefix_space_beside_prepend_scheme",
         {**metaspace, "add_prefix_space": True, "prepend_scheme": "never"}, None),
    ]

    cases = []
    for name, pre_tokenizer, normalizer in pipelines:
        tokenizer_json = _metaspace_pipeline(base, pre_tokenizer, normalizer)
        tokenizer = Tokenizer.from_str(tokenizer_json)
        texts = []
        for text in BPE_METASPACE_TEXTS:
            enc = tokenizer.encode(text)
            texts.append({"text": text, "tokens": enc.tokens, "ids": enc.ids})
        cases.append({
            "id": len(cases),
            "name": name,
            "tokenizer_json": tokenizer_json,
            "texts": texts,
        })

    return {
        "metadata": {
            "algorithm": "BPE metaspace",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


# The pieces a byte_fallback vocabulary carries, and the texts that reach them:
# one per UTF-8 width, a control character, and a covered symbol as the control.
BYTE_FALLBACK_TEXTS = ["ab", "aéb", "日", "🙂", "a\tb", "é", ""]

# The conventional end_of_word_suffix spelling. Named once: the marker's own bytes
# are what an uncovered decorated symbol exposes, the way "##" does below it.
BYTE_FALLBACK_EOW_SUFFIX = "</w>"


def _byte_fallback_vocab(extra=()):
    """The four-piece model plus all 256 byte pieces, and whatever a case adds."""
    vocab = {UNK_TOKEN_LOWER: 0, "a": 1, "b": 2, "ab": 3}
    for value in range(256):
        vocab.setdefault(f"<0x{value:02X}>", len(vocab))
    for piece in extra:
        vocab.setdefault(piece, len(vocab))
    return vocab


def _byte_fallback_file(vocab, merges, *, fuse_unk=False, prefix=None, suffix=None,
                         unk_token=UNK_TOKEN_LOWER, decoder=None, pre_tokenizer=None):
    """One whole tokenizer.json, written by hand so tokenizers parses the exact bytes C# will."""
    return json.dumps({
        "version": "1.0", "truncation": None, "padding": None, "added_tokens": [],
        "normalizer": None, "pre_tokenizer": pre_tokenizer, "post_processor": None, "decoder": decoder,
        "model": {
            "type": "BPE", "dropout": None, "unk_token": unk_token,
            "continuing_subword_prefix": prefix, "end_of_word_suffix": suffix,
            "fuse_unk": fuse_unk, "byte_fallback": True, "ignore_merges": False,
            "vocab": vocab, "merges": merges,
        },
    }, ensure_ascii=False)


# The bare step, and Llama-2's chain -- decision 0063's two decode shapes. Replace
# runs before ByteFallback, so a covered META_SYMBOL below is what keeps it live.
BYTE_FALLBACK_SEQUENCE_DECODER = {"type": "Sequence", "decoders": [
    {"type": "Replace", "pattern": {"String": META_SYMBOL}, "content": " "},
    {"type": "ByteFallback"},
    {"type": "Fuse"},
    {"type": "Strip", "content": " ", "start": 1, "stop": 0},
]}

# Mistral v0.1's own pre_tokenizer shape. Both decoder cases carry it and cover
# META_SYMBOL, so they encode to the same ids and only their decoders differ.
BYTE_FALLBACK_METASPACE_PRE_TOKENIZER = {
    "type": "Metaspace", "replacement": META_SYMBOL, "prepend_scheme": "first", "split": False,
}

# The two cases that carry a `decoded` column -- the only ones a decoder is declared for.
BYTE_FALLBACK_DECODED_CASES = ("decoder_byte_fallback", "decoder_sequence")

# The two bytes of "e-acute", named because four of the runs below reach for the
# lead byte and Sonar's S1192 counts them together -- issue #487's quality gate.
E_ACUTE_LEAD = "<0xC3>"
E_ACUTE_TAIL = "<0xA9>"

# Raw id runs: the `decoded` column above comes from `encode`, which cannot produce a byte
# run cut mid-character. generate_bpe_byte_fallback's docstring has what each row measures.
BYTE_FALLBACK_DECODE_RUNS = [
    [E_ACUTE_LEAD],                            # a two-byte lead byte, alone
    ["<0xF0>", "<0x9F>"],                      # an emoji cut after two of its four bytes
    [E_ACUTE_LEAD, "<0x28>"],                  # a lead byte, then an ASCII byte that cannot continue it
    ["a", "<0xF0>", "<0x9F>", "b"],            # the same cut, between two covered symbols
    [E_ACUTE_LEAD, E_ACUTE_TAIL],              # well-formed: the two bytes of "e-acute"
    ["a", E_ACUTE_LEAD, E_ACUTE_TAIL, "b"],    # well-formed, between two covered symbols
]


def generate_bpe_byte_fallback() -> dict:
    """An uncovered symbol resolving into byte pieces, in every shape the rule has.

    Ten pipelines over one four-piece model that carries all 256 byte pieces.
    `BYTE_FALLBACK_TEXTS` is the widths -- one text per UTF-8 byte count, plus a
    control character and a covered symbol. The two merge cases prove the
    expansion precedes the merges: a post-pass over unmergeable symbols could
    not produce `<0xC3><0xA9>` from a declared merge. `fuse_unk_on` is the half
    of the pair that discriminates -- `aXXb`-shaped fusing never reaches a
    byte-resolved symbol; `fuse_unk_off` is the control, and is byte-identical
    to `complete_alphabet` (same `tokenizer_json`, same seven streams), since
    with the flag off there is nothing left for the setting to change.
    `unk_token_absent` is the same alphabet again with no unknown token
    declared at all: the reference still expands every uncovered symbol, so
    the expansion is not gated on an unk_token being present, on either side
    of this pairing.

    `continuing_prefix` and `end_of_word_suffix` are the pair that show what is
    expanded: the marker itself is encoded as its own bytes, because the
    *decorated* symbol is the string with no entry -- `##` becomes two `#`
    bytes ahead of an uncovered character, `</w>` becomes four bytes after one,
    and a one-character text takes both roles at once.

    The last two carry a `decoded` column, which the metaspace corpus
    deliberately does not: there the decoder was accepted and not applied, and
    here it is declared and reproduced. Both decoder cases declare the same
    `Metaspace` pre_tokenizer and cover `META_SYMBOL` in the same vocabulary, so
    they encode every text to identical ids -- only the decoder differs, which
    is what makes the pair a contrast rather than two texts that each merely
    round-trip themselves. `decoder_sequence`'s Replace and Strip undo the
    escape (`Decode(Encode(x)) == x`); `decoder_byte_fallback`'s bare
    `ByteFallback` step has no Replace to run, so the leading `META_SYMBOL`
    passes straight through un-undone -- on the same ids, `"aéb"` decodes to
    `"aéb"` under one and `"▁aéb"` under the other.

    Those two cases also carry `decode_runs`: the raw id sequences of
    `BYTE_FALLBACK_DECODE_RUNS`, handed straight to `decode`. The `decoded`
    column above comes from `encode`, which cannot produce a byte run cut
    mid-character -- and that run is exactly what a generation truncated
    mid-character hands `decode` on this lineage. It is also where two
    substitution rules part company: `tokenizers` pushes one U+FFFD per byte of
    a run it cannot decode, where a decoder substituting once per maximal
    invalid subpart (.NET's lossy UTF-8 decoder, Rust's `from_utf8_lossy`)
    answers one character where these rows want two. The last two rows are the
    well-formed controls, where every rule agrees.

    No case declares a partial alphabet. `tokenizers` accepts one and degrades
    to the unknown token; Lodestar refuses it, so there is no reference stream
    to record -- BpeByteFallbackLoaderTests pins the refusal instead.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    pipelines = [
        ("complete_alphabet", _byte_fallback_file(_byte_fallback_vocab(), ["a b"])),
        ("merged_byte_pair", _byte_fallback_file(
            _byte_fallback_vocab(["<0xC3><0xA9>"]), ["a b", "<0xC3> <0xA9>"])),
        ("merged_across", _byte_fallback_file(
            _byte_fallback_vocab(["a<0xC3>"]), ["a b", "a <0xC3>"])),
        ("fuse_unk_on", _byte_fallback_file(_byte_fallback_vocab(), ["a b"], fuse_unk=True)),
        ("fuse_unk_off", _byte_fallback_file(_byte_fallback_vocab(), ["a b"], fuse_unk=False)),
        ("unk_token_absent", _byte_fallback_file(_byte_fallback_vocab(), ["a b"], unk_token=None)),
        ("continuing_prefix", _byte_fallback_file(
            _byte_fallback_vocab(["##b"]), [], prefix="##")),
        ("end_of_word_suffix", _byte_fallback_file(
            _byte_fallback_vocab(["b" + BYTE_FALLBACK_EOW_SUFFIX]), [],
            suffix=BYTE_FALLBACK_EOW_SUFFIX)),
        ("decoder_byte_fallback", _byte_fallback_file(
            _byte_fallback_vocab([META_SYMBOL]), ["a b"], decoder={"type": "ByteFallback"},
            pre_tokenizer=BYTE_FALLBACK_METASPACE_PRE_TOKENIZER)),
        ("decoder_sequence", _byte_fallback_file(
            _byte_fallback_vocab([META_SYMBOL]), ["a b"],
            decoder=BYTE_FALLBACK_SEQUENCE_DECODER,
            pre_tokenizer=BYTE_FALLBACK_METASPACE_PRE_TOKENIZER)),
    ]

    cases = []
    for name, tokenizer_json in pipelines:
        tokenizer = Tokenizer.from_str(tokenizer_json)
        decodes = name in BYTE_FALLBACK_DECODED_CASES
        texts = []
        for text in BYTE_FALLBACK_TEXTS:
            enc = tokenizer.encode(text)
            row = {"text": text, "tokens": enc.tokens, "ids": enc.ids}
            if decodes:
                row["decoded"] = tokenizer.decode(enc.ids)
            texts.append(row)
        case = {
            "id": len(cases),
            "name": name,
            "tokenizer_json": tokenizer_json,
            "texts": texts,
        }
        if decodes:
            case["decode_runs"] = [
                {
                    "pieces": pieces,
                    "ids": [tokenizer.token_to_id(piece) for piece in pieces],
                    "decoded": tokenizer.decode([tokenizer.token_to_id(piece) for piece in pieces]),
                }
                for pieces in BYTE_FALLBACK_DECODE_RUNS
            ]
        cases.append(case)

    return {
        "metadata": {
            "algorithm": "BPE byte_fallback",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


# The two files #175 names, vendored by tools/fetch_llama2_mistral_tokenizers.py.
LINEAGE_MODELS = {
    "llama2": "llama2_tokenizer.json",
    "mistral_v01": "mistral_v01_tokenizer.json",
}

# Chosen so every text reaches the path this corpus is about (#208, ADR 0004);
# each row says which half it exercises.
LINEAGE_TEXTS = [
    "",                       # the empty control
    "the cat",                # Metaspace: the escape and the leading prepend
    " the cat",               # not the same ids: the prepend runs, then the space escapes
    "  spaced  out  ",        # the whitespace-run merges, where the mirrors parted
    "aujourd'hui",            # a French elision, deep in the merge table
    "héllo",             # the two models' merge tables answer differently
    "\U0001f999",             # byte_fallback: four byte pieces on both models
    "\U0001f600ok",           # covered by Mistral, byte-resolved by Llama-2
    # A special token as ordinary text: #551, settled by decision 0085. The two models
    # answer these differently on purpose, which is the point of freezing them.
    BOS_TOKEN,                # the token alone: one id on both, two before 0085
    "</s>",                   # its sibling, so the rule is not pinned on one entry
    BOS_TOKEN + THE_CAT,      # at the head, where Llama-2's pattern does match
    THE_CAT + BOS_TOKEN,      # mid-text, where it does not: '<', 's', '>' on Llama-2
    " " + BOS_TOKEN,          # after a space, which the escape turns into the prefix
]


def generate_sentencepiece_bpe_lineage() -> dict:
    """Llama-2 and Mistral v0.1 encoding their own texts, the two files #175 names.

    This is the lot that makes #175's opening sentence false rather than true:
    "files a user actually has, and neither tokenizer here loads them". Both are
    SentencePiece-BPE -- a Metaspace whitespace escape over byte_fallback -- and
    they write that pipeline **two different ways**, which is why the corpus needs
    both files and not one of them twice. Llama-2 carries a `Prepend` plus
    `Replace` normalizer with a null pre_tokenizer; Mistral a `Metaspace`
    pre-tokenizer with `split` off and no normalizer. Decision 0050 section 2 calls
    those two writings of one value, and this is where that claim meets two real
    files rather than a synthetic pair.

    `add_special_tokens` is off. The files declare a `TemplateProcessing` that
    prepends `<s>`, which decision 0083 reads into `BpeVocabulary.PrefixTokens`
    for a caller to apply through `SpecialTokenTemplate` -- `BpeTokenizer.Encode`
    does not apply it, so a corpus recording the reference's prefixed stream would
    compare two different operations.

    `skip_special_tokens` is off on the decode side too, so `decoded` and
    `BpeTokenizer.Decode` are the same operation: the reference skips them by
    default, which would compare an id stream against a shorter one.

    **Five rows carry a special token as ordinary text**, which is what #551 was
    filed for and decision 0085 settled. The two models answer them differently on
    purpose: Llama-2 declares its added tokens `normalized`, so `tokenizers`
    normalizes the pattern too and matches `▁<s>` -- which matches at the head of a
    text and *not* after a letter, where the model spells `<`, `s`, `>` instead.
    Mistral declares them raw and matches `<s>` anywhere. Both are frozen here now
    that Lodestar reproduces them; before 0085 it matched `<s>` on both files while
    escaping only the text, which cost a leading `▁` on one side and turned a
    caller's `<s>` into the BOS id on the other.

    The discriminating row is `\U0001f600ok`: Mistral's vocabulary carries that emoji
    where Llama-2's does not, so one model answers a whole token and the other four
    byte pieces. Without it the two halves of this corpus could both pass while
    measuring the same thing twice.

    Llama-2 is vendored under decision 0084's named exception to 0003's allowed-source
    list -- the LLAMA 2 COMMUNITY LICENSE, for this artifact alone, with the licence,
    the notice and the acceptable-use policy vendored beside it.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    cases = []
    for name, fixture in LINEAGE_MODELS.items():
        tokenizer = Tokenizer.from_file(str(ORACLE_DIR / fixture))
        for text in LINEAGE_TEXTS:
            encoded = tokenizer.encode(text, add_special_tokens=False)
            cases.append({
                "id": len(cases),
                "model": name,
                "text": text,
                "tokens": encoded.tokens,
                "ids": encoded.ids,
                "decoded": tokenizer.decode(encoded.ids, skip_special_tokens=False),
            })

    return {
        "metadata": {
            "algorithm": "SentencePiece BPE (Metaspace + byte_fallback)",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "models": {
                name: f"{fixture} (vendored by tools/fetch_llama2_mistral_tokenizers.py)"
                for name, fixture in LINEAGE_MODELS.items()
            },
            "count": len(cases),
        },
        "cases": cases,
    }


# Two CJK texts, an emoji sequence and two controls: a byte-level token is a
# fragment of a multi-byte character far more often than not.
BYTELEVEL_STREAM_TEXTS = [
    "東京 \U0001f44b",          # 東京 + waving hand
    "日本語のテキスト",  # a Japanese sentence
    "\U0001f1eb\U0001f1f7 emoji",       # a regional-indicator pair
    "déjà vu",                # Latin-1: no fragment, the control
    HELLO_WORLD,                        # ASCII: the other control
]


def generate_bytelevel_decode_stream() -> dict:
    """Each id of a text decoded on its own, which is how a stream is consumed.

    tokenizers substitutes U+FFFD for a byte sequence that is not well-formed
    UTF-8; Lodestar threw until issue #149. The `replacement_count` per case is
    carried so a corpus that stopped exercising the substitution would be noticed
    rather than pass silently.
    """
    tokenizer = _gpt2_tokenizer()

    cases = []
    for text in BYTELEVEL_STREAM_TEXTS:
        enc = tokenizer.encode(text)
        per_id = [tokenizer.decode([i]) for i in enc.ids]
        cases.append({
            "id": len(cases),
            "text": text,
            "ids": enc.ids,
            "tokens": enc.tokens,
            "per_id_decoded": per_id,
            "replacement_count": sum(1 for s in per_id if "\ufffd" in s),
            "decoded": tokenizer.decode(enc.ids),
        })
    return {
        "metadata": {
            "algorithm": "byte-level decode, one id at a time",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "count": len(cases),
        },
        "cases": cases,
    }


# The two settings the rest of the BPE corpus structurally cannot see, exercised
# together because they interact; see generate_bpe_added_tokens's docstring.
BPE_ADDED_TOKEN_TEXTS = BPE_TEXTS + [
    "hi<|endoftext|>bye",             # a segment after an added token, no space of its own
    END_OF_TEXT,                      # nothing but the token
    "<|endoftext|>tail",              # an empty segment before it
    " <|endoftext|> ",                # segments that already start with a space
    "x<|endoftext|> y<|endoftext|>z",  # several, mixed
]


def generate_bpe_added_tokens() -> dict:
    """GPT-2 with its own <|endoftext|> registered, and add_prefix_space on.

    The corpus carries the whole `tokenizer.json` once, in the metadata: the C#
    side parses the exact bytes HuggingFace was handed, as
    `generate_bpe_tokenizer_json` does, rather than a second fixture that could
    drift from it.

    BPE_ADDED_TOKEN_TEXTS exercises two settings the rest of the BPE corpus
    structurally cannot see, together because they interact:

    1. `<|endoftext|>` is id 50256 in GPT-2's own model.vocab and is also
       listed in added_tokens. Every other BPE fixture here either registers
       no added token at all (`BPE.from_file` does not) or never names one in
       its text, so a loader that reads added_tokens as "the entries
       model.vocab lacks" passes the whole corpus while dropping every
       special token there is.
    2. `add_prefix_space` is HuggingFace's ByteLevel default and nothing on
       this branch had ever generated a corpus with it on. It applies per
       added-token-delimited segment, not once to the whole input, and only
       when the segment does not already start with a space -- observable
       only when a text starts with a space, or when an added token sits
       between two segments, hence the extra texts below.
    """
    from tokenizers import AddedToken  # noqa: PLC0415

    tokenizer = _gpt2_tokenizer(add_prefix_space=True)
    tokenizer.add_special_tokens([AddedToken(END_OF_TEXT, special=True)])

    cases = []
    for i, text in enumerate(BPE_ADDED_TOKEN_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({
            "id": i,
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
            "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            "decoded_skip_specials": tokenizer.decode(enc.ids, skip_special_tokens=True),
        })
    return {
        "metadata": {
            "algorithm": "ByteLevelBPE with added tokens and add_prefix_space",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "gpt2 (vendored by tools/fetch_gpt2_bpe.py), <|endoftext|> added as special",
            "tokenizer_json": tokenizer.to_str(),
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Regression metrics (issue #92) ------------------------------------------

REGRESSION_SEED = METRIC_SEED + 2


def _regression_fixtures() -> list[dict]:
    """Fixtures chosen so that each degenerate branch has one case of its own.

    The ordinary ones exist to prove the arithmetic; the degenerate ones exist
    because every implementation agrees on the ordinary ones.

    uniform_fractional_weights carries a uniform *fractional* weight, which is
    where the weighted percentile's tolerance shows and nowhere else. 0.1 is
    not representable in binary, so the cumulative sum overshoots half the
    total by units in the last place; scikit-learn averages anyway (its test
    is `fraction_above > eps`, not `> 0`) and returns 4.5 on these residuals,
    where an exact test returns 4.0. Every other weighted fixture here is
    exactly representable -- 1, 2, 3, 7 -- so none of them can tell the two
    rules apart, and the residuals are distinct so the two averaged order
    statistics differ.
    """
    rng = SeededRandom(REGRESSION_SEED)

    def weights(n: int) -> list[float]:
        return [round(rng.uniform(0.1, 3.0), 3) for _ in range(n)]

    def noisy(truth: list[float], spread: float) -> list[float]:
        return [round(t + rng.gauss(0.0, spread), 12) for t in truth]

    fixtures: list[dict] = []

    # Ordinary single output, positive targets: the only shape where the log
    # family is defined, so it carries msle/rmsle as well as everything else.
    positive = [round(rng.uniform(0.5, 40.0), 12) for _ in range(200)]
    fixtures.append({"name": "positive_single", "output_count": 1,
                     "y_true": positive, "y_pred": noisy(positive, 2.0),
                     "sample_weight": weights(len(positive))})

    # Targets straddling zero: mape's clamp never fires (no exact zero) but the
    # values get small, and msle is undefined, so this case proves the split.
    signed = [round(rng.uniform(-20.0, 20.0), 12) for _ in range(200)]
    fixtures.append({"name": "signed_single", "output_count": 1,
                     "y_true": signed, "y_pred": noisy(signed, 3.0),
                     "sample_weight": weights(len(signed))})

    # Three outputs, deliberately unequal in variance so variance_weighted and
    # uniform_average cannot coincide.
    rows_true: list[float] = []
    rows_pred: list[float] = []
    for _ in range(150):
        base = [round(rng.uniform(0.5, 5.0), 12),
                round(rng.uniform(0.5, 60.0), 12),
                round(rng.uniform(0.5, 400.0), 12)]
        rows_true.extend(base)
        rows_pred.extend(round(b + rng.gauss(0.0, 0.05 * b), 12) for b in base)
    fixtures.append({"name": "positive_three_outputs", "output_count": 3,
                     "y_true": rows_true, "y_pred": rows_pred,
                     "sample_weight": weights(150)})

    # --- the degenerate cases, one fixture each ---

    # A truth with zero variance: force_finite decides, zeroDivision does not.
    fixtures.append({"name": "constant_truth_perfect", "output_count": 1,
                     "y_true": [2.0, 2.0, 2.0], "y_pred": [2.0, 2.0, 2.0],
                     "sample_weight": [1.0, 2.0, 3.0]})
    fixtures.append({"name": "constant_truth_imperfect", "output_count": 1,
                     "y_true": [2.0, 2.0, 2.0], "y_pred": [1.0, 2.0, 3.0],
                     "sample_weight": [1.0, 2.0, 3.0]})

    # One sample: r2 is nan under either force_finite, which is zeroDivision's
    # territory and nothing else's.
    fixtures.append({"name": "single_sample", "output_count": 1,
                     "y_true": [3.0], "y_pred": [5.0], "sample_weight": [2.0]})

    # An exact zero in the truth: mape's epsilon clamp, and nothing else's.
    fixtures.append({"name": "zero_in_truth", "output_count": 1,
                     "y_true": [0.0, 4.0, -2.0], "y_pred": [1.0, 5.0, -1.0],
                     "sample_weight": [1.0, 1.0, 2.0]})

    # An even sample count with a lopsided weight, where the averaged weighted
    # percentile and a plain median part company.
    fixtures.append({"name": "lopsided_weights", "output_count": 1,
                     "y_true": [0.0, 2.0, 4.0, 10.0], "y_pred": [0.0, 0.0, 0.0, 0.0],
                     "sample_weight": [1.0, 1.0, 1.0, 7.0]})

    # See this function's docstring for why 0.1 needs a fractional weight.
    fixtures.append({"name": "uniform_fractional_weights", "output_count": 1,
                     "y_true": [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0],
                     "y_pred": [1.0] * 10,
                     "sample_weight": [0.1] * 10})

    return fixtures


def _regression_call(fn, y_true, y_pred, sw, **fixed):
    """Binds one scikit-learn metric to this case, leaving `multioutput` free.

    `multioutput=None` means "do not pass it", which is what uniform_average is
    on every one of these functions. `fixed` carries whatever else the metric
    takes for the whole case — `alpha` for the pinball loss, `force_finite` for
    R2 and explained variance — so that one binder serves all three families.
    """
    def call(mo):
        kw = dict(fixed)
        if mo is not None:
            kw["multioutput"] = mo
        return fn(y_true, y_pred, sample_weight=sw, **kw)
    return call


def _regression_emit(values: dict, key: str, call, mo, is_vector: bool) -> None:
    result = call(mo)
    if is_vector:
        # raw_values on a 1-D input returns a 0-d array in some paths and a
        # 1-element one in others, so it is widened before being walked.
        values[key] = [_finite_or_name(float(x)) for x in np.atleast_1d(result)]
    else:
        values[key] = _finite_or_name(float(result))


def _regression_shapes(values: dict, key: str, call, ow,
                       suffix: str = "", variance_weighted: bool = False) -> None:
    """Records one metric under every multioutput shape scikit-learn allows it.

    Writing the four shapes here rather than four times per family is what keeps
    `_regression_case` under S3776's limit: each copy carried its own
    `if ow is not None` guard, and four guarded copies in four loops is most of
    what the rule was counting.

    `suffix` exists because R2 and explained variance key on `metric|shape|flag`
    while everything else keys on `metric|shape`, and the flag trails the shape.
    """
    shapes = [("uniform", None, False), ("raw", "raw_values", True)]
    if variance_weighted:
        shapes.append(("variance_weighted", "variance_weighted", False))
    if ow is not None:
        shapes.append(("weights", ow, False))

    for shape, mo, is_vector in shapes:
        _regression_emit(values, f"{key}|{shape}{suffix}", call, mo, is_vector)


_REGRESSION_PLAIN = {
    "mse": skm.mean_squared_error,
    "rmse": skm.root_mean_squared_error,
    "mae": skm.mean_absolute_error,
    "median_ae": skm.median_absolute_error,
    "mape": skm.mean_absolute_percentage_error,
}

# Defined only where every target is above -1, which is why they are their own
# list rather than more entries above.
_REGRESSION_LOG = (
    ("msle", skm.mean_squared_log_error),
    ("rmsle", skm.root_mean_squared_log_error),
)

# The two that take force_finite, and the only two scikit-learn accepts
# variance_weighted for.
_REGRESSION_SCORED = (
    ("r2", skm.r2_score),
    ("ev", skm.explained_variance_score),
)


def _regression_arrays(fx: dict, k: int):
    """The fixture's flat lists as scikit-learn wants to see them.

    Single-output targets are handed over 1-D rather than as an n x 1 matrix,
    because that is the shape a caller writes and the shape `max_error` accepts.
    """
    n = len(fx["y_true"]) // k
    y_true = np.asarray(fx["y_true"], dtype=float).reshape(n, k)
    y_pred = np.asarray(fx["y_pred"], dtype=float).reshape(n, k)
    return (y_true.ravel(), y_pred.ravel()) if k == 1 else (y_true, y_pred)


def _regression_log_defined(y_true, y_pred) -> bool:
    """Whether the log family is defined here: every target strictly above -1."""
    return float(np.min(y_true)) > -1.0 and float(np.min(y_pred)) > -1.0


def _regression_case(fx: dict, weighted: bool) -> dict:
    sw = fx["sample_weight"] if weighted else None
    k = fx["output_count"]
    y_true, y_pred = _regression_arrays(fx, k)

    values: dict = {}

    # Output weights are fixed rather than drawn, so that a reader can check the
    # reduction by hand: 0.3/0.7 on two outputs, 0.2/0.3/0.5 on three.
    ow = {1: None, 2: [0.3, 0.7], 3: [0.2, 0.3, 0.5]}[k]

    for name, fn in _REGRESSION_PLAIN.items():
        _regression_shapes(values, name, _regression_call(fn, y_true, y_pred, sw), ow)

    if _regression_log_defined(y_true, y_pred):
        for name, fn in _REGRESSION_LOG:
            _regression_shapes(values, name, _regression_call(fn, y_true, y_pred, sw), ow)

    # max_error takes neither sample_weight nor multioutput, and refuses 2-D
    # input outright with "Multioutput not supported in max_error".
    if k == 1 and not weighted:
        values["max_error|uniform"] = _finite_or_name(float(skm.max_error(y_true, y_pred)))

    for alpha in (0.5, 0.9):
        _regression_shapes(
            values, f"pinball{alpha}",
            _regression_call(skm.mean_pinball_loss, y_true, y_pred, sw, alpha=alpha), ow)

    for name, fn in _REGRESSION_SCORED:
        for ff in (True, False):
            _regression_shapes(
                values, name,
                _regression_call(fn, y_true, y_pred, sw, force_finite=ff), ow,
                suffix="|force_finite" if ff else "|raw_infinity",
                variance_weighted=True)

    return {
        "fixture": fx["name"],
        "weighted": weighted,
        "output_count": k,
        "y_true": fx["y_true"],
        "y_pred": fx["y_pred"],
        "sample_weight": sw,
        "values": values,
    }


def generate_regression() -> dict:
    with warnings.catch_warnings():
        # scikit-learn warns on every undefined metric; the corpus records the
        # value it returns, which is the thing under test.
        warnings.simplefilter("ignore")
        cases = [
            _regression_case(fx, weighted)
            for fx in _regression_fixtures()
            for weighted in (False, True)
        ]
    return {
        "metadata": {
            "algorithm": "Regression",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.mean_squared_error",
                "sklearn.metrics.root_mean_squared_error",
                "sklearn.metrics.mean_absolute_error",
                "sklearn.metrics.median_absolute_error",
                "sklearn.metrics.mean_absolute_percentage_error",
                "sklearn.metrics.mean_squared_log_error",
                "sklearn.metrics.root_mean_squared_log_error",
                "sklearn.metrics.max_error",
                "sklearn.metrics.r2_score",
                "sklearn.metrics.explained_variance_score",
                "sklearn.metrics.mean_pinball_loss",
            ],
            "count": len(cases),
        },
        "cases": cases,
    }


# --- The conditioning the ordinary regression corpus cannot reach (issue #127) ---
# See generate_regression_conditioning's docstring.

CONDITIONING_SAMPLES = 200_000
CONDITIONING_OFFSET = 1e9
CONDITIONING_SPREAD = 1e-2
# 1e-6, not the ramp's own 5e-8 step, so quantizing it carries the ill
# conditioning; see generate_regression_conditioning's docstring for the ULP math.
CONDITIONING_PERTURBATION = 1e-6
PROBE_INDICES = [0, 1, CONDITIONING_SAMPLES // 2, CONDITIONING_SAMPLES - 2, CONDITIONING_SAMPLES - 1]


def _conditioning_arrays() -> tuple[list[float], list[float]]:
    """The closed form both sides build, and nothing but it."""
    step = CONDITIONING_SPREAD / CONDITIONING_SAMPLES
    y_true = [CONDITIONING_OFFSET + i * step for i in range(CONDITIONING_SAMPLES)]
    y_pred = [y_true[i] + ((i % 7) - 3) * CONDITIONING_PERTURBATION
              for i in range(CONDITIONING_SAMPLES)]
    return y_true, y_pred


def _bits(value: float) -> str:
    """The double's raw IEEE-754 bits, so a probe compares the number and not its spelling."""
    import struct  # noqa: PLC0415

    return f"{struct.unpack('<Q', struct.pack('<d', value))[0]:016x}"


def generate_regression_conditioning() -> dict:
    """scikit-learn's answers on a target no committed array could carry.

    regression.json stores its arrays in full and caps at 450 values, over
    targets in [0.5, 40] -- a range where a sequential sum and numpy's pairwise
    one agree to far more digits than the corpus compares at. The defect #127
    fixes needs the opposite: many samples, and a large offset over a small
    spread, so that the low-order bits of every term fall off the end of the
    accumulator.

    Storing 200 000 samples as JSON would be megabytes, so this case carries
    the closed form instead. The C# side rebuilds the same arrays from the
    same expression, in the same order -- both languages evaluate IEEE-754
    doubles, so the two constructions are identical value for value.
    PROBE_INDICES is how that stops being a matter of faith: the raw bits at
    those positions are recorded and compared before anything is scored.

    CONDITIONING_PERTURBATION is 1e-6, not the ramp's own 5e-8 step: the ULP at
    1e9 is 2^-23, about 1.19e-7 (checked with math.ulp(1e9)), so a
    perturbation below half of that rounds straight back onto the target.
    Measured: with 1e-8 every residual is exactly zero, mse is 0 and r2 is 1,
    and a fixture built that way passes while proving nothing. The ramp's step
    stays below the ULP on purpose -- quantizing it is the ill conditioning
    this case exists to carry.
    """
    y_true, y_pred = _conditioning_arrays()
    yt = np.asarray(y_true)
    yp = np.asarray(y_pred)

    values = {
        "r2": stable(float(skm.r2_score(yt, yp))),
        EXPLAINED_VARIANCE_KEY: stable(float(skm.explained_variance_score(yt, yp))),
        "mse": stable(float(skm.mean_squared_error(yt, yp))),
        "mae": stable(float(skm.mean_absolute_error(yt, yp))),
    }
    return {
        "metadata": {
            "algorithm": "Regression under ill conditioning",
            "library": "scikit-learn",
            "library_version": version("scikit-learn"),
            "reference_calls": [
                "sklearn.metrics.r2_score",
                "sklearn.metrics.explained_variance_score",
                "sklearn.metrics.mean_squared_error",
                "sklearn.metrics.mean_absolute_error",
            ],
            SAMPLES: CONDITIONING_SAMPLES,
            "offset": CONDITIONING_OFFSET,
            "spread": CONDITIONING_SPREAD,
            "perturbation": CONDITIONING_PERTURBATION,
            "construction": (
                "step = spread / samples; y_true[i] = offset + i * step; "
                "y_pred[i] = y_true[i] + ((i % 7) - 3) * perturbation"
            ),
            "probe_indices": PROBE_INDICES,
            "probe_bits_y_true": [_bits(y_true[i]) for i in PROBE_INDICES],
            "probe_bits_y_pred": [_bits(y_pred[i]) for i in PROBE_INDICES],
            "count": len(values),
        },
        "values": values,
    }


# Content/id matching flags over a byte-level model (issue #104); see
# generate_bpe_added_token_flags's docstring.
BPE_FLAG_TEXTS = [
    # lstrip, on <mask> -- roberta-base's own shape.
    "a <mask> b",
    "a<mask>b",
    "a  <mask>  b",
    "<mask> a",
    "a <mask>",
    "a\t<mask>",
    # A no-break space, written as an escape so no editor can flatten it to U+0020.
    "a\u00a0<mask>",
    "a. <mask>",
    # rstrip, on <pad>, the mirror of the same shapes.
    "a <pad> b",
    "a<pad>b",
    "a  <pad>  b",
    "<pad> a",
    "a <pad>",
    "a <pad>\tb",
    "a <pad>. b",
    # single_word, on <m>: a letter, a digit or '_' on either side blocks it,
    # punctuation and the ends of the text do not.
    "a <m> b",
    ".<m>.",
    "-<m>-",
    "1<m>1",
    "_<m>_",
    "é<m>é",
    "<m>",
    "a<m>b",
    # Two matches, one space apart: the probe for AddedTokenScanner's own
    # left-strip-stops-at-the-previous-match choice (see the docstring).
    "<pad> <mask>",
    "<mask> <mask>",
    "a <pad> <mask> b",
]


def generate_bpe_added_token_flags() -> dict:
    """GPT-2 with one added token per matching flag.

    Shaped after `generate_bpe_added_tokens`: the whole `tokenizer.json` rides in
    the metadata, so the C# side parses the exact bytes HuggingFace was handed.

    BPE_FLAG_TEXTS carries one token per flag, because a flag only shows in the
    pieces around the match: lstrip and rstrip make the space beside a match
    disappear -- the id is unchanged, what is gone is the 'Ġ' the whitespace
    would have produced -- and single_word makes a match not happen at all,
    leaving the marker's own characters to the merge loop.

    add_prefix_space is off here, unlike `generate_bpe_added_tokens`'s
    tokenizer: a prefix space is added per segment and would put a 'Ġ' beside
    every match, which is the very piece the strips are read from.
    bpe_added_tokens.json is where that setting is measured; this corpus keeps
    it out of the way.

    <m> is the one entry left non-special, so decoded_skip_specials shows the
    two halves of the table apart: special is what a decoder drops, and it
    decides nothing about where an entry matches.
    """
    from tokenizers import AddedToken  # noqa: PLC0415

    tokenizer = _gpt2_tokenizer()
    tokenizer.add_tokens([
        AddedToken(MASK_TOKEN, lstrip=True, special=True),
        AddedToken("<pad>", rstrip=True, special=True),
        AddedToken("<m>", single_word=True),
    ])

    cases = []
    for i, text in enumerate(BPE_FLAG_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({
            "id": i,
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
            "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            "decoded_skip_specials": tokenizer.decode(enc.ids, skip_special_tokens=True),
        })
    return {
        "metadata": {
            "algorithm": "ByteLevelBPE with added-token matching flags",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "gpt2 (vendored by tools/fetch_gpt2_bpe.py), <mask> lstrip, <pad> rstrip, <m> single_word",
            "tokenizer_json": tokenizer.to_str(),
            "count": len(cases),
        },
        "cases": cases,
    }


# --- The BPE settings that change nothing, and the one with no default (issue
# #118); see generate_bpe_no_op_settings's docstring. ---------------------------

# 'a</w>' etc. exist so no symbol is dropped for want of a vocabulary entry
# under the `</w>` contrast; see this section's docstring for why.
NO_OP_VOCAB = {"a": 0, "b": 1, "c": 2, "ab": 3, "a</w>": 4, "b</w>": 5, "c</w>": 6}
NO_OP_MERGES = [("a", "b")]
NO_OP_TEXTS = ["ab", "abc", "ab c", "a b", "c"]

# Byte-level, for `add_prefix_space`: 'Ġ' is the space, and 'ab' is reachable by
# the one merge, so the flag shows up as a leading 'Ġ' the model has an entry for.
NO_OP_BYTE_LEVEL_VOCAB = {"a": 0, "b": 1, "Ġ": 2, "ab": 3}
NO_OP_BYTE_LEVEL_MERGES = [("a", "b")]
NO_OP_BYTE_LEVEL_TEXTS = ["ab", "a b", " a"]


def _no_op_bpe(**settings):
    """The classic model every no-op case and its baseline are measured on."""
    from tokenizers import Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    tokenizer = Tokenizer(models.BPE(
        vocab=dict(NO_OP_VOCAB), merges=list(NO_OP_MERGES), unk_token=None, **settings))
    tokenizer.pre_tokenizer = pre_tokenizers.Whitespace()
    return tokenizer


def _no_op_byte_level_bpe(add_prefix_space: bool):
    """The byte-level counterpart, whose `add_prefix_space` is declared either way."""
    from tokenizers import Tokenizer, decoders, models, pre_tokenizers  # noqa: PLC0415

    tokenizer = Tokenizer(models.BPE(
        vocab=dict(NO_OP_BYTE_LEVEL_VOCAB), merges=list(NO_OP_BYTE_LEVEL_MERGES), unk_token=None))
    tokenizer.pre_tokenizer = pre_tokenizers.ByteLevel(add_prefix_space=add_prefix_space)
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def _no_op_models() -> list[tuple[str, str, object, list[str]]]:
    """Every model the corpus carries: its name, what it declares, and its texts."""
    return [
        ("baseline", "no end_of_word_suffix, no continuing_subword_prefix, no dropout",
         _no_op_bpe(), NO_OP_TEXTS),
        ("end_of_word_suffix_empty", 'end_of_word_suffix: ""',
         _no_op_bpe(end_of_word_suffix=""), NO_OP_TEXTS),
        ("end_of_word_suffix_marker", 'end_of_word_suffix: "</w>"',
         _no_op_bpe(end_of_word_suffix="</w>"), NO_OP_TEXTS),
        ("continuing_subword_prefix_empty", 'continuing_subword_prefix: ""',
         _no_op_bpe(continuing_subword_prefix=""), NO_OP_TEXTS),
        ("dropout_zero", "dropout: 0.0",
         _no_op_bpe(dropout=0.0), NO_OP_TEXTS),
        ("byte_level_add_prefix_space_true", "ByteLevel add_prefix_space: true",
         _no_op_byte_level_bpe(add_prefix_space=True), NO_OP_BYTE_LEVEL_TEXTS),
        ("byte_level_add_prefix_space_false", "ByteLevel add_prefix_space: false",
         _no_op_byte_level_bpe(add_prefix_space=False), NO_OP_BYTE_LEVEL_TEXTS),
    ]


def _byte_level_documents_without_add_prefix_space() -> list[tuple[str, dict]]:
    """The same byte-level file with `add_prefix_space` removed, once per position.

    The three positions a `ByteLevel` block can appear in: the top-level
    `pre_tokenizer`, the second step of a `Sequence`, and the `decoder`.
    """
    base = json.loads(_no_op_byte_level_bpe(add_prefix_space=True).to_str())
    stripped = {k: v for k, v in base["pre_tokenizer"].items() if k != "add_prefix_space"}

    top_level = json.loads(json.dumps(base))
    top_level["pre_tokenizer"] = stripped

    sequence = json.loads(json.dumps(base))
    sequence["pre_tokenizer"] = {
        "type": "Sequence",
        "pretokenizers": [
            {"type": "Split", "pattern": {"Regex": " "}, "behavior": "Isolated", "invert": False},
            dict(stripped, use_regex=False),
        ],
    }

    decoder = json.loads(json.dumps(base))
    decoder["decoder"] = {k: v for k, v in base["decoder"].items() if k != "add_prefix_space"}

    return [("pre_tokenizer", top_level), ("sequence_step", sequence), ("decoder", decoder)]


def _add_prefix_space_refusals() -> list[dict]:
    """What `tokenizers` answers when a `ByteLevel` block omits `add_prefix_space`.

    These shapes cannot be cases: the reference refuses to build them, so there is
    no token stream to record. Recording the refusal instead keeps "the reference
    refuses this too" a measurement rather than a claim in a commit message.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    refusals = []
    for position, doc in _byte_level_documents_without_add_prefix_space():
        document = json.dumps(doc)
        try:
            Tokenizer.from_str(document)
        except Exception as exc:  # noqa: BLE001 - the refusal IS the measurement
            refusals.append({"position": position, "document": document, "error": str(exc)})
        else:
            raise AssertionError(
                f"tokenizers accepted a ByteLevel {position} declaring no add_prefix_space; "
                "issue #118's refusal rests on it refusing one")
    return refusals


def generate_bpe_no_op_settings() -> dict:
    """Each BPE setting that changes nothing, beside the baseline that proves it.

    `LoadBpe` used to refuse `continuing_subword_prefix: ""` and `dropout: 0.0`,
    and to crash on `end_of_word_suffix: ""`. Accepting them rests on a claim --
    that each of those values is a no-op -- which a load test cannot make: a
    file that loads without throwing proves only that nothing was thrown. So
    each setting is recorded here against a baseline built from the same
    vocabulary and merges with the setting absent, and the equality of the two
    token streams is the evidence.

    The models are hand-built rather than vendored: the claim is about a value
    in the file, not about a particular model, and a 7-entry vocabulary keeps
    the whole corpus readable where GPT-2's would add two megabytes of noise.

    `end_of_word_suffix: "</w>"` is here as a contrast, not as a no-op: the
    same vocabulary tokenizes differently under it, which is what makes the
    empty case's equality a measurement rather than a property of a model too
    small to notice. NO_OP_VOCAB's 'a</w>', 'b</w>' and 'c</w>' entries exist
    so no symbol is ever dropped for want of a vocabulary entry under that
    contrast -- the model declares no unk_token, and a dropped symbol would
    make the contrast a test of that instead.
    """
    carried = _no_op_models()
    cases = []
    for name, _, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases),
                "model": name,
                "text": text,
                "tokens": enc.tokens,
                "ids": enc.ids,
            })
    return {
        "metadata": {
            "algorithm": "BPE model settings that change nothing, and the ByteLevel field with no default",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: a 7-entry classic BPE and a 4-entry byte-level BPE, both defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "tokenizer_json": tokenizer.to_str()}
                for name, declares, tokenizer, _ in carried
            },
            # Read as: this model's token streams equal that model's, text for text.
            "no_op_pairs": [
                {"case": "end_of_word_suffix_empty", "baseline": "baseline"},
                {"case": "continuing_subword_prefix_empty", "baseline": "baseline"},
                {"case": "dropout_zero", "baseline": "baseline"},
            ],
            # And as: these two differ somewhere, which is what makes the equalities
            # above worth asserting.
            "contrast_pairs": [
                {"case": "end_of_word_suffix_marker", "baseline": "baseline"},
                {"case": "byte_level_add_prefix_space_true",
                 "baseline": "byte_level_add_prefix_space_false"},
            ],
            "add_prefix_space_refusals": _add_prefix_space_refusals(),
            "count": len(cases),
        },
        "cases": cases,
    }


# The same four flags over a WordPiece model that has a normalizer, named cases;
# see generate_wordpiece_added_tokens's docstring for why each one is here.
WORDPIECE_ADDED_TOKEN_TEXTS = [
    ("raw_entry_matches_its_own_casing", "the [CLS] cat"),
    ("raw_entry_ignores_the_lowercased_spelling", "the [cls] cat"),
    ("normalized_entry_matches_the_declared_casing", "the <MASK> cat"),
    ("normalized_entry_matches_the_lowercased_spelling", "the <mask> cat"),
    ("special_and_normalized_matches_the_declared_casing", "the [SEP] cat"),
    ("special_and_normalized_matches_the_lowercased_spelling", "the [sep] cat"),
    ("the_raw_pass_wins_over_a_normalized_match_further_left", "the A<R> cat"),
    ("a_normalized_match_stands_where_no_raw_one_does", "the a<r> cat"),
    ("lstrip_absorbs_the_space", "the <L> cat"),
    ("lstrip_absorbs_every_contiguous_space", "the  <L>  cat"),
    ("lstrip_absorbs_a_tab", "the\t<L>"),
    ("lstrip_absorbs_a_no_break_space", "the\u00a0<L>"),
    ("lstrip_stops_at_punctuation", "the. <L>"),
    ("lstrip_with_nothing_to_absorb", "the<L>cat"),
    ("lstrip_at_the_start_of_the_text", "<L> the"),
    ("lstrip_at_the_end_of_the_text", "the <L>"),
    ("rstrip_absorbs_the_space", "the <W> cat"),
    ("rstrip_absorbs_every_contiguous_space", "the  <W>  cat"),
    ("rstrip_with_nothing_to_absorb", "the<W>cat"),
    ("rstrip_at_the_start_of_the_text", "<W> the"),
    ("rstrip_at_the_end_of_the_text", "the <W>"),
    ("single_word_between_spaces", "the <S> cat"),
    ("single_word_between_full_stops", ".<S>."),
    ("single_word_between_hyphens", "-<S>-"),
    ("single_word_blocked_by_digits", "1<S>1"),
    ("single_word_blocked_by_underscores", "_<S>_"),
    ("single_word_blocked_by_accented_letters", "é<S>é"),
    ("single_word_blocked_by_letters", "the<S>cat"),
    ("single_word_is_the_whole_text", "<S>"),
]


def generate_wordpiece_added_tokens() -> dict:
    """A lowercasing WordPiece model with an added_tokens table that uses the flags.

    No other committed WordPiece corpus adds a token at all, so this is the only
    replayed evidence that the table is read and applied. The whole
    `tokenizer.json` rides in the metadata, as the BPE flag corpus does.

    This is the same four flags as the BPE corpus, over a model that has a
    normalizer, which is what BPE structurally cannot show: `normalized`
    decides which of two passes an entry runs in, and only a tokenizer that
    normalizes anything can tell the passes apart. WORDPIECE_ADDED_TOKEN_TEXTS
    is named cases, because each one is here for a reason:

    * a raw entry ([CLS], normalized=false) is matched against the
      un-lowercased text and emits its own casing;
    * a normalized entry (<MASK>) has its own content lowercased and is
      matched against the lowercased text, so both spellings match and both
      emit <mask>;
    * [SEP] is special *and* normalized, which every file add_special_tokens
      wrote makes look impossible -- it sets normalized = !special. It is the
      case that proves the discriminator is `normalized`, not `special`;
    * <R> (raw) and A<R> (normalized) overlap, with the normalized one
      starting further left. HuggingFace splits on the raw trie first and
      runs the normalized one over what is left, so the raw entry wins -- an
      outcome a single merged leftmost-wins scan cannot produce.

    Plus the strip and single_word shapes from the BPE corpus, with lstrip on
    a raw entry and rstrip on a normalized one so both passes carry a strip.

    Note for anyone regenerating: `tokenizers` refuses a tokenizer.json that
    omits `normalized`, so every entry states it. The absent-field default is
    therefore a decision this library makes rather than a behaviour it
    measured, and a C# unit test covers it instead.
    """
    from tokenizers import AddedToken  # noqa: PLC0415

    vocab = {token: index for index, token in enumerate(WORDPIECE_VOCAB)}
    tokenizer = _wordpiece_tokenizer(vocab, lowercase=True)
    tokenizer.add_tokens([
        AddedToken("[CLS]", special=True),
        AddedToken("<MASK>"),
        AddedToken("[SEP]", special=True, normalized=True),
        AddedToken("<R>", special=True),
        AddedToken("A<R>"),
        AddedToken("<L>", lstrip=True, special=True),
        AddedToken("<W>", rstrip=True),
        AddedToken("<S>", single_word=True),
    ])

    cases = []
    for i, (name, text) in enumerate(WORDPIECE_ADDED_TOKEN_TEXTS):
        enc = tokenizer.encode(text)
        cases.append({
            "id": i,
            "name": name,
            "text": text,
            "tokens": enc.tokens,
            "ids": enc.ids,
        })
    return {
        "metadata": {
            "algorithm": "WordPiece with added tokens and a Lowercase normalizer",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "reference_calls": [
                "tokenizers.Tokenizer(WordPiece(...)) with normalizers.Lowercase, then .add_tokens and .encode",
            ],
            "tokenizer_json": tokenizer.to_str(),
            "unk_token": UNK_TOKEN,
            "count": len(cases),
        },
        "cases": cases,
    }


# --- fuse_unk (issue #119) ----------------------------------------------------

# Z is in none of these vocabularies, which is what makes it a run when repeated.
_FUSE_VOCAB = {UNK_TOKEN: 0, "a": 1, "b": 2, "ab": 3}
_FUSE_MERGES = [("a", "b")]

# A merge whose LEFT side is the unknown token; see _fuse_unk_models's docstring.
_FUSE_MERGE_VOCAB = {UNK_TOKEN: 0, "a": 1, UNK_TOKEN + "a": 2}
_FUSE_MERGE_MERGES = [(UNK_TOKEN, "a")]

# The unknown token is ALSO a covered character; see _fuse_unk_models's docstring.
_FUSE_COVERED_UNK_VOCAB = {"q": 0, "a": 1}

# D7, the end-of-word suffix's own trap; see _fuse_unk_models's docstring.
_FUSE_EOW_SUFFIX = "</w>"
_FUSE_EOW_VOCAB = {UNK_TOKEN: 0, "a": 1, "a" + _FUSE_EOW_SUFFIX: 2, "Z": 3}


def _fuse_unk_model(vocab, merges, fuse, *, unk=UNK_TOKEN, byte_level=False, eow=None):
    """One tokenizer, built rather than trained, so the file is byte-stable.

    Every classic model declares Whitespace. A model declaring no pre-tokenizer
    at all does not split at all, which Lodestar reads as `NoPreTokenizer` since
    issue #122 and `bpe_no_split.json` measures; declaring the pre-tokenizer
    explicitly keeps this corpus about fuse_unk rather than about that split.
    """
    from tokenizers import Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    # tokenizers 0.23.1 raises TypeError on unk_token=None or
    # end_of_word_suffix=None: both are omitted rather than passed empty.
    kwargs = {"fuse_unk": fuse}
    if unk is not None:
        kwargs["unk_token"] = unk
    if eow is not None:
        kwargs["end_of_word_suffix"] = eow
    tokenizer = Tokenizer(models.BPE(dict(vocab), list(merges), **kwargs))
    tokenizer.pre_tokenizer = (pre_tokenizers.ByteLevel(add_prefix_space=False) if byte_level
                               else pre_tokenizers.Whitespace())
    return tokenizer


def _fuse_unk_models() -> list[tuple]:
    """(name, declares, fuse, tokenizer, texts) for every shape the spec decided on.

    _FUSE_MERGE_VOCAB states a merge whose LEFT side is the unknown token.
    Training never produces this; it is stated so that "does fusing happen
    before or after merging" has an answer a test can read. Fused, "ZZa"
    merges to [UNK]a; unfused it cannot, because the second [UNK] sits between
    the first and the a.

    _FUSE_COVERED_UNK_VOCAB makes the unknown token ALSO a covered character.
    This is the only shape that tells "the previous symbol was substituted"
    from "the previous id equals the unknown id", and the two disagree: "qZ"
    does not fuse, "ZZ" does. It uses a letter, not punctuation: the
    pre-tokenizer isolates punctuation from letters, so "?" and "Z" would land
    in different pieces and never meet, and the trap needs both characters
    inside one piece.

    _FUSE_EOW_VOCAB is D7's trap: the end-of-word suffix is appended to a
    piece's last code point BEFORE the vocabulary lookup, and only at the last
    position -- there is no fallback to the bare form there. "a" is covered
    both bare (the form looked up everywhere but the last position) and
    suffixed (the form looked up at the last position), so a run ending on a
    covered character still resolves and does not fuse. "Z" is the
    distinguishing case: covered bare, but "Z</w>" is deliberately absent, so a
    "Z" in last position is uncovered and substituted even though the same
    character is a real token everywhere else in the piece -- an
    implementation that fell back to the bare lookup at the last position
    would resolve it instead and never show the difference. Y is covered in
    neither form, so a run it starts still extends across a "Z" the suffix
    turns uncovered.

    plain_texts covers a run in the middle, at each end, the whole text, a
    single unknown (where the two flags must agree), two runs split by a
    covered character, an alternation (no run at all), and an astral run.
    split_texts covers the same idea across a piece boundary: without a
    pre-tokenizer the space is itself uncovered, so "aZ Za" is one run of
    three; with Whitespace it is two pieces and must not fuse across.
    across_split gets only split_texts, never plain_texts: none of the plain
    ones contains a space, so Whitespace makes each one a single piece and the
    run fuses inside it exactly as it does with no pre-tokenizer, which would
    report `differs=True` for a reason that has nothing to do with boundaries.
    """
    from tokenizers import pre_tokenizers  # noqa: PLC0415

    byte_vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}

    plain_texts = ["aZZZa", "ZZa", "aZZ", "ZZZ", "aZa", "ZZaZZ", "ZaZaZ", "a\U0001F600\U0001F601a"]
    split_texts = ["aZ Za", "Z a Z"]

    models_out = []
    for fuse in (False, True):
        suffix = "fused" if fuse else "unfused"
        models_out.append((
            f"in_piece_{suffix}", "a run inside a single piece", fuse,
            _fuse_unk_model(_FUSE_VOCAB, _FUSE_MERGES, fuse), plain_texts))
        # Only the texts that HAVE a boundary; see this function's docstring.
        models_out.append((
            f"across_split_{suffix}", "a run interrupted by a piece boundary", fuse,
            _fuse_unk_model(_FUSE_VOCAB, _FUSE_MERGES, fuse), split_texts))
        models_out.append((
            f"unk_merge_{suffix}", "a merge whose left side is the unknown token", fuse,
            _fuse_unk_model(_FUSE_MERGE_VOCAB, _FUSE_MERGE_MERGES, fuse), ["ZZa", "Za", "ZZZa"]))
        models_out.append((
            f"covered_unk_{suffix}", "an unknown token that is also a covered character", fuse,
            _fuse_unk_model(_FUSE_COVERED_UNK_VOCAB, [], fuse, unk="q"),
            ["qZ", "Zq", "qq", "ZZ", "qZa", "aqZ"]))
        models_out.append((
            f"no_unk_{suffix}", "fuse_unk with no unknown token declared", fuse,
            _fuse_unk_model(_FUSE_VOCAB, _FUSE_MERGES, fuse, unk=None), ["aZZa", "ZZZ"]))
        models_out.append((
            f"byte_level_{suffix}", "byte-level, where no character is uncovered", fuse,
            _fuse_unk_model(byte_vocab, [], fuse, unk=None, byte_level=True),
            ["a\U0001F600b", "ab"]))
        models_out.append((
            f"end_of_word_{suffix}",
            "a character covered bare but not suffixed, where the end-of-word suffix decides"
            " the last-position lookup rather than the character's own coverage (D7)", fuse,
            _fuse_unk_model(_FUSE_EOW_VOCAB, [], fuse, eow=_FUSE_EOW_SUFFIX),
            ["aYZ", "aZZ", "Za", "aZ"]))
    return models_out


def generate_bpe_fuse_unk() -> dict:
    """Every fuse_unk shape, each recorded with the flag off and on."""
    carried = _fuse_unk_models()
    cases = []
    for name, _, _, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases),
                "model": name,
                "text": text,
                "tokens": enc.tokens,
                "ids": enc.ids,
            })

    # Read as: turning the flag on changes this model's stream, or does not.
    # The `differs` column is measured below rather than asserted here.
    streams = {}
    for case in cases:
        streams.setdefault(case["model"], {})[case["text"]] = case["tokens"]
    pairs = []
    for name, _, fuse, _, _ in carried:
        if not fuse:
            continue
        unfused = name.replace("_fused", "_unfused")
        differs = any(streams[name][t] != streams[unfused][t] for t in streams[name])
        pairs.append({"fused": name, "unfused": unfused, "differs": differs})

    return {
        "metadata": {
            "algorithm": "BPE fuse_unk",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: six classic BPE shapes and one byte-level, all defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "fuse_unk": fuse, "tokenizer_json": tokenizer.to_str()}
                for name, declares, fuse, tokenizer, _ in carried
            },
            "fuse_pairs": pairs,
            "count": len(cases),
        },
        "cases": cases,
    }


# --- added tokens are not vocabulary entries (issue #130) ---------------------

# Q is the added token and is deliberately absent from this vocabulary; Z is
# absent from everything, so it is uncovered in every model here.
_COVERAGE_VOCAB = {UNK_TOKEN: 0, "a": 1, "b": 2, "ab": 3}
_COVERAGE_MERGES = [("a", "b")]

# The ignore_merges pair; see _added_coverage_models's docstring for "!!".
_IGNORE_MERGES_VOCAB_WITHOUT_BANG = {UNK_TOKEN: 0, "a": 1, "!": 2}
_IGNORE_MERGES_VOCAB_WITH_BANG = {UNK_TOKEN: 0, "a": 1, "!": 2, "!!": 3}


def _added_coverage_model(single_word):
    """One tokenizer whose added token is not in model.vocab."""
    from tokenizers import AddedToken, Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    tokenizer = Tokenizer(models.BPE(
        dict(_COVERAGE_VOCAB), list(_COVERAGE_MERGES), unk_token=UNK_TOKEN))
    tokenizer.pre_tokenizer = pre_tokenizers.Whitespace()
    tokenizer.add_tokens([AddedToken("Q", single_word=single_word, normalized=False)])
    return tokenizer


def _added_coverage_ignore_merges_model(vocab):
    """One tokenizer with ignore_merges on: "!!" is an added token, in model.vocab or not."""
    from tokenizers import AddedToken, Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    model = models.BPE(dict(vocab), [], unk_token=UNK_TOKEN)
    model.ignore_merges = True
    tokenizer = Tokenizer(model)
    tokenizer.pre_tokenizer = pre_tokenizers.Whitespace()
    tokenizer.add_tokens([AddedToken("!!", single_word=True, normalized=False)])
    return tokenizer


def _added_coverage_models() -> list[tuple]:
    """(name, declares, single_word, tokenizer, texts).

    texts: aQa and ZQZ put the added token inside a word, where single_word
    decides whether the scanner may match it. Q and "a Q a" put it on its own,
    where the scanner matches under either flag and both sides already agree.
    aQ ends a piece with it, and QQ doubles it.

    ignore_merges_texts: "!!" is the added token. a!! is split by Whitespace
    into "a" / "!!"; single_word declines the scanner on "a" (a word
    character), so "!!" reaches the ignore_merges shortcut while still being
    an added token's exact content. One vocabulary omits "!!" from
    model.vocab, the control carries it too so the shortcut can fire from
    there instead of from the fold. Bare "!!" reaches the scanner directly
    under either vocabulary, and is carried as the non-discriminating control.
    """
    texts = ["aQa", "ZQZ", "QQ", "Q", "a Q a", "aQ"]
    ignore_merges_texts = ["a!!", "!!"]
    return [
        ("single_word", "an added token absent from model.vocab, matched only on its own",
         True, _added_coverage_model(True), texts),
        ("any_position", "the same added token, matchable inside a word",
         False, _added_coverage_model(False), texts),
        ("ignore_merges_added_token_only",
         "ignore_merges on; the added token '!!' is absent from model.vocab, "
         "so the whole-piece shortcut cannot see it",
         True, _added_coverage_ignore_merges_model(_IGNORE_MERGES_VOCAB_WITHOUT_BANG),
         ignore_merges_texts),
        ("ignore_merges_added_token_in_vocab",
         "the control: '!!' is in model.vocab too, so the shortcut fires from there",
         True, _added_coverage_ignore_merges_model(_IGNORE_MERGES_VOCAB_WITH_BANG),
         ignore_merges_texts),
    ]


@contextlib.contextmanager
def _rust_stderr_captured():
    """Hold file descriptor 2 open to a temp file, yielding the file itself.

    A Rust panic is printed by the default panic hook, which writes to fd 2
    directly and before pyo3 turns the panic into a PanicException. Nothing
    above that layer can see it: contextlib.redirect_stderr rebinds
    sys.stderr, which the hook never consults. Only dup2 reaches it.
    """
    saved = os.dup(2)
    try:
        with tempfile.TemporaryFile(mode="w+b") as sink:
            os.dup2(sink.fileno(), 2)
            try:
                yield sink
            finally:
                os.dup2(saved, 2)
    finally:
        os.close(saved)


def _load_recording_panic(document: str):
    """(tokenizer, error) -- the reference's verdict on one document, panic and all.

    The error is None when it loaded. Whatever the panic hook wrote to fd 2 is
    dropped when a refusal came with it, and re-emitted when none did: the known
    panic is expected, and suppressing it must not hide the next one.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    with _rust_stderr_captured() as sink:
        try:
            loaded, error = Tokenizer.from_str(document), None
        except BaseException as exc:  # noqa: BLE001 - the refusal IS the measurement
            # BaseException: a Rust panic surfaces as unimportable
            # pyo3_runtime.PanicException; Ctrl-C/SystemExit are re-raised (S5754).
            if isinstance(exc, (KeyboardInterrupt, SystemExit)):
                raise
            loaded, error = None, f"{type(exc).__name__}: {exc}"
        sink.seek(0)
        printed = sink.read().decode("utf-8", "replace")

    if printed and error is None:
        sys.stderr.write(printed)
    return loaded, error


def _added_coverage_refusals() -> list[dict]:
    """Three shapes the reference refuses, recorded with what it said and when.

    Two never produce a tokenizer at all — they fail while the document is
    read. The third produces a working one that answers token_to_id and
    encodes covered text, and refuses only an input needing a substitution.
    None of the three can be an ordinary case, because two have no token
    stream and the third's is beside the point; recording the refusal is what
    makes "the reference refuses this too" a measurement rather than a claim.

    unk_only_in_added_tokens: the unknown token exists only in added_tokens.
    This one LOADS -- the reference defers the check to encode, and raises
    only on text that needs a substitution. "ab" encodes fine; "aZb" does not.

    merge_names_an_added_token: a merge names a token that only added_tokens
    declares. Refused while the document is read.

    merge_result_missing: a merge whose two sides are present but whose
    result is not. The reference PANICS while reading rather than raising;
    the panic is recorded as what it is, and Lodestar refuses in its own words
    (D6). Its stderr is captured rather than let through: the hook prints on
    every run, and a line that is always there is one nobody reads (#214).

    The reference does not refuse all three at the same moment: two fail
    while the document is being read, the first loads and fails from encode
    -- and only on text that needs a substitution, which is why each shape
    carries the text that provokes it.
    """
    def document(vocab, merges, unk, added):
        return json.dumps({
            "version": "1.0", "truncation": None, "padding": None,
            "added_tokens": added,
            "normalizer": None, "pre_tokenizer": {"type": "Whitespace"},
            "post_processor": None, "decoder": None,
            "model": {
                "type": "BPE", "dropout": None, "unk_token": unk,
                "continuing_subword_prefix": None, "end_of_word_suffix": None,
                "fuse_unk": False, "byte_fallback": False, "ignore_merges": False,
                "vocab": vocab, "merges": merges,
            },
        })

    added_q = [{"id": 2, "content": "Q", "single_word": True, "lstrip": False,
                "rstrip": False, "normalized": False, "special": False}]

    # (shape, document, the text that provokes the failure if loading did not)
    # -- see this function's docstring for what each shape is.
    shapes = [
        ("unk_only_in_added_tokens",
         document({"a": 0, "b": 1}, [], UNK_TOKEN_LOWER,
                  [{"id": 2, "content": UNK_TOKEN_LOWER, "single_word": False, "lstrip": False,
                    "rstrip": False, "normalized": False, "special": True}]),
         "aZb"),
        ("merge_names_an_added_token",
         document({UNK_TOKEN: 0, "a": 1, "Qa": 3}, [["Q", "a"]], UNK_TOKEN, added_q),
         "Qa"),
        ("merge_result_missing",
         document({"a": 0, "b": 1}, [["a", "b"]], None, []),
         "ab"),
    ]

    refusals = []
    for shape, doc, provoking_text in shapes:
        tokenizer, refused_at_load = _load_recording_panic(doc)
        if refused_at_load is not None:
            refusals.append({"shape": shape, "document": doc, "raised_by": "load",
                             "text": None, "error": refused_at_load})
            continue
        try:
            tokenizer.encode(provoking_text)
        except BaseException as exc:  # noqa: BLE001
            # Same reasoning as _load_recording_panic's handler.
            if isinstance(exc, (KeyboardInterrupt, SystemExit)):
                raise
            refusals.append({"shape": shape, "document": doc, "raised_by": "encode",
                             "text": provoking_text, "error": f"{type(exc).__name__}: {exc}"})
        else:
            raise AssertionError(
                f"tokenizers accepted {shape} and encoded {provoking_text!r} without complaint; "
                "issue #130's refusal rests on it refusing one")
    return refusals


def generate_bpe_added_token_coverage() -> dict:
    """An added token absent from model.vocab, with the scanner allowed and denied."""
    carried = _added_coverage_models()
    cases = []
    for name, _, _, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({"id": len(cases), "model": name, "text": text,
                          "tokens": enc.tokens, "ids": enc.ids})

    return {
        "metadata": {
            "algorithm": "BPE added-token coverage",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: one 4-entry classic BPE, twice, defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "single_word": single_word,
                       "tokenizer_json": tokenizer.to_str()}
                for name, declares, single_word, tokenizer, _ in carried
            },
            "refusals": _added_coverage_refusals(),
            "count": len(cases),
        },
        "cases": cases,
    }


# --- continuing_subword_prefix (issue #120) -----------------------------------

_PREFIX = "##"
_PREFIX_EOW = "</w>"


def _prefix_model(vocab, merges, *, prefix=_PREFIX, eow=None, unk=None):
    """One tokenizer, built rather than trained, so the file is byte-stable."""
    from tokenizers import Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    kwargs = {}
    if prefix is not None:
        kwargs["continuing_subword_prefix"] = prefix
    if eow is not None:
        kwargs["end_of_word_suffix"] = eow
    if unk is not None:
        kwargs["unk_token"] = unk
    tokenizer = Tokenizer(models.BPE(dict(vocab), list(merges), **kwargs))
    tokenizer.pre_tokenizer = pre_tokenizers.Whitespace()
    return tokenizer


def _prefix_models() -> list[tuple]:
    """(name, declares, tokenizer, texts), one per case nothing else distinguishes."""
    bare = {"a": 0, "b": 1, _PREFIX + "a": 2, _PREFIX + "b": 3}

    return [
        ("base", "a prefix, and a piece long enough to need it",
         _prefix_model(bare, []), ["ab", "a", "b"]),
        # Two words is the only shape that tells per-piece from per-text: the
        # first symbol of the SECOND word is bare.
        ("two_pieces", "two pieces, so the second one's first symbol is bare",
         _prefix_model(bare, []), ["ab ab", "a b", "ab a"]),
        # b exists bare and ##b does not. There is no fallback, so the character
        # is dropped -- or substituted where an unknown token exists.
        ("no_prefixed_form", "a character whose prefixed form is absent, and no unknown token",
         _prefix_model({"a": 0, "b": 1}, []), ["ab", "a", "ba"]),
        ("no_prefixed_form_unk", "the same, with an unknown token to substitute",
         _prefix_model({"a": 0, "b": 1, UNK_TOKEN: 2}, [], unk=UNK_TOKEN), ["ab", "a", "ba"]),
        # The merge result is the stripped concatenation. Only `ab` is present,
        # so a plain concatenation would look for `a##b` and fail.
        ("merge_stripped_result", "a merge whose stripped result alone is in the vocabulary",
         _prefix_model({"a": 0, "b": 1, _PREFIX + "b": 2, "ab": 3}, [("a", _PREFIX + "b")]),
         ["ab", "aba"]),
        # Both sides prefixed: the left keeps its prefix, only the right loses
        # one. "Both lose it" would need `bc` and is the plausible wrong reading.
        ("merge_both_prefixed", "a merge whose two sides both carry the prefix",
         _prefix_model({"a": 0, _PREFIX + "b": 1, _PREFIX + "c": 2, _PREFIX + "bc": 3},
                       [(_PREFIX + "b", _PREFIX + "c")]),
         ["abc", "ab"]),
        # ("a", "##b</w>") must give "ab</w>": strip the prefix, keep the suffix.
        # Stripping both, or neither, looks for a token that is absent.
        ("merge_suffixed_right", "a merge whose right side carries the prefix and the suffix at once",
         _prefix_model({"a": 0, _PREFIX + "b" + _PREFIX_EOW: 1, "ab" + _PREFIX_EOW: 2,
                        "a" + _PREFIX_EOW: 3},
                       [("a", _PREFIX + "b" + _PREFIX_EOW)], eow=_PREFIX_EOW),
         ["ab", "a"]),
        # Prefix and suffix compose, prefix then character then suffix.
        ("prefix_and_suffix", "a prefix and an end-of-word suffix on the same symbol",
         _prefix_model({"a": 0, "b": 1, _PREFIX + "b": 2, "b" + _PREFIX_EOW: 3,
                        _PREFIX + "b" + _PREFIX_EOW: 4, "a" + _PREFIX_EOW: 5},
                       [], eow=_PREFIX_EOW),
         ["ab", "a", "b"]),
        # An empty prefix must give the same stream as none at all. This is the
        # untouched path's own regression proof.
        ("empty_prefix", "an empty prefix, which prefixes nothing",
         _prefix_model(bare, [], prefix=""), ["ab", "a b"]),
        ("no_prefix", "no prefix declared, the baseline the empty one must equal",
         _prefix_model(bare, [], prefix=None), ["ab", "a b"]),
    ]


def _prefix_refusals() -> list[dict]:
    """The shape the reference refuses to build, with what it said.

    A merge whose two sides carry the prefix but whose CONCATENATED form is in
    the vocabulary instead of the stripped one. There is no token stream to
    record -- the reference never produces a tokenizer -- so recording the
    refusal is what makes "the reference refuses this too" a measurement.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    document = json.dumps({
        "version": "1.0", "truncation": None, "padding": None, "added_tokens": [],
        "normalizer": None, "pre_tokenizer": {"type": "Whitespace"},
        "post_processor": None, "decoder": None,
        "model": {
            "type": "BPE", "dropout": None, "unk_token": None,
            "continuing_subword_prefix": _PREFIX, "end_of_word_suffix": None,
            "fuse_unk": False, "byte_fallback": False, "ignore_merges": False,
            "vocab": {"a": 0, _PREFIX + "b": 1, _PREFIX + "c": 2, _PREFIX + "b" + _PREFIX + "c": 3},
            "merges": [[_PREFIX + "b", _PREFIX + "c"]],
        },
    })

    try:
        Tokenizer.from_str(document)
    except BaseException as exc:  # noqa: BLE001 - the refusal IS the measurement
        # Same reasoning as _load_recording_panic's handler.
        if isinstance(exc, (KeyboardInterrupt, SystemExit)):
            raise
        return [{"shape": "merge_result_not_stripped", "document": document,
                 "error": f"{type(exc).__name__}: {exc}"}]
    raise AssertionError(
        "tokenizers accepted a merge whose result is the concatenated form; "
        "issue #120's stripped-result rule rests on it refusing one")


def generate_bpe_continuing_prefix() -> dict:
    """Every continuing_subword_prefix shape, and the one the reference refuses."""
    carried = _prefix_models()
    cases = []
    for name, _, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({"id": len(cases), "model": name, "text": text,
                          "tokens": enc.tokens, "ids": enc.ids})

    return {
        "metadata": {
            "algorithm": "BPE continuing_subword_prefix",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: ten classic BPE shapes, defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "tokenizer_json": tokenizer.to_str()}
                for name, declares, tokenizer, _ in carried
            },
            "refusals": _prefix_refusals(),
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Split + ByteLevel Sequence, both patterns (issue #143) -------------------

# Llama-3's own Split pattern; mirrors BpePatterns.Llama3 in C#, repeated
# rather than imported so the generator does not depend on the library under test.
_SEQ_SPLIT = (
    r"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}{1,3}"
    r"| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+"
)


def _sequence_split_model(use_regex):
    """A byte-level BPE behind Sequence[Split(Llama-3), ByteLevel].

    add_prefix_space is off throughout, deliberately: it prepends a space to
    every piece the Split step produces, so with it on each case here would
    measure that rule on top of this one and none would discriminate. It is
    bpe_prefix_space.json's subject instead. ADR 0022 section 10 recorded the
    same reasoning when bpe_added_token_flags.json was generated with it off.

    The merges exist so the split is observable in the tokens and not only in the
    pieces: a merge never crosses a piece boundary, so "'ai" can only be reached
    when the apostrophe and the letters share a piece.

    They cover 'a/'ai and 'h/'hu, so of the five texts whose PIECES differ
    between the two models, two also differ in TOKENS -- "j'ai vu l'ami d'Anne"
    and "aujourd'hui". "C'est l'été", "O'Brien and D'Angelo" and "rock'n'roll"
    differ in pieces alone, there being no merge starting 'e, 'B, 'n or 'r.

    That is deliberate rather than a gap. The pieces are the evidence, and all
    five carry it; the tokens exist only to prove the pieces reach the merge
    loop, which two texts establish as well as five would. Merges added for 'e,
    'B, 'n and 'r would exist solely to make an established proof redundant.
    """
    from tokenizers import Tokenizer, models, pre_tokenizers, decoders, Regex  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    merges = []
    for left, right in (("'", "a"), ("'a", "i"), ("a", "i"), ("'", "h"), ("'h", "u")):
        merged = left + right
        if merged not in vocab:
            vocab[merged] = len(vocab)
        merges.append((left, right))

    tokenizer = Tokenizer(models.BPE(vocab, merges))
    tokenizer.pre_tokenizer = pre_tokenizers.Sequence([
        pre_tokenizers.Split(Regex(_SEQ_SPLIT), behavior="isolated"),
        pre_tokenizers.ByteLevel(add_prefix_space=False, use_regex=use_regex),
    ])
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def _sequence_split_models() -> list[tuple]:
    """(name, declares, tokenizer, texts) — one per side of the divergence.

    texts opens with the divergence itself, on five shapes of one cause rather
    than five spellings of one shape: elision before a vowel, before an h, an
    accented letter after the apostrophe, a capitalised name, and twice inside
    one word.

    "it's fine", "don't" and "the 'quoted' word" are cases that must NOT move:
    without them the corpus proves something changed, not that the right thing
    changed -- a fix that split on every apostrophe would pass the group above
    and fail here. "hello123 don't" is a fourth must-not-move case covering a
    reason the other three do not: Llama-3's pattern already parts letters
    from digits and already isolates 't, so the second pass changes nothing
    here even though it changes every elision above.
    """
    texts = [
        "j'ai vu l'ami d'Anne",
        "aujourd'hui",
        "C'est l'été",
        "O'Brien and D'Angelo",
        "rock'n'roll",
        "it's fine",
        "don't",
        "the 'quoted' word",
        "hello123 don't",
    ]
    return [
        ("use_regex_on",
         "a Sequence whose ByteLevel step splits again, which is the default",
         _sequence_split_model(True), texts),
        ("use_regex_off",
         "the same Sequence with the second split turned off",
         _sequence_split_model(False), texts),
    ]


def generate_bpe_sequence_split() -> dict:
    """Both patterns of a Split + ByteLevel Sequence, pieces as well as tokens."""
    carried = _sequence_split_models()
    cases = []
    for name, _, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases),
                "model": name,
                "text": text,
                # The pre-tokenizer's own output, which is where the defect is.
                "pieces": [p for p, _span in tokenizer.pre_tokenizer.pre_tokenize_str(text)],
                "tokens": enc.tokens,
                "ids": enc.ids,
            })

    return {
        "metadata": {
            "algorithm": "BPE Sequence[Split, ByteLevel] pre-tokenization",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: the byte-level alphabet plus five merges, defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "tokenizer_json": tokenizer.to_str()}
                for name, declares, tokenizer, _ in carried
            },
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Split behavior and invert (issue #145) -----------------------------------

# Named in the Python constructor's spelling; see _split_behavior_models's
# docstring, and D6.
_SPLIT_BEHAVIORS = ["isolated", "removed", "merged_with_previous",
                    "merged_with_next", "contiguous"]

# Two patterns because one cannot separate every behavior; see
# _split_behavior_models's docstring.
_SPLIT_PATTERN = r"\w+"
_SPLIT_ADJACENT_PATTERN = "X"


def _split_behavior_model(pattern, behavior, invert):
    """One byte-level BPE behind Sequence[Split(pattern), ByteLevel].

    add_prefix_space is off throughout, deliberately: it prepends a space to
    every piece the Split step produces, so with it on each case here would
    measure that rule on top of this one. It is bpe_prefix_space.json's
    subject instead. ADR 0022 section 10 recorded the same reasoning.

    use_regex is off on the ByteLevel step so the Split step's arrangement
    reaches the model untouched -- with it on, GPT-2's pattern would re-split
    every piece and hide the behavior being measured.
    """
    from tokenizers import Tokenizer, models, pre_tokenizers, decoders, Regex  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    tokenizer = Tokenizer(models.BPE(vocab, []))
    tokenizer.pre_tokenizer = pre_tokenizers.Sequence([
        pre_tokenizers.Split(Regex(pattern), behavior=behavior, invert=invert),
        pre_tokenizers.ByteLevel(add_prefix_space=False, use_regex=False),
    ])
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def _split_behavior_texts():
    """The texts, boundaries as well as examples.

    They do NOT make all twenty models differ, and could not: invert is a no-op
    for isolated and contiguous, and it exchanges the two merge directions, so
    twelve of the pairs are equal by the reference's own rules rather than by a
    weakness here. What the set has to do is separate the five behaviors, which
    needs the adjacency case above for isolated against contiguous.
    """
    return [
        # The spec's D2 row: separates removed, merged_with_previous,
        # merged_with_next and their inversions from each other.
        "ab cd!",
        # D7's boundary rows, which are where an off-by-one in the segmentation
        # lands: fully matched, not matched at all, and gaps on both ends.
        "abc",
        "  ",
        " ab ",
        "",
        # Adjacent matches under the second pattern -- the only shape that
        # tells isolated from contiguous (D4).
        "aXXb",
    ]


def _split_behavior_models() -> list[tuple]:
    """(name, declares, pattern, behavior, invert, tokenizer, texts).

    Twenty models: five behaviors x invert x two patterns. _SPLIT_BEHAVIORS is
    named in the Python constructor's spelling; the file it serializes to uses
    PascalCase, which is what the C# loader reads (measured, spec D6).

    Two patterns, because one cannot separate every behavior. _SPLIT_PATTERN
    ("\\w+") leaves gaps in most texts, which is what tells removed and the two
    merge directions apart -- but it is greedy, so it never produces two
    ADJACENT matches, and isolated and contiguous differ nowhere else (spec
    D4). _SPLIT_ADJACENT_PATTERN ("X") over "aXXb" is that shape, and it is
    the only reason the second pattern exists.
    """
    carried = []
    for behavior in _SPLIT_BEHAVIORS:
        for invert in (False, True):
            for pattern, tag in ((_SPLIT_PATTERN, ""), (_SPLIT_ADJACENT_PATTERN, "_adjacent")):
                name = f"{behavior}{'_inverted' if invert else ''}{tag}"
                carried.append((
                    name,
                    f"behavior {behavior}, invert {invert}, pattern {pattern!r}",
                    pattern, behavior, invert,
                    _split_behavior_model(pattern, behavior, invert),
                    _split_behavior_texts(),
                ))
    return carried


def _split_behavior_refusals() -> list[dict]:
    """The three Split-step shapes the reference refuses to build."""
    from tokenizers import Tokenizer  # noqa: PLC0415

    def document(step):
        return json.dumps({
            "version": "1.0", "truncation": None, "padding": None, "added_tokens": [],
            "normalizer": None,
            "pre_tokenizer": {"type": "Sequence", "pretokenizers": [
                step, {"type": "ByteLevel", "add_prefix_space": False,
                       "trim_offsets": True, "use_regex": False}]},
            "post_processor": None, "decoder": None,
            "model": {"type": "BPE", "dropout": None, "unk_token": None,
                      "continuing_subword_prefix": None, "end_of_word_suffix": None,
                      "fuse_unk": False, "byte_fallback": False, "ignore_merges": False,
                      "vocab": {"a": 0}, "merges": []},
        })

    full = {"type": "Split", "pattern": {"Regex": _SPLIT_PATTERN},
            "behavior": "Isolated", "invert": False}
    shapes = [
        ("behavior_absent", {k: v for k, v in full.items() if k != "behavior"}),
        ("invert_absent", {k: v for k, v in full.items() if k != "invert"}),
        ("behavior_unknown", {**full, "behavior": "Nonsense"}),
    ]

    refusals = []
    for shape, step in shapes:
        doc = document(step)
        try:
            Tokenizer.from_str(doc)
        except BaseException as exc:  # noqa: BLE001 - the refusal IS the measurement
            # Same reasoning as _load_recording_panic's handler.
            if isinstance(exc, (KeyboardInterrupt, SystemExit)):
                raise
            refusals.append({"shape": shape, "document": doc,
                             "error": f"{type(exc).__name__}: {exc}"})
            continue
        raise AssertionError(
            f"tokenizers accepted {shape}; issue #145's refusal of it rests on the reference refusing it")
    return refusals


# --- a merge pair listed twice (issue #160) -----------------------------------

# See _duplicate_merge_models's docstring for the a+b duplicate this backs.
_DUPLICATE_VOCAB = {"a": 0, "b": 1, "c": 2, "d": 3, "ab": 4, "bc": 5, "cd": 6}


def _duplicate_merge_document(merges) -> str:
    """A tokenizer.json written by hand, so a duplicate survives into the file.

    Round-tripping through Tokenizer.to_str() cannot be used here: the
    reference collapses a repeated pair while serializing, so the document it
    writes for the duplicate is byte-identical to the one it writes for
    last_kept. A corpus built that way hands the loader under test a file with
    no duplicate in it and passes while measuring nothing.
    """
    return json.dumps({
        "version": "1.0", "truncation": None, "padding": None, "added_tokens": [],
        "normalizer": None, "pre_tokenizer": {"type": "Whitespace"},
        "post_processor": None, "decoder": None,
        "model": {
            "type": "BPE", "dropout": None, "unk_token": None,
            "continuing_subword_prefix": None, "end_of_word_suffix": None,
            "fuse_unk": False, "byte_fallback": False, "ignore_merges": False,
            "vocab": dict(_DUPLICATE_VOCAB), "merges": [list(pair) for pair in merges],
        },
    })


def _duplicate_merge_models() -> list[tuple]:
    """(name, declares, document, tokenizer, texts) -- the duplicate and both readings.

    _DUPLICATE_VOCAB: a+b is listed at rank 0 AND rank 3. Keeping the first
    makes it merge before b+c; keeping the last makes it merge after. Nothing
    else distinguishes the two readings, and no committed corpus contained a
    duplicated pair before this.
    """
    from tokenizers import Tokenizer  # noqa: PLC0415

    first, second, third = ("a", "b"), ("b", "c"), ("c", "d")
    shapes = [
        ("duplicate", "a+b listed at rank 0 and again at rank 3",
         [first, second, third, first]),
        ("first_kept", "the same table with only the rank-0 occurrence",
         [first, second, third]),
        ("last_kept", "the same table with only the rank-3 occurrence",
         [second, third, first]),
    ]
    carried = []
    for name, declares, merges in shapes:
        document = _duplicate_merge_document(merges)
        carried.append((name, declares, document, Tokenizer.from_str(document),
                        ["abcd", "abc", "ab"]))
    return carried


def generate_bpe_duplicate_merge() -> dict:
    """Which occurrence of a repeated merge pair the reference keeps."""
    carried = _duplicate_merge_models()
    cases = []
    for name, _declares, _document, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({"id": len(cases), "model": name, "text": text,
                          "tokens": enc.tokens, "ids": enc.ids})

    return {
        "metadata": {
            "algorithm": "BPE merge table with a duplicated pair",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: seven entries and four merges, defined in tools/generate_oracles.py",
            # tokenizer_json is written here rather than by Tokenizer.to_str(),
            # which collapses the duplicate and would make this corpus vacuous.
            "models": {
                name: {"declares": declares, "tokenizer_json": document}
                for name, declares, document, _, _ in carried
            },
            # Read as: the duplicate's stream equals one of these two, and which
            # one it is is the whole measurement.
            "candidates": ["first_kept", "last_kept"],
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_bpe_split_behavior() -> dict:
    """Every Split behavior and invert, pieces as well as tokens."""
    carried = _split_behavior_models()
    cases = []
    for name, _declares, _pattern, _behavior, _invert, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases),
                "model": name,
                "text": text,
                # The pre-tokenizer's own output, which is where the behavior is.
                "pieces": [p for p, _span in tokenizer.pre_tokenizer.pre_tokenize_str(text)],
                "tokens": enc.tokens,
                "ids": enc.ids,
            })

    return {
        "metadata": {
            "algorithm": "BPE Sequence Split step behavior and invert",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": BYTE_LEVEL_NO_MERGES,
            "models": {
                name: {"declares": declares, "pattern": pattern, "behavior": behavior,
                       "invert": invert, "tokenizer_json": tokenizer.to_str()}
                for name, declares, pattern, behavior, invert, tokenizer, _ in carried
            },
            "refusals": _split_behavior_refusals(),
            "count": len(cases),
        },
        "cases": cases,
    }


# --- a pre-tokenizer that does not split (issue #122) -------------------------

# fuse_unk is on so the unsplit side comes out SHORTER than the split one --
# measured, 3 tokens against 4; with it off the pair still differs, 5 against 4.
_NO_SPLIT_VOCAB = {UNK_TOKEN: 0, "a": 1, "b": 2, "ab": 3}


def _no_split_classic(pre_tokenizer):
    """A classic BPE, built rather than trained, so the file is byte-stable."""
    from tokenizers import Tokenizer, models, pre_tokenizers  # noqa: PLC0415

    tokenizer = Tokenizer(models.BPE(
        dict(_NO_SPLIT_VOCAB), [("a", "b")], unk_token=UNK_TOKEN, fuse_unk=True))
    if pre_tokenizer is not None:
        tokenizer.pre_tokenizer = pre_tokenizer
    return tokenizer


def _no_split_byte_level(use_regex, add_prefix_space=False, added=None):
    """A byte-level BPE whose alphabet covers every byte, so nothing is unknown.

    The one merge spans a piece boundary on purpose: use_regex cuts "hello
    world" between the o and the space, so only the unsplit model can apply it.
    Without it both models emit one token per character and measure nothing.
    """
    from tokenizers import Tokenizer, models, pre_tokenizers, decoders  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    vocab["oĠ"] = len(vocab)
    tokenizer = Tokenizer(models.BPE(vocab, [("o", "Ġ")]))
    tokenizer.pre_tokenizer = pre_tokenizers.ByteLevel(
        add_prefix_space=add_prefix_space, use_regex=use_regex)
    tokenizer.decoder = decoders.ByteLevel()
    if added:
        tokenizer.add_tokens(added)
    return tokenizer


def _no_split_models() -> list[tuple]:
    """(name, declares, tokenizer, texts) -- one per thing no other model shows."""
    from tokenizers import AddedToken, pre_tokenizers  # noqa: PLC0415

    fuse_texts = ["aZ Za", "ab", "Z Z"]
    byte_texts = [HELLO_WORLD, "  leading and trailing  ", "hello world  again", "café \U0001f600"]
    added_texts = ["o o<sep>o o", "o o"]
    return [
        ("absent", "no pre_tokenizer at all -- the shape #122 found Lodestar mis-loading",
         _no_split_classic(None), fuse_texts),
        ("whitespace", "the classic Whitespace split, for the row above to differ from",
         _no_split_classic(pre_tokenizers.Whitespace()), fuse_texts),
        ("byte_level_no_regex", "ByteLevel with use_regex off -- refused before #122",
         _no_split_byte_level(False), byte_texts),
        ("byte_level_regex", "the same with it on, so the pair shows what the flag does",
         _no_split_byte_level(True), byte_texts),
        ("no_regex_prefix_space", "no split and add_prefix_space on -- one space, at the front",
         _no_split_byte_level(False, add_prefix_space=True), byte_texts),
        ("no_regex_added_token", "no split, with an added token the text spans",
         _no_split_byte_level(False, added=[AddedToken("<sep>", special=True)]),
         added_texts),
        ("regex_added_token", "the split counterpart the row above is measured against",
         _no_split_byte_level(True, added=[AddedToken("<sep>", special=True)]),
         added_texts),
    ]


def generate_bpe_no_split() -> dict:
    """What a pre-tokenizer that does not split produces, and what it decodes to."""
    carried = _no_split_models()
    cases = []
    for name, _declares, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases), "model": name, "text": text,
                "tokens": enc.tokens, "ids": enc.ids,
                # D5 is about the input coming back; a token list proves itself.
                "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            })

    return {
        "metadata": {
            "algorithm": "BPE with a pre-tokenizer that does not split",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": "hand-built: a 4-entry classic BPE and a byte-level one, defined in tools/generate_oracles.py",
            "models": {
                name: {"declares": declares, "tokenizer_json": tokenizer.to_str()}
                for name, declares, tokenizer, _ in carried
            },
            "count": len(cases),
        },
        "cases": cases,
    }


# --- add_prefix_space per Split piece (issue #122) ----------------------------

# A Regex, not a bare string: the loader reads both spellings since #167, but
# swapping this one would re-serialize the models and move a frozen corpus.
_PREFIX_SPACE_SPLIT = r"\|"


def _prefix_space_model(pre_split, add_prefix_space, use_regex):
    """A byte-level BPE with no merges, so every piece is one token per character.

    No merges on purpose: this corpus is about where a space is inserted, and a
    merge would fold that evidence into a token whose spelling hides it.
    """
    from tokenizers import Regex, Tokenizer, models, pre_tokenizers, decoders  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    tokenizer = Tokenizer(models.BPE(vocab, []))
    byte_level = pre_tokenizers.ByteLevel(
        add_prefix_space=add_prefix_space, use_regex=use_regex)
    tokenizer.pre_tokenizer = pre_tokenizers.Sequence([
        pre_tokenizers.Split(Regex(_PREFIX_SPACE_SPLIT), behavior="isolated", invert=False),
        byte_level,
    ]) if pre_split else byte_level
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def _prefix_space_models() -> list[tuple]:
    """(name, declares, tokenizer, texts) -- one per thing no other model shows."""
    # The last text has no "|": the four models with the space on agree on what it
    # DECODES to; their pieces still differ, on use_regex rather than on the split.
    texts = ["ab|cd", "a b|c d", "ab| cd", " ab|cd", "a| |b", "a|b|c|d", "no split here"]
    return [
        ("presplit_aps", "Sequence[Split, ByteLevel(aps on, use_regex off)] -- Llama-3's shape",
         _prefix_space_model(True, add_prefix_space=True, use_regex=False), texts),
        ("presplit_aps_regex", "the same with use_regex on, so both patterns and the space are measured together",
         _prefix_space_model(True, add_prefix_space=True, use_regex=True), texts),
        ("presplit_no_aps", "the same with aps off -- the control, and what every shipped model declares",
         _prefix_space_model(True, add_prefix_space=False, use_regex=False), texts),
        ("bare_aps", "a bare ByteLevel with aps on -- GPT-2's shape, which must not move",
         _prefix_space_model(False, add_prefix_space=True, use_regex=True), texts),
        ("no_split_aps", "a bare ByteLevel, aps on and use_regex off -- the no-split mode's boundary",
         _prefix_space_model(False, add_prefix_space=True, use_regex=False), texts),
    ]


def generate_bpe_prefix_space() -> dict:
    """Where add_prefix_space lands, per piece rather than per text."""
    carried = _prefix_space_models()
    cases = []
    for name, _declares, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases), "model": name, "text": text,
                # The pre-tokenizer's own output, which is where the space lands.
                "pieces": [p for p, _span in tokenizer.pre_tokenizer.pre_tokenize_str(text)],
                "tokens": enc.tokens, "ids": enc.ids,
                # The divergence survives Decode, which is how a user meets it.
                "decoded": tokenizer.decode(enc.ids, skip_special_tokens=False),
            })

    return {
        "metadata": {
            "algorithm": "BPE add_prefix_space placement",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": BYTE_LEVEL_NO_MERGES,
            "models": {
                name: {"declares": declares, "tokenizer_json": tokenizer.to_str()}
                for name, declares, tokenizer, _ in carried
            },
            "count": len(cases),
        },
        "cases": cases,
    }


# --- a Split step whose pattern is a literal (issue #167) ---------------------

# Three of these prove the escape happened -- measured, dropping Regex.Escape
# reddens backslash_d, metachar_dot and pipe; ab, the emoji and "" cannot.
_SPLIT_LITERALS = {
    "backslash_d": ("\\d", ["a\\db 7", "\\d\\d", "7\\d7"]),
    "metachar_dot": ("a.c", ["abc a.c", "a.c.a", "aXc"]),
    "pipe": ("|", ["ab|cd", "|ab", "a||b"]),
    "multi_char": ("ab", ["xabyab", "ab", "aab"]),
    "astral": ("\U0001f600", ["a\U0001f600b", "\U0001f600", "a\U0001f601b"]),
    # Carried as a model rather than a divergence because the two agree:
    # measured, BpePreTokenizer over "" gives ["a", "b", "c"] for "abc" too.
    "empty": ("", ["abc", "", "a"]),
}


def _split_literal_model(pattern):
    """A byte-level BPE with no merges, so every piece is one token per character.

    The pattern is handed in already wrapped, as a str for the literal spelling
    and a Regex for the escaped one -- which is the difference under test.
    """
    from tokenizers import Tokenizer, models, pre_tokenizers, decoders  # noqa: PLC0415

    vocab = {c: i for i, c in enumerate(sorted(pre_tokenizers.ByteLevel.alphabet()))}
    tokenizer = Tokenizer(models.BPE(vocab, []))
    tokenizer.pre_tokenizer = pre_tokenizers.Sequence([
        pre_tokenizers.Split(pattern, behavior="isolated", invert=False),
        pre_tokenizers.ByteLevel(add_prefix_space=False, use_regex=False),
    ])
    tokenizer.decoder = decoders.ByteLevel()
    return tokenizer


def _split_literal_models() -> list[tuple]:
    """(name, declares, literal, tokenizer, texts) -- each literal beside its escaped twin."""
    import re  # noqa: PLC0415
    from tokenizers import Regex  # noqa: PLC0415

    carried = []
    for name, (literal, texts) in _SPLIT_LITERALS.items():
        carried.append((
            f"{name}_literal", f"Split pattern spelled {{'String': {literal!r}}}",
            literal, _split_literal_model(literal), texts))
        carried.append((
            f"{name}_escaped", f"the same literal as {{'Regex': {re.escape(literal)!r}}}",
            None, _split_literal_model(Regex(re.escape(literal))), texts))
    return carried


def _split_literal_refusals() -> list[dict]:
    """The two pattern shapes #167 decides to refuse: neither spelling, and both.

    Not a measurement of either side. tokenizers builds neither shape, so there
    is no reference error to capture, which is why both are carried as shapes
    and not as recorded errors. The loader refuses each of them since 01c0de1 --
    the both case on the two keys being present rather than on both values being
    readable -- and the tests assert on those messages, not on anything here.
    """
    shapes = [
        ("pattern_empty", {}),
        ("pattern_both", {"Regex": "a", "String": "a"}),
    ]
    return [{"shape": shape, "pattern": pattern} for shape, pattern in shapes]


def generate_bpe_split_literal() -> dict:
    """What a literal Split pattern produces, beside its escaped-regex twin."""
    carried = _split_literal_models()
    cases = []
    for name, _declares, _literal, tokenizer, texts in carried:
        for text in texts:
            enc = tokenizer.encode(text)
            cases.append({
                "id": len(cases), "model": name, "text": text,
                # The pre-tokenizer's own output, which is where the pattern acts.
                "pieces": [p for p, _span in tokenizer.pre_tokenizer.pre_tokenize_str(text)],
                "tokens": enc.tokens, "ids": enc.ids,
            })

    return {
        "metadata": {
            "algorithm": "BPE Sequence Split step with a literal pattern",
            "library": "tokenizers",
            "library_version": version("tokenizers"),
            "model": BYTE_LEVEL_NO_MERGES,
            "models": {
                name: {"declares": declares, "literal": literal,
                       "tokenizer_json": tokenizer.to_str()}
                for name, declares, literal, tokenizer, _ in carried
            },
            # Lodestar's refusals, not the reference's: tokenizers builds neither
            # shape, so there is no error to capture -- only the pattern node.
            "refusals": _split_literal_refusals(),
            "count": len(cases),
        },
        "cases": cases,
    }


def generate_text_bktree() -> dict:
    """Radius queries answered by brute force, for BkTree to replay (#526).

    No canonical Python BK-tree exists to freeze, and none is wanted: the answer to
    "everything within k" is a property of the distance and the corpus, so scanning
    is the reference. The distances themselves are already frozen by levenshtein.json
    and its siblings, so this corpus adds the set, not the arithmetic.
    """
    alphabet = "abcd"
    cases = []
    for corpus_index, (size, max_length) in enumerate([(20, 4), (60, 6), (150, 8)]):
        rng = SeededRandom(SEED + 52600 + corpus_index)
        words = sorted({
            "".join(rng.choice(alphabet) for _ in range(rng.randint(1, max_length)))
            for _ in range(size * 3)
        })[:size]
        for query_index in range(6):
            query = "".join(
                rng.choice(alphabet) for _ in range(rng.randint(1, max_length)))
            for radius in (0, 1, 2, 3):
                hits = sorted(
                    ({"item": w, "distance": Levenshtein.distance(w, query)} for w in words),
                    key=lambda h: (h["distance"], h["item"]))
                hits = [h for h in hits if h["distance"] <= radius]
                cases.append({
                    "id": len(cases),
                    "corpus": words,
                    "query": query,
                    "radius": radius,
                    "hits": hits,
                })
            del query_index
    return {"metadata": {"library": "brute force", "version": "n/a",
                         "reference_calls": ["linear scan over Levenshtein"],
                         "seed": SEED, "count": len(cases)},
            "cases": cases}


KEYWORDS_STOP_WORDS = [
    "a", "all", "and", "are", "for", "in", "is", "of", "over", "that", "the", "this", "to",
]

KEYWORDS_TOKEN_PATTERN = r"\b\w+\b"

KEYWORDS_DOCUMENTS = [
    ("rose_abstract",
     "Compatibility of systems of linear constraints over the set of natural numbers. "
     "Criteria of compatibility of a system of linear Diophantine equations, strict "
     "inequations, and nonstrict inequations are considered. Upper bounds for components "
     "of a minimal set of solutions and algorithms of construction of minimal generating "
     "sets of solutions for all types of systems are given."),
    ("one_sentence",
     "Compatibility of systems of linear constraints over the set of natural numbers."),
    ("punctuation_only_boundaries", "red, green; blue"),
    ("all_stop_words", "of the and over a"),
    ("empty", ""),
]


def generate_keywords_rake() -> dict:
    """RAKE replayed against rake-nltk 1.0.6, its own tokenizer injected (#525).

    Injecting the tokenizer and the stop words means the reference tokenizes
    exactly as the C# does, and that generation needs no nltk.download.
    """
    import re  # noqa: PLC0415

    from rake_nltk import Metric, Rake  # noqa: PLC0415

    token = re.compile(KEYWORDS_TOKEN_PATTERN)
    stop = set(KEYWORDS_STOP_WORDS)

    # Injected rather than nltk's: the reference then tokenizes exactly as the C# does,
    # and generation needs no nltk.download of punkt_tab or stopwords.
    def sentences(text: str) -> list[str]:
        return [s for s in re.split(r"[.!?;:,\n]", text) if s.strip()]

    def words(sentence: str) -> list[str]:
        return token.findall(sentence.lower())

    metrics = {
        "DegreeToFrequencyRatio": Metric.DEGREE_TO_FREQUENCY_RATIO,
        "WordDegree": Metric.WORD_DEGREE,
        "WordFrequency": Metric.WORD_FREQUENCY,
    }

    cases = []
    for name, text in KEYWORDS_DOCUMENTS:
        for metric_name, metric in metrics.items():
            # Both settings, because the flag changes the degree and frequency tables and
            # not merely the output: freezing only True would leave the other half unread.
            for repeats in (True, False):
                rake = Rake(
                    stopwords=stop,
                    punctuations=set(),
                    ranking_metric=metric,
                    include_repeated_phrases=repeats,
                    sentence_tokenizer=sentences,
                    word_tokenizer=words,
                )
                rake.extract_keywords_from_text(text)
                cases.append({
                    "id": len(cases),
                    "name": f"{name}:{metric_name}:repeats={repeats}",
                    "text": text,
                    "metric": metric_name,
                    "min_length": 1,
                    "max_length": 100000,
                    "include_repeated_phrases": repeats,
                    "expected": [
                        {"phrase": phrase, "score": score}
                        for score, phrase in rake.get_ranked_phrases_with_scores()
                    ],
                })

    return {
        "metadata": {
            "algorithm": "Rake",
            "library": "rake-nltk",
            "library_version": version("rake-nltk"),
            "reference_calls": [
                "rake_nltk.Rake(stopwords=..., punctuations=set(), ranking_metric=...,"
                " sentence_tokenizer=..., word_tokenizer=...).get_ranked_phrases_with_scores()"
            ],
            "stop_words": KEYWORDS_STOP_WORDS,
            "token_pattern": KEYWORDS_TOKEN_PATTERN,
            "count": len(cases),
        },
        "cases": cases,
    }


KEYWORDS_TEXTRANK_DOCUMENTS = [
    # words=5, not the brief's 4: "criteria" and "natural" sit 1.1e-16 apart, a tie no
    # power iteration can be trusted to break the way LAPACK did; 5 clears the next real gap.
    ("two_sentences",
     "Compatibility of systems of linear constraints over the set of natural numbers. "
     "Criteria of compatibility of a system of linear Diophantine equations.", 5),
    ("natural_language",
     "Challenges in natural language processing frequently involve speech recognition, "
     "natural language understanding, natural language generation, and machine translation. "
     "Machine learning algorithms learn statistical models from large corpora of annotated "
     "text, and those models drive modern speech recognition systems.", 6),
    ("domestic_cat",
     "The domestic cat is a small carnivorous mammal. Cats are valued by humans for "
     "companionship and their ability to hunt rodents. Domestic cats communicate by meowing, "
     "purring, trilling, hissing, and growling, and cat body language conveys mood.", 6),
    ("no_co_occurrence", "Alpha.", 4),
    ("empty", "", 4),
]


# Within a run of adjacent scores tied at 1e-9, summa's order is BLAS-noise, not the
# algorithm; TextRankOracleTests.AssertRankingMatches enforces this exact ordinal sort back.
def _canonicalize_tied_runs(published):
    result = list(published)
    start = 0
    for i in range(len(result)):
        tied_with_next = i + 1 < len(result) and abs(result[i][1] - result[i + 1][1]) <= 1e-9
        if tied_with_next:
            continue
        result[start:i + 1] = sorted(result[start:i + 1], key=lambda pair: pair[0])
        start = i + 1
    return result


def generate_keywords_textrank() -> dict:
    """TextRank replayed against summa 1.2.0, with a deterministic pagerank (#525).

    summa's own ``pagerank_weighted_scipy`` takes ``vecs[i][0]`` -- ``scipy.linalg.eig``'s
    first left-eigenvector column -- with no check that it belongs to the largest
    eigenvalue. When the transition matrix has a repeated eigenvalue the eigenvector
    basis is not unique and LAPACK's column order is BLAS-build-dependent: measured,
    'two_sentences' has |eigenvalue| 0.85 with multiplicity 3 (and 'domestic_cat'
    likewise), and a GitHub Actions runner and this machine disagree about which
    column comes first. summa's raw published score for such a document is therefore
    not reproducible across machines.

    So the generator does not trust summa's own eigenvector pick. It replaces
    ``summa.keywords._pagerank`` (that module imports ``pagerank_weighted_scipy``
    under that alias, so it -- not ``summa.pagerank_weighted`` -- is what must be
    patched) with a deterministic version, for the span of one ``summa.keywords.keywords``
    call, that:

    - builds summa's own matrix bit for bit, including ``1 - 0.85 ==
      0.15000000000000002``, not ``0.15``;
    - selects the left eigenvector belonging to the eigenvalue of the largest
      modulus, by index rather than by column position;
    - asserts that eigenvalue is actually dominant and that the vector it picked
      satisfies ``v^T M ~= lambda v^T`` to a tight tolerance, raising ``SystemExit``
      naming the document otherwise;
    - returns ``{node: abs(component)}`` over the unit-normalised vector, matching
      the scale ``process_results`` assumes.

    Every other summa step -- tokenization, graph construction, extraction, phrase
    combination -- runs unchanged, and the original ``_pagerank`` is restored once
    all documents are done so no other generator is affected.
    """
    from scipy.linalg import eig  # noqa: PLC0415
    from summa import keywords as sk  # noqa: PLC0415
    from summa.pagerank_weighted import build_adjacency_matrix, build_probability_matrix  # noqa: PLC0415
    from summa.preprocessing.stopwords import get_stopwords_by_language  # noqa: PLC0415

    # get_stopwords_by_language returns one blob string; summa itself reads it with
    # .split() (textcleaner.py:51) -- sorted() of the raw string sorts characters instead.
    stop_words = sorted(get_stopwords_by_language("english").split())

    def make_deterministic_pagerank(name: str):
        def deterministic_pagerank(graph, damping=0.85):
            # Written exactly as pagerank_weighted_scipy writes it: `1 - 0.85` is
            # 0.15000000000000002, not 0.15, and that changes the matrix LAPACK diagonalises.
            adjacency_matrix = build_adjacency_matrix(graph)
            probability_matrix = build_probability_matrix(graph)
            matrix = damping * adjacency_matrix.todense() + (1 - damping) * probability_matrix

            vals, vecs = eig(matrix, left=True, right=False)
            idx = int(np.argmax(np.abs(vals)))
            dominant = vals[idx]

            if np.any(np.abs(vals) > np.abs(dominant) + 1e-12):
                raise SystemExit(
                    f"keywords_textrank {name!r}: eigenvalue at index {idx} ({dominant}) "
                    f"picked by argmax is not the largest modulus among {vals!r}."
                )

            vector = np.asarray(vecs[:, idx]).ravel()
            vector = vector / np.linalg.norm(vector)
            residual = np.abs(vector @ matrix - dominant * vector).max()
            if residual > 1e-9:
                raise SystemExit(
                    f"keywords_textrank {name!r}: selected vector does not satisfy "
                    f"v^T M = lambda v^T (max residual {residual})."
                )

            return dict(zip(graph.nodes(), np.abs(vector)))

        return deterministic_pagerank

    cases = []
    original_pagerank = sk._pagerank
    try:
        for name, text, words in KEYWORDS_TEXTRANK_DOCUMENTS:
            if text.strip():
                sk._pagerank = make_deterministic_pagerank(name)
                published = _canonicalize_tied_runs(sk.keywords(text, words=words, scores=True))
            else:
                published = []

            cases.append({
                "id": len(cases),
                "name": name,
                "text": text,
                "words": words,
                "expected": [{"phrase": phrase, "score": float(score)} for phrase, score in published],
            })
    finally:
        sk._pagerank = original_pagerank

    return {
        "metadata": {
            "algorithm": "TextRank",
            "library": "summa",
            "library_version": version("summa"),
            "reference_calls": ["summa.keywords.keywords(text, words=n, scores=True)"],
            "stop_words": stop_words,
            "window": 2,
            "damping": 0.85,
            "count": len(cases),
        },
        "cases": cases,
    }


MMR_CASES = [
    ("orthogonal_tail",
     [1.0, 0.0, 0.0],
     [[1.0, 0.0, 0.0], [0.8, 0.6, 0.0], [0.6, 0.0, 0.8], [0.0, 1.0, 0.0]],
     3, [0.0, 0.25, 0.5, 0.75, 1.0]),
    ("two_clusters",
     [1.0, 1.0, 0.0],
     [[1.0, 0.9, 0.0], [0.9, 1.0, 0.0], [0.0, 0.0, 1.0], [0.1, 0.0, 1.0], [1.0, 0.0, 0.0]],
     3, [0.0, 0.5, 1.0]),
    ("opposing_pair",
     [1.0, 0.0],
     [[1.0, 0.0], [-1.0, 0.0], [0.0, 1.0]],
     2, [0.0, 0.25, 0.5, 0.75, 1.0]),
]


def generate_mmr() -> dict:
    """MMR replayed against keybert 0.9.0's own selector (#525).

    keybert sorts its picks by relevance to the document, not by selection
    order, so the corpus freezes the selected *set* rather than the sequence
    -- see Mmr.Select's own doc comment for why the C# side returns order.
    """
    import numpy as np  # noqa: PLC0415
    from keybert._mmr import mmr  # noqa: PLC0415

    cases = []
    for name, query, candidates, count, lambdas in MMR_CASES:
        for lam in lambdas:
            labels = [str(i) for i in range(len(candidates))]
            chosen = mmr(
                np.array([query]), np.array(candidates), labels,
                top_n=count, diversity=1 - lam,
            )
            cases.append({
                "id": len(cases),
                "name": f"{name}:lambda={lam}",
                "query": query,
                "candidates": candidates,
                "count": count,
                "lambda": lam,
                # keybert sorts by similarity to the document, not by selection order,
                # so the set is what the two implementations can be held to.
                "selected": sorted(int(label) for label, _ in chosen),
            })

    return {
        "metadata": {
            "algorithm": "Mmr",
            "library": "keybert",
            "library_version": version("keybert"),
            "reference_calls": ["keybert._mmr.mmr(doc_embedding, word_embeddings, words, top_n, diversity)"],
            "note": "diversity = 1 - lambda; keybert returns its picks sorted by relevance, so only the set is compared",
            "count": len(cases),
        },
        "cases": cases,
    }


# --- Hypothesis tests (#442) ----------------------------------------------

STATS_LIBRARY = "scipy"
CASES = "cases"
PVALUE = "pvalue"
STATISTIC = "statistic"
ALTERNATIVE = "alternative"
METHOD = "method"
GREATER = "greater"
TWO_SIDED = "two-sided"
OBSERVED = "observed"
GROUPS = "groups"
TABLE = "table"
SERIES = "series"
LAG_COUNT = "lag_count"
BARTLETT = "bartlett"
ADFULLER = "adfuller"
KPSS = "kpss"
REGRESSION = "regression"
CRITICAL = "critical"

# nan_policy (#687): one spelling each for the policy key, its values, and the
# "call" names the new cases repeat past check_repeated_literals.py's threshold.
NAN_POLICY = "nan_policy"
PROPAGATE = "propagate"
RAISE_POLICY = "raise"
RAISES = "raises"
EQUAL_VAR = "equal_var"
POPMEAN = "popmean"
TTEST_IND = "ttest_ind"
TTEST_REL = "ttest_rel"
TTEST_1SAMP = "ttest_1samp"
WILCOXON = "wilcoxon"
MANNWHITNEYU = "mannwhitneyu"
KS_2SAMP = "ks_2samp"
F_ONEWAY = "f_oneway"
KRUSKAL = "kruskal"
SHAPIRO = "shapiro"
CHISQUARE = "chisquare"
EXPECTED_INPUT = "expected_input"


def _stats_metadata(family: str, count: int) -> dict:
    """The identity block every stats corpus carries.

    The version is read from the installed distribution rather than written
    down, so a corpus regenerated against a different scipy declares that fact
    instead of silently replacing numbers under the old version's name.
    """
    return {
        "library": STATS_LIBRARY,
        "version": version(STATS_LIBRARY),
        FAMILY: family,
        "count": count,
    }


def _stats_number(value: float) -> float | str:
    """A corpus number, with the three non-finite values spelled as strings.

    main() writes with allow_nan=False, so a bare Infinity would abort the whole
    generation. Two of these corpora produce one legitimately: a one-sided
    t-test's confidence interval is half-open, and Fisher's odds ratio is
    infinite when a diagonal of the table is zero. The spelling is the one
    tools/generate_oracles.py already uses for the ROC thresholds.

    No rounding here, unlike the metrics encoder: a p-value at 1e-53 has to
    survive the round trip to every bit the relative comparison checks.
    """
    if math.isnan(value):
        return "NaN"
    if math.isinf(value):
        return "Infinity" if value > 0 else "-Infinity"
    return float(value)


def _stats_nan_list(values: list[float]) -> list[float | str]:
    """A nan_policy fixture's own values, through _stats_number element-wise.

    The fixtures in _stats_nan_samples() and _stats_nan_paired() carry a literal
    NaN so scipy has something to filter or raise on; main() then writes every
    corpus with allow_nan=False, which refuses a bare NaN wherever it sits, not
    only in a statistic or a p-value. This is the same per-element spelling
    generate_oracles.py already applies to the ROC thresholds list.
    """
    return [_stats_number(v) for v in values]


def _stats_samples() -> list[dict]:
    """Sample pairs that between them exercise every branch the tests have.

    Balanced and unbalanced, tied and untied, small enough for the exact branch
    and large enough for the asymptotic one, plus one pair separated far enough
    that the p-value lands below 1e-15 -- which is where an absolute tolerance
    would stop proving anything.
    """
    rng = SeededRandom(SEED + 442)
    tiny_a = [1.0, 4.0, 7.0, 9.0]
    tiny_b = [2.0, 3.0, 8.0, 12.0, 15.0]
    tied_a = [1.0, 2.0, 2.0, 3.0, 5.0, 5.0]
    tied_b = [2.0, 3.0, 3.0, 4.0, 5.0, 7.0]
    wide_a = [round(rng.gauss(0.0, 1.0), 6) for _ in range(40)]
    wide_b = [round(rng.gauss(3.0, 1.0), 6) for _ in range(40)]
    return [
        {"name": "small and untied, exact branch reachable", "a": tiny_a, "b": tiny_b},
        {"name": "ties in both samples, auto falls back to asymptotic", "a": tied_a, "b": tied_b},
        {"name": "unbalanced, one sample twice the other",
         "a": tiny_a, "b": tiny_b + [20.0, 22.0, 25.0, 30.0, 33.0]},
        {"name": "well separated, p-value below 1e-15", "a": wide_a, "b": wide_b},
    ]


def _stats_paired() -> list[dict]:
    """Paired samples, including the zero differences Wilcoxon's zero_method is about."""
    rng = SeededRandom(SEED + 443)
    drifted = [round(rng.gauss(0.0, 1.0), 6) for _ in range(30)]
    return [
        {"name": "no zero differences", "x": [1.0, 3.0, 5.0, 7.0, 9.0, 11.0],
         "y": [2.0, 3.5, 4.0, 8.0, 8.5, 13.0]},
        {"name": "two zero differences", "x": [1.0, 3.0, 5.0, 7.0, 9.0, 11.0],
         "y": [1.0, 3.5, 5.0, 8.0, 8.5, 13.0]},
        {"name": "thirty pairs, asymptotic branch", "x": drifted,
         "y": [v + 0.8 for v in drifted[:15]] + [v - 0.1 for v in drifted[15:]]},
    ]


def _stats_nan_samples() -> list[dict]:
    """Independent samples holding NaN, for the per-sample arm of nan_policy (#687)."""
    return [
        {"name": "one nan in a", "a": [1.0, 2.0, float("nan"), 4.0, 5.0],
         "b": [2.0, 3.0, 4.0, 5.0, 7.0]},
        {"name": "nan in both", "a": [1.0, 2.0, float("nan"), 4.0],
         "b": [2.0, float("nan"), 3.0, 5.0, 9.0]},
    ]


def _stats_nan_paired() -> list[dict]:
    """Aligned samples whose listwise and per-sample filtering differ (#687).

    The first fixture is the one that catches a wrong implementation: x and y
    each hold exactly one NaN, at different indices, so dropping each sample
    independently leaves four and four values -- which a paired test consumes
    perfectly well, giving t=-5.0, df=3 -- while listwise deletion drops both
    indices together and leaves three pairs, giving t=-4.0, df=2. The fixture
    discriminates by producing a different number, not by raising where the
    other implementation does not.
    """
    return [
        {"name": "listwise differs from per-sample",
         "x": [1.0, 2.0, float("nan"), 4.0, 5.0],
         "y": [2.0, float("nan"), 3.0, 5.0, 7.0]},
        {"name": "one nan, same index",
         "x": [1.0, 2.0, float("nan"), 4.0, 5.0],
         "y": [2.0, 3.0, float("nan"), 5.0, 7.0]},
    ]


def _stats_nan_policy_cases(
        fixtures: list[dict],
        build: Callable[[dict, str], dict]) -> list[dict]:
    """The three cases every nan fixture owes: a value under each policy, then a refusal.

    Stated once here because five entry points across three families owe the same three,
    and a second spelling of the rule is how the corpora drift apart. `build` is handed
    the fixture and the policy, and answers with the case; "raise" asks it for the
    refusal record, which carries no numbers because scipy produced none.
    """
    cases: list[dict] = []
    for fx in fixtures:
        for policy in (PROPAGATE, "omit", RAISE_POLICY):
            cases.append(build(fx, policy))
    return cases


def _ttest_ind_nan_case(fx: dict, policy: str) -> dict:
    """One Welch two-sample case under one policy. Welch, because omission moves its df."""
    from scipy import stats as sps

    name = f"{fx['name']} | ind | nan_policy={policy}"
    args = {EQUAL_VAR: False, ALTERNATIVE: TWO_SIDED, NAN_POLICY: policy}
    a, b = _stats_nan_list(fx["a"]), _stats_nan_list(fx["b"])
    if policy == RAISE_POLICY:
        return {"name": name, "call": TTEST_IND, RAISES: True,
                "args": args, "a": a, "b": b}
    r = sps.ttest_ind(fx["a"], fx["b"], equal_var=False, nan_policy=policy)
    return {
        "name": name, "call": TTEST_IND, "args": args, "a": a, "b": b,
        STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        "df": _stats_number(r.df), "ci_low": None, "ci_high": None,
    }


def _ttest_rel_nan_case(fx: dict, policy: str) -> dict:
    """One paired case under one policy, where omission drops the pair and not the value."""
    from scipy import stats as sps

    name = f"{fx['name']} | rel | nan_policy={policy}"
    args = {ALTERNATIVE: TWO_SIDED, NAN_POLICY: policy}
    a, b = _stats_nan_list(fx["x"]), _stats_nan_list(fx["y"])
    if policy == RAISE_POLICY:
        return {"name": name, "call": TTEST_REL, RAISES: True,
                "args": args, "a": a, "b": b}
    r = sps.ttest_rel(fx["x"], fx["y"], nan_policy=policy)
    return {
        "name": name, "call": TTEST_REL, "args": args, "a": a, "b": b,
        STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        "df": _stats_number(r.df), "ci_low": None, "ci_high": None,
    }


def _ttest_1samp_nan_case(fx: dict, policy: str) -> dict:
    """One one-sample case under one policy, against a population mean of zero."""
    from scipy import stats as sps

    name = f"{fx['name']} | 1samp | nan_policy={policy}"
    args = {POPMEAN: 0.0, ALTERNATIVE: TWO_SIDED, NAN_POLICY: policy}
    a = _stats_nan_list(fx["a"])
    if policy == RAISE_POLICY:
        return {"name": name, "call": TTEST_1SAMP, RAISES: True,
                "args": args, "a": a, "b": []}
    r = sps.ttest_1samp(fx["a"], 0.0, nan_policy=policy)
    return {
        "name": name, "call": TTEST_1SAMP, "args": args, "a": a, "b": [],
        STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        "df": _stats_number(r.df), "ci_low": None, "ci_high": None,
    }


def _mannwhitney_nan_case(fx: dict, policy: str) -> dict:
    """One Mann-Whitney case under one policy, at scipy's own defaults."""
    from scipy import stats as sps

    name = f"{fx['name']} | nan_policy={policy}"
    args = {NAN_POLICY: policy}
    a, b = _stats_nan_list(fx["a"]), _stats_nan_list(fx["b"])
    if policy == RAISE_POLICY:
        return {"name": name, "call": MANNWHITNEYU, RAISES: True,
                "args": args, "a": a, "b": b}
    r = sps.mannwhitneyu(fx["a"], fx["b"], nan_policy=policy)
    return {
        "name": name, "call": MANNWHITNEYU, "args": args, "a": a, "b": b,
        STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
    }


def _wilcoxon_nan_case(fx: dict, policy: str) -> dict:
    """One Wilcoxon paired case under one policy, dropping the pair rather than the value."""
    from scipy import stats as sps

    name = f"{fx['name']} | paired | nan_policy={policy}"
    args = {NAN_POLICY: policy}
    x, y = _stats_nan_list(fx["x"]), _stats_nan_list(fx["y"])
    if policy == RAISE_POLICY:
        return {"name": name, "call": WILCOXON, RAISES: True,
                "args": args, "x": x, "y": y}
    r = sps.wilcoxon(fx["x"], fx["y"], nan_policy=policy)
    return {
        "name": name, "call": WILCOXON, "args": args, "x": x, "y": y,
        STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
    }


def generate_stats_ttest() -> dict:
    """Student, Welch, paired and one-sample t, against scipy.stats (#442)."""
    from scipy import stats as sps

    cases: list[dict] = []
    for fx in _stats_samples():
        for equal_var in (True, False):
            for alternative in (TWO_SIDED, "less", GREATER):
                r = sps.ttest_ind(fx["a"], fx["b"], equal_var=equal_var,
                                  alternative=alternative)
                low, high = r.confidence_interval(0.95)
                cases.append({
                    "name": f"{fx['name']} | ind | equal_var={equal_var} | {alternative}",
                    "call": TTEST_IND,
                    "args": {EQUAL_VAR: equal_var, ALTERNATIVE: alternative},
                    "a": fx["a"], "b": fx["b"],
                    STATISTIC: _stats_number(r.statistic), PVALUE: float(r.pvalue),
                    "df": float(r.df),
                    "ci_low": _stats_number(low), "ci_high": _stats_number(high),
                })

    for fx in _stats_paired():
        for alternative in (TWO_SIDED, "less", GREATER):
            r = sps.ttest_rel(fx["x"], fx["y"], alternative=alternative)
            low, high = r.confidence_interval(0.95)
            cases.append({
                "name": f"{fx['name']} | rel | {alternative}",
                "call": TTEST_REL,
                "args": {ALTERNATIVE: alternative},
                "a": fx["x"], "b": fx["y"],
                STATISTIC: _stats_number(r.statistic), PVALUE: float(r.pvalue),
                "df": float(r.df),
                "ci_low": _stats_number(low), "ci_high": _stats_number(high),
            })

    for fx in _stats_samples():
        for popmean in (0.0, 5.0):
            for alternative in (TWO_SIDED, "less", GREATER):
                r = sps.ttest_1samp(fx["a"], popmean, alternative=alternative)
                low, high = r.confidence_interval(0.95)
                cases.append({
                    "name": f"{fx['name']} | 1samp | mean={popmean} | {alternative}",
                    "call": TTEST_1SAMP,
                    "args": {POPMEAN: popmean, ALTERNATIVE: alternative},
                    "a": fx["a"], "b": [],
                    STATISTIC: _stats_number(r.statistic), PVALUE: float(r.pvalue),
                    "df": float(r.df),
                    "ci_low": _stats_number(low), "ci_high": _stats_number(high),
                })

    cases.extend(_stats_nan_policy_cases(_stats_nan_samples(), _ttest_ind_nan_case))
    cases.extend(_stats_nan_policy_cases(_stats_nan_paired(), _ttest_rel_nan_case))
    cases.extend(_stats_nan_policy_cases(_stats_nan_samples(), _ttest_1samp_nan_case))

    return {"metadata": _stats_metadata("ttest", len(cases)), CASES: cases}


def generate_stats_mannwhitney() -> dict:
    """Mann-Whitney U, over both continuity settings and all three methods (#442)."""
    from scipy import stats as sps

    cases: list[dict] = []
    for fx in _stats_samples():
        for use_continuity in (True, False):
            for method in ("auto", "asymptotic"):
                for alternative in (TWO_SIDED, "less", GREATER):
                    r = sps.mannwhitneyu(fx["a"], fx["b"], use_continuity=use_continuity,
                                         alternative=alternative, method=method)
                    cases.append({
                        "name": f"{fx['name']} | continuity={use_continuity} | "
                                f"{method} | {alternative}",
                        "call": MANNWHITNEYU,
                        "args": {"use_continuity": use_continuity,
                                 ALTERNATIVE: alternative, METHOD: method},
                        "a": fx["a"], "b": fx["b"],
                        STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
                    })

    # The exact branch asked for explicitly, including on tied data: measured,
    # scipy computes there rather than refusing, and parity means matching that.
    for fx in _stats_samples()[:3]:
        for alternative in (TWO_SIDED, "less", GREATER):
            r = sps.mannwhitneyu(fx["a"], fx["b"], use_continuity=True,
                                 alternative=alternative, method="exact")
            cases.append({
                "name": f"{fx['name']} | exact | {alternative}",
                "call": MANNWHITNEYU,
                "args": {"use_continuity": True, ALTERNATIVE: alternative,
                         METHOD: "exact"},
                "a": fx["a"], "b": fx["b"],
                STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
            })

    cases.extend(_stats_nan_policy_cases(_stats_nan_samples(), _mannwhitney_nan_case))

    return {"metadata": _stats_metadata("mannwhitney", len(cases)), CASES: cases}


def generate_stats_wilcoxon() -> dict:
    """Wilcoxon signed-rank, over the three zero methods (#442)."""
    from scipy import stats as sps

    cases: list[dict] = []
    for fx in _stats_paired():
        for zero_method in ("wilcox", "pratt", "zsplit"):
            for correction in (False, True):
                for method in ("auto", "asymptotic"):
                    for alternative in (TWO_SIDED, "less", GREATER):
                        r = sps.wilcoxon(fx["x"], fx["y"], zero_method=zero_method,
                                         correction=correction, alternative=alternative,
                                         method=method)
                        cases.append({
                            "name": f"{fx['name']} | {zero_method} | "
                                    f"correction={correction} | {method} | {alternative}",
                            "call": WILCOXON,
                            "args": {"zero_method": zero_method, "correction": correction,
                                     ALTERNATIVE: alternative, METHOD: method},
                            "x": fx["x"], "y": fx["y"],
                            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
                        })

    cases.extend(_stats_nan_policy_cases(_stats_nan_paired(), _wilcoxon_nan_case))

    return {"metadata": _stats_metadata(WILCOXON, len(cases)), CASES: cases}


def generate_stats_chisquare() -> dict:
    """Goodness of fit and contingency, with Yates on and off (#442)."""
    from scipy import stats as sps

    goodness = [
        {"name": "uniform expectation, six categories",
         OBSERVED: [16.0, 18.0, 16.0, 14.0, 12.0, 12.0], "expected": []},
        # scipy 1.18.0's chisquare refuses an f_exp that does not sum to f_obs's
        # sum (88.0); a uniform [16]*6 sums to 96, so this profile is non-uniform.
        {"name": "explicit expectation",
         OBSERVED: [16.0, 18.0, 16.0, 14.0, 12.0, 12.0],
         "expected": [20.0, 18.0, 16.0, 14.0, 12.0, 8.0]},
        {"name": "a far tail, p below 1e-15",
         OBSERVED: [200.0, 10.0, 10.0, 10.0], "expected": []},
    ]
    tables = [
        {"name": "2x2, Yates applies", TABLE: [[10.0, 20.0], [30.0, 40.0]]},
        {"name": "2x2, strong association", TABLE: [[100.0, 10.0], [10.0, 100.0]]},
        {"name": "3x2, Yates does not apply",
         TABLE: [[10.0, 20.0], [30.0, 40.0], [15.0, 5.0]]},
        {"name": "3x3", TABLE: [[10.0, 20.0, 30.0], [30.0, 40.0, 10.0], [5.0, 15.0, 25.0]]},
    ]

    cases: list[dict] = []
    for fx in goodness:
        expected = fx["expected"] or None
        r = sps.chisquare(fx[OBSERVED], f_exp=expected)
        cases.append({
            "name": f"{fx['name']} | chisquare",
            "call": CHISQUARE,
            "args": {"f_exp": fx["expected"]},
            OBSERVED: fx[OBSERVED], EXPECTED_INPUT: fx["expected"],
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
        })

    for fx in tables:
        for correction in (True, False):
            r = sps.chi2_contingency(np.array(fx[TABLE]), correction=correction)
            cases.append({
                "name": f"{fx['name']} | correction={correction}",
                "call": "chi2_contingency",
                "args": {"correction": correction},
                TABLE: fx[TABLE],
                STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
                "dof": int(r.dof),
                "expected_freq": [[float(v) for v in row] for row in r.expected_freq],
            })

    nan_observed = [10.0, 20.0, float("nan"), 40.0]
    for policy in (PROPAGATE, "omit"):
        r = sps.chisquare(nan_observed, nan_policy=policy)
        cases.append({
            "name": f"one nan, uniform expectation | nan_policy={policy}",
            "call": CHISQUARE, "args": {NAN_POLICY: policy},
            OBSERVED: _stats_nan_list(nan_observed), EXPECTED_INPUT: [],
            STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        })
    cases.append({
        "name": "one nan, uniform expectation | nan_policy=raise",
        "call": CHISQUARE, RAISES: True, "args": {NAN_POLICY: RAISE_POLICY},
        OBSERVED: _stats_nan_list(nan_observed), EXPECTED_INPUT: [],
    })

    return {"metadata": _stats_metadata(CHISQUARE, len(cases)), CASES: cases}


def generate_stats_fisher() -> dict:
    """Fisher's exact test on 2x2 tables, all three alternatives (#442)."""
    from scipy import stats as sps

    tables = [
        {"name": "Fisher's tea tasting", TABLE: [[3, 1], [1, 3]]},
        {"name": "a zero cell", TABLE: [[8, 2], [1, 5]]},
        {"name": "two zero cells", TABLE: [[5, 0], [0, 5]]},
        {"name": "large counts", TABLE: [[100, 40], [35, 120]]},
        {"name": "an empty row is refused by the C# side", TABLE: [[7, 3], [2, 9]]},
    ]

    cases: list[dict] = []
    for fx in tables:
        for alternative in (TWO_SIDED, "less", GREATER):
            r = sps.fisher_exact(np.array(fx[TABLE]), alternative=alternative)
            cases.append({
                "name": f"{fx['name']} | {alternative}",
                "call": "fisher_exact",
                "args": {ALTERNATIVE: alternative},
                TABLE: fx[TABLE],
                # The odds ratio is infinite when a diagonal is zero, which the
                # "two zero cells" fixture is there to reach.
                STATISTIC: _stats_number(r.statistic), PVALUE: float(r.pvalue),
            })

    return {"metadata": _stats_metadata("fisher", len(cases)), CASES: cases}


def generate_stats_ks() -> dict:
    """Two-sample Kolmogorov-Smirnov, exact and asymptotic (#442)."""
    from scipy import stats as sps

    # long-comment: why this family carries two fixtures of its own beyond the shared ones.
    # The equal-size two-sided exact branch has a closed form (#756) where every other shape
    # walks a table, and the shared pairs reach it only at 40 values. These two pin the split at
    # a hundred: one pair of equal sizes, where the closed form runs, and one of 100 against 99,
    # where it must not -- an implementation that used the closed form for both fails the second.
    # Both products stay under the Auto threshold, so both sides take their exact branch: at
    # 100 x 101 they would not, which is a divergence docs/equivalence.md records rather than a
    # property of this branch.
    ks_rng = SeededRandom(SEED + 756)
    hundred_a = [round(ks_rng.gauss(0.0, 1.0), 6) for _ in range(100)]
    hundred_b = [round(ks_rng.gauss(0.4, 1.0), 6) for _ in range(100)]
    fixtures = [
        *_stats_samples(),
        {"name": "equal sizes of 100, the closed-form branch", "a": hundred_a, "b": hundred_b},
        {"name": "sizes 100 and 99, the table branch", "a": hundred_a, "b": hundred_b[:99]},
    ]

    def case(name, a, b, alternative, method):
        r = sps.ks_2samp(a, b, alternative=alternative, method=method)
        return {
            "name": f"{name} | {method} | {alternative}",
            "call": KS_2SAMP,
            "args": {ALTERNATIVE: alternative, METHOD: method},
            "a": a, "b": b,
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
            "statistic_location": float(r.statistic_location),
            "statistic_sign": int(r.statistic_sign),
        }

    cases: list[dict] = []
    # scipy's Auto is exact while max(n, m) <= 10,000 (#802): 1,000 was asymptotic here, and 10,001
    # pins the top, where Exact must answer by the closed form rather than refuse.
    for size, methods in ((1_000, ("auto",)), (10_000, ("auto",)), (10_001, ("auto", "exact"))):
        big_rng = SeededRandom(SEED + 802 + size)
        big_a = [round(big_rng.gauss(0.0, 1.0), 4) for _ in range(size)]
        big_b = [round(big_rng.gauss(0.05, 1.0), 4) for _ in range(size)]
        cases.extend(case(f"equal sizes of {size:,}", big_a, big_b, TWO_SIDED, method) for method in methods)

    for fx in fixtures:
        for method in ("auto", "asymp", "exact"):
            for alternative in (TWO_SIDED, "less", GREATER):
                cases.append(case(fx["name"], fx["a"], fx["b"], alternative, method))

    for fx in _stats_nan_samples():
        for policy in (PROPAGATE, "omit"):
            r = sps.ks_2samp(fx["a"], fx["b"], nan_policy=policy)
            cases.append({
                "name": f"{fx['name']} | nan_policy={policy}",
                "call": KS_2SAMP,
                "args": {ALTERNATIVE: TWO_SIDED, METHOD: "auto", NAN_POLICY: policy},
                "a": _stats_nan_list(fx["a"]), "b": _stats_nan_list(fx["b"]),
                STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
                "statistic_location": _stats_number(r.statistic_location),
                # int(): scipy's own signature, except nan_policy=propagate returns
                # this as NaN when the sign is undefined, which int() cannot hold.
                "statistic_sign": _stats_number(float(r.statistic_sign)),
            })
        cases.append({
            "name": f"{fx['name']} | nan_policy=raise",
            "call": KS_2SAMP, RAISES: True,
            "args": {ALTERNATIVE: TWO_SIDED, METHOD: "auto", NAN_POLICY: RAISE_POLICY},
            "a": _stats_nan_list(fx["a"]), "b": _stats_nan_list(fx["b"]),
        })

    return {"metadata": _stats_metadata("ks", len(cases)), CASES: cases}


def _stats_groups() -> list[dict]:
    """Group sets for the two k-sample tests."""
    rng = SeededRandom(SEED + 444)
    return [
        {"name": "three balanced groups",
         GROUPS: [[1.0, 2.0, 3.0, 4.0], [2.0, 3.0, 4.0, 5.0], [5.0, 6.0, 7.0, 8.0]]},
        {"name": "unbalanced groups",
         GROUPS: [[1.0, 2.0], [2.0, 3.0, 4.0, 5.0, 6.0], [5.0, 6.0, 7.0]]},
        {"name": "ties across groups",
         GROUPS: [[1.0, 2.0, 2.0], [2.0, 2.0, 3.0], [3.0, 3.0, 4.0]]},
        {"name": "three separated groups, p below 1e-15",
         GROUPS: [[round(rng.gauss(m, 1.0), 6) for _ in range(30)] for m in (0.0, 4.0, 8.0)]},
    ]


def generate_stats_anova() -> dict:
    """One-way ANOVA, against scipy.stats.f_oneway (#442)."""
    from scipy import stats as sps

    cases = []
    for fx in _stats_groups():
        r = sps.f_oneway(*[np.array(g) for g in fx[GROUPS]])
        cases.append({
            "name": fx["name"], "call": F_ONEWAY, "args": {},
            GROUPS: fx[GROUPS],
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
        })

    nan_groups = [[1.0, 2.0, float("nan"), 4.0], [2.0, 3.0, 4.0, 5.0], [5.0, 6.0, 7.0, 8.0]]
    for policy in (PROPAGATE, "omit"):
        r = sps.f_oneway(*nan_groups, nan_policy=policy)
        cases.append({
            "name": f"nan in the first group | nan_policy={policy}",
            "call": F_ONEWAY, "args": {NAN_POLICY: policy},
            GROUPS: [_stats_nan_list(g) for g in nan_groups],
            STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        })
    cases.append({
        "name": "nan in the first group | nan_policy=raise",
        "call": F_ONEWAY, RAISES: True, "args": {NAN_POLICY: RAISE_POLICY},
        GROUPS: [_stats_nan_list(g) for g in nan_groups],
    })

    return {"metadata": _stats_metadata("anova", len(cases)), CASES: cases}


def generate_stats_kruskal() -> dict:
    """Kruskal-Wallis, against scipy.stats.kruskal (#442)."""
    from scipy import stats as sps

    cases = []
    for fx in _stats_groups():
        r = sps.kruskal(*[np.array(g) for g in fx[GROUPS]])
        cases.append({
            "name": fx["name"], "call": KRUSKAL, "args": {},
            GROUPS: fx[GROUPS],
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
        })

    nan_groups = [[1.0, 2.0, float("nan"), 4.0], [2.0, 3.0, 4.0, 5.0], [5.0, 6.0, 7.0, 8.0]]
    for policy in (PROPAGATE, "omit"):
        r = sps.kruskal(*nan_groups, nan_policy=policy)
        cases.append({
            "name": f"nan in the first group | nan_policy={policy}",
            "call": KRUSKAL, "args": {NAN_POLICY: policy},
            GROUPS: [_stats_nan_list(g) for g in nan_groups],
            STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        })
    cases.append({
        "name": "nan in the first group | nan_policy=raise",
        "call": KRUSKAL, RAISES: True, "args": {NAN_POLICY: RAISE_POLICY},
        GROUPS: [_stats_nan_list(g) for g in nan_groups],
    })

    return {"metadata": _stats_metadata(KRUSKAL, len(cases)), CASES: cases}


def generate_stats_shapiro() -> dict:
    """Shapiro-Wilk, against scipy.stats.shapiro (#442)."""
    from scipy import stats as sps

    rng = SeededRandom(SEED + 445)
    samples = [
        {"name": "seven normal draws, the smallest n Royston covers",
         "x": [round(rng.gauss(0.0, 1.0), 6) for _ in range(7)]},
        {"name": "twenty normal draws",
         "x": [round(rng.gauss(0.0, 1.0), 6) for _ in range(20)]},
        {"name": "fifty normal draws",
         "x": [round(rng.gauss(0.0, 1.0), 6) for _ in range(50)]},
        # SeededRandom exposes random/randint/randrange/choice/uniform/gauss and
        # no expovariate, so the exponential draw is its own inverse CDF.
        {"name": "two hundred exponential draws, p below 1e-15",
         "x": [round(-math.log(1.0 - rng.random()), 6) for _ in range(200)]},
        {"name": "a sample with ties",
         "x": [1.0, 1.0, 2.0, 2.0, 3.0, 3.0, 4.0, 5.0, 9.0, 9.0]},
    ]

    cases = []
    for fx in samples:
        r = sps.shapiro(np.array(fx["x"]))
        cases.append({
            "name": fx["name"], "call": SHAPIRO, "args": {},
            "x": fx["x"],
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
        })

    nan_sample = [1.0, 2.0, float("nan"), 4.0, 5.0, 6.0, 7.0]
    for policy in (PROPAGATE, "omit"):
        r = sps.shapiro(nan_sample, nan_policy=policy)
        cases.append({
            "name": f"one nan | nan_policy={policy}",
            "call": SHAPIRO, "args": {NAN_POLICY: policy}, "x": _stats_nan_list(nan_sample),
            STATISTIC: _stats_number(r.statistic), PVALUE: _stats_number(r.pvalue),
        })
    cases.append({
        "name": "one nan | nan_policy=raise",
        "call": SHAPIRO, RAISES: True, "args": {NAN_POLICY: RAISE_POLICY},
        "x": _stats_nan_list(nan_sample),
    })

    # Below seven, AS R94's special cases: n = 3 fixes its weights, n = 4 and 5 correct one (#863).
    for name, x in (("three values, skewed", [1.0, 2.0, 4.0]),
                    ("three values, evenly spaced", [1.0, 2.0, 3.0]),
                    ("three values, near symmetric", [0.5, -1.2, 2.3]),
                    ("four values, doubling", [1.0, 2.0, 4.0, 8.0]),
                    ("five values", [0.1, 0.7, -0.3, 1.9, -2.2])):
        r = sps.shapiro(np.array(x))
        cases.append({
            "name": name, "call": SHAPIRO, "args": {}, "x": x,
            STATISTIC: float(r.statistic), PVALUE: float(r.pvalue),
        })

    return {"metadata": _stats_metadata(SHAPIRO, len(cases)), CASES: cases}


def generate_stats_multiple_comparisons() -> dict:
    """Benjamini-Hochberg and Benjamini-Yekutieli from scipy; Bonferroni from its definition.

    scipy has no Bonferroni, and adding statsmodels to reach one would widen the
    surface generate_oracles.py depends on for a rule that is min(p * n, 1). The
    corpus states the definition instead, the way #526 generated the BK-tree
    corpus by brute force rather than by a second library.
    """
    from scipy import stats as sps

    families = [
        {"name": "four p-values, one clearly significant", "p": [0.01, 0.02, 0.2, 0.5]},
        {"name": "already sorted, all small", "p": [0.001, 0.008, 0.039, 0.041, 0.042]},
        {"name": "unsorted, with a tie", "p": [0.3, 0.02, 0.02, 0.9, 0.001]},
        {"name": "a single p-value", "p": [0.04]},
        {"name": "everything at one", "p": [1.0, 1.0, 1.0]},
    ]

    cases = []
    for fx in families:
        n = len(fx["p"])
        cases.append({
            "name": fx["name"], "call": "false_discovery_control", "args": {},
            "p": fx["p"],
            "bonferroni": [min(p * n, 1.0) for p in fx["p"]],
            "bh": [float(v) for v in sps.false_discovery_control(np.array(fx["p"]), method="bh")],
            "by": [float(v) for v in sps.false_discovery_control(np.array(fx["p"]), method="by")],
        })

    return {"metadata": _stats_metadata("multiple_comparisons", len(cases)), CASES: cases}


def _timeseries_fixtures() -> list[dict]:
    """Series whose correlograms differ in ways a wrong implementation cannot fake."""
    rng = SeededRandom(SEED + 617)

    ar1 = [0.0]
    for _ in range(39):
        ar1.append(0.7 * ar1[-1] + rng.gauss(0.0, 1.0))

    shocks = [rng.gauss(0.0, 1.0) for _ in range(61)]
    ma1 = [shocks[i] + 0.6 * shocks[i - 1] for i in range(1, 61)]

    noise = [rng.gauss(0.0, 1.0) for _ in range(100)]
    trend = [0.05 * i + rng.gauss(0.0, 0.3) for i in range(80)]
    seasonal = [
        math.sin(2.0 * math.pi * i / 12.0) + rng.gauss(0.0, 0.2) for i in range(96)
    ]

    return [
        {"name": "AR(1) at 0.7, 40 points", SERIES: [round(v, 10) for v in ar1], LAG_COUNT: 8},
        {"name": "MA(1) at 0.6, 60 points", SERIES: [round(v, 10) for v in ma1], LAG_COUNT: 8},
        {"name": "white noise, 100 points", SERIES: [round(v, 10) for v in noise], LAG_COUNT: 10},
        {"name": "linear trend, 80 points", SERIES: [round(v, 10) for v in trend], LAG_COUNT: 10},
        {"name": "seasonal period 12, 96 points",
         SERIES: [round(v, 10) for v in seasonal], LAG_COUNT: 14},
        {"name": "short series, 12 points", SERIES: [round(v, 10) for v in noise[:12]],
         LAG_COUNT: 5},
        # LAG_COUNT here is len(x) // 2 exactly, so pacf's min(lags, len(x) // 2) binds --
        # the seasonal series is the harshest fixture, near-singular at high order (#617 review).
        {"name": "seasonal period 12, 96 points, at the pacf ceiling",
         SERIES: [round(v, 10) for v in seasonal], LAG_COUNT: 48},
    ]


def generate_stats_timeseries() -> dict:
    """ACF, PACF and Ljung-Box, against statsmodels 0.15.0 (#617)."""
    import numpy as np
    from statsmodels.stats.diagnostic import acorr_ljungbox
    from statsmodels.tsa.stattools import acf, pacf

    cases: list[dict] = []
    for fx in _timeseries_fixtures():
        x = np.array(fx[SERIES])
        lags = fx[LAG_COUNT]

        for level in (0.95, 0.99):
            alpha = round(1.0 - level, 10)
            for adjusted in (False, True):
                for bartlett in (True, False):
                    values, confint = acf(
                        x, nlags=lags, adjusted=adjusted, fft=False,
                        alpha=alpha, bartlett_confint=bartlett, result_object=False)
                    cases.append({
                        "name": f"{fx['name']} | acf | {level} | "
                                f"adjusted={adjusted} | bartlett={bartlett}",
                        "call": "acf",
                        SERIES: fx[SERIES], LAG_COUNT: lags,
                        "level": level, "adjusted": adjusted, BARTLETT: bartlett,
                        "values": [float(v) for v in values],
                        LOWER: [float(row[0]) for row in confint],
                        UPPER: [float(row[1]) for row in confint],
                    })

            pvalues, pconfint = pacf(x, nlags=min(lags, len(x) // 2), alpha=alpha)
            cases.append({
                "name": f"{fx['name']} | pacf | {level}",
                "call": "pacf",
                SERIES: fx[SERIES], LAG_COUNT: min(lags, len(x) // 2),
                "level": level,
                "values": [float(v) for v in pvalues],
                LOWER: [float(row[0]) for row in pconfint],
                UPPER: [float(row[1]) for row in pconfint],
            })

        for model_df in (0, 2):
            frame = acorr_ljungbox(
                x, lags=list(range(1, lags + 1)), model_df=model_df, boxpierce=True)
            cases.append({
                "name": f"{fx['name']} | ljungbox | model_df={model_df}",
                "call": "acorr_ljungbox",
                SERIES: fx[SERIES], LAG_COUNT: lags, "model_df": model_df,
                "statistics": [float(v) for v in frame["lb_stat"]],
                "pvalues": [_stats_number(v) for v in frame["lb_pvalue"]],
                "bp_statistics": [float(v) for v in frame["bp_stat"]],
                "bp_pvalues": [_stats_number(v) for v in frame["bp_pvalue"]],
            })

    return {
        "metadata": {
            "library": STATSMODELS,
            "version": version(STATSMODELS),
            FAMILY: "timeseries",
            "count": len(cases),
        },
        CASES: cases,
    }


def _adf_near_switch(rng: SeededRandom, below: bool) -> list[float]:
    """An AR(1) whose ADF statistic under 'c' falls within 0.3 of MacKinnon's switch point."""
    from statsmodels.tsa.stattools import adfuller

    for step in range(400):
        phi = 0.80 + step * 0.0005
        series = [0.0]
        for _ in range(119):
            series.append(phi * series[-1] + rng.gauss(0.0, 1.0))
        statistic = adfuller(series, result_object=False)[0]
        if (-1.91 < statistic <= -1.61) if below else (-1.61 < statistic < -1.31):
            return [round(v, 10) for v in series]
    raise SystemExit("no series near ADF's switch point; widen the search")


def _kpss_near(rng: SeededRandom, low: float, high: float, walk_weight: float) -> list[float]:
    """Noise plus a scaled random walk whose KPSS level statistic lands in (low, high)."""
    from statsmodels.tsa.stattools import kpss

    for step in range(400):
        weight = walk_weight * (1.0 + step * 0.01)
        walk, series = 0.0, []
        for _ in range(150):
            walk += rng.gauss(0.0, 1.0)
            series.append(rng.gauss(0.0, 1.0) + weight * walk)
        with warnings.catch_warnings():
            warnings.simplefilter("ignore")
            statistic = kpss(series, regression="c", nlags="auto", result_object=False)[0]
        if low < statistic < high:
            return [round(v, 10) for v in series]
    raise SystemExit(f"no series with a KPSS statistic in ({low}, {high}); widen the search")


def _stationarity_fixtures() -> list[dict]:
    """Series chosen for which branch of the reference each one exercises (#671)."""
    rng = SeededRandom(SEED + 671)

    walk = [0.0]
    for _ in range(199):
        walk.append(walk[-1] + rng.gauss(0.0, 1.0))
    ar = [0.0]
    for _ in range(199):
        ar.append(0.5 * ar[-1] + rng.gauss(0.0, 1.0))
    trend = [0.04 * i + rng.gauss(0.0, 1.0) for i in range(150)]
    noise = [rng.gauss(0.0, 1.0) for _ in range(400)]
    explosive = [1.0]
    for _ in range(79):
        explosive.append(1.03 * explosive[-1] + rng.gauss(0.0, 0.1))

    def fixture(name: str, series: list[float]) -> dict:
        return {"name": name, SERIES: [round(v, 10) for v in series]}

    return [
        fixture("random walk, 200 points", walk),
        fixture("AR(1) at 0.5, 200 points", ar),
        fixture("trend-stationary, 150 points", trend),
        fixture("white noise, 400 points", noise),
        fixture("explosive AR(1) at 1.03, 80 points", explosive),
        {"name": "ADF just below the 'c' switch point", SERIES: _adf_near_switch(rng, below=True)},
        {"name": "ADF just above the 'c' switch point", SERIES: _adf_near_switch(rng, below=False)},
        {"name": "KPSS just inside the 10 % end", SERIES: _kpss_near(rng, 0.347, 0.40, 0.02)},
        {"name": "KPSS just inside the 1 % end", SERIES: _kpss_near(rng, 0.68, 0.739, 0.05)},
    ]


def _kpss_bound(caught: list) -> str:
    """The direction statsmodels' InterpolationWarning names, or 'none'."""
    for warning in caught:
        text = str(warning.message)
        if "p-value is smaller" in text:
            return "smaller"
        if "p-value is greater" in text:
            return "greater"
    return "none"


def generate_stats_stationarity() -> dict:
    """The augmented Dickey-Fuller test and KPSS, against statsmodels 0.15.0 (#671)."""
    from statsmodels.tsa.stattools import adfuller, kpss

    cases: list[dict] = []
    for fx in _stationarity_fixtures():
        x = fx[SERIES]
        for regression in ("n", "c", "ct", "ctt"):
            for autolag in ("AIC", "BIC", "t-stat", None):
                stat, p, used, nobs, crit, *rest = adfuller(
                    x, regression=regression, autolag=autolag, result_object=False)
                cases.append({
                    "name": f"{fx['name']} | adfuller | {regression} | {autolag}",
                    "call": ADFULLER, SERIES: x, REGRESSION: regression,
                    "autolag": autolag, "maxlag": None,
                    STATISTIC: float(stat), PVALUE: float(p), "usedlag": int(used),
                    "nobs": int(nobs),
                    CRITICAL: [float(crit["1%"]), float(crit["5%"]), float(crit["10%"])],
                    "icbest": float(rest[0]) if autolag else None,
                })
        for regression in ("c", "ct"):
            for nlags in ("auto", "legacy", 4):
                with warnings.catch_warnings(record=True) as caught:
                    warnings.simplefilter("always")
                    stat, p, lags, crit = kpss(
                        x, regression=regression, nlags=nlags, result_object=False)
                cases.append({
                    "name": f"{fx['name']} | kpss | {regression} | {nlags}",
                    "call": KPSS, SERIES: x, REGRESSION: regression, "nlags": nlags,
                    STATISTIC: float(stat), PVALUE: float(p), "lags": int(lags),
                    CRITICAL: [float(crit[k]) for k in ("10%", "5%", "2.5%", "1%")],
                    "bound": _kpss_bound(caught),
                })

    noise = _stationarity_fixtures()[3][SERIES]
    stat, p, used, nobs, crit = adfuller(
        noise, maxlag=0, autolag=None, result_object=False)
    cases.append({
        "name": "white noise, 400 points | adfuller | c | fixed at 0",
        "call": ADFULLER, SERIES: noise, REGRESSION: "c", "autolag": None, "maxlag": 0,
        STATISTIC: float(stat), PVALUE: float(p), "usedlag": int(used), "nobs": int(nobs),
        CRITICAL: [float(crit["1%"]), float(crit["5%"]), float(crit["10%"])], "icbest": None,
    })

    return {
        "metadata": {"library": STATSMODELS, "version": version(STATSMODELS),
                     FAMILY: "stationarity", "count": len(cases)},
        CASES: cases,
    }


def generate_stats_seasonal() -> dict:
    """Classical seasonal decomposition, against statsmodels 0.15.0 (#671)."""
    import numpy as np
    from statsmodels.tsa.seasonal import seasonal_decompose

    rng = SeededRandom(SEED + 6710)
    monthly = [20.0 + 0.1 * i + 4.0 * math.sin(2.0 * math.pi * i / 12.0) + rng.gauss(0.0, 0.5)
               for i in range(96)]
    weekly = [10.0 + 2.0 * math.cos(2.0 * math.pi * i / 7.0) + rng.gauss(0.0, 0.3)
              for i in range(50)]
    minimal = [3.0, 5.0, 4.0, 6.0]

    cases: list[dict] = []
    for name, series, period, extrapolations in (
        ("monthly, 96 points", monthly, 12, (0, 1, 11)),
        ("period 7, 50 points", weekly, 7, (0, 1, 6)),
        ("period 2, 4 points", minimal, 2, (0, 1)),
    ):
        x = np.array([round(v, 10) for v in series])
        for model in ("additive", "multiplicative"):
            for two_sided in (True, False):
                for extrapolate in extrapolations:
                    result = seasonal_decompose(
                        x, model=model, period=period, two_sided=two_sided,
                        extrapolate_trend=extrapolate)
                    cases.append({
                        "name": f"{name} | {model} | two_sided={two_sided} | extrapolate={extrapolate}",
                        SERIES: x.tolist(), "period": period, "model": model,
                        "two_sided": two_sided, "extrapolate_trend": extrapolate,
                        "trend": [_stats_number(v) for v in result.trend],
                        "seasonal": [_stats_number(v) for v in result.seasonal],
                        "resid": [_stats_number(v) for v in result.resid],
                    })

    return {
        "metadata": {"library": STATSMODELS, "version": version(STATSMODELS),
                     FAMILY: "seasonal", "count": len(cases)},
        CASES: cases,
    }

def main() -> None:
    """Write every oracle deterministically, byte for byte.

    ``newline="\\n"``: these files are committed and CI's "Oracles are
    reproducible" job compares them with a raw ``git diff``, not a text-mode
    read. A contributor with core.autocrlf=false or unset who regenerates on
    Windows would have the platform default translate every "\\n" to "\\r\\n"
    on write, and that CRLF would reach the repository as-is and make the diff
    nonempty forever, even though nothing semantic changed. (core.autocrlf=true
    or =input is unaffected: git normalises CRLF back to LF on add/commit
    regardless of what Python wrote to disk -- verified against all three
    settings by committing an LF file, rewriting it as CRLF, and re-running
    ``git diff --quiet``: true and input exit 0, false exits 1.)

    ``allow_nan=False``: Python would otherwise write a bare NaN or Infinity,
    which is not JSON and which System.Text.Json refuses at load time -- a
    failure that would surface in CI as a broken test run rather than here as
    a broken generation. Non-finite oracle values are encoded deliberately, as
    the strings below.
    """
    ORACLE_DIR.mkdir(parents=True, exist_ok=True)
    generators = {
        "levenshtein.json": generate_levenshtein,
        "osa.json": generate_osa,
        "damerau.json": generate_damerau,
        "hamming.json": generate_hamming,
        "indel.json": generate_indel,
        "jaro.json": generate_jaro,
        "jaro_winkler.json": generate_jaro_winkler,
        "lcs.json": generate_lcs,
        "text_bktree.json": generate_text_bktree,
        "keywords_rake.json": generate_keywords_rake,
        "keywords_textrank.json": generate_keywords_textrank,
        "mmr.json": generate_mmr,
        "ratcliff.json": generate_ratcliff,
        "set_similarity.json": generate_set_similarity,
        "phonetics.json": generate_phonetics,
        "metaphone.json": generate_metaphone,
        "double_metaphone.json": generate_double_metaphone,
        "match_rating_codex.json": generate_match_rating_codex,
        "match_rating_comparison.json": generate_match_rating_comparison,
        "countvectorizer.json": generate_countvectorizer,
        "tfidfvectorizer.json": generate_tfidfvectorizer,
        "hashingvectorizer.json": generate_hashingvectorizer,
        "porter.json": generate_porter,
        "snowball_en.json": generate_snowball_en,
        "snowball_fr.json": generate_snowball_fr,
        "snowball_es.json": generate_snowball_es,
        "snowball_pt.json": generate_snowball_pt,
        "snowball_it.json": generate_snowball_it,
        "snowball_de.json": generate_snowball_de,
        "snowball_nl.json": generate_snowball_nl,
        "snowball_sv.json": generate_snowball_sv,
        "snowball_ru.json": generate_snowball_ru,
        "snowball_da.json": generate_snowball_da,
        "snowball_no.json": generate_snowball_no,
        "snowball_fi.json": generate_snowball_fi,
        "snowball_hu.json": generate_snowball_hu,
        "snowball_ro.json": generate_snowball_ro,
        "snowball_ar.json": generate_snowball_ar,
        "wordpiece.json": generate_wordpiece,
        "batch_encoding.json": generate_batch_encoding,
        "pooling.json": generate_pooling,
        "knn.json": generate_knn,
        "sentencepiece.json": generate_sentencepiece,
        "vocab_txt.json": generate_vocab_txt,
        "tokenizer_json.json": generate_tokenizer_json,
        "spiece_model.json": generate_spiece_model,
        "xlmr_fairseq.json": generate_xlmr_fairseq,
        "normalizer.json": generate_normalizer,
        "fuzz.json": generate_fuzz,
        "process.json": generate_process,
        "classification_metrics.json": generate_classification_metrics,
        "label_losses.json": generate_label_losses,
        "multilabel_confusion.json": generate_multilabel_confusion,
        "likelihood_ratios.json": generate_likelihood_ratios,
        "hinge_loss.json": generate_hinge_loss,
        "clustering_agreement.json": generate_clustering_agreement,
        "silhouette.json": generate_silhouette,
        "internal_validity.json": generate_internal_validity,
        "stats_distributions.json": generate_stats_distributions,
        "search_bm25.json": generate_search_bm25,
        "text_similarity.json": generate_text_similarity,
        "survival_curves.json": generate_survival_curves,
        "survival_logrank.json": generate_survival_logrank,
        "survival_cox.json": generate_survival_cox,
        "stats_ols.json": generate_stats_ols,
        "stats_wls.json": generate_stats_wls,
        "stats_gls.json": generate_stats_gls,
        "preprocessing_encoders.json": generate_preprocessing_encoders,
        "preprocessing_splitters.json": generate_preprocessing_splitters,
        "stats_glm.json": generate_stats_glm,
        "stats_var.json": generate_stats_var,
        "stats_mnlogit.json": generate_stats_mnlogit,
        "regression_log_factorial.json": generate_regression_log_factorial,
        "regression_log_gamma.json": generate_regression_log_gamma,
        "stats_timeseries.json": generate_stats_timeseries,
        "stats_stationarity.json": generate_stats_stationarity,
        "stats_seasonal.json": generate_stats_seasonal,
        "cluster_kmeans.json": generate_cluster_kmeans,
        "cluster_dbscan.json": generate_cluster_dbscan,
        "cluster_agglomerative.json": generate_cluster_agglomerative,
        "preprocessing_partial_fit.json": generate_preprocessing_partial_fit,
        "preprocessing_sparse.json": generate_preprocessing_sparse,
        "preprocessing_scalers.json": generate_preprocessing_scalers,
        "preprocessing_standard_scaler.json": generate_preprocessing_standard_scaler,
        "ranking.json": generate_ranking,
        "ranking_weighted.json": generate_ranking_weighted,
        "label_ranking.json": generate_label_ranking,
        "average_precision.json": generate_average_precision,
        "top_k_accuracy.json": generate_top_k_accuracy,
        "roc_auc.json": generate_roc_auc,
        "curves.json": generate_curves,
        "calibration.json": generate_calibration,
        "calibration_curve.json": generate_calibration_curve,
        "regression.json": generate_regression,
        "regression_conditioning.json": generate_regression_conditioning,
        "regression_deviance.json": generate_regression_deviance,
        "conformal.json": generate_conformal,
        "sparse_matmul.json": generate_sparse_matmul,
        "decomposition_qr.json": generate_decomposition_qr,
        "decomposition_lu.json": generate_decomposition_lu,
        "decomposition_svd.json": generate_decomposition_svd,
        "decomposition_nmf.json": generate_decomposition_nmf,
        "decomposition_pca.json": generate_decomposition_pca,
        "bpe.json": generate_bpe,
        "orphan_bpe.json": generate_orphan_bpe,
        "bytelevel_bpe.json": generate_bytelevel_bpe,
        "bpe_pretokenize.json": generate_bpe_pretokenize,
        "bpe_tokenizer_json.json": generate_bpe_tokenizer_json,
        "bpe_normalizer.json": generate_bpe_normalizer,
        "bpe_metaspace.json": generate_bpe_metaspace,
        "bpe_byte_fallback.json": generate_bpe_byte_fallback,
        "sentencepiece_bpe_lineage.json": generate_sentencepiece_bpe_lineage,
        "bytelevel_decode_stream.json": generate_bytelevel_decode_stream,
        "unicode_forms.json": generate_unicode_forms,
        "bpe_added_tokens.json": generate_bpe_added_tokens,
        "bpe_added_token_flags.json": generate_bpe_added_token_flags,
        "bpe_no_op_settings.json": generate_bpe_no_op_settings,
        "bpe_fuse_unk.json": generate_bpe_fuse_unk,
        "bpe_added_token_coverage.json": generate_bpe_added_token_coverage,
        "bpe_continuing_prefix.json": generate_bpe_continuing_prefix,
        "bpe_sequence_split.json": generate_bpe_sequence_split,
        "bpe_split_behavior.json": generate_bpe_split_behavior,
        "bpe_split_literal.json": generate_bpe_split_literal,
        "bpe_duplicate_merge.json": generate_bpe_duplicate_merge,
        "bpe_no_split.json": generate_bpe_no_split,
        "wordpiece_added_tokens.json": generate_wordpiece_added_tokens,
        "bpe_prefix_space.json": generate_bpe_prefix_space,
        "stats_ttest.json": generate_stats_ttest,
        "stats_mannwhitney.json": generate_stats_mannwhitney,
        "stats_wilcoxon.json": generate_stats_wilcoxon,
        "stats_chisquare.json": generate_stats_chisquare,
        "stats_fisher.json": generate_stats_fisher,
        "stats_ks.json": generate_stats_ks,
        "stats_anova.json": generate_stats_anova,
        "stats_kruskal.json": generate_stats_kruskal,
        "stats_shapiro.json": generate_stats_shapiro,
        "stats_multiple_comparisons.json": generate_stats_multiple_comparisons,
    }
    for filename, gen in generators.items():
        payload = gen()
        path = ORACLE_DIR / filename
        # newline="\n": see this function's docstring for why.
        with path.open("w", encoding="utf-8", newline="\n") as f:
            # allow_nan=False: see this function's docstring for why.
            json.dump(payload, f, ensure_ascii=False, indent=1, allow_nan=False)
            f.write("\n")
        print(f"{filename}: {payload['metadata']['count']} cases -> {path}")


if __name__ == "__main__":
    main()
