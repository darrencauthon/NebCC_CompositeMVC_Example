using MvcMusicStore.Checkout.Helpers;
using MvcMusicStore.Shared.Helpers;
using MvcTurbine.ComponentModel;

namespace MvcMusicStore.Checkout.Registration
{
    public class TopSellingProductsRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            locator.Register<ITopSellingProductsCalculator, TopSellingProductsCalculator>();
        }
    }
}