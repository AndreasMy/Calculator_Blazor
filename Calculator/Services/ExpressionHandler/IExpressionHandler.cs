namespace Calculator.Services.ExpressionHandler;

public interface IExpressionHandler
{
    void HandleCalculatorInput(string input);
    void HandleBackspace();
    void HandleEvalButton();
    void HandleCommaButton(string input);
    void Clear();
    void OperateOnPreviousResult(string input);
    void HandleAcButton();
    void HandleSignToggle();
    
    event Action? OnChange;
    
    
    bool HasCalculated { get; set; }
    bool OperatorClicked { get; set; }
    string? Expression { get; set; }
    string? Result { get; set; }
}