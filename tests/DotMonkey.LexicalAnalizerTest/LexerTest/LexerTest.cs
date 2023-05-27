using DotMonkey.LexicalAnalizer;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System.Collections.Generic;
using Xunit;

namespace DotMonkey.LexicalAnalizerTest.LexerTest;

public class LexerTest
{
    [Theory(DisplayName = "Test next token")]
    [InlineData("test88", Constants.IDENT)]
    [InlineData("let", Constants.LET)]
    [InlineData("fn", Constants.FUNCTION)]
    [InlineData("true", Constants.TRUE)]
    [InlineData("false", Constants.FALSE)]
    [InlineData("if", Constants.IF)]
    [InlineData("else", Constants.ELSE)]
    [InlineData("return", Constants.RETURN)]
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
    [InlineData(null, Constants.EOF)]
    [InlineData("", Constants.EOF)]
    [InlineData(" ", Constants.EOF)]
    [InlineData("\0", Constants.EOF)]
    [InlineData("\t", Constants.EOF)]
    [InlineData("\n", Constants.EOF)]
    [InlineData("^", Constants.ILLEGAL)]
    [InlineData("\"foobar\"", Constants.STRING)]
    [InlineData("\"foo bar\"", Constants.STRING)]
    [Trait("Lexical Analizer", nameof(Lexer))]
    public void test1(string input, string expectedToken)
    {
        var lexer = new Lexer(input);

        var result = lexer.NextToken();

        result.Type.Should().BeEquivalentTo(expectedToken, because: $"'{input}' generates an inexpected ouput");
    }


    [Theory(DisplayName = "Returns tokens based on the input")]
    //[InlineData("test88;", new[] { Constants.IDENT, Constants.SEMICOLON }, Skip = "I don't know if the languge will support this.")]
    [InlineData("test;", new[] { Constants.IDENT, Constants.SEMICOLON, Constants.EOF })]
    [InlineData("===", new[] { Constants.EQ, Constants.ASSING, Constants.EOF })]
    [InlineData("8;", new[] { Constants.INT, Constants.SEMICOLON, Constants.EOF })]
    [InlineData("42;", new[] { Constants.INT, Constants.SEMICOLON, Constants.EOF })]
    [InlineData("=+(){},;", new[] { Constants.ASSING, Constants.PLUS, Constants.LPARENT, Constants.RPARENT, Constants.LBRACE, Constants.RBRACE, Constants.COMMA, Constants.SEMICOLON, Constants.EOF })]
    [InlineData(
        "let five = 5; " +
             "let ten = 10; " +
             "let add = fn( x, y) {" +
                "x + y; " +
             "};" +
             "let result = add(five, ten);" +
             "!-/*5;" +
             "5 < 10 > 5;" +
             "if (5 < 10) {" +
             "  return true; " +
             "} else {" +
             "  return false; " +
             "}" +
             "10 == 10;" +
             "10 != 9;" +
             "\"foobar\""+
             "\"foo bar\""
           ,
            new[]
            {
                Constants.LET, Constants.IDENT, Constants.ASSING, Constants.INT, Constants.SEMICOLON,
                Constants.LET, Constants.IDENT, Constants.ASSING, Constants.INT, Constants.SEMICOLON,
                Constants.LET, Constants.IDENT, Constants.ASSING ,Constants.FUNCTION, Constants.LPARENT, Constants.IDENT, Constants.COMMA, Constants.IDENT, Constants.RPARENT, Constants.LBRACE,
                Constants.IDENT, Constants.PLUS, Constants.IDENT, Constants.SEMICOLON,
                Constants.RBRACE, Constants.SEMICOLON,
                Constants.LET, Constants.IDENT, Constants.ASSING, Constants.IDENT, Constants.LPARENT, Constants.IDENT, Constants.COMMA, Constants.IDENT, Constants.RPARENT, Constants.SEMICOLON,
                Constants.BANG, Constants.MINUS, Constants.SLASH, Constants.ASTERISK, Constants.INT, Constants.SEMICOLON,
                Constants.INT, Constants.LT, Constants.INT, Constants.GT, Constants.INT, Constants.SEMICOLON,
                Constants.IF, Constants.LPARENT, Constants.INT, Constants.LT, Constants.INT, Constants.RPARENT, Constants.LBRACE,
                Constants.RETURN, Constants.TRUE, Constants.SEMICOLON,
                Constants.RBRACE, Constants.ELSE, Constants.LBRACE,
                Constants.RETURN, Constants.FALSE, Constants.SEMICOLON,
                Constants.RBRACE,
                Constants.INT, Constants.EQ, Constants.INT, Constants.SEMICOLON,
                Constants.INT, Constants.NOT_EQ,Constants.INT, Constants.SEMICOLON,
                Constants.STRING,
                Constants.STRING,

                Constants.EOF,
            })]
    [Trait("Lexical Analizer", nameof(Lexer))]
    public void TestNextToken(string input, string[] expectedTokens)
    {
        var lexer = new Lexer(input);

        Token token;
        var listOfTokens = new List<string>();
        do
        {
            token = lexer.NextToken();
            listOfTokens.Add(token.Type);

        } while (token.Type != Constants.EOF);

        for (int i = 0; i < listOfTokens.Count; ++i)
        {
            listOfTokens[i].Should().BeEquivalentTo(expectedTokens[i]);
        }

    }

    [Property(MaxTest = 10_000, Arbitrary = new[] { typeof(NumberGenerator) }, DisplayName = "Test INT token")]
    [Trait("Lexical Analizer", nameof(Lexer))]
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
