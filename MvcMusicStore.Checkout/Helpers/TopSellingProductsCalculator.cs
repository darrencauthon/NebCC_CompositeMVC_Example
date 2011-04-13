using System.Collections.Generic;
using System.Linq;
using MvcMusicStore.Checkout.Models;
using MvcMusicStore.Shared.Helpers;

namespace MvcMusicStore.Checkout.Helpers
{
    public class TopSellingProductsCalculator : ITopSellingProductsCalculator
    {
        private readonly CheckoutEntities storeDB = new CheckoutEntities();

        public IEnumerable<int> GetTheKeysOfTheTopSellingProducts()
        {
            var itemsSold = storeDB.OrderDetails
                .GroupBy(x => x.AlbumId)
                .Select(x =>
                        new
                            {
                                x.Key,
                                Count = x.Count()
                            }).ToList();

            return itemsSold
                .OrderByDescending(x => x.Count)
                .Take(5)
                .Select(x => x.Key);
        }
    }
}