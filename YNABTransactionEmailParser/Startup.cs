using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace YNABTransactionEmailParser
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpClient<Function>(cfg =>{
                
            });
            base.ConfigureServices(context, services);
        }        
    }
}