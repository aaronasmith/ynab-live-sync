using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;
using YNABTransactionEmailParser.Parsers;

namespace YNABTransactionEmailParserTests
{
    public class VenmoParserTests
    {            
        private readonly VenmoParser parser;

        public VenmoParserTests(){
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>{{"VenmoLast4", "1234"}});
            parser = new VenmoParser(configurationBuilder.Build());
        }

        [Fact]
        public void ParsesEmailCorrectly(){
            var transaction = parser.ParseEmail(File.ReadAllText("VenmoTransactionEmail.txt"));

            transaction.Should().NotBeNull();
            transaction.Last4.Should().Be("1234");
            transaction.Payee.Should().Be("A MERCHANT");
            transaction.Amount.Should().Be(21.74m);
            transaction.Date.Should().BeSameDateAs(DateTime.Now);
        }

        [Fact]
        public void InvalidParseReturnsNull(){
            var transaction = parser.ParseEmail("Bad match");

            transaction.Should().BeNull();
        }
    }
}