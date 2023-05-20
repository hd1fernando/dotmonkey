﻿using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using System;
using System.Collections.Generic;

namespace DotMonkey.Parser.AST;

public class Parser
{
    private Dictionary<string, Func<IExpression>> PrefixParserFns = new Dictionary<string, Func<IExpression>>();
    private Dictionary<string, Func<IExpression, IExpression>> InfixParserFns = new Dictionary<string, Func<IExpression, IExpression>>();

    private Dictionary<string, Precedences> _precedenceTable = new Dictionary<string, Precedences>
    {
        {Constants.EQ, Precedences.EQUALS },
        {Constants.NOT_EQ, Precedences.EQUALS },
        {Constants.LT, Precedences.LESSGREATER },
        {Constants.GT, Precedences.LESSGREATER },
        {Constants.PLUS, Precedences.SUM },
        {Constants.MINUS, Precedences.SUM },
        {Constants.SLASH, Precedences.PRODUCT },
        {Constants.ASTERISK, Precedences.PRODUCT },
    };

    private Lexer _lexer { get; init; }
    public Token CurrentToken { get; private set; }
    public Token PeekToken { get; private set; }
    public List<string> Errors { get; private set; } = new List<string>();


    public Parser(Lexer lexer)
    {
        _lexer = lexer;


        RegisterPrefix(Constants.IDENT, () => new Identifier(CurrentToken, CurrentToken.Literal));
        RegisterPrefix(Constants.INT, ParserIntergerLiteral);
        RegisterPrefix(Constants.BANG, ParserPrefixExpression);
        RegisterPrefix(Constants.MINUS, ParserPrefixExpression);
        RegisterPrefix(Constants.TRUE, ParserBoolean);
        RegisterPrefix(Constants.FALSE, ParserBoolean);

        RegisterInfix(Constants.PLUS, ParserInfixExpression);
        RegisterInfix(Constants.MINUS, ParserInfixExpression);
        RegisterInfix(Constants.SLASH, ParserInfixExpression);
        RegisterInfix(Constants.ASTERISK, ParserInfixExpression);
        RegisterInfix(Constants.EQ, ParserInfixExpression);
        RegisterInfix(Constants.NOT_EQ, ParserInfixExpression);
        RegisterInfix(Constants.LT, ParserInfixExpression);
        RegisterInfix(Constants.GT, ParserInfixExpression);

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


    private void RegisterPrefix(string constant, Func<IExpression> fn)
    {
        PrefixParserFns.Add(constant, fn);
    }

    private void RegisterInfix(string constant, Func<IExpression, IExpression> fn)
    {
        InfixParserFns.Add(constant, fn);
    }

    private IExpression ParserIntergerLiteral()
    {
        var lit = new IntegerLiteral(CurrentToken);

        if (int.TryParse(CurrentToken.Literal, out var value) == false)
        {
            var message = $"Could not parser {CurrentToken.Literal} as interger";
            Errors.Add(message);
            return null;
        }

        lit.Value = value;

        return lit;
    }

    private IExpression ParserBoolean()
    {
        return new BooleanExpression(CurrentToken, CurrentTokenIs(Constants.TRUE));
    }


    private IExpression ParserPrefixExpression()
    {
        var expression = new PrefixExpression(CurrentToken, CurrentToken.Literal);

        NextToken();

        expression.Rigth = ParserExpression(Precedences.PREFIX);

        return expression;
    }

    private IExpression ParserInfixExpression(IExpression left)
    {
        var expression = new InfixExpression(CurrentToken, left, CurrentToken.Literal);

        var precedence = CurrentPrecedence();
        NextToken();
        expression.Rigth = ParserExpression(precedence);

        return expression;

    }

    private IStatement ParserStatement()
    {
        return CurrentToken.Type switch
        {
            Constants.LET => ParserLetStatement(),
            Constants.RETURN => ParserReturnStatement(),
            _ => ParserExpressionStatement()
        };
    }

    private ExpressionStatement ParserExpressionStatement()
    {
        var statement = new ExpressionStatement(CurrentToken);

        statement.Expression = ParserExpression(Precedences.LOWEST);

        if (PeekTokenIs(Constants.SEMICOLON))
        {
            NextToken();
        }

        return statement;
    }

    private IExpression ParserExpression(Precedences precedence)
    {
        PrefixParserFns.TryGetValue(CurrentToken.Type, out var prefix);

        if (prefix is null)
        {
            NoPrefixParserFnError(CurrentToken.Type);
            return null;
        }

        var leftExp = prefix();

        while (PeekTokenIs(Constants.SEMICOLON) == false && precedence < PeekPrecedence())
        {
            InfixParserFns.TryGetValue(PeekToken.Type, out var infix);

            if (infix is null)
                return leftExp;

            NextToken();

            leftExp = infix(leftExp);
        }

        return leftExp;
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

    private Precedences PeekPrecedence()
    {
        if (_precedenceTable.TryGetValue(PeekToken.Type, out var value))
        {
            return value;
        }
        return Precedences.LOWEST;
    }

    private Precedences CurrentPrecedence()
    {
        if (_precedenceTable.TryGetValue(CurrentToken.Type, out var value))
        {
            return value;
        }
        return Precedences.LOWEST;
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

    private void NoPrefixParserFnError(string constant)
    {
        var message = $"no prefix parser for function {constant} found";
        Errors.Add(message);
    }
}

public enum Precedences
{
    // the order is important in here. Don't change.
    LOWEST,
    EQUALS, // ==
    LESSGREATER, // > OR <
    SUM, // +
    PRODUCT, // *
    PREFIX, // -x or !x
    CALL, // myFunction(x)
}
