using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest
{
    public class LetStatementTest
    {
        [Theory(DisplayName = "Let steatment should work")]
        [InlineData("let x = 5;")]
        [InlineData(" let y = 10;")]
        public void Test1(string code)
        {
            var lexer = new Lexer(code);
            var parser = new Parser.AST.Parser(lexer);

            Program result = parser.ParserProgram();

            result.Should().NotBeNull();
            result.Statements.Count.Should().Be(3, because: "Let shoud countain 3 statements");
            result.TokenLiteral().Should().BeEquivalentTo("let");
        }
    }
}
