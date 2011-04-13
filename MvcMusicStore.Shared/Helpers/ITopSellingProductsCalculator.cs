using System.Collections.Generic;

namespace MvcMusicStore.Shared.Helpers
{
    public interface ITopSellingProductsCalculator
    {
        IEnumerable<int> GetTheKeysOfTheTopSellingProducts();
    }
}