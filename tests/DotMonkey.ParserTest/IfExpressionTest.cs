using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class IfExpressionTest
{
    [Fact(DisplayName = "Test IF expression")]
    public void Test()
    {
        var input = "if(x<y){x}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        program.Statements.Should().HaveCount(1);
        IStatement identifier = program.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        var ifExpression = ((identifier as ExpressionStatement).Expression as IfExpression);

        Helpers.TestInfixExpression(ifExpression.Condition, "x", "<", "y");

        ifExpression.Consequence.Statements.Should().HaveCount(1);
        var consequences = ifExpression.Consequence.Statements[0];
        consequences.Should().BeOfType<ExpressionStatement>();

        Helpers.TestIdentifier((consequences as ExpressionStatement).Expression, "x");
        ifExpression.Alternative.Should().BeNull();
    }

    [Fact(DisplayName = "Test IF ELSE expression")]
    public void Test2()
    {
        var input = "if(x<y){x}else{y}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        program.Statements.Should().HaveCount(1);
        IStatement identifier = program.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        var ifExpression = ((identifier as ExpressionStatement).Expression as IfExpression);

        Helpers.TestInfixExpression(ifExpression.Condition, "x", "<", "y");

        ifExpression.Consequence.Statements.Should().HaveCount(1);
        var consequences = ifExpression.Consequence.Statements[0];
        consequences.Should().BeOfType<ExpressionStatement>();

        Helpers.TestIdentifier((consequences as ExpressionStatement).Expression, "x");

        ifExpression.Alternative.Should().NotBeNull();
        ifExpression.Alternative.Statements.Should().HaveCount(1);
        var alternative = ifExpression.Alternative.Statements[0];
        Helpers.TestIdentifier((alternative as ExpressionStatement).Expression, "y");

    }
}
