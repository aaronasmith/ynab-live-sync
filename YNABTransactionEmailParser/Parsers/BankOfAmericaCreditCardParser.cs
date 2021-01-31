using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YNABTransactionEmailParser.Parsers
{

    public class BankOfAmericaCreditCardParser : IParser
    {
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

            return null;
        }
    }
}