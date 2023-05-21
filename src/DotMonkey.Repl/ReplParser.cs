using DotMonkey.LexicalAnalizer;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotMonkey
{
    public class ReplParser : IRepl
    {
        public const string PROMPT = ">>> ";

        public void Start()
        {
            Console.WriteLine("Press ctrl+c to end cli mode.");
            Console.WriteLine();

            do
            {
                Console.Write(PROMPT);
                var command = Console.ReadLine();


                var lexer = new Lexer(command);
                var parser = new Parser.AST.Parser(lexer);

                var program = parser.ParserProgram();

                if (parser.Errors.Count > 0)
                {
                    PrintParserErrors(parser.Errors);
                    continue;
                }

                Console.WriteLine(program.String());
                Console.WriteLine();

            } while (true);
        }

        private void PrintParserErrors(List<string> errors)
        {
            Console.WriteLine("parser erros:".Pastel(Color.Red));
            foreach (var error in errors)
                Console.WriteLine("\t" + error.Pastel(Color.Red));
        }
    }
}
