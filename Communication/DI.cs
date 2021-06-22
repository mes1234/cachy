
using Microsoft.Extensions.DependencyInjection;

namespace Cachy.Communication
{
    public static class CommunicationServicesRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddHostedService<Reciever>();
        }
    }
}