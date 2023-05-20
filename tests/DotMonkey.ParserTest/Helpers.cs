using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using System;
using System.Linq.Expressions;

namespace DotMonkey.ParserTest;

public static class Helpers
{
    public static void TestInfixExpression(IExpression expression, object left, string @operator, object right)
    {
        expression.Should().BeOfType<InfixExpression>();

        TestLiteralExpression((expression as InfixExpression).Left, left);
        (expression as InfixExpression).Operator.Should().BeEquivalentTo(@operator);
        TestLiteralExpression((expression as InfixExpression).Rigth, right);
    }

    public static void TestLiteralExpression(IExpression expression, object type)
    {
        var t = type.GetType();

        if (t == typeof(string))
        {
            TestIdentifier(expression, type.ToString());
        }
        else if (t == typeof(int))
        {
            TestIntegerLiteral(expression, int.Parse(type.ToString()));
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public static void TestIdentifier(IExpression expression, string value)
    {
        expression.Should().BeOfType<Identifier>();
        expression.TokenLiteral().Should().Be(value);
        (expression as Identifier).Value.Should().BeEquivalentTo(value);
    }

    public static void TestIntegerLiteral(IExpression expression, int value)
    {
        expression.Should().BeOfType<IntegerLiteral>();
        expression.TokenLiteral().Should().Be(value.ToString());
        (expression as IntegerLiteral).Value.Should().Be(value);
    }
}