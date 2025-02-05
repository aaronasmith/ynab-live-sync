using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YNABTransactionEmailParser.Parsers
{
    public class VenmoParser : IParser
    {
        private IConfiguration configuration;

        private static Regex regex = new Regex(@".*We want to let you know a purchase was made for \$(?<Amount>[\d\.]+)[^M]*Merchant info \*(?<Payee>[^\*]*)\*", RegexOptions.Compiled);
        private readonly ILogger<VenmoParser> logger;

        public VenmoParser(IConfiguration configuration, ILogger<VenmoParser> logger){
            this.logger = logger;
            this.configuration = configuration;
        }

        public Transaction ParseEmail(string contents)
        {
            var match = regex.Match(contents);
            if (!match.Success) return null;

            return new Transaction
            {
                Amount = decimal.Parse(match.Groups["Amount"]?.Value),
                Date = DateTime.Now,
                Last4 = configuration["VenmoLast4"],
                Payee = match.Groups["Payee"].Value
            };
        }

        public string GetEmail(Domain.Email.EmailMessage email){
            logger.LogInformation("Returning html email message");
            return email.html;
        }
    }
}