# Blazor Calculator

A simple calculator build with Blazor as part of a school curriculum. This calculator uses NCalc to perform calculations on string expressions. I decided to use the expression based approach because of the benefits that comes with parsing a string expression. This way, I could reduce state management at the component level and handle the string building logic to its own service.  

### Expected behavior

I tried to model the button behavior to loosely match the behavior of the iPhone calculator. This calculator does not toggle positive or negative numbers (**1 * ( - 2)**), only the operator signs themselves.

#### Backspace button

- Is disabled if result has been calculated.

#### AC button

- Is displayed as 'AC' or 'C' depending on whether a number has been entered. 
- If signed number, number is removed and the sign is kept. A second click will clear all.

#### Sign Toggle button

- Toggles +/- at last index or if the last number is signed with +/-. Does not toggle * or /.

#### Operator buttons

- Replaces previously entered operator.
- Takes the calculated result and adds the operator at the end if a calculation has been performed.

#### Number buttons

- Clears the display and enters the new number if a calculation has been performed.


### Known issues

- I did not get to implement a responsive layout.
- I have not handled display overflow.
- **ExpressionHandler** is cluttered. I would love to have moved the state logic to its own class. I think I could move the state variables to global scope?

# A.I. usage


## Old code

The **ExpressionHandler** was ported from an old project I wrote this spring, and adapted to meet the requirements for this assignment. I cannot document precisely the AI usage, but I'll include the code where I think I received the most help. The rough plan was mine, the solution was provided. 

Now that I think of it, this code could be removed and I could instead use boolean flags in the component to toggle button activation. This code might also be slightly outdated after I changed how the expression string is built for this project.

```csharp
// Old structure
["1", ".", "2", "+", "2", "2", "2"]
```

```csharp
//New structure
["1.2", "+", "222"]
```

```csharp
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
            RemoveLastItem(inputList);
        }
    }
```

```csharp
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
```

## New code

### Prompt

I have one parent page, two components and a service class that handles input and returns that input as a string. This is blazor. I want one component to send the data to the service, and I want the other component to retreive the data from the service. In Avalonia I could use RelayCommand and ObservableProperty. Does Blazor have some equivalent?

### Code used

**ExpressionHandler**
```csharp
    public event Action? OnChange;

    public string? Expression
    {
        get => _expression;
        set
        {
            _expression = value;
            OnChange?.Invoke();   // notify listeners
        }
    }
    
    private string? _expression;
```

**Display.razor.cs**
```csharp
public class DisplayBase : ComponentBase, IDisposable
{
    [Inject] public IExpressionHandler Handler { get; set; } = null!;
    protected string? Expression => Handler.Expression;

    protected override void OnInitialized()
    {
        Handler.OnChange += Refresh;
    }

    private void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Handler.OnChange -= Refresh;
    }
}
```

**Display.razor**
```csharp
    @inherits DisplayBase

<div class="display-frame">
    @Expression
</div>

```