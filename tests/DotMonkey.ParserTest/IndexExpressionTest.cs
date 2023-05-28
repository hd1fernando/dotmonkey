using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Statements;
using FluentAssertions;
using Xunit;

namespace DotMonkey.ParserTest;

public class IndexExpressionTest
{
    [Fact(DisplayName = "Index expression")]
    public void ParserIndexExpression()
    {
        var input = "myArray[1 + 1]";
        var lexer = new Lexer(input);
        var parser = new Parser.AST.Parser(lexer);

        var program = parser.ParserProgram();

        program.Statements[0].Should().BeOfType<ExpressionStatement>();
        var statement = program.Statements[0] as ExpressionStatement;
        statement.Expression.Should().BeOfType<IndexExpression>();
        var indexExp = statement.Expression as IndexExpression;
        Helpers.TestIdentifier(indexExp.Left, "myArray");
        Helpers.TestInfixExpression(indexExp.Index, 1, "+", 1);
    }
}
