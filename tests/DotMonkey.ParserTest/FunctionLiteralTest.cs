using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using System.Collections.Generic;
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

    [Theory(DisplayName = "Test Function Literal Parameter Parsing")]
    [MemberData(nameof(Data))]
    public void Test2(string input, List<string> exptectedParams)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        program.Statements.Should().HaveCount(1);
        IStatement identifier = program.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        (identifier as ExpressionStatement).Expression.Should().BeOfType<FunctionLiteral>();
        var function = ((identifier as ExpressionStatement).Expression as FunctionLiteral);
        function.Parameters.Should().HaveCount(exptectedParams.Count);

        for (int i = 0; i < exptectedParams.Count; i++)
        {
            Helpers.TestLiteralExpression(function.Parameters[i], exptectedParams[i]);

        }
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] {"fn(){}", new List<string>() },
            new object[] {"fn(x){}", new List<string>{"x"} },
            new object[] {"fn(x,y,z){}", new List<string>{"x","y","z"} },
        };

}
