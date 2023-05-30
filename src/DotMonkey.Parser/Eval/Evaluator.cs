using DotMonkey.Parser.AST;
using DotMonkey.Parser.AST.Expressions;
using DotMonkey.Parser.AST.Interfaces;
using DotMonkey.Parser.AST.Statements;
using DotMonkey.Parser.Object;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotMonkey.Parser.Eval;

public class Evaluator
{
    private static _Boolean TRUE = new _Boolean(true);
    private static _Boolean FALSE = new _Boolean(false);
    private static NULL NULL = new NULL();

    private Dictionary<string, BuiltIn> _builtins = new Dictionary<string, BuiltIn>();

    public Evaluator()
    {
        _builtins.Add(
            "len", new BuiltIn((_) =>
            {
                if (_.Count != 1)
                    return NewError("wrong number of arguments. got={0}, want=1", _.Count.ToString());

                var arg = _[0];
                if (arg is _String)
                    return new Integer((long)((_String)arg).Value.Length);
                if (arg is _Array)
                    return new Integer((long)((_Array)arg).Elements.Count);

                return NewError("argument to `len` not supported, got {0}", arg.Type());
            }));
        _builtins.Add("first", new((_) =>
        {
            if (_.Count != 1)
                return NewError("wrong number of arguments. got={0}, want=1", _.Count.ToString());

            var arg = _[0];

            if (arg is _Array)
            {
                if (((_Array)arg).Elements.Count < 1 || ((_Array)arg).Elements[0] == null)
                    return NewError("array can't be empty");
                return ((_Array)arg).Elements[0];
            }

            return NewError("argument to `first` not supported, got {0}", arg.Type());

        }));
        _builtins.Add("last", new((_) =>
        {
            if (_.Count != 1)
                return NewError("wrong number of arguments. got={0}, want=1", _.Count.ToString());

            var arg = _[0];

            if (arg is _Array)
            {
                if (((_Array)arg).Elements.Count < 1 || ((_Array)arg).Elements[0] == null)
                    return NewError("array can't be empty");
                return ((_Array)arg).Elements.Last();
            }

            return NewError("argument to `last` not supported, got {0}", arg.Type());

        }));
        _builtins.Add("rest", new((_) =>
        {
            if (_.Count != 1)
                return NewError("wrong number of arguments. got={0}, want=1", _.Count.ToString());

            var arg = _[0];

            if (arg.Type() != ObjectType.ARRAY_OBJ)
            {
                return NewError("argument to `rest` must be ARRAY, got {0}", arg.Type());
            }

            var element = new List<IObject>(((_Array)arg).Elements.Skip(1));

            return new _Array(element);
        }));
        _builtins.Add("push", new((_) =>
        {
            if (_.Count != 2)
                return NewError("wrong number of arguments. got={0}, want=2", _.Count.ToString());

            var arg = _[0];

            if (arg.Type() != ObjectType.ARRAY_OBJ)
            {
                return NewError("argument to `push` must be ARRAY, got {0}", arg.Type());
            }

            var elements = new List<IObject>(((_Array)arg).Elements);
            elements.Add(_[1]);

            return new _Array(elements);
        }));

    }

    public IObject Eval(INode node, Object.Environment env)
    {
        if (node is Program)
            return EvalProgram((node as Program).Statements, env);

        if (node is ExpressionStatement)
            return Eval((node as ExpressionStatement).Expression, env);

        if (node is IntegerLiteral)
            return new Integer((node as IntegerLiteral).Value);

        if (node is BooleanExpression)
            return NativeBoolToBooleanObject((node as BooleanExpression).Value);

        if (node is PrefixExpression)
        {
            var prefix = (node as PrefixExpression);
            var right = Eval(prefix.Rigth, env);

            if (IsError(right))
                return right;

            return EvalPrefixExpression(prefix.Operator, right);
        }

        if (node is InfixExpression)
        {
            var infix = (node as InfixExpression);

            var left = Eval(infix.Left, env);
            if (IsError(left))
                return left;

            var right = Eval(infix.Rigth, env);
            if (IsError(right))
                return right;

            return EvalInfixExpression(infix.Operator, left, right);
        }

        if (node is BlockStatement)
            return EvalBlockStatement((node as BlockStatement).Statements, env);

        if (node is IfExpression)
            return EvalIfExpression((node as IfExpression), env);

        if (node is ReturnStatement)
        {
            var val = Eval((node as ReturnStatement).ReturnValue, env);

            if (IsError(val))
                return val;

            return new ReturnValue(val);
        }

        if (node is LetStatement)
        {
            var let = (node as LetStatement);
            var val = Eval(let.Value, env);

            if (IsError(val))
                return val;

            env.Set(let.Name.Value, val);
        }

        if (node is Identifier)
            return EvalIdentifier((node as Identifier), env);

        if (node is FunctionLiteral)
        {
            var func = (node as FunctionLiteral);
            var @params = func.Parameters;
            var body = func.Body;

            return new Function(@params, body, env);
        }

        if (node is CallExpression)
        {
            var function = Eval((node as CallExpression).Function, env);

            if (IsError(function))
                return function;

            var args = EvalExpressions((node as CallExpression).Arguments, env);

            if (args.Count == 1 && IsError(args[0]))
                return args[0];

            return ApplyFunction(function, args);
        }

        if (node is StringLiteral)
            return new _String((node as StringLiteral).Value);

        if (node is ArrayLiteral)
        {
            var elements = EvalExpressions((node as ArrayLiteral).Elements, env);
            if (elements.Count == 1 && IsError(elements[0]))
                return elements[0];
            return new _Array(elements);
        }

        if (node is IndexExpression)
        {
            var indexNode = (node as IndexExpression);
            var left = Eval(indexNode.Left, env);

            if (IsError(left))
                return left;

            var index = Eval(indexNode.Index, env);
            if (IsError(index))
                return index;

            return EvalIndexExpression(left, index);
        }

        return null;
    }

    private IObject EvalIndexExpression(IObject left, IObject index)
    {
        if (left.Type() == ObjectType.ARRAY_OBJ && index.Type() == ObjectType.INTERGER_OBJ)
            return EvalArrayIndexExpression(left, index);
        return NewError("index operator not supported: {0}", left.Type());
    }

    private IObject EvalArrayIndexExpression(IObject array, IObject index)
    {
        var arrayObject = (_Array)array;
        var idx = (int)((Integer)index).Value;
        var max = arrayObject.Elements.Count - 1;

        if (idx < 0 || idx > max)
            return NULL;
        return arrayObject.Elements[idx];
    }

    private IObject ApplyFunction(IObject fn, List<IObject> args)
    {
        if (fn is Function)
        {
            var func = (Function)fn;
            var extendendEnv = ExtendedFunctionEnv(func, args);
            var evaluated = Eval(func.Body, extendendEnv);
            return UnwrapReturnValue(evaluated);
        }
        if (fn is BuiltIn)
        {
            return ((BuiltIn)fn).Fn(args);
        }

        return NewError("not a function {0}", fn.Type());
    }

    private IObject UnwrapReturnValue(IObject obj)
    {
        if (obj is ReturnValue)
            return ((ReturnValue)obj).Value;
        return obj;
    }

    private Object.Environment ExtendedFunctionEnv(Function fn, List<IObject> args)
    {
        var env = Object.Environment.NewEnclosedEnvironment(fn.Environment);

        for (int i = 0; i < fn.Parameters.Count; i++)
            env.Set(fn.Parameters[i].Value, args[i]);

        return env;
    }

    private List<IObject> EvalExpressions(List<IExpression> expressions, Object.Environment env)
    {
        var result = new List<IObject>();

        foreach (var expression in expressions)
        {
            var evaluated = Eval(expression, env);
            if (IsError(evaluated))
                return new List<IObject> { evaluated };

            result.Add(evaluated);
        }
        return result;
    }

    private IObject EvalIdentifier(Identifier node, Object.Environment env)
    {
        var (val, ok) = env.Get(node.Value);

        if (ok)
            return val;

        if (_builtins.TryGetValue(node.Value, out var builtin))
            return builtin;

        return NewError("identifier not found: " + node.Value);

    }

    private IObject EvalBlockStatement(List<IStatement> statements, Object.Environment env)
    {
        IObject result = null;

        foreach (var statement in statements)
        {
            result = Eval(statement, env);
            if (result is not null && result.Type() == ObjectType.RETURN_VALUE_OBJ
                || result is not null && result.Type() == ObjectType.ERROR_OBJ)
                return result;
        }

        return result;
    }

    private IObject EvalIfExpression(IfExpression ifExpression, Object.Environment env)
    {
        var condition = Eval(ifExpression.Condition, env);

        if (IsError(condition))
            return condition;

        if (IsTruthy(condition))
            return Eval(ifExpression.Consequence, env);

        if (ifExpression.Alternative is not null)
            return Eval(ifExpression.Alternative, env);

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

        if (left.Type() == ObjectType.STRING_OBJ && right.Type() == ObjectType.STRING_OBJ)
            return EvalStringInfixExpression(@operator, left, right);

        var leftVal = ((_Boolean)left).Value;
        var rightVal = ((_Boolean)right).Value;
        return @operator switch
        {
            "==" => NativeBoolToBooleanObject(leftVal == rightVal),
            "!=" => NativeBoolToBooleanObject(leftVal != rightVal),
            _ => NewError("unknown operator: {0} {1} {2}", left.Type(), @operator, right.Type()),
        };

    }

    private IObject EvalStringInfixExpression(string @operator, IObject left, IObject right)
    {
        if (@operator != "+")
            return NewError("unknown operator: {0} {1} {2}", left.Type(), @operator, right.Type());

        var leftVal = ((_String)left).Value;
        var rightVal = ((_String)right).Value;

        return new _String(leftVal + rightVal);
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

    private IObject EvalProgram(IList<IStatement> statements, Object.Environment env)
    {
        IObject result = null;

        foreach (var statement in statements)
        {
            result = Eval(statement, env);

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