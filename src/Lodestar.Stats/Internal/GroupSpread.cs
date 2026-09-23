namespace Lodestar.Stats.Internal;

/// <summary>The centre a group's spread is measured around, in the three shapes scipy offers.</summary>
internal static class GroupSpread
{
    /// <summary>The group's centre under <paramref name="center"/>.</summary>
    /// <param name="values">The group; at least one value.</param>
    /// <param name="center">Which centre to take.</param>
    /// <param name="proportionToCut">How much of each end <see cref="Center.Trimmed"/> drops.</param>
    internal static double Centre(double[] values, Center center, double proportionToCut) =>
        center switch
        {
            Center.Mean => Mean(values, 0, values.Length),
            Center.Median => Median(values),
            Center.Trimmed => TrimmedMean(values, proportionToCut),
            _ => throw new ArgumentOutOfRangeException(nameof(center), center, null),
        };

    /// <summary>The middle value, or the mean of the two middle ones.</summary>
    /// <remarks>
    /// Selected rather than sorted: Levene's default centre is the median, so this runs once per
    /// group on every call, and a full sort spends <c>n log n</c> where the median needs
    /// <c>n</c>. Measured at three groups of ten thousand (#1121), sorting left the test 1.41x
    /// behind Accord's; selecting puts it ahead. The values are copied first because the caller's
    /// array is its own.
    /// </remarks>
    internal static double Median(double[] values)
    {
        double[] buffer = (double[])values.Clone();
        int n = buffer.Length;
        int half = n / 2;

        if ((n % 2) == 1)
        {
            return Select(buffer, half);
        }

        // Everything right of the selected position is at least as large, so the upper middle
        // value is the smallest of them -- one scan rather than a second selection.
        double lower = Select(buffer, half - 1);
        double upper = buffer[half];
        for (int i = half + 1; i < n; i++)
        {
            if (buffer[i] < upper)
            {
                upper = buffer[i];
            }
        }

        return 0.5 * (lower + upper);
    }

    /// <summary>Puts the <paramref name="k"/>-th smallest value at index <paramref name="k"/>, and returns it.</summary>
    /// <remarks>
    /// Hoare's selection, with the pivot taken as the median of the first, middle and last
    /// values: that is what keeps an already sorted group -- which a caller may well hand over --
    /// off the quadratic path.
    /// </remarks>
    private static double Select(double[] buffer, int k)
    {
        int low = 0;
        int high = buffer.Length - 1;
        while (low < high)
        {
            (int left, int right) = Partition(buffer, low, high);
            if (k <= right)
            {
                high = right;
            }
            else if (k >= left)
            {
                low = left;
            }
            else
            {
                return buffer[k];
            }
        }

        return buffer[k];
    }

    /// <summary>Splits <c>[low, high]</c> about a pivot, returning the two cursors that crossed.</summary>
    private static (int Left, int Right) Partition(double[] buffer, int low, int high)
    {
        double pivot = MedianOfThree(buffer, low, high);
        int left = low;
        int right = high;
        while (left <= right)
        {
            while (buffer[left] < pivot)
            {
                left++;
            }
            while (buffer[right] > pivot)
            {
                right--;
            }

            if (left <= right)
            {
                (buffer[left], buffer[right]) = (buffer[right], buffer[left]);
                left++;
                right--;
            }
        }

        return (left, right);
    }

    private static double MedianOfThree(double[] buffer, int low, int high)
    {
        double first = buffer[low];
        double middle = buffer[low + ((high - low) / 2)];
        double last = buffer[high];

        if (first > middle)
        {
            (first, middle) = (middle, first);
        }
        if (middle > last)
        {
            middle = first > last ? first : last;
        }

        return middle;
    }

    /// <summary>The mean of what is left after dropping each end, as <c>scipy.stats.trim_mean</c> does.</summary>
    /// <remarks>
    /// The count dropped is <c>int(n · proportionToCut)</c>, truncated — so at five values and the
    /// 0.05 default nothing is dropped at all and this returns the plain mean, which is scipy's
    /// behaviour rather than an approximation of it.
    /// </remarks>
    internal static double TrimmedMean(double[] values, double proportionToCut)
    {
        double[] sorted = (double[])values.Clone();
        Array.Sort(sorted);

        int cut = (int)(sorted.Length * proportionToCut);
        int kept = sorted.Length - (2 * cut);
        if (kept <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(proportionToCut),
                proportionToCut,
                $"Trimming {cut} values from each end of {sorted.Length} leaves nothing to average.");
        }

        return Mean(sorted, cut, kept);
    }

    private static double Mean(double[] values, int start, int length)
    {
        double sum = 0.0;
        for (int i = start; i < start + length; i++)
        {
            sum += values[i];
        }

        return sum / length;
    }
}
