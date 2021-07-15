﻿using DotMonkey.LexicalAnalizer.Tokens;
using System;

namespace DotMonkey.LexicalAnalizer.Lexer
{
    public class Lexer
    {
        public string Input { get; set; }
        public int Position { get; set; } // current position in input
        public int ReadPosition { get; set; } // current reading position
        public char Ch { get; set; } // current char under examination

        public Lexer(string input)
        {
            Input = input;
            ReadChar();
        }

        private void ReadChar()
        {
            Ch = ReadPosition >= Input.Length
                ? Convert.ToChar(0) // Unicode and ASCII for NULL char. For more information see: https://www.ascii-code.com/ and https://unicode-table.com/en/
                : Input[ReadPosition];

            Position = ReadPosition;
            ReadPosition++;
        }

        private Token NextToken()
        {
            var token = new Token();
            SkipWhitespace();
            if (int.Parse(Ch.ToString()) == 0)
                token = new Token(Constants.EOF, string.Empty);

            token = Ch switch
            {
                '=' => new Token(Constants.ASSING, Ch.ToString()),
                ';' => new Token(Constants.SEMICOLON, Ch.ToString()),
                '(' => new Token(Constants.LPARENT, Ch.ToString()),
                ')' => new Token(Constants.RPARENT, Ch.ToString()),
                ',' => new Token(Constants.COMMA, Ch.ToString()),
                '+' => new Token(Constants.PLUS, Ch.ToString()),
                '{' => new Token(Constants.LBRACE, Ch.ToString()),
                '}' => new Token(Constants.RBRACE, Ch.ToString()),
                _ => DefautlPattern(token)
            };

            ReadChar();
            return token;

            Token DefautlPattern(Token token)
            {
                if (char.IsLetter(Ch))
                    return new Token(token.LookupIdent(), ReadIdentifier());

                if (char.IsDigit(Ch))
                    return new Token(Constants.INT, ReadNumber());

                return new Token(Constants.ILLEGAL, Ch.ToString());
            }
        }

        private string ReadNumber()
        {
            var position = Position;
            while (char.IsDigit(Ch))
                ReadChar();

            return Input[position..Position];
        }

        private void SkipWhitespace()
        {
            while (Ch is ('\t' or '\n' or ' '))
                ReadChar();
        }

        private string ReadIdentifier()
        {
            var postion = Position;
            while (char.IsLetter(Ch))
                ReadChar();

            return Input[postion..Position];
        }
    }
}
