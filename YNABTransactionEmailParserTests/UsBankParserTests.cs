using System;
using System.IO;
using FluentAssertions;
using Xunit;
using YNABTransactionEmailParser.Parsers;

namespace YNABTransactionEmailParserTests
{
    public class UsBankParserTests
    {            
        private readonly UsBankParser parser;

        public UsBankParserTests(){
            parser = new UsBankParser();
        }

        [Fact]
        public void ParsesEmailCorrectly(){
            var transaction = parser.ParseEmail(File.ReadAllText("USBankEmail.txt"));

            transaction.Should().NotBeNull();
            transaction.Last4.Should().Be("1234");
            transaction.Payee.Should().Be("Dairy Queen #123.456");
            transaction.Amount.Should().Be(8.28m);
            transaction.Date.Should().BeSameDateAs(DateTime.Now);
        }

        [Fact]
        public void InvalidParseReturnsNull(){
            var transaction = parser.ParseEmail("Bad match");

            transaction.Should().BeNull();
        }
    }
}