using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class IdentifierExpressionTest
{
    [Theory(DisplayName = "Test IDENT expression")]
    [InlineData("foobar")]
    public void Test(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();
        (identifier as ExpressionStatement).Expression.Should().BeOfType<Identifier>();
        identifier.TokenLiteral().Should().Be(input);
        ((identifier as ExpressionStatement).Expression as Identifier).Value.Should().Be(input);
    }
}
