using System.Runtime.CompilerServices;

// The public data types this package declared before #1142 live in Lodestar.Abstractions under the
// same names (decision 0003); code built against an earlier Lodestar.Preprocessing still binds through these.

[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.BinEncoding))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.BinStrategy))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.CategoryDrop))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.ImputationStrategy))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.KBinsDiscretizerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.KnnImputerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.MaxAbsScalerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.MinMaxScalerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.NeighbourWeights))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.OneHotEncoderOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.PolynomialFeaturesOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.PowerMethod))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.PowerTransformerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.QuantileMethod))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.QuantileOutput))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.QuantileTransformerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.RobustScalerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.RowNorm))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.SimpleImputerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.StandardScalerOptions))]
[assembly: TypeForwardedTo(typeof(Lodestar.Preprocessing.UnknownCategory))]
