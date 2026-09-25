using Lodestar.Conformal;

namespace Lodestar.Sample;

/// <summary>CV+ over three folds, the jackknife-after-bootstrap over three bootstrap models, and the gamma score.</summary>
internal static class CrossConformalSample
{
    public static void Run()
    {
        Console.WriteLine("Cross-conformal prediction (Lodestar.Conformal)");

        int[] folds = [0, 0, 1, 1, 2, 2];
        double[] yTrue = [10.2, 11.5, 9.8, 12.6, 11.1, 10.4];
        double[] outOfFold = [10.7, 10.5, 10.0, 11.8, 11.4, 11.0];
        double[] atTest = [10.0, 11.0, 12.0];

        double[] scores = SplitConformal.AbsoluteResiduals(yTrue, outOfFold);
        (double plusLow, double plusHigh) = CrossConformal.Interval(atTest, folds, scores, 0.3);
        (double minMaxLow, double minMaxHigh) = CrossConformal.Interval(atTest, folds, scores, 0.3, CrossConformalMethod.MinMax);
        Console.WriteLine($"  CV+ interval         : [{Inv.F4(plusLow)}, {Inv.F4(plusHigh)}]");
        Console.WriteLine($"  min-max interval     : [{Inv.F4(minMaxLow)}, {Inv.F4(minMaxHigh)}]");

        double[] gammaScores = SplitConformal.GammaScores(yTrue, outOfFold);
        (double gammaLow, double gammaHigh) = CrossConformal.GammaInterval(atTest, folds, gammaScores, 0.3);
        (double splitLow, double splitHigh) = SplitConformal.GammaInterval(11.0, gammaScores, 0.3);
        Console.WriteLine($"  CV+ gamma interval   : [{Inv.F4(gammaLow)}, {Inv.F4(gammaHigh)}]");
        Console.WriteLine($"  split gamma interval : [{Inv.F4(splitLow)}, {Inv.F4(splitHigh)}]");

        double[] trainPredictions = [10.1, 10.4, 9.9, 11.2, 11.0, 11.6, 9.5, 9.8, 9.7, 12.0, 12.4, 12.1];
        bool[] heldOut = [true, true, true, false, true, true, true, true, false, false, false, true];
        double[] outOfBag = CrossConformal.OutOfSample(trainPredictions, heldOut, 3, CrossConformalAggregation.Median);
        double[] bagScores = SplitConformal.AbsoluteResiduals([10.3, 11.9, 9.2, 12.8], outOfBag);
        double[] bagTest = [10.5, 10.9, 11.2];
        (double bagLow, double bagHigh) = CrossConformal.Interval(
            bagTest, heldOut, bagScores, 0.4, CrossConformalMethod.Plus, CrossConformalAggregation.Median);
        (double gammaBagLow, double gammaBagHigh) = CrossConformal.GammaInterval(
            bagTest, heldOut, SplitConformal.GammaScores([10.3, 11.9, 9.2, 12.8], outOfBag), 0.4);
        Console.WriteLine($"  bootstrap, median    : [{Inv.F4(bagLow)}, {Inv.F4(bagHigh)}], gamma [{Inv.F4(gammaBagLow)}, {Inv.F4(gammaBagHigh)}]");
        Console.WriteLine();
    }
}
