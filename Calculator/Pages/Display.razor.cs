using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public partial class Display(
    IExpressionHandler expressionHandler
    ) : ComponentBase
{
    private string PrimaryDisplay { get; set; }
    private string SecondaryDisplay { get; set; }

    private void GetExpression()
    {
        string expression = expressionHandler.GetExpressionString();
        Console.WriteLine($"");
    }
}