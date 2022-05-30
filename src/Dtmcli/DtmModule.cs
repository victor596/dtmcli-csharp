using FireWolf.Hosting;
using FireWolf.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dtmcli
{
    public class DtmModule : AbstractModule
    {
        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
            services.AddDtmcli(AppConfig.Configuration, "Dtm");
        }
    }
}
