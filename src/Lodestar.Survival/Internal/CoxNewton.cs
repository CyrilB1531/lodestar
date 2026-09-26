namespace Lodestar.Survival.Internal;

/// <summary>A log partial likelihood with its score and observed information, on standardised covariates.</summary>
internal interface IPartialLikelihood
{
    /// <summary>Evaluates the likelihood, filling the gradient and the negated Hessian.</summary>
    double Evaluate(ReadOnlySpan<double> coefficients, Span<double> score, Span<double> information);
}

/// <summary>How a Newton fit ended.</summary>
internal enum CoxOutcome
{
    Converged,
    Collinear,
    Separated,
    Exhausted,

    /// <summary>lifelines' own loop stopped on a failure rule.</summary>
    GaveUp,
}

/// <summary>A fit on the standardised scale: the coefficients, the penalised log-likelihood, the null one and the penalised information.</summary>
internal sealed record CoxFit(CoxOutcome Outcome, double[] Coefficients, double LogLikelihood, double NullLogLikelihood, double[] Information);

/// <summary>lifelines' elastic net on the standardised coefficients, <c>n · λ · Σ (l1 · softabs(β, a) + (1 − l1) · β² / 2)</c>.</summary>
/// <remarks>
/// <c>softabs(β, a) = (logaddexp(0, −aβ) + logaddexp(0, aβ)) / a</c> tends to <c>|β|</c> as <c>a</c> grows; lifelines
/// sharpens it at every Newton step, <c>a = 1.3^i</c> for the proportional hazards fit and <c>1.5^i</c> for the
/// time-varying one. The derivatives are autograd's, written out.
/// </remarks>
internal sealed record ElasticNet(double Scale, double Penalizer, double L1Ratio)
{
    public bool IsZero => !(Penalizer > 0.0);

    public double Value(ReadOnlySpan<double> beta, double sharpness)
    {
        double total = 0.0;
        foreach (double b in beta)
        {
            double soft = (LogAddExp(-sharpness * b) + LogAddExp(sharpness * b)) / sharpness;
            total += Penalizer * ((L1Ratio * soft) + (0.5 * (1.0 - L1Ratio) * b * b));
        }

        return Scale * total;
    }

    /// <summary>Subtracts the penalty's gradient from <paramref name="score"/> and adds its curvature to the information's diagonal.</summary>
    public void Apply(ReadOnlySpan<double> beta, double sharpness, Span<double> score, Span<double> information)
    {
        int p = beta.Length;
        for (int j = 0; j < p; j++)
        {
            double up = Logistic(sharpness * beta[j]);
            double down = Logistic(-sharpness * beta[j]);
            double gradient = (L1Ratio * (up - down)) + ((1.0 - L1Ratio) * beta[j]);
            double curvature = (L1Ratio * sharpness * ((up * (1.0 - up)) + (down * (1.0 - down)))) + (1.0 - L1Ratio);
            score[j] -= Scale * Penalizer * gradient;
            information[(j * p) + j] += Scale * Penalizer * curvature;
        }
    }

    /// <summary><c>log(1 + eˣ)</c> without overflow, numpy's <c>logaddexp(0, x)</c>.</summary>
    private static double LogAddExp(double x) => Math.Max(0.0, x) + Log1P(Math.Exp(-Math.Abs(x)));

    /// <summary>autograd's derivative of <c>logaddexp(0, x)</c>: <c>exp(x − logaddexp(0, x))</c>.</summary>
    private static double Logistic(double x) => Math.Exp(x - LogAddExp(x));

    /// <summary><c>log(1 + x)</c> for <c>0 ≤ x ≤ 1</c>, exact to rounding: Goldberg's correction of the rounded sum.</summary>
    private static double Log1P(double x)
    {
        double u = 1.0 + x;
        // S1244: an exactly unrounded sum is the case the correction divides by zero in.
#pragma warning disable S1244
        return u == 1.0 ? x : Math.Log(u) * x / (u - 1.0);
#pragma warning restore S1244
    }
}

/// <summary>The two ways a Cox fit reaches its coefficients: to the optimum, or along lifelines' own loop.</summary>
internal static class CoxNewton
{
    // The largest step component below which the fit has converged. Four or five iterations reach it on
    // every fixture of the corpus, from coefficients at zero.
    private const double StepTolerance = 1e-10;

    // Past this many units of log hazard ratio in one step, a flat likelihood is read as a
    // coefficient running away rather than as slow convergence.
    private const double RunawayStep = 0.5;

    // A separated fit "converges" once the score underflows to zero: on five subjects the
    // information fell to 3e-16 of its value at zero, where a strong finite effect kept 0.1.
    private const double CollapsedInformation = 1e-10;

    /// <summary>Newton-Raphson from zero to the maximum of the likelihood less a ridge penalty.</summary>
    /// <remarks>
    /// A singular information while the likelihood is still rising is a coefficient running to
    /// infinity; a singular one that is not is a covariate the others already determine.
    /// </remarks>
    internal static CoxFit Optimum(IPartialLikelihood likelihood, int p, ElasticNet penalty, int maximumIterations)
    {
        double[] coefficients = new double[p];
        double[] score = new double[p];
        double[] information = new double[p * p];
        // At zero the ridge penalty is zero, so this is the null likelihood, with the penalty's curvature folded in.
        double nullLogLikelihood = Penalised(likelihood, coefficients, penalty, 1.0, score, information);
        double[] initial = Diagonal(information, p);
        double logLikelihood = nullLogLikelihood;
        for (int iteration = 1; iteration <= maximumIterations; iteration++)
        {
            if (!Cholesky.TryFactor(information, p, out double[] lower))
            {
                return Ended(iteration > 1 && logLikelihood > nullLogLikelihood ? CoxOutcome.Separated : CoxOutcome.Collinear);
            }

            double largest = Step(coefficients, Cholesky.Solve(lower, p, score));
            double next = Penalised(likelihood, coefficients, penalty, 1.0, score, information);
            if (largest < StepTolerance)
            {
                return Collapsed(information, initial, p)
                    ? Ended(CoxOutcome.Separated)
                    : new CoxFit(CoxOutcome.Converged, coefficients, next, nullLogLikelihood, information);
            }

            if (iteration == maximumIterations && largest > RunawayStep && next - logLikelihood < StepTolerance)
            {
                return Ended(CoxOutcome.Separated);
            }

            logLikelihood = next;
        }

        return Ended(CoxOutcome.Exhausted);
    }

    /// <summary>lifelines' <c>_newton_raphson_for_efron_model</c>, step for step, for a penalty with an L1 part.</summary>
    /// <remarks>
    /// long-comment: why this reproduces a loop rather than reaching an optimum.
    /// The smoothed absolute value is sharpened at every step, so the answer is where lifelines' stopping rules
    /// fire rather than a stationary point: its default and tight fits differ by a factor of 29 (#1160). The step
    /// size, the sharpening, the four stopping rules and which point is returned are lifelines' own, per
    /// <paramref name="variant"/>; on twelve random fits an ulp of noise in the data moved the answer by 8e-16.
    /// </remarks>
    internal static CoxFit Lifelines(IPartialLikelihood likelihood, int p, ElasticNet penalty, LoopVariant variant)
    {
        var state = new LoopState(p);
        var sizer = new StepSizer(0.95);
        double step = sizer.Next;
        int i = 0;
        while (true)
        {
            i++;
            if (!variant.StepsAfterTesting)
            {
                AddScaled(state.Beta, state.Delta, step);
            }

            if (!state.Evaluate(likelihood, penalty, Math.Pow(variant.Sharpening, i), i == 1))
            {
                return Ended(CoxOutcome.Collinear);
            }

            (double normDelta, double decrement) = state.Direction(variant.StepsAfterTesting ? step : 1.0);
            bool? success = variant.Verdict(i, normDelta, decrement, state.LogLikelihood, state.Previous, step);
            state.Previous = state.LogLikelihood;
            step = sizer.Update(normDelta).Next;
            if (variant.StepsAfterTesting)
            {
                AddScaled(state.Beta, state.Delta, 1.0);
            }

            if (success is bool converged)
            {
                return new CoxFit(
                    converged ? CoxOutcome.Converged : CoxOutcome.GaveUp,
                    state.Beta, state.LogLikelihood, state.NullLogLikelihood, state.Kept);
            }
        }
    }

    /// <summary>What lifelines' loop carries from one step to the next.</summary>
    private sealed class LoopState(int p)
    {
        private readonly double[] _score = new double[p];
        private readonly double[] _information = new double[p * p];
        private double[] _direction = [];

        public double[] Beta { get; } = new double[p];

        public double[] Delta { get; } = new double[p];

        /// <summary>The information of the step that ends the loop, which lifelines returns rather than the next one's.</summary>
        public double[] Kept { get; } = new double[p * p];

        public double LogLikelihood { get; private set; }

        public double NullLogLikelihood { get; private set; } = double.NaN;

        public double Previous { get; set; }

        /// <summary>The penalised likelihood at the current coefficients and its Newton direction; false where the information is singular.</summary>
        public bool Evaluate(IPartialLikelihood likelihood, ElasticNet penalty, double sharpness, bool first)
        {
            LogLikelihood = likelihood.Evaluate(Beta, _score, _information);
            if (first)
            {
                NullLogLikelihood = LogLikelihood;
            }

            LogLikelihood -= penalty.Value(Beta, sharpness);
            penalty.Apply(Beta, sharpness, _score, _information);
            if (!Cholesky.TryFactor(_information, p, out double[] lower))
            {
                return false;
            }

            _direction = Cholesky.Solve(lower, p, _score);
            Array.Copy(_information, Kept, Kept.Length);
            return true;
        }

        /// <summary>Scales the direction into <see cref="Delta"/>, and reports its norm and the Newton decrement.</summary>
        public (double NormDelta, double Decrement) Direction(double scale)
        {
            double norm = 0.0;
            double decrement = 0.0;
            for (int j = 0; j < p; j++)
            {
                Delta[j] = scale * _direction[j];
                norm += Delta[j] * Delta[j];
                decrement += _score[j] * _direction[j];
            }

            return (Math.Sqrt(norm), decrement / 2.0);
        }
    }

    /// <summary>The likelihood less the penalty, with the penalty's gradient and curvature folded in.</summary>
    private static double Penalised(
        IPartialLikelihood likelihood, double[] beta, ElasticNet penalty, double sharpness, double[] score, double[] information)
    {
        double value = likelihood.Evaluate(beta, score, information);
        if (penalty.IsZero)
        {
            return value;
        }

        penalty.Apply(beta, sharpness, score, information);
        return value - penalty.Value(beta, sharpness);
    }

    private static CoxFit Ended(CoxOutcome outcome) => new(outcome, [], double.NaN, double.NaN, []);

    private static void AddScaled(double[] beta, double[] delta, double scale)
    {
        for (int j = 0; j < beta.Length; j++)
        {
            beta[j] += scale * delta[j];
        }
    }

    private static double[] Diagonal(double[] matrix, int p)
    {
        double[] diagonal = new double[p];
        for (int j = 0; j < p; j++)
        {
            diagonal[j] = matrix[(j * p) + j];
        }

        return diagonal;
    }

    /// <summary>Whether any coefficient's information fell to a vanishing fraction of its value at zero.</summary>
    private static bool Collapsed(double[] information, double[] initial, int p)
    {
        for (int j = 0; j < p; j++)
        {
            if (information[(j * p) + j] < CollapsedInformation * initial[j])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Applies a Newton step and returns its largest component.</summary>
    private static double Step(double[] coefficients, double[] step)
    {
        double largest = 0.0;
        for (int j = 0; j < coefficients.Length; j++)
        {
            coefficients[j] += step[j];
            largest = Math.Max(largest, Math.Abs(step[j]));
        }

        return largest;
    }
}

/// <summary>What differs between lifelines' two Newton loops: the proportional hazards one and the time-varying one.</summary>
/// <param name="Sharpening">The base of the softabs sharpness, <c>1.3</c> or <c>1.5</c>.</param>
/// <param name="StepsAfterTesting">Whether the step is applied after the stopping tests, and scaled before them, as the time-varying loop does.</param>
/// <param name="Precision">The <c>precision</c> both the step norm and the Newton decrement are held to.</param>
/// <param name="MaximumSteps">lifelines' <c>max_steps</c>.</param>
/// <param name="SmallestStep">The step size at or below which the loop gives up.</param>
/// <param name="RelativeTest">Whether the relative change in log-likelihood is a stopping rule; the time-varying loop's never fires.</param>
internal sealed record LoopVariant(
    double Sharpening, bool StepsAfterTesting, double Precision, int MaximumSteps, double SmallestStep, bool RelativeTest)
{
    public static LoopVariant ProportionalHazards { get; } = new(1.3, false, 1e-7, 500, 1e-5, true);

    public static LoopVariant TimeVarying { get; } = new(1.5, true, 1e-8, 50, 1e-4, false);

    /// <summary>lifelines' stopping rules in its order: success, failure, or keep going.</summary>
    public bool? Verdict(int iteration, double normDelta, double decrement, double logLikelihood, double previous, double step)
    {
        if (normDelta < Precision)
        {
            return true;
        }

        // S1244: lifelines tests an unset previous log-likelihood as exactly zero.
#pragma warning disable S1244
        if (RelativeTest && previous != 0.0 && Math.Abs(logLikelihood - previous) / -previous < 1e-9)
#pragma warning restore S1244
        {
            return true;
        }

        if (decrement < Precision)
        {
            return true;
        }

        if (iteration >= MaximumSteps || step <= SmallestStep || (Math.Abs(logLikelihood) < 1e-4 && normDelta > 1.0))
        {
            return false;
        }

        return null;
    }
}

/// <summary>lifelines' <c>StepSizer</c>: a step that shrinks on large or erratic Newton steps and grows back on steady ones.</summary>
internal sealed class StepSizer
{
    private const double Scale = 1.3;
    private const int Lookback = 3;
    private readonly List<double> _norms = [];
    private readonly double _initial;
    private bool _temperBackUp;

    public StepSizer(double initial)
    {
        _initial = initial;
        Next = initial;
    }

    public double Next { get; private set; }

    public StepSizer Update(double normOfDelta)
    {
        _norms.Add(normOfDelta);
        if (_temperBackUp)
        {
            Next = Math.Min(Next * Scale, _initial);
        }

        if (normOfDelta >= 15.0)
        {
            Next *= 0.1;
            _temperBackUp = true;
        }
        else if (normOfDelta > 5.0)
        {
            Next *= 0.25;
            _temperBackUp = true;
        }

        if (_norms.Count >= Lookback)
        {
            Next = Decreasing() ? Math.Min(Next * Scale, 1.0) : Next * 0.98;
        }

        return this;
    }

    private bool Decreasing()
    {
        for (int k = _norms.Count - Lookback + 1; k < _norms.Count; k++)
        {
            if (!(_norms[k] - _norms[k - 1] < 0.0))
            {
                return false;
            }
        }

        return true;
    }
}
