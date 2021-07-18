using System;

namespace DotMonkey.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, this is .Mokey programming language.");

            new Repl().Start();
        }
    }
}
