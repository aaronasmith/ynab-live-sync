using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YNABTransactionEmailParser
{

    public class ChaseParser : IParser
    {
        private static Regex regex = new Regex(@"account\s+ending\s+in\s+(?<LastFour>\d+)\..*A\s+charge\s+of\s+\(\$USD\)\s+(?<Amount>[\d\.]+)\s+at\s+(?<Payee>.*?)\s+has.*?on\s+(?<Date>[^\.]+)\..*", RegexOptions.Singleline);

        public Transaction ParseEmail(string contents)
        {
            var match = regex.Match(contents);
            if (!match.Success) return null;

            return new Transaction
            {
                Amount = decimal.Parse(match.Groups["Amount"]?.Value),
                Date = DateTime.ParseExact(match.Groups["Date"].Value, "MMM d, yyyy \"at\" h:mm tt \"ET\"", CultureInfo.CurrentCulture),
                Last4 = match.Groups["LastFour"].Value,
                Payee = match.Groups["Payee"].Value
            };
        }
    }
}