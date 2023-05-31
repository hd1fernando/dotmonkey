using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotMonkey.ParserTest;

public class HashLiteralExpressionTest
{
    [Fact(DisplayName = "Parsin hash literals")]
    public void TestParseHashLiterals()
    {
        var input = "{\"one\":1, \"two\":2,\"three\":3}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        program.Statements[0].Should().BeOfType<ExpressionStatement>();
        var stm = (program.Statements[0] as ExpressionStatement);
        stm.Expression.Should().BeOfType<HashLiteral>();
        var hash = (stm.Expression as HashLiteral);
        hash.Pairs.Should().HaveCount(3);

        var expected = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        foreach (var pair in hash.Pairs)
        {
            var literal = pair.Key;
            literal.Should().BeOfType<StringLiteral>();
            var expectedValue = expected[literal.String()];
            Helpers.TestIntegerLiteral(pair.Value, expectedValue);
        }
    }

    [Fact(DisplayName = "Parsin empty hash literals")]
    public void TestParseEmptyHashLiterals()
    {
        var input = "{}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        program.Statements[0].Should().BeOfType<ExpressionStatement>();
        var stm = (program.Statements[0] as ExpressionStatement);
        stm.Expression.Should().BeOfType<HashLiteral>();
        var hash = (stm.Expression as HashLiteral);
        hash.Pairs.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Parsin hash literals with expression")]
    public void TestParseHashLiteralsWithExpression()
    {
        var input = "{\"one\": 0 + 1, \"two\": 10 - 8, \"three\": 15 / 5}";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        program.Statements[0].Should().BeOfType<ExpressionStatement>();
        var stm = (program.Statements[0] as ExpressionStatement);
        stm.Expression.Should().BeOfType<HashLiteral>();
        var hash = (stm.Expression as HashLiteral);
        hash.Pairs.Should().HaveCount(3);

        var tests = new Dictionary<string, Action<IExpression>>
        {
            { "one", _ => Helpers.TestInfixExpression(_,0,"+",1) },
            { "two",  _ => Helpers.TestInfixExpression(_, 10,"-",8)},
            { "three", _ => Helpers.TestInfixExpression(_, 15,"/", 5) }
        };

        foreach (var pair in hash.Pairs)
        {
            var literal = pair.Key;
            literal.Should().BeOfType<StringLiteral>();
            var func = tests[literal.String()];
            func(pair.Value);
        }
    }
}

