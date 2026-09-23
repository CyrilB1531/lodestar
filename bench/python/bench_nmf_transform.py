#!/usr/bin/env python3
"""Time scikit-learn's NMF.transform against Lodestar.Decomposition's (issue #1124).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks,
`compare-nmf-transform` mode) so the two are comparable:

  * same shapes, same operations, in the same order,
  * the same matrices, built by the same rule on both sides rather than read from a
    corpus file -- a committed corpus here would be a file holding that rule's output,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses
    next door,
  * the fit is built outside the timed region on both sides. What is measured is the
    transform, which is a factorization with H held fixed rather than a projection.

`solver='mu'` and `tol=0.0` are pinned: the multiplicative update is the only solver this
package implements, and a disabled early stop makes the iteration count an input rather
than a result on both sides.
"""

from __future__ import annotations

import sys
from importlib.metadata import version
from pathlib import Path

import numpy as np
import scipy.sparse as sp
from sklearn.decomposition import NMF

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-nmf-transform.json"

COLUMNS = 300
NON_ZEROS_PER_ROW = 12
FIT_ROWS = 1500
RANK = 15
ITERATIONS = 40

SIZES = [100, 500, 2000]


def corpus(rows: int, offset: int) -> sp.csr_matrix:
    """A sparse non-negative corpus from the row index alone. The C# harness's rule."""
    data, indices, indptr = [], [], [0]
    for row in range(rows):
        seed = row + offset
        pairs = sorted(
            (((seed * 31) + (j * 97)) % COLUMNS, 1.0 + ((seed + j) % 17) / 4.0)
            for j in range(NON_ZEROS_PER_ROW)
        )
        indices.extend(column for column, _ in pairs)
        data.extend(value for _, value in pairs)
        indptr.append(len(data))

    return sp.csr_matrix(
        (np.array(data), np.array(indices), np.array(indptr)), shape=(rows, COLUMNS))


def measure_loss(loss: str, tag: str) -> list[dict]:
    fitted = NMF(
        n_components=RANK, solver="mu", beta_loss=loss, init="nndsvda",
        max_iter=ITERATIONS, tol=0.0, random_state=1124).fit(corpus(FIT_ROWS, 0))

    results = []
    for rows in SIZES:
        unseen = corpus(rows, rows)
        results.append(measure(f"transform_{tag}_n{rows}", lambda u=unseen: fitted.transform(u)))

    return results


def main() -> None:
    print("Python NMF.transform bench")
    results: list[dict] = []
    for loss, tag in (("frobenius", "frobenius"), ("kullback-leibler", "kl")):
        results.extend(measure_loss(loss, tag))

    write(OUT, results, {"scikit-learn": version("scikit-learn"), "scipy": version("scipy")})


if __name__ == "__main__":
    main()
