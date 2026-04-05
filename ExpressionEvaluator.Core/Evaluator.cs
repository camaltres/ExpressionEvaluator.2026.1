
namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        var expression = InvestExpression(postfix);
        return EvaluatePostfix(expression);
    }

    private static string InfixToPostfix(string infix)
    {
        var contains = string.Empty;

        var postFix = string.Empty;
        var stack = new Stack<char>();
        foreach (var item in infix)
        {
            if (IsOperator(item))
            {

                if (contains != string.Empty)
                { 
                  contains = $"[{contains}]";
                  postFix += contains;
                  contains = string.Empty;
                }
                
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        do
                        {
                            postFix += stack.Pop();
                        } while (stack.Peek() != '(');
                        stack.Pop();
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            postFix += stack.Pop();
                            stack.Push(item);
                        }
                    }
                }

            }
            else
            {
                contains += item;
            }
        }
        if (contains != string.Empty)
        {
            contains = $"[{contains}]";
            postFix += contains;
        }
        while (stack.Count > 0)
        {
            postFix += stack.Pop();
        }
        return postFix;
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static string InvestExpression(string postfix)
    {
        var contains = string.Empty;
        var expression = string.Empty; 
        var stack = new Stack<char>();
        foreach (var item in postfix)
        {
            if (IsBrackets(item))
            {
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ']')
                    {
                        do
                        {
                            contains += stack.Pop();
                        } while (stack.Peek() != '[');
                        stack.Pop();
                        contains = $"[{contains}]";
                        expression += contains;
                        contains = string.Empty;
                    }
                    else
                    {
                        stack.Push(item);
                    }

                }

            }
            else
            { 
                expression += item; 
            }
    
        }
        return expression;
    }

    private static double EvaluatePostfix(string expression)
    {
        var contains = string.Empty;
        var stack = new Stack<double>();
        var assistant = new Stack<char>();
        foreach (var item in expression)
        {
            if (IsOperator(item))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(item switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                if (assistant.Count == 0)
                {
                    assistant.Push(item);
                }
                else
                {
                    if (item == ']')
                    {
                        do
                        {
                            contains += assistant.Pop();
                        } while (assistant.Peek() != '[');
                        assistant.Pop();
                        var number = double.Parse(contains);
                        stack.Push(number);
                        contains = string.Empty;
                    }
                    else
                    {
                        assistant.Push(item);
                    }
                }
            }

        }
        return stack.Pop();
    }
    private static bool IsOperator(char item) => "+-*/^()".Contains(item);

    private static bool IsBrackets(char item) => "[]1234567890,.".Contains(item);

}