using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using YNABTransactionEmailParser.Domain.Email;

namespace YNABTransactionEmailParser
{
    public class Function : IHttpFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Function(ILogger<Function> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task HandleAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            string apikey = request.Query["key"];
            string bank = request.Query["bank"];

            if(apikey != _configuration.GetValue<string>("authenticationKey")){
                _logger.LogError("Invalid api key submitted");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            // If there's a body, parse it as JSON and check for "message" field.
            using TextReader reader = new StreamReader(request.Body);
            string text = await reader.ReadToEndAsync();
            if (text.Length > 0)
            {
                try
                {
                    var email = JsonSerializer.Deserialize<EmailMessage>(text);
                    IParser parser = bank switch {
                        "chase" => new ChaseParser(),
                        _ => null
                    };

                    if(parser == null){
                        _logger.LogError("Invalid bank submitted");
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }

                    var transaction = parser.ParseEmail(email.plain);
                    if(transaction == null){
                        _logger.LogError("Parse did not succeed for content: \"{content}\"", email.plain);                        
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }

                    _logger.LogInformation("Parsing complete: {Amount}, {Date}, {Last4}, {Payee}", transaction.Amount, transaction.Date, transaction.Last4, transaction.Payee);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error parsing JSON request");
                    _logger.LogError(exception.Message);
                    _logger.LogError(exception.StackTrace);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}
