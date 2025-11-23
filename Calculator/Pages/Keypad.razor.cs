using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public partial class Keypad(

    ) : ComponentBase
{
    [Inject] public IExpressionHandler Handler { get; set; } = null!;
    
    private void HandleButtonClick(char btnText)
    {
        if (Handler.HasCalculated)
        {
            Handler.Clear();
            Handler.HasCalculated = false;
        }
        
        Handler.HandleCalculatorInput(btnText.ToString());
    }

    private void HandleEvaluateButton()
    {
        Handler.HandleEvalButton();
        Handler.HasCalculated = true;
    }
}