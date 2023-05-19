using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotMonkey.Parser.AST.Interfaces;

namespace DotMonkey.Parser.AST;

/// <summary>
/// The root node of every AST our parser produces.
/// </summary>
public class Program : INode
{
    /// <summary>
    /// Every valid dotMonkey program is a serie of statements;
    /// This is just a slice of AST nodes that implements IStatement;
    /// </summary>
    public IList<IStatement> Statements { get; private set; } = new List<IStatement>();

    public void AddSteatment(IStatement statement)
        => Statements.Add(statement);

    public string String()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var statement in Statements)
            sb.Append(statement.String());

        return sb.ToString();
    }

    public string TokenLiteral()
    {
        if (Statements.Count() > 0)
            return Statements[0].TokenLiteral();
        return string.Empty;
    }
}