using Calculator.Services.NCalcCalculator;

namespace Calculator.Services.ExpressionHandler;

public class ExpressionHandler(
    INCalcCalculator nCalcCalculator
    ) : IExpressionHandler
{
    private readonly List<string> _mathExpression = [];
    
    private List<string> _currentNumberInput = [];
 
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

    
    private void RemoveDuplicateToken(List<string> inputList, string input, Func<string, bool> tokenChecker)
    {
        int lastIndex = inputList.Count - 1;
        string token = inputList.Count == 0 ? "" : inputList[lastIndex];

        bool tokenIsMatch = tokenChecker(token);
        bool inputIsMatch = tokenChecker(input);

        if (tokenIsMatch && inputIsMatch)
        {
            Remove(inputList);
        }
    }

    private bool ExpressionContainsComma(List<string> inputList)
    {
        bool stringContainsComma = false;
        
        for (int i = inputList.Count; i > 0; i--)
        {
            bool tokenIsComma = IsComma(inputList[i - 1]);
            bool tokenIsOperator = IsOperator(inputList[i - 1]);

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

    private void AddToNumber(string input)
    {
        _currentNumberInput.Add(input);
        string joinedNumber = string.Join("", _currentNumberInput);

        if (_mathExpression.Count >= 1 && !IsOperator(_mathExpression[^1]))
            _mathExpression[^1] = joinedNumber;
        else
            _mathExpression.Add(joinedNumber);
    }
    

    private void AddToExpression(string input)
    {
        RemoveDuplicateToken(_mathExpression, input, IsOperator);
        _mathExpression?.Add(input);
    }

    private void Remove(List<string> inputList)
    {
        int lastIndex = inputList.Count - 1;
        if (inputList.Count > 0)
            inputList?.RemoveAt(lastIndex);
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
        if (IsOperator(_mathExpression[^1]))
        {
            Remove(_mathExpression);
            _currentNumberInput = _mathExpression[^1].Select(ch => ch.ToString()).ToList();
            return SetExpressionStringForDisplay();
        }

        if (IsOperator(_mathExpression[^1]) || _currentNumberInput.Count <= 0)
        {
            Remove(_mathExpression);
            return SetExpressionStringForDisplay();
        }
        
        Remove(_currentNumberInput);
        
        if (!IsOperator(_mathExpression[^1]) && _currentNumberInput.Count == 0)
        {
            Remove(_mathExpression);
            return SetExpressionStringForDisplay();
        }
        
        // Update expression array
        string joinedNumber = string.Join("", _currentNumberInput);
        if (_mathExpression.Count >= 1 && !IsOperator(_mathExpression[^1]))
            _mathExpression[^1] = joinedNumber;
        
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
            AddToExpression(input);
        }
        else
        {
            AddToNumber(input);
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
        if (ExpressionContainsComma(_currentNumberInput))
            return SetExpressionStringForDisplay();
        
        RemoveDuplicateToken(_currentNumberInput, input, IsComma);
        AddToNumber(input);
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