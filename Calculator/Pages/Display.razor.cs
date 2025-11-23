using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

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