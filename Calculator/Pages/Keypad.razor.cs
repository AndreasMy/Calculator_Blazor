using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public partial class Keypad(
    IExpressionHandler expressionHandler
    ) : ComponentBase
{
    
    private void HandleButtonClick(char btnText)
    {
        string newExpression = expressionHandler.HandleCalculatorInput(btnText.ToString());
        Console.WriteLine(newExpression);
    }
}