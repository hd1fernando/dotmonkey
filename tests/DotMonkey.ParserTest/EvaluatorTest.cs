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
    [InlineData("5 + 5 + 5 + 5 - 10", 10)]
    [InlineData("2 * 2 * 2 * 2 * 2", 32)]
    [InlineData("-50 + 100 + -50", 0)]
    [InlineData("5 * 2 + 10", 20)]
    [InlineData("5 + 2 * 10", 25)]
    [InlineData("20 + 2 * -10", 0)]
    [InlineData("50 / 2 * 2 + 10", 60)]
    [InlineData("2 * (5 + 10)", 30)]
    [InlineData("3 * 3 * 3 + 10", 37)]
    [InlineData("3 * (3 * 3) + 10", 37)]
    [InlineData("(5 + 10 * 2 + 15 / 3) * 2 + -10", 50)]
    public void Test1(string input, int expected)
    {
        var evaluated = TestEval(input);

        TestIntergerObject(evaluated, expected);
    }


    [Theory(DisplayName = "Eval Boolean expression")]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("1 < 2", true)]
    [InlineData("1 > 2", false)]
    [InlineData("1 < 1", false)]
    [InlineData("1 > 1", false)]
    [InlineData("1 == 1", true)]
    [InlineData("1 != 1", false)]
    [InlineData("1 == 2", false)]
    [InlineData("1 != 2", true)]
    [InlineData("true == true", true)]
    [InlineData("false == false", true)]
    [InlineData("true == false", false)]
    [InlineData("true != false", true)]
    [InlineData("false != true", true)]
    [InlineData("(1 < 2) == true", true)]
    [InlineData("(1 < 2) == false", false)]
    [InlineData("(1 > 2) == true", false)]
    [InlineData("(1 > 2) == false", true)]
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
