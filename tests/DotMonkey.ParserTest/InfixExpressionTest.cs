﻿using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class InfixExpressionTest
{
    [Theory(DisplayName = "Test Prefix expression")]
    [InlineData("5 + 5", 5, "+", 5)]
    [InlineData("5 - 5", 5, "-", 5)]
    [InlineData("5 * 5", 5, "*", 5)]
    [InlineData("5 / 5", 5, "/", 5)]
    [InlineData("5 > 5", 5, ">", 5)]
    [InlineData("5 < 5", 5, "<", 5)]
    [InlineData("5 == 5", 5, "==", 5)]
    [InlineData("5 != 5", 5, "!=", 5)]
    public void Test(string input, int leftValue, string @operator, int rightValue)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();
        (identifier as ExpressionStatement).Expression.Should().BeOfType<InfixExpression>();
        ((identifier as ExpressionStatement).Expression as InfixExpression).Operator.Should().BeEquivalentTo(@operator);

        ((identifier as ExpressionStatement).Expression as InfixExpression).Rigth.Should().BeOfType<IntegerLiteral>();
        (((identifier as ExpressionStatement).Expression as InfixExpression).Rigth as IntegerLiteral).Value.Should().Be(leftValue);

        ((identifier as ExpressionStatement).Expression as InfixExpression).Left.Should().BeOfType<IntegerLiteral>();
        (((identifier as ExpressionStatement).Expression as InfixExpression).Left as IntegerLiteral).Value.Should().Be(rightValue);
    }

    [Theory(DisplayName = "Test Prefix expression")]
    [InlineData("-a*b", "((-a)*b)")]
    [InlineData("!-a", "(!(-a))")]
    [InlineData("a+b+c", "((a+b)+c)")]
    [InlineData("a + b - c", "((a+b)-c)")]
    [InlineData("a * b * c", "((a*b)*c)")]
    [InlineData("a * b / c", "((a*b)/c)")]
    [InlineData("a + b / c", "((a+(b/c)")]
    [InlineData("a + b * c + d / e - f", "(((a+(b*c))+(d/e))-f)")]
    [InlineData("3 + 4; -5 * 5", "(3+4)((-5)*5)")]
    [InlineData("5 > 4 == 3 < 4", "((5>4)==(3<4))")]
    [InlineData("5 < 4 != 3 > 4", "((5<4)!=(3>4))")]
    [InlineData("3 + 4 * 5 == 3 * 1 + 4 * 5", "((3+(4*5))==((3*1)+(4*5)))")]
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
