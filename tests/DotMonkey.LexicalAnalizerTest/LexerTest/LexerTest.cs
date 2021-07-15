using DotMonkey.LexicalAnalizer;
using FluentAssertions;
using Xunit;

namespace DotMonkey.LexicalAnalizerTest.LexerTest
{
    public class LexerTest
    {
        [Theory(DisplayName = "Test next token")]
        [InlineData("let", Constants.LET)]
        [InlineData("=", Constants.ASSING)]
        [InlineData("+", Constants.PLUS)]
        [InlineData(",", Constants.COMMA)]
        [InlineData(";", Constants.SEMICOLON)]
        [InlineData("(", Constants.LPARENT)]
        [InlineData(")", Constants.RPARENT)]
        [InlineData("{", Constants.LBRACE)]
        [InlineData("}", Constants.RBRACE)]
        public void test1(string input, string expectedToken)
        {
            var lexer = new Lexer(input);

            var result = lexer.NextToken();

            result.Type.Should().BeEquivalentTo(expectedToken);
        }
    }
}
