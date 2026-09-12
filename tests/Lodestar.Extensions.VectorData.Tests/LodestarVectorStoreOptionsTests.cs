using Lodestar.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class LodestarVectorStoreOptionsTests
{
    [Fact]
    public void Defaults_take_RankFusion_s_own_k()
    {
        var options = new LodestarVectorStoreOptions();

        Assert.Equal(60, options.RankFusionK);
        Assert.Null(options.Vectorizer);
        Assert.Null(options.Bm25);
    }

    [Fact]
    public void A_non_positive_fusion_k_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new LodestarVectorStoreOptions { RankFusionK = 0 });
    }
}
