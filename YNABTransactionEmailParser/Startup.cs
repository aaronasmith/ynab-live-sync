using System;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YNABTransactionEmailParser.Parsers;

namespace YNABTransactionEmailParser
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<YNABClient>();
            services.AddHttpClient<YNABClient>((services, cfg) => {
                var configuration = services.GetService<IConfiguration>();

                cfg.BaseAddress = new System.Uri($"https://api.youneedabudget.com/v1/budgets/{configuration.GetValue<string>("YNABBudgetId")}/");
                cfg.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.GetValue<string>("YNABPersonalAccessToken"));
            });

            services.AddSingleton<ChaseParser>();
            services.AddSingleton<UsBankParser>();
            services.AddSingleton<RedCardParser>();
            services.AddSingleton<BankOfAmericaCreditCardParser>();

            services.AddSingleton<Func<string, IParser>>(services => (bank) => 
                bank switch
                {
                    "chase" => services.GetService<ChaseParser>(),
                    "usbank" => services.GetService<UsBankParser>(),
                    "target" => services.GetService<RedCardParser>(),
                    "bankofamerica_creditcard" => services.GetService<BankOfAmericaCreditCardParser>(),
                    _ => null
                }
            );
            base.ConfigureServices(context, services);
        }
    }
}