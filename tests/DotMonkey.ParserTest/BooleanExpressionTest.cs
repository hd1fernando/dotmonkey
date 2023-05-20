using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class BooleanExpressionTest
{
    [Theory(DisplayName = "Test Boolan expression")]
    [InlineData("true")]
    [InlineData("false")]
    public void Test(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        parser.Errors.Should().BeEmpty();
        result.Statements.Should().HaveCount(1);
        IStatement identifier = result.Statements[0];
        identifier.Should().BeOfType<ExpressionStatement>();
        Helpers.TestBoolean((identifier as ExpressionStatement).Expression, bool.Parse(input));
    }
}

