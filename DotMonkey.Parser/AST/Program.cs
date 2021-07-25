using System.Collections.Generic;
using System.Linq;

namespace DotMonkey.Parser.AST
{
    /// <summary>
    /// The root node of every AST our parser produces.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Every valid dotMonkey program is a serie of statements;
        /// This is just a slice of AST nodes that implements IStatement;
        /// </summary>
        IList<IStatement> Statements { get; }

        public string TokenLiteral()
        {
            if (Statements.Count() > 0)
                return Statements[0].TokenLiteral();
            return string.Empty;
        }
    }
}
