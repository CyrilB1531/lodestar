# Shared regression types — `Lodestar.Stats.Regression`

The data types more than one estimator family reads or returns: the test every family reports its
χ² and F statistics as, and the lag windows the kernel covariances weight with. Like every public
data type of the packages they are compiled into `Lodestar.Abstractions`
([decision 0003](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)) and
forwarded from here.

## Types

| Type | What it is |
| --- | --- |
| [`WaldTest`](common/waldtest.md) | A statistic, its p-value and its degrees of freedom. |
| [`KernelType`](common/kerneltype.md) | Bartlett, Parzen or quadratic spectral. |

## See also

- [Instrumental variables](iv.md) and [panel regression](panel.md), which use them.
