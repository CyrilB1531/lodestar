# From Python to .NET — migration inventory

This page is Lodestar's **migration hub**. It answers a simple question: "I do this
in Python, what do I do in C#?"

The project's guiding principle (see the
[rationale](https://github.com/CyrilB1531/lodestar/blob/main/README.md)) is
**honest**: we don't rewrite Python's data-science ecosystem. Most of it already
exists in .NET, and Python's dense linear algebra relies on Fortran BLAS/LAPACK
kernels there's no point reimplementing. We **use** what exists, and only **write**
native code where .NET has no maintained equivalent at the reference's parity.

Sixteen packages in, that gap has a shape. It is almost never the computation — .NET
ships those — and almost always the **apparatus around it**: the tokenizer loader and
not its encoder, the regression's inference table and not its coefficients, the
time-series diagnostics and not the forecast, sparse decomposition and not dense. Each
of those is a decision record with a reading behind it, linked from the rows below.

## The four columns

| Python | Role | .NET recommendation | Verdict |
| --- | --- | --- | --- |
| **PyTorch** | tensors, autograd, training, GPU | [TorchSharp](https://github.com/dotnet/TorchSharp) (= libtorch); [ONNX Runtime](https://onnxruntime.ai/) for inference only | ✅ **Use** |
| **matplotlib** | plotting | [ScottPlot](https://scottplot.net/), [Plotly.NET](https://plotly.net/), OxyPlot | ✅ **Use** |
| **NumPy** | N-dim arrays, dense algebra | [Math.NET Numerics](https://numerics.mathdotnet.com/) (+ native MKL/OpenBLAS provider); `System.Numerics.Tensors` | ✅ **Use** |
| **scikit-learn** | classical ML, pipelines, metrics | [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet); [SharpLearning](https://github.com/mdabros/SharpLearning) | ✅ **Use** *except* text vectorization → **Lodestar.Text**, classification metrics → **Lodestar.Metrics**, and the two sparse decompositions (`TruncatedSVD`, `NMF`) → **Lodestar.Decomposition** |
| **MAPIE** | conformal prediction: intervals and prediction sets with a coverage guarantee | none — no C# implementation exists | 🔴 **Write** — split conformal is **Lodestar.Conformal** |
| **pandas** | DataFrame, groupby, IO | [`Microsoft.Data.Analysis`](https://www.nuget.org/packages/Microsoft.Data.Analysis); [Deedle](https://fslab.org/Deedle/) | 🟡 **Use** (rougher) |
| **statsmodels** | econometric regression, time series, tests | Math.NET (basics) — *not* Accord.NET, see below; [`Microsoft.ML.TimeSeries`](https://www.nuget.org/packages/Microsoft.ML.TimeSeries) for forecasting | 🔴 **Write** — the tests and the OLS table ship as **Lodestar.Stats** and **Lodestar.Stats.Regression**; forecasting delegates; the GLM table ships beside the OLS one, and the time-series diagnostics are being written |
| **scipy.stats** | hypothesis tests, distributions, tails | [Math.NET Numerics](https://numerics.mathdotnet.com/) for the distributions and their tails | 🔴 **Write** — ten test families at scipy parity ship as **Lodestar.Stats**. Math.NET has the distributions and no test battery over them, so the gap is the test and not the tail; [decision 0082](../decisions/0082-scipy-joins-the-allowed-permissive-references.md) admitted scipy as a permissive reference and [0095](../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) says which four tail members this publishes for its neighbours |
| **lifelines** | survival analysis: Kaplan-Meier, Nelson-Aalen, log-rank | **none** — the largest void [#442](https://github.com/CyrilB1531/lodestar/issues/442) surveyed | 🔴 **Write** — right-censored estimators and the log-rank test ship as **Lodestar.Survival**, at lifelines parity. `scikit-survival` is the nearest reference in any language and is refused on its **licence**, not its capability — GPL-3.0-or-later, which [decision 0003](../decisions/0003-provenance-and-licensing.md) excludes ([0099](../decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)) |
| **seaborn** | tidy statistical viz | ScottPlot / Plotly.NET (charts rebuilt) | 🟠 **Decide** — statistical presets missing |

**Legend.** ✅ a solid equivalent exists, use it as is. 🟡 an equivalent exists but
is less mature than Python; expect some glue. 🟠 the foundation exists but a whole
area is missing: a candidate for native code *if your usage justifies it*.
🔴 **nothing exists** and this project wrote it.
⛔ **unmaintained** — do not reach for it, and the row says since when.

## Unmaintained, and why that is stated with dates

Pointing a reader at a library nobody has touched in years is worse than saying
nothing, so every ⛔ here carries **the last published package and the last commit,
both dated**. A verdict without them is a claim that rots quietly; with them, a
reader can check whether it is still true and this page can be corrected rather
than argued about.

| library | last package | last commit | reach for |
| --- | --- | --- | --- |
| **Accord.NET** | `Accord` 3.8.0, **19 October 2017** (3.8.2-alpha, November 2017, is the last of any kind) | **18 November 2020** | Math.NET Numerics for distributions and regression; [`Lodestar.Metrics`](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Metrics) for evaluation |
| **Pandas.NET** | `Pandas.NET` 0.6.0, **6 November 2023** | **4 August 2024**, deleting its CI workflow | `Microsoft.Data.Analysis`, or Deedle for time series |

Accord.NET is the one that matters, because it is still the first result for half
of these searches: the package a reader would install predates .NET Core 3.0.

## When calling Python is still the right answer

This page recommends .NET libraries because the project's thesis is that most of
the ecosystem already exists there. It is not that Python is never the answer. A
model that only exists as a Python package, a notebook workflow with a person in
it, a one-off analysis — for those, calling Python is right, and a migration guide
that pretended otherwise would not be trusted on the cases where it is not.

| option | shape | last release |
| --- | --- | --- |
| [**CSnakes**](https://github.com/tonybaloney/CSnakes) | source-generates typed C# from Python type hints; embeds CPython in the process | `CSnakes.Runtime` 1.2.1, August 2025 |
| [**Python.NET**](https://github.com/pythonnet/pythonnet) | dynamic interop, the long-standing option | `pythonnet` 3.1.0, May 2026 |

Both were active within the last month at the time of writing. What they cost is
what `Lodestar` exists to avoid where it can: a Python runtime to deploy and
version alongside the application, no ahead-of-time compilation to a single
artifact, and the GIL between your threads and theirs.

## What Lodestar writes natively

Text was the first area that justified native code, and it stopped being the only one
some time ago: **sixteen packages** ship now. Each lot below was opened by a reading of
what .NET already exports — never by an assumption that nothing existed — and the ones
that found an incumbent delegated instead. That protocol is
[decision 0074](../decisions/0074-the-phase-2-gaps-restated-on-what-the-packages-export.md),
and it has **replaced** a gap claim this project made about itself twice, so the readings
are worth more than the list.

1. **String distances & similarity** — Levenshtein, Damerau-Levenshtein,
   Jaro-Winkler, Jaccard, Ratcliff-Obershelp, phonetics… *(done)*
2. **Tokenization & sparse vectorization** — `CountVectorizer`, `TfidfVectorizer`
   (exact sklearn semantics), home-grown CSR matrix. *(done)*
3. **Embeddings & semantic search** — ONNX Runtime + sub-word tokenizers. *(done)*
   Native for one measured reason: [`Microsoft.ML.Tokenizers`](https://www.nuget.org/packages/Microsoft.ML.Tokenizers)
   builds every tokenizer from a vocabulary, a merges file or a `spiece.model`, and cannot
   read the `tokenizer.json` that Llama-2 and Mistral v0.1 actually ship. Its encode paths
   are faster than ours; the gap is the loader, not the arithmetic —
   [decision 0068](../decisions/0068-the-tokenizer-gap-is-the-loader-not-the-encode-kernel.md).
4. **Applied fuzzy matching** — `rapidfuzz.fuzz` / `process` equivalents. *(done)*
5. **Classification metrics** — sklearn-parity precision, recall, F1, confusion
   matrix, report and ROC-AUC. *(done)*
6. **Split conformal prediction** — MAPIE-parity intervals and prediction sets, with
   the finite-sample coverage guarantee and the exchangeability assumption it rests
   on. *(done)* The survey behind
   [#441](https://github.com/CyrilB1531/lodestar/issues/441) found **no C#
   implementation at all**, which is why this one is written rather than delegated;
   the guarantee's assumption leads
   [its guide](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/conformal.md)
   rather than closing it.
7. **Sparse truncated SVD and NMF** — scikit-learn-parity `TruncatedSVD` and
   `NMF(solver="mu")` over a `CsrMatrix`. *(done)* Math.NET is still the answer for
   dense linear algebra and this does not replace it, but its sparse SVD request has
   been open since 2013, and ML.NET's `ProjectToPrincipalComponents` centres the data,
   which densifies the very matrix the sparse representation exists to keep sparse —
   [decision 0072](../decisions/0072-omega-is-an-input-not-a-seed.md) and
   [its guide](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/decomposition.md).
8. **Classical hypothesis tests** — ten families at scipy parity, plus the four tail
   members [decision 0095](../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
   publishes for its neighbours. *(done, `Lodestar.Stats`)* Math.NET has the distributions
   and no battery of tests over them, so the gap is the test and not the tail.
9. **The OLS inference table** — standard errors, t and p values, confidence intervals,
   adjusted R², F and its p value, and the VIF, at statsmodels parity. *(done,
   `Lodestar.Stats.Regression`)* The clearest case of the shape above:
   [decision 0096](../decisions/0096-ordinary-least-squares-earns-its-own-package.md)'s
   reading found coefficients everywhere in .NET and **inference nowhere**, which replaced
   this project's own sentence that "nobody in .NET does inference".
10. **Right-censored survival** — Kaplan-Meier, Nelson-Aalen and the log-rank test at
   lifelines parity. *(done, `Lodestar.Survival`)* The largest void
   [#442](https://github.com/CyrilB1531/lodestar/issues/442) surveyed; `scikit-survival` is
   refused on its **licence** rather than its capability
   ([decision 0099](../decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)).
11. **Feature scaling** — fitted on arrays, applied to spans, at scikit-learn parity.
   *(done, `Lodestar.Preprocessing`)* ML.NET scales inside a pipeline over an `IDataView`;
   this is the same arithmetic with nothing between the caller and a `double[]`.
12. **k-means** — Lloyd's algorithm over a row-major span, at scikit-learn parity.
   *(done, `Lodestar.Cluster`)* Same reason as the scaling above, and the same measurement.
13. **GPU kernels** — ILGPU over device-resident matrices and text. *(done,
   `Lodestar.Gpu`)* The one package that does not ship `netstandard2.0`, and the one whose
   figures are **not** measured by CI: a hosted runner has no GPU, so the 5–10× gate is
   measured on a named machine and published by hand
   ([decision 0102](../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md)).
14. **The generalized linear model** — binomial and Poisson fitted by IRLS, with the same
   inference table entry 9 describes, at statsmodels parity. *(done,
   `Lodestar.Stats.Regression`)* A GLM exists in .NET three times and none of them is
   reachable: `Accord.Statistics` ships the whole stack under **LGPL-2.1** and archived in
   2017, `Microsoft.ML` reports coefficient statistics for binary logistic **only**, and
   `cs-glm` installs no assembly at all
   ([decision 0104](../decisions/0104-generalized-linear-models-are-written-natively.md)).

Three more packages carry no lot of their own. `Lodestar.Abstractions` holds the
`CsrMatrix` the others share; `Lodestar.Onnx` exists to carry the one dependency that is
its whole reason to be a package; `Lodestar.Extensions.AI` and `Lodestar.Extensions.MathNet`
convert to foreign types, which
[decision 0089](../decisions/0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md)
distinguishes from computing with them.

## Per-library guides

| Guide | Status |
| --- | --- |
| [NumPy → .NET](numpy.md) | draft |
| [pandas → .NET](pandas.md) | draft |
| [scikit-learn → .NET](sklearn.md) | draft |
| [statsmodels → .NET](statsmodels.md) | draft |
| [PyTorch → .NET](pytorch.md) | draft |
| [matplotlib → .NET](matplotlib.md) | draft |
| [seaborn → .NET](seaborn.md) | draft |

**There is no `scipy.md`, `lifelines.md` or `mapie.md`, and none is owed.** A per-library
guide earns its place where the answer is *another .NET library* and the reader needs the
glue and the pitfalls — NumPy to Math.NET has real traps, and the page is where they live.
For those three the verdict is **write**: the answer is a Lodestar package, the row above
says which and why, and [`equivalence.md`](../equivalence.md) maps each call to its
counterpart with the divergences named. A page between them would carry nothing of its own.

The **detailed equivalence table** (Python call → C# call, behavioral differences,
performance notes), filled in as we go, is in [`equivalence.md`](../equivalence.md).

> This document is not legal advice; third-party dependency licenses are recorded
> in [`THIRD-PARTY-NOTICES.md`](https://github.com/CyrilB1531/lodestar/blob/main/THIRD-PARTY-NOTICES.md).
