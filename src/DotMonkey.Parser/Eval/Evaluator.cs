using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using DotMonkey.Parser.Object;
using System.Collections.Generic;

namespace DotMonkey.Parser.Eval;

public class Evaluator
{
    private static _Boolean TRUE = new _Boolean(true);
    private static _Boolean FALSE = new _Boolean(false);

    public IObject Eval(INode node)
    {
        if (node is Program)
            return EvalStatements((node as Program).Statements);

        if (node is ExpressionStatement)
            return Eval((node as ExpressionStatement).Expression);

        if (node is IntegerLiteral)
            return new Integer((node as IntegerLiteral).Value);

        if (node is BooleanExpression)
            return NativeBoolToBooleanObject((node as BooleanExpression).Value);

        return null;
    }

    private IObject NativeBoolToBooleanObject(bool value)
    {
        if (value)
            return TRUE;
        return FALSE;
    }

    private IObject EvalStatements(IList<IStatement> statements)
    {
        IObject result = null;

        foreach (var statement in statements)
        {
            result = Eval(statement);
        }

        return result;
    }
}
