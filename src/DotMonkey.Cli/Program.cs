using System;

namespace DotMonkey.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var langVersion = "0.1.0";
            var langType = "preview";

            var interpreterVersion = "0.1.0";

            Console.WriteLine($"Mokey Language {langVersion} ({langType})");
            Console.WriteLine($"[DotMonkey {interpreterVersion}]");

            new ReplParser().Start();
        }
    }
}
