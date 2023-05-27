using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.Eval;
using DotMonkey.Parser.Object;
using FluentAssertions;
using System;
using Xunit;
using Environment = DotMonkey.Parser.Object.Environment;

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

    [Theory(DisplayName = "IF ELSE expression")]
    [InlineData("if (true) { 10 }", 10)]
    [InlineData("if (false) { 10 }", null)]
    [InlineData("if (1) { 10 }", 10)]
    [InlineData("if (1 < 2) { 10 }", 10)]
    [InlineData("if (1 > 2) { 10 }", null)]
    [InlineData("if (1 > 2) { 10 } else { 20 }", 20)]
    [InlineData("if (1 < 2) { 10 } else { 20 }", 10)]
    public void TestIfElsExpression(string input, object expected)
    {
        var evalueated = TestEval(input);

        if (expected is int)
        {
            TestIntergerObject(evalueated, (int)expected);
        }
        else
        {
            TestNullObject(evalueated);
        }
    }


    [Theory(DisplayName = "Return Statement")]
    [InlineData("return 10;", 10)]
    [InlineData("return 10; 9;", 10)]
    [InlineData("return 2 * 5; 9;", 10)]
    [InlineData("9; return 2 * 5; 9;", 10)]
    [InlineData("if (10 > 1) { if (10 > 1) { return 10; } return 1; }", 10)]
    public void TestReturnStatement(string input, int expected)
    {
        var evaluated = TestEval(input);
        TestIntergerObject(evaluated, expected);
    }

    [Theory(DisplayName = "Error Handling")]
    [InlineData("5 + true;", "type mismatch: INTEGER + BOOLEAN")]
    [InlineData("5 + true; 5;", "type mismatch: INTEGER + BOOLEAN")]
    [InlineData("-true", "unknown operator: -BOOLEAN")]
    [InlineData("true + false;", "unknown operator: BOOLEAN + BOOLEAN")]
    [InlineData("5; true + false; 5", "unknown operator: BOOLEAN + BOOLEAN")]
    [InlineData("if (10 > 1) { true + false; }", "unknown operator: BOOLEAN + BOOLEAN")]
    [InlineData("if (10 > 1) { if (10 > 1) { return true + false; } return 1; }", "unknown operator: BOOLEAN + BOOLEAN")]
    [InlineData("foobar", "identifier not found: foobar")]
    [InlineData("\"Hello\"-\"World\"", "unknown operator: STRING - STRING")]
    public void TestErrorHandling(string input, string expected)
    {
        var evaluated = TestEval(input);

        evaluated.Should().BeOfType<Error>();
        ((Error)evaluated).Message.Should().BeEquivalentTo(expected);
    }

    [Theory(DisplayName = "Let Statement")]
    [InlineData("let a = 5; a;", 5)]
    [InlineData("let a = 5 * 5; a;", 25)]
    [InlineData("let a = 5; let b = a; b;", 5)]
    [InlineData("let a = 5; let b = a; let c = a + b + 5; c;", 15)]
    public void TestLetStatement(string input, int expected)
    {
        var evaluated = TestEval(input);

        TestIntergerObject(evaluated, expected);
    }

    [Fact(DisplayName = "Function object")]
    public void TestFunctionObject()
    {
        var input = "fn(x){x+2;}";
        var evaluated = TestEval(input);

        evaluated.Should().BeOfType<Function>();
        var function = ((Function)evaluated);
        function.Parameters.Should().HaveCount(1);
        function.Parameters[0].String().Should().BeEquivalentTo("x");
        function.Body.String().Should().BeEquivalentTo("(x+2)");
    }

    [Theory(DisplayName = "Function Application")]
    [InlineData("let identity = fn(x) { x; }; identity(5);", 5)]
    [InlineData("let identity = fn(x) { return x; }; identity(5);", 5)]
    [InlineData("let double = fn(x) { x * 2; }; double(5);", 10)]
    [InlineData("let add = fn(x, y) { x + y; }; add(5, 5);", 10)]
    [InlineData("let add = fn(x, y) { x + y; }; add(5 + 5, add(5, 5));", 20)]
    [InlineData("fn(x) { x; }(5)", 5)]
    public void TestFunctionApplication(string input, int expected)
    {
        var evaluated = TestEval(input);
        TestIntergerObject(evaluated, expected);
    }

    [Theory(DisplayName = "Test Closure")]
    [InlineData(
        "let newAdder = fn(x) { " +
                "fn(y) { x + y }; " +
              "}; " +
            "let addTwo = newAdder(2);" +
            " addTwo(2);", 4)]
    public void TestClosures(string input, int expected)
    {
        var evaluated = TestEval(input);
        TestIntergerObject(evaluated, expected);
    }


    [Fact(DisplayName = "String Literal")]
    public void TestStringLiteral()
    {
        var input = "\"Hello world!\"";

        var evaluated = TestEval(input);

        evaluated.Should().BeOfType<_String>();
        ((_String)evaluated).Value.Should().BeEquivalentTo(input.Replace("\"", ""));
    }

    [Fact(DisplayName = "String Concatenation")]
    public void TestStringConcatenation()
    {
        var input = "\"Hello\"+\" \"+\"world!\"";

        var evaluated = TestEval(input);

        evaluated.Should().BeOfType<_String>();
        ((_String)evaluated).Value.Should().BeEquivalentTo("Hello world!");
    }


    private void TestNullObject(IObject evalueated)
    {
        evalueated.Should().BeOfType<NULL>();
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

        var env = new Environment();

        return new Evaluator().Eval(program, env);
    }

}
