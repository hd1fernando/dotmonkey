using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class InfixExpressionTest
{
    [Theory(DisplayName = "Test parsing infix expression")]
    [InlineData("5 + 5", 5, "+", 5)]
    [InlineData("5 - 5", 5, "-", 5)]
    [InlineData("5 * 5", 5, "*", 5)]
    [InlineData("5 / 5", 5, "/", 5)]
    [InlineData("5 > 5", 5, ">", 5)]
    [InlineData("5 < 5", 5, "<", 5)]
    [InlineData("5 == 5", 5, "==", 5)]
    [InlineData("5 != 5", 5, "!=", 5)]
    [InlineData("true == true", true, "==", true)]
    [InlineData("true != false", true, "!=", false)]
    [InlineData("false == false", false, "==", false)]
    public void Test(string input, object leftValue, string @operator, object rightValue)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();

        Helpers.TestInfixExpression((identifier as ExpressionStatement).Expression, leftValue, @operator, rightValue);
    }

    [Theory(DisplayName = "Test operator precedence parsing")]
    [InlineData("-a*b", "((-a)*b)")]
    [InlineData("!-a", "(!(-a))")]
    [InlineData("a+b+c", "((a+b)+c)")]
    [InlineData("a + b - c", "((a+b)-c)")]
    [InlineData("a * b * c", "((a*b)*c)")]
    [InlineData("a * b / c", "((a*b)/c)")]
    [InlineData("a + b / c", "(a+(b/c))")]
    [InlineData("a + b * c + d / e - f", "(((a+(b*c))+(d/e))-f)")]
    [InlineData("3 + 4; -5 * 5", "(3+4)((-5)*5)")]
    [InlineData("5 > 4 == 3 < 4", "((5>4)==(3<4))")]
    [InlineData("5 < 4 != 3 > 4", "((5<4)!=(3>4))")]
    [InlineData("3 + 4 * 5 == 3 * 1 + 4 * 5", "((3+(4*5))==((3*1)+(4*5)))")]
    [InlineData("1+2+3;", "((1+2)+3)")]
    [InlineData("true", "true")]
    [InlineData("false", "false")]
    [InlineData("3 > 5 == false", "((3>5)==false)")]
    [InlineData("3 < 5 == true", "((3<5)==true)")]
    public void test2(string input, string exptectedString)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);
        var program = parser.ParserProgram();

        var result = program.String();

        parser.Errors.Should().BeEmpty();
        result.Should().BeEquivalentTo(exptectedString);
    }
}