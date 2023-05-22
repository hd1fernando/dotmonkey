using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using DotMonkey.Parser.Object;
using System;
using System.Collections.Generic;

namespace DotMonkey.Parser.Eval;

public class Evaluator
{
    private static _Boolean TRUE = new _Boolean(true);
    private static _Boolean FALSE = new _Boolean(false);
    private static NULL NULL = new NULL();

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

        if (node is PrefixExpression)
        {
            var prefix = (node as PrefixExpression);
            var right = Eval(prefix.Rigth);
            return EvalPrefixExpression(prefix.Operator, right);
        }

        return null;
    }

    private IObject EvalPrefixExpression(string @operator, IObject right)
    {
        return @operator switch
        {
            "!" => EvalBangOperatorExpression(right),
            "-" => EvalMinuxPrefixOperatorExpression(right),
            _ => NULL
        };
    }

    private IObject EvalMinuxPrefixOperatorExpression(IObject right)
    {
        if (right.Type() != ObjectType.INTERGER_OBJ)
            return NULL;

        var value = ((Integer)right).Value;

        return new Integer(-value);
    }

    private IObject EvalBangOperatorExpression(IObject right)
    {
        if (right is not _Boolean)
            return FALSE;

        var r = (_Boolean)right;

        if (r.Equals(TRUE))
            return FALSE;
        if (r.Equals(FALSE))
            return TRUE;
        if (r.Equals(NULL))
            return TRUE;
        return FALSE;
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
