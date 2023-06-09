﻿using System;
using System.Net.NetworkInformation;

namespace DotMonkey.LexicalAnalizer;

public class Lexer
{
    private string _input { get; set; }
    private int _currentPositionInInput { get; set; } // Position
    private int _readPosition { get; set; }  // points to the next char in the input
    private char _charUnderExamination { get; set; } // CH

    private const int NULL_CHAR = 0;

    public Lexer(string input)
    {
        _input = input;
        ReadChar();
    }

    public Token NextToken()
    {
        var token = new Token();
        SkipWhitespace();
        if (_charUnderExamination == NULL_CHAR)
            token = new Token(Constants.EOF, string.Empty);

        token = _charUnderExamination switch
        {
            '=' => CheckForEqualsOrAssing(),
            ';' => new Token(Constants.SEMICOLON, _charUnderExamination.ToString()),
            '(' => new Token(Constants.LPARENT, _charUnderExamination.ToString()),
            ')' => new Token(Constants.RPARENT, _charUnderExamination.ToString()),
            '+' => new Token(Constants.PLUS, _charUnderExamination.ToString()),
            '-' => new Token(Constants.MINUS, _charUnderExamination.ToString()),
            '!' => CheckForBangOrNotEqual(),
            '*' => new Token(Constants.ASTERISK, _charUnderExamination.ToString()),
            '/' => new Token(Constants.SLASH, _charUnderExamination.ToString()),
            '{' => new Token(Constants.LBRACE, _charUnderExamination.ToString()),
            '}' => new Token(Constants.RBRACE, _charUnderExamination.ToString()),
            ',' => new Token(Constants.COMMA, _charUnderExamination.ToString()),
            '<' => new Token(Constants.LT, _charUnderExamination.ToString()),
            '>' => new Token(Constants.GT, _charUnderExamination.ToString()),
            '\0' => new Token(Constants.EOF, _charUnderExamination.ToString()),
            '"' => new Token(Constants.STRING, ReadString()),
            '[' => new Token(Constants.LBRACKET, _charUnderExamination.ToString()),
            ']' => new Token(Constants.RBRACKET, _charUnderExamination.ToString()),
            ':' => new Token(Constants.COLON, _charUnderExamination.ToString()),
            _ => DefautlPattern(token)
        };

        ReadChar();

        return token;

        Token DefautlPattern(Token token)
        {
            if (IsLetter(_charUnderExamination))
            {
                token.Literal = ReadIdentifier();
                return new Token(token.LookupIdent(), token.Literal);
            }

            if (char.IsDigit(_charUnderExamination))
            {
                token.Literal = ReadNumber();
                return new Token(Constants.INT, token.Literal);
            }

            return new Token(Constants.ILLEGAL, _charUnderExamination.ToString());
        }

        Token CheckForEqualsOrAssing()
        {
            if (PeekChar() == '=')
            {
                var localCh = _charUnderExamination;
                ReadChar();
                return new Token(Constants.EQ, localCh.ToString() + _charUnderExamination.ToString());
            }
            return new Token(Constants.ASSING, _charUnderExamination.ToString());
        }

        Token CheckForBangOrNotEqual()
        {
            if (PeekChar() == '=')
            {
                var localCh = _charUnderExamination;
                ReadChar();
                return new Token(Constants.NOT_EQ, localCh.ToString() + _charUnderExamination.ToString());
            }
            return new Token(Constants.BANG, _charUnderExamination.ToString());
        }
    }

    private string ReadString()
    {
        var position = _currentPositionInInput + 1;

        do
        {
            ReadChar();
        }
        while (_charUnderExamination != '"' && _charUnderExamination != 0);

        return _input[position.._currentPositionInInput];
    }

    private void ReadChar()
    {
        _charUnderExamination = IsTheEndOfInput()
            ? NullASCII()
            : _input[_readPosition];

        _currentPositionInInput = _readPosition;
        _readPosition++;
    }

    private bool IsLetter(char ch)
        => char.IsLetter(ch) || ch == '_';

    private void SkipWhitespace()
    {
        while (_charUnderExamination is ('\t' or '\n' or ' '))
            ReadChar();
    }

    private string ReadIdentifier()
    {
        var postion = _currentPositionInInput;
        while (IsLetter(_charUnderExamination))
            ReadChar();

        _readPosition--;

        return _input[postion.._currentPositionInInput];
    }

    private string ReadNumber()
    {
        var position = _currentPositionInInput;
        while (char.IsDigit(_charUnderExamination))
            ReadChar();

        _readPosition--;

        return _input[position.._currentPositionInInput];
    }

    private char PeekChar()
        => IsTheEndOfInput()
        ? NullASCII()
        : _input[_readPosition];

    private char NullASCII()
    {
        // Unicode and ASCII for NULL char. For more information see: https://www.ascii-code.com/ and https://unicode-table.com/en/
        return Convert.ToChar(NULL_CHAR);
    }

    private bool IsTheEndOfInput() => _input is null || _readPosition >= _input?.Length;
}
