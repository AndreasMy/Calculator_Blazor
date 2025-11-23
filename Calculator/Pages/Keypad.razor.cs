using Calculator.Services.ExpressionHandler;
using Microsoft.AspNetCore.Components;

namespace Calculator.Pages;

public partial class Keypad(

    ) : ComponentBase
{
    [Inject] public IExpressionHandler Handler { get; set; } = null!;
    private bool AcToggle { get; set; } = false;
    private bool AcClearAll { get; set; } = false;
    
    
    private void HandleNumpadClick(char btnText)
    {
        Handler.OperatorClicked = false;
        AcToggle = true;
        AcClearAll = false;
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
        
        AcToggle = true;
        Handler.OperatorClicked = true;
        if (Handler.HasCalculated && Handler.OperatorClicked)
        {
            Handler.OperateOnPreviousResult(btnText.ToString());
            Handler.HasCalculated = false;
        }
        
        Handler.HandleCalculatorInput(btnText.ToString());
    }

    
    private void HandleCommaButton(char btnText)
    {
        Handler.HandleCommaButton(btnText.ToString());
    }

    
    private void HandleBackspaceButton()
    {
        Handler.HandleBackspace();
    }


    private void HandleResetButton()
    {
        if (AcClearAll)
            Handler.Clear();
        
        Handler.HandleAcButton();   
        AcToggle = false;
        AcClearAll = true;
    }

    
    private void HandleEvaluateButton()
    {
        Handler.HandleEvalButton();
        Handler.HasCalculated = true;
    }
}