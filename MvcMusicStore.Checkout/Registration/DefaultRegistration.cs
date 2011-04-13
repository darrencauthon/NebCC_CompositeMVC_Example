using System.Linq;
using MvcTurbine.ComponentModel;

namespace MvcMusicStore.Checkout.Registration
{
    public class DefaultRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            var assembly = typeof (DefaultRegistration).Assembly;

            assembly.GetTypes().Where(x => x.IsInterface).ToList()
                .ForEach(@interface =>
                             {
                                 var implementers = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(@interface));
                                 if (implementers.Count() == 1)
                                     locator.Register(@interface, implementers.Single());
                             });
        }
    }
}