using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YNABTransactionEmailParser.Domain.Email;
using YNABTransactionEmailParser.Parsers;

namespace YNABTransactionEmailParser
{
    [FunctionsStartup(typeof(Startup))]
    public class Function : IHttpFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly YNABClient _ynabClient;
        private readonly Func<string, IParser> _parserFactory;

        public Function(ILogger<Function> logger, IConfiguration configuration, YNABClient ynabClient, Func<string, IParser> parserFactory)
        {
            _ynabClient = ynabClient;
            _parserFactory = parserFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task HandleAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            string apikey = request.Query["key"];
            string bank = request.Query["bank"];

            if (apikey != _configuration.GetValue<string>("authenticationKey"))
            {
                _logger.LogError("Invalid api key submitted");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            // If there's a body, parse it as JSON and check for "message" field.
            using TextReader reader = new StreamReader(request.Body);
            string text = await reader.ReadToEndAsync();
            if (text.Length > 0)
            {
                try
                {
                    var email = JsonSerializer.Deserialize<EmailMessage>(text);
                    IParser parser = _parserFactory(bank);

                    string mailBody = parser?.GetText(email) ?? email.plain ?? email.html;
                    _logger.LogInformation("Mail Body: {mailBody}", mailBody);

                    if (parser == null)
                    {
                        _logger.LogError("Invalid bank submitted");
                        return;
                    }

                    var transaction = parser.ParseEmail(mailBody);
                    if (transaction == null)
                    {
                        _logger.LogError("Parse did not succeed for content: \"{content}\"", mailBody);
                        return;
                    } else if (transaction.IgnoreTransaction) {
                        _logger.LogInformation("Ignoring transaction");
                        return;
                    }

                    _logger.LogInformation("Parsing complete: {Amount}, {Date}, {Last4}, {Payee}", transaction.Amount, transaction.Date, transaction.Last4, transaction.Payee);

                    await _ynabClient.PostTransaction(transaction);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error parsing JSON request");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}
