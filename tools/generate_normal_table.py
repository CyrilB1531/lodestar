"""Write Lodestar.Stats' erfcx table, src/Lodestar.Stats/Internal/Normal.Table.cs, from a 70-digit erfc.

erfcx(x) = e^(x^2) erfc(x) on [0, 27.3] is cut into INTERVALS intervals uniform in y = 4/(4 + x) and, on each, into a
polynomial of COEFFICIENTS terms in u = (x - mid) * scale, linear in x: Chebyshev interpolation at the interval's
first-kind nodes, converted to powers of u and rounded to doubles. erfc itself is the Maclaurin series of erf below 3
and Lentz's continued fraction above, in Python's decimal at 70 digits, so the table needs nothing but the standard
library. The run prints the largest relative gap between the rounded polynomials and erfcx over nine points an
interval; it read 1.2e-16 (#1756).

Usage:  python tools/generate_normal_table.py
        python tools/generate_normal_table.py --check   # exit 1 if the committed table differs
        python tools/generate_normal_table.py --help

Exit:   0 written or identical, 1 the committed table differs (--check), 2 bad usage
"""

from __future__ import annotations

import sys
from decimal import Decimal, getcontext
from pathlib import Path

getcontext().prec = 70

INTERVALS = 128
COEFFICIENTS = 9
CUTOFF = Decimal("27.3")
TARGET = Path(__file__).resolve().parent.parent / "src" / "Lodestar.Stats" / "Internal" / "Normal.Table.cs"
PI = Decimal("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899863")
SQRT_PI = PI.sqrt()
TOLERANCE = Decimal(10) ** -66


def erfc(x: Decimal) -> Decimal:
    """erfc at 70 digits: one less erf's series below 3, the continued fraction above."""
    if x < 0:
        return 2 - erfc(-x)
    if x < 3:
        total, power, n, square = Decimal(0), x, 0, x * x
        while True:
            term = power / (2 * n + 1)
            total += term
            if abs(term) < Decimal(10) ** -68:
                break
            n += 1
            power = -power * square / n
        return 1 - 2 * total / SQRT_PI
    tiny = Decimal(10) ** -300
    value, c, d, n = x, x, Decimal(0), 1
    while True:
        a = Decimal(n) / 2
        d = x + a * d
        d = 1 / d if d != 0 else 1 / tiny
        c = x + a / c
        delta = c * d
        value *= delta
        if abs(delta - 1) < TOLERANCE:
            break
        n += 1
    return (-x * x).exp() / SQRT_PI / value


def erfcx(x: Decimal) -> Decimal:
    return (x * x).exp() * erfc(x)


def cosine(t: Decimal) -> Decimal:
    t %= 2 * PI
    total, term, n = Decimal(0), Decimal(1), 0
    while abs(term) > Decimal(10) ** -68:
        total += term
        n += 2
        term = -term * t * t / (n * (n - 1))
    return total


def power_form(chebyshev: list[Decimal]) -> list[Decimal]:
    """T_0 = 1, T_1 = u, T_(m+1) = 2u T_m - T_(m-1), summed as powers of u."""
    previous = [Decimal(0)] * COEFFICIENTS
    current = [Decimal(0)] * COEFFICIENTS
    previous[0], current[1] = Decimal(1), Decimal(1)
    power = [Decimal(0)] * COEFFICIENTS
    power[0], power[1] = chebyshev[0], chebyshev[1]
    for m in range(2, COEFFICIENTS):
        following = [-previous[0]] + [2 * current[p - 1] - previous[p] for p in range(1, COEFFICIENTS)]
        for p in range(m + 1):
            power[p] += chebyshev[m] * following[p]
        previous, current = current, following
    return power


def build() -> tuple[list[tuple[float, float]], list[list[float]], Decimal]:
    lowest = Decimal(4) / (4 + CUTOFF)
    width = (1 - lowest) / INTERVALS
    nodes = [cosine(PI * (k + Decimal("0.5")) / COEFFICIENTS) for k in range(COEFFICIENTS)]
    bounds, table, worst = [], [], Decimal(0)
    for i in range(INTERVALS):
        low = float(Decimal(4) / (lowest + (i + 1) * width) - 4)
        high = 27.3 if i == 0 else float(Decimal(4) / (lowest + i * width) - 4)
        mid, scale = (low + high) / 2, 2.0 / (high - low)
        values = [erfcx(Decimal(mid) + node / Decimal(scale)) for node in nodes]
        chebyshev = [
            (1 if m == 0 else 2) * sum(values[k] * cosine(PI * m * (k + Decimal("0.5")) / COEFFICIENTS)
                                       for k in range(COEFFICIENTS)) / COEFFICIENTS
            for m in range(COEFFICIENTS)
        ]
        row = [float(c) for c in power_form(chebyshev)]
        for j in range(9):
            u = Decimal(-1) + Decimal(j) / 4
            approx = Decimal(0)
            for c in reversed(row):
                approx = approx * u + Decimal(c)
            exact = erfcx(Decimal(mid) + u / Decimal(scale))
            worst = max(worst, abs(approx / exact - 1))
        bounds.append((mid, scale))
        table.append(row)
    return bounds, table, worst


def render(bounds: list[tuple[float, float]], table: list[list[float]]) -> str:
    lines = [
        "namespace Lodestar.Stats.Internal;",
        "",
        "/// <summary>Normal's table, apart from its code so the duplication gate's exclusion covers literals alone; written",
        "/// by tools/generate_normal_table.py.</summary>",
        "internal static partial class Normal",
        "{",
        "    // Each interval's midpoint in x and the reciprocal of its half-width: its polynomial's variable is (x − mid) · scale.",
        "    private static readonly double[] Bounds =",
        "    [",
    ]
    lines += [f"        {mid!r}, {scale!r}," for mid, scale in bounds]
    lines += ["    ];", "", "    // Each interval's coefficients of erfcx, lowest power first.",
              "    private static readonly double[] Table =", "    ["]
    for row in table:
        current = "       "
        for value in row:
            piece = f" {value!r},"
            if len(current) + len(piece) > 120:
                lines.append(current)
                current = "       "
            current += piece
        lines.append(current)
    lines += ["    ];", "}"]
    return "\n".join(lines) + "\n"


def main(argv: list[str]) -> int:
    if argv and argv[0] in ("--help", "-h"):
        print(__doc__)
        return 0
    if argv and argv != ["--check"]:
        print(__doc__, file=sys.stderr)
        return 2
    bounds, table, worst = build()
    print(f"largest relative gap to erfcx: {float(worst):.2e}")
    text = render(bounds, table)
    if argv == ["--check"]:
        return 0 if TARGET.read_text(encoding="utf-8") == text else 1
    TARGET.write_text(text, encoding="utf-8", newline="\n")
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
