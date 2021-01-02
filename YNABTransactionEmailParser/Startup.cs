using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YNABTransactionEmailParser
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpClient<Function>((services, cfg) => {
                var configuration = services.GetService<IConfiguration>();

                cfg.BaseAddress = new System.Uri($"https://api.youneedabudget.com/v1/budgets/{configuration.GetValue<string>("YNABBudgetId")}/");
                cfg.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.GetValue<string>("YNABPersonalAccessToken"));
            });
            base.ConfigureServices(context, services);
        }
    }
}