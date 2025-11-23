# A.I. usage


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