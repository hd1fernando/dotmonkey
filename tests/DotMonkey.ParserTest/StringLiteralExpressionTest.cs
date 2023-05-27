using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class StringLiteralExpressionTest
{
    [Theory(DisplayName = "Test String Literal")]
    [InlineData("\"Hello world;\"")]
    public void Test(string input)
    {
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var result = parser.ParserProgram();

        var statement = result.Statements[0];
        statement.Should().BeOfType<ExpressionStatement>();
        var stringLiteral = (statement as ExpressionStatement).Expression;
        stringLiteral.Should().BeOfType<StringLiteral>();
        (stringLiteral as StringLiteral).Value.Should().BeEquivalentTo(input.Replace("\"", ""));
    }
}

