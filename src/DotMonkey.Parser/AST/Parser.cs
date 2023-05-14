using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using System;

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
    public Program ParserProgram()
    {
        Program program = ConstrucRootNodeOfAST();

        while (CurrentToken.Type != Constants.EOF)
        {
            var statement = ParserStatement();

            if (statement is not null)
            {
                program.AddSteatment(statement);
            }

            NextToken();
        }

        return program;

        static Program ConstrucRootNodeOfAST() => new Program();
    }

    private IStatement ParserStatement()
    {
        return CurrentToken.Type switch
        {
            Constants.LET => ParserLetStatement(),
            _ => null
        };
    }

    private IStatement ParserLetStatement()
    {
        throw new NotImplementedException();
    }

    private void NextToken()
    {
        CurrentToken = PeekToken;
        PeekToken = _lexer.NextToken();
    }
}
