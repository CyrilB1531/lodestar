namespace Lodestar.Sample;

/// <summary>Freireich's leukaemia trial, the data every survival text opens with.</summary>
/// <remarks>
/// Shared by the samples above rather than repeated in each: the same 21 subjects per arm,
/// twelve of the treatment arm censored, every control duration observed.
/// </remarks>
internal static class Trial
{
    public static double[] TreatmentDurations { get; } =
        [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];

    public static bool[] TreatmentObserved { get; } =
        [true, true, true, true, true, true, true, true, true,
         false, false, false, false, false, false, false, false, false, false, false, false];

    public static double[] ControlDurations { get; } =
        [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];

    public static bool[] ControlObserved { get; } = [.. Enumerable.Repeat(true, 21)];
}
