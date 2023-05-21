using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class ReturnStatementTest
{
    [Theory(DisplayName = "Let statement should work")]
    [InlineData("return 5;")]
    [InlineData(" return add(15);")]
    public void Test1(string code)
    {
        var lexer = new Lexer(code);
        var parser = new Parser.AST.Parser(lexer);

        Program result = parser.ParserProgram();

        result.Should().NotBeNull();
        result.TokenLiteral().Should().BeEquivalentTo("return");
    }


    [Theory()]
    [InlineData("return x;", "x")]
    [InlineData("return 5;", 5)]
    public void Test(string input, object expectedValue)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        Program program = parser.ParserProgram();

        program.Statements.Should().HaveCount(1);
        var statement = program.Statements[0];

        Helpers.TestLiteralExpression((statement as ReturnStatement).ReturnValue, expectedValue);
    }
}
