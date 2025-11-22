namespace Calculator.Services.ExpressionHandler;

public class ExpressionHandler : IExpressionHandler
{
private readonly List<string> _mathExpression = [];

    private static bool IsOperator(string token) =>
        new[] { "+", "-", "*", "/"}.Contains(token);

    private static bool IsComma(string token) =>
        new[] { "." }.Contains(token);

    private void RemoveDuplicateToken(string input, Func<string, bool> tokenChecker)
    {
        int lastIndex = _mathExpression.Count - 1;
        string token = _mathExpression.Count == 0 ? "" : _mathExpression[lastIndex];

        bool tokenIsMatch = tokenChecker(token);
        bool inputIsMatch = tokenChecker(input);

        if (tokenIsMatch && inputIsMatch)
        {
            Remove();
        }
    }

    private bool ExpressionContainsComma()
    {
        bool stringContainsComma = false;
        
        for (int i = _mathExpression.Count; i > 0; i--)
        {
            bool tokenIsComma = IsComma(_mathExpression[i - 1]);
            bool tokenIsOperator = IsOperator(_mathExpression[i - 1]);

            if (tokenIsComma)
            {
                stringContainsComma = true;
                break;
            }
            else if (tokenIsOperator)
            {
                stringContainsComma = false;
                break;
            }
        }

        return stringContainsComma;
    }

    private void Add(string input)
    {
        RemoveDuplicateToken(input, IsOperator);
        _mathExpression?.Add(input);
    }

    private void Remove()
    {
        int lastIndex = _mathExpression.Count - 1;
        if (_mathExpression.Count > 0)
            _mathExpression?.RemoveAt(lastIndex);
    }

    public string Clear()
    {
        if (_mathExpression.Count > 0)
            _mathExpression.Clear();

        return string.Empty;
    }

    public string GetExpressionString()
    {
        string mathExpressionCopy = string.Join("", _mathExpression);
        string[] tokens = { "+", "-", "*", "/" };

        foreach (string token in tokens)
            mathExpressionCopy = mathExpressionCopy.Replace(token, $" {token} ");
        
        return mathExpressionCopy;
    } 

    public string HandleBackspace()
    {
        Remove();
        return GetExpressionString();
    }
    
    public string HandleCalculatorInput(string input)
    {
        Add(input);
        return GetExpressionString();
    }
    
    public string HandleCommaButton(string input)
    {
        if (ExpressionContainsComma())
            return GetExpressionString();
        
        RemoveDuplicateToken(input, IsComma);
        _mathExpression?.Add(input);
        return GetExpressionString();
    }
}