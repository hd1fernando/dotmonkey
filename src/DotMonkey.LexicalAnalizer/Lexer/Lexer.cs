using System;

namespace DotMonkey.LexicalAnalizer;

public class Lexer
{
    public string Input { get; set; }
    public int CurrentPositionInInput { get; set; } // Position
    public int ReadPosition { get; set; }  // points to the next char in the input
    public char CharUnderExamination { get; set; } // CH

    private const int NULL_CHAR = 0;

    public Lexer(string input)
    {
        Input = input;
        ReadChar();
    }

    private void ReadChar()
    {
        var isTheEndOfInput = Input is null || ReadPosition >= Input?.Length;
        CharUnderExamination = isTheEndOfInput
            ? NullASCII()
            : Input[ReadPosition];

        CurrentPositionInInput = ReadPosition;
        ReadPosition++;
    }

    public Token NextToken()
    {
        var token = new Token();
        SkipWhitespace();
        if (CharUnderExamination == NULL_CHAR)
            token = new Token(Constants.EOF, string.Empty);

        token = CharUnderExamination switch
        {
            '=' => CheckForEqualsOrAssing(),
            ';' => new Token(Constants.SEMICOLON, CharUnderExamination.ToString()),
            '(' => new Token(Constants.LPARENT, CharUnderExamination.ToString()),
            ')' => new Token(Constants.RPARENT, CharUnderExamination.ToString()),
            '+' => new Token(Constants.PLUS, CharUnderExamination.ToString()),
            '-' => new Token(Constants.MINUS, CharUnderExamination.ToString()),
            '!' => CheckForBangOrNotEqual(),
            '*' => new Token(Constants.ASTERISK, CharUnderExamination.ToString()),
            '/' => new Token(Constants.SLASH, CharUnderExamination.ToString()),
            '{' => new Token(Constants.LBRACE, CharUnderExamination.ToString()),
            '}' => new Token(Constants.RBRACE, CharUnderExamination.ToString()),
            ',' => new Token(Constants.COMMA, CharUnderExamination.ToString()),
            '<' => new Token(Constants.LT, CharUnderExamination.ToString()),
            '>' => new Token(Constants.GT, CharUnderExamination.ToString()),
            '\0' => new Token(Constants.EOF, CharUnderExamination.ToString()),
            _ => DefautlPattern(token)
        };

        ReadChar();

        return token;

        Token DefautlPattern(Token token)
        {
            if (IsLetter(CharUnderExamination))
            {
                token.Literal = ReadIdentifier();
                return new Token(token.LookupIdent(), token.Literal);
            }

            if (char.IsDigit(CharUnderExamination))
            {
                token.Literal = ReadNumber();
                return new Token(Constants.INT, token.Literal);
            }

            return new Token(Constants.ILLEGAL, CharUnderExamination.ToString());
        }

        Token CheckForEqualsOrAssing()
        {
            if (PeekChar() == '=')
            {
                var localCh = CharUnderExamination;
                ReadChar();
                return new Token(Constants.EQ, localCh.ToString() + CharUnderExamination.ToString());
            }
            return new Token(Constants.ASSING, CharUnderExamination.ToString());
        }

        Token CheckForBangOrNotEqual()
        {
            if (PeekChar() == '=')
            {
                var localCh = CharUnderExamination;
                ReadChar();
                return new Token(Constants.NOT_EQ, localCh.ToString() + CharUnderExamination.ToString());
            }
            return new Token(Constants.BANG, CharUnderExamination.ToString());
        }
    }

    private bool IsLetter(char ch)
        => char.IsLetter(ch) || ch == '_';

    private void SkipWhitespace()
    {
        while (CharUnderExamination is ('\t' or '\n' or ' '))
            ReadChar();
    }

    private string ReadIdentifier()
    {
        var postion = CurrentPositionInInput;
        while (IsLetter(CharUnderExamination))
            ReadChar();

        ReadPosition--;

        return Input[postion..CurrentPositionInInput];
    }

    private string ReadNumber()
    {
        var position = CurrentPositionInInput;
        while (char.IsDigit(CharUnderExamination))
            ReadChar();

        ReadPosition--;

        return Input[position..CurrentPositionInInput];
    }

    private char PeekChar()
        => ReadPosition >= Input.Length
        ? NullASCII()
        : Input[ReadPosition];

    private char NullASCII()
    {
        // Unicode and ASCII for NULL char. For more information see: https://www.ascii-code.com/ and https://unicode-table.com/en/
        return Convert.ToChar(NULL_CHAR);
    }
}
