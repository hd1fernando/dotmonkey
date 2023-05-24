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
            return EvalProgram((node as Program).Statements);

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

            if (IsError(right))
                return right;

            return EvalPrefixExpression(prefix.Operator, right);
        }

        if (node is InfixExpression)
        {
            var infix = (node as InfixExpression);

            var left = Eval(infix.Left);
            if (IsError(left))
                return left;

            var right = Eval(infix.Rigth);
            if (IsError(right))
                return right;

            return EvalInfixExpression(infix.Operator, left, right);
        }

        if (node is BlockStatement)
            return EvalBlockStatement((node as BlockStatement).Statements);

        if (node is IfExpression)
            return EvalIfExpression((node as IfExpression));

        if (node is ReturnStatement)
        {
            var val = Eval((node as ReturnStatement).ReturnValue);

            if (IsError(val))
                return val;

            return new ReturnValue(val);
        }

        return null;
    }

    private IObject EvalBlockStatement(List<IStatement> statements)
    {
        IObject result = null;

        foreach (var statement in statements)
        {
            result = Eval(statement);
            var resultType = result.Type();
            if (result is not null && resultType == ObjectType.RETURN_VALUE_OBJ
                || resultType == ObjectType.ERROR_OBJ)
                return result;
        }

        return result;
    }

    private IObject EvalIfExpression(IfExpression ifExpression)
    {
        var condition = Eval(ifExpression.Condition);

        if (IsError(condition))
            return condition;

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
        if (left.Type() != right.Type())
            return NewError("type mismatch: {0} {1} {2}", left.Type(), @operator, right.Type());

        if (left.Type() == ObjectType.INTERGER_OBJ && right.Type() == ObjectType.INTERGER_OBJ)
            return EvalIntegerInfixExpression(@operator, left, right);

        var leftVal = ((_Boolean)left).Value;
        var rightVal = ((_Boolean)right).Value;
        return @operator switch
        {
            "==" => NativeBoolToBooleanObject(leftVal == rightVal),
            "!=" => NativeBoolToBooleanObject(leftVal != rightVal),
            _ => NewError("unknown operator: {0} {1} {2}", left.Type(), @operator, right.Type()),
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
            _ => NewError("unknown operator: {0} {1} {2}", left.Type(), @operator, right.Type()),
        };
    }

    private IObject EvalPrefixExpression(string @operator, IObject right)
    {
        return @operator switch
        {
            "!" => EvalBangOperatorExpression(right),
            "-" => EvalMinuxPrefixOperatorExpression(right),
            _ => NewError("unknown operator: {0} {1}", @operator, right.Type()),
        };
    }

    private IObject EvalMinuxPrefixOperatorExpression(IObject right)
    {
        if (right.Type() != ObjectType.INTERGER_OBJ)
            return NewError("unknown operator: -{0}", right.Type());

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

    private IObject EvalProgram(IList<IStatement> statements)
    {
        IObject result = null;

        foreach (var statement in statements)
        {
            result = Eval(statement);

            if (result is ReturnValue)
                return ((ReturnValue)result).Value;
            if (result is Error)
                return result;
        }

        return result;
    }

    private Error NewError(string format, params string[] a)
    {
        var message = string.Format(format, a);
        return new Error(message);
    }

    private bool IsError(IObject @object)
    {
        if (@object is not null)
            return @object.Type() == ObjectType.ERROR_OBJ;

        return false;
    }
}
