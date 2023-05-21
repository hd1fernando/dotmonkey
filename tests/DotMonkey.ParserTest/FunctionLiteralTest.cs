using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class FunctionLiteralTest
{
    [Fact(DisplayName = "Test Function Literal Parsing")]
    public void Test()
    {
        var input = "fn(x,y){x+y;}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        program.Statements.Should().HaveCount(1);
        IStatement identifier = program.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        (identifier as ExpressionStatement).Expression.Should().BeOfType<FunctionLiteral>();
        var function = ((identifier as ExpressionStatement).Expression as FunctionLiteral);
        function.Parameters.Should().HaveCount(2);

        Helpers.TestLiteralExpression(function.Parameters[0], "x");
        Helpers.TestLiteralExpression(function.Parameters[1], "y");

        function.Body.Statements.Should().HaveCount(1);
        var bodyStatement = function.Body.Statements[0];
        bodyStatement.Should().BeOfType<ExpressionStatement>();
        Helpers.TestInfixExpression((bodyStatement as ExpressionStatement).Expression, "x", "+", "y");

    }
}
