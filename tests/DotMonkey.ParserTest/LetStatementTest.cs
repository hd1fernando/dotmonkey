using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Xunit;

namespace DotMonkey.ParserTest;

public class LetStatementTest
{
    [Theory(DisplayName = "Let statement should work")]
    [InlineData("let x = 5;")]
    [InlineData(" let y = 10;")]
    public void Test1(string code)
    {
        var lexer = new Lexer(code);
        var parser = new Parser.AST.Parser(lexer);

        Program result = parser.ParserProgram();

        result.Should().NotBeNull();
        result.TokenLiteral().Should().BeEquivalentTo("let");
    }

    [Theory()]
    [InlineData("let x = 5;", "x", 5)]
    [InlineData("let y = true;", "y", true)]
    [InlineData("let foobar = y;", "foobar", "y")]
    public void Test(string input, string exptectedIdentifier, object expectedValue)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        Program program = parser.ParserProgram();

        program.Statements.Should().HaveCount(1);
        var statement = program.Statements[0];

        Helpers.TestLetStatement(statement, exptectedIdentifier);
        Helpers.TestLiteralExpression((statement as LetStatement).Value, expectedValue);
    }

    [Theory(DisplayName = "Let statement has error when the value after let word isn't a ident")]
    [InlineData("let = 10;", Constants.IDENT, Constants.ASSING)]
    [InlineData("let 42;", Constants.IDENT, Constants.INT)]
    public void Test2(string code, string expectedConstant, string gotConstant)
    {
        var lexer = new Lexer(code);
        var parser = new Parser.AST.Parser(lexer);

        Program result = parser.ParserProgram();

        parser.Errors.Should().NotBeEmpty();
        parser.Errors.Should().Contain($"expected next token to be {expectedConstant}, got {gotConstant} instead.");
    }

    [Theory(DisplayName = "Let statement has error when the value ident is not assing")]
    [InlineData("let x 5;", Constants.ASSING, Constants.INT)]
    public void Test3(string code, string expectedConstant, string gotConstant)
    {
        var lexer = new Lexer(code);
        var parser = new Parser.AST.Parser(lexer);

        Program result = parser.ParserProgram();

        parser.Errors.Should().NotBeEmpty();
        parser.Errors.Should().Contain($"expected next token to be {expectedConstant}, got {gotConstant} instead.");
    }
}
