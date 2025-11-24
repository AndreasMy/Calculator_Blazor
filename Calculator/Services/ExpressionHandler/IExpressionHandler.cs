namespace Calculator.Services.ExpressionHandler;

public interface IExpressionHandler
{
    string HandleCalculatorInput(string input);
    string HandleBackspace();
    void HandleEvalButton();
    string HandleCommaButton(string input);
    void Clear();
    string OperateOnPreviousResult(string input);
    string? HandleAcButton();
    string HandleSignToggle();
    
    event Action? OnChange;
    
    
    bool HasCalculated { get; set; }
    bool OperatorClicked { get; set; }
    string? Expression { get; set; }
    string? Result { get; set; }
}