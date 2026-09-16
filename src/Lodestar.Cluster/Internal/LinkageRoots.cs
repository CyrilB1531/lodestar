namespace Lodestar.Cluster.Internal;

/// <summary>Which node a sample or slot currently belongs to, as merges create new ids.</summary>
/// <remarks>
/// A union-find over <c>2n − 1</c> ids where a join does not keep either root but creates the next
/// id, <c>n</c>, <c>n + 1</c> and so on — which is how both reference paths number the nodes of the
/// tree they return.
/// </remarks>
internal sealed class LinkageRoots
{
    private const int NoParent = -1;
    private readonly int[] _parent;
    private int _next;

    public LinkageRoots(int sampleCount)
    {
        _parent = new int[(2 * sampleCount) - 1];
        for (int id = 0; id < _parent.Length; id++)
        {
            _parent[id] = NoParent;
        }

        _next = sampleCount;
    }

    /// <summary>The newest node containing <paramref name="id"/>, compressing the path to it.</summary>
    public int Find(int id)
    {
        int root = id;
        while (_parent[root] != NoParent)
        {
            root = _parent[root];
        }

        while (id != root && _parent[id] != root)
        {
            (id, _parent[id]) = (_parent[id], root);
        }

        return root;
    }

    /// <summary>Joins two roots under the next node id.</summary>
    public void Join(int left, int right)
    {
        _parent[left] = _next;
        _parent[right] = _next;
        _next++;
    }
}
