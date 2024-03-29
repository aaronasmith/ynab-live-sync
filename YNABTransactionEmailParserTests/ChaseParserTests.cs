using System.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YNABTransactionEmailParser.Parsers;

public class ChaseParserTests{
    private readonly ChaseParser parser;

    public ChaseParserTests(){        
        parser = new ChaseParser(Mock.Of<ILogger<ChaseParser>>());
    }

    [Fact]
    public void ParsesEmailCorrectly(){
        var transaction = parser.ParseEmail(File.ReadAllText("ChaseEmail.txt"));

        using(new AssertionScope()){
            transaction.Should().NotBeNull();
            transaction.Last4.Should().Be("1769");
            transaction.Payee.Should().Be("LARGE CHARGE");
            transaction.Amount.Should().Be(3919.99m);
            transaction.Date.Should().BeSameDateAs(new System.DateTime(2021, 11, 18, 15, 20, 0));
        }
    }

    [Fact]
    public void InvalidParseReturnsNull(){
        var transaction = parser.ParseEmail("Bad match");

        transaction.Should().BeNull();
    }
}