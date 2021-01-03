using System.IO;
using FluentAssertions;
using Xunit;
using YNABTransactionEmailParser.Parsers;

namespace YNABTransactionEmailParserTests
{
    public class RedCardParserTests
    {
        private readonly RedCardParser parser;

        public RedCardParserTests()
        {
            parser = new RedCardParser();
        }

        [Fact]
        public void ParsesEmailCorrectly()
        {
            var transaction = parser.ParseEmail(File.ReadAllText("RedCardEmail.txt"));

            transaction.Should().NotBeNull();
            transaction.Last4.Should().Be("6645");
            transaction.Payee.Should().Be("TARGET");
            transaction.Amount.Should().Be(4.89m);
            transaction.Date.Should().BeSameDateAs(new System.DateTime(2021, 1, 2));
        }

        [Fact]
        public void InvalidParseReturnsNull()
        {
            var transaction = parser.ParseEmail("Bad match");

            transaction.Should().BeNull();
        }

    }
}