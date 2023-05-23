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

        if (node is InfixExpression)
        {
            var infix = (node as InfixExpression);
            var left = Eval(infix.Left);
            var right = Eval(infix.Rigth);

            return EvalInfixExpression(infix.Operator, left, right);
        }

        if (node is BlockStatement)
            return EvalStatements((node as BlockStatement).Statements);

        if (node is IfExpression)
            return EvalIfExpression((node as IfExpression));

        return null;
    }

    private IObject EvalIfExpression(IfExpression ifExpression)
    {
        var condition = Eval(ifExpression.Condition);

        if (IsTruthy(condition))
            return Eval(ifExpression.Consequence);

        if (ifExpression.Alternative is not null)
            return Eval(ifExpression.Alternative);

        return NULL;
    }

    private bool IsTruthy(IObject condition)
    {
        if (condition is NULL)
            return false;

        if (condition is Integer)
            return true;

        var c = (_Boolean)condition;
        if (c.Equals(TRUE))
            return true;
        if (c.Equals(FALSE))
            return false;

        return true;
    }

    private IObject EvalInfixExpression(string @operator, IObject left, IObject right)
    {
        if (left.Type() == ObjectType.INTERGER_OBJ && right.Type() == ObjectType.INTERGER_OBJ)
            return EvalIntegerInfixExpression(@operator, left, right);

        var leftVal = ((_Boolean)left).Value;
        var rightVal = ((_Boolean)right).Value;
        return @operator switch
        {
            "==" => NativeBoolToBooleanObject(leftVal == rightVal),
            "!=" => NativeBoolToBooleanObject(leftVal != rightVal),
            _ => NULL
        };

    }

    private IObject EvalIntegerInfixExpression(string @operator, IObject left, IObject right)
    {
        var leftVal = ((Integer)left).Value;
        var rightVal = ((Integer)right).Value;

        return @operator switch
        {
            "+" => new Integer(leftVal + rightVal),
            "-" => new Integer(leftVal - rightVal),
            "*" => new Integer(leftVal * rightVal),
            "/" => new Integer(leftVal / rightVal),
            "<" => NativeBoolToBooleanObject(leftVal < rightVal),
            ">" => NativeBoolToBooleanObject(leftVal > rightVal),
            "==" => NativeBoolToBooleanObject(leftVal == rightVal),
            "!=" => NativeBoolToBooleanObject(leftVal != rightVal),
            _ => NULL
        };
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
