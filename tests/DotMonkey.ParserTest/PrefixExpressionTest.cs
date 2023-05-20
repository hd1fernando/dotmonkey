using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class PrefixExpressionTest
{
    [Theory(DisplayName = "Test parsing Prefix expression")]
    [InlineData("!5", "!", 5)]
    [InlineData("-15", "-", 15)]
    [InlineData("!true", "!", true)]
    [InlineData("!false", "!", false)]
    public void Test(string input, string @operator, object value)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        Helpers.TestPrefix((identifier as ExpressionStatement).Expression, @operator);

        Helpers.TestLiteralExpression(((identifier as ExpressionStatement).Expression as PrefixExpression).Rigth, value);
    }
}
