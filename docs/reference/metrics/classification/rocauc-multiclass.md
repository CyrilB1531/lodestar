# RocAuc.MultiClass

Area under the ROC curve for more than two classes, by reducing to binary problems.

<!-- docs-declaration -->

```csharp
public static double MultiClass(ReadOnlySpan<int> yTrue, ReadOnlySpan<double> yScore, int classCount, MultiClassRocOptions options = default)
```

**Parameters** — `yTrue` holds one true label per sample. `yScore` holds the class probabilities
row-major — sample 0's classes, then sample 1's — so its length is `classCount` times the sample
count, and each row must sum to 1. `classCount` is how many classes each row scores. `options`
carries the strategy, the averaging, the label set, the sample weights and the worker count;
`default` is scikit-learn's own defaults, on one thread.

**Returns** — `double` in `[0, 1]`, larger meaning a better ranking. Under one-vs-rest with
`Averaging.Weighted`, sample weights whose class totals sum within `1e-8` of zero — every weight `0`
included — return `0` rather than a refusal, as `roc_auc_score` returns it: `_average_binary_score`
sums each class's weight, adds the totals with numpy's pairwise sum and answers `0` when `isclose`
finds that zero ([#1534](https://github.com/CyrilB1531/lodestar/issues/1534)). The totals are summed
the same way here: labels `[0, 1, 0, 2]` weighted `[1e16, -1e16, 1, 0]`, for instance, total `1` in
sample order and `0` by class ([#1586](https://github.com/CyrilB1531/lodestar/issues/1586)), and
both averages end in `np.average`'s pairwise sums.

**Exceptions**, in the order they are checked, which is `_multiclass_roc_auc_score`'s
([#1601](https://github.com/CyrilB1531/lodestar/issues/1601)):

1. `ArgumentException` when the averaging is `Averaging.Binary` — "The 'average' parameter of
   roc_auc_score must be a str among {'macro', 'micro', 'samples', 'weighted'} or None. Got 'binary'
   instead.", the options listed sorted where Python prints its set in any order — which
   scikit-learn's parameter validation refuses before any array is read.
2. `ArgumentOutOfRangeException` when `MultiClassRocOptions.MaxDegreeOfParallelism` is negative,
   then when `classCount` is below two — neither has a scikit-learn counterpart.
3. `ArgumentException` when `yTrue`, then `yScore`, is empty — "Found array with 0 sample(s)
   (shape=(0,))…" and "(shape=(0, k))…" ([#1593](https://github.com/CyrilB1531/lodestar/issues/1593)).
4. When a score is not finite — "Input contains NaN." or its infinity counterpart
   ([#1569](https://github.com/CyrilB1531/lodestar/issues/1569)).
5. When `yScore` does not hold `classCount` values per sample — the shape a flat span has to state.
6. When a row does not sum to 1 — "Target scores need to be probabilities for multiclass roc_auc,
   i.e. they should sum up to 1.0 over classes".
7. When the averaging is `Averaging.Micro` under one-vs-one — "average must be one of ('macro',
   'weighted', None) for multiclass problems". Under one-vs-rest, micro is the binary score of the
   raveled class matrix, each weight repeated across its classes, as scikit-learn computes it.
8. When `MultiClassRocOptions.Labels` repeats a label — "Parameter 'labels' must be unique" — is
   not ascending — "Parameter 'labels' must be ordered" — or does not hold `classCount` labels;
   when a `yTrue` label is outside it — "'y_true' contains labels not in parameter 'labels'"; or,
   without `Labels`, when `yTrue`'s distinct labels are not `classCount`.
9. When a sample weight is given under one-vs-one — "sample_weight is not supported for multiclass
   one-vs-one ROC AUC, 'sample_weight' must be None in this case." — then when there is not one
   weight per sample.
10. Under one-vs-rest, when some class holds both labels — a column of one label is `NaN` before its
    curve reads a weight — and a sample weight is not finite, or when every one is zero under
    `Averaging.Macro` or `Averaging.Micro` — under `Averaging.Weighted` the zero-total shortcut
    returns `0` first.
11. When a negative sample weight turns a class's false-positive rate back — "x is neither
    increasing nor decreasing", as `auc` refuses it, raised as the lowest such class's refusal on
    any number of workers. A class absent from `yTrue`, or whose negative class's weights total zero
    or less, is `NaN` instead; one whose positive class's weights total zero or less is `NaN` unless
    its false-positive rate turns back.
12. Under one-vs-one, when `k (k - 1) / 2` class pairs pass what one array holds, naming
    `classCount` ([#1480](https://github.com/CyrilB1531/lodestar/issues/1480)), or, weighted, when
    `yTrue` holds one class alone, which pairs with nothing — "Weights sum to zero, can't be
    normalized", as numpy raises it ([#1566](https://github.com/CyrilB1531/lodestar/issues/1566)).

**Example** — six samples over three classes, one probability row each.

```csharp
using Lodestar.Metrics;

int[] yTrue = [0, 1, 2, 2, 2, 1];
double[] yScore =
[
    0.6, 0.3, 0.1,
    0.3, 0.5, 0.2,
    0.2, 0.5, 0.3,
    0.1, 0.2, 0.7,
    0.4, 0.4, 0.2,
    0.2, 0.3, 0.5,
];

double auc = RocAuc.MultiClass(yTrue, yScore, 3);   // => 0.7824…
```

**Remarks** — a separate method rather than an overload of `RocAuc.Score`, because the two
parameter
lists would be indistinguishable to the C# compiler and a call like `Score(y, s, 3)` would stop
compiling in consumer code. Everything optional lives in `MultiClassRocOptions`.

Three traps, and the first two are about the shape of `yScore`. It is **probabilities, not
scores**:
each row has to sum to 1, and the call refuses it otherwise, so a raw logit or a decision-function
output has to go through a softmax first. And it is **row-major** — one sample's classes are
contiguous — which is the transpose of what you get from a column-per-class table; there is no
two-dimensional overload because a span cannot carry one.

The third is the class-to-column mapping. With `Labels` left empty the columns are matched to the
sorted distinct labels of `yTrue`, so a class the model knows about but this evaluation set
happens
not to contain will shift every later column. Pass `MultiClassRocOptions.Labels` whenever the
label
set comes from the model rather than from the data.

The exception behaviour is worth one line for anyone raising `MaxDegreeOfParallelism`: the
parallel
path rethrows the original exception instance — same type, message and `ParamName`, from the
lowest-numbered class or pair that failed — so a `catch` written against the sequential path keeps
working, and no `AggregateException` ever escapes.

**Applies to** — net10.0, netstandard2.0.

**See also** — `RocAuc.Score`, `MultiClassRocOptions`, `MultiClassStrategy`,
`docs/guides/performance.md`,
the [Python equivalence table](../../../equivalence.md).
