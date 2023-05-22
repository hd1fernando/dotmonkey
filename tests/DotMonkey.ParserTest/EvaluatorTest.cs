using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.Eval;
using DotMonkey.Parser.Object;
using FluentAssertions;
using System;
using Xunit;

namespace DotMonkey.ParserTest;

public class EvaluatorTest
{
    [Theory(DisplayName = "Eval integer expresson")]
    [InlineData("5", 5)]
    [InlineData("10", 10)]
    public void Test1(string input, int expected)
    {
        var evaluated = TestEval(input);

        TestIntergerObject(evaluated, expected);
    }

    private void TestIntergerObject(IObject @object, int expected)
    {
        @object.Should().BeOfType<Integer>();
        var result = (Integer)@object;
        result.Value.Should().Be(expected);
    }

    public IObject TestEval(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        Program program = parser.ParserProgram();

        return new Evaluator().Eval(program);
    }

}
