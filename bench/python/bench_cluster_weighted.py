#!/usr/bin/env python3
"""Time scikit-learn's weighted KMeans and DBSCAN, and n_init over given starts, against Lodestar.Cluster (#1163).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-cluster-weighted` mode):

  * the same blobs and weights on both sides, built from the row and column index by one formula,
  * the same starts, rows picked by index, so both sides run the same Lloyd iterations,
  * n_init over given starts through a callable init, which scikit-learn hands the centred matrix,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
from sklearn.cluster import DBSCAN, KMeans

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-cluster-weighted.json"

KMEANS_FEATURES = 8
CLUSTERS = 16
STARTS = 4
KMEANS_SIZES = [10_000, 100_000]
DBSCAN_SIZES = [5_000, 20_000]
EPSILON = 0.3
MINIMUM = 10


def blobs(n: int, features: int, count: int) -> np.ndarray:
    """The C# side's formula: row i sits in blob i % count, jittered by a hash of its index."""
    i = np.arange(n, dtype=np.int64)[:, None]
    j = np.arange(features, dtype=np.int64)[None, :]
    centre = ((i % count) * 7919 + j * 104729) % 97 / 97.0 * 20.0 - 10.0
    jitter = ((i * 2654435761 + j * 40503) % 10007) / 10007.0 - 0.5
    return centre + 2.0 * jitter


def weights(n: int) -> np.ndarray:
    """Whole weights from one to five, as deduplicated rows would carry."""
    return (1 + (np.arange(n, dtype=np.int64) * 37) % 5).astype(np.float64)


def start(x: np.ndarray, index: int) -> np.ndarray:
    """Start `index`: CLUSTERS rows picked by a stride, distinct for every size here."""
    rows = (np.arange(CLUSTERS, dtype=np.int64) * 131 + index * 17) % len(x)
    return x[rows].copy()


def restarts(x: np.ndarray, w: np.ndarray) -> KMeans:
    mean = x.mean(axis=0)
    queue = [start(x, s) - mean for s in range(STARTS)]

    def init(*_args, **_kwargs):
        return queue.pop(0)

    return KMeans(CLUSTERS, init=init, n_init=STARTS, random_state=0, algorithm="lloyd").fit(x, sample_weight=w)


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python weighted cluster bench")
    results: list[dict] = []
    for n in KMEANS_SIZES:
        x, w = blobs(n, KMEANS_FEATURES, CLUSTERS), weights(n)
        results.append(measure(f"kmeans_weighted_{n}", lambda x=x, w=w: KMeans(
            CLUSTERS, init=start(x, 0), n_init=1, random_state=0, algorithm="lloyd").fit(x, sample_weight=w)))
        results.append(measure(f"kmeans_restarts_{n}", lambda x=x, w=w: restarts(x, w)))
    for n in DBSCAN_SIZES:
        x, w = blobs(n, 2, 10), weights(n)
        results.append(measure(f"dbscan_weighted_{n}", lambda x=x, w=w: DBSCAN(
            eps=EPSILON, min_samples=MINIMUM).fit(x, sample_weight=w)))
    write(OUT, results, {"scikit-learn": version("scikit-learn"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
