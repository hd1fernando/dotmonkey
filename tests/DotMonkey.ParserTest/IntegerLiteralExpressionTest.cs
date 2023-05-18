using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class IntegerLiteralExpressionTest
{
    [Theory(DisplayName = "Test INT expression")]
    [InlineData("5")]
    public void Test(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();
        (identifier as ExpressionStatement).Expression.Should().BeOfType<IntegerLiteral>();
        identifier.TokenLiteral().Should().Be(input);
        ((identifier as ExpressionStatement).Expression as IntegerLiteral).Value.Should().Be(int.Parse(input));
    }
}