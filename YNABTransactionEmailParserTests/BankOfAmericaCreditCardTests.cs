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
    public void ParsesTransactionEmailCorrectly(){
        var transaction = parser.ParseEmail(File.ReadAllText("BankOfAmericaCreditCardTransactionEmail.txt"));

        transaction.Last4.Should().Be("4321");
        transaction.Payee.Should().Be("PAYPAL TWITCHINTER");
        transaction.Amount.Should().Be(123.45m);
        transaction.Date.Should().BeSameDateAs(new System.DateTime(2021, 1, 31));
    }

    [Fact]
    public void InvalidParseReturnsNull(){
        var transaction = parser.ParseEmail("Bad match");

        transaction.Should().BeNull();
    }
}