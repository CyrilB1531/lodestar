using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Lodestar.Abstractions;
using Lodestar.Conformal;
using Lodestar.Decomposition;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Fuzzy;
using Lodestar.Metrics;
using Lodestar.Sample;
using Lodestar.Text.Distances;

// A consumer of the published packages; runs in CI so it can't rot. Also ADR 0009's
// packaging gate: a new public type needs a call from <ClassName>Sample.cs (ADR 0041).

// Every number below goes through Inv.F3 and friends, which a hole carrying no
// format specifier cannot; this covers those, so the run reads the same everywhere (#205).
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

Console.WriteLine("Lodestar sample — consuming the NuGet packages");
Console.WriteLine(new string('-', 46));

// Confirms NuGet resolved the package's net10.0 lib folder, not something else —
// must report .NETCoreApp,Version=v10.0.
Console.WriteLine($"running on        : {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Lodestar.Text      : {FrameworkOf(typeof(Levenshtein))}");
Console.WriteLine($"Lodestar.Fuzzy     : {FrameworkOf(typeof(Fuzz))}");
Console.WriteLine($"Lodestar.Embeddings: {FrameworkOf(typeof(WordPieceTokenizer))}");
Console.WriteLine($"Lodestar.Metrics   : {FrameworkOf(typeof(ConfusionMatrix))}");
Console.WriteLine($"Lodestar.Conformal : {FrameworkOf(typeof(SplitConformal))}");
Console.WriteLine($"Lodestar.Abstractions: {FrameworkOf(typeof(CsrMatrix))}");
Console.WriteLine($"Lodestar.Decomposition: {FrameworkOf(typeof(TruncatedSvd))}");
Console.WriteLine();

TextSamples.Run();
Lot3Embeddings.Run();
Lot4Fuzzy.Run();
Lot5Metrics.Run();
Lot6Regression.Run();
RakeSample.Run();
RakeOptionsSample.Run();
TextRankSample.Run();
TextRankOptionsSample.Run();
KeywordMatchSample.Run();
SplitConformalSample.Run();
DecompositionSamples.Run();
DistributionsSample.Run();
QrDecompositionSample.Run();
PrincipalComponentVarianceSample.Run();
KMeansSample.Run();
KMeansOptionsSample.Run();
StandardScalerSample.Run();
StandardScalerOptionsSample.Run();
OrdinaryLeastSquaresSample.Run();
OlsSummarySample.Run();
OlsOptionsSample.Run();
GeneralizedLinearModelSample.Run();
GlmSummarySample.Run();
GlmOptionsSample.Run();

MinHashPermutationsSample.Run();
MinHashSample.Run();
SimHashSample.Run();
LshBandingSample.Run();
LshIndexSample.Run();

GpuContextSample.Run();
GpuSearchResultSample.Run();
DeviceEmbeddingMatrixSample.Run();
TiledCosineTopKSample.Run();
DeviceSparseMatrixSample.Run();
TiledSparseDenseProductSample.Run();
DeviceDenseBlockSample.Run();
DeviceTextBlockSample.Run();
BitParallelEditDistanceSample.Run();
DeviceTokenHashesSample.Run();
TiledMinHashSignaturesSample.Run();

Bm25IndexSample.Run();
Bm25OptionsSample.Run();
SearchHitSample.Run();
RankFusionSample.Run();

SurvivalStepSample.Run();
KaplanMeierSample.Run();
KaplanMeierCurveSample.Run();
NelsonAalenSample.Run();
NelsonAalenCurveSample.Run();
LogRankSample.Run();
LogRankResultSample.Run();
MathNetInteropSample.Run();
BkTreeSample.Run();
BkTreeMatchSample.Run();
TTestSample.Run();
TTestResultSample.Run();
MannWhitneySample.Run();
WilcoxonSample.Run();
ChiSquareSample.Run();
Chi2ContingencyResultSample.Run();
FisherExactSample.Run();
KolmogorovSmirnovSample.Run();
KsResultSample.Run();
OneWayAnovaSample.Run();
KruskalWallisSample.Run();
ShapiroWilkSample.Run();
MultipleComparisonsSample.Run();
TestResultSample.Run();
SerialCorrelationSample.Run();
AutocorrelationOptionsSample.Run();
AutocorrelationResultSample.Run();
LjungBoxOptionsSample.Run();
LjungBoxResultSample.Run();

if (!PackagingGate.Verify())
{
    return 1;
}

Console.WriteLine("OK");
return 0;

static string FrameworkOf(Type probe) =>
    probe.Assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? "<unknown>";
