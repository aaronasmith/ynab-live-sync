using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace YNABTransactionEmailParser.Parsers
{

    public class ChaseParser : IParser
    {
        public Transaction ParseEmail(string contents)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(contents);

            var htmlContent = doc.DocumentNode.Descendants("td").Select(y => y.InnerText).ToArray();

            var last4 = Regex.Match(GetValue(htmlContent, "Account") ?? "", @"(?<Account>\d{4})")?.Groups?["Account"]?.Value;
            
            DateTime.TryParseExact(GetValue(htmlContent, "Date") ?? "", "MMM d, yyyy \"at\" h:mm tt \"ET\"", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var date);
            
            var payee = GetValue(htmlContent, "Merchant");

            var test = GetValue(htmlContent, "Amount");
            decimal.TryParse(GetValue(htmlContent, "Amount"), NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var amount);

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
            return content.SkipWhile(v => v != value).Skip(1).FirstOrDefault();
        }
    }
}