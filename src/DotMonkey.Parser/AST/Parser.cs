using DotMonkey.LexicalAnalizer;

namespace DotMonkey.Parser.AST;

public class Parser
{
    private Lexer _lexer { get; init; }
    public Token CurrentToken { get; private set; }
    public Token PeekToken { get; private set; }

    public Parser(Lexer lexer)
    {
        _lexer = lexer;

        // Read two tokens, so CurrentToken and PeekToken are both set.
        NextToken();
        NextToken();
    }

    private void NextToken()
    {
        CurrentToken = PeekToken;
        PeekToken = _lexer.NextToken();
    }

    public Program ParserProgram()
    {
        return null;
    }
}
