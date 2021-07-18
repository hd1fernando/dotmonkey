using DotMonkey.LexicalAnalizer;
using System;

namespace DotMonkey
{
    public class Repl
    {
        public const string PROMPT = ">>";

        public void Start()
        {
            Console.WriteLine("Press ctrl+c to end cli mode.");

            do
            {
                Console.Write(PROMPT);
                var command = Console.ReadLine();


                var lexer = new Lexer(command);

                Console.WriteLine(command);
                Token token;
                do
                {
                    token = lexer.NextToken();
                    Console.WriteLine(token.ToString());

                } while (token.Type != Constants.EOF);


            } while (true);
        }
    }
}
