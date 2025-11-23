using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public class DisplayBase : ComponentBase, IDisposable
{
    [Inject] public IExpressionHandler Handler { get; set; } = null!;
    protected string? PrimaryDisplay => Handler.HasCalculated ? Handler.Result : Handler.Expression;
    protected string? SecondaryDisplay => Handler.HasCalculated ? Handler.Expression : string.Empty;

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