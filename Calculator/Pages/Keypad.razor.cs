using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public partial class Keypad(

    ) : ComponentBase
{
    [Inject] public IExpressionHandler Handler { get; set; } = null!;
    
    private void HandleNumpadClick(char btnText)
    {
        Handler.OperatorClicked = false;
        if (Handler.HasCalculated)
        {
            Handler.Clear();
            Handler.HasCalculated = false;
        }
        Handler.HandleCalculatorInput(btnText.ToString());
    }
    
    private void HandleOperatorClick(char btnText)
    {
        if (Handler.Expression?.Length < 1)
            return;

        Handler.OperatorClicked = true;
        Handler.HandleCalculatorInput(btnText.ToString());
        // Handler.OperatorClicked = false;
    }

    private void HandleEvaluateButton()
    {
        Handler.HandleEvalButton();
        Handler.HasCalculated = true;
    }
}