using DotMonkey.LexicalAnalizer;
using System;

namespace DotMonkey
{
    public class Repl
    {
        public const string PROMPT = ">>";

        public void Start()
        {
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;

            Console.WriteLine("Press ctrl esc to end cli mode.");

            do
            {

                Console.Write(PROMPT);
                var command = Console.ReadLine();

                cki = Console.ReadKey();

                var lexer = new Lexer(command);

                Token token;
                do
                {
                    token = lexer.NextToken();
                    Console.WriteLine(token.ToString());

                } while (token.Type != Constants.EOF);


            } while (cki.Key != ConsoleKey.Escape);
        }
    }
}
