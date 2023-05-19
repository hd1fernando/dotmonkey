using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class PrefixExpressionTest
{
    [Theory(DisplayName = "Test Prefix expression")]
    [InlineData("!5", "!", 5)]
    [InlineData("-15", "-", 15)]
    public void Test(string input, string @operator, int intValue)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();
        (identifier as ExpressionStatement).Expression.Should().BeOfType<PrefixExpression>();
        identifier.TokenLiteral().Should().Be(@operator);
        ((identifier as ExpressionStatement).Expression as PrefixExpression).Operator.Should().BeEquivalentTo(@operator);

        ((identifier as ExpressionStatement).Expression as PrefixExpression).Rigth.Should().BeOfType<IntegerLiteral>();
        (((identifier as ExpressionStatement).Expression as PrefixExpression).Rigth as IntegerLiteral).Value.Should().Be(intValue);
    }
}
