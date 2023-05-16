using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
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
}
