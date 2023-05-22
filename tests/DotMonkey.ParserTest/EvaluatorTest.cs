using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.Eval;
using DotMonkey.Parser.Object;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class EvaluatorTest
{
    [Theory(DisplayName = "Eval integer expression")]
    [InlineData("5", 5)]
    [InlineData("10", 10)]
    [InlineData("-5", -5)]
    [InlineData("-10", -10)]
    public void Test1(string input, int expected)
    {
        var evaluated = TestEval(input);

        TestIntergerObject(evaluated, expected);
    }


    [Theory(DisplayName = "Eval Boolean objects")]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void Test2(string input, bool expected)
    {
        var evaluated = TestEval(input);

        TestIBooleanObject(evaluated, expected);
    }

    [Theory(DisplayName = "Bang operator ")]
    [InlineData("!true", false)]
    [InlineData("!false", true)]
    [InlineData("!5", false)]
    [InlineData("!!true", true)]
    [InlineData("!!false", false)]
    [InlineData("!!5", true)]
    public void TestBangOperator(string input, bool expected)
    {
        var evaluated = TestEval(input);

        TestIBooleanObject(evaluated, expected);
    }



    private void TestIBooleanObject(IObject @object, bool expected)
    {
        @object.Should().BeOfType<_Boolean>();
        var result = (_Boolean)@object;
        result.Value.Should().Be(expected);
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
