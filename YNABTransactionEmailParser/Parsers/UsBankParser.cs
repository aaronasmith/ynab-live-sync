using System;
using System.Text.RegularExpressions;

namespace YNABTransactionEmailParser.Parsers
{
    public class UsBankParser : IParser
    {
        private static Regex regex = new Regex(@"was\s+charged\s+\$(?<Amount>[\d\.]+)\s+at\s+(?<Payee>.*?)\.\s+A\s+purchase.*ending\s+in\s+(?<LastFour>\d+)\..*", RegexOptions.Singleline);

        public Transaction ParseEmail(string contents)
        {
            var match = regex.Match(contents);
            if (!match.Success) return null;

            return new Transaction
            {
                Amount = decimal.Parse(match.Groups["Amount"]?.Value),
                Date = DateTime.Now,
                Last4 = match.Groups["LastFour"].Value,
                Payee = match.Groups["Payee"].Value
            };
        }
    }
}