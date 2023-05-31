using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class ArrayLiteralsExpressionTest
{
    [Fact(DisplayName = "Array Literal")]
    public void TestParsingArrayLiterals()
    {
        var input = "[1, 2 * 2, 3 + 3]";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        program.Statements[0].Should().BeOfType<ExpressionStatement>();
        var stm = (program.Statements[0] as ExpressionStatement);
        stm.Expression.Should().BeOfType<ArrayLiteral>();
        var array = (stm.Expression as ArrayLiteral);
        array.Elements.Should().HaveCount(3);

        Helpers.TestIntegerLiteral(array.Elements[0], 1);
        Helpers.TestInfixExpression(array.Elements[1], 2, "*", 2);
        Helpers.TestInfixExpression(array.Elements[2], 3, "+", 3);
    }
}

