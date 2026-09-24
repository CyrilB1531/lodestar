#!/usr/bin/env python3
"""Assert the shipped NuGet dependency graph is exactly the intended one.

`dotnet pack` derives a package's ``<dependencies>`` from whatever the project
resolved at restore time, so the graph consumers see is a *build output*, not
something anyone wrote down. That makes it easy to change by accident: a
PackageReference added for a compile-time helper without ``PrivateAssets=all``
lands in it, and a dependency that quietly disappears is worse still — the
package installs and then fails at run time on a missing assembly.

The expected graph below is the written-down version. It is deliberately exact:
an unexpected dependency fails just as loudly as a missing one, and so does a
dependency whose declared version range has moved — an edge with the wrong floor
is a different edge, however right its id looks.

Usage:  python tools/check_nuspec_dependencies.py <artifacts-directory> [--require-all]

``--require-all`` additionally fails when a known package is absent, which is
what CI wants after packing all eighteen. A release job packs exactly one package,
so it omits the flag.

``EXPECTED`` states the intended graph directly: ``Lodestar.Text`` carries
nothing on ``net10.0`` (the dependency-free core) and only
``System.Text.Json`` on ``netstandard2.0`` (the one deliberate exception, so
persisting a fitted model does not mean hand-rolling a JSON writer);
``Lodestar.Fuzzy`` depends on ``Lodestar.Text`` because ``Fuzz.Ratio`` is built
on ``Indel``, and since 0.5.0 ``Lodestar.Text`` depends on
``Lodestar.Abstractions`` because that is where ``CsrMatrix`` moved --
``Lodestar.Decomposition`` depends on ``Lodestar.Abstractions`` the same way,
for the same matrix -- ``Lodestar.Onnx`` depends on ``Lodestar.Embeddings``
for the tokenizers and the pooling it feeds a session with, and
``Lodestar.Extensions.AI`` depends on both of those: on ``Lodestar.Onnx`` for the
embedder it adapts, and on ``Lodestar.Embeddings`` because its constructor names
``BatchEncoder``, which is the ``EmbedBatch`` overload that owns the padding, and
``Lodestar.Stats.Regression`` depends on ``Lodestar.Stats`` for the Student and Fisher
tails and on ``Lodestar.Decomposition`` for the Householder QR -- the four members
decision 0003 published for it. The prose above walks the first of the twenty-seven edges;
``EXPECTED`` below is the authority for all of them. The ranges are asserted too, not
only the ids: a bare ``"0.2.0"`` is NuGet's shorthand for ``[0.2.0, )``, and an
edge with the wrong floor is a different edge.

Each external dependency appears exactly once: ``Microsoft.ML.OnnxRuntime`` under
``Lodestar.Onnx``, ``Microsoft.Extensions.AI.Abstractions`` under
``Lodestar.Extensions.AI``, ``MathNet.Numerics`` under
``Lodestar.Extensions.MathNet``. That is the tier rule of #533, restated by decision
0003 -- a core package carries no external dependency, and an external dependency
earns its own satellite named for it -- in assertable form, and decision 0003 adds
that an interop satellite may take a dependency a core package refused. This file is
what fails when one reappears where it should not.
"""

from __future__ import annotations

import pathlib
import sys
import xml.etree.ElementTree as ET
import zipfile

NET = "net10.0"
NETSTANDARD = ".NETStandard2.0"
# Lodestar.Gpu alone: ILGPU publishes no netstandard2.0 asset and does publish this one.
NETSTANDARD21 = ".NETStandard2.1"

TEXT = "Lodestar.Text"
FUZZY = "Lodestar.Fuzzy"
EMBEDDINGS = "Lodestar.Embeddings"
METRICS = "Lodestar.Metrics"
ABSTRACTIONS = "Lodestar.Abstractions"
CONFORMAL = "Lodestar.Conformal"
DECOMPOSITION = "Lodestar.Decomposition"
STATS = "Lodestar.Stats"
STATS_REGRESSION = "Lodestar.Stats.Regression"
SURVIVAL = "Lodestar.Survival"
STATS_TIMESERIES = "Lodestar.Stats.TimeSeries"
PREPROCESSING = "Lodestar.Preprocessing"
CLUSTER = "Lodestar.Cluster"
ONNX = "Lodestar.Onnx"
EXTENSIONS_AI = "Lodestar.Extensions.AI"
EXTENSIONS_VECTORDATA = "Lodestar.Extensions.VectorData"
EXTENSIONS_MATHNET = "Lodestar.Extensions.MathNet"
GPU = "Lodestar.Gpu"
ILGPU = "ILGPU"
ONNX_RUNTIME = "Microsoft.ML.OnnxRuntime"
MS_EXTENSIONS_AI = "Microsoft.Extensions.AI.Abstractions"
MS_VECTORDATA = "Microsoft.Extensions.VectorData.Abstractions"
MATHNET = "MathNet.Numerics"
STJ = "System.Text.Json"

# Span/Memory/Vector<T> are in-box on net10.0, packaged on netstandard2.0 --
# every package carries this pair there only. Floors match what STJ 10.0.x needs.
POLYFILLS = {"System.Memory": "4.6.3", "System.Numerics.Vectors": "4.6.1"}

# netstandard2.0-only, two-package status: docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md.
PERSISTENCE = {STJ: "10.0.12"}

# long-comment: Directory.Packages.props' PackageVersion, one per sibling a package reaches, and
# why the Lodestar floors moved together on 2026-09-24. A PackageReference emits this floor, but
# LodestarUseProjectRefs emits the sibling's own version instead, which is how a build left in the
# developer loop is caught. #1142 moved every package's public data types into
# Lodestar.Abstractions 0.2.0 and forwards them from the package that declared them, so a floor on
# a release from before the move, beside Lodestar.Abstractions 0.2.0, declares the same type
# twice (CS0433). Each sibling that moved types is floored at the release that forwards them.

# Lodestar.Abstractions, Lodestar.Cluster and Lodestar.Stats.Regression all sit at their second
# minor, named once rather than spelled three times (S1192).
SECOND_MINOR = "0.2.0"
ABSTRACTIONS_FLOOR = SECOND_MINOR
TEXT_FLOOR = "0.7.0"
STATS_FLOOR = "0.5.0"
DECOMPOSITION_FLOOR = "0.3.0"
CLUSTER_FLOOR = SECOND_MINOR
# 0.6.0 was already the lowest possible: 0.5.0 still declares OnnxRuntime.
EMBEDDINGS_FLOOR = "0.8.0"

# Lodestar.Onnx moved no type, so 0.1.0 still holds: its first release, and OnnxTextEmbedder has
# been public since it.
ONNX_FLOOR = "0.1.0"

# 0.1.0 declares OlsOptions.WithIntercept with a plain setter where 0.2.0 has an init one, so a
# build against 0.1.0 throws MissingMethodException beside 0.2.0 (#671).
STATS_REGRESSION_FLOOR = SECOND_MINOR

# Directory.Packages.props' PackageVersion for both Microsoft.Extensions.*.Abstractions
# pins: Extensions.AI and Extensions.VectorData agree on it without a range to reconcile.
MS_ABSTRACTIONS_FLOOR = "10.10.0"

# package id -> target framework -> {dependency id: declared version range}.
# See this module's docstring for what EXPECTED's shape and ranges prove.
EXPECTED: dict[str, dict[str, dict[str, str]]] = {
    # netstandard2.1 because ILGPU ships no 2.0 asset; no sibling may depend on it. Its one
    # Lodestar edge is to the data types it forwards (#1142).
    GPU: {
        NET: {ILGPU: "1.5.3", ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD21: {ILGPU: "1.5.3", ABSTRACTIONS: ABSTRACTIONS_FLOOR},
    },
    ABSTRACTIONS: {
        # A sparse matrix and its products serialise nothing, so no System.Text.Json
        # here: persistence stays in the packages that persist.
        NET: {},
        NETSTANDARD: {**POLYFILLS},
    },
    TEXT: {
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS, **PERSISTENCE},
    },
    FUZZY: {
        NET: {TEXT: TEXT_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {TEXT: TEXT_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    EMBEDDINGS: {
        # Nothing external since 0.6.0, when ONNX Runtime left with OnnxTextEmbedder; one
        # Lodestar edge, to the data types it forwards (#1142).
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS, **PERSISTENCE},
    },
    ONNX: {
        # The repository's only external dependency, and the only package that
        # carries one. That is what makes this package worth its release checklist.
        NET: {EMBEDDINGS: EMBEDDINGS_FLOOR, ONNX_RUNTIME: "1.30.0"},
        NETSTANDARD: {EMBEDDINGS: EMBEDDINGS_FLOOR, ONNX_RUNTIME: "1.30.0", **POLYFILLS},
    },
    EXTENSIONS_AI: {
        # The first interop package, and the second external dependency. Two Lodestar edges:
        # the embedder it adapts, and the package whose BatchEncoder its constructor names.
        NET: {ONNX: ONNX_FLOOR, EMBEDDINGS: EMBEDDINGS_FLOOR, MS_EXTENSIONS_AI: MS_ABSTRACTIONS_FLOOR},
        NETSTANDARD: {
            ONNX: ONNX_FLOOR,
            EMBEDDINGS: EMBEDDINGS_FLOOR,
            MS_EXTENSIONS_AI: MS_ABSTRACTIONS_FLOOR,
            **POLYFILLS,
        },
    },
    EXTENSIONS_VECTORDATA: {
        # The third interop package: two Lodestar edges, one per half of hybrid search --
        # Embeddings for the vectors, Text for the BM25 index and the fusion.
        NET: {EMBEDDINGS: EMBEDDINGS_FLOOR, TEXT: TEXT_FLOOR, MS_VECTORDATA: MS_ABSTRACTIONS_FLOOR},
        NETSTANDARD: {
            EMBEDDINGS: EMBEDDINGS_FLOOR,
            TEXT: TEXT_FLOOR,
            MS_VECTORDATA: MS_ABSTRACTIONS_FLOOR,
            **POLYFILLS,
        },
    },
    EXTENSIONS_MATHNET: {
        # The second interop package. One Lodestar edge, to the package that owns CsrMatrix,
        # because converting that type is the whole of this package's surface.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, MATHNET: "5.0.0"},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, MATHNET: "5.0.0", **POLYFILLS},
    },
    CLUSTER: {
        # One Lodestar edge, to the data types it forwards (#1142): Lloyd's algorithm is
        # arithmetic over spans, and the scoring half lives in Lodestar.Metrics.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    PREPROCESSING: {
        # Three Lodestar edges and nothing external, which keeps this core tier: the normal
        # quantile (0138), the CsrMatrix (0139), and KBinsDiscretizer's Lloyd (1122).
        NET: {STATS: STATS_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR, CLUSTER: CLUSTER_FLOOR},
        NETSTANDARD: {
            STATS: STATS_FLOOR,
            ABSTRACTIONS: ABSTRACTIONS_FLOOR,
            CLUSTER: CLUSTER_FLOOR,
            **POLYFILLS,
        },
    },
    METRICS: {
        # One Lodestar edge, to the data types it forwards (#1142): metrics are pure span
        # computation, no I/O to serialise, so no System.Text.Json.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    CONFORMAL: {
        # The same shape, for the same reason: split conformal prediction is
        # arithmetic over spans, with no model and nothing to serialise.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    STATS_REGRESSION: {
        # Two Lodestar edges and nothing external, which is what keeps this core tier:
        # the tails that make a p-value, and the QR that solves without squaring XtX.
        NET: {STATS: STATS_FLOOR, DECOMPOSITION: DECOMPOSITION_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {
            STATS: STATS_FLOOR,
            DECOMPOSITION: DECOMPOSITION_FLOOR,
            ABSTRACTIONS: ABSTRACTIONS_FLOOR,
            **POLYFILLS,
        },
    },
    STATS_TIMESERIES: {
        # Two core edges: the tails from Lodestar.Stats, and the per-lag least-squares fits of the
        # augmented Dickey-Fuller test from Lodestar.Stats.Regression -- the edge that earned the package.
        NET: {STATS: STATS_FLOOR, STATS_REGRESSION: STATS_REGRESSION_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {
            STATS: STATS_FLOOR,
            STATS_REGRESSION: STATS_REGRESSION_FLOOR,
            ABSTRACTIONS: ABSTRACTIONS_FLOOR,
            **POLYFILLS,
        },
    },
    SURVIVAL: {
        # One Lodestar edge and nothing external, which keeps this core tier: the
        # chi-squared tail and the normal quantile, published for it (0097, 0098).
        NET: {STATS: STATS_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {STATS: STATS_FLOOR, ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    DECOMPOSITION: {
        # The one edge of this package, and the reason Lodestar.Abstractions exists:
        # CsrMatrix and its two dense-block products, with no Lodestar.Text behind them.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
    STATS: {
        # One Lodestar edge, to the data types it forwards (#1142); otherwise arithmetic over
        # arrays, with tail probabilities computed here, not fetched.
        NET: {ABSTRACTIONS: ABSTRACTIONS_FLOOR},
        NETSTANDARD: {ABSTRACTIONS: ABSTRACTIONS_FLOOR, **POLYFILLS},
    },
}

NUSPEC_NS = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"


def read_nuspec(nupkg: pathlib.Path) -> ET.Element:
    """Return the parsed .nuspec, or fail with a message naming the package.

    A package that cannot be read is a failure like any other, and it earns the
    same attributable one-line message as a wrong dependency: an unreadable
    archive here means `dotnet pack` produced something no consumer can install.
    Left to propagate, these surface as a bare traceback under a step named for
    the dependency graph, which describes neither the file nor the problem.
    """
    try:
        with zipfile.ZipFile(nupkg) as archive:
            names = [n for n in archive.namelist() if n.endswith(".nuspec")]
            if not names:
                raise SystemExit(f"{nupkg.name}: no .nuspec inside the package")
            content = archive.read(names[0])
    except zipfile.BadZipFile as error:
        raise SystemExit(f"{nupkg.name}: not a readable package ({error})") from error

    try:
        return ET.fromstring(content)
    except ET.ParseError as error:
        raise SystemExit(f"{nupkg.name}: malformed .nuspec ({error})") from error


def read_graph(nupkg: pathlib.Path) -> tuple[str, str, dict[str, dict[str, str]]]:
    """Return (id, version, {target framework: {dependency id: version range}})."""
    root = read_nuspec(nupkg)

    metadata = root.find(f"{{{NUSPEC_NS}}}metadata")
    if metadata is None:
        raise SystemExit(f"{nupkg.name}: no <metadata> element")

    package_id = metadata.findtext(f"{{{NUSPEC_NS}}}id", default="")
    version = metadata.findtext(f"{{{NUSPEC_NS}}}version", default="")

    graph: dict[str, dict[str, str]] = {}
    for group in metadata.iterfind(
        f"{{{NUSPEC_NS}}}dependencies/{{{NUSPEC_NS}}}group"
    ):
        framework = group.get("targetFramework", "")
        graph[framework] = {
            dependency.get("id", ""): dependency.get("version", "")
            for dependency in group.iterfind(f"{{{NUSPEC_NS}}}dependency")
        }
    return package_id, version, graph


def parse_arguments(arguments: list[str]) -> tuple[pathlib.Path, bool] | None:
    """Return (artifacts directory, require-all), or None on a usage error."""
    require_all = False
    positional: list[str] = []
    for argument in arguments:
        if argument == "--require-all":
            require_all = True
        elif argument.startswith("--"):
            return None
        else:
            positional.append(argument)

    if len(positional) != 1:
        return None
    return pathlib.Path(positional[0]), require_all


def describe(dependencies: dict[str, str]) -> str:
    """Render a dependency group the way the failure message should read it."""
    if not dependencies:
        return "none"
    return ", ".join(f"{name} {range_}" for name, range_ in sorted(dependencies.items()))


def check_package(nupkg: pathlib.Path) -> tuple[str, str, list[str]]:
    """Return the package id, its version, and every way its graph differs."""
    package_id, version, actual = read_graph(nupkg)

    expected = EXPECTED.get(package_id)
    if expected is None:
        return package_id, version, [f"{package_id}: not in the expected table"]

    if actual.keys() != expected.keys():
        return (
            package_id,
            version,
            [
                f"{package_id} {version}: dependency groups are "
                f"{sorted(actual)}, expected {sorted(expected)}"
            ],
        )

    return (
        package_id,
        version,
        [
            f"{package_id} {version} [{framework}]: dependencies are "
            f"{describe(actual[framework])}, expected {describe(dependencies)}"
            for framework, dependencies in expected.items()
            if actual[framework] != dependencies
        ],
    )


def main(argv: list[str]) -> int:
    parsed = parse_arguments(argv[1:])
    if parsed is None:
        print(__doc__, file=sys.stderr)
        return 2
    artifacts, require_all = parsed

    packages = sorted(artifacts.glob("*.nupkg"))
    if not packages:
        print(f"error: no .nupkg found in {artifacts}", file=sys.stderr)
        return 1

    failures: list[str] = []
    seen: set[str] = set()

    for nupkg in packages:
        package_id, version, package_failures = check_package(nupkg)
        seen.add(package_id)
        failures.extend(package_failures)
        if not package_failures:
            print(f"ok  {package_id} {version}")

    if require_all:
        failures.extend(
            f"{missing}: expected a package, none was packed"
            for missing in sorted(EXPECTED.keys() - seen)
        )

    for failure in failures:
        print(f"::error::{failure}")
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
