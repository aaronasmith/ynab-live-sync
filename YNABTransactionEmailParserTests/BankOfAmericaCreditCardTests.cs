using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YNABTransactionEmailParser.Parsers;

public class BankOfAmericaAuthorizationTests{
    private readonly BankOfAmericaCreditCardParser parser;

    public BankOfAmericaAuthorizationTests(){
        parser = new BankOfAmericaCreditCardParser();
    }

    [Fact]
    public void ParsesAuthorizationEmailCorrectly(){
        var transaction = parser.ParseEmail(File.ReadAllText("BankOfAmericaAuthorizationEmail.txt"));

        transaction.IgnoreTransaction.Should().Be(true);
    }

    [Fact]
    public void InvalidParseReturnsNull(){
        var transaction = parser.ParseEmail("Bad match");

        transaction.Should().BeNull();
    }
}