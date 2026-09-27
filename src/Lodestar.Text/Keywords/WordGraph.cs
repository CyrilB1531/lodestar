namespace Lodestar.Text.Keywords;

/// <summary>
/// The undirected co-occurrence graph TextRank ranks, and the power iteration that ranks it.
/// </summary>
/// <remarks>
/// The window runs over the RAW token stream: a stop word is a null that occupies a
/// position and forms no node, so two words separated by one are not adjacent. Nodes of
/// zero weighted degree are then deleted in one pass, without which the transition matrix
/// is substochastic and its dominant eigenvector is a different vector. Both measured
/// against summa, whose pipeline does the same two things.
/// </remarks>
internal sealed class WordGraph
{
    private readonly List<string> _nodes = [];
    private readonly Dictionary<string, int> _index = new(StringComparer.Ordinal);

    // Each node's off-diagonal neighbours ascending, and its self-loop: every weight is 1, so that
    // is the whole row, where a dense n x n matrix cost 6.4 GB at 20,000 stems (#1199).
    private int[][] _neighbours;
    private bool[] _selfLoop;

    public WordGraph(IReadOnlyList<string?> stream, int window)
    {
        Guard.NotNull(stream);
        Guard.NotLessThan(window, 1);

        foreach (string token in stream.OfType<string>().Where(t => !_index.ContainsKey(t)))
        {
            _index[token] = _nodes.Count;
            _nodes.Add(token);
        }

        (_neighbours, _selfLoop) = BuildAdjacency(stream, window);
        RemoveUnreachable();
    }

    /// <summary>The ranked words, in first-occurrence order, with the unreachable ones gone.</summary>
    public IReadOnlyList<string> Nodes => _nodes;

    /// <summary>
    /// How many undirected off-diagonal edges survive, which is what a window bug changes
    /// first. A self-loop lives on the diagonal and is never counted here.
    /// </summary>
    public int EdgeCount
    {
        get
        {
            int ends = 0;
            foreach (int[] row in _neighbours)
            {
                ends += row.Length;
            }

            return ends / 2;
        }
    }

    /// <summary>The dominant left eigenvector of <c>d·A + (1 − d)/n</c>, L2-normalised.</summary>
    /// <param name="damping">The probability of following an edge rather than teleporting.</param>
    /// <param name="tolerance">The largest per-component change that counts as converged.</param>
    /// <param name="maxIterations">The most iterations to run before giving up.</param>
    /// <exception cref="InvalidOperationException">The iteration did not converge within <paramref name="maxIterations"/>.</exception>
    public double[] Rank(double damping, double tolerance, int maxIterations)
    {
        int n = _nodes.Count;
        if (n == 0)
        {
            return [];
        }

        // Degree sums the whole row, diagonal included, but only i != j gets a damping term —
        // build_adjacency_matrix's rule. Every surviving node has a degree of at least 1.
        double[] share = new double[n];
        for (int i = 0; i < n; i++)
        {
            share[i] = damping / (_neighbours[i].Length + (_selfLoop[i] ? 1 : 0));
        }

        double teleport = (1 - damping) / n;
        double[] x = InitialVector(n);
        double[] next = new double[n];
        double[] weighted = new double[n];

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            double delta = Iterate(x, next, weighted, share, teleport);
            (x, next) = (next, x);
            if (delta < tolerance)
            {
                // The transition matrix is non-negative and x starts non-negative, so every
                // iterate stays non-negative; the abs guards only against a stray -0.0.
                MakeNonNegative(x, n);
                return x;
            }
        }

        throw new InvalidOperationException(
            $"The power iteration did not converge to {tolerance} within {maxIterations} iterations.");
    }

    private static double[] InitialVector(int n)
    {
        double[] x = new double[n];
        double initial = 1.0 / Math.Sqrt(n);
        for (int i = 0; i < n; i++)
        {
            x[i] = initial;
        }

        return x;
    }

    private static void MakeNonNegative(double[] x, int n)
    {
        for (int i = 0; i < n; i++)
        {
            x[i] = Math.Abs(x[i]);
        }
    }

    // x·M renormalised, returning the largest component change. M is teleport plus share[i] on each
    // edge, and symmetric, so column j of x·M reads j's own neighbour list.
    private double Iterate(double[] x, double[] next, double[] weighted, double[] share, double teleport)
    {
        int n = x.Length;
        double total = 0;
        for (int i = 0; i < n; i++)
        {
            total += x[i];
            weighted[i] = x[i] * share[i];
        }

        double base_ = teleport * total;
        double norm = 0;
        for (int j = 0; j < n; j++)
        {
            double sum = base_;
            foreach (int i in _neighbours[j])
            {
                sum += weighted[i];
            }

            next[j] = sum;
            norm += sum * sum;
        }

        norm = Math.Sqrt(norm);

        double delta = 0;
        for (int j = 0; j < n; j++)
        {
            next[j] /= norm;
            double componentDelta = Math.Abs(next[j] - x[j]);
            if (componentDelta > delta)
            {
                delta = componentDelta;
            }
        }

        return delta;
    }

    // Pairs token i with i+1 .. i+window-1 (summa's window), always at weight 1 — mirrors
    // add_edge's `not has_edge` guard. a == b marks the one self-loop, not a skip.
    private (int[][] Neighbours, bool[] SelfLoop) BuildAdjacency(IReadOnlyList<string?> stream, int window)
    {
        int n = _nodes.Count;
        var sets = new HashSet<int>[n];
        bool[] selfLoop = new bool[n];
        for (int i = 0; i < stream.Count; i++)
        {
            if (stream[i] is not string left)
            {
                continue;
            }

            int a = _index[left];
            int end = Math.Min(i + window, stream.Count);
            for (int j = i + 1; j < end; j++)
            {
                if (stream[j] is not string right)
                {
                    continue;
                }

                int b = _index[right];
                if (a == b)
                {
                    selfLoop[a] = true;
                    continue;
                }

                (sets[a] ??= []).Add(b);
                (sets[b] ??= []).Add(a);
            }
        }

        var neighbours = new int[n][];
        for (int i = 0; i < n; i++)
        {
            neighbours[i] = sets[i] is null ? [] : [.. sets[i]];
            Array.Sort(neighbours[i]);
        }

        return (neighbours, selfLoop);
    }

    // One pass is enough: an isolated node lowers nobody's degree when removed. All of them go in one
    // compaction (#816), renumbering the survivors' neighbour lists.
    private void RemoveUnreachable()
    {
        int n = _nodes.Count;
        int[] renumber = new int[n];
        int kept = 0;
        for (int i = 0; i < n; i++)
        {
            renumber[i] = _neighbours[i].Length > 0 || _selfLoop[i] ? kept++ : -1;
        }

        if (kept == n)
        {
            return;
        }

        var neighbours = new int[kept][];
        bool[] selfLoop = new bool[kept];
        var nodes = new List<string>(kept);
        for (int i = 0; i < n; i++)
        {
            int target = renumber[i];
            if (target < 0)
            {
                continue;
            }

            // A neighbour has this node as its neighbour, so it survives too; ascending order holds.
            int[] row = _neighbours[i];
            int[] mapped = new int[row.Length];
            for (int k = 0; k < row.Length; k++)
            {
                mapped[k] = renumber[row[k]];
            }

            neighbours[target] = mapped;
            selfLoop[target] = _selfLoop[i];
            nodes.Add(_nodes[i]);
        }

        _nodes.Clear();
        _nodes.AddRange(nodes);
        _neighbours = neighbours;
        _selfLoop = selfLoop;
        _index.Clear();
        for (int i = 0; i < _nodes.Count; i++)
        {
            _index[_nodes[i]] = i;
        }
    }
}
