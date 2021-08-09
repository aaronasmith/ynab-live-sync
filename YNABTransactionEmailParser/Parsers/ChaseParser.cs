using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace YNABTransactionEmailParser.Parsers
{

    public class ChaseParser : IParser
    {
        private readonly ILogger<ChaseParser> logger;

        public ChaseParser(ILogger<ChaseParser> logger){
            this.logger = logger;
        }

        public Transaction ParseEmail(string contents)
        {
            logger.LogInformation("Parsing chase email");
            if(contents.Contains("Thank you for activating Account Alerts.")){
                logger.LogInformation("Ignoring confirmation email");
                return new Transaction { IgnoreTransaction = true };
            }
        
            var doc = new HtmlDocument();
            logger.LogInformation("Loading as html document");
            doc.LoadHtml(contents);

            logger.LogInformation("Getting all table cells");
            var htmlContent = doc.DocumentNode.Descendants("td").Select(y => y.InnerText).ToArray();

            string account = GetValue(htmlContent, "Account");
            var last4 = Regex.Match(account ?? "", @"(?<Account>\d{4})")?.Groups?["Account"]?.Value;

            string rawDate = GetValue(htmlContent, "Date");
            DateTime.TryParseExact(rawDate ?? "", "MMM d, yyyy \"at\" h:mm tt \"ET\"", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out var date);
            
            var payee = GetValue(htmlContent, "Merchant");

            var rawAmount = GetValue(htmlContent, "Amount");
            decimal.TryParse(rawAmount, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-US"), out var amount);

            logger.LogInformation("RawAccount: {RawAccount}; RawDate: {RawDate}, RawAmount: {RawAmount}", account, rawDate, rawAmount);
            logger.LogInformation("Last4: {Last4}; Payee: {Payee}, Date: {Date}, Amount: {Amount}", last4, payee, date, amount);

            if(last4 == null || payee == null || date == default(DateTime) || amount == default(decimal)){
                return null;
            }

            return new Transaction
            {
                Amount = amount,
                Date = date,
                Last4 = last4,
                Payee = payee
            };
        }

        public string GetValue(IEnumerable<string> content, string value){
            logger.LogInformation("Finding content for {value}", value);
            var foundValue = content.SkipWhile(v => v != value).Skip(1).FirstOrDefault();
            logger.LogInformation("Found {value}", foundValue);
            return foundValue;
        }
    }
}
