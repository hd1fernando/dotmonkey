using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using System;
using System.Collections.Generic;

namespace DotMonkey.Parser.AST;

public class Parser
{
    private Dictionary<string, PrefixParserFn> PrefixParserFns = new Dictionary<string, PrefixParserFn>();
    private Dictionary<string, InfixParserFn> InfixParserFns = new Dictionary<string, InfixParserFn>();

    public delegate IExpression PrefixParserFn();
    public delegate IExpression InfixParserFn(IExpression expression);



    private Lexer _lexer { get; init; }
    public Token CurrentToken { get; private set; }
    public Token PeekToken { get; private set; }
    public List<string> Errors { get; private set; } = new List<string>();


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

        while (CurrentTokenIs(Constants.EOF) == false)
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


    public void RegisterPrefix(string constant, PrefixParserFn fn)
    {
        //PrefixParserFns[constant] = fn;
        PrefixParserFns.Add(constant, fn);
    }

    public void RegisterInfix(string constant, InfixParserFn fn)
    {
        InfixParserFns.Add(constant, fn);
    }


    private IStatement ParserStatement()
    {
        return CurrentToken.Type switch
        {
            Constants.LET => ParserLetStatement(),
            Constants.RETURN => ParserReturnStatement(),
            _ => null
        };
    }

    private ReturnStatement ParserReturnStatement()
    {
        var statement = new ReturnStatement(CurrentToken);

        NextToken();

        // TODO: we're skipping the expression until we enconter a semicolon
        while (CurrentTokenIs(Constants.SEMICOLON) == false)
        {
            NextToken();
        }

        return statement;
    }

    private LetStatement ParserLetStatement()
    {
        var statement = new LetStatement(CurrentToken);

        if (ExpectedPeek(Constants.IDENT) == false)
        {
            return null;
        }

        statement.Name = new Identifier(CurrentToken, CurrentToken.Literal);

        if (ExpectedPeek(Constants.ASSING) == false)
        {
            return null;
        }

        // TODO: we're skipping the expression until we enconter a semicolon
        while (CurrentTokenIs(Constants.SEMICOLON) == false)
        {
            NextToken();
        }

        return statement;
    }

    private bool PeekTokenIs(string constant)
    {
        return PeekToken.Type == constant;
    }

    private void PeekError(string constant)
    {
        var message = $"expected next token to be {constant}, got {PeekToken.Type} instead.";
        Errors.Add(message);
    }

    private bool CurrentTokenIs(string constant)
    {
        return CurrentToken.Type == constant;
    }

    private bool ExpectedPeek(string constant)
    {
        if (PeekTokenIs(constant))
        {
            NextToken();
            return true;
        }

        PeekError(constant);
        return false;
    }

    private void NextToken()
    {
        CurrentToken = PeekToken;
        PeekToken = _lexer.NextToken();
    }
}
