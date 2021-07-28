using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YNABTransactionEmailParser.Parsers;

public class ChaseParserTests{
    private readonly ChaseParser parser;

    public ChaseParserTests(){
        parser = new ChaseParser();
    }

    [Fact]
    public void ParsesEmailCorrectly(){
        var transaction = parser.ParseEmail(File.ReadAllText("ChaseEmail.txt"));

        transaction.Should().NotBeNull();
        transaction.Last4.Should().Be("1769");
        transaction.Payee.Should().Be("WALMART.COM");
        transaction.Amount.Should().Be(12.41m);
        transaction.Date.Should().BeSameDateAs(new System.DateTime(2021, 7, 21, 13, 18, 0));
    }

    [Fact]
    public void InvalidParseReturnsNull(){
        var transaction = parser.ParseEmail("Bad match");

        transaction.Should().BeNull();
    }
}