using Calculator.Services.NCalcCalculator;

namespace Calculator.Services.ExpressionHandler;

public class ExpressionHandler(
    INCalcCalculator nCalcCalculator
    ) : IExpressionHandler
{
    private readonly List<string> _mathExpression = [];
    
    private readonly List<string> _currentNumberInput = [];
 
    public event Action? OnChange;
    
    private string? _expression;
    private string? _result; 
    private bool _hasCalculated;
    private bool _operatorClicked;

    public string? Expression
    {
        get => _expression;
        set
        {
            _expression = value;
            OnChange?.Invoke();   
        }
    }

    public string? Result
    {
        get => _result;
        set
        {
            _result = value;
            OnChange?.Invoke();
        }
    }

    public bool HasCalculated
    {
        get => _hasCalculated;
        set
        {
            _hasCalculated = value;
            OnChange?.Invoke();
        }
    }

    public bool OperatorClicked
    {
        get => _operatorClicked;
        set
        {
            _operatorClicked = value;
            OnChange?.Invoke();
        }
    }

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
            if (tokenIsOperator)
            {
                stringContainsComma = false;
                break;
            }
        }

        return stringContainsComma;
    }

    private void AddToNumberList(string input)
    {
        _currentNumberInput.Add(input);
        string joinedNumber = string.Join("", _currentNumberInput);

        if (_mathExpression.Count >= 1 && !IsOperator(_mathExpression[^1]))
            _mathExpression[^1] = joinedNumber;
        else
            _mathExpression.Add(joinedNumber);
    }
    

    private void AddToExpressionList(string input)
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

    public void Clear()
    {
        if (_mathExpression.Count > 0)
        {
            _mathExpression.Clear();
            _currentNumberInput.Clear();
        }

        Expression = string.Empty;
    }
    
    public string HandleBackspace()
    {
        Remove();
        return SetExpressionStringForDisplay();
    }

    public void HandleEvalButton()
    {
        if (_expression == null) return;
        if (_expression.Length <= 2) return;
        string? result = nCalcCalculator.Evaluate(_expression).ToString();
        Result = result;
    }

    public string HandleCalculatorInput(string input)
    {
        if (_operatorClicked)
        {
            _currentNumberInput.Clear();
            AddToExpressionList(input);
        }
        else
        {
            AddToNumberList(input);
        }
        return SetExpressionStringForDisplay();
    }

    public string OperateOnPreviousResult(string input)
    {
        string? copiedResult = _result;
        Clear();
        
        if (copiedResult != null) _mathExpression.Add(copiedResult);
        _mathExpression.Add(input);
        
        return SetExpressionStringForDisplay();
    }
    
    public string HandleCommaButton(string input)
    {
        if (ExpressionContainsComma())
            return SetExpressionStringForDisplay();
        
        RemoveDuplicateToken(input, IsComma);
        _mathExpression?.Add(input);
        return SetExpressionStringForDisplay();
    }
    
    private string SetExpressionStringForDisplay()
    {
        string mathExpressionCopy = string.Join("", _mathExpression);
        string[] tokens = { "+", "-", "*", "/" };

        foreach (string token in tokens)
            mathExpressionCopy = mathExpressionCopy.Replace(token, $" {token} ");

        Expression = mathExpressionCopy; 
        return mathExpressionCopy;
    } 
}