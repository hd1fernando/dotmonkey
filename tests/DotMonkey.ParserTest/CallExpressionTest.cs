using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class CallExpressionTest
{
    [Fact(DisplayName = "Test CALL expression parsing")]
    public void Test()
    {
        var input = "add(1, 2 * 3, 4 + 5)";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        program.Statements.Should().HaveCount(1);
        IStatement identifier = program.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        var callExpression = ((identifier as ExpressionStatement).Expression as CallExpression);

        Helpers.TestIdentifier(callExpression.Function, "add");

        callExpression.Arguments.Should().HaveCount(3);

        Helpers.TestLiteralExpression(callExpression.Arguments[0], 1);
        Helpers.TestInfixExpression(callExpression.Arguments[1], 2, "*", 3);
        Helpers.TestInfixExpression(callExpression.Arguments[2], 4, "+", 5);
    }
}
