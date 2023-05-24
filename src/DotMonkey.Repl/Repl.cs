using DotMonkey.LexicalAnalizer;
using DotMonkey.Parser.Eval;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using Environment = DotMonkey.Parser.Object.Environment;

namespace DotMonkey;

public class Repl : IRepl
{
    public const string PROMPT = ">>> ";

    public void Start()
    {
        Console.WriteLine("Press ctrl+c to end cli mode.");
        Console.WriteLine();

        var env = new Environment();

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

            var evaluated = new Evaluator().Eval(program, env);

            if (evaluated is not null)
            {
                Console.WriteLine(evaluated.Inspect());
            }


        } while (true);
    }

    private void PrintParserErrors(List<string> errors)
    {
        Console.WriteLine("parser erros:".Pastel(Color.Red));
        foreach (var error in errors)
            Console.WriteLine("\t" + error.Pastel(Color.Red));
    }
}
