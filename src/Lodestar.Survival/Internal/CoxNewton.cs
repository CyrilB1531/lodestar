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

    /// <summary>No halving of the Newton step kept the likelihood, short of the maximum.</summary>
    Stalled,

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
    private static double LogAddExp(double x) => Math.Max(0.0, x) + NearZero.Log1P(Math.Exp(-Math.Abs(x)));

    /// <summary>autograd's derivative of <c>logaddexp(0, x)</c>: <c>exp(x − logaddexp(0, x))</c>.</summary>
    private static double Logistic(double x) => Math.Exp(x - LogAddExp(x));
}

/// <summary>The two ways a Cox fit reaches its coefficients: to the optimum, or along lifelines' own loop.</summary>
internal static class CoxNewton
{
    // The largest step component below which the fit has converged. Four or five iterations reach it on
    // the corpus' random fixtures from coefficients at zero; the #1242 one, whose steps are halved, takes seven.
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
        double[] information = new double[TableLength.Of(p, p, ResultTable.FeatureCount)];
        // At zero the ridge penalty is zero, so this is the null likelihood, with the penalty's curvature folded in.
        double nullLogLikelihood = Penalised(likelihood, coefficients, penalty, 1.0, score, information);
        double[] initial = Diagonal(information, p);
        double logLikelihood = nullLogLikelihood;
        for (int iteration = 1; iteration <= maximumIterations; iteration++)
        {
            if (NewtonStep(information, score, p) is not { } step)
            {
                return Ended(iteration > 1 && logLikelihood > nullLogLikelihood ? CoxOutcome.Separated : CoxOutcome.Collinear);
            }

            // Newton's decrement, the likelihood the step would gain twice over: at a maximum far out the step kept the
            // score's rounding, 8e-10, past the tolerance, and a design with a maximum at β = 661 never converged (#1756).
            bool settled = Settled(score, step, logLikelihood);
            (double largest, bool stuck, double next) = Halved(likelihood, coefficients, step, penalty, logLikelihood, score, information);
            if (Stopped(settled, largest, stuck, information, initial) is { } outcome)
            {
                return outcome == CoxOutcome.Converged
                    ? new CoxFit(CoxOutcome.Converged, coefficients, next, nullLogLikelihood, information)
                    : Ended(outcome);
            }

            if (iteration == maximumIterations && largest > RunawayStep && next - logLikelihood < StepTolerance)
            {
                return Ended(CoxOutcome.Separated);
            }

            logLikelihood = next;
        }

        return Ended(Spent(information, initial, p));
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
        private readonly double[] _information = new double[TableLength.Of(p, p, ResultTable.FeatureCount)];
        private double[] _direction = [];

        public double[] Beta { get; } = new double[p];

        public double[] Delta { get; } = new double[p];

        /// <summary>The information of the step that ends the loop, which lifelines returns rather than the next one's.</summary>
        public double[] Kept { get; } = new double[TableLength.Of(p, p, ResultTable.FeatureCount)];

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

    private static bool Settled(double[] score, double[] step, double logLikelihood) =>
        Decrement(score, step) <= 1e-20 * Math.Max(1.0, Math.Abs(logLikelihood));

    private static double Decrement(double[] score, double[] step)
    {
        double decrement = 0.0;
        for (int j = 0; j < step.Length; j++)
        {
            decrement += score[j] * step[j];
        }

        return decrement;
    }

    /// <summary>How a Newton step of <paramref name="largest"/> ends the fit, or null where it goes on: a collapsed
    /// information is a separation, a settled decrement or a step below the tolerance converges, and a search no
    /// halving completed stalls.</summary>
    private static CoxOutcome? Stopped(bool settled, double largest, bool stuck, double[] information, double[] initial)
    {
        int p = initial.Length;
        settled |= largest < StepTolerance;
        if (!stuck && !settled)
        {
            return null;
        }

        if (Collapsed(information, initial, p))
        {
            return CoxOutcome.Separated;
        }

        return settled ? CoxOutcome.Converged : CoxOutcome.Stalled;
    }

    /// <summary>Newton's step, or null where the information does not factor or the step leaves the doubles, from an
    /// overflowing score or information: as singular as an unfactored one.</summary>
    private static double[]? NewtonStep(double[] information, double[] score, int p)
    {
        if (!Cholesky.TryFactor(information, p, out double[] lower))
        {
            return null;
        }

        double[] step = Cholesky.Solve(lower, p, score);
        return step.All(s => !double.IsNaN(s) && !double.IsInfinity(s)) ? step : null;
    }

    /// <summary>How a spent budget ends: a collapsed information is a plateau a coefficient runs along, separated (#1756).</summary>
    private static CoxOutcome Spent(double[] information, double[] initial, int p) =>
        Collapsed(information, initial, p) ? CoxOutcome.Separated : CoxOutcome.Exhausted;

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

    /// <summary>Applies the Newton step, halved while it lowers the penalised likelihood or leaves the finite values, by
    /// the search it shares with ParametricNewton, NewtonSearch.Scale: taken whole, a 23-subject fit lifelines solves ran to β ≈ [164, 1906] and was refused
    /// as separated (#1242). Returns Newton's whole step's largest component, so a halved one never reads as converged
    /// nor a runaway as slowing, whether no halving kept the likelihood, the coefficients then left where they were, and
    /// the likelihood reached.</summary>
    private static (double Largest, bool Stuck, double Next) Halved(
        IPartialLikelihood likelihood, double[] coefficients, double[] step, ElasticNet penalty, double current,
        double[] score, double[] information)
    {
        double[] start = [.. coefficients];
        // The score and information at the start, restored where no halving holds: re-evaluating cost a pass of the data.
        double[] startScore = [.. score];
        double[] startInformation = [.. information];
        double largest = step.Max(Math.Abs);

        double next = current;
        double scale = NewtonSearch.Scale(current, fraction =>
        {
            for (int j = 0; j < step.Length; j++)
            {
                coefficients[j] = start[j] + (fraction * step[j]);
            }

            // A step halved below half an ulp of every coefficient no longer moves them: the halvings left would
            // evaluate the start again.
            if (coefficients.AsSpan().SequenceEqual(start))
            {
                return null;
            }

            next = Penalised(likelihood, coefficients, penalty, 1.0, score, information);
            return next;
        });
        if (double.IsNaN(scale))
        {
            Array.Copy(start, coefficients, start.Length);
            Array.Copy(startScore, score, score.Length);
            Array.Copy(startInformation, information, information.Length);
            return (largest, true, current);
        }

        return (largest, false, next);
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
