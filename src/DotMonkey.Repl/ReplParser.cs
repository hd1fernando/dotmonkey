using DotMonkey.LexicalAnalizer;
using System;
using System.Collections.Generic;

namespace DotMonkey
{
    public class ReplParser : IRepl
    {
        public const string PROMPT = ">>> ";

        public void Start()
        {
            Console.WriteLine("Press ctrl+c to end cli mode.");

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
            foreach (var error in errors)
                Console.WriteLine("\t" + error);
        }
    }
}
