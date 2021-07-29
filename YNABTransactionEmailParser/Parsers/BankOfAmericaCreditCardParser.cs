using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YNABTransactionEmailParser.Parsers
{

    public class BankOfAmericaCreditCardParser : IParser
    {
        private static Regex regex = new Regex(@"Credit Card ending in (?<LastFour>\d+).*Amount:\s+\$(?<Amount>[\d\.]+).*Date:\s+(?<Date>\w+\s+\d+,\s+\d+).*Where:\s+(?<Payee>.*(?=\s+View))", RegexOptions.Singleline);
        public Transaction ParseEmail(string contents)
        {
            //check if it's an authorization email
            if(contents.Contains("Here's the code") && contents.Contains("you asked for")) {
                return new Transaction
                {
                    IgnoreTransaction = true
                };
            }

            //try to get transaction information
            var match = regex.Match(contents);
            if(!match.Success) return null;

            //January 31, 2021
            return new Transaction
            {
                Amount = decimal.Parse(match.Groups["Amount"]?.Value),
                Date = DateTime.ParseExact(match.Groups["Date"].Value, "MMMM dd, yyyy", CultureInfo.CurrentCulture),
                Last4 = match.Groups["LastFour"].Value,
                Payee = match.Groups["Payee"].Value
            };
        }
    }
}