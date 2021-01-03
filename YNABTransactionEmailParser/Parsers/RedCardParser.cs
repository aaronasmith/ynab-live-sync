using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YNABTransactionEmailParser.Parsers
{
    public class RedCardParser : IParser
    {
        private static Regex regex = new Regex(@"Date:\s*(?<Date>.*(AM|PM)).*ending\s+in\s+(?<LastFour>\d+).*transaction\s+of\s+\$(?<Amount>[\d\.]*)\s+at\s+(?<Payee>.+?)\s+was\s+recently\s+.*", RegexOptions.Singleline);

        public Transaction ParseEmail(string contents)
        {
            var match = regex.Match(contents);
            if (!match.Success) return null;

            return new Transaction
            {
                Amount = decimal.Parse(match.Groups["Amount"]?.Value),
                Date = DateTime.ParseExact(match.Groups["Date"].Value, "ddd, MMM d, yyyy \"at\" h:mm tt", CultureInfo.CurrentCulture),
                Last4 = match.Groups["LastFour"].Value,
                Payee = match.Groups["Payee"].Value
            };
        }
    }
}