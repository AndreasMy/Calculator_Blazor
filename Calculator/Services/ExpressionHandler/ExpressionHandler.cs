using System.Text.RegularExpressions;
using Calculator.Services.NCalcCalculator;

namespace Calculator.Services.ExpressionHandler;

public partial class ExpressionHandler(
    INCalcCalculator nCalcCalculator
    ) : IExpressionHandler
{
    private readonly List<string> _mathExpression = [];
    private List<string> _currentNumberInput = [];


    public event Action? OnChange;
    private string? _expression = "0";
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

    
    private static void RemoveDuplicateToken(List<string> inputList, string input, Func<string, bool> tokenChecker)
    {
        int lastIndex = inputList.Count - 1;
        string token = inputList.Count == 0 ? "" : inputList[lastIndex];

        bool tokenIsMatch = tokenChecker(token);
        bool inputIsMatch = tokenChecker(input);

        if (tokenIsMatch && inputIsMatch)
        {
            RemoveLastItem(inputList);
        }
    }

    
    private static bool ExpressionContainsComma(List<string> inputList)
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

            if (!tokenIsOperator) continue;
            
            stringContainsComma = false;
            break;
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

    
    private static void RemoveLastItem(List<string> inputList)
    {
        int lastIndex = inputList.Count - 1;
        if (inputList.Count > 0)
            inputList?.RemoveAt(lastIndex);
    }

    
    public void Clear()
    {
        if (_mathExpression.Count > 0)
            _mathExpression.Clear();
        
        if (_currentNumberInput.Count > 0)
            _currentNumberInput.Clear();

        UpdateDisplay();
        Expression = "0";
    }
    
    
    public void HandleAcButton()
    {
        if (_mathExpression.Count == 0)
            return;
        
        if (IsOperator(_mathExpression[^1]))
            UpdateDisplay();
        
        RemoveLastItem(_mathExpression);
        _currentNumberInput.Clear();
        UpdateDisplay();
    }

    
    public void HandleBackspace()
    {
        if (_mathExpression.Count < 1)
        {
            UpdateDisplay();
        }
        
        if (IsOperator(_mathExpression[^1]))
        {
            RemoveLastItem(_mathExpression);
            
            // Received help from gpt to use Select instead of Join
            _currentNumberInput = _mathExpression[^1].Select(ch => ch.ToString()).ToList();
            UpdateDisplay();
        }

        if (IsOperator(_mathExpression[^1]) || _currentNumberInput.Count <= 0)
        {
            RemoveLastItem(_mathExpression);
            UpdateDisplay();
        }
        
        RemoveLastItem(_currentNumberInput);
        
        if (!IsOperator(_mathExpression[^1]) && _currentNumberInput.Count == 0)
        {
            RemoveLastItem(_mathExpression);
            UpdateDisplay();
        }
        
        // Update expression array
        string joinedNumber = string.Join("", _currentNumberInput);
        
        if (_mathExpression.Count >= 1 && !IsOperator(_mathExpression[^1]))
            _mathExpression[^1] = joinedNumber;
        
        UpdateDisplay();
    }

    
    public void HandleEvalButton()
    {
        if (_expression == null) return;
        if (_expression.Length <= 2) return;
        string? result = nCalcCalculator.Evaluate(_expression).ToString();
        Result = result;
    }

    
    public void HandleCalculatorInput(string input)
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
        UpdateDisplay();
    }


    public void HandleSignToggle()
    {
        int operatorIndex = _mathExpression.Count - 2;
        
        // If last index IS +/- operator
        if (IsOperator(_mathExpression[^1]) 
            && !MultiplyOrDivide().IsMatch(_mathExpression[^1]))
        {
            string replaceWith = _mathExpression[^1] == "+" ? "-" : "+";
            RemoveLastItem(_mathExpression);
            _mathExpression.Add(replaceWith);
        }

        // If last index HAS +/- sign
        if (!IsOperator(_mathExpression[^1]) 
            && !(MultiplyOrDivide().IsMatch(_mathExpression[operatorIndex])))
        {
            _mathExpression[operatorIndex] = _mathExpression[operatorIndex] == "+" ? "-" : "+";
        }
        
        // if operator at either index is not +/-
        if (IsOperator(_mathExpression[^1]) && MultiplyOrDivide().IsMatch(_mathExpression[^1])
            || !IsOperator(_mathExpression[^1]) && (MultiplyOrDivide().IsMatch(_mathExpression[operatorIndex])))
            return;
        
        UpdateDisplay();
    }
    
    
    public void OperateOnPreviousResult(string input)
    {
        string? copiedResult = _result;
        Clear();
        
        if (copiedResult != null) _mathExpression.Add(copiedResult);
        _mathExpression.Add(input);
        
        UpdateDisplay();
    }
    
    
    public void HandleCommaButton(string input)
    {
        if (ExpressionContainsComma(_currentNumberInput))
            UpdateDisplay();
        
        RemoveDuplicateToken(_currentNumberInput, input, IsComma);
        AddToNumber(input);
        UpdateDisplay();
    }
    
    
    private void UpdateDisplay()
    {
        string mathExpressionCopy = string.Join("", _mathExpression);
        string[] tokens = { "+", "-", "*", "/" };

        foreach (string token in tokens)
            mathExpressionCopy = mathExpressionCopy.Replace(token, $" {token} ");
        
        if (_mathExpression.Count == 0)
        {
            mathExpressionCopy = "0"; 
        }
        
        Expression = mathExpressionCopy; 
    }

    
    [GeneratedRegex(@"[*/]")]
    private static partial Regex MultiplyOrDivide();
}