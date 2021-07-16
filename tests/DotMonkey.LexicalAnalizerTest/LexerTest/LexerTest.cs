using DotMonkey.LexicalAnalizer;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
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

        [Property(MaxTest = 10_000, Arbitrary = new[] { typeof(NumberGenerator) })]
        public void TestIntToken(int value)
        {
            var code = value.ToString();
            var lexer = new Lexer(code);

            var result = lexer.NextToken();

            result.Type.Should().BeEquivalentTo(Constants.INT);
        }
    }

    public static class NumberGenerator
    {
        public static Arbitrary<int> Generate()
            => Arb.Default.Int32().Filter(d => d >= 0 && d <= int.MaxValue);
    }
}
