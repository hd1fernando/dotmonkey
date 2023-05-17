using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Interfaces;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class IdentifierExpressionTest
{
    [Theory]
    [InlineData("foobar")]
    public void Test(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<Identifier>();
        identifier.TokenLiteral().Should().Be(input);
        (identifier as Identifier).Value.Should().Be(input);
    }
}
