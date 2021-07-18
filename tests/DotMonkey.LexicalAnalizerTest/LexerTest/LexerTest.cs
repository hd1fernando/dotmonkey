using DotMonkey.LexicalAnalizer;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System.Collections.Generic;
using Xunit;

namespace DotMonkey.LexicalAnalizerTest.LexerTest
{
    public class LexerTest
    {
        [Theory(DisplayName = "Test next token")]
        [InlineData("let", Constants.LET)]
        [InlineData("fn", Constants.FUNCTION)]
        [InlineData("true", Constants.TRUE)]
        [InlineData("false", Constants.FALSE)]
        [InlineData("if", Constants.IF)]
        [InlineData("else", Constants.ELSE)]
        [InlineData("return", Constants.RETURN)]
        [InlineData("\0", Constants.EOF)]
        [InlineData("=", Constants.ASSING)]
        [InlineData("+", Constants.PLUS)]
        [InlineData(",", Constants.COMMA)]
        [InlineData(";", Constants.SEMICOLON)]
        [InlineData("(", Constants.LPARENT)]
        [InlineData(")", Constants.RPARENT)]
        [InlineData("{", Constants.LBRACE)]
        [InlineData("}", Constants.RBRACE)]
        [InlineData("-", Constants.MINUS)]
        [InlineData("!", Constants.BANG)]
        [InlineData("*", Constants.ASTERISK)]
        [InlineData("/", Constants.SLASH)]
        [InlineData("<", Constants.LT)]
        [InlineData(">", Constants.GT)]
        [InlineData("==", Constants.EQ)]
        [InlineData("!=", Constants.NOT_EQ)]
        public void test1(string input, string expectedToken)
        {
            var lexer = new Lexer(input);

            var result = lexer.NextToken();

            result.Type.Should().BeEquivalentTo(expectedToken);
        }

        [Theory(DisplayName = "Returns a pair token based in a pair input")]
        [InlineData("test;", new[] { Constants.IDENT, Constants.SEMICOLON })]
        [InlineData("===", new[] { Constants.EQ, Constants.ASSING })]
        public void test2(string input, string[] expectedTokens)
        {
            var lexer = new Lexer(input);

            Token token;
            var listOfTokens = new List<string>();
            do
            {
                token = lexer.NextToken();
                listOfTokens.Add(token.Type);

            } while (token.Type != Constants.EOF);

            listOfTokens[0].Should().BeEquivalentTo(expectedTokens[0]);
            listOfTokens[1].Should().BeEquivalentTo(expectedTokens[1]);

        }

        [Property(MaxTest = 10_000, Arbitrary = new[] { typeof(NumberGenerator) }, DisplayName = "Test INT token")]
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
